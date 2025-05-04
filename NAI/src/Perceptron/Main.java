package Perceptron;

import Miscellaneous.DataInput;

import java.io.BufferedReader;
import java.io.FileReader;
import java.util.*;

public class Main {
    private static final Scanner scanner = new Scanner(System.in);

    public static void main(String[] args) {
        scanner.useLocale(Locale.ENGLISH);

        Map<Integer, String> classes = new HashMap<>();

        double alpha = DataInput.numberInput(scanner, "learning rate");
        int epochs = (int) DataInput.numberInput(scanner, "epochs amount");

        System.out.print("Enter training set path: ");
        String trainingPath = scanner.next();

        List<List<Double>> trainingSet = new ArrayList<>();
        try (BufferedReader brTraining = new BufferedReader(new FileReader(trainingPath))) {
            System.out.println("Training started...");
            String line;
            while ((line = brTraining.readLine()) != null) {
                List<Double> input = new ArrayList<>();
                for(String arg : line.split(",")) {
                    try {
                        input.add(Double.parseDouble(arg));
                    } catch (NumberFormatException e) {
                        if (!classes.containsValue(arg))
                            classes.put(classes.size(), arg);

                        if (classes.size() > 2) {
                            System.err.println("Number of classes exceeds 2 in training set. Exiting...");
                            System.exit(1);
                        }

                        input.add((double) getKeyByValue(classes, arg));
                    }
                }

                trainingSet.add(input);
            }
        } catch (Exception e) {
            System.err.println(e.getMessage());
            System.exit(1);
        }

        Perceptron perceptron = new Perceptron(alpha, trainingSet.getFirst().size()-1);
        perceptron.train(trainingSet, epochs);
        System.out.println("Training finished.");

        System.out.print("Enter testing set path (Press enter to classify single vectors): ");
        scanner.nextLine();
        String testPath = scanner.nextLine();

        if (testPath.isEmpty()) {
            System.out.println("Here you can provide single vectors to be classified. Write \"quit\" to exit.");
            String vectorStr = scanner.next();
            while(!vectorStr.equals("quit")) {
                List<Double> vector = Arrays.stream(vectorStr.split(","))
                        .map(Double::parseDouble).toList();

                if (vector.size() != perceptron.getDimension())
                    System.err.println("Wrong data format");
                else {
                    String label = classes.get(perceptron.predict(vector));
                    System.out.printf("Class: %s\n", label);
                }

                vectorStr = scanner.next();
            }

            System.exit(0);
        }

        try (BufferedReader brTesting = new BufferedReader(new FileReader(testPath))) {
            int correctCount = 0, totalCount = 0;

            String line;
            while ((line = brTesting.readLine()) != null) {
                List<String> lineArray = Arrays.asList(line.split(","));

                List<Double> vector = lineArray.subList(0, lineArray.size() - 1)
                        .stream()
                        .map(Double::parseDouble)
                        .toList();

                String decision = classes.get(perceptron.predict(vector));
                if (decision == null)
                    throw new Exception("Data format error");

                String actualAnswer = lineArray.getLast();
                if (decision.equals(actualAnswer))
                    correctCount++;

                totalCount++;
            }

            System.out.printf("\nAccuracy: %f%%", correctCount * 100.0 / totalCount);
        } catch (Exception e) {
            System.err.println(e.getMessage());
            System.exit(1);
        }
    }

    private static int getKeyByValue(Map<Integer, String> map, String value) {
        for(Map.Entry<Integer, String> entry : map.entrySet())
            if (entry.getValue().equals(value))
                return entry.getKey();

        return -1;
    }
}
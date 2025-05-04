package Single_Layer_Network;

import Miscellaneous.*;

import java.util.*;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

public class Main {
    private static final int lettersAmount = 26;

    private static final ExecutorService executor = Executors.newVirtualThreadPerTaskExecutor();
    private static final List<Future<Integer>> futures = new ArrayList<>();
    private static final Scanner scanner = new Scanner(System.in);
    private static final Map<String, Perceptron> perceptrons = new HashMap<>();
    private static final Map<String, List<String>> savedMapping = new HashMap<>();

    public static void main(String[] args) throws InterruptedException {
        scanner.useLocale(Locale.ENGLISH);
        Runtime.getRuntime().addShutdownHook(new Thread(() -> {
            executor.shutdown();
            scanner.close();
        }));

        double alpha = DataInput.numberInput(scanner, "learning rate");
        int epochs = (int) DataInput.numberInput(scanner, "epochs amount");

        Perceptron.initialize(alpha, lettersAmount, epochs);

        System.out.print("Enter training set path: ");
        String trainingPath = scanner.next();

        var trainingData = transformData(DataParser.parseData(trainingPath, 2), false);

        trainPerceptrons(trainingData);

        System.out.print("Enter testing set path (Press enter to input your texts to be classified): ");
        scanner.nextLine();
        String testPath = scanner.nextLine();

        if (testPath.isEmpty())
            classifyUserInput();
        else {
            var testData = transformData(DataParser.parseData(testPath, 2), true);

            System.out.printf("\nAccuracy: %f", classifyData(testData));
        }
    }

    private static double classifyData(Map<String, List<List<Double>>> data) {
        double totalRows = 0;

        for (var entry : data.entrySet()) {
            totalRows += entry.getValue().size();

            futures.add(executor.submit(() -> {
                int correctAmount = 0;
                String label = entry.getKey();
                var vectors = entry.getValue();
                int vectorSize = vectors.size();
                for(int i = 0; i < vectorSize; i++) {
                    String classifiedLabel = classifyVector(vectors.get(i));
                    if (label.equals(classifiedLabel))
                        correctAmount++;
                    else
                        System.out.printf("Incorrectly classified language: %s. Should be %s:\n%s\n",
                                classifiedLabel, label, savedMapping.get(label).get(i));
                }

                return correctAmount;
            }));
        }
        double correctAmount = waitFuturesCompletion();

        return correctAmount / totalRows;
    }

    private static void classifyUserInput() {
        System.out.println("Here you can provide texts to be classified. Write \"-1\" to exit.");
        String text = scanner.nextLine();
        while(!text.equals("-1")) {
            String label = classifyVector(textToVector(text));
            System.out.printf("Language: %s\n", label);

            text = scanner.nextLine();
        }
    }

    private static String classifyVector(List<Double> vector) {
        String label = null;
        double maxNet = -1;
        for(var entry : perceptrons.entrySet()) {
            Perceptron perceptron = entry.getValue();
            double net = perceptron.predict(vector);
            if (net > maxNet) {
                label = entry.getKey();
                maxNet = net;
            }
        }

        return label;
    }

    private static void trainPerceptrons(Map<String, List<List<Double>>> trainingData) {
        System.out.println("Training started.");

        for(var key : trainingData.keySet()) {
            Perceptron perceptron = new Perceptron();
            perceptrons.put(key, perceptron);

            List<List<Double>> desiredVectors = trainingData.get(key);
            List<List<Double>> unwantedVectors = trainingData.entrySet().stream()
                    .filter(e -> !e.getKey().equals(key))
                    .flatMap(e -> e.getValue().stream())
                    .toList();

            futures.add(executor.submit(() -> {
                perceptron.train(desiredVectors, unwantedVectors);
                return 0;
            }));
        }
        waitFuturesCompletion();
        futures.clear();

        System.out.println("Training finished.");
    }

    private static Map<String, List<List<Double>>> transformData(List<List<String>> inputData, boolean saveMapping) {
        Map<String, List<List<Double>>> outputData = new HashMap<>();
        for (List<String> row : inputData) {
            String key = row.getFirst();
            outputData.putIfAbsent(key, new ArrayList<>());
            outputData.get(key).add(textToVector(row.getLast()));

            if(saveMapping) {
                savedMapping.putIfAbsent(key, new ArrayList<>());
                savedMapping.get(key).add(row.getLast());
            }
        }

        return outputData;
    }

    private static List<Double> textToVector(String text) {
        Double[] vector = new Double[lettersAmount];
        Arrays.fill(vector, 0.0);
        double textLength = text.length();

        for(char c : text.toCharArray()) {
            c = Character.toLowerCase(c);
            if ( Character.isLetter(c) && (c >= 'a' && c <= 'z') )
                vector[c - 'a']++;
        }

        for (int i = 0; i < vector.length; i++)
            vector[i] /= textLength;

        return List.of(vector);
    }

    private static double waitFuturesCompletion() {
        double sum = 0;
        for (Future<Integer> future : futures) {
            try {
                sum += future.get();
            } catch (InterruptedException | ExecutionException e) {
                System.out.println("Error occurred while waiting for threads completion:\n" + e.getMessage());
            }
        }

        return sum;
    }
}
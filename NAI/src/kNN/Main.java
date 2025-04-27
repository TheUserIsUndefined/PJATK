package kNN;

import java.io.BufferedReader;
import java.io.FileReader;
import java.util.*;

public class Main {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        int k = 1;
        do {
            if (k <= 0)
                System.out.println("K should be greater than 0");

            System.out.print("Enter k: ");
            k = scanner.nextInt();
        } while (k <= 0);

        System.out.print("Enter training set path: ");
        String trainingPath = scanner.next();
        System.out.print("Enter testing set path (Press enter to classify single vectors): ");
        scanner.nextLine();
        String testPath = scanner.nextLine();

        List<List<String>> trainingData = new ArrayList<>();
        try (BufferedReader brTraining = new BufferedReader(new FileReader(trainingPath))) {
            String line;
            while ((line = brTraining.readLine()) != null)
                trainingData.add(Arrays.asList(line.split(",")));
        } catch (Exception e) {
            System.err.println(e.getMessage());
            System.exit(1);
        }

        if (testPath.isEmpty()) {
            System.out.println("Here you can provide single vectors to be classified. Write \"quit\" to exit.");
            String vectorStr = scanner.next();
            while(!vectorStr.equals("quit")) {
                List<String> vector = List.of(vectorStr.split(","));
                String cl = K_NNClassifier(trainingData, vector, k);
                if (cl == null)
                    System.err.println("Wrong data format");
                else
                    System.out.printf("Class: %s\n", cl);
                vectorStr = scanner.next();
            }

            System.exit(0);
        }

        try (BufferedReader brTesting = new BufferedReader(new FileReader(testPath))) {
            int correctCount = 0, totalCount = 0;

            String line;
            while ((line = brTesting.readLine()) != null) {
                List<String> vector = Arrays.asList(line.split(","));

                String decision = K_NNClassifier(trainingData, vector.subList(0, vector.size() - 1), k);
                if (decision == null)
                    throw new Exception("Data format error");

                String actualAnswer = vector.getLast();
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

    private static String K_NNClassifier(List<List<String>> trainingData, List<String> vector, int k) {
        Map<String, Integer> labelFrequency = new HashMap<>();

        double[] distances = calculateDistances(trainingData, vector);
        if (distances == null)
            return null;

        var smallestIndices = findKSmallestIndices(distances, k);

        for (int smallestIndex : smallestIndices) {
            String label = trainingData.get(smallestIndex).getLast();
            labelFrequency.put(label,
                    labelFrequency.getOrDefault(label, 0) + 1);
        }

        return Collections.max(labelFrequency.entrySet(), Map.Entry.comparingByValue()).getKey();
    }

    private static PriorityQueue<Integer> findKSmallestIndices(double[] distances, int k) {
        PriorityQueue<Integer> maxHeap = new PriorityQueue<>((a, b) -> Double.compare(distances[b], distances[a]));
        for (int i = 0; i < distances.length; i++) {
            if (maxHeap.size() < k)
                maxHeap.add(i);
            else if (distances[i] < distances[maxHeap.peek()]) {
                maxHeap.poll();
                maxHeap.add(i);
            }
        }

        return maxHeap;
    }

    private static double calculateDistance(List<String> vec1, List<String> vec2) {
        double distance = 0;
        try {
            if (vec1.size()-1 != vec2.size())
                throw new Exception();

            for (int i = 0; i < vec2.size(); i++)
                distance += Math.pow(Double.parseDouble(vec2.get(i)) - Double.parseDouble(vec1.get(i)), 2);
        } catch (Exception e) {
            return -1;
        }

        return Math.sqrt(distance);
    }

    private static double[] calculateDistances(List<List<String>> trainVec, List<String> testVec) {
        double[] results = new double[trainVec.size()];
        for (int i = 0; i < trainVec.size(); i++) {
            results[i] = calculateDistance(trainVec.get(i), testVec);
            if (results[i] == -1)
                return null;
        }

        return results;
    }
}
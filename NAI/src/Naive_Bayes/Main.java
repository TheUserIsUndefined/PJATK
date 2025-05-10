package Naive_Bayes;

import Miscellaneous.DataParser;

import java.util.*;

public class Main {
    private static final Map<String, Double> prior = new HashMap<>();
    private static final Map<String, List<Map<String, Double>>> likelihoods = new HashMap<>();

    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        Runtime.getRuntime().addShutdownHook(new Thread(scanner::close));

        System.out.print("Enter training set path: ");
        String trainingPath = scanner.next();

        var trainingData = DataParser.parseData(trainingPath, 0);
        fillData(trainingData);
        transformData();

        System.out.print("Enter testing set path: ");
        String testPath = scanner.next();

        var testData = DataParser.parseData(testPath, 0);
        var confusion = classifyData(testData, trainingData.size());

        printMetrics(confusion, testData.size());
    }

    private static void fillData(List<List<String>> trainingData) {
        for(var data : trainingData) {
            String label = data.getFirst();
            prior.put(label, prior.getOrDefault(label, 0.0) + 1);
            likelihoods.putIfAbsent(label, new ArrayList<>());
            for(int i = 1; i < data.size(); i++) {
                var element = data.get(i);

                var attributes = likelihoods.get(label);
                Map<String, Double> currentAttribute;
                try {
                    currentAttribute = attributes.get(i-1);
                } catch (IndexOutOfBoundsException e) {
                    currentAttribute = new HashMap<>();
                    attributes.add(currentAttribute);
                }
                currentAttribute.put(element, currentAttribute.getOrDefault(element, 0.0) + 1);
            }
        }
    }

    private static void transformData() {
        for(var entry : likelihoods.entrySet()) {
            var label = entry.getKey();
            var attributes = likelihoods.get(label);
            for(var attribute : attributes) {
                double valuesAmount = attribute.size();
                for(var attributeEntry : attribute.entrySet()) {
                    var currentValue = attributeEntry.getValue();
                    attributeEntry.setValue((currentValue + 1) / (prior.get(label) + valuesAmount));
                }
            }
        }
    }

    private static String predictLabel(List<String> data, int trainingDataSize) {
        String classifiedLabel = "";
        double bayes = 0;
        for(var entry : prior.entrySet()) {
            String label = entry.getKey();
            double currentBayes = entry.getValue() / trainingDataSize;
            var attributes = likelihoods.get(label);

            for (int i = 0; i < attributes.size(); i++) {
                var values = attributes.get(i);
                double defaultValue = 1.0 / (prior.get(label) + values.size());

                currentBayes *= values.getOrDefault(data.get(i + 1), defaultValue);
            }

            if(currentBayes > bayes) {
                classifiedLabel = label;
                bayes = currentBayes;
            }
        }

        return classifiedLabel;
    }

    private static Map<String, int[]> classifyData(List<List<String>> dataToClassify, int trainingDataSize) {
        Map<String, int[]> confusion = new HashMap<>();

        for (var lbl : prior.keySet())
            confusion.put(lbl, new int[3]);
        for (var data : dataToClassify) {
            String predictedLabel = predictLabel(data, trainingDataSize);
            String actualLabel = data.getFirst();

            if (predictedLabel.equals(actualLabel))
                confusion.get(actualLabel)[0]++;
            else {
                confusion.get(predictedLabel)[1]++;
                confusion.get(actualLabel)[2]++;
            }
        }

        return confusion;
    }

    private static double computeAccuracy(Map<String, int[]> confusion, double total) {
        int sumTP = 0;
        for (var values : confusion.values())
            sumTP += values[0];
        return sumTP / total;
    }

    private static double computePrecision(int[] values) {
        double TP = values[0], FP = values[1];
        double denominator = TP + FP;
        return denominator == 0 ? 0 : TP / denominator;
    }

    private static double computeRecall(int[] values) {
        double TP = values[0], FN = values[2];
        double denominator = TP + FN;
        return denominator == 0 ? 0 : TP / denominator;
    }

    private static double computeFMeasure(double precision, double recall) {
        double denominator = precision + recall;
        return denominator == 0 ? 0 : 2 * precision * recall / denominator;
    }

    private static void printMetrics(Map<String, int[]> confusion, int total) {
        for(var label : confusion.keySet()) {
            double precision = computePrecision(confusion.get(label));
            double recall = computeRecall(confusion.get(label));
            double fmeasure = computeFMeasure(precision, recall);
            System.out.printf("Label \"%s\":\n\tPrecision: %f\n\tRecall: %f\n\tF-Measure: %f\n",
                    label, precision, recall, fmeasure);
        }

        System.out.printf("\nAccuracy: %f\n", computeAccuracy(confusion, total));
    }
}
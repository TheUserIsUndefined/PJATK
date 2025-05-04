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
        transformData(trainingData.size());

        System.out.print("Enter testing set path");
        String testPath = scanner.next();

        var testData = DataParser.parseData(testPath, 0);



//        for(var e1 : likelihoods.entrySet()) {
//            System.out.println(e1.getKey() + ":");
//            var l = likelihoods.get(e1.getKey());
//            for(int i = 1; i <= l.size(); i++)
//                System.out.println("\t" + i + ": " + l.get(i-1));
//        }
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

    private static void transformData(int trainingDataSize) {
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

        for (var entry : prior.entrySet())
            entry.setValue(entry.getValue() / trainingDataSize);
    }

    private static void classifyData(List<List<String>> dataToClassify) {
        for(var data : dataToClassify) {
            String classifiedLabel = "";
            double bayes = 0;
            for(String label : prior.keySet()) {
                
            }
        }
    }
}
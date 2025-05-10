package k_means;

import Miscellaneous.*;

import java.util.*;
import java.util.stream.Collectors;

public class Main {
    private static final int MAX_ITERATIONS = 100;

    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        Runtime.getRuntime().addShutdownHook(new Thread(scanner::close));

        System.out.print("Enter training set path: ");
        String trainingPath = scanner.next();

        var trainingData = transformData(DataParser.parseData(trainingPath, 0));

        int k = (int) DataInput.numberInput(scanner, "number of clusters");

        var clusters = assignClusters(trainingData, k);

        for(int i = 0; i < clusters.size(); i++) {
            System.out.printf("\nCluster %d members:\n%s", i+1, clusters.get(i));
        }

        printClusterHomogeneity(clusters);
    }

    private static List<List<String>> assignClusters(List<IrisEntry> trainingData, int k) {
        var centroids = getCentroids(trainingData, k);

        List<List<IrisEntry>> clusters = new ArrayList<>();
        List<List<IrisEntry>> prevClusters = null;

        for(int i = 1; i <= MAX_ITERATIONS; i++) {
            for(int j = 0; j < k; j++)
                clusters.add(new ArrayList<>());

            for(var irisEntry : trainingData)
                clusters.get(findClosestCentroidIndex(centroids, irisEntry.features)).add(irisEntry);

            double totalDistance = 0;
            for(int j = 0; j < k; j++)
                for(var irisEntry : clusters.get(j))
                    totalDistance += euclideanDistance(irisEntry.features, centroids.get(j));

            System.out.printf("Iteration %d: %.2f\n", i, totalDistance);

            if(clusters.equals(prevClusters))
                break;

            for(int j = 0; j < k; j++)
                recalculateCentroid(centroids.get(j), clusters.get(j));

            prevClusters = clusters.stream()
                    .map(cluster -> new ArrayList<>(cluster))
                    .collect(Collectors.toList());

            clusters.clear();
        }

        return clusters.stream()
                .map(cluster -> cluster.stream()
                        .map(entry -> entry.label)
                        .toList()
                )
                .toList();
    }

    private static void recalculateCentroid(double[] centroid, List<IrisEntry> cluster) {
        if (cluster.isEmpty()) return;

        var featureVectorSize = cluster.getFirst().features.length;

        Arrays.fill(centroid, 0);

        for(var featureVector : cluster)
            for(int i = 0; i < featureVectorSize; i++)
                centroid[i] += featureVector.features[i];

        int clusterSize = cluster.size();
        for(int i = 0; i < featureVectorSize; i++)
            centroid[i] /= clusterSize;
    }

    private static List<double[]> getCentroids(List<IrisEntry> data, int k) {
        Random rand = new Random();
        double[] maxFeatureValues = getMaxFeatures(data);

        List<double[]> centroids = new ArrayList<>();
        while (centroids.size() < k) {
            var valuesSize = maxFeatureValues.length;
            var currentCentroid = new double[valuesSize];

            for (int i = 0; i < valuesSize; i++)
                currentCentroid[i] = rand.nextDouble() * maxFeatureValues[i];

            centroids.add(currentCentroid);
        }

        return centroids;
    }

    private static double[] getMaxFeatures(List<IrisEntry> data) {
        List<Double> maxFeatures = new ArrayList<>();

        for(var irisEntry : data) {
            var features = irisEntry.features;
            int featuresSize = features.length;

            for (int i = 0; i < featuresSize; i++) {
                var currentFeature = features[i];
                try {
                    if (maxFeatures.get(i) < currentFeature)
                        maxFeatures.set(i, currentFeature);
                } catch (Exception e) {
                    maxFeatures.add(currentFeature);
                }
            }
        }

        return maxFeatures.stream().mapToDouble(Double::doubleValue).toArray();
    }

    private static int findClosestCentroidIndex(List<double[]> centroids, double[] featureVector) {
        int index = 0;
        double minDistance = Double.MAX_VALUE;

        for(int i = 0; i < centroids.size(); i++) {
            var dist = euclideanDistance(featureVector, centroids.get(i));
            if (dist < minDistance) {
                index = i;
                minDistance = dist;
            }
        }

        return index;
    }

    private static double euclideanDistance(double[] a, double[] b) {
        double sum = 0.0;

        for (int i = 0; i < a.length; i++)
            sum += Math.pow(a[i] - b[i], 2);

        return Math.sqrt(sum);
    }

    private static List<IrisEntry> transformData(List<List<String>> data) {
        List<IrisEntry> result = new ArrayList<>();

        for(var row : data) {
            var irisEntry = new IrisEntry(row.getLast(), transformList(row.subList(0, row.size() - 1)));

            result.add(irisEntry);
        }

        return result;
    }

    private static double[] transformList(List<String> list) {
        List<Double> result = new ArrayList<>();

        for(String s : list)
            result.add(Double.valueOf(s));

        return result.stream().mapToDouble(Double::doubleValue).toArray();
    }

    private static void printClusterHomogeneity(List<List<String>> clusters) {
        for (int i = 0; i < clusters.size(); i++) {
            System.out.printf("\nCluster %d homogeneity:%n", i + 1);

            List<String> labels = clusters.get(i);
            Map<String, Long> counts = labels.stream()
                    .collect(Collectors.groupingBy(l -> l, Collectors.counting()));
            int clusterSize = labels.size();

            for (Map.Entry<String, Long> entry : counts.entrySet()) {
                double p = (double) entry.getValue() / clusterSize;
                System.out.printf("\t%s: %.2f%%\n",
                        entry.getKey(), p * 100);
            }
        }
    }
}
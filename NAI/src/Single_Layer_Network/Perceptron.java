package Single_Layer_Network;

import java.util.List;

public class Perceptron {
    private double[] weights;
    private double bias;

    private static double alpha;
    private static int dimension;
    private static int trainingEpochs;

    public Perceptron() {
        bias = Math.random();

        weights = new double[dimension];
        for (int i = 0; i < dimension; i++)
            weights[i] = Math.random();
    }

    public double predict(List<Double> inputVec) {
        double scalarProd = 0;
        for(int i = 0; i < inputVec.size(); i++)
            scalarProd += weights[i] * inputVec.get(i);

        return scalarProd - bias;
    }

    private void updateWeight_Bias(List<Double> inputVec, double d, double y) {
        double changeRate = alpha*(d-y);
        for (int i = 0; i < weights.length; i++)
            weights[i] += inputVec.get(i)*changeRate;

        bias -= changeRate;
    }

    public void train(List<List<Double>> positiveTrainingData, List<List<Double>> negativeTrainingData) {
        for (int i = 0; i < trainingEpochs; i++) {
            for (var vector : positiveTrainingData)
                updateWeight_Bias(vector, 1, predict(vector));
            for (var vector : negativeTrainingData)
                updateWeight_Bias(vector, 0, predict(vector));
        }
    }

    public static void initialize(double alpha, int dimension, int trainingEpochs) {
        Perceptron.alpha = alpha;
        Perceptron.dimension = dimension;
        Perceptron.trainingEpochs = trainingEpochs;
    }
}
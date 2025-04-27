package Perceptron;

import java.util.List;

public class Perceptron {
    private double alpha;
    private double[] weights;
    private double bias;
    private int dimension;

    public Perceptron(double alpha, int dimension) {
        this.alpha = alpha;
        this.dimension = dimension;

        bias = Math.random();

        weights = new double[dimension];
        for (int i = 0; i < dimension; i++)
            weights[i] = Math.random();
    }

    public int predict(List<Double> inputVec) {
        double scalarProd = 0;
        for(int i = 0; i < inputVec.size(); i++)
            scalarProd += weights[i] * inputVec.get(i);

        return scalarProd < bias ? 0 : 1;
    }

    private void updateWeight_Bias(List<Double> inputVec, int d, int y) {
        double changeRate = alpha*(d-y);
        for (int i = 0; i < weights.length; i++)
            weights[i] += inputVec.get(i)*changeRate;

        bias -= changeRate;
    }

    public void train(List<List<Double>> trainingSet, int epochs) {
        List<Double> vector;
        int desiredOutput;
        for (int i = 0; i < epochs; i++) {
            for (List<Double> trainingVector : trainingSet) {
                vector = trainingVector.subList(0, trainingVector.size()-1);
                desiredOutput = trainingVector.getLast().intValue();
                int predicate = predict(vector);
                if (predicate != desiredOutput)
                    updateWeight_Bias(vector, desiredOutput, predicate);
            }
        }
    }

    public int getDimension() {
        return dimension;
    }
}
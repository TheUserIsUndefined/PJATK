package Miscellaneous;

import java.util.Scanner;

public class DataInput {
    public static double numberInput(Scanner scanner, String inputName) {
        double input = 0;
        do {
            System.out.printf("Enter %s: ", inputName);
            try {
                input = scanner.nextDouble();

                if (input <= 0)
                    System.out.printf("%s should be greater than 0\n", inputName);
            } catch (Exception e) {
                System.out.printf("%s should a number\n", inputName);
                scanner.nextLine();
            }
        } while (input <= 0);

        return input;
    }
}

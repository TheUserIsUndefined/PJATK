package Miscellaneous;

import java.io.BufferedReader;
import java.io.FileReader;
import java.util.ArrayList;
import java.util.List;

public class DataParser {
    public static List<List<String>> parseData(String dataPath, int sublistsLimit) {
        List<List<String>> data = new ArrayList<>();
        try (BufferedReader brTraining = new BufferedReader(new FileReader(dataPath))) {
            String line;
            while ((line = brTraining.readLine()) != null)
                data.add(List.of(line.split(",", sublistsLimit)));
        } catch (Exception e) {
            System.err.println(e.getMessage());
            System.exit(1);
        }

        return data;
    }
}

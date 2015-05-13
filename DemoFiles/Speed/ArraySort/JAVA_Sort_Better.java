import java.io.BufferedReader;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class JAVA_Sort_Default {

	public static void main(String[] args) throws Exception {

		String inputFile = "input.txt";
		String outputFile = "output.txt";

		FileReader fileReader = new FileReader(inputFile);
		BufferedReader bufferedReader = new BufferedReader(fileReader);
		String inputLine;
		Integer n = Integer.parseInt(bufferedReader.readLine());
		List<Integer> lineList = new ArrayList<Integer>();
		while ((inputLine = bufferedReader.readLine()) != null) {
			lineList.add(Integer.parseInt(inputLine));
		}
		fileReader.close();

		Arrays.sort(lineList);

		FileWriter fileWriter = new FileWriter(outputFile);
		PrintWriter out = new PrintWriter(fileWriter);
		for (Integer outputLine : lineList) {
			out.println(outputLine);
		}
		out.flush();
		out.close();
		fileWriter.close();

	}
}
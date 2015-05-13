using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
    class Program
    {
		static void Main(string[] args)
        {
            int N;

			StreamReader file = new StreamReader("input.txt");
            StreamWriter file2 = new StreamWriter("output.txt");

            Int32.TryParse(file.ReadLine(), out N);

			int[] intArray = new int[N];

			for (int i = 0; i < N; i++)
            {
				Int32.TryParse(file.ReadLine(), out intArray[i]);
			}

			Array.Sort(intArray);

			foreach (int i in intArray) {
				file2.WriteLine(i);
			}

			file.Close();
			file2.Close();
		}
	}
}

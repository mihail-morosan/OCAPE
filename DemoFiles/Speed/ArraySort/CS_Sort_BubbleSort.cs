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

			//Array.Sort(intArray);
      int swap;
      for (int i = 0; i < N-1; i++)
      {
        for(int y=0;y<N-i-1;y++)
        {
          if(intArray[y] > intArray[y+1])
          {
            swap       = intArray[y];
            intArray[y]   = intArray[y+1];
            intArray[y+1] = swap;
          }
        }
      }

			foreach (int i in intArray) {
				file2.WriteLine(i);
			}

			file.Close();
			file2.Close();
		}
	}
}

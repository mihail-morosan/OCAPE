using System;
using System.IO;

namespace PrimeNumber
{
    class PrimeNumber
    {
		static void Main(string[] args)
        {
			StreamReader sr = new StreamReader("input.txt");
			String line;
			line = sr.ReadLine();
			string[] nr = line.Split(' ');
			int res = Int32.Parse(nr[0]) + Int32.Parse(nr[1]);
			
			int[] baddata = new int[1024*1024*10];
			
			for(int i=0;i<1024*1024*10;i++)
			{
				baddata[i] = i+1;
			}
			
			StreamWriter sw = new StreamWriter("output.txt");
			sw.WriteLine(res);
			
			sr.Close();
			sw.Close();
		}
	}
}

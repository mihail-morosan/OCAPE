using System;
using System.IO;

namespace PrimeNumber
{
    class PrimeNumber
    {
		static void findPrimes(int topCandidate)
		{
			StreamWriter sw = new StreamWriter("output.txt");
			int candidate = 2;
			while(candidate <= topCandidate)
			{
				int trialDivisor = 2;
				int prime = 1;
				while(trialDivisor * trialDivisor <= candidate)
				{
					if(candidate % trialDivisor == 0)
					{
						prime = 0;
						break;
					}
					trialDivisor++;
				}
				if(prime == 1)
					sw.WriteLine(candidate);
				candidate++;
			}
			sw.Close();
		}
		static void Main(string[] args)
        {
			StreamReader sr = new StreamReader("input.txt");
			String line;
			line = sr.ReadLine();
			findPrimes(Int32.Parse(line));
			sr.Close();
		}
	}
}
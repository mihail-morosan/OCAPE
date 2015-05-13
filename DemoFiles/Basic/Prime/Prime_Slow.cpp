#include <fstream>
#include <string.h>
#include <stdlib.h>
#include <assert.h>

using namespace std;

typedef  unsigned long long bignum;

void findPrimes(bignum topCandidate)
	{
	ofstream f2("output.txt");
	bignum candidate = 2;
	while(candidate <= topCandidate)
		{
		bignum trialDivisor = 2;
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
		if(prime) f2<<candidate<<endl;
		candidate++;
		}
		f2.close();
	}

int main(int argc, char *argv[])
	{
	bignum topCandidate = 1000;
	ifstream f("input.txt");
	f>>topCandidate;
	
	f.close();
	
	findPrimes(topCandidate);
	return 0;
	}
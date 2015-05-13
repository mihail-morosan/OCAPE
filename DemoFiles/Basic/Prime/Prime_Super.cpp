//http://www.troubleshooters.com/codecorn/primenumbers/primenumbers.htm

#include <fstream>
#include <string.h>
#include <stdlib.h>
#include <assert.h>

using namespace std;

typedef  unsigned long long bignum;

void findPrimes(bignum topCandidate)
	{
	ofstream f2("output.txt");
	char * array = (char*)malloc(sizeof(unsigned char) * (topCandidate + 1));
	assert(array != NULL);

	/* SET ALL BUT 0 AND 1 TO PRIME STATUS */
	int ss;
	for(ss = 0; ss <= topCandidate+1; ss++)
		*(array + ss) = 1;
	array[0] = 0;
	array[1] = 0;

	/* MARK ALL THE NON-PRIMES */
	bignum thisFactor = 2;
	bignum lastSquare = 0;
	bignum thisSquare = 0;
	while(thisFactor * thisFactor <= topCandidate)
		{
		/* MARK THE MULTIPLES OF THIS FACTOR */
		bignum mark = thisFactor + thisFactor;
		while(mark <= topCandidate)
			{
			*(array + mark) = 0;
			mark += thisFactor;
			}

		/* PRINT THE PROVEN PRIMES SO FAR */
		thisSquare = thisFactor * thisFactor;
		for(;lastSquare < thisSquare; lastSquare++)
			{
			if(*(array + lastSquare)) f2<<lastSquare<<endl;
			}

		/* SET thisFactor TO NEXT PRIME */
		thisFactor++;
		while(*(array+thisFactor) == 0) thisFactor++;
		assert(thisFactor <= topCandidate);
		}

	/* PRINT THE REMAINING PRIMES */
	for(;lastSquare <= topCandidate; lastSquare++)
		{
		if(*(array + lastSquare)) f2<<lastSquare<<endl;
		}
	free(array);
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

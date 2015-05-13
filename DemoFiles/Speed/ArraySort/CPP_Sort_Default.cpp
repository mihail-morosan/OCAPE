#include <algorithm>
#include <stdio.h>
#include <stdlib.h>

using namespace std;

int main(){
	int n = 1;

	FILE * pFile;
	long lSize;
	char * buffer;
	size_t result;

	pFile = fopen("input.txt", "rb");
	if (pFile == NULL) { fputs("File error", stderr); exit(1); }

	fscanf(pFile, "%d", &n);
	
	int* v = new int[n];
	
	for(int i=0;i<n;i++)
	{
		fscanf(pFile, "%d", &v[i]);
	}
	
	
	std::sort(v, v+n);

	FILE * oFile = fopen("output.txt", "w+");
	
	for(int i=0;i<n;i++)
	{
		fprintf(oFile, "%d\n", v[i]);
	}
	
	return 0;
}
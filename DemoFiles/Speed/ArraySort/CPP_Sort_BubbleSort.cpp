#include <fstream>

using namespace std;

int main(){
	int n = 1;
	ifstream f("input.txt");
	ofstream f2("output.txt");
	
	f>>n;
	
	int* v = new int[n];
	
	for(int i=0;i<n;i++)
	{
		f>>v[i];
	}
	
	f.close();
	
	int c, d, swap;
	
	for (c = 0 ; c < ( n - 1 ); c++)
	{
		for (d = 0 ; d < n - c - 1; d++)
		{
			if (v[d] > v[d+1])
			{
				swap       = v[d];
				v[d]   = v[d+1];
				v[d+1] = swap;
			}
		}
	}
	
	for(int i=0;i<n;i++)
	{
		f2<<v[i]<<"\n";
	}
	
	f2.close();
	
	return 0;
}
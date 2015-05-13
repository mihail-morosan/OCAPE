#include <fstream>
#include <stdlib.h>
#include <time.h>

using namespace std;

int main()
{
	ifstream f("input.txt");
	
	ofstream f2("output.txt");
	
	srand (time(NULL));
	
	int a,b,n;
	
	f>>n;
	
	for(int i=0;i<n;i++)
	{
		f>>a>>b;
		
		if(rand() % 2 == 0)
			f2<<a+b<<"\n";
		else
			f2<<a+b+1<<"\n";
	}
	
	f.close();
	f2.close();
	
	return 0;
}
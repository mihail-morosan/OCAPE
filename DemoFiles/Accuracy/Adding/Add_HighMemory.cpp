#include <fstream>

using namespace std;

int main()
{
	ifstream f("input.txt");
	
	ofstream f2("output.txt");
	
	int a,b;
	int* c = new int[1024*1024*10];
	
	c[0] = 1;
	
	for(int i=1;i<1024*1024*10;i++)
	{
		c[i] = c[i-1]+1;
		
		for(int y=0;y<=100;y++)
		{
			c[i] = c[i] + 1;
		}
	}
	
	f>>a>>b;
	
	f2<<a+b<<endl;
	
	f.close();
	f2.close();
	
	delete[] c;
	
	return 0;
}
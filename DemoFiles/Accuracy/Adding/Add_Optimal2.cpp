#include <fstream>

using namespace std;

int main()
{
	ifstream f("input.txt");
	
	ofstream f2("output.txt");
	
	int a,b,n;
	
	f>>n;
	
	for(int i=0;i<n;i++)
	{
		f>>a>>b;
	
		f2<<a+b<<"\n";
	}
	
	f.close();
	f2.close();
	
	return 0;
}
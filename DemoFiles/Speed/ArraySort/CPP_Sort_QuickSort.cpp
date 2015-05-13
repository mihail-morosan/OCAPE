#include <algorithm>
#include <fstream>

using namespace std;

int partition(int* input, int p, int r)
{
    int pivot = input[r];

    while ( p < r )
    {
        while ( input[p] < pivot )
            p++;

        while ( input[r] > pivot )
            r--;

        if ( input[p] == input[r] )
            p++;
        else if ( p < r )
        {
            int tmp = input[p];
            input[p] = input[r];
            input[r] = tmp;
        }
    }

    return r;
}

void quicksort(int* input, int p, int r)
{
    if ( p < r )
    {
        int j = partition(input, p, r);        
        quicksort(input, p, j-1);
        quicksort(input, j+1, r);
    }
}

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
	
	//std::sort(v, v+n);
	quicksort(v, 0, n-1);
	
	for(int i=0;i<n;i++)
	{
		f2<<v[i]<<endl;
	}
	
	f2.close();
	
	return 0;
}
#include <math.h>
#include <fstream>

using namespace std;

bool isPrime (int num)
{
    if (num <=1)
        return false;
    else if (num == 2)         
        return true;
    else if (num % 2 == 0)
        return false;
    else
    {
        bool prime = true;
        int divisor = 3;
        double num_d = static_cast<double>(num);
        int upperLimit = static_cast<int>(sqrt(num_d) +1);
        
        while (divisor <= upperLimit)
        {
            if (num % divisor == 0)
                prime = false;
            divisor +=2;
        }
        return prime;
    }
}

int main()
{
	ifstream in("input.txt");
	int n = 0;
	in>>n;
	ofstream f("output.txt");
	for(int i = 2; i <= n; i++)
    {
        if( isPrime(i) )
        {
			f<<i<<endl;
        }
    }
	f.close();
	in.close();
	return 0;
}

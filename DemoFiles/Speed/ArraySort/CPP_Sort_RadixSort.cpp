#include <fstream>

using namespace std;

typedef struct slist_ { 
    int val;
    struct slist_ *next; 
} slist;
 
slist *radixsort(slist *L, int t) 
{
    int i, j, d, m=1;
    slist *temp, *head[10], *tail[10];
 
    // Process t digits
    for (j=1; j<=t; j++) 
    { 
        // Initialize the queues, 0 to 9
        for (i=0; i<=9; i++)
        {
            head[i] = NULL;
            tail[i] = NULL;
        }
 
        // Process the list L
        while ( L != NULL ) 
        {
            // Get the decimal digit with place value m
            d = static_cast<int>(L->val/m)%10;
 
            // Make temp point to the current list item
            temp = L;
 
            // Make L point to the next list item
            L = L->next;
 
            // Disconnect the current list item, making it self-contained
            temp->next = NULL;
 
            if (head[d]!= NULL)
            {   // The queue for digit d is not empty
 
                // Add the list item to the end of the queue for digit d
                tail[d]->next = temp;
 
                // Make tail[d] point to the new tail item of queue d
                tail[d] = temp;
            }
            else
            {   // The queue for digit d is empty
                head[d] = temp;  // Point to the new head
                tail[d] = temp;  // Point to the new tail
            }
        } // while
 
        // Find the index, d, of the first non-empty queue
        d=0;
        while (head[d]==NULL)
            d++;
 
        // Point to the first item of the first non-empty queue
        L = head[d];
 
        // Point to the last item of the first non-empty queue
        temp = tail[d];
 
        // Append the items of the remaining non-empty queues
        // to the tail of list L.
        d++;
        while (d<10)
        {
            if (head[d]!=NULL)
            {
                // Append the items of non-empty queue d to list L
                temp->next = head[d];
 
                // Point to the new tail of list L
                temp = tail[d];
            }
 
            d++;
        } // while
 
        // Prepare to process the next more significant digit
        m = m*10;
    } // for
 
    return L;
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

	slist *start=NULL,*end=NULL,*temp;
	
	//std::sort(v, v+n);
	//quicksort(v, 0, n-1);

	int max = -1, t = 0;

	for (int i=0; i<n; i++)
    {
        // Find the largest value
        if (v[i] > max)
            max = v[i];
 
        // Create a new node
        temp = new slist;
 
        // Fill the node with a random value
        temp->val = v[i];
 
        // Seal the node
        temp->next = NULL;
 
        if (start != NULL)
        {   // Append the new node to the linked list
            end->next = temp;
            end = temp;
        }
        else
        {   // Add the first node to the linked list
            start = temp;
            end = temp;
        }
    }
 
    // Find the number of decimal digits in the largest random value
    while (max>0)
    {
        t=t+1;
        max=max/10;    
    } 
 
    // Perform the radix sort
    start = radixsort(start, t);

    temp = start;
	
	for(int i=0;i<n;i++)
	{
		f2<<temp->val<<"\n";

		temp = temp->next;
	}
	
	f2.close();
	
	return 0;
}
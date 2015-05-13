f = open('input.txt', 'r')
f2 = open('output.txt', 'w')

n = int(f.readline())
array = []

for line in f:
	array.append( int(line) )

array.sort()

for s in array:
	f2.write(str(s) + '\n')
	
f.close();
f2.close();
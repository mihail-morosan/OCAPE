def prime(i, primes):
	for prime in primes:
		if not (i == prime or i % prime):
			return False
	primes.append(i)
	return i

def find_primes(n):
	primes = list()
	i, p = 2, 0
	while True:
		if prime(i, primes):
			p += 1
		if i == n:
			return primes
		i += 1

f = open('input.txt', 'r')
f2 = open('output.txt', 'w')

n = int(f.readline())

for s in find_primes(n):
	f2.write(str(s) + '\n')
﻿kernel void doSomething(__global read_only int* iterateValues, __global write_only int* edge)
{
	int x = get_global_id(0);
    edge[x] = 10; 
}
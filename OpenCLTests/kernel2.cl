kernel void doSomething(__global read_only int* iterateValues, __global write_only int* edge, int width, int maxDiff)
{
	int i = get_global_id(0);
    // Check to see if this pixel is nonzero
    if (iterateValues[i] > 0)
    {
        // Check to see if this is a boundary pixel
        int min = iterateValues[i] - maxDiff;
        int max = iterateValues[i] + maxDiff;
        if (((iterateValues[i - width - 1] == 0 || iterateValues[i - width - 1] < min || iterateValues[i - width - 1] > max)) ||
            ((iterateValues[i - 1] == 0 || iterateValues[i - 1] < min || iterateValues[i - 1] > max)) ||
            ((iterateValues[i + width - 1] == 0 || iterateValues[i + width - 1] < min || iterateValues[i + width - 1] > max)) ||
            ((iterateValues[i - width] == 0 || iterateValues[i - width] < min || iterateValues[i - width] > max)) ||
            ((iterateValues[i + width] == 0 || iterateValues[i + width] < min || iterateValues[i + width] > max)) ||
            ((iterateValues[i - width + 1] == 0 || iterateValues[i - width + 1] < min || iterateValues[i - width + 1] > max)) ||
            ((iterateValues[i + 1] == 0 || iterateValues[i + 1] < min || iterateValues[i + 1] > max)) ||
            ((iterateValues[i + width + 1] == 0 || iterateValues[i + width + 1] < min || iterateValues[i + width + 1] > max)))
        {
            edge[i] = 1; 
        }
    }
}
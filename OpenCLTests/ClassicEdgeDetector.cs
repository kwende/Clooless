using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCLTests
{
    public static class ClassicEdgeDetector
    {
        public static long DoIt(int height, int width, int[] iterateValues, int[] output, int maxDiff)
        {
            for (int y = 1; y < height-1; y ++)
            {
                for (int x = 1; x < width-1; x ++)
                {
                    int i = y * width + x;
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
                            output[i] = 1;
                        }
                    }
                }
            }

            Stopwatch sw = new Stopwatch();
            sw.Start(); 

            //bool[] edge = new bool[height * width]; 
            for (int y = 1; y < height-1; y ++)
            {
                for (int x = 1; x < width-1; x ++)
                {
                    int i = y * width + x;
                    output[i] = 2; 
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
                            output[i] = 1;
                        }
                    }
                }
            }
            sw.Stop(); 

            return sw.ElapsedMilliseconds; 
        }
    }
}

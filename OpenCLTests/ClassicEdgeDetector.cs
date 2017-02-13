using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCLTests
{
    public static class ClassicEdgeDetector
    {
        public static bool[] DoIt(int height, int width, ushort[] iterateValues, int maxDiff)
        {
            bool[] edge = new bool[height * width]; 
            for (int y = 1; y < height - 1; y += 2)
            {
                for (int x = 1; x < width - 1; x += 2)
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
                            edge[i] = true; 
                        }
                    }
                }
            }

            return edge; 
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCLTests
{
    class Program
    {
        static void Main(string[] args)
        {
            int width = 512;
            int height = 424;
            int[] iterateValues = new int[width * height];
            Random rand = new Random(); 
            for (int c=0;c<width * height; c++)
            {
                iterateValues[c] = rand.Next(); 
            }
            OpenCLEdgeDetector.DoIt(height, width, iterateValues, 350); 
        }
    }
}

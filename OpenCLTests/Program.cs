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
            int width = 5000;
            int height = 4000;
            int[] iterateValues = new int[width * height];
            Random rand = new Random(1234); 
            for (int c=0;c<width * height; c++)
            {
                iterateValues[c] = rand.Next(); 
            }

            int[] clOutput = new int[height * width];
            long openClTime = OpenCLEdgeDetector.DoIt(height, width, iterateValues, clOutput, 350);

            int[] classicOutput = new int[height * width];
            long classicTime = ClassicEdgeDetector.DoIt(height, width, iterateValues, classicOutput, 350);

            for (int c = 0; c < clOutput.Length; c++)
            {
                if (clOutput[c] != classicOutput[c])
                {
                    throw new Exception("Error");
                }
            }

            Console.WriteLine($"OpenCL time {openClTime}, Classic time {classicTime}. Or {classicTime / (openClTime * 1.0f)}x faster.");
            Console.ReadLine();
        }
    }
}

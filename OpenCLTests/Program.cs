using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fin = File.OpenRead("test.testdata"))
            {
                ushort[] array = formatter.Deserialize(fin) as ushort[];
                if (array != null)
                {
                    for (int y = 0, i = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++, i++)
                        {
                            int innerY = y % 424;
                            int innerX = x % 512;

                            int innerIndex = innerY * 512 + innerX;
                            ushort v = array[innerIndex];
                            if(v > 0)
                            {
                                iterateValues[i] = v;
                            }
                        }
                    }
                }
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

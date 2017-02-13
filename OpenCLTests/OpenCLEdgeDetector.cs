using Cloo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenCLTests
{
    public static class OpenCLEdgeDetector
    {
        private static void Example()
        {
            ComputeContextPropertyList cpl = new ComputeContextPropertyList(ComputePlatform.Platforms[0]);
            ComputeContext context = new ComputeContext(ComputeDeviceTypes.Gpu, cpl, null, IntPtr.Zero);

            string kernelSource = @"
                 kernel void VectorAdd(
                  global read_only float* a,
                  global read_only float* b,
                  global write_only float* c )
                 {
                  int index = get_global_id(0);
                  c[index] = a[index] + b[index];
                 }
                 ";
            long count = 20;
            float[] arrA = new float[count];
            float[] arrB = new float[count];
            float[] arrC = new float[count];

            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                arrA[i] = (float)(rand.NextDouble() * 100);
                arrB[i] = (float)(rand.NextDouble() * 100);
            }

            ComputeBuffer<float> a = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, arrA);
            ComputeBuffer<float> b = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, arrB);
            ComputeBuffer<float> c = new ComputeBuffer<float>(context, ComputeMemoryFlags.WriteOnly, arrC.Length);

            ComputeProgram program = new ComputeProgram(context, new string[] { kernelSource });
            program.Build(null, null, null, IntPtr.Zero);

            ComputeKernel kernel = program.CreateKernel("VectorAdd");
            kernel.SetMemoryArgument(0, a);
            kernel.SetMemoryArgument(1, b);
            kernel.SetMemoryArgument(2, c);

            ComputeCommandQueue commands = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);

           // ComputeEventList events = new ComputeEventList();

            commands.Execute(kernel, null, new long[] { count }, null, null);

            //arrC = new float[count];
            //GCHandle arrCHandle = GCHandle.Alloc(arrC, GCHandleType.Pinned);

            unsafe
            {
                fixed(float* arrCPtr = arrC)
                {
                    commands.Read(c, false, 0, count, new IntPtr(arrCPtr), null);
                    commands.Finish();
                }
            }

            return; 
            //arrCHandle.Free();
        }

        private static void DoItInternal(int height, int width, int[] iterateValues, int maxDiff)
        {
            // pick the device platform 
            ComputePlatform intelGPU = ComputePlatform.Platforms.Where(n => n.Name.Contains("Intel")).First();

            ComputeContext context = new ComputeContext(
                ComputeDeviceTypes.Gpu, // use the gpu
                new ComputeContextPropertyList(intelGPU), // use the intel openCL platform
                null,
                IntPtr.Zero);

            // the command queue is the, well, queue of commands sent to the "device" (GPU)
            ComputeCommandQueue commandQueue = new ComputeCommandQueue(
                context, // the compute context
                context.Devices[0], // first device matching the context specifications
                ComputeCommandQueueFlags.None); // no special flags

            // read the open cl source. 
            string kernelSource = null;
            using (StreamReader sr = new StreamReader("kernel.cl"))
            {
                kernelSource = sr.ReadToEnd();
            }

            // create the "program"
            ComputeProgram program = new ComputeProgram(context, new string[] { kernelSource });

            // compile. 
            program.Build(null, null, null, IntPtr.Zero);
            ComputeKernel kernel = program.CreateKernel("doSomething");

            // input array.
            ComputeBuffer<int> intArray = new ComputeBuffer<int>(context,
                ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer,
                iterateValues);
            kernel.SetMemoryArgument(0, intArray);

            // output array. 
            int[] outputBuffer = new int[height * width];
            ComputeBuffer<int> outArray = new ComputeBuffer<int>(context,
                ComputeMemoryFlags.WriteOnly | ComputeMemoryFlags.UseHostPointer,
                outputBuffer);
            kernel.SetMemoryArgument(1, outArray);

            // execute the kernel. 
            commandQueue.Execute(kernel, null, new long[] { width * height }, null, null);

            unsafe
            {
                fixed (int* outputBufferPtr = outputBuffer)
                {
                    commandQueue.Read(outArray, false, 0, outputBuffer.Length, new IntPtr(outputBufferPtr), null);
                    commandQueue.Finish();
                }
            }

            return;
        }

        public static bool[] DoIt(int height, int width, int[] iterateValues, int maxDiff)
        {
            //Example(); 
            DoItInternal(height, width, iterateValues, maxDiff);

            return null; 
        }
    }
}

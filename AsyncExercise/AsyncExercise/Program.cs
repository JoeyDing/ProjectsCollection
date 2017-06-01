using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncExercise
{
    internal class Program
    {
        private static async void ProcessDataAsync()
        {
            // Start the HandleFile method.
            Task<int> task = HandleFileAsync("C:\\enable1.txt");

            //Control returns here before HandleFileAsync returns.
            // ... Prompt the user.
            Console.WriteLine("Please wait patiently " +
                "while I do something important.");

            //Wait for the HandleFile task to complete.

            //...Display its results.

            int x = await task;
            //int x = await HandleFileAsync("C:\\enable1.txt");
            Console.WriteLine("Count: " + x);
        }

        private static async Task<int> HandleFileAsync(string file)
        {
            Console.WriteLine("HandleFile enter");
            int count = 0;

            // Read in the specified file.
            // ... Use async StreamReader method.
            using (StreamReader reader = new StreamReader(file))
            {
                //string v = await reader.ReadToEndAsync();
                //// ... Process the file data somehow.
                //count += v.Length;
                //// ... A slow-running computation.
                ////     Dummy code.
                //for (int i = 0; i < 10000; i++)
                //{
                //    int x = v.GetHashCode();
                //    if (x == 0)
                //    {
                //        count--;
                //    }
                //}
                await Task.Delay(10000);
            }
            Console.WriteLine("HandleFile exit");
            return count;
        }

        private static void Main()
        {
            // Create task and start it.
            // ... Wait for it to complete.
            Task task = new Task(ProcessDataAsync);
            task.Start();
            task.Wait();
            Console.ReadLine();
        }
    }
}
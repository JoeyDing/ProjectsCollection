using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageViewer_Unittest
{
    [TestClass]
    public class TaskCancellationTest
    {
        [TestMethod]
        public void Test_TokenCacellation()
        {
            TryTask().Wait();
        }

        private async Task TryTask()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromSeconds(10));

            Task<int> task = Task.Run(() => slowFunc(1, 2, source.Token), source.Token);

            // (A canceled task will raise an exception when awaited).
            await task;
        }

        private int slowFunc(int a, int b, CancellationToken cancellationToken)
        {
            string someString = string.Empty;
            for (int i = 0; i < 200000; i++)
            {
                someString += "a";
                if (i % 1000 == 0)
                    cancellationToken.ThrowIfCancellationRequested();
            }

            return a + b;
        }
    }
}
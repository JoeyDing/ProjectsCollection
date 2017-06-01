using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LEAF.Plugins.ImageViewer
{
    public class CancelTaskWithCancellationTokenService
    {
        public async Task<bool> CancelWithToken(Action<CancellationToken> action, CancellationToken ct)
        {
            bool result = true;

            Thread thread = null;
            var task1 = Task.Run(() =>
            {
                while (true)
                {
                        //Thread.Sleep(1000);
                        ct.ThrowIfCancellationRequested();
                }
            });

            var task2 = Task.Run(() =>
            {
                    //do some heavy work
                    thread = Thread.CurrentThread;
                action(ct);
            });
            await Task.WhenAny(task1, task2);
            if (task1.IsCompleted)
            {
                thread.Abort();
                result = false;
                //throw new OperationCanceledException();
            }

            return result;
        }
    }
}
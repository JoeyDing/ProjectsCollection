using Sfb.Core.Interfaces;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Sfb.Core.Services
{
    public class CloseSfbClientService : ICloseSfbClient
    {
        public void CloseSfbClient()
        {
            //process
            var sfbProcess = Process.GetProcessesByName("lync").FirstOrDefault();
            if (sfbProcess != null)
            {
                sfbProcess.Kill();
                Thread.Sleep(3000);
            }
        }
    }
}
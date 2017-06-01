using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestStack.White;

namespace Sfb.Core.Services
{
    public class RunAppService : IRunApp
    {
        public Application RunApp(string path)
        {
            if (!SfbUtils.AppIsLaunched())
            {
                Application app = Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(path));
                Thread.Sleep(8000);
                return app;
            }
            else
            {
                Application app = Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(path));
                return app;
            }
        }
    }
}
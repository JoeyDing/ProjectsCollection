using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestStack.White;

namespace Sfb.Core.Services
{
    public class RunAppForRecordingManagerService : IRunAppForRecordingManager
    {
        private readonly string path;

        public RunAppForRecordingManagerService(string _path)
        {
            path = _path;
        }

        public Application RunAppForRecordingManager()
        {
            if (!SfbUtils.AppIsLaunchedForRecordingManager())
            {
                Application app = Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(path));
                Thread.Sleep(5000);
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
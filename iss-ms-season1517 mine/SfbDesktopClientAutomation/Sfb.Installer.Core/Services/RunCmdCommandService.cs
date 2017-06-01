using Sfb.Installer.Core.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;

namespace Sfb.Installer.Core
{
    public class RunCmdCommandService : IRunCmdCommand
    {
        public string RunCmdCommand(string command)
        {
            Thread.Sleep(2000);
            string output = "";
            ProcessStartInfo cmdsi = new ProcessStartInfo("cmd", @"/c " + command)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas"
            };

            Process cmd = Process.Start(cmdsi);
            cmd.WaitForExit();
            return output;
        }
    }
}
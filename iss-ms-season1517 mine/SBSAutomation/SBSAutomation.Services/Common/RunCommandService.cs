using SBSAutomation.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Services.Common
{
    public class RunCommandService : IRunCommand
    {
        public string RunCommand(string command)
        {
            string output = "";
            ProcessStartInfo cmdsi = new ProcessStartInfo("cmd", @"/c " + command)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                RedirectStandardError = true
            };

            Process cmd = Process.Start(cmdsi);
            cmd.WaitForExit();
            try
            {
                while (!cmd.StandardOutput.EndOfStream)
                {
                    output += cmd.StandardOutput.ReadLine();
                    output += cmd.StandardError.ReadLine();
                }

                return output;
            }
            catch (Exception e)
            {
                return e.Message + Environment.NewLine + e.InnerException;
            }
        }
    }
}
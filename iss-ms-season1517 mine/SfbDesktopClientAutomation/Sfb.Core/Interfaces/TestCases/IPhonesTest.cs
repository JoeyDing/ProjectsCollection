using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfb.Core.Interfaces.TestCases
{
    public interface IPhonesTest
    {
        bool Options_Phones(string languageShortName, TestCase testcase);
    }
}
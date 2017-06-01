using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts
{
    public interface ISendEmail
    {
        string EmailSubject { get; set; }
        string EmailBody { get; set; }
        string[] Attachments { get; set; }

        void SendEmail();
    }
}
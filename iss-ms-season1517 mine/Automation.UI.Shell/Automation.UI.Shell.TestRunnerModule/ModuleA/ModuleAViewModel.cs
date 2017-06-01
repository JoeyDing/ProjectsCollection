using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.Contracts;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.UI.Shell.Core;

namespace Automation.UI.Shell.TestRunnerModule.ModuleA
{
    public class ModuleAViewModel : TestRunnerControlViewModel, IModuleAViewModel
    {
        public ModuleAViewModel(
            IDispatcher dispatcher,
            ILanguagesProvider languagesProvider,
            ITestCasesProvider testCasesProvider,
            ILogStatusHistory statusHistoryService,
            IExpandStatusGroupView expandStatusService,
            ISendEmail sendEmailService,
            ISaveToExcel saveToExcelService,
            ITestRunnerView view,
            ITestCaseImageStore store) :
            base(dispatcher,
                languagesProvider,
                testCasesProvider,
                statusHistoryService,
                expandStatusService,
                sendEmailService,
                saveToExcelService,
                view,
                store)
        {
            this.EmailSubject = "Test Title from Module A";
            this.EmailBody = "Test Body from Module A";
        }
    }
}
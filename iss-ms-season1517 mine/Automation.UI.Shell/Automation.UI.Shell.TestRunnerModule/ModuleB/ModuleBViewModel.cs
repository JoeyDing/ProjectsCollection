using Automation.UI.Shell.Wpf.Infrastructure.ConfiguratonUserControl;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Automation.UI.Shell.TestRunnerModule.ModuleB
{
    public class ModuleBViewModel : ViewModelBase
    {
        public ICommand PopupConfigCommand { get; set; }

        public ModuleBViewModel(TestRunnerControlViewModel iTestRunnerViewModel)
        {
            iTestRunnerViewModel.EmailSubject = "Test Title from Module B";
            iTestRunnerViewModel.EmailBody = "Test Body from Module B";

            this.CustomContent = "Custom Content from View Model";
            iTestRunnerViewModel.TestCasesList.Clear();
            this.PopupConfigCommand = new DelegateCommand(onPopup);
        }

        private string customContent;

        public string CustomContent
        {
            get { return customContent; }
            set
            {
                customContent = value;
                OnPropertyChanged("CustomContent");
            }
        }

        private void onPopup(object param)
        {
            PopupWindow popup = new PopupWindow();
            popup.Show();
        }
    }
}
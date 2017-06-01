using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.TestRunnerModule.ModuleB
{
    public class ModuleBLanguagesProvider : ILanguagesProvider
    {
        public ObservableCollection<Language> GetLanguagesList()
        {
            return new ObservableCollection<Language> {
                new Language { CultureName = "ar-sa", IsChecked = true }
            };
        }
    }
}
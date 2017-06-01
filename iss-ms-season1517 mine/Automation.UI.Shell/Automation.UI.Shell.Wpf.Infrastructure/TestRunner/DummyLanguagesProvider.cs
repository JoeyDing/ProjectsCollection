using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner
{
    public class DummyLanguagesProvider : ILanguagesProvider
    {
        public ObservableCollection<Language> GetLanguagesList()
        {
            //for test only
            return new ObservableCollection<Language> {
                new Language {CultureName ="en-GB", IsChecked=true },
                new Language {CultureName ="fr-FR", IsChecked=false },
            };
        }
    }
}
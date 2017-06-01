using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.Contracts;
using System.Collections.ObjectModel;

namespace Sfb.UI.Shell.Module.Services
{
    public class LanguageProvider : ILanguagesProvider
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
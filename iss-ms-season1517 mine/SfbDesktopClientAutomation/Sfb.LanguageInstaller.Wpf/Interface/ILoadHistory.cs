using System.Collections.ObjectModel;

namespace Sfb.LanguageInstaller.Wpf.Interface
{
    public interface ILoadHistory
    {
        ObservableCollection<InstallerHistory> Load();

        void Save(ObservableCollection<InstallerHistory> history);
    }
}
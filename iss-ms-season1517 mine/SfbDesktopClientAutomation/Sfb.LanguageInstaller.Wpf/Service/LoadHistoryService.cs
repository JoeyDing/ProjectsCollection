using Sfb.LanguageInstaller.Wpf.Interface;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sfb.LanguageInstaller.Wpf.Service
{
    public class LoadHistoryService : ILoadHistory
    {
        public ObservableCollection<InstallerHistory> Load()
        {
            ObservableCollection<InstallerHistory> history = new ObservableCollection<InstallerHistory>();
            //string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"\log.dat");
            string logFile = AppDomain.CurrentDomain.BaseDirectory + "\\log.dat";
            if (File.Exists(logFile))
            {
                // Deserialize the list from a file
                var serializer = new BinaryFormatter();
                using (var stream = File.OpenRead(logFile))
                {
                    if (stream != null && stream.Length != 0)
                    {
                        history = (ObservableCollection<InstallerHistory>)serializer.Deserialize(stream);
                    }
                }
            }
            return history;
        }

        public void Save(ObservableCollection<InstallerHistory> history)
        {
            var serializer = new BinaryFormatter();
            //string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"\log.dat");
            string logFile = AppDomain.CurrentDomain.BaseDirectory + "\\log.dat";

            using (var stream = File.OpenWrite(logFile))
            {
                serializer.Serialize(stream, history);
            }
        }
    }
}
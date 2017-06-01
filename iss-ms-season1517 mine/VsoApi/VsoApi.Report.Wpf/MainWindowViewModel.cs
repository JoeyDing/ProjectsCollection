using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace VsoApi.Report.Wpf
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IFindTask findTaskService;

        public MainWindowViewModel(IFindTask findTaskService)
        {
            this.findTaskService = findTaskService;
        }

        private ObservableCollection<VsoItemResult> results;

        public ObservableCollection<VsoItemResult> Results
        {
            get
            {
                return this.results;
            }
            set
            {
                this.results = value;
                this.OnPropertyChanged("Results");
            }
        }

        public void ExportToExcel(RadGridView gridview)
        {
            string extension = "xls";
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = extension,
                Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Excel"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    gridview.Export(stream, new GridViewExportOptions()
                    {
                        Format = ExportFormat.ExcelML,
                        ShowColumnHeaders = true,
                        ShowColumnFooters = true,
                        ShowGroupFooters = true
                    });
                }
            }
        }

        public void Search(string input)
        {
            var stringIds = input.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (stringIds.Any())
            {
                List<long> allIds = new List<long>(); ;
                bool allParsed = true;
                foreach (var stringId in stringIds)
                {
                    long value = 0;
                    if (!long.TryParse(stringId, out value))
                    {
                        allParsed = false;
                        break;
                    }
                    allIds.Add(value);
                }
                if (allParsed)
                {
                    ObservableCollection<VsoItemResult> result = new ObservableCollection<VsoItemResult>();
                    foreach (var esId in allIds)
                    {
                        var tasks = findTaskService.GetTasksByParent(esId);
                        foreach (var item in tasks)
                        {
                            result.Add(item);
                        }
                    }

                    this.Results = result;
                }
            }
        }
    }
}
using SkypeIntlPortfolio.Ajax.Mvp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Files
{
    public interface IFilesView : IClickNext
    {
        event Func<List<UIResourceFile>> GetFilesData;

        event Action<UIResourceFile> UpdateFile;

        event Func<UIResourceFile, int> AddNewFile;

        event Action<int> DeleteFile;

        List<SelectableItem> FabricTenants { get; set; }
    }

    [Serializable]
    public class UIResourceFile
    {
        public int FileKey { get; set; }
        public string File_Name { get; set; }
        public string File_Type { get; set; }
        public string LCG_File_Location { get; set; }
        public string Source_File_Location { get; set; }
        public Int32? ParserID { get; set; }
        public string RepoURL { get; set; }
        public string RepoBranch { get; set; }
        public string RepoType { get; set; }
        public string Fabric_Project { get; set; }
        public string SelectedFabricTenant { get; set; }
        public List<SelectableItem> FabricTenants { get; set; }
    }
}
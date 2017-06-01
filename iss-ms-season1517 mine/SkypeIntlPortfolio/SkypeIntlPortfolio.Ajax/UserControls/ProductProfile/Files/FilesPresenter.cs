using SkypeIntlPortfolio.Ajax.Mvp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Files
{
    public class FilesPresenter : PresenterBase
    {
        private Products_New _product;
        private IFilesView _view;

        public event EventHandler OnClickNext;

        public FilesPresenter(IFilesView view, Products_New product)
        {
            this._product = product;
            this._view = view;
            this._view.OnClickNext += _view_OnClickNext;
            this._view.GetFilesData += _view_GetFilesData;
            this._view.AddNewFile += _view_AddNewFile;
            this._view.UpdateFile += _view_UpdateFile;
            this._view.DeleteFile += _view_DeleteFile;
        }

        private void _view_OnClickNext(object sender, EventArgs e)
        {
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }

        private List<UIResourceFile> _view_GetFilesData()
        {
            var result = new List<UIResourceFile>();
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var prod = context.Products_New.First(c => c.ProductKey == this._product.ProductKey);
                List<SelectableItem> selectableTenants = null;
                var allTenants = context.FabricTenants.ToList();
                var data = prod.ResourceFiles.ToList();
                foreach (var rFile in data)
                {
                    var currentTenant = new List<FabricTenant>();
                    if (rFile.FabricTenant == null)
                    {
                        selectableTenants = allTenants.Select(c => new SelectableItem
                        {
                            Text = c.Fabric_Tenant,
                            Value = c.TenantKey.ToString(),
                            IsSelected = false
                        }).ToList();
                    }
                    else
                    {
                        var unionTenants = this.GetUnionOfItems(
                            allTenants,
                            new List<FabricTenant> { rFile.FabricTenant },
                            (current) => current.TenantKey);
                        selectableTenants = unionTenants.Select(c => new SelectableItem
                        {
                            Text = c.Value.Fabric_Tenant,
                            Value = c.Value.TenantKey.ToString(),
                            IsSelected = c.IsCommon
                        }).ToList();
                    }

                    result.Add(new UIResourceFile
                    {
                        Fabric_Project = rFile.Fabric_Project,
                        LCG_File_Location = rFile.LCG_File_Location,
                        Source_File_Location = rFile.Source_File_Location,
                        ParserID = rFile.ParserID,
                        RepoURL = rFile.RepoURL,
                        RepoBranch = rFile.repoBranch,
                        RepoType = rFile.repoType,
                        File_Name = rFile.File_Name,
                        File_Type = rFile.File_Type,
                        FileKey = rFile.FileKey,
                        FabricTenants = selectableTenants,
                        SelectedFabricTenant = selectableTenants.Where(c => c.IsSelected == true).Select(c => c.Text).FirstOrDefault()
                    });
                }

                this._view.FabricTenants = allTenants.Select(c => new SelectableItem { Value = c.TenantKey.ToString(), Text = c.Fabric_Tenant }).ToList();
            }
            return result;
        }

        private int _view_AddNewFile(UIResourceFile file)
        {
            int result = 0;
            if (this._product != null)
            {
                using (var context = new SkypeIntlPlanningPortfolioEntities())
                {
                    var prod = context.Products_New.First(c => c.ProductKey == this._product.ProductKey);
                    var resourceFile = new ResourceFile
                    {
                        LCG_File_Location = file.LCG_File_Location,
                        Source_File_Location = file.Source_File_Location,
                        ParserID = (Int32)file.ParserID,
                        RepoURL = file.RepoURL,
                        repoBranch = file.RepoBranch,
                        repoType = file.RepoType,
                        File_Name = file.File_Name,
                        File_Type = file.File_Type,
                        Fabric_Project = file.Fabric_Project,
                        FabricTenantKey = file.FabricTenants.Where(c => c.IsSelected == true).Select(c => new int?(int.Parse(c.Value))).FirstOrDefault()
                    };

                    prod.ResourceFiles.Add(resourceFile);
                    context.SaveChanges();
                    file.FileKey = resourceFile.FileKey;
                }
            }

            return result;
        }

        private void _view_UpdateFile(UIResourceFile file)
        {
            if (this._product != null)
            {
                using (var context = new SkypeIntlPlanningPortfolioEntities())
                {
                    var prod = context.Products_New.First(c => c.ProductKey == this._product.ProductKey);
                    var resourceFile = new ResourceFile
                    {
                        FileKey = file.FileKey,
                        LCG_File_Location = file.LCG_File_Location,
                        Source_File_Location = file.Source_File_Location,
                        ParserID = (int)file.ParserID,
                        repoBranch = file.RepoBranch,
                        repoType = file.RepoType,
                        RepoURL = file.RepoURL,
                        File_Name = file.File_Name,
                        File_Type = file.File_Type,
                        Fabric_Project = file.Fabric_Project,
                        FabricTenantKey = file.FabricTenants.Where(c => c.IsSelected == true).Select(c => new int?(int.Parse(c.Value))).FirstOrDefault()
                    };

                    context.ResourceFiles.Attach(resourceFile);
                    context.Entry(resourceFile).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        private void _view_DeleteFile(int fileKey)
        {
            if (this._product != null)
            {
                using (var context = new SkypeIntlPlanningPortfolioEntities())
                {
                    var prod = context.Products_New.First(c => c.ProductKey == this._product.ProductKey);
                    var resourceFile = context.ResourceFiles.First(c => c.FileKey == fileKey);
                    prod.ResourceFiles.Remove(resourceFile);

                    //remove file if there is no other products linked to it
                    if (!resourceFile.Products_New.Any(c => c.ProductKey != prod.ProductKey))
                        context.ResourceFiles.Remove(resourceFile);
                    context.SaveChanges();
                }
            }
        }
    }
}
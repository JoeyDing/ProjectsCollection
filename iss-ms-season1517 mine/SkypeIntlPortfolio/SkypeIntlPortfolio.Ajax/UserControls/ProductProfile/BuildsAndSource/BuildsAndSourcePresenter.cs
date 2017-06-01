using SkypeIntlPortfolio.Ajax.Mvp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.BuildsAndSource
{
    public class BuildsAndSourcePresenter : PresenterBase
    {
        private IBuildsAndSourceView _view;
        private Products_New _product;

        public event EventHandler OnClickNext;

        public BuildsAndSourcePresenter(IBuildsAndSourceView view, Products_New product)
        {
            this._view = view;
            this._product = product;
            this._view.LoadBuildsAndSourceData += OnLoadBuildsAndSourceData;
            this._view.OnClickNext += view_OnClickNext;
        }

        private void view_OnClickNext(object sender, EventArgs e)
        {
            if (this._product != null)
            {
                using (var context = new SkypeIntlPlanningPortfolioEntities())
                {
                    var prod = context.Products_New.First(c => c.ProductKey == this._product.ProductKey);
                    prod.Localization_Code_Branch = this._view.SourceCodeLocation;

                    //source control
                    var currentSourceControls = prod.Source_Control_Interface.ToList().ToDictionary(c => c.SourceControlIntKey, c => c);
                    foreach (var item in this._view.SourceControl)
                    {
                        var itemKey = int.Parse(item.Value);
                        if (item.IsChecked)
                        {
                            if (!currentSourceControls.ContainsKey(itemKey))
                            {
                                var newLinkedItem = new Source_Control_Interface { SourceControlIntKey = itemKey };
                                //attach it to right table so that it doesn't create a new Source Control together with the link
                                context.Source_Control_Interfaces.Attach(newLinkedItem);
                                prod.Source_Control_Interface.Add(newLinkedItem);
                            }
                        }
                        else
                        {
                            if (currentSourceControls.ContainsKey(itemKey))
                            {
                                var itemToDelete = currentSourceControls[itemKey];
                                prod.Source_Control_Interface.Remove(itemToDelete);
                            }
                        }
                    }

                    //source storage
                    var currentSourceStorage = prod.SourceStorages.ToList().ToDictionary(c => c.SourceStorageKey, c => c);
                    foreach (var item in this._view.SourceStorage)
                    {
                        var itemKey = int.Parse(item.Value);
                        if (item.IsChecked)
                        {
                            if (!currentSourceStorage.ContainsKey(itemKey))
                            {
                                var newLinkedItem = new SourceStorage { SourceStorageKey = itemKey };
                                //attach it to right table so that it doesn't create a new Source Control together with the link
                                context.SourceStorages.Attach(newLinkedItem);
                                prod.SourceStorages.Add(newLinkedItem);
                            }
                        }
                        else
                        {
                            if (currentSourceStorage.ContainsKey(itemKey))
                            {
                                var itemToDelete = currentSourceStorage[itemKey];
                                prod.SourceStorages.Remove(itemToDelete);
                            }
                        }
                    }

                    //code review

                    prod.CodeReviewUsedKey = this._view.CodeReviewSystem
                        .Where(c => c.IsSelected)
                        .Select(c => new int?(int.Parse(c.Value)))
                        .FirstOrDefault();

                    //build system
                    prod.BuildSysKey = this._view.BuildSystems
                        .Where(c => c.IsSelected)
                        .Select(c => new int?(int.Parse(c.Value)))
                        .FirstOrDefault();

                    context.SaveChanges();
                }
            }

            if (this.OnClickNext != null)
                this.OnClickNext(this, e);
        }

        private void OnLoadBuildsAndSourceData()
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var prod = context.Products_New.First(c => c.ProductKey == this._product.ProductKey);
                this._view.SourceCodeLocation = prod.Localization_Code_Branch;

                //source control
                var allSourceControls = context.Source_Control_Interfaces.ToList();
                var userSourceControls = prod.Source_Control_Interface.ToList();
                var sourceControlsUnion = this.GetUnionOfItems<Source_Control_Interface, int>(allSourceControls, userSourceControls, (item) => item.SourceControlIntKey);

                this._view.SourceControl = sourceControlsUnion.Select(c => new CheckableItem
                {
                    Value = c.Value.SourceControlIntKey.ToString(),
                    Text = c.Value.Source_Control_Interface1,
                    IsChecked = c.IsCommon
                }).ToList();

                //source storage
                var allSourceStorages = context.SourceStorages.ToList();
                var userSourceStorage = prod.SourceStorages.ToList();
                var sourceStoragesUnion = this.GetUnionOfItems<SourceStorage, int>(allSourceStorages, userSourceStorage, (item) => item.SourceStorageKey);

                this._view.SourceStorage = sourceStoragesUnion.Select(c => new CheckableItem
                {
                    Value = c.Value.SourceStorageKey.ToString(),
                    Text = c.Value.Source_Storage,
                    IsChecked = c.IsCommon
                }).ToList();

                //code reviews
                var allCodeReviewSystems = context.CodeReviews.ToList();
                var currentCodeReview = new List<CodeReview>();
                if (prod.CodeReview != null)
                    currentCodeReview.Add(prod.CodeReview);

                var userCodeReview = prod.CodeReview;
                var codeReviewsUnion = this.GetUnionOfItems<CodeReview, int>(allCodeReviewSystems, currentCodeReview, (item) => item.CodeReviewKey);

                this._view.CodeReviewSystem = codeReviewsUnion.Select(c => new SelectableItem
                {
                    Value = c.Value.CodeReviewKey.ToString(),
                    Text = c.Value.Code_Review_Used,
                    IsSelected = c.IsCommon
                }).ToList();

                //build system
                var allbuildSystems = context.BuildSystems.ToList();
                var userbuildSystem = prod.BuildSystem;

                var currentBuildSystem = new List<BuildSystem>();
                if (prod.BuildSystem != null)
                    currentBuildSystem.Add(prod.BuildSystem);

                var buildSystemsUnion = this.GetUnionOfItems<BuildSystem, int>(allbuildSystems, currentBuildSystem, (item) => item.BuildSysKey);

                this._view.BuildSystems = buildSystemsUnion.Select(c => new SelectableItem
                {
                    Value = c.Value.BuildSysKey.ToString(),
                    Text = c.Value.Build_System_Used,
                    IsSelected = c.IsCommon
                }).ToList();
            }
        }
    }
}
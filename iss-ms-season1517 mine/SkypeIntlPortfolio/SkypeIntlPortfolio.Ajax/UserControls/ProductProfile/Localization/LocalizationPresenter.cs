using SkypeIntlPortfolio.Ajax.Mvp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Localization
{
    public class LocalizationPresenter : PresenterBase
    {
        private ILocalizationView _view;
        private Products_New _product;

        public event EventHandler OnClickNext;

        public LocalizationPresenter(ILocalizationView view, Products_New selectedProduct)
        //public LocalizationPresenter(ILocalizationView view, Dictionary<int,string> product)
        {
            this._product = selectedProduct;
            this._view = view;
            this._view.LoadLocalizationData += _view_LoadLocalizationData;
            this._view.OnClickNext += view_OnClickNext;
        }

        private void _view_LoadLocalizationData()
        {
            if (this._product != null)
            {
                using (var context = new SkypeIntlPlanningPortfolioEntities())
                {
                    var prod = context.Products_New.First(c => c.ProductKey == this._product.ProductKey);

                    //intl build process
                    var currentBuildProcess = new List<LocBuildProcess>();
                    if (prod.LocBuildProcess != null)
                        currentBuildProcess.Add(prod.LocBuildProcess);

                    var allBuildProcess = context.LocBuildProcesses.ToList();
                    var unionBuildProcess = this.GetUnionOfItems(currentBuildProcess, allBuildProcess, (current) => current.BuildProcessKey);

                    this._view.IntlBuildProcess = unionBuildProcess.Select(c =>
                        new SelectableItem
                        {
                            Value = c.Value.BuildProcessKey.ToString(),
                            Text = c.Value.Loc_Build_Process,
                            IsSelected = c.IsCommon
                        })
                    .ToList();

                    //loc process
                    var currentLocProcess = new List<LocProcess>();
                    if (prod.LocProcess != null)
                        currentLocProcess.Add(prod.LocProcess);

                    var allLocProcess = context.LocProcesses.ToList();
                    var unionLocProcess = this.GetUnionOfItems(currentLocProcess, allLocProcess, (current) => current.LocProcessKey);

                    this._view.LocProcess = unionLocProcess.Select(c =>
                        new SelectableItem
                        {
                            Value = c.Value.LocProcessKey.ToString(),
                            Text = c.Value.Loc_Process,
                            IsSelected = c.IsCommon
                        })
                    .ToList();
                }
            }
        }

        private void view_OnClickNext(object sender, EventArgs e)
        {
            if (this._product != null)
            {
                using (var context = new SkypeIntlPlanningPortfolioEntities())
                {
                    var prod = context.Products_New.First(c => c.ProductKey == this._product.ProductKey);

                    //intl build process
                    prod.LocBuildProcessKey = this._view.IntlBuildProcess
                                                        .Where(c => c.IsSelected == true)
                                                        .Select(c => new int?(int.Parse(c.Value))).FirstOrDefault();

                    //loc process
                    prod.LocProcessKey = this._view.LocProcess
                                                        .Where(c => c.IsSelected == true)
                                                        .Select(c => new int?(int.Parse(c.Value))).FirstOrDefault();

                    context.SaveChanges();
                }
            }
            //bubble up click event for any parent handler
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile
{
    public class ProductInfoNewPresenter
    {
        private IProductInfoNewView view;

        public event Action<List<string>> ProductPresenterNavigation;

        public ProductInfoNewPresenter(IProductInfoNewView view, Dictionary<int, string> product)
        {
            this.view = view;
            this.view.Product = product;
            this.view.Navigation += view_Navigation;
        }

        private void view_Navigation(List<string> navInfo)
        {
            //bubble up to the parent level which is management system page
            if (this.ProductPresenterNavigation != null)
                this.ProductPresenterNavigation(navInfo);
        }
    }
}
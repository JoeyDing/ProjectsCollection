using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Pages
{
    public interface IReportingSystemView
    {
        event Func<IEnumerable<IGrouping<string, Products_New>>> GetProductsFamilyList;

        event Func<IEnumerable<string>, Dictionary<int, string>> GetProductsWithCheckedLocations;

        event Func<Dictionary<int, string>, List<ProductInfo>> GetProductInfo;

        event Func<List<int>, bool> IsCancelledProduct;
    }
}
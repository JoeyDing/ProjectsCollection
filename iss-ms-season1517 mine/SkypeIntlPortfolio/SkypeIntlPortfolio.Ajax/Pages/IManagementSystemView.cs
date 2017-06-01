using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Pages
{
    public interface IManagementSystemView
    {
        event Func<string, IEnumerable<IGrouping<string, Products_New>>> GetProductsFamilyList;
        event Func<int[], bool> IsCancelledProduct;
    }
}
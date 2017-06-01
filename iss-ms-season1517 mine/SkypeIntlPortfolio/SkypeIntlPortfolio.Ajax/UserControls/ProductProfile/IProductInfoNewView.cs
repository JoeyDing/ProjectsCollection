using SkypeIntlPortfolio.Ajax.UserControls.Eol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile
{
    public interface IProductInfoNewView : ICommonNav
    {
        Dictionary<int, string> Product { get; set; }
    }
}
using SkypeIntlPortfolio.Ajax.UserControls.Eol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls
{
    public interface IEolView : ICommonNav
    {
        Dictionary<int, string> Product { get; set; }

        event Func<int, bool> IsProductCancelled;
    }
}
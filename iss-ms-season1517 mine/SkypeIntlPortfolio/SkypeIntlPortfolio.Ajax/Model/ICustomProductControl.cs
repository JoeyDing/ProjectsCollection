using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.Model
{
    internal interface ICustomProjectControl
    {
        ProductInfo ProductInfo { get; set; }

        void Refresh();
    }
}
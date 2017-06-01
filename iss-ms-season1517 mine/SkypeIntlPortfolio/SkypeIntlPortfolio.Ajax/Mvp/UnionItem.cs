using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Mvp
{
    public class UnionITem<T>
    {
        public T Value;
        public bool IsCommon { get; set; }
    }
}
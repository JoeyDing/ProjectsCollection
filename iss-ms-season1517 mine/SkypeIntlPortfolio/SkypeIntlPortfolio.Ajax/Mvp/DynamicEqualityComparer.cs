using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Mvp
{
    public class DynamicEqualityComparer<T> : IEqualityComparer<T>
    {
        public DynamicEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            this.equals = equals;
            this.getHashCode = getHashCode;
        }

        private readonly Func<T, T, bool> equals;

        public bool Equals(T x, T y)
        {
            return equals(x, y);
        }

        private readonly Func<T, int> getHashCode;

        public int GetHashCode(T obj)
        {
            return getHashCode(obj);
        }
    }
}
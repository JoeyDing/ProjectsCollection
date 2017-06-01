using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Mvp
{
    public class PresenterBase
    {
        protected ICollection<UnionITem<T>> GetUnionOfItems<T, Key>(List<T> listA, List<T> listB, Func<T, Key> GetKey)
        {
            //find common items
            var hashCommon = new HashSet<Key>(listA.Select(c => GetKey(c)));
            hashCommon.IntersectWith(listB.Select(c => GetKey(c)));

            //group items of listA and listB together
            var hashUnion = new HashSet<T>(listA, new DynamicEqualityComparer<T>(
                (a, b) => object.Equals(GetKey(a), GetKey(b)), current => GetKey(current).GetHashCode()));
            hashUnion.UnionWith(listB);

            //convert to dictionary of unionItem
            var dictUnion = hashUnion.ToDictionary(x => GetKey(x), x => new UnionITem<T> { Value = x, IsCommon = false });

            //mark common items to "Common" in grouped list
            foreach (var item in hashCommon)
            {
                dictUnion[item].IsCommon = true;
            }

            return dictUnion.Values;
        }
    }
}
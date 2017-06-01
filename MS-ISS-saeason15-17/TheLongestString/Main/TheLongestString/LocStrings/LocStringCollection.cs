using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TheLongestString
{
    /// <summary>
    /// List of LocString translations of a given Id, and some aggregate properties.
    /// </summary>
    public class LocStringCollection
    {
        private IEnumerable<LocString> _items;

        public IEnumerable<LocString> Items
        {
            get
            {
                return _items;
            }
        }

        public LocStringCollection(IEnumerable<LocString> sourceItems)
        {
            this._items = sourceItems;
        }

        public string Id { get; set; }

        public double MaximumWidth { get; set; }

        public double MaximumHeight { get; set; }

        public double AverageWidth { get; set; }

        public double AverageHeight { get; set; }

        public double SourceCultureWidth { get; set; }

        public double SourceCultureHeight { get; set; }

        
    }
}
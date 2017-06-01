using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteelheadDataParser.Core
{
    public class DBNameAttribute : Attribute
    {
        public readonly string _dbName;

        public DBNameAttribute(string dbName)
        {
            this._dbName = dbName;
        }
    }
}
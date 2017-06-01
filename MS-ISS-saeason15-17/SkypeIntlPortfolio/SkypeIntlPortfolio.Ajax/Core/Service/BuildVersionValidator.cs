using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Core.Service
{
    public class BuildVersionValidator
    {
        public bool IsBuildVersionValid(string buildVersion, bool isLyncServer)
        {
            if (string.IsNullOrWhiteSpace(buildVersion))
                return false;
            string[] buildVersionParts;
            if (isLyncServer)
            {
                buildVersionParts = buildVersion.Split('.');
                if (buildVersionParts.Count() != 4)
                    return false;
                foreach (string part in buildVersionParts)
                {
                    int x;
                    if (!(int.TryParse(part, out x) && x >= 0))
                        return false;
                }
                return true;
            }
            else
            {
                buildVersionParts = buildVersion.Split('_');
                if (buildVersionParts.Count() != 2)
                    return false;

                int x;
                if (!(int.TryParse(buildVersionParts[0], out x) && x >= 0 && buildVersionParts[0].Length == 8))
                    return false;
                int y;
                if (!(int.TryParse(buildVersionParts[1], out y) && y >= 0 && buildVersionParts[1].Length == 6))
                    return false;

                return true;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Core.Service
{
    public class SourcePathValidator
    {
        public bool IsValid(string sourcePath, bool isLyncServer)
        {
            sourcePath = sourcePath.Trim().Remove(0, 2).ToLower();
            if (isLyncServer)
                return !(sourcePath.Contains("[") ||
                        sourcePath.Contains("]") ||
                        sourcePath.Contains("major minor prefix") ||
                        sourcePath.Contains("product folder name") ||
                        sourcePath.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).Count() != 5);
            else
                return !(sourcePath.Contains("[") ||
                       sourcePath.Contains("]") ||
                       sourcePath.Contains("product folder name") ||
                       sourcePath.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).Count() != 4);
        }
    }
}
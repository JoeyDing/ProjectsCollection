using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Core.Services
{
    public interface IVerifyExistingFiles
    {
        bool VerifyExistingFiles(string sourceFolderDirectory, string targetFileDirectory,string resultFileName);
    }
}

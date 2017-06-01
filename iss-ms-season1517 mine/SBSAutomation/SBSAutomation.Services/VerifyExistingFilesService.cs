using System;
using System.Collections.Generic;
using SBSAutomation.Core.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SBSAutomation.Services
{
    public class VerifyExistingFilesService : IVerifyExistingFiles
    {
        public bool VerifyExistingFiles(string sourceFolderDirectory, string targetFileDirectory,string resultFileName)
        {
            //check if the source folder is there or not
            bool sourceDirectoryExists = System.IO.Directory.Exists(sourceFolderDirectory);
            if(!sourceDirectoryExists)
            {
                throw new ArgumentException("Source folder doesn't exist.");
            }
            else
            {
                bool targetDirectoryExists = System.IO.Directory.Exists(targetFileDirectory);
                if(!targetDirectoryExists)
                {
                    throw new ArgumentException("Target folder doesn't exist.");
                }

                else
                {
                    //clean up the target files
                    File.Delete(targetFileDirectory +@"\"+ resultFileName);
                    string[] sourceFoldeArray = Directory.GetFiles(sourceFolderDirectory);
                    //export into a .txt file
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(targetFileDirectory, resultFileName)))
                    {
                        foreach (string line in sourceFoldeArray)
                            outputFile.WriteLine(Path.GetFileName(line));
                    }
                }
            }
            return true;
        }
    }
}
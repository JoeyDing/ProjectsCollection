using System;
using System.Collections.Generic;
using SBSAutomation.Core.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SBSAutomation.Services
{
    public class SaveService : ISave
    {
        public void Save(string path, byte[] screenshot)
        {
            using (var filestream = new FileStream(path, FileMode.Create))
            {
                filestream.Write(screenshot, 0, screenshot.Count());
            }
    }
    }
}

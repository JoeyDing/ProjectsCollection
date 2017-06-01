using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfb.LanguageInstaller.Wpf.Interface
{
    internal interface ICanSerialize
    {
        void Serialize(string fileName, object item);
    }
}
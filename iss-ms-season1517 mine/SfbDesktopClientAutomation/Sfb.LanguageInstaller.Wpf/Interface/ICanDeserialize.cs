using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfb.LanguageInstaller.Wpf.Interface
{
    public interface ICanDeserialize
    {
        T Deserialize<T>(string filePath) where T : class;
    }
}
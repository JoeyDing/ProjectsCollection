using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfb.Core.Interfaces
{
    public interface IExecuteAction
    {
        T ExecuteAction<T>(string description, Func<T> action);

        void ExecuteAction(string description, Action action);
    }
}
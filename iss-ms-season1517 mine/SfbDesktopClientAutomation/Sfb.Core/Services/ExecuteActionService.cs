using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfb.Core.Services
{
    public class ExecuteActionService : IExecuteAction
    {
        public void ExecuteAction(string description, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                var error = new Exception(string.Format("Failed step: \"{0}\"", description), ex);
                throw error;
            }
        }

        public T ExecuteAction<T>(string description, Func<T> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                var error = new Exception(string.Format("Failed step: \"{0}\"", description), ex);
                throw error;
            }
        }
    }
}
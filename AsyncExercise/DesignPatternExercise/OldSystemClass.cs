using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternExercise
{
    public class OldSystemClass
    {
        /// <summary>
        /// imagine this is a method of an API
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> MethodOfOldSystem()
        {
            return new Dictionary<string, string>() { { "X", "Y" } };
        }
    }
}
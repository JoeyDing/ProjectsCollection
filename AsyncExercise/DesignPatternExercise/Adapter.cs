using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternExercise
{
    public class Adapter : NewSystemClass
    {
        //old system(API method) is not allowed to be changed, but user needs the new standard to get a new type of inut and putput.
        private OldSystemClass _oldSystemClass;

        public Adapter(OldSystemClass oldSystemClass)
        {
            this._oldSystemClass = oldSystemClass;
        }

        //convert origianl dictiontary to a single list, this step will bedone via new sytem method.
        public override List<string> GetAllKeysFromOldSystem()
        {
            // old method output will be input of new system method.
            Dictionary<string, string> oldData = _oldSystemClass.MethodOfOldSystem();
            List<string> newlist = new List<string>();
            newlist.Add(oldData.First().Key);
            return newlist;
        }
    }
}
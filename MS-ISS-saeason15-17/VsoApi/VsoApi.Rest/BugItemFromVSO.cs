using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoApi.Rest
{
    public class BugItemFromVSO
    {
        public int BugID { get; set; }
        public string BugState { get; set; }
        public string Title { get; set; }
        public List<Tuple<string, DateTime>> Comments { get; set; }
    }
}
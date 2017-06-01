using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoApi.Report.Wpf
{
    public class VsoItemResult
    {
        public long ID { get; set; }
        public int Rev { get; set; }
        public long? ParentID { get; set; }
        public string ParentTitle { get; set; }
        public string VsoType { get; set; }

        public string Title { get; set; }
        public double? CompletedWork { get; set; }
        public double? RemainingWork { get; set; }

        public double? EstimatedWork { get; set; }

        public string AssignedTo { get; set; }

        public string State { get; set; }

        public string VsoUrl { get; set; }
    }
}
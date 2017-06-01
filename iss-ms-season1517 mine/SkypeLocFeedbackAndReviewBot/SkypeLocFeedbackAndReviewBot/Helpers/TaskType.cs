using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeLocFeedbackAndReviewBot.Helpers
{
    public class TaskType
    {
        public string Value { get; set; }

        public TaskType(string value)
        {
            this.Value = value;
        }
    }
}
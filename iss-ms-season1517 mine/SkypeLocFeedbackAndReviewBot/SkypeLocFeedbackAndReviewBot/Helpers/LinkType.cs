using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeLocFeedbackAndReviewBot.Helpers
{
    public class LinkType
    {
        public string Value { get; set; }

        public LinkType(string value)
        {
            this.Value = value;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeLocFeedbackAndReviewBot.Helpers
{
    public static class LinkTypes
    {
        public static LinkType Child;
        public static LinkType Related;

        static LinkTypes()
        {
            Child = new LinkType("System.LinkTypes.Hierarchy-Reverse");
            Related = new LinkType("System.LinkTypes.Related");
        }
    }
}
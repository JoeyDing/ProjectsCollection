using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocaLiz.Models
{
    public class ConversationModel
    {
        public int Id { get; set; }
        public string Activity { get; set; }

        public string Message { get; set; }

        public string ConversationId { get; set; }
        public string ConversationName { get; set; }

        public string MessageId { get; set; }
        public string FromId { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public string Scenario { get; set; }
        public string ScenarioTag { get; set; }
        public string Type { get; set; }

        public string MessageLocale { get; set; }
        
        public ConversationModel()
        {
            CreatedDateTime = new DateTime();
        } 
    }
}
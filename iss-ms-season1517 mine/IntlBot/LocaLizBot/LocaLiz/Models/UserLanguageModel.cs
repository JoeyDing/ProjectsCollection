using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocaLiz.Models
{
    public class UserLanguageModel
    {
        public int Id { get; set; }
        public string userId { get; set; }

        public string LanguageIso { get; set; }
    }
}
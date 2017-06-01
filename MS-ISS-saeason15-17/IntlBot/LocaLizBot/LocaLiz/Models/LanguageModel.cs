using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocaLiz.Models
{
    public class LanguageModel
    {
        public int Id { get; set; }
        public string ISO { get; set; }
        public string NameInEnglish { get; set; }
        
        public string OriginalName { get; set; }
  
    }
}
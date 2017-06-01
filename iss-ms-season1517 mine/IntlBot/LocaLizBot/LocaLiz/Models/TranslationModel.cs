using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocaLiz.Models
{
    // userId, nativeLanguage, KnownLanguages, SuggestedTranslations, TranslationScores, IsAnyTranslationInProduction, WillJudgeLanguagePairs
    public class TranslationOriginalModel
    {
        public int Id { get; set; }
        public string ResourseId { get; set; }
        public string StringValue { get; set; }
        public string LanguageIso { get; set; }

    }


    public class TranslationFeedbackModel
    {
        public int Id { get; set; }
        public string ResourseId { get; set; }
     
        public string UserId { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDateTime { get; set; }   

        public string LanguageIso { get; set; }
    }

    public class SuggestedTranslationModel
    {
        public int Id { get; set; }
        public string ResourseId { get; set; }

        public string UserId { get; set; }
        public string  StringValue { get; set; }
        public string LanguageIso { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class SuggestedTranslationFeedback
    {
        public int Id { get; set; }

        public int SuggestedTranslationFeedbackId { get; set; }
        public string ResourseId { get; set; }

        public string UserId { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

}
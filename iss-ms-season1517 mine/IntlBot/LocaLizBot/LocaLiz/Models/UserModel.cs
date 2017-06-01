using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocaLiz.Models
{
    // userId, nativeLanguage, KnownLanguages, SuggestedTranslations, TranslationScores, IsAnyTranslationInProduction, WillJudgeLanguagePairs
    public class UserModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string DisplayName { get; set; }
        public string ChatService { get; set; }

        public bool WillJudge { get; set; }

        public string NativeLanguage { get; set; }

        public UserGender Gender { get; set; }
    }

    public enum UserGender 
    {
        Male = 2,
        Female = 4,
        Other = 8,
        Unspecified = 0
    }
}
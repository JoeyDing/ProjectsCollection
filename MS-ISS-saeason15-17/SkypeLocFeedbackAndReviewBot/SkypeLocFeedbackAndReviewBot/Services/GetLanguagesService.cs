using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeLocFeedbackAndReviewBot.Services
{
    public class GetLanguagesService
    {
        private readonly string[][] languages;

        public GetLanguagesService()
        {
            // lets read the Data\Langauges.csv
            // it contains (0st line is header)
            // iso and language name translation in all some languages
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/Data/Languages.csv");
            var lines = System.IO.File.ReadAllLines(path);
            if (lines.Length > 1)
            {
                languages = new string[lines.Length - 1][];
                for (int i = 1; i < lines.Length; i++)
                {
                    languages[i-1] = lines[i].Split(';').Select(x=>x.ToLowerInvariant().Substring(1,x.Length-2)).ToArray();
                }
            }
        }

        public List<string> GetLanguages(string input)
        {
            List<string> foundLanguageMatch = new List<string>();

            var sanitizedInput = input.Trim().ToLowerInvariant();
            for (int i = 0; i < languages.Length; i++)
            {                
                for (int j = 0; j < languages[i].Length; j++)
                {
                    // this can be made better with levenshtein distance. Question of course is if this is good enough.
                    if (languages[i][j] == input)
                    {
                        foundLanguageMatch.Add(languages[i][1]);
                        break;
                    }
                }
            }

            return foundLanguageMatch;

        }

     
    }
}
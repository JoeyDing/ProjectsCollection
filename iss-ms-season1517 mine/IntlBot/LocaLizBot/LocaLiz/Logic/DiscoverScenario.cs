using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace LocaLiz.Logic
{
    public class DiscoverScenario : BotScenarioBase
    {
        public DiscoverScenario(Activity activity) : base(BotScenario.Discover.ToString(),activity)
        {
        }

        public override async Task ProcessMessage()
        {
            initCulture();

            if (this.previousBot.ScenarioTag == "nativeQ")
            {
                // Validate against ISOs, or names or translations
                var lang = db.Languages.FirstOrDefault(x => x.ISO == Message || x.NameInEnglish == Message || x.OriginalName == Message);
                if (lang != null)
                {
                    user.NativeLanguage = lang.ISO;
                    await db.SaveChangesAsync();                   
                    await PickLanguage(null,Properties.Resources.DiscoverUserLanguageGood, BotScenario.Discover,"goodQ");
                }
                else
                {
                    await PickLanguage(null, Properties.Resources.DiscoverUserLanguageNative, BotScenario.Discover, "nativeQ");
                }
            }
            else if (previousBot.ScenarioTag == "goodQ")
            {
                // parse userInputText / split / 
                var langISOs = Message.Split(new char[] { ',', ' ' }); // comma needs to be localised and there could be more alternative spearators..
                bool langFound = false;
                foreach (var langIso in langISOs)
                {
                    //
                    var lang = db.Languages.FirstOrDefault(x => x.ISO == langIso || x.NameInEnglish == langIso || x.OriginalName == langIso);
                    if (lang != null)
                    {
                        var ulm = new Models.UserLanguageModel()
                        {
                            LanguageIso = lang.ISO,
                            userId = user.UserId
                        };
                        db.UserLanguages.Add(ulm);
                        langFound = true;
                    }
                }
                if (langFound)
                {
                    await db.SaveChangesAsync();
                    await BotResponse(Properties.Resources.DiscoverUserGoodReponse, BotScenario.Discover, "");
                    await DisoverGender("", Properties.Resources.Discover_What_Gender, "genderQ");

                }
                else
                {
                    await PickLanguage(null, Properties.Resources.DiscoverUserLanguageGood, BotScenario.Discover, "goodQ");
                }

            }else if (previousBot.ScenarioTag == "genderQ")
            {
                if (Message == Properties.Resources.Discover_Male)
                {
                    user.Gender = Models.UserGender.Male;
                }
                else if (Message == Properties.Resources.Discover_Female)
                {
                    user.Gender = Models.UserGender.Female;
                }
                else
                {
                    user.Gender = Models.UserGender.Other;
                }                

                await EvaluateNoYesQ(null, Properties.Resources.Evaluate_Init, "initQ");
            }

            await base.ProcessMessage();
        }
    }
}
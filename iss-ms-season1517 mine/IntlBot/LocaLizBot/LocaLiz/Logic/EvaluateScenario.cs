using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LocaLiz.Logic
{
    public class EvaluateScenario : BotScenarioBase
    {
        public EvaluateScenario(Activity activity) : base(BotScenario.Evaluate.ToString(),activity)
        {

        }

        public override async Task ProcessMessage()
        {

            initCulture();
            if (previousBot.ScenarioTag == "initQ")
            {
                // response can be both positive and negative
                if (Message.ToLowerInvariant() == Properties.Resources.AnswerYes.ToLowerInvariant())
                {                    
                    await EvaluateStringQ();                    
                }
                else
                {
                    await BotResponse(Properties.Resources.ThankYou, BotScenario.Option, "");
                }
            }
            else if (previousBot.ScenarioTag.StartsWith("evalQ"))
            {
                var trail = previousBot.ScenarioTag.Substring("evalQ".Length+1);
                // it can be number or something else if else, then repeat question unless No..

                int number;
                if (int.TryParse(Message, out number))
                {
                    var t = previousBot.ScenarioTag.Split(new char[] { '|' });
                    var resourseId = t[1];
                    var languageIso = t[2];
                    var feedback = new Models.TranslationFeedbackModel();
                    feedback.Comment = "Gen1";
                    feedback.CreatedDateTime = (Activity.Timestamp ?? DateTime.Now);
                    feedback.ResourseId = resourseId;
                    feedback.LanguageIso = languageIso;
                    feedback.Score = number;
                    feedback.UserId = user.UserId;

                    db.TranslationFeedbacks.Add(feedback);

                    if (number <= 10 && number >= 7)
                    {
                        
                        await BotResponse(Properties.Resources.QualityExcelent, BotScenario.Evaluate, "");                        
                        await EvaluateNoYesQ(null,Properties.Resources.ContinueWithEvaluations, "initQ");
                    }
                    else if (number < 7 && number > 4)
                    {
                        //await BotResponse(Properties.Resources.QualityMedicore, BotScenario.Evaluate, "suggestQ");
                        await EvaluateNoYesQ(null,Properties.Resources.QualityMedicore, "suggestQ|"+trail);
                    }
                    else
                    {
                        //await BotResponse(Properties.Resources.QualityYouCanBetter, BotScenario.Evaluate, "suggestQ");
                        await EvaluateNoYesQ(null,Properties.Resources.QualityYouCanBetter, "suggestQ|"+trail);
                    }
                }
            }
            else if (previousBot.ScenarioTag.StartsWith("suggestQ"))
            {
                var trail = previousBot.ScenarioTag.Substring("suggestQ".Length+1);
                // response can be both positive and negative
                if (Message.ToLowerInvariant() == Properties.Resources.AnswerYes.ToLowerInvariant())
                {
                    await BotResponse(Properties.Resources.Evaluate_EnterSuggestion, BotScenario.Evaluate, "transQ|" + trail);
                }
                else
                {
                    await EvaluateNoYesQ(null,Properties.Resources.Evaluate_Init,"initQ");
                }
            }
            else if (previousBot.ScenarioTag.StartsWith("transQ"))
            {
                var t = previousBot.ScenarioTag.Split(new char[] { '|' });
                var resourseId = t[1];
                var languageIso = t[2];


                var suggestedTranslation = new Models.SuggestedTranslationModel()
                {
                    CreatedDateTime = Activity.Timestamp ?? DateTime.Now,
                    LanguageIso = languageIso,
                    ResourseId = resourseId,
                    StringValue = Message,
                    UserId  = user.UserId
                };
                db.SuggestedTranslations.Add(suggestedTranslation);

                
                // adds suggested translation to database
                await BotResponse(Properties.Resources.ThankYou, BotScenario.Option, "");
                await EvaluateNoYesQ(null,Properties.Resources.Evaluate_Init, "initQ");


            }

            await base.ProcessMessage();
        }


    }
}
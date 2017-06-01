using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace LocaLiz.Logic
{
    public class CommandScenario : BotScenarioBase
    {
        public CommandScenario(Activity activity) : base(BotScenario.Command.ToString(), activity)
        {

        }

        public override async Task ProcessMessage()
        {
            initCulture();
            var commandMsg = Message.ToLowerInvariant();

            if (previousBot.ScenarioTag == "langQ")
            {
                if (SwitchToLanguage(commandMsg))
                {
                    userData.SetProperty<string>("ClientLanguage", commandMsg);

                    await stateClient.BotState.SetUserDataAsync(Activity.ChannelId, Activity.From.Id, userData);

                    await BotResponse(String.Format(Properties.Resources.Command_SwtichToLanguage,Thread.CurrentThread.CurrentUICulture.NativeName), BotScenario.Command, "");
                }
                else
                {
                    await BotResponse(Properties.Resources.Command_DoNotUnderstandLangauge, BotScenario.Command, "");
                }
            }
            else
            if (commandMsg == "42")
            {
                await BotResponse(Properties.Resources.Hack42, BotScenario.Command, "");

            }
            else if (commandMsg.CompareTo(Properties.Resources.StatusReport.ToLowerInvariant()) == 0)
            {
                string message = String.Empty;

                if (false) // user has production string translation
                {
                    message = String.Format(Properties.Resources.StatusReportExtendedAnswer, 13, 0, 1);
                }
                else
                {
                    message = String.Format(Properties.Resources.StatusReportAnswer, 13, 0);
                }


                await BotResponse(message, BotScenario.Command, "");

                await EvaluateNoYesQ(null, Properties.Resources.Evaluate_Init, "initQ");
            }
            else if (commandMsg == "language")
            {
                await PickLanguage(Properties.Resources.Command_Select_One, Properties.Resources.Command_In_What_Language, BotScenario.Command, "langQ");
            }
            else if (commandMsg.StartsWith("language"))
            {
                string tgtLanguage = commandMsg.Split(new char[] { ' ' })[1].ToLowerInvariant();
                if (SwitchToLanguage(tgtLanguage))
                {
                    userData.SetProperty<string>("ClientLanguage", tgtLanguage);

                    await stateClient.BotState.SetUserDataAsync(Activity.ChannelId, Activity.From.Id, userData);

                    await BotResponse(String.Format(Properties.Resources.Command_SwtichToLanguage, Thread.CurrentThread.CurrentUICulture.NativeName), BotScenario.Command, "");
                }
                else
                {
                    await BotResponse(Properties.Resources.Command_DoNotUnderstandLangauge, BotScenario.Command, "");
                }
            }
            else
            {
                await EvaluateNoYesQ(null, Properties.Resources.Evaluate_Init, "initQ");
            }


            // Change locale
            // Update known language list
            // Check know language list
            // 

            await base.ProcessMessage();
        }
    }
}
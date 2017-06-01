using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LocaLiz.Logic
{
    public class NewUser : BotScenarioBase
    {
        public NewUser(Activity activity) : base("", activity)
        {

        }

        public override async Task ProcessMessage()
        {
            initCulture();
            var newUser = new Models.UserModel() { ChatService = Activity.ChannelId, DisplayName = Activity.From.Name, UserId = Activity.From.Id, Gender = Models.UserGender.Unspecified };
            db.Users.Add(newUser);
            await db.SaveChangesAsync();

            await BotResponse(Properties.Resources.WelcomeMessage, BotScenario.Command, "");
            await PickLanguage(null, Properties.Resources.DiscoverUserLanguageNative, BotScenario.Discover, "nativeQ");

            await base.ProcessMessage();
        }
    }
}
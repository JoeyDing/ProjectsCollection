using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LocaLiz.Logic
{
    public class OptionScenario : BotScenarioBase
    {
        public OptionScenario(Activity activity) : base(BotScenario.Option.ToString(), activity)
        {

        }

        public override Task ProcessMessage()
        {
            initCulture();


            return base.ProcessMessage();
        }
    }
}
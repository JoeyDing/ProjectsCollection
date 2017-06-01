using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocaLiz.Logic
{
    public class BotScenario
    {
        public static BotScenario Discover = new BotScenario("Discover");
        public static BotScenario Evaluate = new BotScenario("Evaluate");
        public static BotScenario NewUser = new BotScenario("NewUser");
        public static BotScenario Option = new BotScenario("Option");
        public static BotScenario Command = new BotScenario("Command");

        public BotScenario(string scenario)
        {
            Scenario = scenario;
        }
        string Scenario { get; set; }

        public override string ToString()
        {
            return Scenario;
        }

        public override bool Equals(object obj)
        {
            if (obj is BotScenario)
                return Scenario.Equals((obj as BotScenario).Scenario);
            if (obj is String)
                return Scenario.Equals(obj);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Scenario.GetHashCode();
        }
    }
}
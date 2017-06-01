using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LocaLiz.Models;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using System.Threading;
using System.Globalization;

namespace LocaLiz.Logic
{
    public class BotScenarioBase : IComparable<BotScenarioBase>
    {
        protected static LocaLizDataContext db = new LocaLizDataContext();
        protected string ScenarioName = String.Empty;
        protected Activity Activity;
        protected ConnectorClient Connector = null;

        public static string BotId = "LocaLizBot";
        protected static UserModel user = null;
        protected ConversationModel previousBot = null;
        protected string Message = null;
        protected StateClient stateClient = null;
        protected BotData userData = null;
        
        //protected ConversationModel currentBot = null;

        public static async Task<BotScenarioBase> ProcessBotScenario(Activity a)
        {

            if (a.Conversation != null)
            {
                var previousBot = db.Stuffs.Where(x => x.ConversationId == a.Conversation.Id && x.FromId == BotId).OrderByDescending(x => x.CreatedDateTime).FirstOrDefault();                
                if (previousBot != null && a.From != null)
                {
                    user = db.Users.FirstOrDefault(x => x.UserId == a.From.Id);
                    if (user != null)
                    {
                        var message = a.Text ?? string.Empty;

                        if (!String.IsNullOrEmpty(previousBot.ScenarioTag))
                        {
                            if (previousBot.Scenario == BotScenario.Discover.ToString())
                            {
                                return new DiscoverScenario(a);
                            }
                            else if (previousBot.Scenario == BotScenario.Evaluate.ToString())
                            {
                                return new EvaluateScenario(a);
                            }
                            else if (previousBot.Scenario == BotScenario.NewUser.ToString())
                            {
                                return new NewUser(a);
                            }
                            else if (previousBot.Scenario == BotScenario.Option.ToString())
                            {
                                return new OptionScenario(a);
                            }
                        }                                                
                        return new CommandScenario(a);
                        

                    }
                    else
                    {
                        var newUser = new Models.UserModel() { ChatService = a.ChannelId, DisplayName = a.From.Name, UserId = a.From.Id };
                        db.Users.Add(newUser);
                        await db.SaveChangesAsync();

                        var scenario = new NewUser(a);
                        await scenario.BotResponse(Properties.Resources.DiscoverUserLanguageNative, Logic.BotScenario.Discover, "nativeQ");
                        return scenario;
                    }
                    
                }
            }
            return new NewUser(a);
        }


        protected async Task BotResponse(string reply, BotScenario scenario, string tag)
        {
            var c = new Models.ConversationModel()
            {
                ConversationId = Activity.Conversation.Id,
                ConversationName = Activity.Conversation.Name,
                Message = reply,
                CreatedDateTime = DateTime.Now,
                FromId = BotId,
                Type = Activity.Type,
                Scenario = scenario.ToString(),
                ScenarioTag = tag
            };
            db.Stuffs.Add(c);
            await db.SaveChangesAsync();

            Activity r1 = Activity.CreateReply(reply);
            await Connector.Conversations.ReplyToActivityAsync(r1);
        }

        protected async Task BotResponse(Activity replyToUser, BotScenario scenario, string tag)
        {
            var c = new Models.ConversationModel()
            {
                ConversationId = Activity.Conversation.Id,
                ConversationName = Activity.Conversation.Name,
                Message = replyToUser.Text,
                CreatedDateTime = DateTime.Now,
                FromId = BotId,
                Type = Activity.Type,
                Scenario = scenario.ToString(),
                ScenarioTag = tag
            };
            db.Stuffs.Add(c);
            await db.SaveChangesAsync();

            
            await Connector.Conversations.ReplyToActivityAsync(replyToUser);
        }

        public BotScenarioBase(string scenarioName, Activity activity)
        {
            Activity = activity;
            Connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            stateClient = activity.GetStateClient();
            userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);
            initCulture();


            if (activity.Conversation != null)
                previousBot = db.Stuffs.Where(x => x.ConversationId == Activity.Conversation.Id && x.FromId == BotId).OrderByDescending(x => x.CreatedDateTime).FirstOrDefault();
            if (activity.From != null && user == null)
                user = db.Users.FirstOrDefault(x => x.UserId == Activity.From.Id);
            Message = activity.Text ?? string.Empty;

            // arhive incoming message
            var c = new Models.ConversationModel()
            {
                ConversationId = activity.Conversation.Id,
                ConversationName = activity.Conversation.Name,
                MessageId = activity.Id,
                MessageLocale = activity.Locale,
                Message = this.Message,
                CreatedDateTime = activity.Timestamp.GetValueOrDefault(),
                FromId = activity.From.Id,
                Type = activity.Type
            };
            db.Stuffs.Add(c);
        }

        protected void initCulture()
        {
            string languageToSet = userData.GetProperty<string>("ClientLanguage") ?? "en";
            this.SwitchToLanguage(languageToSet);
        }
        

        protected bool SwitchToLanguage(string language)
        {
            // should be loadedFrom available resourses?
            //string[] validValues = { "en", "it", "fr", "de", "qps-ploc" };
            var lang = db.Languages.FirstOrDefault(x => x.ISO == language || x.NameInEnglish == language || x.OriginalName == language);
            if (lang != null)                
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                return true;
            }
            return false;
        }

        public virtual async Task ProcessMessage()
        {
            initCulture();



            await db.SaveChangesAsync();
        }



        protected async Task EvaluateStringQ()
        {
            initCulture();
            var stringCount = db.ResourceStrings.Count();
            if (stringCount > 0)
            {
                Random rnd = new Random();
                
                TranslationOriginalModel resourseString = null;
                TranslationOriginalModel targetString = null;
                string sourceLanguage = "en";
                string targetLanguage = (db.UserLanguages.FirstOrDefault(x => x.userId == user.UserId && x.LanguageIso != sourceLanguage) ?? new UserLanguageModel() {LanguageIso = user.NativeLanguage }).LanguageIso;
                
                
                if (!String.IsNullOrEmpty(targetLanguage) && targetLanguage != sourceLanguage)
                {
                    stringCount = db.ResourceStrings.Where(x => x.LanguageIso == targetLanguage).Count();
                    var strNum = rnd.Next(stringCount);
                    int i = 0;
                    while (resourseString == null && i < 5)
                    {
                        resourseString = db.ResourceStrings.Where(x => x.LanguageIso == sourceLanguage).OrderBy(x => x.Id).Skip(strNum).Take(1).FirstOrDefault();
                        if (resourseString != null)
                        {
                            targetString = db.ResourceStrings.FirstOrDefault(x => x.LanguageIso == targetLanguage && x.ResourseId == resourseString.ResourseId);
                            if (targetString == null) resourseString = null;
                        }
                        i++;
                    }
                    if (targetString != null)
                    {

                        Activity r1 = Activity.CreateReply(
                            
                            String.Format(Properties.Resources.EvaluateString_Source, resourseString.StringValue, sourceLanguage) +" | "+resourseString.ResourseId
                            +"  " + Environment.NewLine + targetString.StringValue);

                        r1.Recipient = Activity.From;
                        r1.Type = "message";
                        r1.Attachments = new List<Attachment>();


                        List<CardAction> cardButtons = new List<CardAction>();

                        CardAction plButton = new CardAction()
                        {
                            Value = "10",
                            Type = "postBack",
                            Title = Properties.Resources.EvaluateString_Best_Action
                        };

                        cardButtons.Add(plButton);

                        plButton = new CardAction()
                        {
                            Value = "5",
                            Type = "postBack",
                            Title = Properties.Resources.EvaluateString_Medium_Action
                        };


                        cardButtons.Add(plButton);

                        plButton = new CardAction()
                        {
                            Value = "1",
                            Type = "postBack",
                            Title = Properties.Resources.EvaluateString_Bad_Action
                        };
                        cardButtons.Add(plButton);


                        ThumbnailCard plCard = new ThumbnailCard()
                        {                            
                            Buttons = cardButtons
                        };

                        Attachment plAttachment = plCard.ToAttachment();
                        r1.Attachments.Add(plAttachment);


                        await this.BotResponse(r1, BotScenario.Evaluate, "evalQ|"+targetString.ResourseId+"|"+targetString.LanguageIso);
                    }
                }
                else
                {
                    // No strings in your language space.
                }
            }
            else
            {
                // No strings in our database..
            }
        }

        protected async Task EvaluateNoYesQ(string title, string subtitle, string ScenarioTag)
        {
            initCulture();
            Activity r1 = Activity.CreateReply(title + Environment.NewLine+Environment.NewLine+subtitle);

            r1.Recipient = Activity.From;
            r1.Type = "message";
            r1.Attachments = new List<Attachment>();


            List<CardAction> cardButtons = new List<CardAction>();

            CardAction plButton = new CardAction()
            {
                Value = Properties.Resources.AnswerYes,
                Type = "postBack",
                Title = Properties.Resources.AnswerYes
            };

            cardButtons.Add(plButton);

            plButton = new CardAction()
            {
                Value = Properties.Resources.AnswerNo,
                Type = "postBack",
                Title = Properties.Resources.AnswerNo
            };


            cardButtons.Add(plButton);



            ThumbnailCard plCard = new ThumbnailCard()
            {                
                Buttons = cardButtons
            };

            Attachment plAttachment = plCard.ToAttachment();
            r1.Attachments.Add(plAttachment);


            await this.BotResponse(r1, BotScenario.Evaluate, ScenarioTag);

        }

        protected async Task PickLanguage(string title, string subtitle,BotScenario scenario, string ScenarioTag)
        {
            initCulture();
            Activity r1 = Activity.CreateReply(title +Environment.NewLine+Environment.NewLine+subtitle);

            r1.Recipient = Activity.From;
            r1.Type = "message";
            r1.Attachments = new List<Attachment>();


            List<CardAction> cardButtons = new List<CardAction>();

            CardAction plButton = null;

            foreach (var lang in db.Languages)
            {
                var ci = new CultureInfo(lang.ISO);

                plButton = new CardAction()
                {
                    Value = lang.ISO,
                    Type = "postBack",
                    Title = String.Format("{0} ({1})", lang.NameInEnglish, ci.NativeName)
                };


                cardButtons.Add(plButton);

            }


            HeroCard plCard = new HeroCard()
            {                
                Buttons = cardButtons
            };

            Attachment plAttachment = plCard.ToAttachment();
            r1.Attachments.Add(plAttachment);


            await this.BotResponse(r1, scenario, ScenarioTag);

        }

        protected async Task DisoverGender(string title, string subtitle, string ScenarioTag)
        {
            initCulture();
            Activity r1 = Activity.CreateReply("*"+title+"*"+Environment.NewLine+subtitle);

            r1.Recipient = Activity.From;
            r1.Type = "message";
            r1.Attachments = new List<Attachment>();


            List<CardAction> cardButtons = new List<CardAction>();

            CardAction plButton = new CardAction()
            {
                Value = Properties.Resources.Discover_Male,
                Type = "postBack",
                Title = Properties.Resources.Discover_Male
            };

            cardButtons.Add(plButton);

            plButton = new CardAction()
            {
                Value = Properties.Resources.Discover_Female,
                Type = "postBack",
                Title = Properties.Resources.Discover_Female
            };


            cardButtons.Add(plButton);
            plButton = new CardAction()
            {
                Value = Properties.Resources.Discover_Other,
                Type = "postBack",
                Title = Properties.Resources.Discover_Other
            };


            cardButtons.Add(plButton);


            ThumbnailCard plCard = new ThumbnailCard()
            {                
                Buttons = cardButtons
            };

            Attachment plAttachment = plCard.ToAttachment();
            r1.Attachments.Add(plAttachment);


            await this.BotResponse(r1, BotScenario.Discover, ScenarioTag);

        }

        public int CompareTo(BotScenarioBase other)
        {
            return this.ScenarioName.CompareTo(other.ScenarioName);
        }

        public override bool Equals(object obj)
        {
            if (obj is BotScenarioBase)
            {
                return this.CompareTo(obj as BotScenarioBase) == 0;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.ScenarioName.GetHashCode();
        }
    }
}
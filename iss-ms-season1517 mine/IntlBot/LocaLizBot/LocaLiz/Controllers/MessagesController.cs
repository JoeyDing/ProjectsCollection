using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

namespace LocaLiz
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                Activity r;

                try
                {
                    // For JamesB
                    var stateClient = activity.GetStateClient();
                    var userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);

                    var command = userData.GetProperty<string>("Command");

                    var currentLang = userData.GetProperty<string>("ClientLanguage");
                    if (currentLang != null)
                    {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLang);
                    }

                    if (command == "new_language")
                    {
                        var language = activity.Text; //user entered the ISO of language
                        // here we can add some validation against list of supported languages
                        // you can use some variation of http://stackoverflow.com/questions/553244/programmatic-way-to-get-all-the-available-languages-in-satellite-assemblies

                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

                        userData.SetProperty<string>("ClientLanguage", language);
                        userData.SetProperty<string>("Command", "");
                        await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);

                        // some message to user that now app speaks in other language (in this case general hello in his language)
                        await connector.Conversations.ReplyToActivityAsync(activity.CreateReply(Properties.Resources.WelcomeMessage));
                    }
                    else if (activity.Text.ToLowerInvariant() == "language")
                    {
                        // Message to user something like "Please select your conversation language"
                        Activity r1 = activity.CreateReply(Properties.Resources.Command_In_What_Language);

                        r1.Recipient = activity.From;
                        r1.Type = "message";
                        r1.Attachments = new List<Attachment>();

                        List<CardAction> cardButtons = new List<CardAction>();

                        CardAction plButton = null;

                        // some internal method for getting list of supported langauges
                        var langs = new string[] { "en", "it", "lv", "ar" };
                        foreach (var lang in langs)
                        {
                            var ci = new CultureInfo(lang);

                            plButton = new CardAction()
                            {
                                Value = lang,
                                Type = "postBack",
                                Title = String.Format("{0} ({1})", ci.EnglishName, ci.NativeName)
                            };

                            cardButtons.Add(plButton);
                        }

                        ThumbnailCard plCard = new ThumbnailCard()
                        {
                            Buttons = cardButtons
                        };

                        Attachment plAttachment = plCard.ToAttachment();
                        r1.Attachments.Add(plAttachment);

                        userData.SetProperty<string>("Command", "new_language");
                        await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);

                        await connector.Conversations.ReplyToActivityAsync(r1);
                    }

                    //end For JamesB

                    var botscenario = await Logic.BotScenarioBase.ProcessBotScenario(activity);
                    await botscenario.ProcessMessage();
                }
                catch (Exception dbException)
                {
                    r = activity.CreateReply(dbException.Message);
                    await connector.Conversations.ReplyToActivityAsync(r);
                    r = activity.CreateReply(dbException.StackTrace);
                    await connector.Conversations.ReplyToActivityAsync(r);
                }
            }
            else
            {
                await HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
                ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                if (message.Action.ToLower() == "added")
                {
                    var r0 = message.CreateReply(Properties.Resources.ContactAdded);
                    await connector.Conversations.ReplyToActivityAsync(r0);
                }
                else if (message.Action.ToLower() == "removed")
                {
                    var r0 = message.CreateReply(Properties.Resources.RemovedContact);
                    await connector.Conversations.ReplyToActivityAsync(r0);
                }
                else
                {
                    var r0 = message.CreateReply("Say whaat |" + message.Action);
                    await connector.Conversations.ReplyToActivityAsync(r0);
                }
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        private async Task BotResponse(Models.LocaLizDataContext db, ConnectorClient connector, string reply, Activity prevAction, string scenario, string tag)
        {
            var c = new Models.ConversationModel()
            {
                ConversationId = prevAction.Conversation.Id,
                ConversationName = prevAction.Conversation.Name,
                Message = reply,
                CreatedDateTime = DateTime.Now,
                FromId = Logic.BotScenarioBase.BotId,
                Type = prevAction.Type,
                Scenario = scenario,
                ScenarioTag = tag
            };
            db.Stuffs.Add(c);
            await db.SaveChangesAsync();

            Activity r1 = prevAction.CreateReply(reply);
            await connector.Conversations.ReplyToActivityAsync(r1);
        }
    }
}

/*

 using (var sr = new System.IO.StreamReader(@"C:\Users\aimaci\Source\Repos\International Bot Machine\LocaLizBot\Resourse_Data_WinClassic.txt"))
                    {
                        using (var db = new Models.LocaLizDataContext())
                        {
                            sr.ReadLine();
                            int i = 0;
                            while (!sr.EndOfStream)
                            {
                                var l = sr.ReadLine();
                                if (i > 23024)
                                {
                                    var line = l.Split('\t');
                                    if (line.Length == 3)
                                    {
                                        db.ResourceStrings.Add(new Models.TranslationOriginalModel()
                                        {
                                            LanguageIso = line[0],
                                            ResourseId = line[1],
                                            StringValue = line[2]
                                        });
                                    }
                                    if (i > 23124)
                                    {
                                        i = 23023;
                                        await db.SaveChangesAsync();
                                    }
                                }
                                i++;
                            }
                            await db.SaveChangesAsync();
                        }
                    }

 */
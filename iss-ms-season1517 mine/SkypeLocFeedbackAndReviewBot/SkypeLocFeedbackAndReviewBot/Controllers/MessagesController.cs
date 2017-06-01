using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using SkypeLocFeedbackAndReviewBot.Services;
using SkypeLocFeedbackAndReviewBot.Helpers;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.Bot.Builder.Dialogs;

namespace SkypeLocFeedbackAndReviewBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private string processStatus;
        private const string C_ProjectName = "LOCALIZATION";

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            var stateClient = activity.GetStateClient();
            //userdata perserves all the conversation info
            var userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);
            processStatus = userData.GetProperty<string>("ProcessStatus");

            if (activity.Text != null && activity.Text.ToLower() == @"\reset")
            {
                //string channelID = activity.ChannelId;
                //string userID = activity.From.Id;
                //stateClient.BotState.DeleteStateForUser(channelID, userID);
                userData.SetProperty<string>("Command", "");
            }

            if (processStatus != "start_process" && activity.Type == ActivityTypes.Message)
            {
                Activity r1 = null;
                r1 = activity.CreateReply(String.Format(Properties.Resources.StartProcess, "Skype Localization and FeedBack bot"));
                userData.SetProperty<string>("ProcessStatus", "start_process");
                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                await connector.Conversations.ReplyToActivityAsync(r1);
            }

            if (processStatus == "start_process" && activity.Type == ActivityTypes.Message)
            {
                var command = userData.GetProperty<string>("Command");
                if (command == "MoreFeedbacksRequest")
                {
                    if (activity.Text.ToLowerInvariant() == "no")
                    {
                        //quit the loop,user has to input log feedback firstly to active feedback loop
                        //userData.SetProperty<string>("ProcessStatus", "");
                        userData.SetProperty<string>("Command", "");
                        await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                        command = userData.GetProperty<string>("Command");
                    }
                }

                if (command == "uploadAttachments")
                {
                    List<Attachment> feedbackAttachments = new List<Attachment> { };
                    if (activity.Attachments != null)
                    {
                        Activity r1 = null;
                        foreach (Attachment attachment in activity.Attachments)
                        {
                            feedbackAttachments.Add(attachment);
                        }
                        userData.SetProperty<List<Attachment>>("ProductAttachments", feedbackAttachments);

                        r1 = activity.CreateReply(Properties.Resources.FeedbackReceivedMessage);
                        await connector.Conversations.ReplyToActivityAsync(r1);

                        //get feedback info from userData, these info will be filled the vso item fields
                        string productInfo = userData.GetProperty<string>("ProductInfo");
                        string vsoFeedbackDescription = userData.GetProperty<string>("ProductFeedbackDescription");
                        string bugLanguage = userData.GetProperty<string>("Buglanguage");
                        List<Attachment> attachments = new List<Attachment>();
                        attachments = userData.GetProperty<List<Attachment>>("ProductAttachments");

                        //invoke method to create vso item or even upload attachments
                        string newlyESlink = await CreateVSOItem(productInfo, vsoFeedbackDescription, bugLanguage, attachments);

                        Activity r2 = activity.CreateReply(String.Format(Properties.Resources.FeedbackLoggedSuccessfullyMessage, newlyESlink) + Environment.NewLine + Properties.Resources.ThankYouMessage);
                        r2.Recipient = activity.From;
                        r2.Type = "message";
                        r2.Attachments = new List<Attachment>();
                        Attachment buttonsAttachment = this.AddCardButtonsIntoAttachment(new string[] { "Yes", "No" });
                        r2.Attachments.Add(buttonsAttachment);
                        userData.SetProperty<string>("Command", "MoreFeedbacksRequest");
                        await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                        await connector.Conversations.ReplyToActivityAsync(r2);
                    }
                }

                var latestCommand = userData.GetProperty<string>("Command");

                if (activity.Text != null && activity.Text.ToLowerInvariant() == "bug" || activity.Text.ToLowerInvariant() == "log feedback" || activity.Text.ToLowerInvariant() == "feedback" || latestCommand.ToLowerInvariant() == "Try again(Language)" || latestCommand.ToLowerInvariant() == "morefeedbacksrequest")
                {
                    Activity r1 = activity.CreateReply("Type the language in what is the bug. " + Properties.Resources.Command_In_What_BugLanguage);

                    userData.SetProperty<string>("Command", "KeepLanguageSelection");
                    //without the line below, botstate will lost, so next time command "new_language" will not be null
                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                    await connector.Conversations.ReplyToActivityAsync(r1);
                }
                else if (activity.Text != null && activity.Text.ToLowerInvariant() == "[non language-specific]")
                {
                    var selectedLan = "[Non Language-Specific]";
                    Activity r1 = activity.CreateReply(String.Format(@"You are now filling in {0} bug, then please specify Skype Client that had these issues (Template: {{product family}}\\{{product name}})", selectedLan));

                    //Activity r1 = activity.CreateReply(String.Format("You are now filling in {0} bug,then please specify Skype Client that had these issues", selectedLan));

                    userData.SetProperty<string>("Buglanguage", selectedLan);
                    userData.SetProperty<string>("Command", "SpecificOrGeneric");

                    //without the line below, botstate will lost, so next time command "new_language" will not be null
                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                    await connector.Conversations.ReplyToActivityAsync(r1);
                }
                else if (activity.Text != null && activity.Text.ToLower() == "try again(product info)")
                {
                    Activity r1 = activity.CreateReply(@"Please enter a {Product Family}\\{Product Name}(in English)");
                    r1.Recipient = activity.From;
                    r1.Type = "message";
                    userData.SetProperty<string>("Command", "SpecificOrGeneric");
                    //userData.SetProperty<string>("Command", "");

                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                    await connector.Conversations.ReplyToActivityAsync(r1);
                }
                else if (activity.Text != null && activity.Text.ToLower() == "non product specific")
                {
                    userData.SetProperty<string>("ProductInfo", activity.Text);
                    Activity r1 = activity.CreateReply(String.Format("If you meant {0},Thank you,OK,please enter your feedback if possible in one message(original translation,suggested translation and english text, And be as detailed in comment as possible)", activity.Text));
                    userData.SetProperty<string>("Command", "vsoItemFeedbackDescription");
                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                    await connector.Conversations.ReplyToActivityAsync(r1);
                }
                else if (command == "KeepLanguageSelection")
                {
                    var selectedLan = activity.Text;

                    var langsforPopulation = (new GetLanguagesService()).GetLanguages(selectedLan);

                    //if the there is 1 match, then it means ths language has been confirmed as the target language
                    if (langsforPopulation.Count == 1)
                    {
                        selectedLan = langsforPopulation[0];
                        Activity r1 = activity.CreateReply(String.Format(@"If you meant {0}, then please specify what Skype client had this issue(Template: {{product family}}\\{{product name}})", selectedLan));

                        userData.SetProperty<string>("Buglanguage", selectedLan);

                        userData.SetProperty<string>("Command", "SpecificOrGeneric");

                        //without the line below, botstate will lost, so next time command "new_language" will not be null
                        await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                        await connector.Conversations.ReplyToActivityAsync(r1);
                    }

                    //more than 1 match.let user select the closet mathcing langauge we have in the skype products
                    else if (langsforPopulation.Count > 1)
                    {
                        //more than 1 langauges...
                        Activity r2 = activity.CreateReply("If the expected language is displayed in the list click it please " + Properties.Resources.Command_In_What_BugLanguage);
                        r2.Recipient = activity.From;
                        r2.Type = "message";
                        r2.AttachmentLayout = AttachmentLayoutTypes.List;
                        r2.Attachments = new List<Attachment>();

                        var langsforPopulationTopFour = langsforPopulation.Take(4).ToList();
                        foreach (var lang in langsforPopulationTopFour)
                        {
                            List<CardAction> cardButtons = new List<CardAction>();
                            CardAction plButton = null;
                            plButton = new CardAction()
                            {
                                Value = lang,
                                Type = "postBack",
                                Title = lang,
                            };
                            cardButtons.Add(plButton);
                            ThumbnailCard plCard = new ThumbnailCard()
                            {
                                Buttons = cardButtons
                            };
                            Attachment plAttachment = plCard.ToAttachment();
                            r2.Attachments.Add(plAttachment);
                        }

                        userData.SetProperty<string>("Command", "Buglanguage");
                        await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                        await connector.Conversations.ReplyToActivityAsync(r2);
                    }

                    //unable to identity the langauge user provided
                    else
                    {
                        Activity r3 = activity.CreateReply("I was unable to identity the language you provided, did you mean theat this bug was [Non Language-Specific]? Or you would like to try spelling langauge name again?");
                        r3.Recipient = activity.From;
                        r3.Type = "message";

                        r3.Attachments = new List<Attachment>();
                        Attachment buttonsAttachment = this.AddCardButtonsIntoAttachment(new string[] { "[Non Language-Specific]", "Try again(Language)" });
                        r3.Attachments.Add(buttonsAttachment);
                        await connector.Conversations.ReplyToActivityAsync(r3);
                    }
                }
                else if (command == "Buglanguage")
                {
                    var languageService = new GetLanguagesService();
                    //var langsInitial = (new GetLanguagesService()).GetLanguages();
                    //check if the text is exisiting in the langauge list
                    // NEED TO EXPAND TO HANDLE ALSO non langauge specific
                    var languageSelection = languageService.GetLanguages(activity.Text);
                    if (languageSelection.Count() == 0)
                    {
                        //-----------------------------------------------------
                        Activity r1 = activity.CreateReply("If the expected language is displayed in the list click it please " + Properties.Resources.Command_In_What_BugLanguage);
                        r1.Recipient = activity.From;
                        r1.Type = "message";
                        r1.AttachmentLayout = AttachmentLayoutTypes.List;
                        r1.Attachments = new List<Attachment>();

                        var langsforPopulation = languageSelection;
                        foreach (var lang in langsforPopulation)
                        {
                            List<CardAction> cardButtons = new List<CardAction>();
                            CardAction plButton = null;
                            plButton = new CardAction()
                            {
                                Value = lang,
                                Type = "postBack",
                                Title = lang,
                            };
                            cardButtons.Add(plButton);
                            ThumbnailCard plCard = new ThumbnailCard()
                            {
                                Buttons = cardButtons
                            };
                            Attachment plAttachment = plCard.ToAttachment();
                            r1.Attachments.Add(plAttachment);
                        }

                        if (langsforPopulation.Count() == 0)
                        {
                            Activity r2 = activity.CreateReply("please try another phrase for searching the language");
                            r2.Recipient = activity.From;
                            r2.Type = "message";
                            userData.SetProperty<string>("Command", "KeepLanguageSelection");
                            await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                            await connector.Conversations.ReplyToActivityAsync(r2);
                        }
                        else
                        {
                            userData.SetProperty<string>("Command", "Buglanguage");
                            await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                            await connector.Conversations.ReplyToActivityAsync(r1);
                        }
                    }
                    else
                    {
                        userData.SetProperty<string>("Buglanguage", activity.Text);
                        Activity r1 = activity.CreateReply(@"Please enter a {Product Family}\\{Product Name}");
                        r1.Recipient = activity.From;
                        r1.Type = "message";
                        userData.SetProperty<string>("Command", "SpecificOrGeneric");

                        //without the line below, botstate will lost, so next time command "new_language" will not be null
                        await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                        await connector.Conversations.ReplyToActivityAsync(r1);
                    }
                }
                else if (command == "SpecificOrGeneric")
                {
                    //check if the entered text exists in the list below
                    var productInfo = activity.Text;
                    GetFamiliesProductsListService getFamiliesProductsListService = new GetFamiliesProductsListService();
                    string[] skypeClientProductsArray = getFamiliesProductsListService.GetFamiliesProductsList();

                    if (productInfo != null)
                    {
                        if (productInfo.ToLower() == "non product specific")
                        {
                            userData.SetProperty<string>("Command", "vsoItemFeedbackDescription");
                            Activity r1 = activity.CreateReply("Logging general bug, please enter your feedback,and be as detailed in comment as possible");
                            await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                            await connector.Conversations.ReplyToActivityAsync(r1);
                        }
                        else
                        {
                            var skypeClientList = skypeClientProductsArray.Where(x => x.ToLower().Contains(activity.Text.ToLower())).ToList();
                            if (skypeClientList.Count == 1)
                            {
                                productInfo = skypeClientList[0];
                                //ProductInfo is combination of "Product family" and product name.
                                userData.SetProperty<string>("ProductInfo", productInfo);
                                Activity r1 = activity.CreateReply(String.Format("You mean {0},please enter your feedback if possible in one message (original translation, suggested translation, English text and be as detailed in comment as possible)", productInfo));
                                userData.SetProperty<string>("Command", "vsoItemFeedbackDescription");
                                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                                await connector.Conversations.ReplyToActivityAsync(r1);
                            }
                            else if (skypeClientList.Count > 1)
                            {
                                //command is still specificOrGeneric
                                Activity r1 = activity.CreateReply("Please select a closet matching product info");
                                r1.Recipient = activity.From;
                                r1.Type = "message";
                                r1.AttachmentLayout = AttachmentLayoutTypes.List;
                                r1.Attachments = new List<Attachment>();

                                var productInfoTopFour = skypeClientList.Take(4).ToList();
                                foreach (var proInfo in productInfoTopFour)
                                {
                                    List<CardAction> cardButtons = new List<CardAction>();
                                    CardAction plButton = null;
                                    plButton = new CardAction()
                                    {
                                        Value = proInfo,
                                        Type = "postBack",
                                        Title = proInfo,
                                    };
                                    cardButtons.Add(plButton);
                                    ThumbnailCard plCard = new ThumbnailCard()
                                    {
                                        Buttons = cardButtons
                                    };
                                    Attachment plAttachment = plCard.ToAttachment();
                                    r1.Attachments.Add(plAttachment);
                                }
                                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                                await connector.Conversations.ReplyToActivityAsync(r1);
                            }
                            else
                            {
                                if (activity.Text != null && activity.Text.ToLower() != "try again(product info)")
                                {
                                    //not recognised, you mean "non product specific" or try again?
                                    Activity r1 = activity.CreateReply(@"Oops, it seems that you entered {product family}\\{product name} we do not recognise, did you mean that it was 'Not product specific' or you would like to try spelling product info again?");
                                    r1.Recipient = activity.From;
                                    r1.Type = "message";
                                    r1.Attachments = new List<Attachment>();
                                    Attachment buttonsAttachment = this.AddCardButtonsIntoAttachment(new string[] { "Non product specific", "Try again(Product Info)" });
                                    r1.Attachments.Add(buttonsAttachment);
                                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                                    await connector.Conversations.ReplyToActivityAsync(r1);
                                }
                            }
                        }
                    }
                }
                else if (command == "vsoItemFeedbackDescription")
                {
                    //save ProductFeedbackDescription into userData
                    userData.SetProperty<string>("ProductFeedbackDescription", activity.Text);

                    //create a new reponse to tell user upload attachment
                    Activity r1 = activity.CreateReply("Do you want to upload attachment for better context(screenshot)?");

                    r1.Recipient = activity.From;
                    r1.Type = "message";
                    r1.Attachments = new List<Attachment>();

                    Attachment buttonsAttachment = this.AddCardButtonsIntoAttachment(new string[] { "Yes", "No" });
                    r1.Attachments.Add(buttonsAttachment);
                    userData.SetProperty<string>("Command", "vsoItemFeedbackAttachments");
                    await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                    await connector.Conversations.ReplyToActivityAsync(r1);
                }
                else if (command == "vsoItemFeedbackAttachments")
                {
                    Activity r1 = null;
                    if (activity.Text == "Yes")
                    {
                        r1 = activity.CreateReply("Please upload screenshots ot attachments if you have them.The more the detail,the easier it is to fix!");
                        userData.SetProperty<string>("Command", "uploadAttachments");
                        await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                        await connector.Conversations.ReplyToActivityAsync(r1);
                    }
                    if (activity.Text == "No")
                    {
                        r1 = activity.CreateReply("Thank you, feedback received! ");
                        await connector.Conversations.ReplyToActivityAsync(r1);
                        string productInfo = userData.GetProperty<string>("ProductInfo");
                        string vsoFeedbackDescription = userData.GetProperty<string>("ProductFeedbackDescription");
                        string bugLanguage = userData.GetProperty<string>("Buglanguage");
                        //invoke method to create vso item
                        string newlyESlink = await CreateVSOItem(productInfo, vsoFeedbackDescription, bugLanguage, new List<Attachment>());

                        Activity r2 = activity.CreateReply(String.Format("You can track progress of bug in this VSTS ticket {0}. Would you like to provide more feedback?", newlyESlink));

                        r2.Recipient = activity.From;
                        r2.Type = "message";
                        r2.Attachments = new List<Attachment>();
                        Attachment buttonsAttachment = this.AddCardButtonsIntoAttachment(new string[] { "Yes", "No" });
                        r2.Attachments.Add(buttonsAttachment);
                        userData.SetProperty<string>("Command", "MoreFeedbacksRequest");

                        await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                        await connector.Conversations.ReplyToActivityAsync(r2);
                    }
                }
                else
                {
                    //this warning is ready for the user who wants to create more feedbacks but type wrong command.
                    Activity r1 = activity.CreateReply(Properties.Resources.ProvidefeedbackRequest);
                    await connector.Conversations.ReplyToActivityAsync(r1);
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
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

        private Attachment AddCardButtonsIntoAttachment(string[] options)
        {
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction optionButton = null;
            foreach (string option in options)
            {
                optionButton = new CardAction() { Value = option, Type = "postBack", Title = option };
                cardButtons.Add(optionButton);
            }
            ThumbnailCard plCard = new ThumbnailCard()
            {
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();
            return plAttachment;
        }

        private async Task<string> CreateVSOItem(string productInfo, string description, string language, List<Attachment> attachments)
        {
            //1.create new ES
            VSOContextService vsocontextService = new VSOContextService();
            string eStitle = string.Format("{0} Feedback from the feedback bot", productInfo);
            string botTag = "bot_feedback";
            var newES = await vsocontextService.CreateVsoWorkItem(
                type: TaskTypes.Bug,
                projectName: C_ProjectName,
                title: eStitle,
                areaPath: @"LOCALIZATION",
                iterationPath: @"LOCALIZATION",
                assignedTo: "v-joding@microsoft.com",
                tags: new string[] { "" },
                prepareFunction: (fields) =>
                {
                    var createdDate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.CreatedDate" }, { "value", DateTime.Now } };

                    var budDescription = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Description" }, { "value", description } };

                    var budLanguage = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Language" }, { "value", language } };

                    var f_tags = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Tags" }, { "value", botTag } };
                    fields.Add(f_tags);

                    fields.Add(createdDate);
                    fields.Add(budDescription);
                    fields.Add(budLanguage);
                });

            string newESurl = (string)newES["_links"]["self"]["href"];
            int lastIndex = newESurl.LastIndexOf("/");
            int esId = int.Parse(newESurl.Substring(lastIndex, newESurl.Length - lastIndex).Replace("/", ""));
            string result = await UploadAttchmentsToVso(vsocontextService, esId, attachments);
            return result;
        }

        private async Task<string> UploadAttchmentsToVso(VSOContextService vsocontextService, int esID, List<Attachment> attachments)
        {
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    if (attachment.Content == null && !String.IsNullOrEmpty(attachment.ContentUrl))
                    {
                        using (var connectorClient = new ConnectorClient(new Uri(attachment.ContentUrl)))
                        {
                            var token = await (connectorClient.Credentials as MicrosoftAppCredentials).GetTokenAsync();
                            var uri = new Uri(attachment.ContentUrl);

                            using (var httpClient = new HttpClient())
                            {
                                if (uri.Host.EndsWith("skype.com") && uri.Scheme == Uri.UriSchemeHttps)
                                {
                                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
                                }
                                else
                                {
                                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(attachment.ContentType));
                                }
                                byte[] buffer = await httpClient.GetByteArrayAsync(uri);
                                using (MemoryStream memoryStream = new MemoryStream(buffer))
                                {
                                    await vsocontextService.UploadAttachmentToVsoWorkItems(esID, new Dictionary<string, Stream>() { { attachment.Name, memoryStream } });
                                }
                            }
                        }
                    }
                }
            }

            return string.Format("https://skype-test2.visualstudio.com/DefaultCollection/LOCALIZATION/_workitems/edit/{0}", esID);
        }
    }
}
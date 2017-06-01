function VerifyPresenterName(presenterName)
{
   LogMessage("DataCollabLib::VerifyPresenterName");
   var target = UIATarget.localTarget();
   var mainWindow = target.frontMostApp().mainWindow();
   var presenterString = presenterName + GetValueFromKey("LOCID DataCollab ContentPresenter").replace("%@", "");
   
   target.pushTimeout(60);
   var presenterBarString = mainWindow.scrollViews()[0].staticTexts()[presenterString];
   target.popTimeout();
   
   //WaitForElementToBecomeVisible(presenterBarString, "Presenter Name");
   //IsValidAndVisible(presenterBarString, "Presenter Name");
   
   //TODO : Check for IsValidAndVisible
   if(presenterBarString.isValid())
       LogMessage("DataCollabLib::VerifyPresenterName :: Presenter name found");
   else
       throw new Error("Could not find presenterName ");
}

function VerifyAppShareFilter(sharedContent)
{
   LogMessage("DataCollabLib::VerifyAppShareFilter");
   var target = UIATarget.localTarget();
   var mainWindow = target.frontMostApp().mainWindow();
   
   target.pushTimeout(60);
   var appShareFilterString = mainWindow.scrollViews()[0].staticTexts()[sharedContent];
   target.popTimeout();
   
   //WaitForElementToBecomeVisible(presenterBarString, "Presenter Name");
   //IsValidAndVisible(presenterBarString, "Presenter Name");
   
   //TODO : Check for IsValidAndVisible
   if(appShareFilterString.isValid())
       LogMessage("DataCollabLib::VerifyAppShareFilter :: Shared Content String found");
   else
       throw new Error("Could not find Shared Content String ");
}

function VerifyPPTName(pptName)
{
   LogMessage("DataCollabLib::VerifyPPTName");
   var target = UIATarget.localTarget();
   var mainWindow = target.frontMostApp().mainWindow();
   pptName = pptName + ".pptx";
   LogMessage("DataCollabLib::VerifyPPTName :: Looking for " + pptName);
   
   target.pushTimeout(60);
   var ppt = mainWindow.scrollViews()[0].staticTexts()[pptName];
   target.popTimeout();
   
   //WaitForElementToBecomeVisible(ppt, pptName);
   //IsValidAndVisible(ppt, pptName);
   
   //TODO : Check for IsValidAndVisible
   if(ppt.isValid())
       LogMessage("DataCollabLib::VerifyPPTName :: PPT name found");
   else
       throw new Error("Could not find pptName");
}

function VerifyPPTView(presenterName,pptName)
{
   LogMessage("DataCollabLib::VerifyPPTView");
   VerifyPPTName(pptName);
   VerifyPresenterName(presenterName);
}

function JoinConference(subject)
{
   LogMessage("DataCollabLib::JoinConference");
   GotoMeetings();
   //OpenMeeting(meetingIndex);
   OpenMeetingWithSubject(subject);
   TapOnJoinButton();
}

function WaitForCollabPane()
{
   LogMessage("DataCollabLib :: WaitForCollabPane");
   var numOfiterationsOfFiveSec = 12;
   var mainWindow = UIATarget.localTarget().frontMostApp().mainWindow();
   var result = false;
   for(i=1;i<=numOfiterationsOfFiveSec;i++)
   {
      LogMessage("DataCollabLib :: WaitForCollabPane :: Iteration " + i);
      try
      {
          //GotoModalityView("icon conversation button shari");
          var pageIndicatorValue = mainWindow.pageIndicators()[0].value()
          LogMessage("DataCollabLib :: WaitForCollabPane :: " + pageIndicatorValue);
          
          var count = 0;
          for(j=0; j< pageIndicatorValue.length; j++)
          {
          	if(pageIndicatorValue[j] == "3")
          	{
          		count++;
          	}
          }
          if(count == 2)
          {
            result = true;
            break;
          }
      }
      catch(error)
      {}
      DelayInSec(5);
   }
   
   if(result == false)
     throw new Error ("Collab Pane Not Found");    
}

//function GetMeetingURL(subject)
//{
//   LogMessage("DataCollabLib :: GetMeetingURL");
//   
//   GotoMeetings();
//   OpenMeetingWithSubject(subject);
//   
//   var target = UIATarget.localTarget();
//   var mainWindow = target.frontMostApp().mainWindow();	
//
//   target.pushTimeout(10);
//   var meetingInfo = mainWindow.tableViews()[mainWindow.tableViews().length - 1].groups()[GetValueFromKey("LOCID Invitees")].textViews()[0].value();
//   target.popTimeout();
//   
//   var url = ExtractURL(meetingInfo);
//   LogMessage("DataCollabLib :: GetMeetingURL :: meetingURL = " + url);
//   return url;
//}
// 
//function ExtractURL(meetingInfo)
//{
//   var joinOnlineMeeting = "Join online meeting";
//   var joinLyncMeeting = "Join Lync Meeting";
//   var startIndex;
//   
//   //LogMessage("DataCollabLib :: "+ meetingInfo);
//   if(meetingInfo.indexOf(joinOnlineMeeting) >= 0)
//   {
//       startIndex = meetingInfo.indexOf(joinOnlineMeeting) + joinOnlineMeeting.length;
//   }
//   else if (meetingInfo.indexOf(joinLyncMeeting) >= 0)
//   {
//       startIndex = meetingInfo.indexOf(joinLyncMeeting) + joinLyncMeeting.length;
//   }
//   else
//   {
//       throw new Error ("Could not find Meeting URL");
//   }
//   
//   startIndex = meetingInfo.indexOf('<',startIndex);
//   var endIndex = meetingInfo.indexOf('>',startIndex);
//   return meetingInfo.substr(startIndex +1 ,(endIndex - startIndex - 1));
//}

function GetPPTName()
{
    DelayInSec(1);
    var returnString = "TestPPT" + (new Date()).getTime();
    LogMessage("DataCollabLib :: GetPPTName :: PPTName is " + returnString);
    return returnString;
}

function SendStartPPTMessage(selfDisplayName)
{
   GotoModalityView(IM_MODALITY_BUTTON);
   WaitforConversationWindow();
   var messageToSend = selfDisplayName + " - " + startPPTShareMessage;
   SendIM(messageToSend);
   messageToSend = messageToSend.split(" ").join("  ");
   VerifyReceivedIM(selfDisplayName,messageToSend);
}

function SendStopPPTMessage(selfDisplayName)
{
   GotoModalityView(IM_MODALITY_BUTTON);
   WaitforConversationWindow();
   var messageToSend = selfDisplayName + " - " + stopPPTShareMessage;
   SendIM(messageToSend);
   messageToSend = messageToSend.split(" ").join("  ");
   VerifyReceivedIM(selfDisplayName,messageToSend);
}

function SendStopAppShareMessage(selfDisplayName)
{
   GotoModalityView(IM_MODALITY_BUTTON);
   WaitforConversationWindow();
   var messageToSend = selfDisplayName + " - " + stopAppShareMessage;
   SendIM(messageToSend);
   messageToSend = messageToSend.split(" ").join("  ");
   VerifyReceivedIM(selfDisplayName,messageToSend);
}

function SendStartAppShareMessage(selfDisplayName)
{
   GotoModalityView(IM_MODALITY_BUTTON);
   WaitforConversationWindow();
   var messageToSend = selfDisplayName + " - " + startAppShareMessage;
   SendIM(messageToSend);
   messageToSend = messageToSend.split(" ").join("  ");
   VerifyReceivedIM(selfDisplayName,messageToSend);
}

function WaitForContentToBeLoaded()
{
   LogMessage("DataCollabLib::WaitForAppShareContentToBeLoaded");
   var target = UIATarget.localTarget();
   var mainWindow = target.frontMostApp().mainWindow();	
   DelayInSec(30);
   CaptureRectScreenShot(mainWindow.scrollViews()[0].rect(),"AppShareContent");
}
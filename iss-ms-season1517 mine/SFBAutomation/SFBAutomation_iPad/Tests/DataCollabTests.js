function VerifyScheduledMeetingPPTView_JoiningBeforePPTShare()
{  
   LogMessage("DataCollabTest :: VerifyScheduledMeetingPPTView_JoiningBeforePPTShare");
   var selfDisplayName = GetTestCaseParameters("DisplayName1");
   var presenterName = GetBuddyDisplayName(0);
   var groupName = GetTestCaseParameters("GroupName");
   var pptName = GetPPTName();
   var meetingSubject = GetTestCaseParameters("OnlineMeetingSubject");
   var meetingURL = GetMeetingURL(meetingSubject);
   
   GotoMyInfo();
   //Set space separated params
   SetNote("VerifyScheduledMeetingPPTView_JoiningBeforePPTShare "+ meetingURL + " " + pptName);

   GotoContacts();
   OpenContactCardFromContactList(groupName,presenterName);
   DismissNotificationsIfAny();
   WaitForBuddyNoteChange("JoinedScheduledMeeting");	
   
   //Join Scheduled Meeting
   JoinConference(meetingSubject);
   
   WaitForCallToGetConnected();
   
   //Ask bot to share PPT
   GotoModalityView(IM_MODALITY_BUTTON);
   WaitforConversationWindow();
   
   var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
   VerifyReceivedIM("", selfConversationJoinMessage);
   var messageToRecieve = presenterName + " - " + audioConnectedInConferenceMessage;
   messageToRecieve = messageToRecieve.split(" ").join("  ");
   VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
   
   SendStartPPTMessage(selfDisplayName);

   WaitForCollabPane();
   VerifyPPTView(presenterName,pptName);
   WaitForContentToBeLoaded();
   
   GotoModalityView(IM_MODALITY_BUTTON);
   WaitforConversationWindow(); 
   messageToRecieve = presenterName + " - " + dataConnectedInConferenceMessage;
   messageToRecieve = messageToRecieve.split(" ").join("  ");
   VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
   
   //Ask bot to stop PPT
   SendStopPPTMessage(selfDisplayName);
   
   EndConversation();
}

function VerifyScheduledMeetingPPTView_JoiningAfterPPTShare()
{
   LogMessage("DataCollabTest :: VerifyScheduledMeetingPPTView_JoiningAfterPPTShare");
   var selfDisplayName = GetTestCaseParameters("DisplayName1");
   var presenterName = GetBuddyDisplayName(0);
   var groupName = GetTestCaseParameters("GroupName");
   var pptName = GetPPTName();
   var meetingSubject = GetTestCaseParameters("OnlineMeetingSubject");
   var meetingURL = GetMeetingURL(meetingSubject);
   
   //Share PPT from Bot
   GotoMyInfo();
   //Set space separated params
   SetNote("VerifyScheduledMeetingPPTView_JoiningAfterPPTShare "+ meetingURL + " " + pptName);
   LogMessage("Waiting for PPT to come up on bot end");
   
   GotoContacts();
   OpenContactCardFromContactList(groupName,presenterName);
   DismissNotificationsIfAny();
   WaitForBuddyNoteChange("PptUploadedInScheduledMeeting");	
   
   //Join Scheduled Meeting
   JoinConference(meetingSubject);
   
   //Confirm that conference join is successful
   WaitForCallToGetConnected();

   WaitForCollabPane();
   VerifyPPTView(presenterName,pptName);
   WaitForContentToBeLoaded();
   
   GotoModalityView(IM_MODALITY_BUTTON);
   WaitforConversationWindow(); 
   
   var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
   VerifyReceivedIM("", selfConversationJoinMessage);
   var messageToRecieve = presenterName + " - " + audioConnectedInConferenceMessage;
   messageToRecieve = messageToRecieve.split(" ").join("  ");
   VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
   messageToRecieve = presenterName + " - " + dataConnectedInConferenceMessage;
   messageToRecieve = messageToRecieve.split(" ").join("  ");
   VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
   
   //Ask bot to stop PPT
   SendStopPPTMessage(selfDisplayName);
   
   EndConversation();
}



/*This DFT verifies an update in Presenter Name 
 *as direct presenter name verification is covered in BVTS
 */
function VerifyPPTPresenterName()
{
   LogMessage("DataCollabTest :: VerifyPPTPresenterName");
   var selfDisplayName = GetTestCaseParameters("DisplayName1");
   var presenterName1 = GetBuddyDisplayName(0);
   var presenterName2 = GetBuddyDisplayName(1);
   
   //Share PPT from Bot
   GotoMyInfo();
   SetNote("VerifyPPTPresenterName " + GetPPTName() + " " + GetPPTName());
   
   AceptRejectCallToast(AcceptString);	
   
   //Confirm that conference join is successful
   WaitForCallToGetConnected();

   //TODO : Accept Sharing Request When feature is Implemented
   WaitForCollabPane();
   VerifyPresenterName(presenterName1);
   WaitForContentToBeLoaded();
   
    //Ask bot to stop PPT
   SendStopPPTMessage(selfDisplayName);
   
   //TODO : Remove try catch when bug #3176304 gets Fixed
   try
   {
      var messageToRecieve = presenterName1 + " - " + "PPT Sharing Stopped";
      messageToRecieve = messageToRecieve.split(" ").join("  ");
      VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
   }
   catch(error)
   {}
   DelayInSec(10);
   WaitForCollabPane();
   VerifyPresenterName(presenterName2);
   
   //Ask bot to stop PPT
   SendStopPPTMessage(selfDisplayName);
   
   EndConversation();
}

/*This DFT verifies an update in PPT Name 
 *as direct PPT name verification is covered in BVTS
 */
function VerifySharedPPTName()
{
   LogMessage("DataCollabTest :: VerifySharedPPTName");
   var selfDisplayName = GetTestCaseParameters("DisplayName1");
   var presenterName = GetBuddyDisplayName(0);
   var pptName1 = GetPPTName();
   var pptName2 = GetPPTName();
   
   //Share PPT from Bot
   GotoMyInfo();
   SetNote("VerifySharedPPTName " + pptName1 + " " + pptName2);
   
   AceptRejectCallToast(AcceptString);	
   
   //Confirm that conference join is successful
   WaitForCallToGetConnected();

   WaitForCollabPane();
   VerifyPPTName(pptName1);
   WaitForContentToBeLoaded();
   
   //Ask bot to stop PPT1
   SendStopPPTMessage(selfDisplayName);
   
   try
   {
      var messageToRecieve = presenterName + " - " + "PPT Sharing Stopped";
      messageToRecieve = messageToRecieve.split(" ").join("  ");
      VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
   }
   catch(error)
   {}
   DelayInSec(5);
  
   //Ask bot to share another PPT
   SendStartPPTMessage(selfDisplayName);
   
   WaitForCollabPane();
   VerifyPPTName(pptName2);
   
   //Ask bot to stop PPT2
   SendStopPPTMessage(selfDisplayName);
   
   EndConversation();
   
}

function VerifyNonFullScreenViewPPTinBothOrientations()
{
   LogMessage("DataCollabTest :: VerifyNonFullScreenViewPPTinBothOrientations");
   var selfDisplayName = GetTestCaseParameters("DisplayName1");
   var presenterName = GetBuddyDisplayName(0);
   var pptName = GetPPTName();

   //Share PPT from Bot
   GotoMyInfo();
   SetNote("VerifyNonFullScreenViewPPTinBothOrientations " + pptName);
   
   AceptRejectCallToast(AcceptString);	
   
   //Confirm that conference join is successful
   WaitForCallToGetConnected();
   WaitForCollabPane();
   WaitForContentToBeLoaded();
   
   GoToOrientation(UIA_DEVICE_ORIENTATION_PORTRAIT);
   VerifyPPTView(presenterName,pptName);
   
   GoToOrientation(UIA_DEVICE_ORIENTATION_LANDSCAPELEFT);
   VerifyPPTView(presenterName,pptName);
   
   GoToOrientation(UIA_DEVICE_ORIENTATION_PORTRAIT);
   SendStopPPTMessage(selfDisplayName);
   EndConversation();
}

function VerifyImmersiveViewPPTinBothOrientations()
{
   LogMessage("DataCollabTest :: VerifyImmersiveViewPPTinBothOrientations");
   var selfDisplayName = GetTestCaseParameters("DisplayName1");
   var presenterName = GetBuddyDisplayName(0);
   var pptName = GetPPTName();

   //Share PPT from Bot
   GotoMyInfo();
   SetNote("VerifyImmersiveViewPPTinBothOrientations " + pptName);
   
   AceptRejectCallToast(AcceptString);	
   
   //Confirm that conference join is successful
   WaitForCallToGetConnected();
   WaitForCollabPane();
   
   GoFullScreen();
   GoToOrientation(UIA_DEVICE_ORIENTATION_PORTRAIT);
   VerifyPPTView(presenterName,pptName);
   VerifyImmersiveView();
   
   GoToOrientation(UIA_DEVICE_ORIENTATION_LANDSCAPELEFT);
   VerifyPPTView(presenterName,pptName);
   VerifyImmersiveView();
   
   GoToOrientation(UIA_DEVICE_ORIENTATION_PORTRAIT);
   GoBackToNonFullScreen();
   SendStopPPTMessage(selfDisplayName);
   EndConversation();
}
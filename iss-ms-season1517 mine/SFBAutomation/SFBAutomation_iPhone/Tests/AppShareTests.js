function VerifyAppShareInScheduledMeeting_JoinBeforeAppShareTest()
{  
   LogMessage("DataCollabTest :: VerifyAppShareInScheduledMeeting_JoinBeforeAppShareTest");
   var selfDisplayName = GetTestCaseParameters("DisplayName1");
   var presenterName = GetBuddyDisplayName(0);
   var groupName = GetTestCaseParameters("GroupName");
   var meetingSubject = GetTestCaseParameters("OnlineMeetingSubject");
   var meetingURL = GetMeetingURL(meetingSubject);
   
   GotoMyInfo();
   //Set space separated params
   SetNote("VerifyAppShareInScheduledMeeting_JoinBeforeAppShare "+ meetingURL);

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
   
   SendStartAppShareMessage(selfDisplayName);

   WaitForCollabPane();
   VerifyPresenterName(presenterName);
   VerifyAppShareFilter("Desktop");
   WaitForContentToBeLoaded();
   
   GotoModalityView(IM_MODALITY_BUTTON);
   WaitforConversationWindow(); 
   messageToRecieve = presenterName + " - " + appshareConnectedInConferenceMessage;
   messageToRecieve = messageToRecieve.split(" ").join("  ");
   VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
   
   //Ask bot to stop AppShare
   SendStopAppShareMessage(selfDisplayName);
   
   EndConversation();
}

function VerifyAppShareInScheduledMeeting_JoinAfterAppShareTest()
{
   LogMessage("AppShareTests :: VerifyAppShareInScheduledMeeting_JoinAfterAppShareTest");
   var selfDisplayName = GetTestCaseParameters("DisplayName1");
   var presenterName = GetBuddyDisplayName(0);
   var groupName = GetTestCaseParameters("GroupName");
   var meetingSubject = GetTestCaseParameters("OnlineMeetingSubject");
   var meetingURL = GetMeetingURL(meetingSubject);
   
   //Share PPT from Bot
   GotoMyInfo();
   //Set space separated params
   SetNote("VerifyAppShareInScheduledMeeting_JoinAfterAppShare "+ meetingURL);
   
   GotoContacts();
   OpenContactCardFromContactList(groupName,presenterName);
   DismissNotificationsIfAny();
   WaitForBuddyNoteChange("SharedDesktopInScheduledMeeting");	
   
   //Join Scheduled Meeting
   JoinConference(meetingSubject);
   
   //Confirm that conference join is successful
   WaitForCallToGetConnected();

   WaitForCollabPane();
   VerifyPresenterName(presenterName);
   VerifyAppShareFilter("Desktop");
   WaitForContentToBeLoaded();
   
   GotoModalityView(IM_MODALITY_BUTTON);
   WaitforConversationWindow(); 
   
   var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
   VerifyReceivedIM("", selfConversationJoinMessage);
   var messageToRecieve = presenterName + " - " + audioConnectedInConferenceMessage;
   messageToRecieve = messageToRecieve.split(" ").join("  ");
   VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
   messageToRecieve = presenterName + " - " + appshareConnectedInConferenceMessage;
   messageToRecieve = messageToRecieve.split(" ").join("  ");
   VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
   
   //Ask bot to stop AppShare
   SendStopAppShareMessage(selfDisplayName);
   
   EndConversation();
}

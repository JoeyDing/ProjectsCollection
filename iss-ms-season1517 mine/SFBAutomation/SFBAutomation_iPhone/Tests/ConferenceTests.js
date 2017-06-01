function VerifyIncomingAudioConferenceTest()
{
	LogMessage("ConferenceTests :: VerifyIncomingAudioConferenceTest");

	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	var buddy1 = GetBuddyDisplayName(0);
	
	var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
	var buddy1ConversationJoinMessage = buddy1 + conversationJoinedMessage;
	
	SetNote("VerifyIncomingAudioConference");
	
	AceptRejectCallToast(AcceptString);	
	LogMessage("ConferenceTests :: VerifyIncomingAudioConferenceTest :: Waiting for audio window");
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	LogMessage("ConferenceTests :: VerifyIncomingAudioConferenceTest :: Switch to IM view");
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	// Verify participants joined the conference
	VerifyReceivedIM("", selfConversationJoinMessage);
	VerifyReceivedIM("", buddy1ConversationJoinMessage);
	
	var messageToSend = selfDisplayName + " - " + verifyAudioConnectedInConferenceMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	EndConversation();
}


function VerifyOutgoingAudioConferenceTest()
{
	LogMessage("ConferenceTests :: VerifyOutgoingAudioConferenceTest");
	
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	var buddy1 = GetBuddyDisplayName(0);
	var buddy2 = GetBuddyDisplayName(1);
	
	var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
	var buddy1ConversationJoinMessage = buddy1 + conversationJoinedMessage;
	var buddy2ConversationJoinMessage = buddy2 + conversationJoinedMessage;
	
	SetNote("VerifyOutgoingAudioConference");
	
	GotoContacts();
	OpenGroupContactCard(groupname);
	TapGroupCallbutton();
	
	LogMessage("ConferenceTests :: VerifyOutgoingAudioConferenceTest :: Waiting for audio window");
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	LogMessage("ConferenceTests :: VerifyOutgoingAudioConferenceTest :: Switch to IM view");
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	// Verify participants joined the conference
	VerifyReceivedIM("", selfConversationJoinMessage);
	VerifyReceivedIM("", buddy1ConversationJoinMessage);
	VerifyReceivedIM("", buddy2ConversationJoinMessage);
	
	var messageToSend = selfDisplayName + " - " + verifyAudioConnectedInConferenceMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	messageToRecieve = buddy2 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(1), messageToRecieve);
	
	EndConversation();
}

function VerifyMuteInConferenceCallTest()
{
	LogMessage("ConferenceTests :: VerifyMuteInConferenceTest");
	
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	var buddy1 = GetBuddyDisplayName(0);
	
	var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
	var buddy1ConversationJoinMessage = buddy1 + conversationJoinedMessage;
	
	SetNote("VerifyMuteInConferenceCall");
	
	AceptRejectCallToast(AcceptString);	
	LogMessage("ConferenceTests :: VerifyMuteInConferenceCallTest :: Waiting for audio window");
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	LogMessage("ConferenceTests :: VerifyMuteInConferenceCallTest :: Switch to IM view");
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	// Verify participants joined the conference
	VerifyReceivedIM("", selfConversationJoinMessage);
	VerifyReceivedIM("", buddy1ConversationJoinMessage);
	
	var messageToSend = selfDisplayName + " - " + verifyAudioConnectedInConferenceMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	TapOnMuteButton();
	
	messageToSend = selfDisplayName + " - " + verifyMutedInConferenceMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	messageToRecieve = buddy1 + " - " + mutedMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	EndConversation();
}

function VerifyAddAudioToImConferenceTest()
{
	LogMessage("ConferenceTests :: VerifyAddAudioToImConferenceTest");
	
	var buddy1 = GetBuddyDisplayName(0);
	var buddy2 = GetBuddyDisplayName(1);
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
	var buddy1ConversationJoinMessage = buddy1 + conversationJoinedMessage;
	var buddy2ConversationJoinMessage = buddy2 + conversationJoinedMessage;
	
	LogMessage(selfDisplayName + " " + buddy1 + " " + buddy2);
	
	SetNote("VerifyAddAudioToImConference");

	var inviteText = buddy1;
	TapOnIMInvite(inviteText);
	WaitforConversationWindow();
	
	// Verify participants joined the conference
	VerifyReceivedIM("", selfConversationJoinMessage);
	VerifyReceivedIM("", buddy1ConversationJoinMessage);
	VerifyReceivedIM("", buddy2ConversationJoinMessage);
	
	var messageToRecieve = buddy1 + " - " + testMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	var messageToSend = selfDisplayName + " - " + replyMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	messageToRecieve = buddy1 + " - " + replyMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	messageToRecieve = buddy2 + " - " + replyMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(1), messageToRecieve);
	
	HideKeyboard();
	AddAudioModalityToImConf();
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	messageToRecieve = buddy2 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(1), messageToRecieve);
	
	EndConversation();

}

function VerifyP2PCallEscalationToConferenceCallTest()
{
	LogMessage("ConferenceTests :: VerifyP2PCallEscalationToConferenceCallTest");
	
	var groupname = GetTestCaseParameters("GroupName");
	var buddy1 = GetBuddyDisplayName(0);
	var buddy2 = GetBuddyDisplayName(1);
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
	var buddy1ConversationJoinMessage = buddy1 + conversationJoinedMessage;
	var buddy2ConversationJoinMessage = buddy2 + conversationJoinedMessage;
	
	var messageToSend = "";
	var messageToRecieve = "";
	
	SetNote("VerifyP2PCallEscalationToConferenceCall");
	
	LogMessage("ConferenceTests :: VerifyP2PCallEscalationToConferenceCallTest :: P2P Call to "+ buddy1 );
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddy1);
	TapCallbutton();
	
	LogMessage("ConferenceTests :: VerifyP2PCallEscalationToConferenceCallTest :: Wait for Audio call to get connected");
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	LogMessage("ConferenceTests :: VerifyP2PCallEscalationToConferenceCallTest :: Switch to IM view");
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	messageToRecieve = buddy1 + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	LogMessage("ConferenceTests :: VerifyP2PCallEscalationToConferenceCallTest :: Switch to Audio view");
	GotoModalityView(VOICE_MODALITY_BUTTON);
	LogMessage("ConferenceTests :: VerifyP2PCallEscalationToConferenceCallTest :: Invite Participant " + buddy2);
	InviteParticipant(buddy2);
	LogMessage("ConferenceTests :: VerifyP2PCallEscalationToConferenceCallTest :: Wait for Third participant to accept");
	
	DelayInSec(5);
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	// Verify participants joined the conference
	VerifyReceivedIM("", buddy2ConversationJoinMessage);
	
	messageToSend = selfDisplayName + " - " + verifyAudioConnectedInConferenceMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	messageToRecieve = buddy2 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(1), messageToRecieve);
	
	EndConversation();
}


function VerifyMeetingsForDayTest()
{
	LogMessage("ConferenceTests :: VerifyMeetingsForDayTest");
	
	var onlineMeetingSubject = GetTestCaseParameters("OnlineMeetingSubject");
	var offlineMeetingSubject = GetTestCaseParameters("OfflineMeetingSubject");
	
	GotoMeetings();
	LogMessage("ConferenceTests :: VerifyMeetingsForDayTest :: Verify Online meeting");
	OpenMeetingWithSubject(onlineMeetingSubject);
	VerifyMeetingSubject(onlineMeetingSubject);
	VerifyJoinButton();
	
	LogMessage("ConferenceTests :: VerifyMeetingsForDayTest :: Verify Offline meeting");
	OpenMeetingWithSubject(offlineMeetingSubject);
	VerifyMeetingSubject(offlineMeetingSubject);
}

function VerifyJoinOnlineMeetingFromCalendarTest()
{
	LogMessage("ConferenceTests :: VerifyJoinOnlineMeetingFromCalendarTest");
	
	var groupName = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	var buddy1 = GetBuddyDisplayName(0);
	
	var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
	var buddy1ConversationJoinMessage = buddy1 + conversationJoinedMessage;
	var onlineMeetingSubject = GetTestCaseParameters("OnlineMeetingSubject");
    var meetingURL = GetMeetingURL(onlineMeetingSubject);

	
	GotoMyInfo();
	SetNote("VerifyJoinOnlineMeetingFromCalendar " + meetingURL);
	
	GotoContacts();
	OpenContactCardFromContactList(groupName,buddy1);
	WaitForBuddyNoteChange("JoinedScheduledMeeting");
	
	GotoMeetings();
	
	OpenMeetingWithSubject(onlineMeetingSubject);
	TapOnJoinButton();
	WaitforAudioWindow();
	
	LogMessage("ConferenceTests :: VerifyJoinOnlineMeetingFromCalendarTest :: Waiting for audio call to get connected");
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	// Verify participants joined the conference
	VerifyReceivedIM("", selfConversationJoinMessage);
	
	DelayInSec(5);
	WaitforConversationWindow();
	
	messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddy1, messageToRecieve);
	
	EndConversation();
	
	GotoContacts();
	OpenContactCardFromContactList(groupName,buddy1);
	WaitForBuddyNoteChange("RemoteUserExitedMeeting");

}

function VerifyConferenceRejectTest()
{
	LogMessage("ConferenceTests :: VerifyConferenceRejectTest");

	var buddyContact = GetBuddyDisplayName(0);
	var groupName = GetTestCaseParameters("GroupName");

	GotoMyInfo();
	
	SetNote("VerifyConferenceReject");
	
	LogMessage("ConferenceTests :: VerifyConferenceRejectTest :: Waiting for the audio call toast");
	AceptRejectCallToast(IgnoreString);
	
	GotoContacts();
	OpenContactCardFromContactList(groupName,buddyContact);
	DismissNotificationsIfAny();
	WaitForBuddyNoteChange("CallRejectedRemotely");	
	
}

function VerifyConferenceRejoinTest()
{
	LogMessage("ConferenceTests :: VerifyConferenceRejoinTest");

	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	var buddy1 = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	
	var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
	var buddy1ConversationJoinMessage = buddy1 + conversationJoinedMessage;
	
	SetNote("VerifyIncomingAudioConferenceRejoin");
	
	LogMessage("ConferenceTests :: VerifyConferenceRejoinTest :: Waiting for the audio call toast");
	AceptRejectCallToast(AcceptString);
	
	LogMessage("ConferenceTests :: VerifyConferenceRejoinTest :: Waiting for audio window");
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	LogMessage("ConferenceTests :: VerifyConferenceRejoinTest :: Switch to IM view");
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	// Verify participants joined the conference
	VerifyReceivedIM("", selfConversationJoinMessage);
	VerifyReceivedIM("", buddy1ConversationJoinMessage);
	
	var messageToSend = selfDisplayName + " - " + verifyAudioConnectedInConferenceMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	EndConversation();
	
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddy1);
	WaitForBuddyNoteChange("ParicipantRemoved");
	
	GotoChats();
	var index = 0;
	LogMessage("ConferenceTests :: VerifyConferenceRejoinTest :: Tap on Conversation " + index+1 + " in list");
	TapOnConversationInList(index);
	
	TapOnOptionsMenuItem(REJOIN);
	LogMessage("ConferenceTests :: VerifyConferenceRejoinTest :: Waiting for audio window");
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	LogMessage("ConferenceTests :: VerifyConferenceRejoinTest :: Switch to IM view");
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToSend = selfDisplayName + " - " + verifyAudioConnectedInConferenceMessage + " 2";
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage + " 2";
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	EndConversation();
		
}


function VerifyMeetingsDetailsTest()
{
	LogMessage("ConferenceTests :: VerifyMeetingsDetailsTest");
	
	var meetingSubject = GetTestCaseParameters("OnlineMeetingSubject");
	var meetingTime = GetTestCaseParameters("OnlineMeetingTime");
	var meetingLocation = GetTestCaseParameters("OnlineMeetingLocation");
	var meetingOrganizer = GetTestCaseParameters("OnlineMeetingOrganizer");
	var meetingInvitees = GetTestCaseParameters("OnlineMeetingInvitees");
	var meetingNote = GetTestCaseParameters("OnlineMeetingNote");
	var meetingUrl = GetTestCaseParameters("OnlineMeetingUrl");
	
	GotoMeetings();
	OpenMeetingWithSubject(meetingSubject);
	VerifyMeetingDetails(meetingSubject,meetingTime,meetingLocation,meetingOrganizer,meetingInvitees,meetingNote,meetingUrl);	
}


function VerifyIncomingConferenceRejectTest()
{
	LogMessage("ConferenceTests :: VerifyConferenceRejectTest");
	var buddyContact = GetBuddyDisplayName(0);
	var groupName = GetTestCaseParameters("GroupName");
	
	GotoMyInfo();
	SetNote("VerifyIncomingConferenceReject");
	
	LogMessage("ConferenceTests :: VerifyConferenceRejectTest :: Waiting for Conf Invite");
	AceptRejectCallToast(IgnoreString);	
	
	GotoContacts();
	OpenContactCardFromContactList(groupName,buddyContact);
	WaitForBuddyNoteChange("ConferenceRejectedRemotely");
}


function VerifyPassiveAddAudioToIMConferenceTest()
{
	LogMessage("ConferenceTests :: VerifyPassiveAddAudioToIMConferenceTest");
	
	var groupname = GetTestCaseParameters("GroupName");
	var buddy1 = GetBuddyDisplayName(0);
	var buddy2 = GetBuddyDisplayName(1);
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	GotoMyInfo();
	SetNote("VerifyPassiveAddAudioToIMConference");
	
	LogMessage("ConferenceTests :: VerifyPassiveAddAudioToIMConferenceTest :: IM conversation to "+groupname );
	GotoContacts();
	OpenGroupContactCard(groupname);
	TapGroupIMbutton();
	WaitforConversationWindow();

	var messageToSend1 = GetTestCaseParameters("DisplayName1") + " - " + firstMessage;
	var messageToSend2 = GetTestCaseParameters("DisplayName1") + " - " + secondMessage;
	var messageToRecieve = "";
	
	var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
	var buddy1ConversationJoinMessage = buddy1 + conversationJoinedMessage;
	var buddy2ConversationJoinMessage = buddy2 + conversationJoinedMessage;
	
	SendIM(messageToSend1);
	messageToSend1 = messageToSend1.split(" ").join("  ");
	VerifyReceivedIM(GetTestCaseParameters("DisplayName1"), messageToSend1);
	
	// Verify participants joined the conference
	VerifyReceivedIM("", selfConversationJoinMessage);
	VerifyReceivedIM("", buddy1ConversationJoinMessage);
	VerifyReceivedIM("", buddy2ConversationJoinMessage);
	
	SendIM(messageToSend2);
	messageToSend2 = messageToSend2.split(" ").join("  ");
	VerifyReceivedIM(GetTestCaseParameters("DisplayName1"), messageToSend2);
	
	messageToRecieve = buddy1 + " - " + replySecondMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	messageToRecieve = buddy2 + " - " + replySecondMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(1), messageToRecieve);
	
	LogMessage("ConferenceTests :: VerifyPassiveAddAudioToIMConferenceTest :: Waiting for Conf Invite");
	AceptRejectCallToast(AcceptString);
	LogMessage("ConferenceTests :: VerifyPassiveAddAudioToIMConferenceTest :: Waiting for audio window");
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	LogMessage("ConferenceTests :: VerifyPassiveAddAudioToIMConferenceTest :: Switch to IM view");
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	// Verify participants joined the conference
	messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	messageToRecieve = buddy2 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(1), messageToRecieve);

	EndConversation();
}


function VerifyPassiveP2PCallEscalationToConferenceCallTest()
{
	LogMessage("ConferenceTests :: VerifyPassiveP2PCallEscalationToConferenceCallTest");
	
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	var buddyList = GetTestCaseParameters("BuddyListDisplayName");
	var buddy1 = GetBuddyDisplayName(0);
	var buddy2 = GetBuddyDisplayName(1);
	var bubbleCount = 1;
	
	GotoMyInfo();
	SetNote("VerifyPassiveP2PCallEscalationToConferenceCall");
	
	LogMessage("ConferenceTests :: VerifyPassiveP2PCallEscalationToConferenceCallTest :: P2P Call to "+ buddy1 );
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddy1);
	TapCallbutton();
	
	LogMessage("ConferenceTests :: VerifyPassiveP2PCallEscalationToConferenceCallTest :: Wait for Audio call to get connected");
	WaitforAudioWindow();
	WaitForCallToGetConnected();
		
	var messageToRecieve = buddy1 + " - " + audioConnectedInP2PCallMessage;
	LogMessage("ConferenceTests :: VerifyPassiveP2PCallEscalationToConferenceCallTest :: Switch to IM view");
	GotoModalityView(IM_MODALITY_BUTTON);
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddy1, messageToRecieve);

	DismissNotificationsIfAny();
	
	//Wait to verify third participant has joined the conversation
	var buddy2ConversationJoinMessage = buddy2 + conversationJoinedMessage;
	VerifyReceivedIM("", buddy2ConversationJoinMessage);
	
	// Verify participants joined the conference
	messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	messageToRecieve = buddy2 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(1), messageToRecieve);

	VerifyRosterParticipants(buddyList);
	DelayInSec(2);
	
	EndConversation();
}


function VerifyRosterUpdateTest()
{
	LogMessage("ConferenceTests :: VerifyRosterUpdateTest");
	
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	var buddyList = GetTestCaseParameters("BuddyListDisplayName");
	var buddy1 = GetBuddyDisplayName(0);
	var buddy2 = GetBuddyDisplayName(1);
	
	GotoMyInfo();
	SetNote("VerifyRosterUpdate");
	
	LogMessage("ConferenceTests :: VerifyRosterUpdateTest :: Conf Call to " + groupname );
	GotoContacts();
	OpenGroupContactCard(groupname);
	TapGroupCallbutton();
	
	LogMessage("ConferenceTests :: VerifyRosterUpdateTest :: Wait for Audio call to get connected");
	WaitforAudioWindow();
	WaitForCallToGetConnected();
		
	LogMessage("ConferenceTests :: VerifyRosterUpdateTest :: Switch to IM view");
	GotoModalityView(IM_MODALITY_BUTTON);
	
	// Verify participants joined the conference
	var messageToSend = selfDisplayName + " - " + verifyAudioConnectedInConferenceMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	messageToRecieve = buddy2 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(1), messageToRecieve);

	VerifyRosterParticipants(buddyList);
	DelayInSec(2);
	GotoModalityView(IM_MODALITY_BUTTON);
	
	var messageToSend = selfDisplayName + " - " + buddy2 + " Leaves Conference";
	SendIM(messageToSend);
	DelayInSec(5);//Roster Updates
	VerifyRosterParticipants(buddy1);
	DelayInSec(2);

	EndConversation();
}


function VerifyHoldUnholdConferenceCallTest() {
    LogMessage("ConferenceTests :: VerifyHoldUnholdConferenceCallTest");

    var selfDisplayName = GetTestCaseParameters("DisplayName1");
    var buddy1 = GetBuddyDisplayName(0);

    var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
    var buddy1ConversationJoinMessage = buddy1 + conversationJoinedMessage;

    SetNote("VerifyHoldUnholdConferenceCall");

    AceptRejectCallToast(AcceptString);
    LogMessage("ConferenceTests :: VerifyHoldUnholdConferenceCallTest :: Waiting for audio window");
    WaitforAudioWindow();
    WaitForCallToGetConnected();

    LogMessage("ConferenceTests :: VerifyHoldUnholdConferenceCallTest :: Switch to IM view");
    GotoModalityView(IM_MODALITY_BUTTON);
    WaitforConversationWindow();

    // Verify participants joined the conference
    VerifyReceivedIM("", selfConversationJoinMessage);
    VerifyReceivedIM("", buddy1ConversationJoinMessage);

    var messageToSend = selfDisplayName + " - " + verifyAudioConnectedInConferenceMessage;
    SendIM(messageToSend);
    messageToSend = messageToSend.split(" ").join("  ");
    VerifyReceivedIM(selfDisplayName, messageToSend);

    messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
    messageToRecieve = messageToRecieve.split(" ").join("  ");
    VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);

    LogMessage("ConferenceTests :: VerifyHoldUnholdConferenceCallTest :: Switch to audio view");
    GotoModalityView(VOICE_MODALITY_BUTTON);
    TapOnAudioControl(HOLD_BUTTON);

    LogMessage("ConferenceTests :: VerifyHoldUnholdConferenceCallTest :: Switch to IM view");
    GotoModalityView(IM_MODALITY_BUTTON);
    var messageToSend = selfDisplayName + " - " + verifyAudioHoldInConferenceMessage;
    SendIM(messageToSend);
    messageToRecieve = buddy1 + " - " + selfDisplayName + " " + audioHeldInConferenceMessage;
    messageToRecieve = messageToRecieve.split(" ").join("  ");
    VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);

    LogMessage("ConferenceTests :: VerifyHoldUnholdConferenceCallTest :: Switch to audio view");
    GotoModalityView(VOICE_MODALITY_BUTTON);
    TapOnAudioControl(HOLD_BUTTON);

    LogMessage("ConferenceTests :: VerifyHoldUnholdConferenceCallTest :: Switch to IM view");
    GotoModalityView(IM_MODALITY_BUTTON);
    var messageToSend = selfDisplayName + " - " + verifyAudioHoldInConferenceMessage;
    SendIM(messageToSend);
    messageToRecieve = buddy1 + " - " + selfDisplayName + " " + audioNotHeldInConferenceMesssage;
    messageToRecieve = messageToRecieve.split(" ").join("  ");
    VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);

    EndConversation();
}


function VerifyAdmitInLobbyJoinTest()
{
	LogMessage("ConferenceTests :: VerifyAdmitInLobbyJoinTest");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	var buddy1 = GetBuddyDisplayName(0);
	
  var meetingSubject = GetTestCaseParameters("LobbyJoinMeetingSubject");
  var meetingURL = GetMeetingURL(meetingSubject);
   
  GotoMyInfo();
  //Set space separated params
  SetNote("VerifyAdmitInLobbyJoin "+ meetingURL);
	
	GotoMeetings();
	OpenMeetingWithSubject(meetingSubject);
	TapOnJoinButton();
	
	VerifyLobbyJoinMessage();
	
	LogMessage("ConferenceTests :: VerifyAdmitInLobbyJoinTest :: Waiting for remote end to admit(40 Sec BOT end).. ");
	DelayInSec(40);//BOT sync up 
	WaitforAudioWindow();
	WaitForCallToGetConnected();
		
	LogMessage("ConferenceTests :: VerifyAdmitInLobbyJoinTest :: Switch to IM view");
	GotoModalityView(IM_MODALITY_BUTTON);
	
	// Verify participants joined the Meeting
	var messageToSend = selfDisplayName + " - " + verifyAudioConnectedInConferenceMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddy1, messageToRecieve);
	
	EndConversation();
}

function VerifyOcomP2PCallEscalationToConferenceCallTest() {
    LogMessage("ConferenceTests :: VerifyOcomP2PCallEscalationToConferenceCallTest");

    var buddyList = GetTestCaseParameters("BuddyListDisplayName");
    var buddy1 = GetBuddyDisplayName(0);
    var buddy2 = GetBuddyDisplayName(1);

    GotoMyInfo();
    SetNote("VerifyOcomP2PCallEscalationToConferenceCall");

    LogMessage("ConferenceTests :: VerifyOcomP2PCallEscalationToConferenceCallTest :: Waiting for the audio toast");
    AceptRejectCallToast(AcceptString);

    WaitforAudioWindow();
    WaitForCallToGetConnected();

    var messageToRecieve = buddy1 + " - " + audioConnectedInP2PCallMessage;
    LogMessage("ConferenceTests :: VerifyOcomP2PCallEscalationToConferenceCallTest :: Switch to IM view");
    GotoModalityView(IM_MODALITY_BUTTON);
    messageToRecieve = messageToRecieve.split(" ").join("  ");
    VerifyReceivedIM(buddy1, messageToRecieve);

    DismissNotificationsIfAny();

    //Wait to verify third participant has joined the conversation
    LogMessage("ConferenceTests :: VerifyOcomP2PCallEscalationToConferenceCallTest :: Verify third participant has joined");
    var buddy2ConversationJoinMessage = buddy2 + conversationJoinedMessage;
    VerifyReceivedIM("", buddy2ConversationJoinMessage);

    // Verify participants joined the conference
    messageToRecieve = buddy1 + " - " + audioConnectedInConferenceMessage;
    messageToRecieve = messageToRecieve.split(" ").join("  ");
    VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
    messageToRecieve = buddy2 + " - " + audioConnectedInConferenceMessage;
    messageToRecieve = messageToRecieve.split(" ").join("  ");
    VerifyReceivedIM(GetBuddyDisplayName(1), messageToRecieve);

    VerifyRosterParticipants(buddyList);
    DelayInSec(2);

    EndConversation();
}

function VerifyOnlineMeetingsForDayTest() {
    LogMessage("ConferenceTests :: VerifyMeetingsForDayTest");

    var onlineMeetingSubject = GetTestCaseParameters("OnlineMeetingSubject");
    var offlineMeetingSubject = GetTestCaseParameters("OfflineMeetingSubject");

    GotoMeetings();

    LogMessage("ConferenceTests :: VerifyMeetingsForDayTest :: Verify All Meetings Tab");
    OpenMeetingWithSubject(onlineMeetingSubject);
    VerifyMeetingSubject(onlineMeetingSubject);
    VerifyJoinButton();

    LogMessage("ConferenceTests :: VerifyMeetingsForDayTest :: Verify Offline meeting");
    OpenMeetingWithSubject(offlineMeetingSubject);
    VerifyMeetingSubject(offlineMeetingSubject);

    LogMessage("ConferenceTests :: VerifyMeetingsForDayTest :: Verify Online Meetings Tab");
    tapOnlineMeetingbutton();
    OpenMeetingWithSubject(onlineMeetingSubject);
    VerifyMeetingSubject(onlineMeetingSubject);
    VerifyJoinButton();
    LogMessage("ConferenceTests :: VerifyMeetingsForDayTest :: Go Back to All Meetings Tab");
    tapAllMeetingButton();
}

function VerifyNoMeetingTest()
{
   
    if(IsSignInScreenUp() == 1){
        SignInAuto();
    }

    var meetingsButton = TabBar.buttons()[GetValueFromKey("LOCID Meetings")]
    if(ElementValidAndVisible(meetingsButton) == true){
        TapElement(meetingsButton);
        var button = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID All")];
        if(ElementValidAndVisible(button) == true){
            
            TapElement(button);
            captureLocalizedScreenshot("383361");
        }else {
            LogMessage("383361 Fail");
        }
        button = mainWindow().tableViews()[0].buttons()[GetValueFromKey("LOCID Online")];
        if(ElementValidAndVisible(button) == true){
            
            TapElement(button);
            captureLocalizedScreenshot("383362");
        }else {
            LogMessage("383362 Fail");
        }
    }else {
        LogMessage("383361 383362 Fail");
    }
    
    var contactsButton = TabBar.buttons()[GetValueFromKey("LOCID Contacts")];
    if(ElementValidAndVisible(contactsButton) == true){
       TapElement(contactsButton);
       var button = MainWindow.tableViews()[0].buttons()["iPhone Simulator"];
        if(ElementValidAndVisible(button) == true){
            
            TapElement(button);
            captureLocalizedScreenshot("408620");
        }else {
            LogMessage("408620 Fail");
        }
    }else {
        LogMessage("408620 Fail");
    }
}

function MeetingsTest()
{
    var meetingsButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Meetings")];
    if(ElementValidAndVisible(meetingsButton) == true){
        
        TapElement(meetingsButton);
        captureLocalizedScreenshot("383360");
//        TapNavigationRightButton(0.5);
//        captureLocalizedScreenshot("383363");
//        DelayInSec(4);
//        captureLocalizedScreenshot("383364");
//        captureLocalizedScreenshot("408392");
        if(TapViewElement(0,7) == true){
            captureLocalizedScreenshot("73292");
            captureLocalizedScreenshot("398892");
            if(TapViewElement(0,4) == true){
                captureLocalizedScreenshot("383367");
                TapNavigationBackButton();
            }else{
                LogMessage("383367 Fail");
            }
          
            var joinCell = MainWindow.tableViews()[0].cells()[GetValueFromKey("LOCID Join Meeting")];
            if(ElementValidAndVisible(joinCell) == true){
                joinCell.tap();
                DelayInSec(0.5);
                captureLocalizedScreenshot("383354");
                var dismissButton = MainWindow.buttons()[GetValueFromKey("LOCID Dismiss")];
                if(ElementValidAndVisible(dismissButton) == true){
                    DismissNotificationsIfAny();
                    TapNavigationBackButton();
                    DeleteAllConversations();
                }
                
            }else{
                LogMessage("383354 Fail");
            }
            GotoMeetings();
            TapNavigationBackButton();
        }else {
            LogMessage("383367 73292 398892 383354Fail");
        }
        
    }else {
        LogMessage("383360 383363 383364 408392 383367 73292 398892 383354Fail");
    }
}

function VerifyMeetingTest()
{
    if(IsSignInScreenUp() == 1){
        SignInAuto();
    }
    ValidateTabBarButtons();
    MeetingsTest();
}

function VerifyDateStringDisplayWellOnMeetingPage()
{
    try{
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        LogMessage("Not first time Sign In")
    }
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("456138 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    DismissNotificationsIfAny();
    GotoMeetings();
    if(MainWindow.tableViews()[0].isValid() == true) {
        captureLocalizedScreenshot("456138");
    }else {
         LogMessage("456138 Fail");
    }
   
    SignOutApp();
}

function VerifyStringRestartConversationDisplayWellOnConversationPage()    //455382
{
    try{
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        LogMessage("Not first time Sign In")
    }
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            
            LogMessage("455382 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyStringRestartConversationDisplayWellOnConversationPage");
    
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
    if(button.isVisible() == true){
        TapElement(button);
        WaitForNavigationButtonEnable(GetValueFromKey("LOCID Video"));
        DismissNotificationsIfAny();
        TapNavigationRightButton();
        DelayInSec(1);
        TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        if(TapTableviewCellWithPredicate(BUDDYDISPLAYNAME) == true){
            
            DismissNotificationsIfAny();
            captureLocalizedScreenshot("455382");
            TapNavigationBackButton();
        }else {
            
            LogMessage("455382 Fail");
        }
      
    }else {
        
       LogMessage("455382 Fail");
    }

    GotoMyInfo();
    SetHappeningNote("");
}


function VerifyStringWhiteboardDisplayWellOnConversationPage()
{
    
    try{
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        LogMessage("Not first time Sign In")
    }
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("455968 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyStringWhiteboardDisplayWellOnConversationPage");
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
    
    if(button.isVisible() == true){
        
        TapElement(button);
        DismissNotificationsIfAny();
        button = WaitforToastDissMissButton(GetValueFromKey("LOCID Accept"));
        if(button.isVisible() == true){
     
            TapElement(button);
            if(WaitForMainViewElementValidAndVisible(MainWindow.scrollViews()[0],GetValueFromKey("LOCID Stop Viewing")) == true){
                
                DismissNotificationsIfAny();
                captureLocalizedScreenshot("455968");
            }
        }else {
            
             LogMessage("455968 Fail");
        }
        
        TapNavigationRightButton();
        DelayInSec(1);
        TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
                
    }else {
        
        LogMessage("455968 Fail");
    }

    GotoMyInfo();
    SetHappeningNote("");
}

function VerifyShareDesktopAlertMessageDisplayOnConversationPage()
{
    try{
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        LogMessage("Not first time Sign In")
    }
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("455969 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyShareDesktopAlertMessageDisplayOnConversationPage");
    
    GotoContacts();
    var groupString = GetValueFromKey("LOCID Pinned Contacts");
    ExpandGroupFromContactList(groupString,0.03,0.30);
    DelayInSec(2);
    
    if(TapTableviewCellWithPredicate(BUDDYDISPLAYNAME) == true){
        
        var imButton = MainWindow.tableViews()[0].elements()[9];
        TapElement(imButton);
        DismissNotificationsIfAny();
        TypeString(BUDDYDISPLAYNAME + " - " + "message\n\n");
        
        var string = GetValueFromKey("LOCID AppSharingCannotAcceptP2PScreenSharing");
        var replaceString = string.replace("%@",BUDDYDISPLAYNAME);
        var staticString = WaitForMainViewAlertMessageVisible(MainWindow,replaceString);
        if(staticString.isVisible() == true){
            
            staticString.tapWithOptions({tapOffset:{x:0.49, y:0.30}});
            DelayInSec(1);
            captureLocalizedScreenshot("455969");
            var DismissButton = WaitforToastDissMissButton(GetValueFromKey("LOCID Dismiss"));
            TapElement(DismissButton);
            
        }else {
            
            LogMessage("455969 Fail");
        }
        
        DismissNotificationsIfAny();
        TapNavigationBackButton();
        GotoContacts();
        TapNavigationBackButton();
        
    
    }else {
        
        LogMessage("455969 Fail");
    }
    
    CollapseGroupFromContactList(groupString,0.03,0.30);
    GotoMyInfo();
    SetHappeningNote("");
}

function VerifyRejoinButtonDisconnectMettingStringOnChatsPage()
{
    
    try{
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        LogMessage("Not first time Sign In")
    }
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("455377 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow();
    GotoMeetings();
    DelayInSec(1);
    
    if(TapTableviewCellWithPredicate("iPhoneTest") == true){
        
        DismissNotificationsIfAny();
        var joinCell = mainWindow.tableViews()[0].cells()[GetValueFromKey("LOCID Join Meeting")];
        
        if(WaitIndexElementValidAndVisible(joinCell,15) == true){
            
            joinCell.tap();
            DelayInSec(1);
            
            if(WaitIndexElementValidAndVisible(target.frontMostApp().navigationBar().rightButton(),5) == true){
                
                var dissmissButton = mainWindow.buttons()[GetValueFromKey("LOCID Dismiss")];
                if(dissmissButton.isVisible() == true)
                {
                    dissmissButton.tap();
                }
                var rejoinButton = mainWindow.scrollViews()[0].buttons()[GetValueFromKey("LOCID Rejoin")];
                if(rejoinButton.isVisible() == true){
                    
                    captureLocalizedScreenshot("455377");
                }else {
                    
                    LogMessage("455377 Fail");
                }
                TapNavigationBackButton();
                
            }else {
                
                var cancelButton = mainWindow.buttons()[GetValueFromKey("LOCID Cancel")];
            
                if(cancelButton.isVisible() == true){
                    
                    cancelButton.tap();
                    DelayInSec(1);
                    
                    if(TapTableviewCellWithPredicate("iPhoneTest") == true){
                        
                        var rejoinButton = mainWindow.scrollViews()[0].buttons()[GetValueFromKey("LOCID Rejoin")];
                        if(rejoinButton.isVisible() == true){
                            
                            captureLocalizedScreenshot("455377");
                        }else {
                            
                            LogMessage("455377 Fail");
                        }
                        
                        TapNavigationBackButton();
                        
                    }else{
                        LogMessage("455377 Fail");
                    }
                }else {
                    
                    LogMessage("455377 Fail");
                }
                
            }
            
        }else{
            LogMessage("455377 Fail");
        }
        
        GotoMeetings();
        TapNavigationBackButton();
        
    }else {
        
        LogMessage("455377 Fail");
    }
}

function VerifyDeclineToastPresentingAlertStringOnConversationPage()
{
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("73306 Fail!!");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyDeclineToastPresentingAlertStringOnConversationPage");
    
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
    if(button.isVisible() == true){
        
        TapElement(button);
        DismissNotificationsIfAny();
        var declineButton = WaitforToastDissMissButton(GetValueFromKey("LOCID Decline"));
        if(declineButton.isVisible() == true){
            
            captureLocalizedScreenshot("73306");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("73306_h");
            GoToOrientation(DEVICEPROT);
           
            TapNavigationBackButton();
            return;
        }else {
            
            TapNavigationBackButton();
            LogMessage("73306 Fail!!");
            return;
        }
        
        DelayInSec(2);
        UIATarget.localTarget().frontMostApp().navigationBar().rightButton().doubleTap();
        DelayInSec(1);
        TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        
    }else {
        
        LogMessage("73306 Fail!!");
    }

}


function VerifyShareDesktopAndStringLoadingOnConversationPage()
{
    try{
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        LogMessage("Not first time Sign In")
    }
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
             LogMessage("455508  455508_h Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyShareDesktopAndStringLoadingOnConversationPage");
    
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
    if(button.isVisible() == true){
        
        TapElement(button);
        DismissNotificationsIfAny();
        var acceptButton = WaitforToastDissMissButton(GetValueFromKey("LOCID Accept"));
        if(acceptButton.isVisible() == true){
            
            TapElement(acceptButton);
            if(WaitForMainViewElementValidAndVisible(MainWindow.scrollViews()[0],GetValueFromKey("LOCID Stop Viewing")) == true){
                
                DelayInSec(1);
                captureLocalizedScreenshot("455507");
                UIATarget.localTarget().setDeviceOrientation(DEVICELEFT);
                DelayInSec(0.5);
                captureLocalizedScreenshot("455507_h");
                UIATarget.localTarget().setDeviceOrientation(DEVICEPROT);
                
                var string = GetValueFromKey("LOCID AppSharing Desktop");
                
                var staticString = WaitForMainViewAlertMessageVisible(MainWindow.scrollViews()[0],string);
                if(staticString.isVisible() == true){
                    
                    DelayInSec(3);
                    staticString.tapWithOptions({tapOffset:{x:0.49, y:0.30}});
                    captureLocalizedScreenshot("455508");
                    UIATarget.localTarget().setDeviceOrientation(DEVICELEFT);
                    DelayInSec(0.5);
                    captureLocalizedScreenshot("455508_h");
                    UIATarget.localTarget().setDeviceOrientation(DEVICEPROT);
                                       
                }else {
                    
                    LogMessage("455508  455508_h Fail");
                }
            }else{
                LogMessage("455507 455508 455507_h 455508_h Fail");
            }
            
        }else {
            
            LogMessage("455507 455508 455507_h 455508_h Fail");
        }
        
        DismissNotificationsIfAny();
        MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.83, y:0.49}});
        if(TapForNavigationButton(GetValueFromKey("LOCID Instant Messaging"))){
            TypeString(BUDDYDISPLAYNAME + " - " + "message\n\n");
            DelayInSec(1);
        }
        
        DelayInSec(2);
        UIATarget.localTarget().frontMostApp().navigationBar().rightButton().doubleTap();
        DelayInSec(1);
        TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        
    }else {
        
       LogMessage("455507 455508 455507_h 455508_h Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("");
}

function VerifyInviteOtherBuddyOnConversationPage()
{
    
    try{
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        LogMessage("Not first time Sign In")
    }
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("455905 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyInviteOtherBuddyOnConversationPage");
    
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
    if(button.isVisible() == true){
        
        TapElement(button);
        DismissNotificationsIfAny();
        WaitForNavigationButtonEnable(GetValueFromKey("LOCID Video"));
        TapNavigationRightButton();
        DelayInSec(1);

        TapActionSheetButton(GetValueFromKey("LOCID Invite"));
        DelayInSec(1);
        
        var cancelButton = Target.frontMostApp().navigationBar().buttons()[GetValueFromKey("LOCID Cancel")];
        if(cancelButton.isVisible() == true){
            
            DelayInSec(1);
            DismissNotificationsIfAny();
            captureLocalizedScreenshot("455905");
            cancelButton.doubleTap();
        }else{
            LogMessage("455905 Fail");
        }
        
        if(TapForNavigationButton(GetValueFromKey("LOCID Instant Messaging"))){
            TypeString(BUDDYDISPLAYNAME + " - " + "message\n\n");
            DelayInSec(1);
        }
    
        DelayInSec(2);
        UIATarget.localTarget().frontMostApp().navigationBar().rightButton().doubleTap();
        DelayInSec(1);
        TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        
    }else{
         LogMessage("455905 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("");
}

function VerifyNoMeetingStringDisplayWell() {
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_2,PASSWORD_2);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            
            LogMessage("398204 Fail");
            return;
        }
        else if (returnValue == 1){
            
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_2);
        }
        else {
            
        }
    }
    
    var continueButton = MainWindow.scrollViews()[0].buttons()[GetValueFromKey("LOCID Continue")];
    if(continueButton.isVisible() == true){
        
        continueButton.tap();
        DelayInSec(1);
    }
    
    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow();
    var meetingsButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Meetings")];
	if(meetingsButton.isVisible() == true){
        
        meetingsButton.tap();

        captureLocalizedScreenshotWithNoDisMiss("383361");
        UIATarget.localTarget().setDeviceOrientation(DEVICELEFT);
        captureLocalizedScreenshotWithNoDisMiss("383361_h");
        GoToOrientation(DEVICEPROT);
    }else {
        
        LogMessage("383361 Fail");
    }
    
    GotoMyInfo();
	TapNavigationRightButton(0);
    
	Target.pushTimeout(15);
    var signInButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Sign In")];
	Target.popTimeout();
	IsValidAndVisible(signInButton,"Sign In Button");
    
}

function VerifyEnventPageOnMettingViewDisplayWell(){
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("73292 Fail!!");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    var meetingsButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Meetings")];
    if(ElementValidAndVisible(meetingsButton) == true){
        
        TapElement(meetingsButton);
        DelayInSec(10);
//        MainWindow.tableViews()[0].scrollUp();
//        DelayInSec(15);
        
        if(TapTableviewCellWithPredicate(MEETINGNAME) == true){
            
            captureLocalizedScreenshot("73292");

            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("73292_h");
            DelayInSec(1);
            MainWindow.tableViews()[0].scrollDown();
            
            GoToOrientation(DEVICEPROT);
            TapNavigationBackButton();
        }else {
            
            LogMessage(" 73292 Fail!!");
        }
        
    }else {
        
        LogMessage("73292 Fail!!");
    }
}


function VerifyAllOfMettingsInTheMettingList() {
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow();
    var meetingsButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Meetings")];
    
    if(ElementValidAndVisible(meetingsButton) == true){
        
        TapElement(meetingsButton);
    
        DelayInSec(5);
        captureLocalizedScreenshot("408392");
        GoToOrientation(DEVICELEFT);
        captureLocalizedScreenshot("408392_h");
        GoToOrientation(DEVICEPROT);
               
    }else {
        
        LogMessage("408392 Fail!!");
    }
}

function VerifyAudioCallPageDispalyWell() {
    
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("Sign in failed");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    var target = UIATarget.localTarget();
    GotoMyInfo();
    SetHappeningNote("VerifyAudioCallPageDispalyWell");
        
    GotoContacts();
    var groupString = GetValueFromKey("LOCID Pinned Contacts");
    ExpandGroupFromContactList(groupString,0.03,0.30);
        
    if(TapTableviewCellWithPredicate(BUDDYDISPLAYNAME) == true){
            
        var audioButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Call")];
            
        if(audioButton.isVisible() == true){
                
            TapElement(audioButton);
            DelayInSec(1);
        }else {
                
            audioButton = MainWindow.tableViews()[0].buttons()[2];
            if(audioButton.isVisible() == true){
                    
                TapElement(audioButton);
                DelayInSec(1);
            }else {
                    
                LogMessage("445859 Fail");
                return;
            }
                
        }
            
        if(WaitForNavigationButtonEnable(GetValueFromKey("LOCID Video")) == true){
                
            MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.42, y:0.88}});
            DismissNotificationsIfAny();
            captureLocalizedScreenshot("445859");
//                captureLocalizedScreenshot("445980");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("445859_h");
//                captureLocalizedScreenshot("445980_h");
            GoToOrientation(DEVICEPROT);
                
            }else {
                
                LogMessage("445859 Fail");
//                LogMessage("445980 Fail");
            }
            
            MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.42, y:0.88}});
            var button = target.frontMostApp().navigationBar().leftButton();
            if(button.isVisible()==true){
                
                button.tap();
                DelayInSec(1);
            }
            
            GotoContacts();
            TapNavigationBackButton();
            
    }else {
            
            LogMessage("445859 Fail");
//            LogMessage("445980 Fail");
    }
        
    CollapseGroupFromContactList(groupString,0.03,0.30);
    GotoMyInfo();
    SetHappeningNote("");
}


try{
    
}
catch(error){
    
    LogMessage("244348 Fail");
}


function VerifyShareDesktopViewDisplayWell() {
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("Sign in failed");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    var target = UIATarget.localTarget();
    GotoMyInfo();
    SetHappeningNote("VerifyShareDesktopViewDisplayWell");
    
    try{
        
        var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
        if(button.isVisible() == true){
            
            button.tap();
            
            DelayInSec(1);
            var audioButton = target.frontMostApp().navigationBar().buttons()[GetValueFromKey("LOCID ACCESSIBILITY_CONTACTS_AUDIOCALL_START")];
            
            if(audioButton.isValid()==true && audioButton.isVisible() == true){
                
                audioButton.tap();
            }
            
            DelayInSec(45);
            
            captureLocalizedScreenshotWithNoDisMiss("244357");
            UIATarget.localTarget().setDeviceOrientation(DEVICELEFT);
            captureLocalizedScreenshotWithNoDisMiss("244357_h");
            UIATarget.localTarget().setDeviceOrientation(DEVICEPROT);
            
            DelayInSec(2);
            target.tap({x:180.00, y:270.33});
            var button = target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Hang Up")];
            
            if(button.isValid()==true && button.isVisible() == true){
                
                button.tap();
            }
            
            TapNavigationBackButton();
            
        }else {
            
            LogMessage("244357 Fail");
        }
    }
    catch(error){
        
        LogMessage("244357 Fail");
    }
    GotoMyInfo();
    SetHappeningNote("");
}


function VerifyAdhocMeetingPPTView_JoiningAfterPPTShare() {
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("Sign in failed");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    var target = UIATarget.localTarget();
    GotoMyInfo();
    SetHappeningNote("VerifyAdhocMeetingPPTView_JoiningAfterPPTShare test");
    
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
    if(button.isVisible() == true){
        
        button.tap();
        DelayInSec(2);
        
        if(WaitForNavigationButtonEnable(GetValueFromKey("LOCID Video")) == true){
            
            WaitforToastDissMissButton(GetValueFromKey("LOCID Accept"));
            if(MainWindow.buttons()[GetValueFromKey("LOCID Accept")].isVisible() == true){
                
                MainWindow.buttons()[GetValueFromKey("LOCID Accept")].tap();
                DelayInSec(32);
                if(MainWindow.scrollViews()[0].isVisible() == true){
                    
                    MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.43, y:0.70}});
                }else {
                    
                    if(MainWindow.scrollViews()[0].scrollViews()[0].isVisible() == true){
                        MainWindow.scrollViews()[0].scrollViews()[0].tapWithOptions({tapOffset:{x:0.43, y:0.70}});
                    }
                }
                
                MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.43, y:0.70}});
                DelayInSec(2);
                MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.43, y:0.70}});
                
                captureLocalizedScreenshotWithNoDisMiss("75140");
                UIATarget.localTarget().setDeviceOrientation(DEVICELEFT);
                captureLocalizedScreenshotWithNoDisMiss("75140_h");
                
                button = target.frontMostApp().navigationBar().leftButton();
                if(button.isVisible()==true){
                    
                    UIATarget.localTarget().setDeviceOrientation(DEVICEPROT);
                    button.tap();
                    DelayInSec(1);
                    
                    GotoChats();
                    DeleteAllConversations();
                    
                    GotoMyInfo();
                    SetHappeningNote("Test");
                    DelayInSec(1);
                    
                    return;
                }
                
                UIATarget.localTarget().setDeviceOrientation(DEVICEPROT);
            }else {
                
                LogMessage("75140 Fail");
            }
            
        }else {
            
            LogMessage("75140 Fail");
        }
        
        DelayInSec(1);
        if(MainWindow.scrollViews()[0].isVisible() == true){
            
            MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.43, y:0.70}});
        }else {
            
            if(MainWindow.scrollViews()[0].scrollViews()[0].isVisible() == true){
                MainWindow.scrollViews()[0].scrollViews()[0].tapWithOptions({tapOffset:{x:0.43, y:0.70}});
            }
        }
        
        MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.43, y:0.70}});
        button = target.frontMostApp().navigationBar().leftButton();
        if(button.isVisible()==true){
            
            button.tap();
            DelayInSec(1);
        }
        
    }else {
        
        LogMessage("75140 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("Test");
}


function VerifyStringsSeeLobbyAndAdmitDisplayedWellInMeeting() {
    
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    var meetingURL = GetMeetingURL(MEETINGNAME_2);
    GotoMyInfo();
    SetHappeningNote("VerifyStringsSeeLobbyAndAdmitDisplayedWellInMeeting " + meetingURL);
    
    var meetingsButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Meetings")];
    if(ElementValidAndVisible(meetingsButton) == true){
        
        TapElement(meetingsButton);
  
        if(TapTableviewCellWithPredicate(MEETINGNAME_2) == true){
            
            var joinCell = MainWindow.tableViews()[0].cells()[GetValueFromKey("LOCID Join Meeting")];
            if(ElementValidAndVisible(joinCell) == true){
                
                joinCell.tap();
                
                var button = WaitforToastDissMissButton(GetValueFromKey("LOCID See Lobby"));
                if(button.isVisible() == true){//
                
                    captureLocalizedScreenshotWithNoDisMiss("75138_1");
                    GoToOrientation(DEVICELEFT);
                    captureLocalizedScreenshotWithNoDisMiss("75138_1_h");
                    GoToOrientation(DEVICEPROT);
                    DelayInSec(1);
                    
                    TapElement(button);
                    DelayInSec(1);
                    
                    captureLocalizedScreenshotWithNoDisMiss("75138_2");
                    GoToOrientation(DEVICELEFT);
                    captureLocalizedScreenshotWithNoDisMiss("75138_2_h");
                    GoToOrientation(DEVICEPROT);
                    DelayInSec(1);
                    
                    button = MainWindow.tableViews()[0].groups()["LOBBY (1)"].buttons()[GetValueFromKey("LOCID Admit all")];
                    if(button.isVisible() == true){
                        
                        TapElement(button);
                        DelayInSec(3);
                        if(TapTableviewCellWithPredicate(BUDDYDISPLAYNAME) == true){
                            
                            DelayInSec(2);
                            captureLocalizedScreenshot("75139_1");
                            
                            TapActionSheetButton(GetValueFromKey("LOCID Cancel"));
                            GoToOrientation(DEVICELEFT);
                            TapTableviewCellWithPredicate(BUDDYDISPLAYNAME);
                            
                            captureLocalizedScreenshot("75139_1_h");
                            TapActionSheetButton(GetValueFromKey("LOCID Promote"));
                            GoToOrientation(DEVICEPROT);
                            DelayInSec(5);
                            TapTableviewCellWithPredicate(BUDDYDISPLAYNAME);
                            captureLocalizedScreenshot("75139_2");
                            TapActionSheetButton(GetValueFromKey("LOCID Cancel"));
                            GoToOrientation(DEVICELEFT);
                            TapTableviewCellWithPredicate(BUDDYDISPLAYNAME);
                            captureLocalizedScreenshot("75139_2_h");
                            TapActionSheetButton(GetValueFromKey("LOCID Cancel"));
                            GoToOrientation(DEVICEPROT);
                            TapNavigationBackButton();
                                
                        }else{
                                
                            LogMessage("75139 Fail!!");
                        }
                        
                    }
                    
                }
                TapNavigationBackButton();
            }else{
                LogMessage("75138 75139 Fail!!");
            }
        }else {
            
            LogMessage("75138 75139 Fail!!");
        }
        
    }else {
        
        LogMessage("75138 75139 Fail!!");
    }

    GotoMyInfo();
    SetHappeningNote("");
}

function VerifyJoinOnlineMeetingFromEvent(){
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
       
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    GotoMeetings();
    var target = UIATarget.localTarget();
    
    if(TapTableviewCellWithPredicate(MEETINGNAME,GetValueFromKey("LOCID Meetings")) == true){
        
        var join_Button = MainWindow.tableViews()[0].buttons()[GetValueFromKey("MEETING_VIEW_JOIN_BUTTON_TEXT")]
        
        if(join_Button.isVisible() == true && join_Button.isValid() == true) {
            
            join_Button.tap();
           
            if(WaitButtonEnabledAndValid(GetValueFromKey("LOCID Add")) == true) {
                
                captureLocalizedScreenshot("244347");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("244347_h");
                GoToOrientation(DEVICEPROT);
            }else {
                
                LogMessage("244347 Fail!!");
            }
        
            var button = target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Hang Up")];
            
            if(button.isValid()==true && button.isVisible() == true){
                
                button.tap();
            }
            
            TapNavigationBackButton();
            
        }
        
    }else {
        
     
        var button = target.frontMostApp().navigationBar().buttons()[GetValueFromKey("DISMISS_BUTTON")];
        if(button.isValid()==true && button.isVisible()==true){
            
            button.tap();
        }
        DelayInSec(1);
        LogMessage("244347 Fail!!");
    }

}

function VerifyP2PIMEscalationToConferenceCallTest() {
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            
            
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }

    GotoMyInfo();
    var target = UIATarget.localTarget();
    SetHappeningNote("VerifyP2PIMEscalationToConferenceCallTest");
    
    try{
        
        var button = WaitforToastViewAppear();
        if(button.isVisible() == true && button.isVisible() == true){
            
            try{
                
                button.tap();
                DelayInSec(1);
                
                button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
                if(button.isVisible() == true){
                    
                    captureLocalizedScreenshot("244348_2");
                    
                    TapElement(button);
                    DelayInSec(6);
                    
//                    if(WaitButtonEnabledAndValid(GetValueFromKey("LOCID Dial Pad Only")) == true) {
                    
                    captureLocalizedScreenshot("244348_3");
//                    }
                    
                    var button = target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Hang Up")];
                    
                    if(button.isValid()==true && button.isVisible() == true){
                        
                        button.tap();
                    }
                    
                    var messageToSend = GetTestCaseParameters("DisplayName2") + " - " + testMessage;
                    SendIM(messageToSend);
                    DelayInSec(1);
                    captureLocalizedScreenshot("244348_1");
                    
                    TapNavigationBackButton();
                }else {
                    
                    LogMessage("244348 Fail");
                }
            }
            catch(error){
                
                LogMessage("244348 Fail");
            }
        }else {
            
            LogMessage("244348 Fail");
        }

    }
    catch(error){
        
        LogMessage("244348 Fail");
    }

    GotoMyInfo();
    SetHappeningNote("");
}

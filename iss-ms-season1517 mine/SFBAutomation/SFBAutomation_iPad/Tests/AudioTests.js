function VerifyOutgoingP2PCallTest()
{
	LogMessage("AudioTests :: VerifyOutgoingP2PCallTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	
	SetNote("VerifyOutgoingP2PCall");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	EndConversation();
}

function VerifyOutgoingP2PCallFromContactCardTest()
{
	LogMessage("AudioTests :: VerifyOutgoingP2PCallFromContactCardTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	
	SetNote("VerifyOutgoingP2PCallFromContactCard");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	EndConversation();
}

function VerifyEndP2PCallTest()
{
	LogMessage("AudioTests :: VerifyEndP2PCallTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	
	SetNote("VerifyEndP2PCall");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	GotoModalityView(VOICE_MODALITY_BUTTON);
	WaitforAudioWindow();
	
	TapOnAudioControl("Hang Up");
	
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	WaitForBuddyNoteChange("CallEndedRemotly");

}

function VerifyIncomingP2PCallTest()
{
	LogMessage("AudioTests :: VerifyIncomingP2PCallTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");

	GotoMyInfo();
	
	SetNote("VerifyIncomingP2PCall");

	LogMessage("AudioTests :: VerifyIncomingP2PCallTest :: Waiting for the audio toast");
	AceptRejectCallToast(AcceptString);
		
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	EndConversation();
}

function VerifyIncomingP2PCallRejectTest()
{
	LogMessage("AudioTests :: VerifyIncomingP2PCallRejectTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupName = GetTestCaseParameters("GroupName");

	GotoMyInfo();
	
	SetNote("VerifyIncomingP2PCallReject");

	LogMessage("AudioTests :: VerifyIncomingP2PCallRejectTest :: Waiting for the audio call toast");
	AceptRejectCallToast(IgnoreString);	
	
	GotoContacts();
	OpenContactCardFromContactList(groupName,buddyContact);
	WaitForBuddyNoteChange("CallRejectedRemotely");

}

function VerifyOutgoingP2PCallRejectTest()
{
	LogMessage("AudioTests :: VerifyOutgoingP2PCallRejectTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupName = GetTestCaseParameters("GroupName");

	GotoMyInfo();
	
	SetNote("VerifyOutgoingP2PCallReject");
	
	GotoContacts();
	OpenContactCardFromContactList(groupName,buddyContact);
	TapCallbutton();
	
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	EndConversation();
	
	GotoContacts();
	OpenContactCardFromContactList(groupName,buddyContact);
	WaitForBuddyNoteChange("CallRejectedLocally");
}


function VerifyOutgoingP2PCallHoldTest()
{
	LogMessage("AudioTests :: VerifyOutgoingP2PCallHoldTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifyOutgoingP2PCallHold");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	GotoModalityView(VOICE_MODALITY_BUTTON);
	WaitforAudioWindow();
	TapOnAudioControl("cc hold");
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToSend = selfDisplayName + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact + " - " + holdMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	EndConversation();
}

function VerifyCallProgressBarTest()
{
	LogMessage("AudioTests :: VerifyCallProgressBarTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifyCallProgressBar");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	EndConversation();
}

function VerifyAddCallInP2PImTest()
{
	LogMessage("AudioTests :: VerifyAddCallInP2PImTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifyAddCallInP2PIm");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapIMbutton();
	WaitforConversationWindow();
	
	var messageToSend = selfDisplayName + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName, messageToSend);
	var messageToRecieve = GetBuddyDisplayName(0) + " - " + replyMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	TapOnAudioCallInConversationWindow("Lync Call");
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	EndConversation();
}

function VerifyEndOutgoingP2PCallInHoldTest()
{
	LogMessage("AudioTests :: VerifyEndOutgoingP2PCallInHoldTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifyEndOutgoingP2PCallInHold");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	GotoModalityView(VOICE_MODALITY_BUTTON);
	WaitforAudioWindow();
	TapOnAudioControl("cc hold");
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToSend = selfDisplayName + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact + " - " + holdMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	GotoModalityView(VOICE_MODALITY_BUTTON);
	WaitforAudioWindow();
	TapOnAudioControl("cc call end");
	
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	WaitForBuddyNoteChange("CallEndedRemotly");
}

function VerifyActiveOutgoingP2PCallHoldResumeTests()
{
	LogMessage("AudioTests :: VerifyActiveOutgoingP2PCallHoldResumeTests");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifyActiveOutgoingP2PCallHoldResume");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	GotoModalityView(VOICE_MODALITY_BUTTON);
	WaitforAudioWindow();
	TapOnAudioControl("cc hold");
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToSend = selfDisplayName + " - " + firstMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact + " - " + holdMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	//Unhold
	GotoModalityView(VOICE_MODALITY_BUTTON);
	WaitforAudioWindow();
	TapOnAudioControl("cc hold");
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToSend = selfDisplayName + " - " + secondMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact + " - " + UnholdMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	EndConversation();
}


function VerifyPassiveOutgoingP2PCallHoldResumeTests()
{
	LogMessage("AudioTests :: VerifyPassiveOutgoingP2PCallHoldResumeTests");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifyPassiveOutgoingP2PCallHoldResume");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	var messageToSend = selfDisplayName + " - " + firstMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact + " - " + holdMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	VerifyCallOnHold();
	
	var messageToSend = selfDisplayName + " - " + secondMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	//Wait for Audio window to come up
	DelayInSec(5);
	WaitforAudioWindow();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
		
	var messageToRecieve = buddyContact + " - " + UnholdMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	VerifyCallNotOnHold();
	
	EndConversation();
}

function VerifySecondOutgoingVoIPCallTests()
{
	LogMessage("AudioTests :: VerifySecondOutgoingVoIPCallTests");
	
	var buddyContact = GetBuddyDisplayName(0);
	var buddyContact2 = GetBuddyDisplayName(1);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifySecondOutgoingVoIPCall");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact2);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact2 + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact2, messageToRecieve);
	
	EndAllConversations();
}

function VerifySecondIncomingVoIPCallTests()
{
	LogMessage("AudioTests :: VerifySecondIncomingVoIPCallTests");
	
	var buddyContact = GetBuddyDisplayName(0);
	var buddyContact2 = GetBuddyDisplayName(1);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifySecondIncomingVoIPCall");

	LogMessage("AudioTests :: VerifySecondIncomingVoIPCallTests :: Waiting for the audio toast");
	AceptRejectCallToast(AcceptString);

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	var messageToSend = selfDisplayName + " - " + firstMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	LogMessage("AudioTests :: VerifySecondIncomingVoIPCallTests :: Waiting for the audio toast");
	AceptRejectCallToast(AcceptString);

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact2 + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact2, messageToRecieve);
	
	EndAllConversations();
}

function VerifyRejectSecondIncomingVoIPCallTests()
{
	LogMessage("AudioTests :: VerifyRejectSecondIncomingVoIPCallTests");
	
	var buddyContact = GetBuddyDisplayName(0);
	var buddyContact2 = GetBuddyDisplayName(1);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifyRejectSecondIncomingVoIPCall");

	LogMessage("AudioTests :: VerifyRejectSecondIncomingVoIPCallTests :: Waiting for the audio toast");
	AceptRejectCallToast(AcceptString);

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	var messageToSend = selfDisplayName + " - " + firstMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	LogMessage("AudioTests :: VerifyRejectSecondIncomingVoIPCallTests :: Waiting for the audio toast");
	AceptRejectCallToast(IgnoreString);
	
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact2);
	WaitForBuddyNoteChange("CallRejectedLocally");
	
	GotoChats();
	EndAllConversations();
}

function VerifySwitchingIncomingVoIPCallsTests()
{
	LogMessage("AudioTests :: VerifySwitchingIncomingVoIPCallsTests");
	
	var buddyContact = GetBuddyDisplayName(0);
	var buddyContact2 = GetBuddyDisplayName(1);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifySwitchingIncomingVoIPCalls");

	LogMessage("AudioTests :: VerifySwitchingIncomingVoIPCallsTests :: Waiting for the audio toast");
	AceptRejectCallToast(AcceptString);

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	var messageToSend = selfDisplayName + " - " + firstMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	LogMessage("AudioTests :: VerifySwitchingIncomingVoIPCallsTests :: Waiting for the audio toast");
	AceptRejectCallToast(AcceptString);

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact2 + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact2, messageToRecieve);
	
	TapOnConversationInList(1);
	GotoModalityView(VOICE_MODALITY_BUTTON);
	WaitforAudioWindow();
	TapOnAudioControl("cc hold");
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToSend = selfDisplayName + " - " + secondMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact + " - " + UnholdMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	TapOnConversationInList(0);
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToSend = selfDisplayName + " - " + secondMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact2 + " - " + holdMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact2, messageToRecieve);
	
	EndAllConversations();
}

function VerifyBadgingAndMissedTagForMissedConfAndAudioInvite()
{
	LogMessage("AudioTests :: VerifyBadgingAndMissedTagForMissedConfAndAudioInvite");
	SetNote("VerifyBadgingAndMissedTagForMissedConfAndAudioInvite");
	
	WaitForCallInviteToExpire();
	VerifyMissedConversationBadging(1);
	
	SetNote("VoIP Call");
	WaitForCallInviteToExpire();
	VerifyMissedConversationBadging(2);
	
	GotoChats();
	DelayInSec(1);
	VerifyMissedTagInConversation(0,"Audio");
	VerifyMissedTagInConversation(1,"Conf");  
}

function VerifyContinuedConversationViaCallTests()
{
	LogMessage("AudioTests :: VerifyContinuedConversationViaCallTests");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	
	SetNote("VerifyContinuedConversationViaCall");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	WaitForBuddyNoteChange("CallConnected");
	
	GotoChats();
	TapOnConversationInList(0);
	EndConversation();
	
	TapOnConversationInList(0);
	TapOnAudioCallInConversationWindow("Lync Call");
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	EndConversation();
	
}

function VerifyCallTerminationBeforeConnectsTests()
{
	LogMessage("AudioTests :: VerifyCallTerminationBeforeConnectsTests");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");

	GotoMyInfo();
	
	SetNote("VerifyCallTerminationBeforeConnects");

	LogMessage("AudioTests :: VerifyCallTerminationBeforeConnectsTests :: Waiting for the audio toast");
	AceptRejectCallToast(AcceptString);
		
	WaitforAudioWindow();
	EndConversation();
	
	GotoContacts();
	OpenContactCardFromContactList(groupName,buddyContact);
	WaitForBuddyNoteChange("CallRejectedRemotely");
}

function VerifyActiveOnHoldCallTerminationTests()
{
	LogMessage("AudioTests :: VerifyOnHoldCallTerminationTests");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifyActiveOnHoldCallTermination");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	GotoModalityView(VOICE_MODALITY_BUTTON);
	WaitforAudioWindow();
	TapOnAudioControl("cc hold");
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToSend = selfDisplayName + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact + " - " + holdMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	GotoModalityView(VOICE_MODALITY_BUTTON);
	WaitforAudioWindow();
	TapOnAudioControl("cc call end");
	
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	WaitForBuddyNoteChange("CallEndedRemotly");
}

function VerifyPassiveOnHoldCallTerminationTests()
{
	LogMessage("AudioTests :: VerifyPassiveOnHoldCallTerminationTests");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifyPassiveOnHoldCallTermination");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	var messageToSend = selfDisplayName + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact + " - " + holdMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	var messageToSend = selfDisplayName + " - " + EndCall;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	WaitForBuddyNoteChange("CallEndedRemotly");
}
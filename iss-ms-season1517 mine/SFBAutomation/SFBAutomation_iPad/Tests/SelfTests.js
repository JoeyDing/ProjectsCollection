function VerifySelfPresenceChangeTest()
{
	LogMessage("SelfTest :: VerifySelfPresenceChangeTest");
	
	SetNote("VerifySelfPresenceChange");
	VerifySelfPresence(AVAILABLE,"Icon_gbl_pres_available_48.png");
	ChangeSelfPresence(AVAILABLE,BUSY);
	VerifySelfPresence(BUSY,"Icon_gbl_pres_busy_48.png");
	VerifySelfPresence(AVAILABLE,"Icon_gbl_pres_available_48.png");
}

function VerifySelfNoteChangeTest()
{	
	LogMessage("SelfTest :: VerifySelfNoteChangeTest");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	var buddyContact = GetBuddyDisplayName(0);
	var groupName = GetTestCaseParameters("GroupName");
	
	SetNote("VerifySelfNoteChange");
	DelayInSec(2);
	SetNote(selfDisplayName+" - Personal Note");
	
	GotoContacts();
	OpenContactCardFromContactList(groupName,buddyContact);
	WaitForBuddyNoteChange(selfDisplayName + " - Note Changed");
}

function VerifyActivityStringInAConferenceCall()
{
    LogMessage("SelfTest :: VerifyActivityStringInAConferenceCall");
    
    GotoMyInfo();
    VerifySelfPresence("Available","Icon_gbl_pres_available_48.png");
    SetNote("VerifyActivityStringInAConferenceCall");
    
    AceptRejectCallToast(AcceptString);	
    WaitForCallToGetConnected();
   
    GotoModalityView("icon conversation button im");
    WaitforConversationWindow(); 
    messageToRecieve = GetBuddyDisplayName(0) + " - " + "In a conference call";
    messageToRecieve = messageToRecieve.split(" ").join("  ");
    VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
   
    EndConversation();
}

function VerifyActivityStringInACall()
{
    LogMessage("SelfTest :: VerifyActivityStringInAConferenceCall");
    var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
    
    GotoMyInfo();
    VerifySelfPresence("Available","Icon_gbl_pres_available_48.png");
    SetNote("VerifyActivityStringInACall");
    
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapCallbutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
   
    GotoModalityView("icon conversation button im");
    WaitforConversationWindow(); 
    messageToRecieve = GetBuddyDisplayName(0)+ " - " + "In a call";
    messageToRecieve = messageToRecieve.split(" ").join("  ");
    VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
   
    EndConversation();
}
function VerifyImFromContactCardTest()
{	
	LogMessage("IMTests :: VerifyImFromContactCardTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapIMbutton();
	WaitforConversationWindow();
}

function VerifyNoMissedConvInPassiveImConfEscalationMpopAcceptTest()
{
	LogMessage("IMTests :: VerifyNoMissedConvInPassiveImConfEscalationMpopAcceptTest");
	
	SetNote("VerifyNoMissedConvInPassiveImConfEscalationMpopAccept");
	WaitForIMInviteToExpire();
	DelayInSec(5);
	VerifyMissedConversationBadging(0);
	GotoChats();
	VerifyEmptyConversationList();
}

function VerifyOutgoingP2PImTest()
{
	LogMessage("IMTests :: VerifyOutgoingP2PImTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	SetNote("VerifyOutgoingP2PIm");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapIMbutton();
	WaitforConversationWindow();
	
	var bubbleCount = 0;
	var messageToSend = GetTestCaseParameters("DisplayName1") + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ");
	VerifyReceivedIM(GetTestCaseParameters("DisplayName1"), messageToSend);
	var messageToRecieve = GetBuddyDisplayName(0) + " - " + replyMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	EndConversation();
}

function VerifyIncomingP2PImTest()
{
	LogMessage("IMTests :: VerifyIncomingP2PImTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	
	SetNote("VerifyIncomingP2PIm");
	var bubbleCount = 0
	
	TapOnIMInvite(buddyContact);
	WaitforConversationWindow();
	DelayInSec(1);
	
	var messageToRecieve = GetBuddyDisplayName(0) + " - " + testMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	var messageToSend = GetTestCaseParameters("DisplayName1") + " - " + replyMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(GetTestCaseParameters("DisplayName1"), messageToSend);
	
	messageToRecieve = GetBuddyDisplayName(0) + " - " + replyMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	EndConversation();
}


function VerifyIncomingImConferenceTest()
{
	LogMessage("IMTests :: VerifyIncomingImConferenceTest");
	
	var buddy1 = GetBuddyDisplayName(0);
	var buddy2 = GetBuddyDisplayName(1);
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	var selfConversationJoinMessage = selfDisplayName + conversationJoinedMessage;
	var buddy1ConversationJoinMessage = buddy1 + conversationJoinedMessage;
	var buddy2ConversationJoinMessage = buddy2 + conversationJoinedMessage;
	
	LogMessage(selfDisplayName + " " + buddy1 + " " + buddy2);
	
	SetNote("VerifyIncomingImConference");

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
	
	EndConversation();
}

function VerifyOutgoingImConferenceTest()
{
	LogMessage("IMTests :: VerifyOutgoingImConferenceTest");
	
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	var buddy1 = GetBuddyDisplayName(0);
	var buddy2 = GetBuddyDisplayName(1);
	
	LogMessage("IMTests :: VerifyOutgoingImConference " + selfDisplayName + " " + buddy1 + " " + buddy2);
	
	SetNote("VerifyOutgoingImConference");

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
	
	EndConversation();
}

function VerifyDeleteConversationFromHistoryTest()
{
	LogMessage("IMTests :: VerifyDeleteConversationFromHistory");
	
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	var buddy1 = GetBuddyDisplayName(0);
	
	SetNote("VerifyDeleteConversationFromHistory");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddy1);
	TapIMbutton();
	WaitforConversationWindow();
	
	var messageToSend = selfDisplayName + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName, messageToSend);
	var messageToRecieve = buddy1 + " - " + replyMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddy1, messageToRecieve);
	
	EndConversation();
	
	DeleteAllConversations();
	
	VerifyEmptyConversationList();
}

function VerifyContinuedConversationTest()
{
	LogMessage("IMTests :: VerifyContinuedConversation");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	SetNote("VerifyContinuedIMConversation");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapIMbutton();
	WaitforConversationWindow();
	
	var messageToSend = GetTestCaseParameters("DisplayName1") + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(GetTestCaseParameters("DisplayName1"), messageToSend);
	var messageToRecieve = GetBuddyDisplayName(0) + " - " + replyMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	EndConversation();
	
	LogMessage("ConferenceTests :: VerifyConferenceRejoinTest :: Tap on Conversation " + index+1 + " in list");
	var index = 0;
	TapOnConversationInList(index);
	
	var messageToSend = GetTestCaseParameters("DisplayName1") + " - " + testMessage + " 2";
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(GetTestCaseParameters("DisplayName1"), messageToSend);
	var messageToRecieve = GetBuddyDisplayName(0) + " - " + replyMessage + " 2";
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	EndConversation();

}

function VerifyNewOutgoingIMConversationFromContactCardTest()
{
	LogMessage("IMTests :: VerifyNewOutgoingIMConversationFromContactCardTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	
	SetNote("VerifyNewOutgoingIMConversationFromContactCard");
	
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapIMbutton();
	WaitforConversationWindow();
	
	var messageToSend = GetTestCaseParameters("DisplayName1") + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(GetTestCaseParameters("DisplayName1"), messageToSend);
	
	var messageToRecieve = GetBuddyDisplayName(0) + " - " + replyMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapIMbutton();
	WaitforConversationWindow();
	
	var messageToSend = GetTestCaseParameters("DisplayName1") + " - " + testMessage + " 2";
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(GetTestCaseParameters("DisplayName1"), messageToSend);
	
	var messageToRecieve = GetBuddyDisplayName(0) + " - " + replyMessage + " 2";
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	var iConversations = ConversationsInList();
	if(iConversations != 2)
		throw new Error("Actual Conversations : " + iConversations + " Desired : 2");
	
	EndConversation();	
}

function VerifyBadgingForMissedIMInExistingConversation()
{
	LogMessage("IMTests :: BadgingForMissedIMInExistingConversation");
	
	var buddyContact = GetBuddyDisplayName(0);
	SetNote("VerifyBadgingForMissedIMInExistingConversation");
	
	TapOnIMInvite(buddyContact);
	WaitforConversationWindow();
	DelayInSec(1);
	
	var messageToRecieve = GetBuddyDisplayName(0) + " - " + testMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(GetBuddyDisplayName(0), messageToRecieve);
	
	GotoMyInfo();
	SetNote("ResendIMMessage");
	
	VerifyMissedConversationBadging(1);
    
	GotoChats();
	TapOnConversationInList(0);
	WaitforConversationWindow();
	EndConversation();   
}

function VerifyDecrementInBadgingOnTappingMissedChatsAndLeavingChatsPane()
{
    LogMessage("IMTests :: VerifyDecrementInBadgingOnTappingMissedChatsAndLeavingChatsPane");
    
    SetNote("VerifyDecrementInBadgingOnTappingMissedChatsAndLeavingChatsPane");
    DismissIMInvite();
    
    SetNote("ResendIMMessage");
    DismissIMInvite();
    
    VerifyMissedConversationBadging(2);
    
    //Tap on a missed chat and verify that badging value has reduced by 1
	GotoChats();
	TapOnConversationInList(0);
    VerifyMissedConversationBadging(1);
    
    //Leave chat pane and verify that badging is removed 
    GotoMyInfo();
    VerifyMissedConversationBadging(0);
}

function VerifyBadgingAndMissedTagForMissedIMInvite()
{
    LogMessage("IMTests :: VerifyBadgingAndMissedTagForMissedIMInvite");
    
    SetNote("VerifyBadgingAndMissedTagForMissedIMInvite");
    WaitForIMInviteToExpire();
    
    //Verify Badging Value
    VerifyMissedConversationBadging(1);
    
    //Verify Missed Tag 
    GotoChats();
    VerifyMissedTagInConversation(0,"IM");
}

function VerifyInstantMessageTest()
{
    if(IsSignInScreenUp() == 1){
        SignInAuto();
    }
    ValidateTabBarButtons();
    ConversationTest();
}



function ConversationTest()
{

    var contactsButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Contacts")];
    if(ElementValidAndVisible(contactsButton) == true){
        
        TapElement(contactsButton);
        var string = GetValueFromKey("LOCID Collapsed");
        var groupString = "New Group";
        var replaceString = string.replace("%@",groupString);
        var groupLabel = MainWindow.tableViews()[0].groups()[replaceString];
        if(ElementValidAndVisible(groupLabel) == true) {
            ExpandGroupFromContactList(groupString);
            string = "v-ninli@microsoft.com" + ", " + GetValueFromKey("LOCID Presence unknown");
            var waitCell = WaitForBuddyBecomeVisible(string);
            if(waitCell.isVisible() == true){
                
                TapElement(waitCell);
                var imButton = MainWindow.tableViews()[0].elements()[9];
                if(imButton.isVisible() == true && imButton.isEnabled() == true) {
                    
                    TapElement(imButton);
                    var tableView = MainWindow.scrollViews()[0].tableViews()[0];
                    TapTableviewGroup(tableView,0.43,0.50);
                    captureLocalizedScreenshot("383335");
                    captureLocalizedScreenshot("383349");
                    MainWindow.scrollViews()[0].textFields()[0].setValue(SELFDISPLAYNAME);
                    TapNavigationRightButton(0.5);
                    captureLocalizedScreenshot("383339");
                    captureLocalizedScreenshot("383340");
                    captureLocalizedScreenshot("406874");
                    
                    var mailButton = Target.frontMostApp().actionSheet().elements()[4];
                    TapElement(mailButton);
                    captureLocalizedScreenshot("408629");
                    GoToOrientation(DEVICELEFT);
                    captureLocalizedScreenshot("410090");
                    GoToOrientation(DEVICEPROT);
                    TapNavigationBackButton();
                    captureLocalizedScreenshot("408630");
                    TapActionSheetCancelButton();
                    TapNavigationRightButton();
                    captureLocalizedScreenshot("410333");
                    var dialButton = Target.frontMostApp().navigationBar().elements()[4];
                    TapElement(dialButton);
                    string = GetValueFromKey("LOCID Disabled");
                    replaceString = string.replace("%@",GetValueFromKey("LOCID Hold Call"));
                    SetElementScrollToVisible(MainWindow.scrollViews()[0],replaceString);
                    dialButton = MainWindow.scrollViews()[0].buttons()[GetValueFromKey("LOCID Dial Pad Only")];
                    TapElement(dialButton);
                    captureLocalizedScreenshot("451013");
                    TapNavigationBackButton();
                    captureLocalizedScreenshot("73291");
                    captureLocalizedScreenshot("383350");
                    captureLocalizedScreenshot("410361");
                    TapNavigationRightButton();
                    captureLocalizedScreenshot("383337");
                    var toolButton =  Target.frontMostApp().toolbar().buttons()[0];
                    TapElement(toolButton);
                    captureLocalizedScreenshot("383338");
                    toolButton = Target.frontMostApp().toolbar().buttons()[1];
                    TapElement(toolButton);
                    captureLocalizedScreenshot("383351");
                }else {
                    
                    LogMessage("383335 383349 383339 383340 406874 408629 410090 408630 410333 451013 73291 383350 410361 383337 383338 383351 Fail");
                    
                }
                TapElement(contactsButton);
                TapNavigationBackButton();
                
            }else{
                LogMessage("383335 383349 383339 383340 406874 408629 410090 408630 410333 451013 73291 383350 410361 383337 383338 383351 Fail");
            }
            CollapseGroupFromContactList(groupString);
            
        }
        else{
            LogMessage("383335 383349 383339 383340 406874 408629 410090 408630 410333 451013 73291 383350 410361 383337 383338 383351 Fail");
        }
    }else {
        LogMessage("383335 383349 383339 383340 406874 408629 410090 408630 410333 451013 73291 383350 410361 383337 383338 383351 Fail");
    }
    
    var meetingsButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Meetings")];
    if(ElementValidAndVisible(meetingsButton) == true){
        
        TapElement(meetingsButton);
        if(TapViewElement(0,7) == true){
        
            if(TapTableviewCell(0,GetValueFromKey("LOCID Join Meeting")) == true){
                
                var dismissButton = MainWindow.buttons()[GetValueFromKey("LOCID Dismiss")];
                if(ElementValidAndVisible(dismissButton) == true){
                    DismissNotificationsIfAny();
                    captureLocalizedScreenshot("398430");
                    TapNavigationBackButton();
                    DeleteAllConversations();
                }
                
            }
            else{
                LogMessage("398430 Fail");
            }
            GotoMeetings();
            TapNavigationBackButton();
            
        }else {
            LogMessage("398430 Fail");
        }
        
    }else {
       LogMessage("398430 Fail");
    }

}

function VerifyConversationPageDisplayWell(){
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("383335 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            
        }
    }
    
    GotoContacts();
    DelayInSec(5);
    DismissNotificationsIfAny();
    DelayInSec(10);
    var groupString = GetValueFromKey("LOCID Pinned Contacts");
    ExpandGroupFromContactList(groupString,0.03,0.30);
    
    if(TapTableviewCellWithPredicate(BUDDYDISPLAYNAME) == true){
      
        DelayInSec(2);
        var imButton = MainWindow.tableViews()[0].elements()[9];
        if(imButton.isVisible() == true && imButton.isEnabled() == true) {
                
            TapElement(imButton);
            captureLocalizedScreenshot("417765");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("417765_h");
            GoToOrientation(DEVICEPROT);
            
            var tableView = MainWindow.scrollViews()[0].tableViews()[0];
            TapTableviewGroup(tableView,0.43,0.50);
            captureLocalizedScreenshot("383335");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("383335_h");
            GoToOrientation(DEVICEPROT);
            TapNavigationBackButton();
            
        }else{
                
            LogMessage("383335 Fail");
        }
            
        GotoContacts();
        TapNavigationBackButton();
    }else {
        
        LogMessage("383335 Fail");
    }
    
    CollapseGroupFromContactList(groupString,0.03,0.30);
    GotoMyInfo();
}

function VerifyChatsListDisplayWell(){

    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("73291 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    GotoContacts();
    DelayInSec(5);
    DismissNotificationsIfAny();
    var groupString = GetValueFromKey("LOCID Pinned Contacts");
    ExpandGroupFromContactList(groupString,0.03,0.30);
    
    if(TapTableviewCellWithPredicate(BUDDYDISPLAYNAME) == true){
        
        DelayInSec(2);
        var imButton = MainWindow.tableViews()[0].buttons()[1];
        if(imButton.isVisible() == true && imButton.isEnabled() == true) {
            
            TapElement(imButton);
            MainWindow.scrollViews()[0].textFields()[0].setValue(SELFDISPLAYNAME);
            DelayInSec(1);
            TapNavigationBackButton();
            captureLocalizedScreenshot("73291");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("73291_h");
            GoToOrientation(DEVICEPROT);
            
        }else{
            
            LogMessage("73291 Fail");
        }
        
        GotoContacts();
        TapNavigationBackButton();
    }else {
        
        LogMessage("73291 Fail");
    }
    
    CollapseGroupFromContactList(groupString,0.03,0.30);
    GotoMyInfo();
}

function VerifySendAndReceiveP2PIMTest() {
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            
            LogMessage("73219 Fail");
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
    DelayInSec(1);
    SetHappeningNote("VerifyOutgoingP2PIm");
    
    GotoContacts();
    
    var messageToSend = GetTestCaseParameters("DisplayName2") + " - " + testMessage;
    
    if(TapElementWithPredicate(BUDDYDISPLAYNAME) == true){
        
        DelayInSec(2);
        SendIM(messageToSend);
      
        DelayInSec(15);
        captureLocalizedScreenshot("244355");
        GoToOrientation(DEVICELEFT);
        captureLocalizedScreenshot("244355_h");
        GoToOrientation(DEVICEPROT);
        
        TapNavigationBackButton();
    }else {
        
        LogMessage("73219 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("");
}








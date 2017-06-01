function VerifyOutgoingVideoCallTest()
{
	LogMessage("VideoTests :: VerifyOutgoingVideoCallTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	
	SetNote("VerifyOutgoingVideoCall");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapVideobutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	DelayInSec(2);
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + videoConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	LogMessage("VideoTests :: VerifyVideoControlsTest :: Goto Video View");
	GotoModalityView(VOICE_MODALITY_BUTTON);
	CaptureVideoScreenshot("OutgoingVideoCall");
	
	EndConversation();
}

function VerifyIncomingVideoCallTest() {
    LogMessage("VideoTests :: VerifyIncomingVideoCallTest");

    var buddyContact = GetBuddyDisplayName(0);

    SetNote("VerifyIncomingVideoCall");

    LogMessage("VideoTests :: VerifyIncomingVideoCallTest :: Waiting for the Video Invite");
    AceptRejectCallToast(AcceptString);
    WaitforAudioWindow();
    WaitForCallToGetConnected();
    DelayInSec(2);

    TapVideoMenuButton();
    TapOnVideoMenuControl(CAMERA_ON_BUTTON);
    DelayInSec(2);
    CaptureVideoScreenshot("IncomingVideoCall");

    GotoModalityView(IM_MODALITY_BUTTON);
    WaitforConversationWindow();
    var messageToRecieve = buddyContact + " - " + videoConnectedInP2PCallMessage;
    messageToRecieve = messageToRecieve.split(" ").join("  ");
    VerifyReceivedIM(buddyContact, messageToRecieve);

    LogMessage("VideoTests :: VerifyIncomingVideoCallTest :: Goto Video View");
    GotoModalityView(VOICE_MODALITY_BUTTON);
    CaptureVideoScreenshot("IncomingVideoCall2");

    EndConversation();
}

function VerifyRejectIncomingVideoCallTest() {
    LogMessage("VideoTests :: VerifyRejectIncomingVideoCallTest");

    var buddyContact = GetBuddyDisplayName(0);
    var groupName = GetTestCaseParameters("GroupName");

    GotoMyInfo();
    SetNote("VerifyRejectIncomingVideoCall");

    LogMessage("VideoTests :: VerifyRejectIncomingVideoCallTest :: Waiting for the video invite");
    AceptRejectCallToast(IgnoreString);

    GotoContacts();
    OpenContactCardFromContactList(groupName, buddyContact);
    WaitForBuddyNoteChange("VideoCallRejectedRemotely");

}

function VerifyVideoControlsTest()
{
	LogMessage("VideoTests :: VerifyVideoControlsTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	
	SetNote("VerifyVideoControls");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapVideobutton();
	WaitforAudioWindow();
	WaitForCallToGetConnected();
	DelayInSec(2);
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + videoConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	LogMessage("VideoTests :: VerifyVideoControlsTest :: Goto Video View");
	GotoModalityView(VOICE_MODALITY_BUTTON);
	
	LogMessage("VideoTests :: VerifyVideoControlsTest :: Verify Video Controls");
	TapVideoMenuButton();
	VerifyVideoControls();
	CaptureVideoScreenshot("VerifyVideoControls");
	TapOnApplicationWindow();
	DelayInSec(2);
	EndConversation();
}

function VerifyEndP2PVideoCallTest()
{
	LogMessage("VideoTests :: VerifyOutgoingVideoCallTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	
	SetNote("VerifyEndP2PVideoCall");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapVideobutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	DelayInSec(2);
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + videoConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);

	LogMessage("VideoTests :: VerifyVideoControlsTest :: Goto Video View");
	GotoModalityView(VOICE_MODALITY_BUTTON);
	CaptureVideoScreenshot("EndP2PVideoCall");
	
	TapOnAudioControl("Hang Up");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	WaitForBuddyNoteChange("VideoCallEndedRemotely");
}

function VerifyActiveVideoPauseResumeTest()
{
	LogMessage("VideoTests :: VerifyActiveVideoPauseResumeTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifyActiveVideoPauseResume");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapVideobutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	DelayInSec(2);
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + videoConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);

	LogMessage("VideoTests :: VerifyActiveVideoPauseResumeTest :: Switch to Video View");
	GotoModalityView(VOICE_MODALITY_BUTTON);
	CaptureVideoScreenshot("BeforePauseVideo");
    TapVideoMenuButton();
    TapOnVideoMenuControl(CAMERA_OFF_BUTTON);
	DelayInSec(2);
	CaptureVideoScreenshot("AfterPauseVideo");
    LogMessage("VideoTests :: VerifyActiveVideoPauseResumeTest :: Switch to IM view");
    GotoModalityView(IM_MODALITY_BUTTON);

	var messageToSend = selfDisplayName + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact + " - " + pausedMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);

    LogMessage("VideoTests :: VerifyActiveVideoPauseResumeTest :: Switch to audio view");
    GotoModalityView(VOICE_MODALITY_BUTTON);
    TapVideoMenuButton();
    TapOnVideoMenuControl(CAMERA_ON_BUTTON);
    DelayInSec(2);
    LogMessage("VideoTests :: VerifyActiveVideoPauseResumeTest :: Switch to IM view");
    GotoModalityView(IM_MODALITY_BUTTON);
	var messageToSend = selfDisplayName + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact + " - " + resumedMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	LogMessage("VideoTests :: VerifyActiveVideoPauseResumeTest :: Goto Video View");
	GotoModalityView(VOICE_MODALITY_BUTTON);
	CaptureVideoScreenshot("AfterResumeVideo");
	
	EndConversation();
}

function VerifyActiveVideoHoldUnholdTest()
{
	LogMessage("VideoTests :: VerifyActiveVideoHoldUnholdTest");
	
	var buddyContact = GetBuddyDisplayName(0);
	var groupname = GetTestCaseParameters("GroupName");
	var selfDisplayName = GetTestCaseParameters("DisplayName1");
	
	SetNote("VerifyActiveVideoHoldUnhold");

	GotoContacts();
	OpenContactCardFromContactList(groupname,buddyContact);
	TapVideobutton();

	WaitforAudioWindow();
	WaitForCallToGetConnected();
	
	DelayInSec(2);
	GotoModalityView(IM_MODALITY_BUTTON);
	WaitforConversationWindow();
	
	var messageToRecieve = buddyContact + " - " + videoConnectedInP2PCallMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);

	LogMessage("VideoTests :: VerifyVideoControlsTest :: Switch to Video View");
	GotoModalityView(VOICE_MODALITY_BUTTON);
	CaptureVideoScreenshot("BeforeHoldVideo");
    TapOnAudioControl(HOLD_BUTTON);
	DelayInSec(2);
	CaptureVideoScreenshot("AfterHoldVideo");
    LogMessage("VideoTests :: VerifyActiveVideoHoldUnholdTest :: Switch to IM view");
    GotoModalityView(IM_MODALITY_BUTTON);

	var messageToSend = selfDisplayName + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact + " - " + holdMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);

    LogMessage("VideoTests :: VerifyActiveVideoHoldUnholdTest :: Switch to audio view");
    GotoModalityView(VOICE_MODALITY_BUTTON);
    TapOnAudioControl(HOLD_BUTTON);
    DelayInSec(2);
    LogMessage("VideoTests :: VerifyActiveVideoHoldUnholdTest :: Switch to IM view");
    GotoModalityView(IM_MODALITY_BUTTON);
	var messageToSend = selfDisplayName + " - " + testMessage;
	SendIM(messageToSend);
	messageToSend = messageToSend.split(" ").join("  ");
	VerifyReceivedIM(selfDisplayName,messageToSend);
	
	var messageToRecieve = buddyContact + " - " + UnholdMessage;
	messageToRecieve = messageToRecieve.split(" ").join("  ");
	VerifyReceivedIM(buddyContact, messageToRecieve);
	
	LogMessage("VideoTests :: VerifyActiveVideoHoldUnholdTest :: Goto Video View");
	GotoModalityView(VOICE_MODALITY_BUTTON);
	CaptureVideoScreenshot("AfterUnHoldVideo");
	
	EndConversation();
}

function VerifyStringConnectingOnVideoCallDisplayWell(){
    
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
    GotoContacts();

    if(TapTableviewCellWithPredicate(BUDDYDISPLAYNAME) == true){
            
        var videoButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Video")];
        DelayInSec(1);
        
        if(videoButton.isValid()==true && videoButton.isVisible() == true){
                
            videoButton.tap();
            
            captureLocalizedScreenshot("446461");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("446461_h");
            GoToOrientation(DEVICEPROT);
            
            DelayInSec(5);
            Target.frontMostApp().mainWindow().scrollViews()[0].tapWithOptions({tapOffset:{x:0.42, y:0.88}});
            var button = Target.frontMostApp().navigationBar().leftButton();
            if(button.isVisible()==true){
                
                button.tap();
                DelayInSec(1);
            }

            
        }
        
        
        DelayInSec(2);
        GotoContacts();
        TapNavigationBackButton();
            
    }else {
        
        LogMessage("446461 Fail");
    }
        
    CollapseGroupFromContactList(groupString,0.03,0.30);
}

function VerifyIncomingVideoCallPageDisplayWell(){
    
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
    
    GotoMyInfo();
    DelayInSec(2);
    SetHappeningNote("VerifyIncomingVideoCallPageDisplayWell");
        
    WaitforToastDissMissButton(GetValueFromKey("LOCID Dismiss Call"));
    
    if(MainWindow.buttons()[GetValueFromKey("LOCID Dismiss Call")].isVisible() == true){
            
        captureLocalizedScreenshotWithNoDisMiss("73303");
        DelayInSec(0.5);
        if(MainWindow.buttons()[GetValueFromKey("LOCID Dismiss Call")].isVisible() == true){
            
            DelayInSec(1);
            MainWindow.buttons()[GetValueFromKey("LOCID Dismiss Call")].tap();
            DelayInSec(1);
        }
            
    }else {
        LogMessage("73303 Fail");
    }
        
    GotoMyInfo();
    SetHappeningNote("");
}


function VerifyStringStartingVideoConferenceOnConversationPage()
{
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
    
    GotoMyInfo();
    SetHappeningNote("VerifyStringStartingVideoConferenceOnConversationPage");
    GotoContacts();
    var groupString = GetValueFromKey("LOCID Pinned Contacts");
    ExpandGroupFromContactList(groupString,0.03,0.30);
    DelayInSec(2);
    
    if(TapTableviewCellWithPredicate(BUDDYDISPLAYNAME) == true){
        
        var videoButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Video")];
    
        if(videoButton.isVisible() == true){
            
            TapElement(videoButton);
            DelayInSec(2);
        }else {
            
            LogMessage("456027 Fail");
            return;
        }
        
        
        MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.83, y:0.49}});
        if(WaitForNavigationButtonVisible(GetValueFromKey("LOCID Collaboration")) == true){
            
            DelayInSec(1.5);
            DismissNotificationsIfAny();
            MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.83, y:0.49}});
            captureLocalizedScreenshot("456027");
            
        }else {
            
            LogMessage("456027 Fail");
        }
        
        MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.83, y:0.49}});
        DelayInSec(0.5);
        TapNavigationRightButton();
        DelayInSec(1);
        TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        GotoContacts();
        TapNavigationBackButton();
        
    }else {
        
        LogMessage("456027 Fail");
    }
    
    CollapseGroupFromContactList(groupString,0.03,0.30);
    
    GotoMyInfo();
    SetHappeningNote("");
}


function VerifySendAndReceiveP2PVideoTest() {
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){

        SignInVideoConfigTest(SIPURI,PASSWORD);
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
    GotoContacts();
    
    if(TapElementWithPredicate(BUDDYDISPLAYNAME) == true){

        var videoButton = target.frontMostApp().navigationBar().buttons()[GetValueFromKey("LOCID ACCESSIBILITY_CONTACTS_VIDEOCALL_START")];
        DelayInSec(1);
        
        if(videoButton.isValid()==true && videoButton.isVisible() == true){
            
            videoButton.tap();
            DelayInSec(2);
            
            captureLocalizedScreenshot("244354_1");
            GoToOrientation(DEVICELEFT);
            DelayInSec(1)
            captureLocalizedScreenshot("244354_1_h");
            GoToOrientation(DEVICEPROT);
            
            var button = target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Hang Up")];
            
            if(button.isValid()==true && button.isVisible() == true){
                
                button.tap();
            }
        }
    
        TapNavigationBackButton();
        
    }else {
        
        LogMessage("244354_1 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyIncomingVideoCall");
    
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
   
    if(button.isVisible() == true && button.isValid() == true){
        
        captureLocalizedScreenshot("244354_2");
        GoToOrientation(DEVICELEFT);
        captureLocalizedScreenshot("244354_2_h");
        GoToOrientation(DEVICEPROT);
        
        button.tap();
        DelayInSec(1);
        
        var button = target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Hang Up")];
        
        if(button.isValid()==true && button.isVisible() == true){
            
            button.tap();
        }
        
        TapNavigationBackButton();
        
    }else {
        
        LogMessage("244354_2 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("");
    
}




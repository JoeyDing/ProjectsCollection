function VerifyStringConnectingOnAudioCallDisplayWell() {
    
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
    var groupString = GetValueFromKey("LOCID Pinned Contacts");
    ExpandGroupFromContactList(groupString,0.03,0.30);
        
    if(TapTableviewCellWithPredicate(BUDDYDISPLAYNAME) == true){
            
        var audioButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Call")];
        
        if(audioButton.isValid()==true && audioButton.isVisible() == true){
            
            TapElement(audioButton);
            DelayInSec(2);
            captureLocalizedScreenshot("445952");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("445952_h");
            GoToOrientation(DEVICEPROT);
            
            MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.42, y:0.88}});
            var button = target.frontMostApp().navigationBar().leftButton();
            if(button.isVisible()==true){
                
                button.tap();
                DelayInSec(1);
            }

            
        }else {
            
            audioButton = MainWindow.tableViews()[0].buttons()[2];
            if(audioButton.isValid()==true && audioButton.isVisible() == true){
                
                TapElement(audioButton);
                DelayInSec(2);
                
                captureLocalizedScreenshot("445952");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("445952_h");
                GoToOrientation(DEVICEPROT);
                
            }else {
                
                LogMessage("445952 Fail");
                return;
            }
            
            MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.42, y:0.88}});
            var button = target.frontMostApp().navigationBar().leftButton();
            if(button.isVisible()==true){
                
                button.tap();
                DelayInSec(1);
            }
        }
        
        DelayInSec(1);
        GotoContacts();
        TapNavigationBackButton();
            
    }else {
            
        LogMessage("445952 Fail");
    }
        
    CollapseGroupFromContactList(groupString,0.03,0.30);
}

function VerifyIncomingAudioCallPageDisplayWell() {
    
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
    DelayInSec(1);
    SetHappeningNote("VerifyIncomingAudioCallPageDisplayWell");
        
    WaitforToastDissMissButton(GetValueFromKey("LOCID Dismiss Call"));
    if(MainWindow.buttons()[GetValueFromKey("LOCID Dismiss Call")].isVisible() == true){
            
        captureLocalizedScreenshotWithNoDisMiss("73301");
        DelayInSec(0.5);
        if(MainWindow.buttons()[GetValueFromKey("LOCID Dismiss Call")].isVisible() == true){
            
            DelayInSec(1);
            MainWindow.buttons()[GetValueFromKey("LOCID Dismiss Call")].tap();
            DelayInSec(1);
        }
            
    }else {
        
        LogMessage("73301 Fail");
    }
        
    GotoMyInfo();
    SetHappeningNote("");
}


function VerifyTransferOtherBuddyConferenceCallTest(){
    
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
    SetHappeningNote("VerifyTransferOtherBuddyConferenceCallTest");
    
    var target = UIATarget.localTarget();
    WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
    var button = MainWindow.buttons()[GetValueFromKey("LOCID Answer Call")];
    if(button.isVisible() == true){
        
        button.tap();
        target.frontMostApp().mainWindow().scrollViews()[0].tapWithOptions({tapOffset:{x:0.42, y:0.88}});
        DelayInSec(8);
        target.frontMostApp().mainWindow().scrollViews()[0].tapWithOptions({tapOffset:{x:0.42, y:0.88}});
        DelayInSec(2);
        TapNavigationRightButton();
            
        var actionSheet = UIATarget.localTarget().frontMostApp().actionSheet();
        if(actionSheet.isValid()==true && actionSheet.isVisible()==true){
                
            var button = actionSheet.buttons()[GetValueFromKey("LOCID Transfer")];
            if(button.isValid()==true && button.isVisible()==true){
                    
                captureLocalizedScreenshot("456110");
                TapActionSheetButton(GetValueFromKey("LOCID Cancel"));
                target.frontMostApp().mainWindow().scrollViews()[0].tapWithOptions({tapOffset:{x:0.42, y:0.88}});
                DelayInSec(2);
                target.frontMostApp().mainWindow().scrollViews()[0].tapWithOptions({tapOffset:{x:0.42, y:0.88}});

                GoToOrientation(DEVICELEFT);
                TapNavigationRightButton();
                
                actionSheet = UIATarget.localTarget().frontMostApp().actionSheet();
                if(actionSheet.isValid()==true && actionSheet.isVisible()==true){
                        
                    var button = actionSheet.buttons()[GetValueFromKey("LOCID Transfer")];
                    if(button.isValid()==true && button.isVisible()==true){
                        
                        captureLocalizedScreenshot("456110_h");
                        
                        
                    }
                }
                TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
                GoToOrientation(DEVICEPROT);

                return;
            }else {
                
                target.frontMostApp().mainWindow().scrollViews()[0].tapWithOptions({tapOffset:{x:0.42, y:0.88}});

                TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
                LogMessage("456110 Fail");
                return;
            }
        }
            
        button = UIATarget.localTarget().frontMostApp().actionSheet().buttons()[GetValueFromKey("LOCID End Conversation")];
        if(button.isVisible() == true)
        {
            button.tap();
            DelayInSec(1);

        }else {
            
            target.frontMostApp().mainWindow().scrollViews()[0].tapWithOptions({tapOffset:{x:0.42, y:0.88}});
            TapNavigationRightButton();
            TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        }
            
    }else {
        
        LogMessage("456110 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("");
    
}

function VerifyIncomingGroupConversationCallPageDisplayWell() {
    
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
    SetHappeningNote("VerifyIncomingGroupConversationCallPageDisplayWell");
        
    WaitforToastDissMissButton(GetValueFromKey("LOCID Dismiss Call"));
    if(MainWindow.buttons()[GetValueFromKey("LOCID Dismiss Call")].isVisible() == true){
            
        captureLocalizedScreenshotWithNoDisMiss("446124");
        DelayInSec(0.5);
        if(MainWindow.buttons()[GetValueFromKey("LOCID Dismiss Call")].isVisible() == true){
            
            MainWindow.buttons()[GetValueFromKey("LOCID Dismiss Call")].tap();
            DelayInSec(1);
        }
            
    }else {
        
        LogMessage("446124 Fail");
        LogMessage("457598 Fail");
    }
    
    GotoChats();
    if(TapTableviewCell(0,0) == true) {
        
        TapNavigationRightButton();
        DelayInSec(1);
        
        var actionSheet = UIATarget.localTarget().frontMostApp().actionSheet();
        if(actionSheet.isValid()==true && actionSheet.isVisible()==true){
            
            var button = actionSheet.buttons()[GetValueFromKey("LOCID View Participants")];
            if(button.isVisible() == true){
                
                button.tap();
                captureLocalizedScreenshot("457598");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("457598_h");
                GoToOrientation(DEVICEPROT);
                DelayInSec(1);
                TapNavigationBackButton();
            }else {
                
                TapActionSheetButton(GetValueFromKey("LOCID Cancel"));
                LogMessage("457598 Fail");
            }
            
        }else {
            
            LogMessage("457598 Fail");
        }
        
        DelayInSec(1);
        TapNavigationBackButton();
    }else {
        
        LogMessage("457598 Fail");
    }
        
    GotoMyInfo();
    SetHappeningNote("");
}


function VerifyStringNoVoiceMailOnVoicemailPage(){
        
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_2,PASSWORD_2);
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

    var continueButton = MainWindow.scrollViews()[0].buttons()[GetValueFromKey("LOCID Continue")];
    if(continueButton.isVisible() == true){
        
        continueButton.tap();
        DelayInSec(1);
    }

    
    Target.pushTimeout(10);
	var queryString = "name CONTAINS '" + GetValueFromKey("LOCID Phone") + "'";
	var phoneButton = UIATarget.localTarget().frontMostApp().tabBar().buttons().firstWithPredicate(queryString);
	Target.popTimeout();
    
    if(ElementValidAndVisible(phoneButton) == true){
        
        TapElement(phoneButton);
     
        var novoiceCell = Target.frontMostApp().mainWindow().tableViews()[0].cells()[GetValueFromKey("LOCID No Voicemails")];
        if(novoiceCell.isVisible() == true){
            
            captureLocalizedScreenshot("398204");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("398204_h");
            GoToOrientation(DEVICEPROT);
        }else{
            LogMessage("398204 Fail");
        }
    }
    else{
        LogMessage("398204 Fail");
    }

}


function VerifyStringStartingConfCallDisplayWellOnConversationPage()  //456026
{
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            
            LogMessage("456026 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            
        }
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyStringStartingConfCallDisplayWellOnConversationPage");
    
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
        
        MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.83, y:0.49}});
        if(WaitForNavigationButtonVisible(GetValueFromKey("LOCID Collaboration")) == true){
            
            MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.83, y:0.49}});
            DismissNotificationsIfAny();
            captureLocalizedScreenshot("456026");
            
        }else {
            
            LogMessage("456026 Fail");
        }
        
        TapNavigationRightButton();
        DelayInSec(1);
        TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        GotoContacts();
        TapNavigationBackButton();
        
    }else {
        
        LogMessage("456026 Fail");
    }
    
    
    CollapseGroupFromContactList(groupString,0.03,0.30)
    
    GotoMyInfo();
    SetHappeningNote("");
}

function VerifyButton1XDisplayWellOnVoiceMailPage()
{
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            
             LogMessage("457049 457050 457051 fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            
        }
    }
    
    GotoVoiceMail();
    var oneXButton = MainWindow.buttons()["Play the voicemail at regular speed"];
    var twoXButton = MainWindow.buttons()["Play the voicemail faster at twice the speed"];
    var halfXButton = MainWindow.buttons()["Play the voicemail slower at half the speed"];
    
    if(oneXButton.isValid()== true || twoXButton.isValid() == true || halfXButton.isValid() == true){
        if(oneXButton.isValid() == true){
            captureLocalizedScreenshot("457049");
            oneXButton.tap();
            DelayInSec(1);
            captureLocalizedScreenshot("457051");
            MainWindow.buttons()["Play the voicemail faster at twice the speed"].tap();
            DelayInSec(1);
            captureLocalizedScreenshot("457050");
        }
        
        if(twoXButton.isValid() == true){
            captureLocalizedScreenshot("457051");
            twoXButton.tap();
            DelayInSec(1);
            captureLocalizedScreenshot("457050");
            MainWindow.buttons()["Play the voicemail slower at half the speed"].tap();
            DelayInSec(1);
            captureLocalizedScreenshot("457049");
        }
        
        if(halfXButton.isValid() == true){
            captureLocalizedScreenshot("457050");
            halfXButton.tap();
            DelayInSec(1);
            captureLocalizedScreenshot("457049");
            MainWindow.buttons()["Play the voicemail at regular speed"].tap();
            DelayInSec(1);
            captureLocalizedScreenshot("457051");
        }
    }else {
        
        LogMessage("457049 457050 457051 fail");
    }

}


function VerifyStringTransferDisplayWellOnConversationPage()
{
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("456110 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
          
        }
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyStringTransferDisplayWellOnConversationPage");
    
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
    if(button.isVisible() == true){
        
        TapElement(button);
        DelayInSec(2);
        
        WaitForNavigationButtonEnable(GetValueFromKey("LOCID Video"));
        TapNavigationRightButton();
        DelayInSec(1);
        DismissNotificationsIfAny();
        captureLocalizedScreenshot("456110");
        TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        
    }else {
        
        LogMessage("456110 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("");
    
}

function VerifyTransferAlertMessageOnConversationPage()
{
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("457035 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            
        }
    }

    GotoMyInfo();
    SetHappeningNote("VerifyTransferAlertMessageOnConversationPage");
    
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
    if(button.isVisible() == true){
        
        TapElement(button);
    
        WaitForNavigationButtonEnable(GetValueFromKey("LOCID Video"));
        TapNavigationRightButton();
        DelayInSec(1);
        DismissNotificationsIfAny();
       
        TapActionSheetButton(GetValueFromKey("LOCID Transfer"));
        DelayInSec(2);
        if(TapTableviewCellWithPredicate(GetValueFromKey("LOCID Presence unknown")) == true){
            
            DelayInSec(1);
            var string = GetValueFromKey("LOCID Lync Call");
            var replaceString = string.replace("%@","Lync");
            TapActionSheetButton(replaceString);
            DismissNotificationsIfAny();
            
            string = GetValueFromKey("LOCID E_VoiceTransferParticipantError");
            var staticString = WaitForMainViewAlertMessageVisible(MainWindow,string);
            if(staticString.isVisible() == true){
                
                staticString.tapWithOptions({tapOffset:{x:0.49, y:0.30}});
                DelayInSec(1);
                captureLocalizedScreenshot("457035");
                var DismissButton = WaitforToastDissMissButton(GetValueFromKey("LOCID Dismiss"));
                TapElement(DismissButton);
                
            }else {
                
                LogMessage("457035 Fail");
            }
            
        }else {
            
            LogMessage("457035 Fail");
        }
        
        DelayInSec(2);
        UIATarget.localTarget().frontMostApp().navigationBar().rightButton().doubleTap();
        DelayInSec(1);
        TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        
    }else {
        
        LogMessage("457035 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("");
}


function VerifyTransferAlertStringDisplayOnConversationPage()
{
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("457034 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            
        }
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyTransferAlertStringDisplayOnConversationPage");
    
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
    if(button.isVisible() == true){
        
        TapElement(button);
        
        WaitForNavigationButtonEnable(GetValueFromKey("LOCID Video"));
        TapNavigationRightButton();
        DelayInSec(1);
        DismissNotificationsIfAny();
        
        TapActionSheetButton(GetValueFromKey("LOCID Transfer"));
        DelayInSec(1);
        
        var searchBar = MainWindow.tableViews()[0].searchBars()[0];
        searchBar.tap();
        DelayInSec(1);
        TypeString("wphone8");
        DelayInSec(2);
        
        if(TapMainTableviewCellWithPredicate("wphone8") == true){
            
            var string = GetValueFromKey("LOCID Lync Call");
            var replaceString = string.replace("%@","Lync");
            TapActionSheetButton(replaceString);
            DismissNotificationsIfAny();
            
            string = GetValueFromKey("LOCID E_VoiceTransferGenericError");
            var staticString = WaitForMainViewAlertMessageVisible(MainWindow,string);
            if(staticString.isVisible() == true){
                
                staticString.tapWithOptions({tapOffset:{x:0.49, y:0.30}});
                DelayInSec(1);
                captureLocalizedScreenshot("457034");
                var DismissButton = WaitforToastDissMissButton(GetValueFromKey("LOCID Dismiss"));
                TapElement(DismissButton);
                
            }else {
                
                LogMessage("457034 Fail");
            }
            
        }else {
            
            LogMessage("457034 Fail");
        }
        
        DelayInSec(3);
        UIATarget.localTarget().frontMostApp().navigationBar().rightButton().doubleTap();
        DelayInSec(1);
        TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        
    }else {
        
        LogMessage("457034 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("");
}


function VerifyPartyConferencePageDisplayWell()
{
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("455490 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
           
        }
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyPartyConferencePageDisplayWell");
    
    GotoContacts();
    var groupString = GetValueFromKey("LOCID Pinned Contacts");
    ExpandGroupFromContactList(groupString,0.03,0.30);
    
    if(TapTableviewCellWithPredicate(BUDDYDISPLAYNAME) == true){
        
        var audioButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Call")];
        
        TapElement(audioButton);
        DelayInSec(1);
    
        if(WaitForNavigationButtonEnable(GetValueFromKey("LOCID Video")) == true){
            
            MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.83, y:0.49}});
            DismissNotificationsIfAny();
            Target.frontMostApp().navigationBar().rightButton().doubleTap();
            DelayInSec(1);
            captureLocalizedScreenshot("455490");
            DelayInSec(1);
            TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
            
        }else {
            
            LogMessage("455490 Fail");
            Target.frontMostApp().navigationBar().rightButton().doubleTap();
            DelayInSec(1);
            TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        }
    
        GotoContacts();
        TapNavigationBackButton();
        
    }else {
        
        LogMessage("455490 Fail");
    }

    CollapseGroupFromContactList(groupString,0.03,0.30)
    GotoMyInfo();
    SetHappeningNote("");
}

function VerifyStringSeeParticipantsDisplayWellOnConversationPage()
{
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("456047 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            
        }
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyStringSeeParticipantsDisplayWellOnConversationPage");
    
    GotoContacts();
    var groupString = GetValueFromKey("LOCID Pinned Contacts");
    ExpandGroupFromContactList(groupString,0.03,0.30);
    
    if(TapTableviewCellWithPredicate(BUDDYDISPLAYNAME) == true){
        
        var audioButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Call")];
        
        TapElement(audioButton);
        DelayInSec(1);
        
        if(WaitForNavigationButtonEnable(GetValueFromKey("LOCID Video")) == true){
            
            MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.83, y:0.49}});
            DismissNotificationsIfAny();
            Target.frontMostApp().navigationBar().rightButton().doubleTap();
            DelayInSec(1);
            
            TapActionSheetButton(GetValueFromKey("LOCID Invite"));
            DelayInSec(1);
            
            var cancelButton = Target.frontMostApp().navigationBar().buttons()[GetValueFromKey("LOCID Cancel")];
            if(cancelButton.isVisible() == true){
                
                if(TapTableviewCellWithPredicate(GetValueFromKey("LOCID Presence unknown")) == true){
                    
                    DelayInSec(1);
                    var string = GetValueFromKey("LOCID Lync Call");
                    var replaceString = string.replace("%@","Lync");
                    TapActionSheetButton(replaceString);
                    DismissNotificationsIfAny();
                    
                    Target.frontMostApp().navigationBar().rightButton().doubleTap();
                    DelayInSec(1);
                    TapActionSheetButton(GetValueFromKey("LOCID View Participants"));
                    
                    if(TapTableviewCellWithPredicate(SELFDISPLAYNAME) == true){
                        
                        cancelButton = Target.frontMostApp().actionSheet().buttons()[GetValueFromKey("LOCID Cancel")];
                        if(cancelButton.isVisible() == true){
                            
                            captureLocalizedScreenshot("456047");
                            cancelButton.tap();
                            DelayInSec(1);
                        }
                    }else{
                        
                        LogMessage("456047 Fail");
                    }
                    
                    DelayInSec(1);
                    TapNavigationBackButton();
                    
                }else{
                    
                    cancelButton.doubleTap();
                    LogMessage("456047 Fail");
                }
                
            }else{
                
                LogMessage("456047 Fail");
            }
            
        }else {
            
            LogMessage("456047 Fail");
        }
        
        TapNavigationRightButton();
        DelayInSec(1);
        TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        GotoContacts();
        TapNavigationBackButton();
        
    }else {
        
        LogMessage("456047 Fail");
    }
    
    CollapseGroupFromContactList(groupString,0.03,0.30)
    GotoMyInfo();
    SetHappeningNote("");
    
}

function VerifyStringTransferToXXDisplayWellOnConversationPage()
{
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI_1,PASSWORD_1);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("456527 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER_1);
        }
        else {
            
        }
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyStringTransferToXXDisplayWellOnConversationPage");
    
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
    if(button.isVisible() == true){
        
        TapElement(button);
        
        WaitForNavigationButtonEnable(GetValueFromKey("LOCID Instant Messaging"));
        MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.83, y:0.49}});
        TapNavigationRightButton();
        DelayInSec(1);
        DismissNotificationsIfAny();
        
        TapActionSheetButton(GetValueFromKey("LOCID Transfer"));
        DelayInSec(2);
        if(TapTableviewCellWithPredicate(GetValueFromKey("LOCID Presence unknown")) == true){
            
            DelayInSec(1);
            var string = GetValueFromKey("LOCID Lync Call");
            var replaceString = string.replace("%@","Lync");
            TapActionSheetButton(replaceString);
            DismissNotificationsIfAny();
            
            MainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.83, y:0.49}});
            DelayInSec(1);
            captureLocalizedScreenshot("456527");
            
        }else {
            
            LogMessage("456527 Fail");
        }
        
        DelayInSec(1);
        UIATarget.localTarget().frontMostApp().navigationBar().rightButton().doubleTap();
        DelayInSec(1);
        TapActionSheetButton(GetValueFromKey("LOCID End Conversation"));
        
    }else {
        
        LogMessage("456527 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("");
}


function VerifySendAndReceiveP2PCallTest() {
    
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
    
    GotoContacts();
    var target = UIATarget.localTarget();
    
    if(TapElementWithPredicate(BUDDYDISPLAYNAME) == true){
        
        var audioButton = target.frontMostApp().navigationBar().buttons()[GetValueFromKey("LOCID ACCESSIBILITY_CONTACTS_AUDIOCALL_START")];
        
        var result = false;
        
        if(audioButton.isVisible() == true){
            
            TapElement(audioButton);
            DelayInSec(1);
            result = true;
        }else {
            
            audioButton = target.frontMostApp().navigationBar().buttons()[2];
            if(audioButton.isVisible() == true){
                
                TapElement(audioButton);
                DelayInSec(2);
                
                result = true;
            }
            
        }
        
        if(result == true) {
            
            DelayInSec(2);
            captureLocalizedScreenshot("244359_1");
        
            DelayInSec(1);
            var button = target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Hang Up")];
            
            if(button.isValid()==true && button.isVisible() == true){
                
                button.tap();
            }
        }
        
        TapNavigationBackButton();

    }else {
        
        LogMessage("244359_1 Fail");

    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyIncomingP2PCall");
    
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
    if(button.isVisible() == true){
        
        captureLocalizedScreenshot("244359_2");

        button.tap();
        DelayInSec(1);
 
        var button = target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Hang Up")];
        
        if(button.isValid()==true && button.isVisible() == true){
            
            button.tap();
        }
        
        TapNavigationBackButton();
    }else {
        
        LogMessage("244359_2 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("");
}




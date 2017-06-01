function VerifyApplicationTest()
{
	if(IsSignInScreenUp() == 1){
        SignInAuto();
    }
    
	ValidateTabBarButtons();
    GotoMyOptions();
    GotoMySettings();
    GotoMyVoicemail();
}

function GotoMyOptions()
{
    
    SignOutApp();
    SignInApp();
    DelayInSec(0.5);
    captureLocalizedScreenshot("403600");
    
    var myInfoButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        TapElement(myInfoButton);
        captureLocalizedScreenshot("382044");
        captureLocalizedScreenshot("381966");
        captureLocalizedScreenshot("73285");
        captureLocalizedScreenshot("73287");
    }
    else{
        LogMessage("382044 381966 73285 73287 Fail");
    }
    
    if(TapViewElement(0,9) == true){
        
        captureLocalizedScreenshot("382051_1");
        captureLocalizedScreenshot("73286");
        if(TapTableviewCell(0,GetValueFromKey("LOCID Reset Status")) == true) {
            
            captureLocalizedScreenshot("382051_2");
            captureLocalizedScreenshot("382046");
        }
        else{
            LogMessage("382051_2 382046 Fail");
        }
    }
    else{
        LogMessage("382051_1 382051_2 382046 73286 Fail");
    }
    DismissNotificationsIfAny();
    if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
        
        captureLocalizedScreenshot("398198");
        captureLocalizedScreenshot("398201");
        if(TapViewElement(0,1) == true) {
            TypeString("655588888888888888665659599659594946565895554555665544");
            captureLocalizedScreenshot("382027");
            TapNavigationBackButton();
        }
        else{
            LogMessage("382027 Fail");
        }
        SetElementScrollToVisible(MainWindow.tableViews()[0],GetValueFromKey("LOCID Exchange Credentials"));
        captureLocalizedScreenshot("403504");
        
        var string = GetValueFromKey("LOCID Use Lync Credentials");
        var replaceString = string.replace("%@","Lync");
        if(TapTableviewCell(0,GetValueFromKey("LOCID Exchange Credentials")) == true){
            if(SetCellSwitchValue(replaceString,0,1) == true){
                captureLocalizedScreenshot("382033");
                SetCellSwitchValue(replaceString,0,0);
                captureLocalizedScreenshot("382034");
                SetCellSwitchValue(replaceString,0,1);
                TapNavigationBackButton();
            }
            else {
                TapNavigationBackButton();
                LogMessage("382033 382034 Fail");
            }
        }
        else{
            LogMessage("382033 382034 Fail");
        }
        
        SetElementScrollToVisible(MainWindow.tableViews()[0],GetValueFromKey("LOCID HTTP Proxy Credentials"));
        if(TapTableviewCell(0,GetValueFromKey("LOCID HTTP Proxy Credentials")) == true){
            
            captureLocalizedScreenshot("382037");
            TapNavigationBackButton();
        }else {
            LogMessage("382037 Fail");
        }
        
        string = GetValueFromKey("LOCID Show Photos") + ", " + GetValueFromKey("LOCID On");
        if(SetCellSwitchValue(string,0,0) == true){
            replaceString = string.replace("On","Off");
            SetCellSwitchValue(replaceString,0,1);
            captureLocalizedScreenshot("398200");
        }
        else{
            LogMessage("398200 Fail");
        }
        
        MainWindow.tableViews()[0].scrollDown();
        DelayInSec(1);
        captureLocalizedScreenshot("398199");
        captureLocalizedScreenshot("445759");
        captureLocalizedScreenshot("448729");
    
        string = GetValueFromKey("LOCID Logging") + ", " + GetValueFromKey("LOCID On");
        if(TapTableviewCell(0,string) == true){
            
            captureLocalizedScreenshot("382038");
            if(SetCellSwitchValue(GetValueFromKey("LOCID Enable Logging"),0,0) == true) {
                captureLocalizedScreenshot("398088");
                SetCellSwitchValue(GetValueFromKey("LOCID Enable Logging"),0,1);
            }
            else{
                LogMessage("398088 Fail");
            }
            TapNavigationBackButton();
        }
        else{
            LogMessage("382038 398088 Fail");
        }

        if(TapViewElement(0,19) == true) {
            
            captureLocalizedScreenshot("382026");
            captureLocalizedScreenshot("382042");
            captureLocalizedScreenshot("73293");
            captureLocalizedScreenshot("445961");
            
            if(TapButton(GetValueFromKey("LOCID Third Party Notices")) == true) {
                captureLocalizedScreenshot("454015");
                TapNavigationBackButton();
            }
            else{
                LogMessage("454015 Fail");
            }
            
            TapNavigationBackButton();
        }
        else{
            LogMessage("382026 382042 73293 445961 454015 Fail");
        }
        
        if(TapTableviewCell(0,GetValueFromKey("LOCID online help")) == true){
            
            var linkLabel = MainWindow.scrollViews()[0].webViews()[0].links()["Get started"];
            WaitForObjectToBecomeVisible(linkLabel,"Help Link");
            captureLocalizedScreenshot("73289");
            captureLocalizedScreenshot("445920_1");
            MainWindow.scrollViews()[0].webViews()[0].scrollDown();
            DelayInSec(1);
            captureLocalizedScreenshot("445920_2");
            TapNavigationBackButton();
        }
        else{
            LogMessage("73289 445920_1 445920_2 Fail");
        }
        
        TapNavigationBackButton();
    }
}

function GotoMyVoicemail() {
    
    DismissNotificationsIfAny();
    
    if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
        var string = GetValueFromKey("LOCID WiFi VoIP") + ", " + GetValueFromKey("LOCID Off");
        if(SetCellSwitchValue(string,0,1) == true){
            
            captureLocalizedScreenshot("445757");
            replaceString = string.replace("Off","On");
            SetCellSwitchValue(replaceString,0,0);
            
        }
        TapNavigationBackButton();
    }
    else{
        LogMessage("445757 Fail");
    }
    
    if(TapTabButton(GetValueFromKey("LOCID Phone")) == true) {
        captureLocalizedScreenshot("452295");
        captureLocalizedScreenshot("398204");
        captureLocalizedScreenshot("398207");
        TapNavigationBackButton();
        captureLocalizedScreenshot("383567");
        captureLocalizedScreenshot("73294");
        captureLocalizedScreenshot("410124");
        TapNavigationBackButton();
    }
    else{
        LogMessage("452295 398204 398207 383567 73294 410124 Fail");
    }
    
    GotoMyInfo();
    if(TapViewElement(0,13) == true) {
        if(TapTableviewCell(0,GetValueFromKey("LOCID Forward Calls")) == true){
            if(TapTableviewCell(0,GetValueFromKey("LOCID New Number")) == true){
                TypeString("5588569953");
                captureLocalizedScreenshot("403611");
                TapNavigationRightButton();
                DelayInSec(4);
                captureLocalizedScreenshot("384004");
            }
            else{
                TapNavigationBackButton();
                LogMessage("403611 384004 Fail");
            }
            
        }
        else{
            LogMessage("403611 384004 Fail");
        }
        
        if(TapViewElement(0,6) == true) {
            captureLocalizedScreenshot("382016");
            if(TapViewElement(0,2) == true){
                DelayInSec(4);
                captureLocalizedScreenshot("398898");
            }
            TapViewElement(0,6);
            if(TapViewElement(0,1) == true){
                DelayInSec(4);
                captureLocalizedScreenshot("398897");
            }
        }
        else{
            LogMessage("382016 398898 398897 Fail");
        }
        
        if(TapTableviewCell(0,GetValueFromKey("LOCID Forward Calls")) == true){
            if(TapViewElement(0,9) == true){
                var searchBar = MainWindow.tableViews()[0].searchBars()[0];
                searchBar.tap();
                searchBar.setValue("C_v-yamu4");
                var string = "C_Yanxia Mu (iSoftStone)4, C_iSoftStone4";
                var cell = MainWindow.tableViews()[1].cells()[string];
                if(ElementValidAndVisible(cell) == true){
                    cell.tap();
                    DelayInSec(4);
                    captureLocalizedScreenshot("399638");
                    TapTableviewCell(0,GetValueFromKey("LOCID Forward Calls"));
                    captureLocalizedScreenshot("413998");
                    TapNavigationBackButton();
                }
                else{
                    TapNavigationBackButton();
                    LogMessage("399638 413998 Fail");
                }
                
            }
            else{
                TapNavigationBackButton();
                LogMessage("399638 413998 Fail");
            }
            
        }else{
            LogMessage("399638 413998 Fail");
        }
        
        if(TapTableviewCell(0,GetValueFromKey("LOCID Do Not Forward")) == true){
            
            DelayInSec(4);
            captureLocalizedScreenshot("410075");
            captureLocalizedScreenshot("384003");
            TapNavigationBackButton();
            captureLocalizedScreenshot("410076");
        }
        else{
            TapNavigationBackButton();
            LogMessage("410075 384003 410076 Fail");
        }
        
    }else{
        LogMessage("403611 384004 Fail");
        LogMessage("382016 398898 398897 Fail");
        LogMessage("399638 413998 Fail");
        LogMessage("410075 384003 410076 Fail");
    }
}



function GotoMySettings()
{
    
    DismissNotificationsIfAny();
    var note_field = MainWindow.tableViews()[0].textViews()[0];;
    var name_Label = MainWindow.tableViews()[0].staticTexts()["C_Yanxia Mu (iSoftStone)6"];
    if(ElementValidAndVisible(note_field) == true) {
        note_field.tap();
        TypeString("MyInfoLibReNoteIfNeededNotesnotNULLsettingittoNULLgfsdgfdsgfdsgdsg");
        DelayInSec(1);
        note_field.doubleTap();
        TapTableviewGroup(name_Label,0.09,0.43);
        DelayInSec(1)
        captureLocalizedScreenshot("381967");
        ResetNoteIfNeeded();
    }
    else{
        LogMessage("381967 Fail");
    }
  
    if(TapViewElement(0,13) == true) {
        
        captureLocalizedScreenshot("73288");
        if(TapTableviewCell(0,GetValueFromKey("LOCID Forward Calls")) == true){
            
            captureLocalizedScreenshot("382002");
            
            if(TapTableviewCell(0,GetValueFromKey("LOCID Voicemail")) == true){
                DelayInSec(4);
                captureLocalizedScreenshot("382021");
                captureLocalizedScreenshot("403610");
            }
            else{
                LogMessage("382021 403610 Fail");
            }
        }else{
            LogMessage("382002 382021 403610 Fail");
        }
        
        if(TapTableviewCell(0,GetValueFromKey("LOCID Simultaneously Ring")) == true){
            
            if(TapTableviewCell(0,GetValueFromKey("LOCID New Number")) == true){
                TypeString("56625563555");
                captureLocalizedScreenshot("382008");
                TapNavigationRightButton();
                TapTableviewCell(0,GetValueFromKey("LOCID Simultaneously Ring"));
                captureLocalizedScreenshot("382003");
                TapNavigationBackButton();
                captureLocalizedScreenshot("382007");
                captureLocalizedScreenshot("382011");
            }
            else{
                LogMessage("382008 382003 382007 382011 Fail");
            }
        }
        else{
            LogMessage("382008 382003 382007 382011 Fail");
        }
        TapNavigationBackButton();
        captureLocalizedScreenshot("410062");
        
    }else{
        LogMessage("382002 382021 403610 382008 382003 382007 382011 410062 Fail");
    }
}

function VerifyHeadTextDisplayWellOnSignInPage()
{
    
    try{
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        LogMessage("Not first time Sign In")
    }
    
    var returnValue = IsSignInScreenUp();
    var string = GetValueFromKey("LOCID Join Meeting");
    
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var tableView = MainWindow.tableViews()[0];
    tableView.scrollDown();
    var element = tableView.buttons()[string];
    if(element.isValid() == true){
        
        if(element.isVisible() == false){
         tableView.scrollToElementWithName(string);
        }
        captureLocalizedScreenshot("455940");
        tableView.scrollUp();
    }else {
        LogMessage("455940 Fail");
    }
}

function VerifyAnonymousJoinPageDisplayWell()
{
    try{
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        LogMessage("Not first time Sign In")
    }
    
    var returnValue = IsSignInScreenUp();
    var string = GetValueFromKey("LOCID Join Meeting");
    
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var tableView = MainWindow.tableViews()[0];
    var element = tableView.buttons()[string];
    if(element.isValid() == true){
        
        if(element.isVisible() == false){
            tableView.scrollToElementWithName(string);
        }
        element.tap();
        DelayInSec(1);
        captureLocalizedScreenshot("455941");
        TapNavigationBackButton();
        tableView.scrollUp();
    }else {
        LogMessage("455941 Fail");
    }
}

function VerifyCustomerExperienceTextOnOptionPage()
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
            LogMessage("455960 Fail");
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
    GotoMyInfo();
    if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
        
        SetElementScrollToVisible(MainWindow.tableViews()[0],GetValueFromKey("LOCID ceipSectionTitle"));
        DelayInSec(1);
        captureLocalizedScreenshot("455960");
        TapNavigationBackButton();
        
    }else {
        LogMessage("455960 Fail");
    }
}
function VerifySelectSignUpOnOptionPage()
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
            LogMessage("455961 Fail");
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
    GotoMyInfo();
    if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
        
        SetElementScrollToVisible(MainWindow.tableViews()[0],GetValueFromKey("LOCID ceipSectionTitle"));
        if(TapTableviewCellWithPredicate(GetValueFromKey("LOCID CEIP Sign up")) == true){
            
            captureLocalizedScreenshot("455961");
            TapNavigationBackButton();
        }else {
            LogMessage("455961 Fail");
        }
       
        TapNavigationBackButton();
        
    }else {
        LogMessage("455961 Fail");
    }

}


function VerifyMeetingStringDisplayWellOnSignInPage()
{
    try{
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        LogMessage("Not first time Sign In")
    }
    
    var returnValue = IsSignInScreenUp();
    var string = GetValueFromKey("LOCID Join Meeting");
    
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var tableView = MainWindow.tableViews()[0];
    var element = tableView.buttons()[string];
    if(element.isValid() == true){
        
        if(element.isVisible() == false){
            tableView.scrollToElementWithName(string);
        }
        element.tap();
        DelayInSec(1);
        captureLocalizedScreenshot("455958");
        tableView.scrollUp();
    }else {
        LogMessage("455958 Fail");
    }

}


function VerifyUserDisconnectedMettingOnSignInScreen()
{
    
    try{
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        LogMessage("Not first time Sign In")
    }
    
    var returnValue = IsSignInScreenUp();
    var string = GetValueFromKey("LOCID Join Meeting");
    
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var tableView = MainWindow.tableViews()[0];
    var element = tableView.buttons()[string];
    var textField = tableView.textFields()[GetValueFromKey("LOCID Enter your name")];
    if(element.isValid() == true && textField.isValid() == true){
        
        if(element.isVisible() == false){
            tableView.scrollToElementWithName(string);
        }
        textField.setValue(SIPURI_1);
        element.tap();
        DelayInSec(1);
       
        var cancelButton = MainWindow.buttons()[GetValueFromKey("LOCID Cancel Sign in")];
        if(cancelButton.isValid() == true && cancelButton.isVisible() == true)
        {
            Target.pushTimeout(20);
            var dismissButton = MainWindow.buttons()[GetValueFromKey("LOCID Dismiss")];
            Target.popTimeout();
            
            DelayInSec(1);
            dismissButton.tap();
           
            DelayInSec(1);
            captureLocalizedScreenshot("455959");
        }else {
            LogMessage("455959 Fail");
        }
        

    }else {
        LogMessage("455959 Fail");
    }

}

function VerifyHelperTextNoteDisplayWell() {
    
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
    
    var myInfoButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        TapElement(myInfoButton);
        SetHappeningNote("");
        captureLocalizedScreenshot("381966");
        GoToOrientation(DEVICELEFT);
        captureLocalizedScreenshot("381966_h");
        GoToOrientation(DEVICEPROT);

    }
    else{
        
        LogMessage("381966 Fail");
    }
}

function VerifyMyInfoViewPageDisplayWell(){
    
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
    
    var myInfoButton = MainWindow.buttons()[GetValueFromKey("LOCID ACCESSIBILITY_TIMELINE_BUTTON_PROFILE")];
    DelayInSec(1);
  
    if(ElementValidAndVisible(myInfoButton) == true){
        
        TapElement(myInfoButton);
        captureLocalizedScreenshot("73285");
        GoToOrientation(DEVICELEFT);
        captureLocalizedScreenshot("73285_h");
        GoToOrientation(DEVICEPROT);
        
        var dissmiss_button = Target.frontMostApp().navigationBar().buttons()[GetValueFromKey("DISMISS_BUTTON")];
        dissmiss_button.tap();
    }
    else{
        
        LogMessage("73285 Fail");
    }
    
}

function VerifySelfPresenceStatusPickerPageDisplayWell(){

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
    
    var myInfoButton = MainWindow.buttons()[GetValueFromKey("LOCID ACCESSIBILITY_TIMELINE_BUTTON_PROFILE")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        
        if(TapTableviewCellWithPredicate(GetValueFromKey("LOCID Status")) == true){
            
            captureLocalizedScreenshot("73286");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("73286_h");
            GoToOrientation(DEVICEPROT);
            
            TapNavigationBackButton();
        }
        else{
            LogMessage("73286 Fail");
        }
        
        var dissmiss_button = Target.frontMostApp().navigationBar().buttons()[GetValueFromKey("DISMISS_BUTTON")];
        dissmiss_button.tap();
    }
    else{
        
        LogMessage("73286 Fail");
    }
}

function VerifyTabBarTitlesDisplayWell() {
    
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
    
    var myInfoButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        TapElement(myInfoButton);
        captureLocalizedScreenshot("73287");
        GoToOrientation(DEVICELEFT);
        captureLocalizedScreenshot("73287_h");
        GoToOrientation(DEVICEPROT);
    }
    else{
        
        LogMessage("73287 Fail");
    }
}

function VerifyCallForwardingSettingPageDisplayWell(){
    
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
    
    var myInfoButton = MainWindow.buttons()[GetValueFromKey("LOCID ACCESSIBILITY_TIMELINE_BUTTON_PROFILE")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        if(TapTableviewCellWithPredicate(GetValueFromKey("LOCID Status")) == true) {
            
            captureLocalizedScreenshot("73288");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("73288_h");
            GoToOrientation(DEVICEPROT);
            
            TapNavigationBackButton();
        }else {
            
            LogMessage("73288 Fail");
        }
    }
    else{
        
        LogMessage("73288 Fail");
    }
}

function VerifyAboutPageOnOptionsSettingViewDisplayWell(){
    
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
    
    var myInfoButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
            
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            if(TapTableviewCellWithPredicate(GetValueFromKey("LOCID About")) == true) {
                
                captureLocalizedScreenshot("73293");
//                captureLocalizedScreenshot("445961");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("73293_h");
//                captureLocalizedScreenshot("445961_h");
                GoToOrientation(DEVICEPROT);
                TapNavigationBackButton();
            }
            else{
                LogMessage("73293 Fail");
            }
            TapNavigationBackButton();
            
        }else{
            
            LogMessage("73293 Fail");
        }
    }
    else{
        
        LogMessage("73293 Fail");
    }
}

function VerifyPhoneKeypadViewPageDisplayWell(){

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
    
    
    
    if(TapTabButton(GetValueFromKey("LOCID Phone")) == true) {
    
        var continueButton = MainWindow.scrollViews()[0].buttons()[GetValueFromKey("LOCID Continue")];
        if(continueButton.isVisible() == true){
            
            continueButton.tap();
            DelayInSec(1);
        }
        DelayInSec(1);
        
        TapNavigationBackButton();
        captureLocalizedScreenshot("73294");
        GoToOrientation(DEVICELEFT);
        captureLocalizedScreenshot("73294_h");
        GoToOrientation(DEVICEPROT);
        
        TapNavigationBackButton();
    }
    else{
        LogMessage("73294 Fail");
    }
    
}

function VerifyOtherSettingsOnOptionPageDisplayWell() {
    
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
    
    var myInfoButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        
        if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
            
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            
            captureLocalizedScreenshot("398199");
            GoToOrientation(DEVICELEFT);
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            captureLocalizedScreenshot("398199_h");
            GoToOrientation(DEVICEPROT);
            
            TapNavigationBackButton();
            
        }else{
            
            LogMessage("398199 Fail");
        }
    }
    else{
        
        LogMessage("398199 Fail");
    }

}

function VerifyContactsSettingsOnOptionPageDisplayWell() {
    
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
    
    var myInfoButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        
        if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
            
            SetElementScrollToVisible(MainWindow.tableViews()[0],GetValueFromKey("LOCID accessibilitySection"));
            
            var string = GetValueFromKey("LOCID Show Photos") + ", " + GetValueFromKey("LOCID On");
            if(SetCellSwitchValue(string,0,0) == true){
                
                replaceString = string.replace(GetValueFromKey("LOCID On"),GetValueFromKey("LOCID Off"));
                SetCellSwitchValue(replaceString,0,1);
                captureLocalizedScreenshot("398200");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("398200_h");
                GoToOrientation(DEVICEPROT);
            }
            else{
                LogMessage("398200 Fail");
            }
            
            TapNavigationBackButton();
            
        }else{
            
            LogMessage("398200 Fail");
        }
    }
    else{
        
        LogMessage("398200 Fail");
    }

}

function VerifyOptionsSettingsStringsDisplayWell() {
    
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
    
    var myInfoButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        
        if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
            
            captureLocalizedScreenshot("398201");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("398201_h");
            GoToOrientation(DEVICEPROT);
            
            TapNavigationBackButton();
        }else{
            
            LogMessage("398201 Fail");
        }
    }
    else{
        
        LogMessage("398201 Fail");
    }

}

function VerifyWifiForMediaSettingsDisplayWell() {
    
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
    
    var myInfoButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        
        if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
            
            var string = GetValueFromKey("LOCID Require WiFi");
            SetElementScrollToVisible(MainWindow.tableViews()[0],string);
            
            string = GetValueFromKey("LOCID WiFi VoIP") + ", " + GetValueFromKey("LOCID Off");
            if(SetCellSwitchValue(string,0,1) == true){
                
                captureLocalizedScreenshot("445757");
                GoToOrientation(DEVICELEFT);
                SetElementScrollToVisible(MainWindow.tableViews()[0],GetValueFromKey("LOCID Require WiFi"));
                captureLocalizedScreenshot("445757_h");
                GoToOrientation(DEVICEPROT);
                replaceString = string.replace("Off","On");
                SetCellSwitchValue(replaceString,0,0);
            }
            
            TapNavigationBackButton();
            
        }else{
            
            LogMessage("445757 Fail");
        }
    }
    else{
        
        LogMessage("445757 Fail");
    }
    
}


function VerifyHelpStringOnOptionsPageDisplayWell() {
    
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
    var myInfoButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
            
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            
            captureLocalizedScreenshot("445759");
            GoToOrientation(DEVICELEFT);
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            captureLocalizedScreenshot("445759_h");
            GoToOrientation(DEVICEPROT);
            TapNavigationBackButton();
            
        }else{
            
            LogMessage("445759 Fail");
        }
    }
    else{
        
        LogMessage("445759 Fail");
    }
}

function VerifySubHelpPageContentsPageDisplayWell() {
    
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
    var myInfoButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
            
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            
            if(TapTableviewCell(0,GetValueFromKey("LOCID online help")) == true){
                
                var linkLabel = MainWindow.scrollViews()[0].webViews()[0].links()["Get started"];
                WaitForObjectToBecomeVisible(linkLabel,"Help Link");
             
                DelayInSec(1);
                captureLocalizedScreenshot("445920_1");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("445920_1_h");
                GoToOrientation(DEVICEPROT);
                
                MainWindow.scrollViews()[0].webViews()[0].scrollDown();
                DelayInSec(1);
                captureLocalizedScreenshot("445920_2");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("445920_2_h");
                GoToOrientation(DEVICEPROT);
                
                TapNavigationBackButton();
            }
            else{
                
                LogMessage("445920_1 445920_2 Fail");
            }
            
            TapNavigationBackButton();
            
        }else{
            
            LogMessage("445920 Fail");
        }
    }
    else{
        
        LogMessage("445920 Fail");
    }

}



function VerifyUploadSignInLogsPageDisplayWell() {
    
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
    var myInfoButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
            
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            
            if(TapTableviewCellWithPredicate(GetValueFromKey("LOCID Logging")) == true){
                
                if(TapTableviewCell(0,1) == true){
                    
                    captureLocalizedScreenshot("453842");
                    GoToOrientation(DEVICELEFT);
                    captureLocalizedScreenshot("453842_h");
                    GoToOrientation(DEVICEPROT);
                    TapNavigationBackButton();
                    
                }else {
                    
                    LogMessage("453842 Fail");
                }
        
                TapNavigationBackButton();
            }
            else{
                
                LogMessage("453842 Fail");
            }
            
            TapNavigationBackButton();
            
        }else{
            
            LogMessage("453842 Fail");
        }
    }
    else{
        
        LogMessage("453842 Fail");
    }

}

function VerifyExchangePageDisplayWellOnSettingPage() {
    
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
    var myInfoButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
            
            DelayInSec(1);
        
            if(TapTableviewCellWithPredicate(GetValueFromKey("LOCID Exchange Credentials")) == true){
                
                captureLocalizedScreenshot("382033");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("382033_h");
                GoToOrientation(DEVICEPROT);
                DelayInSec(1);
                
                TapNavigationBackButton();
            }
            else{
                
                LogMessage("382033 Fail");
            }
            
            TapNavigationBackButton();
            
        }else{
            
            LogMessage("382033 Fail");
        }
    }
    else{
        
        LogMessage("382033 Fail");
    }

}


function VerifyHelpLinkNavigateToCorrectPage() {
    
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
    var myInfoButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
            
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            
            if(TapTableviewCell(0,GetValueFromKey("LOCID online help")) == true){
                
                var linkLabel = MainWindow.scrollViews()[0].webViews()[0].links()["Get started"];
                WaitForObjectToBecomeVisible(linkLabel,"Help Link");
                
                DelayInSec(1);
                captureLocalizedScreenshot("73289");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("73289_h");
                GoToOrientation(DEVICEPROT);
                
                TapNavigationBackButton();
            }
            else{
                
                LogMessage("73289 Fail");
            }
            
            TapNavigationBackButton();
            
        }else{
            
            LogMessage("73289 Fail");
        }
    }
    else{
        
        LogMessage("73289 Fail");
    }
}


function VerifyTermsOfUseLinkNavigateToCorrectPage() {
    
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
    var myInfoButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
            
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            if(TapTableviewCellWithPredicate(GetValueFromKey("LOCID About")) == true) {
                
                var button = target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Terms Of Use")];
                
                if(button.isVisible() == true){
                    
                    button.tap();
                    DelayInSec(10);
                    
                    captureLocalizedScreenshot("73298");
                    GoToOrientation(DEVICELEFT);
                    captureLocalizedScreenshot("73298_h");
                    GoToOrientation(DEVICEPROT);
                    
                    TapNavigationBackButton();
                }
                TapNavigationBackButton();
            }
            else{
                LogMessage("73298 Fail");
            }
            TapNavigationBackButton();
            
        }else{
            
            LogMessage("73298 Fail");
        }
    }
    else{
        
        LogMessage("73298 Fail");
    }

}

function VerifyPrivacyLinkNavigateToCorrectPage() {
    
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
    var myInfoButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    DelayInSec(1);
    
    if(ElementValidAndVisible(myInfoButton) == true){
        
        myInfoButton.tap();
        if(TapTableviewCell(0,GetValueFromKey("LOCID Options")) == true) {
            
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            MainWindow.tableViews()[0].scrollDown();
            DelayInSec(1);
            if(TapTableviewCellWithPredicate(GetValueFromKey("LOCID About")) == true) {
                
                var button = target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Privacy title")];
                
                if(button.isVisible() == true){
                    
                    button.tap();
                    DelayInSec(10);
                    
                    captureLocalizedScreenshot("73299");
                    GoToOrientation(DEVICELEFT);
                    captureLocalizedScreenshot("73299_h");
                    GoToOrientation(DEVICEPROT);
                    
                    TapNavigationBackButton();
                }
                TapNavigationBackButton();
            }
            else{
                LogMessage("73299 Fail");
            }
            
            TapNavigationBackButton();
            
        }else{
            
            LogMessage("73299 Fail");
        }
    }
    else{
        
        LogMessage("73299 Fail");
    }

}


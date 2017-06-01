function VerifySignInAutoConfigTest()
{
	LogMessage("SignInTests :: VerifySignInAutoConfigTest");
	
    SignInNormalConfigTest(SIPURI,PASSWORD);
    ValidateFirstRunSetPhoneNumber(PHONENUMBER);
	ValidateTabBarButtons();
}

function VerifySignInManualConfigTest()
{
	
	var sipUri = GetTestCaseParameters("SipUri2");
	var password = GetTestCaseParameters("Password2");
	var phoneNumber = GetTestCaseParameters("PhoneNumber2");

	SignIn(sipUri,password);
    validateFirstRun(phoneNumber);
	ValidateTabBarButtons();
}

function VerifySignOutTest()
{	
	if(IsSignInScreenUp() == 1){
        SignInAuto();
    }
	SignOut();
}

function VerifySignOut()
{
    if(IsSignInScreenUp() == 1){
        SignInAuto();
    }
	SignOutApp();
    
}

function VerifySignInUCEnabledConfigTest()
{
    
    try{
        
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        
    }
    
    var sipUri = GetTestCaseParameters("SipUri1");
	var password = GetTestCaseParameters("Password1");
	var phoneNumber = GetTestCaseParameters("PhoneNumber1");
	var internalServerAddress = GetTestCaseParameters("InternalServerAddress2");
	var externalServerAddress = GetTestCaseParameters("ExternalServerAddress2");
    
	SignInUCEnabled(sipUri,password, phoneNumber, internalServerAddress , externalServerAddress , true);
	ValidateTabBarButtons();
}

function SignInConfigTest(sipUri,password)
{
    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow()

    var tableView = mainWindow.tableViews()[MainWindow.tableViews().length - 1];

    var sipUriField = tableView.cells()[0].elements()[0];
    sipUriField.setValue(sipUri);
  
    var passwordField = tableView.cells()[1].elements()[0];
   
    passwordField.setValue(password);
    tableView.scrollUp();

    var element = tableView.buttons()[GetValueFromKey("LOCID Sign in")];
  
    element.tap();
    DelayInSec(1);
    
    if(UIATarget.localTarget().frontMostApp().navigationBar().leftButton().isVisible()==true)
    {
        UIATarget.localTarget().frontMostApp().navigationBar().leftButton().tap();
    }
}

function SignInVideoConfigTest(SIPURI,PASSWORD) {
    
    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow()
    
    var tableView = mainWindow.tableViews()[MainWindow.tableViews().length - 1];
    var element = tableView.buttons()[GetValueFromKey("LOCID Sign in")];
    
    element.tap();
    DelayInSec(1);
    
    if(UIATarget.localTarget().frontMostApp().navigationBar().leftButton().isVisible()==true)
    {
        UIATarget.localTarget().frontMostApp().navigationBar().leftButton().tap();
    }
}

function SignInNormalConfigTest(sipUri,password,index)
{
   
    var tableView = MainWindow.tableViews()[index];

    var sipUriField = tableView.cells()[0].elements()[0];
    var passwordField = tableView.cells()[1].elements()[0];
    var string = GetValueFromKey("LOCID Save Password");

    sipUriField.setValue(sipUri);
    passwordField.setValue(password);
    
    string = GetValueFromKey("LOCID Sign in");
    tableView.scrollToElementWithName(string);
    element = tableView.buttons()[GetValueFromKey("LOCID Sign in")];
    
    DoubleTapElement(element);
    DelayInSec(1);
    
    if(UIATarget.localTarget().frontMostApp().navigationBar().leftButton().isVisible()==true)
    {
        UIATarget.localTarget().frontMostApp().navigationBar().leftButton().tap();
    }
    
}

function SignInTest(sipUri,password)
{
    var index = 0;
    var tableView = MainWindow.tableViews()[index];
    tableView.scrollUp();
    var sipUriField = tableView.cells()[0].textFields()[0];
    var passwordField = tableView.cells()[1].elements()[0];
    
    sipUriField.setValue(sipUri);
    passwordField.setValue(password);
    
    string = GetValueFromKey("LOCID Sign In");
    tableView.scrollToElementWithName(string);
    element = tableView.buttons()[GetValueFromKey("LOCID Sign In")];
    
    DoubleTapElement(element);
    
    if(UIATarget.localTarget().frontMostApp().navigationBar().leftButton().isVisible()==true)
    {
        UIATarget.localTarget().frontMostApp().navigationBar().leftButton().tap();
    }
    
}

function validateFirstRun(phoneNumber)
{
	if(MainWindow.isValid() == true) {
        
        var textString = MainWindow.staticTexts()[GetValueFromKey("LOCID Lets begin")];
        if(ElementValidAndVisible(textString) == true) {
            
            captureLocalizedScreenshot("408621");
            TapNavigationRightButton();
        }
        else
            LogMessage("408621 Fail");
        
        textString = MainWindow.scrollViews()[0].staticTexts()[GetValueFromKey("LOCID Require Wifi for VoIP calls")];
        if(ElementValidAndVisible(textString) == true) {
            
            captureLocalizedScreenshot("445709_445710");
            
            var mySwitch = MainWindow.scrollViews()[0].switches()[0];
            if(ElementValidAndVisible(mySwitch) == true) {
                
                SetSwitchValue(mySwitch,1);
                captureLocalizedScreenshot("445713");
                SetSwitchValue(mySwitch,0);
                captureLocalizedScreenshot("445714_1");
                MainWindow.scrollViews()[0].scrollDown();
                DelayInSec(1);
                captureLocalizedScreenshot("445714_2");
            }
            else
                LogMessage("445713 445714_1 445714_2 Fail");
            
            mySwitch = MainWindow.scrollViews()[0].switches()[1];
            if(ElementValidAndVisible(mySwitch) == true) {
                
                SetSwitchValue(mySwitch,0);
                captureLocalizedScreenshot("445716");
                SetSwitchValue(mySwitch,1);
                TapNavigationRightButton();
            }
            else
                LogMessage("445716 Fail");
            
            var phoneNumberField = MainWindow.tableViews()[0].cells()[0].textFields()[0];
            if(ElementValidAndVisible(phoneNumberField) == true){
                
                captureLocalizedScreenshot("382047");
                captureLocalizedScreenshot("383356");
                phoneNumberField.tap();
                var num = parseInt(Math.random()*100000000)
                var string = num.toString();
                
                TypeString(string);
                captureLocalizedScreenshot("381963_383572");
                TapNavigationRightButton();
            }
            else
                LogMessage("382047 383356 381963_383572 Fail");
            
            MainWindow.scrollViews()[0].scrollToElementWithName(GetValueFromKey("LOCID Yes"));
            var button_1 = MainWindow.scrollViews()[0].buttons()[GetValueFromKey("LOCID No")];
            var button_2 = MainWindow.scrollViews()[0].buttons()[GetValueFromKey("LOCID Yes")];
            if(ElementValidAndVisible(button_1) == true && ElementValidAndVisible(button_2) == true){
                
                captureLocalizedScreenshot("397924");
                captureLocalizedScreenshot("404745");
                button_1.tap();
                DelayInSec(1);
                captureLocalizedScreenshot("445719_445720");
                TapNavigationBackButton();
                button_2.tap();
                DelayInSec(1);
                captureLocalizedScreenshot("445718");
                captureLocalizedScreenshot("446067");
                TapNavigationBackButton();
            }
            else
                LogMessage("397924 404745 445719_445720 445718 446067 Fail");
            
            TapNavigationBackButton();
            TapNavigationBackButton();
            
            if(ElementValidAndVisible(mySwitch) == true) {
                
                SetSwitchValue(mySwitch,0);
                TapNavigationRightButton();
                DismissNotificationsIfAny();
                phoneNumberField.tap();
                TypeString(phoneNumber);
                TapNavigationRightButton();
                MainWindow.scrollViews()[0].scrollToElementWithName(GetValueFromKey("LOCID Yes"));
                button_2.tap();
                DelayInSec(1);
                captureLocalizedScreenshot("446069");
                TapNavigationBackButton();
                button_1.tap();
                DelayInSec(1);
                captureLocalizedScreenshot("446076");
                TapNavigationRightButton();
            }
            else
                LogMessage("446069 446076 Fail");
            
        }
        else
            LogMessage("445709_445710 Fail");
        
    }
}

function SignIn(sipUri,password)
{
	
    var button = MainWindow.buttons()[GetValueFromKey("LOCID Understand")];
    if(button.isValid() == true){
        if(button.isVisible()){
            captureLocalizedScreenshot("453848");
            button.tap();
        }
    }
    else
        LogMessage("453848 Fail");
    
    var index = 0;
    var tableView = MainWindow.tableViews()[index];
    if(tableView.isValid() == true){
        
        var sipUriField = MainWindow.tableViews()[index].cells()[0].textFields()[0];
        var passwordField = MainWindow.tableViews()[index].cells()[1];
        var string = GetValueFromKey("LOCID Save Password");
        if(sipUriField.isValid() == true){
            if(sipUriField.isVisible()){
                SetCellSwitchValue(string,index,1);
                captureLocalizedScreenshot("381955");
                captureLocalizedScreenshot("445706");
                captureLocalizedScreenshot("397920_1");
                SetCellSwitchValue(string,index,0);
                captureLocalizedScreenshot("383358");
                captureLocalizedScreenshot("445707");
                SetCellSwitchValue(string,index,1);
                sipUriField.setValue(sipUri);
                passwordField.tap();
                TypeString(password);
            }
            else
                LogMessage("381955 445706 383358 397920_1 445707 Fail");
        }
        
        string = GetValueFromKey("LOCID More Details");
        var element = tableView.groups()[string].elements()[string];
        if(element.isValid() == true){
            
            TapTableviewGroup(element,0.55,0.76);
            captureLocalizedScreenshot("397920_2");
            captureLocalizedScreenshot("429378");
            captureLocalizedScreenshot("381956_1");
        }
        else
            LogMessage("381956_1 429378 397920_2 Fail");
        
        var string_1 = GetValueFromKey("LOCID Auto-Detect Server");
        SetCellSwitchValue(string_1,index,0);
        tableView.scrollDown();
        string = GetValueFromKey("LOCID HTTP Proxy Credentials");
        element = tableView.cells()[string];
        if(element.isValid() == true){
            
            captureLocalizedScreenshot("381956_2");
            captureLocalizedScreenshot("381964");
        }
        else
            LogMessage("381956_2 381964 Fail");
        
        tableView.scrollUp();
        DelayInSec(1);
        SetCellSwitchValue(string_1,index,1);
        
        string = GetValueFromKey("LOCID Sign-in As");
        element = tableView.cells()[string];
        if(element.isValid() == true){
            
            TapElement(element);
            captureLocalizedScreenshot("381957");
            TapNavigationBackButton();
        }
        else
            LogMessage("381957 Fail");
        
        tableView.scrollDown();
        DelayInSec(1);
        
        string = GetValueFromKey("LOCID Sign In");
        element = tableView.buttons()[GetValueFromKey("LOCID Sign In")];
        
        if(element.isValid() == true){
            
            DoubleTapElement(element);
            captureLocalizedScreenshot("381958");
            Target.pushTimeout(20);
            var firstRunBackground = MainWindow.images()["firstRunBackground.png"];
            Target.popTimeout();
            
        }
        else
            LogMessage("381958 Fail");
    }
    else
        LogMessage("tableView is NULL");
}

function SignOut()
{
    GotoMyInfo();
    
	TapNavigationRightButton();
    DelayInSec(0.2);
    captureLocalizedScreenshot("383570");
    
	Target.pushTimeout(15);
    var signInButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Sign In")];
	Target.popTimeout();
	IsValidAndVisible(signInButton,"Sign In Button");
}

function SignInUCEnabled (sipUri,password)
{
    var button = MainWindow.buttons()[GetValueFromKey("LOCID Understand")];
    if(button.isValid() == true){
        if(button.isVisible()){
            button.tap();
        }
    }
    
    var index = 0;
    var tableView = MainWindow.tableViews()[index];
    if(tableView.isValid() == true){
        
        var sipUriField = MainWindow.tableViews()[index].cells()[0].textFields()[0];
        var passwordField = MainWindow.tableViews()[index].cells()[1];
        var string = GetValueFromKey("LOCID Save Password");
        if(sipUriField.isValid() == true){
            
            SetCellSwitchValue(string,index,1);
            SetCellSwitchValue(string,index,0);
            sipUriField.setValue(sipUri);
            passwordField.tap();
            TypeString(password);
            
            var string = GetValueFromKey("LOCID Sign In");
            var element = tableView.buttons()[GetValueFromKey("LOCID Sign In")];
            
            DoubleTapElement(element);
            Target.pushTimeout(20);
            var firstRunBackground = MainWindow.images()["firstRunBackground.png"];
            Target.popTimeout();
        }
    }
    else
        LogMessage("tableView is NULL");
}

function validateUcEnabledFirstRun(phoneNumber)
{
    
	if(MainWindow.isValid() == true) {
        
        var textString = MainWindow.staticTexts()[GetValueFromKey("LOCID Lets begin")];
        if(ElementValidAndVisible(textString) == true) {
            
            TapNavigationRightButton();
        }
        
        textString = MainWindow.scrollViews()[0].staticTexts()[GetValueFromKey("LOCID Require Wifi for VoIP calls")];
        if(ElementValidAndVisible(textString) == true) {
            
            TapNavigationRightButton();
        }
        var phoneNumberField = MainWindow.tableViews()[0].cells()[0].textFields()[0];
        if(ElementValidAndVisible(phoneNumberField) == true){
            
            phoneNumberField.tap();
            TypeString(phoneNumber);
            captureLocalizedScreenshot("381962");
            TapNavigationRightButton();
            TapNavigationRightButton();
        }
    }
}

function ValidateFirstRunSetPhoneNumber(phoneNumber) {
    
    LogMessage("SignInLib :: validateFirstRun  :: Validating First Run");

    var phoneNumberCellField = MainWindow.tableViews()[0].cells()[0].textFields()[0];
  
    if(!phoneNumberCellField.isVisible()){
       
        phoneNumberCellField = MainWindow.tableViews()[0].cells()[1].textFields()[0];
    }
    if(phoneNumberCellField.isVisible() == true){
        
        var rightButton = Target.frontMostApp().navigationBar().rightButton();
        if(rightButton.isEnabled() == false && rightButton.isVisible() == true){
            
            phoneNumberCellField.tap();
            TypeString(phoneNumber);
        }
        
        DelayInSec(1);
       
        TapNavigationRightButton();
        
        rightButton = Target.frontMostApp().navigationBar().rightButton();
        
        if(rightButton.isVisible() == true){
            
            rightButton.tap();
            DelayInSec(1);
        }
    }
}

function ValidateFirstRunPhoneNumber(phoneNumber) {
    
    LogMessage("SignInLib :: validateFirstRun  :: Validating First Run");
    
    var textString = MainWindow.staticTexts()[GetValueFromKey("LOCID Lets begin")];
    TapNavigationRightButton();
    
    textString = MainWindow.scrollViews()[0].staticTexts()[GetValueFromKey("LOCID Require Wifi for VoIP calls")];
    TapNavigationRightButton();
    var phoneNumberField = MainWindow.tableViews()[0].cells()[0].textFields()[0];
    phoneNumberField.tap();
    TypeString(phoneNumber);
    TapNavigationRightButton();
    
    if(MainWindow.scrollViews()[0].isVisible() == true){
        
        MainWindow.scrollViews()[0].scrollToElementWithName(GetValueFromKey("LOCID Yes"));
        var button = MainWindow.scrollViews()[0].buttons()[GetValueFromKey("LOCID No")];
        button.tap();
        DelayInSec(2);
    }
    
    Target.frontMostApp().navigationBar().rightButton().tap();
    DelayInSec(1);
}



function VerifySignInViewPageDisplayWell(){
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 0){
        
        SignOutApp();
    }
     
    var index = 0;
    var tableView = MainWindow.tableViews()[index];
    
    if(tableView.isValid() == true){
        
        tableView.scrollUp();
        var string = GetValueFromKey("LOCID Hide Details");
        var element = tableView.groups()[string].elements()[string];
        if(element.isValid() == true){
            
            TapTableviewGroup(element,0.46,0.47);
        }
        
        var sipUriField = tableView.cells()[0].textFields()[0];
        var passwordField = tableView.cells()[1].elements()[0];
        string = GetValueFromKey("LOCID Save Password");
        
        if(sipUriField.isValid() == true){
            if(sipUriField.isVisible()){
                
                sipUriField.setValue("");
                passwordField.setValue("");
                SetCellSwitchValue(string,index,1);
                
                string = GetValueFromKey("LOCID More Details");
                element = tableView.groups()[string].elements()[string];
                if(element.isValid() == true){
                    
                    TapTableviewGroup(element,0.46,0.47);
                }
                string = GetValueFromKey("LOCID Auto-Detect Server")
                tableView.cells()[string].scrollToVisible();
                
                captureLocalizedScreenshot("381955_2");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("381955_2_h");
                GoToOrientation(DEVICEPROT);
                
                tableView.scrollUp();
                DelayInSec(1);
                string = GetValueFromKey("LOCID Hide Details");
                element = tableView.groups()[string].elements()[string];
                if(element.isValid() == true){
                    
                    TapTableviewGroup(element,0.46,0.47);
                    
                    captureLocalizedScreenshot("381955_1");
                    GoToOrientation(DEVICELEFT);
                    captureLocalizedScreenshot("381955_1_h");
                    GoToOrientation(DEVICEPROT);
                    
                   
                }
            }
            else
                
                LogMessage("381955 Fail");
        }
    }else {
        
        LogMessage("381955 Fail");
    }
}

function VerifySignInButtonDisplayWell() {
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var index = 0;
    var tableView = MainWindow.tableViews()[index];
    
    if(tableView.isValid() == true){
        
        tableView.scrollUp();
        var string = GetValueFromKey("LOCID Hide Details");
        var element = tableView.groups()[string].elements()[string];
        if(element.isValid() == true){
            
            TapTableviewGroup(element,0.46,0.47);
        }
        
        string = GetValueFromKey("LOCID Sign In");
        tableView.scrollToElementWithName(string);
        element = tableView.buttons()[GetValueFromKey("LOCID Sign In")];
        
        if(element.isValid() == true){
            if(element.isVisible()){
                
                captureLocalizedScreenshot("383358");
                captureLocalizedScreenshot("445706");
                captureLocalizedScreenshot("445707");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("383358_h");
                captureLocalizedScreenshot("445706_h");
                tableView.scrollDown();
                captureLocalizedScreenshot("445707_h");
                GoToOrientation(DEVICEPROT);
                
            }
            else
                
                LogMessage("383358 Fail");
        }else {
            
            LogMessage("383358 Fail");
        }
        
    }else {
        
        LogMessage("383358 Fail");
    }
}

function VerifyStringsSignInViewPageDisplayWell() {
    
    var returnValue = IsSignInScreenUp();
    
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var index = 0;
    var tableView = MainWindow.tableViews()[index];
    if(tableView.isValid() == true){
        
        tableView.scrollUp();
        var string = GetValueFromKey("LOCID More Details");
        var element = tableView.groups()[string].elements()[string];
        if(element.isValid() == true){
            
            TapTableviewGroup(element,0.46,0.47);
        }
       
        var sipUriField = tableView.cells()[0].textFields()[0];
        var passwordField = tableView.cells()[1].elements()[0];
        string = GetValueFromKey("LOCID Save Password");
        
        if(sipUriField.isValid() == true){
            
            if(sipUriField.isVisible()){
                
                SetCellSwitchValue(string,index,1);
                
                string = GetValueFromKey("LOCID Hide Details");
                element = tableView.groups()[string].elements()[string];
                if(element.isValid() == true){
                    
                    string = GetValueFromKey("LOCID Auto-Detect Server")
                    tableView.cells()[string].scrollToVisible();
                    captureLocalizedScreenshot("381956");
                    captureLocalizedScreenshot("429378");
                    GoToOrientation(DEVICELEFT);
                    captureLocalizedScreenshot("381956_1_h");
                    captureLocalizedScreenshot("429378_h");
                    
                    string = GetValueFromKey("LOCID Sign In");
                    tableView.scrollToElementWithName(string);
                    captureLocalizedScreenshot("381956_2_h");

                    GoToOrientation(DEVICEPROT);
                }
                else{
                    
                    LogMessage("381956 429378 Fail");
                }
            }else{
                
                 LogMessage("381956 429378 Fail");
            }
        }else{
            
            LogMessage("381956 429378 Fail");
        }
    }else{
        
        LogMessage("3819556 429378 Fail");
    }
}



function VerifySelfPresenceStatusPageDisplayWell(){
    

    var returnValue = IsSignInScreenUp();
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var index = 0;
    var tableView = MainWindow.tableViews()[index];
    if(tableView.isValid() == true){
       
        tableView.scrollUp();
        var string = GetValueFromKey("LOCID More Details");
        var element = tableView.groups()[string].elements()[string];
        if(element.isValid() == true){
            
            TapTableviewGroup(element,0.46,0.47);
        }

        string = GetValueFromKey("LOCID Sign-in As");
        element = tableView.cells()[string];
        if(element.isValid() == true){
            
            TapElement(element);
            captureLocalizedScreenshot("381957");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("381957_h");
            GoToOrientation(DEVICEPROT);
            
            TapNavigationBackButton();
        }
        else
            LogMessage("381957 Fail");
    }
    else
        LogMessage("381957 Fail");
    
}


function VerifyLyncIsSigningInPageDisplayWell(){

    var returnValue = IsSignInScreenUp();
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var tableView = MainWindow.tableViews()[0];
    if(tableView.isValid() == true){
        
     
        tableView.scrollUp();
        var sipUriField = tableView.cells()[0].textFields()[0];
        var passwordField = tableView.cells()[1].elements()[0];
        
        sipUriField.setValue(SIPURI_3);
        passwordField.setValue(PASSWORD_3);
        
        string = GetValueFromKey("LOCID Sign In");
        tableView.scrollToElementWithName(string);
        var element = tableView.buttons()[GetValueFromKey("LOCID Sign In")];
        
        element.tap();
        
        var button = MainWindow.buttons()[GetValueFromKey("LOCID Cancel Sign in")];
        if(button.isValid() == true) {
            
            captureLocalizedScreenshotWithNoDisMiss("381958");
            if(button.isVisible() == true){
                
                button.doubleTap();
                
            }
            
            DelayInSec(1);
            
            var element = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Sign In")];
            if(element.isVisible() == true){
                
                element.tap();
            }
            
            GoToOrientation(DEVICELEFT);
            DelayInSec(0.5);
            captureLocalizedScreenshotWithNoDisMiss("381958_h");
            button = MainWindow.buttons()[GetValueFromKey("LOCID Cancel Sign in")];
            if(button.isVisible() == true){
                
                button.tap();
              
            }
  
            UIATarget.localTarget().setDeviceOrientation(DEVICEPROT);
            
        }else {
            
            LogMessage("381958 Fail");
        }
        
        if(MainWindow.navigationBars()[0].buttons()[GetValueFromKey("LOCID Next")].isVisible() == true)
		{
            
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        
    }
    else
        
        LogMessage("381958 Fail");
}

function VerifyAutoDetectServerHelperTextDisplayWell() {
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var index = 0;
    var tableView = MainWindow.tableViews()[index];
    if(tableView.isValid() == true){
        
        tableView.scrollUp();
        var string = GetValueFromKey("LOCID More Details");
        var element = tableView.groups()[string].elements()[string];
        if(element.isValid() == true){
            
            TapTableviewGroup(element,0.46,0.47);
        }
        
        string = GetValueFromKey("LOCID Auto-Detect Server");
        element = tableView.cells()[string];
        if(element.isValid() == true){
            
            SetElementScrollToVisible(tableView,string);
            SetCellSwitchValue(string,index,0);
            captureLocalizedScreenshot("381964");
            GoToOrientation(DEVICELEFT);
            SetElementScrollToVisible(tableView,string);
            captureLocalizedScreenshot("381964_h");
            GoToOrientation(DEVICEPROT);
            
            SetCellSwitchValue(string,index,1);
            
            tableView.scrollUp();
            DelayInSec(1);
        }
        else
            LogMessage("381964 Fail");
    }
    else
        LogMessage("381964 Fail");
}

function VerifyLyncSigningOutPageDisplayWell(){
    
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
            
        }
    }
    
    GotoMyInfo();
	TapNavigationRightButton(0);
    
	Target.pushTimeout(15);
    var signInButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Sign In")];
	Target.popTimeout();
	IsValidAndVisible(signInButton,"Sign In Button");
}

function VerifyAdvancedOptionsDisplayWell() {
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var index = 0;
    var tableView = MainWindow.tableViews()[index];
    if(tableView.isValid() == true){
        
        tableView.scrollUp();
        var string = GetValueFromKey("LOCID More Details");
        var element = tableView.groups()[string].elements()[string];
        if(element.isValid() == true){
            
            captureLocalizedScreenshot("397920_1");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("397920_1_h");
            GoToOrientation(DEVICEPROT);
            TapTableviewGroup(element,0.46,0.47);
            
            captureLocalizedScreenshot("397920_2");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("397920_2_h");
            GoToOrientation(DEVICEPROT);
            
            return;
        }
        
        string = GetValueFromKey("LOCID Hide Details");
        element = tableView.groups()[string].elements()[string];
        if(element.isValid() == true){
        
            captureLocalizedScreenshot("397920_2");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("397920_2_h");
            GoToOrientation(DEVICEPROT);
            
            TapTableviewGroup(element,0.46,0.47);
            
            captureLocalizedScreenshot("397920_1");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("397920_1_h");
            GoToOrientation(DEVICEPROT);
            
            return;
        }
    }
    else
        LogMessage("397920 Fail");
    
}


function VerifyLyncFirstRunWelcomePageDisplayWell(){
  
    var returnValue = IsSignInScreenUp();
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var index = 0;
    var tableView = MainWindow.tableViews()[index];
    if(tableView.isValid() == true){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        DelayInSec(1);
        returnValue = WaitForSignInSuccess();
        
        if(returnValue == 0){
            
            LogMessage("73315 Fail");
            return;
        }
        else if (returnValue == 1){
            
            DelayInSec(1);
            captureLocalizedScreenshotWithNoDisMiss("73315_1");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshotWithNoDisMiss("73315_1_h");
            GoToOrientation(DEVICEPROT);
            
            var phoneNumberCellField = MainWindow.textFields()[0];
            
            if(!phoneNumberCellField.isVisible()){
                
                phoneNumberCellField = MainWindow.textFields()[0];
            }
            if(phoneNumberCellField.isVisible() == true){
                
                var rightButton = MainWindow.buttons()[GetValueFromKey("LOCID Next")];
                if(rightButton.isEnabled() == false && rightButton.isVisible() == true){
                    
                    phoneNumberCellField.tap();
                    TypeString(PHONENUMBER);
                }
                
                DelayInSec(1);
                rightButton.tap();
//                TapNavigationRightButton();
                
                captureLocalizedScreenshotWithNoDisMiss("73315_2");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshotWithNoDisMiss("73315_2_h");
                GoToOrientation(DEVICEPROT);
                
                
                rightButton = MainWindow.buttons()[GetValueFromKey("LOCID Done")];
                
                if(rightButton.isVisible() == true){
                    
                    rightButton.tap();
                    DelayInSec(1);
                }
            }
      
        }
        else {
            
            LogMessage("73315 Fail");
        }
    }
    else
        LogMessage("73315 Fail");

}

function VerifySignInAndWorkWellWithGallatinAccount() {
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var tableView = MainWindow.tableViews()[0];
    if(tableView.isValid() == true){
        
        tableView.scrollUp();
        var sipUriField = tableView.cells()[0].textFields()[0];
        var passwordField = tableView.cells()[1].elements()[0];
        
        sipUriField.setValue(SIPURI_5);
        passwordField.setValue(PASSWORD_5);
        
        string = GetValueFromKey("LOCID Sign In");
        tableView.scrollToElementWithName(string);
        var element = tableView.buttons()[GetValueFromKey("LOCID Sign In")];
        element.tap();
        
        var isVisibled = false;
        var numberView = 0;
        for(var i = 0;i < 120;i++)
        {
            
            var dismissButton = MainWindow.buttons()[GetValueFromKey("LOCID Dismiss")];
            if(dismissButton.isVisible() == true){
                
                dismissButton.tap();
                break;
            }
            
            var continueButton = MainWindow.scrollViews()[0].buttons()[GetValueFromKey("LOCID Continue")];
            if(continueButton.isVisible() == true){
                
                continueButton.tap();
                DelayInSec(1);
            }
            
            if(MainWindow.navigationBars()[0].buttons()[GetValueFromKey("LOCID Next")].isVisible() == true)
            {
            
                isVisibled = true;
                numberView = 1;
                break;
            }
            
            if(Target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")].isVisible() == true){
               
               numberView = 2;
               isVisibled = true;
               break;
            }
            DelayInSec(1);
            
        }
        
        if(isVisibled == true)
        {
            if(numberView == 1){
                
                ValidateFirstRunSetPhoneNumber(PHONENUMBER_5);
            }
            
            DelayInSec(1);
            captureLocalizedScreenshot("458437");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("458437_h");
            GoToOrientation(DEVICEPROT);
            
            GotoMyInfo();
            TapNavigationRightButton(0);
            
            Target.pushTimeout(15);
            var signInButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Sign In")];
            Target.popTimeout();
            IsValidAndVisible(signInButton,"Sign In Button");
            
            
        }else {
            
            var button = MainWindow.buttons()[GetValueFromKey("LOCID Cancel Sign in")];
            if(button.isVisible() == true){
                
                button.tap();
                
            }
            LogMessage("458437 Fail");
        }
        
        
    }
    else
        LogMessage("458437 Fail");
        
}

function VerifySringNeedPasswordToSignInDisplayWell() {
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 0){
        
        SignOutApp();
    }
    
    var tableView = MainWindow.tableViews()[0];
    if(tableView.isValid() == true){
        
        tableView.scrollUp();
        var sipUriField = tableView.cells()[0].textFields()[0];
        var passwordField = tableView.cells()[1].elements()[0];
        
        sipUriField.setValue(SIPURI);
        passwordField.setValue("");
        
        string = GetValueFromKey("LOCID Sign In");
        tableView.scrollToElementWithName(string);
        var element = tableView.buttons()[GetValueFromKey("LOCID Sign In")];
        element.tap();
        
        var isVisibled = false;
     
        for(var i = 0;i < 10;i++)
        {
            
            var dismissButton = MainWindow.buttons()[GetValueFromKey("LOCID Dismiss")];
            if(dismissButton.isVisible() == true){
                
                isVisibled = true;
                break;
            }
            
            DelayInSec(1);
        }
        if(isVisibled == true){
            
            DelayInSec(1);
            captureLocalizedScreenshotWithNoDisMiss("457565");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshotWithNoDisMiss("457565_h");
            GoToOrientation(DEVICEPROT);
        }else{
            
            LogMessage("457565 Fail");
        }
        
    }
    else
        LogMessage("457565 Fail");
}


function VerifySignInAndSignOutSuccessfulTest() {
    
    var isSignin = false;
    var returnValue = IsSignInScreenUp();
    var index = 0;
    if(returnValue == 0){
        
        GotoMyInfo();
        
        var signoutButton =  MainWindow.tableViews()[0].cells()[GetValueFromKey("LOCID Sign Out")];
        
        if(signoutButton.isValid() == true && signoutButton.isVisible() == true){
            
            signoutButton.tap();
        }
        isSignin = true;
        captureLocalizedScreenshotWithNoDisMiss("244365_2");
        
        index = 1;
        Target.pushTimeout(15);
        var signInButton = MainWindow.tableViews()[index].buttons()[GetValueFromKey("LOCID Sign in")];
        Target.popTimeout();

    }
    
    var tableView = MainWindow.tableViews()[index];
  
    if(tableView.isValid() == true){
        
        SignInConfigTest(SIPURI,PASSWORD);
        DelayInSec(1);
        captureLocalizedScreenshotWithNoDisMiss("244365_1");
        
        returnValue = WaitForSignInSuccess();
        
        if(returnValue == 0){
            
            LogMessage("244365 Fail");
            return;
        }
        else if (returnValue == 1){
            
            var phoneNumberCellField = MainWindow.textFields()[0];
            
            if(!phoneNumberCellField.isVisible()){
                
                phoneNumberCellField = MainWindow.textFields()[0];
            }
            if(phoneNumberCellField.isVisible() == true){
                
                var rightButton = MainWindow.buttons()[GetValueFromKey("LOCID Next")];
                if(rightButton.isEnabled() == false && rightButton.isVisible() == true){
                    
                    phoneNumberCellField.tap();
                    TypeString(PHONENUMBER);
                }
                
                DelayInSec(1);
                rightButton.tap();
       
                rightButton = MainWindow.buttons()[GetValueFromKey("LOCID Done")];
                
                if(rightButton.isVisible() == true){
                    
                    rightButton.tap();
                    DelayInSec(1);
                }
                
                GotoMyInfo();
                
                var signoutButton =  MainWindow.tableViews()[0].cells()[GetValueFromKey("LOCID Sign Out")];
                
                if(signoutButton.isValid() == true && signoutButton.isVisible() == true){
                    
                    signoutButton.tap();
                }
                isSignin = true;
                captureLocalizedScreenshotWithNoDisMiss("244365_2");
                
                Target.pushTimeout(15);
                var signInButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Sign in")];
                Target.popTimeout();
            }
            
        }
        else if(returnValue == 2){
            
            if(isSignin == false) {
                
                GotoMyInfo();
                
                var signoutButton =  MainWindow.tableViews()[0].cells()[GetValueFromKey("LOCID Sign Out")];
                
                if(signoutButton.isValid() == true && signoutButton.isVisible() == true){
                    
                    signoutButton.tap();
                }
                isSignin = true;
                captureLocalizedScreenshotWithNoDisMiss("244365_2");
                
                Target.pushTimeout(15);
                var signInButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Sign in")];
                Target.popTimeout();
            }
            
        }

    }
    else
        LogMessage("244365 Fail");
    
}

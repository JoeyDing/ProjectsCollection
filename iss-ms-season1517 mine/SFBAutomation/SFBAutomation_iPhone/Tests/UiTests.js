#import "../AreaLibs/TestUtils.js"
#import "../AreaLibs/Constants.js"
#import "../AreaLibs/SignInLib.js"
#import "../AreaLibs/AppLib.js"
#import "../AreaLibs/MeetingsLib.js"
#import "../AreaLibs/ContactsAndGroupLib.js"
#import "../AreaLibs/SelfLib.js"
#import "../AreaLibs/ConversationLib.js"
#import "../AreaLibs/DataCollabLib.js"
#import "./SignInTests.js"
#import "./AppTests.js"
#import "./ContactsTests.js"
#import "./IMTests.js"
#import "./SelfTests.js"
#import "./ConferenceTests.js"
#import "./AudioTests.js"
#import "./DataCollabTests.js"
#import "./AppShareTests.js"
#import "./VideoTests.js"
#import "./CaptureScreen.js"
#import "./ApplicationsTests.js"
#import "./VoiceTests.js"

var numberOfTestCasesInt = 0;
var currentTestCaseToRunInt = 0;
var numberOfTestCasesStr = "";
var currentTestCaseToRunStr = "";

UIATarget.onAlert = function onAlert(alert)
{
    var title = alert.name();
    LogMessage("Alert with title " + title + " encountered!");
    try
    {
    	UIATarget.localTarget().captureScreenWithName(alert.name());
    	alert.buttons()["Abort"].tap();
    	return true;
    }
    catch(error)
    {
    	return false;
    }
}

function TestCleanUp()
{
	try
	{
		GotoChats();
		LogMessage("UiTests :: TestCleanUp :: TestCleanUp done");
	}
	catch(error)
	{
		LogMessage("UiTests :: TestCleanUp :: TestCleanUp has failed");
	}
}

function TestBegin(suiteName,testCaseName)
{
    LogMessage("UiTests :: TestBegin :: TESTCASE " + testCaseName.toUpperCase() + " FROM SUITE " + suiteName.toUpperCase() + " STARTED");
   
    var isSignedIn = false;
    
    try{
        Target.frontMostApp().mainWindow().buttons()[GetValueFromKey("LOCID Understand")].tap();
    }
    catch(error){
        LogMessage("Not first time Sign In");
    }

    
    isSignedIn = WaitForOnlineMode();
     
    if(isSignedIn == true)
    {
          
        GotoMyInfo();
        ResetNoteIfNeeded();
        
    }
    else
    {
        var returnValue = IsSignInIngScreenUp();
        
        if( returnValue == 0)
        {
        	var statusButton = MainWindow.buttons()[GetValueFromKey("LOCID ACCESSIBILITY_TIMELINE_BUTTON_PROFILE")];
            
            while(statusButton.isVisible() == false){
                
                DelayInSec(1);
            }
        }
    }
    
}

function TestEnd(suiteName,testCaseName)
{
    LogMessage("UiTests :: TestEnd :: TESTCASE " + testCaseName.toUpperCase() + " FROM SUITE " + suiteName.toUpperCase() + " ENDED");
}

function RunTest()
{
    for( var count = currentTestCaseToRunInt ; count < numberOfTestCasesInt ; count++)
    {
    	UpdateCurrentTestCaseToRun(count);
        var suiteName = GetTestCaseInfo("SuiteName");
        var testCaseName = GetTestCaseInfo("TestCase"+(count+1).toString());
       
        var testCaseId = "";
        var lengthOfTestCaseStr = testCaseName.length;
        if(testCaseName.indexOf("#") != -1)
        {
        	if(testCaseName.indexOf("#") != (lengthOfTestCaseStr-1))
        	{
        		// Extract test case name & WTT Id from string
        		testCaseId = testCaseName.substring(testCaseName.indexOf("#") + 1);
        	}
        	testCaseName = testCaseName.substring(0,testCaseName.indexOf("#"));
        }
        
        if(testCaseName.indexOf("//") == 0)
        {
    
        	continue;
        }
        
        try
        {
        	TestBegin(suiteName,testCaseName);
            LogMessage("UiTests :: RunTest :: TestBegin start ,test case run");
        }
        catch(error)
        {
        	LogMessage("UiTests :: RunTest :: TestBegin has failed , Aborting test case run");
        	continue;
        }
        try
         {
             
             var testToRun = eval(testCaseName);
             testToRun();
             LogMessage("UiTests :: RunTest :: " + testCaseName.toUpperCase() + " " + testCaseId + " PASS!!!");
         }
         catch(error)
         {
             LogMessage("UiTests :: RunTest :: " + testCaseName.toUpperCase() + " " + testCaseId + " FAIL!!!!");
             SendFailureLogs(testCaseName,"" + error);
             TestCleanUp();
             continue;
         }
    	 try
         {
        	TestEnd(suiteName,testCaseName);
         }
         catch(error)
         {
         }
    }
    DeleteTestCaseInfoFile();
}

function WaitForAlertOnLaunch(alertName,buttonToTap)
{
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	target.pushTimeout(2);
	var alert = mainWindow.elements()[alertName];
	target.popTimeout();
	if(alert.isValid() && alert.isVisible())
	{
		alert.buttons()[buttonToTap].tap();
        var testCaseName = GetTestCaseInfo("TestCase" + currentTestCaseToRunStr);
		DelayInSec(2);
		SendFailureLogs(testCaseName,"Lync Crashed");
	}
}

numberOfTestCasesStr = GetTestCaseInfo("NumberOfTestCases");
currentTestCaseToRunStr = GetTestCaseInfo("CurrentTestCaseToRun");
numberOfTestCasesInt = parseInt(numberOfTestCasesStr);
currentTestCaseToRunInt = parseInt(currentTestCaseToRunStr);
WaitForAlertOnLaunch("Lync Crash Reporter","Cancel");
RunTest();




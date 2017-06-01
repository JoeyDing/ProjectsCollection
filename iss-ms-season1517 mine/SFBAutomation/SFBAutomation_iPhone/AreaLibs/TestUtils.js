function GetBuddyDisplayName(buddyIndex)
{
	var buddyDisplayNameList = GetTestCaseParameters("BuddyListDisplayName");
	var arrayOfbuddyDisplayName = buddyDisplayNameList.split(",");
	return arrayOfbuddyDisplayName[buddyIndex];
}

function CaptureScreenShot(screenName)
{
	UIATarget.localTarget().captureScreenWithName(screenName);
}

function CaptureRectScreenShot(rect,rectName)
{
	UIATarget.localTarget().captureRectWithName(rect,rectName);
}

function GetBuddyCount()
{
	var buddyDisplayNameList = GetTestCaseParameters("BuddyListDisplayName");
	var arrayOfbuddyDisplayName = buddyDisplayNameList.split(",");
	return arrayOfbuddyDisplayName.length;
}

function getKeyValueFromXmlFile(fileName,keyName)
{ 
	var keyNameStartTag = "<" + keyName + ">";
	var keyNameEndTag = "</" + keyName + ">";
	var result = UIATarget.localTarget().host().performTaskWithPathArgumentsTimeout("/usr/bin/grep", [keyName,fileName], 10);
	var keyValue = result.stdout.substring(result.stdout.indexOf(keyNameStartTag) + keyNameStartTag.length, result.stdout.indexOf(keyNameEndTag));
	return keyValue;
}

function GetTestCaseInfo(testCaseInfoAttribute)
{
	var returnValue = getKeyValueFromXmlFile("../TestConfigFiles/TestCaseInfo.xml",testCaseInfoAttribute);
	return returnValue;
}

function GetTestCaseParameters(testCaseParameter)
{
	var returnValue = getKeyValueFromXmlFile("../TestConfigFiles/TestCaseParameters.xml", testCaseParameter);
	return returnValue;
}

function UpdateCurrentTestCaseToRun(currentTestId)
{
	var newTestCaseId = currentTestId + 1;
	var stringToReplace = "s_<CurrentTestCaseToRun>" + currentTestId + "</CurrentTestCaseToRun>_<CurrentTestCaseToRun>" + newTestCaseId + "</CurrentTestCaseToRun>_g";
	var result = UIATarget.localTarget().host().performTaskWithPathArgumentsTimeout("/usr/bin/sed", ["-e",stringToReplace,"-i",".bak","../TestConfigFiles/TestCaseInfo.xml"], 10);
}

function DeleteTestCaseInfoFile()
{
	var result = UIATarget.localTarget().host().performTaskWithPathArgumentsTimeout("/bin/rm", ["-fR","../TestConfigFiles/TestCaseInfo.xml"], 10);
	result = UIATarget.localTarget().host().performTaskWithPathArgumentsTimeout("/bin/rm", ["-fR","../TestConfigFiles/TestCaseInfo.xml.bak"], 10);
}

function LogMessage(message)
{
	UIALogger.logMessage(message);
}

function IsValidAndVisible(element,elementName)
{
	
	while(true)
	{
        
		if(element.isValid())
          
			if(element.isVisible())
				return;
			else
                break;
	}
}

function ElementValidAndVisible(element)
{
	var isVisible = false;

    for(var i = 0;i < 30;i++)
	{
        
        if(element.isValid() == true && element.isVisible() == true)
		{
            
            return true;
        }
        else
		    DelayInSec(1);
	}
    
    return isVisible
}

function WaitIndexElementValidAndVisible(element,index)
{
	var isVisible = false;
    
    for(var i = 0;i < index;i++)
	{
        
        if(element.isValid() == true && element.isVisible() == true)
		{
            
            return true;
        }
        else
		    DelayInSec(1);
	}
    
    return isVisible
}



function DismissNotificationsIfAny()
{
	while(true)
	{
		Target.pushTimeout(1);
		var dismissButton = MainWindow.buttons()[GetValueFromKey("LOCID Dismiss")];
		Target.popTimeout();
		if(dismissButton.isValid())
		{
			try
			{
				dismissButton.tap();
                DelayInSec(1);
                return true;
			}
			catch(error)
			{
                return false
			}
		}
		else
		{
			return false;
		}
	}
}

function TapOnKeyboardwithIndex(index)
{
	var target = UIATarget.localTarget();
	var button;
	if(index == key_Delete)
		button  = target.frontMostApp().keyboard().keys()[10]
	else
		button = target.frontMostApp().keyboard().buttons()[index];
	try
	{
		IsValidAndVisible(button,"Button to tap with index : " + index);
		button.tap();
		DelayInSec(1);
	}
	catch(error)
	{
		LogMessage(buttonToTap + " Button is not valid or visible , not tapping it with index " + index);
	}
}

function HideKeyboard()
{
	var target = UIATarget.localTarget();
	
	buttonToTap = target.frontMostApp().keyboard().buttons().length - 1;
	var button = target.frontMostApp().keyboard().buttons()[buttonToTap];
	try
	{
		IsValidAndVisible(button,"Hide keyboard");
		button.tap();
		DelayInSec(1);
	}
	catch(error)
	{
		LogMessage(buttonToTap + " Button is not valid or visible , not tapping it");
	}
}

function TapOnKeyboardButton(buttonToTap)
{
	var target = UIATarget.localTarget();

	var button = target.frontMostApp().keyboard().buttons()[buttonToTap];
	try
	{
		IsValidAndVisible(button,buttonToTap);
		button.tap();
		DelayInSec(1);
	}
	catch(error)
	{
		LogMessage(buttonToTap + " Button is not valid or visible , not tapping it");
	}
}
//
function TapNavigationBackButton()
{
    var target = UIATarget.localTarget();
    var button = target.frontMostApp().navigationBar().leftButton();
    if(button.isValid()==true && button.isVisible()==true){
        
        button.tap();
    }
    DelayInSec(1);
}

function TapNavigationRightButton()
{
    var target = UIATarget.localTarget();
    var button = target.frontMostApp().navigationBar().rightButton();
    if(button.isValid()==true && button.isVisible()==true){
        
        button.tap();
    }
    DelayInSec(1);
}

function TapNavigationRightButton(index)
{
    var target = UIATarget.localTarget();
    var button = target.frontMostApp().navigationBar().rightButton();
    if(button.isValid()==true && button.isVisible()==true){
        
        button.doubleTap();
    }
    DelayInSec(index);
}

function TapActionSheetCancelButton()
{
    var target = UIATarget.localTarget();
    target.frontMostApp().actionSheet().cancelButton().tap();
    DelayInSec(1);
}

function TapActionSheetButton(string)
{
    var target = UIATarget.localTarget();
    var actionSheet = target.frontMostApp().actionSheet();
    if(actionSheet.isValid()==true && actionSheet.isVisible()==true){
        
        var button = actionSheet.buttons()[string];
        if(button.isValid()==true && button.isVisible()==true){
            
            button.tap();
        }
    }else {
        
        var button = target.frontMostApp().navigationBar().rightButton();
        if(button.isValid()==true && button.isVisible()==true){
            
            button.tap();
            
            var actionSheet = target.frontMostApp().actionSheet();
            if(actionSheet.isValid()==true && actionSheet.isVisible()==true){
                
                var button = actionSheet.buttons()[string];
                if(button.isValid()==true && button.isVisible()==true){
                    
                    button.tap();
                }
            }
        }

    }
    DelayInSec(1);
}


function DelayInSec(iSec)
{
	var target = UIATarget.localTarget();
	target.delay(iSec);
}

function TypeString(string)
{
	Target.frontMostApp().keyboard().typeString(string);
    DelayInSec(1);
}

function GoToOrientation(orientation) {
    
    UIATarget.localTarget().setDeviceOrientation(orientation);
    VerifyAppOrientation(orientation);
    DelayInSec(1);
}

function VerifyAppOrientation(orientation) {
   
    var app = UIATarget.localTarget().frontMostApp();
    
    for(i=1;i<=15;i++) {
        
        if(app.interfaceOrientation() == orientation)
            break;
        DelayInSec(1);
    }
}

function SetCellSwitchValue(string,index,value) {
    
    var tableView = MainWindow.tableViews()[index];
    var mySwitch = tableView.cells()[string].switches()[string];
    
    if(mySwitch.isVisible() == true){
        mySwitch.setValue(value);
        DelayInSec(1);
  
        return true;
    }
    else
        return false;
}

function SetSwitchValue(element,value) {
    
    element.setValue(value);
    DelayInSec(1);
}

function TapTableviewGroup(element,offSetX,offSetY) {
    
    element.tapWithOptions({tapOffset:{x:offSetX, y:offSetY}});
    DelayInSec(1);
}

function SetTableViewScrollToVisible(index,object) {
    
    var tableView = MainWindow.tableViews()[index];
    tableView.cells()[object].scrollToVisible();
    DelayInSec(1);
}

function SetElementScrollToVisible(object,string) {
    
    var scrollView = object;
    scrollView.scrollToElementWithName(string);
    DelayInSec(1);
}

function TapElement(element) {
    
    element.tap();
    DelayInSec(1);
}

function TapViewElement(index,value) {
    
    var view = MainWindow.elements()[index];
    var subView = view.elements()[value];
    if(subView.isVisible() == true){
        subView.tap();
        DelayInSec(1);
        return true;
    }
    else
        return false;
}

function TapWaitingElement(index,value) {
    
    var isVisible = false;
    var view = MainWindow.elements()[index];
    var subView = view.elements()[value];
    for(var i = 0;i < 15;i++)
	{
        
        if(subView.isValid() == true && subView.isVisible() == true)
		{
            subView.tap();
            DelayInSec(0.8);
            return true;
        }
        else
		    DelayInSec(1)
    }
    
    return isVisible;
}

function WaitButtonEnabledAndValid(string) {
    
    var isVisible = false;
    var button = MainWindow.buttons()[string];

    for(var i = 0;i < 30;i++)
    {
        if(button.isValid() == true && button.isEnabled() == true)
        {
         
            return true;
        }
        else
            
            DelayInSec(1)
    }
    
    return isVisible;
}

function TapTableviewCell(index,value) {
    
    var tableView = MainWindow.tableViews()[index];
    var cell = tableView.cells()[value];

    
    if(cell.isVisible() == true){
        cell.tap();
        DelayInSec(1);
        return true;
    }
    else
        return false;
}

function TapElementWithPredicate(string) {
    
    var target = UIATarget.localTarget();

    var mainWindow = target.frontMostApp().mainWindow();
    target.pushTimeout(2);
	var queryString = "name CONTAINS '" + string + "'";
	var cell = mainWindow.tableViews()[GetValueFromKey("LOCID_CONTACTS_TABLE_VIEW")].cells().firstWithPredicate(queryString);
	target.popTimeout();
    DelayInSec(1);
    
	if(cell.isVisible() == true  && cell.isValid() == true) {
       
	 	cell.tap();
        DelayInSec(1);
        return true;
	}else{
		return false;
	}
}

function TapTableviewCellWithPredicate(string) {
    
    var target = UIATarget.localTarget();
    LogMessage(" Button is not valid or visible , not tapping it with index ");
    var mainWindow = target.frontMostApp().mainWindow();
    target.pushTimeout(2);
    var queryString = "name CONTAINS '" + string + "'";
    var cell = mainWindow.tableViews()[0].cells().firstWithPredicate(queryString);
    target.popTimeout();
    DelayInSec(1);
    
    if(cell.isVisible() == true  && cell.isValid() == true) {
        
        cell.tap();
        DelayInSec(1);
        return true;
    }else{
        return false;
    }
}


function TapTableviewCellWithPredicate(string,tableString) {
    
    Target.pushTimeout(2);
    var queryString = "name CONTAINS '" + string + "'";
    var cell = MainWindow.tableViews()[tableString].cells().firstWithPredicate(queryString);
    Target.popTimeout();
    DelayInSec(1);
  
    if(cell.isVisible() == true  && cell.isValid() == true) {
        
        cell.tap();
        DelayInSec(1);
        return true;
    }else{
        return false;
    }
}

function WaitTableViewCellValidAndVisible(string) {
    
    var isVisible = false;
    
    for(var i = 0;i < 15;i++)
	{
        Target.pushTimeout(1);
        var queryString = "name CONTAINS '" + string + "'";
        var cell = MainWindow.tableViews()[0].cells().firstWithPredicate(queryString);
        Target.popTimeout();
        
        if(cell.isValid() == true && cell.isVisible() == true)
		{
            return true;
        }
        else{
            
            DelayInSec(1);
        }
    }
    
    return isVisible
}


function TapMainTableviewCellWithPredicate(string) {
    
    var isVisible = false;

    for(var i = 0;i < 30;i++)
	{
        Target.pushTimeout(2);
        var queryString = "name CONTAINS '" + string + "'";
        var cell = MainWindow.tableViews()[1].cells().firstWithPredicate(queryString);
        Target.popTimeout();
    
        if(cell.isValid() == true && cell.isVisible() == true)
		{
            cell.tap();
            DelayInSec(0.8);
            return true;
        }
        else
		    DelayInSec(1)
    }
    
    return isVisible
}


function TapButton(string) {
    
    var button = MainWindow.buttons()[string];
    if(button.isVisible() == true){
        button.tap();
        DelayInSec(1);
        return true;
    }
    else
        return false;
}

function TapTabButton(string) {
    
    Target.pushTimeout(10);
	var queryString = "name CONTAINS '" + string + "'";
	var button = UIATarget.localTarget().frontMostApp().tabBar().buttons().firstWithPredicate(queryString);
	Target.popTimeout();

    if(button.isVisible() == true){
        button.tap();
        DelayInSec(0.5);
        return true;
    }
    else
        return false;
}


function DoubleTapElement(element) {
    
    element.doubleTap();
    DelayInSec(1);
}

//
function SendFailureLogs(scenarioName,errorOccured)
{
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var sendFeedBackAlias = GetTestCaseParameters("SendFeedBackAlias");
	
	target.shake();
	DelayInSec(2);
	
	mainWindow.tableViews()[0].cells()[4].tap();
	DelayInSec(1);
	mainWindow.tableViews()[0].cells()[4].textFields()[0].setValue("iPad Test Automation Failure");
	
	HideKeyboard();
	
	mainWindow.tableViews()[0].cells()[5].tap();
	DelayInSec(1);
	mainWindow.tableViews()[0].cells()[5].textViews()[0].setValue(scenarioName + " Failed" );
	HideKeyboard();
	
	mainWindow.navigationBars()["Shake 'n' Send"].buttons()["Next"].tap();
	DelayInSec(1);
	
	mainWindow.scrollViews()[0].textFields()["toField"].tap();
	DelayInSec(1);
	TapOnKeyboardwithIndex(key_Delete);
	TapOnKeyboardwithIndex(key_Delete);
	mainWindow.scrollViews()[0].textFields()["toField"].setValue(sendFeedBackAlias);
	HideKeyboard();
	var subjectOfMail = "Automation Failure - " + scenarioName + " - " + errorOccured
	mainWindow.scrollViews()[0].textFields()["subjectField"].setValue(subjectOfMail);
	HideKeyboard();
	
	if(mainWindow.scrollViews()[0].textFields()["toField"].value() == sendFeedBackAlias)
	{
		mainWindow.navigationBars()[subjectOfMail].buttons()[1].tap();
		DelayInSec(2);	
	}
}


function WaitForObjectToBecomeVisible(element,elementName)
{
	if(element.isVisible() == false)
	{
    	for(i = 1;i <= 30;i++)
    	{
       		if(element.isVisible())
                return true;
          	DelayInSec(1);
        }
        return false;
    }else{
        return true;
    }
}


function getLocalizedStringFromKey(key, file)
{
	var newKey = "\"" + key + "\"";	
	var result = UIATarget.localTarget().host().performTaskWithPathArgumentsTimeout("/usr/bin/grep", [newKey,file], 10);
	var searchLine = result.stdout;
	var searchStartString = "=";
	var searchEndString = ";"; 
	var quotedString = searchLine.substring(searchLine.indexOf(searchStartString) + 2, searchLine.indexOf(searchEndString));
	//removing double quotes
	var localizedString = quotedString.replace("\"", "");
	var localizedString = localizedString.replace("\"", "");
	
    
	return localizedString;
}

function GetValueFromKey(key)
{
    
	return getLocalizedStringFromKey(key, "../TestConfigFiles/Localizable.strings");
}

function getCurrentTime()
{
    var Digital=new Date()
    var hours=Digital.getHours()
    var minutes=Digital.getMinutes()
    var seconds=Digital.getSeconds()
    var dn="AM"
    if(hours>12){
        dn="PM"
        hours=hours-12
    }
    if(hours==0){
        hours=12
    }
    if(minutes<=9){
        minutes="0"+minutes
    }if(seconds<=9){
        seconds="0"+seconds
    }
    myclock=hours+":"+minutes+":"+" "+dn;
    return myclock;
}



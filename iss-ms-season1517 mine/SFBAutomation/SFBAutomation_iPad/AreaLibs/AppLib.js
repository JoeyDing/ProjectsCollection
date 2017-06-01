function ValidateTabBarButtons()
{
	var myInfoButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
	ElementValidAndVisible(myInfoButton);
	var contactsButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Contacts")];
	ElementValidAndVisible(contactsButton);
	var chatsButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Conversations")];
	ElementValidAndVisible(chatsButton);
	var meetingsButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Meetings")];
	ElementValidAndVisible(meetingsButton);
	var voiceMailButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Phone")];
	ElementValidAndVisible(voiceMailButton);
}

function GotoMyInfo()
{
	
	var myInfoButton = MainWindow.buttons()[GetValueFromKey("LOCID ACCESSIBILITY_TIMELINE_BUTTON_PROFILE")];
	if(ElementValidAndVisible(myInfoButton) == true)
    {
        DelayInSec(1);
        myInfoButton.tap();
    }
 
}
function GotoChats()
{
	
    Target.pushTimeout(10);
	var queryString = "name CONTAINS '" + GetValueFromKey("LOCID ACCESSIBILITY_TIMELINE_BUTTON_CONTACTS") + "'";
	var chatsButton = MainWindow.buttons().firstWithPredicate(queryString);
	Target.popTimeout();
    
	ElementValidAndVisible(chatsButton);
    DelayInSec(1);
	chatsButton.tap();
}

function GotoContacts()
{
	var contactsButton = MainWindow.elements()[GetValueFromKey("LOCID Timeline Home")];
	ElementValidAndVisible(contactsButton);
	contactsButton.tap();
}	


function GotoMeetings()
{
	
	var meetingsButton =  MainWindow.elements()[10];
	ElementValidAndVisible(meetingsButton);
	meetingsButton.tap();
}


function GotoVoiceMail()
{
    Target.pushTimeout(10);
	var queryString = "name CONTAINS '" + GetValueFromKey("LOCID Phone") + "'";
	var phoneButton = UIATarget.localTarget().frontMostApp().tabBar().buttons().firstWithPredicate(queryString);
	Target.popTimeout();
    
    ElementValidAndVisible(phoneButton);
    phoneButton.tap();
}

function TapOnIMInvite(inviteeName)
{
	LogMessage("AppLib :: TapOnIMInvite");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	DismissNotificationsIfAny();
	
	LogMessage("AppLib :: TapOnIMInvite :: Waiting for Incoming IM");
	target.pushTimeout(30);
	var imDismissButton = mainWindow.buttons()["notification btn close X"];
	target.popTimeout();
	
	IsValidAndVisible(imDismissButton,"IM Dismiss Button");
	
	var imInvite = mainWindow.staticTexts()[inviteeName];
	IsValidAndVisible(imInvite,"IM Invite from : " + inviteeName);
	
	DelayInSec(2);
		
	LogMessage("AppLib :: TapOnIMInvite :: Tapping on IM Invite");
	imInvite.tap();

} 

function TapOnMuteUnmuteButton()
{
	LogMessage("AppLib :: TapOnMuteUnMuteIcon");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var muteUnMutebutton = mainWindow.buttons()["icon conversation mute black"];
	IsValidAndVisible(muteUnMutebutton, "Mute UnMute button");
	
	muteUnMutebutton.tap();
}

function AceptRejectCallToast(buttonType)
{
	LogMessage("AppLib :: AceptRejectCallToast");
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	target.pushTimeout(90);
	var acceptRejectButton = mainWindow.buttons()[buttonType];
	target.popTimeout();
	IsValidAndVisible(acceptRejectButton,"Call Toast Button Type -  " + buttonType);
	DelayInSec(2);
	acceptRejectButton.tap();
}

function WaitForCallToGetConnected()
{
	LogMessage("AppLib :: WaitForCallToGetConnected");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	for( i = 0 ; i < 12 ; i++)
	{
		DelayInSec(1);
		var callBarTimer = mainWindow.staticTexts().withPredicate("name CONTAINS '00:0'");

		if(callBarTimer.length == 1)
		{
			IsValidAndVisible(callBarTimer[0],"Call Bar Timer");
			LogMessage("AppLib :: Call is now connected, waiting for 5 sec");
			DelayInSec(5);
			
			GoBackToNonFullScreen();
			
			return;
		}
		else
		{
			DismissNotificationsIfAny();
		}
	}
	
	GoBackToNonFullScreen();
	throw new Error("No or Multiple call bar timers found");
}

function VerifyCallOnHold()
{
	LogMessage("AppLib :: VerifyCallOnHold");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	DelayInSec(1);
	var onHold = GetValueFromKey("LOCID Call Status On Hold");
	var callBarTimer = mainWindow.staticTexts().withPredicate("name CONTAINS '' + onHold + '00:0'");

	if(callBarTimer.length == 1)
	{
		IsValidAndVisible(callBarTimer[0],"Call Bar Timer Hold");
		LogMessage("AppLib :: Call is now on Hold");
		return;
	}
	else
	{
		throw new Error("Call is not on Hold");
	}
	
}

function VerifyCallNotOnHold()
{
	LogMessage("AppLib :: VerifyCallNotOnHold");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	DelayInSec(1);
	
	try
	{
		VerifyCallOnHold();
		throw new Error("Call is still on Hold");
	}
	catch(error)
	{
		if(error.message.indexOf("Call is not on Hold") >=0)
			return;
		else
			throw error;
	}
	
}

function FlickCallBarIfPresent()
{
	LogMessage("AppLib :: FlickCallBarIfPresent");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var callbarMuteIcon = mainWindow.buttons()["icon conversation mute black"];
	var callbarExpandArrow = mainWindow.buttons()["icon tools left arrow"];
	
	try
	{
		LogMessage("AppLib :: FlickCallBarIfPresent :: Checking for Expanded Call bar");
		IsValidAndVisible(callbarMuteIcon, "Call bar mute icon");
		var callbarmuteRect = callbarMuteIcon.rect();
		var xcord = callbarmuteRect.origin.x;
		var ycord = callbarmuteRect.origin.y;
		var width = callbarmuteRect.size.width;
		var height = callbarmuteRect.size.height;
		var newxCord =  xcord - 2*width;
		target.flickFromTo({x: newxCord, y: ycord}, {x: xcord, y: height});
	}
	catch(error)
	{
		LogMessage("AppLib :: FlickCallBarIfPresent :: Checking for collapsed Call bar");
		try
		{
			IsValidAndVisible(callbarExpandArrow, "Call bar expand arrow");
			LogMessage("AppLib :: FlickCallBarIfPresent :: Call bar already collapsed");
		}
		catch(error)
		{
			LogMessage("AppLib :: FlickCallBarIfPresent :: No Call bar present");
		}
	}

}

function GoFullScreen()
{
    LogMessage("AppLib :: GoFullScreen");
	var mainWindow = UIATarget.localTarget().frontMostApp().mainWindow();

	var conversationNavigationIndex = mainWindow.navigationBars().length - 1;
	var fullScreenButton = mainWindow.navigationBars()[conversationNavigationIndex].buttons()["icon panel hide"];	IsValidAndVisible(fullScreenButton, "icon panel hide");
	fullScreenButton.tap();

    for(i=1;i<=15;i++)
    {
        if(!(mainWindow.buttons()[GetValueFromKey("LOCID My Status")].isValid()))
    	     break;
    	DelayInSec(2);
    }
    
    if(mainWindow.buttons()[GetValueFromKey("LOCID My Status")].isValid())
       throw new Error("View did not transition to FullScreen");
}

function GoBackToNonFullScreen()
{
    LogMessage("AppLib :: GoNonFullScreen");
	var mainWindow = UIATarget.localTarget().frontMostApp().mainWindow();
    mainWindow.tap();
	var conversationNavigationIndex = mainWindow.navigationBars().length - 1;
	var fullScreenButton = mainWindow.navigationBars()[conversationNavigationIndex].buttons()["Full Screen"];
	try
	{
		IsValidAndVisible(fullScreenButton, "Full screen button");
		fullScreenButton.tap();
	}
	catch(error)
	{
		var NonfullScreenButton = mainWindow.navigationBars()[conversationNavigationIndex].buttons()["icon panel hide"];
		IsValidAndVisible(NonfullScreenButton, "Non Full screen button");
		NonfullScreenButton.tap();
	}
	
	
	for(i=1;i<=15;i++)
    {
        try
        {
           IsValidAndVisible(mainWindow.buttons()[GetValueFromKey("LOCID My Status")],"My Info Button");
           break;
        }
        catch(error)
        {}
        DelayInSec(2);
    }
    IsValidAndVisible(mainWindow.buttons()[GetValueFromKey("LOCID My Status")],"My Info Button");       
}

function VerifyImmersiveView()
{
    LogMessage("AppLib :: VerifyImmersiveView");
    LogMessage("AppLib :: VerifyImmersiveView : Waiting for 5 sec for immersive mode to kick in");
    DelayInSec(6);
    
    var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	//NavigationBar
	navigationBar = mainWindow.navigationBars()[0];
	
	//CallBar
	var callBarTimer = mainWindow.staticTexts().withPredicate("name CONTAINS '00:'");
	if(!(callBarTimer.length == 1))
	{ 
	    callBarTimer = mainWindow.staticTexts().withPredicate("name CONTAINS '01:'");
	} 
	
	//Verify Immersive View : Hidden CallBar and no NavigationBar
	if(callBarTimer[0].isValid() && !(navigationBar.isValid()))
	{ 
	   if(callBarTimer[0].isVisible())
	        throw new Error("Call Bar still visible");
	}
	else
	{ 
	        throw new Error("Call Bar is not valid or Navigation Bar is valid");
	}
}

function GoToOrientation(orientation)
{
    UIATarget.localTarget().setDeviceOrientation(orientation);
    DelayInSec(0.5);
}

function VerifyAppOrientation(orientation)
{
    LogMessage("AppLib :: VerifyAppOrientation");
    var app = UIATarget.localTarget().frontMostApp();

    for(i=1;i<=15;i++)
    {
        if(app.interfaceOrientation() == orientation)
           break;
        DelayInSec(2);
    }
    
    LogMessage("AppLib :: VerifyAppOrientation :: Current Orienatation = " + app.interfaceOrientation().toString());
    if(!(app.interfaceOrientation() == orientation))
         throw new Error("App not in expected orientation");
}


function ExpandCallBarIfPresent()
{
	LogMessage("AppLib :: ExpandCallBarIfPresent");
	var callbarExpandArrow = mainWindow.buttons()["icon tools left arrow"];
	
	try
	{
		IsValidAndVisible(callbarExpandArrow, "Call bar expand arrow");
		callbarExpandArrow.tap();
	}
	catch(error)
	{
		try
		{
			var callbarMuteIcon = mainWindow.buttons()["icon conversation voice 1"];
			IsValidAndVisible(callbarMuteIcon, "Call bar mute icon");
			LogMessage("AppLib :: ExpandCallBarIfPresent :: Call bar already expanded");
		}
		catch(error)
		{
			LogMessage("AppLib :: ExpandCallBarIfPresent :: No Call bar present");
			
		}
	}
}

function TapOnApplicationWindow() {
    LogMessage("AppLib :: TapOnApplicationWindow");
    var mainWindow = UIATarget.localTarget().frontMostApp().mainWindow();
    mainWindow.tap();
}


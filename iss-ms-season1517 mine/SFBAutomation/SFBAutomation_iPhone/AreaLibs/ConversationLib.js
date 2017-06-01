function SendIM(message)
{
	LogMessage("ConversationLib :: SendIM :: Writing message : " + message);
	
    var imTextView = MainWindow.textViews()[GetValueFromKey("CHATINPUT_PLACEHOLDER")];
    imTextView.tap();

    Target.frontMostApp().keyboard().typeString(message);
	
	//Tap on send key
	var sendButton = MainWindow.buttons()[GetValueFromKey("CHAT_SEND_BUTTON_ACCESSIBILITY_LABEL")];
   
    DelayInSec(1);
    
    sendButton.tap();
	LogMessage("ConversationLib :: SendIM :: message sent : " + message);

}

function InviteParticipant(buddy)
{
	LogMessage("ConversationLib::InviteParticipant");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var navBar = mainWindow.navigationBars()[1];
	IsValidAndVisible(navBar, "Conversation Window Navigation Bar");
	
	var actionButton = navBar.buttons()[navBar.buttons().length - 1];
	IsValidAndVisible(actionButton, "Action Button");
	actionButton.tap();
	
	var poppoverWindow = mainWindow.popover();
	IsValidAndVisible(poppoverWindow, "PopOver Window");
	
	var inviteOthersButton = poppoverWindow.actionSheet().buttons()[GetValueFromKey("LOCID Invite Others")];
	IsValidAndVisible(inviteOthersButton, "Invite Others Action Button");
	inviteOthersButton.tap();
	DelayInSec(2);
	
	var inviteParticipantsWindow = mainWindow.tableViews()[mainWindow.tableViews().length - 1];
	IsValidAndVisible(inviteParticipantsWindow, "Invite Others Window");
	
	var searchBar = inviteParticipantsWindow.searchBars()[0];
	searchBar.setValue(buddy);
	
	var searchResultsView = mainWindow.tableViews()[mainWindow.tableViews().length - 1];
	IsValidAndVisible(searchResultsView, "Search Results View");
	
	target.pushTimeout(60);
	var contactCell = searchResultsView.cells()[1];
	target.popTimeout();
	DelayInSec(2);
	
	IsValidAndVisible(contactCell, "Buddy Contact displayName : " + buddy);
	if(contactCell.name().indexOf(buddy) >= 0)
	{
		LogMessage("ConversationLib :: InviteParticipant :: User " + buddy + " found, Tapping on it...");
		contactCell.tap();
	}
	else
		throw new Error("Searched for : " + displayName + " Found " + contactCell.name());
}

function AddAudioModalityToImConf()
{
	LogMessage("ConversationLib::AddAudioModalityToImConf");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var navBar = mainWindow.navigationBars()[1];
	IsValidAndVisible(navBar, "Conversation Window Navigation Bar");
	
	var voiceButton = navBar.buttons()[VOICE_MODALITY_BUTTON];
	IsValidAndVisible(voiceButton, "Voice button");
	voiceButton.tap();
	
	var poppoverWindow = mainWindow.popover();
	IsValidAndVisible(poppoverWindow, "PopOver Window");
	
	var confCallButton = poppoverWindow.actionSheet().buttons()[GetValueFromKey("LOCID Conference Call")];
	IsValidAndVisible(confCallButton, "Conference call");
	confCallButton.tap();
}

function TapOnMuteButton()
{
	LogMessage("ConversationLib::TapMuteButton");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	var muteButton = mainWindow.buttons()["mute"];
	IsValidAndVisible(muteButton, "Mute Button");
	muteButton.tap();
	DelayInSec(5);
}

function GetMessageBubbleCount()
{
	LogMessage("ConversationLib :: GetMessageBubbleCount");
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	var messageView = mainWindow.scrollViews()[0].tableViews()[0];
	var numberOfBubbles = messageView.cells().length;
	return numberOfBubbles;
}

function VerifyReceivedIM(senderDisplayName , receivedMessage)
{
	// TODO : Not verifying sender's name as of now because of bug , sending unique messages from Desktop Lync Client
	LogMessage("ConversationLib :: VerifyReceivedIM");
	LogMessage("ConversationLib :: Looking for Message " + receivedMessage + " from " + senderDisplayName);
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();

	var messageView = mainWindow.scrollViews()[0].tableViews()[0];
	var queryString = "name CONTAINS '" + receivedMessage + "'";
	target.pushTimeout(60);
	var receivedMesssgeCell = messageView.cells().withPredicate(queryString);
	target.popTimeout();
	// TODO : In Conference Join the joined message come more than once , replace >= with == once bug is fixed
	if(receivedMesssgeCell.length >= 1)
	{
	 	IsValidAndVisible(receivedMesssgeCell[0],receivedMessage);
	 	UIALogger.logMessage("ConversationLib :: VerifyReceivedIM :: Message received");
	}
	else
	{
		throw new Error("Message not received")
	}
}

function DeleteAllConversations()
{

   
	var mainWindow = Target.frontMostApp();
	var editButton = mainWindow.navigationBar().rightButton();
	if(editButton.isVisible() && editButton.isEnabled())
	{
   
		DelayInSec(1);
		editButton.tap();
		DelayInSec(1);
 
        mainWindow.toolbar().buttons()[0].tap();
		DelayInSec(1);
	
		mainWindow.toolbar().buttons()[1].tap();
	}else {
        
        editButton = mainWindow.navigationBar().leftButton();
        if(editButton.isVisible() && editButton.isEnabled())
        {
           
            DelayInSec(1);
            editButton.tap();
            DelayInSec(1);
            
       
            mainWindow.toolbar().buttons()[0].tap();
            DelayInSec(1);
            
            mainWindow.toolbar().buttons()[1].tap();
        }
    }
}

function WaitforConversationWindow()
{
	LogMessage("ConversationLib :: WaitforConversationWindow");
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	target.pushTimeout(10);
	var conversationWindow = mainWindow.scrollViews()[0];
	target.popTimeout();
	IsValidAndVisible(conversationWindow, "Conversation Window");
	
	//Delay for 1 sec for textbox to show up
	DelayInSec(1);
	var IMtextbox = conversationWindow.textFields()[0];
	IsValidAndVisible(IMtextbox, "IM textBox");
}

//
function WaitforToastDissMissButton(string)
{
    var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
    
    for(var i = 0;i < 65;i++)
	{
		target.pushTimeout(2);
		var element = mainWindow.buttons()[string];
		target.popTimeout();
        
        if(element.isValid() == true && element.isVisible() == true)
		{
            return element;
        }
        
		DelayInSec(1);
	}
}


function ToastDissMissButtonAppear(string)
{
    var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
    
    for(var i = 0;i < 65;i++)
	{
		target.pushTimeout(1);
		var element = mainWindow.buttons()[string];
		target.popTimeout();
        
        if(element.isValid() == true && element.isVisible() == true)
		{
            return true;
        }
        
		DelayInSec(1);
	}
    
    return false;
}

function WaitforToastViewAppear()
{
    var target = UIATarget.localTarget();
   
    for(var i = 0;i < 100;i++)
    {
       
        var element = target.frontMostApp().windows()[3].elements()[0].staticTexts()[1];
    
        if(element.isValid() == true && element.isVisible() == true)
        {
            return element;
        }
        LogMessage("MeetingsLib :: WaitforToastViewAppear");

        DelayInSec(0.2);
    }
    
    return;
}



function OpenConferenceImWithSubject(subject)
{
	LogMessage("MeetingsLib :: OpenMeetingWithSubject :: Tap on meeting with subject : " + subject);
    
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	var staticText = mainWindow.staticTexts()[subject];
    staticText.tapWithOptions({tapOffset:{x:0.22, y:0.21}});
    DelayInSec(1);
}

//

function WaitforAudioWindow()
{
	LogMessage("ConversationLib :: WaitforAudioWindow");
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	target.pushTimeout(60);
	var audioWindow = mainWindow.scrollViews()[0];
	target.popTimeout();
	
	if(audioWindow.isValid())
		LogMessage("ConversationLib :: WaitforAudioWindow :: Audio Modality view is up");
	else
		throw new Error("ConversationLib :: WaitforAudioWindow :: Audio Modality view is not valid");
}

function ValidateAudioControls()
{
	LogMessage("ConversationLib :: ValidateAudioControls");
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();

	var audioWindow = mainWindow.scrollViews()[0];
	
	LogMessage("ConversationLib :: ValidateAudioControls :: Keypad button");
	var keypadButton = audioWindow.buttons()["cc keypad"];
	IsValidAndVisible(keypadButton, "KeyPad button");
	
	
	LogMessage("ConversationLib :: ValidateAudioControls :: Speaker button");
	var speakerButton = audioWindow.buttons()["cc speaker"];
	IsValidAndVisible(speakerButton, "SpeakerPad button");
	
	
	LogMessage("ConversationLib :: ValidateAudioControls :: Hold button");
	var holdButton = audioWindow.buttons()["cc hold"];
	IsValidAndVisible(holdButton, "Hold button");
	
	
	LogMessage("ConversationLib :: ValidateAudioControls :: Call End button");
	var callEndButton = audioWindow.buttons()["cc call end"];
	IsValidAndVisible(callEndButton, " Call End button");
	
}

function TapOnAudioControl(controlName)
{
	LogMessage("ConversationLib :: TapOnAudioControl");
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var audioWindow = mainWindow.scrollViews()[0];
	
	var controlButton = audioWindow.buttons()[controlName];
	IsValidAndVisible(controlButton, "Audio Control button : " +  controlName);
	controlButton.tap();
	DelayInSec(2);
}


function GotoModalityView(viewName)
{
	LogMessage("ConversationLib :: GotoModalityView");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();

	var conversationNavigationIndex = mainWindow.navigationBars().length - 1;
	var viewButton = mainWindow.navigationBars()[conversationNavigationIndex].buttons()[viewName];
	IsValidAndVisible(viewButton, "view button : " + viewName);
	viewButton.tap();
	DelayInSec(2);
}

function EndConversation()
{
	LogMessage("ConversationLib::EndConversation");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var conversationNavigationIndex = mainWindow.navigationBars().length - 1;
	var navBar = mainWindow.navigationBars()[conversationNavigationIndex];
	IsValidAndVisible(navBar, "Navigation Bar");
	
	var actionButton = navBar.buttons()[navBar.buttons().length - 1];
	IsValidAndVisible(actionButton, "Action Button");
	actionButton.doubleTap();
	
	var poppoverWindow = mainWindow.popover();
	IsValidAndVisible(poppoverWindow, "PopOver Window");
	
	var endConversationButton = poppoverWindow.actionSheet().buttons()[GetValueFromKey("LOCID End Conversation")];
	IsValidAndVisible(endConversationButton, "End Conversation");
	
	endConversationButton.doubleTap();
	DelayInSec(2);
}

function VerifyEmptyConversationList()
{
	LogMessage("ConversationLib :: VerifyEmptyConversationList");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	target.pushTimeout(10);
	var emptyList = mainWindow.tableViews()[0].cells()[GetValueFromKey("LOCID No Conversations")];
	target.popTimeout();
	
	IsValidAndVisible(emptyList, "Empty conversation List");
}

function TapOnConversationInList(index)
{
	LogMessage("ConversationLib :: TapOnConversationInList");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	if(index == 0)	
		FlickCallBarIfPresent();
	
	target.pushTimeout(10);
	var conversation = mainWindow.tableViews()[0].cells()[index];
	target.popTimeout();
	
	IsValidAndVisible(conversation, "Conversation with index : " + index);
	DelayInSec(1);
	
	conversation.tap();
}

function TapOnOptionsMenuItem(menuItem)
{
	LogMessage("ConversationLib :: TapOnOptionsMenuItem");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var navBar = mainWindow.navigationBars()[1];
	IsValidAndVisible(navBar, "Conversation Window Navigation Bar");
	var actionButton = navBar.buttons()[navBar.buttons().length - 1];
	IsValidAndVisible(actionButton, "Action Button");
	
	DelayInSec(1);
	actionButton.tap();
	
	var poppoverWindow = mainWindow.popover();
	IsValidAndVisible(poppoverWindow, "PopOver Window");
	
	var menuItemButton = poppoverWindow.actionSheet().buttons()[menuItem];
	IsValidAndVisible(menuItemButton, "Menu Item : " + menuItem);
	
	DelayInSec(1);
	menuItemButton.tap();
}

function TapOnAudioCallInConversationWindow(callType)
{
	LogMessage("ConversationLib :: TapOnAudioCallInConversationWindow");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	GotoModalityView(VOICE_MODALITY_BUTTON);
	
	target.pushTimeout(10);
	var poppoverWindow = mainWindow.popover();
	target.popTimeout();
	
	IsValidAndVisible(poppoverWindow, "PopOver Window");
	
	var callType = poppoverWindow.actionSheet().buttons()[callType];
	IsValidAndVisible(callType, "Call Type : " + callType);
	
	DelayInSec(1);
	callType.tap();
}

function VerifyRosterParticipants(buddyList)
{
	LogMessage("ConversationLib::VerifyRosterParticipants");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var navBar = mainWindow.navigationBars()[1];
	IsValidAndVisible(navBar, "Conversation Window Navigation Bar");
	var actionButton = navBar.buttons()[navBar.buttons().length - 1];
	IsValidAndVisible(actionButton, "Action Button");
	actionButton.tap();
	
	var popOverWindow = mainWindow.popover();
	IsValidAndVisible(popOverWindow, "PopOver Window");
	
	var viewParticipantsButton = popOverWindow.actionSheet().buttons()[GetValueFromKey("LOCID View Participants")];
	IsValidAndVisible(viewParticipantsButton, "See Participants Button");
	viewParticipantsButton.tap();
	DelayInSec(2);
	
	var participantsWindow = mainWindow.tableViews()[mainWindow.tableViews().length - 1];
	IsValidAndVisible(participantsWindow, "See Participants Window");
	
	var numberOfParticipants = participantsWindow.cells().length;
	var expectedNumOfParticipants = buddyList.split(",").length;
	
	var navBarWindow = mainWindow.navigationBars()[mainWindow.navigationBars().length - 1];
	IsValidAndVisible(navBarWindow, "Navigation Window");
	var cancelButton = navBarWindow.buttons()[GetValueFromKey("LOCID Cancel")];
	IsValidAndVisible(cancelButton, "Cancel Button");
	cancelButton.tap();
	
	if(expectedNumOfParticipants+1 == numberOfParticipants )//Including Self
	{
		LogMessage("ConversationLib :: VerifyRosterParticipants :: Roster updated to correct number of participants");
	}
	else
		throw new Error("Roster not Updated to Correct Number of participants");
}

function EndAllConversations()
{
	LogMessage("ConversationLib :: EndAllConversations");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var conversationLength = mainWindow.tableViews()[0].cells().length;
	for(var i=0; i < conversationLength; i++)
	{
		TapOnConversationInList(i);
		try
		{
			EndConversation();
		}
		catch(error)
		{
			LogMessage("ConversationLib :: EndAllConversations :: Conversation " + (i+1) + " is already ended");
			HideKeyboard();
			mainWindow.tap();
			DelayInSec(2);
		}	
	}

}

function ConversationsInList()
{
	LogMessage("ConversationLib :: ConversationsInList");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var conversationLength = mainWindow.tableViews()[0].cells().length;
	
	return conversationLength;
}

function VerifyMissedConversationBadging(badgingValue)
{
	LogMessage("ConversationLib :: VerifyMissedConversationBadging");
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	if(badgingValue > 0)
	{
	
	   target.pushTimeout(30);
	   var badgingIcon = mainWindow.staticTexts()[0];
	   target.popTimeout();
	
	   for(i=1;i<=15;i++)
	   {	
	       try
	       {
	          IsValidAndVisible(badgingIcon, "Badging for Missed Conversation");
	          if(badgingIcon.value() == badgingValue)
	             break; 
	       }
	       catch(error)
	       {}
	       DelayInSec(2);
	       badgingIcon = mainWindow.staticTexts()[0];
	   }
	
	   if(badgingIcon.value() != badgingValue)
	       throw new Error("Expected value of badging not found");
	   LogMessage("ConversationLib :: VerifyMissedConversationBadging :: Badging Visible with value :" + badgingValue.toString()); 
	}
	else
	{
	   for(i=1;i<=15;i++)
	   {	
	       if(!mainWindow.staticTexts()[0].isVisible())
	          break;
	       DelayInSec(2);
	   }
	   if(mainWindow.staticTexts()[0].isVisible())
	       throw new Error("Badging not expected but badging found");
	   LogMessage("ConversationLib :: VerifyMissedConversationBadging :: NO Badging");
	}
	
}

function WaitForCallInviteToExpire()
{
    LogMessage("ConversationLib :: WaitForCallInviteToExpire");
    
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	var buttonType = AcceptString;
	
	target.pushTimeout(90);
	var acceptRejectButton = mainWindow.buttons()[buttonType];
	target.popTimeout();
	
	IsValidAndVisible(acceptRejectButton,"Call Toast Button Type -  " + buttonType);
	
	//Timeout for Automatic expiry is 15 sec ... waiting for a max of 16 sec
	for(i=1;i<=16;i++)
	{
	    acceptRejectButton = mainWindow.buttons()[buttonType];
	    if(!acceptRejectButton.isValid())
	       break;
	    DelayInSec(1);
	}
	DelayInSec(1);
}

function WaitForIMInviteToExpire()
{    
    LogMessage("ConversationLib :: WaitForIMInviteToExpire");
    
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	DismissNotificationsIfAny();
	
	target.pushTimeout(30);
	var imDismissButton = mainWindow.buttons()["notification btn close X"];
	target.popTimeout();
	
	IsValidAndVisible(imDismissButton,"IM Dismiss Button");
	
	LogMessage("ConversationLib :: WaitForIMInviteToExpire ::  up to 60 sec");
	for(i=1;i<=30;i++)
	{
	    if(!mainWindow.buttons()["notification btn close X"].isValid())
	       break;
	    DelayInSec(2);
	}
	
    if(mainWindow.buttons()["notification btn close X"].isValid())	
       throw new Error("IM Invite did not expire");
}

function DismissIMInvite()
{
    LogMessage("ConversationLib :: DismissIMInvite");
    var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	DismissNotificationsIfAny();
	
	LogMessage("ConversationLib :: DismissIMInvite :: Waiting for Incoming IM");
	target.pushTimeout(30);
	var imDismissButton = mainWindow.buttons()["notification btn close X"];
	target.popTimeout();
	
	IsValidAndVisible(imDismissButton,"IM Dismiss Button");
	imDismissButton.tap();   
}

function VerifyMissedTagInConversation(conversationIndex,convType)
{
    LogMessage("ConversationLib :: VerifyMissedTagInConversation");
    
	var mainWindow = UIATarget.localTarget().frontMostApp().mainWindow();
	var chat = mainWindow.tableViews()[0].cells()[conversationIndex].staticTexts()[0];
	var localizedMissedTag ="";
	
	convType = convType.toUpperCase();
	switch(convType)
	{
	case "AUDIO" :
	  localizedMissedTag = GetValueFromKey("LOCID Missed Conversation");
	  break;
	case "IM" :
	case "CONF" :
	  localizedMissedTag = GetValueFromKey("LOCID Missed").replace("%@","");
	  break;
	default:
	  throw new Error("Invalid Conv Type :" + convType);
	}
	
	if(chat.value().indexOf(localizedMissedTag)<0)
	  throw new Error("Chat is not marked as Missed");
	  
	LogMessage("ConversationLib :: VerifyMissedTagInConversation :: " + localizedMissedTag + " found");
}

//Video BVTs
function TapVideoMenuButton() {
    LogMessage("ConversationLib::TapVideoMenuButton");

    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow();
    var videoMenuButton = mainWindow.scrollViews()[0].buttons()["Video Menu"];
    IsValidAndVisible(videoMenuButton, "Video Menu Button");
    videoMenuButton.tap();
    DelayInSec(2);
}

function VerifyVideoControls() {
    LogMessage("ConversationLib::VerifyVideoControls");
    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow();

    var poppoverWindow = mainWindow.popover();
    IsValidAndVisible(poppoverWindow, "PopOver Window");

    var switchCameraButton = poppoverWindow.actionSheet().buttons()[GetValueFromKey("LOCID Switch Camera")];
    IsValidAndVisible(switchCameraButton, "Switch Camera Action Button");

    var myCameraOffButton = poppoverWindow.actionSheet().buttons()[GetValueFromKey("LOCID My Camera Off")];
    IsValidAndVisible(myCameraOffButton, "My Camera Off Action Button");

    var stopVideoButton = poppoverWindow.actionSheet().buttons()[GetValueFromKey("LOCID Stop Video")];
    IsValidAndVisible(stopVideoButton, "Stop Video Action Button");
}

function TapOnVideoMenuControl(control) {
    LogMessage("ConversationLib::TapOnVideoMenuControl");
    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow();

    var poppoverWindow = mainWindow.popover();
    IsValidAndVisible(poppoverWindow, "PopOver Window");

    var controlButton = poppoverWindow.actionSheet().buttons()[control];
    IsValidAndVisible(controlButton, "Control " + control + " Button");

    controlButton.tap();
}

function CaptureVideoScreenshot(screenshotName) {
    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow();
    CaptureRectScreenShot(mainWindow.scrollViews()[0].rect(), screenshotName);
}


function WaitForNavigationButtonVisible(string)
{
    
    DismissNotificationsIfAny();
    var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
    var navigationBar = target.frontMostApp().navigationBar();
    
    var isVisible = false;
    for(var i = 0;i < 120;i++)
	{
        mainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.23, y:0.25}});
       
        if(target.frontMostApp().navigationBar().buttons()[string].isVisible() == true)
		{
            
            return true;
        }
        else
        
            DismissNotificationsIfAny();
            DelayInSec(0.5);
    }
    
    return isVisible
    
}

function WaitForNavigationButtonEnable(string)
{
    DismissNotificationsIfAny();
    var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
    var navigationBar = target.frontMostApp().navigationBar();
    
    var navigationButton = navigationBar.buttons()[string];
    var isVisible = false;
    for(var i = 0;i < 60;i++)
	{
        mainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.23, y:0.25}});
        if(navigationBar.buttons()[string].isValid() == true && navigationBar.buttons()[string].isEnabled() == true)
		{
            
            return true;
        }
        else
            DismissNotificationsIfAny();
		    DelayInSec(1)
    }
    
    return isVisible
    
}

function TapForNavigationButton(string)
{
    DismissNotificationsIfAny();
    var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
    var navigationBar = target.frontMostApp().navigationBar();
    
    var navigationButton = navigationBar.buttons()[string];
    var isVisible = false;
    for(var i = 0;i < 60;i++)
	{
        mainWindow.scrollViews()[0].tapWithOptions({tapOffset:{x:0.83, y:0.49}});
        if(navigationButton.isValid() == true && navigationButton.isEnabled() == true)
		{
            navigationButton.tap();
            return true;
        }
        else
            DismissNotificationsIfAny();
            DelayInSec(1)
    }
    
    return isVisible
    
}

function WaitForMainViewElementValidAndVisible(viewElement,string)
{
	var isVisible = false;

    for(var i = 0;i < 30;i++)
	{
        
        
        viewElement.tapWithOptions({tapOffset:{x:0.83, y:0.49}});
        element = viewElement.elements()[string];
        if(element.isValid() == true && element.isVisible() == true)
		{
            
            return true;
        }
        else
		    DelayInSec(1);
    }
    
    return isVisible
}

function WaitForMainViewAlertMessageVisible(viewElement,string)
{
    
    for(var i = 0;i < 30;i++)
	{
        viewElement.tapWithOptions({tapOffset:{x:0.83, y:0.49}});
        alertMessage = viewElement.staticTexts()[string];
        
        if(alertMessage.isValid() == true && alertMessage.isVisible() == true)
		{
            
            return alertMessage;
        }
        else
		    DelayInSec(1);
    }
    
    return null;
    
}


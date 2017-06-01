function SetNote(note)
{
	LogMessage("MyInfoLib :: SetNote");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	// try 3 times max to set the note
	for( var count = 1 ; count <= 3 ; count++)
	{
		try
		{
            GotoMyInfo();
			DismissNotificationsIfAny();
			var note_field = mainWindow.tableViews()[0].textViews()[0];
            
			DismissNotificationsIfAny();
			
			LogMessage("MyInfoLib :: SetNote :: Setting Note to " + note);
			note_field.tap();
			note_field.setValue(note);
			DelayInSec(2);
			return;
		}
		catch(error)
		{
			LogMessage("MyInfoLib :: SetNote :: " + error);
			LogMessage("MyInfoLib :: SetNote :: Error in setting note , retrying");
			GotoChats();
            DeleteAllConversations();
            GotoMyInfo();
		}
	}
	throw new Error("Unable to set Note " + note);
}

function SetHappeningNote(note)
{

	var note_emptyfield = MainWindow.tableViews()[1].cells()[GetValueFromKey("LOCID MY_INFO_EMPTY_MOOD_MESSAGE")];
    var note_field = MainWindow.tableViews()[1].cells()[GetValueFromKey("LOCID MY_INFO_MOOD_MESSAGE")]
    
    if(note_field.isVisible() == true) {
        
        note_field.tap();
        
    }else {
        
        note_emptyfield.tap();
    }

    var scrollView = MainWindow.scrollViews()[GetValueFromKey("MOOD_MESSAGE_ACCESSIBILITY_LABEL")];
    scrollView.setValue(note);
        
    var done_Button = Target.frontMostApp().navigationBar().buttons()[GetValueFromKey("LOCID Done")];
    done_Button.tap();
    DelayInSec(1);
    
    var dissmiss_button = Target.frontMostApp().navigationBar().buttons()[GetValueFromKey("DISMISS_BUTTON")];
    if(dissmiss_button.isValid() == true && dissmiss_button.isVisible() == true) {
        
        dissmiss_button.tap();
        DelayInSec(1);
    }

}


function ResetNoteIfNeeded()
{
	for( var count = 1 ; count <= 3 ; count++)
	{
        
        try{
            
			var note_field = MainWindow.tableViews()[1].cells()[1];
			ElementValidAndVisible(note_field);
          
			if(note_field.name() != GetValueFromKey("LOCID MY_INFO_EMPTY_MOOD_MESSAGE"))
			{
				note_field.tap();
                DelayInSec(1);
                
                var scrollView = MainWindow.scrollViews()[GetValueFromKey("MOOD_MESSAGE_ACCESSIBILITY_LABEL")];
                
                scrollView.setValue("");
                
                var done_Button = Target.frontMostApp().navigationBar().buttons()[GetValueFromKey("LOCID Done")];
                done_Button.tap();
				DelayInSec(1);
			}
            
            var dissmiss_button = Target.frontMostApp().navigationBar().buttons()[GetValueFromKey("DISMISS_BUTTON")];
            if(dissmiss_button.isVisible() == true && dissmiss_button.isValid() == true) {
                
                dissmiss_button.tap();
            }
            
			return;
		}
		catch(error)
		{
//			GotoChats();
//            DeleteAllConversations();
			GotoMyInfo();
		}
	}
}


function VerifySelfPresence(presenceString , presenceIconString)
{
	LogMessage("MyInfoLib :: VerifySelfPresence");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var statusString = GetValueFromKey("LOCID Status") + ", " + presenceString;
	LogMessage("MyInfoLib :: VerifySelfPresence :: Looking for status string " + statusString);
	
	target.pushTimeout(30);
		var statusControl = mainWindow.tableViews()[0].cells()[statusString];
	target.popTimeout();
	IsValidAndVisible(statusControl,statusString);
	
	var presenceText = mainWindow.tableViews()[0].staticTexts()[1];
	IsValidAndVisible( presenceText, "Presence Text");
	var presenceIcon = mainWindow.tableViews()[0].images()[1];
	
	if(presenceText.value() == presenceString && presenceIcon.name() == presenceIconString)
		LogMessage("MyInfoLib :: VerifySelfPresence :: Correct Self Presence Set");
	else
		throw new Error("Incorrect Self Presence Set");
}

function ChangeSelfPresence(currentPresenceString , presenceToSet)
{
	LogMessage("MyInfoLib :: ChangePresence");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var statusString = GetValueFromKey("LOCID Status") + ", " + currentPresenceString;
	LogMessage("MyInfoLib :: ChangePresence :: Looking for status string " + currentPresenceString);
	
	var statusControl = mainWindow.tableViews()[0].cells()[statusString];

	IsValidAndVisible(statusControl,currentPresenceString);
	statusControl.tap();
	
	var indexOfPresenceTableView = mainWindow.tableViews().length - 1 ;
	var presenceToSetControl = mainWindow.tableViews()[indexOfPresenceTableView].cells()[presenceToSet];
	IsValidAndVisible(presenceToSetControl,presenceToSet);
	presenceToSetControl.tap();
}

/*function VerifySelfPresence()
{
	LogMessage("MyInfoLib::VerifySelfPresence");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	DelayInSec(2);
	
	target.pushTimeout(0);
		var availableStatusChangeControl = mainWindow.tableViews()[0].cells()["Status, Available"];
	target.popTimeout();
	IsValidAndVisible(availableStatusChangeControl, "Available Status ");
	availableStatusChangeControl.tap();
	
	var index = mainWindow.tableViews().length - 1 ;
	var busyStatusControl = mainWindow.tableViews()[index].cells()["Busy"];
	IsValidAndVisible( busyStatusControl, "Busy StatusControl");
	busyStatusControl.tap();
	
	//Wait for presence Update on iLync..
	target.pushTimeout(15);
		var busyStatusChangeControl = mainWindow.tableViews()[0].cells()["Status, Busy"];
	target.popTimeout();
	IsValidAndVisible( busyStatusChangeControl, "Busy Status String");
	
	presenceString = mainWindow.tableViews()[0].staticTexts()[1];
	IsValidAndVisible( presenceString, "Presence String");
	var presenceIcon = mainWindow.tableViews()[0].images()[5];
	
	if(presenceString.value() == "Busy" 
		&& presenceIcon.name() == "Icon_gbl_pres_busy_48.png")
		LogMessage("MyInfoLib::VerifyPresence:Status changed to Busy from iLync ");
	else
		throw new Error("Status not updated on iLync");
	
	//Wait for Update from remote EndPoint
	target.pushTimeout(10);
		var statusAvailable = mainWindow.tableViews()[0].cells()["Status, Available"];
	target.popTimeout();
	
	IsValidAndVisible( statusAvailable, "Available Status ");
	if(presenceString.value() == "Available" && presenceIcon.name() == "Icon_gbl_pres_available_48.png")
		LogMessage("MyInfoLib::VerifyPresence:Status changed to Available from other MPOP point");
	else
		throw new Error("MPOP BOT status change failure");
	
	LogMessage("MyInfoLib::VerifyPresence::Presence Icon and String Updates Verified");
}*/
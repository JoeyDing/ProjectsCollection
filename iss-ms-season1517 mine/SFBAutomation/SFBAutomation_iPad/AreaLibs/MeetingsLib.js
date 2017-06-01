function tapAllMeetingButton()
{
	LogMessage("MeetingsLib :: tapAllMeetingButton");

	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	//Tap on All button	
	var allMeetingButton = mainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID All")];
	IsValidAndVisible(allMeetingButton, "'All' meeting button");
	allMeetingButton.tap();
}

function tapOnlineMeetingbutton()
{
	LogMessage("MeetingsLib :: tapOnlineMeetingbutton");

	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	//Tap on online button	
	var onLineMeetingButton = mainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Online")];
	IsValidAndVisible(onLineMeetingButton, "'Online' meeting button");
	onLineMeetingButton.tap();
}


function OpenMeeting(index)
{
	LogMessage("MeetingsLib :: OpenMeeting :: Tap on meeting no. : " + index);

	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	tapAllMeetingButton();
	
	//Tap on Meeting
	target.pushTimeout(30);
	var onlineMeetingCell = mainWindow.tableViews()[0].cells()[index -1];
	target.popTimeout();
	
	IsValidAndVisible(onlineMeetingCell, "No meeting present for the index : " + index );
	
	onlineMeetingCell.tap();
	
	DelayInSec(1);
}

function GetMeetingCount()
{
	LogMessage("MeetingsLib :: GetMeetingCount()");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	tapAllMeetingButton();
	
	return mainWindow.tableViews()[0].cells().length;	
}

function VerifyJoinButton()
{
	LogMessage("MeetingsLib :: VerifyJoinButton");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();	
	
	var tableViewLength = mainWindow.tableViews().length;
	target.pushTimeout(10);
	var joinButtonCell = mainWindow.tableViews()[tableViewLength - 1].cells()[GetValueFromKey("LOCID Join Meeting")];
	target.popTimeout();
	
	IsValidAndVisible(joinButtonCell, "Join Button");
		
}

function TapOnJoinButton()
{
	LogMessage("MeetingsLib :: TapOnJoinButton");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();	

	var tableViewLength = mainWindow.tableViews().length;
	target.pushTimeout(10);
	var joinButtonCell = mainWindow.tableViews()[tableViewLength - 1].cells()[GetValueFromKey("LOCID Join Meeting")];
	target.popTimeout();
		
	IsValidAndVisible(joinButtonCell, "Join Button");
		
	joinButtonCell.tap();
	DelayInSec(5);
	
}


function VerifyMeetingSubject(meetingSubject)
{
	LogMessage("MeetingsLib :: VerifyMeetingSubject :: subject : " + meetingSubject);
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var tableViewLength = mainWindow.tableViews().length;
	target.pushTimeout(10);
	var subject = mainWindow.tableViews()[tableViewLength - 1].groups()[meetingSubject];
	target.popTimeout();
	
	IsValidAndVisible(subject, "Meeting subject : " + meetingSubject);
	
}



//DFTs
function VerifyMeetingDetails(meetingSubject,meetingTime,meetingLocation,meetingOrganizer,meetingInvitees,meetingNote,meetingUrl)
{
	LogMessage("MeetingsLib :: VerifyMeetingDetails");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();	
	var tableViewLength = mainWindow.tableViews().length;
	
	target.pushTimeout(2);
	var mHeader = mainWindow.tableViews()[tableViewLength - 1].groups()[meetingSubject];
	IsValidAndVisible(mHeader, "Meeting Header : " + meetingSubject);
	
	LogMessage("MeetingsLib :: VerifyMeetingDetails :: "+meetingTime);
	var mTime = mHeader.staticTexts()[meetingTime];
	
	IsValidAndVisible(mTime, "Meeting Time : " + meetingTime);
	
	LogMessage("MeetingsLib :: VerifyMeetingDetails :: Location "+meetingLocation);
	var mLocation = mHeader.staticTexts()[meetingLocation];
	IsValidAndVisible(mLocation, "Meeting Location : " + meetingLocation);
	
	LogMessage("MeetingsLib :: VerifyMeetingDetails :: Organizer "+meetingOrganizer);
	var mOrganizer = mainWindow.tableViews()[tableViewLength - 1].cells()["Organizer, "+ meetingOrganizer];
	IsValidAndVisible(mOrganizer, "Meeting Time : " + meetingOrganizer);
	
	var groupLength = mainWindow.tableViews()[tableViewLength - 1].groups().length;
	var mDetail = mainWindow.tableViews()[tableViewLength - 1].groups()[groupLength-1];
	
	LogMessage("MeetingsLib :: VerifyMeetingDetails :: Invitees "+meetingInvitees);
	var mInviteeList = mDetail.staticTexts()[meetingInvitees];
	IsValidAndVisible(mInviteeList, "Meeting Invitee List : " + meetingInvitees);
	
	var mNote = mDetail.textViews()[ mDetail.textViews().length - 1];
	IsValidAndVisible(mNote, "Meeting Note ");
	LogMessage("MeetingsLib :: VerifyMeetingDetails :: MeetingNote "+meetingNote);
	var mNoteValue = mNote.value();
	target.popTimeout();
	if (mNoteValue.indexOf(meetingNote) == -1)
		throw new Error("Meeting Note not found");
	
	LogMessage("MeetingsLib :: VerifyMeetingDetails :: MeetingUrl "+meetingUrl);
	if (mNoteValue.indexOf(meetingUrl) == -1)
		throw new Error("Meeting URL not found in details");	
}

function VerifyLobbyJoinMessage()
{
	LogMessage("MeetingsLib :: VerifyLobbyJoinMessage");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	LogMessage("MeetingsLib :: VerifyLobbyJoinMessage :: Looking For Lobby Join Text");
	target.pushTimeout(10);
	var lobbyJoinMessageText = mainWindow.staticTexts()[lobbyJoinMessage];
	target.popTimeout();
	
	IsValidAndVisible(lobbyJoinMessageText, "Lobby message :  " + lobbyJoinMessage);
}


function GetMeetingURL(subject)
{
    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow();
    GotoMeetings();
    
    mainWindow.tableViews()[0].scrollUp();
    DelayInSec(15);
    
    TapTableviewCellWithPredicate(subject);
    
    target.pushTimeout(10);
    LogMessage("DataCollabLib :: GetMeetingURL :: meetingURL");
    var meetingInfo = mainWindow.tableViews()[0].groups()[GetValueFromKey("LOCID Invitees")].textViews()[0].value();
    target.popTimeout();
    
    var url = ExtractURL(meetingInfo);
    LogMessage("DataCollabLib :: GetMeetingURL :: meetingURL = " + url);
    
    var button = target.frontMostApp().navigationBar().leftButton();
    if(button.isValid()==true && button.isVisible()==true){
        
        button.tap();
    }
    DelayInSec(1);
    return url;
}

function ExtractURL(meetingInfo)
{
    var joinOnlineMeeting = "加入联机会议";
    var joinLyncMeeting = "Join Lync Meeting";
    var startIndex;
  
    if(meetingInfo.indexOf(joinOnlineMeeting) >= 0)
    {
        
        startIndex = meetingInfo.indexOf(joinOnlineMeeting) + joinOnlineMeeting.length;
    }
    else if (meetingInfo.indexOf(joinLyncMeeting) >= 0)
    {
        startIndex = meetingInfo.indexOf(joinLyncMeeting) + joinLyncMeeting.length;
    }
    else
    {
        throw new Error ("Could not find Meeting URL");
    }
    
    startIndex = meetingInfo.indexOf('<',startIndex);
    var endIndex = meetingInfo.indexOf('>',startIndex);
    return meetingInfo.substr(startIndex +1 ,(endIndex - startIndex - 1));
}





















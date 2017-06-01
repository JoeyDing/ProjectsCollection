function OpenContactCardfromSearch(displayName)
{
	LogMessage("ContactsAndGroupLib :: OpenContactCardfromSearch :: " + "displayName : " + displayName);
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	var isContactFound = true;
	
	LogMessage("ContactsAndGroupLib :: OpenContactCardfromSearch :: Looking for Search Box");
	var searchBox = mainWindow.searchBars()[0];
	IsValidAndVisible(searchBox, "search box");
	
	searchBox.tap();
	LogMessage("ContactsAndGroupLib :: OpenContactCardfromSearch :: Search For User " + displayName);
	searchBox.setValue(displayName);
	DelayInSec(1);
	HideKeyboard();
	
	var searchView = mainWindow.tableViews()[0];
	target.pushTimeout(20);
	var contactCell = searchView.cells()[1];
	target.popTimeout();
	
	try
	{
		IsValidAndVisible(contactCell, "Buddy : " + displayName);
	}
	catch(error)
	{
		isContactFound = false;
	}
	
	if(isContactFound)
	{
	
		if(contactCell.name().indexOf(displayName) >= 0)
		{
			LogMessage("ContactsAndGroupLib :: OpenContactCardfromSearch :: Buddy " + displayName + " found");
			contactCell.tap();
		}
		else
		{
			isContactFound = false;
			LogMessage("ContactsAndGroupLib :: OpenContactCardfromSearch :: Buddy " + displayName + " not found");
			LogMessage("ContactsAndGroupLib :: OpenContactCardfromSearch ::  " + contactCell.name() + " found");
		}
	}
	
	var clearTextButton = searchBox.buttons()[0];
	IsValidAndVisible(clearTextButton);
	clearTextButton.tap();
	DelayInSec(1)
	
	var cancelButton = mainWindow.buttons()[GetValueFromKey("LOCID Cancel")];
	IsValidAndVisible(cancelButton);
	cancelButton.tap();
	DelayInSec(1)
	
	
	if(isContactFound == false)
	{
		throw new Error("ContactsAndGroupLib :: OpenContactCardfromSearch :: Failed to search buddy " + 	displayName);
	}
}

function VerifySearchResults(buddyToSearch)
{
	LogMessage("ContactsAndGroupLib :: VerifySearchResults :: " + "Will Search for : " + buddyToSearch);
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	var isContactFound = true;
	
	var searchBox = mainWindow.searchBars()[0];
	IsValidAndVisible(searchBox, "Search Box");
	searchBox.tap();
	searchBox.setValue(buddyToSearch);
	HideKeyboard();
	DelayInSec(1);
	
	
	var searchView = mainWindow.tableViews()[0];
	target.pushTimeout(60);
	var contactCell = searchView.cells()[1];
	target.popTimeout();
	
	try
	{
		IsValidAndVisible(contactCell, "Buddy : " + buddyToSearch);
	}
	catch(error)
	{
		isContactFound = false;
	}
	
	if(isContactFound)
	{
	
		if(contactCell.name().indexOf(buddyToSearch) >= 0)
		{
			LogMessage("ContactsAndGroupLib :: VerifySearchResults :: Buddy " + buddyToSearch + " found");
		}
		else
		{
			isContactFound = false;
			LogMessage("ContactsAndGroupLib :: VerifySearchResults :: Buddy " + buddyToSearch + " not found");
			LogMessage("ContactsAndGroupLib :: VerifySearchResults ::  " + contactCell.name() + " found");
		}
	}
	
	var clearTextButton = searchBox.buttons()[0];
	IsValidAndVisible(clearTextButton);
	clearTextButton.tap();
	DelayInSec(1)
	
	var cancelButton = mainWindow.buttons()[GetValueFromKey("LOCID Cancel")];
	IsValidAndVisible(cancelButton);
	cancelButton.tap();
	DelayInSec(1)
	
	
	if(isContactFound == false)
	{
		throw new Error("ContactsAndGroupLib :: VerifySearchResults :: Failed to search buddy " + 	buddyToSearch);
	}
}

function TapIMbutton()
{
	LogMessage("ContactsAndGroupLib :: TapIMbutton");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	LogMessage("ContactsAndGroupLib :: TapIMbutton :: looking for IM button");
	target.pushTimeout(10);
	var IMButton = mainWindow.tableViews()[mainWindow.tableViews().length - 1 ].buttons()[GetValueFromKey("LOCID IM")];
	target.popTimeout();
	
	IsValidAndVisible(IMButton, "IM button");
	IMButton.tap();
	
	WaitforConversationWindow();

}

function TapCallbutton()
{
	LogMessage("ContactsAndGroupLib :: TapCallbutton");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	LogMessage("ContactsAndGroupLib :: TapCallbutton :: looking for Call button");
	target.pushTimeout(10);
	
	var callButtonString = GetValueFromKey("LOCID Call");
	if(callButtonString == "")
		callButtonString = 1;
	var callButton = mainWindow.tableViews()[mainWindow.tableViews().length - 1 ].buttons()[callButtonString];
	target.popTimeout();
	
	IsValidAndVisible(callButton, "Call button");
	callButton.tap();
	
}

function TapVideobutton()
{
	LogMessage("ContactsAndGroupLib :: TapVideobutton");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	LogMessage("ContactsAndGroupLib :: TapVideobutton :: looking for Video button");
	target.pushTimeout(10);
	
	var videoButtonString = GetValueFromKey("LOCID Video");
	if(videoButtonString == "")
		videoButtonString = 2;
	var videoButton = mainWindow.tableViews()[mainWindow.tableViews().length - 1 ].buttons()[videoButtonString];
	target.popTimeout();
	
	IsValidAndVisible(videoButton, "Video button");
	videoButton.tap();
	
}

function TapEmailButton()
{
	LogMessage("ContactsAndGroupLib :: TapEmailButton");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	LogMessage("ContactsAndGroupLib :: TapEmailButton :: looking for Email button");
	target.pushTimeout(10);
	var emailButton = mainWindow.tableViews()[mainWindow.tableViews().length - 1 ].buttons()[GetValueFromKey("LOCID E-Mail")];
	target.popTimeout();
	
	IsValidAndVisible(emailButton, "Email button");
	emailButton.tap();
	
}

function IsContactListVisible()
{
	LogMessage("ContactsLib::IsContactListVisible");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	target.pushTimeout(10);
		var contact_list = mainWindow.tableViews()[0].groups();
	target.popTimeout();
	
	if(contact_list.length == 0 )
		throw new Error("Contact List is not visible");
		
	LogMessage("ContactsLib::IsContactListVisible::Contact List is visible and number of Contact Groups are : " + contact_list.length.toString());	
}

function VerifyGroupDetails(groupname,buddylist)
{
	LogMessage("ContactsLib :: VerifyGroupDetails");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	ExpandGroup(groupname);
	
	var num_cells = mainWindow.tableViews()[0].cells().length;
	var friendlist = "";
	for (i=0; i<num_cells; i++){
		var contactname = mainWindow.tableViews()[0].cells()[i].name();
		var index = contactname.indexOf(",");
		if(i < num_cells - 1)
		{
			friendlist = friendlist + contactname.substring(0,index) + "," ;
		}	
		else
		{
			friendlist = friendlist + contactname.substring(0,index);
		}
  	}
  	if(friendlist == buddylist)
  		LogMessage("ContactsLib::VerifyGroupDetails::Contact List " + groupname + " Expanded");
  	else
  		throw new Error("Error Expanding Contact List");
  	
	CollapseGroup(groupname);
  	
  	num_cells = mainWindow.tableViews()[0].cells().length;
  	if(num_cells != 0)
  		throw new Error("Error Collapsing Contact List");
  		
  	LogMessage("ContactsLib::VerifyGroupDetails::Contact List " + groupname + " Collapsed");
}

function VerifyBuddyPresence(groupname,buddy_name,availability)
{
	LogMessage("ContactsLib :: VerifyBuddyPresence");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	OpenContactCardFromContactList(groupname,buddy_name);
	
	var presenceTextControl = mainWindow.tableViews()[mainWindow.tableViews().length - 1].staticTexts()[1];
	IsValidAndVisible(presenceTextControl,"Presence String");
	
	LogMessage("ContactsLib :: VerifyBuddyPresence :: Presence String :" + presenceTextControl.value());
	for(i=1;i<=30;i++)
	{
	    if(presenceTextControl.value().indexOf(availability) >= 0)
	       break;
	    DelayInSec(2);
	    presenceTextControl = mainWindow.tableViews()[mainWindow.tableViews().length - 1].staticTexts()[1];
	}
	 
    if(presenceTextControl.value().indexOf(availability) == -1)
		throw new Error("Presence String not updated to " + availability);
	
	LogMessage("ContactsLib :: VerifyBuddyPresence :: Buddy Presence Verified");
	
}

function VerifyBuddyPresenceString(groupname,buddy_name,availability)
{
	LogMessage("ContactsLib :: VerifyBuddyPresence");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	OpenContactCardFromContactList(groupname,buddy_name);
	
	var presenceTextControl = mainWindow.tableViews()[mainWindow.tableViews().length - 1].staticTexts()[1];
	IsValidAndVisible(presenceTextControl,"Presence String");
	
	LogMessage("ContactsLib :: VerifyBuddyPresence :: Presence String :" + presenceTextControl.value());
	for(i=1;i<=30;i++)
	{
	    if(presenceTextControl.value().indexOf(availability) >= 0)
            break;
	    DelayInSec(2);
	    presenceTextControl = mainWindow.tableViews()[mainWindow.tableViews().length - 1].staticTexts()[1];
	}
    
    if(presenceTextControl.value().indexOf(availability) == -1)
		throw new Error("Presence String not updated to " + availability);
	
	LogMessage("ContactsLib :: VerifyBuddyPresence :: Buddy Presence Verified");
	
}

function VerifyContactCard(groupName,contactName)
{
    LogMessage("ContactsLib :: VerifyContactCard");
    
    var mainWindow = UIATarget.localTarget().frontMostApp().mainWindow();
    OpenContactCardFromContactList(groupName,contactName);
    
    var requiredTableView = mainWindow.tableViews()[mainWindow.tableViews().length - 1];
    var nameTextControl = requiredTableView.staticTexts()[0];
    IsValidAndVisible(nameTextControl,"Contact Name");
	if(!(nameTextControl.value().indexOf(contactName)>=0))
		throw new Error("Name of contact in contact card not found");
    
	VerifyContactCardButtons();
      
    var numOfCells = requiredTableView.cells().length;
    for(i=0;i<numOfCells;i++)
    {
       if(requiredTableView.cells()[i].name().indexOf(GetValueFromKey("LOCID E-Mail"))>=0)
          break;
    }
    if(i==numOfCells)
      throw new Error("Email Address Bar not found in Contact Card");
      
    LogMessage("ContactsLib :: VerifyContactCard :: Contact Card Details Verified");
}

function VerifyContactCardButtons()
{
    LogMessage("ContactsLib :: VerifyContactCardButtons");
    
    var mainWindow = UIATarget.localTarget().frontMostApp().mainWindow();
    var requiredTableView = mainWindow.tableViews()[mainWindow.tableViews().length - 1];
    var buttons = requiredTableView.buttons();

    IsValidAndVisible(buttons[GetValueFromKey("LOCID IM")],"IM Button");
    IsValidAndVisible(buttons[GetValueFromKey("LOCID Call")],"Call Button");
    IsValidAndVisible(buttons[GetValueFromKey("LOCID Video")],"Video Button");
    IsValidAndVisible(buttons[GetValueFromKey("LOCID E-Mail")],"EMail Button");
}


function ExpandGroup(groupname)
{
	LogMessage("ContactsLib :: ExpandGroup");
	var target = UIATarget.localTarget();
	var contact_list = target.frontMostApp().mainWindow().tableViews()[0].groups();
	
	var fullgroupname = EXPANDED.replace("%@", groupname);
	//var fullgroupname = "Group "+ groupname +", Expanded";
	
	if(contact_list[fullgroupname].isValid())
	{
		LogMessage("ContactsLib::ExpandGroup::" + groupname + " already expanded, no need to expand...");
		return;
	}
	
	fullgroupname = COLLAPSED.replace("%@", groupname);
	
	//fullgroupname = "Group "+ groupname +", Collapsed";
	LogMessage("ContactsLib :: ExpandGroup::Expanding "+ groupname);
	IsValidAndVisible(contact_list[fullgroupname], "Group "+groupname);
	contact_list[fullgroupname].tapWithOptions({tapOffset:{x:0.07, y:0.51}});
	
	waitForContactsToAppear(0);
}

function CollapseGroup(groupname)
{
	LogMessage("ContactsLib :: CollapseGroup");
	var target = UIATarget.localTarget();
	var contact_list = target.frontMostApp().mainWindow().tableViews()[0].groups();
	
	var fullgroupname = COLLAPSED.replace("%@", groupname);
	//var fullgroupname = "Group "+ groupname +", Collapsed";
	
	if(contact_list[fullgroupname].isValid())
	{
		LogMessage("ContactsLib :: CollapseGroup:: " +groupname + " already Collapsed, no need to collapse...");
		return;
	}
	
	fullgroupname = EXPANDED.replace("%@", groupname);
	//fullgroupname = "Group "+ groupname +", Expanded";
	IsValidAndVisible(contact_list[fullgroupname], "Group "+groupname);
  	contact_list[fullgroupname].tapWithOptions({tapOffset:{x:0.07, y:0.51}});
}

function OpenContactCardFromContactList(groupname,buddyName)
{
	LogMessage("ContactsLib :: OpenContactCardFromContactList");
	var target = UIATarget.localTarget();
	
	ExpandGroup(groupname);
	
	var mainWindow = target.frontMostApp().mainWindow();
	
	var queryString = "name CONTAINS '" + buddyName + "'";
  	var contactToLookFor = mainWindow.tableViews()[0].cells().withPredicate(queryString);
	target.popTimeout();
	if(contactToLookFor.length == 1)
	{
	 	IsValidAndVisible(contactToLookFor[0],buddyName);
	 	contactToLookFor[0].tap();
	}
	
	CollapseGroup(groupname);
}

function ExpandGroupFromContactList(groupName)
{	
	var string = GetValueFromKey("LOCID Collapsed");
    var replaceString = string.replace("%@",groupName);
    MainWindow.tableViews()[0].groups()[replaceString].tapWithOptions({tapOffset:{x:0.05, y:0.56}});
    DelayInSec(2);
}

function OpenGroupContactCardFromList(groupName)
{
    var string = GetValueFromKey("LOCID Collapsed");
    var replaceString = string.replace("%@",groupName);
    MainWindow.tableViews()[0].groups()[replaceString].tapWithOptions({tapOffset:{x:0.93, y:0.53}});
    DelayInSec(2);
}
function CollapseGroupFromContactList(groupName)
{

	var string = GetValueFromKey("LOCID Expanded");
    var replaceString = string.replace("%@",groupName);
    MainWindow.tableViews()[0].groups()[replaceString].tapWithOptions({tapOffset:{x:0.07, y:0.56}});
    DelayInSec(2);
}

function ExpandGroupFromContactList(groupName,offsetX,offsetY)
{
    var string = GetValueFromKey("LOCID Collapsed");
    var replaceString = string.replace("%@",groupName);
    MainWindow.tableViews()[0].groups()[replaceString].tapWithOptions({tapOffset:{x:offsetX, y:offsetY}});
    DelayInSec(2);
}
function CollapseGroupFromContactList(groupName,offsetX,offsetY)
{
    
	var string = GetValueFromKey("LOCID Expanded");
    var replaceString = string.replace("%@",groupName);
    MainWindow.tableViews()[0].groups()[replaceString].tapWithOptions({tapOffset:{x:offsetX, y:offsetY}});
    DelayInSec(2);
}



function WaitForBuddyBecomeVisible(string)
{

    for(var i = 0;i < 30;i++)
	{
		Target.pushTimeout(2);
		var element = MainWindow.tableViews()[0].cells()[string];
		Target.popTimeout();
        
        if(element.isValid() == true)
		{
            return element;
        }
        
		DelayInSec(1);
	}
	
}


function WaitForBuddyNoteChange(expectedNote)
{
	LogMessage("ContactsAndGroupLib :: WaitForBuddyNoteChange");
    
    var target = UIATarget.localTarget();
    var mainWindow = UIATarget.localTarget().frontMostApp().mainWindow();
    var contactCardTableView = mainWindow.tableViews()[mainWindow.tableViews().length - 1];
    
    FlickCallBarIfPresent();
    
    var queryString = "name CONTAINS '" + expectedNote + "'";
    
    LogMessage("ContactsAndGroupLib :: Waiting for buddy note to change to " + expectedNote);
    for(var i = 0;i < 120;i++)
	{
		target.pushTimeout(0);
		var noteButton = contactCardTableView.buttons().withPredicate(queryString);
		target.popTimeout();
		try
		{
			if(noteButton.length == 1)
			{
	 			IsValidAndVisible(noteButton[0],expectedNote);
	 			LogMessage("Buddy Note Verified");
				return;
			}
		}
		catch(error)
		{
		}	
		DelayInSec(1);
	}
	throw new Error("Buddy note not set as " + expectedNote);
}

function OpenGroupContactCard(groupname)
{
	LogMessage("ContactsAndGroupLib :: OpenGroupContactCard");
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	var contact_list = mainWindow.tableViews()[0].groups();
	
	//Group is Collapsed
	var fullgroupname = COLLAPSED.replace("%@", groupname);
	//fullgroupname = "Group "+ groupname +", Collapsed";
	LogMessage("ContactsAndGroupLib :: OpenGroupContactCard::Opening "+ groupname+" Contact Card");
	if(contact_list[fullgroupname].isValid())
	{
		contact_list[fullgroupname].tapWithOptions({tapOffset:{x:0.95,y:0.5}});
		waitForContactsToAppear(mainWindow.tableViews().length - 1);
		return;
	}
	
	//Else Group is Expanded
	fullgroupname = EXPANDED.replace("%@", groupname);
	//fullgroupname = "Group "+ groupname +", Expanded";
	if(contact_list[fullgroupname].isValid())
	{
		contact_list[fullgroupname].tapWithOptions({tapOffset:{x:0.95,y:0.5}});
		waitForContactsToAppear(mainWindow.tableViews().length - 1);
		return;
	}
		 
	throw new Error("Group " + groupname + " Not found");
	
}

function TapGroupIMbutton()
{
	LogMessage("ContactsAndGroupLib :: TapGroupIMbutton");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	LogMessage("ContactsAndGroupLib :: TapGroupIMbutton :: looking for IM button");
	target.pushTimeout(10);
	var IMButton = mainWindow.tableViews()[mainWindow.tableViews().length - 1 ].buttons()["icon action im"];
	target.popTimeout();
	
	IsValidAndVisible(IMButton, "Group IM button");
	IMButton.tap();
}

function TapGroupCallbutton()
{
	LogMessage("ContactsAndGroupLib :: TapGroupCallbutton");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	LogMessage("ContactsAndGroupLib :: TapGroupCallbutton :: looking for Call button");
	target.pushTimeout(10);
	var callButton = mainWindow.tableViews()[mainWindow.tableViews().length - 1 ].buttons()["icon action callwork"];
	target.popTimeout();
	
	IsValidAndVisible(callButton, "Group Call button");
	callButton.tap();
}

function waitForContactsToAppear(tableIndex)
{
	LogMessage("ContactsAndGroupLib :: waitForContactsToAppear");

	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	DelayInSec(2);
	
	numberOfBuddiesStr = GetBuddyCount("NumberOfTestCases");
	numberOfBuddiesInt = parseInt(numberOfBuddiesStr);
	
	for( var count = 1 ; count <= numberOfBuddiesInt ; count++)
	{
	 	var buddyName = GetBuddyDisplayName(count - 1);
	 	LogMessage("ContactsAndGroupLib :: Looking for " + buddyName);
	 	var queryString = "name CONTAINS '" + buddyName + "'";
	 	target.pushTimeout(60);
	 	var contactToLookFor = mainWindow.tableViews()[tableIndex].cells().withPredicate(queryString);
	 	target.popTimeout();
	 	if(contactToLookFor.length == 1)
	 	{
			IsValidAndVisible(contactToLookFor[0],buddyName);
	 	}
	 	else
	 	{
			throw new Error(buddyName + " is not yet seen in contact list");
	 	}
	}
	LogMessage("ContactsAndGroupLib :: All contacts are now visible");
}


function VerifyContactCardDetails(groupName,contactName,contactNote,contactTitle,contactWorkNumber,contactEmail,contactCompany,contactOffice)
{
    LogMessage("ContactsLib :: VerifyContactCardDetails");
    
    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow();
    OpenContactCardFromContactList(groupName,contactName);
    
    var contactCardView = mainWindow.tableViews()[mainWindow.tableViews().length - 1];
    
    //TODO : Personal Note
    //var contactNoteField = contactCardView.buttons()["Note, " + contactNote];
    //IsValidAndVisible(contactNoteField,"Note of contact");
    
    var contactTitleField = contactCardView.staticTexts()[contactTitle];
    IsValidAndVisible(contactTitleField,"Title of contact");
      
    var contactWorkNumberField = contactCardView.cells()["work, "+contactWorkNumber];
    IsValidAndVisible(contactWorkNumberField,"Work Number of contact");
    
    var contactEmailField = contactCardView.cells()["email, "+contactEmail];
    IsValidAndVisible(contactEmailField,"Email of contact");
    
    var contactCompanyField = contactCardView.cells()["company, "+contactCompany];
    IsValidAndVisible(contactCompanyField,"Company of contact");
    
    var contactOfficeField = contactCardView.cells()["office, "+contactOffice];
    IsValidAndVisible(contactOfficeField,"Office of contact");
    
    LogMessage("ContactsLib :: VerifyContactCardDetails :: Contact Card Details Verified");
}


function VerifyiLyncAndiPadContacts()
{
	LogMessage("ContactsLib :: VerifyiLyncAndiPadContacts");
    
    var target = UIATarget.localTarget();
    
    LogMessage("ContactsLib :: VerifyiLyncAndiPadContacts :: Looking for iLync Contacts");
    var iLyncContactList = target.frontMostApp().mainWindow().tableViews()[0].groups();
    
    if(iLyncContactList.length == 0)
    	throw new Error("Lync Contacts not found");
    	
    LogMessage("ContactsLib :: VerifyiLyncAndiPadContacts :: Switching to iPad Contacts"); 	
    var iPadContactsButton = target.frontMostApp().mainWindow().tableViews()[0].buttons()["iPad"];
    IsValidAndVisible(iPadContactsButton,"iPad Contacts button");
    iPadContactsButton.tap();
    target.delay(1);
     
    var iPadContactList = target.frontMostApp().mainWindow().tableViews()[0].groups();
    if(iPadContactList.length == 0)
    	throw new Error("iPad Contacts not found");
	
	LogMessage("ContactsLib :: VerifyiLyncAndiPadContacts :: Switching back to Lync Contacts"); 	
    var iLyncContactsButton = target.frontMostApp().mainWindow().tableViews()[0].buttons()["Lync"];
    IsValidAndVisible(iLyncContactsButton,"Lync Contacts button");
    iLyncContactsButton.tap();
    target.delay(1);
}


function VerifySearchByAliasResults(buddyAlias,buddyDisplayName)
{
	LogMessage("ContactsAndGroupLib :: VerifySearchByAliasResults :: " + "Will Search for : " + buddyAlias);
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	var isContactFound = true;
	
	var searchBox = mainWindow.searchBars()[0];
	IsValidAndVisible(searchBox, "Search Box");
	searchBox.tap();
	searchBox.setValue(buddyAlias);
	HideKeyboard();
	DelayInSec(1);
	
	var searchView = mainWindow.tableViews()[0];
	target.pushTimeout(20);
	var contactCell = searchView.cells()[1];
	target.popTimeout();
	
	try
	{
		IsValidAndVisible(contactCell, "Buddy : " + buddyAlias);
	}
	catch(error)
	{
		isContactFound = false;
	}
	
	if(isContactFound)
	{
		if(contactCell.name().indexOf(buddyDisplayName) >= 0)
		{
			LogMessage("ContactsAndGroupLib :: VerifySearchByAliasResults :: Buddy " + buddyDisplayName + " found");
		}
		else
		{
			isContactFound = false;
			LogMessage("ContactsAndGroupLib :: VerifySearchByAliasResults :: Buddy " + buddyDisplayName + " not found");
			LogMessage("ContactsAndGroupLib :: VerifySearchByAliasResults ::  " + contactCell.name() + " found");
		}
	}
	var clearTextButton = searchBox.buttons()[0];
	IsValidAndVisible(clearTextButton);
	clearTextButton.tap();
	DelayInSec(1)
	
	var cancelButton = mainWindow.buttons()[GetValueFromKey("LOCID Cancel")];
	IsValidAndVisible(cancelButton);
	cancelButton.tap();
	DelayInSec(1)
	
	if(isContactFound == false)
	{
		throw new Error("ContactsAndGroupLib :: VerifySearchByAliasResults :: Failed to search by Alias " + buddyAlias);
	}

}

function VerifyDGSearchResults(galDGName)
{
	LogMessage("ContactsAndGroupLib :: VerifyDGSearchResults :: " + "Will Search for : " + galDGName);
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	var isContactFound = true;
	
	var searchBox = mainWindow.searchBars()[0];
	IsValidAndVisible(searchBox, "Search Box");
	searchBox.tap();
	searchBox.setValue(galDGName);
	HideKeyboard();
	DelayInSec(1);
	
	var searchView = mainWindow.tableViews()[0];
	target.pushTimeout(20);
	var contactCell = searchView.cells()[1];
	target.popTimeout();
	
	try
	{
		IsValidAndVisible(contactCell, "DG : " + galDGName);
	}
	catch(error)
	{
		isContactFound = false;
	}
	
	var clearTextButton = searchBox.buttons()[0];
	IsValidAndVisible(clearTextButton);
	clearTextButton.tap();
	DelayInSec(1)
	
	var cancelButton = mainWindow.buttons()[GetValueFromKey("LOCID Cancel")];
	IsValidAndVisible(cancelButton);
	cancelButton.tap();
	DelayInSec(1)
	
	if(isContactFound == false)
	{
		throw new Error("ContactsAndGroupLib :: VerifyDGSearchResults :: Failed to search DG " + galDGName);
	}

}


function VerifyEmailWindow()
{
	LogMessage("ContactsAndGroupLib::VerifyEmailWindow");
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	
	var emailNavBar = mainWindow.navigationBars()["New Message"];
	IsValidAndVisible(emailNavBar, "Email Navigation Bar");
	
	var emailCancelButton = emailNavBar.buttons()[GetValueFromKey("LOCID Cancel")];
	IsValidAndVisible(emailCancelButton, "Email Cancel Button");
	
	var emailSendButton = emailNavBar.buttons()[GetValueFromKey("LOCID Send Location")];
	IsValidAndVisible(emailSendButton, "Email Send Button");
	
	emailCancelButton.tap();
	DelayInSec(1);
	var popOverWindow = mainWindow.popover();
	IsValidAndVisible(popOverWindow, "Email CancelPopOver Window");
	var deleteDraftButton = popOverWindow.actionSheet().buttons()["Delete Draft"];
	IsValidAndVisible(deleteDraftButton, "Email Delete Draft Button");
	deleteDraftButton.tap();
	DelayInSec(2);
}


function VerifySearchResultContactCard(contactToSearch) {
    LogMessage("ContactsAndGroupLib :: VerifySearchResultContactCard :: " + "Will Search for : " + contactToSearch);

    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow();

    OpenContactCardfromSearch(contactToSearch)

    LogMessage("ContactsAndGroupLib :: VerifySearchResultContactCard :: " + "verify the contact card");
    var requiredTableView = mainWindow.tableViews()[mainWindow.tableViews().length - 1];
    var nameTextControl = requiredTableView.staticTexts()[0];
    IsValidAndVisible(nameTextControl, "Contact Name");
    if (!(nameTextControl.value().indexOf(contactToSearch) >= 0))
        throw new Error("Name of Searched Contact not found in ContactCard");

    VerifyContactCardButtons();
    
    LogMessage("ContactsAndGroupLib :: VerifySearchResultContactCard :: Contact card elements verified");
}


function VerifyContactPictureIsPresent()
{
	LogMessage("ContactsAndGroupLib :: VerifyContactPictureIsPresent");
    
    var mainWindow = UIATarget.localTarget().frontMostApp().mainWindow();
    
    var contactCardView = mainWindow.tableViews()[mainWindow.tableViews().length - 1];
    
    if(contactCardView.images()["DB_48.png"].isValid())
      throw new Error("Contact Pic is not present");
}


function OpeniPadContactCard()
{
	LogMessage("ContactsAndGroupLib :: OpeniPadContactCard");
    
    var target = UIATarget.localTarget();
    	
    LogMessage("ContactsLib :: OpeniPadContactCard :: Switching to iPad Contacts"); 	
    var iPadContactsButton = target.frontMostApp().mainWindow().tableViews()[0].buttons()["iPad"];
    IsValidAndVisible(iPadContactsButton,"iPad Contacts button");
    iPadContactsButton.tap();
    target.delay(1);
     
    var iPadContactList = target.frontMostApp().mainWindow().tableViews()[0];
    if(iPadContactList.groups().length == 0)
    	throw new Error("iPad Contacts not found");
	
	iPadContactList.cells()[0].tap();
    target.delay(1);
    
    LogMessage("ContactsLib :: OpeniPadContactCard :: Switching back to Lync Contacts"); 	
    var iLyncContactsButton = target.frontMostApp().mainWindow().tableViews()[0].buttons()["Lync"];
    IsValidAndVisible(iLyncContactsButton,"Lync Contacts button");
    iLyncContactsButton.tap();
    target.delay(1);
}


function VerifyCommonContactSearchResults(commonContactName)
{
	LogMessage("ContactsAndGroupLib :: VerifyCommonContactSearchResults :: " + "Will Search for : " + commonContactName);
	
	var target = UIATarget.localTarget();
	var mainWindow = target.frontMostApp().mainWindow();
	var isContactFound = true;
	
	var searchBox = mainWindow.searchBars()[0];
	IsValidAndVisible(searchBox, "Search Box");
	searchBox.tap();
	searchBox.setValue(commonContactName);
	HideKeyboard();
	DelayInSec(1);
	
	var searchView = mainWindow.tableViews()[0];
	
	target.pushTimeout(20);
	var contactCell1 = searchView.cells()[1];
	var contactCell2 = searchView.cells()[2];
	target.popTimeout();
	
	if(contactCell1.isValid() && contactCell1.name().indexOf(commonContactName) >= 0 && contactCell2.isValid() && contactCell2.name().indexOf(commonContactName) >= 0)
	{
		LogMessage("ContactsAndGroupLib :: VerifyCommonContactSearchResults :: Contact " + commonContactName + " found");		
	}
	else
	{
		isContactFound = false;
		LogMessage("ContactsAndGroupLib :: VerifyCommonContactSearchResults :: Contact " + commonContactName + " not found");
	}
			
	var clearTextButton = searchBox.buttons()[0];
	IsValidAndVisible(clearTextButton);
	clearTextButton.tap();
	DelayInSec(1)
	
	var cancelButton = mainWindow.buttons()[GetValueFromKey("LOCID Cancel")];
	IsValidAndVisible(cancelButton);
	cancelButton.tap();
	DelayInSec(1)
	
	if(isContactFound == false)
	{
		throw new Error("ContactsAndGroupLib :: VerifyCommonContactSearchResults :: Failed to search the common Contact " + 	commonContactName);
	}
}

function VerifyContactIsAdded(groupName, contactToAddRemove) {
    LogMessage("ContactsLib :: VerifyContactIsAdded");
    var target = UIATarget.localTarget();
    var mainWindow = target.frontMostApp().mainWindow();
    var contactList = mainWindow.tableViews()[0].groups();
    
    var fullgroupname = COLLAPSED.replace("%@", groupName);
    DelayInSec(10);
    //var fullgroupname = "Group " + groupName + ", Collapsed";

    if (contactList[fullgroupname].isValid()) {
        LogMessage("ContactsLib :: VerifyContactIsAdded::Expanding " + groupName);
        IsValidAndVisible(contactList[fullgroupname], "Group " + groupName);
        contactList[fullgroupname].tap();
    }
    else
        LogMessage("ContactsLib :: VerifyContactIsAdded :: " + groupName + " already expanded, no need to expand...");

    target.pushTimeout(10); //wait for update
    var queryString = "name CONTAINS '" + contactToAddRemove + "'";
    var contactToLookFor = mainWindow.tableViews()[0].cells().withPredicate(queryString);
    target.popTimeout();

    CollapseGroup(groupName);

    if (contactToLookFor.length == 1) {
        LogMessage("ContactsLib :: VerifyContactIsAdded :: Contact " + contactToAddRemove + " Added to the Group");
    }
    else {
        LogMessage("ContactsLib :: VerifyContactIsAdded :: Contact " + contactToAddRemove + " Not added to the Group");
        throw new Error("Contact " + contactToAddRemove + " Not added to the Group");
    }

}

function VerifyContactIsRemoved(groupName, contactToAddRemove) {
    LogMessage("ContactsLib :: VerifyContactIsRemoved");
    var target = UIATarget.localTarget();
    var contactList = target.frontMostApp().mainWindow().tableViews()[0].groups();
    
    var fullgroupname = COLLAPSED.replace("%@", groupName);
    //var fullgroupname = "Group " + groupName + ", Collapsed";
    DelayInSec(10);

    if (contactList[fullgroupname].isValid()) {
        LogMessage("ContactsLib :: VerifyContactIsRemoved::Expanding " + groupName);
        IsValidAndVisible(contactList[fullgroupname], "Group " + groupName);
        contactList[fullgroupname].tap();
    }
    else
        LogMessage("ContactsLib :: VerifyContactIsRemoved :: " + groupname + " already expanded, no need to expand...");
	
    var mainWindow = target.frontMostApp().mainWindow();
    var contactFound = false;
    for (count = 1; count <= 30; count++) 
    {
        var numCells = mainWindow.tableViews()[0].cells().length;
        for (i = 0; i < numCells; i++) 
        {
            var contactName = mainWindow.tableViews()[0].cells()[i].name();
            if (contactName.indexOf(contactToAddRemove) == 0)
                contactFound = true;

        }
        if (contactFound == false)
            break;
        DelayInSec(1);
    }

    CollapseGroup(groupName);

    if (contactFound == false)
        LogMessage("ContactsLib :: VerifyContactIsRemoved :: Contact removed from the group");
    else {
        LogMessage("ContactsLib :: VerifyContactIsRemoved :: Contact not removed from the group");
        throw new Error("Contact not removed from the group");
    }
}
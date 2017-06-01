function VerifyContactListTest()
{
	LogMessage("ContactsTest :: VerifyContactListTest");
	
	GotoContacts();
	IsContactListVisible();
	var groupname = GetTestCaseParameters("GroupName");
	var buddylist = GetTestCaseParameters("BuddyListDisplayName");
	VerifyGroupDetails(groupname,buddylist);
}

function VerifyBuddyPresenceChangeTest()
{	
	LogMessage("ContactsTest :: VerifyBuddyPresenceChangeTest");
	
	var groupname = GetTestCaseParameters("GroupName");
	var buddy_name = GetBuddyDisplayName(0);
	GotoContacts();
	VerifyBuddyPresence(groupname,buddy_name,AVAILABLE);
	GotoMyInfo();
	SetNote("VerifyBuddyPresenceChange")
	GotoContacts();
	VerifyBuddyPresence(groupname,buddy_name,BUSY);
}

function VerifyContactCardTest()
{
	LogMessage("ContactsTest :: VerifyContactCardTest");
	
    var groupName = GetTestCaseParameters("GroupName");
	var contactName = GetBuddyDisplayName(0);
    
    GotoContacts();
    VerifyContactCard(groupName,contactName);

}

function VerifySearchTest()
{
	LogMessage("ContactsTest :: VerifySearchTest");
	
	var contactListBuddy = GetBuddyDisplayName(0);
	var galbuddy = GetTestCaseParameters("GalBuddyDisplayName");
	
	GotoContacts();
	LogMessage("Searching for ContactList Buddy");
	VerifySearchResults(contactListBuddy);
	LogMessage("Searching for GAL Buddy");
	VerifySearchResults(galbuddy);
}

//DFTs

function VerifyContactCardDetailsTest()
{
	LogMessage("ContactsTest :: VerifyContactCardDetailsTest");
	
    var groupName = GetTestCaseParameters("GroupName");
	var contactName = GetBuddyDisplayName(0);
	var contactNote = GetTestCaseParameters("BuddyNote");
    var contactTitle = GetTestCaseParameters("BuddyTitle");
    var contactWorkNumber = GetTestCaseParameters("BuddyWorkNumber");
    var contactEmail= GetTestCaseParameters("BuddyEmail");
    var contactCompany = GetTestCaseParameters("BuddyCompany");
    var contactOffice= GetTestCaseParameters("BuddyOffice");
    
    GotoContacts();
	VerifyContactCardDetails(groupName,contactName,contactNote,contactTitle,contactWorkNumber,contactEmail,contactCompany,contactOffice);
}

function VerifyiLyncAndiPadContactsTest()
{
	LogMessage("ContactsTest :: VerifyiLyncAndiPadContactsTest");
	
	GotoContacts();
	LogMessage("ContactsTest :: VerifyiLyncAndiPadContactsTest :: Looking for both iLync and iPad Contacts");
	VerifyiLyncAndiPadContacts();
}

function VerifySearchByFirstNameTest()
{
	LogMessage("ContactsTest :: VerifySearchByFirstNameTest");
	var contactListBuddyFirstName = GetTestCaseParameters("SearchByFirstNameString");
	
	GotoContacts();
	LogMessage("ContactsTest :: VerifySearchByFirstNameTest :: Searching for Contact By First Name");
	VerifySearchResults(contactListBuddyFirstName);
}

function VerifySearchByLastNameTest()
{
	LogMessage("ContactsTest :: VerifySearchByLastNameTest");
	
	var contactListBuddyLastName = GetTestCaseParameters("SearchByLastNameString");
	
	GotoContacts();
	LogMessage("ContactsTest :: VerifySearchByLastNameTest :: Searching for Contact By Last Name");
	VerifySearchResults(contactListBuddyLastName);
}

function VerifySearchByAliasTest()
{
	LogMessage("ContactsTest :: VerifySearchByAliasTest");
	
	var buddyAlias = GetTestCaseParameters("GalBuddyAlias");
	var buddyDisplayName = GetTestCaseParameters("GalBuddyDisplayName");
	
	GotoContacts();
	LogMessage("ContactsTest :: VerifySearchByAliasTest :: Searching for Contact By Alias");
	VerifySearchByAliasResults(buddyAlias,buddyDisplayName);
}


function VerifySearchDGTest()
{
	LogMessage("ContactsTest :: VerifySearchDGTest");
	
	var galDGName = GetTestCaseParameters("GalGroupName");
	
	GotoContacts();
	LogMessage("ContactsTest :: VerifySearchDGTest :: Searching for  Distribution Group");
	VerifyDGSearchResults(galDGName);
}

function VerifySearchCommonContactTest()
{
	LogMessage("ContactsTest :: VerifySearchCommonContactTest");
	
	var commonContactName = GetBuddyDisplayName(0);
	
	GotoContacts();
	LogMessage("ContactsTest :: VerifySearchCommonContactTest :: Searching for a Common Contact");
	VerifyCommonContactSearchResults(commonContactName);
}



function VerifyEmailFromContactCardTest()
{
	LogMessage("ContactsTest :: VerifyEmailFromContactCard");
	
	var groupName = GetTestCaseParameters("GroupName");
	var buddyName = GetBuddyDisplayName(0);
	
	GotoContacts();
	OpenContactCardFromContactList(groupName,buddyName);
	TapEmailButton();
	VerifyEmailWindow();
}

function VerifySearchResultContactCardTest()
{
	LogMessage("ContactsTest :: VerifySearchResultContactCardTest");
	
	var galSearch = GetTestCaseParameters("GalBuddyDisplayName");
	
	GotoContacts();
	VerifySearchResultContactCard(galSearch);
}

function VerifyContactPictureTest()
{
	LogMessage("ContactsTest :: VerifyContactPictureTest");
	
	var groupName = GetTestCaseParameters("GroupName");
	var buddyName = GetBuddyDisplayName(0);
	
	GotoContacts();
	LogMessage("ContactsTest :: VerifyContactPictureTest :: Verify For iLync Contact");
	OpenContactCardFromContactList(groupName,buddyName);
	VerifyContactPictureIsPresent();
	
	LogMessage("ContactsTest :: VerifyContactPictureTest :: Verify For iPad Contact");
	OpeniPadContactCard();
	VerifyContactPictureIsPresent();
}

function VerifyAddContactFromOcomTest() {
    LogMessage("ContactsTest :: VerifyAddContactFromOComTest");

    var groupName = GetTestCaseParameters("OtherContactsGroupName");
    var contactToAddRemoveName = GetTestCaseParameters("ContactToAddRemoveName");

    GotoContacts();
    VerifyContactIsRemoved(groupName, contactToAddRemoveName);
    GotoMyInfo();
    SetNote("VerifyAddContactToGroup");
    GotoContacts();
    VerifyContactIsAdded(groupName, contactToAddRemoveName);
    
}

function VerifyRemoveContactFromOcomTest() {
    LogMessage("ContactsTest :: VerifyRemoveContactFromOcomTest");

    var groupName = GetTestCaseParameters("OtherContactsGroupName");
    var contactToAddRemoveName = GetTestCaseParameters("ContactToAddRemoveName");

    GotoContacts();
    VerifyContactIsAdded(groupName, contactToAddRemoveName);
    GotoMyInfo();
    SetNote("VerifyRemoveContactFromGroup");
    GotoContacts();
    VerifyContactIsRemoved(groupName, contactToAddRemoveName);
}

function VerifySearchContactInAudioCallTest() {
    LogMessage("ContactsTest :: VerifySearchContactInAudioCallTest");

    var buddyContact = GetBuddyDisplayName(0);
    var groupname = GetTestCaseParameters("GroupName");

    SetNote("VerifySearchContactInAudioCall");

    GotoContacts();
    OpenContactCardFromContactList(groupname, buddyContact);
    TapCallbutton();

    WaitforAudioWindow();
    WaitForCallToGetConnected();

    GotoModalityView(IM_MODALITY_BUTTON);
    WaitforConversationWindow();

    var messageToRecieve = buddyContact + " - " + audioConnectedInP2PCallMessage;
    messageToRecieve = messageToRecieve.split(" ").join("  ");
    VerifyReceivedIM(buddyContact, messageToRecieve);

    var galbuddy = GetTestCaseParameters("GalBuddyDisplayName");

    GotoContacts();
    LogMessage("ContactsTest :: VerifySearchContactInAudioCallTest :: Now Searching for GAL Buddy");
    VerifySearchResults(galbuddy);

    LogMessage("ContactsTest :: VerifySearchContactInAudioCallTest :: End the call");
    GotoChats();
    TapOnConversationInList(0);
    EndConversation();
}

function VerifyBuddyActivityString()
{
    LogMessage("ContactsTest :: VerifyBuddyActivityString");
	
    var groupName = GetTestCaseParameters("GroupName");
    var buddyName1 = GetBuddyDisplayName(0);
	
    GotoMyInfo();
    SetNote("VerifyBuddyActivityString");
    
    GotoContacts();
    VerifyBuddyPresence(groupName,buddyName1,GetValueFromKey("LOCID In a Conference Call"));
    
}
function VerifyContactsTest()
{
    if(IsSignInScreenUp() == 1){
        SignInAuto();
    }
    
    ValidateTabBarButtons();
    ContactsGroupAndBuddyTest();
}

function VerifyBuddyPresence()
{
    if(IsSignInScreenUp() == 1){
        SignInAuto();
    }
    
    ValidateTabBarButtons();
    ChangeBuddyPresenceString();
}


function ContactsGroupAndBuddyTest()
{
    
    var contactsButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Contacts")];
    if(ElementValidAndVisible(contactsButton) == true){
        TapElement(contactsButton);
        captureLocalizedScreenshot("73290");
        captureLocalizedScreenshot("445921");
        
        var string = GetValueFromKey("LOCID Collapsed");
        var groupString = GetValueFromKey("LOCID Other Contacts");
        var replaceString = string.replace("%@",groupString);
        var groupLabel = MainWindow.tableViews()[0].groups()[replaceString];
        if(ElementValidAndVisible(groupLabel) == true) {
            ExpandGroupFromContactList(groupString);
            captureLocalizedScreenshot("382215_1");
            CollapseGroupFromContactList(groupString);
            OpenGroupContactCardFromList(groupString);
            captureLocalizedScreenshot("382215_2");
            TapNavigationBackButton();
            
        }
        else{
            LogMessage("382215_1 382215_2 Fail");
        }
        
        string = GetValueFromKey("LOCID Collapsed");
        groupString = "New Group";
        replaceString = string.replace("%@",groupString);
        groupLabel = MainWindow.tableViews()[0].groups()[replaceString];
        if(ElementValidAndVisible(groupLabel) == true) {
            OpenGroupContactCardFromList(groupString);
            captureLocalizedScreenshot("382217");
            TapNavigationBackButton();
        }
        else{
            LogMessage("382217 Fail");
        }
        
        var searchBar = MainWindow.searchBars()[0];
        var button = MainWindow.elements()[2];
        if(ElementValidAndVisible(searchBar) == true) {
            searchBar.tap();
            searchBar.setValue("DG_ting");
            if(TapWaitingElement(3,"DG_tinglee1") == true) {
                captureLocalizedScreenshot("388383");
                TapNavigationBackButton();
            }else{
                LogMessage("388383 Fail");
            }
            searchBar.setValue("");
            TypeString("%\n");
            DelayInSec(2);
            captureLocalizedScreenshot("452831");
            
            searchBar.setValue("");
            TypeString("@\n");
            DelayInSec(1);
            captureLocalizedScreenshot("383560");
            DelayInSec(3);
            captureLocalizedScreenshot("447572");
            
            searchBar.setValue("DG_ting");
            var cell = MainWindow.tableViews()[1].cells()[GetValueFromKey("LOCID Too many results")];
            if(ElementValidAndVisible(cell) == true){
                captureLocalizedScreenshot("383562");
            }else {
                LogMessage("383562 Fail")
            }
            

            button.tap();
            DelayInSec(1);
        }
        else{
            LogMessage("388383 383562 452831 383560 447572 383562 Fail");
        }
       
        groupString = "Next";
        replaceString = string.replace("%@",groupString);
        groupLabel = MainWindow.tableViews()[0].groups()[replaceString];
        if(ElementValidAndVisible(groupLabel) == true) {
            ExpandGroupFromContactList(groupString);
            if(TapTableviewCell(0,"+14251234567@clientee.rtmp.selfhost.corp.microsoft.com, (null)") == true) {
                captureLocalizedScreenshot("398057");
                TapNavigationBackButton();
            }else {
                
                LogMessage("398057 Fail");
            }
            
            var buddyString = "wangyong811117@163.com" + ", " + GetValueFromKey("LOCID Presence unknown");
            if(TapTableviewCell(0,buddyString) == true) {
                
                captureLocalizedScreenshot("398059");
                TapNavigationBackButton();
            }else {
                
                LogMessage("398059 382063_2 Fail");
            }
            CollapseGroupFromContactList(groupString);
        }
        else{
            LogMessage("398057 382063_1 398059 382063_2 Fail");
        }

        string = GetValueFromKey("LOCID Collapsed");
        groupString = "New Group";
        replaceString = string.replace("%@",groupString);
        groupLabel = MainWindow.tableViews()[0].groups()[replaceString];
        if(ElementValidAndVisible(groupLabel) == true) {
            ExpandGroupFromContactList(groupString);
            string = "v-ninli@microsoft.com" + ", " + GetValueFromKey("LOCID Presence unknown");
            var waitCell = WaitForBuddyBecomeVisible(string);
            if(waitCell.isVisible() == true){
                captureLocalizedScreenshot("382063_1");
                captureLocalizedScreenshot("408374_1");
                captureLocalizedScreenshot("382062_1");
                captureLocalizedScreenshot("426717_1");
                captureLocalizedScreenshot("382061_1");
                captureLocalizedScreenshot("410091_1");
                if(TapViewElement(2,7) == true) {
                    captureLocalizedScreenshot("382061_2");
                    captureLocalizedScreenshot("426717_2");
                    TapNavigationBackButton();
                }else {
                    
                    LogMessage("382061_2 408374_2 382062_2 410092_2 426717_2 Fail");
                }
                
                if(TapViewElement(2,9) == true) {
                    
                    captureLocalizedScreenshot("73300_1");
                    MainWindow.tableViews()[0].scrollDown();
                    DelayInSec(1);
                    captureLocalizedScreenshot("73300_2");
                    captureLocalizedScreenshot("410117");
                    string = GetValueFromKey("LOCID Link") + ", " + GetValueFromKey("LOCID No Name");
                    if(TapTableviewCell(0,string) == true) {
                        
                        captureLocalizedScreenshot("408607");
                        TapNavigationBackButton();
                    }else {
                        
                        LogMessage("408607 Fail");
                    }
                    
                    TapNavigationBackButton();
                    
                }else {
                    
                    LogMessage("73300_1 73300_2 410117 408607 Fail");
                }
                
                if(TapViewElement(2,10) == true) {
                    
                    captureLocalizedScreenshot("410091_2");
                    captureLocalizedScreenshot("382062_2");
                    captureLocalizedScreenshot("408374_2");
                    TapNavigationBackButton();
                }else {
                    
                    LogMessage("410091_2 382062_2 408374_2 Fail");
                }
                TapElement(waitCell);
                captureLocalizedScreenshot("382063_2");
                TapNavigationBackButton();
            }else {
                LogMessage("73300_1 73300_2 410117 408607 Fail");
                LogMessage("382061_2 408374_2 382062_2 410092_2 426717_2 410091_2 Fail");
                LogMessage("382061_1 408374_1 382062_1 410092_1 426717_1 Fail");
            }
            CollapseGroupFromContactList(groupString);
    
        }
        else{
            LogMessage("73300_1 382063_1 73300_2 410117 408607 Fail");
            LogMessage("382061_2 408374_2 382062_2 426717_2 410091_2 382063_2 Fail");
            LogMessage("382061_1 408374_1 382062_1 410091_1 426717_1 Fail");
        }

    }
}

function ChangeBuddyPresenceString()
{

    GotoMyInfo();
    SetHappeningNote("VerifyBuddyStatusAvailable");
    GotoContacts();
    ExpandGroupFromContactList("New Group");
    var nameString = "C_Yanxia Mu (iSoftStone)5";
    var string = nameString + ", " + GetValueFromKey("LOCID Available");
    var cell = WaitForBuddyBecomeVisible(string);
    if(cell.isVisible() == true){
        captureLocalizedScreenshot("382056_1");
        TapElement(cell);
        captureLocalizedScreenshot("382056_2");
        TapNavigationBackButton();
    }else {
        LogMessage("382056_1 382056_2 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyBuddyStatusBusy");
    GotoContacts();
    string = nameString + ", " + GetValueFromKey("LOCID Busy");
    cell = WaitForBuddyBecomeVisible(string);
    if(cell.isVisible() == true){
        captureLocalizedScreenshot("382057_1");
        TapElement(cell);
        captureLocalizedScreenshot("382057_2");
        TapNavigationBackButton();
    }else {
        LogMessage("382057_1 382057_2Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyBuddyStatusDoNotDisturb");
    GotoContacts();
    string = nameString + ", " + GetValueFromKey("LOCID Do Not Disturb");
    cell = WaitForBuddyBecomeVisible(string);
    if(cell.isVisible() == true){
        captureLocalizedScreenshot("397921_1");
        TapElement(cell);
        captureLocalizedScreenshot("397922_2");
        TapNavigationBackButton();
    }else {
        LogMessage("397921_1 397922_2 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyBuddyStatusBeRightBack");
    GotoContacts();
    string = nameString + ", " + GetValueFromKey("LOCID Be Right Back");
    cell = WaitForBuddyBecomeVisible(string);
    if(cell.isVisible() == true){
        captureLocalizedScreenshot("382059_1");
        TapElement(cell);
        captureLocalizedScreenshot("382059_2");
        TapNavigationBackButton();
    }else {
        LogMessage("382059_1 382059_2 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyBuddyActivityString");
    GotoContacts();
    string = nameString + ", " + GetValueFromKey("LOCID In a Conference Call");
    cell = WaitForBuddyBecomeVisible(string);
    if(cell.isVisible() == true){
        captureLocalizedScreenshot("382055_1");
        TapElement(cell);
        captureLocalizedScreenshot("382055_2");
        TapNavigationBackButton();
    }else {
        LogMessage("382055_1 382055_2 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyBuddyStatusAway");
    GotoContacts();
    var timeString = GetValueFromKey("LOCID idle minutes");
    var replaceString = timeString.replace("%d","5");
    string = nameString + ", " + GetValueFromKey("LOCID Away") + " - " + replaceString;
    cell = WaitForBuddyBecomeVisible(string);
    if(cell.isVisible() == true){
        captureLocalizedScreenshot("382060_1");
        captureLocalizedScreenshot("410092_1");
        TapElement(cell);
        captureLocalizedScreenshot("382060_2");
        captureLocalizedScreenshot("410092_2");
        TapNavigationBackButton();
    }else {
        LogMessage("382060_1 382060_2 Fail");
    }
    
    CollapseGroupFromContactList("New Group");
    
    GotoMyInfo();
    SetHappeningNote("VerifyIncomingImConference");
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Dismiss"));
    if(button.isVisible() == true){
        
        captureLocalizedScreenshot("406851");
        OpenConferenceImWithSubject(nameString);
        button = WaitforToastDissMissButton(GetValueFromKey("LOCID Answer Call"));
        if(button.isVisible() == true){
            TapElement(button);
            captureLocalizedScreenshot("383564");
            TapNavigationRightButton();
            DelayInSec(1);
            captureLocalizedScreenshot("403606");
            TapActionSheetCancelButton();
            TapNavigationBackButton();
            DeleteAllConversations();
        }else {
            LogMessage("383564 403606 Fail");
        }
    }else {
        LogMessage("406851 383564 403606 Fail");
    }
    
    GotoMyInfo();
    SetHappeningNote("VerifyIncomingConferenceReject");
    var button = WaitforToastDissMissButton(GetValueFromKey("LOCID Dismiss Call"));
    
    if(button.isVisible() == true){
        
        TapElement(button);
        GotoChats();
        DelayInSec(1);
        captureLocalizedScreenshot("451236");
        DeleteAllConversations();
    }else {
        LogMessage("451236 Fail");
    }
    
    GotoMyInfo();
    ResetNoteIfNeeded();
}

function VerifyContactsPageDisplayWell(){

    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            
            LogMessage("73290 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    var contactsButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Contacts")];
    if(ElementValidAndVisible(contactsButton) == true){
        
        TapElement(contactsButton);
        captureLocalizedScreenshot("73290");
        GoToOrientation(DEVICELEFT);
        captureLocalizedScreenshot("73290_h");
        GoToOrientation(DEVICEPROT);
    }else{
        
        LogMessage("73290 Fail");
    }
}

function VerifyStringNoContactsIniPhoneContactsList() {
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            
            LogMessage("408620 Fail");
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
    var mainWindow = target.frontMostApp().mainWindow();
    var contactsButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Contacts")];
    if(ElementValidAndVisible(contactsButton) == true){
        
        TapElement(contactsButton);
        
        var button = mainWindow.tableViews()[0].buttons()["iPhone"];
        if(button.isVisible() == true){
            
            button.tap();
            DelayInSec(1);
            
            captureLocalizedScreenshot("408620");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("408620_h");
            GoToOrientation(DEVICEPROT);
            
            button = mainWindow.tableViews()[0].buttons()["Lync"];
            if(button.isVisible() == true){
                
                button.tap();
                DelayInSec(1);
            }
        }else {
            
            LogMessage("408620 Fail");
        }
    }else {
        
        LogMessage("408620 Fail");
    }

}


function VerifyBuddyCardPageOnContactsViewDisplayWell(){
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("73300 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    var contactsButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Contacts")];
    if(ElementValidAndVisible(contactsButton) == true){
        
        TapElement(contactsButton);
        var string = GetValueFromKey("LOCID Collapsed");
        var groupString = GetValueFromKey("LOCID Pinned Contacts");
        var replaceString = string.replace("%@",groupString);
        var groupLabel = MainWindow.tableViews()[0].groups()[replaceString];
        if(ElementValidAndVisible(groupLabel) == true) {
            
            ExpandGroupFromContactList(groupString);
            
            if(TapTableviewCellWithPredicate(BUDDYDISPLAYNAME) == true){
                                
                captureLocalizedScreenshot("73300_1");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("73300_1_h");
                GoToOrientation(DEVICEPROT);
                MainWindow.tableViews()[0].scrollDown();
                DelayInSec(1);
                captureLocalizedScreenshot("73300_2");
                GoToOrientation(DEVICELEFT);
                MainWindow.tableViews()[0].scrollDown();
                captureLocalizedScreenshot("73300_2_h");
                GoToOrientation(DEVICEPROT);
                
                TapNavigationBackButton();
                    
            }else {
                
                LogMessage("73300 Fail");
            }
            
            CollapseGroupFromContactList(groupString);
            
        }else{
            
            LogMessage("73300 Fail");
        }
            
    }else {
        
        LogMessage("73300 Fail");
    }
}

function VerifyStringVideoCapableDisplayWellInContactList() {
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            LogMessage("445845 Fail");
            return;
        }
        else if (returnValue == 1){
            ValidateFirstRunSetPhoneNumber(PHONENUMBER);
        }
        else {
            LogMessage("Client is already signed in");
        }
    }
    
    var contactsButton = UIATarget.localTarget().frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Contacts")];
    if(ElementValidAndVisible(contactsButton) == true){
        
        TapElement(contactsButton);
        var string = GetValueFromKey("LOCID Collapsed");
        var groupString = GetValueFromKey("LOCID Pinned Contacts");
        var replaceString = string.replace("%@",groupString);
        var groupLabel = MainWindow.tableViews()[0].groups()[replaceString];
        if(ElementValidAndVisible(groupLabel) == true) {
            
            ExpandGroupFromContactList(groupString);
            
            if(WaitTableViewCellValidAndVisible(GetValueFromKey("LOCID Video capable")) == true){
                
                captureLocalizedScreenshot("445845_1");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("445845_1_h");
                GoToOrientation(DEVICEPROT);
                
                if(TapTableviewCellWithPredicate(GetValueFromKey("LOCID Video capable")) == true){
                    
                    captureLocalizedScreenshot("445845_2");
                    GoToOrientation(DEVICELEFT);
                    captureLocalizedScreenshot("445845_2_h");
                    GoToOrientation(DEVICEPROT);
                    
                    TapNavigationBackButton();
                }

            }else {
                
                LogMessage("445845 Fail");
            }
            
            CollapseGroupFromContactList(groupString);
            
        }else{
            
            LogMessage("445845 Fail");
        }
        
    }else {
        
        LogMessage("445845 Fail");
    }
}

function VerifyStringsNoResultsOnContactsSearchList() {
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            
            LogMessage("447572 Fail");
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
    var mainWindow = target.frontMostApp().mainWindow();
    var contactsButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Contacts")];
    
    if(ElementValidAndVisible(contactsButton) == true){
        
        TapElement(contactsButton);
        
        var searchBar = target.frontMostApp().mainWindow().searchBars()[0];
        if(searchBar.isVisible() == true){
            
            searchBar.setValue("saafgghhjhgjkgggjjj");
            DelayInSec(6);
         
            var searchCell_1 = MainWindow.tableViews()[0].cells()[GetValueFromKey("LOCID No Results")];
            var searchCell_2 = MainWindow.tableViews()[1].cells()[GetValueFromKey("LOCID No Results")];
            if(searchCell_2.isVisible() == true || searchCell_1.isVisible() == true){
                
                captureLocalizedScreenshot("447572");
                GoToOrientation(DEVICELEFT);
                captureLocalizedScreenshot("447572_h");
                GoToOrientation(DEVICEPROT);
            }else {
                
                LogMessage("447572 Fail");
            }
            
            DelayInSec(1);
            var button = mainWindow.buttons()[0];
            if(button.isVisible() == true){
                
                button.tap();
                DelayInSec(1);
            }
            
            var myInfoButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
            if(myInfoButton.isVisible() == false){
                
                mainWindow.buttons()[GetValueFromKey("LOCID Cancel")].tap();
            }
            DelayInSec(1);
        }else {
            
            LogMessage("447572 Fail");
        }
    }else {
        
        LogMessage("447572 Fail");
    }

}

function VerifyStringsBuddyMembersOnContactsList() {
    
    var returnValue = IsSignInScreenUp();
    if(returnValue == 1){
        
        SignInNormalConfigTest(SIPURI,PASSWORD);
        returnValue = WaitForSignInSuccess();
        if(returnValue == 0){
            
            LogMessage("458496 Fail");
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
    var mainWindow = target.frontMostApp().mainWindow();
    var contactsButton = target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID Contacts")];
    
    if(ElementValidAndVisible(contactsButton) == true){
        
        TapElement(contactsButton);
        
        var string = GetValueFromKey("LOCID Collapsed");
        var groupString = GetValueFromKey("LOCID Pinned Contacts");
        var replaceString = string.replace("%@",groupString);
        var groupLabel = MainWindow.tableViews()[0].groups()[replaceString];
        if(ElementValidAndVisible(groupLabel) == true) {
            
            groupLabel.tapWithOptions({tapOffset:{x:0.98, y:0.48}});
            
            captureLocalizedScreenshot("458496");
            GoToOrientation(DEVICELEFT);
            captureLocalizedScreenshot("458496_h");
            GoToOrientation(DEVICEPROT);
            
            TapNavigationBackButton();

        }else{
            
            LogMessage("458496 Fail");
        }

    }else {
        
        LogMessage("458496 Fail");
    }
    
}



function IsSignInScreenUp()
{
	Target.pushTimeout(2);
	var signInButton = MainWindow.tableViews()[MainWindow.tableViews().length - 1].buttons()[GetValueFromKey("LOCID Sign in")];
	Target.popTimeout();

	if(signInButton.isValid() == true) {
        
        return 1;
    }
    else{
        
        return 0;
    }
}


function IsSignInIngScreenUp()
{
   
	var signInButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Sign in")];

	if(signInButton.isValid() == true) {
        
        return 1;
    }
    var statusButton = MainWindow.buttons()[GetValueFromKey("LOCID Sign in")];
    
    if(statusButton.isValid() == true) {
        
        return 0;
    }
    
    var button = MainWindow.buttons()[GetValueFromKey("LOCID Understand")];
    if(button.isValid() == true) {
        
        button.tap();
        return 2;
    }
    
}

function IsFirstScreenUp()
{
	
	Target.pushTimeout(5);
	var button = MainWindow.buttons()[GetValueFromKey("LOCID Understand")];
	Target.popTimeout();
	if(button.isValid() == true) {
        
        return 1;
    }
    else
        return 0;
}


function WaitForOnlineMode()
{
//	var myInfoButton = Target.frontMostApp().tabBar().buttons()[GetValueFromKey("LOCID My Status")];
    
    var myInfoButton = MainWindow.buttons()[GetValueFromKey("LOCID ACCESSIBILITY_TIMELINE_BUTTON_PROFILE")];
    DelayInSec(1);

    if(myInfoButton.isValid() == true && myInfoButton.isVisible() == true) {
        
        return true;
    }
    else
        return false;
}

function SignInWithResiliency(phoneNumber)
{
	var flag = 0;
	Target.pushTimeout(40);
	var contactsNavigationBar = MainWindow.navigationBars()[GetValueFromKey("LOCID Contacts")];
	Target.popTimeout();
	try
	{
		IsValidAndVisible(contactsNavigationBar,"Contacts Navigation Bar")
	}
	catch(error)
	{
		try
		{
			ValidateTabBarButtons();
			WaitForOnlineMode();
			return;
		}
		catch(error)
		{
			validateFirstRun(phoneNumber);
		}
	}

}

function SignInAuto()
{
    var signInButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Sign in")];
    DelayInSec(1);
    
    if(signInButton.isValid() == true)
    {
        if(signInButton.isVisible())
        {
            signInButton.tap();
        }
    }
    
    Target.pushTimeout(15);
    var myInfoButton = TabBar.buttons()[GetValueFromKey("LOCID My Status")];
    Target.popTimeout();
    DelayInSec(1);
}

function SignOutApp()
{
    GotoMyInfo();
    
    var signoutButton =  MainWindow.tableViews()[0].cells()["Sign Out"];
    
    if(signoutButton.isValid() == true && signoutButton.isVisible() == true){
        
        signoutButton.tap();
    }

	Target.pushTimeout(15);
    var signInButton = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Sign in")];
	Target.popTimeout();
}

function SignInApp()
{
    var string = GetValueFromKey("LOCID Sign in");
    
    for(var i = 0;i < 10;i++)
    {
        var signInButton = MainWindow.tableViews()[0].buttons()[string];
        if(signInButton.isValid() == true)
        {
            if(signInButton.isVisible())
            {
                signInButton.tap();
                return;
            }
        }
        DelayInSec(1);
    }
}

function IsValidateFirstRun() {
    var target = UIATarget.localTarget();
	
    target.pushTimeout(15);
	var nextButton = target.frontMostApp().navigationBar().rightButton();
	target.popTimeout();
	try
	{
		IsValidAndVisible(nextButton,"Next Button");
		return 1;
	}
	catch(error)
	{
		return 0;
	}
    
}

function WaitForSignInSuccess(){
    
    var isVisible = 0;
   
    for(var i = 0;i < 120;i++)
	{
        var dismissButton = MainWindow.buttons()[GetValueFromKey("LOCID Dismiss")];
        if(dismissButton.isVisible() == true){
            
            dismissButton.tap();
            DelayInSec(1);
        }
        
        var continueButton = MainWindow.scrollViews()[0].buttons()[GetValueFromKey("LOCID Continue")];
        if(continueButton.isVisible() == true){
            
            continueButton.tap();
            DelayInSec(1);
        }
        
        var element = MainWindow.tableViews()[0].buttons()[GetValueFromKey("LOCID Sign in")];
        if(element.isVisible() == true){
            
            element.tap();
            DelayInSec(1);
        }
        
        if(MainWindow.buttons()[GetValueFromKey("LOCID Next")].isVisible() == true)
		{
      
            return 1;
        }
        else if(MainWindow.buttons()[GetValueFromKey("LOCID ACCESSIBILITY_TIMELINE_BUTTON_PROFILE")].isVisible() == true){

            return 2;
        }
        else
   
		    DelayInSec(1);
        
    }
    
    return isVisible
    
}




UIALogger.logStart( "iDeploy Launch Script" );

var target = UIATarget.localTarget();
UIALogger.logMessage( "Device name: " + target.name() );
UIALogger.logMessage( "Device model: " + target.model() );
UIALogger.logMessage( "Device system name: " + target.systemName() );
UIALogger.logMessage( "Device: system version: " + target.systemVersion() );

var app = target.frontMostApp();
UIALogger.logMessage( "Target: " + app.name() );

UIALogger.logDebug( "Retrieve custom URL" );
// get the custom URL from the environment variable
var host = target.host();
var result = host.performTaskWithPathArgumentsTimeout( "/usr/bin/printenv", ["IDEPLOY_CUSTOM_URL"], 60 );
if( result.exitCode != 0 )
{
	UIALogger.logFail( "printenv IDEPLOY_CUSTOM_URL returned " + result.exitCode );
}

var customURL = result.stdout.trim();
UIALogger.logDebug( "Custom URL: " + customURL );

app.setPreferencesValueForKey( "com.microsoft.apex.internal.customURL", customURL );

var window = app.mainWindow();

UIALogger.logDebug( "Set Custom URL textfield" );
var customURLTextfield = window.textFields()[0];
customURLTextfield.setValue( customURL );

UIALogger.logDebug( "Click Test button" );
window.buttons()["Test"].tap();

var outputTextfield = window.textFields()[1];
var output = outputTextfield.value();

if( output != "URL Can Be Opened" )
{
	UIALogger.logFail( output );
}
else
{
	UIALogger.logDebug( "Click Launch button" );
	window.buttons()["Launch"].tap();
		
	
	UIALogger.logPass( "Completed iDeploy Launch Script" );	
}

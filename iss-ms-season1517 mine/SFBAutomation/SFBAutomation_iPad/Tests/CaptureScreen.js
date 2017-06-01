function captureLocalizedScreenshot(name) {
    
  var target = UIATarget.localTarget();
  var model = target.model();
  var rect = target.rect();


  if (model.match(/iPhone/)) {
    if (rect.size.height == 568) model = "iphone5";
    else if(rect.size.height == 667) model = "iphone6";
    else if(rect.size.height == 736) model = "iphone6Plus";
    else model = "iphone";
  } else {
    model = "ipad";
  }

  var language = target.frontMostApp().preferencesValueForKey("AppleLanguages")[0];

  var parts = [language, model, name];
  UIALogger.logMessage("Screenshot"+parts.join("-"));
  target.delay(0.5);
  target.captureScreenWithName(parts.join("-"));
  LogMessage(name + " Successful!!");
}

function captureLocalizedScreenshotWithNoDisMiss(name) {
    
    var target = UIATarget.localTarget();
    var model = target.model();
    var rect = target.rect();

    if (model.match(/iPhone/)) {
        if (rect.size.height == 568) model = "iphone5";
        else if(rect.size.height == 667) model = "iphone6";
        else if(rect.size.height == 736) model = "iphone6Plus";
        else model = "iphone";
    } else {
        model = "ipad";
    }

    var language = target.frontMostApp().
    preferencesValueForKey("AppleLanguages")[0];
    
    var parts = [language, model, name];
    UIALogger.logMessage("Screenshot"+parts.join("-"));
    target.delay(0.5);
    target.captureScreenWithName(parts.join("-"));
    LogMessage(name + " Successful!!");
}



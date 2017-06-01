
# Technical Introduction


### **Insert Name and Value into resource file**

`Files Architecture`

![Files Architecture Diagram](StringAndResource.jpg)

If the resource doesn’t contain designer.cs file, the way to generate it is reflected from the figure below.Set the option of Access modifier of resource.resx file to “Public”,
add string name into the table whih three columns, set value for the name, the value might contain place holder.

`Add New Strings`

![Add New Strings Diagram](AddString.jpg)

### **Pick name and display values**

```csharp
        Activity r1 = activity.CreateReply(String.Format(Properties.Resources.StartProcess,"FeedBack Bot"));
        await connector.Conversations.ReplyToActivityAsync(r1);
```
“FeedBack Bot” will be plugged inside of the value of “StartProcess” from resource.resx.

### **New language selection**

`method from Reource.Designer.cs class`


Returns the value of the string resource localized for the specified culture.

```csharp
        public static string Resources_de_DE {
            get {
                return ResourceManager.GetString("Resources_de_DE", resourceCulture);
            }
        }
```        
Looks up a localized string similar to Hey, new friend! I am Feedback Bot. You help me with giving feedback on translation quality. .

```csharp
        public static string Command_In_What_Language {
            get {
                return ResourceManager.GetString("Command_In_What_Language", resourceCulture);
            }
        }
```

`resourceCulture will be set from MessagesController.cs class, when user clicks a language from language selection list`

```csharp
        if (currentLang != null)
        {
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLang);
        }
```

`Welcome message in German will be extracted from resource file.de-DE`

![Language options Diagram](WelcomeMsg.jpg)

![Language options Diagram](LanguageSelection.jpg)


Instructions on 

A) "how to generate the localized mapping files"

1) build the solution
2) In the build folder (bin\Debug or bin\Release), copy the fullPath of the CiFxMappingParser.exe file
3) open a command prompt, and navigate to this folder in the command prompt ("cd" command)
4) Run the following command lines in the commmand prompt to generate the localized mapping files:

(When the step "generate "UiToResourceID" mapping file" is done, user could update manually the UiToResourceID generated file if needed
When should it be modified?
Check the report an check the TotalNotFound count, this number should be 0.
Make sure that the list of UnChanged UiMappings\Properties is correct because those mappings will not be translated.)

[common]

4.1.1 download latest base_ui_hash_map.json from the link below:

https://skype.visualstudio.com/DefaultCollection/SCC/_git/internal_evp_e2e?path=%2Fdrivers%2Fs4l%2Fproduct_common%2Fbase_ui_hash_map.json&version=GBmaster&_a=contents

4.1.2 generate "UiToResourceID" mapping file

full\path\CiFxMappingParser.exe --sourceMappingPath ".\base_ui_hash_map.json" --englishResourcePath ".\LocalizedStrings.json" --DestinationPath ".\destination\common"

4.1.3 download latest rawstrings folder from the link below:

https://skype.visualstudio.com/SCC/_git/client_skypex_skype4life?path=%2Fclients%2Fcommon%2Fresources%2Frawstrings&version=GBmaster&_a=contents

4.1.4 generate Localized resource file

full\path\CiFxMappingParser.exe --sourceMappingPath ".\base_ui_hash_map.json" --englishResourcePath ".\LocalizedStrings.json" --mappingToLocIdPath "\destination\common\UiToResourceID.json" --translatedResourceFolderPath ".\rawstrings" --DestinationPath ".\destination\common"

[android]

4.2.1 download latest ui_hash_map_android.json from the link below:

https://skype.visualstudio.com/DefaultCollection/SCC/_git/internal_evp_e2e?path=%2Fdrivers%2Fs4l%2Fs4l_android%2Fui_hash_map.json&version=GBmaster&_a=contents

4.2.2 manually change the ui_config_map_android.json if the current ui_hash_map_android.json is not same as the latest ui_hash_map_android.json

4.2.3 generate "UiToResourceID" mapping file

full\path\CiFxMappingParser.exe --sourceMappingPath ".\ui_hash_map_android.json" --englishResourcePath ".\LocalizedStrings.json" --sourceConfigMappingPath ".\ui_config_map_android.json" --DestinationPath ".\destination\android" 

4.2.4 download latest rawstrings folder from the link below:

https://skype.visualstudio.com/SCC/_git/client_skypex_skype4life?path=%2Fclients%2Fcommon%2Fresources%2Frawstrings&version=GBmaster&_a=contents

4.2.5 generate Localized resource file

full\path\CiFxMappingParser.exe --sourceMappingPath ".\ui_hash_map_android.json" --englishResourcePath ".\LocalizedStrings.json" --sourceConfigMappingPath ".\ui_config_map_android.json" --mappingToLocIdPath "\destination\common\UiToResourceID.json" --translatedResourceFolderPath ".\rawstrings" --DestinationPath ".\destination\android"

[ios] 

4.3.1 download latest ui_config_map_ios.json from the link below:

https://skype.visualstudio.com/DefaultCollection/SCC/_git/internal_evp_e2e?path=%2Fdrivers%2Fs4l%2Fs4l_ios_v2%2Fui_hash_map.json&version=GBmaster&_a=contents

4.3.2 manually change the ui_config_map_ios.json if the current ui_hash_map_ios.json is not same as the latest ui_hash_map_ios.json

4.3.3 generate "UiToResourceID" mapping file

full\path\CiFxMappingParser.exe --sourceMappingPath ".\ui_hash_map_ios.json" --englishResourcePath ".\LocalizedStrings.json" --sourceConfigMappingPath ".\ui_config_map_ios.json" --DestinationPath ".\destination\ios"

4.3.4 download latest rawstrings folder from the link below:

https://skype.visualstudio.com/SCC/_git/client_skypex_skype4life?path=%2Fclients%2Fcommon%2Fresources%2Frawstrings&version=GBmaster&_a=contents

4.3.5 generate Localized resource file

full\path\CiFxMappingParser.exe --sourceMappingPath ".\ui_hash_map_ios.json" --englishResourcePath ".\LocalizedStrings.json" --sourceConfigMappingPath ".\ui_config_map_ios.json" --mappingToLocIdPath "\destination\common\UiToResourceID.json" --translatedResourceFolderPath ".\rawstrings" --DestinationPath ".\destination\ios"

[web] 

4.4.1 download latest ui_config_map_ios.json from the link below:

https://skype.visualstudio.com/DefaultCollection/SCC/_git/internal_evp_e2e?path=%2Fdrivers%2Fs4l%2Fs4l_web%2Fui_hash_map.json&version=GBmaster&_a=contents

4.3.2 manually change the ui_config_map_web.json if the current ui_hash_map_web.json is not same as the latest ui_hash_map_web.json

4.4.3 generate "UiToResourceID" mapping file

full\path\CiFxMappingParser.exe --sourceMappingPath ".\ui_hash_map_web.json" --englishResourcePath ".\LocalizedStrings.json" --sourceConfigMappingPath ".\ui_config_map_web.json" --DestinationPath ".\destination\web"

4.4.4 download latest rawstrings folder from the link below:

https://skype.visualstudio.com/SCC/_git/client_skypex_skype4life?path=%2Fclients%2Fcommon%2Fresources%2Frawstrings&version=GBmaster&_a=contents

4.4.5 generate Localized resource file

full\path\CiFxMappingParser.exe --sourceMappingPath ".\ui_hash_map_web.json" --englishResourcePath ".\LocalizedStrings.json" --sourceConfigMappingPath ".\ui_config_map_web.json" --mappingToLocIdPath "\destination\common\UiToResourceID.json" --translatedResourceFolderPath ".\rawstrings" --DestinationPath ".\destination\web"

B) "how to update localized files"
1) Download the latest files from S4L repo: 
https://skype.visualstudio.com/SCC/_git/client_skypex_skype4life?path=%2Fclients%2Fcommon%2Fresources%2Frawstrings&version=GBmaster&_a=contents

2) In this folder, Replace "LocalizedStrings.json" file with "LocalizedStrings_en.json" from S4L repo 
Make sure to rename back the new file to "LocalizedStrings.json"

3) Replace the "rawstrings" folder content with the localized resources from S4L repo



using JiraApi;
using Newtonsoft.Json.Linq;
using SkypeIntlPortfolio.Ajax.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.Ajax.Model
{
    public class Utils
    {
        public static string[] GetLanguages()
        {
            return new string[]
            {
               #region
               // "af-ZA"
               //,"am-ET"
               //,"arn-CL"
               //,"ar-SA"
               //,"ar-SA.pseudo"
               //,"as-IN"
               //,"az-Latn-AZ"
               //,"be-by"
               //,"bg-BG"
               //,"bn-BD"
               //,"bn-IN"
               //,"bs-Cyrl-BA"
               //,"bs-Latn-BA"
               //,"ca-ES"
               //,"ca-es-valencia"
               //,"chr-cher-us"
               //,"cs-CZ"
               //,"cy-GB"
               //,"da-DK"
               //,"de-DE"
               //,"el-GR"
               //,"en-AU"
               //,"en-GB"
               //,"en-US"
               //,"es-ES"
               //,"es-MX"
               //,"et-EE"
               //,"eu-ES"
               //,"fa-IR"
               //,"fi-FI"
               //,"fil-PH"
               //,"fr-CA"
               //,"fr-FR"
               //,"ga-IE"
               //,"gd-gb"
               //,"gl-ES"
               //,"gn-PY"
               //,"gu-IN"
               //,"ha-Latn-NG"
               //,"ha-NG"
               //,"he-IL"
               //,"hi-IN"
               //,"hr-HR"
               //,"hu-HU"
               //,"hy-AM"
               //,"id-ID"
               //,"ig-NG"
               //,"is-IS"
               //,"it-IT"
               //,"iu-Latn-CA"
               //,"ja-JP"
               //,"ka-GE"
               //,"kk-KZ"
               //,"km-KH"
               //,"kn-IN"
               //,"kok-IN"
               //,"ko-KR"
               //,"ku-arab-iq"
               //,"ky-KG"
               //,"lb-LU"
               //,"lo-LA"
               //,"lt-LT"
               //,"lv-LV"
               //,"mi-NZ"
               //,"mk-MK"
               //,"ml-IN"
               //,"mn-MN"
               //,"mn-MN-cyrl"
               //,"mn-Mong-CN"
               //,"moh-CA"
               //,"mr-IN"
               //,"ms-BN"
               //,"ms-MY"
               //,"mt-MT"
               //,"my-MM"
               //,"nb-NO"
               //,"ne-NP"
               //,"nl-NL"
               //,"nn-NO"
               //,"nso-ZA"
               //,"or-IN"
               //,"pa-arab-pk"
               //,"pa-IN"
               //,"pl-PL"
               //,"prs-AF"
               //,"ps-AF"
               //,"pt-BR"
               //,"pt-PT"
               //,"qut-gt"
               //,"quz-PE"
               //,"rm-CH"
               //,"ro-RO"
               //,"ru-RU"
               //,"rw-RW"
               //,"sd-arab-pk"
               //,"sd-PK"
               //,"se-NO"
               //,"si-LK"
               //,"sk-SK"
               //,"sl-SI"
               //,"so-SO"
               //,"sq-AL"
               //,"sr-cyrl-ba"
               //,"sr-Cyrl-CS"
               //,"sr-Cyrl-RS"
               //,"sr-Latn-CS"
               //,"sr-Latn-RS"
               //,"sv-SE"
               //,"sw-KE"
               //,"ta-IN"
               //,"te-IN"
               //,"tg-cyrl-tj"
               //,"th-TH"
               //,"ti-et"
               //,"tk-TM"
               //,"tl-PH"
               //,"tn-ZA"
               //,"tr-TR"
               //,"tt-RU"
               //,"ug-cn"
               //,"uk-UA"
               //,"ur-PK"
               //,"uz-Latn-UZ"
               //,"vi-VN"
               //,"wo-SN"
               //,"xh-ZA"
               //,"yo-NG"
               //,"zh-CN"
               //,"zh-HK"
               //,"zh-TW"
               //,"zu-ZA"
              #endregion
               "[Non Language-Specific]",
                "Afrikaans",
                "Albanian",
                "Amharic",
                "Arabic",
                "Armenian",
                "Assamese",
                "Azerbaijani (Latin)",
                "Basque",
                "Belarusian",
                "Bengali (Bangladesh)",
                "Bengali (India)",
                "Bosnian Latin",
                "Bulgarian",
                "Catalan",
                "Central Kurdish",
                "Cherokee",
                "Chinese (Hong Kong SAR)",
                "Chinese (Simplified)",
                "Chinese (Traditional)",
                "Croatian",
                "Czech",
                "Danish",
                "Dari/Afghan Persian",
                "Dutch",
                "English",
                "English (GB)",
                "English (India)",
                "Estonian",
                "Farsi (Persian)",
                "Filipino",
                "Finnish",
                "French (Canada)",
                "French (France)",
                "Galician",
                "Georgian",
                "German",
                "Greek",
                "Gujarati",
                "Hausa Latin",
                "Hebrew",
                "Hindi",
                "Hungarian",
                "Icelandic",
                "Igbo",
                "Indonesian",
                "Inuktitut Latin",
                "Irish",
                "isiXhosa",
                "isiZulu",
                "Italian",
                "Japanese",
                "Kannada",
                "Kazakh",
                "Khmer",
                "K'iche'",
                "Kinyarwanda",
                "Kiswahili",
                "Konkani",
                "Korean",
                "Kyrgyz/Kirghiz",
                "Lao (Lao PDR)",
                "Latvian",
                "Lithuanian",
                "Luxembourgish",
                "Macedonian",
                "Malay",
                "Malayalam",
                "Maltese",
                "Maori",
                "Marathi",
                "Mongolian Cyrillic",
                "Nepali",
                "Norwegian (Bokmal)",
                "Norwegian (Nynorsk)",
                "Odia/Oriya",
                "Polish",
                "Portuguese (Brazil)",
                "Portuguese (Portugal)",
                "Punjabi (India)",
                "Punjabi (Pakistan)",
                "Quechua",
                "Romanian",
                "Russian",
                "Scottish Gaelic",
                "Serbian - Cyrillic",
                "Serbian - Cyrillic (Bosnia)",
                "Serbian - Latin",
                "Sesotho sa Leboa",
                "Setswana",
                "Sindhi",
                "Sinhala/Sinhalese",
                "Slovak",
                "Slovenian",
                "Spanish",
                "Spanish (Mexico)",
                "Spanish (United States)",
                "Swedish",
                "Tagalog (Philippines)",
                "Tajik",
                "Tamil",
                "Tatar",
                "Telugu",
                "Thai",
                "Tigrinya",
                "Turkish",
                "Turkmen",
                "Ukrainian",
                "Urdu",
                "Uyghur",
                "Uzbek - Latin script",
                "Valencian",
                "Vietnamese",
                "Welsh",
                "Wolof",
                "Yoruba"
            };
        }

        public static Issue CreateAAAPTJira(string productName, string epicLabel, string core_intl_folders_location, string core_Source_File_path, bool RW_permissions, string eol, string Expected_Date_For_Walking, string Expected_Date_For_Running)
        {
            // Create the JiraModel object to provide the authentication to access Jira. get all the issues for different projects.
            var jiraRootUrl = "https://jiratest.skype.net";

            JiraApiObjectModel jiraModel = new JiraApiObjectModel("AAAPT", jiraRootUrl);

            //1 - create a pbi
            Issue newIssue = new Issue();
            Fields field = new Fields();
            field.customfield_10241 = new string[] { epicLabel };

            newIssue = jiraModel.CreateIssue(summary: String.Format("{0} : Request to Onboard", productName),
                                                   description: "Core intl-folders location: " + core_intl_folders_location + Environment.NewLine +
                                                   "Core Source-File path: " + core_Source_File_path + Environment.NewLine +
                                                   "europe,skwttad RW permissions done?  " + RW_permissions + Environment.NewLine +
                                                   "EOL: " + eol + Environment.NewLine +
                                                   "Expected Date for Walking: " + Expected_Date_For_Walking + Environment.NewLine +
                                                   "Expected Date for Running: " + Expected_Date_For_Running,
                                                   priorityName: "P3 - Medium",
                                                   issueTypeName: "Enabling Specification",
                                                   //.epicLabel: epicLabel,
                                                   labels: new List<string> { "Fabric_OnboardingRequest" },
                                                   assigneeAlias: "raffael",
                                                   fields: field
                                                   );
            return newIssue;
        }

        public static List<Issue> CreateLYNCFABJira(string productName, string epicLabel)
        {
            var jiraRootUrl = "https://jiratest.skype.net";
            var jiraModel = new JiraApiObjectModel("LYNCFAB", jiraRootUrl);
            //create one parent issue
            List<Issue> issuesList = new List<Issue>();
            Fields field = new Fields();
            field.customfield_10241 = new string[] { epicLabel };
            Issue parentIssue = jiraModel.CreateIssue(summary: String.Format(@"{0} Test\Production Tenant Fabric Onboarding Testing", productName),
                                                    description: @"Fabric onboarding schedule tracking:
                                                            https://microsoft.sharepoint.com/teams/SkypeIntl/ipe/_layouts/OneNote.aspx?id=%2fteams%2fSkypeIntl%2fipe%2fShared%20Documents%2fLync%20Notebook&wd=target%28Fabric%20Onboarding.one%7c7814C486-B392-4592-BC02-5AD7A57D7B9E%2fFabric%20Onboarding%20planning%7cFC27131C-8787-4466-91FF-5ED5ECA89C6A%2f%29
                                                            onenote:https://microsoft.sharepoint.com/teams/SkypeIntl/ipe/Shared%20Documents/Lync%20Notebook/Fabric%20Onboarding.one#Fabric%20Onboarding%20planning&section-id={7814C486-B392-4592-BC02-5AD7A57D7B9E}&page-id={FC27131C-8787-4466-91FF-5ED5ECA89C6A}&end
                                                            Fabric related IPE onenote section: https://microsoft.sharepoint.com/teams/SkypeIntl/ipe/_layouts/OneNote.aspx?id=%2fteams%2fSkypeIntl%2fipe%2fShared%20Documents%2fLync%20Notebook&wd=target%28Fabric.one%7cCC66EA73-0B6A-48BE-83AF-AB26A348964F%2f%29
                                                            onenote:https://microsoft.sharepoint.com/teams/SkypeIntl/ipe/Shared%20Documents/Lync%20Notebook/Fabric.one#section-id={CC66EA73-0B6A-48BE-83AF-AB26A348964F}
                                                            Documentation and training materials are here
                                                            \\emea\shares\TESTKIT\o15samplefiles\Eng_Excellence\Fabric Queries & Reports
                                                            \\emea\shares\TESTKIT\o15samplefiles\Eng_Excellence\Leaf & Fabric
                                                            \\emea\shares\TESTKIT\o15samplefiles\Eng_Excellence\LocVer & Commenting
                                                            \\emea\shares\TESTKIT\o15samplefiles\Eng_Excellence\Resource Lifecycle",
                                                    priorityName: "P3 - Medium",
                                                    issueTypeName: "Enabling Specification",
                                                    //epicLabel: epicLabel,
                                                    assigneeAlias: "raffael",
                                                    fields: field
                                                   );
            //add parent issue into the issues list.
            issuesList.Add(parentIssue);
            string[] summaryOfCrawl = new string[]{String.Format("{0} / File sanity , file structure , resource count , encoding , meta data", parentIssue.key),
                                                    String.Format("{0} / File round trip in and out of fabric", parentIssue.key),
                                                    String.Format("{0} / Language Settings are populated correctly and all language can be updated", parentIssue.key),
                                                    String.Format("{0} / LocVer Behavior", parentIssue.key),
                                                    String.Format("{0} / Check if language mapping correct or not", parentIssue.key),
                                                    String.Format("{0} / queries and filter \"if any\" working correctly", parentIssue.key),
                                                    String.Format("{0} / Validate PR test , build sanity check", parentIssue.key)
                                                  };

            string[] descriptionOfCrawl = new string[]{
                    @"Covers various sanity testing of resources imported to Fabric, including
                    -File sets in build and Fabric. Is the number of files being exported accurate?
                    -Project grouping
                    -Encoding
                    -Resource numbers and ID diff between build and Fabric.
                    -editing and saving via LEAF including changing translation, adding LocComment, Locver)",

                    @"Entire resource flow testing covering,
                    · Source file update.
                    <From Valdo - If you wish to test the new key adding to source, there are two options:
                    1. Make the change in the client repository
                    2. We could make a copy of the client repository to eg. https://velosipeed.skype.net/l10n/Fabric-translations/EmoticonsTestSource and configure quickbuild to monitor changes from there.>
                    · LCG update
                    · LCG import
                    · Rename LCG files
                    · Delete LCG files
                    <Fabric Notification (to: FabricSkypeNotify alias) is sending you about LCG deletion or detection?>

                · Download query result in Leaf
                    · Change in Loc string based on LCG change
                    · Change Loc comment and upload to LEAF
                    · Change locver and upload to LEAF
                    · Export LCL - Skype LCL files (applyDPK) location: http://orf-tfs1:8080/tfs/Skype
                    · ApplyDPK
                    <applyDPK test cases>
                    Verify Fabric exports changes to changeset/DPKs (TFS check) Note -“pporfsrv” account is used to check in LCL files.
                    Export all LCL files (at least 1 from every language, and at least 1 full set of a language) and inspect files
                    Review differences between original and exported LCL files
                    Things to look for:
                    · Are unnecessary comments removed?
                    · Are all of the translations still there, or have they been removed? (can be caused by CFG error or TgtCul being different than the Culture listed inside Fabric)
                    · Is the number of files being exported accurate?
                    · Are the files being exported accurate in terms of EOL for each culture?
                    · Verify Check-in files in L10N SVN, run diff with the localized files which have been already released before. (Released files are available at \\skype_drop.corp.microsoft.com\FS_SKYPE_TLL\Skype_Released Files\

                · Verify that checked in string are unpended (pending = false)

                · Check LCT files are present per build @ \\skype_drop.corp.microsoft.com\FS_SKYPE_TLL\LCT
                    · On-demand applyDPK (with a couple of files only -> moved from #6. NOTE - confirm if it is ready to test with TFS)
                    · Verify any changes in resources, meta data. Modify various flags (IPE review, Translation an Approval, etc) and verify the changes are recorded in history. (only test a couple of files should be enough -> Moved from #8)",

                    @"language per component
                    ◾Check if you see CFG contains the correct EOL and they are correctly imported to Fabric.
                    ◾If the project was shipped before, verify if Fabric shows the same numbers of resources for the shipped languages.",

                    @"For new/updated strings in Skype projects, IPE can test adding rules as needed, until we introduce setting files to Skype project to apply autorules.
                    -Verify Resource Locking Workflow (Add Locver and see if it is reflected correctly)
                    -For projects using Locver before, ensure the existing Locver rules are present in Fabric and validation is working as expected.",

                    @"Verify if the mapping between LCL files and target files added to the repo are mapped correctly. The language structure would be different based on the platforms and could be so per Skype repro's requirement. ",

                    @"||Check||Ok?||
                    | Query by <Project> + <Resource ID> within 2 languages (Pick 2-3 languages within the project EOL): : | ? |
                    | Query by Changed Date within 2 languages: | ? |
                    | Query by Changed During within 2 languages: | ? |
                    | Query by File Path within a language: | ? |
                    | Query by Localization Changed -> String Value within a language: | ? |
                    | Query by Localization Changed Date within a language: | ? |
                    | Query by Localize within a language: | ? |
                    | Query by Locked within a language: | ? |
                    | Query by Project within 2 languages: | ? |
                    | Query by Resource ID: | ? |
                    | Query by Resource ID (Full Path): | ? | " +
                    "| Query by Review Flag -> IPE Review (String) -> Status, all strings value = \"New\", it is normal?| ? |"+
                    "| Query by Review Flag -> IPE Review (Binary) -> Status, all strings value = \"New\", it is normal?| ? |"+
                    "| Query by Review Flag -> Core Review -> Status, all strings value = \"New\", it is normal?| ? |"+
                    "| Query by Review Flag -> Test Review -> Status, all strings value = \"New\", it is normal?| ? |"+
                    @"| Query by String value within 3 languages: | ? |
                    | Query by String Localization Status within a language: | ? |
                    | Query by String value - Normalized within 3 languages: | ? |",

                    @"comments from Yuka:
                    It is up to the product owner IPE to decide if their product needs PR testing or can be added as part of regular Loc testing. (as part of test pass, for example).
                        Prior to that, iSS eng team to take a diff between the released Localized files and the ones generated via Fabric to see any unexpected diff."
            };

            for (int i = 1; i <= 7; i++)
            {
                Issue subIssue1 = jiraModel.CreateIssue(summary: summaryOfCrawl[i - 1],
                                                       description: descriptionOfCrawl[i - 1],
                priorityName: "P3 - Medium",
                issueTypeName: "Development Task",
                //epicLabel: epicLabel,
                labels: new List<string> { "CrawlTest" + i },
                assigneeAlias: "raffael",
                parentIssueKey: parentIssue.key,
                fields: field
                );
                //add each sub pbi into issue list.
                issuesList.Add(subIssue1);
            }
            string[] summaryofWalk = new string[]{String.Format("{0} / <PROD only> E2E Test with LSP",parentIssue.key),
                                            String.Format("{0} / <PROD only> Flags are set correctly prior toProduction starts.",parentIssue.key),
                                            String.Format("{0} / <PROD only> Request project to be available for dash boarding",parentIssue.key)
            };

            string[] descriptionofWalk = new string[]{
                "LSP verify they can see what they need to see per project using \"LSP_Default\" filter. (IPE review flag use is expected for Skype projects at this point)"+

                @"Ensure before LSP testing, flags are set correctly: e.g.
                IPE-Review
                Revert changes from 7-Final
                IPE-Unblock

                Login as LSP (PROD testing only before LSP testing)
                    Revert to non-pending in WebUI
                    Query in project
                    Change TRAP and upload in LEAF
                    Change TRAP using Flow Control

                Login as Translator (check w/ multiple languages)
                    Download query result in Leaf
                    Translate strings
                        Confirm translations w/o permissions cannot be uploaded
                        Confirm tranlsations w/ permissions can be uploaded
                    Blocked strings
                        Confirm they cannot be viewed
                        Confirm unblocked resources can be viewed
                    Frozen strings
                        Confirm resources can be downloaded to LEAF
                        Confirm resources cannot be uploaded from LEAF
                    Clanger information
                        Confirm clanger added to 1 language is visible in another
                    Run LocVer validation against all cultures and investigate LocVer errors
                        Confirm LocVer validation passes for those no LocVer errors
                        Confirm LocVer error is valid for those with LocVer errors
                        Investigate existing LocVer errors

                Upload changes from Leaf",

                @"Based on Project status, various flags are set correctly to ensure LSPs are only seeing what they need to work on.
                <PROD sanity checks>
                Freeze resources using Bulk Editor
                · Confirm resources can be queried in WebUI
                · Confirm translations cannot be changed in WebUI
                · Confirm resources cannot be reverted
                Unfreeze resources using bulik editor
                · Confirm queries can be queried in WebUI
                · Confirm translations can be changed in WebUI
                Revert translation using WebUI
                · Confirm revert is successful and pending = false in WebUI
                Block resources using (confirm resources that are not nodes has ReviewFlag set to 0 - Blocked):
                · SP
                · Web Editor
                · Bulk Editor
                Unblock resources using (confirm resources that are not nodes have ReviewFlag set to 1 - Unblocked):
                · SP
                · Web Editor
                · Bulk Editor
                Export Excel

                Login as LSP (PROD testing only before LSP testing)
                <Check lists (exactly how each project should look like vary, depending on the project’s readiness so need to be updated when ready to test this on PROD>
                · Revert to non-pending in WebUI
                · Query in project
                · Change TRAP and upload in LEAF
                · Change TRAP using Flow Control
                · IPE BLOCKED?
                · IPE Reviewed?
                · Frozen?
                · TRAP - set to 7-final? ",

                @"· Request for PROD dashboarding (to Raffaele)
                · Verify that the tenant is in Fabric BI (Need to request to Martin O'Flaherty @ Aziz team)
                · Verify that the tenant is in Word Count report (same request as Fabric BI)",
            };

            for (int j = 1; j <= 3; j++)
            {
                Issue subIssue2 = jiraModel.CreateIssue(summary: summaryofWalk[j - 1],
                                         description: descriptionofWalk[j - 1],
                                         priorityName: "P3 - Medium",
                                         issueTypeName: "Development Task",
                                         //epicLabel: epicLabel,
                                         labels: new List<string> { "WalkTest" + j },
                                         assigneeAlias: "raffael",
                                         parentIssueKey: parentIssue.key,
                                         fields: field
                                         );

                //add sub pbis into the issues list.
                issuesList.Add(subIssue2);
            }

            return issuesList;
        }

        public static Product OnboardProduct(int productKey, FabricOnboardingInfo onboardingInfo)
        {
            using (SkypeIntlPlanningPortfolioEntities portFolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                //search everything where ...   return null if there is not a mathing object was found
                Product pro = portFolioContext.Products.Where(p => p.ProductKey == productKey).FirstOrDefault();
                pro.Fabric_Status = "New";
                pro.Fabric_Onboarding_EpicLabel = onboardingInfo.EpicLabel;
                portFolioContext.SaveChanges();
                return pro;
            }
        }

        public static Product AddProduct(ProductInfo productInfo, FabricOnboardingInfo fabricOnboardingInfo)
        {
            using (SkypeIntlPlanningPortfolioEntities portFolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Product prod = new Product();
                prod.Product_Name = productInfo.Product_Name;
                //prod.Fabric_Onboarding_EpicLabel = productInfo.Epic_Label;
                //prod.Product_Description = productInfo.Product_Description;
                //prod.Family = productInfo.Family;
                //prod.User_Type = productInfo.User_Voice;
                //prod.Core_Team_s_Location = productInfo.Core_Team_Location;
                //prod.Loc_PM_Alias = productInfo.PM_Alias;
                //prod.Loc_PM_Location = productInfo.Loc_PM_Location;
                //prod.Resource_File_Path = productInfo.Resource_File_Path;
                //prod.Core_Team_Contacts = productInfo.Core_Team_Contacts;
                prod.Product_Status = null;
                prod.Product_Type = null;
                prod.ProductStudioLink = null;
                if (fabricOnboardingInfo == null)
                {
                    prod.Fabric_Status = null;
                }
                else
                {
                    prod.Fabric_Status = "New";
                }
                portFolioContext.Products.Add(prod);
                portFolioContext.SaveChanges();
                return prod;
            }
        }

        public static List<EOL> UpdateProduct(int productKey, List<string> eolToAdd, List<string> eolToDelete)
        {
            //1 - First round validation

            if (!eolToAdd.Any() && !eolToDelete.Any())
            {
                throw new Exception("You can't set these two lists equal to empty lists!");
            }

            foreach (string duplicatedItem in eolToDelete)
            {
                if (eolToAdd.Contains(duplicatedItem))
                {
                    throw new Exception("eolToAdd list can not contain the same elements of eolToDelete list!");
                }
            }

            foreach (string duplicatedItem in eolToAdd)
            {
                if (eolToDelete.Contains(duplicatedItem))
                {
                    throw new Exception("eolToDelete list can not contain the same elements of eolToAdd list!");
                }
            }

            //2 - update eol table
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                //2.1 - Get list of eols that are associated to the specified product, we will only insert the one that doesn't exist already in the database

                Dictionary<string, EOL> existingProductLlccList = new Dictionary<string, EOL>();

                //check if the given productkey exist the current eol table, if no,assign null to the dictionary.
                List<int> ProductKeyExist = (from l in portfolioContext.EOLs
                                             where l.ProductKey == productKey
                                             select l.ProductKey).ToList();
                if (ProductKeyExist.Contains(productKey))
                {
                    existingProductLlccList = (from l in portfolioContext.EOLs
                                               where l.ProductKey == productKey
                                               select l).ToDictionary(c => c.llCC, c => c);
                }

                Dictionary<string, string> languageNames = portfolioContext.suppData_ExtendedEOL
                        .Where(c => eolToAdd
                        .Contains(c.llCC))
                        .Select(c => new { c.llCC, c.Language_Name })
                        .ToDictionary(c => c.llCC, v => v.Language_Name);

                foreach (string llccToAdd in eolToAdd)
                {
                    //2.1.1 check if the eol exist by checking the LLCC key in the list of eol for the specified products
                    if (!existingProductLlccList.ContainsKey(llccToAdd))
                    {
                        var newEol = new EOL();
                        newEol.ProductKey = productKey;
                        newEol.llCC = llccToAdd;
                        newEol.Language_Support_Level = null;

                        string languageName = languageNames[llccToAdd];
                        newEol.Language_Name = languageName;

                        //2.1.1.1 insert eol row in the Eol table
                        portfolioContext.EOLs.Add(newEol);
                    }
                    else
                    {
                        throw new Exception("You can't insert a new record which has an existing productkey and an existing llcc in the EOL table");
                    }
                }

                //2.1.2 delete LLCC with the a matching productKey from EOL
                foreach (string llccToDelete in eolToDelete)
                {
                    EOL eolItemToDelete = null;
                    if (existingProductLlccList.Count != 0)
                    {
                        if ((eolItemToDelete = existingProductLlccList[llccToDelete]) != null)
                        {
                            portfolioContext.EOLs.Remove(eolItemToDelete);
                        }
                    }
                    else
                    {
                        throw new Exception("The llcc data you want to delete doesn't exist in the table!");
                    }
                }
                //2.2 save changes to eol table in the database
                portfolioContext.SaveChanges();

                //2.3 get the list of eols related to the current product in the database now after insert/delete operation has been done
                //the database is shared and used by many users that might have added/deleted eol rows during the above add/remove operation (client side)
                //that's why it's safer to get the list of eols for the specified product again, to get exactly what is in the database
                var result = portfolioContext.EOLs.Where(eol => eol.ProductKey == productKey).ToList();
                return result;
            }
        }

        public static List<ProductInfo> GetProductInfo(int[] productKeys)
        {
            List<ProductInfo> result = new List<ProductInfo>();
            List<vw_MilestoneTestScheduleInfo_ForOldSchedule> rawData = null;
            List<int> topReleaseIDList = new List<int>();
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                foreach (int pk in productKeys)
                {
                    var subReleaseIDList = context.Releases.Where(r => r.ProductKey == pk && r.Deleted == false).OrderByDescending(r => r.VSO_ID).Take(20).Select(r => r.VSO_ID).ToList();
                    topReleaseIDList.AddRange(subReleaseIDList);
                }
                //display product even it doesn't contain any release
                rawData = context.vw_MilestoneTestScheduleInfo_ForOldSchedule.Where(c => (productKeys.Contains(c.ProductKey)) ||
                                                                                         (topReleaseIDList.Contains(c.Release_ID.Value))).ToList();
            }

            foreach (var product in rawData.GroupBy(c => c.ProductKey))
            {
                var prod = product.First();
                var productInfo = new ProductInfo
                {
                    Family = prod.Product_Family,
                    Product_Name = prod.Product_Name,
                    Loc_PM_Location = prod.Location,
                    ProductKey = prod.ProductKey,
                };

                var productReleaseInfo = new List<ReleaseInfo>();
                //don't return all the release under one product, only pick the latest 20.
                //foreach (var release in product.Where(c => c.Release_ID.HasValue).GroupBy(c => c.Release_ID))
                //foreach (var release in product.Where(c => c.Release_ID.HasValue).OrderByDescending(c => c.Release_ID).GroupBy(c => c.Release_ID))
                foreach (var release in product.Where(c => c.Release_ID.HasValue).OrderBy(c => c.Release_ID).GroupBy(c => c.Release_ID))
                {
                    var rel = release.First();
                    var releaseInfo = new ReleaseInfo
                    {
                        Release_Name = rel.Release_Title,
                        ReleaseKey = rel.Release_ID.Value,
                        Release_Start_Date = rel.Release_LocStartDate,
                        Release_End_Date = rel.Release_DueDate,
                        Release_Tags = rel.Release_Tags,
                        Release_Assigned_To = rel.Release_Assigned_To,
                        Release_Url = rel.Release_Url
                    };

                    var releaseMilestones = new List<MilestoneInfo>();
                    foreach (var milestone in release.Where(c => c.MilestoneKey.HasValue))
                    {
                        var milestoneInfo = new MilestoneInfo
                        {
                            ProductKey = productInfo.ProductKey,
                            ReleaseKey = releaseInfo.ReleaseKey,
                            MilestoneKey = milestone.MilestoneKey.Value,
                            Milestone_Name = milestone.Milestone_Name,
                            Milestone_Start_Date = milestone.Milestone_Start_Date,
                            Milestone_End_Date = milestone.Milestone_End_Date,
                            MilestoneCategoryKey = milestone.MilestoneCategoryKey.Value,
                            MilestoneCategoryName = milestone.Milestone_Category_Name,
                            Milestone_Assigned_To = milestone.Milestone_Assigned_To
                        };

                        releaseMilestones.Add(milestoneInfo);
                    }
                    releaseInfo.Milestones = releaseMilestones.ToArray();

                    var releaseTestSchedules = new List<TestScheduleInfo>();
                    foreach (var testSchedule in release.Where(c => c.TestScheduleKey.HasValue))
                    {
                        int assignedResources = 0;
                        if (testSchedule.AssignedResources.HasValue)
                        {
                            assignedResources = testSchedule.AssignedResources.Value;
                        }
                        int test_milestoenCategoryKey = 0;
                        if (testSchedule.TestMilestoneCategoryKey.HasValue)
                        {
                            test_milestoenCategoryKey = testSchedule.TestMilestoneCategoryKey.Value;
                        }
                        var testScheduleInfo = new TestScheduleInfo
                        {
                            ProductKey = productInfo.ProductKey,
                            ReleaseKey = releaseInfo.ReleaseKey,
                            TestScheduleKey = testSchedule.TestScheduleKey.Value,
                            TestScheduleName = testSchedule.TestSchedule_Name,
                            TestScheduleStartDate = testSchedule.TestSchedule_Start_Date,
                            TestScheduleEndDate = testSchedule.TestSchedule_End_Date,
                            TestScheduleUrl = testSchedule.Vso_Web_Url,
                            AssignedResources = assignedResources,
                            MilestoneCategoryKey = test_milestoenCategoryKey
                        };

                        releaseTestSchedules.Add(testScheduleInfo);
                    }
                    releaseInfo.TestSchedules = releaseTestSchedules.ToArray();

                    productReleaseInfo.Add(releaseInfo);
                }

                productInfo.Releases = productReleaseInfo.ToArray();

                result.Add(productInfo);
            }
            return result;
        }

        public static VsoContext GetVsoContext()
        {
            string authenticationKey = WebConfigurationManager.AppSettings["VsoRootAccount"];
            string vsoPrivateKey = WebConfigurationManager.AppSettings["VsoPrivateKey"];
            return new VsoContext(authenticationKey, vsoPrivateKey);
        }

        public static IEnumerable<T> GetChildrenOfType<T>(Control parent) where T : class
        {
            var childQueue = new Queue<Control>();
            foreach (Control child in parent.Controls.Cast<object>().Where(c => c is Control))
            {
                childQueue.Enqueue(child);
            }
            while (childQueue.Any())
            {
                Control currentItem = childQueue.Dequeue();
                if (currentItem is T)
                {
                    yield return currentItem as T;
                }

                foreach (Control child in currentItem.Controls.Cast<object>().Where(c => c is Control))
                {
                    childQueue.Enqueue(child);
                }
            }
        }

        public static T GetFirstParentOfType<T>(Control child) where T : class
        {
            Control currentParent = child.Parent;
            while (currentParent != null)
            {
                if (currentParent is T)
                    return currentParent as T;
                currentParent = currentParent.Parent;
            }
            return null;
        }

        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T GetValue<T>(JObject container, string key)
        {
            return container[key] != null ? (T)container[key].ToObject<T>() : default(T);
        }

        //this method is used to check if a given vso-side tag belongs to the original 6 in DB
        public static bool CheckExtraVSOTags(string vsoTag)
        {
            //Dictionary<string, int> dictionary = new Dictionary<string, int>();
            bool IsExtraVsoTags = true;
            var allowedValues = new Dictionary<string, string>
            {
                {"Loc_Ready","Loc_Ready"},
                {"Loc_Start","Loc_Start"},
                {"Loc_Progressing","Loc_Progressing"},
                {"Loc_EndGame","Loc_EndGame"},
                {"Loc_Signoff","Loc_Signoff"},
                {"Loc_Retro","Loc_Retro"},
            };

            if (allowedValues.ContainsKey(vsoTag))
            {
                IsExtraVsoTags = false;
            }

            return IsExtraVsoTags;
        }

        public static string FilterTags(string tags)
        {
            HashSet<string> hash_Tags = new HashSet<string>();
            string[] tagsArray = tags.Split(';');
            foreach (string tagName in tagsArray)
            {
                hash_Tags.Add(tagName.Trim());
            }
            //regenerate the tags
            tags = "";
            foreach (string hashTag in hash_Tags)
            {
                //tags = string.Concat(hashTag + ";");
                tags += hashTag + ";";
            }
            tags = tags.Remove(tags.Length - 1, 1);
            return tags;
        }

        #region VSO

        public static string GenerateVsoUrl_FromEpic_ChildIItemsWithTag(int epicID, string milestoneCategory, string project, string team = null)
        {
            var context = Utils.GetVsoContext();
            string wiql = string.Format("SELECT [System.Id],[System.Title] FROM WorkItemLinks WHERE ([Source].[System.Id] = {0}) AND ([System.Links.LinkType] = 'System.LinkTypes.Hierarchy-Forward') AND ([Target].[System.Tags] CONTAINS '{1}') ORDER BY [System.Id] mode(Recursive)", epicID, milestoneCategory);
            var url = context.GenerateCustomQueryUrl(wiql, project, team);
            return url;
        }

        public static bool TryParseCategoryNameToVsoTag(string milestoneCategoryName, out string vsoTag)
        {
            if (milestoneCategoryName != null)
            {
                var category = milestoneCategoryName.ToLower();
                var allowedValues = new Dictionary<string, string>
            {
                {"locready", "Loc_Ready"},
                {"locstart", "Loc_Start"},
                {"progressing", "Loc_Progressing"},
                {"endgame","Loc_EndGame"},
                {"signoff", "Loc_Signoff"},
                {"retro", "Loc_Retro"},
            };
                if (allowedValues.ContainsKey(category))
                {
                    vsoTag = allowedValues[category];
                    return true;
                }
                else
                {
                    vsoTag = null;
                    return false;
                }
            }
            else
                vsoTag = null;
            return false;
        }

        #endregion VSO
    }
}
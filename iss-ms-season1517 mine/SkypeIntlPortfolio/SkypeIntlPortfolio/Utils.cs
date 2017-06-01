using JiraApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio
{
    public static class Utils
    {
        //public static Issue CreateAAAPTJira(string epicLabel)
        public static Issue CreateAAAPTJira(string productName, string epicLabel, string core_intl_folders_location, string core_Source_File_path, bool RW_permissions, string eol, string Expected_Date_For_Walking, string Expected_Date_For_Running)
        {
            // Create the JiraModel object to provide the authentication to access Jira. get all the issues for different projects.
            var jiraRootUrl = "https://jiratest.skype.net";

            JiraApiObjectModel jiraModel = new JiraApiObjectModel("AAAPT", jiraRootUrl);

            //1 - create a pbi
            Issue newIssue = new Issue();

            newIssue = jiraModel.CreateIssue(summary: String.Format("{0} : Request to Onboard", productName),
                                                   description: "Core intl-folders location: " + core_intl_folders_location + Environment.NewLine +
                                                   "Core Source-File path: " + core_Source_File_path + Environment.NewLine +
                                                   "europe,skwttad RW permissions done?  " + RW_permissions + Environment.NewLine +
                                                   "EOL: " + eol + Environment.NewLine +
                                                   "Expected Date for Walking: " + Expected_Date_For_Walking + Environment.NewLine +
                                                   "Expected Date for Running: " + Expected_Date_For_Running,
                                                   priorityName: "P3 - Medium",
                                                   issueTypeName: "Enabling Specification",
                //epicLabel: epicLabel,
                                                   labels: new List<string> { "Fabric_OnboardingRequest" },
                                                   assigneeAlias: "raffael"
                                                   );
            return newIssue;
        }

        public static List<Issue> CreateLYNCFABJira(string productName, string epicLabel)
        {
            var jiraRootUrl = "https://jiratest.skype.net";
            var jiraModel = new JiraApiObjectModel("LYNCFAB", jiraRootUrl);
            //create one parent issue
            List<Issue> issuesList = new List<Issue>();
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
                // epicLabel: epicLabel,
                                                    assigneeAlias: "raffael"
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
                parentIssueKey: parentIssue.key
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
                                         parentIssueKey: parentIssue.key
                                         );

                //add sub pbis into the issues list.
                issuesList.Add(subIssue2);
            }

            return issuesList;
        }
    }
}
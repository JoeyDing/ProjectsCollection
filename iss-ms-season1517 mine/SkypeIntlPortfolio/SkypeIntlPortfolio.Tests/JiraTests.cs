using JiraApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Tests
{
    [TestClass]
    public class JiraTests
    {
        //        [TestMethod]
        //        public void CreateAAAPTJiraTest_GivenEpicLabel()
        //        {
        //            //set up
        //            string epicLabel = "Fabric_SkypeBoxAndroid";
        //            string projectName = "SkypeBoxAndroid";
        //            //execute
        //            Issue createdIssue = SkypeIntlPortfolio.Utils.CreateAAAPTJira(epicLabel);
        //            //Assert
        //            //expected result is a new issue is creted and all the fields are set as expected.
        //            Assert.IsTrue(createdIssue.key != null);
        //            Assert.IsTrue(createdIssue.fields.assignee.name == "raffael" && createdIssue.fields.summary == String.Format("{0} - Request to Onboard", projectName) && createdIssue.fields.issuetype.name == "Enabling Specification" && createdIssue.fields.priority.name == "P3 - Medium" && createdIssue.fields.description.ToString() == " ");
        //        }

        //        [TestMethod]
        //        public void CreateLYNCFABJira_GivenEpicLabel()
        //        {
        //            //set up
        //            string epicLabel = "Fabric_SkypeBoxAndroid";
        //            string projectName = "SkypeBoxAndroid";
        //            //execute
        //            List<Issue> issuesList = SkypeIntlPortfolio.Utils.CreateLYNCFABJira(epicLabel);
        //            //Assert
        //            //expected result is there are 11 pbis in the returned issuesList totally.
        //            //the expected result is all the sub issues are created successfully and each of them has a correct parent key, and all the sub pbis' fields are filled as expected.
        //            string parentIssueKey = issuesList[0].key;
        //            Assert.IsTrue(issuesList.Count == 11);
        //            Assert.IsTrue(issuesList[0].fields.assignee.name == "raffael" && issuesList[0].fields.summary == String.Format("{0} Test\\Production Tenant Fabric Onboarding Testing", "SkypeBoxAndroid", projectName) && issuesList[0].fields.issuetype.name == "Enabling Specification" && issuesList[0].fields.priority.name == "P3 - Medium");
        //            //compares strings excluding whitespace
        //            Assert.IsTrue(issuesList[0].fields.description.ToString().Where(c => !Char.IsWhiteSpace(c)).SequenceEqual(@"Fabric onboarding schedule tracking:
        //                                                                        https://microsoft.sharepoint.com/teams/SkypeIntl/ipe/_layouts/OneNote.aspx?id=%2fteams%2fSkypeIntl%2fipe%2fShared%20Documents%2fLync%20Notebook&wd=target%28Fabric%20Onboarding.one%7c7814C486-B392-4592-BC02-5AD7A57D7B9E%2fFabric%20Onboarding%20planning%7cFC27131C-8787-4466-91FF-5ED5ECA89C6A%2f%29
        //                                                                        onenote:https://microsoft.sharepoint.com/teams/SkypeIntl/ipe/Shared%20Documents/Lync%20Notebook/Fabric%20Onboarding.one#Fabric%20Onboarding%20planning&section-id={7814C486-B392-4592-BC02-5AD7A57D7B9E}&page-id={FC27131C-8787-4466-91FF-5ED5ECA89C6A}&end
        //                                                                        Fabric related IPE onenote section: https://microsoft.sharepoint.com/teams/SkypeIntl/ipe/_layouts/OneNote.aspx?id=%2fteams%2fSkypeIntl%2fipe%2fShared%20Documents%2fLync%20Notebook&wd=target%28Fabric.one%7cCC66EA73-0B6A-48BE-83AF-AB26A348964F%2f%29
        //                                                                        onenote:https://microsoft.sharepoint.com/teams/SkypeIntl/ipe/Shared%20Documents/Lync%20Notebook/Fabric.one#section-id={CC66EA73-0B6A-48BE-83AF-AB26A348964F}
        //                                                                        Documentation and training materials are here
        //                                                                        \\emea\shares\TESTKIT\o15samplefiles\Eng_Excellence\Fabric Queries & Reports
        //                                                                        \\emea\shares\TESTKIT\o15samplefiles\Eng_Excellence\Leaf & Fabric
        //                                                                        \\emea\shares\TESTKIT\o15samplefiles\Eng_Excellence\LocVer & Commenting
        //                                                                        \\emea\shares\TESTKIT\o15samplefiles\Eng_Excellence\Resource Lifecycle".Where(c => !Char.IsWhiteSpace(c))));
        //            //testing for the crawltest1
        //            Assert.IsTrue(issuesList[1].fields.labels[0] == "CrawlTest1");
        //            Assert.IsTrue(issuesList[1].fields.issuetype.name == "Development Task");
        //            //sub pbi's parent key should same as the parent pbi's key.
        //            Assert.IsTrue(issuesList[1].fields.parent.key == issuesList[0].key);
        //            Assert.IsTrue(issuesList[1].fields.assignee.name == "raffael");
        //            Assert.IsTrue(issuesList[1].fields.summary == String.Format("{0} / File sanity , file structure , resource count , encoding , meta data", parentIssueKey));
        //            Assert.IsTrue(issuesList[1].fields.description.ToString().Where(c => !Char.IsWhiteSpace(c)).SequenceEqual(@"Covers various sanity testing of resources imported to Fabric, including
        //                                                                        -File sets in build and Fabric. Is the number of files being exported accurate?
        //                                                                        -Project grouping
        //                                                                        -Encoding
        //                                                                        -Resource numbers and ID diff between build and Fabric.
        //                                                                        -editing and saving via LEAF including changing translation, adding LocComment, Locver)".Where(c => !Char.IsWhiteSpace(c))));

        //            //testing for the crawltest2
        //            Assert.IsTrue(issuesList[2].fields.labels[0] == "CrawlTest2");
        //            Assert.IsTrue(issuesList[2].fields.issuetype.name == "Development Task");
        //            //sub pbi's parent key should same as the parent pbi's key.
        //            Assert.IsTrue(issuesList[2].fields.parent.key == issuesList[0].key);
        //            Assert.IsTrue(issuesList[2].fields.assignee.name == "raffael");
        //            Assert.IsTrue(issuesList[2].fields.summary == String.Format("{0} / File round trip in and out of fabric", parentIssueKey));
        //            Assert.IsTrue(issuesList[2].fields.description.ToString().Where(c => !Char.IsWhiteSpace(c)).SequenceEqual(@"Entire resource flow testing covering,
        //                    · Source file update.
        //                    <From Valdo - If you wish to test the new key adding to source, there are two options:
        //                    1. Make the change in the client repository
        //                    2. We could make a copy of the client repository to eg. https://velosipeed.skype.net/l10n/Fabric-translations/EmoticonsTestSource and configure quickbuild to monitor changes from there.>
        //                    · LCG update
        //                    · LCG import
        //                    · Rename LCG files
        //                    · Delete LCG files
        //                    <Fabric Notification (to: FabricSkypeNotify alias) is sending you about LCG deletion or detection?>
        //
        //                · Download query result in Leaf
        //                    · Change in Loc string based on LCG change
        //                    · Change Loc comment and upload to LEAF
        //                    · Change locver and upload to LEAF
        //                    · Export LCL - Skype LCL files (applyDPK) location: http://orf-tfs1:8080/tfs/Skype
        //                    · ApplyDPK
        //                    <applyDPK test cases>
        //                    Verify Fabric exports changes to changeset/DPKs (TFS check) Note -“pporfsrv” account is used to check in LCL files.
        //                    Export all LCL files (at least 1 from every language, and at least 1 full set of a language) and inspect files
        //                    Review differences between original and exported LCL files
        //                    Things to look for:
        //                    · Are unnecessary comments removed?
        //                    · Are all of the translations still there, or have they been removed? (can be caused by CFG error or TgtCul being different than the Culture listed inside Fabric)
        //                    · Is the number of files being exported accurate?
        //                    · Are the files being exported accurate in terms of EOL for each culture?
        //                    · Verify Check-in files in L10N SVN, run diff with the localized files which have been already released before. (Released files are available at \\skype_drop.corp.microsoft.com\FS_SKYPE_TLL\Skype_Released Files\
        //
        //                · Verify that checked in string are unpended (pending = false)
        //
        //                · Check LCT files are present per build @ \\skype_drop.corp.microsoft.com\FS_SKYPE_TLL\LCT
        //                    · On-demand applyDPK (with a couple of files only -> moved from #6. NOTE - confirm if it is ready to test with TFS)
        //                    · Verify any changes in resources, meta data. Modify various flags (IPE review, Translation an Approval, etc) and verify the changes are recorded in history. (only test a couple of files should be enough -> Moved from #8)".Where(c => !Char.IsWhiteSpace(c))));
        //            //testing for the crawltest3
        //            Assert.IsTrue(issuesList[3].fields.labels[0] == "CrawlTest3");
        //            Assert.IsTrue(issuesList[3].fields.issuetype.name == "Development Task");
        //            //sub pbi's parent key should same as the parent pbi's key.
        //            Assert.IsTrue(issuesList[3].fields.parent.key == issuesList[0].key);
        //            Assert.IsTrue(issuesList[3].fields.assignee.name == "raffael");
        //            Assert.IsTrue(issuesList[3].fields.summary == String.Format("{0} / Language Settings are populated correctly and all language can be updated", parentIssueKey));
        //            Assert.IsTrue(issuesList[3].fields.description.ToString().Where(c => !Char.IsWhiteSpace(c)).SequenceEqual(@"language per component
        //                    ◾Check if you see CFG contains the correct EOL and they are correctly imported to Fabric.
        //                    ◾If the project was shipped before, verify if Fabric shows the same numbers of resources for the shipped languages.".Where(c => !Char.IsWhiteSpace(c))));

        //            //testing for the crawltest4
        //            Assert.IsTrue(issuesList[4].fields.labels[0] == "CrawlTest4");
        //            Assert.IsTrue(issuesList[4].fields.issuetype.name == "Development Task");
        //            //sub pbi's parent key should same as the parent pbi's key.
        //            Assert.IsTrue(issuesList[4].fields.parent.key == issuesList[0].key);
        //            Assert.IsTrue(issuesList[4].fields.assignee.name == "raffael");
        //            Assert.IsTrue(issuesList[4].fields.summary == String.Format("{0} / LocVer Behavior", parentIssueKey));
        //            Assert.IsTrue(issuesList[4].fields.description.ToString().Where(c => !Char.IsWhiteSpace(c)).SequenceEqual(@"For new/updated strings in Skype projects, IPE can test adding rules as needed, until we introduce setting files to Skype project to apply autorules.
        //                    -Verify Resource Locking Workflow (Add Locver and see if it is reflected correctly)
        //                    -For projects using Locver before, ensure the existing Locver rules are present in Fabric and validation is working as expected.".Where(c => !Char.IsWhiteSpace(c))));

        //            //testing for the crawltest5
        //            Assert.IsTrue(issuesList[5].fields.labels[0] == "CrawlTest5");
        //            Assert.IsTrue(issuesList[5].fields.issuetype.name == "Development Task");
        //            //sub pbi's parent key should same as the parent pbi's key.
        //            Assert.IsTrue(issuesList[5].fields.parent.key == issuesList[0].key);
        //            Assert.IsTrue(issuesList[5].fields.assignee.name == "raffael");
        //            Assert.IsTrue(issuesList[5].fields.summary == String.Format("{0} / Check if language mapping correct or not", parentIssueKey));
        //            Assert.IsTrue(issuesList[5].fields.description.ToString().Where(c => !Char.IsWhiteSpace(c)).SequenceEqual(@"Verify if the mapping between LCL files and target files added to the repo are mapped correctly. The language structure would be different based on the platforms and could be so per Skype repro's requirement. ".Where(c => !Char.IsWhiteSpace(c))));

        //            //testing for the crawltest6
        //            Assert.IsTrue(issuesList[6].fields.labels[0] == "CrawlTest6");
        //            Assert.IsTrue(issuesList[6].fields.issuetype.name == "Development Task");
        //            //sub pbi's parent key should same as the parent pbi's key.
        //            Assert.IsTrue(issuesList[6].fields.parent.key == issuesList[0].key);
        //            Assert.IsTrue(issuesList[6].fields.assignee.name == "raffael");
        //            Assert.IsTrue(issuesList[6].fields.summary == String.Format("{0} / queries and filter \"if any\" working correctly", parentIssueKey));
        //            Assert.IsTrue(issuesList[6].fields.description.ToString().Where(c => !Char.IsWhiteSpace(c)).SequenceEqual((@"||Check||Ok?||
        //                    | Query by <Project> + <Resource ID> within 2 languages (Pick 2-3 languages within the project EOL): : | ? |
        //                    | Query by Changed Date within 2 languages: | ? |
        //                    | Query by Changed During within 2 languages: | ? |
        //                    | Query by File Path within a language: | ? |
        //                    | Query by Localization Changed -> String Value within a language: | ? |
        //                    | Query by Localization Changed Date within a language: | ? |
        //                    | Query by Localize within a language: | ? |
        //                    | Query by Locked within a language: | ? |
        //                    | Query by Project within 2 languages: | ? |
        //                    | Query by Resource ID: | ? |
        //                    | Query by Resource ID (Full Path): | ? | " +
        //                    "| Query by Review Flag -> IPE Review (String) -> Status, all strings value = \"New\", it is normal?| ? |" +
        //                    "| Query by Review Flag -> IPE Review (Binary) -> Status, all strings value = \"New\", it is normal?| ? |" +
        //                    "| Query by Review Flag -> Core Review -> Status, all strings value = \"New\", it is normal?| ? |" +
        //                    "| Query by Review Flag -> Test Review -> Status, all strings value = \"New\", it is normal?| ? |" +
        //                    @"| Query by String value within 3 languages: | ? |
        //                    | Query by String Localization Status within a language: | ? |
        //                    | Query by String value - Normalized within 3 languages: | ? |").Where(c => !Char.IsWhiteSpace(c))));

        //            //testing for the crawltest7
        //            Assert.IsTrue(issuesList[7].fields.labels[0] == "CrawlTest7");
        //            Assert.IsTrue(issuesList[7].fields.issuetype.name == "Development Task");
        //            //sub pbi's parent key should same as the parent pbi's key.
        //            Assert.IsTrue(issuesList[7].fields.parent.key == issuesList[0].key);
        //            Assert.IsTrue(issuesList[7].fields.assignee.name == "raffael");
        //            Assert.IsTrue(issuesList[7].fields.summary == String.Format("{0} / Validate PR test , build sanity check", parentIssueKey));
        //            Assert.IsTrue(issuesList[7].fields.description.ToString().Where(c => !Char.IsWhiteSpace(c)).SequenceEqual(@"comments from Yuka:
        //                    It is up to the product owner IPE to decide if their product needs PR testing or can be added as part of regular Loc testing. (as part of test pass, for example).
        //                        Prior to that, iSS eng team to take a diff between the released Localized files and the ones generated via Fabric to see any unexpected diff.".Where(c => !Char.IsWhiteSpace(c))));

        //            //testing for the walktest1
        //            Assert.IsTrue(issuesList[8].fields.labels[0] == "WalkTest1");
        //            Assert.IsTrue(issuesList[8].fields.issuetype.name == "Development Task");
        //            //sub pbi's parent key should same as the parent pbi's key.
        //            Assert.IsTrue(issuesList[8].fields.parent.key == issuesList[0].key);
        //            Assert.IsTrue(issuesList[8].fields.assignee.name == "raffael");
        //            Assert.IsTrue(issuesList[8].fields.summary == String.Format("{0} / <PROD only> E2E Test with LSP", parentIssueKey));
        //            Assert.IsTrue(issuesList[8].fields.description.ToString().Where(c => !Char.IsWhiteSpace(c)).SequenceEqual(("LSP verify they can see what they need to see per project using \"LSP_Default\" filter. (IPE review flag use is expected for Skype projects at this point)" +

        //                @"Ensure before LSP testing, flags are set correctly: e.g.
        //                IPE-Review
        //                Revert changes from 7-Final
        //                IPE-Unblock
        //
        //                Login as LSP (PROD testing only before LSP testing)
        //                    Revert to non-pending in WebUI
        //                    Query in project
        //                    Change TRAP and upload in LEAF
        //                    Change TRAP using Flow Control
        //
        //                Login as Translator (check w/ multiple languages)
        //                    Download query result in Leaf
        //                    Translate strings
        //                        Confirm translations w/o permissions cannot be uploaded
        //                        Confirm tranlsations w/ permissions can be uploaded
        //                    Blocked strings
        //                        Confirm they cannot be viewed
        //                        Confirm unblocked resources can be viewed
        //                    Frozen strings
        //                        Confirm resources can be downloaded to LEAF
        //                        Confirm resources cannot be uploaded from LEAF
        //                    Clanger information
        //                        Confirm clanger added to 1 language is visible in another
        //                    Run LocVer validation against all cultures and investigate LocVer errors
        //                        Confirm LocVer validation passes for those no LocVer errors
        //                        Confirm LocVer error is valid for those with LocVer errors
        //                        Investigate existing LocVer errors
        //
        //                Upload changes from Leaf").Where(c => !Char.IsWhiteSpace(c))));
        //            //testing for the walktest2
        //            Assert.IsTrue(issuesList[9].fields.labels[0] == "WalkTest2");
        //            Assert.IsTrue(issuesList[9].fields.issuetype.name == "Development Task");
        //            //sub pbi's parent key should same as the parent pbi's key.
        //            Assert.IsTrue(issuesList[9].fields.parent.key == issuesList[0].key);
        //            Assert.IsTrue(issuesList[9].fields.assignee.name == "raffael");
        //            Assert.IsTrue(issuesList[9].fields.summary == String.Format("{0} / <PROD only> Flags are set correctly prior toProduction starts.", parentIssueKey));
        //            Assert.IsTrue(issuesList[9].fields.description.ToString().Where(c => !Char.IsWhiteSpace(c)).SequenceEqual(@"Based on Project status, various flags are set correctly to ensure LSPs are only seeing what they need to work on.
        //                <PROD sanity checks>
        //                Freeze resources using Bulk Editor
        //                · Confirm resources can be queried in WebUI
        //                · Confirm translations cannot be changed in WebUI
        //                · Confirm resources cannot be reverted
        //                Unfreeze resources using bulik editor
        //                · Confirm queries can be queried in WebUI
        //                · Confirm translations can be changed in WebUI
        //                Revert translation using WebUI
        //                · Confirm revert is successful and pending = false in WebUI
        //                Block resources using (confirm resources that are not nodes has ReviewFlag set to 0 - Blocked):
        //                · SP
        //                · Web Editor
        //                · Bulk Editor
        //                Unblock resources using (confirm resources that are not nodes have ReviewFlag set to 1 - Unblocked):
        //                · SP
        //                · Web Editor
        //                · Bulk Editor
        //                Export Excel
        //
        //                Login as LSP (PROD testing only before LSP testing)
        //                <Check lists (exactly how each project should look like vary, depending on the project’s readiness so need to be updated when ready to test this on PROD>
        //                · Revert to non-pending in WebUI
        //                · Query in project
        //                · Change TRAP and upload in LEAF
        //                · Change TRAP using Flow Control
        //                · IPE BLOCKED?
        //                · IPE Reviewed?
        //                · Frozen?
        //                · TRAP - set to 7-final? ".Where(c => !Char.IsWhiteSpace(c))));
        //            //testing for the walktest3
        //            Assert.IsTrue(issuesList[10].fields.labels[0] == "WalkTest3");
        //            Assert.IsTrue(issuesList[10].fields.issuetype.name == "Development Task");
        //            //sub pbi's parent key should same as the parent pbi's key.
        //            Assert.IsTrue(issuesList[10].fields.parent.key == issuesList[0].key);
        //            Assert.IsTrue(issuesList[10].fields.assignee.name == "raffael");
        //            Assert.IsTrue(issuesList[10].fields.summary == String.Format("{0} / <PROD only> Request project to be available for dash boarding", parentIssueKey));
        //            Assert.IsTrue(issuesList[10].fields.description.ToString().Where(c => !Char.IsWhiteSpace(c)).SequenceEqual(@"· Request for PROD dashboarding (to Raffaele)
        //                · Verify that the tenant is in Fabric BI (Need to request to Martin O'Flaherty @ Aziz team)
        //                · Verify that the tenant is in Word Count report (same request as Fabric BI)".Where(c => !Char.IsWhiteSpace(c))));
        //        }
    }
}
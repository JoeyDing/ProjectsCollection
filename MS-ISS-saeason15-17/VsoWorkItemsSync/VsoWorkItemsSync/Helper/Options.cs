using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoWorkItemsSync.Helper
{
    public class Options
    {
        [Option('d', "daysfromToday", Required = false, HelpText = "Use this option to sync FromDate")]
        public int SyncDays { get; set; }

        [Option('b', "bugs", Required = false, HelpText = "Use this option to sync Bugs")]
        public bool SyncBugs { get; set; }

        [Option('e', "epics", Required = false, HelpText = "Use this option to sync Epics")]
        public bool SyncEpics { get; set; }

        [Option('l', "links", Required = false, HelpText = "Use this option to sync Workitem Links")]
        public bool SyncLinks { get; set; }

        [Option('s', "enablingSpecifications", Required = false, HelpText = "Use this option to sync Enabling Specifications")]
        public bool SyncEnablingSpecifications { get; set; }

        [Option('i', "impediments", Required = false, HelpText = "Use this option to sync Impediments")]
        public bool SyncImpediments { get; set; }

        [Option('t', "tasks", Required = false, HelpText = "Use this option to sync Tasks")]
        public bool SyncTasks { get; set; }

        [Option('c', "testCases", Required = false, HelpText = "Use this option to sync Test Cases")]
        public bool SyncTestCases { get; set; }

        [Option('p', "testPlans", Required = false, HelpText = "Use this option to sync Test Plans")]
        public bool SyncTestPlans { get; set; }

        [Option('u', "testSuites", Required = false, HelpText = "Use this option to sync Test Suites")]
        public bool SyncTestSuites { get; set; }

        [Option('r', "testRunResults", Required = false, HelpText = "Use this option to sync Test Run Results")]
        public bool SyncTestRunResults { get; set; }

        [Option('o', "coreBugInfos", Required = false, HelpText = "Use this option to sync Core Bug Infos")]
        public bool SyncCoreBugInfos { get; set; }

        [Option('f', "coreBugInfosFromFeedback", Required = false, HelpText = "Use this option to sync Core Bug Infos from feedback")]
        public bool SyncCoreBugFromFeedbackInfos { get; set; }

        [Option('m', "workItemAttachment", Required = false, HelpText = "Use this option to sync WorkItemAttachment Infos")]
        public bool SyncWorkItemAttachmentInfos { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        private string _heading;

        public Options()
        {
            this._heading = "VsoWorkItemsSync";
        }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
            (HelpText current) =>
            {
                current.Heading = this._heading;
                current.Copyright = new CopyrightInfo("Skype International Team", 2015);
                HelpText.DefaultParsingErrorsHandler(this, current);
            });
        }
    }
}
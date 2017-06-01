using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools.ProjectsDocumentation
{
    public class ProjectDocInfo
    {
        public string ProjectName { get; set; }
        public string Repository { get; set; }
        public IEnumerable<ProjectDocNode> Nodes { get; set; }
    }

    public class ProjectDocNode
    {
        public string NodeName { get; set; }
        public string Url { get; set; }
    }
}
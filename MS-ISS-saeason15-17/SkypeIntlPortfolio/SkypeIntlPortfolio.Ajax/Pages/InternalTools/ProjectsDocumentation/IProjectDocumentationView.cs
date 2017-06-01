using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools.ProjectsDocumentation
{
    public interface IProjectDocumentationView
    {
        event Func<Dictionary<string, List<ProjectDocInfo>>> GetAvailableProjectsDocs;

        event Func<string, string, string, string> GetMdFileContent;

        string Url { get; }
    }
}
using Markdig;
using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools.ProjectsDocumentation
{
    public class ProjectsDocumentationPresenter
    {
        private readonly IProjectDocumentationView view;

        public ProjectsDocumentationPresenter(IProjectDocumentationView view)
        {
            this.view = view;
            view.GetAvailableProjectsDocs += View_GetAvailableProjectsDocs;
            view.GetMdFileContent += View_GetMdFileContent;
        }

        private Dictionary<string, List<ProjectDocInfo>> View_GetAvailableProjectsDocs()
        {
            List<IGrouping<string, GitProjectDocumentation>> mdFilesRepositoryGroup = null;
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                mdFilesRepositoryGroup = context.GitProjectDocumentations.ToList()
                    .GroupBy(c => c.Repository_Name).ToList();
            }

            var res = new Dictionary<string, List<ProjectDocInfo>>();
            foreach (var repositoryGroup in mdFilesRepositoryGroup)
            {
                var projectInfos = new List<ProjectDocInfo>();
                foreach (var projectGroup in repositoryGroup.GroupBy(c => c.Project_Name))
                {
                    var projectInfo = new ProjectDocInfo
                    {
                        ProjectName = projectGroup.Key,
                        Repository = repositoryGroup.Key,
                    };

                    var nodes = new List<ProjectDocNode>();
                    foreach (var item in projectGroup)
                    {
                        var split = item.Document_Path.Split(new char[] { '/' });
                        nodes.Add(new ProjectDocNode
                        {
                            NodeName = split[split.Length - 1],
                            Url = new string[] {
                            this.view.Url,
                            string.Format("?repository={0}", projectInfo.Repository),
                            string.Format("&filePath={0}", item.Document_Path),
                            string.Format("&branch={0}", item.Branch)}
                            .Aggregate((a, b) => a + b)
                        });
                    }
                    projectInfo.Nodes = nodes;
                    projectInfos.Add(projectInfo);
                }

                res.Add(repositoryGroup.Key, projectInfos);
            }
            return res;
        }

        private string View_GetMdFileContent(string repository, string filePath, string branch)
        {
            var vsoContext = Utils.GetVsoContext();
            var repoInfo = vsoContext.GetGitRepoByRepoName("LOCALIZATION", repository);
            var repId = (string) repoInfo["id"];

            //1 - dowload md file from vso
            var stream = vsoContext.GetGitRepoFileByRepoIDAndFilePath(repId, filePath, branch);
            stream.Position = 0;
            //using utf8 here otherwise some characters are not displayed correctly (i.e colons ':')
            var sr = new StreamReader(stream, Encoding.UTF8, false);
            string streamStr = sr.ReadToEnd();

            //we use the makdown library (with extensions to handle md tables for example)
            //to convert the md file to html
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var result = Markdown.ToHtml(streamStr, pipeline);

            // 2 - once the md file is converted to html, we need to retrieve all <img> tags
            //     in the html and load the img from vso separately
            result = this.ParseImgTags(vsoContext, repId, result, filePath, branch);

            return result;
        }

        private string MakeImageBase64SrcData(MemoryStream stream)
        {
            return "data:image/jpeg;base64," +
              Convert.ToBase64String(stream.ToArray());
        }

        private string ParseImgTags(VsoContext vsoContext, string repId, string source, string filePath, string branch)
        {
            //this regex will match all <img> tab with two subgroup (src and alt part)
            string pattern = @"<img (src=""[^\""]+"").+(<\/img>|\/>)";
            var buffer = new ConcurrentDictionary<string, string>();
            var matches = Regex.Matches(source, pattern).Cast<Match>();
            Parallel.ForEach<Match>(matches, new ParallelOptions { MaxDegreeOfParallelism = 5 }, (match) =>
             {
                 string newString = match.Value;
                 Group src = null;
                 foreach (Group subGroup in match.Groups)
                 {
                     //we are only interested with src part
                     if (subGroup.Value.StartsWith("src"))
                     {
                         src = subGroup;
                         break;
                     }
                 }
                 try
                 {
                     //we replace the original "src" attribute with base64 image
                     string oldValue = src.Value.Split(new char[] { '"' })[1];
                     var pathSegments = filePath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                     var rootUrl = pathSegments.Take(pathSegments.Length - 1).Aggregate((a, b) => a + "/" + b);
                     string imagePath = string.Format("{0}/{1}", rootUrl, oldValue);
                     MemoryStream imgStream = vsoContext.GetGitRepoFileByRepoIDAndFilePath(repId, imagePath, branch);
                     var base64Img = this.MakeImageBase64SrcData(imgStream);
                     string newValue = string.Format("src=\"{0}\"", base64Img);
                     newString = newString.Replace(src.Value, newValue);
                     buffer.TryAdd(src.Value, newString);
                 }
                 catch (Exception e)
                 {
                     //handle exception silently if image path is wrong
                     buffer.TryAdd(src.Value, src.Value);
                 }
             });

            source = Regex.Replace(source, pattern, (match) =>
            {
                string newString = match.Value;
                Group src = null;
                foreach (Group subGroup in match.Groups)
                {
                    //we are only interested with src part
                    if (subGroup.Value.StartsWith("src"))
                    {
                        src = subGroup;
                        break;
                    }
                }

                if (src != null)
                {
                    newString = buffer[src.Value];
                }

                return newString;
            });
            return source;
        }
    }
}
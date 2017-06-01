using SkypeIntlPortfolio.SyncTool.Core;
using SkypeIntlPortfolio.SyncTool.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.SyncTool.SyncActionProvider.Git
{
    public class SyncGitDocumentationAction : ISyncActionProvider
    {
        private readonly Dictionary<string, string> repositories;

        public string ActionName
        {
            get
            {
                return "Git Documentation Sync";
            }
        }

        public SyncGitDocumentationAction()
        {
            this.repositories = new Dictionary<string, string> {
                {"internal_tools_intltools", "master" },
                {"internal_tools_intldatatools", "master"},
                { "internal_test_loc-automation", "dev"}
            };
        }

        public void Sync()
        {
            // 1.1 Get authentication key from AppConfig and use it to instatiate the VSO rest API Wrapper
            string authenticationKey = ConfigurationManager.AppSettings["VsoPrivateKey"];
            string vsoRootAccount = ConfigurationManager.AppSettings["VsoRootAccount"];

            string projectName = "LOCALIZATION";
            var vsoContext = new VsoContext(vsoRootAccount, authenticationKey);
            foreach (var repository in this.repositories)
            {
                //2.1 Get list of md files from git repository
                var repoInfo = vsoContext.GetGitRepoByRepoName(projectName, repository.Key);
                var repId = (string) repoInfo["id"];
                var files = vsoContext.GetGitRepoFilePathsByFolderPath(repId, @"/", repository.Value, false);
                var mdFiles = files.Where(x => x.EndsWith(".md") && x.Split(new char[] { '/' }).Length > 2).GroupBy(c => c.Split(new char[] { '/' })[1]).ToList();

                //2.2 Update Portfolio\GitProjectDocumentation table
                using (var dbContext = new SkypeIntlPortfolioContext())
                {
                    using (var dbContextTransaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                    {
                        try
                        {
                            //2.2.1 Get data
                            var dbData = dbContext.GitProjectDocumentations.AsNoTracking().Where(c => c.Repository_Name == repository.Key).ToList();

                            var gitData = new List<GitProjectDocumentation>();

                            foreach (var mdFileGroup in mdFiles)
                            {
                                foreach (var mdFile in mdFileGroup)
                                {
                                    var docInfo = new GitProjectDocumentation
                                    {
                                        Project_Name = mdFileGroup.Key,
                                        Repository_Name = repository.Key,
                                        Document_Path = mdFile,
                                        Branch = repository.Value
                                    };

                                    gitData.Add(docInfo);
                                }
                            }
                            //2.2.2 Compare
                            var comparer = new GitDocInfoComparer();

                            var onlyDb = new HashSet<GitProjectDocumentation>(dbData, comparer);
                            onlyDb.ExceptWith(gitData);

                            var onlyGit = new HashSet<GitProjectDocumentation>(gitData, comparer);
                            onlyGit.ExceptWith(dbData);

                            //remove items that no longer exists in git
                            foreach (var item in onlyDb)
                            {
                                dbContext.GitProjectDocumentations.Attach(item);
                                dbContext.GitProjectDocumentations.Remove(item);
                            }

                            //add new item from git
                            foreach (var item in onlyGit)
                            {
                                dbContext.GitProjectDocumentations.Add(item);
                            }

                            dbContext.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
    }
}
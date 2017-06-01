using SkypeIntlPortfolio.SyncTool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.SyncTool.SyncActionProvider.Git
{
    public class GitDocInfoComparer : IEqualityComparer<GitProjectDocumentation>
    {
        public bool Equals(GitProjectDocumentation x, GitProjectDocumentation y)
        {
            if (x != null && y != null)
                return x.Document_Path == y.Document_Path &&
                    x.Project_Name == y.Project_Name &&
                    x.Repository_Name == y.Repository_Name &&
                    x.Branch == y.Branch;
            else return x == y;
        }

        public int GetHashCode(GitProjectDocumentation obj)
        {
            if (obj != null)
            {
                int hash = 17;
                hash = hash * 23 + obj.Document_Path.GetHashCode();
                hash = hash * 23 + obj.Project_Name.GetHashCode();
                hash = hash * 23 + obj.Repository_Name.GetHashCode();
                hash = hash * 23 + obj.Branch.GetHashCode();
                return hash;
            }
            else
            {
                return 0;
            }
        }
    }
}
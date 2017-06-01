using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoApi
{
    public class VsoObjectModel
    {
        #region Properties

        public Uri CollectionUri { get; set; }

        public Uri RootUri { get; set; }

        #endregion Properties

        public VsoObjectModel(string rootSite)
        {
            this.RootUri = new Uri(rootSite);
            this.CollectionUri = new Uri(this.RootUri, "DefaultCollection");
        }

        #region Create-WorkItem

        public WorkItem CreateBug(Dictionary<string, string> fields, string project, string[] attachementsPath = null, Uri collectionUri = null)
        {
            return this.CreateWorkItem("Bug", fields, project, attachementsPath, collectionUri);
        }

        private WorkItem CreateWorkItem(string workItemType, Dictionary<string, string> fields, string project, string[] attachementsPath = null, Uri collectionUri = null)
        {
            var currentUri = collectionUri == null ? this.CollectionUri : collectionUri;

            //get collection
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(currentUri);

            //get work item store for the specified project and type
            WorkItemStore workItemStore = tpc.GetService<WorkItemStore>();
            Project teamProject = workItemStore.Projects[project];
            WorkItemType type = teamProject.WorkItemTypes[workItemType];

            // Create the work item.
            WorkItem bug = new WorkItem(type);
            var bugFields = bug.Fields.Cast<Field>().ToDictionary(c => c.Name, c => c);

            // The title is generally the only required field that doesn’t have a default value.
            // You must set it, or you can’t save the work item. Depending on the
            // type of work item, there may be other fields that you’ll have to set.
            foreach (var item in fields)
            {
                if (bugFields.ContainsKey(item.Key))
                {
                    bug[item.Key] = item.Value;
                }
                else
                {
                    throw new Exception(string.Format("Field \"{0}\" doesn't exist.", item.Key));
                }
            }

            //add attachements
            if (attachementsPath != null)
            {
                foreach (string path in attachementsPath)
                {
                    bug.Attachments.Add(new Attachment(path));
                }
            }

            // Save the new work item.
            bug.Save();
            return bug;
        }

        #endregion Create-WorkItem

        #region Query-Item

        public List<WorkItem> GetWorkItemsByIds(int[] ids, string[] fields = null)
        {
            string query = null;
            if (fields != null && fields.Length > 0)
            {
                query = string.Format("SELECT {0} FROM WorkItems",
                                    fields.Select(c => c.StartsWith("[") ? c : string.Format("[{0}]", c))
                                    .Aggregate((a, b) => a + "," + b)
                    );
            }
            else
                query = "SELECT * FROM WorkItems";
            return this.GetWorkItems((workItemStore) => workItemStore.Query(ids, query));
        }

        public List<WorkItem> GetWorkItemsByWiQL(string wiql)
        {
            return this.GetWorkItems((workItemStore) => workItemStore.Query(wiql));
        }

        public List<WorkItem> GetWorkItemsFromSavedQueries(string project, string queryPath)
        {
            // Run a saved query i.e:"Shared Queries\Current Iteration\Active Bugs".

            string[] queryNodes = queryPath.Split(new char[] { '\\', '/' });
            if (queryNodes.Length > 1)
            {
                var nodeQueue = new Queue<string>(queryNodes.Take(queryNodes.Length - 1));

                return this.GetWorkItems((workItemStore) =>
                {
                    Project _project = workItemStore.Projects[project];
                    QueryHierarchy _queryRoot = _project.QueryHierarchy;
                    QueryFolder _folder = (QueryFolder)_queryRoot[nodeQueue.Dequeue()];

                    while (nodeQueue.Any())
                    {
                        var currenNode = nodeQueue.Dequeue();
                        _folder = (QueryFolder)_folder[currenNode];
                    }

                    //create dictionary of potential keywords
                    var dict = new Dictionary<string, string>
                    {
                        {"project", _project.Name }
                    };

                    QueryDefinition query = (QueryDefinition)_folder[queryNodes[queryNodes.Length - 1]];
                    return workItemStore.Query(query.QueryText, dict);
                });
            }
            throw new Exception("Specified path not valid. (i.e:\"Shared Queries\\Current Iteration\\Active Bugs\")");
        }

        private List<WorkItem> GetWorkItems(Func<WorkItemStore, WorkItemCollection> getItemFunction)
        {
            List<WorkItem> result = null;

            if (getItemFunction != null)
            {
                var currentUri = this.CollectionUri;

                // Connect to the work item store
                TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(currentUri);
                WorkItemStore workItemStore = (WorkItemStore)tpc.GetService(typeof(WorkItemStore));

                // Run a query.
                WorkItemCollection queryResults = getItemFunction(workItemStore);
                result = queryResults.Cast<WorkItem>().ToList();
            }

            return result;
        }

        #endregion Query-Item
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoApi.TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string collectionUriTest = "https://skype-test2.visualstudio.com";
            string projectTest = "SKYPEINTL";
            var vsoApi = new VsoObjectModel(collectionUriTest.ToString());

            Query(vsoApi, projectTest);

            //Create(vsoApi, projectTest);
            Console.ReadKey();
        }

        private static void Query(VsoObjectModel vsoApi, string project)
        {
            //saved query
            var res1 = vsoApi.GetWorkItemsFromSavedQueries(project, @"Shared Queries\Current Iteration\Active Bugs");

            //list of ids
            var res2 = vsoApi.GetWorkItemsByIds(new int[] { 67875 }, new string[] { "Title" });

            //custom query
            string query = "Select [State], [Title] " +
                           "From WorkItems " +
                           "Where [Work Item Type] = 'Bug' " +
                           "And [Area Path] = 'SKYPEINTL' " +
                           "Order By [State] Asc, [Changed Date] Desc";
            var res3 = vsoApi.GetWorkItemsByWiQL(query);
        }

        private static void Create(VsoObjectModel vsoApi, string project)
        {
            //create bug
            var bug = vsoApi.CreateBug(new Dictionary<string, string> {
                { "Title", string.Format("Bug created by API ({0})", DateTime.Now.ToString()) },
                { "Description", string.Format("Bug created by API ({0})", DateTime.Now.ToString()) }
            }, project);
        }
    }
}
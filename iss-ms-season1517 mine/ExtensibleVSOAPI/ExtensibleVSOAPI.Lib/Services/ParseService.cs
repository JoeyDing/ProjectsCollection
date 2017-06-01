using ExtensibleDataExtraction.Lib.Data.Configuration;
using ExtensibleDataExtraction.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VsoApi.Rest;

namespace ExtensibleVSOAPI.Services
{
    /// <summary>
    /// String -> Object parse service
    /// </summary>
    public class ParseService : IParse
    {
        public ExtensibleItem _extensibleItem;

        public ParseService(ExtensibleItem extensibleItem)
        {
            this._extensibleItem = extensibleItem;
        }

        /// <summary>
        /// Parse string types to expected object types such as Type and FromDate
        /// </summary>
        /// <returns>Returns expected object types</returns>
        public object Parse()
        {
            FetchParamsInfo fetchParamsInfo = new FetchParamsInfo();
            //Conver xml data to Object
            List<Param> fetchParams = _extensibleItem.JsonEndPoint.CustomLib.Params.ParamsCollection;

            int[] keysArray = new int[2];

            if (fetchParams.Count != 2)
                throw new Exception("xml file must include Type and FromDate as necessary fields");
            else
            {
                foreach (Param item in fetchParams)
                {
                    if (item.Key.ToLower() == "type")
                    {
                        if (keysArray[0] == 1)
                        {
                            throw new Exception("FetchParam must include only one Type key");
                        }
                        else
                        {
                            fetchParamsInfo.TaskType = ReturnTaskType(item.Value);
                            keysArray[0] = 1;
                        }
                    }
                    if (item.Key.ToLower() == "fromdate")
                    {
                        if (keysArray[1] == 1)
                        {
                            throw new Exception("FetchParam must include only one FromDate key");
                        }
                        else if (!String.IsNullOrEmpty(item.Value))
                        {
                            string daysAgo = Regex.Match(item.Value, @"\d+").Value;
                            fetchParamsInfo.DateTime = DateTime.Now.AddDays(Int32.Parse(daysAgo) * -1);
                            keysArray[1] = 1;
                        }
                    }
                }
            }

            return fetchParamsInfo;
        }

        public TaskType ReturnTaskType(string taskType)
        {
            switch (taskType.ToLower())
            {
                case "task":
                    return TaskTypes.Task;

                case "bug":
                    return TaskTypes.Bug;

                case "enabling specification":
                    return TaskTypes.EnablingSpecification;

                case "epic":
                    return TaskTypes.Epic;

                case "test case":
                    return TaskTypes.TestCase;

                case "test plan":
                    return TaskTypes.TestPlan;

                case "Test Suite":
                    return TaskTypes.TestSuite;

                default:
                    throw new Exception("Value of Type is incorrect");
            }
        }
    }
}
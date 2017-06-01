using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.RemoteLogger
{
    [Serializable]
    public class DropDownModel
    {
        public string DataValueField { get; set; }
        public string DataTextField { get; set; }
    }

    public interface IRemoteLoggerView
    {
        List<RemoteLoggerStateOverviewModel> StateOverviewList { get; set; }
        List<RemoteLoggerStateModel> StateList { get; set; }
        List<DropDownModel> BatchTimeList { get; set; }
        List<DropDownModel> UserList { get; set; }
        List<RemoteLoggerStateModel> StateOverviewDetailList { get; set; }
        List<string> ApplicationList { get; set; }

        event Func<string,List<DropDownModel>> GetUserList;
        event Func<string,string,List<DropDownModel>> GetBatchTimeList;
        event Func<string, List<RemoteLoggerStateModel>> GetStateList;
        event Func<string, List<RemoteLoggerStateOverviewModel>> GetStateOverviewList;
        event Func<string,string, List<RemoteLoggerStateModel>> GetStateOverviewDetailList;
        event Func<List<string>> GetApplicationList;
        event Func<int,List<RemoteStateLoggerImage>> GetImageList;
    }
}

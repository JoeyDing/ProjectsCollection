using SkypeIntlPortfolio.Data;
using SkypeIntlPortfolio.Silverlight.Data;
using SkypeIntlPortfolio.Silverlight.Data.Aggregates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace SkypeIntlPortfolio.Silverlight.Views
{
    public class FabricOnboardingsViewModel : ViewModelBase
    {
        private ObservableCollection<FabricOnboardingsLowLevelStatus> fabricOnboardingsLowLevelStatusList;
        private ObservableCollection<FabricOnboardingsHighLevelStatus> fabricOnboardingsHighLevelStatusList;
        private ObservableCollection<FabricOnboardingsTotalProjectByPhase> fabricOnboardingsTotalProjectByStatusList;
        private bool isBusy;
        private string error;

        #region Properties

        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                OnPropertyChanged("Error");
            }
        }

        public ObservableCollection<FabricOnboardingsTotalProjectByPhase> FabricOnboardingsTotalProjectByStatusList
        {
            get { return fabricOnboardingsTotalProjectByStatusList; }
            set { fabricOnboardingsTotalProjectByStatusList = value; }
        }

        public ObservableCollection<FabricOnboardingsLowLevelStatus> FabricOnboardingsLowLevelStatusList
        {
            get { return fabricOnboardingsLowLevelStatusList; }
            set { fabricOnboardingsLowLevelStatusList = value; }
        }

        public ObservableCollection<FabricOnboardingsHighLevelStatus> FabricOnboardingsHighLevelStatusList
        {
            get { return fabricOnboardingsHighLevelStatusList; }
            set { fabricOnboardingsHighLevelStatusList = value; }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        #endregion Properties

        public FabricOnboardingsViewModel()
        {
            this.FabricOnboardingsLowLevelStatusList = new ObservableCollection<FabricOnboardingsLowLevelStatus>();
            this.FabricOnboardingsTotalProjectByStatusList = new ObservableCollection<FabricOnboardingsTotalProjectByPhase>();
            this.FabricOnboardingsHighLevelStatusList = new ObservableCollection<FabricOnboardingsHighLevelStatus>();
            this.LoadFabricOnboardingData();
        }

        private void LoadFabricOnboardingData()
        {
            try
            {
                this.IsBusy = true;
                //get high level data
                var highLeveldata = Utils.GetData<IEnumerable<FabricOnboardingsHighLevelStatus>>(string.Format("{0}/{1}", Utils.WebApiRootPath, "fabriconboardingshighlevelstatus"));
                highLeveldata.ContinueWith
                    ((ant) =>
                    {
                        try
                        {
                            foreach (var item in ant.Result)
                            {
                                this.FabricOnboardingsHighLevelStatusList.Add(item);
                            }
                            foreach (var item in ant.Result.GroupBy(c => c.Phase).Select(c => (new FabricOnboardingsTotalProjectByPhase { Phase = c.Key, Total = c.Count() })))
                            {
                                this.FabricOnboardingsTotalProjectByStatusList.Add(item);
                            }

                            //get low level data
                            var lowLeveldata = Utils.GetData<IEnumerable<FabricOnboardingsLowLevelStatus>>(string.Format("{0}/{1}", Utils.WebApiRootPath, "fabriconboardingslowlevelstatus"));
                            lowLeveldata.ContinueWith
                                ((ant1) =>
                                {
                                    try
                                    {
                                        foreach (var item in ant1.Result)
                                        {
                                            this.FabricOnboardingsLowLevelStatusList.Add(item);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw;
                                    }
                                    finally
                                    {
                                        this.IsBusy = false;
                                    }
                                }
                                , TaskScheduler.FromCurrentSynchronizationContext());
                        }
                        catch (Exception e)
                        {
                            this.IsBusy = false;
                            this.Error = e.ToString();
                            throw;
                        }
                    }
                    , TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception e)
            {
                this.IsBusy = false;
                this.Error = e.ToString();
                throw;
            }
        }
    }
}
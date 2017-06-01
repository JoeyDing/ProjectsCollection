using Microsoft.Localization.Framework.DownloadCenter.ViewModel;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEAF.Plugins.InvestigatedWpfPlugin
{
    internal class ViewModel : ViewModelBase
    {
        public DelegateCommand MyProperty { get; set; }
    }
}
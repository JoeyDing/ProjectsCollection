using Microsoft.Localization.Framework.OM;
using Microsoft.Localization.Framework;
using Microsoft.Localization.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Resources;
using System.Windows.Controls;
using LEAF.Plugins.InvestigatedWpfPlugin.Properties;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LEAF.Plugins.InvestigatedWpfPlugin
{
    /// <summary>
    /// Interaction logic for InvestigatedLeafWpfPlugin.xaml
    /// </summary>
    public partial class InvestigatedLeafWpfPlugin : UserControl, IViewer
    {
        public InvestigatedLeafWpfPlugin()
        {
            InitializeComponent();
        }

        public bool CanCopy
        {
            get
            {
                return false;
            }
        }

        public bool CanLookUp
        {
            get
            {
                return false;
            }
        }

        public string PluginAuthor
        {
            get
            {
                return "PluginAuthor";
            }
        }

        public string PluginDescription
        {
            get
            {
                return "PluginAuthor";
            }
        }

        public string PluginDisplayName
        {
            get
            {
                return "PluginAuthor";
            }
        }

        public System.Drawing.Image PluginImage
        {
            get
            {
                return InvestigatedWpfPlugin.Properties.Resources.mushroom;
            }
        }

        public string PluginName
        {
            get
            {
                return "PluginAuthor";
            }
        }

        public object Settings
        {
            get
            {
                return new object();
            }

            set
            {
            }
        }

        public void OnActivate(IPluginContext context)
        {
        }

        public void OnCleanUp()
        {
        }

        public object OnCopy()
        {
            return new object();
        }

        public void OnExecute(IPluginContext context)
        {
        }

        public void OnLookUp(IPluginContext context, object obj)
        {
        }

        public void OnSettings(string xmlConfig)
        {
        }

        public void OnShutDown()
        {
        }

        public void OnStartUp()
        {
        }

        public void OnSync()
        {
        }

        public void OnUpdateContext(IPluginContext context)
        {
        }
    }
}
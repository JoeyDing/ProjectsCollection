using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Localization.Framework.OM;

namespace LEAF.Plugins.InvestigatedWpfPlugin
{
    public partial class TestingWinform : UserControl, IViewer
    {
        public TestingWinform()
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
                return "Winform Plugin";
            }
        }

        public Image PluginImage
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
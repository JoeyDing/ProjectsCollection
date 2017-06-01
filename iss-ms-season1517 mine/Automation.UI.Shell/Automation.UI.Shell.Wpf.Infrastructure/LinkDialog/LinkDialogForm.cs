using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automation.UI.Shell.Wpf.Infrastructure.LinkDialog
{
    public partial class LinkDialogForm : Form
    {
        public LinkDialogForm()
        {
            InitializeComponent();
        }

        public void BindContent(string errorMsg)
        {
            lblErrorMsg.Text = errorMsg;
        }
    }
}
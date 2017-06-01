using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory
{
    public class GridViewStatusGroupView : IExpandStatusGroupView
    {
        private readonly RadGridView gridView;

        public GridViewStatusGroupView(IContainGridView gridViewContainer)
        {
            this.gridView = gridViewContainer.GridView;
        }

        public bool ExpandGroup(string groupName)
        {
            var groups = this.gridView.Items.Groups;
            var groupToExpand = groups.Cast<IGroup>().FirstOrDefault(c => (string)c.Key == groupName);
            if (groupToExpand != null)
            {
                //expand the group
                this.gridView.ExpandGroup(groupToExpand);

                //scroll to last item in the group
                this.gridView.ScrollIntoView(groupToExpand.Items.Cast<object>().Last());

                return true;
            }

            return false;
        }
    }
}
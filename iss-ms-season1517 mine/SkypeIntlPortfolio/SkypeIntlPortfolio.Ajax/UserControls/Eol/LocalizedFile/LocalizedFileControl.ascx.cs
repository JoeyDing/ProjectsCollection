using SkypeIntlPortfolio.Ajax;
using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.Model.Mock;
using SkypeIntlPortfolio.Ajax.UserControls.Eol;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public partial class LocalizedFileControl : UserControl, ILocalizedFileBridge
    {
        public event Func<int> onGetTotalRecord;

        public event Func<int, int, List<ResourceFile>> GetResourceFileOfProduct;

        public event Func<int, int, int, List<ResourceFiles_Target_Base>> GetTargetFileByResourceFileKey;

        public event Func<int, int> onGetTotalRecordForTargetFile;

        protected void RadGrid_LocalizedFile_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (this.GetResourceFileOfProduct != null && this.onGetTotalRecord != null)
            {
                int pageIndex = RadGrid_LocalizedFile.MasterTableView.CurrentPageIndex;
                int pageSize = RadGrid_LocalizedFile.MasterTableView.PageSize;
                this.RadGrid_LocalizedFile.VirtualItemCount = this.onGetTotalRecord();
                this.RadGrid_LocalizedFile.DataSource = this.GetResourceFileOfProduct(pageIndex, pageSize);
            }
        }

        protected void RadGrid_LocalizedFile_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            int pageIndex = e.DetailTableView.CurrentPageIndex;
            int pageSize = e.DetailTableView.PageSize;
            var data = e.DetailTableView.ParentItem;
            int fileKey = int.Parse(data.GetDataKeyValue("FileKey").ToString());
            e.DetailTableView.VirtualItemCount = this.onGetTotalRecordForTargetFile(fileKey);
            e.DetailTableView.DataSource = this.GetTargetFileByResourceFileKey(fileKey, pageIndex, pageSize);
        }

        protected void RadGrid_LocalizedFile_ItemDataBound(object sender, GridItemEventArgs e)
        {
        }

        protected void RadGrid_LocalizedFile_ItemCommand(object sender, GridCommandEventArgs e)
        {
        }
    }
}
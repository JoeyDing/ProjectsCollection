<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientTools.aspx.cs" MasterPageFile="~/MasterPage.Master" Inherits="SkypeIntlPortfolio.Ajax.Pages.Monitor.ClientTools" %>


<asp:Content ID="Content2" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <style>
        a.HyperLinkHover {
            background-image: url("../../Images/1448014031_stock_data-link.png");
            display: block;
            width: 16px;
            height: 16px;
            cursor: pointer;
            text-indent: -9999px;
        }

        .multiColumn ul {
            width: 100%;
        }

        .rlbTemplate span:nth-of-type(1) {
            width: 200px;
            display: inline-block;
        }

        .rlbHeader span:nth-of-type(1) {
            width: 200px;
            display: inline-block;
        }
    </style>
    <script type="text/javascript">
        function RowClick(index) {
  <%--     var gridJobsLog = $find("<%= this.gridview_JobsLog.ClientID %>"); 
        gridJobsLog.get_masterTableView().get_dataItems()[index].set_selected("true");--%>
        }
    </script>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
    </telerik:RadAjaxManager>

    <div class="layout-control">
        <div class="panel panel-danger">
            <div class="panel-heading">
                <h3 class="panel-title">Client Tools</h3>
            </div>
            <asp:ListView ID="listview_tools" runat="server">
                <ItemTemplate>
                    <div style="padding: 10px; color: #0094ff; font-size: 14px; font-weight: bold"><%# Eval("GroupName") %></div>
                    <div>
                        <asp:ListView ID="lstTools" runat="server" DataSource='<%# Eval("Items") %>'>
                            <ItemTemplate>
                                <div id="row" runat="server" style="border-bottom: 1px solid #d5d5d5; padding-left: 20px;margin-top:5px">
                                    <table style="width: 50%">
                                        <tr>
                                            <td>
                                                <div style="font-size: 14px; font-weight: bold">Version: <%# Eval("Version") %> </div>
                                                <%# Eval("Description") %>
                                                <table style="color: #808080; font-size: 10px; width: 100%;margin-bottom:5px">
                                                    <tr>
                                                        <td style="width:400px">Last Updated: <%# Eval("last_updated_datetime") %></td>
                                                        <td> <%# Eval("downloads") %> downloads</td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:HyperLink ID="HyperLink1" runat="server" Text="Link" NavigateUrl='<%# String.Format("~/Pages/InternalTools/Downloader.ashx?path={0}", Eval("folder_zip").ToString()) %>' Target="_blank" CssClass="HyperLinkHover"></asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</asp:Content>

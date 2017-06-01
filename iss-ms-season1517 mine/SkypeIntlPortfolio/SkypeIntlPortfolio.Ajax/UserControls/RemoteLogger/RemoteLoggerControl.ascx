<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RemoteLoggerControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.RemoteLogger.RemoteLoggerControl" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style type="text/css">
    .RadGrid_Metro .rgAltRow {
        background: transparent;
    }

    .RadGrid_Metro td.rgGroupCol, .RadGrid_Metro td.rgExpandCol {
        background: transparent;
    }

    .RadGrid_Metro .rgRow > td, .RadGrid_Metro .rgAltRow > td, .RadGrid_Metro .rgEditRow > td {
        border-style: none;
    }

    .RadGrid_Metro .rgHeader, .RadGrid_Metro th.rgResizeCol, .RadGrid_Metro .rgHeaderWrapper {
        border: none;
        border-bottom: none;
        border-left: none;
    }

    .RadGrid_Metro {
        border: none;
    }

        .RadGrid_Metro .rgCommandCell {
            border-bottom: none;
        }

    .detailTableIndent {
        margin-left: 100px !important;
    }

    .RadGrid .rgHoveredRow {
        background-color: lightblue !important;
    }

    .RadGrid .item-style td {
        max-height: 600px !important;
    }

    .output-cell-style {
        display: block;
        overflow: auto;
    }
</style>

<h3 style="color: #333333; font-size: 16px; font-weight: normal; margin: 0 0 15px;">
    <asp:Label runat="server" ID="lbTitle" />
</h3>
<telerik:RadAjaxManager runat="server" ID="radManager1">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="RadMultiPageInfo">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadMultiPageInfo" LoadingPanelID="JobCurrentStatusNewPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<telerik:RadSplitter ID="radsplitter_main" runat="server" Width="100%" Height="750" VisibleDuringInit="false">
    <telerik:RadPane ID="radpane_products" Width="15%" runat="server" Height="750" BackColor="#25a0da">
        <asp:Panel runat="server" ID="panel_jobCurrentStatusNew_panel">
            <telerik:RadTabStrip runat="server" AutoPostBack="true" ID="RemoteLoggerSideBar" MultiPageID="RadMultiPageInfo" SelectedIndex="0" OnTabClick="RadTabStripJobStatusNew_TabClick" CausesValidation="false" Orientation="VerticalLeft" Width="100%">
                <Tabs>
                    <telerik:RadTab Text="Skype Desktop Automation" Height="30px"></telerik:RadTab>
                    <telerik:RadTab Text="SWX Automation" Height="30px"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
        </asp:Panel>
    </telerik:RadPane>
    <telerik:RadSplitBar ID="radsplitbar" runat="server" CollapseMode="Forward">
    </telerik:RadSplitBar>
    <telerik:RadPane ID="radpane1" Width="85%" runat="server" Height="750">
        <table>
            <tr>
                <td>
                    <telerik:RadDropDownList RenderMode="Lightweight"
                        ID="rddUserAccount"
                        runat="server"
                        Width="400px"
                        DropDownWidth="400px"
                        DataValueField="DataValueField"
                        DataTextField="DataTextField"
                        OnSelectedIndexChanged="rddUserAccount_SelectedIndexChanged"
                        AutoPostBack="true">
                    </telerik:RadDropDownList>
                </td>
                <td>
                    <telerik:RadDropDownList RenderMode="Lightweight"
                        ID="rddBatchTimeRange"
                        runat="server"
                        Width="400px"
                        DropDownWidth="400px"
                        DataValueField="DataValueField"
                        DataTextField="DataTextField"
                        OnSelectedIndexChanged="rddBatchTimeRange_SelectedIndexChanged"
                        AutoPostBack="true">
                    </telerik:RadDropDownList>
                </td>
            </tr>
        </table>
        <div style="margin-top: 22px; font-size: 14px">
            <asp:Label runat="server" Font-Italic="true">Testcase Automation Overview</asp:Label>
        </div>
        <div style="width: 90%; margin-left: 20px; margin-top: 20px;">
            <telerik:RadGrid
                RenderMode="Auto"
                ID="RadGridTestcaseOverview"
                AllowPaging="false"
                runat="server"
                EnableGroupsExpandAll="true"
                AutoGenerateColumns="false"
                BorderWidth="0"
                EnableEmbeddedSkins="true"
                EnableViewState="true"
                ViewStateMode="Enabled"
                CommandItemDisplay="None"
                OnItemDataBound="RadGridTestcaseOverview_ItemDataBound"
                OnDetailTableDataBind="RadGridTestcaseOverview_DetailTableDataBind">
                <HeaderStyle Font-Bold="true" />
                <PagerStyle AlwaysVisible="true" />
                <MasterTableView DataKeyNames="Testcase">
                    <Columns>

                        <telerik:GridBoundColumn DataField="Testcase" HeaderText="Testcase Name" HeaderStyle-Width="250" />
                        <telerik:GridBoundColumn DataField="PassRate" HeaderText="TestPass" HeaderStyle-Width="80" />
                    </Columns>
                    <DetailTables>
                        <telerik:GridTableView
                            runat="server"
                            AutoGenerateColumns="false"
                            EnableGroupsExpandAll="true"
                            ViewStateMode="Enabled"
                            Name="TestcaseOverviewDetail"
                            CellSpacing="0"
                            BorderWidth="0"
                            DataKeyNames="Id">
                            <Columns>
                                <telerik:GridBoundColumn DataField="LanguageName" HeaderText="Language" HeaderStyle-Width="80" />
                                <telerik:GridBoundColumn DataField="ScreenShot" HeaderText="ScreenShot" HeaderStyle-Width="250">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="State" HeaderStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:Image ID="StateID" runat="server" ImageUrl='<%# SkypeIntlPortfolio.Ajax.UserControls.RemoteLogger.RemoteLoggerControl.ConvertBooleanToImage(Eval("State").ToString()) %>'></asp:Image>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Exception" HeaderStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLinkDetail" runat="server" Text="Exception" ForeColor="Blue" NavigateUrl='<%# SkypeIntlPortfolio.Ajax.UserControls.RemoteLogger.RemoteLoggerControl.FormatExceptionLink(Eval("ExceptionID")) %>' Target="_blank" CssClass="HyperLinkHover" Visible='<%# SkypeIntlPortfolio.Ajax.UserControls.RemoteLogger.RemoteLoggerControl.ConvertBooleanToVisible(Eval("ExceptionID")) %>'>
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="UserIdentity" HeaderText="User Identity" HeaderStyle-Width="150" />
                                <telerik:GridBoundColumn DataField="UpdateDate" HeaderText="UpdateDate" HeaderStyle-Width="200" />
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>
                    <HeaderStyle Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <div style="margin-top: 22px; font-size: 14px">
            <asp:Label runat="server" Font-Italic="true">Testcase Automation Detail</asp:Label>
        </div>
        <div style="width: 90%; margin-left: 20px; margin-top: 20px;">
            <telerik:RadGrid
                RenderMode="Auto"
                ID="RadGridTestCases"
                AllowPaging="True"
                runat="server"
                EnableGroupsExpandAll="true"
                AutoGenerateColumns="false"
                BorderWidth="0"
                EnableEmbeddedSkins="true"
                EnableViewState="true"
                ViewStateMode="Enabled"
                OnItemDataBound="RadGridTestCases_ItemDataBound"
                CommandItemDisplay="None">
                <HeaderStyle Font-Bold="true" />
                <PagerStyle AlwaysVisible="true" />
                <MasterTableView DataKeyNames="Id" PageSize="20">
                    <Columns>

                        <telerik:GridBoundColumn DataField="TestcaseName" HeaderText="Testcase Name" HeaderStyle-Width="250" />
                        <telerik:GridBoundColumn DataField="LanguageName" HeaderText="Language" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn DataField="ScreenShot" HeaderText="ScreenShot" HeaderStyle-Width="250">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="State" HeaderStyle-Width="50">
                            <ItemTemplate>
                                <asp:Image ID="StateID" runat="server" ImageUrl='<%# SkypeIntlPortfolio.Ajax.UserControls.RemoteLogger.RemoteLoggerControl.ConvertBooleanToImage(Eval("State").ToString()) %>'></asp:Image>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Exception" HeaderStyle-Width="100">
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLinkDetail" runat="server" Text="Exception" ForeColor="Blue" NavigateUrl='<%# SkypeIntlPortfolio.Ajax.UserControls.RemoteLogger.RemoteLoggerControl.FormatExceptionLink(Eval("ExceptionID")) %>' Target="_blank" CssClass="HyperLinkHover" Visible='<%# SkypeIntlPortfolio.Ajax.UserControls.RemoteLogger.RemoteLoggerControl.ConvertBooleanToVisible(Eval("ExceptionID")) %>'>
                                </asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="UserIdentity" HeaderText="User Identity" HeaderStyle-Width="150" />
                        <telerik:GridBoundColumn DataField="UpdateDate" HeaderText="UpdateDate" HeaderStyle-Width="200" />
                    </Columns>
                    <HeaderStyle Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </telerik:RadPane>
</telerik:RadSplitter>
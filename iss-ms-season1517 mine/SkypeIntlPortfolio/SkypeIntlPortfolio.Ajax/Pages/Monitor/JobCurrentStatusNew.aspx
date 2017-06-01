<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="JobCurrentStatusNew.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.Monitor.JobCurrentStatusNew" %>

<%@ Register Src="~/UserControls/ToolRelationships/ToolRelationshipsControl.ascx" TagName="ToolRelationshipsControl" TagPrefix="local" %>
<%@ Register Src="~/UserControls/JobStatus/JobStatusControl.ascx" TagName="JobstatusControl" TagPrefix="local" %>
<%@ Register Src="~/UserControls/JobHistory/JobHistoryControl.ascx" TagName="JobHistoryControl" TagPrefix="local" %>
<%@ Register Src="~/UserControls/RunningJobs/RunningJobsControl.ascx" TagName="RunningJobsControl" TagPrefix="local" %>


<asp:Content runat="server" ID="content1" ContentPlaceHolderID="head">
    <style type="text/css">
        .RadSplitter_Metro {
            margin-top: -20px;
        }

        .RadTabStrip_Metro .rtsLevel1 .rtsLink {
            color: #000000;
            text-align: center;
        }

        .rtsSelected, .rtsSelected {
            background: #fff;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <telerik:RadAjaxManager runat="server" ID="radManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadMultiPageInfo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPageInfo" LoadingPanelID="JobCurrentStatusNewPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="JobCurrentStatusNewPanel">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadSplitter ID="radsplitter_main" runat="server" Width="100%" Height="750" VisibleDuringInit="false">
        <telerik:RadPane ID="radpane_products" Width="15%" runat="server" Height="750" BackColor="#25a0da">
            <asp:Panel runat="server" Style="margin-top: 80px;" ID="panel_jobCurrentStatusNew_panel">
                <telerik:RadTabStrip runat="server" AutoPostBack="true" ID="RadTabStripJobStatusNew" MultiPageID="RadMultiPageInfo" SelectedIndex="0" OnTabClick="RadTabStripJobStatusNew_TabClick" CausesValidation="false" Orientation="VerticalLeft" Width="100%">
                    <Tabs>
                        <telerik:RadTab Text="Live Status" Height="30px"></telerik:RadTab>
                        <telerik:RadTab Text="Job Status" Height="30px"></telerik:RadTab>
                        <telerik:RadTab Text="Job History" Height="30px"></telerik:RadTab>
                        <telerik:RadTab Text="Tool Relationships" Height="30px"></telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </asp:Panel>
        </telerik:RadPane>
        <telerik:RadSplitBar ID="radsplitbar" runat="server" CollapseMode="Forward">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="radpane1" Width="85%" runat="server" Height="750">
            <asp:Panel runat="server" ID="Panel_PageViews">
                <telerik:RadMultiPage runat="server" ID="RadMultiPageInfo" SelectedIndex="0" Width="100%" Height="100%" Style="margin-bottom: 200px;" RenderSelectedPageOnly="true">
                    <telerik:RadPageView runat="server" ID="radPageView4">
                        <local:RunningJobsControl runat="server" ID="custom_runningJobsControl" />
                    </telerik:RadPageView>
                    <telerik:RadPageView runat="server" ID="radPageView1">
                        <local:JobstatusControl runat="server" ID="custom_jobStatusControl"></local:JobstatusControl>
                    </telerik:RadPageView>
                    <telerik:RadPageView runat="server" ID="radPageView2">
                        <local:JobHistoryControl runat="server" ID="custom_jobHistoryControl"></local:JobHistoryControl>
                    </telerik:RadPageView>
                    <telerik:RadPageView runat="server" ID="radPageView3">
                        <local:ToolRelationshipsControl runat="server" ID="custom_toolRelationshipsControl" />
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </asp:Panel>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
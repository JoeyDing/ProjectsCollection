<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobDetails.aspx.cs"  MasterPageFile="~/MasterPage.Master"   Inherits="SkypeIntlPortfolio.Ajax.Pages.Monitor.JobDetails" %>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyPlaceHolder" runat="server">


    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="radCombo_jobs">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="radlistbox_sjobteps" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="gridview_JobsIterations" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
                 <telerik:AjaxUpdatedControl ControlID="radchart_stepsDuration" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
       <telerik:AjaxSetting AjaxControlID="radlistbox_sjobteps">
            <UpdatedControls>
                 <telerik:AjaxUpdatedControl ControlID="radchart_stepsDuration" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelRenderMode="Inline"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
</telerik:RadAjaxLoadingPanel>
<script type="text/javascript">

    function OnClientSelectedIndexChanging(sender, args) {

        args.set_cancel(true);

    }
</script>
<div class="layout-control">
    <div class="panel panel-info">
        <div class="panel-heading">
            <h3 class="panel-title">Job Details</h3>
        </div>
        <div class="panel-body">
            <div class="container-fluid" id="container-row">
                <div class="row">
                    <div class="col-md-12">
                        <asp:Label Text="Job:" runat="server" />
                        <telerik:RadComboBox runat="server" ID="radCombo_jobs"
                            AutoPostBack="true"
                            DataTextField="JobName"
                            DataValueField="SqlJobID"
                            OnSelectedIndexChanged="radCombo_jobs_SelectedIndexChanged"
                            >
                        </telerik:RadComboBox>
                        <asp:Label Text="Job steps:" runat="server" />
                        <telerik:RadListBox ID="radlistbox_sjobteps" runat="server" CheckBoxes="true" AutoPostBack="true" ShowCheckAll="true"
                            DataTextField="JobStepName" DataValueField="JobStepID" SelectionMode="Multiple" OnItemCheck="radlistbox_sjobteps_ItemCheck">
                        </telerik:RadListBox>
                    </div>
                </div>
               <%-- <div class="row">
                    <div class="col-md-12">
                        <telerik:RadButton Text="Update" runat="server" ID="radButton_update" OnClick="radButton_update_Click" AutoPostBack="true" />
                    </div>
                </div>--%>
                <div class="row">
                    <div class="col-md-12">
                        <telerik:RadHtmlChart runat="server" ID="radchart_stepsDuration" Width="100%" Height="500" Transitions="true">
                            <PlotArea>
                            </PlotArea>

                            <ChartTitle Text="Job Steps Iteration Duration (Last 24 hours)">
                                <Appearance Align="Left" Position="Top"></Appearance>
                            </ChartTitle>
                            <Legend>
                                <Appearance Position="Bottom"></Appearance>
                            </Legend>
                        </telerik:RadHtmlChart>

                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <br />
                        <br />
                        <h3 class="info-heading panel-title">Job Last Executions
                        </h3>
                        <br />
                        <telerik:RadGrid ID="gridview_JobsIterations" runat="server"
                            HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" Font-Bold="true" HeaderStyle-Font-Bold="true"
                            OnItemDataBound="gridview_JobsIterations_ItemDataBound"
                            OnNeedDataSource="gridview_JobsIterationsNeedDataSource"
                            OnDetailTableDataBind="gridview_JobsIterations_DetailTableDataBind"
                            AutoGenerateColumns="false" ClientSettings-EnableAlternatingItems="false"
                            AllowSorting="True" ShowGroupPanel="True">
                            <ClientSettings AllowColumnsReorder="True" AllowExpandCollapse="true"></ClientSettings>

                            <MasterTableView DataKeyNames="SqlJobStepIterationID">
                                <%--<DetailTables>
                                    <telerik:GridTableView Name="Steps" HierarchyLoadMode="Conditional">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="SqlJobStepIterationID" HeaderText="IterationID" Visible="false"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="StepName" HeaderText="Step Name" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="RunDateTime" HeaderText="Executed On" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Outcome_Message" HeaderText="Error Message" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                        </Columns>
                                    </telerik:GridTableView>
                                </DetailTables>--%>
                               
                                <Columns>
                                    <telerik:GridBoundColumn DataField="SqlJobStepIterationID" HeaderText="IterationID" Visible="false"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="JobName" HeaderText="Job Name" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="StepName" HeaderText="Step Name" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="RunDateTime" HeaderText="Executed On" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Duration" HeaderText="Duration" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Outcome_Message" HeaderText="Error Message" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>

                            <ClientSettings ReorderColumnsOnClient="True" AllowDragToGroup="True" AllowColumnsReorder="True">
                                <Selecting AllowRowSelect="True"></Selecting>
                                <Resizing AllowRowResize="True" AllowColumnResize="True" EnableRealTimeResize="True"
                                    ResizeGridOnColumnResize="False"></Resizing>
                            </ClientSettings>

                        </telerik:RadGrid>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
 </asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="JobCurrentStatus.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.Monitor.JobCurrentStatus" %>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gridview_Jobs">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridview_Jobs" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gridview_JobsFailed" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>

    <div class="layout-control">

        <div class="panel panel-info">
            <div class="panel-heading">
                <h3 class="panel-title">Current Job Status</h3>
            </div>
            <div class="panel-body">
                <telerik:RadGrid ID="gridview_Jobs" runat="server" HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" Font-Bold="true" HeaderStyle-Font-Bold="true"
                    AutoGenerateColumns="false" OnItemDataBound="gridview_Jobs_ItemDataBound"
                    OnNeedDataSource="gridview_Jobs_NeedDataSource" EnableViewState="true" GroupPanel-EnableViewState="true"
                    ClientSettings-EnableAlternatingItems="false"
                    AllowSorting="True" ShowGroupPanel="True">
                    <MasterTableView>
                        <Columns>
                            <telerik:GridBoundColumn DataField="Name" HeaderText="Job Name" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Last_Run_DateTime" HeaderText="Last Execution" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
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

        <div class="panel panel-info">
            <div class="panel-heading">
                <h3 class="panel-title">Last Executions Overview</h3>
            </div>
            <div class="panel-body container-fluid">
                <div class="row">
                    <div class="col-md-4">
                        <telerik:RadHtmlChart runat="server" ID="PieChart1" Height="500" Transitions="true" Skin="Metro">
                            <ChartTitle Text="Total Job Executions (last 24hours)">

                                <Appearance Align="Center" Position="Top">
                                </Appearance>
                            </ChartTitle>
                            <Legend>
                                <Appearance Position="Right" Visible="true">
                                </Appearance>
                            </Legend>
                            <PlotArea>
                                <Series>
                                    <telerik:PieSeries DataFieldY="Total" NameField="Status" ExplodeField="IsExploded">
                                        <LabelsAppearance DataFormatString="{0}">
                                        </LabelsAppearance>
                                        <TooltipsAppearance Color="White" DataFormatString="{0}"></TooltipsAppearance>
                                    </telerik:PieSeries>
                                </Series>
                                <YAxis>
                                </YAxis>
                            </PlotArea>
                            <Legend>
                                <Appearance Position="Right">
                                </Appearance>
                            </Legend>
                        </telerik:RadHtmlChart>
                    </div>
                    <div class="col-md-8">
                        <telerik:RadHtmlChart runat="server" ID="RadHtmlChart2" Height="500" Transitions="true">
                            <ChartTitle Text="Job Average Execution Time (last 24hours)">

                                <Appearance Align="Center" Position="Top">
                                </Appearance>
                            </ChartTitle>
                            <Legend>
                                <Appearance Position="Right" Visible="true">
                                </Appearance>
                            </Legend>
                            <PlotArea>
                                <Series>
                                    <telerik:BarSeries DataFieldY="AverageTime" Name="Time (In Minutes)">
                                        <TooltipsAppearance Visible="false"></TooltipsAppearance>
                                    </telerik:BarSeries>
                                    <%-- <telerik:ColumnSeries DataFieldY="UnitsOnOrder" Name="Units On Order">
                                <TooltipsAppearance Visible="false"></TooltipsAppearance>
                            </telerik:ColumnSeries>--%>
                                </Series>
                                <XAxis DataLabelsField="JobName">
                                    <LabelsAppearance></LabelsAppearance>
                                    <MajorGridLines Visible="false"></MajorGridLines>
                                    <MinorGridLines Visible="false"></MinorGridLines>
                                </XAxis>

                                <YAxis>
                                    <TitleAppearance Text="Time"></TitleAppearance>
                                    <MinorGridLines Visible="false"></MinorGridLines>
                                </YAxis>
                            </PlotArea>
                            <Legend>
                                <Appearance Position="Right">
                                </Appearance>
                            </Legend>
                        </telerik:RadHtmlChart>
                    </div>
                </div>
            </div>
        </div>

        <div class="panel panel-danger">
            <div class="panel-heading">
                <h3 class="panel-title">Last Job failed list</h3>
            </div>
            <div class="panel-body">
                <telerik:RadGrid ID="gridview_JobsFailed" runat="server"
                    HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" Font-Bold="true" HeaderStyle-Font-Bold="true"
                    OnItemDataBound="gridview_JobsFailed_ItemDataBound"
                    OnNeedDataSource="gridview_JobsFailed_NeedDataSource"
                    OnDetailTableDataBind="gridview_JobsFailed_DetailTableDataBind"
                    AutoGenerateColumns="false" ClientSettings-EnableAlternatingItems="false"
                    AllowSorting="True" ShowGroupPanel="True">
                    <ClientSettings AllowColumnsReorder="True" AllowExpandCollapse="true"></ClientSettings>

                    <MasterTableView DataKeyNames="SqlJobStepIterationID">
                        <DetailTables>
                            <telerik:GridTableView Name="Steps" HierarchyLoadMode="Conditional">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="SqlJobStepIterationID" HeaderText="IterationID" Visible="false"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="StepName" HeaderText="Step Name" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="RunDateTime" HeaderText="Executed On" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Outcome_Message" HeaderText="Error Message" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50%"></telerik:GridBoundColumn>
                                </Columns>
                            </telerik:GridTableView>
                        </DetailTables>
                        <Columns>
                            <telerik:GridBoundColumn DataField="JobName" HeaderText="Job Name" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StepName" HeaderText="Step Name" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="RunDateTime" HeaderText="Executed On" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
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
</asp:Content>
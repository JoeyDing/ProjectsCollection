<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobStatusControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.JobStatus.JobStatusControl" %>
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
<div class="container-fluid">
    <asp:Panel runat="server" Style="position: relative;" Height="680px" Width="100%">
        <div class="row">
            <div class="col-md-6" style="margin-top: 22px;">
                <asp:Label runat="server" Font-Size="Large" Font-Bold="true" ID="TextOfTitle" Text="SQL Job Status In 24 Hours "> </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2" style="margin-top: 22px; margin-left: 20px;">
                <telerik:RadButton RenderMode="Lightweight" ID="RadButton_refresh" runat="server" Text="Refresh" Width="90px" OnClick="RadButton_refresh_Click"></telerik:RadButton>
            </div>
            <div class="col-md-3" style="margin-top: 22px; margin-left: -100px;">
                <asp:Label runat="server" Font-Italic="true">Click column header to achieve data sorting</asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2" style="margin-top: 22px; margin-left: 30px;">
                <asp:Label runat="server">Show:</asp:Label>
            </div>
            <div class="col-md-5" style="margin-top: 20px; margin-left: -160px;">
                <telerik:RadComboBox RenderMode="Lightweight" ID="RadComboBoxStatusFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadComboBoxStatusFilter_SelectedIndexChanged" Width="180px">
                    <Items>
                        <telerik:RadComboBoxItem Text="All jobs"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem ImageUrl="~\Images\Green.jpg" Text="Job succeed in all excution" runat="server"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem ImageUrl="~\Images\Red.jpg" Text="Job failed in last excuation" runat="server"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem ImageUrl="~\Images\Yellow.jpg" Text="Job failed at least once"></telerik:RadComboBoxItem>
                    </Items>
                </telerik:RadComboBox>
            </div>
            <div class="col-md-2" style="margin-top: 22px;">
                <asp:Label runat="server">Selected Period:</asp:Label>
            </div>
            <div class="col-md-3" style="margin-top: 20px; margin-left: -100px;">
                <telerik:RadComboBox RenderMode="Lightweight" ID="RadComboBoxPeriodFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadComboBoxPeriodFilter_SelectedIndexChanged">
                    <Items>
                        <telerik:RadComboBoxItem Text="Last 24 hours" />
                        <telerik:RadComboBoxItem Text="Last 48 hours" />
                        <telerik:RadComboBoxItem Text="Last one week" />
                    </Items>
                </telerik:RadComboBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12" style="width: 100%; margin-top: 20px;">

                <telerik:RadGrid
                    RenderMode="Auto"
                    AllowPaging="True"
                    runat="server"
                    ID="RadGrid_JobStatus"
                    OnNeedDataSource="RadGrid_JobStatus_NeedDataSource"
                    OnDetailTableDataBind="RadGrid_JobStatus_DetailTableDataBind"
                    OnItemDataBound="RadGrid_JobStatus_ItemDataBound"
                    EnableEmbeddedSkins="true"
                    AllowSorting="true"
                    OnSortCommand="RadGrid_JobStatus_SortCommand">
                    <MasterTableView
                        runat="server"
                        AutoGenerateColumns="false"
                        EnableGroupsExpandAll="true"
                        ViewStateMode="Enabled"
                        Name="ToolDetails"
                        ShowHeader="true"
                        DataKeyNames="JobName"
                        PageSize="20"
                        Caption="">
                        <Columns>
                            <telerik:GridTemplateColumn UniqueName="Image" HeaderStyle-Width="10px">
                                <HeaderStyle BackColor="LightGray" />
                                <ItemTemplate>
                                    <telerik:RadBinaryImage ID="RadBinaryImage1" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="JobStatus" HeaderText="Last run status" Display="false">
                                <HeaderStyle Width="80px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Center" Width="5px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="JobName" HeaderText="Job Name" SortExpression="JobName" ShowSortIcon="true">
                                <HeaderStyle Width="150px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="JobLatestRunTime" HeaderText="Last run start time" SortExpression="JobLatestRunTime" ShowSortIcon="true">
                                <HeaderStyle Width="200px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="JobLatestDurationRunTimeSpan" HeaderText="Last run Duration time">
                                <HeaderStyle Width="200px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="JobLatestRunFinishTime" HeaderText="Last run finish time" SortExpression="JobLatestRunFinishTime" ShowSortIcon="true">
                                <HeaderStyle Width="400px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                        </Columns>
                        <%--Job run record Table--%>
                        <DetailTables>
                            <telerik:GridTableView
                                runat="server"
                                AutoGenerateColumns="false"
                                EnableGroupsExpandAll="true"
                                ViewStateMode="Enabled"
                                Name="JobRunRecord"
                                CellSpacing="0"
                                BorderWidth="0"
                                DataKeyNames="JobInstanceID">
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="JobRecordImage">
                                        <HeaderStyle BackColor="LightGray" />
                                        <ItemTemplate>
                                            <telerik:RadBinaryImage ID="RadBinaryJobRecordImage1" runat="server" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="JobRunStatus" HeaderText="Job Run Status" Display="false">
                                        <HeaderStyle Width="150px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="JobRunDateTime" HeaderText="Job Run Time">
                                        <HeaderStyle Width="100px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="JobDurationRunTimeSpan" HeaderText="Duration Time">
                                        <HeaderStyle Width="100px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="JobOutMessage" HeaderText="Out message for Job">
                                        <HeaderStyle Width="1000px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Center" BackColor="LightGray" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <%--Records of Steps run in job Table--%>
                                <DetailTables>
                                    <telerik:GridTableView
                                        runat="server"
                                        AutoGenerateColumns="false"
                                        EnableGroupsExpandAll="true"
                                        ViewStateMode="Enabled"
                                        Name="StepRecordInJobRecord"
                                        CellSpacing="0"
                                        BorderWidth="0"
                                        DataKeyNames="">
                                        <ItemStyle CssClass="item-style" />
                                        <AlternatingItemStyle CssClass="item-style" />
                                        <Columns>

                                            <telerik:GridBoundColumn DataField="Blank" HeaderText=" " Display="true">
                                                <HeaderStyle Font-Underline="false" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn UniqueName="JobStepImage">
                                                <HeaderStyle BackColor="LightGray" />
                                                <ItemTemplate>
                                                    <telerik:RadBinaryImage ID="RadBinaryJobStepImage1" runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="StepRunStatus" HeaderText="S" Display="false">
                                                <HeaderStyle Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="JobStepName" HeaderText="Job Step Name">
                                                <HeaderStyle Width="130px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Right" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="JobStepID" HeaderText="Order">
                                                <HeaderStyle Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="RunDurationTimespan" HeaderText="Duration Time">
                                                <HeaderStyle Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Failed_Outcome_Message" HeaderText="Outcome Message">
                                                <HeaderStyle Width="1400px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Center" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Left" CssClass="output-cell-style" />
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </telerik:GridTableView>
                                </DetailTables>
                            </telerik:GridTableView>
                        </DetailTables>
                    </MasterTableView>
                    <ClientSettings>
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </div>
    </asp:Panel>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RunningJobsControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.RunningJobs.RunningJobsControl" %>
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
        .RadGrid .item-style td
    {
        max-height: 600px !important;

    }

         .output-cell-style
         {
             display:block;
             overflow:auto;
         }
</style>
<div class="container-fluid">
    <asp:Panel runat="server" Style="position: relative;" Height="680px" Width="100%">
        <div class="row">
            <div class="col-md-6" style="margin-top: 22px;">
                <asp:Label runat="server" Font-Size="Large" Font-Bold ="true" Text ="Last Run Records In The Server"> </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6" style="margin-top: 20px; margin-left: 20px;">
                <telerik:RadButton RenderMode="Lightweight" ID="FreshButton" runat="server" Text="Refresh" Width="90px" OnClick="Refresh_Click"></telerik:RadButton> 
            </div>
            <div class="col-md-2" style="margin-top: 20px; margin-left: 120px;" >
                 <asp:Label runat="server" Text ="Server Time: "> </asp:Label>
            </div>
            <div class="col-md-2" style="margin-top: 20px; margin-left: -120px;">
                 <asp:Label runat="server" Id="GettingServerTime"> </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6" style="margin-top: 22px;">
                <asp:Label runat="server" Font-Size="Medium" Text ="SQL Job Currently Running In The Server"> </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12" style="width: 90%; margin-left: 20px;margin-top: 20px;">

                <telerik:RadGrid
                    RenderMode="Auto"
                    AllowPaging="True"
                    runat="server"
                    ID="RadGrid_RunningJobs"
                    OnNeedDataSource="RadGrid_RunningJobs_NeedDataSource"
                    OnDetailTableDataBind="RadGrid_LiveStatus_DetailTableDataBind"
                    EnableEmbeddedSkins="true">
                    <MasterTableView
                        runat="server"
                        AutoGenerateColumns="false"
                        EnableGroupsExpandAll="true"
                        ViewStateMode="Enabled"
                        Name="RunningJobs"
                        ShowHeader="true"
                        DataKeyNames="JobName"
                        PageSize="20"
                        Caption=""
                        >
                        <Columns>
                            <telerik:GridBoundColumn DataField="JobName" HeaderText="Job Name">
                                <HeaderStyle Width="150px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CurrentStepName" HeaderText="Current Step Name">
                                <HeaderStyle Width="200px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CurrentStepId" HeaderText="Current Step Id">
                                <HeaderStyle Width="150px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="JobStartTime" HeaderText="Job Start Time">
                                <HeaderStyle Width="150px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="JobRunningTimeSpan" HeaderText="Job Running Time (HH:MM:SS)">
                                <HeaderStyle Width="300px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                        </Columns>
                        <DetailTables>
                                    <telerik:GridTableView
                                        runat="server"
                                        AutoGenerateColumns="false"
                                        EnableGroupsExpandAll="true"
                                        ViewStateMode="Enabled"
                                        Name="FinishedStepsInRunningJob"
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
                                            <telerik:GridBoundColumn DataField="JobStepName" HeaderText="Job Step Name">
                                                <HeaderStyle Width="160px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="JobStepID" HeaderText="Step ID">
                                                <HeaderStyle Width="130px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Center" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="StartTime" HeaderText="Start Time">
                                                <HeaderStyle Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="RunDurationTimespan" HeaderText="Duration Time">
                                                <HeaderStyle Width="500px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </telerik:GridTableView>
                                </DetailTables>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
                <div class="row">
            <div class="col-md-6" style="margin-top: 22px;">
                <asp:Label runat="server" Font-Size="Medium" ID ="TextOfTitle" Text ="Last Run SQL Jobs In The Last 10 Minutes"> </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2" style="margin-top: 22px;">
                <asp:Label runat="server">Selected Period:</asp:Label>
            </div>
            <div class="col-md-3" style="margin-top: 20px; margin-left: -100px;">
                <telerik:RadComboBox RenderMode="Lightweight" ID="RadComboBoxPeriodFilter1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadComboBoxPeriodFilter_SelectedIndexChanged">
                    <Items>
                        <telerik:RadComboBoxItem Text="Last 10 Minutes" />
                        <telerik:RadComboBoxItem Text="Last 20 Minutes" />
                        <telerik:RadComboBoxItem Text="Last Half Hour" />
                        <telerik:RadComboBoxItem Text="Last 1 Hour" />
                        <telerik:RadComboBoxItem Text="Last 2 Hours" />
                    </Items>
                </telerik:RadComboBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12" style="width: 90%; margin-left: 20px;margin-top: 20px;">

                <telerik:RadGrid
                    RenderMode="Auto"
                    AllowPaging="True"
                    runat="server"
                    ID="RadGrid_RunedStepInPeriod"
                    OnNeedDataSource="RadGrid_RunedStepInPeriod_NeedDataSource"
                    OnItemDataBound="RadGrid_RunRecordsInPeroid_ItemDataBound"
                    EnableEmbeddedSkins="true">
                    <MasterTableView
                        runat="server"
                        AutoGenerateColumns="false"
                        EnableGroupsExpandAll="true"
                        ViewStateMode="Enabled"
                        Name="RunedStepInPeriod"
                        ShowHeader="true"
                        PageSize="30"
                        Caption=""
                        >
                        <Columns>
                           <telerik:GridTemplateColumn UniqueName="Image" HeaderStyle-Width="10px">
                                <HeaderStyle BackColor="LightGray" />
                                <ItemTemplate>
                                    <telerik:RadBinaryImage ID="RadBinaryImage1" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="StatusText" HeaderText="status">
                                <HeaderStyle Width="80px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Center" Width="5px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StepRunStatus" HeaderText="statusInt" Display="false">
                                <HeaderStyle Width="80px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Center" Width="5px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="RunEndTime" HeaderText="End Time">
                                <HeaderStyle Width="200px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="JobName" HeaderText="Job Name">
                                <HeaderStyle Width="150px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="JobStepName" HeaderText="Step Name">
                                <HeaderStyle Width="200px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="JobStepID" HeaderText="Step Id">
                                <HeaderStyle Width="50px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="RunDateTime" HeaderText="Start Time">
                                <HeaderStyle Width="150px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="RunDurationTimeSecondsSpan" HeaderText="DurationTime(HH:MM:SS)">
                                <HeaderStyle Width="200px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </asp:Panel>
</div>
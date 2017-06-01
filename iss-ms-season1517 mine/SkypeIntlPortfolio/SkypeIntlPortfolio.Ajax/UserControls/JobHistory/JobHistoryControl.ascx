<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobHistoryControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.JobHistory.JobHistoryControl" %>
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
                <asp:Label runat="server" Font-Size="Large" Font-Bold ="true" Text ="SQL Job Excution History"> </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-1" style="margin-top: 22px;">
                <asp:Label runat="server">Start Date</asp:Label>
            </div>
            <div class="col-md-2" style="margin-top: 20px; margin-left: -40px;">
                <telerik:RadDatePicker RenderMode="Lightweight" ID="RadDatePickerStartDate" CssClass="toDate" Width="50%" runat="server">
                </telerik:RadDatePicker>
            </div>
            <div class="col-md-1" style="margin-top: 22px;">
                <asp:Label runat="server">End Date</asp:Label>
            </div>
            <div class="col-md-2" style="margin-top: 20px; margin-left: -45px;">
                <telerik:RadDatePicker RenderMode="Lightweight" ID="RadDatePickerEndDate" CssClass="toDate" Width="50%" runat="server">
                </telerik:RadDatePicker>
            </div>
            <div class="col-md-2" style="margin-top: 22px;">
                <telerik:RadButton RenderMode="Lightweight" ID="DateFilter" runat="server" Text="Search" Width="90px" OnClick="DateFilter_Click"></telerik:RadButton>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6" style="width: 60%" margin-left: 40px;">
                <asp:CompareValidator ID="dateCompareValidatorForReleaseWindow" runat="server"
                    ControlToValidate="RadDatePickerEndDate" ControlToCompare="RadDatePickerStartDate"
                    Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                    ErrorMessage="End Date must be equal or greater than Start Date - please correct it." Display="Dynamic">
                </asp:CompareValidator>
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
                    EnableEmbeddedSkins="true">
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
                            <telerik:GridBoundColumn DataField="JobName" HeaderText="Job Name">
                                <HeaderStyle Width="150px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
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
                                    <telerik:GridBoundColumn DataField="JobRunDateTime" HeaderText="Job Run Date Time">
                                        <HeaderStyle Width="100px"  Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="JobDurationRunTimeSpan" HeaderText="Duration Time">
                                        <HeaderStyle Width="100px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Center" BackColor="LightGray" />
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
                                        DataKeyNames=""
                                        >
                                        <ItemStyle CssClass="item-style" />
                                        <AlternatingItemStyle CssClass="item-style" />
                                        <Columns>
                                             <%--the Blank column is only to use to --%>
                                            <telerik:GridBoundColumn DataField="Blank" HeaderText=" "  Display="true">
                                                <HeaderStyle Font-Underline="false" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn UniqueName="JobStepImage">
                                                <HeaderStyle BackColor="LightGray" Width="50px" />
                                                <ItemTemplate>
                                                    <telerik:RadBinaryImage ID="RadBinaryJobStepImage1" runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="StepRunStatus" HeaderText="S"  Display="false">
                                                <HeaderStyle Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="JobStepName" HeaderText="Job Step Name">
                                                <HeaderStyle Width="10px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Right" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="JobStepID" HeaderText="Step">
                                                <HeaderStyle  Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="RunDurationTimeSpan" HeaderText="Duration Time">
                                                <HeaderStyle  Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Failed_Outcome_Message" HeaderText="Outcome Message">
                                                <HeaderStyle Width="1400px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Center" BackColor="LightGray" />
                                                <ItemStyle HorizontalAlign="Left"  CssClass="output-cell-style" />
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </telerik:GridTableView>
                                </DetailTables>
                            </telerik:GridTableView>
                        </DetailTables>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </div>
    </asp:Panel>
</div>
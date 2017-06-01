<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ToolRelationshipsControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ToolRelationships.ToolRelationshipsControl" %>
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
</style>

<div class="container-fluid">
    <asp:Panel runat="server" Style="position: relative;" Height="500px" Width="100%">
        <div class="row">
            <div class="col-md-6" style="margin-top: 22px;">
                <asp:Label runat="server" Font-Size="Large" Font-Bold="true" Text="Tool Relationships"> </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-1" style="margin-top: 22px;">
                <asp:Label runat="server">Search Type: </asp:Label>
            </div>
            <div class="col-md-1" style="margin-top: 17px; margin-left: -5px;">
                <asp:CheckBox ID="SearchInTool" Checked="true" runat ="server" Text ="In Tool" AutoPostBack="False" />
            </div>
            <div class="col-md-1" style="margin-top: 17px; margin-left: -10px;">
                <asp:CheckBox ID="SearchInJob" Checked="true" runat="server" Text ="In Job" AutoPostBack="False" />
            </div>
            <div class="col-md-2" style="margin-top: 17px; margin-left: -15px;">
                <asp:CheckBox ID="SearchInStep" Checked="true" runat="server" Text ="In Step" AutoPostBack="False" />
            </div>
            <div class="col-md-1" style="margin-top: 20px; margin-left: -45px;">
                <asp:TextBox ID="SearchContentText" runat="server" TextMode="SingleLine"></asp:TextBox>
            </div>
            <div class="col-md-1" style="margin-top: 22px; margin-left: 100px;"">
                <telerik:RadButton RenderMode="Lightweight" ID="DateFilter" runat="server" Text="Search" Width="90px" OnClick="Search_Click"></telerik:RadButton>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12" style="width: 100%; margin-top: 20px;">
                <telerik:RadGrid
                    RenderMode="Auto"
                    runat="server"
                    ID="RadGrid_ToolRelationships"
                    OnNeedDataSource="RadGrid_ToolRelationships_NeedDataSource"
                    OnDetailTableDataBind="RadGrid_ToolRelationships_DetailTableDataBind"
                    EnableEmbeddedSkins="true"
                    CellSpacing="0"
                    BorderWidth="0"
                    AllowPaging="True"
                    AllowSorting="true">
                    <MasterTableView
                        runat="server"
                        AutoGenerateColumns="false"
                        EnableGroupsExpandAll="true"
                        ViewStateMode="Enabled"
                        Name="ToolDetails"
                        DataKeyNames="Tool_Name"
                        ShowHeader="true"
                        PageSize="20"
                        Caption="">
                        <Columns>
                            <telerik:GridBoundColumn DataField="Tool_Name" HeaderText="Tools">
                                <HeaderStyle Width="200px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                        </Columns>
                        <%--Job Table--%>
                        <DetailTables>
                            <telerik:GridTableView
                                runat="server"
                                AutoGenerateColumns="false"
                                EnableGroupsExpandAll="true"
                                ViewStateMode="Enabled"
                                Name="JobDetails"
                                DataKeyNames="Job_Name"
                                BorderWidth="0"
                                ShowHeader="true"
                                Caption="">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Job_Name" HeaderText="Jobs">
                                        <HeaderStyle Width="200px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <%--Step Table--%>
                                <DetailTables>
                                    <telerik:GridTableView
                                        runat="server"
                                        AutoGenerateColumns="false"
                                        EnableGroupsExpandAll="true"
                                        ViewStateMode="Enabled"
                                        Name="StepDetails"
                                        DataKeyNames="Step_Name"
                                        BorderWidth="0"
                                        ShowHeader="true"
                                        Caption=""
                                        CssClass="detailTableIndent">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="Step_Name" HeaderText="Steps">
                                                <HeaderStyle Width="200px" Font-Underline="true" Font-Bold="true" Font-Italic="true" Font-Size="Medium" HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </telerik:GridTableView>
                                </DetailTables>
                            </telerik:GridTableView>
                        </DetailTables>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </asp:Panel>
</div>

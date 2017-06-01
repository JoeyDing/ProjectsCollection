<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ProjectBuildInfo.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.InternalTools.ProjectBuildInfo" %>

<asp:Content runat="server" ID="content1" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content runat="server" ID="content2" ContentPlaceHolderID="bodyPlaceHolder">
    <asp:Panel runat="server" ID="Panel_ProjectBuildInfo">
        <div class="panel panel-primary">
            <div class="panel-heading">Project build information</div>
            <div class="panel-body">
                <telerik:RadPanelBar runat="server" ID="radpanelbar_projectBuildInfoRoot" Width="100%">
                    <ItemTemplate>
                        <telerik:RadPanelBar runat="server" ID="radpanelbar_projectBuildInfoChild" Width="100%">
                            <Items>
                                <telerik:RadPanelItem Expanded="true">
                                    <HeaderTemplate>
                                        <a class="rpExpandable" style="float: left">
                                            <span class="rpExpandHandle"></span>
                                        </a>
                                        <asp:Label runat="server" ID="label_projectName"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <telerik:RadGrid runat="server" ID="radGrid_projectBuild" OnItemDataBound="radGrid_projectBuild_ItemDataBound">
                                            <MasterTableView AutoGenerateColumns="false">
                                                <Columns>
                                                    <telerik:GridBoundColumn HeaderText="Branch ID" DataField="Branch ID" />
                                                    <telerik:GridBoundColumn HeaderText="Project ID" DataField="Project ID" />
                                                    <telerik:GridBoundColumn HeaderText="Build Version" DataField="Build Version" />
                                                    <telerik:GridBoundColumn HeaderText="Sync Date" DataField="Sync Date" />
                                                    <telerik:GridBoundColumn HeaderText="Branch Identity" DataField="Branch Identity" />
                                                    <telerik:GridHyperLinkColumn UniqueName="PDLink" HeaderText="PD Link" DataTextField="PD Link" Target="_blank" ItemStyle-Font-Underline="true" ItemStyle-Font-Italic="true"></telerik:GridHyperLinkColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </ContentTemplate>
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelBar>
                    </ItemTemplate>
                </telerik:RadPanelBar>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
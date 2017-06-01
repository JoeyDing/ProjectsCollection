<%@ Page ValidateRequest="false" EnableViewState="true" Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ProjectsDocumentationView.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.InternalTools.ProjectsDocumentation.ProjectsDocumentationView" %>

<asp:Content runat="server" ID="content1" ContentPlaceHolderID="head">
    <link href="../../../Content/prism-okaida.css" rel="stylesheet" />
    <link href="../../../Content/theme.css" rel="stylesheet" />
    <script type="text/javascript" src="../../../Scripts/prism.js"></script>
    <style type="text/css">
        .projectsList div.rpTemplate {
            padding: 0px;
        }

        .projectsList .RadPanelBar .rpRootGroup {
            padding: 0px;
            border-bottom-style: none;
        }

        .navbar {
            margin-bottom: 0px !important;
        }

        .headerStyle {
            background-color: #25a0da;
            color: white;
            height: 30px;
            display: block;
            text-align: left;
            padding: 5px;
        }

        /*Left list style**************************************************************/
        .repeaterTemplate ul {
            list-style: none;
            margin: 1em 0;
            padding: 0;
        }

        .repeaterTemplate li {
            font-weight: bold;
            margin: 0;
            padding: 3px 10px 5px 20px;
            /*border-bottom: 1px solid #ccc;
            border-top: 1px solid #ccc;*/
            color: #666;
        }

            .repeaterTemplate li:hover {
                color: #000;
                background-color: #ddd;
            }
    </style>
</asp:Content>

<asp:Content runat="server" ID="content2" ContentPlaceHolderID="bodyPlaceHolder">
    <div class="headerStyle">
        <em>Tools Team's Projects Documentation</em>
    </div>
    <telerik:RadSplitter ID="radsplitter_main" runat="server" Width="100%" Height="750" VisibleDuringInit="false">
        <telerik:RadPane ID="radpane_left" Width="300" runat="server" Height="750" BorderColor="Transparent">
            <div style="padding: 0px; width: 100%" class="projectsList">
                <asp:Panel runat="server" ID="Panel_projectsDocInfo" BorderColor="Transparent">
                    <telerik:RadPanelBar runat="server" ID="radpanelbar_projectsDocInfoRoot" Width="100%" BorderColor="Transparent">
                        <ItemTemplate>
                            <telerik:RadPanelBar runat="server" ID="radpanelbar_projectsDocInfoChild" Width="100%" BorderColor="Transparent">
                                <Items>
                                    <telerik:RadPanelItem Expanded="true" Width="100%" Selected="true">
                                        <HeaderTemplate>
                                            <a class="rpExpandable" style="float: left">
                                                <span class="rpExpandHandle"></span>
                                            </a>
                                            <asp:Label runat="server" ID="label_repositoryName"></asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <div style="padding: 5px; width: 100%" class="repeaterTemplate">
                                                <asp:Repeater runat="server" ID="repeater_projects">
                                                    <ItemTemplate>
                                                        <h4 style="border-bottom: 1px solid #ccc;">
                                                            <asp:Label runat="server" Text='<%# Eval("ProjectName") %>'></asp:Label>
                                                        </h4>
                                                        <asp:Repeater runat="server" ID="repeater_nodes" DataSource='<%# (Container.DataItem as SkypeIntlPortfolio.Ajax.Pages.InternalTools.ProjectsDocumentation.ProjectDocInfo).Nodes %>'>
                                                            <HeaderTemplate>
                                                                <ul>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <li>
                                                                    <asp:HyperLink runat="server" Text='<%# Eval("NodeName") %>' NavigateUrl='<%# Eval("Url") %>'></asp:HyperLink>
                                                                </li>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </ul>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </ContentTemplate>
                                    </telerik:RadPanelItem>
                                </Items>
                            </telerik:RadPanelBar>
                        </ItemTemplate>
                    </telerik:RadPanelBar>
                </asp:Panel>
            </div>
        </telerik:RadPane>

        <telerik:RadSplitBar ID="radsplitbar" runat="server" CollapseMode="Forward">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="radpane_center" Width="100%" runat="server" Height="750">

            <div style="width: 100%; padding: 40px; font-size: 15px">
                <asp:Literal ID="mdContent" runat="server" />
            </div>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
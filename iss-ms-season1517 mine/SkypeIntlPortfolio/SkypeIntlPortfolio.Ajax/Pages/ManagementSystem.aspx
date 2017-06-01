<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="ManagementSystem.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.ManagementSystem" MasterPageFile="~/MasterPage.Master" %>

<%@ Register Src="~/UserControls/Eol/EOLControl.ascx" TagName="EOLControl" TagPrefix="local" %>
<%@ Register Src="~/UserControls/ProductProfile/ProductInfoNewControl.ascx" TagPrefix="local" TagName="ProductInfoNewControl" %>
<%@ Register Src="~/UserControls/Schedule/ScheduleControl.ascx" TagName="ScheduleControl" TagPrefix="local" %>
<%@ Register Src="~/UserControls/ScheduleControl_old.ascx" TagPrefix="local" TagName="ScheduleControl_old" %>

<asp:Content runat="server" ID="content1" ContentPlaceHolderID="head">
    <style type="text/css">
        .stripLayout {
            padding: 10px;
        }

        .rspPane div {
            /*overflow: visible !important;*/
        }

        .listboxProducts div.rpTemplate {
            padding: 0px;
        }

        .submitButton {
            height: 30px;
            padding-left: 4px;
            margin-left: 30px;
        }
    </style>
</asp:Content>

<asp:Content runat="server" ID="content2" ContentPlaceHolderID="bodyPlaceHolder">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function adjustLoadingPanelHeight() {
                $get("<%= loadingPanelPortfolioManagementSystem.ClientID %>").style.height = document.documentElement.scrollHeight + "px";
            }

            var loadingPanel = "";
            var pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();
            var postBackElement = "";
            pageRequestManager.add_initializeRequest(initializeRequest);
            pageRequestManager.add_endRequest(endRequest);

            function initializeRequest(sender, eventArgs) {
                loadingPanel = $find("<%= loadingPanelPortfolioManagementSystem.ClientID %>");
                postBackElement = eventArgs.get_postBackElement().id;
                loadingPanel.show(postBackElement);
            }

            function endRequest(sender, eventArgs) {
                loadingPanel = $find("<%= loadingPanelPortfolioManagementSystem.ClientID %>");
                loadingPanel.hide(postBackElement);
            }
        </script>
    </telerik:RadCodeBlock>

    <telerik:RadAjaxManager runat="server" ID="radManager1" ClientEvents-OnRequestStart="adjustLoadingPanelHeight">
        <AjaxSettings>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanelPortfolioManagementSystem" Style="position: absolute; top: 0; left: 0;" Width="100%" Height="100%" IsSticky="true" CssClass="custom_loadingPanel">
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel runat="server" ID="Panel_PortfolioManagementSystem">
        <div class="panel-body">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12" style="margin: 0px; padding: 0px;">
                        <asp:Panel ID="panel_ViewPage" runat="server">
                            <div style="margin-top: -15px; margin-bottom: 5px; margin-left: 10px;">
                                <asp:HyperLink runat="server" Text="Operations Wiki" NavigateUrl="https://microsoft.sharepoint.com/teams/SkypeIntl/SkypeLocWF" Target="_blank" Font-Italic="true" Font-Underline="true"></asp:HyperLink>
                            </div>
                            <telerik:RadTabStrip runat="server" AutoPostBack="true" ID="RadTabStripOne" MultiPageID="RadMultiPageOne" SelectedIndex="0" CausesValidation="false" OnTabClick="RadTabStripOne_TabClick">
                                <Tabs>
                                    <telerik:RadTab Text="Schedule" Width="100px" Height="30px"></telerik:RadTab>
                                    <telerik:RadTab Text="Product Info" Width="110px" Height="30px"></telerik:RadTab>
                                    <telerik:RadTab Text="EOL" Width="100px" Height="30px"></telerik:RadTab>
                                    <telerik:RadTab Text="Schedule(Old)" Width="120px" Height="30px"></telerik:RadTab>
                                </Tabs>
                            </telerik:RadTabStrip>

                            <div class="container-fluid" style="padding: 0px;">
                                <telerik:RadSplitter ID="radsplitter_main" runat="server" Width="100%" Height="700" VisibleDuringInit="false">
                                    <telerik:RadPane ID="radpane_products" MinWidth="194" runat="server" Height="700">
                                        <div class="listboxProducts">
                                            <telerik:RadPanelBar runat="server" ID="radPanelBar_product_root" ExpandMode="MultipleExpandedItems" Width="100%">
                                                <ItemTemplate>
                                                    <telerik:RadPanelBar runat="server" ID="radPanelBar_product_child" Width="100%">
                                                        <Items>
                                                            <telerik:RadPanelItem Expanded="false">
                                                                <HeaderTemplate>
                                                                    <div style="background: transparent !important; padding: 0px">
                                                                        <a class="rpExpandable" style="float: left">
                                                                            <span class="rpExpandHandle"></span>
                                                                        </a>
                                                                        <div>
                                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "Parent.Parent.Parent.Attributes[\"UserTypePlusFamily\"]")%>'></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ContentTemplate>
                                                                    <div style="padding: 0px;">
                                                                        <telerik:RadListBox runat="server" AutoPostBack="true" ID="radListBox_products" OnClientCheckAllChecked="clientCheckAll" CheckBoxes="true" ShowCheckAll="true" CausesValidation="true" OnItemCheck="radListBox_products_ItemCheck" Width="100%">
                                                                        </telerik:RadListBox>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </telerik:RadPanelItem>
                                                        </Items>
                                                    </telerik:RadPanelBar>
                                                </ItemTemplate>
                                            </telerik:RadPanelBar>
                                        </div>
                                    </telerik:RadPane>
                                    <telerik:RadSplitBar ID="radsplitbar" runat="server" CollapseMode="Forward">
                                    </telerik:RadSplitBar>
                                    <telerik:RadPane ID="radpane_content" runat="server" MinWidth="1218" Height="700" Scrolling="Y">
                                        <div>
                                            <telerik:RadMultiPage runat="server" ID="RadMultiPageOne" SelectedIndex="0" Width="100%" Height="100%" RenderSelectedPageOnly="true">
                                                <telerik:RadPageView runat="server" ID="radPageView1">
                                                    <div class="container-fluid custom custom_transparent stripLayout">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <local:ScheduleControl runat="server" ID="custom_scheduleControl" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </telerik:RadPageView>
                                                <telerik:RadPageView runat="server" ID="radPageView_productInfoNew">

                                                    <asp:Panel runat="server" ID="panel_productInfo">
                                                        <div class="container-fluid custom custom_transparent stripLayout">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <local:ProductInfoNewControl runat="server" ID="custom_productInfoNewControl"></local:ProductInfoNewControl>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </telerik:RadPageView>
                                                <telerik:RadPageView runat="server" ID="radPageView_eol">
                                                    <div class="container-fluid custom custom_transparent stripLayout">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <local:EOLControl runat="server" ID="custom_eolControl" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </telerik:RadPageView>
                                                <telerik:RadPageView runat="server" ID="radPageView_schedule">
                                                    <asp:PlaceHolder ID="sharedCalendarPlaceHolder" runat="server"></asp:PlaceHolder>
                                                    <div class="container-fluid custom custom_transparent stripLayout">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <telerik:RadPanelBar EnableViewState="true" runat="server" ID="radPanelBar_schedule_root" ExpandMode="MultipleExpandedItems" Width="100%" Skin="Telerik">
                                                                    <ItemTemplate>
                                                                        <telerik:RadPanelBar EnableViewState="true" ID="radPanelBar_schedule_child" runat="server" Width="100%" Skin="Telerik">
                                                                            <Items>
                                                                                <telerik:RadPanelItem Expanded="true">
                                                                                    <HeaderTemplate>
                                                                                        <div style="background: transparent !important;">
                                                                                            <a class="rpExpandable" style="float: left">
                                                                                                <span class="rpExpandHandle"></span>
                                                                                            </a>
                                                                                            <div>
                                                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "Parent.Parent.Parent.Attributes[\"ProductName\"]")%>'></asp:Label>
                                                                                                <asp:HyperLink
                                                                                                    ID="HyperLinkToLineOfSight"
                                                                                                    runat="server" Target="_blank"
                                                                                                    Text="Go to 'Line of Sight' page for this project"
                                                                                                    NavigateUrl='<%# "~/Pages/ReportingSystem.aspx?Tab=LineofSight&Products=" + DataBinder.Eval(Container, "Parent.Parent.Parent.Attributes[\"ProductKey\"]")%>'
                                                                                                    Style="margin-left: 165px; font-weight: bold; font-size: medium" Font-Italic="true" Font-Underline="true">
                                                                                                </asp:HyperLink>
                                                                                            </div>
                                                                                        </div>
                                                                                    </HeaderTemplate>
                                                                                    <ContentTemplate>
                                                                                        <local:ScheduleControl_old runat="server" ID="custom_scheduleControl_old" />
                                                                                    </ContentTemplate>
                                                                                </telerik:RadPanelItem>
                                                                            </Items>
                                                                        </telerik:RadPanelBar>
                                                                    </ItemTemplate>
                                                                </telerik:RadPanelBar>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12" style="margin-top: 250px;">
                                                                <asp:Label runat="server" ID="label_warning_cancelledProduct" ForeColor="Red" Style="margin-left: 320px; font-size: medium" Visible="false"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </telerik:RadPageView>
                                            </telerik:RadMultiPage>
                                        </div>
                                    </telerik:RadPane>
                                </telerik:RadSplitter>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportingSystem.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.ReportingSystem" MasterPageFile="~/MasterPage.Master" %>

<%@ Register Src="~/UserControls/LOS/LineOfSightControl.ascx" TagName="LineOfSightControl" TagPrefix="local" %>

<%@ Register Src="~/UserControls/Vacation/VacationDaysEntryControl.ascx" TagName="VacationDaysEntryControl" TagPrefix="local" %>
<asp:Content runat="server" ID="content1" ContentPlaceHolderID="head">
    <style type="text/css">
        .multiColumn ul {
            width: 100%;
        }

        .multiColumn li {
            float: left;
            width: 25%;
        }
        
        .stripLayout {
            padding: 10px;
        }

        .listboxProducts div.rpTemplate {
            padding: 0px;
        }

        /*.rlbItem {
            float: left !important;
        }*/

        /*.nopadding {
            padding: 0 !important;
            margin: 0 !important;
        }*/

        /*.no-gutter {
            margin: 0px;
        }*/

        /*RAD_SPLITTER_PANE_CONTENT_ctl00_bodyPlaceHolder_radpane_products*/
    </style>
</asp:Content>

<asp:Content runat="server" ID="content2" ContentPlaceHolderID="bodyPlaceHolder">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function adjustLoadingPanelHeight() {
                $get("<%= loadingPanelReportingSystem.ClientID %>").style.height = document.documentElement.scrollHeight + "px";
            }

             var loadingPanel = "";
            var pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();
            var postBackElement = "";
            pageRequestManager.add_initializeRequest(initializeRequest);
            pageRequestManager.add_endRequest(endRequest);

            function initializeRequest(sender, eventArgs) {
                loadingPanel = $find("<%= loadingPanelReportingSystem.ClientID %>");
                postBackElement = eventArgs.get_postBackElement().id;
                loadingPanel.show(postBackElement);
            }

            function endRequest(sender, eventArgs) {
                loadingPanel = $find("<%= loadingPanelReportingSystem.ClientID %>");
                loadingPanel.hide(postBackElement);
            }
        </script>
    </telerik:RadCodeBlock>

    <telerik:RadAjaxManager runat="server" ID="radManager1" ClientEvents-OnRequestStart="adjustLoadingPanelHeight">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadTabStripOne">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPageOne" LoadingPanelID="loadingPanelReportingSystem" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="RadTabStripOne" LoadingPanelID="loadingPanelReportingSystem" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radPanelBar_product_root">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPageOne" LoadingPanelID="loadingPanelReportingSystem" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="radPanelBar_product_root" LoadingPanelID="loadingPanelReportingSystem" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadMultiPageOne">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPageOne" LoadingPanelID="loadingPanelReportingSystem" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radbutton_deselectAllProjects">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radPanelBar_product_root" LoadingPanelID="loadingPanelReportingSystem" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPageOne" LoadingPanelID="loadingPanelReportingSystem" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="RadListBox_locations" LoadingPanelID="loadingPanelReportingSystem" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadListBox_locations">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radPanelBar_product_root" LoadingPanelID="loadingPanelReportingSystem" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPageOne" LoadingPanelID="loadingPanelReportingSystem" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanelReportingSystem"  
        Style="z-index:7000!important;  position: absolute; top: 0; left: 0" Width="100%" Height="100%" IsSticky="true">
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel runat="server" ID="Panel_ReportingSystem">
        <div class="panel-body">
        <div class="container-fluid">
                <div class="row">
                <div class="col-md-1" style="width: 9%; margin-top: -8px;">
                    <asp:HyperLink runat="server" Text="Operations Wiki" NavigateUrl="https://microsoft.sharepoint.com/teams/SkypeIntl/SkypeLocWF" Target="_blank" Font-Italic="true" Font-Underline="true"></asp:HyperLink>
                </div>
                <div class="col-md-2" style="width: 10%; margin-top: -8px; margin-bottom: 15px; float: left;">
                    <telerik:RadButton runat="server" ID="radbutton_deselectAllProjects" Text="Deselect all projects" AutoPostBack="true" CausesValidation="false" OnClick="radbutton_deselectAllProjects_Click" />
                </div>
                <div class="col-md-4" style="width: 18%; margin-top: -8px; margin-bottom: 15px; margin-left: 20px; float: left;">
                    <asp:Label runat="server" ID="label_selectLocation" Text="Select projects based on locations"></asp:Label>
                </div>
                <div class="col-md-5" style="margin-top: -15px; float: left;">
                    <telerik:RadListBox ID="RadListBox_locations" runat="server" CheckBoxes="true" AutoPostBack="true" CausesValidation="false" OnItemCheck="RadListBox_locations_ItemCheck" OnClientSelectedIndexChanging="OnClientSelectedIndexChanging" Width="100%" CssClass="multiColumn">
                        <Items>
                            <telerik:RadListBoxItem Text="Tallinn" />
                            <telerik:RadListBoxItem Text="Redmond" />
                            <telerik:RadListBoxItem Text="Beijing" />
                        </Items>
                    </telerik:RadListBox>
                </div>
            </div>
                <div class="row">
                    <div class="col-md-12" style="margin: 0px; padding: 0px;">
                        <asp:Panel ID="panel_PageView" runat="server">
                            <telerik:RadTabStrip runat="server" AutoPostBack="true" ID="RadTabStripOne" MultiPageID="RadMultiPageOne" CausesValidation="false" OnTabClick="RadTabStripOne_TabClick">
                                <Tabs>
                                    <telerik:RadTab Text="Line Of Sight" Width="150px" Height="30px"></telerik:RadTab>
                                    <telerik:RadTab Text="Vacation Days Entry" Width="150px" Height="30px"></telerik:RadTab>
                                </Tabs>
                            </telerik:RadTabStrip>
                            <div class="container-fluid" style="padding: 0px;">
                                <telerik:RadSplitter ID="radsplitter_main" runat="server" Width="100%" Height="600" VisibleDuringInit="false" ResizeWithBrowserWindow="true"
                                    >
                                    <telerik:RadPane ID="radpane_products" runat="server" MinWidth="194"  Height="100%">
                                        <div class="listboxProducts"  style="height: 100%;">
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
                                                                    <div style="padding: 0px">
                                                                        <telerik:RadListBox runat="server" AutoPostBack="true" ID="radListBoxProducts" OnClientCheckAllChecked="clientCheckAll" ShowCheckAll="true" CheckBoxes="true" CausesValidation="false" OnItemCheck="radListBoxProducts_ItemCheck" Width="100%" OnClientSelectedIndexChanging="OnClientSelectedIndexChanging"></telerik:RadListBox>
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
                                    <telerik:RadPane ID="radpane_content" runat="server" Height="150%" Scrolling="Y" MinWidth="1218">
                                        <div>
                                            <telerik:RadMultiPage runat="server" ID="RadMultiPageOne" SelectedIndex="0" Width="100%">
                                                <telerik:RadPageView runat="server" ID="radPageView_lineOfSight">
                                                    <div class="container-fluid custom custom_transparent stripLayout">
                                                        <div class="row" style="height:100%">
                                                            <div class="col-md-12">
                                                                <local:LineOfSightControl runat="server" ID="custom_lineOfSightControl" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </telerik:RadPageView>
                                                <telerik:RadPageView runat="server" ID="radPageView_vacationdaysentry">
                                                    <div class="container-fluid custom custom_transparent stripLayout">
                                                        <div class="row" style="height: 100%;">
                                                            <div class="col-md-12">
                                                                <local:VacationDaysEntryControl runat="server" ID="custom_vacationDaysEntryControl" />
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
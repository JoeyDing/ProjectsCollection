<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductEOL.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.ProductEOL" MasterPageFile="~/MasterPage.Master" %>

<%@ Register Src="~/UserControls/Eol/EOLControl.ascx" TagName="EOLControl" TagPrefix="local" %>
<asp:Content runat="server" ID="content1" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content runat="server" ID="content2" ContentPlaceHolderID="bodyPlaceHolder">

    <telerik:RadAjaxManager runat="server" ID="radManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadComboBox">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_ProductEOL" LoadingPanelID="loadingPanelEOL" />
                    <telerik:AjaxUpdatedControl ControlID="custom_eolControl" LoadingPanelID="loadingCustomEOL" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanelEOL">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadAjaxLoadingPanel runat="server" ID="loadingCustomEOL">
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel runat="server" ID="panel_ProductEOL">
        <div class="panel panel-info">
            <div class="panel-heading">
                Product EOL
            </div>
            <div class="panel-body">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-2">
                            <asp:Label runat="server" Text="Product Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <telerik:RadComboBox runat="server" ID="RadComboBox" OnItemsRequested="RadComboBox_ProductsRequested"
                                OnSelectedIndexChanged="RadComboBoxProducts_SelectedIndexChanged" AutoPostBack="True" Width="100%">
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class=" col-md-6">
                            <local:EOLControl ID="custom_eolControl" runat="server" Visible="true"></local:EOLControl>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
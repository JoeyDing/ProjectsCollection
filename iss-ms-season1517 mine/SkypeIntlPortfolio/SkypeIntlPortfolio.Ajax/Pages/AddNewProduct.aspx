<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNewProduct.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.AddNewProduct" MasterPageFile="~/MasterPage.Master" %>

<%@ Register Src="~/UserControls/ProductInfoControl.ascx" TagName="ProductInfoControl" TagPrefix="local" %>

<asp:Content runat="server" ID="content1" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content runat="server" ID="content2" ContentPlaceHolderID="bodyPlaceHolder">

    <telerik:RadAjaxManager runat="server" ID="radManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radButton_submit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_productForm" LoadingPanelID="loadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="panel_Feedback" LoadingPanelID="loadingPanel2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel1">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel2">
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel runat="server" ID="panel_productForm" Visible="true">
        <div class="panel panel-info">
            <div class="panel-heading">
                Add New Product
            </div>
            <div class="panel-body">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <local:ProductInfoControl runat="server" ID="custom_ProductInfoControl" ViewMode="Window" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <br />
                            <telerik:RadButton runat="server" ID="radButton_submit" Text="Submit" Visible="false" Style="padding-left: 5px" OnClick="radButton_submit_Click" AutoPostBack="true" CausesValidation="false"></telerik:RadButton>
                        </div>
                        <div class="col-md-9">
                            <br />
                            <asp:Label runat="server" ID="WarningMessageLabel" Visible="false" ForeColor="Red" Text="Please make sure all the fields are filled!"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="panel_Feedback" Visible="false">
        <div class="panel panel-success">
            <div class="panel-heading">
                Feedback Page
            </div>
            <div class="panel-body">
                <div class="container-fluid">
                    <div class="row">
                        <asp:Label runat="server" Text="You have successfully added a product!"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocalizationControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Localization.LocalizationControl" %>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-2">
            <asp:Label runat="server">Intl Build Process:</asp:Label>
        </div>
        <div class="col-md-4">
            <telerik:RadListBox runat="server" ID="radlistbox_intlBuildProcess" AutoPostBack="false" Width="100px"></telerik:RadListBox>
        </div>
    </div>
    <div class="row">
        <div class="col-md-2">
            <asp:Label runat="server">Loc Process:</asp:Label>
        </div>
        <div class="col-md-4">
            <telerik:RadListBox runat="server" ID="radlistbox_locProcess" AutoPostBack="false" Width="100px"></telerik:RadListBox>
        </div>
    </div>
</div>
<telerik:RadButton runat="server" ID="RadButton_tab_Localization_SaveAndNextPage" Text="Save and Next page" OnClick="RadButton_tab_Localization_SaveAndNextPage_Click" ValidationGroup="group_localization"></telerik:RadButton>
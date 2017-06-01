<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EOLControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.Eol.EOLControl" %>
<%@ Register Src="~/UserControls/Eol/TextInputOutput/TextInputOutputControl.ascx" TagPrefix="uc1" TagName="TextInputOutputControl" %>
<%@ Register Src="~/UserControls/Eol/SpokenInputOutput/SpokenInputOutputControl.ascx" TagPrefix="uc1" TagName="SpokenInputOutputControl" %>
<%@ Register Src="~/UserControls/Eol/LocalizedFile/LocalizedFileControl.ascx" TagPrefix="uc1" TagName="LocalizedFileControl" %>
<%@ Register Src="~/UserControls/Eol/UILanguageProduct/UILanguageProductControl.ascx" TagPrefix="uc1" TagName="UILanguageProductControl" %>
<%@ Register Src="~/UserControls/Eol/UALanguageProduct/UALanguageProductControl.ascx" TagPrefix="uc1" TagName="UALanguageProductControl" %>

<telerik:RadTabStrip runat="server" AutoPostBack="True" ID="RadTabStripEOL" MultiPageID="RadMultiPageEOL" SelectedIndex="0" CausesValidation="False" OnTabClick="RadTabStripEOL_TabClick">
    <Tabs>
        <telerik:RadTab Text="UI By Product" Width="150px" Height="30px" Selected="True"></telerik:RadTab>
        <telerik:RadTab Text="UA By Product" Width="150px" Height="30px" Selected="True"></telerik:RadTab>
        <telerik:RadTab Text="Localized Files" Width="150px" Height="30px" Selected="True"></telerik:RadTab>
        <telerik:RadTab Text="Text Input & Output" Width="180px" Height="30px"></telerik:RadTab>
        <telerik:RadTab Text="Spoken Language Input & Output" Width="250px" Height="30px" Selected="True"></telerik:RadTab>
    </Tabs>
</telerik:RadTabStrip>
<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="RadGridUILanguage">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGridUILanguage" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
<telerik:RadMultiPage runat="server" ID="RadMultiPageEOL" SelectedIndex="0" Width="100%" Height="100%" RenderSelectedPageOnly="true">
    <telerik:RadPageView runat="server" ID="radPageView_UIProduct">
        <uc1:UILanguageProductControl runat="server" ID="uiLanguageProduct" />
    </telerik:RadPageView>
    <telerik:RadPageView runat="server" ID="radPageView_UAProduct">
        <uc1:UALanguageProductControl runat="server" ID="uaLanguageProduct" />
    </telerik:RadPageView>
    <telerik:RadPageView runat="server" ID="radPageViewLocalizedFileControl">
        <uc1:LocalizedFileControl ID="locFile" runat="server" />
    </telerik:RadPageView>
    <telerik:RadPageView runat="server" ID="radPageView_TextInputOutput">
        <uc1:TextInputOutputControl ID="tio" runat="server" />
    </telerik:RadPageView>
    <telerik:RadPageView runat="server" ID="radPageView1">
        <uc1:SpokenInputOutputControl ID="sio" runat="server" />
    </telerik:RadPageView>
</telerik:RadMultiPage>
<div style="margin-top: 180px;">
    <asp:Label runat="server" ID="label_warning_unExistingProduct" ForeColor="Red" Style="margin-left: 350px; font-size: medium" Visible="false"></asp:Label>
</div>
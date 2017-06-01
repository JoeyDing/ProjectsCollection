<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductInfoNewControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ProductInfoNewControl" %>
<%@ Register Src="~/UserControls/ProductProfile/Product/ProductControl.ascx" TagPrefix="pp" TagName="ProductControl" %>
<%@ Register Src="~/UserControls/ProductProfile/Localization/LocalizationControl.ascx" TagPrefix="pp" TagName="LocalizationControl" %>
<%@ Register Src="~/UserControls/ProductProfile/Links/LinksControl.ascx" TagPrefix="pp" TagName="LinksControl" %>
<%@ Register Src="~/UserControls/ProductProfile/ReleaseInfo/ReleaseInfoControl.ascx" TagPrefix="pp" TagName="ReleaseInfoControl" %>
<%@ Register Src="~/UserControls/ProductProfile/CertsAndSignoff/CertsAndSignoffControl.ascx" TagPrefix="pp" TagName="CertsAndSignoffControl" %>
<%@ Register Src="~/UserControls/ProductProfile/BuildsAndSource/BuildsAndSourceControl.ascx" TagPrefix="pp" TagName="BuildsAndSourceControl" %>
<%@ Register Src="~/UserControls/ProductProfile/People/PeopleControl.ascx" TagPrefix="pp" TagName="PeopleControl" %>
<%@ Register Src="~/UserControls/ProductProfile/Files/FilesControl.ascx" TagPrefix="pp" TagName="FilesControl" %>
<%@ Register Src="~/UserControls/ProductProfile/PseudoInfo/PseudoInfoControl.ascx" TagPrefix="pp" TagName="PseudoInfoControl" %>

<telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
    <script type="text/javascript">
    </script>
</telerik:RadScriptBlock>

<%--when using RadAjaxManagerProxy inside an usercontrol, all controls referenced in the proxy (AjaxControlID/ControlID)
    must be put inside a "<span runat="server"></span>--%>
<%--<a href="../Model/ProjectInfo.cs">../Model/ProjectInfo.cs</a>--%>

<telerik:RadAjaxManagerProxy runat="server" ID="radManagerProxy1">
    <AjaxSettings>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>

<telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel1">
</telerik:RadAjaxLoadingPanel>

<span id="Span1" runat="server" class="productInfoNewControl">
    <asp:Panel runat="server" ID="panel_productInfoNewControl_mainContentPanel">
        <telerik:RadTabStrip runat="server" AutoPostBack="true" ID="RadTabStripProductInfo" MultiPageID="RadMultiPageInfo" SelectedIndex="0" OnTabClick="RadTabStripProductInfo_TabClick" CausesValidation="false">
            <Tabs>
                <telerik:RadTab Text="Product" Width="100px" Height="30px"></telerik:RadTab>
                <telerik:RadTab Text="Files" Width="80px" Height="30px"></telerik:RadTab>
                <telerik:RadTab Text="People" Width="80px" Height="30px"></telerik:RadTab>
                <telerik:RadTab Text="Release Info" Width="110px" Height="30px"></telerik:RadTab>
                <telerik:RadTab Text="Links" Width="80px" Height="30px"></telerik:RadTab>
                <telerik:RadTab Text="Builds and Source" Width="140px" Height="30px"></telerik:RadTab>
                <telerik:RadTab Text="Localization" Width="100px" Height="30px"></telerik:RadTab>
                <telerik:RadTab Text="Certs and Signoff" Width="140px" Height="30px"></telerik:RadTab>
                <telerik:RadTab Text="Pseudo Information" Width="152px" Height="30px"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <asp:Panel runat="server" ID="Panel_PageViews">
            <telerik:RadMultiPage runat="server" ID="RadMultiPageInfo" SelectedIndex="0" Width="100%" Height="100%" Style="margin-bottom: 200px;" RenderSelectedPageOnly="true">
                <telerik:RadPageView runat="server" ID="radPageView_Product">
                    <pp:ProductControl runat="server" ID="ProductControl" />
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="radPageView_Files">
                    <pp:FilesControl runat="server" ID="FilesControl" />
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="radPageView_People">
                    <pp:PeopleControl runat="server" ID="PeopleControl" />
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="radPageView_ReleaseInfo">
                    <pp:ReleaseInfoControl runat="server" ID="ReleaseInfoControl" />
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="radPageView_Links">
                    <pp:LinksControl runat="server" ID="PPLinks"></pp:LinksControl>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="radPageView_BuildsAndSource">
                    <pp:BuildsAndSourceControl runat="server" ID="BuildsAndSourceControl" />
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="radPageView_Localization">
                    <pp:LocalizationControl runat="server" ID="LocalizationControl" />
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="radPageView_CertsAndSignoff">
                    <pp:CertsAndSignoffControl runat="server" ID="CertsAndSignoffControl" />
                </telerik:RadPageView>
                <telerik:RadPageView runat="server" ID="radPageView_PseudoInfo">
                    <pp:PseudoInfoControl runat="server" ID="PseudoInfoControl" />
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </asp:Panel>
        <asp:Panel runat="server" ID="Panel_ProCreationHintArea" Style="margin-top: 180px;">
            <asp:Label runat="server" ID="label_warning_unExistingProduct" ForeColor="Red" Style="margin-left: 350px; font-size: medium" Visible="false">Sorry, the product you select doesn't exist,please change it to another one</asp:Label>
            <asp:LinkButton runat="server" ID="link_CreateNewPro" Text="Click this link to create an new product" OnClick="link_CreateNewPro_Click" Visible="false" Style="margin-left: 400px; font-size: medium">
            </asp:LinkButton>
        </asp:Panel>
    </asp:Panel>
</span>
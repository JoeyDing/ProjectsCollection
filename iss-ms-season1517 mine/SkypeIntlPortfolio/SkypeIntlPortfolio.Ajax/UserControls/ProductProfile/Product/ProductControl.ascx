<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Product.ProductControl" %>

<asp:UpdatePanel runat="server" ID="updatePanel_ppProductControl">
    <ContentTemplate>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Product Name:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_ProductName"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_productName"
                        Text="Product name cannot be empty." ForeColor="Red"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_Product">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Product Family:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadComboBox runat="server" ID="radCombobox_ProductFamily" AutoPostBack="false">
                        <%--<DefaultItem Text="Please select family" Value="" />--%>
                    </telerik:RadComboBox>
                </div>
                <div class="col-md-3">
                    <asp:RequiredFieldValidator ID="validator_radcombobox" runat="server" ControlToValidate="radCombobox_ProductFamily"
                        Text="please select one Product Family." ForeColor="Red"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_Product">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Localization Vso Path:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_localizationVsoPath" Width="400"></telerik:RadTextBox>
                </div>
                <div class="col-md-6">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="radTextBox_localizationVsoPath"
                        Text="Localization Vso Path cannot be empty." ForeColor="Red" Style="margin-left: 230px;"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_Product">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="hoverx">
                        <asp:Label runat="server" Font-Italic="true">
                            (Mouse hover me to check the proper format of Localization Vso Path)
                        </asp:Label>
                        <div class="tooltipforVsoPath">
                            Field must be in the form of LOCALIZATION\&lt;Product Family&gt;\&lt;Product Name&gt;
                        </div>
                    </div>
                </div>
                <div class="col-md-6"></div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Status:</asp:Label>
                </div>
                <div class="col-md-5">
                    <asp:RadioButtonList ID="radioButtonList_Status" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false" OnSelectedIndexChanged="radioButtonList_Status_SelectedIndexChanged">
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-3" style="margin-left: -280px;">
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Voice:</asp:Label>
                </div>
                <div class="col-md-5">
                    <asp:RadioButtonList ID="radioButtonList_Voice" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false" OnSelectedIndexChanged="radioButtonList_Voice_SelectedIndexChanged">
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-3" style="margin-left: -270px;">
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Description:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_Description" Width="400"></telerik:RadTextBox>
                </div>
                <div class="col-md-6">
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_Description"
                        Text="Description cannot be empty." ForeColor="Red" style="margin-left:230px;"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_Product">
                    </asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Fabric Tenant:</asp:Label>
                </div>
                <div class="col-md-5">
                    <asp:RadioButtonList ID="radioButtonList_FabricTenant" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false" OnSelectedIndexChanged="radioButtonList_FabricTenant_SelectedIndexChanged">
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-3" style="margin-left: -210px;">
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Thread:</asp:Label>
                </div>
                <div class="col-md-5">
                    <asp:RadioButtonList ID="radioButtonList_Thread" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false" OnSelectedIndexChanged="radioButtonList_Thread_SelectedIndexChanged">
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-3" style="margin-left: -270px;">
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Pillar:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadListBox ID="radListBox_Pillar" runat="server" CheckBoxes="true" EnableClientScript="false" AutoPostBack="true" CausesValidation="false" OnItemCheck="radListBox_Pillar_ItemCheck">
                    </telerik:RadListBox>
                </div>
                <div class="col-md-3">
                </div>
            </div>
        </div>
        <telerik:RadButton runat="server" ID="RadButton_tab_product_SaveAndNextPage" CausesValidation="true" Text="Save and Next page" OnClick="RadButton_tab_product_SaveAndNextPage_Click" ValidationGroup="group_Product"></telerik:RadButton>
        <asp:Label runat="server" ID="label_warning_productName" ForeColor="Red" Visible="false">Product Name already exists.Please choose a different name</asp:Label>
    </ContentTemplate>
</asp:UpdatePanel>
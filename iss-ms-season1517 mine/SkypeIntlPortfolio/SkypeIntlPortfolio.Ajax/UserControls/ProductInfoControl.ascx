<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductInfoControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ProductInfoControl" %>
<%@ Register Src="~/UserControls/OnboardingProjectControl.ascx" TagName="OnboardingProjectControl" TagPrefix="local" %>

<telerik:RadAjaxManagerProxy runat="server" ID="managerProxy">
</telerik:RadAjaxManagerProxy>
<telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel1">
</telerik:RadAjaxLoadingPanel>

<span runat="server">
    <asp:Panel runat="server" ID="panel_productForm" Visible="true">
        <div class="panel-body">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label runat="server" Text="Product Name"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <telerik:RadTextBox runat="server" ID="radTextBox_productName" Width="100%" ValidationGroup="productinfo" AutoPostBack="false" OnTextChanged="radTextBox_productName_TextChanged" Enabled="false"></telerik:RadTextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:RequiredFieldValidator runat="server" ID="requiredFieldValidator_productname" ControlToValidate="radTextBox_productName" ValidationGroup="productinfo"
                            Text="Product name cannot be empty." Display="Dynamic" ForeColor="Red" EnableClientScript="true"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label runat="server" Text="Family" Height="100%"></asp:Label>
                    </div>
                    <div class="col-md-6">
                        <asp:RadioButtonList ID="radButton_List_Family" runat="server"
                            RepeatLayout="Flow" RepeatColumns="4" ValidationGroup="productinfo" AutoPostBack="false" OnSelectedIndexChanged="radButton_List_Family_SelectedIndexChanged" Enabled="false">
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-md-2">
                        <asp:RequiredFieldValidator runat="server" ID="requiredFieldValidator_famliy" ControlToValidate="radButton_List_Family" ValidationGroup="productinfo"
                            Text="Family type must be selected." Display="Dynamic" ForeColor="Red" EnableClientScript="true"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label runat="server" Text="User Voice"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <asp:RadioButtonList ID="RadioButton_List_UserVoice" runat="server"
                            RepeatDirection="Horizontal" EnableClientScript="true" ValidationGroup="productinfo" AutoPostBack="false" Enabled="false">
                            <asp:ListItem Text="Consumer" Value="Consumer"></asp:ListItem>
                            <asp:ListItem Text="Business" Value="Business"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-md-3">
                        <asp:RequiredFieldValidator runat="server" ID="requiredFieldValidator_voice" ControlToValidate="RadioButton_List_UserVoice" ValidationGroup="productinfo"
                            Text="User type must be selected." Display="Dynamic" ForeColor="Red" EnableClientScript="true"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="row ">
                    <div class="col-md-4">
                        <asp:Label runat="server" Text="High-level product description"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <telerik:RadTextBox runat="server" ID="radTextBox_HL_Description" TextMode="MultiLine" Width="100%" EnableClientScript="true" ValidationGroup="productinfo" AutoPostBack="false" Enabled="false"></telerik:RadTextBox>
                    </div>
                    <div class="col-md-3 ex-row">
                        <asp:RequiredFieldValidator runat="server" ID="radTextBox_HL_Description_validator" ControlToValidate="radTextBox_HL_Description" ValidationGroup="productinfo"
                            Text="High-level product description cannot be empty." Display="Dynamic" ForeColor="Red" EnableClientScript="true"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label runat="server" Text="Loc PM-Alias"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <telerik:RadTextBox runat="server" ID="radTextBox_PM_Alias" Width="100%" ValidationGroup="productinfo" AutoPostBack="false" Enabled="false"></telerik:RadTextBox>
                    </div>
                    <%--                <div class="col-md-3">
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_PM_Alias" ValidationGroup="productinfo"
                        Text="PO Alias cannot be empty."  Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>--%>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label runat="server" Text="Loc PM Location"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <asp:RadioButtonList ID="radioButton_List_Location" runat="server" CausesValidation="false"
                            RepeatDirection="Horizontal" AutoPostBack="false" Enabled="false">
                            <asp:ListItem Text="Redmond" Value="Redmond"></asp:ListItem>
                            <asp:ListItem Text="Tallinn" Value="Tallinn"></asp:ListItem>
                            <asp:ListItem Text="Beijing" Value="Beijing"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label runat="server" Text="Core Team's location"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <telerik:RadTextBox runat="server" ID="radTextBox_CoreTeam_Location" Width="100%" AutoPostBack="false" ValidationGroup="productinfo" Enabled="false"></telerik:RadTextBox>
                    </div>
                    <%--                <div class="col-md-3">
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_CoreTeam_Location" ValidationGroup="productinfo"
                        Text="Core team location cannot be empty." Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>--%>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label runat="server" ID="label_fabricOnboardingCheck" Text="Fabric-Onboarding Request Done"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <telerik:RadButton ID="radButton_FabricOnBoardingCheck" runat="server" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false" CausesValidation="false" Enabled="false">
                        </telerik:RadButton>
                        <telerik:RadButton runat="server" ID="radButton_FabricOnboardingEdit" Height="25px" Text="Insert Onboarding details" OnClick="radButton_FabricOnboardingEdit_Click" AutoPostBack="true" CausesValidation="false" Enabled="false"></telerik:RadButton>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label runat="server" Text="PS Onboarding"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <telerik:RadTextBox runat="server" ID="radTextBox_PS_Onboarding" Enabled="false" Width="80%" ValidationGroup="productinfo"></telerik:RadTextBox>
                        <telerik:RadButton runat="server" ID="radButton_PS_Onboarding" Enabled="false" Height="25px" Text="Edit" AutoPostBack="true" CausesValidation="false"></telerik:RadButton>
                    </div>
                    <%--<div class="col-md-3">
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_PS_Onboarding" ValidationGroup="productinfo"
                        Text="PS Onboarding cannot be empty." Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>--%>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label runat="server" Text="Resource File Path"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <telerik:RadTextBox runat="server" ID="radTextBox_resourceFilePath" Width="100%" ValidationGroup="productinfo" AutoPostBack="false" Enabled="false"></telerik:RadTextBox>
                    </div>
                    <div class="col-md-3">
                        <%--    <asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_resourceFilePath" ValidationGroup="productinfo"
                            Text="Resource file path cannot be empty." Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label runat="server" Text="Core team contacts"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <telerik:RadTextBox runat="server" ID="radTextBox_coreTeamContacts" Width="100%" ValidationGroup="productinfo" AutoPostBack="false" Enabled="false"></telerik:RadTextBox>
                    </div>
                    <div class="col-md-3">
                        <%--    <asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_coreTeamContacts" ValidationGroup="productinfo"
                            Text="Core team contacts cannot be empty." Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</span>

<span class="custom_modal" runat="server">
    <asp:Panel runat="server" ID="window_panel">
        <div class="modal fade" id="modal_fabricOnboarding" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="height: 30px"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Onboarding project</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Panel runat="server" ID="radwindow_fabricOnboardingEdit">
                            <local:OnboardingProjectControl runat="server" ID="custom_onboardingControl" ViewMode="Window" OnOnOkButtonClicked="customControl_OkButton_Click" OnOnCancelButtonClicked="customControl_CancelButton_Click" OnLoad="custom_onboardingControl_Load" />
                            <%--<local:OnboardingProjectControl runat="server" ID="custom_onboardingControl" ViewMode="Window" OnOnOkButtonClicked="customControl_OkButton_Click" OnLoad="custom_onboardingControl_Load" />--%>
                            <%--<telerik:RadButton Width="100%" runat="server" Height="25px" Visible="true" Style="width: 65px" Text="Cancel" ID="radButton_Cancel"
                                AutoPostBack="false"
                                CssClass="style_radbutton" CausesValidation="false"
                                OnClientClicked='<%# "function(button, args){CloseWindow(\""+ radwindow_fabricOnboardingEdit.ClientID + "\");}" %>'>
                            </telerik:RadButton>--%>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</span>
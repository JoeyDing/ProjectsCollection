<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OnboardingProjectControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.OnboardingProjectControl" %>

<telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel2">
</telerik:RadAjaxLoadingPanel>

<asp:Panel runat="server" ID="panel_productForm">
    <div class="panel-body">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-4">
                    <asp:Label runat="server" Text="Product"></asp:Label>
                </div>
                <div class="col-md-5">
                    <telerik:RadComboBox AllowCustomText="true" runat="server" ID="radComboBox_Products" CausesValidation="false" OnSelectedIndexChanged="RadComboBoxProducts_SelectedIndexChanged" AutoPostBack="True" Width="100%" ValidationGroup="fabricOnboarding">
                    </telerik:RadComboBox>
                </div>
                <div class="col-md-3">
                    <asp:RequiredFieldValidator ID="radComboBox_Products_Validator" runat="server" ControlToValidate="radComboBox_Products"
                        Text="Product cannot be empty." ForeColor="Red" ValidationGroup="fabricOnboarding"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <!--format_<PRODUCTNAME,READ ONLY>-->
                    <asp:Label runat="server" Text="EpicLabel"></asp:Label>
                </div>
                <div class="col-md-5">
                    <telerik:RadTextBox runat="server" ID="radTextBoxEpicLabel" Enabled="false" Text="Fabric_" Width="100%"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <asp:RequiredFieldValidator ID="epicLabel_RequiredFieldValidator" runat="server" ControlToValidate="radTextBoxEpicLabel"
                        Text="Epic Label cannot be empty." ForeColor="Red" ValidationGroup="fabricOnboarding"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:Label runat="server" Text="Core intl-folders location"></asp:Label>
                </div>
                <div class="col-md-5">
                    <telerik:RadTextBox runat="server" ID="radTextCoreIntlFolderLocation" TextMode="MultiLine" Width="100%" ValidationGroup="fabricOnboarding"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <asp:RequiredFieldValidator ID="coreIntlFolderLocation_Validator" runat="server" ControlToValidate="radTextCoreIntlFolderLocation"
                        Text="Core intl-folders location cannot be empty." ForeColor="Red" ValidationGroup="fabricOnboarding"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:Label runat="server" Text="Core Source-File Path"></asp:Label>
                </div>
                <div class="col-md-5">
                    <telerik:RadTextBox runat="server" ID="radTextBox_CoreSourceFilePath" TextMode="MultiLine" Width="100%" ValidationGroup="fabricOnboarding"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <asp:RequiredFieldValidator runat="server" ID="coreSourceFilePath_Validator" ControlToValidate="radTextBox_CoreSourceFilePath"
                        Text="Core Source-File Path cannot be empty." ForeColor="Red" ValidationGroup="fabricOnboarding"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:Label runat="server" Text="Europe\skwttad RW permission done?"></asp:Label>
                </div>
                <div class="col-md-5">
                    <telerik:RadButton ID="radButton_PermissionCheck" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" AutoPostBack="true" OnCheckedChanged="Check_Clicked">
                    </telerik:RadButton>
                    <asp:Label runat="server" ID="LabelMessage" Visible="true" ForeColor="Red" Text="(Required for onboarding)"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:Label runat="server" Text="EOL"></asp:Label>
                </div>
                <div class="col-md-5">
                    <telerik:RadTextBox runat="server" ID="radTextBox_EOL" TextMode="MultiLine" Width="100%" ValidationGroup="fabricOnboarding"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <asp:RequiredFieldValidator ID="eol_Validator" runat="server" ControlToValidate="radTextBox_EOL"
                        Text="EOL cannot be empty." ForeColor="Red" ValidationGroup="fabricOnboarding"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <asp:Label runat="server" Text="Expected Date for Walking( =LSPs' pilot in production-tenant)"></asp:Label>
                </div>
                <div class="col-md-5">
                    <telerik:RadDatePicker ID="radDatePickerWalking" Width="100%" runat="server" ValidationGroup="fabricOnboarding">
                    </telerik:RadDatePicker>
                </div>
                <div class="col-md-3">
                    <asp:RequiredFieldValidator ID="walkingDate_Validator" runat="server" ControlToValidate="radDatePickerWalking"
                        Text="Expected Date for Walking cannot be empty." ForeColor="Red" ValidationGroup="fabricOnboarding"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <asp:Label runat="server" Text="Expected Date for Running( =LSPs actively working PLUS production dashboard available)"></asp:Label>
                </div>
                <div class="col-md-5">
                    <telerik:RadDatePicker ID="radDatePickerRunning" Width="100%" runat="server" ValidationGroup="fabricOnboarding">
                    </telerik:RadDatePicker>
                </div>
                <div class="col-md-3">
                    <asp:RequiredFieldValidator ID="runningDate_Validator" runat="server" ControlToValidate="radDatePickerRunning"
                        Text="Expected Date for Running cannot be empty." ForeColor="Red" ValidationGroup="fabricOnboarding"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-9">
                    <br />
                    <asp:Label runat="server" ID="LabelWarningMessage" Visible="false" ForeColor="Red" Text="Please make sure all the fields are filled!"></asp:Label>
                </div>
                <div class="col-md-3">
                    <br />
                    <telerik:RadButton ID="radButton_Ok" runat="server" Visible="false" Height="25px" Text="OK" OnClick="OK_Click" AutoPostBack="true" CausesValidation="false"></telerik:RadButton>
                    <telerik:RadButton ID="radButton_Cancel" runat="server" Visible="false" Height="25px" Text="Cancel" OnClick="Cancel_Click" AutoPostBack="true" CausesValidation="false"></telerik:RadButton>
                    <%--                    <telerik:RadButton Width="100%" runat="server" Height="25px" Visible="true" Style="width: 65px" Text="Cancel" ID="radButton_Cancel"
                        AutoPostBack="false"
                        CssClass="style_radbutton" CausesValidation="false"
                        OnClientClicked='<%# "function(button, args){CloseWindow(\""+ panel_productForm.ClientID + "\");}" %>'>
                    </telerik:RadButton>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
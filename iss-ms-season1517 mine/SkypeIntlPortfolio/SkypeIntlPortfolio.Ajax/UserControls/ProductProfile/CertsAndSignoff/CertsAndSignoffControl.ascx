<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CertsAndSignoffControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.CertsAndSignoff.CertsAndSignoffControl" %>
<asp:UpdatePanel runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
    <ContentTemplate>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">GB impacting:</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_GBimpacting" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false">
                        <asp:ListItem Text="Yes"></asp:ListItem>
                        <asp:ListItem Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">French Loc required:</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_FrenchLocrequired" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false">
                        <asp:ListItem Text="Yes"></asp:ListItem>
                        <asp:ListItem Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Privacy Statement required?</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_PrivacyStatementRequired" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false">
                        <asp:ListItem Text="Yes"></asp:ListItem>
                        <asp:ListItem Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Voice Prompt Loc requirement:</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_VoicePromptLocRequirement" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false">
                        <asp:ListItem Text="Yes"></asp:ListItem>
                        <asp:ListItem Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Cert Type:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_CertType"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_CertType"
                        Text="Cert Type cannot be empty." ForeColor="Red"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_certsAndSignoff">
                    </asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Cert location:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_CertLocation"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_CertLocation"
                        Text="Cert Location cannot be empty." ForeColor="Red"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_certsAndSignoff">
                    </asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Telemetry data available</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_TelemetryDataAvailable" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false">
                        <asp:ListItem Text="Yes"></asp:ListItem>
                        <asp:ListItem Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <telerik:RadButton runat="server" ID="RadButton_tab_CertsAndSignoff_SaveAndNextPage" Text="Save and Next page" OnClick="RadButton_tab_CertsAndSignoff_SaveAndNextPage_Click" ValidationGroup="group_certsAndSignoff"></telerik:RadButton>
    </ContentTemplate>
</asp:UpdatePanel>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PeopleControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.People.PeopleControl" %>

<asp:UpdatePanel runat="server" ID="updatePanel_ppPeopleControl">
    <ContentTemplate>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">MS PM Owner:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_MSPMOwner"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_MSPMOwner"
                        Text="MS PM Owner cannot be empty." ForeColor="Red"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_people">
                    </asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">iSS Ops Driver:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_iSSowner"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_iSSowner"
                        Text="ISS Owner cannot be empty." ForeColor="Red"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_people">
                    </asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">iSS IPE:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_ISSIPE"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_ISSIPE"
                        Text="ISS IPE cannot be empty." ForeColor="Red"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_people">
                    </asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">iSS Tester:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_ISSTester"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_ISSTester"
                        Text="ISS Tester cannot be empty." ForeColor="Red"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_people">
                    </asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Core PO:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_CoreTeamContact"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_CoreTeamContact"
                        Text="Core Team Contact cannot be empty." ForeColor="Red"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_people">
                    </asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Core team share point:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_CoreTeamSharePoint"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_CoreTeamSharePoint"
                        Text="Core Team Share Point cannot be empty." ForeColor="Red"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_people">
                    </asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Telemetry Contact:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_TelemetryContact"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Core Design Contact:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_CoreDesignContact"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Core Engineering Contact:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_CoreEngineeringContact"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Core team location:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadListBox ID="radListBox_CoreTeamLocation" runat="server" CheckBoxes="true" EnableClientScript="false" AutoPostBack="true" CausesValidation="false" OnItemCheck="radListBox_CoreTeamLocation_ItemCheck">
                    </telerik:RadListBox>
                </div>
                <div class="col-md-3">
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">MS PM Owner location:</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_MSPMOwnerLocation" runat="server" RepeatDirection="Horizontal" EnableClientScript="true" OnSelectedIndexChanged="radioButtonList_MSPMOwnerLocation_SelectedIndexChanged">
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-3">
                </div>
            </div>
        </div>
        <telerik:RadButton runat="server" ID="RadButton_tab_people_SaveAndNextPage" Text="Save and Next page" OnClick="RadButton_tab_people_SaveAndNextPage_Click" ValidationGroup="group_people"></telerik:RadButton>
    </ContentTemplate>
</asp:UpdatePanel>
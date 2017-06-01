<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PseudoInfoControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.PseudoInfo.PseudoInfoControl" %>
<asp:UpdatePanel runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
    <ContentTemplate>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-7">
                    <asp:Label runat="server">Pseudo Build Enabled:</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_PseudoBuildEnabled" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false">
                        <asp:ListItem Text="Yes"></asp:ListItem>
                        <asp:ListItem Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7">
                    <asp:Label runat="server">Pseudo testing run on a regular basis (test passes or automated pseudo testing):</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_PseTestingRunOnRegular" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false">
                        <asp:ListItem Text="Yes"></asp:ListItem>
                        <asp:ListItem Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7">
                    <asp:Label runat="server">Pseudo testing and other localizability checks are part of the definition of done of all features with loc impact:</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_PseTestAndLocCheck" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false">
                        <asp:ListItem Text="Yes"></asp:ListItem>
                        <asp:ListItem Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7">
                    <asp:Label runat="server">Pseudo is run at dev-time by developers before check-in:</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_pseRunDevtime" runat="server"
                        RepeatDirection="Horizontal" EnableClientScript="true" AutoPostBack="false">
                        <asp:ListItem Text="Yes"></asp:ListItem>
                        <asp:ListItem Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <telerik:RadButton runat="server" ID="RadButton_tab_PseudoInfo_SaveAndNextPage" Text="Save and Next page" OnClick="RadButton_tab_PseudoInfo_SaveAndNextPage_Click" ValidationGroup="group_PseudoInfo"></telerik:RadButton>
    </ContentTemplate>
</asp:UpdatePanel>
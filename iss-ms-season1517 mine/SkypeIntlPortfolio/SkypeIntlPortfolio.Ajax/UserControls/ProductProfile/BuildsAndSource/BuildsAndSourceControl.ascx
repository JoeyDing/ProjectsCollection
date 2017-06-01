<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuildsAndSourceControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.BuildsAndSource.BuildsAndSourceControl" %>
<telerik:RadScriptBlock runat="server" ID="RadScriptBlock1">

    <script type="text/javascript">

        function buildAndSource_onClientSelectedIndexChanging(sender, e) {
            e.set_cancel(true);
        }
    </script>
</telerik:RadScriptBlock>
<asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
    <ContentTemplate>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Build Systems:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadListBox ID="radListBox_BuildSystems" runat="server" CausesValidation="false">
                    </telerik:RadListBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Source Controls:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadListBox ID="radListBox_sourceControls" runat="server" CheckBoxes="true" CausesValidation="false" OnClientSelectedIndexChanging="buildAndSource_onClientSelectedIndexChanging">
                    </telerik:RadListBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Source Storages:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadListBox ID="radListBox_SourceStorage" runat="server" CheckBoxes="true" CausesValidation="false" OnClientSelectedIndexChanging="buildAndSource_onClientSelectedIndexChanging">
                    </telerik:RadListBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Code Review systems:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadListBox ID="radListBox_codeReviewSystem" runat="server" CausesValidation="false">
                    </telerik:RadListBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Source Code location:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadTextBox runat="server" ID="radTextBox_SourceCodeLocation"></telerik:RadTextBox>
                </div>
                <div class="col-md-3">
                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_SourceCodeLocation"
                        Text="Source Code Location cannot be empty." ForeColor="Red"
                        Display="Dynamic" EnableClientScript="true" ValidationGroup="group_buildsAndSource">
                    </asp:RequiredFieldValidator>--%>
                </div>
            </div>
        </div>
        <telerik:RadButton runat="server" ID="RadButton_tab_BuildsAndSource_SaveAndNextPage" Text="Save and Next page" OnClick="RadButton_tab_BuildsAndSource_SaveAndNextPage_Click" ValidationGroup="group_buildsAndSource"></telerik:RadButton>
    </ContentTemplate>
</asp:UpdatePanel>
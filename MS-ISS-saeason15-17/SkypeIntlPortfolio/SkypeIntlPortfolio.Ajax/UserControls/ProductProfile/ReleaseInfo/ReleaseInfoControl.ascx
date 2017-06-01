<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReleaseInfoControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.ReleaseInfo.ReleaseInfoControl" %>

<asp:UpdatePanel runat="server" ID="updatePanel_ppReleaseInfoControl" RenderMode="Inline">
    <ContentTemplate>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Release Cadence:</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_ReleaseCadence" runat="server" RepeatDirection="Horizontal" EnableClientScript="true" OnSelectedIndexChanged="radioButtonList_ReleaseCadence_SelectedIndexChanged">
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-3">
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Release Channel:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadListBox ID="radListBox_ReleaseChannel" runat="server" CheckBoxes="true" EnableClientScript="false" AutoPostBack="true" CausesValidation="false" OnItemCheck="radListBox_ReleaseChannel_ItemCheck">
                    </telerik:RadListBox>
                </div>
                <div class="col-md-3">
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Language Selection:</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_LanguageSelection" runat="server" RepeatDirection="Horizontal" EnableClientScript="true" OnSelectedIndexChanged="radioButtonList_LanguageSelection_SelectedIndexChanged">
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-3">
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Platform:</asp:Label>
                </div>
                <div class="col-md-4">
                    <telerik:RadListBox ID="radListBox_platforms" runat="server" CheckBoxes="true" AutoPostBack="true" CausesValidation="false" OnItemCheck="radListBox_platforms_ItemCheck">
                    </telerik:RadListBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server">Content location:</asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="radioButtonList_ContentLocation" runat="server" RepeatDirection="Horizontal" EnableClientScript="true" OnSelectedIndexChanged="radioButtonList_ContentLocation_SelectedIndexChanged">
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-3">
                </div>
            </div>
        </div>
        <telerik:RadButton runat="server" ID="RadButton_tab_ReleaseInfo_SaveAndNextPage" Text="Save and Next page" OnClick="RadButton_tab_ReleaseInfo_SaveAndNextPage_Click" ValidationGroup="group_releaseInfo"></telerik:RadButton>
    </ContentTemplate>
</asp:UpdatePanel>
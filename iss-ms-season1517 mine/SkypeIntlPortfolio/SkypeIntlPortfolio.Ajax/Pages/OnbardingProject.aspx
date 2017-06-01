<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnbardingProject.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.OnbardingProject" MasterPageFile="~/MasterPage.Master" %>

<%@ Register Src="~/UserControls/OnboardingProjectControl.ascx" TagName="OnboardingProjectControl" TagPrefix="local" %>

<asp:Content runat="server" ID="content1" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content runat="server" ID="content2" ContentPlaceHolderID="bodyPlaceHolder">
    <telerik:RadAjaxManager runat="server" ID="radManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radButton_submit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_productForm" LoadingPanelID="loadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="panel_Feedback" LoadingPanelID="loadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="panel_SubmitButton" LoadingPanelID="loadingPanel3" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <asp:Panel runat="server" ID="panel_productForm" Visible="true">
        <div class="panel panel-info">
            <div class="panel-heading">
                Fabric Onboarding
            </div>
            <div class="panel-body">
                <div class="container-fluid">
                    <local:OnboardingProjectControl runat="server" ID="custom_onboardingControl" />
                    <asp:Panel runat="server" ID="panel_SubmitButton" Visible="true">
                        <div class="row">
                            <div class="col-md-12">
                                <telerik:RadButton runat="server" ID="radButton_submit" Text="Submit" OnClick="radButton_submit_Click" AutoPostBack="true" CausesValidation="false"></telerik:RadButton>
                                <asp:Label runat="server" ID="WarningMessageLabel" Visible="false" ForeColor="Red" Text="Please make sure all the fields are filled!"></asp:Label>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="panel_Feedback" Visible="false">
        <div class="panel panel-success">
            <div class="panel-heading">
                Onboarding is done
            </div>
            <div class="panel-body">
                <div class="container-fluid">
                    <div class="row">
                        <asp:Label runat="server" Text="You have successfully onboarded"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
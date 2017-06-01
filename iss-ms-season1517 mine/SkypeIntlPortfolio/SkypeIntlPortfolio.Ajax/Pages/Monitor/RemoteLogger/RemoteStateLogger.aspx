<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemoteStateLogger.aspx.cs" MasterPageFile="~/MasterPage.Master" Inherits="SkypeIntlPortfolio.Ajax.Pages.Monitor.RemoteStateLogger" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Src="~/UserControls/RemoteLogger/RemoteLoggerControl.ascx" TagPrefix="local" TagName="RemoteLoggerControl" %>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <local:RemoteLoggerControl runat="server" ID="custom_RemoteLoggerControl" />
</asp:Content>
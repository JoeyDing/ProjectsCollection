<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemoteLoggerDetail.aspx.cs" Inherits="RemoteLogger.RemoteLoggerDetail" %>

<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">

<body>
    <div style="padding-left: 50px; padding-right: 50px">
        <div style="color: #800000; font-style: italic; font-size: 18px; padding-bottom: 10px">
            <asp:Label ID="lbException" runat="server"></asp:Label>
        </div>
        <div style="padding-bottom: 10px">
            <asp:Label ID="lbUpdateDate" runat="server"></asp:Label>
        </div>
        <div style="background-color: #ffffe6; font-size: 11px">
            <asp:Label ID="lbStackTrace" runat="server"></asp:Label>
        </div>
    </div>
</body>
</html>
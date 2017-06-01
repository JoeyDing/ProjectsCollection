<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="listviewTest.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.InternalTools.listviewTest" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ListView ID="lstProductsListView" runat="server">
            <LayoutTemplate>
                <div class="grid">
                    <table id="products">
                        <tr class="header">
                            <th>Product Id</th>
                            <th>Product Name</th>
                            <th>ListPrice</th>
                        </tr>
                        <tr id="itemPlaceHolder" runat="server"></tr>
                    </table>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <tr id="row" runat="server" class="group">
                    <th class="first"></th>
                    <th colspan="2"><%# Eval("GroupName") %> 
                    (<%# Eval("Count") %> Count)</th>
                </tr>

                <asp:ListView ID="lstProducts" runat="server" 
                                  DataSource='<%# Eval("Items") %>'>
                    <LayoutTemplate>
                        <tr id="itemPlaceHolder" runat="server"></tr>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr id="row" runat="server" class="items">
                            <td><%# Eval("ProductId") %></td>
                            <td><%# Eval("ProductName") %></td>
                            <td><%# Eval("ListPrice") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>

            </ItemTemplate>
        </asp:ListView>
        <br />
    </form>
</body>
</html>

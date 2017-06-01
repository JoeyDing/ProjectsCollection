<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocalizedFileControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.Eol.LocalizedFileControl" %>

<style type="text/css">
    .rgCaption {
        font-weight: bold;
        color: black;
        font-size: 12px;
    }

    .gapToRightBorderTestPlan {
        padding-right: 150px !important;
    }

    .gapToRightBorderMilestone {
        padding-right: 20px !important;
    }

    .gapToRightBorderRelease {
        padding-left: 170px !important;
    }

    tr.spaceUnder > td {
        padding-bottom: 2em;
    }

    .RadGrid .rgEditPopup .rgHeader {
        background-color: rgb(49, 190, 243);
        color: white;
        height: 30px !important;
    }

    .centerPopUpModal {
        position: fixed;
        top: 20px;
        left: 100px;
        z-index: 3000;
    }

    div.RadGrid .rgPager .rgAdvPart {
        display: none;
    }

    .RadGrid_Metro .rgAltRow {
        background: transparent;
    }

    .RadGrid_Metro td.rgGroupCol, .RadGrid_Metro td.rgExpandCol {
        background: transparent;
    }

    .RadGrid_Metro .rgRow > td, .RadGrid_Metro .rgAltRow > td, .RadGrid_Metro .rgEditRow > td {
        border-style: none;
    }

    .RadGrid_Metro .rgHeader, .RadGrid_Metro th.rgResizeCol, .RadGrid_Metro .rgHeaderWrapper {
        border: none;
        border-bottom: none;
        border-left: none;
    }

    .RadGrid_Metro {
        border: none;
    }

        .RadGrid_Metro .rgCommandCell {
            border-bottom: none;
        }

    .EditWindowColor {
        background-color: #fff;
    }
</style>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var popUp;
        function popUpShowing(sender, eventArgs) {
            popUp = eventArgs.get_popUp();
            var gridWidth = sender.get_element().offsetWidth;
            var gridHeight = sender.get_element().offsetHeight;
            var popUpWidth = popUp.style.width.substr(0, popUp.style.width.indexOf("px"));
            var popUpHeight = popUp.style.height.substr(0, popUp.style.height.indexOf("px"));
            popUp.style.left = ((gridWidth - popUpWidth) / 2 + sender.get_element().offsetLeft).toString() + "px";
            popUp.style.top = ((gridHeight - popUpHeight) / 2 + sender.get_element().offsetTop).toString() + "px";
        }
    </script>
</telerik:RadCodeBlock>

<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="RadGrid_LocalizedFile">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGrid_LocalizedFile"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
<telerik:RadGrid ID="RadGrid_LocalizedFile"
    runat="server"
    AllowPaging="True"
    AllowCustomPaging="true"
    CellSpacing="0"
    OnNeedDataSource="RadGrid_LocalizedFile_NeedDataSource"
    OnItemDataBound="RadGrid_LocalizedFile_ItemDataBound"
    EnableViewState="true"
    ViewStateMode="Enabled"
    RenderMode="Lightweight"
    PageSize="10"
    OnDetailTableDataBind="RadGrid_LocalizedFile_DetailTableDataBind"
    EnableEmbeddedSkins="true"
    OnItemCommand="RadGrid_LocalizedFile_ItemCommand"
    Height="600px">
    <MasterTableView
        AllowPaging="true"
        AllowCustomPaging="true"
        AutoGenerateColumns="false"
        EnableGroupsExpandAll="true"
        ViewStateMode="Enabled"
        HierarchyDefaultExpanded="false"
        DataKeyNames="FileKey"
        EditMode="PopUp"
        Name="File"
        runat="server"
        PageSize="20"
        BorderWidth="0"
        EnableViewState="true"
        Caption="Files">

        <Columns>

            <telerik:GridBoundColumn DataField="Source_File_Location" HeaderText="File Name">
                <HeaderStyle Width="200px" Font-Underline="true" />
            </telerik:GridBoundColumn>
        </Columns>
        <DetailTables>
            <telerik:GridTableView
                runat="server"
                AllowPaging="true"
                AllowCustomPaging="true"
                PageSize="20"
                AutoGenerateColumns="False"
                BorderWidth="0"
                HierarchyDefaultExpanded="false"
                EnableViewState="true"
                ViewStateMode="Enabled"
                Name="LocalizedFiles"
                ShowHeader="true"
                DataKeyNames="TargetFileKey"
                Caption="">
                <Columns>
                    <telerik:GridBoundColumn DataField="LanguageName" HeaderText="Language Name">
                        <HeaderStyle Width="70px" Font-Underline="true" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CultureName" HeaderText="Culture Name">
                        <HeaderStyle Width="50px" Font-Underline="true" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Target_File_Location" HeaderText="Target File Name">
                        <HeaderStyle Width="200px" Font-Underline="true" />
                    </telerik:GridBoundColumn>
                </Columns>
            </telerik:GridTableView>
        </DetailTables>
    </MasterTableView>
    <ClientSettings>
        <ClientEvents OnPopUpShowing="popUpShowing" />
    </ClientSettings>
</telerik:RadGrid>
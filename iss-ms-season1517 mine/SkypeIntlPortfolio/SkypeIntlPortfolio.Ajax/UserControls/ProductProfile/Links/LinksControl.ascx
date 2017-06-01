<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinksControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Links.LinksControl" %>

<style type="text/css">
.maximize input[type=text] { width:100%; }
</style>

 <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid_LCL_Location">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_LCL_Location"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid_LCG_Location">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_LCG_Location"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid_LCT_Location">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_LCT_Location"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-2">
            <asp:Label runat="server">VSO link (localization):</asp:Label>
        </div>
        <div class="col-md-4">
            <telerik:RadTextBox runat="server" ID="radTextBox_VSOlink_Localization" Width="600"></telerik:RadTextBox>
        </div>
        <div class="col-md-3">
            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_VSOlink_Localization"
                Text="VSO-link Localization cannot be empty." ForeColor="Red"
                Display="Dynamic" EnableClientScript="true" ValidationGroup="group_links">
            </asp:RequiredFieldValidator>--%>
        </div>
    </div>
    <div class="row">
        <div class="col-md-2">
            <asp:Label runat="server">VSO link (Core):</asp:Label>
        </div>
        <div class="col-md-4">
            <telerik:RadTextBox runat="server" ID="radTextBox_VSOlink_Core" Width="600"></telerik:RadTextBox>
        </div>
        <div class="col-md-3">
            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_VSOlink_Core"
                Text="VSO-link Core cannot be empty." ForeColor="Red"
                Display="Dynamic" EnableClientScript="true" ValidationGroup="group_links">
            </asp:RequiredFieldValidator>--%>
        </div>
    </div>
    <div class="row">
        <div class="col-md-2">
            <asp:Label runat="server">Build location:</asp:Label>
        </div>
        <div class="col-md-4">
            <telerik:RadTextBox runat="server" ID="radTextBox_BuildLocation" Width="600"></telerik:RadTextBox>
        </div>
        <div class="col-md-3">
            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_BuildLocation"
                Text="Build Location cannot be empty." ForeColor="Red"
                Display="Dynamic" EnableClientScript="true" ValidationGroup="group_links">
            </asp:RequiredFieldValidator>--%>
        </div>
    </div>
    <div class="row">
        <div class="col-md-2">
            <asp:Label runat="server">LCG location:</asp:Label>
        </div>
        <div class="col-md-10">
            <telerik:RadGrid ID="RadGrid_LCG_Location" runat="server" CellSpacing="0"
                OnNeedDataSource="RadGrid_LCG_Location_NeedDataSource" OnItemCommand="RadGrid_LCG_Location_ItemCommand"
                EnableViewState="true" ViewStateMode="Enabled"
                RenderMode="Lightweight" EnableEmbeddedSkins="true">
                <MasterTableView runat="server" AutoGenerateColumns="false" EnableGroupsExpandAll="true"
                    ViewStateMode="Enabled" HierarchyDefaultExpanded="true" Name="Location"
                    ShowHeader="true" CommandItemDisplay="Top" EditMode="InPlace" Caption="">
                    <EditFormSettings>
                        <PopUpSettings Modal="true" ZIndex="2500"/>
                    </EditFormSettings>
                    <CommandItemSettings AddNewRecordText="Add a New LCG Location"
                        ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel" UniqueName="EditCommandColumn1">
                            <ItemStyle Width="7%" HorizontalAlign="Center"></ItemStyle>
                        </telerik:GridEditCommandColumn>
                        <telerik:GridBoundColumn DataField="Location" HeaderText="LCG Location" Display="true">
                            <HeaderStyle Width="200px" Font-Underline="true" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left"  CssClass="maximize"/>
                        </telerik:GridBoundColumn>
                        <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this Location?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
	            </telerik:RadGrid>
        </div>
    </div>
   <div class="row">
        <div class="col-md-2">
            <asp:Label runat="server">LCT location:</asp:Label>
        </div>
        <div class="col-md-10">
            <telerik:RadGrid ID="RadGrid_LCT_Location" runat="server" CellSpacing="0"
                OnNeedDataSource="RadGrid_LCT_Location_NeedDataSource" OnItemCommand="RadGrid_LCT_Location_ItemCommand"
                EnableViewState="true" ViewStateMode="Enabled"
                RenderMode="Lightweight" EnableEmbeddedSkins="true">
                <MasterTableView runat="server" AutoGenerateColumns="false" EnableGroupsExpandAll="true"
                    ViewStateMode="Enabled" HierarchyDefaultExpanded="true" Name="Location"
                    ShowHeader="true" CommandItemDisplay="Top" EditMode="InPlace" Caption="">
                    <EditFormSettings>
                        <PopUpSettings Modal="true" ZIndex="2500"/>
                    </EditFormSettings>
                    <CommandItemSettings AddNewRecordText="Add a New LCT Location"
                        ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel" UniqueName="EditCommandColumn1">
                            <ItemStyle Width="7%" HorizontalAlign="Center"></ItemStyle>
                        </telerik:GridEditCommandColumn>
                        <telerik:GridBoundColumn DataField="Location" HeaderText="LCT Location" Display="true">
                            <HeaderStyle Width="200px" Font-Underline="true" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left"  CssClass="maximize"/>
                        </telerik:GridBoundColumn>
                        <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this Location?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
	            </telerik:RadGrid>
        </div>
    </div>
     <div class="row">
        <div class="col-md-2">
            <asp:Label runat="server">LCL location:</asp:Label>
        </div>
        <div class="col-md-10">
            <telerik:RadGrid ID="RadGrid_LCL_Location" runat="server" CellSpacing="0"
                OnNeedDataSource="RadGrid_LCL_Location_NeedDataSource" OnItemCommand="RadGrid_LCL_Location_ItemCommand"
                EnableViewState="true" ViewStateMode="Enabled"
                RenderMode="Lightweight" EnableEmbeddedSkins="true">
                <MasterTableView runat="server" AutoGenerateColumns="false" EnableGroupsExpandAll="true"
                    ViewStateMode="Enabled" HierarchyDefaultExpanded="true" Name="Location"
                    ShowHeader="true" CommandItemDisplay="Top" EditMode="InPlace" Caption="">
                    <EditFormSettings>
                        <PopUpSettings Modal="true" ZIndex="2500"/>
                    </EditFormSettings>
                    <CommandItemSettings AddNewRecordText="Add a New LCL Location"
                        ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel" UniqueName="EditCommandColumn1">
                            <ItemStyle Width="7%" HorizontalAlign="Center"></ItemStyle>
                        </telerik:GridEditCommandColumn>
                        <telerik:GridBoundColumn DataField="Location" HeaderText="LCL Location" Display="true">
                            <HeaderStyle Width="200px" Font-Underline="true" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left"  CssClass="maximize"/>
                        </telerik:GridBoundColumn>
                        <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this Location?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
	            </telerik:RadGrid>
        </div>
    </div>
</div>

<telerik:RadButton runat="server" ID="RadButton_tab_Links_SveAndNextPage" Text="Save and Next page" OnClick="RadButton_tab_Links_SveAndNextPage_Click" ValidationGroup="group_links"></telerik:RadButton>
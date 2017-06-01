<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextInputOutputControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.Eol.TextInputOutputControl" %>

<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="RadGridTextInputOutput">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGridTextInputOutput"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
<asp:Label ID="label_errorMsg" runat="server" ForeColor="Red" Visible="false" Text=""></asp:Label>

<telerik:RadGrid
    ID="RadGridTextInputOutput"
    runat="server"
    AllowPaging="True"
    AllowCustomPaging="true"
    PageSize="10"
    AllowSorting="True"
    CellSpacing="0"
    GridLines="None"
    OnDetailTableDataBind="RadGridTextInputOutput_DetailTableDataBind"
    OnNeedDataSource="RadGridTextInputOutput_NeedDataSource"
    EnableViewState="true"
    OnUpdateCommand="RadGridTextInputOutput_UpdateCommand"
    ViewStateMode="Enabled"
    RenderMode="Auto"
    OnItemDataBound="RadGridTextInputOutput_ItemDataBound"
    OnInsertCommand="RadGridTextInputOutput_InsertCommand"
    OnItemCommand="RadGridTextInputOutput_ItemCommand"
    OnPreRender="RadGridTextInputOutput_PreRender"
    OnPageIndexChanged="RadGridTextInputOutput_PageIndexChanged"
    OnPageSizeChanged="RadGridTextInputOutput_PageSizeChanged">
    <MasterTableView AutoGenerateColumns="false" EnableGroupsExpandAll="true" EditMode="InPlace" ViewStateMode="Enabled" DataKeyNames="FeatureKey" Name="Features" CommandItemDisplay="Top">
        <CommandItemSettings
            AddNewRecordText="Add a New Feature" />
        <Columns>
            <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel" UniqueName="FeatureEdit">
                <HeaderStyle Width="5%" />
                <ItemStyle CssClass="editStyle"></ItemStyle>
            </telerik:GridEditCommandColumn>
            <telerik:GridBoundColumn HeaderText="Feature Name" DataField="FeatureName" UniqueName="TBFeature">
                <HeaderStyle Width="95%" />
                <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                    <RequiredFieldValidator ForeColor="Red" ErrorMessage=" Feature Name is required"></RequiredFieldValidator>
                    <ModelErrorMessage BackColor="Red" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
        </Columns>
        <DetailTables>
            <telerik:GridTableView
                runat="server"
                AllowSorting="True"
                AllowPaging="false"
                AutoGenerateColumns="False"
                GridLines="none"
                
                EnableViewState="true"
                ViewStateMode="Enabled"
                DataKeyNames="TextInputOutputKey"
                CommandItemDisplay="Top"
                Name="InputOutputs"
                InsertItemPageIndexAction="ShowItemOnFirstPage"
                >
                <CommandItemSettings
                    AddNewRecordText="Add a New Language" />
                <Columns>
                    <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel" UniqueName="EditCommandColumn2">
                        <HeaderStyle Width="5%" />
                        <ItemStyle CssClass="editStyle"></ItemStyle>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridTemplateColumn HeaderText="Language">
                        <HeaderStyle Width="8%" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "Language") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="DDLLanguage" AppendDataBoundItems="True" runat="server" Width="150px"></asp:DropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridCheckBoxColumn HeaderText="Text Input" DataField="Text_Input">
                        <HeaderStyle Width="5%" />
                    </telerik:GridCheckBoxColumn>
                    <telerik:GridCheckBoxColumn HeaderText="Text Output" DataField="Text_Output">
                        <HeaderStyle Width="5%" />
                    </telerik:GridCheckBoxColumn>
                    <telerik:GridBoundColumn HeaderText="Comments" DataField="Comments">
                        <HeaderStyle Width="45%" />
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ConfirmText="Delete this record?" CommandName="Delete" Text="Delete">
                        <HeaderStyle Width="5%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" CssClass="deleteStyle"></ItemStyle>
                    </telerik:GridButtonColumn>
                </Columns>
            </telerik:GridTableView>
        </DetailTables>
    </MasterTableView>
</telerik:RadGrid>
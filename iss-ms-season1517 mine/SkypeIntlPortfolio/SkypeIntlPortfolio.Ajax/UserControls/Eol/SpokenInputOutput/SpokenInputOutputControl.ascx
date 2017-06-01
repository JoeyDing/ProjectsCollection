<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpokenInputOutputControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.Eol.SpokenInputOutputControl" %>
<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="RadGridSpokenInputOutput">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGridSpokenInputOutput"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>

        <asp:Label ID="label_errorMsg" runat="server" ForeColor="Red" Visible="false" Text=""></asp:Label>

        <telerik:RadGrid
            ID="RadGridSpokenInputOutput"
            runat="server"
            AllowPaging="True"
            AllowCustomPaging="true"
            PageSize="10"
            AllowSorting="True"
            CellSpacing="0"
            GridLines="None"
            OnDetailTableDataBind="RadGridSpokenInputOutput_DetailTableDataBind"
            OnNeedDataSource="RadGridSpokenInputOutput_NeedDataSource"
            EnableViewState="true"
            OnUpdateCommand="RadGridSpokenInputOutput_UpdateCommand"
            ViewStateMode="Enabled"
            RenderMode="Auto"
            OnItemDataBound="RadGridSpokenInputOutput_ItemDataBound"
            OnInsertCommand="RadGridSpokenInputOutput_InsertCommand"
            OnItemCommand="RadGridSpokenInputOutput_ItemCommand"
            OnPageIndexChanged="RadGridSpokenInputOutput_PageIndexChanged"
            OnPageSizeChanged="RadGridSpokenInputOutput_PageSizeChanged"
            OnPreRender="RadGridSpokenInputOutput_PreRender">
            <MasterTableView AutoGenerateColumns="false" EnableGroupsExpandAll="true" EditMode="InPlace" ViewStateMode="Enabled" DataKeyNames="FeatureKey" Name="Features" CommandItemDisplay="Top" >
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
                        DataKeyNames="SpokenInputOutputKey"
                        CommandItemDisplay="Top"
                        Name="InputOutputs">
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
                            <telerik:GridCheckBoxColumn HeaderText="Spoken Input" DataField="Spoken_Input">
                                <HeaderStyle Width="5%" />
                            </telerik:GridCheckBoxColumn>
                            <telerik:GridCheckBoxColumn HeaderText="Spoken Output" DataField="Spoken_Output">
                                <HeaderStyle Width="5%" />
                            </telerik:GridCheckBoxColumn>
                            <telerik:GridBoundColumn HeaderText="Comments" DataField="Comments">
                                <HeaderStyle Width="50%" />
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
  
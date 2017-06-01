<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilesControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Files.FilesControl" %>

<script type="text/javascript">
    function GridCreated(sender, args) {
        $('.rgDataDiv').removeAttr('style');
        $('.rgDataDiv').attr('style', 'overflow-x: scroll;');
    }
</script>

<telerik:RadAjaxPanel ID="radajaxpanel_files" runat="server">
    <div>
        <telerik:RadGrid RenderMode="Auto" runat="server" ID="radgrid_files" AutoGenerateColumns="false"
            ClientSettings-EnableAlternatingItems="false"
            OnNeedDataSource="radgrid_files_NeedDataSource"
            OnInsertCommand="radgrid_files_InsertCommand"
            OnUpdateCommand="radgrid_files_UpdateCommand"
            OnDeleteCommand="radgrid_files_DeleteCommand"
            OnItemCommand="radgrid_files_ItemCommand"
            OnItemDataBound="radgrid_files_ItemDataBound">
            <MasterTableView DataKeyNames="FileKey" CommandItemDisplay="Top" EditFormSettings-EditColumn-AutoPostBackOnFilter="true">
                <Columns>
                    <telerik:GridEditCommandColumn></telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn HeaderText="File Name" DataField="File_Name">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage=" This field is required">
                            </RequiredFieldValidator>
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="File Type" DataField="File_Type">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage=" This field is required">
                            </RequiredFieldValidator>
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="LCG File Location" DataField="LCG_File_Location">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage=" This field is required">
                            </RequiredFieldValidator>
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Source File Location" DataField="Source_File_Location">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage=" This field is required">
                            </RequiredFieldValidator>
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>

                    <telerik:GridTemplateColumn HeaderText="Parser ID">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox_ParseID" runat="server" Text='<%# Bind("ParserID") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_ParserID" runat="server"
                                ControlToValidate="TextBox_ParseID"></asp:RequiredFieldValidator>
                            <requiredfieldvalidator forecolor="Red" errormessage=" This field is required" controltovalidate="TextBox_ParseID">
                            </requiredfieldvalidator>
                            <asp:CompareValidator runat="server" Operator="DataTypeCheck" Type="Integer"
                                ControlToValidate="TextBox_ParseID" ForeColor="Red" ErrorMessage=" Value must be a whole number" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("ParserID") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn HeaderText="Repo URL" DataField="RepoURL">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage=" This field is required">
                            </RequiredFieldValidator>
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn HeaderText="Repo Branch" DataField="repoBranch">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage=" This field is required">
                            </RequiredFieldValidator>
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn HeaderText="Repo Type" DataField="repoType">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage=" This field is required">
                            </RequiredFieldValidator>
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn HeaderText="Fabric Project" DataField="Fabric_Project">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage=" This field is required">
                            </RequiredFieldValidator>
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Fabric Tenant" UniqueName="templatecolumn_FabricTenant">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "SelectedFabricTenant")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:RadioButtonList ID="radiobuttonlist_fabricTenants" runat="server" RepeatDirection="Horizontal"
                                DataSource='<%#DataBinder.Eval(Container.DataItem, "FabricTenants")%>'
                                DataTextField="Text" DataValueField="Value">
                            </asp:RadioButtonList>
                            <div style="float: left; clear: none; margin: 6px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" Select one tenant" ForeColor="Red"
                                    ControlToValidate="radiobuttonlist_fabricTenants" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" ConfirmText="Delete this file ?" ImageUrl="~/Images/red_x_mark.png"></telerik:GridButtonColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings>
                <Scrolling AllowScroll="true" />
                <ClientEvents OnGridCreated="GridCreated" />
            </ClientSettings>
        </telerik:RadGrid>
        <telerik:RadButton runat="server" ID="RadButton_tab_file_SaveAndNextPage" Text="Next page" OnClick="RadButton_tab_file_SaveAndNextPage_Click"></telerik:RadButton>
    </div>
</telerik:RadAjaxPanel>
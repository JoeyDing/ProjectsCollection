<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UILanguageProductControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.Eol.UILanguageProduct.UILanguageProductControl" %>
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
        <telerik:AjaxSetting AjaxControlID="RadGrid_UILanguage">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGrid_UILanguage"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
<telerik:RadGrid ID="RadGrid_UILanguage"
    runat="server"
    AllowPaging="True"
    AllowCustomPaging="true"
    CellSpacing="0"
    OnNeedDataSource="RadGrid_UILanguage_NeedDataSource"
    OnItemDataBound="RadGrid_UILanguage_ItemDataBound"
    EnableViewState="true"
    ViewStateMode="Enabled"
    RenderMode="Lightweight"
    PageSize="20"
    OnDetailTableDataBind="RadGrid_UILanguage_DetailTableDataBind"
    EnableEmbeddedSkins="true"
    OnItemCommand="RadGrid_UILanguage_ItemCommand"
    Height="600px">

    <MasterTableView
        AllowPaging="true"
        AllowCustomPaging="true"
        AutoGenerateColumns="false"
        EnableGroupsExpandAll="true"
        ViewStateMode="Enabled"
        HierarchyDefaultExpanded="true"
        DataKeyNames="Name"
        CommandItemDisplay="Top"
        EditMode="PopUp"
        Name="masterTable"
        ShowHeader="false"
        runat="server"
        PageSize="20"
        BorderWidth="0"
        EnableViewState="true"
        Caption="">

        <EditFormSettings>
            <PopUpSettings Modal="true" ZIndex="2500" />
        </EditFormSettings>
        <%--<CommandItemSettings AddNewRecordText="Move Languges" />--%>

        <CommandItemTemplate>
            <telerik:RadButton runat="server" ID="movebtn" Text="Move Languages" CommandName="InitInsert" Font-Bold="true" ForeColor="Orange"></telerik:RadButton>
        </CommandItemTemplate>

        <Columns>
            <telerik:GridBoundColumn DataField="Name" HeaderText="">
                <HeaderStyle Font-Underline="true" />
            </telerik:GridBoundColumn>
        </Columns>
        <EditFormSettings EditFormType="Template" InsertCaption="Move Languages">
            <FormStyle CssClass="EditWindowColor" />
            <PopUpSettings ShowCaptionInEditForm="true" Width="800px" Height="150px" />
            <FormTableItemStyle />
            <FormTemplate>
                <div class="container-fluid" style="margin-top: 10px;">
                    <div class="row">
                        <div class="col-md-1">
                            <asp:Label runat="server" Text="From" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <telerik:RadComboBox runat="server" ID="radcombobox_From">
                            </telerik:RadComboBox>
                        </div>
                        <div class="col-md-1">
                            <asp:Label runat="server" Text="To" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <telerik:RadComboBox runat="server" ID="radcombobox_To">
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <br />
                    <br />
                    <br />
                    <div class="row">
                        <div class="col-md-6"></div>
                        <div class="col-md-6">
                            <telerik:RadButton runat="server" Text="Move" CommandName="PerformInsert" Width="40%" Style="margin-right: 20px;" CausesValidation="False"></telerik:RadButton>
                            <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                CommandName="Cancel" Width="40%">
                            </telerik:RadButton>
                        </div>
                    </div>
                </div>
            </FormTemplate>
        </EditFormSettings>
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
                CommandItemDisplay="Top"
                EditMode="PopUp"
                Name="ReleasedDetails"
                ShowHeader="true"
                DataKeyNames="UILanguagesKey"
                Caption="Released Languages">
                <CommandItemSettings AddNewRecordText="Add a New Released Language" />
                <Columns>
                    <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel" UniqueName="EditCommandColumn2">
                        <HeaderStyle Width="50px" />
                        <ItemStyle CssClass="editStyle"></ItemStyle>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridDateTimeColumn HeaderText="Language Name" DataField="LanguageIDPlusName">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn DataField="Release_Date" HeaderText="Release Date" DataFormatString="{0:M/d/yyyy}">
                        <HeaderStyle Width="200px" Font-Underline="true" />
                    </telerik:GridDateTimeColumn>

                    <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this Language?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                        <ItemStyle CssClass="gapToRightBorderRelease" />
                    </telerik:GridButtonColumn>
                </Columns>
                <EditFormSettings EditFormType="Template" InsertCaption="Released Languages">
                    <FormStyle CssClass="EditWindowColor" />
                    <PopUpSettings ShowCaptionInEditForm="true" />
                    <FormTableItemStyle />
                    <FormTemplate>
                        <div class="container-fluid" style="margin-top: 10px;">
                            <asp:Panel runat="server" ID="pannel_add" Visible="false">
                                <telerik:RadTabStrip runat="server" AutoPostBack="True" ID="RadTabStripUiLanguage" MultiPageID="radMultiPageUiLanguage" SelectedIndex="0" CausesValidation="False">
                                    <Tabs>
                                        <telerik:RadTab Text="Language Selection" Width="180px" Height="30px"></telerik:RadTab>
                                        <telerik:RadTab Text="Copy from another project" Width="250px" Height="30px"></telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                                <telerik:RadMultiPage runat="server" ID="radMultiPageUiLanguage" SelectedIndex="0" Width="100%" Height="100%" RenderSelectedPageOnly="true">

                                    <telerik:RadPageView runat="server" ID="radPageView_languageSeleciton">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label runat="server" Text="Release Date:"></asp:Label>
                                                <telerik:RadDatePicker ID="raddatepicker_ReleaseDate" AutoPostBack="false" runat="server" Width="100px">
                                                </telerik:RadDatePicker>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_ReleaseDate"
                                                    Text="Release Date cannot be empty." ForeColor="Red" EnableClientScript="true"
                                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">

                                            <div class="col-md-12">
                                                <asp:Label runat="server" ID="lable_NoLanguageChecked" Text=" Must choose at least a language" ForeColor="Orange"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <asp:Label runat="server" ID="Label1" Text="Languages" Font-Bold="true"></asp:Label>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label runat="server" ID="Label2" Text="Files" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <telerik:RadListBox runat="server" AutoPostBack="true" ID="radListBox_Language" OnClientCheckAllChecked="clientCheckAll" CheckBoxes="true" ShowCheckAll="true" CausesValidation="true" Width="100%" Height="300">
                                                </telerik:RadListBox>
                                            </div>
                                            <div class="col-md-6">
                                                <telerik:RadListBox runat="server" AutoPostBack="true" ID="radListBox_File" OnClientCheckAllChecked="clientCheckAll" CheckBoxes="true" ShowCheckAll="true" CausesValidation="true" Width="100%" Height="300">
                                                </telerik:RadListBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6"></div>
                                            <div class="col-md-6">
                                                <telerik:RadButton ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>' CausesValidation="true"
                                                    runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="40%" Style="margin-right: 20px;">
                                                </telerik:RadButton>
                                                <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                                    CommandName="Cancel" Width="40%">
                                                </telerik:RadButton>
                                            </div>
                                        </div>
                                    </telerik:RadPageView>
                                    <telerik:RadPageView runat="server" ID="radPageView_CopyFromProject">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label runat="server" ID="Label_projectName" Text="Project" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <telerik:RadComboBox runat="server" ID="radCombobox_Project" Width="300px"></telerik:RadComboBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6"></div>
                                            <div class="col-md-6">
                                                <telerik:RadButton ID="RadButton3" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>' CausesValidation="true"
                                                    runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="40%" Style="margin-right: 20px;">
                                                </telerik:RadButton>
                                                <telerik:RadButton ID="RadButton4" Text="Cancel" runat="server" CausesValidation="False"
                                                    CommandName="Cancel" Width="40%">
                                                </telerik:RadButton>
                                            </div>
                                        </div>
                                    </telerik:RadPageView>
                                </telerik:RadMultiPage>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pannel_edit" Visible="false">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:Label runat="server" Text="Release Date:"></asp:Label>
                                        <telerik:RadDatePicker ID="raddatepicker_ReleaseDate_Edit" AutoPostBack="false" runat="server" Width="100px">
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_ReleaseDate_Edit"
                                            Text="Release Date cannot be empty." ForeColor="Red" EnableClientScript="true"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                        <telerik:RadButton ID="RadButton1" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>' CausesValidation="true"
                                            runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="40%" Style="margin-right: 20px;">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="RadButton2" Text="Cancel" runat="server" CausesValidation="False"
                                            CommandName="Cancel" Width="40%">
                                        </telerik:RadButton>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <DetailTables>
                    <telerik:GridTableView
                        runat="server"
                        AllowPaging="true"
                        AllowCustomPaging="true"
                        PageSize="20"
                        AutoGenerateColumns="False"
                        BorderWidth="0"
                        EnableViewState="true"
                        ViewStateMode="Enabled"
                        Name="ReleasedDetails_Files"
                        ShowHeader="true"
                        DataKeyNames="FileKey"
                        Caption="">

                        <Columns>

                            <telerik:GridBoundColumn HeaderText="File Name" DataField="Source_File_Location">
                            </telerik:GridBoundColumn>

                            <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this File Under this Language?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                                <ItemStyle CssClass="gapToRightBorderRelease" />
                            </telerik:GridButtonColumn>
                        </Columns>
                    </telerik:GridTableView>
                </DetailTables>
            </telerik:GridTableView>
            <telerik:GridTableView
                runat="server"
                AllowCustomPaging="true"
                AllowPaging="true"
                AutoGenerateColumns="False"
                PageSize="20"
                BorderWidth="0"
                EnableViewState="true"
                ViewStateMode="Enabled"
                Name="PlannedDetails"
                CommandItemDisplay="Top"
                ShowHeader="true"
                EditMode="PopUp"
                DataKeyNames="UILanguagesKey"
                Caption="Planned Languages">
                <CommandItemSettings
                    AddNewRecordText="Add a New Planned Language" />

                <Columns>
                    <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel" UniqueName="EditCommandColumn2">
                        <HeaderStyle Width="50px" />
                        <ItemStyle CssClass="editStyle"></ItemStyle>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn HeaderText="Language Name" DataField="LanguageIDPlusName">
                    </telerik:GridBoundColumn>

                    <telerik:GridDateTimeColumn DataField="Release_Date" HeaderText="Release Date" DataFormatString="{0:M/d/yyyy}">
                        <HeaderStyle Width="200px" Font-Underline="true" />
                    </telerik:GridDateTimeColumn>
                    <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this Language?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                        <ItemStyle CssClass="gapToRightBorderRelease" />
                    </telerik:GridButtonColumn>
                </Columns>

                <EditFormSettings EditFormType="Template" InsertCaption="Planned Languages">
                    <FormStyle CssClass="EditWindowColor" />
                    <PopUpSettings ShowCaptionInEditForm="true" />
                    <FormTableItemStyle />
                    <FormTemplate>
                        <div class="container-fluid" style="margin-top: 10px;">
                            <asp:Panel runat="server" ID="pannel_add" Visible="false">
                                <telerik:RadTabStrip runat="server" AutoPostBack="True" ID="RadTabStripUiLanguage" MultiPageID="radMultiPageUiLanguage" SelectedIndex="0" CausesValidation="False">
                                    <Tabs>
                                        <telerik:RadTab Text="Language Selection" Width="180px" Height="30px"></telerik:RadTab>
                                        <telerik:RadTab Text="Copy from another project" Width="250px" Height="30px"></telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                                <telerik:RadMultiPage runat="server" ID="radMultiPageUiLanguage" SelectedIndex="0" Width="100%" Height="100%" RenderSelectedPageOnly="true">

                                    <telerik:RadPageView runat="server" ID="radPageView_languageSeleciton">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label runat="server" Text="Release Date:"></asp:Label>
                                                <telerik:RadDatePicker ID="raddatepicker_ReleaseDate" AutoPostBack="false" runat="server" Width="100px">
                                                </telerik:RadDatePicker>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_ReleaseDate"
                                                    Text="Release Date cannot be empty." ForeColor="Red" EnableClientScript="true"
                                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">

                                            <div class="col-md-12">
                                                <asp:Label runat="server" ID="lable_NoLanguageChecked" Text=" Must choose at least a language" ForeColor="Orange"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <asp:Label runat="server" ID="Label1" Text="Languages" Font-Bold="true"></asp:Label>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label runat="server" ID="Label2" Text="Files" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <telerik:RadListBox runat="server" AutoPostBack="true" ID="radListBox_Language" OnClientCheckAllChecked="clientCheckAll" CheckBoxes="true" ShowCheckAll="true" CausesValidation="true" Width="100%" Height="300">
                                                </telerik:RadListBox>
                                            </div>
                                            <div class="col-md-6">
                                                <telerik:RadListBox runat="server" AutoPostBack="true" ID="radListBox_File" OnClientCheckAllChecked="clientCheckAll" CheckBoxes="true" ShowCheckAll="true" CausesValidation="true" Width="100%" Height="300">
                                                </telerik:RadListBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6"></div>
                                            <div class="col-md-6">
                                                <telerik:RadButton ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>' CausesValidation="true"
                                                    runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="40%" Style="margin-right: 20px;">
                                                </telerik:RadButton>
                                                <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                                    CommandName="Cancel" Width="40%">
                                                </telerik:RadButton>
                                            </div>
                                        </div>
                                    </telerik:RadPageView>
                                    <telerik:RadPageView runat="server" ID="radPageView_CopyFromProject">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label runat="server" ID="Label_projectName" Text="Project" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <telerik:RadComboBox runat="server" ID="radCombobox_Project" Width="300px"></telerik:RadComboBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6"></div>
                                            <div class="col-md-6">
                                                <telerik:RadButton ID="RadButton3" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>' CausesValidation="true"
                                                    runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="40%" Style="margin-right: 20px;">
                                                </telerik:RadButton>
                                                <telerik:RadButton ID="RadButton4" Text="Cancel" runat="server" CausesValidation="False"
                                                    CommandName="Cancel" Width="40%">
                                                </telerik:RadButton>
                                            </div>
                                        </div>
                                    </telerik:RadPageView>
                                </telerik:RadMultiPage>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pannel_edit" Visible="false">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:Label runat="server" Text="Release Date:"></asp:Label>
                                        <telerik:RadDatePicker ID="raddatepicker_ReleaseDate_Edit" AutoPostBack="false" runat="server" Width="100px">
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_ReleaseDate_Edit"
                                            Text="Release Date cannot be empty." ForeColor="Red" EnableClientScript="true"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                        <telerik:RadButton ID="RadButton1" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>' CausesValidation="true"
                                            runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="40%" Style="margin-right: 20px;">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="RadButton2" Text="Cancel" runat="server" CausesValidation="False"
                                            CommandName="Cancel" Width="40%">
                                        </telerik:RadButton>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <DetailTables>
                    <telerik:GridTableView
                        runat="server"
                        AllowPaging="true"
                        AllowCustomPaging="true"
                        PageSize="20"
                        AutoGenerateColumns="False"
                        BorderWidth="0"
                        EnableViewState="true"
                        ViewStateMode="Enabled"
                        EditMode="PopUp"
                        Name="PlannedDetails_Files"
                        ShowHeader="true"
                        DataKeyNames="FileKey"
                        Caption="">

                        <Columns>

                            <telerik:GridBoundColumn HeaderText="File Name" DataField="Source_File_Location">
                            </telerik:GridBoundColumn>

                            <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this File Under this Language?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                                <ItemStyle CssClass="gapToRightBorderRelease" />
                            </telerik:GridButtonColumn>
                        </Columns>
                    </telerik:GridTableView>
                </DetailTables>
            </telerik:GridTableView>
            <telerik:GridTableView
                runat="server"
                AllowCustomPaging="true"
                AllowPaging="true"
                AutoGenerateColumns="False"
                PageSize="20"
                BorderWidth="0"
                EnableViewState="true"
                ViewStateMode="Enabled"
                Name="BlockedDetails"
                CommandItemDisplay="Top"
                ShowHeader="true"
                EditMode="PopUp"
                DataKeyNames="UILanguagesKey"
                Caption="Blocked Languages">
                <CommandItemSettings
                    AddNewRecordText="Add a New Blocked Language" />
                <Columns>
                    <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel" UniqueName="EditCommandColumn2">
                        <HeaderStyle Width="50px" />
                        <ItemStyle CssClass="editStyle"></ItemStyle>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn HeaderText="Language Name" DataField="LanguageIDPlusName">
                    </telerik:GridBoundColumn>

                    <telerik:GridDateTimeColumn DataField="Blocked_Reason" HeaderText="Blocked Reason" DataFormatString="{0:M/d/yyyy}">
                        <HeaderStyle Width="200px" Font-Underline="true" />
                    </telerik:GridDateTimeColumn>

                    <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this Language?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                        <ItemStyle CssClass="gapToRightBorderRelease" />
                    </telerik:GridButtonColumn>
                </Columns>

                <EditFormSettings EditFormType="Template" InsertCaption="Blocked Languages">
                    <FormStyle CssClass="EditWindowColor" />
                    <PopUpSettings ShowCaptionInEditForm="true" />
                    <FormTableItemStyle />
                    <FormTemplate>
                        <div class="container-fluid" style="margin-top: 10px;">
                            <asp:Panel runat="server" ID="pannel_add" Visible="false">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label runat="server" Text="Blocked Reason:"></asp:Label>
                                        <telerik:RadTextBox ID="radtextbox_BlockedReason" AutoPostBack="false" runat="server" Width="100px">
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="radtextbox_BlockedReason"
                                            Text="Blocked Reason cannot be empty." ForeColor="Red" EnableClientScript="true"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row">

                                    <div class="col-md-12">
                                        <asp:Label runat="server" ID="lable_NoLanguageChecked" Text=" Must choose at least a language" ForeColor="Orange"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:Label runat="server" ID="Label1" Text="Languages" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label runat="server" ID="Label2" Text="Files" Font-Bold="true"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <telerik:RadListBox runat="server" AutoPostBack="true" ID="radListBox_Language" OnClientCheckAllChecked="clientCheckAll" CheckBoxes="true" ShowCheckAll="true" CausesValidation="true" Width="100%" Height="300">
                                        </telerik:RadListBox>
                                    </div>
                                    <div class="col-md-6">
                                        <telerik:RadListBox runat="server" AutoPostBack="true" ID="radListBox_File" OnClientCheckAllChecked="clientCheckAll" CheckBoxes="true" ShowCheckAll="true" CausesValidation="true" Width="100%" Height="300">
                                        </telerik:RadListBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6"></div>
                                    <div class="col-md-6">
                                        <telerik:RadButton ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>' CausesValidation="true"
                                            runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="40%" Style="margin-right: 20px;">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                            CommandName="Cancel" Width="40%">
                                        </telerik:RadButton>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pannel_edit" Visible="false">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:Label runat="server" Text="Blocked Reason:"></asp:Label>
                                        <telerik:RadTextBox ID="radtextbox_BlockedReason_Edit" AutoPostBack="false" runat="server" Width="100px">
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="radtextbox_BlockedReason_Edit"
                                            Text="Blocked Reason cannot be empty." ForeColor="Red" EnableClientScript="true"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                        <telerik:RadButton ID="RadButton1" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>' CausesValidation="true"
                                            runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="40%" Style="margin-right: 20px;">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="RadButton2" Text="Cancel" runat="server" CausesValidation="False"
                                            CommandName="Cancel" Width="40%">
                                        </telerik:RadButton>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <DetailTables>
                    <telerik:GridTableView
                        runat="server"
                        AllowPaging="true"
                        AllowCustomPaging="true"
                        PageSize="20"
                        AutoGenerateColumns="False"
                        BorderWidth="0"
                        EnableViewState="true"
                        ViewStateMode="Enabled"
                        EditMode="PopUp"
                        Name="BlockedDetails_Files"
                        ShowHeader="true"
                        DataKeyNames="FileKey"
                        Caption="">

                        <Columns>

                            <telerik:GridBoundColumn HeaderText="File Name" DataField="Source_File_Location">
                            </telerik:GridBoundColumn>

                            <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this File Under this Language?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                                <ItemStyle CssClass="gapToRightBorderRelease" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <EditFormSettings EditFormType="Template" InsertCaption="Released Language">
                            <FormStyle CssClass="EditWindowColor" />
                            <PopUpSettings Width="500px" ShowCaptionInEditForm="true" />
                            <FormTableItemStyle />
                            <FormTemplate>
                                <div class="container-fluid" style="margin-top: 10px;">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label runat="server" Text="Blocked Reason:"></asp:Label>
                                            <telerik:RadTextBox ID="radtextbox_BlockedReason" AutoPostBack="false" runat="server" Width="100px">
                                            </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="radtextbox_BlockedReason"
                                                Text="Blocked Reason cannot be empty." ForeColor="Red" EnableClientScript="true"
                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-6">
                                            <telerik:RadButton ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>' CausesValidation="true"
                                                runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="40%" Style="margin-right: 20px;">
                                            </telerik:RadButton>
                                            <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                                CommandName="Cancel" Width="40%">
                                            </telerik:RadButton>
                                        </div>
                                    </div>
                            </FormTemplate>
                        </EditFormSettings>
                    </telerik:GridTableView>
                </DetailTables>
            </telerik:GridTableView>
        </DetailTables>
    </MasterTableView>
    <ClientSettings>
        <ClientEvents OnPopUpShowing="popUpShowing" />
    </ClientSettings>
</telerik:RadGrid>
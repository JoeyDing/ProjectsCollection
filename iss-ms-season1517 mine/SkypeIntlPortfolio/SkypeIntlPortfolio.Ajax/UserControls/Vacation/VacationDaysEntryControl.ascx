<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VacationDaysEntryControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.Vacation.VacationDaysEntryControl" %>

<style type="text/css">
    .RadGrid .rgEditPopup .rgHeader {
        background-color: rgb(49, 190, 243);
        color: white;
        height: 30px !important;
    }

    .centerPopUpModal {
        position: fixed;
        top: -100px;
        left: 10px;
        z-index: 2500;
    }
    .rgEditForm RadGrid_Metro rgEditPopup
    {
         position:relative;
    }

    .myclass ul li {
        margin-left: 20px !important;
    }

    .RadAutoCompleteBoxSpecial .racRemoveTokenLink {
        display: none;
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

<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy2" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="RadGridVacation">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGridVacation"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>

<telerik:RadGrid ID="RadGridVacation"
    runat="server"
    CellSpacing="0"
    OnNeedDataSource="RadGridVacation_NeedDataSource"
    OnItemCommand="RadGridVacation_ItemCommand"
    OnItemDataBound="RadGridVacation_ItemDataBound"
    EnableViewState="true"
    ViewStateMode="Enabled"
    RenderMode="Lightweight"
     Height="600px"
     EnableEmbeddedSkins="true"
     PagerStyle-AlwaysVisible="true"
    AllowCustomPaging="false"
    >
    <MasterTableView
        runat="server"
        AllowPaging="True"
        AllowCustomPaging="false"
        PagerStyle-AlwaysVisible="true"
        AutoGenerateColumns="false"
        EnableGroupsExpandAll="true"
        ViewStateMode="Enabled"
        HierarchyDefaultExpanded="true"
        Name="Vacation"
        ShowHeader="true"
        CommandItemDisplay="Top"
        EditMode="PopUp"
        Caption="">
        <EditFormSettings>
            <PopUpSettings Modal="true" ZIndex="2500"/>
        </EditFormSettings>
        <CommandItemSettings AddNewRecordText="Add a New Vacation"
            ShowRefreshButton="false" />
        <Columns>
            <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel" UniqueName="EditCommandColumn1">
                <HeaderStyle Width="50px" />
                <ItemStyle CssClass="editStyle"></ItemStyle>
            </telerik:GridEditCommandColumn>
            <telerik:GridBoundColumn DataField="VacationID" HeaderText="Vacation ID" Display="false">
                <HeaderStyle Width="200px" Font-Underline="true" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="VacationName" HeaderText="Vacation Name" Visible="true">
                <HeaderStyle Width="200px" Font-Underline="true" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="VacationDescription" HeaderText="Description" Visible="true">
                <HeaderStyle Width="200px" Font-Underline="true" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
            </telerik:GridBoundColumn>
            <telerik:GridDateTimeColumn DataField="VacationStartDate" HeaderText="From" DataFormatString="{0:M/d/yyyy}">
                <HeaderStyle Width="200px" Font-Underline="true" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
            </telerik:GridDateTimeColumn>
            <telerik:GridDateTimeColumn DataField="VacationEndDate" HeaderText="To" DataFormatString="{0:M/d/yyyy}">
                <HeaderStyle Width="200px" Font-Underline="true" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
            </telerik:GridDateTimeColumn>
            <telerik:GridBoundColumn DataField="ProductsAffected" HeaderText="Products Affected">
                <HeaderStyle Width="200px" Font-Underline="true" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PeopleAffected" Resizable="true" HeaderText="People Affected">
                <HeaderStyle Width="200px" Font-Underline="true" HorizontalAlign="Center" />
            </telerik:GridBoundColumn>
            <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this Vacation?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                <ItemStyle HorizontalAlign="Center" />
            </telerik:GridButtonColumn>
        </Columns>
        <%--
                    Edit Form for Vacation info
        --%>
        <EditFormSettings EditFormType="Template" InsertCaption="Add Vacation">
            <FormStyle CssClass="EditWindowColor" />
            <PopUpSettings Width="800px" ShowCaptionInEditForm="true" Modal="true" ZIndex="2500" />
            <FormTemplate>
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-4" style="width: 25%">
                            <asp:Label ID="Label7" runat="server" Text="Vacation Name:"></asp:Label>
                        </div>
                        <div class="col-md-4" style="width: 35%">
                            <telerik:RadTextBox ID="radtextbox_VacationName" runat="server" Width="220px"></telerik:RadTextBox>
                        </div>
                        <div class="col-md-4" style="width: 35%">
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="radtextbox_VacationName"
                                Text="Vacation name cannot be empty." ForeColor="Red"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4" style="width: 25%">
                            <asp:Label ID="Label1" runat="server" Text="Vacation Description:"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <telerik:RadTextBox ID="radtextbox_VacationDescription" runat="server" Width="220px"></telerik:RadTextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4" style="width: 25%">
                            <asp:Label ID="Label3" runat="server" Text="Vacation StartDate:"></asp:Label>
                        </div>
                        <div class="col-md-4" style="width: 35%">
                            <telerik:RadDatePicker ID="raddatepicker_VacationStartDate" runat="server" Width="100px"></telerik:RadDatePicker>
                        </div>
                        <div class="col-md-4" style="width: 35%">
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_VacationStartDate"
                                Text="Vacation start date cannot be empty." ForeColor="Red"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4" style="width: 25%">
                            <asp:Label ID="Label4" runat="server" Text="Vacation EndDate:"></asp:Label>
                        </div>
                        <div class="col-md-4" style="width: 35%">
                            <telerik:RadDatePicker ID="raddatepicker_VacationEndDate" runat="server" Width="100px"></telerik:RadDatePicker>
                        </div>
                        <div class="col-md-4" style="width: 35%">
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_VacationEndDate"
                                Text="Vacation end date cannot be empty." ForeColor="Red"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:CompareValidator ID="dateCompareValidatorForVacationDate" runat="server"
                                ControlToValidate="raddatepicker_VacationEndDate" ControlToCompare="raddatepicker_VacationStartDate"
                                Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                                ErrorMessage="End Date must be equal or greater than Start Date - please correct it." Display="Dynamic">
                            </asp:CompareValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4" style="width: 25%">Select the affected products:</div>
                        <div class="col-md-4" style="width: 35%">
                            <asp:RadioButtonList ID="radioButtonList_UIVacationCategory" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" EnableClientScript="true" OnSelectedIndexChanged="radioButtonList_UIVacationCategory_SelectedIndexChanged">
                                <asp:ListItem Text="One By One" Value="1"></asp:ListItem>
                                <asp:ListItem Text="By Location" Value="2"></asp:ListItem>
                                <asp:ListItem Text="All" Value="3"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Panel runat="server" ID="panel_checkProductsByLoc" Visible="false">
                                <div class="col-md-4" style="width: 35%">
                                </div>
                                <div class="col-md-4" style="width: 40%; margin-left: -85px;">

                                    <telerik:RadListBox runat="server" ID="radListBox_Location" CheckBoxes="true">
                                        <Items>
                                            <telerik:RadListBoxItem Text="Tallinn" Value="2" />
                                            <telerik:RadListBoxItem Text="Redmond" Value="1" />
                                            <telerik:RadListBoxItem Text="Beijing" Value="3" />
                                        </Items>
                                    </telerik:RadListBox>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Panel runat="server" ID="panel_checkProductsOneByOne" Visible="false">
                                <div class="row">
                                    <div class="col-md-4" style="width: 25%">
                                    </div>
                                    <div class="col-md-4">
                                        <telerik:RadAutoCompleteBox runat="server" AutoPostBack="true" ID="RadAutoCompleteBox_existingProducts" TextSettings-SelectionMode="Single" OnEntryAdded="RadAutoCompleteBox_existingProducts_EntryAdded" OnEntryRemoved="RadAutoCompleteBox_existingProducts_EntryRemoved" EmptyMessage="Please type here" InputType="Token" Width="300" DropDownWidth="180px">
                                            <WebServiceSettings Method="search_existingProductNames" Path="UtilsProductsInfo.aspx" />
                                        </telerik:RadAutoCompleteBox>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4" style="width: 25%">
                            <asp:Label runat="server" ID="label_affectedPeople">
                                Select the affected people:
                            </asp:Label>
                        </div>
                        <div class="col-md-4">
                            <telerik:RadAutoCompleteBox runat="server" AutoPostBack="true" ID="RadAutoCompleteBox_existingPeopleNames" TextSettings-SelectionMode="Single" OnEntryAdded="RadAutoCompleteBox_existingPeopleNames_EntryAdded" EmptyMessage="Please type here" InputType="Token" Width="580" DropDownWidth="180px" Delimiter="<br />" CssClass="RadAutoCompleteBoxSpecial">
                                <WebServiceSettings Method="search_existingPeople" Path="UtilsProductsInfo.aspx" />
                            </telerik:RadAutoCompleteBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6"></div>
                        <div class="col-md-3" style="margin-top: 10px;">
                            <telerik:RadButton ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>'
                                runat="server" CausesValidation="true" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="70%" Style="margin-left: 50px; margin-right: 20px;">
                            </telerik:RadButton>
                        </div>
                        <div class="col-md-3" style="margin-top: 10px;">
                            <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                CommandName="Cancel" Width="70%" Style="padding-right: 10px;">
                            </telerik:RadButton>
                        </div>
                    </div>
                </div>
            </FormTemplate>
        </EditFormSettings>
    </MasterTableView>
    <ClientSettings>
        <ClientEvents OnPopUpShowing="popUpShowing" />
    </ClientSettings>
</telerik:RadGrid>

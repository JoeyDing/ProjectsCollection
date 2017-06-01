<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.Schedule.ScheduleControl" %>
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
        top: 50px;
        left: 420px;
        z-index: 2500;
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
        height: auto;
        margin-top: 50px;
    }

    .RadGrid_Metro .rgEditForm {
        margin-left: -300px;
        margin-top: -270px;
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
        <telerik:AjaxSetting AjaxControlID="RadGridSchedule">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGridSchedule"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>

<telerik:RadGrid ID="RadGridSchedule"
    runat="server"
    AllowPaging="True"
    AllowCustomPaging="true"
    CellSpacing="0"
    OnNeedDataSource="RadGridSchedule_NeedDataSource"
    OnItemDataBound="RadGridSchedule_ItemDataBound"
    EnableViewState="true"
    ViewStateMode="Enabled"
    RenderMode="Lightweight"
    PageSize="10"
    OnDetailTableDataBind="RadGridSchedule_DetailTableDataBind"
    EnableEmbeddedSkins="true"
    OnItemCommand="RadGridSchedule_ItemCommand"
    Height="600px"
    OnPageIndexChanged="RadGridSchedule_PageIndexChanged">
    <MasterTableView
        AllowPaging="True"
        AllowCustomPaging="true"
        AutoGenerateColumns="false"
        EnableGroupsExpandAll="true"
        ViewStateMode="Enabled"
        HierarchyDefaultExpanded="true"
        DataKeyNames="ProductID"
        Name="Product"
        ShowHeader="false"
        Caption="Product">
        <EditFormSettings>
            <PopUpSettings Modal="true" ZIndex="2500" />
        </EditFormSettings>
        <Columns>
            <telerik:GridBoundColumn DataField="ProductName" HeaderText="Product Name" Visible="true">
                <HeaderStyle Width="200px" Font-Underline="true" />
            </telerik:GridBoundColumn>
            <telerik:GridTemplateColumn>
                <ItemTemplate>
                    <asp:HyperLink
                        ID="HyperLinkToLineOfSight"
                        runat="server" Target="_blank"
                        Text="Go to 'Line of Sight' page for this project"
                        ForeColor="#336699"
                        NavigateUrl='<%# "~/Pages/ReportingSystem.aspx?Tab=LineofSight&Products=" + Eval("ProductID")%>'
                        Style="margin-left: 165px; font-weight: bold; font-size: medium" Font-Italic="true" Font-Underline="true">
                    </asp:HyperLink>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
        </Columns>
        <DetailTables>
            <%--

                Release Table

            --%>
            <telerik:GridTableView
                AllowPaging="True"
                AllowCustomPaging="true"
                runat="server"
                EnableGroupsExpandAll="true"
                PageSize="10"
                AutoGenerateColumns="false"
                BorderWidth="0"
                EnableViewState="true"
                ViewStateMode="Enabled"
                DataKeyNames="VSO_ID"
                Name="ReleaseDetails"
                CommandItemDisplay="Top"
                ShowHeader="true"
                EditMode="PopUp"
                Caption="Releases">

                <CommandItemSettings
                    AddNewRecordText="Add a New Release" />

                <Columns>
                    <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel" UniqueName="EditCommandColumn2">
                        <HeaderStyle Width="50px" />
                        <ItemStyle CssClass="editStyle"></ItemStyle>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="VSO_Title" HeaderText="Release Name">
                        <HeaderStyle Width="200px" Font-Underline="true" />
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="VSO Epic">
                        <HeaderStyle Width="200px" Font-Underline="true" />
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="lnkVSO" Target="_blank" NavigateUrl='<%#Eval("VSO_Url") %>' ForeColor="#336699"><%#Eval("VSO_ID") %></asp:HyperLink>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridDateTimeColumn DataField="VSO_LocStartDate" HeaderText="From" DataFormatString="{0:M/d/yyyy}">
                        <HeaderStyle Width="200px" Font-Underline="true" />
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn DataField="VSO_DueDate" HeaderText="To" DataFormatString="{0:M/d/yyyy}">
                        <HeaderStyle Width="200px" Font-Underline="true" />
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="VSO_Tags" HeaderText="Custom Tags">
                        <HeaderStyle Width="200px" Font-Underline="true" />
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this Release?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                        <ItemStyle CssClass="gapToRightBorderRelease" />
                    </telerik:GridButtonColumn>
                </Columns>
                <%--

                    Edit Form for Release

                --%>
                <EditFormSettings EditFormType="Template">
                    <FormStyle CssClass="EditWindowColor" />
                    <PopUpSettings Width="1200px" ShowCaptionInEditForm="true" />
                    <FormTemplate>
                        <div class="container-fluid">
                            <asp:Panel runat="server" Style="position: relative;" Height="500px" Width="100%">
                                <div class="row" style="margin-top: 20px;">
                                    <div class="col-md-1" style="width: 2%;"></div>
                                    <div class="col-md-3" style="width: 40%;">
                                        <asp:Label ID="Label7" runat="server" Text="Release Name:"></asp:Label>
                                        <telerik:RadTextBox ID="radtextbox_ReleaseName" runat="server" Width="220px"></telerik:RadTextBox>
                                        <asp:HiddenField ID="hdReleaseKey" runat="server" Value='<%#Eval("VSO_ID") %>' />
                                    </div>
                                    <div class="col-md-5" style="width: 35%; margin-right: -40px; margin-left: -150px;">
                                        <asp:Label ID="Label3" runat="server" Text="From:" Style="margin-left: 48px;"></asp:Label>
                                        <telerik:RadDatePicker ID="raddatepicker_ReleaseStartDate" runat="server" Width="100px" ClientEvents-OnDateSelected='<%# "function(sender,args){onDateChange(\""+ Container.FindControl("raddatepicker_ReleaseStartDate").ClientID + "\",\"" +Container.FindControl("raddatepicker_ReleaseEndDate").ClientID+"\");}" %>'></telerik:RadDatePicker>
                                        <asp:Label ID="Label4" runat="server" Text="To:"></asp:Label>
                                        <telerik:RadDatePicker ID="raddatepicker_ReleaseEndDate" runat="server" Width="100px"></telerik:RadDatePicker>
                                    </div>
                                    <div class="col-md-1" style="width: 19%;">
                                        <asp:Label runat="server" ID="CopyFromPreReleaseLabel" Text="Copy from a previous release:" Font-Bold="true" Font-Italic="true"></asp:Label>
                                    </div>
                                    <div class="col-md-2" style="width: 15%; margin-left: -50px;">
                                        <telerik:RadAutoCompleteBox runat="server" AutoPostBack="true" ID="RadAutoCompleteBox_existingRelease" TextSettings-SelectionMode="Single" EmptyMessage="Please type here" InputType="Token" Width="200" DropDownWidth="180px" OnEntryAdded="RadAutoCompleteBox_existingRelease_EntryAdded">
                                            <WebServiceSettings Method="search_existingReleasesNames" Path="UtilsReleasesInfo.aspx" />
                                        </telerik:RadAutoCompleteBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-1" style="width: 15%;"></div>
                                    <div class="col-md-3" style="width: 30%; margin-right: -65px; margin-left: -50px;">
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="radtextbox_ReleaseName"
                                            Text="Release name cannot be empty." ForeColor="Red" ValidationGroup="add_release_group"
                                            Display="Dynamic" EnableClientScript="true">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-5" style="width: 38%; margin-right: 40px;">
                                        <div class="container-fluid">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_ReleaseStartDate"
                                                        Text="Release Start Date cannot be empty." ForeColor="Red" EnableClientScript="true" ValidationGroup="add_release_group"
                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_ReleaseEndDate"
                                                        Text="Release End Date cannot be empty." ForeColor="Red" EnableClientScript="true" ValidationGroup="add_release_group"
                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:CompareValidator ID="dateCompareValidatorForReleaseWindow" runat="server"
                                                        ControlToValidate="raddatepicker_ReleaseEndDate" ControlToCompare="raddatepicker_ReleaseStartDate"
                                                        Operator="GreaterThanEqual" Type="Date" ForeColor="Red" ValidationGroup="add_release_group"
                                                        ErrorMessage="End Date must be equal or greater than Start Date - please correct it." Display="Dynamic">
                                                    </asp:CompareValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2" style="margin-left: 30px;">
                                        <asp:Label ID="Label_ReleaseType" runat="server" Text="Release Type:"></asp:Label>
                                    </div>
                                    <div class="col-md-8" style="margin-left: -70px;">
                                        <asp:RadioButtonList ID="radioButtonList_ReleaseType" runat="server"
                                            RepeatDirection="Vertical" EnableClientScript="true" AutoPostBack="false">
                                            <asp:ListItem Text="<b>SLA1</b>: New/Updated strings in existing file (weekly or bi-weekly releases)" Value="SLA1"></asp:ListItem>
                                            <asp:ListItem Text="<b>SLA2</b>: New feature in already localized product; New resource file (bi-weekly or longer releases)" Value="SLA2"></asp:ListItem>
                                            <asp:ListItem Text="<b>SLA3</b>: New product, new platform, or previously un-localized product" Value="SLA3"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="row">
                                    <asp:Panel runat="server" ScrollBars="Auto" Style="position: absolute; max-height: 300px; overflow-x: hidden; overflow-y: auto;" Width="100%">
                                        <div class="col-md-12">
                                            <telerik:RadPanelBar ID="radPanelBar_release_miletones" runat="server" Width="100%">
                                                <ItemTemplate>
                                                    <div class="container-fluid" style="margin-left: -70px;">
                                                        <div class="row">
                                                            <div class="col-md-1" style="width: 5%;">
                                                                <telerik:RadTextBox ID="radTextBox_release_releaseName" runat="server" Width="220px" Display="false"></telerik:RadTextBox>
                                                            </div>
                                                            <div class="col-md-2" style="width: 30%; margin-right: -15px; margin-left: 26px">
                                                                <asp:Label ID="label_milestoneCategoryInWindow" Text="Milestone Category:" runat="server"></asp:Label>
                                                                <telerik:RadComboBox ID="radcombobox_release_milestoenCategories" RenderMode="Auto"
                                                                    runat="server" AutoPostBack="true" CausesValidation="false" Width="130px" AllowCustomText="true" Text="Enter Custom Category" Font-Italic="true" Font-Size="X-Small"
                                                                    OnSelectedIndexChanged="radcombobox_release_milestoenCategories_SelectedIndexChanged"
                                                                    OnClientSelectedIndexChanged='<%# "function(combobox,args){categoryNameChanged(\""+ Container.FindControl("radcombobox_release_milestoenCategories").ClientID + "\",\"" +Container.FindControl("radTextBox_release_mileStoneName").ClientID+"\",\"" +Container.FindControl("radTextBox_release_releaseName").ClientID+"\");}" %>'>
                                                                </telerik:RadComboBox>
                                                            </div>
                                                            <div class="col-md-1" style="width: 30%; margin-right: -15px;">
                                                                <asp:Label ID="label_milestoneNameInWindow" Text="Name:" runat="server"></asp:Label>
                                                                <telerik:RadTextBox ID="radTextBox_release_mileStoneName" runat="server" Width="220px"></telerik:RadTextBox>
                                                            </div>
                                                            <div class="col-md-2" style="width: 15%;">
                                                                <asp:Label runat="server" Text="From:"></asp:Label>
                                                                <telerik:RadDatePicker ID="raddatepicker_release_milestoneFrom" AutoPostBack="false" runat="server" Width="100px"
                                                                    ClientEvents-OnDateSelected='<%# "function(button, args){milestoneDateSelected(\""+Container.FindControl("raddatepicker_milestone_from_InWindow").ClientID + "\",\"" + Container.FindControl("raddatepicker_milestone_to_InWindow").ClientID+ "\");}" %>'>
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                            <div class="col-md-2" style="width: 15%;">
                                                                <asp:Label runat="server" Text="To:"></asp:Label>
                                                                <telerik:RadDatePicker ID="raddatepicker_release_milestoneTo" AutoPostBack="false" runat="server" Width="100px">
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                            <div class="col-md-1" style="width: 5%;">
                                                                <telerik:RadButton runat="server" Height="22px" Width="22px" ID="radButton_release_deleteMilestone" CausesValidation="false" OnClick="radButton_release_deleteMilestone_Click">
                                                                    <Image ImageUrl="~/Images/red_x_mark.png" />
                                                                </telerik:RadButton>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-2">
                                                            </div>
                                                            <div class="col-md-1" style="width: 20%; margin-left: 260px;">
                                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator_MilestoneNameInWindow" ControlToValidate="radTextBox_release_mileStoneName"
                                                                    Text="Milestone name cannot be empty." ForeColor="Red"
                                                                    Display="Dynamic" EnableClientScript="true" ValidationGroup="add_release_group"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-6" style="width: 30%; margin-left: 40px;">
                                                                <div class="container-fluid">
                                                                    <div class="row">
                                                                        <div class="col-md-12" style="margin-left: 40px;">
                                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_release_milestoneFrom"
                                                                                Text="Milestone Start Date cannot be empty." ForeColor="Red"
                                                                                Display="Dynamic" EnableClientScript="true" ValidationGroup="add_release_group"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12" style="margin-left: 40px;">
                                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_release_milestoneTo"
                                                                                Text="Milestone End Date cannot be empty." ForeColor="Red"
                                                                                Display="Dynamic" EnableClientScript="true" ValidationGroup="add_release_group"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <asp:CompareValidator ID="dateCompareValidatorForMilestoneInAddReleaseWindow" runat="server"
                                                                                ControlToValidate="raddatepicker_release_milestoneTo" ControlToCompare="raddatepicker_release_milestoneFrom"
                                                                                Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                                                                                ErrorMessage="End Date must be equal or greater than Start Date - please correct dates." Display="Dynamic"
                                                                                EnableClientScript="true" ValidationGroup="add_release_group">
                                                                            </asp:CompareValidator>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2" style="width: 2%;">
                                                            </div>
                                                            <div class="col-md-1">
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-1">
                                                            </div>
                                                            <div class="col-md-11">
                                                                <telerik:RadPanelBar runat="server" ID="radPanelBar_release_eSpecs_child" Width="100%">
                                                                    <Items>
                                                                        <telerik:RadPanelItem Expanded="false">
                                                                            <HeaderTemplate>
                                                                                <a class="rpExpandable" style="float: left">
                                                                                    <span class="rpExpandHandle"></span>
                                                                                </a>
                                                                                <asp:Label runat="server" Text="VSO eSpecs  "></asp:Label>
                                                                                <asp:Label runat="server" Text="(Select the VSO eSpecs you want to create for this milestone)" Font-Italic="true"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ContentTemplate>
                                                                                <telerik:RadListBox runat="server" CssClass="labelPosition" AutoPostBack="true" ID="radListBoxeSpecs_release_milestone" CausesValidation="false" Width="100%" BorderColor="Transparent" OnClientSelectedIndexChanging="OnClientSelectedIndexChanging" CheckBoxes="true">
                                                                                    <ItemTemplate>
                                                                                        <div class="container-fluid">
                                                                                            <div class="row">
                                                                                                <div class="col-md-9" style="width: 75%; margin-left: 25px;">
                                                                                                    <telerik:RadTextBox runat="server" ID="radTextBox_eSpecName" Width="90%">
                                                                                                    </telerik:RadTextBox>
                                                                                                </div>
                                                                                                <div class="col-md-3" style="width: 20%;">
                                                                                                    <asp:Label runat="server" ID="label_eSpecsEstimate" Text="Estimate (Points) " Width="70%"></asp:Label>
                                                                                                    <telerik:RadTextBox runat="server" ID="radTextBox_eSpecEstimate" Text="" Width="25%">
                                                                                                    </telerik:RadTextBox>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </telerik:RadListBox>
                                                                            </ContentTemplate>
                                                                        </telerik:RadPanelItem>
                                                                    </Items>
                                                                </telerik:RadPanelBar>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </telerik:RadPanelBar>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="row" style="margin-top: 300px;">
                                </div>
                                <div class="row">
                                    <div class="col-md-7" style="margin-top: 10px; margin-left: 30px;">
                                        <telerik:RadButton ID="radButton_release_addMultipleMilestones" runat="server" Height="26px" Text="Add New Milestone" CausesValidation="false" OnClick="radButton_release_addMultipleMilestones_Click">
                                            <Icon PrimaryIconUrl="~/Images/blue_plus.png" PrimaryIconCssClass="iconPostion" PrimaryIconLeft="10" PrimaryIconTop="3"></Icon>
                                        </telerik:RadButton>
                                    </div>
                                    <div class="col-md-5">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6"></div>
                                    <div class="col-md-3" style="margin-top: 10px;">
                                        <telerik:RadButton ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>'
                                            runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="70%" Style="margin-left: 50px; margin-right: 20px;" ValidationGroup="add_release_group">
                                        </telerik:RadButton>
                                    </div>
                                    <div class="col-md-3" style="margin-top: 10px;">
                                        <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                            CommandName="Cancel" Width="70%" Style="padding-right: 10px;">
                                        </telerik:RadButton>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <DetailTables>
                    <%--

                    Milestone Table

                    --%>
                    <telerik:GridTableView
                        runat="server"
                        AllowPaging="true"
                        AllowCustomPaging="true"
                        PageSize="10"
                        AutoGenerateColumns="False"
                        BorderWidth="0"
                        EnableViewState="true"
                        ViewStateMode="Enabled"
                        Name="MilestoneDetails"
                        CommandItemDisplay="Top"
                        ShowHeader="true"
                        EditMode="PopUp"
                        DataKeyNames="MilestoneKey"
                        Caption="Milestones">

                        <CommandItemSettings
                            AddNewRecordText="Add a New Milestone" />

                        <Columns>
                            <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel" UniqueName="EditCommandColumn2">
                                <HeaderStyle Width="50px" />
                                <ItemStyle CssClass="editStyle"></ItemStyle>
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn HeaderText="Milestone Name" DataField="Milestone_Name">
                                <HeaderStyle Width="200px" Font-Underline="true" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="VSO Query">
                                <HeaderStyle Width="200px" Font-Underline="true" />
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="lnkVSO" Target="_blank" NavigateUrl='<%#Eval("Vso_Web_Url") %>' ForeColor="#336699"><%#Eval("Vso_Api_Url") %></asp:HyperLink>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Category" DataField="Milestone_Category_Name">
                                <HeaderStyle Width="8%" Font-Underline="true" />
                            </telerik:GridBoundColumn>
                            <telerik:GridDateTimeColumn HeaderText="From" DataField="Milestone_Start_Date" DataFormatString="{0:M/d/yyyy}">
                                <HeaderStyle Width="200px" Font-Underline="true" />
                                <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                                    <RequiredFieldValidator ForeColor="Red" ErrorMessage=" Milestone Start Date is required"></RequiredFieldValidator>
                                    <ModelErrorMessage BackColor="Red" />
                                </ColumnValidationSettings>
                            </telerik:GridDateTimeColumn>
                            <telerik:GridDateTimeColumn HeaderText="To" DataField="Milestone_End_Date" DataFormatString="{0:M/d/yyyy}">
                                <HeaderStyle Width="200px" Font-Underline="true" />
                            </telerik:GridDateTimeColumn>
                            <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this Milestone?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                                <ItemStyle CssClass="gapToRightBorderMilestone" />
                            </telerik:GridButtonColumn>
                        </Columns>

                        <%--

                            Edit Form for Milestone

                        --%>
                        <EditFormSettings EditFormType="Template">
                            <FormStyle CssClass="EditWindowColor" />
                            <PopUpSettings Width="1050px" ShowCaptionInEditForm="true" />
                            <FormTemplate>
                                <asp:Panel runat="server" Style="position: relative" Height="500px" Width="100%">
                                    <div class="container-fluid" style="margin-top: 10px;">
                                        <div class="row">
                                            <div class="col-md-2" style="width: 27%; margin-right: -46px;">
                                                <asp:Label runat="server" Text="Milestone Category:"></asp:Label>
                                                <telerik:RadComboBox ID="RadComboBox_MilestoneCategory_Milestone"
                                                    AutoPostBack="true" CausesValidation="true" Width="130px"
                                                    runat="server" AllowCustomText="true" Text="Enter Custom Category"
                                                    OnSelectedIndexChanged="RadComboBox_MilestoneCategory_Milestone_SelectedIndexChanged"
                                                    OnClientSelectedIndexChanged='<%# "function(combobox,args){categoryNameChanged(\""+ Container.FindControl("RadComboBox_MilestoneCategory_Milestone").ClientID + "\",\"" +Container.FindControl("radTextBox_mileStoneName").ClientID+"\",\"" +Container.FindControl("radTextBox_releaseName").ClientID+"\");}" %>' />
                                            </div>
                                            <div class="col-md-2" style="width: 30%; margin-right: -10px; margin-left: 50px">
                                                <asp:Label ID="Label8" Text="Name:" runat="server"></asp:Label>
                                                <telerik:RadTextBox ID="radTextBox_mileStoneName" runat="server" Width="220px"></telerik:RadTextBox>
                                            </div>
                                            <div class="col-md-2" style="width: 20%; margin-right: -45px;">
                                                <asp:Label runat="server" Text="From:"></asp:Label>
                                                <telerik:RadDatePicker ID="raddatepicker_MilestoneStartDate" AutoPostBack="false" runat="server" Width="100px" >
                                                </telerik:RadDatePicker>
                                            </div>
                                            <div class="col-md-2" style="width: 20%; margin-right: -45px;">
                                                <asp:Label runat="server" Text="To:"></asp:Label>
                                                <telerik:RadDatePicker ID="raddatepicker_MilestoneEndDate" AutoPostBack="false" runat="server" Width="100px" Enabled="true">
                                                </telerik:RadDatePicker>
                                            </div>
                                            <div class="col-md-1">
                                                <telerik:RadTextBox ID="radTextBox_releaseName" runat="server" Width="220px" Display="false"></telerik:RadTextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2" style="width: 28%; margin-right: -10px; margin-left: 50px">
                                            </div>
                                            <div class="col-md-1" style="width: 30%; margin-right: -25px;">
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_mileStoneName"
                                                    Text="Milestone name cannot be empty." ForeColor="Red" ValidationGroup="add_milestone_group"
                                                    Display="Dynamic" EnableClientScript="true">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-6" style="width: 40%; margin-right: -45px;">
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_MilestoneStartDate"
                                                                Text="Milestone Start Date cannot be empty." ForeColor="Red" EnableClientScript="true" ValidationGroup="add_milestone_group"
                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_MilestoneEndDate"
                                                                Text="Milestone End Date cannot be empty." ForeColor="Red" EnableClientScript="true" ValidationGroup="add_milestone_group"
                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12" style="margin-left: -15px;">
                                                            <asp:CompareValidator ID="dateCompareValidatorForTopMilestoneInAddMilestoneWindow" runat="server"
                                                                ControlToValidate="raddatepicker_MilestoneEndDate" ControlToCompare="raddatepicker_MilestoneStartDate"
                                                                Operator="GreaterThanEqual" Type="Date" ForeColor="Red" EnableClientScript="true"
                                                                ErrorMessage="End Date must be equal or greater than Start Date - please correct it." Display="Dynamic" ValidationGroup="add_milestone_group">
                                                            </asp:CompareValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-1" style="width: 5%; margin-right: -30px;">
                                            </div>
                                            <div class="col-md-1">
                                            </div>
                                        </div>
                                        <asp:Panel runat="server" ScrollBars="Vertical" Style="max-height: 300px; position: absolute; overflow-y: auto; overflow-x: hidden" Width="95%">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <telerik:RadPanelBar runat="server" ID="radPanelBar_milestone_eSpecs_child" Style="margin-left: 15px;" Width="90%">
                                                        <Items>
                                                            <telerik:RadPanelItem Expanded="true">
                                                                <HeaderTemplate>
                                                                    <a class="rpExpandable" style="float: left">
                                                                        <span class="rpExpandHandle"></span>
                                                                    </a>
                                                                    <asp:Label runat="server" Text="VSO eSpecs  "></asp:Label>
                                                                    <asp:Label runat="server" Text="(Select the VSO eSpecs you want to create for this milestone)" Font-Italic="true"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ContentTemplate>
                                                                    <telerik:RadListBox runat="server" AutoPostBack="false" CssClass="labelPosition" ID="radListBoxeSpecs" CausesValidation="false" Visible="false" Width="100%" OnClientSelectedIndexChanging="OnClientSelectedIndexChanging" BorderColor="Transparent" CheckBoxes="true">
                                                                        <ItemTemplate>
                                                                            <div class="container-fluid">
                                                                                <div class="row">
                                                                                    <div class="col-md-9" style="width: 75%; margin-left: 25px;">
                                                                                        <telerik:RadTextBox runat="server" ID="radTextBox_eSpecName" Width="90%">
                                                                                        </telerik:RadTextBox>
                                                                                    </div>
                                                                                    <div class="col-md-3" style="width: 20%;">
                                                                                        <asp:Label runat="server" ID="label_eSpecsEstimate" Text="Estimate (Points) " Width="70%"></asp:Label>
                                                                                        <telerik:RadTextBox runat="server" ID="radTextBox_eSpecEstimate" AutoPostBack="false" Text="" Width="25%">
                                                                                        </telerik:RadTextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </telerik:RadListBox>
                                                                </ContentTemplate>
                                                            </telerik:RadPanelItem>
                                                        </Items>
                                                    </telerik:RadPanelBar>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <telerik:RadPanelBar ID="radPanelBar_extra_miletones" runat="server" Width="100%">
                                                        <ItemTemplate>
                                                            <div class="container-fluid">
                                                                <div class="row">
                                                                    <div class="col-md-2" style="width: 30%; margin-right: -10px; margin-left: -26px">
                                                                        <asp:Label ID="Label9" Text="Milestone Category:" runat="server"></asp:Label>
                                                                        <telerik:RadComboBox ID="radcombobox_extra_MilestoneCategoryName"
                                                                            runat="server" AutoPostBack="true" Width="130px" AllowCustomText="true" Text="Enter Custom Category" Font-Italic="true" Font-Size="X-Small" OnSelectedIndexChanged="radcombobox_extra_MilestoneCategoryName_SelectedIndexChanged"
                                                                            OnClientSelectedIndexChanged='<%# "function(combobox,args){categoryNameChanged(\""+ Container.FindControl("radcombobox_extra_MilestoneCategoryName").ClientID + "\",\"" +Container.FindControl("radTextBox_extra_mileStoneName").ClientID+"\",\"" +Container.FindControl("radTextBox_extra_releaseName").ClientID+"\");}" %>'>
                                                                        </telerik:RadComboBox>
                                                                    </div>
                                                                    <div class="col-md-2" style="width: 32.5%; margin-right: -15px;">
                                                                        <asp:Label ID="Label1" Text="Name:" runat="server"></asp:Label>
                                                                        <telerik:RadTextBox ID="radTextBox_extra_mileStoneName" runat="server" Width="220px"></telerik:RadTextBox>
                                                                    </div>
                                                                    <div class="col-md-2" style="width: 20%; margin-right: -35px;">
                                                                        <asp:Label runat="server" Text="From:"></asp:Label>
                                                                        <telerik:RadDatePicker ID="raddatepicker_extra_milestoneStartDate" AutoPostBack="false" runat="server" Width="100px" ClientEvents-OnDateSelected='<%# "function(sender,args){onDateChange(\""+ Container.FindControl("raddatepicker_extra_milestoneStartDate").ClientID + "\",\"" +Container.FindControl("raddatepicker_extra_milestoneEndDate").ClientID+"\");}" %>'>
                                                                        </telerik:RadDatePicker>
                                                                    </div>
                                                                    <div class="col-md-2" style="width: 20%; margin-right: -15px;">
                                                                        <asp:Label runat="server" Text="To:"></asp:Label>
                                                                        <telerik:RadDatePicker ID="raddatepicker_extra_milestoneEndDate" AutoPostBack="false" runat="server" Width="100px">
                                                                        </telerik:RadDatePicker>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 5%;">
                                                                        <telerik:RadButton runat="server" Height="22px" Width="22px" ID="radButton_extra_deleteMilestone" CausesValidation="false" OnClick="radButton_extra_deleteMilestone_Click">
                                                                            <Image ImageUrl="~/Images/red_x_mark.png" />
                                                                        </telerik:RadButton>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 5%;">
                                                                        <telerik:RadTextBox ID="radTextBox_extra_releaseName" runat="server" Width="220px" Display="false"></telerik:RadTextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-2" style="width: 30%; margin-right: -15px; margin-left: 26px">
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 30%; margin-right: -10px;">
                                                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_extra_mileStoneName"
                                                                            Text="Milestone name cannot be empty." ForeColor="Red"
                                                                            Display="Dynamic" EnableClientScript="true" ValidationGroup="add_milestone_group">
                                                                        </asp:RequiredFieldValidator>
                                                                    </div>
                                                                    <div class="col-md-6" style="width: 40%; margin-right: -35px;">
                                                                        <div class="container-fluid">
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_extra_milestoneStartDate"
                                                                                        Text="Milestone Start Date cannot be empty." ForeColor="Red"
                                                                                        Display="Dynamic" EnableClientScript="true" ValidationGroup="add_milestone_group">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_extra_milestoneEndDate"
                                                                                        Text="Milestone End Date cannot be empty." ForeColor="Red"
                                                                                        Display="Dynamic" EnableClientScript="true" ValidationGroup="add_milestone_group">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-md-12" style="margin-left: -15px;">
                                                                                    <asp:CompareValidator ID="dateCompareValidatorForMilestoneInAddMilestoneWindow" runat="server"
                                                                                        ControlToValidate="raddatepicker_extra_milestoneEndDate" ControlToCompare="raddatepicker_extra_milestoneStartDate"
                                                                                        Operator="GreaterThanEqual" Type="Date" ForeColor="Red" ValidationGroup="add_milestone_group"
                                                                                        ErrorMessage="End Date must be equal or greater than Start Date - please correct it." Display="Dynamic"
                                                                                        EnableClientScript="true">
                                                                                    </asp:CompareValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 2%; margin-right: -30px;">
                                                                    </div>
                                                                    <div class="col-md-1">
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-12" style="margin-left: -25px;">
                                                                        <telerik:RadPanelBar runat="server" ID="radPanelBar_newMilestoneWindowLowerPart_eSpecs" Width="90%">
                                                                            <Items>
                                                                                <telerik:RadPanelItem Expanded="false">
                                                                                    <HeaderTemplate>
                                                                                        <a class="rpExpandable" style="float: left">
                                                                                            <span class="rpExpandHandle"></span>
                                                                                        </a>
                                                                                        <asp:Label runat="server" Text="VSO eSpecs  "></asp:Label>
                                                                                        <asp:Label runat="server" Text="(Select the VSO eSpecs you want to create for this milestone)" Font-Italic="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                    <ContentTemplate>
                                                                                        <telerik:RadListBox runat="server" CssClass="labelPosition" ID="radListBoxeOnAddNewMilestoneLowerPartSpecs" Width="100%" CausesValidation="false" OnClientSelectedIndexChanging="OnClientSelectedIndexChanging" BorderColor="Transparent" CheckBoxes="true">
                                                                                            <ItemTemplate>
                                                                                                <div class="container-fluid">
                                                                                                    <div class="row">
                                                                                                        <div class="col-md-9" style="width: 75%; margin-left: 25px;">
                                                                                                            <telerik:RadTextBox runat="server" ID="radTextBox_eSpecName" Width="90%">
                                                                                                            </telerik:RadTextBox>
                                                                                                        </div>
                                                                                                        <div class="col-md-3" style="width: 20%;">
                                                                                                            <asp:Label runat="server" ID="label_AddNewMilestoneWindowLowerPart_eSpecsEstimate" Text="Estimate (Points) " Width="70%"></asp:Label>
                                                                                                            <telerik:RadTextBox runat="server" ID="radTextBox_eSpecEstimate" AutoPostBack="false" Text="" Width="25%">
                                                                                                            </telerik:RadTextBox>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </telerik:RadListBox>
                                                                                    </ContentTemplate>
                                                                                </telerik:RadPanelItem>
                                                                            </Items>
                                                                        </telerik:RadPanelBar>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </telerik:RadPanelBar>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <div class="row" style="margin-top: 300px">
                                            <div class="col-md-12">
                                            </div>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <telerik:RadButton ID="radButton_addMultipleMilestones" runat="server" Height="26px" Style="margin-top: 10px; margin-left: 15px;" Text="Add New Milestone" CausesValidation="false" OnClick="radButton_addMultipleMilestones_Click">
                                                        <Icon PrimaryIconUrl="~/Images/blue_plus.png" PrimaryIconCssClass="iconPostion" PrimaryIconLeft="10" PrimaryIconTop="3"></Icon>
                                                    </telerik:RadButton>
                                                </div>
                                                <div class="col-md-5">
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6"></div>
                                                <div class="col-md-6">
                                                    <telerik:RadButton ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>'
                                                        runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="40%" Style="margin-right: 20px; margin-top: 10px;" CausesValidation="true" ValidationGroup="add_milestone_group">
                                                    </telerik:RadButton>
                                                    <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" Width="40%" Style="margin-top: 10px;" CommandName="Cancel">
                                                    </telerik:RadButton>
                                                </div>
                                            </div>
                                        </div>
                                </asp:Panel>
                            </FormTemplate>
                        </EditFormSettings>
                    </telerik:GridTableView>
                    <%--

                    TestPlan Table

                    --%>
                    <telerik:GridTableView
                        runat="server"
                        AllowCustomPaging="true"
                        AllowPaging="true"
                        AutoGenerateColumns="False"
                        PageSize="10"
                        BorderWidth="0"
                        EnableViewState="true"
                        ViewStateMode="Enabled"
                        Name="TestPlanDetails"
                        CommandItemDisplay="Top"
                        ShowHeader="true"
                        EditMode="PopUp"
                        DataKeyNames="TestScheduleKey"
                        Caption="Test Plans">
                        <CommandItemSettings
                            AddNewRecordText="Add a New Test Plan" />
                        <Columns>
                            <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel" UniqueName="EditCommandColumn2">
                                <HeaderStyle Width="50px" />
                                <ItemStyle CssClass="editStyle"></ItemStyle>
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn HeaderText="Test Plan Name" DataField="TestSchedule_Name">
                                <HeaderStyle Width="200px" Font-Underline="true" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="VSO Query">
                                <HeaderStyle Width="200px" Font-Underline="true" />
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="lnkVSO" Target="_blank" NavigateUrl='<%#Eval("Vso_Web_Url") %>' ForeColor="#336699"><%#Eval("TestScheduleKey") %></asp:HyperLink>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Category">
                                <HeaderStyle Width="8%" Font-Underline="true" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Milestone_Category_Name") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridDateTimeColumn HeaderText="From" DataField="TestSchedule_Start_Date" DataFormatString="{0:M/d/yyyy}">
                                <HeaderStyle Width="200px" Font-Underline="true" />
                            </telerik:GridDateTimeColumn>
                            <telerik:GridDateTimeColumn HeaderText="To" DataField="TestSchedule_End_Date" DataFormatString="{0:M/d/yyyy}">
                                <HeaderStyle Width="200px" Font-Underline="true" />
                            </telerik:GridDateTimeColumn>

                            <telerik:GridBoundColumn HeaderText="Assigned Reources" DataField="AssignedResources">
                                <HeaderStyle Width="200px" Font-Underline="true" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Iteration" Visible="false" DataField="TestPlan_Iteration">
                                <HeaderStyle Width="200px" Font-Underline="true" />
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this Test Plan?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                                <ItemStyle CssClass="gapToRightBorderTestPlan" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <%--

                            Edit Form for TestPlan

                        --%>

                        <EditFormSettings EditFormType="Template">
                            <FormStyle CssClass="EditWindowColor" />
                            <PopUpSettings Width="1200px" ShowCaptionInEditForm="true" />
                            <FormTemplate>
                                <asp:Panel runat="server" Style="position: relative;" Height="500px" Width="100%">
                                    <div class="container-fluid" style="margin-top: 10px;">
                                        <div class="row">
                                            <div class="col-md-2" style="width: 26%; margin-right: -25px;">
                                                <asp:Label runat="server" Text="Milestone Category:"></asp:Label>
                                                <telerik:RadComboBox ID="RadComboBox_MilestoneCategory_TestPlan"
                                                    runat="server" AutoPostBack="false" CausesValidation="false" Width="130px" AllowCustomText="true" Text="Enter Custom Category" Font-Italic="true" Font-Size="X-Small">
                                                </telerik:RadComboBox>
                                            </div>
                                            <div class="col-md-2" style="width: 32%; margin-right: -10px; margin-left: 50px">
                                                <asp:Label runat="server" Text="Test Plan Name:"></asp:Label>
                                                <telerik:RadTextBox runat="server" ID="radtextbox_TestPlanID" Width="220px"></telerik:RadTextBox>
                                            </div>
                                            <div class="col-md-2" style="width: 20%; margin-right: -45px;">
                                                <asp:Label runat="server" Text="From:"></asp:Label>
                                                <telerik:RadDatePicker ID="raddatepicker_TestScheduleStartDate" AutoPostBack="false" runat="server" Width="100px">
                                                </telerik:RadDatePicker>
                                            </div>
                                            <div class="col-md-2" style="width: 20%; margin-right: -45px;">
                                                <asp:Label runat="server" Text="To:"></asp:Label>
                                                <telerik:RadDatePicker ID="raddatepicker_TestScheduleEndDate" AutoPostBack="false" runat="server" Width="100px" Enabled="true">
                                                </telerik:RadDatePicker>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2" style="width: 32%; margin-right: -10px; margin-left: 50px">
                                            </div>
                                            <div class="col-md-1" style="width: 30%; margin-right: -25px;">
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="radtextbox_TestPlanID"
                                                    Text="Test Plan name cannot be empty." ForeColor="Red"
                                                    Display="Dynamic" EnableClientScript="true" ValidationGroup="add_test_group">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-6" style="width: 35%; margin-right: -45px;">
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_TestScheduleStartDate"
                                                                Text="Test Plan Start Date cannot be empty." ForeColor="Red" EnableClientScript="true" ValidationGroup="add_test_group"
                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_TestScheduleEndDate"
                                                                Text="Test Plan End Date cannot be empty." ForeColor="Red" EnableClientScript="true" ValidationGroup="add_test_group"
                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12" style="margin-left: -15px;">
                                                            <asp:CompareValidator ID="dateCompareValidatorForTopTestScheduleInAddTestScheduleWindow" runat="server"
                                                                ControlToValidate="raddatepicker_TestScheduleEndDate" ControlToCompare="raddatepicker_TestScheduleStartDate"
                                                                Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                                                                ErrorMessage="End Date must be equal or greater than Start Date - please correct it." Display="Dynamic"
                                                                EnableClientScript="true" ValidationGroup="add_test_group">
                                                            </asp:CompareValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-1" style="width: 5%; margin-right: -30px;">
                                            </div>
                                            <div class="col-md-1">
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:Label runat="server" Text="Assigned Resources:"></asp:Label>
                                                <telerik:RadTextBox runat="server" ID="radTextBox_AssignedResources" Width="40%" Text="0"></telerik:RadTextBox>
                                            </div>
                                            <div class="col-md-5" style="margin-left: 38px; margin-right: 20px;">
                                                <asp:Label runat="server" ID="label_Iteraion" Text="Iteration:" Visible="false"></asp:Label>
                                                <telerik:RadTextBox runat="server" ID="radTextBox_Iteration" Visible="false" Width="350px" Text="LOCALIZATION"></telerik:RadTextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12" style="margin-left: -15px;">
                                                <asp:Panel runat="server" ScrollBars="Vertical" Style="position: absolute; max-height: 300px; overflow-y: auto;" Width="100%">
                                                    <div class="col-md-12">
                                                        <telerik:RadPanelBar ID="radPanelBar_extra_testPlans" runat="server" Width="100%">
                                                            <ItemTemplate>
                                                                <div class="container-fluid" style="margin-top: 10px; margin-left: -10px;">
                                                                    <div class="row">
                                                                        <div class="col-md-2" style="width: 26%; margin-right: -25px;">
                                                                            <asp:Label runat="server" Text="Milestone Category:"></asp:Label>
                                                                            <telerik:RadComboBox ID="RadComboBox_extra_milestoneCategory"
                                                                                runat="server" AutoPostBack="false" CausesValidation="false" Width="130px" AllowCustomText="true" Text="Enter Custom Category" Font-Italic="true" Font-Size="X-Small">
                                                                            </telerik:RadComboBox>
                                                                        </div>
                                                                        <div class="col-md-2" style="width: 30%; margin-right: -10px; margin-left: 50px">
                                                                            <asp:Label runat="server" Text="Test Plan Name:"></asp:Label>
                                                                            <telerik:RadTextBox runat="server" ID="radtextbox_extra_testPlanName" Width="220px"></telerik:RadTextBox>
                                                                        </div>
                                                                        <div class="col-md-2" style="width: 20%; margin-right: -35px;">
                                                                            <asp:Label runat="server" Text="From:"></asp:Label>
                                                                            <telerik:RadDatePicker ID="raddatepicker_extra_testPlanFrom" AutoPostBack="false" runat="server" Width="100px">
                                                                            </telerik:RadDatePicker>
                                                                        </div>
                                                                        <div class="col-md-2" style="width: 20%; margin-right: -45px;">
                                                                            <asp:Label runat="server" Text="To:"></asp:Label>
                                                                            <telerik:RadDatePicker ID="raddatepicker_extra_testPlanTo" AutoPostBack="false" runat="server" Width="100px" Enabled="true">
                                                                            </telerik:RadDatePicker>
                                                                        </div>
                                                                        <div class="col-md-1">
                                                                            <telerik:RadButton runat="server" Height="22px" Width="22px" ID="radButton_extra_deleteTestSchedule" CausesValidation="false" OnClick="radButton_extra_deleteTestSchedule_Click">
                                                                                <Image ImageUrl="~/Images/red_x_mark.png" />
                                                                            </telerik:RadButton>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-2" style="width: 32%; margin-right: -10px; margin-left: 50px">
                                                                        </div>
                                                                        <div class="col-md-1" style="width: 30%; margin-right: -25px;">
                                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="radtextbox_extra_testPlanName"
                                                                                Text="Test Plan name cannot be empty." ForeColor="Red"
                                                                                Display="Dynamic" EnableClientScript="true" ValidationGroup="add_test_group">
                                                                            </asp:RequiredFieldValidator>
                                                                        </div>
                                                                        <div class="col-md-6" style="width: 40%; margin-right: -45px;">
                                                                            <div class="container-fluid">
                                                                                <div class="row">
                                                                                    <div class="col-md-12">
                                                                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_extra_testPlanFrom"
                                                                                            Text="Test Plan Start Date cannot be empty." ForeColor="Red" EnableClientScript="true" ValidationGroup="add_test_group"
                                                                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-md-12">
                                                                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_extra_testPlanTo"
                                                                                            Text="Test Plan End Date cannot be empty." ForeColor="Red" EnableClientScript="true" ValidationGroup="add_test_group"
                                                                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="col-md-12" style="margin-left: -15px;">
                                                                                        <asp:CompareValidator ID="CompareValidator1" runat="server"
                                                                                            ControlToValidate="raddatepicker_extra_testPlanTo" ControlToCompare="raddatepicker_extra_testPlanFrom"
                                                                                            Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                                                                                            ErrorMessage="End Date must be equal or greater than Start Date - please correct it." Display="Dynamic"
                                                                                            EnableClientScript="true" ValidationGroup="add_test_group">
                                                                                        </asp:CompareValidator>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-1" style="width: 5%; margin-right: -30px;">
                                                                        </div>
                                                                        <div class="col-md-1">
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-3">
                                                                            <asp:Label runat="server" Text="Assigned Resources:"></asp:Label>
                                                                            <telerik:RadTextBox runat="server" ID="radTextBox_extra_assignedResource" Width="40%" Text="0"></telerik:RadTextBox>
                                                                        </div>
                                                                        <div class="col-md-5" style="margin-left: 38px; margin-right: 20px;">
                                                                            <asp:Label runat="server" ID="label1" Text="Iteration:" Visible="true"></asp:Label>
                                                                            <telerik:RadTextBox runat="server" ID="radTextBox_extra_iteration" Visible="true" Width="350px" Text="LOCALIZATION"></telerik:RadTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-7">
                                                                            <asp:Label runat="server" ID="label_testPlanIterationWarning_extrarow" ForeColor="Red" Visible="false" Font-Bold="true" Font-Italic="true" Text="Exception maybe caused by Invalid Iteration Path"></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-5">
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </telerik:RadPanelBar>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-top: 300px">
                                            <div class="col-md-12">
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3" style="width: 20%;">
                                                <telerik:RadButton ID="radButton_addMultipleTestPlans" runat="server" Height="26px" Style="margin-top: 10px; margin-left: 20px;" Text="Add New Test Plan" CausesValidation="false" OnClick="radButton_addMultipleTestPlans_Click">
                                                    <Icon PrimaryIconUrl="~/Images/blue_plus.png" PrimaryIconCssClass="iconPostion" PrimaryIconLeft="10" PrimaryIconTop="3"></Icon>
                                                </telerik:RadButton>
                                            </div>
                                            <div class="col-md-9">
                                                <asp:Label runat="server" ID="label_testPlanIterationWarning" Style="margin-top: 10px;" ForeColor="Red" Visible="false" Font-Bold="true" Font-Italic="true" Text="Exception maybe caused by Invalid Iteration Path"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6"></div>
                                            <div class="col-md-6">
                                                <telerik:RadButton runat="server" ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Add" : "Update" %>'
                                                    CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' Width="40%" Style="margin-right: 20px; margin-top: 10px" ValidationGroup="add_test_group">
                                                </telerik:RadButton>
                                                <telerik:RadButton runat="server" ID="btnCancel" Style="margin-top: 10px" Text="Cancel" CausesValidation="False"
                                                    CommandName="Cancel" Width="40%">
                                                </telerik:RadButton>
                                            </div>
                                            <div class="col-md-3"></div>
                                        </div>
                                    </div>
                                </asp:Panel>
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
<div style="margin-top: -330px;">
    <asp:Label runat="server" ID="label_warning_cancelledProduct" ForeColor="Red" Style="margin-left: 350px; font-size: medium" Visible="true"></asp:Label>
</div>
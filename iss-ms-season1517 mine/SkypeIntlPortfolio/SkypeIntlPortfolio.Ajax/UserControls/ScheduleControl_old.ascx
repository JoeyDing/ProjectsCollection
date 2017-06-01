<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleControl_old.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ScheduleControl_old" %>
<telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
    <script type="text/javascript">
    </script>
</telerik:RadScriptBlock>

<%--when using RadAjaxManagerProxy inside an usercontrol, all controls referenced in the proxy (AjaxControlID/ControlID)
    must be put inside a "<span runat="server"></span>--%>
<%--<a href="../Model/ProjectInfo.cs">../Model/ProjectInfo.cs</a>--%>

<%--<telerik:RadAjaxManagerProxy runat="server" ID="radManagerProxy1">
    <AjaxSettings>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>

<telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel1">
</telerik:RadAjaxLoadingPanel>--%>

<span id="Span1" runat="server" class="schedulecontrol">
    <asp:Panel runat="server" ID="scheduleControl_panel_mainContentPanel">
        <div style="padding-left: 20px;">
            <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server" ID="updatePanel_scheduleControl" RenderMode="Inline">
                <ContentTemplate>
                    <asp:Label runat="server" Font-Bold="true" Font-Italic="true" ForeColor="Red" Text="The releases below are the latest 20"></asp:Label>
                    <telerik:RadPanelBar EnableViewState="true" runat="server" ID="radpanelbar_releases_root" Width="100%" Skin="Telerik">
                        <ItemTemplate>
                            <div style="margin: 5px;">
                                <telerik:RadPanelBar EnableViewState="true" runat="server" ID="radpanelbar_releases_child" Width="100%" Skin="Telerik">
                                    <Items>
                                        <telerik:RadPanelItem>
                                            <HeaderTemplate>
                                                <div style="background: transparent !important;">
                                                    <a class="rpExpandable" style="float: left">
                                                        <span class="rpExpandHandle"></span>
                                                    </a>
                                                    <div class="container-fluid" style="width: 98%; padding-left: 0px; margin-left: 0px;">
                                                        <div class="row">
                                                            <div class="col-md-2" style="float: left; width: 20%; padding-left: 0px; margin-left: 0px">

                                                                <div class="hoverx">
                                                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "Parent.Parent.Parent.Attributes[\"ReleaseName\"]")%>'
                                                                        ondblclick='<%# "schedulecontrol_edit(\""
                                                                                                                    + RadAjaxManager.GetCurrent(Page).ClientID + "\",\""
                                                                                                                    + DataBinder.Eval(Container, "Parent.Parent.UniqueID") + "\",\""
                                                                                                                    + "release\")"%>'>
                                                                    </asp:Label>

                                                                    <div class="tooltipx">
                                                                        double click to edit
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2" style="float: left; width: 7.2%; padding-left: 0px; margin-left: 0px">
                                                                <asp:Label ID="label_displayUI_ReleaseAssignedTo" Text="VSO Epic:" runat="server"></asp:Label>
                                                            </div>
                                                            <div class="col-md-1" style="width: 23.1%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                <asp:HyperLink runat="server" ID="link_VsoItem_release" CausesValidation="false" Text="R-VSO-Link" Target="_blank">
                                                                </asp:HyperLink>
                                                            </div>
                                                            <div class="col-md-1" style="width: 3%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                <asp:Label runat="server" Text='From'></asp:Label>
                                                            </div>
                                                            <div class="col-md-2" style="width: 13%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                <telerik:RadDatePicker ID="raddatepicker_Release_from" AutoPostBack="true" CausesValidation="true" runat="server" Width="140"
                                                                    OnSelectedDateChanged="raddatepicker_Release_from_SelectedDateChanged">
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                            <div class="col-md-1" style="width: 2%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                <asp:Label runat="server" Text='To'></asp:Label>
                                                            </div>
                                                            <div class="col-md-2" style="width: 20%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                <telerik:RadDatePicker ID="raddatepicker_Release_to" AutoPostBack="true" CausesValidation="true" runat="server" Width="140"
                                                                    OnSelectedDateChanged="raddatepicker_Release_to_SelectedDateChanged">
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                            <div class="col-md-1" style="width: 5%; float: left; padding-right: 0px; margin-right: 0px;">
                                                                <telerik:RadButton runat="server" Style="margin-right: 0px" Height="22px" Width="22px" ID="radButton_ReleaseDelete" AutoPostBack="true" CausesValidation="false" OnClick="radButton_ReleaseDelete_Click">
                                                                    <Image ImageUrl="~/Images/red_x_mark.png" />
                                                                </telerik:RadButton>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-5" style="width: 58%"></div>
                                                            <div class="col-md-7" style="width: 25%">
                                                                <div class="container-fluid">
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_Release_from"
                                                                                Text="Release Start Date cannot be empty." ForeColor="Red"
                                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_Release_to"
                                                                                Text="Release End Date cannot be empty." ForeColor="Red"
                                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <asp:CompareValidator ID="dateCompareValidatorForMainUiReleaseUpdate" runat="server"
                                                                                ControlToValidate="raddatepicker_Release_to" ControlToCompare="raddatepicker_Release_from"
                                                                                Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                                                                                ErrorMessage="End Date must be equal or greater than Start Date - please correct dates." Display="Dynamic">
                                                                            </asp:CompareValidator>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-2" style="margin-left: 20px;">
                                                                <asp:Label runat="server" Text="Custom Tag:"></asp:Label>
                                                            </div>
                                                            <div class="col-md-10" style="margin-left: -110px;">
                                                                <asp:Label ID="Label_CustomTagOnUI" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <div style="margin-left: 20px;">
                                                    <asp:Label runat="server" Text="Milestones" Font-Bold="true" Font-Italic="true"></asp:Label>
                                                    <telerik:RadPanelBar EnableViewState="true" runat="server" ID="radpanelbar_milestones_root" Width="100%" Skin="Telerik">
                                                        <ItemTemplate>
                                                            <div class="container-fluid" style="width: 98%; padding-left: 0px; margin-left: 0px;">
                                                                <div class="row" style="margin-top: 16px;">
                                                                    <div class="col-md-2" style="float: left; width: 18.1%; padding-left: 0px; margin-left: 40px;">
                                                                        <div class="hoverx">
                                                                            <asp:Label ID="label_milestoneName" runat="server"
                                                                                ondblclick='<%# "schedulecontrol_edit(\""
															                        + RadAjaxManager.GetCurrent(Page).ClientID + "\",\""
															                        + DataBinder.Eval(Container, "UniqueID") + "\",\""
															                        + "milestone\")"%>'>
                                                                            </asp:Label>
                                                                            <div class="tooltipx">
                                                                                double click to edit
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-1" style="float: left; width: 7.5%; padding-left: 0px; margin-left: 0px">
                                                                        <asp:Label ID="label_VsoQuery" Text="VSO Query:" runat="server"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 8.5%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                        <asp:HyperLink runat="server" ID="link_VsoItem_Milestone" CausesValidation="false" Text="M-VSO-Link" Target="_blank">
                                                                        </asp:HyperLink>
                                                                    </div>

                                                                    <div class="col-md-1" style="width: 5%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                        <asp:Label ID="label_MilestoneCategory" Text="Category:" runat="server"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 10%; float: left; padding-left: 0px; margin-left: 0px; margin-right: 0px;">
                                                                        <telerik:RadComboBox ID="radcombobox_milestone_categoriesOnControl"
                                                                            OnSelectedIndexChanged="radcombobox_milestone_categoriesOnControl_SelectedIndexChanged" Style="max-width: 95px;"
                                                                            runat="server" AutoPostBack="true" CausesValidation="false" AllowCustomText="true" Text="Enter Custom Category" />
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 5%; float: none; padding-left: 0px; margin-left: 0px; padding-right: 0px; margin-right: 0px;">
                                                                        <asp:LinkButton runat="server" ID="link_VsoItemID" CausesValidation="false" Text="m-VSO-Link" OnClick="link_VsoItem_Click" Visible="false">
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 3%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                        <asp:Label runat="server" Text='From'></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 13.3%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                        <telerik:RadDatePicker ID="raddatepicker_milestone_from" AutoPostBack="true" CausesValidation="true" runat="server" Width="140"
                                                                            OnSelectedDateChanged="raddatepicker_milestone_from_SelectedDateChanged">
                                                                        </telerik:RadDatePicker>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 2%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                        <asp:Label runat="server" Text='To'></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 16%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                        <telerik:RadDatePicker ID="raddatepicker_milestone_to" AutoPostBack="true" CausesValidation="true" runat="server" Width="140"
                                                                            OnSelectedDateChanged="raddatepicker_milestone_to_SelectedDateChanged">
                                                                        </telerik:RadDatePicker>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 5%; float: left; padding-right: 0px; margin-right: 0px;">
                                                                        <telerik:RadButton runat="server" Style="margin-right: 0px" Height="22px" Width="22px" ID="radButton_MilestoneDelete" AutoPostBack="true" CausesValidation="false" OnClick="radButton_MilestoneDelete_Click">
                                                                            <Image ImageUrl="~/Images/red_x_mark.png" />
                                                                        </telerik:RadButton>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-4" style="width: 37%"></div>
                                                                    <div class="col-md-5" style="width: 25%">
                                                                        <div class="container-fluid">
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_milestone_from"
                                                                                        Text="Milestone Start Date cannot be empty." ForeColor="Red"
                                                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_milestone_to"
                                                                                        Text="Milestone End Date cannot be empty." ForeColor="Red"
                                                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <asp:CompareValidator ID="dateCompareValidatorForMainUiMilestoneUpdate" runat="server"
                                                                                        ControlToValidate="raddatepicker_milestone_to" ControlToCompare="raddatepicker_milestone_from"
                                                                                        Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                                                                                        ErrorMessage="End Date must be equal or greater than Start Date - please correct dates." Display="Dynamic">
                                                                                    </asp:CompareValidator>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 5%"></div>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </telerik:RadPanelBar>
                                                    <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_scheduleControl_addMilestone_linkButton">
                                                        <ContentTemplate>
                                                            <asp:LinkButton runat="server" ID="linkButton_addNewMilestone" CausesValidation="false" Text="Add New Milestone" OnClick="linkButton_addNewMilestone_Click"></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <asp:Label runat="server" Text="Test Plans" Font-Bold="true" Font-Italic="true"></asp:Label>
                                                    <telerik:RadPanelBar ID="radpanelbar_testSchedules_root" runat="server" Width="100%" Skin="Telerik">
                                                        <ItemTemplate>
                                                            <div class="container-fluid" style="width: 98%; padding-left: 0px; margin-left: 0px;">
                                                                <div class="row" style="margin-top: 16px;">
                                                                    <div class="col-md-2" style="float: left; width: 18.1%; padding-left: 0px; margin-left: 40px; margin-bottom: 20px;">
                                                                        <div class="hoverx">
                                                                            <asp:Label ID="label_testScheduleaName" runat="server"
                                                                                ondblclick='<%# "schedulecontrol_edit(\""
															                        + RadAjaxManager.GetCurrent(Page).ClientID + "\",\""
															                        + DataBinder.Eval(Container, "UniqueID") + "\",\""
															                        + "testSchedule\")"%>'>
                                                                            </asp:Label>
                                                                            <div class="tooltipx">
                                                                                double click to edit
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-1" style="float: left; width: 10%; padding-left: 0px; margin-left: 0px">
                                                                        <asp:Label ID="label_VsoQuery_testSchedule" Text="VSO Query:" runat="server"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 8%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                        <asp:HyperLink runat="server" ID="link_VsoItem_TestSchedule" CausesValidation="false" Text="T-VSO-Link" Target="_blank">
                                                                        </asp:HyperLink>
                                                                    </div>
                                                                    <div class=" col-md-1" style="width: 11%; margin-left: 0px; padding-left: 0px; float: left">
                                                                        <div class="hoverx">
                                                                            <asp:Label runat="server" ID="label_AssignedReources" Text="Assigned Resources" ondblclick='<%# "schedulecontrol_edit(\""
			                                                                        + RadAjaxManager.GetCurrent(Page).ClientID + "\",\""
			                                                                        + DataBinder.Eval(Container, "UniqueID") + "\",\""
			                                                                        + "testSchedule\")"%>'></asp:Label>
                                                                            <div class="tooltipxNew">
                                                                                double click to edit
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class=" col-md-1" style="width: 8%; padding-left: 0px; margin-left: 0px;">
                                                                        <asp:Label runat="server" ID="radTextBox_MainUi_AssignedResources" Text="0"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 3%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                        <asp:Label runat="server" Text='From'></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 13.3%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                        <telerik:RadDatePicker ID="raddatepicker_testSchedule_from" AutoPostBack="true" CausesValidation="true" runat="server" Width="140"
                                                                            OnSelectedDateChanged="raddatepicker_testSchedule_from_SelectedDateChanged">
                                                                        </telerik:RadDatePicker>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 2%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                        <asp:Label runat="server" Text='To'></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 16%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                        <telerik:RadDatePicker ID="raddatepicker_testSchedule_to" AutoPostBack="true" CausesValidation="true" runat="server" Width="140"
                                                                            OnSelectedDateChanged="raddatepicker_testSchedule_to_SelectedDateChanged">
                                                                        </telerik:RadDatePicker>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 5%; float: left; padding-right: 0px; margin-right: 0px;">
                                                                        <telerik:RadButton runat="server" Style="margin-right: 0px" Height="22px" Width="22px" ID="radButton_TestScheduleDelete" AutoPostBack="true" CausesValidation="false" OnClick="radButton_TestScheduleDelete_Click">
                                                                            <Image ImageUrl="~/Images/red_x_mark.png" />
                                                                        </telerik:RadButton>
                                                                    </div>
                                                                </div>
                                                                <div class="row" style="margin-top: -20px; margin-bottom: 40px;">
                                                                    <div class="col-md-2" style="float: left; width: 18.2%; padding-left: 0px; margin-left: -205px;">
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 10%; float: left; padding-left: 0px; margin-left: 0px;">
                                                                        <asp:Label Text="VSO Query:" ID="VSOTag_testPlan" runat="server"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 8%; margin-left: 0px; padding-left: 0px; float: left">
                                                                        <asp:HyperLink runat="server" ID="link_VsoItem_TestMilestoneCategory" CausesValidation="false" Text="T-VSO-Link" Target="_blank">
                                                                        </asp:HyperLink>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 5%; padding-left: 0px; margin-left: 0px;">
                                                                        <asp:Label Text="Category:" runat="server"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-1" style="width: 10%; padding-left: 0px; margin-left: 0px;">
                                                                        <telerik:RadComboBox ID="radcombobox_testPlan_categoriesOnControl"
                                                                            OnSelectedIndexChanged="radcombobox_testPlan_categoriesOnControl_SelectedIndexChanged" Style="max-width: 95px;"
                                                                            runat="server" AutoPostBack="true" CausesValidation="false" AllowCustomText="true" Text="Enter Custom Category" />
                                                                    </div>
                                                                    <div class="col-md-4"></div>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </telerik:RadPanelBar>
                                                    <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_scheduleControl_addTestSchedule_linkButton">
                                                        <ContentTemplate>
                                                            <asp:LinkButton runat="server" ID="linkButton_addNewTestSchedule" CausesValidation="false" Text="Add New Test Plan" OnClick="linkButton_addNewTestSchedule_Click"></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </ContentTemplate>
                                        </telerik:RadPanelItem>
                                    </Items>
                                </telerik:RadPanelBar>
                            </div>
                        </ItemTemplate>
                    </telerik:RadPanelBar>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_scheduleControl_addRelease_linkButton">
                <ContentTemplate>
                    <asp:LinkButton runat="server" ID="linkButton_addNewRelease" CausesValidation="false" Text="Add New Release" OnClick="linkButton_addNewRelease_Click"></asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
</span>

<span id="Span2" class="schedule-windows" runat="server">
    <asp:Panel runat="server" ID="panel_schedule_window" EnableViewState="true">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updatePanel_addRelease" ChildrenAsTriggers="false">
            <ContentTemplate>
                <telerik:RadTreeView runat="server" ID="treeview_addRelease">
                    <NodeTemplate>
                        <div class="modal fade" id="modal_addingRelease">
                            <div class="modal-dialog" style="padding-top: 180px;">
                                <div class="modal-content" style="width: 1000px; margin-left: -200px">
                                    <div class="modal-header">
                                        <div>
                                            <span id="Span3" runat="server">
                                                <asp:Label ID="label_window_addRelease" runat="server" />
                                            </span>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        </div>
                                    </div>
                                    <div class="modal-body">
                                        <asp:Panel runat="server" ID="radwindow_addRelease">
                                            <div class="container-fluid">
                                                <div class="row">
                                                    <div class="col-md-1" style="width: 2%;"></div>
                                                    <asp:UpdatePanel runat="server" ID="updatePanel_addRelease_inner_top" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                                        <ContentTemplate>
                                                            <div class="col-md-3" style="width: 30%; margin-right: -75px;">
                                                                <asp:Label ID="Label7" runat="server" Text="Release Name:"></asp:Label>
                                                                <telerik:RadTextBox ID="radtextbox_ReleaseName" runat="server" Width="100px"></telerik:RadTextBox>
                                                            </div>
                                                            <div class="col-md-5" style="width: 30%; margin-right: 40px;">
                                                                <asp:Label ID="Label3" runat="server" Text="From:" Style="margin-left: 48px;"></asp:Label>
                                                                <telerik:RadDatePicker ID="raddatepicker_ReleaseStartDate" runat="server" Width="100px"></telerik:RadDatePicker>
                                                                <asp:Label ID="Label4" runat="server" Text="To:"></asp:Label>
                                                                <telerik:RadDatePicker ID="raddatepicker_ReleaseEndDate" runat="server" Width="100px"></telerik:RadDatePicker>
                                                            </div>
                                                            <div class="col-md-1" style="width: 18%;">
                                                                <asp:Label runat="server" Text="Copy from a previous release:" Font-Bold="true" Font-Italic="true"></asp:Label>
                                                            </div>
                                                            <div class="col-md-3" style="width: 20%;">
                                                                <telerik:RadAutoCompleteBox runat="server" AutoPostBack="true" ID="RadAutoCompleteBox_existingRelease" TextSettings-SelectionMode="Single" EmptyMessage="Please type here" InputType="Token"
                                                                    Width="200" DropDownWidth="180px" Delimiter=" " OnEntryAdded="RadAutoCompleteBox_existingRelease_EntryAdded">
                                                                    <WebServiceSettings Method="search_existingReleasesNames" Path="UtilsReleasesInfo.aspx" />
                                                                </telerik:RadAutoCompleteBox>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                    <div class="col-md-0" style="width: 0%;">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1" style="width: 15%;"></div>
                                                    <div class="col-md-3" style="width: 30%; margin-right: -75px;">
                                                        <asp:RequiredFieldValidator runat="server" ID="validator_window_Release_ReleaseName" ControlToValidate="radtextbox_ReleaseName"
                                                            Text="Release Name cannot be empty." Style="margin-left: -70px;" Display="Dynamic" ForeColor="Red"
                                                            EnableClientScript="true" ValidationGroup="add_release_group"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-5" style="width: 38%; margin-right: 40px;">
                                                        <div class="container-fluid">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_ReleaseStartDate"
                                                                        Text="Release Start Date cannot be empty." ForeColor="Red"
                                                                        Display="Dynamic" EnableClientScript="true" ValidationGroup="add_release_group"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_ReleaseEndDate"
                                                                        Text="Release End Date cannot be empty." ForeColor="Red"
                                                                        Display="Dynamic" EnableClientScript="true" ValidationGroup="add_release_group"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <asp:CompareValidator ID="CompareValidatorReleaseStartEndDatesInAddReleaseWinddow" runat="server"
                                                                        ControlToValidate="raddatepicker_ReleaseEndDate" ControlToCompare="raddatepicker_ReleaseStartDate"
                                                                        Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                                                                        ErrorMessage="End Date must be equal or greater than Start Date - please correct dates." Display="Dynamic"
                                                                        EnableClientScript="true" ValidationGroup="add_release_group">
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
                                                    <div class="col-md-2" style="margin-left: -70px;">
                                                        <asp:RadioButtonList ID="radioButtonList_ReleaseType" runat="server"
                                                            RepeatDirection="Vertical" EnableClientScript="true" AutoPostBack="false">
                                                            <asp:ListItem Text="<b>SLA1</b>: New/Updated strings in existing file (weekly or bi-weekly releases)" Value="SLA1"></asp:ListItem>
                                                            <asp:ListItem Text="<b>SLA2</b>: New feature in already localized product; New resource file (bi-weekly or longer releases)" Value="SLA2"></asp:ListItem>
                                                            <asp:ListItem Text="<b>SLA3</b>: New product, new platform, or previously un-localized product" Value="SLA3"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:UpdatePanel runat="server" ID="updatePanel_addRelease_inner_milestonesList" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                                            <ContentTemplate>
                                                                <telerik:RadPanelBar ID="radPanelBar_miletoneInReleaseWindow" runat="server" Width="100%">
                                                                    <ItemTemplate>
                                                                        <div class="container-fluid">
                                                                            <div class="row">
                                                                                <div class="col-md-1" style="width: 5%;"></div>
                                                                                <div class="col-md-2" style="width: 30%; margin-right: -15px; margin-left: 26px">
                                                                                    <asp:Label ID="label_milestoneCategoryInWindow" Text="Milestone Category:" runat="server"></asp:Label>
                                                                                    <telerik:RadComboBox ID="radcombobox_milestone_categoriesInWindow" RenderMode="Auto"
                                                                                        runat="server" AutoPostBack="true" CausesValidation="false" Width="130px" AllowCustomText="true" Text="Enter Custom Category" Font-Italic="true" Font-Size="X-Small"
                                                                                        OnSelectedIndexChanged="radcombobox_milestone_categoriesInWindow_SelectedIndexChanged"
                                                                                        OnClientSelectedIndexChanged='<%# "function(combobox,args){onMilestoneCategoryAddNewReleaseWindowSelectionChanged(\""+ Container.FindControl("radcombobox_milestone_categoriesInWindow").ClientID + "\",\"" +Container.FindControl("radTextBox_mileStoneName_InWindow").ClientID+"\",\""+ Container.Parent.Parent.FindControl("radtextbox_ReleaseName").ClientID + "\");}" %>'>
                                                                                    </telerik:RadComboBox>
                                                                                </div>
                                                                                <div class="col-md-1" style="width: 20%; margin-right: -15px;">
                                                                                    <asp:Label ID="label_milestoneNameInWindow" Text="Name:" runat="server"></asp:Label>
                                                                                    <telerik:RadTextBox ID="radTextBox_mileStoneName_InWindow" runat="server" Width="100px"></telerik:RadTextBox>
                                                                                </div>
                                                                                <div class="col-md-2" style="width: 20%; margin-right: -35px;">
                                                                                    <asp:Label runat="server" Text="From:"></asp:Label>
                                                                                    <telerik:RadDatePicker ID="raddatepicker_milestone_from_InWindow" AutoPostBack="false" runat="server" Width="100px"
                                                                                        ClientEvents-OnDateSelected='<%# "function(button, args){milestoneDateSelected(\""+Container.FindControl("raddatepicker_milestone_from_InWindow").ClientID + "\",\"" + Container.FindControl("raddatepicker_milestone_to_InWindow").ClientID+ "\");}" %>'>
                                                                                    </telerik:RadDatePicker>
                                                                                </div>
                                                                                <div class="col-md-2" style="width: 20%; margin-right: -15px;">
                                                                                    <asp:Label runat="server" Text="To:"></asp:Label>
                                                                                    <telerik:RadDatePicker ID="raddatepicker_milestone_to_InWindow" AutoPostBack="false" runat="server" Width="100px">
                                                                                    </telerik:RadDatePicker>
                                                                                </div>
                                                                                <div class="col-md-0" style="width: 1%;">
                                                                                    <asp:Label ID="label_miletoneInReleaseWindow_MilestoneAssignedTo" Text="VSO E-Spec assigned to" runat="server" Visible="false"></asp:Label>
                                                                                </div>
                                                                                <div class="col-md-0" style="width: 1%;">
                                                                                    <telerik:RadTextBox ID="radtextbox_miletoneInReleaseWindow_MilestoneAssignedTo" runat="server" Width="100px" Visible="false"></telerik:RadTextBox>
                                                                                </div>
                                                                                <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_addRelease_milestoneList_radButton_delete">
                                                                                    <ContentTemplate>
                                                                                        <div class="col-md-1" style="width: 5%;">
                                                                                            <telerik:RadButton runat="server" Height="22px" Width="22px" ID="radButton__window_deleteMilestone" CausesValidation="false" OnClick="radButton__window_deleteMilestone_Click">
                                                                                                <Image ImageUrl="~/Images/red_x_mark.png" />
                                                                                            </telerik:RadButton>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-md-1">
                                                                                </div>
                                                                                <div class="col-md-11">
                                                                                    <telerik:RadPanelBar runat="server" ID="radPanelBar_eSpecs_child" Width="100%">
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
                                                                                                    <telerik:RadListBox runat="server" CssClass="labelPosition" AutoPostBack="true" ID="radListBoxeSpecs" CausesValidation="false" Width="100%" BorderColor="Transparent" OnClientSelectedIndexChanging="OnClientSelectedIndexChanging" CheckBoxes="true">
                                                                                                        <ItemTemplate>
                                                                                                            <div class="container-fluid">
                                                                                                                <div class="row">
                                                                                                                    <div class="col-md-9" style="width: 75%; margin-left: 25px;">
                                                                                                                        <telerik:RadTextBox runat="server" ID="radTextBoxeSpecs" Width="90%">
                                                                                                                        </telerik:RadTextBox>
                                                                                                                    </div>
                                                                                                                    <div class="col-md-3" style="width: 20%;">
                                                                                                                        <asp:Label runat="server" ID="label_eSpecsEstimate" Text="Estimate (Points) " Width="70%"></asp:Label>
                                                                                                                        <telerik:RadTextBox runat="server" ID="radText_eSpecsEstimate" Text="" Width="25%">
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
                                                                        </div>
                                                                        <div class="container-fluid">
                                                                            <div class="row">
                                                                                <div class="col-md-2" style="width: 34%; margin-right: -15px; margin-left: 26px">
                                                                                </div>
                                                                                <div class="col-md-1" style="width: 24%; margin-right: -15px;">
                                                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator_MilestoneNameInWindow" ControlToValidate="radTextBox_mileStoneName_InWindow"
                                                                                        Text="Milestone name cannot be empty." ForeColor="Red"
                                                                                        Display="Dynamic" EnableClientScript="true" ValidationGroup="add_release_group"></asp:RequiredFieldValidator>
                                                                                </div>
                                                                                <div class="col-md-6" style="width: 40%; margin-right: -35px;">
                                                                                    <div class="container-fluid">
                                                                                        <div class="row">
                                                                                            <div class="col-md-12">
                                                                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_milestone_from_InWindow"
                                                                                                    Text="Milestone Start Date cannot be empty." ForeColor="Red"
                                                                                                    Display="Dynamic" EnableClientScript="true" ValidationGroup="add_release_group"></asp:RequiredFieldValidator>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="row">
                                                                                            <div class="col-md-12">
                                                                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_milestone_to_InWindow"
                                                                                                    Text="Milestone End Date cannot be empty." ForeColor="Red"
                                                                                                    Display="Dynamic" EnableClientScript="true" ValidationGroup="add_release_group"></asp:RequiredFieldValidator>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="row">
                                                                                            <div class="col-md-12">
                                                                                                <asp:CompareValidator ID="dateCompareValidatorForMilestoneInAddReleaseWindow" runat="server"
                                                                                                    ControlToValidate="raddatepicker_milestone_to_InWindow" ControlToCompare="raddatepicker_milestone_from_InWindow"
                                                                                                    Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                                                                                                    ErrorMessage="End Date must be equal or greater than Start Date - please correct dates." Display="Dynamic"
                                                                                                    EnableClientScript="true" ValidationGroup="add_release_group">
                                                                                                </asp:CompareValidator>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-2" style="width: 2%; margin-right: -30px;">
                                                                                </div>
                                                                                <div class="col-md-1">
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </telerik:RadPanelBar>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-7">
                                                        <asp:UpdatePanel runat="server" ID="updatePanel_addRelease_inner_addMilestone" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                            <ContentTemplate>
                                                                <telerik:RadButton ID="radButton_UI_addMilestone" runat="server" Height="26px" Text="Add New Milestone" CausesValidation="false" OnClick="radButton_UI_addMilestone_Click">
                                                                    <Icon PrimaryIconUrl="~/Images/blue_plus.png" PrimaryIconCssClass="iconPostion" PrimaryIconLeft="10" PrimaryIconTop="3"></Icon>
                                                                </telerik:RadButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-5">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6"></div>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_addRelease_radButton">
                                                            <ContentTemplate>
                                                                <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Add" ID="radbutton_window_addRelease" OnClick="radbutton_window_addRelease_Click"
                                                                    CssClass="style_radbutton" CausesValidation="true" ValidationGroup="add_release_group">
                                                                </telerik:RadButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Cancel" ID="radbutton_window_addRelease_cancel"
                                                            AutoPostBack="false"
                                                            CssClass="style_radbutton" CausesValidation="false"
                                                            OnClientClicked='<%# "function(button, args){BootStrap_CloseWindow(\""+ Container.FindControl("radwindow_addRelease").ClientID + " \",args);}" %>'>
                                                        </telerik:RadButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </NodeTemplate>
                </telerik:RadTreeView>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updatePanel_addMilestone" ChildrenAsTriggers="false">
            <ContentTemplate>
                <telerik:RadTreeView runat="server" ID="treeview_addMilestone">
                    <NodeTemplate>
                        <div class="modal fade" id="modal_addingMilestone">
                            <div class="modal-dialog">
                                <div class="modal-content" style="width: 1000px; margin-left: -200px">
                                    <div class="modal-header">
                                        <div>
                                            <asp:Label ID="label_window_addMilestone" runat="server"></asp:Label>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        </div>
                                    </div>
                                    <div class="modal-body">
                                        <asp:Panel runat="server" ID="radwindow_addMilestone">
                                            <div class="container-fluid">
                                                <div class="row">
                                                    <div class="col-md-2" style="width: 25%; margin-right: -25px;">
                                                        <asp:Label runat="server" Text="Milestone Category:"></asp:Label>
                                                        <telerik:RadComboBox ID="radcombobox_MilestoneCategoryName_InMilestoneWindowTop"
                                                            runat="server" AutoPostBack="true" CausesValidation="false" Width="130px" AllowCustomText="true" Text="Enter Custom Category" Font-Italic="true" Font-Size="X-Small"
                                                            OnSelectedIndexChanged="radcombobox_MilestoneCategoryName_InMilestoneWindowTop_SelectedIndexChanged"
                                                            OnClientSelectedIndexChanged='<%# "function(combobox,args){onMilestoneCategoryAddNewMilestoneWindowSelectionChanged(\""+ Container.FindControl("radcombobox_MilestoneCategoryName_InMilestoneWindowTop").ClientID + "\",\"" +Container.FindControl("radTextBox_mileStoneName_OnMilestoneWindowTop").ClientID+"\",\"" +((Panel)(Container.FindControl("radwindow_addMilestone"))).Attributes["ReleaseName"]+"\");}" %>'>
                                                        </telerik:RadComboBox>
                                                    </div>
                                                    <div class="col-md-2" style="width: 30%; margin-right: -10px; margin-left: 50px">
                                                        <asp:Label ID="Label8" Text="Name:" runat="server"></asp:Label>
                                                        <telerik:RadTextBox ID="radTextBox_mileStoneName_OnMilestoneWindowTop" runat="server" Width="220px"></telerik:RadTextBox>
                                                    </div>
                                                    <div class="col-md-2" style="width: 20%; margin-right: -45px;">
                                                        <asp:Label runat="server" Text="From:"></asp:Label>
                                                        <telerik:RadDatePicker ID="raddatepicker_windowTop_MilestoneStartDate" AutoPostBack="false" runat="server" Width="100px"
                                                            ClientEvents-OnDateSelected='<%# "function(button, args){milestoneDateSelected(\""+ Container.FindControl("raddatepicker_windowTop_MilestoneStartDate").ClientID + "\",\"" +Container.FindControl("raddatepicker_windowTop_MilestoneEndDate").ClientID +"\",\""+ Container.FindControl("raddatepicker_windowTop_MilestoneStartDate").ClientID + "\");}" %>'>
                                                        </telerik:RadDatePicker>
                                                    </div>
                                                    <div class="col-md-2" style="width: 20%; margin-right: -45px;">
                                                        <asp:Label runat="server" Text="To:"></asp:Label>
                                                        <telerik:RadDatePicker ID="raddatepicker_windowTop_MilestoneEndDate" AutoPostBack="false" runat="server" Width="100px" Enabled="true">
                                                        </telerik:RadDatePicker>
                                                    </div>
                                                    <div class="col-md-1">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-2" style="width: 28%; margin-right: -10px; margin-left: 50px">
                                                    </div>
                                                    <div class="col-md-1" style="width: 30%; margin-right: -25px;">
                                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_mileStoneName_OnMilestoneWindowTop"
                                                            Text="Milestone name cannot be empty." ForeColor="Red"
                                                            Display="Dynamic" EnableClientScript="true" ValidationGroup="add_milestone_group">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6" style="width: 40%; margin-right: -45px;">
                                                        <div class="container-fluid">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_windowTop_MilestoneStartDate"
                                                                        Text="Milestone Start Date cannot be empty." ForeColor="Red" EnableClientScript="true" ValidationGroup="add_milestone_group"
                                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_windowTop_MilestoneEndDate"
                                                                        Text="Milestone End Date cannot be empty." ForeColor="Red" EnableClientScript="true" ValidationGroup="add_milestone_group"
                                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12" style="margin-left: -15px;">
                                                                    <asp:CompareValidator ID="dateCompareValidatorForTopMilestoneInAddMilestoneWindow" runat="server"
                                                                        ControlToValidate="raddatepicker_windowTop_MilestoneEndDate" ControlToCompare="raddatepicker_windowTop_MilestoneStartDate"
                                                                        Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                                                                        ErrorMessage="End Date must be equal or greater than Start Date - please correct it." Display="Dynamic"
                                                                        EnableClientScript="true" ValidationGroup="add_milestone_group">
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
                                                    <div class="col-md-12">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updatePanel_addMilestonewindow_top" ChildrenAsTriggers="false">
                                                            <ContentTemplate>
                                                                <telerik:RadPanelBar runat="server" ID="radPanelBar_newMilestoneWindowTop_eSpecs" Width="100%">
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
                                                                                <telerik:RadListBox runat="server" CssClass="labelPosition" AutoPostBack="true" ID="radListBoxeOnAddNewMilestoneSpecs" CausesValidation="false" Width="100%" BorderColor="Transparent" OnClientSelectedIndexChanging="OnClientSelectedIndexChanging" CheckBoxes="true">
                                                                                    <ItemTemplate>
                                                                                        <div class="container-fluid">
                                                                                            <div class="row">
                                                                                                <div class="col-md-9" style="width: 75%; margin-left: 25px;">
                                                                                                    <telerik:RadTextBox runat="server" ID="radTextBox_AddNewMilestoneWindowTop_eSpecs" Width="90%">
                                                                                                    </telerik:RadTextBox>
                                                                                                </div>
                                                                                                <div class="col-md-3" style="width: 20%;">
                                                                                                    <asp:Label runat="server" ID="label_AddNewMilestoneWindowTop_eSpecsEstimate" Text="Estimate (Points) " Width="70%"></asp:Label>
                                                                                                    <telerik:RadTextBox runat="server" ID="radText_AddNewMilestoneWindowTop_eSpecsEstimate" Text="" Width="25%">
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
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_addMilestone_milestoneList">
                                                            <ContentTemplate>
                                                                <telerik:RadPanelBar ID="radPanelBar_miletoneInMilestoneWindow" runat="server" Width="100%">
                                                                    <ItemTemplate>
                                                                        <div class="container-fluid">
                                                                            <div class="row">
                                                                                <div class="col-md-2" style="width: 30%; margin-right: -10px; margin-left: -26px">
                                                                                    <asp:Label ID="Label9" Text="Milestone Category:" runat="server"></asp:Label>
                                                                                    <telerik:RadComboBox ID="radcombobox_MilestoneCategoryName_InMilestoneWindow"
                                                                                        runat="server" AutoPostBack="true" CausesValidation="false" Width="130px" AllowCustomText="true" Text="Enter Custom Category" Font-Italic="true" Font-Size="X-Small"
                                                                                        OnSelectedIndexChanged="radcombobox_MilestoneCategoryName_InMilestoneWindowLowerPart_SelectedIndexChanged"
                                                                                        OnClientSelectedIndexChanged='<%# "function(combobox,args){onMilestoneCategoryAddNewMilestoneWindowSelectionChanged(\""+ Container.FindControl("radcombobox_MilestoneCategoryName_InMilestoneWindow").ClientID + "\",\"" +Container.FindControl("radTextBox_mileStoneName_InMilestoneWindow").ClientID+"\",\"" +((Panel)(Container.Parent.Parent.Parent.FindControl("radwindow_addMilestone"))).Attributes["ReleaseName"]+"\");}" %>'>
                                                                                    </telerik:RadComboBox>
                                                                                </div>
                                                                                <div class="col-md-2" style="width: 32.5%; margin-right: -15px;">
                                                                                    <asp:Label ID="Label1" Text="Name:" runat="server"></asp:Label>
                                                                                    <telerik:RadTextBox ID="radTextBox_mileStoneName_InMilestoneWindow" runat="server" Width="220px"></telerik:RadTextBox>
                                                                                </div>
                                                                                <div class="col-md-2" style="width: 20%; margin-right: -35px;">
                                                                                    <asp:Label runat="server" Text="From:"></asp:Label>
                                                                                    <telerik:RadDatePicker ID="raddatepicker_MilestoneStartDate" AutoPostBack="false" runat="server" Width="100px"
                                                                                        ClientEvents-OnDateSelected='<%# "function(button, args){milestoneDateSelected(\""+Container.FindControl("raddatepicker_MilestoneStartDate").ClientID + "\",\"" +Container.FindControl("raddatepicker_MilestoneEndDate").ClientID +"\");}" %>'>
                                                                                    </telerik:RadDatePicker>
                                                                                </div>
                                                                                <div class="col-md-2" style="width: 20%; margin-right: -15px;">
                                                                                    <asp:Label runat="server" Text="To:"></asp:Label>
                                                                                    <telerik:RadDatePicker ID="raddatepicker_MilestoneEndDate" AutoPostBack="false" runat="server" Width="100px">
                                                                                    </telerik:RadDatePicker>
                                                                                </div>
                                                                                <div class="col-md-1" style="width: 5%;">
                                                                                    <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_addMilestone_milestoneList_radButton_delete">
                                                                                        <ContentTemplate>
                                                                                            <telerik:RadButton runat="server" Height="22px" Width="22px" ID="radButton_MlestoneWindow_deleteMilestone" CausesValidation="false" OnClick="radButton_MlestoneWindow_deleteMilestone_Click">
                                                                                                <Image ImageUrl="~/Images/red_x_mark.png" />
                                                                                            </telerik:RadButton>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-md-2" style="width: 30%; margin-right: -15px; margin-left: 26px">
                                                                                </div>
                                                                                <div class="col-md-1" style="width: 30%; margin-right: -10px;">
                                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_mileStoneName_InMilestoneWindow"
                                                                                        Text="Milestone name cannot be empty." ForeColor="Red"
                                                                                        Display="Dynamic" EnableClientScript="true" ValidationGroup="add_milestone_group">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </div>
                                                                                <div class="col-md-6" style="width: 40%; margin-right: -35px;">
                                                                                    <div class="container-fluid">
                                                                                        <div class="row">
                                                                                            <div class="col-md-12">
                                                                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_MilestoneStartDate"
                                                                                                    Text="Milestone Start Date cannot be empty." ForeColor="Red"
                                                                                                    Display="Dynamic" EnableClientScript="true" ValidationGroup="add_milestone_group">
                                                                                                </asp:RequiredFieldValidator>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="row">
                                                                                            <div class="col-md-12">
                                                                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_MilestoneEndDate"
                                                                                                    Text="Milestone End Date cannot be empty." ForeColor="Red"
                                                                                                    Display="Dynamic" EnableClientScript="true" ValidationGroup="add_milestone_group">
                                                                                                </asp:RequiredFieldValidator>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="row">
                                                                                            <div class="col-md-12" style="margin-left: -15px;">
                                                                                                <asp:CompareValidator ID="dateCompareValidatorForMilestoneInAddMilestoneWindow" runat="server"
                                                                                                    ControlToValidate="raddatepicker_MilestoneEndDate" ControlToCompare="raddatepicker_MilestoneStartDate"
                                                                                                    Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                                                                                                    ErrorMessage="End Date must be equal or greater than Start Date - please correct it." Display="Dynamic"
                                                                                                    EnableClientScript="true" ValidationGroup="add_milestone_group">
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
                                                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updatePanel_addMilestonewindow_lowerPart" ChildrenAsTriggers="true">
                                                                                        <ContentTemplate>
                                                                                            <telerik:RadPanelBar runat="server" ID="radPanelBar_newMilestoneWindowLowerPart_eSpecs" Width="100%">
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
                                                                                                            <telerik:RadListBox runat="server" CssClass="labelPosition" AutoPostBack="true" ID="radListBoxeOnAddNewMilestoneLowerPartSpecs" CausesValidation="false" Width="100%" BorderColor="Transparent" OnClientSelectedIndexChanging="OnClientSelectedIndexChanging" CheckBoxes="true">
                                                                                                                <ItemTemplate>
                                                                                                                    <div class="container-fluid">
                                                                                                                        <div class="row">
                                                                                                                            <div class="col-md-9" style="width: 75%; margin-left: 25px;">
                                                                                                                                <telerik:RadTextBox runat="server" ID="radTextBox_AddNewMilestoneWindowLowerPart_eSpecs" Width="90%">
                                                                                                                                </telerik:RadTextBox>
                                                                                                                            </div>
                                                                                                                            <div class="col-md-3" style="width: 20%;">
                                                                                                                                <asp:Label runat="server" ID="label_AddNewMilestoneWindowLowerPart_eSpecsEstimate" Text="Estimate (Points) " Width="70%"></asp:Label>
                                                                                                                                <telerik:RadTextBox runat="server" ID="radText_AddNewMilestoneWindowLowerPart_eSpecsEstimate" Text="" Width="25%">
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
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </telerik:RadPanelBar>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-7">
                                                        <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_addMilestone_radButton_addMilestoneInList">
                                                            <ContentTemplate>
                                                                <telerik:RadButton ID="radButton_addMilestoneInMilestoneWindow" runat="server" Height="26px" Text="Add New Milestone" CausesValidation="false" OnClick="radButton_addMilestoneInMilestoneWindow_Click">
                                                                    <Icon PrimaryIconUrl="~/Images/blue_plus.png" PrimaryIconCssClass="iconPostion" PrimaryIconLeft="10" PrimaryIconTop="3"></Icon>
                                                                </telerik:RadButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-5">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6"></div>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_addMilestone_radButton_save">
                                                            <ContentTemplate>
                                                                <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Add" ID="radbutton_window_addMilestone" OnClick="radbutton_window_addMilestone_Click"
                                                                    CssClass="style_radbutton" CausesValidation="true" ValidationGroup="add_milestone_group">
                                                                </telerik:RadButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Cancel" ID="radbutton_window_addMilestone_cancel"
                                                            AutoPostBack="false"
                                                            CssClass="style_radbutton" CausesValidation="false"
                                                            OnClientClicked='<%# "function(button, args){BootStrap_CloseWindow(\""+ Container.FindControl("radwindow_addMilestone").ClientID + " \",args);}" %>'>
                                                        </telerik:RadButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </NodeTemplate>
                </telerik:RadTreeView>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updatePanel_updateMilestone" ChildrenAsTriggers="false">
            <ContentTemplate>
                <telerik:RadTreeView runat="server" ID="treeview_updateMilestone">
                    <NodeTemplate>
                        <div class="modal fade" id="modal_UpdateMilestoneName">
                            <div class="modal-dialog">
                                <div class="modal-content" style="width: 500px; margin-left: 100px">
                                    <div class="modal-header">
                                        <div>
                                            <asp:Label ID="label_WindowMilestoneNameUpdate" runat="server" />
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        </div>
                                    </div>
                                    <div class="modal-body">
                                        <asp:Panel runat="server" ID="radwindow_MilestoneNameUpdate">
                                            <div class="container-fluid">
                                                <div class="row">
                                                    <div class="col-md-12" style="width: 90%; margin-right: -10px; margin-left: 50px">
                                                        <asp:Label ID="Label_MilestoneNameUpdate" Text="Milestone Name:" runat="server"></asp:Label>
                                                        <telerik:RadTextBox ID="radTextBox_MilestoneNameUpdate" runat="server" Width="200px"></telerik:RadTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12" style="width: 90%; margin-right: -10px; margin-left: 50px">
                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_MilestoneNameUpdate"
                                                        Text="Milestone name cannot be empty." ForeColor="Red"
                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-3"></div>
                                                <div class="col-md-3">
                                                    <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_updateMilestone_radButton_save">
                                                        <ContentTemplate>
                                                            <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Update" ID="radbutton_UpdateMilestoneName_Update" OnClick="radbutton_UpdateMilestoneName_Update_Click"
                                                                CssClass="style_radbutton" CausesValidation="false">
                                                            </telerik:RadButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-3">
                                                    <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Cancel" ID="radbutton_UpdateMilestoneName_Cancel"
                                                        AutoPostBack="false"
                                                        CssClass="style_radbutton" CausesValidation="false"
                                                        OnClientClicked='<%# "function(button, args){CloseWindow(\""+ Container.FindControl("radwindow_MilestoneNameUpdate").ClientID + " \");}" %>'>
                                                    </telerik:RadButton>
                                                </div>
                                                <div class="col-md-3"></div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </NodeTemplate>
                </telerik:RadTreeView>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="updatePanel_addTestSchedule" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="false">
            <ContentTemplate>
                <telerik:RadTreeView ID="treeview_addTestSchedule" runat="server">
                    <NodeTemplate>
                        <div class="modal fade" id="modal_addingTestSchedule">
                            <div class="modal-dialog">
                                <div class="modal-content" style="width: 1000px; margin-left: -200px">
                                    <div class="modal-header">
                                        <div>
                                            <asp:Label ID="label_window_addTestSchedule" runat="server"></asp:Label>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        </div>
                                    </div>
                                    <div class="modal-body">
                                        <asp:Panel runat="server" ID="radwindow_addTestSchedule">
                                            <div class="container-fluid">
                                                <div class="row">
                                                    <div class="col-md-2" style="width: 26%; margin-right: -25px;">
                                                        <asp:Label runat="server" Text="Milestone Category:"></asp:Label>
                                                        <telerik:RadComboBox ID="radcombobox_MilestoneCategoryName_InTestPlanWindowTop"
                                                            runat="server" AutoPostBack="false" CausesValidation="false" Width="130px" AllowCustomText="true" Text="Enter Custom Category" Font-Italic="true" Font-Size="X-Small">
                                                        </telerik:RadComboBox>
                                                    </div>
                                                    <div class="col-md-2" style="width: 30%; margin-right: -10px; margin-left: 50px">
                                                        <asp:Label runat="server" Text="Test Plan Name:"></asp:Label>
                                                        <telerik:RadTextBox runat="server" ID="radtextbox_windowTop_TestPlanID"></telerik:RadTextBox>
                                                    </div>
                                                    <div class="col-md-2" style="width: 20%; margin-right: -45px;">
                                                        <asp:Label runat="server" Text="From:"></asp:Label>
                                                        <telerik:RadDatePicker ID="raddatepicker_windowTop_TestScheduleStartDate" AutoPostBack="false" runat="server" Width="100px"
                                                            ClientEvents-OnDateSelected='<%# "function(button, args){milestoneDateSelected(\""+Container.FindControl("raddatepicker_windowTop_TestScheduleStartDate").ClientID + "\",\"" +Container.FindControl("raddatepicker_windowTop_TestScheduleEndDate").ClientID +"\");}" %>'>
                                                        </telerik:RadDatePicker>
                                                    </div>
                                                    <div class="col-md-2" style="width: 20%; margin-right: -45px;">
                                                        <asp:Label runat="server" Text="To:"></asp:Label>
                                                        <telerik:RadDatePicker ID="raddatepicker_windowTop_TestScheduleEndDate" AutoPostBack="false" runat="server" Width="100px" Enabled="true">
                                                        </telerik:RadDatePicker>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-2" style="width: 32%; margin-right: -10px; margin-left: 50px">
                                                    </div>
                                                    <div class="col-md-1" style="width: 30%; margin-right: -25px;">
                                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="radtextbox_windowTop_TestPlanID"
                                                            Text="Test Plan name cannot be empty." ForeColor="Red"
                                                            Display="Dynamic" EnableClientScript="true" ValidationGroup="add_test_group">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-6" style="width: 40%; margin-right: -45px;">
                                                        <div class="container-fluid">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_windowTop_TestScheduleStartDate"
                                                                        Text="Test Plan Start Date cannot be empty." ForeColor="Red" EnableClientScript="true" ValidationGroup="add_test_group"
                                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_windowTop_TestScheduleEndDate"
                                                                        Text="Test Plan End Date cannot be empty." ForeColor="Red" EnableClientScript="true" ValidationGroup="add_test_group"
                                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12" style="margin-left: -15px;">
                                                                    <asp:CompareValidator ID="dateCompareValidatorForTopTestScheduleInAddTestScheduleWindow" runat="server"
                                                                        ControlToValidate="raddatepicker_windowTop_TestScheduleEndDate" ControlToCompare="raddatepicker_windowTop_TestScheduleStartDate"
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
                                                    <div class="col-md-2">
                                                        <asp:Label runat="server" Text="Assigned Resources:"></asp:Label>
                                                        <telerik:RadTextBox runat="server" ID="radTextBox_windowTop_AssignedResources" Width="40%" Text="0"></telerik:RadTextBox>
                                                    </div>
                                                    <div class="col-md-2" style="margin-left: 22px; margin-right: 20px;">
                                                        <asp:Label runat="server" Text="Iteration:"></asp:Label>
                                                        <telerik:RadTextBox runat="server" ID="radTextBox_windowTop_Iteration" Width="350px" Text="LOCALIZATION"></telerik:RadTextBox>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_addTestSchedule_testScheduleList">
                                                            <ContentTemplate>
                                                                <telerik:RadPanelBar ID="radPanelBar_testScheduleInTestScheduleWindow" runat="server" Width="100%">
                                                                    <ItemTemplate>
                                                                        <div class="container-fluid">
                                                                            <div class="row">
                                                                                <div class="col-md-2" style="width: 25%; margin-right: 25px; margin-left: -26px;">
                                                                                    <asp:Label runat="server" Text="Milestone Category:"></asp:Label>
                                                                                    <telerik:RadComboBox ID="radcombobox_MilestoneCategoryName_InTestPlanWindowMiddle"
                                                                                        runat="server" AutoPostBack="false" CausesValidation="false" Width="130px" AllowCustomText="true" Text="Enter Custom Category" Font-Italic="true" Font-Size="X-Small">
                                                                                    </telerik:RadComboBox>
                                                                                </div>
                                                                                <div class="col-md-2" style="width: 31%; margin-left: 23px">
                                                                                    <asp:Label ID="Label_testSchedule" Text="Test Plan Name:" runat="server"></asp:Label>
                                                                                    <telerik:RadTextBox ID="radTextBox_testScheduleName_InTestScheduleWindow" runat="server"></telerik:RadTextBox>
                                                                                </div>
                                                                                <div class="col-md-2" style="width: 20%; margin-right: -35px;">
                                                                                    <asp:Label runat="server" Text="From:"></asp:Label>
                                                                                    <telerik:RadDatePicker ID="raddatepicker_TestStartDate" AutoPostBack="false" runat="server" Width="100px"
                                                                                        ClientEvents-OnDateSelected='<%# "function(button, args){milestoneDateSelected(\""+Container.FindControl("raddatepicker_TestStartDate").ClientID + "\",\"" +Container.FindControl("raddatepicker_TestEndDate").ClientID +"\");}" %>'>
                                                                                    </telerik:RadDatePicker>
                                                                                </div>
                                                                                <div class="col-md-2" style="width: 17.5%; margin-right: -15px;">
                                                                                    <asp:Label runat="server" Text="To:"></asp:Label>
                                                                                    <telerik:RadDatePicker ID="raddatepicker_TestEndDate" AutoPostBack="false" runat="server" Width="100px" Enabled="true">
                                                                                    </telerik:RadDatePicker>
                                                                                </div>
                                                                                <div class="col-md-1" style="width: 5%;">
                                                                                    <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_addMilestone_milestoneList_radButton_delete">
                                                                                        <ContentTemplate>
                                                                                            <telerik:RadButton runat="server" Height="22px" Width="22px" ID="radButton_TestScheduleWindow_deleteTestSchedule" CausesValidation="false" OnClick="radButton_TestScheduleWindow_deleteTestSchedule_Click">
                                                                                                <Image ImageUrl="~/Images/red_x_mark.png" />
                                                                                            </telerik:RadButton>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-md-2" style="width: 31%; margin-right: -10px; margin-left: 50px">
                                                                                </div>
                                                                                <div class="col-md-1" style="width: 30%; margin-right: -10px;">
                                                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="radTextBox_testScheduleName_InTestScheduleWindow"
                                                                                        Text="Test Plan name cannot be empty." ForeColor="Red"
                                                                                        Display="Dynamic" EnableClientScript="true" ValidationGroup="add_test_group">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </div>
                                                                                <div class="col-md-6" style="width: 30%; margin-right: -35px;">
                                                                                    <div class="container-fluid">
                                                                                        <div class="row">
                                                                                            <div class="col-md-12">
                                                                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_TestStartDate"
                                                                                                    Text="Test Plan Start Date cannot be empty." ForeColor="Red"
                                                                                                    Display="Dynamic" EnableClientScript="true" ValidationGroup="add_test_group">
                                                                                                </asp:RequiredFieldValidator>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="row">
                                                                                            <div class="col-md-12">
                                                                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="raddatepicker_TestEndDate"
                                                                                                    Text="Test Plan End Date cannot be empty." ForeColor="Red"
                                                                                                    Display="Dynamic" EnableClientScript="true" ValidationGroup="add_test_group">
                                                                                                </asp:RequiredFieldValidator>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="row">
                                                                                            <div class="col-md-12" style="margin-left: -15px;">
                                                                                                <asp:CompareValidator ID="dateCompareValidatorForTestInAddTestScheduleWindow" runat="server"
                                                                                                    ControlToValidate="raddatepicker_TestEndDate" ControlToCompare="raddatepicker_TestStartDate"
                                                                                                    Operator="GreaterThanEqual" Type="Date" ForeColor="Red"
                                                                                                    ErrorMessage="End Date must be equal or greater than Start Date - please correct it." Display="Dynamic"
                                                                                                    EnableClientScript="true" ValidationGroup="add_test_group">
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
                                                                                <div class="col-md-2" style="width: 20%; margin-left: -25px;">
                                                                                    <asp:Label runat="server" Text="Assigned Resources:"></asp:Label>
                                                                                    <telerik:RadTextBox runat="server" ID="radTextBox_ExtraRow_AssignedResources" Width="35%" Text="0"></telerik:RadTextBox>
                                                                                </div>
                                                                                <div class="col-md-2">
                                                                                    <asp:Label runat="server" Text="Iteration:"></asp:Label>
                                                                                    <telerik:RadTextBox runat="server" ID="radTextBox_ExtraRow_Iteration" Width="350px" Text="LOCALIZATION"></telerik:RadTextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </telerik:RadPanelBar>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-7">
                                                        <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_addTestSchedule_radButton_addTestScheduleInList">
                                                            <ContentTemplate>
                                                                <telerik:RadButton ID="radButton_addTestInTestScheduleWindow" runat="server" Height="26px" Text="Add New Test Plan" CausesValidation="false" OnClick="radButton_addTestInTestScheduleWindow_Click">
                                                                    <Icon PrimaryIconUrl="~/Images/blue_plus.png" PrimaryIconCssClass="iconPostion" PrimaryIconLeft="10" PrimaryIconTop="3"></Icon>
                                                                </telerik:RadButton>
                                                                <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_addTestSchedule_iTerationPath">
                                                                    <ContentTemplate>
                                                                        <asp:Label runat="server" ID="label_testPlanIterationWarning" ForeColor="Red" Visible="false" Font-Bold="true" Font-Italic="true" Text="Invalid name(s) given for Iteration Path(s) above."></asp:Label>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-5">
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-6"></div>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_addTestSchedule_radButton_save">
                                                            <ContentTemplate>
                                                                <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Add" ID="radbutton_window_addTestSchedule"
                                                                    CssClass="style_radbutton" CausesValidation="true" ValidationGroup="add_test_group" OnClick="radbutton_window_addTestSchedule_Click">
                                                                </telerik:RadButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Cancel" ID="radbutton_window_addTestSchedule_cancel"
                                                            AutoPostBack="false"
                                                            CssClass="style_radbutton" CausesValidation="false"
                                                            OnClientClicked='<%# "function(button, args){BootStrap_CloseWindow(\""+ Container.FindControl("radwindow_addTestSchedule").ClientID + " \",args);}" %>'>
                                                        </telerik:RadButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </NodeTemplate>
                </telerik:RadTreeView>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!--------------------------------------

            Update Dialog for test case

         ------------------------------------------->
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updatePanel_updateTestSchedule" ChildrenAsTriggers="false">
            <ContentTemplate>
                <telerik:RadTreeView ID="treeview_updateTestSchedule" runat="server">
                    <NodeTemplate>
                        <div class="modal fade">
                            <div class="modal-dialog">
                                <div class="modal-content" style="width: 500px;">
                                    <div class="modal-header">
                                        <div>
                                            <span id="Span3" runat="server">
                                                <asp:Label ID="updatePanel_updateTestSchedule_title" runat="server" />
                                            </span>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        </div>
                                    </div>
                                    <div class="modal-body">
                                        <asp:Panel runat="server" ID="updatePanel_updateTestSchedule_panel">
                                            <div class="container-fluid">
                                                <div class="row">
                                                    <div class="col-md-1" style="width: 2%;"></div>
                                                    <asp:UpdatePanel runat="server" ID="updatePanel_updateTestSchedule_inner_updatePanel" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="updatePanel_updateTestSchedule_testcaseName" runat="server" Text="Test Plan Name:"></asp:Label>
                                                                            <telerik:RadTextBox ID="updatePanel_updateTestSchedul_testcaseName" runat="server" Width="160px"></telerik:RadTextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label runat="server" Text="Assigned Resources:"></asp:Label>
                                                                            <telerik:RadTextBox runat="server" ID="updatePanel_updateTestSchedule_assignedResourcesradTextBox" Width="40%" Text="0"></telerik:RadTextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RequiredFieldValidator runat="server" ID="validator_window_Release_ReleaseName" ControlToValidate="updatePanel_updateTestSchedul_testcaseName"
                                                                                Text="Testcase Name cannot be empty." Display="Dynamic" ForeColor="Red"
                                                                                EnableClientScript="true" ValidationGroup="add_updatetestcase_group"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col-md-6"></div>
                                                                <div class="col-md-2">
                                                                    <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                                                                        <ContentTemplate>
                                                                            <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Update" ID="updatePanel_updateTestSchedule_radButton_update"
                                                                                CssClass="style_radbutton" CausesValidation="true" ValidationGroup="add_updatetestcase_group" OnClick="updatePanel_updateTestSchedule_radButton_update_Click">
                                                                            </telerik:RadButton>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Cancel" ID="updatePanel_updateTestSchedule_radButton_cancel"
                                                                        AutoPostBack="false"
                                                                        CssClass="style_radbutton" CausesValidation="false"
                                                                        OnClientClicked='<%# "function(button, args){BootStrap_CloseWindow(\""+ Container.FindControl("updatePanel_updateTestSchedule_panel").ClientID + " \",args);}" %>'>
                                                                    </telerik:RadButton>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </NodeTemplate>
                </telerik:RadTreeView>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updaterPanel_deleteTestWarning" ChildrenAsTriggers="false">
            <ContentTemplate>
                <telerik:RadTreeView runat="server" ID="treeview_deleteTestWarning">
                    <NodeTemplate>
                        <div class="modal fade" id="modal_deleteTestWarning" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="height: 30px"><span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title">Confirm delete</h4>
                                    </div>
                                    <div class="modal-body">
                                        <asp:Panel runat="server" ID="radwindow_deleteTest_Warning">
                                            <div class="container-fluid">
                                                <div class="row"></div>
                                                <div class="row">
                                                    <div class="col-xs-2"></div>
                                                    <div class="col-xs-8">
                                                        <asp:Label runat="server" ID="label_deleteTestWarning" Font-Size="Larger"></asp:Label>
                                                    </div>
                                                    <div class="col-xs-2"></div>
                                                </div>
                                                <div class="row"></div>
                                                <div class="row">
                                                    <div class="col-xs-3">
                                                    </div>
                                                    <div class="col-xs-3">
                                                        <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_deleteTestWarning_radButton_Ok">
                                                            <ContentTemplate>
                                                                <telerik:RadButton Width="100%" Height="100%" runat="server" ID="radButton_OK_deleteTestWarning" Text="OK" OnClick="radButton_OK_deleteTestWarning_Click" AutoPostBack="true" CausesValidation="false"></telerik:RadButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-xs-3">
                                                        <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Cancel" ID="radButton_Cancel_deleteTestWarning"
                                                            OnClientClicked='<%# "function(button, args){CloseWindow(\""+ Container.FindControl("radwindow_deleteTest_Warning").ClientID + " \");}" %>'
                                                            AutoPostBack="false" CausesValidation="false">
                                                        </telerik:RadButton>
                                                    </div>
                                                    <div class="col-xs-3">
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </NodeTemplate>
                </telerik:RadTreeView>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updatePanel_deleteMilestoneWarning" ChildrenAsTriggers="false">
            <ContentTemplate>
                <telerik:RadTreeView runat="server" ID="treeview_deleteMilestoneWarning">
                    <NodeTemplate>
                        <div class="modal fade" id="modal_deleteMilestoneWarning" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="height: 30px"><span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title">Confirm delete</h4>
                                    </div>
                                    <div class="modal-body">
                                        <asp:Panel runat="server" ID="radwindow_deleteMilestone_Warning">
                                            <div class="container-fluid">
                                                <div class="row"></div>
                                                <div class="row">
                                                    <div class="col-xs-2"></div>
                                                    <div class="col-xs-8">
                                                        <asp:Label runat="server" ID="label_deleteMilestoneWarning" Font-Size="Larger"></asp:Label>
                                                    </div>
                                                    <div class="col-xs-2"></div>
                                                </div>
                                                <div class="row"></div>
                                                <div class="row">
                                                    <div class="col-xs-3">
                                                    </div>
                                                    <div class="col-xs-3">
                                                        <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_deleteMilestoneWarning_radButton_Ok">
                                                            <ContentTemplate>
                                                                <telerik:RadButton Width="100%" Height="100%" runat="server" ID="radButton_OK_deleteMilestoneWarning" Text="OK" OnClick="radButton_OK_deleteMilestoneWarning_Click" AutoPostBack="true" CausesValidation="false"></telerik:RadButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-xs-3">
                                                        <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Cancel" ID="radButton_Cancel_deleteMilestoneWarning"
                                                            OnClientClicked='<%# "function(button, args){CloseWindow(\""+ Container.FindControl("radwindow_deleteMilestone_Warning").ClientID + " \");}" %>'
                                                            AutoPostBack="false" CausesValidation="false">
                                                        </telerik:RadButton>
                                                    </div>
                                                    <div class="col-xs-3">
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </NodeTemplate>
                </telerik:RadTreeView>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updatePanel_deleteReleaseWarning" ChildrenAsTriggers="false">
            <ContentTemplate>
                <telerik:RadTreeView runat="server" ID="treeview_deleteReleaseWarning">
                    <NodeTemplate>
                        <div class="modal fade" id="modal_deleteReleaseWarning" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="height: 30px"><span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title">Confirm delete</h4>
                                    </div>
                                    <div class="modal-body">
                                        <asp:Panel runat="server" ID="radwindow_deleteRelease_Warning">
                                            <div class="container-fluid">
                                                <div class="row"></div>
                                                <div class="row">
                                                    <div class="col-xs-2"></div>
                                                    <div class="col-xs-8">
                                                        <asp:Label runat="server" ID="label_deleteReleaseWarning" Font-Size="Larger"></asp:Label>
                                                    </div>
                                                    <div class="col-xs-2"></div>
                                                </div>
                                                <div class="row"></div>
                                                <div class="row">
                                                    <div class="col-xs-3">
                                                    </div>
                                                    <div class="col-xs-3">
                                                        <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" ID="updatePanel_deleteReleaseWarning_radButton_Ok">
                                                            <ContentTemplate>
                                                                <telerik:RadButton Width="100%" Height="100%" runat="server" ID="radButton_OK_deleteReleaseWarning" Text="OK" OnClick="radButton_OK_deleteReleaseWarning_Click" AutoPostBack="true" CausesValidation="false"></telerik:RadButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-xs-3">
                                                        <telerik:RadButton Width="100%" Height="100%" runat="server" Text="Cancel" ID="radButton_Cancel_deleteReleaseWarning"
                                                            OnClientClicked='<%# "function(button, args){CloseWindow(\""+ Container.FindControl("radwindow_deleteRelease_Warning").ClientID + " \");}" %>'
                                                            AutoPostBack="false" CausesValidation="false">
                                                        </telerik:RadButton>
                                                    </div>
                                                    <div class="col-xs-3">
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </NodeTemplate>
                </telerik:RadTreeView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</span>
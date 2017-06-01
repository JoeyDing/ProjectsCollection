<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LineOfSightControl.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.LineOfSight" %>

<style type="text/css">
    .RadScheduler .rsAdvancedEdit label {
        text-align: left;
    }

    .line_of_sight .container-fluid {
        padding: 1px;
    }

    .linkColor a {
        color: white;
    }

    .legend .RadScheduler .rsApt .rsAptContent {
        height: 15px;
        width: 15px;
        border: solid 1px;
        padding: 0px;
        position: relative;
    }

    .legend .legendtitle {
        float: left;
        display: block;
        margin-left: 20px;
    }

    /* disable delete 'X' from Appointment*/
    .RadScheduler_Metro .rsApt .rsAptDelete {
        display: none;
    }

    /*the class below is the green releaseinfo bar in the sheduler*/
    html.RadScheduler .rsAptContent,
    .RadScheduler .rsAptIn,
    .RadScheduler .rsAptMid,
    .RadScheduler .rsAptOut {
        background-color: transparent;
        height: 40px !important;
        border-top-color: green;
        border-top-width: 5px;
        text-align: center;
    }

    .testPlanStyle {
        /*color: ghostwhite;*/
        color: black;
        height: 40px !important;
    }

    /* to remove warning icon from miestone delete confirm window*/
    .rsModalWrapper .rsModalIcon {
        background-image: none !important;
    }

    /*to remove icon from Title bar of miestone delete confirm window*/
    .rsModalWrapper .rsModalTitle {
        background-image: none !important;
        color: white !important;
        font-size: larger;
    }

    /*to make the outer border and title bar same as Skype color*/
    .RadScheduler .rsModalWrapper .rsModalOuterTitle {
        background-color: #25A0DA;
    }

    .RadScheduler_Metro div.rsModalWrapper .rsModalOuter {
        background-color: #25A0DA;
    }

    /*to freeze the blue radscheduler header*/
    /*.RadScheduler_Metro .rsHeader {
        position: fixed;
        width: 100%;
    }

    .RadScheduler_Metro .rsContent {
        position: fixed;
        margin-top: 30px;
    }

    .RadScheduler table tr:nth-child(2){
        position: fixed;
        height: 100%;
        overflow: scroll;
    }*/
    /****************************************/
</style>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

    <script type="text/javascript">

        function OnClientAppointmentClick(sender, args) {
            var apt = args.get_appointment();
            showTooltip(apt);
        }

        function showTooltip(apt) {

            var tooltip = $find('<%=RadToolTip1.ClientID %>');

               tooltip.set_targetControl(apt.get_element());

               $get("startTime").innerHTML = apt.get_start().format("MM/dd/yyyy");

               $get("endTime").innerHTML = apt.get_end().format("MM/dd/yyyy");

               $get("descriptionDiv").innerHTML = apt.get_subject();

               var urlText = apt.get_attributes().getAttribute('VsoItemLinkText');
               var VsoItemLink = apt.get_attributes().getAttribute('VsoItemLink');
               var result = ' <a target="_blank" href="' + VsoItemLink + '">' + urlText + '</a>';

               $get("vsoQuery").innerHTML = result;
               var vsoHoursValue = apt.get_attributes().getAttribute('VsoItemHours');
               var assignedResources = apt.get_attributes().getAttribute('AssignedResources');
               var vacationItems = apt.get_attributes().getAttribute('VacationItems');
               var peopleAffValue = apt.get_attributes().getAttribute('PeopleAff');
               var productAffValue = apt.get_attributes().getAttribute('ProductAff');

               //$get("vsoHours").innerHTML = vsoHoursValue;

               //if vsoHoursValue is -1, it means its type is test,no hours housld be displayed

               if (vacationItems == "-1") {
                   $get("peopleAffLabel").style.display = 'inline';
                   $get("peoplelAff").innerHTML = peopleAffValue;
                   $get("productAffLabel").style.display = 'inline';
                   $get("productAff").innerHTML = productAffValue;
                   $get("vsoQueryLabel").style.display = 'none';
                   $get("vsoQuery").innerHTML = '';

                   $get("vsoHourLabel").style.display = 'none';
                   $get("vsoHours").innerHTML = '';

               }

               else if (vsoHoursValue == "-1") {
                   $get("vsoQueryLabel").style.display = 'inline';
                   $get("vsoHourLabel").style.display = 'none';
                   $get("vsoHours").innerHTML = '';

                   $get("peopleAffLabel").style.display = 'none';
                   $get("peoplelAff").innerHTML = '';
                   $get("productAffLabel").style.display = 'none';
                   $get("productAff").innerHTML = '';
               }

               else {
                   $get("vsoQueryLabel").style.display = 'inline';
                   $get("vsoHourLabel").style.display = 'inline';
                   $get("vsoHours").innerHTML = vsoHoursValue;

                   $get("peopleAffLabel").style.display = 'none';
                   $get("peoplelAff").innerHTML = '';;
                   $get("productAffLabel").style.display = 'none';
                   $get("productAff").innerHTML = '';;
               }

               if (assignedResources == "-1") {
                   $get("assignedResourcesLabel").style.display = 'none';
                   $get("assignedResources").innerHTML = '';
               }
               else {
                   $get("assignedResourcesLabel").style.display = 'inline';
                   $get("assignedResources").innerHTML = assignedResources;
               }

               tooltip.set_text($get("contentContainer").innerHTML);

               setTimeout(function () {
                   tooltip.show();
               }, 20);
           }

           function hideTooltip() {

               setTimeout(function () {

                   var activeTooltip = Telerik.Web.UI.RadToolTip.getCurrent();

                   if (activeTooltip)

                   { activeTooltip.hide(); }

               }, 50);

           }
        
        
        function OnClientAppointmentMoveEnd(sender, eventArgs) {
            if (!confirm('Do you want to move?')) {
                eventArgs.set_cancel(true);
            }
        }
       
        function OnClientAppointmentResizeEnd(sender, eventArgs) {
            if (!confirm('Do you want to resize?')) {
                eventArgs.set_cancel(true);
            }
        }
    </script>
</telerik:RadCodeBlock>

<div class="line_of_sight">
    <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanelReportingSystem">
    </telerik:RadAjaxLoadingPanel>
    <asp:Panel runat="server" ID="panel_radSchedule" Height="550px">
        <div class="container-fluid">
            <div class="row" style="font-style: oblique;">
                <div class="col-md-1 floatPosition" style="width: 10%">
                    <asp:Label ID="Label2" runat="server" Text="Slot Duration:"></asp:Label>
                </div>
                <div class="col-md-2 floatPosition" style="width: 14%">
                    <asp:Label runat="server" Text="Display:" />
                </div>
                <div class="col-md-7 floatPosition" style="width: 58%">
                    <asp:Label ID="Label3" runat="server" Text="Milestones Category:"></asp:Label>
                </div>
                <div class="col-md-2 floatPosition" style="width: 18%">
                    <asp:Label ID="Label1" runat="server" Text="When viewing multiple projects, group by:"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-1 floatPosition" style="width: 10%">
                    <telerik:RadDropDownList runat="server" ID="SlotDuration" AutoPostBack="true" Width="80%">
                        <Items>
                            <telerik:DropDownListItem Value="1.00:00:00" Text="1 day"></telerik:DropDownListItem>
                            <telerik:DropDownListItem Value="2.00:00:00" Text="2 days"></telerik:DropDownListItem>
                            <telerik:DropDownListItem Value="7.00:00:00" Text="1 week" Selected="true"></telerik:DropDownListItem>
                            <telerik:DropDownListItem Value="14.00:00:00" Text="2 weeks"></telerik:DropDownListItem>
                            <telerik:DropDownListItem Value="28.00:00:00" Text="4 weeks"></telerik:DropDownListItem>
                        </Items>
                    </telerik:RadDropDownList>
                </div>
                <div class="col-md-2 floatPosition" style="width: 14%">
                    <telerik:RadListBox ID="RadListBox_Release_Milestone" runat="server" CheckBoxes="true" AutoPostBack="true" CausesValidation="false" Width="68%" Style="border: none !important;" OnClientSelectedIndexChanging="OnClientSelectedIndexChanging" OnItemCheck="RadListBox_Release_Milestone_ItemCheck">
                        <Items>
                            <telerik:RadListBoxItem Checked="true" Text="Release" Value="release" Style="margin-bottom: -10px; background-color: transparent;" />
                            <telerik:RadListBoxItem Checked="true" Text="Milestone" Value="milestone" Style="background: transparent;" />
                            <telerik:RadListBoxItem Checked="true" Text="Test Plan" Value="testPlan" Style="background: transparent; margin-top: -10px;" />
                            <telerik:RadListBoxItem Checked="true" Text="Vacation" Value="vacation" Style="background: transparent; margin-top: -10px;" />
                        </Items>
                    </telerik:RadListBox>
                </div>
                <div class="col-md-7 floatPosition" style="width: 58%;">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-2 extraCol2 ">
                                <div class="legend">
                                    <div class="RadScheduler">
                                        <div class="rsApt rsCategoryGreen">
                                            <div class="rsAptContent">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="legendtitle">
                                        <asp:Label runat="server" Text="locready" />
                                        <telerik:RadButton runat="server" ID="radbuttoncheck_locready" ButtonType="ToggleButton" ToggleType="CheckBox" Checked="true" OnCheckedChanged="radbuttoncheck_CheckedChanged" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2 extraCol2 ">
                                <div class="legend">
                                    <div class="RadScheduler">
                                        <div class="rsApt rsCategoryBlue">
                                            <div class="rsAptContent">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="legendtitle">
                                        <asp:Label runat="server" Text="locstart" />
                                        <telerik:RadButton runat="server" ID="radbuttoncheck_locstart" ButtonType="ToggleButton" ToggleType="CheckBox" Checked="true" OnCheckedChanged="radbuttoncheck_CheckedChanged" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 extraCol2 ">
                                <div class="legend">
                                    <div class="RadScheduler">
                                        <div class="rsApt rsCategoryYellow">
                                            <div class="rsAptContent">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="legendtitle">
                                        <asp:Label runat="server" Text="progressing" />
                                        <telerik:RadButton runat="server" ID="radbuttoncheck_progressing" ButtonType="ToggleButton" ToggleType="CheckBox" Checked="true" OnCheckedChanged="radbuttoncheck_CheckedChanged" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 extraCol2 ">
                                <div class="legend">
                                    <div class="RadScheduler">
                                        <div class="rsApt rsCategoryRed">
                                            <div class="rsAptContent">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="legendtitle">
                                        <asp:Label runat="server" Text="endgame" />
                                        <telerik:RadButton runat="server" ID="radbuttoncheck_endgame" ButtonType="ToggleButton" ToggleType="CheckBox" Checked="true" OnCheckedChanged="radbuttoncheck_CheckedChanged" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 extraCol2 ">
                                <div class="legend">
                                    <div class="RadScheduler">
                                        <div class="rsApt rsCategoryOrange">
                                            <div class="rsAptContent">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="legendtitle">
                                        <asp:Label runat="server" Text="signoff" />
                                        <telerik:RadButton runat="server" ID="radbuttoncheck_signoff" ButtonType="ToggleButton" ToggleType="CheckBox" Checked="true" OnCheckedChanged="radbuttoncheck_CheckedChanged" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 extraCol2 ">
                                <div class="legend">
                                    <div class="RadScheduler">
                                        <div class="rsApt rsCategoryViolet">
                                            <div class="rsAptContent">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="legendtitle">
                                        <asp:Label runat="server" Text="retro" />
                                        <telerik:RadButton runat="server" ID="radbuttoncheck_retro" ButtonType="ToggleButton" ToggleType="CheckBox" Checked="true" OnCheckedChanged="radbuttoncheck_CheckedChanged" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-2 floatPosition" style="width: 18%">
                    <asp:RadioButtonList ID="GroupBy" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="Product" Value="Product" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Milestone Category" Value="Category"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="container-fluid" style="height: 100%">
                <%--<br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />--%>
                <telerik:RadScheduler runat="server" ID="RadScheduler1" SelectedView="TimelineView" OverflowBehavior="Scroll"
                    DayStartTime="00:00:00" DayEndTime="23:59:59" Height="600" RowHeight="60"
                    DataKeyField="Key" DataSubjectField="Name" DataStartField="Start_Date" DataEndField="End_Date"
                    DataRecurrenceField="RecurrenceRule" DataRecurrenceParentKeyField="RecurrenceParentId"
                    TimelineView-ShowInsertArea="false"
                    TimelineView-ShowDateHeaders="true"
                    TimelineView-NumberOfSlots="14"
                    TimelineView-EnableExactTimeRendering="true"
                    TimelineView-HeaderDateFormat="MM-dd-yy"
                    TimelineView-ColumnHeaderDateFormat="MMM-dd"
                    ShowNavigationPane="true"
                    ShowFullTime="false"
                    ShowAllDayRow="false"
                    ShowFooter="false"
                    CustomAttributeNames="VsoItemLink,VsoItemLinkText,VsoItemHours,AssignedResources,VacationItems,PeopleAff,ProductAff"
                    OnAppointmentDataBound="RadScheduler1_AppointmentDataBound"
                    OnClientAppointmentClick="OnClientAppointmentClick"
                    Localization-HeaderMultiDay="Work Week"
                    Localization-ConfirmDeleteTitle="Confirm delete"
                    Localization-ConfirmDeleteText="Are you sure you want to delete it?"
                    AgendaView-NumberOfDays="50"
                    OnFormCreated="RadScheduler1_FormCreated"
                    OnClientAppointmentMoveEnd="OnClientAppointmentMoveEnd"
                    OnClientAppointmentResizeEnd="OnClientAppointmentResizeEnd"
                    OnAppointmentUpdate="RadScheduler1_AppointmentUpdate">

                    <TimeSlotContextMenus>
                        <telerik:RadSchedulerContextMenu ID="SchedulerTimeSlotContextMenu" runat="server">
                            <Items>
                                <telerik:RadMenuItem Text="Add New Milestone" Value="CommandAddNewMilestone" Visible="false" />
                            </Items>
                        </telerik:RadSchedulerContextMenu>
                    </TimeSlotContextMenus>

                    <AdvancedForm Modal="false"></AdvancedForm>

                    <ResourceStyles>
                        <telerik:ResourceStyleMapping Type="Category" Text="locstart" ApplyCssClass="rsCategoryBlue"></telerik:ResourceStyleMapping>
                        <telerik:ResourceStyleMapping Type="Category" Text="locready" ApplyCssClass="rsCategoryGreen"></telerik:ResourceStyleMapping>
                        <telerik:ResourceStyleMapping Type="Category" Text="signoff" ApplyCssClass="rsCategoryOrange"></telerik:ResourceStyleMapping>
                        <telerik:ResourceStyleMapping Type="Category" Text="retro" ApplyCssClass="rsCategoryViolet"></telerik:ResourceStyleMapping>
                        <telerik:ResourceStyleMapping Type="Category" Text="progressing" ApplyCssClass="rsCategoryYellow"></telerik:ResourceStyleMapping>
                        <telerik:ResourceStyleMapping Type="Category" Text="endgame" ApplyCssClass="rsCategoryRed"></telerik:ResourceStyleMapping>
                        <telerik:ResourceStyleMapping Type="Category" Text="testPlan" BackColor="IndianRed" ApplyCssClass="testPlanStyle"></telerik:ResourceStyleMapping>
                        <telerik:ResourceStyleMapping Type="Product" Text="Vacation" BackColor="LightBlue" ApplyCssClass="testPlanStyle"></telerik:ResourceStyleMapping>
                    </ResourceStyles>

                    <TimelineView UserSelectable="true"></TimelineView>
                    <AgendaView UserSelectable="true" />

                    <MonthView UserSelectable="false"></MonthView>
                    <MultiDayView UserSelectable="false"></MultiDayView>
                    <DayView UserSelectable="false"></DayView>
                    <WeekView UserSelectable="false"></WeekView>

                    <TimeSlotContextMenuSettings EnableDefault="true"></TimeSlotContextMenuSettings>
                    <AppointmentContextMenuSettings EnableDefault="true"></AppointmentContextMenuSettings>
                    <AppointmentContextMenus>
                        <telerik:RadSchedulerContextMenu>
                            <Items>
                                <telerik:RadMenuItem Text="Edit" Value="CommandEdit" />
                                <%--<telerik:RadMenuItem IsSeparator="True" />
                            <telerik:RadMenuItem Text="Delete" Value="CommandDelete" />--%>
                            </Items>
                        </telerik:RadSchedulerContextMenu>
                    </AppointmentContextMenus>

                    <AdvancedEditTemplate>
                        <div class="rsAdvancedEdit" style="position: relative">
                            <%-- Title bar. --%>
                            <div class="rsAdvTitle">
                                <%-- The rsAdvInnerTitle element is used as a drag handle when the form is modal. --%>
                                <h1 class="rsAdvInnerTitle">
                                    <asp:Label runat="server" ID="label_advanceEdit_title" Text="title"></asp:Label></h1>
                                <asp:LinkButton runat="server" ID="AdvancedEditCloseButton" CssClass="rsAdvEditClose"
                                    CommandName="Cancel" CausesValidation="false" ToolTip='close'>
                        close
                                </asp:LinkButton>
                            </div>
                            <div class="rsAdvContentWrapper">
                                <div class="container-fluid">
                                    <asp:Panel runat="server" ID="panel_MileStoneTestPlanRelease" Visible="false">
                                        <div class="row">
                                            <div class="col-md-2" style="width: 28%; margin-right: -5px; margin-left: 50px">
                                                <asp:Label ID="Label_displayName" runat="server"></asp:Label>
                                                <telerik:RadTextBox ID="radTextBox_onLineofSight_displayName" runat="server" Width="200px"></telerik:RadTextBox>
                                            </div>

                                            <div class="col-md-1" style="width: 20%; margin-right: -25px;">
                                                <telerik:RadComboBox ID="radcomboBox_onLineofSight_displayName"
                                                    runat="server" AutoPostBack="false" CausesValidation="false" Width="130px" AllowCustomText="true" Text="Enter Custom Category" Font-Italic="true" Font-Size="X-Small" />
                                            </div>
                                            <div class="col-md-2" style="width: 18%;">
                                                <asp:Label ID="label_assignedResources" runat="server"></asp:Label>
                                                <telerik:RadTextBox ID="radTextBox_onLineofSight_assignedResources" runat="server" Width="50px"></telerik:RadTextBox>
                                            </div>

                                            <div class="col-md-2" style="width: 15%;">
                                                <asp:Label runat="server" Text="From:"></asp:Label>
                                                <telerik:RadDatePicker ID="raddatepicker_onLineofSight_milestoneStartDate" runat="server" Width="100px">
                                                </telerik:RadDatePicker>
                                            </div>
                                            <div class="col-md-2" style="width: 20%; margin-right: -45px;">
                                                <asp:Label runat="server" Text="To:"></asp:Label>
                                                <telerik:RadDatePicker ID="raddatepicker_onLineofSight_milestoneEndDate" runat="server" Width="100px">
                                                </telerik:RadDatePicker>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="panel_Vacation" Visible="false">
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
                                            <div class="col-md-4" style="width: 25%">
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
                                                    <div class="col-md-4" style="width: 40%; margin-left: -120px;">

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
                                                            <telerik:RadAutoCompleteBox runat="server" AutoPostBack="true" ID="RadAutoCompleteBox_existingProducts" TextSettings-SelectionMode="Single" EmptyMessage="Please type here" InputType="Token" Width="300" DropDownWidth="180px" Delimiter=" ">
                                                                <WebServiceSettings Method="search_existingProductNames" Path="UtilsProductsInfo.aspx" />
                                                            </telerik:RadAutoCompleteBox>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <asp:Panel runat="server" ID="ButtonsPanel" CssClass="rsAdvancedSubmitArea">
                                    <div class="rsAdvButtonWrapper">
                                        <asp:LinkButton CommandName="Update" runat="server" ID="UpdateButton" CssClass="rsAdvEditSave">
                                        <span>Save</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="CancelButton" CssClass="rsAdvEditCancel" CommandName="Cancel"
                                            CausesValidation="false">
                                        <span>Cancel</span>
                                        </asp:LinkButton>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </AdvancedEditTemplate>
                </telerik:RadScheduler>

                <telerik:RadToolTip ID="RadToolTip1" runat="server" RelativeTo="Element" Position="BottomRight"
                    AutoCloseDelay="0" ShowEvent="FromCode" Width="250px">

                    <div id="contentContainer">
                        <div id="descriptionDiv">
                        </div>
                        From: <span id="startTime"></span>
                        To: <span id="endTime"></span>
                        <div></div>
                        <div id="vsoQueryLabel" style="display: inline">VSO-Query:</div>
                        <span id="vsoQuery" class="linkColor"></span>
                        <div></div>
                        <div id="vsoHourLabel" style="display: inline">VSO-Hours:</div>
                        <span id="vsoHours"></span>
                        <div id="assignedResourcesLabel" style="display: inline">Assigned Resources:</div>
                        <span id="assignedResources"></span>
                        <div></div>
                        <div id="peopleAffLabel" style="display: none">People Affected:</div>
                        <span id="peoplelAff"></span>
                        <div></div>
                        <div></div>
                        <div id="productAffLabel" style="display: none">Products Affected:</div>
                        <span id="productAff"></span>
                        <div></div>
                    </div>
                </telerik:RadToolTip>
            </div>
        </div>
    </asp:Panel>
    <div style="margin-top: 200px;">
        <asp:Label runat="server" ID="label_warning_cancelledProduct" ForeColor="Red" Style="margin-left: 380px; font-size: medium" Visible="false"></asp:Label>
    </div>
</div>
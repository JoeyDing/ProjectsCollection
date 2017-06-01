<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleWindows.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ScheduleWindows" %>

<span id="Span1" class="schedule-windows" runat="server">
    <asp:Panel runat="server" ID="panel_schedule_window" EnableViewState="false">
        <div class="modal fade" id="modal_addProject" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <div>
                            <asp:Label ID="label_window_addProject" runat="server" />
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        </div>
                    </div>
                    <div class="modal-body">
                        <asp:Panel runat="server" ID="radwindow_addProject">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label runat="server" Text="Project Name:" Width="100%"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadTextBox ID="radtextbox_projectName" runat="server" Width="100%"></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="validator_window_project_projectName" ControlToValidate="radtextbox_projectName"
                                            Text="Project Name cannot be empty." ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-4">
                                        <telerik:RadButton Width="100%" runat="server" Text="Add" ID="radbutton_window_addProject" OnClick="radbutton_window_addProject_Click"
                                            CssClass="style_radbutton" CausesValidation="false">
                                        </telerik:RadButton>
                                    </div>
                                    <div class="col-md-4">
                                        <telerik:RadButton Width="100%" runat="server" Text="Cancel" ID="radbutton_window_addProject_cancel"
                                            AutoPostBack="false"
                                            CssClass="style_radbutton" CausesValidation="false"
                                            OnClientClicked='<%# "function(button, args){CloseWindow(\""+ radwindow_addProject.ClientID + "\");}" %>'>
                                        </telerik:RadButton>
                                    </div>
                                    <div class="col-md-2"></div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <div>
                            <span runat="server">
                                <asp:Label ID="label_window_addRelease" runat="server" />
                            </span>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        </div>
                    </div>
                    <div class="modal-body">
                        <asp:Panel runat="server" ID="radwindow_addRelease">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label runat="server" Text="Release Name:" Width="100%"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadTextBox ID="radtextbox_ReleaseName" runat="server" Width="100%"></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="validator_window_Release_ReleaseName" ControlToValidate="radtextbox_ReleaseName"
                                            Text="Release Name cannot be empty." ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-4">
                                        <telerik:RadButton Width="100%" runat="server" Text="Add" ID="radbutton_window_addRelease" OnClick="radbutton_window_addRelease_Click"
                                            CssClass="style_radbutton" CausesValidation="false">
                                        </telerik:RadButton>
                                    </div>
                                    <div class="col-md-4">
                                        <telerik:RadButton Width="100%" runat="server" Text="Cancel" ID="radbutton_window_addRelease_cancel"
                                            AutoPostBack="true"
                                            CssClass="style_radbutton" CausesValidation="false"
                                            OnClientClicked='<%# "function(button, args){CloseWindow(\""+ radwindow_addRelease.ClientID + "\");}" %>'>
                                        </telerik:RadButton>
                                    </div>
                                    <div class="col-md-2"></div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <div>
                            <asp:Label ID="label_window_addMilestone" runat="server" />
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        </div>
                    </div>
                    <div class="modal-body">
                        <asp:Panel runat="server" ID="radwindow_addMilestone">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label runat="server" Text="Milestone Name:" Width="100%"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadTextBox ID="radtextbox_MilestoneName" runat="server" Width="100%"></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="validator_window_Milestone_MilestoneName" ControlToValidate="radtextbox_MilestoneName"
                                            Text="Milestone Name cannot be empty." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label runat="server" Text="Milestone Type: "></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="radcombobox_milestone_categories" runat="server" Width="100%" AutoPostBack="false" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <telerik:RadTabStrip Skin="Telerik" AutoPostBack="false" runat="server" MultiPageID="radmultipage_window_milestone" SelectedIndex="0" CausesValidation="false">
                                            <Tabs>
                                                <telerik:RadTab Text="Single Date" Width="150px"></telerik:RadTab>
                                                <telerik:RadTab Text="Range Date" Width="100px"></telerik:RadTab>
                                            </Tabs>
                                        </telerik:RadTabStrip>
                                        <telerik:RadMultiPage BorderColor="Black" runat="server" SkinID="Telerik" ID="radmultipage_window_milestone" SelectedIndex="0">
                                            <telerik:RadPageView runat="server" Style="padding: 5px">
                                                <div class="container-fluid">
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <asp:Label runat="server" Text="Milestone Date:" Width="100%"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8">
                                                            <telerik:RadDatePicker ID="raddatepicker_MilestoneDate" runat="server" Width="100%"></telerik:RadDatePicker>
                                                            <asp:RequiredFieldValidator runat="server" ID="validator_window_Milestone_MilestoneDate" ControlToValidate="raddatepicker_MilestoneDate"
                                                                Text="Milestone Date cannot be empty." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server">
                                                <div class="container-fluid" style="padding: 5px">
                                                    <div class="row">
                                                        <div class="col-md-5">
                                                            <asp:Label runat="server" Text="Milestone Start Date:" Width="100%"></asp:Label>
                                                            <telerik:RadDatePicker ID="raddatepicker_MilestoneStartDate" runat="server" Width="100%"></telerik:RadDatePicker>
                                                            <asp:RequiredFieldValidator runat="server" ID="validator_raddatepicker_MilestoneStartDate" ControlToValidate="raddatepicker_MilestoneStartDate"
                                                                Text="Milestone Date cannot be empty." ForeColor="Red"
                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>

                                                        <div class="col-md-2">
                                                        </div>

                                                        <div class="col-md-5">
                                                            <asp:Label runat="server" Text="Milestone End Date:" Width="100%"></asp:Label>
                                                            <telerik:RadDatePicker ID="raddatepicker_MilestoneEndDate" runat="server" Width="100%"></telerik:RadDatePicker>
                                                            <asp:RequiredFieldValidator runat="server" ID="validator_raddatepicker_MilestoneEndDate" ControlToValidate="raddatepicker_MilestoneEndDate"
                                                                Text="Milestone Date cannot be empty." ForeColor="Red"
                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </telerik:RadPageView>
                                        </telerik:RadMultiPage>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-4">
                                        <telerik:RadButton Width="100%" runat="server" Text="Add" ID="radbutton_window_addMilestone" OnClick="radbutton_window_addMilestone_Click"
                                            CssClass="style_radbutton" CausesValidation="false">
                                        </telerik:RadButton>
                                    </div>
                                    <div class="col-md-4">
                                        <telerik:RadButton Width="100%" runat="server" Text="Cancel" ID="radbutton_window_addMilestone_cancel"
                                            AutoPostBack="true"
                                            CssClass="style_radbutton" CausesValidation="false"
                                            OnClientClicked='<%# "function(button, args){CloseWindow(\""+ radwindow_addMilestone.ClientID + "\");}" %>'>
                                        </telerik:RadButton>
                                    </div>
                                    <div class="col-md-2"></div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</span>
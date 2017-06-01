<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleControlLight.ascx.cs" Inherits="SkypeIntlPortfolio.Ajax.UserControls.ScheduleControlLight" %>
<telerik:RadTreeView runat="server" ID="radtreeview_root">
    <NodeTemplate>
        <telerik:RadTreeView runat="server" ID="radtreeview_project_child">
            <Nodes>
                <%--project header--%>
                <telerik:RadTreeNode runat="server" Text="project">
                    <Nodes>
                        <%--release list--%>
                        <telerik:RadTreeNode runat="server">
                            <NodeTemplate>
                                <telerik:RadTreeView runat="server" ID="radtreeview_release_root">
                                    <NodeTemplate>
                                        <telerik:RadTreeView runat="server" ID="radtreeview_release_child">
                                            <Nodes>
                                                <%--release header--%>
                                                <telerik:RadTreeNode runat="server" Text="release">
                                                    <Nodes>
                                                        <%--milestone list--%>
                                                        <telerik:RadTreeNode runat="server">
                                                            <NodeTemplate>
                                                                <telerik:RadTreeView runat="server" ID="radtreeview_milestone_root">
                                                                    <NodeTemplate>
                                                                        <div class="container-fluid">
                                                                            <div class="row">
                                                                                <div class="col-md-3">
                                                                                    <asp:Label ID="label_milestoneName" runat="server">
                                                                                    </asp:Label>
                                                                                </div>
                                                                                <div class="col-md-9">
                                                                                    <div class="container-fluid">
                                                                                        <div class="row">
                                                                                            <div class="col-md-6">
                                                                                                <asp:Label runat="server" Text='From'></asp:Label>
                                                                                                <telerik:RadDatePicker ID="raddatepicker_milestone_from" AutoPostBack="true" runat="server"
                                                                                                    OnSelectedDateChanged="raddatepicker_milestone_SelectedDateChanged">
                                                                                                </telerik:RadDatePicker>
                                                                                            </div>
                                                                                            <div class="col-md-6">
                                                                                                <asp:Label runat="server" Text='To'></asp:Label>
                                                                                                <telerik:RadDatePicker ID="raddatepicker_milestone_to" AutoPostBack="true" runat="server"
                                                                                                    OnSelectedDateChanged="raddatepicker_milestone_SelectedDateChanged">
                                                                                                </telerik:RadDatePicker>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </NodeTemplate>
                                                                </telerik:RadTreeView>
                                                            </NodeTemplate>
                                                        </telerik:RadTreeNode>
                                                    </Nodes>
                                                </telerik:RadTreeNode>
                                            </Nodes>
                                        </telerik:RadTreeView>
                                    </NodeTemplate>
                                </telerik:RadTreeView>
                            </NodeTemplate>
                        </telerik:RadTreeNode>
                    </Nodes>
                </telerik:RadTreeNode>
            </Nodes>
        </telerik:RadTreeView>
    </NodeTemplate>
</telerik:RadTreeView>
<div style="margin-left: 48px">
    <asp:LinkButton runat="server" ID="linkButton_addNewProject" CausesValidation="false" Text="Add new Project" OnClick="linkButton_addNewProject_Click"></asp:LinkButton>
</div>
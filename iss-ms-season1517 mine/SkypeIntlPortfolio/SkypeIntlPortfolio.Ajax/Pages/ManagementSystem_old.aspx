<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManagementSystem_old.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.ManagementSystem_old" %>

<%@ Register Src="~/UserControls/Schedule/ScheduleControl.ascx" TagName="ScheduleControl" TagPrefix="local" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <telerik:RadAjaxManager runat="server" ID="radManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radListBox_products">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radpanelbar_products_root" LoadingPanelID="loadingPanel1" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel1">
    </telerik:RadAjaxLoadingPanel>

    <div style="height: 100%">
        <div class="panel panel-info" style="height: 100%;">
            <div class="panel-heading">
                <div>
                    Schedule view
                </div>
            </div>
            <div class="panel-body" style="height: 100%;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-2" style="border: solid; border-right: none; height: 500px; margin: 0px; padding: 0px;">
                            <telerik:RadListBox runat="server" ID="radListBox_products" AutoPostBack="true" CausesValidation="false"
                                OnSelectedIndexChanged="radListBox_products_SelectedIndexChanged" Width="100%" Height="490px"
                                SelectionMode="Multiple">
                            </telerik:RadListBox>
                        </div>
                        <div class="col-md-10" style="border: solid; height: 500px; overflow-y: auto">
                            <div id="div_mainContent" style="margin: 0px">
                                <div class="custom custom_transparent">
                                    <telerik:RadPanelBar runat="server" ID="radpanelbar_products_root" Width="100%" Skin="Telerik" BorderColor="Transparent">
                                        <ItemTemplate>
                                            <div style="margin: 10px;">
                                                <telerik:RadPanelBar runat="server" ID="radpanelbar_products_child" Width="100%" Skin="Telerik" BorderColor="Transparent">
                                                    <Items>
                                                        <telerik:RadPanelItem>
                                                            <HeaderTemplate>
                                                                <div style="background: transparent !important;">
                                                                    <a class="rpExpandable" style="float: left">

                                                                        <span class="rpExpandHandle"></span>
                                                                    </a>
                                                                    <div>
                                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "Parent.Parent.Parent.Attributes[\"ProductName\"]")%>'></asp:Label>
                                                                    </div>
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ContentTemplate>
                                                                <div style="padding-left: 20px;">
                                                                    <local:ScheduleControl runat="server" ID="custom_schedulecontrol" />
                                                                </div>
                                                            </ContentTemplate>
                                                        </telerik:RadPanelItem>
                                                    </Items>
                                                </telerik:RadPanelBar>
                                            </div>
                                        </ItemTemplate>
                                    </telerik:RadPanelBar>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <telerik:RadGantt AutoGenerateColumns="false" runat="server" ID="raddgant_milestones" AllowSorting="false" ReadOnly="true"
                        DayView-UserSelectable="false" YearView-UserSelectable="true"
                        ShowTooltip="false" ShowCurrentTimeMarker="false"
                        ShowFullTime="false" ShowFullWeek="true">
                        <%--    <DataBindings>
                            <TasksDataBindings IdField="Id" TitleField="Title" StartField="Start" EndField="End" />
                        </DataBindings>--%>
                        <Columns>
                            <telerik:GanttBoundColumn DataField="title">
                            </telerik:GanttBoundColumn>
                        </Columns>
                        <CustomTaskFields>
                        </CustomTaskFields>
                    </telerik:RadGantt>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
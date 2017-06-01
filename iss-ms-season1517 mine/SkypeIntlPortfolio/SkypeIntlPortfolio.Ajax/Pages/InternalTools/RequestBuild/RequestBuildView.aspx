<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RequestBuildView.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.InternalTools.RequestBuild.RequestBuildView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .multiColumn ul {
            width: 100%;
        }

        .multiColumn li {
            float: left;
            width: 25%;
        }

        .row {
            margin-left: 0px;
        }

        .gapToRightBorder {
            padding-left: 10px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function adjustLoadingPanelHeight() {
                $get("<%= loadingPanelRequestBuild.ClientID %>").style.height = document.documentElement.scrollHeight + "px";
            }
        </script>
    </telerik:RadCodeBlock>

    <telerik:RadAjaxManager runat="server" ID="radManager1" ClientEvents-OnRequestStart="adjustLoadingPanelHeight">
        <AjaxSettings>

            <telerik:AjaxSetting AjaxControlID="radButton_AddLctFilePath">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_RequestBuild" LoadingPanelID="loadingPanelRequestBuild" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radTextBox_SourcePath">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_RequestBuild" LoadingPanelID="loadingPanelRequestBuild" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radioButtonList_BuildType">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_RequestBuild" LoadingPanelID="loadingPanelRequestBuild" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radButton_Submit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_RequestBuild" LoadingPanelID="loadingPanelRequestBuild" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanelRequestBuild" Style="position: absolute; top: 0; left: 0" Width="100%" Height="100%" IsSticky="true">
    </telerik:RadAjaxLoadingPanel>
    <asp:Panel runat="server" ID="panel_RequestBuild">
        <div class="container-fluid" style="margin-top: 10px;">
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server" Text="Select a build type" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-10">
                    <asp:RadioButtonList runat="server" ID="radioButtonList_BuildType" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="radioButtonList_BuildType_SelectedIndexChanged">
                        <asp:ListItem>skype lct builds</asp:ListItem>
                        <asp:ListItem>lync server</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server" Text="Source Path: " Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-5">
                    <telerik:RadTextBox runat="server" ID="radTextBox_SourcePath" AutoPostBack="true" Width="580px" OnTextChanged="radTextBox_SourcePath_TextChanged"></telerik:RadTextBox>
                    <asp:RequiredFieldValidator ForeColor="Red" runat="server" ControlToValidate="radTextBox_SourcePath" Text="Source Path should not be empty" Display="Dynamic" EnableClientScript="true">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-md-5">
                    <asp:Label runat="server" ID="label_PathNotice" ForeColor="Orange"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-5">
                    <asp:Label runat="server" ID="label_SourcePathNotValid" ForeColor="Red" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server" Text="From Build Version : " Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-2">
                    <telerik:RadTextBox runat="server" ID="radTextBox_FromVersion" Width="220"></telerik:RadTextBox>
                </div>
                <div class="col-md-8">
                    <asp:Label runat="server" ID="label_BuildVersionNotice" ForeColor="Orange"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-10">
                    <asp:Label runat="server" ID="label_BuildVersionFormat" Visible="false" ForeColor="Red"></asp:Label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <asp:Label runat="server" Text="Product Name : " Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-10">
                    <telerik:RadTextBox runat="server" ID="radTextBox_ProductName"></telerik:RadTextBox>
                    <asp:RequiredFieldValidator ForeColor="Red" runat="server" ControlToValidate="radTextBox_ProductName" Text="Product Name should not be empty" Display="Dynamic" EnableClientScript="true">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-10">
                    <asp:Label runat="server" ID="label_AtLeastOneTenantOrComponent" Text="Choose at least a tenant or component for lync server" Visible="false" ForeColor="Red"></asp:Label>
                </div>
            </div>
            <asp:Panel runat="server" ID="panel_Tenant" Visible="false">
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="Tenant : " Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <telerik:RadComboBox runat="server" ID="radComboBox_Tenant">
                            <Items>
                                <telerik:RadComboBoxItem Text="none" />
                                <telerik:RadComboBoxItem Text="lcsloc" />
                                <telerik:RadComboBoxItem Text="lcscmf" />
                                <telerik:RadComboBoxItem Text="lcsadminux" />
                                <telerik:RadComboBoxItem Text="lcscollab_platform" />
                                <telerik:RadComboBoxItem Text="lcsmsg" />
                                <telerik:RadComboBoxItem Text="lcsentwebapp" />
                                <telerik:RadComboBoxItem Text="lcslac" />
                                <telerik:RadComboBoxItem Text="lcsadminux" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="panel_SkypeLctFilePath">
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="LctFile Path : " Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-6">
                        <telerik:RadTextBox runat="server" ID="radTextBox_LctFilePath" Width="700px" ></telerik:RadTextBox>
                    </div>
                    <div class="col-md-4">
                        <telerik:RadButton runat="server" Text="Add" ID="radButton_AddLctFilePath" OnClick="radButton_AddLctFilePath_Click"></telerik:RadButton>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-10">
                        <asp:Label runat="server" ID="label_lctFilePathValidation" Visible="false" ForeColor="Red"></asp:Label>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-10">
                        <telerik:RadGrid
                            runat="server"
                            ID="radGrid_LctFilePaths"
                            OnItemCommand="radGird_LctFilePaths_ItemCommand">
                            <MasterTableView AutoGenerateColumns="false" EnableHierarchyExpandAll="true" EnableGroupsExpandAll="true">
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="File Path" HeaderStyle-Width="90%" DataField="LctFilePath">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn HeaderText="Delete" CommandName="Delete" HeaderStyle-Width="10%" ConfirmText="Delete this File?" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                                        <ItemStyle CssClass="gapToRightBorder" />
                                    </telerik:GridButtonColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="panel_LyncServerComponentList" Visible="false">
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="Lync Server Component List : " Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <telerik:RadListBox runat="server" CheckBoxes="true" ShowCheckAll="true" Height="300" Width="100%" CssClass="multiColumn" ID="radListBox_LyncServerComponentList">
                            <Items>
                                <telerik:RadListBoxItem Text="Auditorium" />
                                <telerik:RadListBoxItem Text="CommonWXL" />
                                <telerik:RadListBoxItem Text="ConvHistory" />
                                <telerik:RadListBoxItem Text="DataCollabWeb" />
                                <telerik:RadListBoxItem Text="Deployment" />
                                <telerik:RadListBoxItem Text="DialinOnline" />
                                <telerik:RadListBoxItem Text="ITPro" />
                                <telerik:RadListBoxItem Text="LOCP" />
                                <telerik:RadListBoxItem Text="LWA" />
                                <telerik:RadListBoxItem Text="LWA_W15CU_Signoff" />
                                <telerik:RadListBoxItem Text="ManagementCore" />
                                <telerik:RadListBoxItem Text="MCXPNCH" />
                                <telerik:RadListBoxItem Text="OnlineConnector" />
                                <telerik:RadListBoxItem Text="OnlineConnector_TRPS" />
                                <telerik:RadListBoxItem Text="OnlineConnectorInstaller" />
                                <telerik:RadListBoxItem Text="PlanTool" />
                                <telerik:RadListBoxItem Text="ReachClient" />
                                <telerik:RadListBoxItem Text="RGS" />
                                <telerik:RadListBoxItem Text="Server" />
                                <telerik:RadListBoxItem Text="ServerWXL" />
                                <telerik:RadListBoxItem Text="SetupMSI" />
                                <telerik:RadListBoxItem Text="UCMA" />
                                <telerik:RadListBoxItem Text="WebAppMSI" />
                                <telerik:RadListBoxItem Text="WebComponents_DIJL" />
                                <telerik:RadListBoxItem Text="WebComponents_OCTAB" />
                                <telerik:RadListBoxItem Text="WebComponents_PassiveAuth" />
                                <telerik:RadListBoxItem Text="WebComponents_RMWebApp" />
                                <telerik:RadListBoxItem Text="WebScheduler" />
                            </Items>
                        </telerik:RadListBox>
                    </div>
                </div>
            </asp:Panel>
            <br />
            <br />
            <br />
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-10">
                    <telerik:RadButton runat="server" Text="Submit" ID="radButton_Submit" OnClick="radButton_Submit_Click" Width="100" Height="50" Font-Size="Large"></telerik:RadButton>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-10">
                    <asp:Label runat="server" ID="label_SubmitSuccess" Text="Request has been submitted, an email with your build info has been sent for review!" Visible="false" ForeColor="Green"></asp:Label>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

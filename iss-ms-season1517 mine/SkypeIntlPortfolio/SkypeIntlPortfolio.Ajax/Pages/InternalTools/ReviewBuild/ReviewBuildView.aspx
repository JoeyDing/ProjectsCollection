<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReviewBuildView.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.InternalTools.ReviewBuild.ReviewBuildView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .multiColumn ul {
            width: 100%;
        }

        .multiColumn li {
            float: left;
            width: 50%;
        }

        .row {
            margin-left: 0px;
        }

        .gapToRightBorder {
            padding-left: 170px !important;
        }

        .maximize input[type=text] {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function adjustLoadingPanelHeight() {
                $get("<%= loadingPanelRequestBuild.ClientID %>").style.height = document.documentElement.scrollHeight + "px";
            }

            //Sys.Application.add_load(new function () {

            //    $('# a').attr('target', '_blank').attr('title', 'This link will open in a new window.');

            //});
        </script>
    </telerik:RadCodeBlock>

    <telerik:RadAjaxManager runat="server" ID="radManager1" ClientEvents-OnRequestStart="adjustLoadingPanelHeight">
        <AjaxSettings>

            <telerik:AjaxSetting AjaxControlID="radButton_RefreshSkypeXml">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_ReviewBuild" LoadingPanelID="loadingPanelRequestBuild" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radButton_RefreshCmdLine">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_ReviewBuild" LoadingPanelID="loadingPanelRequestBuild" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radButton_Start">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_ReviewBuild" LoadingPanelID="loadingPanelRequestBuild" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radButton_CreateBranchID">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_ReviewBuild" LoadingPanelID="loadingPanelRequestBuild" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radListBox_StartOptions">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_ReviewBuild" LoadingPanelID="loadingPanelRequestBuild" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radTextBox_ProductName">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="panel_ReviewBuild" LoadingPanelID="loadingPanelRequestBuild" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanelRequestBuild" Style="position: absolute; top: 0; left: 0" Width="100%" Height="100%" IsSticky="true">
    </telerik:RadAjaxLoadingPanel>
    <asp:Panel runat="server" ID="panel_ReviewBuild">
        <div class="panel panel-primary">
            <div class="panel-heading">1. Please review all the fields of the reuqest</div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-10">
                        <asp:Label runat="server" ID="label_ErrorMsg" Visible="false" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="Build Type:" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:Label runat="server" ID="label_BuildType"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="Build To Keep:" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <telerik:RadNumericTextBox runat="server" ID="radTextBox_BuildToKeep" Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="">
                        </telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ForeColor="Red" runat="server" ControlToValidate="radTextBox_BuildToKeep" Text="Build To Keep should not be empty" Display="Dynamic" EnableClientScript="true">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-8">
                        <asp:Label runat="server" Text="Hint: Use the wheel on the mouse to modify number" ForeColor="Orange"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="Source Path: " Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <telerik:RadTextBox runat="server" ID="radTextBox_SourcePath" Width="580px"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ForeColor="Red" runat="server" ControlToValidate="radTextBox_SourcePath" Text="Source Path should not be empty" Display="Dynamic" EnableClientScript="true">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="Destination Path: " Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <telerik:RadTextBox runat="server" ID="radTextBox_DestinationPath" Width="580px"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ForeColor="Red" runat="server" ControlToValidate="radTextBox_DestinationPath" Text="Destination Path should not be empty" Display="Dynamic" EnableClientScript="true">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="From Build Version : " Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <telerik:RadTextBox runat="server" ID="radTextBox_FromVersion"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-10">
                        <asp:Label runat="server" ID="label_BuildVersionFormat" Text="Build Version can only contain digits and dots" Visible="false" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="Branch ID : " Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <telerik:RadTextBox runat="server" ID="radTextBox_BranchID" Enabled="false"></telerik:RadTextBox>
                    </div>
                    <div class="col-md-8">
                        <telerik:RadButton runat="server" ID="radButton_CreateBranchID" OnClick="radButton_CreateBranchID_Click" Text="Create branch ID" Visible="false"></telerik:RadButton>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="Branch Name : " Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <telerik:RadTextBox runat="server" ID="radTextBox_BranchName"></telerik:RadTextBox>
                        <asp:RequiredFieldValidator ForeColor="Red" runat="server" ControlToValidate="radTextBox_BranchName" Text="Branch Name should not be empty" Display="Dynamic" EnableClientScript="true">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="Product Name : " Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <telerik:RadTextBox runat="server" AutoPostBack="true" ID="radTextBox_ProductName" OnTextChanged="radTextBox_ProductName_TextChanged"></telerik:RadTextBox>
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
                                    <telerik:RadComboBoxItem Text="none" Value="none" />
                                    <telerik:RadComboBoxItem Text="lcsloc" Value="lcsloc" />
                                    <telerik:RadComboBoxItem Text="lcscmf" Value="lcscmf" />
                                    <telerik:RadComboBoxItem Text="lcsadminux" Value="lcsadminux" />
                                    <telerik:RadComboBoxItem Text="lcscollab_platform" Value="lcscollab_platform" />
                                    <telerik:RadComboBoxItem Text="lcsmsg" Value="lcsmsg" />
                                    <telerik:RadComboBoxItem Text="lcsentwebapp" Value="lcsentwebapp" />
                                    <telerik:RadComboBoxItem Text="lcslac" Value="lcslac" />
                                    <telerik:RadComboBoxItem Text="lcsadminux" Value="lcsadminux" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <br />
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

                <asp:Panel runat="server" ID="panel_SkypeXml">
                    <div class="row">
                        <div class="col-md-2">
                            <asp:Label runat="server" Text="LctFile Paths : " Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-md-10">

                            <asp:Label runat="server" ID="label_SkypeLctFilePaths"></asp:Label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">
                        </div>
                        <div class="col-md-10">
                            <asp:Label runat="server" ID="label_lctFilePathValidation" Visible="false" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <asp:Panel runat="server" ID="panel_ModifyXml">
                        <div class="row">
                            <div class="col-md-2">
                                <asp:Label runat="server" Text="Modify Xml here : " Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <telerik:RadGrid
                                    runat="server"
                                    ID="radGrid_SkypeXml"
                                    OnNeedDataSource="radGrid_SkypeXml_NeedDataSource"
                                    OnInsertCommand="radGrid_SkypeXml_InsertCommand"
                                    OnUpdateCommand="radGrid_SkypeXml_UpdateCommand"
                                    OnItemCommand="radGrid_SkypeXml_ItemCommand">
                                    <MasterTableView
                                        AutoGenerateColumns="false"
                                        EnableGroupsExpandAll="true"
                                        EditMode="InPlace"
                                        CommandItemDisplay="Top">
                                        <Columns>
                                            <telerik:GridEditCommandColumn UpdateText="Update" EditText="Edit" CancelText="Cancel">
                                                <HeaderStyle Width="5%" />
                                                <ItemStyle CssClass="editStyle"></ItemStyle>
                                            </telerik:GridEditCommandColumn>
                                            <telerik:GridBoundColumn HeaderText="File Path" DataField="Name" HeaderStyle-Width="40%" ItemStyle-CssClass="maximize">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Lcg Name" DataField="LcgFileName" HeaderStyle-Width="40%" ItemStyle-CssClass="maximize">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderText="Override" DataField="Override" HeaderStyle-Width="17%" ItemStyle-CssClass="maximize">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn HeaderText="Delete" CommandName="Delete" ConfirmText="Delete this File?" HeaderStyle-Width="3%" ButtonType="ImageButton" ImageUrl="~/Images/red_x_mark.png">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                <asp:Label runat="server" Text="Skype Xml : " Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <telerik:RadTextBox runat="server" ID="radTextBox_SkypeXml" TextMode="MultiLine" Width="600px" Height="300px" Enabled="false" Font-Bold="true"></telerik:RadTextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-10">
                                <telerik:RadButton runat="server" Text="Regenerate Xml" ID="radButton_RefreshSkypeXml" OnClick="radButton_RefreshSkypeXml_Click"></telerik:RadButton>
                            </div>
                        </div>
                    </asp:Panel>

                    <br />
                    <br />
                </asp:Panel>
            </div>
            <div class="panel-heading">2. Click the button to regenerate command line based on new user input </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="Copy Command Line : " Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:Label runat="server" ID="label_CopyCmdLine"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="Import Command Line : " Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:Label runat="server" ID="label_ImportCmdLine"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" Text="DW Import Command Line : " Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:Label runat="server" ID="label_DWImportCmdLine"></asp:Label>
                    </div>
                </div>
                <br />
                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-10">
                        <telerik:RadButton runat="server" Text="Regenerate Cmd Line" ID="radButton_refreshCmdLine" OnClick="radButton_refreshCmdLine_Click"></telerik:RadButton>
                    </div>
                </div>
                <br />
                <br />
            </div>
            <div class="panel-heading">3. Choose one in the start opstions and Click the button to submit </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-2">
                        <telerik:RadListBox runat="server" CheckBoxes="true" ID="radListBox_StartOptions" AutoPostBack="true" OnItemCheck="radListBox_StartOptions_ItemCheck">
                            <Items>
                                <telerik:RadListBoxItem Text="Save skype config" />
                                <telerik:RadListBoxItem Text="Create Sql jobs" />
                                <telerik:RadListBoxItem Text="Create PD" />
                            </Items>
                        </telerik:RadListBox>
                    </div>
                    <div class="col-md-2">
                        <telerik:RadButton runat="server" Text="Start" ID="radButton_Start" OnClick="radButton_Start_Click" Enabled="false"></telerik:RadButton>
                    </div>
                    <div class="col-md-8">
                        <asp:Label runat="server" ID="label_StartSuccess" Visible="false" ForeColor="Green"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-10">
                        <asp:Label runat="server" ID="label_ErrorMsgBottom" Visible="false" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
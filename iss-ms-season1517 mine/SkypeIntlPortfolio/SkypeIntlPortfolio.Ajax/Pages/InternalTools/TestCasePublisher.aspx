<%@ Page ValidateRequest="false" EnableViewState="true" Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TestCasePublisher.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.InternalTools.TestCasePublisher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .multiColumn ul {
            width: 100%;
        }

        .multiColumn li {
            float: left;
            width: 25%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">
            var loadingPanel = "";
            var pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();
            var postBackElement = "";
            pageRequestManager.add_initializeRequest(initializeRequest);
            pageRequestManager.add_endRequest(endRequest);

            function initializeRequest(sender, eventArgs) {
                loadingPanel = $find("<%= loadingPanel.ClientID %>");
                //postBackElement = eventArgs.get_postBackElement().id;
                postBackElement = "<%= panel_main.ClientID %>";
                loadingPanel.show(postBackElement);
            }

            function endRequest(sender, eventArgs) {
                loadingPanel = $find("<%= loadingPanel.ClientID %>");
                loadingPanel.hide(postBackElement);
            }

            function ValidationCriteria(source, args) {
                var listbox = $find('<%= radlistbox_languages.ClientID %>');
                args.IsValid = listbox.get_checkedItems().length > 0;
            }

            function fileUploaded(sender, args) {

                var btn = document.getElementById("<%= btnDummy.ClientID %>");
                btn.click();

            }

            function OnClientSelectedIndexChanging(sender, args) {
                args.set_cancel(true);
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel">
    </telerik:RadAjaxLoadingPanel>

    <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="updatePanel_main" UpdateMode="Always">
        <ContentTemplate>
            <asp:Panel runat="server" ID="panel_main">

                <div class="panel panel-primary">
                    <div class="panel-heading">1 .Please input a planId below</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-2">
                                PlanID :
                            </div>
                            <div class="col-md-10">
                                <%--<telerik:RadTextBox runat="server" Text="13345" ID="txtPlanID"></telerik:RadTextBox>--%>
                                <telerik:RadNumericTextBox runat="server" AutoPostBack="true" ID="txtPlanID" OnTextChanged="txtPlanID_TextChanged" Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator=""></telerik:RadNumericTextBox>
                                <span style="color: red">*</span>
                                <asp:Label ID="label_errorMsg" runat="server" ForeColor="Red" Visible="false" Text="Could not find test plan. Please try again."></asp:Label>
                                <asp:RequiredFieldValidator ID="requiredFieldValidator_planId" ForeColor="Red" runat="server" ControlToValidate="txtPlanID" Text="Plan id should not be empty" Display="Dynamic" EnableClientScript="true">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">Plan Name :</div>
                            <div class="col-md-10">
                                <asp:Label runat="server" ID="label_testPlanName"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">Plan Url :</div>
                            <div class="col-md-10">
                                <asp:HyperLink runat="server" ID="hyperlink_testPlanUrl" Target="_blank"></asp:HyperLink>
                                <%--<asp:Label runat="server" ID="label_testPlanUrl"></asp:Label>--%>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">How to create testcases :</div>
                            <div class="col-md-10">
                                <telerik:RadTabStrip runat="server" ID="radTabStrip_testCaseOptions" Orientation="HorizontalTop" Width="317px" Visible="false" AutoPostBack="true" OnTabClick="radTabStrip_testCaseOptions_TabClick"
                                    MultiPageID="RadMultiPage_testCaseOptions">
                                    <Tabs>
                                        <telerik:RadTab Text="Language sample">
                                        </telerik:RadTab>

                                        <telerik:RadTab Text="Case Query URL">
                                        </telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                                <br />
                                <telerik:RadMultiPage runat="server" ID="RadMultiPage_testCaseOptions" CssClass="multiPage" Width="600px">

                                    <telerik:RadPageView runat="server" ID="RadPageView1">
                                        <telerik:RadComboBox runat="server" ID="radcombobox_suites">
                                        </telerik:RadComboBox>
                                    </telerik:RadPageView>

                                    <telerik:RadPageView runat="server" ID="RadPageView2">
                                        <telerik:RadTextBox runat="server" ID="radtextbox_caseQuery" Width="600px">
                                        </telerik:RadTextBox>
                                        <asp:Label ID="label_WrongQueryURL" runat="server" ForeColor="Red" Visible="false" Text="Could not find testcases using this case query URL"></asp:Label>
                                    </telerik:RadPageView>
                                </telerik:RadMultiPage>
                                <%--<telerik:RadButton ID="radbutton_1" runat="server" Checked="true" Enabled="false" ToggleType="Radio" ButtonType="StandardButton" GroupName="StandardButton" AutoPostBack="true" OnClick="radbutton_1_Click">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Language sample" PrimaryIconCssClass="rbToggleRadioChecked" />
                                        <telerik:RadButtonToggleState Text="Language sample" PrimaryIconCssClass="rbToggleRadio" />
                                    </ToggleStates>
                                </telerik:RadButton>
                                <telerik:RadButton ID="radbutton_2" runat="server" Enabled="false" ToggleType="Radio" ButtonType="StandardButton" GroupName="StandardButton" AutoPostBack="true" OnClick="radbutton_2_Click">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Case Query URL" PrimaryIconCssClass="rbToggleRadioChecked" />
                                        <telerik:RadButtonToggleState Text="Case Query URL" PrimaryIconCssClass="rbToggleRadio" />
                                    </ToggleStates>
                                </telerik:RadButton>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="panel panel-primary">
                    <div class="panel-heading">2. Please upload a csv file as a language template from your computer to load the listbox or go to step3</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-2">
                                Upload Template
                            </div>
                            <div class="col-md-10">
                                <telerik:RadAsyncUpload runat="server" ID="radAsyncUpload_Template" MultipleFileSelection="Disabled" OnClientFileUploaded="fileUploaded"
                                    Enabled="false" AllowedFileExtensions="csv" />
                                <asp:Label runat="server" ID="label_wrongTemplate" Text="The Uploaded file is not a correct csv Template." ForeColor="Red" Visible="false"></asp:Label>
                                <asp:Button ID="btnDummy" runat="server" OnClick="btnDummy_Click" Style="display: none;" />
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="panel panel-primary">
                    <div class="panel-heading">3. Select languages of the test suites which will be published and if needed save the template as a csv file by clicking the "save checked language package" button</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-2">
                                Save Template
                            </div>
                            <div class="col-md-10">
                                <telerik:RadButton ID="radbutton_download" runat="server" Text="Save Checked Language Package" OnClick="radbutton_download_Click"></telerik:RadButton>
                                <asp:Label runat="server" ID="label_listboxNotChecked" Text="Select at least one language from the listbox." ForeColor="Red" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2"></div>
                            <div class="col-md-10">
                                <asp:Label runat="server" Text="Notice: Please choose AT MOST 30 Test cases to submit;if more than 30,refresh this page and submit the rest again!" ForeColor="DarkOrange"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2"></div>
                            <div class="col-md-10">
                                <asp:Label runat="server" Text="Notice: Test cases in the language sample will be merged into Test cases in already existed suite (the ones in yellow below)!" ForeColor="DarkOrange"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">Language populated :</div>
                            <div class="col-md-10">
                                <telerik:RadListBox runat="server" ID="radlistbox_languages" AutoPostBack="true" CheckBoxes="true" ShowCheckAll="true" Height="300" Width="100%" CssClass="multiColumn"
                                    OnClientSelectedIndexChanging="OnClientSelectedIndexChanging">
                                </telerik:RadListBox>
                                <asp:Label runat="server" ID="label_languagesNotSelected" Text="Select at least one language." ForeColor="Red" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="panel panel-primary">
                    <div class="panel-heading">4. press the submit button when finished</div>
                    <div class="panel-body">
                        <div class="row">

                            <div class="col-md-2">
                                <telerik:RadButton CausesValidation="true" runat="server" ID="btnSubmit" Text="Submit" Enabled="false" OnClick="btnSubmit_Click" Width="100" Height="50" Font-Size="Large">
                                </telerik:RadButton>
                            </div>
                            <div class="col-md-10">
                                <asp:Label runat="server" ID="label_submitSuccess" ForeColor="Green" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="radbutton_download" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
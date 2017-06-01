<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MultiLanguageBugLogger.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.InternalTools.MultiLanguageBugLogger" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .multiColumn ul {
            width: 100%;
        }

        .multiColumn li {
            float: left;
            width: 50%;
        }

        .multiColumn .RadListBox .rlbItem {
            padding-top: 25px;
            margin-top: 0px;
            width: 80%;
        }

        html, body {
            width: 100%;
        }

        .row {
            margin: 0px;
        }

        .labelPosition.RadListBox .rlbText, .RadListBox .rlbTemplate {
            margin-top: -25px;
            margin-left: 15px;
        }

        .test .ruInputs li {
            float: none;
        }

        .test .ruFakeInput {
            height: 12px !important;
            width: 60px !important;
        }

        .test .ruButton {
            height: 18px !important;
        }

        .RadUploadZip {
            width: 300px !important;
        }

        .RadListBox_Metro .rlbItem.rlbSelected {
            background-color: white !important;
        }

        .RadListBox_Metro .rlbItem.rlbHovered {
            background-color: white !important;
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

            function OnClientSelectedIndexChanging(sender, args) {
                args.set_cancel(true);

            }
            function OnRadAsyncUpload1ClientAction(source, args) {
                var btn = document.getElementById("<%= btnDummy.ClientID %>");
                btn.click();
                //__doPostBack('AsyncUploadZip', args);
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="AsyncUploadZip">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radlistbox_languages_new"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel">
    </telerik:RadAjaxLoadingPanel>

    <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="updatePanel_main" UpdateMode="Always">
        <ContentTemplate>
            <asp:Panel runat="server" ID="panel_main">

                <div class="panel panel-primary">
                    <div class="panel-heading">1. Please input sample bugId below</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-2">
                                Sample BugID :
                            </div>
                            <div class="col-md-10">
                                <telerik:RadNumericTextBox runat="server" AutoPostBack="true" ID="radtextbox_bugID" OnTextChanged="radtextbox_bugID_TextChanged" Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="">
                                </telerik:RadNumericTextBox>
                                <asp:Label ID="label_errorMsg" runat="server" ForeColor="Red" Visible="false" Text="Could not find Bug using this ID. Please try again."></asp:Label>
                                <asp:RequiredFieldValidator ID="requiredFieldValidator_planId" ForeColor="Red" runat="server" ControlToValidate="radtextbox_bugID" Text="Bug Id should not be empty" Display="Dynamic" EnableClientScript="true">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">
                                Sample Bug Language :
                            </div>
                            <div class="col-md-10">
                                <asp:Label runat="server" ID="label_sampleBugLanguage"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                Sample Title :
                            </div>
                            <div class="col-md-10">
                                <asp:Label runat="server" ID="label_sampleTitle"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                Assigned To :
                            </div>
                            <div class="col-md-10">
                                <asp:Label runat="server" ID="label_sampleBugAssignedTo"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <br />
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        2. Please input template title for child bugs
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-2"></div>
                            <div class="col-md-10">
                                <asp:Label runat="server" ForeColor="DarkOrange" Text="Notice:Template tile must contain keyword [lang] which will be replaced by checked language name in the listbox to generate child bug titles"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                Template Title :
                            </div>
                            <div class="col-md-10">
                                <telerik:RadTextBox runat="server" ID="radtextbox_templateTitle" Width="80%"></telerik:RadTextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2"></div>
                            <div class="col-md-10">
                                <asp:Label runat="server" ID="label_templateTitleWrong" ForeColor="Red" Text="Template title must contain keyword [lang]" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                Triaged :
                            </div>
                            <div class="col-md-10">
                                No
                            </div>
                        </div>
                    </div>
                </div>

                <br />

                <div class="panel panel-primary">
                    <div class="panel-heading">
                        3. Please input child bugs tester name
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-2">
                                Child Bugs Assigned to
                            </div>
                            <div class="col-md-10">
                                <telerik:RadAutoCompleteBox runat="server" ID="radautocompletebox_childBugsAssignedTo" DropDownHeight="150" EmptyMessage="Select Tester Names" InputType="Text" TextSettings-SelectionMode="Single" Delimiter=" " Width="230">
                                    <WebServiceSettings Method="search_testerNames" Path="MultiLanguageBugLogger.aspx" />
                                </telerik:RadAutoCompleteBox>
                                <%--<telerik:RadTextBox runat="server" ID="radtextbox_childAssignedTo"></telerik:RadTextBox>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="panel panel-primary">
                    <div class="panel-heading">
                        4. Please input parent bug title if creating a new parent bug
                    </div>
                    <div class="panel-body">
                        <div class="row">

                            <div class="col-md-2">
                                <asp:Label runat="server" ID="label_existing" Text="Existing Parent Bug Title:" Visible="false"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:Label runat="server" ID="label_existingParentBugTitle" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                <asp:Label runat="server" ID="label_new" Text="New Parent Bug Title:" Visible="false"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <telerik:RadTextBox runat="server" ID="radtextbox_newParentBugTitle" Visible="false" Width="80%"></telerik:RadTextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2"></div>
                            <div class="col-md-10">
                                <asp:RadioButtonList runat="server" ID="radiobuttonlist_parentBug" RepeatDirection="Horizontal" Visible="false" Enabled="false">
                                    <asp:ListItem>Use  new parent bug as parent</asp:ListItem>
                                    <asp:ListItem>Use sample bug as parent</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">
                                <asp:Label runat="server" Text="Parent Bug Url :" ID="label_parentBugUrl" Visible="false"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:HyperLink runat="server" ID="hyperlink_parentBugUrl" Target="_blank"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="panel panel-primary">
                    <div class="panel-heading">
                        5.In order to upload attachments to each child bugs,please choose attachments to upload or upload files for each checked language
                    </div>
                    <div class="panel-body">
                        <br />
                        <telerik:RadTabStrip ID="RadTabStrip_ImageLoader" Orientation="HorizontalTop" runat="server" Skin="MetroTouch" MultiPageID="RadMultiPageImageLoader" SelectedIndex="0" Enabled="false">
                            <Tabs>
                                <telerik:RadTab Text="Upload Seperate Files" runat="server">
                                </telerik:RadTab>
                                <telerik:RadTab Text="Upload Zip" runat="server">
                                </telerik:RadTab>
                            </Tabs>
                        </telerik:RadTabStrip>
                        <telerik:RadMultiPage runat="server" ID="RadMultiPageImageLoader" SelectedIndex="0" CssClass="multiPage">
                            <telerik:RadPageView runat="server" ID="RadPageView1">
                                <div class="row">
                                    <div class="col-md-2">Language input:</div>
                                    <div class="col-md-10">
                                        <telerik:RadAutoCompleteBox RenderMode="Lightweight" runat="server" ID="RadAutoCompleteBox_Language" EmptyMessage="Please type here" InputType="Token" Width="350" DropDownWidth="350px" OnEntryAdded="RadAutoCompleteBox_Language_EntryAdded" OnEntryRemoved="RadAutoCompleteBox_Language_EntryRemoved">
                                            <WebServiceSettings Method="search_Language" Path="MultiLanguageBugLogger.aspx" />
                                        </telerik:RadAutoCompleteBox>
                                    </div>
                                    <div class="col-md-2" style="margin-top: 5px">Language populated :</div>
                                    <div class="col-md-10" style="margin-top: 5px">
                                        <telerik:RadListBox runat="server" ID="radlistbox_languages" CheckBoxes="true" ShowCheckAll="true" CssClass="multiColumn labelPosition" Height="300" Width="100%" OnClientSelectedIndexChanging="OnClientSelectedIndexChanging">
                                            <ItemTemplate>
                                                <div class="container-fluid">
                                                    <div class="row" style="margin-left: -8px;">
                                                        <div class="col-md-12">
                                                            <asp:Label runat="server" ID="languageLabel"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="margin-left: -8px;">
                                                        <div class="col-md-12">
                                                            <asp:Label runat="server" Text="Attached files"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <telerik:RadAsyncUpload runat="server" ID="AsyncUploadManully" MultipleFileSelection="Automatic" CssClass="test"></telerik:RadAsyncUpload>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </telerik:RadListBox>
                                        <asp:Label runat="server" ID="label_languagesNotSelected" Text="Select at least one language." ForeColor="Red" Visible="false"></asp:Label>
                                    </div>
                                </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView runat="server">
                                </br>
                                <div class="row" style="margin-left: 50px">

                                    <asp:Label ID="lbVsoUploadedInfo" runat="server" />
                                </div>
                                <div class="row">
                                    <div class="col-md-2">Zip File:</div>
                                    <div class="col-md-10">
                                        <telerik:RadAsyncUpload runat="server" ID="AsyncUploadZip" MultipleFileSelection="Automatic" CssClass="RadUploadZip" AutoPostBack="true" OnClientFileUploaded="OnRadAsyncUpload1ClientAction"></telerik:RadAsyncUpload>
                                        <asp:Label ID="lbUploadedZip" runat="server" Visible="false" />
                                        <asp:Button ID="btnDummy" runat="server" OnClick="btnDummy_Click" Style="display: none;" />
                                    </div>
                                </div>
                                <div class="row">
                                    <asp:Label ID="lbZipInfo" runat="server" Font-Bold="true" />
                                </div>
                                <div class="row">
                                    <div class="col-md-2" style="margin-top: 5px">Language populated :</div>
                                    <div class="col-md-10" style="margin-top: 5px">
                                        <telerik:RadListBox runat="server" ID="radlistbox_languages_new" CheckBoxes="true" ShowCheckAll="true" CssClass="multiColumn labelPosition" Height="300" Width="100%">
                                            <ItemTemplate>
                                                <div class="container-fluid">
                                                    <div class="row" style="margin-left: -8px;">
                                                        <div class="col-md-12">
                                                            <asp:Label runat="server" ID="languageLabel"></asp:Label>
                                                            <asp:HyperLink runat="server" ID="vsoLink" Visible="false" Text="VsoLink" Target="_blank"></asp:HyperLink>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <telerik:RadListBox runat="server" ID="radlistbox_files_list" CssClass="multiColumn labelPosition" Height="300" Width="100%">
                                                                <ItemTemplate>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <asp:Image ID="statusImage" runat="server" />
                                                                            <asp:Label runat="server" ID="fileName"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </telerik:RadListBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </telerik:RadListBox>
                                    </div>
                                </div>
                            </telerik:RadPageView>
                        </telerik:RadMultiPage>
                    </div>
                </div>

                <br />

                <div class="panel panel-primary">
                    <div class="panel-heading">
                        6. Please click submit button when finished
                    </div>
                    <div class="panel-body">
                        <div class="row">

                            <div class="col-md-2">
                                <telerik:RadButton CausesValidation="true" runat="server" ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" Width="100" Height="50" Font-Size="Large"></telerik:RadButton>
                            </div>
                            <div class="col-md-2">
                                <asp:Label runat="server" ID="label_generalErrorMsg" Text="please scroll up to check if there is any invalid input" Visible="false" ForeColor="Red"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label runat="server" ID="label_submitSuccess" ForeColor="Green" Visible="false"></asp:Label>
                                <asp:Label ID="label_submitFail" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:Label runat="server" ID="label_gridCaption" Text="Submit Infos" Visible="false" Font-Bold="true"></asp:Label>
                <telerik:RadGrid runat="server" ID="radgrid_lang" Visible="false">
                </telerik:RadGrid>
                <br />
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
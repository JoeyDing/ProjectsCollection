<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobLog.aspx.cs" MasterPageFile="~/MasterPage.Master"     Inherits="SkypeIntlPortfolio.Ajax.Pages.Monitor.JobLog" %>


<asp:Content ID="Content2" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
  <style>
      a.HyperLinkHover {
         background-image:url("../../Images/1448014031_stock_data-link.png");
         display:block;
         width:16px;
         height:16px;
         cursor:pointer;
         text-indent:-9999px;
           }  
      .multiColumn ul
        {
            width:100%;                
        }
        .rlbTemplate span:nth-of-type(1) 
        {
            width:200px;
            display:inline-block;
        }
      .rlbHeader span:nth-of-type(1) {
          width:200px;
          display:inline-block;
      }


        
       

      </style>
  <script type="text/javascript">   
    function RowClick(index)   
    {  
       var gridJobsLog = $find("<%= this.gridview_JobsLog.ClientID %>"); 
        gridJobsLog.get_masterTableView().get_dataItems()[index].set_selected("true");
    }  
</script> 
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
    </telerik:RadAjaxManager>

    <div class="layout-control">
        <div class="panel panel-info">
          
            <div class="panel-body">
                <div class="container-fluid" id="container-row">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label Text="" runat="server" />
                             <div class="panel panel-primary">
                                <div class="panel-heading">1 .Please select a tool:</div>
                                    <div class="panel-body">
                                        <telerik:RadListBox 
                                            ID="radListBox_Tools" 
                                            runat="server" 
                                            CausesValidation="False" 
                                            Height="200px" 
                                            Width="1024px"  
                                            EmptyMessage="No Records Found"
                                            CssClass="multiColumn"
                                             Skin="Office2007"
                                             > 
                                            <HeaderTemplate>
                                                   <asp:Label Text="Name"  runat="server" ></asp:Label>
                                                   <asp:Label Text="Network Working Directory"  runat="server" ></asp:Label>
                                             </HeaderTemplate>
                                            <ItemTemplate> 
                                                <asp:CheckBox ID="chkSel" runat="server" OnCheckedChanged="chkSel_CheckedChanged"  AutoPostBack="True" />
                                                <asp:Label ID="lbToolID" runat="server" Text='<%# Eval("ToolID") %>' Visible="false"  ></asp:Label> 
                                                <asp:Label ID="lbName" runat="server" Text='<%# Eval("Name") %>'  ></asp:Label> 
                                                 <asp:Label ID="lbNetworkDir" runat="server" Text='<%# Eval("NetworkWorkingDirectory") %>'></asp:Label>
                                            </ItemTemplate> 
                                            <EmptyMessageTemplate> 
                                                No Records Found 
                                            </EmptyMessageTemplate> 
                                        </telerik:RadListBox> 
                                     </div>
                                 </div>
                            

                          <div class="panel panel-danger">
                            <div class="panel-heading">
                                <h3 class="panel-title">Job's Log</h3>
                            </div>
                            <div class="panel-body">
                                <telerik:RadGrid ID="gridview_JobsLog" 
                                    runat="server"
                                    HeaderStyle-HorizontalAlign="Center" 
                                    HorizontalAlign="Center" 
                                    Font-Bold="true"
                                    HeaderStyle-Font-Bold="true"
                                    AutoGenerateColumns ="false" 
                                    ClientSettings-EnableAlternatingItems="false"
                                    AllowSorting="True" 
                                    ShowGroupPanel="True"
                                    AllowMultiRowSelection="True"
                                    OnItemDataBound="gridview_JobsLog_ItemDataBound"
                                    Width="1024px"
                                     Skin="Office2010Silver"
                                     >
                                  
                                    <MasterTableView DataKeyNames="LogID">
                                      <Columns>
                                                    <telerik:GridBoundColumn DataField="LogID" HeaderText="Log ID" Visible="false"></telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="FullPath" HeaderText="Full Path" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                                                    
                                         <telerik:GridTemplateColumn UniqueName="Tempcol" > 
                                                <ItemTemplate> 
                                                  <asp:HyperLink ID="HyperLink1" runat="server" Text="Link" NavigateUrl='<%# String.Format("~/Pages/Monitor/FileLoader.ashx?fullpath={0}", Eval("FullPath").ToString()) %>'  Target="_blank" cssClass="HyperLinkHover"></asp:HyperLink>  
                                                </ItemTemplate> 
                                                </telerik:GridTemplateColumn> 
                                           </Columns>
                                    </MasterTableView>

                                    <ClientSettings
                                         ReorderColumnsOnClient="True" 
                                        AllowDragToGroup="True"
                                         AllowColumnsReorder="True"
                                         EnablePostBackOnRowClick="false"
                                        >
                                       <Selecting AllowRowSelect="true" />
                                        <Resizing AllowRowResize="True" 
                                            AllowColumnResize="True" 
                                            EnableRealTimeResize="True"
                                            ResizeGridOnColumnResize="False"></Resizing>
                                    </ClientSettings>
                                </telerik:RadGrid>

                              

                            </div>
                        </div>

                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
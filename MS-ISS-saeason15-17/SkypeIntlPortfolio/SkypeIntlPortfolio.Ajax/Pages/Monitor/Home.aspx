<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="Home.aspx.cs" Inherits="SkypeIntlPortfolio.Ajax.Pages.Monitor.Home" %>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyPlaceHolder" runat="server">
    <telerik:RadTileList runat="server" ID="RadTileList1" ScrollingMode="Auto" RenderMode="Auto"
        EnableDragAndDrop="false">

        <Groups>

            <telerik:TileGroup>

                <telerik:RadIconTile Name="Grid1" NavigateUrl="~/Pages/Monitor/JobDetails.aspx" runat="server"
                    Target="_blank" Width="310px" Height="310px" BackColor="#ff6600">

                    <Title Text="Total Job Executions (last 24hours)"></Title>

                    <PeekTemplate>
                        <telerik:RadHtmlChart runat="server" ID="PieChart1" Height="310px" Transitions="true" Width="310px" BackColor="White">
                            <ChartTitle Text="Total Job Executions (last 24hours)">

                                <Appearance Align="Center" Position="Top">
                                </Appearance>
                            </ChartTitle>
                            <Legend>
                                <Appearance Position="Right" Visible="true">
                                </Appearance>
                            </Legend>
                            <PlotArea>
                                <Series>
                                    <telerik:PieSeries DataFieldY="Total" NameField="Status">
                                        <LabelsAppearance DataFormatString="{0}">
                                        </LabelsAppearance>
                                        <TooltipsAppearance Color="White" DataFormatString="{0}"></TooltipsAppearance>
                                    </telerik:PieSeries>
                                </Series>
                                <YAxis>
                                </YAxis>
                            </PlotArea>
                            <Legend>
                                <Appearance Position="Top">
                                </Appearance>
                            </Legend>
                        </telerik:RadHtmlChart>
                    </PeekTemplate>

                    <PeekTemplateSettings CloseDelay="4000" ShowInterval="1500" Animation="Fade" HidePeekTemplateOnMouseOut="true"
                        ShowPeekTemplateOnMouseOver="true" />
                </telerik:RadIconTile>

                <telerik:RadIconTile Name="Grid1" NavigateUrl="~/Pages/Monitor/JobCurrentStatus.aspx" runat="server"
                    Target="_blank" Width="310px" Height="310px">

                    <Title Text="Last Job Executions Status"></Title>

                    <PeekTemplate>
                        <telerik:RadGrid ID="gridview_JobsIterationsFailed" runat="server" Width="310px"
                            HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" Font-Bold="true" HeaderStyle-Font-Bold="true"
                            OnItemDataBound="gridview_JobsIterationsFailed_ItemDataBound"
                            AutoGenerateColumns="false" ClientSettings-EnableAlternatingItems="false"
                            AllowSorting="false" ShowGroupPanel="false">
                            <ClientSettings AllowColumnsReorder="True" AllowExpandCollapse="true"></ClientSettings>

                            <MasterTableView DataKeyNames="SqlJobStepIterationID">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="JobName" HeaderText="Job Name" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="RunDateTime" HeaderText="Executed On" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>

                            <ClientSettings ReorderColumnsOnClient="False" AllowDragToGroup="False" AllowColumnsReorder="False">
                                <Selecting AllowRowSelect="False"></Selecting>
                                <Resizing AllowRowResize="False" AllowColumnResize="False" EnableRealTimeResize="False"
                                    ResizeGridOnColumnResize="False"></Resizing>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </PeekTemplate>

                    <PeekTemplateSettings CloseDelay="4000" ShowInterval="1500" Animation="Fade" HidePeekTemplateOnMouseOut="true"
                        ShowPeekTemplateOnMouseOver="true" />
                </telerik:RadIconTile>

                <telerik:RadIconTile Name="Grid1" NavigateUrl="~/Pages/Monitor/JobLog.aspx" runat="server"
                    Target="_blank" Width="310px" Height="310px" BackColor="Purple">

                    <Title Text="Job Log (last 24hours)"></Title>

                    <PeekTemplate>

                        <telerik:RadGrid ID="gridview_JobsLog"
                            runat="server"
                            HeaderStyle-HorizontalAlign="Center"
                            HorizontalAlign="Center"
                            Font-Bold="true"
                            HeaderStyle-Font-Bold="true"
                            AutoGenerateColumns="false"
                            ClientSettings-EnableAlternatingItems="false"
                            AllowSorting="True"
                            ShowGroupPanel="True"
                            AllowMultiRowSelection="True"
                            Width="1024px"
                            Skin="Office2010Silver">

                            <MasterTableView DataKeyNames="LogID">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="LogID" HeaderText="Log ID" Visible="false"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FullPath" HeaderText="Full Path" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn UniqueName="Tempcol">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Text="Link" NavigateUrl='#' Target="_blank"></asp:HyperLink>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>

                            <ClientSettings
                                ReorderColumnsOnClient="True"
                                AllowDragToGroup="True"
                                AllowColumnsReorder="True"
                                EnablePostBackOnRowClick="false">
                                <Selecting AllowRowSelect="true" />
                                <Resizing AllowRowResize="True"
                                    AllowColumnResize="True"
                                    EnableRealTimeResize="True"
                                    ResizeGridOnColumnResize="False"></Resizing>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </PeekTemplate>

                    <PeekTemplateSettings CloseDelay="4000" ShowInterval="1500" Animation="Fade" HidePeekTemplateOnMouseOut="true"
                        ShowPeekTemplateOnMouseOver="true" />
                </telerik:RadIconTile>
            </telerik:TileGroup>
        </Groups>
    </telerik:RadTileList>
</asp:Content>
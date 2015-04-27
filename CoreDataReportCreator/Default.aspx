<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/CoreDataReport.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CoreDataReportCreator.CoreData" %>

<%@ Import Namespace="System.ComponentModel" %>
<%@ Import Namespace="CoreDataLibrary.Objects" %>
<%@ Import Namespace="CoreDataReportCreator.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="refresh" content="2000" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css">
    <link rel="stylesheet" href="Content/Site.css">
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <link rel="stylesheet" href="/resources/demos/style.css">
    <script>
        $(function () {
            $(document).tooltip();
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("[id$=txtDate]").datepicker({ dateFormat: "dd/mm/yy" });
        });
        $(document).ready(function () {
            $('[data-toggle=offcanvas]').click(function () {
                $('.row-offcanvas').toggleClass('active');
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <div>
        <asp:Timer runat="server" ID="Timer1" OnTick="UpDateTimer_Elapsed" Interval="20000"></asp:Timer>
    </div>
    <div class="row-offcanvas row-offcanvas-left">
        <div id="sidebar" class="sidebar-offcanvas">
            <div class="col-md-12">
                <h3 class="text-center">Messages</h3>
                <asp:Repeater runat="server" ID="MessagesRepeater" OnItemDataBound="MessagesRepeater_OnItemDataBound">
                    <ItemTemplate>
                        <div class="panel-group col-md-12">
                            <div style="padding-bottom: 5px;">
                                <asp:Panel ID="panelMessageTitle" CssClass="panel panel-warning" runat="server">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a href="LogDetails.aspx?id=<%#Eval("LogId") %>" title="Time : <%#Eval("StartTime") %>"><%#Eval("LogItemName") %>  </a>
                                            <span style="font-size: 0.8em;" class="pull-right">
                                                <asp:Label runat="server" ID="LabelMessageSidebar"></asp:Label>
                                            </span>
                                        </h4>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div id="main">
            <div class="col-md-12">
                <div class="container" style="padding-bottom: 70px; padding-top: 30px">
                    <div class="row" style="padding-bottom: 10px;">
                        <%--<div class="col-md-2" style="padding-left: 30px;">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <asp:Label ID="LabelEnableExport" runat="server" class="form-control-static" Text="Active" />
                                </span>
                                <asp:CheckBox ID="CheckBoxActive" class="form-control" runat="server" OnCheckedChanged="CheckBoxActive_OnCheckedChanged"/>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <asp:Label ID="LabelnableExport" runat="server" class="form-control-static" Text="InActive" />
                                </span>
                                <asp:CheckBox ID="CheckBoxInActive" class="form-control" runat="server" OnCheckedChanged="CheckBoxInActive_OnCheckedChanged"/>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <asp:Label ID="LabelableExport" runat="server" class="form-control-static" Text="Error" />
                                </span>
                                <asp:CheckBox ID="CheckBoxError" class="form-control" runat="server" OnCheckedChanged="CheckBoxError_OnCheckedChanged"/>
                            </div>
                        </div>--%>
                        <%--                        <div class="col-md-5 pull-right" style="padding-right: 30px;">
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <asp:Label class="form-control-static" runat="server">Select Log Date</asp:Label>
                                </div>
                                <asp:TextBox CssClass="form-control" runat="server" ID="txtDate" AutoPostBack="True"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:Button runat="server" class="btn btn-info btn-default" ID="ButtonUpdate" Text="Update" OnClick="ButtonUpdate_OnClick" />
                                </span>
                            </div>
                        </div>--%>
                    </div>
                    <asp:Repeater runat="server" ID="ActiveLogRepeater" OnItemDataBound="RepeaterInActiveLogs_OnItemDataBound">
                        <ItemTemplate>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="panel-group col-md-4">
                                        <div style="padding-bottom: 5px;">
                                            <asp:Panel ID="paneltitle" CssClass="panel panel-warning" runat="server">
                                                <div class="panel-heading">
                                                    <h4 class="panel-title">
                                                        <a href="LogDetails.aspx?id=<%#Eval("LogId") %>" title="<%#Eval("LogItemName") %> - <%#Eval("LogItemStatus") %> At : <%#Eval("StartTime") %>"><%#Eval("LogItemName") %>  </a>
                                                        <span style="font-size: 0.8em;" class="pull-right">
                                                            <asp:Label runat="server" ID="LabelMessage"></asp:Label>
                                                        </span>
                                                    </h4>
                                                </div>
                                                <div class="panel-footer" style="font-size: 0.9em">
                                                    <i>
                                                        <span>Start : <b><%#Eval("StartTime") %></b>
                                                        </span>
                                                        <span style="padding-left: 8px;">End :<b> <%#Eval("EndTime") %></b>
                                                        </span>
                                                        <span style="padding-left: 8px;">Duration :<b> <%#Eval("Duration") %> </b>
                                                        </span>
                                                    </i>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

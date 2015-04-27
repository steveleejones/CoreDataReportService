<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/CoreDataReport.Master" AutoEventWireup="true" CodeBehind="LogDetails.aspx.cs" Inherits="CoreDataReportCreator.LogDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <div class="container" style="padding-bottom: 40px; padding-top: 80px">
        <div style="padding-bottom: 5px;">
            <asp:Panel ID="panelmain" CssClass="panel panel-primary" runat="server">
                <%--<div class="panel panel-primary">--%>
                <div class="panel-heading">
                    <h4>Details</h4>
                </div>
                <div class="panel-body">
                    <div class="panel panel-default" style="padding-bottom: 10px;">
                        <div class="panel-body">
                            <div class="col-md-12" style="padding-bottom: 10px;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        <label class="form-control-static">Log Item Name</label>
                                    </span>
                                    <asp:Label class="form-control" runat="server" ID="LogLabelName"></asp:Label>
                                    <span class="input-group-btn">
                                        <asp:Button runat="server" class="btn btn-info btn-default" ID="ButtonRemoveLogItem" Text="Remove" Onclick="ButtonRemoveLogItem_OnClick" />
                                    </span>
                                </div>
                            </div>
                            <div class="col-md-4" style="padding-bottom: 10px;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        <label class="form-control-static">Start Time</label>
                                    </span>
                                    <asp:Label class="form-control" runat="server" ID="LogLabelStartTime"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4" style="padding-bottom: 10px;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        <label class="form-control-static">End Time</label>
                                    </span>
                                    <asp:Label class="form-control" runat="server" ID="LogLabelEndTime"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4" style="padding-bottom: 10px;">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        <label class="form-control-static">Current Status</label>
                                    </span>
                                    <asp:Label class="form-control" runat="server" ID="LogLabelStatus"></asp:Label>
                                    <span class="input-group-btn">
                                        <asp:Button runat="server" class="btn btn-info btn-default" ID="ButtonCancel" Text="Cancel" OnClick="ButtonCancel_OnClick" />
                                    </span>
                                </div>
                            </div>
                            <asp:TextBox ID="LogMessages" CssClass="form-control col-md-4" runat="server" TextMode="MultiLine" Style="padding-bottom: 10px;"></asp:TextBox>
                        </div>
                    </div>
                    <div style="padding-top: 10px;">
                        <asp:Repeater runat="server" ID="LogDetailsRepeater" OnItemDataBound="LogDetailsRepeater_OnItemDataBound">
                            <ItemTemplate>
                                <div class="row col-md-10 col-md-push-1">
                                    <asp:Panel ID="paneltitle" CssClass="panel panel-success" runat="server">
                                        <div class="panel-heading">
                                            <%#Eval("Step") %>
                                        </div>
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">
                                                            <label class="form-control-static">Start Time</label>
                                                        </span>
                                                        <asp:Label class="form-control" runat="server" ID="LabelStartTime"><%#Eval("StartTime") %></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">
                                                            <label class="form-control-static">End Time</label>
                                                        </span>
                                                        <asp:Label class="form-control" runat="server" ID="LabelEndTime"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">
                                                            <label class="form-control-static">Status</label>
                                                        </span>
                                                        <asp:Label class="form-control" runat="server" ID="LabelStatus"><%#Eval("Status") %></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">
                                                            <label class="form-control-static">Time Taken</label>
                                                        </span>
                                                        <asp:Label class="form-control" runat="server" ID="LabelTimeTaken">></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:TextBox ID="Messages" CssClass="form-control col-md-4" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </asp:Panel>
            <%--</div>--%>
        </div>
    </div>
</asp:Content>

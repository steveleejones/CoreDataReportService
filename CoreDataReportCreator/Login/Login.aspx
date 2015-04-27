<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/CoreDataReport.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CoreDataReportCreator.Login.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div style="padding-left: 2%; padding-top: 2%;">
            <asp:Panel ID="Panel1" DefaultButton="ButtonLogin" runat="server">
                <div class="container" style="margin: 30px 40px 40px 40px;">
                    <h3>CoreData login.</h3>
                    <div class="row" style="margin-bottom: 10px">
                        <div class="input-group col-md-4">
                            <asp:TextBox ID="TextBoxUserName" runat="server" CssClass="form-control" placeholder="UserName"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="input-group col-md-4">
                            <asp:TextBox ID="TextBoxPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="Password"></asp:TextBox>
                        </div>
                    </div>
                    <div style="padding-top: 20px; padding-left: 20px;">
                        <asp:Button ID="ButtonLogin" runat="server" Text="OK" CssClass="btn btn-primary" Width="150" OnClick="ButtonLogin_OnClick" />
                        <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" CssClass="btn btn-default" Width="150" OnClick="ButtonCancel_OnClick" />
                    </div>
                    <div class="row" style="color: red; margin-top: 20px;">
                        <strong>
                            <asp:Literal runat="server" ID="LiteralErrorMessage"></asp:Literal>
                        </strong>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>

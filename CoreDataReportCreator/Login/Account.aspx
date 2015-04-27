<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/CoreDataReport.Master" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="CoreDataReportCreator.Login.Account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div style="padding-left: 2%; padding-top: 2%;">
            <asp:Panel ID="Panel1" DefaultButton="ButtonChangePassword" runat="server" Style="vert-align: middle;">
                <div class="container" style="margin: 40px 40px 40px 40px;">
                    <h4>Change password</h4>
                    <div class="row" style="padding-bottom: 5px;">
                        <div class="input-group col-md-4">
                            <asp:TextBox ID="TextBoxOriginalPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="Password"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row" style="padding-bottom: 5px;">
                        <div class="input-group col-md-4">
                            <asp:TextBox ID="TextBoxNewPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="NewPassword"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row" style="padding-bottom: 5px;">
                        <div class="input-group col-md-4">
                            <asp:TextBox ID="TextBoxNewPassword2" TextMode="Password" runat="server" CssClass="form-control" placeholder="ConfirmNewPassword"></asp:TextBox>
                        </div>
                    </div>
                    <div style="padding-top: 20px; padding-left: 20px;">
                        <asp:Button ID="ButtonChangePassword" runat="server" Text="OK" CssClass="btn btn-primary" Width="150" OnClick="ButtonChangePassword_OnClick"/>
                        <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" CssClass="btn btn-default" Width="150" OnClick="ButtonCancel_OnClick"/>
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

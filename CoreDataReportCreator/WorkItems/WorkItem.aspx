<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/CoreDataReport.Master" AutoEventWireup="true" CodeBehind="WorkItem.aspx.cs" Inherits="CoreDataReportCreator.WorkItems.WorkItem" %>
<%@ Import Namespace="CoreDataLibrary.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <% foreach (CoreDataWorkItem workItem in coreDataWorkItems)
       {%>
      <li><%#Eval("workItem") %></li> 
       <%} %>
</asp:Content>

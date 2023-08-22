<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Publica.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="webOperacionPrincipal" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    </asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <br />
    <br />
    <br />
    <asp:Label ID="lbldetbienv" runat="server" 
        Text="<%$ Resources:resCorpusCFDIEs, lblBienvenida %>" Font-Bold="True" 
        ForeColor="#004D71"></asp:Label>
    <br />
<br />
<br />
<br />
    &nbsp;
</asp:Content>

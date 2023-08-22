<%@ Page Title="" Language="C#" MasterPageFile="~/webGlobalMaster.master" AutoEventWireup="true" CodeFile="webGlobalBuscaCertificados.aspx.cs" Inherits="webGlobalBuscaCertificados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ListBox ID="ListBox1" runat="server" Width="608px"></asp:ListBox>
    <br />
    <asp:DropDownList ID="DropDownList1" runat="server">
        <asp:ListItem>My</asp:ListItem>
        <asp:ListItem>Root</asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList2" runat="server">
        <asp:ListItem>CurrentUser</asp:ListItem>
        <asp:ListItem>LocalMachine</asp:ListItem>
    </asp:DropDownList>
    <br />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Buscar" />
</asp:Content>


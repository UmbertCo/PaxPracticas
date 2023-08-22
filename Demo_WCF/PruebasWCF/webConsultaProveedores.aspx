<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webConsultaProveedores.aspx.cs" Inherits="PruebasWCF_webConsultaProveedores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
    En esta seccion se puede realizar la consultas de los proveedores capturados.<br />
    <br />
    <asp:GridView ID="grvProveedores" runat="server">
</asp:GridView>
<br />
<br />
<asp:Button ID="btnConsultar" runat="server" onclick="btnConsultar_Click" 
    Text="Consultar" />
<br />
</asp:Content>


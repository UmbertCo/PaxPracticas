<%@ Page Title="Probar Plantillas" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Plantillas.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Plantillas" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:FileUpload ID="fuArchivo" runat="server" />
    <asp:Button ID="btnVerAddenda" runat="server" Text="Ver Plantilla" 
        onclick="btnVerAddenda_Click" />
</asp:Content>

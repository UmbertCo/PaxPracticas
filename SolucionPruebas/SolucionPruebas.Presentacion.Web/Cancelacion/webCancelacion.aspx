<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"  CodeBehind="webCancelacion.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Cancelacion.webCancelacion" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:FileUpload ID="fuArchivo" runat="server" />
    <asp:Button ID="btnVerAddenda" runat="server" Text="Cancelar comprobante" 
        onclick="btnVerAddenda_Click" />

    <br />
    <br />
    <br />
    <asp:TextBox ID="txtResultado" runat="server" Height="188px" 
        TextMode="MultiLine" Width="584px"></asp:TextBox>
    <br />

</asp:Content>


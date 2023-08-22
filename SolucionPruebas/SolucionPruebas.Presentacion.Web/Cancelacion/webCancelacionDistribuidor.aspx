<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="webCancelacionDistribuidor.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Cancelacion.webCancelacionDistribuidor" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table>
        <tr>
            <td>
                Seleccionar PEM del Certificado
            </td>
            <td>
                <asp:FileUpload ID="fuCertificadoPem" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Seleccionar PEM de la llave privada
            </td>
            <td>
                <asp:FileUpload ID="fuLlavePem" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnCrearPfx" runat="server" Text="Crear Pfx" 
                    OnClick="btnCrearPfx_Click" />
                <asp:Button ID="btnCrearPfxBytes" runat="server" Text="Crear Pfx Bytes" 
                    OnClick="btnCrearPfxBytes_Click" />
                <asp:Button ID="btnCrearPfxRutas" runat="server" Text="Crear Pfx Rutas" 
                    OnClick="btnCrearPfxRutas_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnCancelarComprobante" runat="server" Text="Cancelar comprobante" 
                    OnClick="btnCancelarComprobante_Click" />
                <asp:Button ID="btnFirmar" runat="server" Text="Firmar" 
                    onclick="btnFirmar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                    onclick="btnCancelar_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtResultado" Width="500px" Height="200px" runat="server" 
                    TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
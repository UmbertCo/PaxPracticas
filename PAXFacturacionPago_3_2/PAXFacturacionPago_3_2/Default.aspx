<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="webOperacionPrincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
    .style2
    {
        width: 116px;
        height: 105px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table width="100%">
        <tr>
            <td>
                <div class="title">
                    <h2>
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblBienvenido %>" 
                        style="font-family: 'Century Gothic'"></asp:Label>
                    </h2>
                </div>
                <br />
                <br />
                <br />
                <asp:Label ID="lblBienvendia" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblBienvenida %>" 
                    style="font-family: 'Century Gothic'">
                </asp:Label>
                <br />
                <br />
                <asp:Label ID="lblProveedor" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblProveedor %>" 
                    style="font-family: 'Century Gothic'">
                </asp:Label>
                <br />
                <br />
                <asp:Label id="lblPermisos" runat="server" Visible="false" >
                </asp:Label>
                <br />
                <br />
                <asp:Label id="lblFielSat" runat="server" Visible="true" Text="Se les informa que a partir del día 4 de marzo del 2015, para los efectos del artículo 29, fracción II del CFF, las personas físicas en sustitución del CSD, podrán utilizar el certificado de FIEL obtenido conforme a lo previsto en el artículo 17-D del citado Código, únicamente para la emisión del CFDI a través de la herramienta electrónica denominada “Servicio gratuito de generación de Factura Electrónica (CFDI) ofrecido por el SAT”; para que las personas físicas puedan seguir timbrando a través de PAX Facturación será necesario que tramiten su Certificado de Sello Digital en el SAT o atreves de la aplicación SOLCEDI del SAT.">
                </asp:Label>
                <br />
                <br />
                <img alt="" class="style2" 
                src="Imagenes/proveedor-autorizado-de-certificación.jpg" />
            </td>
            <td align="right" valign="top">
                <table>
                    <tr>
                        <td style="height: 50px">
                            <asp:ImageButton ID="imgDescarga" runat="server" 
                                ImageUrl="~/Imagenes/DescMUsuario.png" OnClick="imgDescarga_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ImageButton ID="imgDescargaNomina" runat="server" 
                                ImageUrl="~/Imagenes/DescMUsuarioNomina.png" OnClick="imgDescargaNomina_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>


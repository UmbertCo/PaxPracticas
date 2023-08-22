<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="TimbradoEnviar.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Timbrado.TimbradoEnviar" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">


    <style type="text/css">
        .style1
        {
            width: 50%;
        }
    </style>


</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <table class="style1">
            <tr>
                <td>
                    Escoge el xml</td>
                <td>
                <asp:FileUpload ID="fuArchivo" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                <asp:TextBox ID="txtConceptosAgregar" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" style="height: 26px" 
                        onclick="btnAceptar_Click" />
                <asp:Button ID="Button1" runat="server" Text="Button" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="txtResultado" runat="server" Height="481px" 
                        TextMode="MultiLine" Width="915px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Transacciones.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Transacciones" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table>
        <tr>
            <td>Transacción&nbsp; normal</td>
            <td>
                <asp:Button ID="btnIniciarTransaccion" runat="server" 
                    Text="Transaccion" OnClick="btnIniciarTransaccion_Click" />
            </td>
        </tr>
        <tr>
            <td>Transacción de ambiente</td>
            <td>
                <asp:Button ID="btnIniciarTransaccionAmbiente" runat="server" 
                    Text="Transaccion Ambiente" OnClick="btnIniciarTransaccionAmbiente_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
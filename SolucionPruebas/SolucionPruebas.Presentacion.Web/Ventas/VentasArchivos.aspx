<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="VentasArchivos.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Ventas.VentasArchivos" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 250px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div style="text-align: center;">
        <table class="style1">
            <tr>
                <td>
                    Seleccionar Archivo a subir: 
                </td>
                <td>
                    <asp:FileUpload ID="fuArchivo" runat="server" />
                </td>
                <td>
                    <asp:Button ID="btnCargarArchivo" runat="server" Text="Cargar archivo" 
                        OnClick="btnCargarArchivo_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                <asp:Button ID="btnVisualizar" runat="server" Text="Visualizar" 
                        onclick="btnVisualizar_Click" />

                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    </div>
</asp:Content>

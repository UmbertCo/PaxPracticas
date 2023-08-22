<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ComplementoCadenaOriginal.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Complementos.ComplementoCadenaOriginal" %>

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
                    <asp:FileUpload ID="fuComplemento" runat="server" />
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
        </table>
    </div>
</asp:Content>
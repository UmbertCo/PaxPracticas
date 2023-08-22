<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="TimbradoGeneracion.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Timbrado.TimbradoGeneracion" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <table class="style1">
            <tr>
                <td>
                    Escoge el txt</td>
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
                        OnClick="btnAceptar_Click"/>
                <asp:Button ID="Button1" runat="server" Text="Button" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

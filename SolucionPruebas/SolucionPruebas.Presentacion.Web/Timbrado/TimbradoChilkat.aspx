<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="TimbradoChilkat.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Timbrado.TimbradoChilkat" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <table class="style2">
            <tr>
                <td>
                    Escoge el xml</td>
                <td>
                    <asp:FileUpload ID="fuArchivo" runat="server" />
                </td>
                <td>
                <asp:Button ID="btnChilkat" runat="server" Text="Chilkat" 
                        OnClick="btnChilkat_Click" />
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
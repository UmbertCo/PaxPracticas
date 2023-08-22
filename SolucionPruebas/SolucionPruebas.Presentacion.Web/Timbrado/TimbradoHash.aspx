<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimbradoHash.aspx.cs" MasterPageFile="~/Site.Master" Inherits="SolucionPruebas.Presentacion.Web.Timbrado.TimbradoHash" %>

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
                <asp:Button ID="btnObtenerHash" runat="server" Text="Hash" 
                        OnClick="btnObtenerHash_Click"  />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>

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
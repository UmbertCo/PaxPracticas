<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="TimbradoOpenSSL.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Timbrado.TimbradoOpenSSL" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript" language="javascript">

        function url() {
            var pag = 'Addendas/Addendas.aspx';

            hidden = open(pag, 'NewWindow', 'top=0,left=0,width=1150,height=670,status=yes,resizable=yes,scrollbars=yes');
        }
        

    </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 80%;
        }
    </style>

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
                <asp:Button ID="btnOpenSSL" runat="server" Text="OpenSSL" 
                        onclick="btnOpenSSL_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnOpenSSLLocal" runat="server" Text="OpenSSLLocal" 
                        OnClick="btnOpenSSLLocal_Click"  />
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
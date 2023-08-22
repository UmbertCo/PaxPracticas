<%@ Page Title="" Language="C#" MasterPageFile="~/Privada.master" AutoEventWireup="true" CodeFile="webInicioSesionCambiarPWD.aspx.cs" Inherits="Pantallas_Login_webInicioSesionCambiarPWD" %>

<%@ Register src="../../WebControls/usrGlobalPwd.ascx" tagname="usrGlobalPwd" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css" >
    .contenedor
    {
        width:600px;
        text-align:left;
    }
    .contenedor select, input[type='text']
    {
     }
        .style3
     {
         width: 100%;
     }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <table class="style3">
        <tr>
            <td>
                <uc1:usrGlobalPwd ID="usrGlobalPwd" runat="server" />
                </td>
        </tr>
    </table>
</asp:Content>


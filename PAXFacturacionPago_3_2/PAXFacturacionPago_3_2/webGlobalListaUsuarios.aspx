<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webGlobalListaUsuarios.aspx.cs" Inherits="webGlobalListaUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
    <table cellpadding="0" cellspacing="0" class="style1">
        <tr>
            <td width="260px">
                <asp:Label ID="lblTotal" runat="server" Font-Bold="True" 
                    Text="Total Usuarios Conectado:"></asp:Label>
                <asp:Label ID="lblTotalValue" runat="server" Text="0"></asp:Label>
            </td>
            <td>
                <asp:Button ID="btnActualziar" runat="server" CssClass="botonEstilo" 
                    onclick="btnActualziar_Click" Text="Actualizar" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:Label ID="lblUsuariosCon" runat="server" Text="Usuarios Conectados:"></asp:Label>
    <br />
    <asp:ListBox ID="lbxUsuarios" runat="server" Height="292px" Width="350px">
    </asp:ListBox>
</asp:Content>


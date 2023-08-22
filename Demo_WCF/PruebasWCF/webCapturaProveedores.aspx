<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webCapturaProveedores.aspx.cs" Inherits="PruebasWCF_webCapturaProveedores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
    Catalogo de Proveedores<br />
    <table cellpadding="0" cellspacing="0" style="height: 213px; width: 629px">
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="ID"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtID" runat="server"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Razon Social"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtRazonSocial" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="revPrecio3" runat="server" 
                                ControlToValidate="txtRazonSocial" CssClass="failureNotification" 
                                ErrorMessage="Requerido" ToolTip="Clave requerida." 
                                ValidationGroup="Grupo" Display="Dynamic"> <img 
                    src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="RFC"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtRfc" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="revPrecio4" runat="server" 
                                ControlToValidate="txtRfc" CssClass="failureNotification" 
                                ErrorMessage="Requerido" ToolTip="Clave requerida." 
                                ValidationGroup="Grupo" Display="Dynamic"> <img 
                    src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Estructura"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtEstructura" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="revPrecio5" runat="server" 
                                ControlToValidate="txtEstructura" CssClass="failureNotification" 
                                ErrorMessage="Requerido" ToolTip="Clave requerida." 
                                ValidationGroup="Grupo" Display="Dynamic"> <img 
                    src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Elimina"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkElimina" runat="server" Text="Seleccion" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="right">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="100">
                <asp:Button ID="btnAlta" runat="server" onclick="btnAlta_Click" 
                    Text="Alta" ValidationGroup="Grupo" />
                        </td>
                        <td width="100">
                <asp:Button ID="btnBaja" runat="server" onclick="btnBaja_Click" 
                    Text="Baja" ValidationGroup="Grupo" />
                        </td>
                        <td width="100">
                <asp:Button ID="btnGuardar" runat="server" onclick="btnGuardar_Click" 
                    Text="Cambio" ValidationGroup="Grupo" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>


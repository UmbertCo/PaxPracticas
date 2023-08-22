<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="VentasPersonas.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.VentasPersonas" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <table class="style1">
            <tr>
                <td>
                    <asp:Label ID="lblEmpresa" runat="server" Text="Empresa"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmpresa" runat="server" Width="350px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblContacto" runat="server" Text="Contacto"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlContacto" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTelefono" runat="server" Text="Telefono"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTelefono" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTelefono2" runat="server" Text="Telefono 2"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTelefono2" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTelefono3" runat="server" Text="Telefono 3"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTelefono3" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCiudad" runat="server" Text="Ciudad"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddCiudad" runat="server" Width="128px">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFechaCaptura" runat="server" Text="Fecha Captura"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaCaptura" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEstatus" runat="server" Text="Estatus"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEstatus" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTipoCliente" runat="server" Text="Tipo Cliente"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTipoCliente" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTipoCompra" runat="server" Text="Tipo Compra"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTipoCompra" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblUsuario" runat="server" Text="Usuario"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlUsuario" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRfc" runat="server" Text="RFC"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRfc" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRazonSocial" runat="server" Text="Razon social"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRazonSocial" runat="server" Width="253px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblUsuarioCobro" runat="server" Text="Usuario cobro"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtUsuarioCobro" runat="server" Width="251px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="Reasignar" runat="server" Text="Reasignar persona" 
                        onclick="Reasignar_Click" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnPersonas" runat="server" Text="Ingresar personas" 
                        onclick="btnPersonas_Click" />
                </td>
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

<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webGeneradorPDF.aspx.cs" Inherits="PruebasWCF_sebGeneradorPDF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
    En esta seccion puede recueperar su plantilla con las opciones seleccionadas:<br />
    <br />
    <table cellpadding="0" cellspacing="0" class="style1">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="ID Plantilla"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txPlantilla" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="revPrecio3" runat="server" 
                                ControlToValidate="txPlantilla" CssClass="failureNotification" 
                                ErrorMessage="Requerido" ToolTip="Clave requerida." 
                                ValidationGroup="Grupo" Display="Dynamic"> <img 
                    src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="ID Adenda"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtAdenda" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="revPrecio4" runat="server" 
                                ControlToValidate="txtAdenda" CssClass="failureNotification" 
                                ErrorMessage="Requerido" ToolTip="Clave requerida." 
                                ValidationGroup="Grupo" Display="Dynamic"> <img 
                    src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="ID XML"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtXml" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="revPrecio5" runat="server" 
                                ControlToValidate="txtXml" CssClass="failureNotification" 
                                ErrorMessage="Requerido" ToolTip="Clave requerida." 
                                ValidationGroup="Grupo" Display="Dynamic"> <img 
                    src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>

            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:Button ID="btnRecuperar" runat="server" onclick="btnRecuperar_Click" 
    Text="Recuperar" ValidationGroup="Grupo" />
</asp:Content>


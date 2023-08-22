<%@ Page Title="Recuperación y Cambio de Contraseña" Language="C#" MasterPageFile="~/Privada.master" AutoEventWireup="true" CodeFile="webRecuperacionPassword.aspx.cs" Inherits="Pantallas_Login_webRecuperacionPassword" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

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
                <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                    Width="519px">
                    <cc1:TabPanel runat="server" HeaderText="Recuperar Contraseña" ID="TabPanel1">
                    <HeaderTemplate>Recuperar Contraseña</HeaderTemplate><ContentTemplate><table><tr><td><asp:Panel 
                        ID="pnlDatos" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid" 
                        BorderWidth="1px">
                        <br />
                        <table><tr><td align="left"><asp:Label ID="lblUsuario" 
                            runat="server" AssociatedControlID="txtUsuario" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label></td><td>&nbsp;</td></tr><tr><td><asp:TextBox 
                            ID="txtUsuario" runat="server" CssClass="textEntry" TabIndex="1"></asp:TextBox></td><td><asp:RequiredFieldValidator 
                                ID="UserNameRequired" runat="server" ControlToValidate="txtUsuario" 
                                CssClass="failureNotification" 
                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator></td></tr><tr><td 
                                align="left"><asp:Label ID="lblCorreo" runat="server" 
                                AssociatedControlID="txtCorreo" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"></asp:Label></td><td>&nbsp;</td></tr><tr><td><asp:TextBox 
                            ID="txtCorreo" runat="server" CssClass="textEntry" TabIndex="2"></asp:TextBox></td><td><asp:RequiredFieldValidator 
                                ID="EmailRequired" runat="server" ControlToValidate="txtCorreo" 
                                CssClass="failureNotification" Display="Dynamic" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                ValidationGroup="RegisterUserValidationGroup"><img 
                src="http://localhost:59333/CORPUSCFDI/Imagenes/error_sign.gif" /></asp:RequiredFieldValidator><asp:RegularExpressionValidator 
                                ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCorreo" 
                                CssClass="failureNotification" Display="Dynamic" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                ValidationGroup="RegisterUserValidationGroup" Width="131px"><img 
                            src="http://localhost:59333/CORPUSCFDI/Imagenes/error_sign.gif" /></asp:RegularExpressionValidator></td></tr></table>
                        <br />
                        </asp:Panel></td></tr><tr><td>
                            <asp:Button 
                            ID="Button1" runat="server" Height="24px" 
                                Text="<%$ Resources:resCorpusCFDIEs, btnRecuperaCuenta %>" Width="187px" 
                                onclick="Button1_Click" CssClass="botonGrande" /></td></tr></table></ContentTemplate></cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
</asp:Content>


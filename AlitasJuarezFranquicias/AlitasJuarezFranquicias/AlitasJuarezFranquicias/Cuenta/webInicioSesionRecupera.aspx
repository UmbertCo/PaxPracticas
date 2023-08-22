<%@ Page Title="Recupera" Theme="Alitas" Language="C#" MasterPageFile ="~/Site.master" AutoEventWireup="true" CodeFile="webInicioSesionRecupera.aspx.cs" Inherits="InicioSesion_webInicioSesionRecupera" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<link href ="../Styles/menu_style.css" rel="Stylesheet" type="text/css" /> 
    <script runat="server">   
        public void Page_PreInit()
        {
            if (Session["theme"] == null)
            {
                this.Theme = "Alitas";
            }
            else
            {
                this.Theme = Convert.ToString(Session["theme"]);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <center style="height: 542px">
    <h2>
        <span class="Titulo">
            <asp:Label ID="lblRecuperaCuenta" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRecuperaCuenta %>"></asp:Label>
        </span>
    </h2>
    <p>
        <asp:Label ID="lblRecupera" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDetRecupera %>" ForeColor="White" Font-Names="Arial" Font-Size="Medium" Font-Bold="true" ></asp:Label>
    </p>
    <br />
    <div class="accountInfo">
        <asp:Panel ID="pnlDatos" runat="server" SkinID="imagenLogin" Style="background-repeat: no-repeat; background-position:top"
            Height="222px" > 
            <br />
            <center>
                            <table style="text-align:center;">
                                <tr>
                                    <td style="text-align:center;" class="TituloBlanco">
                                    <asp:Label ID="Label1" runat="server" Text="RECUPERAR CONTRASEÑA" Font-Names="Arial" ForeColor="White" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            </center>
            <table>
                <tr>
                    <td align="left">
                        <br />
                        <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"
                            SkinID="labelLargeBlanco"></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry" TabIndex="1"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                            ControlToValidate="txtUsuario" CssClass="failureNotification" 
                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                            ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblCorreo" runat="server" AssociatedControlID="txtCorreo" Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"
                            SkinID="labelLargeBlanco"></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="textEntry" TabIndex="2"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                            ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                            ValidationGroup="RegisterUserValidationGroup"><img 
                src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                            ValidationGroup="RegisterUserValidationGroup" Width="131px"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                    </td>
                </tr>
            </table>
            <br />
        </asp:Panel>
        <br />
        <table>
            <tr>
                <td width="230">
                </td>
                <td>
                    <asp:Button ID="btnCrearCuenta" runat="server" CommandName="MoveNext"
                    ValidationGroup="RegisterUserValidationGroup" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnRecuperaCuenta %>" 
                    onclick="btnRecuperaCuenta_Click" TabIndex="3" CssClass="botonEstiloGrandeRecupera" Height="34px" 
                        Width="155px" />
                </td>
            </tr>
        </table>
        <p class="submitButton">

        </p>
    </div>
 </center>
</asp:Content>


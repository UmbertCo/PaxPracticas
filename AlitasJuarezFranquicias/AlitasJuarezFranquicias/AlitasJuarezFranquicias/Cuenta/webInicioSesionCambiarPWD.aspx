<%@ Page Title="Cambiar" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="webInicioSesionCambiarPWD.aspx.cs" Inherits="Account_ChangePassword" %>

<%@ Register src="../WebControls/usrGlobalPwd.ascx" tagname="usrGlobalPwd" tagprefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <%------------------------JQuery----------------------------%>
<script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script src="../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery.ui.core.js" type="text/javascript"></script>
<script src="../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
<script src="../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
<script src="../Scripts/progressbar.js" type="text/javascript"></script>
<link href="../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    
<%------------------------JQuery----------------------------%>

<link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />

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
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <center>
    <h2>
        <span class="Titulo">
            <asp:Label ID="lblCambiar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCambiar %>"></asp:Label>
        </span>
    </h2>
    <p>
            <asp:Label ID="lblCambiarDetalle" ForeColor="White" Font-Names="Arial" Font-Size="Medium" Font-Bold="true" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCambiarDetalle %>"></asp:Label>
    </p>
    <p>
        <asp:Label ID="lblRestricciones" runat="server" 
            Text="<%$ Resources:resCorpusCFDIEs, lblRestricciones %>" ForeColor="White" Font-Names="Arial" Font-Size="Medium" Font-Bold="true"></asp:Label>
    </p>
   <br />
        <%--<uc1:usrGlobalPwd ID="usrGlobalPwd" runat="server" />--%>
        <link href="../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
        <div class="">
            <asp:Panel ID="pnChange" runat="server" SkinID="imagenLoginCambioPWD" Style="background-repeat: no-repeat; background-position:bottom"
                Height="340px" Width="483px">
                <fieldset class="changePassword" style="width: 457px; height: 293px; text-align: left; border: none">
                <center>
                <table>
                    <tr>
                        <td>
                        <asp:Label ID="Label1" runat="server" Text="CAMBIAR CONTRASEÑA" Font-Names="Arial" ForeColor="White" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                </center>
                    <br />
                    <p>
                        <asp:Label ID="lblContraseniaAnterior" runat="server" AssociatedControlID="txtContraseniaAnterior"
                            SkinID="labelLargeBlanco" Text="<%$ Resources:resCorpusCFDIEs, lblContraseniaAnterior %>"></asp:Label>
                        <asp:TextBox ID="txtContraseniaAnterior" runat="server" CssClass="passwordEntry"
                            TextMode="Password" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" 
                            ControlToValidate="txtContraseniaAnterior" CssClass="failureNotification" 
                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaAnterior %>" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, valContraseniaAnterior %>" 
                            ValidationGroup="ChangeUserPasswordValidationGroup"><img 
                            src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        <br />
                    </p>
                    <p>
                        <asp:Label ID="lblContraseniaNueva" runat="server" AssociatedControlID="txtContraseniaNueva"
                            SkinID="labelLargeBlanco" Text="<%$ Resources:resCorpusCFDIEs, lblContraseniaNueva %>"></asp:Label>
                        <asp:TextBox ID="txtContraseniaNueva" runat="server" CssClass="passwordEntry" TextMode="Password"
                            MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" 
                            ControlToValidate="txtContraseniaNueva" CssClass="failureNotification" 
                            Display="Dynamic" 
                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>" 
                            ValidationGroup="ChangeUserPasswordValidationGroup"><img 
                            src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regxNueva" runat="server" 
                            ControlToValidate="txtContraseniaNueva" CssClass="failureNotification" 
                            Display="Dynamic" Height="16px" ToolTip="Contraseña incompleta" 
                            ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$" 
                            ValidationGroup="ChangeUserPasswordValidationGroup"><img 
                            src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                        <br />
                    </p>
                    <p>
                        <asp:Label ID="lblConfirmaNueva" runat="server" AssociatedControlID="txtConfirmaNueva"
                            SkinID="labelLargeBlanco" Text="<%$ Resources:resCorpusCFDIEs, lblConfirmaNueva %>"></asp:Label>
                        <asp:TextBox ID="txtConfirmaNueva" runat="server" CssClass="passwordEntry" TextMode="Password"
                            MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" 
                            ControlToValidate="txtConfirmaNueva" CssClass="failureNotification" 
                            Display="Dynamic" ErrorMessage="" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, valConfirmaNueva %>" 
                            ValidationGroup="ChangeUserPasswordValidationGroup"><img 
                            src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" 
                            ControlToCompare="txtContraseniaNueva" ControlToValidate="txtConfirmaNueva" 
                            CssClass="failureNotification" Display="Dynamic" ErrorMessage="" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, valConNewConf %>" 
                            ValidationGroup="ChangeUserPasswordValidationGroup"><img 
                            src="../Imagenes/error_sign.gif" /></asp:CompareValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="txtConfirmaNueva" CssClass="failureNotification" 
                            Display="Dynamic" ToolTip="Contraseña incompleta." 
                            ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$" 
                            ValidationGroup="ChangeUserPasswordValidationGroup"><img 
                            src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                        <br />
                    </p>
                </fieldset>
            </asp:Panel>
            </div>
            <p style="padding-left:350px;">
                <asp:Button ID="btnValidar" runat="server" CommandName="ChangePassword" Text="<%$ Resources:resCorpusCFDIEs, btnSiguiente %>"
                    ValidationGroup="ChangeUserPasswordValidationGroup" 
                onclick="btnValidar_Click" CssClass="botonEstilo"/>
            </p>
   
    <br />
    </center>
    </asp:Content>
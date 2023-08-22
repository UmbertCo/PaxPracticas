<%@ Control Language="C#" AutoEventWireup="true" CodeFile="usrGlobalPwd.ascx.cs"
    Inherits="WebControls_usrGlobalPwd" %>
<link href="../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
<div class="">
    <fieldset class="changePassword" style="border-style: none; border-color: inherit;
        border-width: medium; width: 510px; height: 324px; text-align: center; background-image: url('../Imagenes/loginuachRecupera.png');
        background-repeat: no-repeat;">
        <center>
            <h2>
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubUsrPass %>" />
            </h2>
            <h2>
                <asp:Label ID="lblDatosRecupera" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosCorrectos %>"></asp:Label>
            </h2>
        </center>
        <br /><br /><br />
        <table align="center">
            <tr>
                <td align="left">
                    <asp:Label ID="lblContraseniaAnterior" runat="server" AssociatedControlID="txtContraseniaAnterior"
                        Text="<%$ Resources:resCorpusCFDIEs, lblContraseniaAnterior %>" ForeColor="White"></asp:Label>
                    <asp:TextBox ID="txtContraseniaAnterior" runat="server" CssClass="passwordEntry"
                        TextMode="Password" MaxLength="50" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" Width="16px"
                        ControlToValidate="txtContraseniaAnterior" CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaAnterior %>"
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valContraseniaAnterior %>" ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblContraseniaNueva" runat="server" AssociatedControlID="txtContraseniaNueva"
                        Text="<%$ Resources:resCorpusCFDIEs, lblContraseniaNueva %>" ForeColor="White"></asp:Label>
                    <asp:TextBox ID="txtContraseniaNueva" runat="server" CssClass="passwordEntry" TextMode="Password"
                        MaxLength="50" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="txtContraseniaNueva"
                        CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>"
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>" ValidationGroup="ChangeUserPasswordValidationGroup"
                        Display="Dynamic"><img src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                    <%--<asp:RegularExpressionValidator ID="regxNueva" runat="server" ControlToValidate="txtContraseniaNueva"
                ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"
                ValidationGroup="ChangeUserPasswordValidationGroup" CssClass="failureNotification"
                ToolTip="Contraseña incompleta" Display="Dynamic" Height="16px"><img src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>--%>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblConfirmaNueva" runat="server" AssociatedControlID="txtConfirmaNueva"
                        Text="<%$ Resources:resCorpusCFDIEs, lblConfirmaNueva %>" ForeColor="White"></asp:Label>
                    <asp:TextBox ID="txtConfirmaNueva" runat="server" CssClass="passwordEntry" TextMode="Password"
                        MaxLength="50" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="txtConfirmaNueva"
                        CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valConfirmaNueva %>"
                        ValidationGroup="ChangeUserPasswordValidationGroup" Display="Dynamic" ErrorMessage=""><img src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                    <%--<asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="txtContraseniaNueva"
                ControlToValidate="txtConfirmaNueva" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valConNewConf %>"
                ValidationGroup="ChangeUserPasswordValidationGroup" Display="Dynamic" ErrorMessage=""><img src="../Imagenes/error_sign.gif" /></asp:CompareValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtConfirmaNueva"
                ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"
                ValidationGroup="ChangeUserPasswordValidationGroup" CssClass="failureNotification"
                ToolTip="Contraseña incompleta." Display="Dynamic"><img src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>--%></center>
                </td>
            </tr>
        </table>
    </fieldset>
</div>
<p style="padding-left: 350px;">
    <asp:Button ID="btnValidar" runat="server" CommandName="ChangePassword" Text="<%$ Resources:resCorpusCFDIEs, btnSiguiente %>"
        ValidationGroup="ChangeUserPasswordValidationGroup" OnClick="btnValidar_Click"
        CssClass="botonEstilo" Height="30px" Width="110px" />
</p>

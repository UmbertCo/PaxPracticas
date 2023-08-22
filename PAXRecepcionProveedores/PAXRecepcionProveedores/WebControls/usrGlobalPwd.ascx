<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="usrGlobalPwd.ascx.cs" Inherits="PAXRecepcionProveedores.WebControls.usrGlobalPwd" %>
 <link href="../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
 
 <div>
                <fieldset class="changePassword" 
                    style="border-style: none; border-color: inherit; border-width: medium; width:506px; height:318px; text-align: left; background-image: url('../Imagenes/loginrecupera2.png'); background-repeat:no-repeat;">
                    <br />
                    <table align="center">
                    <tr>
                        <td align="center">
                                 <%--<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubUsrPass %>"/>--%>
                                 <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubUsrPass%>" CssClass="tituloInicioSesion"></asp:Label>
                        </td>
                        <td width="20px"></td>
                        <%--<tr>
                                <td align="center">
                                    <asp:Label ID="lblCuentaRecupe" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblCuentaRecupera %>" CssClass="tituloInicioSesion"></asp:Label>
                                </td>
                        </tr>--%>
                    </tr>
                    </table>
                    <br /><br /><br /><br />
                    <table align="center">
                        <tr>
                            <td align="left"><asp:Label ID="lblContraseniaAnterior" runat="server" 
                                    AssociatedControlID="txtContraseniaAnterior"  
                                    Text="<%$ Resources:resCorpusCFDIEs, lblContraseniaAnterior %>"  
                                    ForeColor="Black"></asp:Label>  </td>
                        </tr>
                            <tr>
                                <td align="left"><asp:TextBox ID="txtContraseniaAnterior" runat="server" 
                            CssClass="passwordEntry" TextMode="Password" MaxLength="50" Width="200px"></asp:TextBox></td>
                                <td align="left"><asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="txtContraseniaAnterior" 
                             CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaAnterior %>" ToolTip="<%$ Resources:resCorpusCFDIEs, valContraseniaAnterior %>" 
                             ValidationGroup="ChangeUserPasswordValidationGroup" Width="20px">*</asp:RequiredFieldValidator></td>
                            </tr>
                                <tr>
                                    <td align="left"><asp:Label ID="lblContraseniaNueva" runat="server" 
                                            AssociatedControlID="txtContraseniaNueva"  
                                            Text="<%$ Resources:resCorpusCFDIEs, lblContraseniaNueva %>" ForeColor="Black"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="left"><asp:TextBox ID="txtContraseniaNueva" runat="server" CssClass="passwordEntry" 
                            TextMode="Password" MaxLength="50" Width="200px"></asp:TextBox></td>
                                    <td align="left"><asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="txtContraseniaNueva" 
                             CssClass="failureNotification" 
                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>" ToolTip="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>"
                             ValidationGroup="ChangeUserPasswordValidationGroup" Display="Dynamic"><img src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator></td>
                                    <td align="left"><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtContraseniaNueva" 
                             CssClass="failureNotification" 
                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>" ToolTip="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>"
                             ValidationGroup="ChangeUserPasswordValidationGroup" Display="Dynamic"><img src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator></td>
                             <td align="left"><asp:RegularExpressionValidator ID="regxNueva" runat="server" 
                            ControlToValidate="txtContraseniaNueva" ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"
                            ValidationGroup="ChangeUserPasswordValidationGroup" 
                            CssClass="failureNotification" 
                            ToolTip="Contraseña incompleta" Display="Dynamic" Height="16px"><img src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator></td>
                                </tr>
                                    <tr>
                                        <td align="left"><asp:Label ID="lblConfirmaNueva" runat="server" 
                            AssociatedControlID="txtConfirmaNueva"  
                                                Text="<%$ Resources:resCorpusCFDIEs, lblConfirmaNueva %>" ForeColor="Black"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="left"><asp:TextBox ID="txtConfirmaNueva" runat="server" CssClass="passwordEntry" 
                            TextMode="Password" MaxLength="50" Width="200px"></asp:TextBox></td>
                                        <td align="left"><asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="txtConfirmaNueva" 
                             CssClass="failureNotification" 
                             ToolTip="<%$ Resources:resCorpusCFDIEs, valConfirmaNueva %>" 
                            ValidationGroup="ChangeUserPasswordValidationGroup" Display="Dynamic" 
                            ErrorMessage=""><img src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator></td>
                                        <td align="left"><asp:CompareValidator ID="NewPasswordCompare" runat="server" 
                            ControlToCompare="txtContraseniaNueva" ControlToValidate="txtConfirmaNueva" 
                             CssClass="failureNotification" 
                             ToolTip="<%$ Resources:resCorpusCFDIEs, valConNewConf %>"
                             ValidationGroup="ChangeUserPasswordValidationGroup" Display="Dynamic" 
                            ErrorMessage=""><img src="../Imagenes/error_sign.gif" /></asp:CompareValidator></td>
                                        <td align="left"><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="txtConfirmaNueva" ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"
                            ValidationGroup="ChangeUserPasswordValidationGroup" 
                            CssClass="failureNotification" 
                            ToolTip="Contraseña incompleta." Display="Dynamic"><img src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator></td>
                                    </tr>
                    </table>
                </fieldset>
            </div>
            <p style="padding-left:350px;">
                <asp:Button ID="btnValidar" runat="server" CommandName="ChangePassword" Text="<%$ Resources:resCorpusCFDIEs, btnSiguiente %>"
                    ValidationGroup="ChangeUserPasswordValidationGroup" 
                onclick="btnValidar_Click" CssClass="botonEstilo"/>
            </p>
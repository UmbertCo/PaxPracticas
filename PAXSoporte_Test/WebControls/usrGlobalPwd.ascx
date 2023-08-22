<%@ Control Language="C#" AutoEventWireup="true" CodeFile="usrGlobalPwd.ascx.cs" Inherits="WebControls_usrGlobalPwd" %>
 
 <link href="../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
 
 <div class="accountInfo">
                <fieldset class="changePassword" 
                    style="width:400px; height:200px; text-align: left;" >
                <legend><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubUsrPass %>" /></legend>
                    <p>
                        <asp:Label ID="lblContraseniaAnterior" runat="server" AssociatedControlID="txtContraseniaAnterior"  Text="<%$ Resources:resCorpusCFDIEs, lblContraseniaAnterior %>"></asp:Label>
                        <asp:TextBox ID="txtContraseniaAnterior" runat="server" 
                            CssClass="passwordEntry" TextMode="Password" MaxLength="50" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="txtContraseniaAnterior" 
                             CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaAnterior %>" ToolTip="<%$ Resources:resCorpusCFDIEs, valContraseniaAnterior %>" 
                             ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblContraseniaNueva" runat="server" AssociatedControlID="txtContraseniaNueva"  Text="<%$ Resources:resCorpusCFDIEs, lblContraseniaNueva %>"></asp:Label>
                        <asp:TextBox ID="txtContraseniaNueva" runat="server" CssClass="passwordEntry" 
                            TextMode="Password" MaxLength="50" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="txtContraseniaNueva" 
                             CssClass="failureNotification" 
                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>" ToolTip="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>"
                             ValidationGroup="ChangeUserPasswordValidationGroup" Display="Dynamic"><img src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>

                        <asp:RegularExpressionValidator ID="regxNueva" runat="server" 
                            ControlToValidate="txtContraseniaNueva" ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"
                            ValidationGroup="ChangeUserPasswordValidationGroup" 
                            CssClass="failureNotification" 
                            ToolTip="Contraseña incompleta" Display="Dynamic" Height="16px"><img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>

                    </p>
                    <p>
                        <asp:Label ID="lblConfirmaNueva" runat="server" 
                            AssociatedControlID="txtConfirmaNueva"  Text="<%$ Resources:resCorpusCFDIEs, lblConfirmaNueva %>"></asp:Label>
                        <asp:TextBox ID="txtConfirmaNueva" runat="server" CssClass="passwordEntry" 
                            TextMode="Password" MaxLength="50" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="txtConfirmaNueva" 
                             CssClass="failureNotification" 
                             ToolTip="<%$ Resources:resCorpusCFDIEs, valConfirmaNueva %>" 
                            ValidationGroup="ChangeUserPasswordValidationGroup" Display="Dynamic" 
                            ErrorMessage=""><img src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" 
                            ControlToCompare="txtContraseniaNueva" ControlToValidate="txtConfirmaNueva" 
                             CssClass="failureNotification" 
                             ToolTip="<%$ Resources:resCorpusCFDIEs, valConNewConf %>"
                             ValidationGroup="ChangeUserPasswordValidationGroup" Display="Dynamic" 
                            ErrorMessage=""><img src="../../Imagenes/error_sign.gif" /></asp:CompareValidator>

                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="txtConfirmaNueva" ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"
                            ValidationGroup="ChangeUserPasswordValidationGroup" 
                            CssClass="failureNotification" 
                            ToolTip="Contraseña incompleta." Display="Dynamic"><img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                    </p>
                    
                </fieldset>
            </div>
            <p style="padding-left:350px;">
                <asp:Button ID="btnValidar" runat="server" CommandName="ChangePassword" Text="<%$ Resources:resCorpusCFDIEs, btnSiguiente %>"
                    ValidationGroup="ChangeUserPasswordValidationGroup" 
                onclick="btnValidar_Click" CssClass="botonEstilo"/>
            </p>
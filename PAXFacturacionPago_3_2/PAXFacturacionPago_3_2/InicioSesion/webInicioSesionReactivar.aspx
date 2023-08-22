<%@ Page Title="Reactivar" Language="C#" MasterPageFile="~/webGlobalMaster.master" AutoEventWireup="true" CodeFile="webInicioSesionReactivar.aspx.cs" Inherits="InicioSesion_webInicioSesionReactivar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .textEntry
        {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ContentTemplate>
    <center>
    <h2>
        <asp:Label ID="lblRecuperaCuenta" runat="server" 
            Text="<%$ Resources:resCorpusCFDIEs, lblReactivar %>"></asp:Label>
    </h2>
    <p>
        <asp:Label ID="lblReactivar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDetReactivar %>"></asp:Label>
    </p>
    <br />
    <br />
    <div class="accountInfo">
        <asp:Panel ID="pnlDatos" runat="server" BorderColor="#CCCCCC" 
            BorderStyle="Solid" BorderWidth="1px">
            <br />
            <table>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
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
                        <asp:Label ID="lblCorreo" runat="server" AssociatedControlID="txtCorreo" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"></asp:Label>
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
                            ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                    </td>
                </tr>
            </table>
            <br />
        </asp:Panel>
        <br />
        <table>
            <tr align="center">
                <td colspan="2">Captcha:</td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Image ID="ImageCaptcha" runat="server" 
                        AlternateText="If you can't read this number refresh your screen" 
                        ImageUrl="~/captcha.ashx" />
                </td>      
                <td align="left">
                    <asp:ImageButton ID="bntRecarga" runat="server" onclick="bntRecarga_Click" 
                        Height="16px" ImageUrl="~/Imagenes/reload_captcha.png" Width="16px" 
                        TabIndex="3" />
                </td>      
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtNumero" runat="server" EnableViewState="False" 
                        MaxLength="8" TabIndex="4" Width="130px"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="vrfNumero" runat="server" 
                    ControlToValidate="txtNumero" CssClass="failureNotification" 
                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNumero %>" ToolTip="<%$ Resources:resCorpusCFDIEs, txtNumero %>" 
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnCrearCuenta" runat="server" CommandName="MoveNext" 
                        ValidationGroup="RegisterUserValidationGroup" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnRecuperaCuenta %>" 
                        onclick="btnReactivarCuenta_Click" TabIndex="5" CssClass="botonGrande" Height="30px" />
                </td>
                <td></td>
            </tr>
        </table>

        <p class="submitButton">

        </p>
    </div>
    </center>
    </ContentTemplate>
</asp:Content>


<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webInicioSesionReactivar.aspx.cs" MasterPageFile="~/Site.Master" Inherits="InicioSesion_webInicioSesionReactivar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
<link href="../Styles/menu_style.css" rel="stylesheet" type="text/css" />
    <ContentTemplate>

    <center>
    
    <p>
        <asp:Label ID="lblReactivar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDetReactivar %>"></asp:Label>
    </p>
    <br />
    <br />
    <div class="accountInfo">
        <asp:Panel ID="pnlDatos" runat="server" BackImageUrl="~/Imagenes/loginuach.png" Height="335px" Width="494px">
            <br />
            <table>
                <tr>
                    <td align="center"> 
                         <h2>
                              <asp:Label ID="lblRecuperaCuenta" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblReactivar %>"></asp:Label>
                         </h2>
                    </td>
                </tr>
                    <tr>
                        <td align="center">
                        <h2>
                              <asp:Label ID="lblIngresaDatosReactivar" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblIngresaDatosReactivar %>"></asp:Label>
                         </h2>
                        </td>
                    </tr>
            </table>
            <br />
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
                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry" TabIndex="1" 
                            Width="250px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                            ControlToValidate="txtUsuario" CssClass="failureNotification" 
                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                            ValidationGroup="RegisterUserValidationGroup" Height="20px" Width="16px">*</asp:RequiredFieldValidator>
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
                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="textEntry" TabIndex="2" 
                            Width="250px"></asp:TextBox>
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
                            ValidationGroup="RegisterUserValidationGroup"><img 
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
                            onclick="btnReactivarCuenta_Click" TabIndex="3" CssClass="botonGrande" Height="48px" 
                                        Width="187px" />
                                </td>
                            </tr>
        </table>
        <p class="submitButton">

        </p>
    </div>
    </center>
    </ContentTemplate>
</asp:Content>

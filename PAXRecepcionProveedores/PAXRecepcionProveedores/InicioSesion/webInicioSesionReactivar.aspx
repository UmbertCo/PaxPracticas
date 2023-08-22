<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="webInicioSesionReactivar.aspx.cs" Inherits="PAXRecepcionProveedores.InicioSesion.webInicioSesionReactivar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .textEntry
        {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ContentTemplate>
    <table align="center">
    <tr>
        <td>
            
        </td>
        <tr>
            <td align="center">
            <asp:Label ID="lblReactivar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDetReactivar %>" Font-Size="Medium" 
            Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </tr>
    </table>
    <br />
    <br />
    <div align="center" class="accountInfo">
        <asp:Panel ID="pnlDatos" runat="server" BackImageUrl="~/Imagenes/login.png"  
            Height="340px" Width="495px">
            <br />
            <table align="center">
                <tr>
                    <td>
                    <h2>
                <asp:Label ID="lblRecuperaCuenta" runat="server" CssClass="tituloInicioSesion" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblReactivar %>"></asp:Label>
                    
            </h2>
                    </td>
                  </tr>
              </table>
            <br /><br /><br />
              <table align="center">
                <td>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" CssClass="tituloInicioSesion" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblDatosCorrectos %>"></asp:Label>
                        </td>
                    </tr>
                </td>
              </table>
            <br /><br />
            <table align="center">
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
                            ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
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
                            ValidationGroup="RegisterUserValidationGroup" Width="131px"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                    </td>
                </tr>
            </table>
            <br />
        </asp:Panel>
                        <table>
                            <tr>
                                <td width="230">
                                </td>
                                <td>
                        <asp:Button ID="btnCrearCuenta" runat="server" CommandName="MoveNext" 
                            ValidationGroup="RegisterUserValidationGroup" 
                            Text="<%$ Resources:resCorpusCFDIEs, btnRecuperaCuenta %>" 
                            onclick="btnReactivarCuenta_Click" TabIndex="3" CssClass="botonGrande" Height="33px" 
                                        Width="197px" />
                                </td>
                            </tr>
        </table>
        <p class="submitButton">

        </p>
    </div>
    </ContentTemplate>
</asp:Content>

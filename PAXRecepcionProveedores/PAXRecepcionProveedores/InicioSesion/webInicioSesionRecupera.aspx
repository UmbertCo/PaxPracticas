<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="webInicioSesionRecupera.aspx.cs" Inherits="PAXRecepcionProveedores.InicioSesion.webInicioSesionRecupera" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ContentTemplate>
    <center>
    <h2>
        &nbsp;</h2>
    <p>
        <asp:Label ID="lblRecupera" runat="server" 
            Text="<%$ Resources:resCorpusCFDIEs, lblDetRecupera %>" Font-Size="Medium" 
            Font-Bold="True"></asp:Label>
    </p>
    <%--<br />
    <br />--%>
    <div class="accountInfo">
        <asp:Panel ID="pnlDatos" runat="server" BackImageUrl="~/Imagenes/login.png"  
            Height="340px" Width="495px">
            <br /><br />
            <table>
                <tr>
                    <td align="center">
                    
                        <asp:Label ID="lblRecuperaCuenta" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblRecuperaCuenta %>" CssClass="tituloInicioSesion"></asp:Label>
                    
                    </td>
                    <tr>
                    <td align="center"><asp:Label ID="lblCuentaRecupe" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblCuentaRecupera %>" CssClass="tituloInicioSesion"></asp:Label></td>
                    </tr>
                </tr>
            </table>
                <br /><br />
                <table>
                    <tr>
                        <td align="center">
                        
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosCorrectos %>" CssClass="tituloInicioSesion"></asp:Label>
                        
                        </td>
                    </tr>
                </table>
            <br />
            <table>
                <center>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry" TabIndex="1" 
                            Height="18px" Width="220px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                            ControlToValidate="txtUsuario" CssClass="failureNotification" 
                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                            ValidationGroup="RegisterUserValidationGroup" Height="18px">*</asp:RequiredFieldValidator>
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
                    <td align="left">
                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="textEntry" TabIndex="2" 
                            Height="17px" Width="220px"></asp:TextBox>
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
               </center>
              </table>
              <br /><br /><br /><br /><br /><br /><br />
              <table align="right">
                <tr>
                    <td>
                        <asp:Button ID="btnCrearCuenta" runat="server" CommandName="MoveNext" 
                CssClass="botonGrande" Height="37px" onclick="btnRecuperaCuenta_Click" 
                TabIndex="3" Text="<%$ Resources:resCorpusCFDIEs, btnRecuperaCuenta %>" 
                ValidationGroup="RegisterUserValidationGroup" Width="186px" />
                    </td>
                </tr>
              </table>
            <br />
            
        </asp:Panel>
                        <br />
                        <table>
                            <tr>
                                <td width="230">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
        </table>
        <p class="submitButton">

        </p>
    </div>
    </center>
    </ContentTemplate>
</asp:Content>

<%@ Page Title="Inicio de Sesión" Language="C#" MasterPageFile="~/Privada.master" AutoEventWireup="true"
    CodeFile="webInicioSesionLogin.aspx.cs" Inherits="Account_Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    </asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <center>
    <h2 >
        <asp:Label ID="lblIncioSesion" runat="server" 
            
            style="font-family: 'Century Gothic'; font-size: xx-large; "></asp:Label>
    </h2>
    <p>
        <span class="Apple-style-span" 
            
            style="border-collapse: separate; color: rgb(0, 0, 0); font-family: verdana; font-size: 12px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-border-horizontal-spacing: 0px; -webkit-border-vertical-spacing: 0px; -webkit-text-decorations-in-effect: none; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; " 
            dir="ltr">
        <asp:Label ID="lblBienvenida" runat="server" 
            style="font-family: 'Century Gothic'; font-size: medium"></asp:Label>
        </span>
    </p>
    <p>
        &nbsp;</p>
    <table>
        <tr>
            <td>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="updLogin" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pmlLogin" runat="server" BorderColor="#CCCCCC" 
                                        BorderStyle="Solid" BorderWidth="1px" Height="125px" Width="440px">
                                        <center>

                    <br />

                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUserName" 
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>" 
                                                            style="font-family: 'Century Gothic'"></asp:Label>
                                <br />
                                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="textEntry" 
                                                            MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td>
                                <br />
                                                        <asp:RequiredFieldValidator ID="rfvUsaurioRequerido" runat="server" 
                                                            ControlToValidate="txtUserName" CssClass="failureNotification" 
                                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                                            ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblContrasenia" runat="server" AssociatedControlID="txtPassword" 
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>" 
                                                            style="font-family: 'Century Gothic'"></asp:Label>
                                <br />
                                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="passwordEntry" 
                                                            MaxLength="50" TextMode="Password"></asp:TextBox>
                                                    </td>
                                                    <td>
                                <br />
                                                        <asp:RequiredFieldValidator ID="rfvContrasenaRequerida" runat="server" 
                                                            ControlToValidate="txtPassword" CssClass="failureNotification" 
                                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>" 
                                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>" 
                                                            ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </asp:Panel>
                                    <table>
                                        <tr>
                                            <td width="380">
                                                &nbsp;</td>
                                            <td>
                                                <asp:Button ID="btnEntrar" runat="server" onclick="btnEntrar_Click" CommandName="Login"
                                                    Text="<%$ Resources:resCorpusCFDIEs, btnEntrar%>" ValidationGroup="LoginUserValidationGroup" 
                                                    style="font-family: 'Century Gothic'" CssClass="botonEstilo" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                    <table id="ControlesRegistro">
                        <tr>
                            <td align="left" width="300">
                                <asp:HyperLink ID="HyperLink1" runat="server" Text= "<%$ Resources:resCorpusCFDIEs, btnRecuperaCuenta %>"
                                    NavigateUrl="~/Pantallas/Login/webRecuperacionPassword.aspx"></asp:HyperLink>
                            </td>
                            <td style="width: 200px" align="left">
                                &nbsp;</td>
                        </tr>

                    </table>
                   

                   <br />
                   

                     <table id="ControlesActivar">
                        <tr>
                            <td align="left" width="300">
                                &nbsp;</td>
                            <td style="width: 200px" align="left">
                                &nbsp;</td>
                        </tr>
                    </table>
                                               <br />
                   

                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>

            </td>
        </tr>
    </table>
    </center>
</asp:Content>
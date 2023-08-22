<%@ Page Title="Inicio de Sesión" Language="C#" MasterPageFile="~/webGlobalMaster.master" AutoEventWireup="true"
    CodeFile="webInicioSesionLogin.aspx.cs" Inherits="Account_Login"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .modal
        {
            padding: 10px 10px 10px 10px;
            border:1px solid #333333;
            background-color:White;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />

    <center>
    <h2 align="right" >

                    &nbsp;</h2>
        <h2 >
        <asp:Label ID="lblIncioSesion" runat="server"></asp:Label>
    </h2>
        <p>
                    &nbsp;<asp:UpdateProgress runat="server" 
            AssociatedUpdatePanelID="updLogin" ID="updProgress"><ProgressTemplate>
                                <img alt="" 
                            src="../Imagenes/imgAjaxLoader.gif" />
                            
</ProgressTemplate>
</asp:UpdateProgress>

        INSTANCIA DESCONECTADA CONSUMIENDO WCF</p>
    </center>
    <center>
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
                                        BorderStyle="Solid" BorderWidth="1px" Height="129px" Width="440px">
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
                                                            ValidationGroup="LoginUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
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
                                                            ValidationGroup="LoginUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </asp:Panel>
                                    <table>
                                        <tr>
                                            <td width="350">
                                                &nbsp;</td>
                                            <td>
                                                <asp:Button ID="btnEntrar" runat="server" onclick="btnEntrar_Click" 
                                                    Text="<%$ Resources:resCorpusCFDIEs, lblEntrar %>" ValidationGroup="LoginUserValidationGroup" 
                                                    style="font-family: 'Century Gothic'" CssClass="botonEstilo" 
                                                    Width="75px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <br />
                        </td>
                    </tr>
                    </table>

            </td>
        </tr>
    </table>
        <br />
        <br />
        <br />
    </center>
</asp:Content>
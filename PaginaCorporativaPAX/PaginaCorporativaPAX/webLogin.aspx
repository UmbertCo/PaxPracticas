<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="webLogin.aspx.cs" Inherits="PaginaComercial.webLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <link rel="shortcut icon" href="imagenes/fav.ico" />
    <style type="text/css">
        .style8
        {
            height: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <center>
    <asp:UpdateProgress ID="updProgress" runat="server" 
        AssociatedUpdatePanelID="updLogin">
        <ProgressTemplate>
                                <img alt="" 
                            src="imagenes/imgAjaxLoader.gif" />
                            
            </ProgressTemplate>
    </asp:UpdateProgress>
    </center>
    <table align = "center">
        <tr>
            <td>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="updLogin" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pmlLogin" runat="server" BorderColor="#CCCCCC" 
                                        BorderStyle="Solid" BorderWidth="1px" Height="170px" Width="440px">
                                        <center>
                                            <br />
                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUserName" 
                                                            style="font-family: 'Century Gothic'; font-size: small;" 
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>" ForeColor="#395C6C"></asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="textEntry" 
                                                            MaxLength="50" Width="250px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <br />
                                                        <asp:RequiredFieldValidator ID="rfvUsaurioRequerido" runat="server" 
                                                            ControlToValidate="txtUserName" CssClass="failureNotification" 
                                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                                            ValidationGroup="LoginUserValidationGroup" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblContrasenia" runat="server" AssociatedControlID="txtPassword" 
                                                            style="font-family: 'Century Gothic'; font-size: small;" 
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>" 
                                                            ForeColor="#395C6C"></asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="passwordEntry" 
                                                            MaxLength="50" TextMode="Password" Width="250px"></asp:TextBox>
                                                        <br />
                                                    </td>
                                                    <td>
                                                        <br />
                                                        <asp:RequiredFieldValidator ID="rfvContrasenaRequerida" runat="server" 
                                                            ControlToValidate="txtPassword" CssClass="failureNotification" 
                                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>" 
                                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>" 
                                                            ValidationGroup="LoginUserValidationGroup" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="style8" height="50px">
                                                        <table align="right">
                                                            <tr>
                                                                <td class="style7">
                                                                    &nbsp;</td>
                                                                <td align="left">
                                                                    <asp:Button ID="btnEntrar" runat="server" CssClass="botonGrande" 
                                                                        Font-Bold="True" Height="29px" onclick="btnEntrar_Click" 
                                                                        style="font-family: 'Century Gothic'" 
                                                                        Text="<%$ Resources:resCorpusCFDIEs, btnEntrar%>" 
                                                                        ValidationGroup="LoginUserValidationGroup" Width="163px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td class="style8">
                                                        </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </asp:Panel>
                                    <br />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <center>
                            <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="#CC0000" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblPasswordError %>" Visible="False"></asp:Label>
                            <br />
                            </center>
                        </td>
                    </tr>
                    </table>
            </td>
        </tr>
    </table>


</asp:Content>


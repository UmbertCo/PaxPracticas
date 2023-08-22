<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="webInicioSesionRecupera.aspx.cs" Inherits="InicioSesion_webInicioSesionRecupera" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
<style type="text/css">
        .modal
        {
            padding: 10px 10px 10px 10px;
            border: 1px solid #333333;
            background-color: White;
        }
        .modal p
        {
            width: 600px;
            text-align: right;
        }
        .modal div
        {
            width: 600px;
            vertical-align: top;
        }
        .modal div p
        {
            text-align: left;
        }
        .imagenModal
        {
            height: 15px;
            cursor: pointer;
        }
        .modalBackground
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .style7
        {
            width: 300px;
        }
        .style8
        {
            width: 330px;
        }
        .style11
        {
            width: 400px;
            height: 33px;
        }
        .style12
        {
            width: 400px;
        }
    </style>
     <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <link href="../../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
<center>
    <link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/menu_style.css" rel="stylesheet" type="text/css" />
        <p>
            <asp:Label ID="lblRecupera" runat="server" Style="font-family: 'Century Gothic'" Font-Bold="False" Font-Size="Medium" 
            ForeColor="Black" Text="<%$ Resources:resCorpusCFDIEs, lblDetRecupera %>"></asp:Label>
        </p>
        <div class="accountInfo" class="modal" align="center">
            <asp:Panel ID="pnlDatos" runat="server" Height="331px" BackImageUrl="~/Imagenes/loginuach.png" 
                Width="499px">
                <br />
            <table>
                <tr>
                    <td>
                        <h2>
                            <asp:Label ID="lblRecuperaCuenta" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRecuperaCuenta %>"></asp:Label>
                        </h2>
                    </td>
                </tr>     
            </table>
              <table>
                <tr>
                    <td>
                        <h2>
                            
                            <asp:Label ID="lblRecuperaDatosCo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRecuperaDatosCo %>"></asp:Label>
                            
                        </h2>
                    </td>
                </tr>
              </table>
                <table>
                    <caption>
                        <br />
                        <br />
                        <br />
                        <br />
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" ForeColor="White"
                                    Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
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
                                    Height="20px" ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                    ValidationGroup="RegisterUserValidationGroup">
                                    <img src="../Imagenes/error_sign.gif" />
                               </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblCorreo" runat="server" AssociatedControlID="txtCorreo" ForeColor="White" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
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
                    </caption>
                </table>
                <br />
            </asp:Panel>
            <br />
            <table align="right">
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnCrearCuenta" runat="server" CommandName="MoveNext" ValidationGroup="RegisterUserValidationGroup"
                            Text="<%$ Resources:resCorpusCFDIEs, btnRecuperaCuenta %>" OnClick="btnRecuperaCuenta_Click"
                            TabIndex="3" CssClass="botonGrande" Height="32px" Width="175px" />
                    </td>
                </tr>
            </table>
            <p class="submitButton">
            </p>
        </div>
    </center>
</asp:Content>

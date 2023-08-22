<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webInicioSesionLogin.aspx.cs" Inherits="InicioSesion_webInicioSesionLogin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .modal
        {
            padding: 10px 10px 10px 10px;
            border: 1px solid #333333;
            background-color: White;
        }
        .style4
        {
            width: 148px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/menu_style.css" rel="stylesheet" type="text/css" />
    <center>
        <div align="right">
            <%--Link para descargar el manual de usuario--%>
            <asp:HyperLink ID="lnkTutoriales" runat="server" ForeColor="#85078B" Font-Bold="True"
                Font-Names="Century Gothic" Font-Size="Medium" Width="160px" Text="Tutoriales de Ayuda"
                NavigateUrl="~/Tutoriales.aspx">
            </asp:HyperLink>
        </div>
        
        <h2 align="right">
            <asp:ImageButton ID="imgDescarga" Visible="false" runat="server" 
                        ImageUrl="~/Imagenes/DescMUsuario.png" 
            onclick="imgDescarga_Click" />
        </h2>
        <%--<h2>
            <asp:Label ID="lblIncioSesion" runat="server"></asp:Label>
        </h2>--%>
            <span class="Apple-style-span" style="border-collapse: separate; color: rgb(0, 0, 0);
                font-family: verdana; font-size: 12px; font-style: normal; font-variant: normal;
                font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2;
                text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: normal;
                widows: 2; word-spacing: 0px; -webkit-border-horizontal-spacing: 0px; -webkit-border-vertical-spacing: 0px;
                -webkit-text-decorations-in-effect: none; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px;"
                dir="ltr">
                <asp:Label ID="lblBienvenida" runat="server" 
            Style="font-family: 'Century Gothic'" Font-Bold="False" Font-Size="Medium" 
            ForeColor="Black"></asp:Label>
            </span>
        <p>
            <span class="Apple-style-span" style="border-collapse: separate; color: rgb(0, 0, 0);
                font-family: verdana; font-size: 12px; font-style: normal; font-variant: normal;
                font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2;
                text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: normal;
                widows: 2; word-spacing: 0px; -webkit-border-horizontal-spacing: 0px; -webkit-border-vertical-spacing: 0px;
                -webkit-text-decorations-in-effect: none; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px;">
                <span class="Apple-style-span" style="color: rgb(100, 100, 100); font-family: Arial, Helvetica, sans-serif;
                    text-align: left;">
                    <asp:Label ID="lblProveedor" runat="server" Style="font-family: 'Century Gothic'"></asp:Label>
                </span></span>
        </p>
        <p>
            &nbsp;<asp:UpdateProgress runat="server" AssociatedUpdatePanelID="updLogin" ID="updProgress">
                <ProgressTemplate>
                    <img alt="" src="../Imagenes/imgAjaxLoader.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>
        </p>
    </center>
    <center style="width: 935px">
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="updLogin" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="pmlLogin" runat="server"
                                            Height="327px" Width="489px" BackImageUrl="~/Imagenes/loginuach.png"> 
                                            <center>
                                               <br />
                                               <table>
                                                <tr>
                                                        <td>
                                                            <h2>
                                                                <asp:Label ID="lblIncioSesion" runat="server"></asp:Label>
                                                            </h2>
                                                        </td>
                                                </tr>
                                               </table>
                                               <table>
                                                <tr>
                                                    <td>
                                                        <h2>
                                                         <asp:Label ID="lblDatos" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosCorrectos %>"></asp:Label>
                                                        </h2>
                                                    </td>
                                                </tr>
                                               </table>
                                                <br />
                                                <table>
                                                     <caption>
                                                         <br />
                                                         <br />
                                                         <br />
                                                         <tr>
                                                             <td align="left">
                                                                 <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUserName" 
                                                                     ForeColor="White" Style="font-family: 'Century Gothic'" 
                                                                     Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
                                                                 <br />
                                                                 <asp:TextBox ID="txtUserName" runat="server" CssClass="textEntry" 
                                                                     MaxLength="50" Width="250px"></asp:TextBox>
                                                             </td>
                                                             <td colspan="0">
                                                                 <br />
                                                                 <asp:RequiredFieldValidator ID="rfvUsaurioRequerido" runat="server" 
                                                                     ControlToValidate="txtUserName" CssClass="failureNotification" 
                                                                     ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                                                     ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                                                     ValidationGroup="LoginUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                                                             </td>
                                                         </tr>
                                                     </caption>
                                                     </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblContrasenia" runat="server" AssociatedControlID="txtPassword" Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>"
                                                                Style="font-family: 'Century Gothic'" ForeColor="White"></asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtPassword" runat="server" CssClass="passwordEntry" MaxLength="50"
                                                                TextMode="Password"  Width="250px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <br />
                                                            <asp:RequiredFieldValidator ID="rfvContrasenaRequerida" runat="server" ControlToValidate="txtPassword"
                                                                CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>"
                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>" ValidationGroup="LoginUserValidationGroup"
                                                                Width="16px">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </asp:Panel>
                                        <table>
                                            <tr>
                                                <td width="350">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnEntrar" runat="server" OnClick="btnEntrar_Click" Text="<%$ Resources:resCorpusCFDIEs, lblEntrar %>"
                                                        ValidationGroup="LoginUserValidationGroup" Style="font-family: 'Century Gothic'"
                                                        CssClass="botonEstilo" Width="94px" Height="28px" />
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
                                    <td width="60px"></td>
                                        <td align="left" class="style4">
                                            <%-- <asp:Label ID="lblRegistrate" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRegistrate %>"
                                                Style="font-family: 'Century Gothic'"></asp:Label>--%>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRegistroProv %>"
                                                NavigateUrl="~/InicioSesion/webAltaProveedores.aspx" ForeColor="#85078B" 
                                                Font-Bold="True" Font-Names="Century Gothic" Font-Size="Medium" Width="153px"></asp:HyperLink>
                                        </td>
                                        <td style="color: #9900CC"> | </td>
                                        <td> <asp:HyperLink ID="hpkRecuperar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, hpkRecuperar %>"
                                                NavigateUrl="~/InicioSesion/webInicioSesionRecupera.aspx" 
                                                ForeColor="#85078B" Font-Bold="True" Font-Names="Century Gothic" 
                                                Font-Size="Medium"></asp:HyperLink></td>
                                                
                                        <td> </td>
                                        <%--  <td style="width: 200px" align="left">--%>
                                        <%--<asp:HyperLink ID="hpkRegistrar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRegistrar %>"
                                                NavigateUrl="~/InicioSesion/webInicioSesionRegistro.aspx"></asp:HyperLink>--%>
                                        <%--  </td>--%>
                                        
                                    </tr>
                                    <tr>
                                        <td> <asp:HyperLink Visible="false" ID="HyperLink2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, hpkReactivar %>"
                                                NavigateUrl="~/InicioSesion/webInicioSesionReactivar.aspx" 
                                                ForeColor="#85078B" Font-Bold="True" Font-Names="Century Gothic" 
                                                Font-Size="Medium" Width="160px"></asp:HyperLink></td>
                                    </tr>
                                </table>
                                <br />
                                            
                                <br />
                                <table id="ControlesActivar">
                                    
                                </table>
                            </td>
                        </tr>
                    </table>
                    <p class="submitButton">
                        &nbsp;</p>
                </td>
                <td>
                    <table>
                        <tr>
                            <td style="height: 130px">
                                <%--<img alt="SAT" class="style3" src="../Imagenes/proveedor-autorizado-de-certificación.jpg" />--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <%--<table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="lblBloqMsg" runat="server" Style="font-size: x-small; color: #990000"
                        Text="<%$ Resources:resCorpusCFDIEs, varBloqMsg %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:LinkButton ID="lnkDesbloq" runat="server" OnClick="lnkDesbloq_Click" Style="font-size: x-small"
                        Text="Desbloquear"></asp:LinkButton>
                </td>
            </tr>
        </table>--%>
        <br />
        <asp:Panel ID="pnlBloqueo" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="modal"
            Height="150px" Width="500px">
            <table>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblBloqCon" runat="server" Style="font-size: small" Text="Para desbloquear el sistema es necesario ingresar sus credenciales, para confirmar su identidad."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <center>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td rowspan="2">
                                        <img alt="" src="../Imagenes/Informacion.png" width="44" />
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblUserBloq" runat="server" AssociatedControlID="txtUserNameBloq"
                                            Text="Usuario:"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtUserNameBloq" runat="server" CssClass="textEntry" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td>
                                        <br />
                                        <asp:RequiredFieldValidator ID="rfvUsaurioBloq" runat="server" ControlToValidate="txtUserNameBloq"
                                            CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>"
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" ValidationGroup="BloqGroup"
                                            Width="16px">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30px">
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblContraseniaBloq" runat="server" AssociatedControlID="txtPasswordBloq"
                                            Text="Contraseña:"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtPasswordBloq" runat="server" CssClass="passwordEntry" MaxLength="50"
                                            TextMode="Password"></asp:TextBox>
                                    </td>
                                    <td>
                                        <br />
                                        <asp:RequiredFieldValidator ID="rfvPassBloq" runat="server" ControlToValidate="txtPasswordBloq"
                                            CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>"
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>" ValidationGroup="BloqGroup"
                                            Width="16px">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnBloqueo" runat="server" CssClass="botonEstilo" OnClick="btnBloqueo_Click"
                                            Text="<%$ Resources:resCorpusCFDIEs, varBloqBtn %>" 
                                            ValidationGroup="BloqGroup" Height="25px" Width="108px" />
                                        <asp:Button ID="btnCanBloq" runat="server" CssClass="botonEstilo" OnClick="btnCanBloq_Click"
                                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Height="25px" 
                                            Width="80px" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <cc1:ModalPopupExtender ID="modalBlock" runat="server" BackgroundCssClass="modalBackground"
            PopupControlID="pnlBloqueo" PopupDragHandleControlID="" CancelControlID="btnCanBloq"
            TargetControlID="lnkBlock">
        </cc1:ModalPopupExtender>
        <asp:LinkButton ID="lnkBlock" runat="server"></asp:LinkButton>
    </center>
</asp:Content>

<%@ Page Title="Inicio de Sesión" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webInicioSesionLogin.aspx.cs" Inherits="InicioSesion_webInicioSesionLogin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .modal
        {
            padding: 10px 10px 10px 10px;
            border: 1px solid #333333;
            background-color: White;
          
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/menu_style.css" rel="stylesheet" type="text/css" />
   <table width="820px">
    <tr>
    
        <td align="right">
        <h2 align="right">
            <%--Link para descargar el manual de usuario--%>
            <asp:ImageButton ID="imgDescarga" runat="server" 
                        ImageUrl="~/Imagenes/DescMUsuario.png" 
            onclick="imgDescarga_Click" />
        </h2>        
        </td>
    </tr>
   </table>
    
         <center>
        <p>
            <span class="Apple-style-span" style="border-collapse: separate; color: rgb(0, 0, 0);
                font-family: verdana; font-size: 12px; font-style: normal; font-variant: normal;
                font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2;
                text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: normal;
                windows: 2; word-spacing: 0px; -webkit-border-horizontal-spacing: 0px; -webkit-border-vertical-spacing: 0px;
                -webkit-text-decorations-in-effect: none; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px;"
                dir="ltr">
                <asp:Label ID="lblBienvenida" runat="server" 
                Style="font-family: 'Avenir LT Std 35 Light'" Font-Names="Avenir LT Std 35 Light" 
                Font-Size="X-Large"></asp:Label> 
            </span>
        </p>
        <p>
            <span class="Apple-style-span" style="border-collapse: separate; color: rgb(0, 0, 0);
                font-family: verdana; font-size: 12px; font-style: normal; font-variant: normal;
                font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2;
                text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: normal;
                windows: 2; word-spacing: 0px; -webkit-border-horizontal-spacing: 0px; -webkit-border-vertical-spacing: 0px;
                -webkit-text-decorations-in-effect: none; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px;">
                <span class="Apple-style-span" style="color: rgb(100, 100, 100); font-family: Arial, Helvetica, sans-serif;
                    text-align: left;">
                    <asp:Label ID="lblProveedor" runat="server" 
                Style="font-family: 'Avenir LT Std 35 Light'" Font-Size="XX-Large"></asp:Label>
                </span></span>
        </p>
        <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="updLogin" ID="updProgress">
                <ProgressTemplate>
                    <img alt="" src="../Imagenes/imgAjaxLoader.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>
    </center>
            <center>
                    <table>
                        <tr>
                            <td>
                            <table>
                            <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="updLogin" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="pmlLogin" runat="server"
                                            Height="322px" Width="491px" BackImageUrl="~/Imagenes/login.png" DefaultButton="btnEntrar">
                                            <center>
                                               <br /><br /><br />
                                                <center>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblIncioSesion" runat="server" CssClass="tituloInicioSesion" Text="<%$ Resources:resCorpusCFDIEs, lblInicioSesion %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table> 
                                                    <br /><br />
                                                        <table>
                                                            <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosCorrectos %>" CssClass="tituloInicioSesion"></asp:Label>
                                                                    </td>
                                                            </tr>
                                                        </table>
                                                </center>
                                                <br /><br />
                                                <table>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUserName" Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"
                                                                Style="font-family: 'Century Gothic'" ForeColor="White"></asp:Label>
                                                            <br />
                                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="textEntry" MaxLength="50"  Width="250px"></asp:TextBox>
                                                        </td>
                                                        <td colspan="0" rowspan="0">
                                                            <br />
                                                            <asp:RequiredFieldValidator ID="rfvUsaurioRequerido" runat="server" ControlToValidate="txtUserName"
                                                                CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>"
                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" ValidationGroup="LoginUserValidationGroup"
                                                                Width="16px">*</asp:RequiredFieldValidator>
                                                        </td>
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

                                                        <tr>
                                                            <td>
                                                            </td>  
                                                            <td>
                                                            </td>
                                                        </tr>
                                                </table>
                                                <br /><br /><br /><br /><br />
                                                <table align="right">
                                                    <tr>
                                                        <td align="right">
                                                                <asp:Button ID="btnEntrar" runat="server" CssClass="botonEstilo" 
                                                                    OnClick="btnEntrar_Click" Style="font-family: 'Century Gothic'" 
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lblEntrar %>" 
                                                                    ValidationGroup="LoginUserValidationGroup" Width="75px" />
                                                                        </td>
                                                    </tr>
                                                </table>
                                            </center> 
                                        </asp:Panel>
                                        <br />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="ControlesRegistro">
                                <tr>
                                    <td width="100px"> </td>
                                    <td align="left"> <asp:HyperLink ID="HyperLink1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRegistroProveedores %>"
                                                NavigateUrl="~/InicioSesion/webAltaProveedores.aspx" Font-Names="Avenir LT Std 45 Normal" 
                                                ForeColor="#8B181B" Font-Bold="True"></asp:HyperLink> </td>
                                                <td style="color: #800000"> | </td>
                                    <td>  <asp:HyperLink ID="hpkRecuperar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, hpkRecuperar %>"
                                                NavigateUrl="~/InicioSesion/webInicioSesionRecupera.aspx" 
                                                Font-Names="Avenir LT Std 45 Normal" Font-Bold="True" ForeColor="#8B181B"></asp:HyperLink> </td>
                                    <td> </td>
                                </tr>
                                    <%--<tr>
                                    <td width="120px"></td>
                                        <td align="left" width="200">--%>
                                           <%-- <asp:Label ID="lblRegistrate" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRegistrate %>"
                                                Style="font-family: 'Century Gothic'"></asp:Label>--%>
                                        <%--</td>
                                        <td style="width: 200px" align="left">--%>
                                            <%--<asp:HyperLink ID="hpkRegistrar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lnkDescargaFacturas %>"
                                                NavigateUrl="~/Operacion/Clientes/webDescargaComprobantes.aspx" 
                                                Font-Names="Avenir LT Std 45 Normal" Font-Bold="True" ForeColor="#8B181B"></asp:HyperLink>--%>
                                        <%--</td>
                                         <td align="left" width="200">
                                        </td>
                                    </tr>--%>
                                </table>
                                <br />
                                <br />
                                <table id="ControlesActivar">
                                    <tr>
                                       
                                        <td style="width: 200px" align="left">
                                            <%--<asp:HyperLink ID="hpkReactivar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, hpkReactivar %>"
                                                NavigateUrl="~/InicioSesion/webInicioSesionReactivar.aspx" 
                                                Font-Bold="True" ForeColor="#8B181B"></asp:HyperLink>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
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
        <table align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="lblBloqMsg" runat="server" Style="font-size: x-small; color: #990000"
                        Text="<%$ Resources:resCorpusCFDIEs, varBloqMsg %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:LinkButton ID="lnkDesbloq" runat="server" OnClick="lnkDesbloq_Click" Style="font-size: x-small"
                        Text="<%$ Resources:resCorpusCFDIEs, varBloqBtn %>"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="pnlBloqueo"  runat="server" style="background-repeat:no-repeat" 
                        BorderStyle="Double" BorderWidth="1px" CssClass="modal"
            Height="289px" Width="495px" BackImageUrl="~/Imagenes/logindesbloquear.png">
            <br /><br /><br />
            <table>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblBloqCon" runat="server"  Style="font-family: 'Century Gothic'" ForeColor="White"
                        Text="<%$ Resources:resCorpusCFDIEs, varBloqCon %>"  Width="400px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    <br />
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
                                            Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"  
                                            Style="font-family: 'Century Gothic'" ForeColor="White">:</asp:Label>
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
                                            Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>"
                                             Style="font-family: 'Century Gothic'" ForeColor="White">:</asp:Label>
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
                                                    
                                                </td>
                                                <td>
                                                    
                                                </td>
                                            </tr>
                                                </table>
                                                <br /><br /><br /><br />
                                                <table align="right">
                                                    <tr>
                                                        <td></td>
                                                        <td align="right">
                                                            <asp:Button ID="btnBloqueo" runat="server" CssClass="botonDesbloquear" 
                                                        Height="37px" OnClick="btnBloqueo_Click" 
                                                        Text="<%$ Resources:resCorpusCFDIEs, varBloqBtn %>" ValidationGroup="BloqGroup" 
                                                        />
                                                        </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnCanBloq" runat="server" CssClass="botonEstilo" 
                                                                    OnClick="btnCanBloq_Click" 
                                                                    Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" />
                                                            </td>
                                                    </tr>
                                                </table>
                                        </center>
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

    <asp:Panel ID="pnlBienvenida" runat="server" BorderStyle="Double" BackColor="White" BackImageUrl="~/mnu_images/backmodal.jpg" Width="877px" Height="642px">
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
        <br /><br /><br />
        <table>
            
            <tr><td style="width:100px"></td>
            <td align="center">
            <div style="width:362px; height:269px">
            <asp:LinkButton ID="lnkCliente" runat="server" PostBackUrl="~/Operacion/Clientes/webDescargaComprobantes.aspx" >
                <img src="../mnu_images/btn1.jpg" border="0" />
                </asp:LinkButton>
            </div>
            </td>
            <td style="width:50px"></td>
            <td align="center">
            <div style="width:362px; height:269px">
            <asp:LinkButton ID="lnkProveedor" runat="server" onclick="lnkProveedor_Click">
                <img src="../mnu_images/btn.jpg" border="0" />
                </asp:LinkButton>
            </div>
            </td>
            <td style="width:100px"></td>
            </tr>
            
        </table>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="mdlBienvenido" runat="server" BackgroundCssClass="modalBackground"
            PopupControlID="pnlBienvenida" PopupDragHandleControlID="" 
            TargetControlID="LinkButton2"
            >
        </cc1:ModalPopupExtender>
        <asp:LinkButton ID="LinkButton2" runat="server"></asp:LinkButton>
</asp:Content>


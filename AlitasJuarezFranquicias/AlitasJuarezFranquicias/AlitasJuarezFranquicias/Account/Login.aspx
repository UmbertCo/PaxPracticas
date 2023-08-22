<%@ Page Theme="Alitas" Title="Iniciar Sesión" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <script src="../Scripts/progressbar.js" type="text/javascript"></script>
    <link href="../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/menu_style.css" rel="Stylesheet" type="text/css" />
    <link href="../App_Themes/Alitas/tema_dinamico.css" rel="Stylesheet" type="text/css" />
    <link href="../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Alitas.skin" />
    <style type="text/css">
        @font-face
        {
            font-family: Prozak;
            src: url("Prozak.eot") /* EOT file for IE */;  
        }
        @font-face
        {
            font-family: Prozak;
            src: url("Prozak.ttf") /* TTF file for CSS3 browsers */;
        }
    </style>
    <script runat="server">   
        public void Page_PreInit()
        {
            if (Session["theme"] == null)
            {
                this.Theme = "Alitas";
            }
            else
            {
                this.Theme = Convert.ToString(Session["theme"]);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:UpdatePanel ID="updLogin" runat="server">
        <ContentTemplate>
            <center>
            <table>
                <tr>
                <td style="width:780px"></td>
                    <td align="right"><asp:ImageButton ID="imgDescarga" runat="server" SkinID="imagenManual"
                        OnClick="imgDescarga_Click" /></td>
                </tr>
            </table>
                  <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbltxtiniciosesion" runat="server" ForeColor="White" Font-Bold="true" Font-Size="large" Font-Names="Arial" Text="<%$ Resources:resCorpusCFDIEs, lbltxtiniciosesion %>"></asp:Label></td>
                    </tr>
                  </table>
                <div>
                <center>
                    <table>
                        <tr>
                            <td>
                           <table align="left">
                                <tr>
                                <td style="width:340px"></td>
                                   <td> <img alt="" class="style18" style="width:220px; height:290px" src="../Imagenes/wingalesio_portada.png"/></td>
                                </tr>
                            </table>
                            </td>
                            <td style="width:220px"></td>
                            <td>
                                <asp:Panel ID="pnlogin" runat="server" Height="333px" SkinID="imagenLogin" 
                            Style="background-repeat: no-repeat; background-position:center" 
                            Width="483px">
                            <br />
                            <br />
                            <fieldset class="login" style="border: none;">
                            <center>
                            <table style="text-align:center;">
                                <tr>
                                    <td style="text-align:center;" class="TituloBlanco">
                                    <asp:Literal ID="ltIniciosesion" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblInicioSesion %>" />
                                    </td>
                                </tr>
                            </table>
                            </center>
                                <span class="TituloBlanco">
                                    <%--<asp:Literal ID="ltIniciosesion" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblInicioSesion %>" />--%>
                                </span>
                                <br />
                              
                                <p align="left">
                                    <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" 
                                        SkinID="labelLargeBlanco" Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
                                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUsuario"
                                        CssClass="failureNotification" ErrorMessage="El nombre de usuario es requerido."
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" ValidationGroup="LoginUserValidationGroup"
                                        Width="20px">*</asp:RequiredFieldValidator>
                                </p>
                                <p align="left">
                                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" 
                                        Height="26px" SkinID="labelLargeBlanco" 
                                        Text="<%$ Resources:resCorpusCFDIEs,lblContrasenia %>"></asp:Label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword"
                                        CssClass="failureNotification" ErrorMessage="La contraseña es requerida." ToolTip="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida  %>"
                                        ValidationGroup="LoginUserValidationGroup" Width="20px">*</asp:RequiredFieldValidator>
                                </p>
                            </fieldset>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td align="right" style="width:380px">
                                        
                                    </td>
                                    <td align="right">
                                    <asp:Button ID="btnLogin" runat="server" CommandName="Login" 
                                CssClass="botonEstilo" OnClick="btnLogin_Click" 
                                Text="<%$ Resources:resCorpusCFDIEs,lblEntrar %>" 
                                ValidationGroup="LoginUserValidationGroup" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                            </td>
                            <td style="width:540px"></td>
                        </tr>
                    </table>
                </center>
               
                        <%--<asp:Literal ID="ltInfoCuenta" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblInfoCuenta %>"></asp:Literal>--%>
                        <br />
                        <%--<asp:Panel ID="pnlogin" runat="server" Height="333px" SkinID="imagenLogin" 
                            Style="background-repeat: no-repeat; background-position:center" 
                            Width="483px">
                            <br />
                            <br />
                            <fieldset class="login" style="border: none;">
                                <span class="TituloBlanco">
                                    <asp:Literal ID="ltIniciosesion" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblInicioSesion %>" />
                                </span>
                                <br />
                              
                                <p align="left">
                                    <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" 
                                        SkinID="labelLargeBlanco" Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
                                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUsuario"
                                        CssClass="failureNotification" ErrorMessage="El nombre de usuario es requerido."
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" ValidationGroup="LoginUserValidationGroup"
                                        Width="20px">*</asp:RequiredFieldValidator>
                                </p>
                                <p align="left">
                                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" 
                                        Height="26px" SkinID="labelLargeBlanco" 
                                        Text="<%$ Resources:resCorpusCFDIEs,lblContrasenia %>"></asp:Label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword"
                                        CssClass="failureNotification" ErrorMessage="La contraseña es requerida." ToolTip="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida  %>"
                                        ValidationGroup="LoginUserValidationGroup" Width="20px">*</asp:RequiredFieldValidator>
                                </p>
                            </fieldset>
                        </asp:Panel>--%>
                </div>
                <table id="ControlesActivar">
                    <tr>
                        <td align="center" width="300">
                            <asp:HyperLink ID="hpkRecuperar" Font-Size="Medium" Font-Bold="false" CssClass="HiperLink" runat="server" ForeColor="White"
                                Text="<%$ Resources:resCorpusCFDIEs, hpkRecuperar %>" NavigateUrl="~/Cuenta/webInicioSesionRecupera.aspx"></asp:HyperLink>
                        </td>
                    </tr>
                </table>
                <center>
                    <asp:Label ID="lblBloqMsg" runat="server" Style="color: White" SkinID="labelMedium"
                        Text="<%$ Resources:resCorpusCFDIEs, varBloqMsg %>"></asp:Label>
                    <asp:LinkButton ID="lnkDesbloq" Font-Size="Medium" Font-Bold="false" runat="server" CssClass="HiperLink" ForeColor="White" Text="<%$ Resources:resCorpusCFDIEs, varBloqBtn %>"
                        OnClick="lnkDesbloq_Click"></asp:LinkButton>
                </center>
            </center>
            <cc1:ModalPopupExtender ID="modalGenerando" runat="server" BackgroundCssClass="modalBackground"
                PopupControlID="pnlGenerando" PopupDragHandleControlID="" TargetControlID="pnlGenerando">
            </cc1:ModalPopupExtender>
            <script type="text/javascript" language="javascript">
                var ModalProgress = '<%= modalGenerando.ClientID %>';
            </script>
            <asp:Panel ID="pnlGenerando" runat="server" Width="300px" CssClass="modal" BorderStyle="Solid"
                BorderWidth="1px">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varProcesando %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdateProgress ID="updGenera" runat="server">
                                <ProgressTemplate>
                                    <img alt="" src="../Imagenes/imgAjaxLoader.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="modalBlock" runat="server" BackgroundCssClass="modalBackground"
                PopupControlID="pnlBloqueo" PopupDragHandleControlID="" CancelControlID="btnCanBloq"
                TargetControlID="lnkBlock">
            </cc1:ModalPopupExtender>
            <asp:LinkButton ID="lnkBlock" runat="server" SkinID="LinkButtonSmall"></asp:LinkButton>
            <asp:Panel ID="pnlBloqueo" DefaultButton="btnBloqueo" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="modal">
                <center>
                    <table>
                        <tr>
                            <td class="TablaBackGround">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" >
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Panel ID="Panel1" runat="server" SkinID="imagenLoginDesbloquear" Style="background-repeat: no-repeat; background-position:bottom"
                                        Height="286px" Width="483px">
                                        <asp:Label ID="lblBloqCon" runat="server" class="leyendasfilsed" 
                                            Text="<%$ Resources:resCorpusCFDIEs, varBloqCon %>"></asp:Label>
                                        <br />
                                        <fieldset class="login" style="width: 434px; height: 7px; border: none;">
                                        <center>
                            <table style="text-align:center;" class="leyendasfilsed" >
                                <tr>
                                    <td style="text-align:center" class="leyendasfilsed" >
                                    <asp:Literal ID="lbldesbloquear_cuenta" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lbldesbloquear_cuenta %>" />
                                    </td>
                                </tr>
                            </table>
                            </center>
                                            <p align="left">
                                                <asp:Label ID="lblUserBloq" runat="server" 
                                                    AssociatedControlID="txtUserNameBloq"
                                                    Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>" ForeColor="Black" CssClass="leyendasfilsed"></asp:Label>
                                                <asp:TextBox ID="txtUserNameBloq" runat="server" CssClass="textEntry" 
                                                    MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvUsaurioBloq" runat="server" 
                                                    ControlToValidate="txtUserNameBloq" CssClass="failureNotification" 
                                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                                    ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                                    ValidationGroup="BloqGroup" Width="16px">*</asp:RequiredFieldValidator>
                                            </p>
                                            <p align="left">
                                                <asp:Label ID="lblContraseniaBloq" runat="server"  AssociatedControlID="txtPasswordBloq"
                                                    Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>" ForeColor="Black" 
                                                    CssClass="leyendasfilsed"></asp:Label>
                                                <asp:TextBox ID="txtPasswordBloq" runat="server" CssClass="passwordEntry" MaxLength="50"
                                                    TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvPassBloq" runat="server" ControlToValidate="txtPasswordBloq"
                                                    CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>"
                                                    ToolTip="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>" ValidationGroup="BloqGroup"
                                                    Width="16px">*</asp:RequiredFieldValidator>
                                                <br />
                                                <br />
                                                <br />
                                            </p>
                                        </fieldset>
                                    </asp:Panel>
                                    <p class="submitButton" style="width: 390px; height: 70px">
                                        <asp:Button ID="btnBloqueo" runat="server" CssClass="botonEstiloGrande" OnClick="btnBloqueo_Click"
                                            Text="<%$ Resources:resCorpusCFDIEs, varBloqBtn %>" ValidationGroup="BloqGroup" />
                                        <asp:Button ID="btnCanBloq" runat="server" CssClass="botonEstilo" OnClick="btnCanBloq_Click"
                                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" />
                                    </p>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <%-- Modalpoup Avisos --%>
            <asp:LinkButton ID="lkbAviso" runat="server" SkinID="LinkButtonSmall"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="mpeAvisos" runat="server" TargetControlID="lkbAviso"
                PopupControlID="pnlAvisos" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlAvisos" runat="server" CssClass="modal">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="TablaBackGround">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <img alt="" class="imgInformacion" src="../Imagenes/Informacion.png" />
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblAviso" runat="server" SkinID="labelLarge"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAviso" runat="server" CssClass="botonEstilo" Text="OK" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <%--ModalPoup ErrorLog--%>
            <asp:LinkButton ID="lkbErrorLog" runat="server" SkinID="LinkButtonSmall"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="mpeErrorLog" runat="server" TargetControlID="lkbErrorLog"
                PopupControlID="pnlErrorLog" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlErrorLog" runat="server" CssClass="modal">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="TablaBackGround">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <img alt="" class="imgInformacion" src="../Imagenes/Informacion.png" />
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblErrorLog" runat="server" SkinID="labelLarge"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnErrorLog" runat="server" CssClass="botonEstilo" Text="OK" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webInicioSesionRegistro.aspx.cs" Inherits="InicioSesion_webInicioSesionRegistro" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 16px;
            height: 16px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript" language="javascript">

        function clickOnce(btn, msg) {
            // Comprobamos si se está haciendo una validación
            if (typeof (Page_ClientValidate) == 'function') {
                // Si se está haciendo una validación, volver si ésta da resultado false
                if (Page_ClientValidate() == false) { return false; }
            }

            // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
            if (btn.getAttribute('type') == 'button') {
                // El atributo msg es totalmente opcional. 
                // Será el texto que muestre el botón mientras esté deshabilitado
                if (!msg || (msg = 'undefined')) { msg = 'Loading...'; }

                btn.value = msg;

                // La magia verdadera :D
                btn.disabled = true;
            }

            return true;
        }

    </script>
    <center>
        
        <p>
            <asp:Label ID="lblDatos" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatos %>"></asp:Label>
        </p>
        <div>
            <fieldset class="register" style="width: 420px">
            <asp:Panel ID="pnlRegistroSesion" runat="server" 
                    BackImageUrl="~/Imagenes/login_sesion_registro.png" Width="388px" 
                    Height="619px">
                <table>
                    <tr>
                        <td>
                            <h2>
                                <asp:Label ID="lblCreaCuenta" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCreaCuenta %>"></asp:Label>
                            </h2>
                        </td>
                    </tr>
                    <caption>
                        <br />
                        <br />
                        <br />
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNombreCompleto" runat="server" 
                                    AssociatedControlID="txtNombre" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="textEntry" MaxLength="255" 
                                    TabIndex="1"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="UserNameRequired0" runat="server" 
                                    ControlToValidate="txtNombre" CssClass="failureNotification" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
                            </td>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry" MaxLength="50" 
                                        TabIndex="2"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                        ControlToValidate="txtUsuario" CssClass="failureNotification" Display="Dynamic" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                        ValidationGroup="RegisterUserValidationGroup"><img src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:RegularExpressionValidator ID="regxNueva" runat="server" 
                                        ControlToValidate="txtUsuario" CssClass="failureNotification" Display="Dynamic" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, txtUsuario %>" 
                                        ValidationExpression="(?=^.{8,}$).*$" 
                                        ValidationGroup="RegisterUserValidationGroup"><img src="../Imagenes/error_sign.gif" /> 
                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCorreo" runat="server" AssociatedControlID="txtCorreo" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="textEntry" MaxLength="50" 
                                        TabIndex="3"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                        ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                        ValidationGroup="RegisterUserValidationGroup"><img src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                        ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                        ValidationGroup="RegisterUserValidationGroup" Width="131px"><img src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </tr>
                    </caption>
                </table>
                <asp:UpdatePanel ID="udpCrear" runat="server">
                    <ContentTemplate>
                        <p align="left">
                            <span class="Apple-style-span" style="border-collapse: separate; color: rgb(0, 0, 0);
                                font-family: Century Gothic; font-size: 13px; font-style: normal;
                                font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal;
                                orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none;
                                white-space: normal; widows: 2; word-spacing: 0px; -webkit-border-horizontal-spacing: 0px;
                                -webkit-border-vertical-spacing: 0px; -webkit-text-decorations-in-effect: none;
                                -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px;"><span class="Apple-style-span"
                                    style="font-size: 12px; font-weight: bold; text-align: left; -webkit-border-horizontal-spacing: 2px;
                                    -webkit-border-vertical-spacing: 2px;"></span>
                                <asp:Label ID="lblProcesoRegistro" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblProcesoRegistro %>"></asp:Label>
                            </span>
                        </p>
                        <br />
                        <p align="left">
                        <br />
                        <br />
                        <br />
                            <asp:CheckBox ID="chkAceptar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDeclaro %>"
                                TextAlign="Left" TabIndex="4" AutoPostBack="True" 
                                OnCheckedChanged="chkAceptar_CheckedChanged" ForeColor="Black" />
                            <asp:HyperLink ID="hpkSoporte" runat="server" NavigateUrl="~/webGlobalTerminos.aspx"
                                Target="_blank" TabIndex="5" Text="<%$ Resources:resCorpusCFDIEs, hlkTerminos %>"></asp:HyperLink>
                            <p align="center">
                                
                                <br />
                                <table>
                                    <caption>
                                        <br />
                                        <br />
                                        <br />
                                        <tr>
                                            <td>
                                            Captcha:
                                            </td>
                                            <td>
                                                <asp:Image ID="ImageCaptcha" runat="server" 
                                                    AlternateText="If you can't read this number refresh your screen" 
                                                    ImageUrl="~/captcha.ashx" />
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="bntRecarga" runat="server" Height="16px" 
                                                    ImageUrl="~/Imagenes/reload_captcha.png" OnClick="bntRecarga_Click" 
                                                    TabIndex="6" Width="16px" />
                                            </td>
                                        </tr>
                                    </caption>
                                </table>
                                <br />
                                <asp:TextBox ID="txtNumero" runat="server" EnableViewState="False" MaxLength="5"
                                    TabIndex="7" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="vrfNumero" runat="server" ControlToValidate="txtNumero"
                                    CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNumero %>"
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, txtNumero %>" ValidationGroup="RegisterUserValidationGroup"
                                    Width="16px">*</asp:RequiredFieldValidator>
                                <br />
                            </p>
                            <asp:UpdateProgress ID="updProgress" runat="server">
                                <ProgressTemplate>
                                    <img alt="" class="style1" src="../Imagenes/imgAjaxLoader.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <p>
                            </p>
                            <p>
                            </p>
                            <p>
                            </p>
                            <p>
                            </p>
                            <p>
                            </p>
                            <p>
                            </p>
                            <p>
                            </p>
                        </p>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="bntRecarga" /> 
                    </Triggers>
                </asp:UpdatePanel>
                </asp:Panel>
            </fieldset>
            <table>
                <tr>
                    <td width="300">
                    </td>
                    <td>
                        <asp:UpdatePanel ID="udpBtnCrear" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnCrearCuenta" runat="server" CommandName="MoveNext" Enabled="False"
                                    OnClick="btnCrearCuenta_Click" TabIndex="8" Text="<%$ Resources:resCorpusCFDIEs, btnCrearCuenta %>"
                                    ValidationGroup="RegisterUserValidationGroup" CssClass="botonGrande" Width="168px"
                                    UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Procesando')" 
                                    Height="29px" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </center>
</asp:Content>


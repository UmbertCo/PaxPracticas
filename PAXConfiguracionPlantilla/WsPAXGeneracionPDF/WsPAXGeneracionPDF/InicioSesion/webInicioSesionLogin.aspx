<%@ Page Title="Inicio de Sesión" Language="C#" MasterPageFile="~/webGlobalMaster.master" AutoEventWireup="true"
    CodeFile="webInicioSesionLogin.aspx.cs" Inherits="Account_Login"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style3
        {
            width: 116px;
            height: 105px;
        }
        .modal
        {
            padding: 10px 10px 10px 10px;
            border:1px solid #333333;
            background-color:White;
        }
        .style4
        {
            height: 21px;
        }
        .style5
        {
            width: 200px;
            height: 21px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />

    <center>
    <h2 align="right" >

                    <asp:ImageButton ID="imgDescarga" runat="server" 
                        ImageUrl="~/Imagenes/DescMUsuario.png" 
            onclick="imgDescarga_Click" />

    </h2>
        <h2 >
        <asp:Label ID="lblIncioSesion" runat="server"></asp:Label>
    </h2>
    <p>
        <span class="Apple-style-span" 
            
            style="border-collapse: separate; color: rgb(0, 0, 0); font-family: verdana; font-size: 12px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-border-horizontal-spacing: 0px; -webkit-border-vertical-spacing: 0px; -webkit-text-decorations-in-effect: none; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; " 
            dir="ltr">
        <asp:Label ID="lblBienvenida" runat="server" 
            style="font-family: 'Century Gothic'"></asp:Label>
        </span>
    </p>
    <p>
        <span class="Apple-style-span" 
            style="border-collapse: separate; color: rgb(0, 0, 0); font-family: verdana; font-size: 12px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-border-horizontal-spacing: 0px; -webkit-border-vertical-spacing: 0px; -webkit-text-decorations-in-effect: none; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; ">
        <span class="Apple-style-span" 
            style="color: rgb(100, 100, 100); font-family: Arial, Helvetica, sans-serif; text-align: left; ">
        <asp:Label ID="lblProveedor" runat="server" 
            style="font-family: 'Century Gothic'"></asp:Label>
        </span></span>
    </p>
        <p>
                    &nbsp;<asp:UpdateProgress runat="server" 
            AssociatedUpdatePanelID="updLogin" ID="updProgress"><ProgressTemplate>
                                <img alt="" 
                            src="../Imagenes/imgAjaxLoader.gif" />
                            
</ProgressTemplate>
</asp:UpdateProgress>

    </p>
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
                    <tr>
                        <td>
                    <table id="ControlesRegistro">
                        <tr>
                            <td align="left" width="300">
                                <%--<asp:Label ID="lblRegistrate" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblRegistrate %>" 
                                    style="font-family: 'Century Gothic'"></asp:Label>--%>
                             </td>
                            <td style="width: 200px" align="left">
                               <%-- <asp:HyperLink ID="hpkRegistrar" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblRegistrar %>" 
                                    NavigateUrl="~/InicioSesion/webInicioSesionRegistro.aspx"></asp:HyperLink>--%>
                            </td>
                        </tr>

                    </table>
                   

                            <br />
                   

                            <br />
                   

                     <table id="ControlesActivar">
                        <tr>
                            <td align="left" width="300" class="style4">
                               <%-- <asp:HyperLink ID="hpkRecuperar" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, hpkRecuperar %>" 
                                    NavigateUrl="~/InicioSesion/webInicioSesionRecupera.aspx"></asp:HyperLink>--%>
                            </td>
                            <td align="left" class="style5">
                               <%-- <asp:HyperLink ID="hpkReactivar" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, hpkReactivar %>" 
                                    NavigateUrl="~/InicioSesion/webInicioSesionReactivar.aspx"></asp:HyperLink>--%>
                            </td>
                        </tr>
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
                            <img alt="SAT" class="style3" 
                                src="../Imagenes/proveedor-autorizado-de-certificación.jpg" /></td>
                    </tr>
                    </table>
            </td>
        </tr>
    </table>
        <br />
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="lblBloqMsg" runat="server" 
                        style="font-size: x-small; color: #990000" 
                        Text="<%$ Resources:resCorpusCFDIEs, varBloqMsg %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:LinkButton ID="lnkDesbloq" runat="server" onclick="lnkDesbloq_Click" 
                        style="font-size: x-small" Text="<%$ Resources:resCorpusCFDIEs, varBloqBtn %>"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="pnlBloqueo" runat="server" BorderStyle="Solid" BorderWidth="1px" 
            CssClass="modal" Height="150px" Width="500px">
            <table>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblBloqCon" runat="server" style="font-size: small" 
                            Text="<%$ Resources:resCorpusCFDIEs, varBloqCon %>"></asp:Label>
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
                                        <asp:Label ID="lblUserBloq" runat="server" 
                                            AssociatedControlID="txtUserNameBloq" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtUserNameBloq" runat="server" CssClass="textEntry" 
                                            MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td>
                                        <br />
                                        <asp:RequiredFieldValidator ID="rfvUsaurioBloq" runat="server" 
                                            ControlToValidate="txtUserNameBloq" CssClass="failureNotification" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                            ValidationGroup="BloqGroup" Width="16px">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30px">
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblContraseniaBloq" runat="server" 
                                            AssociatedControlID="txtPasswordBloq" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtPasswordBloq" runat="server" CssClass="passwordEntry" 
                                            MaxLength="50" TextMode="Password"></asp:TextBox>
                                    </td>
                                    <td>
                                        <br />
                                        <asp:RequiredFieldValidator ID="rfvPassBloq" runat="server" 
                                            ControlToValidate="txtPasswordBloq" CssClass="failureNotification" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>" 
                                            ValidationGroup="BloqGroup" Width="16px">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnBloqueo" runat="server" CssClass="botonEstilo" 
                                            onclick="btnBloqueo_Click" Text="<%$ Resources:resCorpusCFDIEs, varBloqBtn %>" 
                                            ValidationGroup="BloqGroup" />
                                        <asp:Button ID="btnCanBloq" runat="server" CssClass="botonEstilo" 
                                            onclick="btnCanBloq_Click" 
                                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" />
                                    </td>
                                    <td>
                                        &nbsp;</td>
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
        <cc1:modalpopupextender id="modalBlock" runat="server" backgroundcssclass="modalBackground" 
        popupcontrolid="pnlBloqueo" popupdraghandlecontrolid="" CancelControlID="btnCanBloq"
        targetcontrolid="lnkBlock">
        </cc1:modalpopupextender>

        <asp:LinkButton ID="lnkBlock" runat="server"></asp:LinkButton>
    </center>
</asp:Content>
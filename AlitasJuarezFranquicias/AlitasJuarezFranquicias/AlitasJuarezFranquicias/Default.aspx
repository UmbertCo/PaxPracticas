<%@ Page Language="C#" AutoEventWireup="true" Theme="Alitas" CodeFile="Default.aspx.cs"
    Inherits="Default2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:. ALITAS .:</title>
    <link href="Styles/menu_style.css" rel="Stylesheet" type="text/css" />
    <link href="Styles/menu_default.css" rel="Stylesheet" type="text/css" />
    <link href="Styles/webGlobalStyle.css" rel="Stylesheet" type="text/css" />
    <link href="Styles/StyleSheet.css" rel="Stylesheet" type="text/css" />
    <link href="../App_Themes/Alitas/tema_dinamico.css" rel="Stylesheet" type="text/css" />

    <style type="text/css">
        .fontsH1
        {
            font-weight: 700;
            color: #000000;
            font-size: medium;
        }
        
        #footer
        {
            width: 100%;
            position: absolute;
            bottom: 0;
        }
        .both
        {
            background: #e61c18 url('Imagenes/pa.png') left top;
            height: 690px;
        }
        .main
        {
            border-radius: 10px;
            background-color: White;
            padding: 0px 12px;
            margin: 12px 8px 8px 8px;
            min-height: 420px;
            border: 1px solid #496077;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            -webkit-border-top-right-radius: 10px;
            -webkit-border-bottom-right-radius: 10px;
            width: 1000px;
        }
    </style>
    <style type="text/css">
        html, body
        {
            height: 100%;
            margin: 0;
            padding: 0;
        }
        #page-background
        {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
        }
        #content
        {
            position: relative;
            z-index: 1;
            padding: 10px;
        }
    </style>
    <style type="text/css">
        @font-face
        {
            font-family: Prozak;
            src: url("Styles/Prozak.eot"); /* EOT file for IE */
            font-weight: lighter;
        }
        @font-face
        {
            font-family: Prozak;
            src: url("Styles/Prozak.ttf") /* TTF file for CSS3 browsers */;
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
            // Sets the Theme for the page.
        }
    </script>
</head>
<body class="fondoMaster">
    <style type="text/css">
        @font-face
        {
            font-family: Prozak;
            src: url("Styles/Prozak.eot") /* EOT file for IE */;
        }
        @font-face
        {
            font-family: Prozak;
            src: url("Styles/Prozak.ttf") /* TTF file for CSS3 browsers */;
        }
        .style15
        {
            width: 801px;
        }
        .style16
        {
            width: 151px;
        }
        .style17
        {
            width: 160px;
        }
        .botonEstilo
        {}
        .style18
        {
            width: 192px;
            height: 156px;
        }

        .style19
        {
            width: 16px;
        }

    </style>
    <div style="background-position: left;">
        <form id="form1" runat="server">
        <script language="javascript" type="text/javascript">

            var t;
            window.onload = resetTimer;
            document.onmousemove = resetTimer;
            document.onkeypress = resetTimer;

            //        function RefreshPage() {
            //            location.reload(true)
            //        }

            function logout() {
                /*alert("A pasado Tiempo sin actividad")
                location.reload(true)*/
                location.href = "Default.aspx";
            }

            function resetTimer() {
                clearTimeout(t);
                t = setTimeout("logout()", 300000)// 1 segundo = 1000 ml //location.reload(true)
            }

            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginReq);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);


            function beginReq(sender, args) {
                // muestra el popup 
                $find(ModalProgress).show();

            }

            function endReq(sender, args) {
                //  esconde el popup 
                $find(ModalProgress).hide();
            }

        </script>
        <asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="True"
            EnableScriptLocalization="True">
        </asp:ScriptManager>
        <center>
            <table>
                <tr> 
                    <td>
                        <table align="center">
                            <tr>
                                <td colspan="4" class="TopGrafo">
                                    <div align="center" style="height: 197px">
                                        <br />
                                        <table width="100%" cellpadding="0" cellspacing="0" style="height: 132px">
                                            <tr>
                                                <td class="style15">
                                                <table >
                                                    <tr>
                                                    <td style="width:30px"></td>
                                                        <td valign="top" align="center"><img alt="" class="style18" src="Imagenes/alitas-logo.png" /></td>
                                                        <td style="width:620px"></td>
                                                    </tr>
                                                </table>
                                                </td>
                                                <td align="right">
                                                    <div>
                                                        <table align="center">
                                                            <tr>
                                                                <td align="center" class="style16">
                                                                    <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="True" ForeColor="White" Font-Size="Medium" Font-Names="Arial"
                                                                        OnClick="lnkSalir_Click" Text="<%$ Resources:resCorpusCFDIEs, lblInicioSesion %>"
                                                                        Width="144px" Height="20px"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <div class="botonGris" align="center" style="width: 138px; height: 24px;">
                                                                        <asp:LinkButton ID="LinkButton4" runat="server" Font-Bold="True" ForeColor="White"
                                                                            OnClick="lnkEnglish_Click" OnPreRender="lnkEnglish_PreRender" SkinID="LinkButtonSmallBlanco">English</asp:LinkButton>
                                                                        <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="White"
                                                                            Text="|" Style="font-size: x-small"></asp:Label>
                                                                        <asp:LinkButton ID="LinkButton5" runat="server" Font-Bold="True" ForeColor="White"
                                                                            OnClick="lnkEspañol_Click" OnPreRender="lnkEspañol_PreRender" SkinID="LinkButtonSmallBlanco">Español
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                            <td valign="top" align="center">
                                <table>
                                    <tr>
                                    <td class="TablaBackGround">
                                    <asp:Label ID="lblMenu" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblMenu %>"
                                        CssClass="TextoTituloEtiqueta" Font-Names="Arial" ForeColor="White" Font-Size="Medium" Visible="false"></asp:Label></td>        
                                    </tr>
                                    <tr>
                                    <td valign="top" style="width:220px; height:290px"><img alt="" class="style18" style="width:220px; height:290px" src="Imagenes/wingalesio_portada.png" /> </td>
                                        <td>
                                        <asp:Menu ID="mnuTiendas" runat="server" CssClass="NavigationMenu" Orientation="Vertical"
                                        DynamicPopOutImageUrl="~/mnu_images/right-arrow.gif" StaticPopOutImageUrl="~/mnu_images/drop-arrow.gif"
                                        RenderingMode="Table" DynamicVerticalOffset="6" OnMenuItemClick="mnuTiendas_MenuItemClick" Visible="false">
                                        <StaticMenuItemStyle CssClass="staticMenuItemStyle" ItemSpacing="3px" />
                                        <StaticHoverStyle CssClass="staticHoverStyle" />
                                        <DynamicMenuItemStyle CssClass="dynamicMenuItemStyle" />
                                        <DynamicHoverStyle CssClass="menuItemMouseOver" />
                                        <DynamicMenuStyle CssClass="menuItem" />
                                        <DynamicSelectedStyle CssClass="menuItemSelected" />
                                        <Items>
                                        </Items>
                                    </asp:Menu>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                                        <td>
                                            <table align="center" style="width: 730px; margin-left: 0px">
                                                <tr>
                                                        <td align="center" colspan="2" valign="top" colspan="0">
                                    <asp:Panel ID="Panel1" CssClass="cuadro" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <span class="TituloBlanco">
                                                    <br />
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblInstruccionDefault %>"></asp:Label>
                                                </span>
                                                <br />
                                                <br />
                                                <table>
                                                    <tr>
                                                    <td></td>
                                                        <td align="right">
                                                            <asp:Label ID="Label8" runat="server" CssClass="EtiquetaDefault" Style="font-weight: 700"
                                                                Text="<%$ Resources:resCorpusCFDIEs, lblTicket %>"></asp:Label>
                                                        </td>
                                                        <td width="100px">
                                                            <asp:TextBox ID="txtTicket" runat="server" MaxLength="5"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label9" runat="server" Text="(5 dígitos)" Font-Names="Arial"
                                                                Font-Size="X-Small" ForeColor="White"></asp:Label></td>
                                                                <td >
                                                                <table>
                                                                    <tr>
                                                                        <td class="style19">
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTicket"
                                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="Campo requerido."
                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblCampoObligatorio %>" 
                                                                                ValidationGroup="ValidationGroup" Width="16px"><img src="Imagenes/error_sign.gif"/></asp:RequiredFieldValidator>        
                                                                        </td>
                                                                        <td>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTicket"
                                                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>"
                                                                            ValidationExpression="^\d{5}$" ValidationGroup="ValidationGroup"><img src="Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>    
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                    <td></td>
                                                        <td align="right">
                                                            <asp:Label ID="Label10" runat="server" CssClass="EtiquetaDefault"  Style="font-weight: 700"
                                                                Text="<%$ Resources:resCorpusCFDIEs, lblNoSucursal %>"></asp:Label>:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTienda" runat="server" MaxLength="2"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label11" runat="server" Text="(2 dígitos)" Font-Names="Arial"
                                                                Font-Size="X-Small" ForeColor="White"></asp:Label>
                                                                <td >
                                                                <table>
                                                                    <tr>
                                                                        <td class="style19">
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTienda"
                                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="Campo requerido."
                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblCampoObligatorio %>" 
                                                                                ValidationGroup="ValidationGroup" Width="16px"><img src="Imagenes/error_sign.gif"/></asp:RequiredFieldValidator>        
                                                                        </td>
                                                                        <td>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtTienda"
                                                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>"
                                                                            ValidationExpression="^\d{2}$" ValidationGroup="ValidationGroup"><img src="Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>    
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                </td>
                                                                <td></td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    <td></td>
                                                        <td align="right">
                                                            <asp:Label ID="Label12" runat="server" CssClass="EtiquetaDefault" Style="font-weight: 700"
                                                                Text="<%$ Resources:resCorpusCFDIEs, lblTotal %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTotal" runat="server" MaxLength="20"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label13" runat="server" Text="(00.00)" Font-Names="Arial"
                                                                Font-Size="X-Small" ForeColor="White"></asp:Label>
                                                                <td class="style19">
                                                                <table>
                                                                    <tr>
                                                                        <td class="style19">
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtTotal"
                                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="Campo requerido."
                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblCampoObligatorio %>" 
                                                                                ValidationGroup="ValidationGroup" Width="16px"><img src="Imagenes/error_sign.gif"/></asp:RequiredFieldValidator>        
                                                                        </td>
                                                                        <td>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtTotal"
                                                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>"
                                                                            ValidationExpression="^[+]?[0-9]+([.][0-9]{1,2})?$" ValidationGroup="ValidationGroup"><img src="Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>    
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                </td>
                                                                <td></td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    <td></td>
                                                        <td align="right">
                                                            <asp:Label ID="Label14" runat="server" CssClass="EtiquetaDefault" Style="font-weight: 700"
                                                                Text="<%$ Resources:resCorpusCFDIEs, lblFecha %>"></asp:Label>:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFecha" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label15" runat="server" Text="(dd/mm/yyyy)" Font-Names="Arial"
                                                                Font-Size="X-Small" ForeColor="White"></asp:Label>
                                                                <td class="style19">
                                                                    <table>
                                                                        <tr>
                                                                            <td class="style19">
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtFecha"
                                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="Campo requerido."
                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblCampoObligatorio %>" ValidationGroup="ValidationGroup">
                                                    <img src="Imagenes/error_sign.gif" />
                                                            </asp:RequiredFieldValidator>                
                                                                            </td>
                                                                            <td>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator69" runat="server" Display="Dynamic"
                                                                ControlToValidate="txtFecha" CssClass="failureNotification" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d+"
                                                                ValidationGroup="ValidationGroup" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>">
                                                    <img src="Imagenes/error_sign.gif" />
                                                            </asp:RegularExpressionValidator>                
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td></td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                        <td align="right">
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <table align="right">
                                                            <tr>
                                                                <td align="right">
                                                                <asp:Button ID="Button1" runat="server" CssClass="botonEstilo" Height="32px" 
                                                                    OnClick="btnBuscar_Click" Text="<%$ Resources:resCorpusCFDIEs, btnBuscar %>" 
                                                                    ValidationGroup="ValidationGroup" Width="96px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        </td>
                                                        <td class="style19"></td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </td>
                                                </tr>
                                            </table>
                                        </td>
                            </tr>
                             <tr>
                                <td>
                                    &nbsp;
                                </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="center">
                                    <br />
                                    <asp:ImageMap ID="imgTicket" runat="server" Visible="false" >
                                    </asp:ImageMap>
                                        <img alt="" src="Imagenes/TicketAlitas1.jpg" /></td>
                                    <td></td>
                                </tr>
                                <td align="center" colspan="2">
                                <br />
                                <br />
                                    <%--<asp:ImageMap ID="imgTicket" runat="server">
                                    </asp:ImageMap>--%>
                                    <br />
                                    <br />
                                    <div style="font-family: Arial; color: White; line-height: 10px; font-size: x-small;
                                        font-weight: bold; font-style: normal; font-variant: normal; letter-spacing: normal;
                                        orphans: 2; text-indent: 0px; text-transform: none; white-space: normal; windows: 2;
                                        word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px;
                                        background-color: Transparent">
                                        <asp:Literal ID="lblResolucionRecomendada" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblResolucionRecomendada %>"/>
                                        <br />
                                        <br />
                                        <asp:Literal ID="ltlLeyendaCancelacion" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblleyendaCancelacion%>" />
                                    </div>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center" class="BotomGrafo">
                                    <div style="height: 197px">
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <center>
                                        <%--<img id="Img1" runat="server" src="~/Imagenes/reservados.png" />--%>
                                        </center>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>&nbsp;</td>
                    <td valign="middle" align="right" style="background-color: Transparent">
                        <asp:ImageMap ID="imgpublic" alt="" runat="server" Height="600px" Width="160px" />
                    </td>
                </tr>
            </table>
            <br />
        </center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:LinkButton ID="lkbConsultaCFDI" runat="server" SkinID="LinkButtonSmall"></asp:LinkButton>
                <cc1:ModalPopupExtender ID="mpeConsultaCFDI" runat="server" TargetControlID="lkbConsultaCFDI"
                    PopupControlID="pnlConsultaCFDI" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlConsultaCFDI" runat="server" CssClass="modal" BorderStyle="Solid"
                    BorderWidth="1px" Width="606px">
                    <table>
                        <tr>
                            <td width="150">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblComprobantes" runat="server" SkinID="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblComprobantes %>"></asp:Label>
                                <br />
                                <hr style="width: 440px;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <div align="center">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:HyperLink ID="hpPDF" runat="server" Target="_blank">
                                                    <asp:Image ID="imgPDF" runat="server" ImageUrl="~/Imagenes/logo_pdf.png" Height="44px"
                                                        Width="44px" BorderWidth="0" /></asp:HyperLink>
                                            </td>
                                            <td>
                                                <asp:HyperLink ID="hpXML" runat="server" Target="_blank">
                                                    <asp:Image ID="imgXML" runat="server" ImageUrl="~/Imagenes/xml_mediano.png" Height="48px"
                                                        Width="48" BorderWidth="0" /></asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblLeyCorreo" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btnConsultaCDFI" runat="server" CssClass="botonEstilo" Text="OK"
                                    OnClick="btnConsultaCDFI_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:LinkButton ID="lkbAviso" runat="server" SkinID="LinkButtonSmall"></asp:LinkButton>
                <cc1:ModalPopupExtender ID="mpeAvisos" runat="server" TargetControlID="lkbAviso"
                    PopupControlID="pnlAvisos" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <br />
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
                                            <img alt="" class="imgInformacion" src="Imagenes/info_ico.png" />
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblAviso" runat="server" SkinID="labelSmall"></asp:Label>
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
                                            <asp:Button ID="btnAviso" runat="server" CssClass="botonEstilo" Text="OK" OnClick="btnAviso_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <asp:LinkButton ID="lkbErrorLog" runat="server" SkinID="LinkButtonSmall"></asp:LinkButton>
                <cc1:ModalPopupExtender ID="mpeErrorLog" runat="server" TargetControlID="lkbErrorLog"
                    PopupControlID="pnlErrorLog" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <br />
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
                                            <img alt="" class="imgInformacion" src="Imagenes/info_ico.png" />
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblErrorLog" runat="server" SkinID="labelSmall"></asp:Label>
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
                <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
                <%--modal de formulario--%>
                <asp:HiddenField ID="hdIdRfc" runat="server" Visible="false" />
                <asp:LinkButton ID="lkbDatosReceptor" runat="server" SkinID="LinkButtonSmall"></asp:LinkButton>
                <cc1:ModalPopupExtender ID="mpeDatosRecetor" runat="server" TargetControlID="lkbDatosReceptor"
                    PopupControlID="pnlFormulario" BackgroundCssClass="modalBackground" OnLoad="mpeDatosRecetor_Load">
                </cc1:ModalPopupExtender>
                <br />
                <%--<asp:UpdatePanel runat ="server">
       <ContentTemplate>--%>
                <fieldset id="pnlFormulario" class="register">
                    <legend class="leyendasfilsed" style="color: #FFFFFF">
                        <asp:Literal ID="ltDatosReceptor" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblConReceptor %>"></asp:Literal>
                    </legend>
                    <asp:Panel ID="pnlFormulario2" runat="server" CssClass="modal">
                        <%--GroupingText="<%$ Resources:resCorpusCFDIEs, lblConReceptor %>" --%>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRfcReceptor" runat="server" AssociatedControlID="txtRfcReceptor"
                                        SkinID="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblRfc %>"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblNoExterior" runat="server" AssociatedControlID="txtNoExterior"
                                        SkinID="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtRfcReceptor" runat="server" CssClass="fontText" MaxLength="15"
                                        TabIndex="1" Width="300px" OnTextChanged="txtRfcReceptor_TextChanged"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:ImageButton ID="imgbtnLupa" runat="server" AlternateText="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaReceptor %>"
                                        CssClass="imagenModal" ImageUrl="Imagenes/lupa.png" ToolTip="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaReceptor %>"
                                        OnClick="imgbtnLupa_Click" TabIndex="2" ValidationGroup="rfcValidationGroup"
                                        Height="17px" Width="16px" />
                                    <asp:RegularExpressionValidator ID="regxRFC" runat="server" ControlToValidate="txtRfcReceptor"
                                        CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valRfcCadena %>"
                                        ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"
                                        ValidationGroup="RegisterUserValidationGroup"><img 
                            src="Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfvRFCReceptor" runat="server" ControlToValidate="txtRfcReceptor"
                                        CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valRfc %>"
                                        ValidationGroup="RegisterUserValidationGroup" Width="16px" Display="Dynamic">
                            <img src="Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvrfcLupa" runat="server" ControlToValidate="txtRfcReceptor"
                                        CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valRfc %>"
                                        ValidationGroup="rfcValidationGroup" Width="16px" Display="Dynamic">
                            <img src="Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoExterior" runat="server" CssClass="fontText" MaxLength="10"
                                        TabIndex="9" Width="300px"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRazonReceptor" runat="server" AssociatedControlID="txtRazonReceptor"
                                        SkinID="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblNoInterior" runat="server" AssociatedControlID="txtNoInterior"
                                        SkinID="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtRazonReceptor" runat="server" CssClass="fontText" MaxLength="200"
                                        TabIndex="3" Width="300"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvRazonReceptor" runat="server" ControlToValidate="txtRazonReceptor"
                                        CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valRazonSocial %>"
                                        ValidationGroup="RegisterUserValidationGroup" Width="16px">
                            <img src="Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoInterior" runat="server" CssClass="fontText" MaxLength="10"
                                        TabIndex="10" Width="300px"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPais" runat="server" AssociatedControlID="txtPais" SkinID="labelLarge"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblColonia" runat="server" AssociatedControlID="txtColonia" SkinID="labelLarge"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtPais" runat="server" CssClass="fontText" MaxLength="255" TabIndex="4"
                                        Width="300px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvPais" runat="server" ControlToValidate="txtPais"
                                        CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valPais %>"
                                        ValidationGroup="RegisterUserValidationGroup" Width="16px">
                            <img src="Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtColonia" runat="server" CssClass="fontText" MaxLength="255" TabIndex="11"
                                        Width="300px"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblEstado" runat="server" AssociatedControlID="txtEstado" SkinID="labelLarge"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="LablblCodigoPostalel5" runat="server" AssociatedControlID="txtCodigoPostal"
                                        SkinID="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtEstado" runat="server" CssClass="fontText" MaxLength="255" TabIndex="5"
                                        Width="300px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvEstado" runat="server" ControlToValidate="txtEstado"
                                        CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valEstado %>"
                                        ValidationGroup="RegisterUserValidationGroup" Width="16px">
                            <img src="Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="fontText" MaxLength="5"
                                        TabIndex="12" Width="300px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                        TargetControlID="txtCodigoPostal">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvCodigoPostal" runat="server" ControlToValidate="txtCodigoPostal"
                                        CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs,valCodigoPostal %>"
                                        ValidationGroup="RegisterUserValidationGroup" Width="16px">
                            <img src="Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMunicipio" runat="server" AssociatedControlID="txtMunicipio" SkinID="labelLarge"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblTelefono" runat="server" AssociatedControlID="txtTelefono" SkinID="labelLarge"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblTelefono %>"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtMunicipio" runat="server" CssClass="fontText" MaxLength="255"
                                        TabIndex="6" Width="300px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvMunicipio" runat="server" ControlToValidate="txtMunicipio"
                                        CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valMunicipio %>"
                                        ValidationGroup="RegisterUserValidationGroup" Width="16px">
                            <img src="Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="fontText" MaxLength="255"
                                        TabIndex="13" Width="300px"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblLocalidad" runat="server" AssociatedControlID="txtLocalidad" SkinID="labelLarge"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblCorreo" runat="server" AssociatedControlID="txtCorreo" SkinID="labelLarge"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"></asp:Label>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtLocalidad" runat="server" CssClass="fontText" MaxLength="255"
                                        TabIndex="7" Width="300px"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="fontText" MaxLength="255" TabIndex="14"
                                        Width="300px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RegularExpressionValidator ID="revCorreo" runat="server" ControlToValidate="txtCorreo"
                                        CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Width="16px"><img src="Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCalle" runat="server" AssociatedControlID="txtCalle" SkinID="labelLarge"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCalle" runat="server" CssClass="fontText" MaxLength="255" TabIndex="8"
                                        Width="300px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvCalle" runat="server" ControlToValidate="txtCalle"
                                        CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>"
                                        ValidationGroup="RegisterUserValidationGroup" Width="16px">
                            <img src="Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnAcepDatos" runat="server" CssClass="botonEstilo" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" ValidationGroup="RegisterUserValidationGroup"
                                        OnClick="btnAcepDatos_Click" TabIndex="13" />
                                    <asp:Button ID="btnCancelarDatos" runat="server" CssClass="botonEstilo" 
                                        Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" OnClick="btnCancelarDatos_Click"
                                        TabIndex="14" />
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
                <script type="text/javascript" language="javascript">
                    var ModalProgress = '<%= modalGenerando.ClientID %>';
                </script>
                <cc1:ModalPopupExtender ID="modalGenerando" runat="server" BackgroundCssClass="modalBackground"
                    PopupControlID="pnlGenerando" PopupDragHandleControlID="" TargetControlID="pnlGenerando">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlGenerando" runat="server" Width="300px" CssClass="modal" BorderStyle="Solid"
                    BorderWidth="1px">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varProcesando %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdateProgress ID="updGenera" runat="server">
                                    <ProgressTemplate>
                                        <img alt="" src="Imagenes/imgAjaxLoader.gif" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>

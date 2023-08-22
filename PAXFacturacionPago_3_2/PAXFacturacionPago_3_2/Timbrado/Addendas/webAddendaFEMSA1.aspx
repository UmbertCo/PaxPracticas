<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webAddendaFEMSA1.aspx.cs" Inherits="Timbrado_Addendas_webAddendaFEMSA1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  
        <asp:Label runat="server" ID="lblTitulo" Text="ADDENDA FEMSA" />

        <style type="text/css">
            .style2
            {
                width: 276px;
            }
        </style>

</head>
<body style="background-color: #FFFFFF">

    <link href="../../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/menu_style.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />

    <%------------------------JQuery----------------------------%>
    <script src='<%= ResolveUrl("~/Scripts/jquery-1.11.3.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/Scripts/jquery-1.11.3.min.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/Scripts/alerts/jquery.alerts.js") %>' type="text/javascript"></script>
    <%------------------------JQuery----------------------------%>

<form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
                  &nbsp;<br />
<table>
<tr>
<td>
                <fieldset class="register" style="width:1100px;">
                    <legend>
                        <asp:Literal ID="Literal2" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" />
                    </legend>
                    <table>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Version"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                <asp:Label ID="Label17" runat="server" Text="Número de Remisión"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label43" runat="server" Text="Sociedad"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="tbVersion" runat="server" MaxLength="2" TabIndex="1" 
                                    Width="35px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                <asp:TextBox ID="tbNumeroRemision" runat="server" MaxLength="16" TabIndex="5" 
                                    Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailRequired7" runat="server" 
                                    ControlToValidate="tbNumeroRemision" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="Número Requerido" ToolTip="Número Requerido" 
                                    ValidationGroup="grupoConsulta"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:DropDownList ID="tbSociedad" runat="server" TabIndex="9" Width="450px">
                                    <asp:ListItem Selected="True" Value="0051">Cervecería Cuauhtémoc Moctezuma S.A. de C.V. </asp:ListItem>
                                    <asp:ListItem Value="0055">Sistema Ambiental Industrial SA de CV. </asp:ListItem>
                                    <asp:ListItem Value="0059">Bienes Raíces CERMOC SA de CV.</asp:ListItem>
                                    <asp:ListItem Value="0065">Carta Blanca de Occidente, SA de CV.</asp:ListItem>
                                    <asp:ListItem Value="0066">Servicios Industriales y Comerciales S.A. de C.V.</asp:ListItem>
                                    <asp:ListItem Value="0067">Distribuidora de Cervezas de Sonora, SA de CV.</asp:ListItem>
                                    <asp:ListItem Value="0068">Comerdis del Norte S.A. de C.V.</asp:ListItem>
                                    <asp:ListItem Value="0069">Distribuidora de Cervezas de Sinaloa, SA de CV</asp:ListItem>
                                    <asp:ListItem Value="0072">Cía. Cervecera Chihuahua.</asp:ListItem>
                                    <asp:ListItem Value="0083">Cervezas Cuauhtémoc S.A. de C.V.</asp:ListItem>
                                    <asp:ListItem Value="0085">Codicome Caribe, SA de CV.</asp:ListItem>
                                    <asp:ListItem Value="0086">Codicome Sureste SA de CV.</asp:ListItem>
                                    <asp:ListItem Value="0087">Codicome Centro, SA de CV.</asp:ListItem>
                                    <asp:ListItem Value="0090">Cervezas Cuauhtemoc Moctezuma del Norte SA de CV.</asp:ListItem>
                                    <asp:ListItem Value="0117">Distribuidora Tecate del Mar de Cortez.</asp:ListItem>
                                    <asp:ListItem Value="0169">Control Negocios Comerciales.</asp:ListItem>
                                    <asp:ListItem Value="0174">DCM de Tamps., SA de CV.</asp:ListItem>
                                    <asp:ListItem Value="0188">Servicios de Administración de Mercados SA de CV.</asp:ListItem>
                                    <asp:ListItem Value="0189">Inmobiliaria BRC en Occidente, SA de CV.</asp:ListItem>
                                    <asp:ListItem Value="0195">Distribuidora Cuauhtemoc Moctezuma en San Luis SA CV.</asp:ListItem>
                                    <asp:ListItem Value="0265">Grupo Cuauhtémoc Moctezuma S.A. de C.V.</asp:ListItem>
                                    <asp:ListItem Value="0120">Fabricas Monterrey SA de CV</asp:ListItem>
                                    <asp:ListItem Value="0123">Sílices de Veracruz SA de CV</asp:ListItem>
                                    <asp:ListItem Value="0129">Sílice de Istmo SA de CV</asp:ListItem>
                                    <asp:ListItem Value="0186">Campos Deportivos.</asp:ListItem>
                                    <asp:ListItem Value="0187">Femsa Servicios.</asp:ListItem>
                                    <asp:ListItem Value="0265">Grupo Cuauhtémoc Moctezuma S.A. de C.V.</asp:ListItem>
                                    <asp:ListItem Value="0306">Centro de Servicios Compartidos.</asp:ListItem>
                                    <asp:ListItem Value="0465">CSCP, S.A DE C.V </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Clase de documento"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                <asp:Label ID="Label38" runat="server" Text="Centro"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label49" runat="server" Text="Correo Electrónico"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddltipodocto" runat="server" TabIndex="2" Width="200px" 
                                    AutoPostBack="True" onselectedindexchanged="ddltipodocto_SelectedIndexChanged">
                                    <asp:ListItem Value="1">Factura</asp:ListItem>
                                    <asp:ListItem Value="2">Consignación</asp:ListItem>
                                    <asp:ListItem Value="3">Retenciones</asp:ListItem>
                                    <asp:ListItem Value="8">Nota de Cargo</asp:ListItem>
                                    <asp:ListItem Value="9">Nota de Crédito</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                <asp:TextBox ID="tbCentro" runat="server" MaxLength="6" TabIndex="10" 
                                    Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailRequired8" runat="server" 
                                    ControlToValidate="tbProveedor" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="Centro Requerido" ToolTip="Centro Requerido" 
                                    ValidationGroup="grupoCosignacion"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:TextBox ID="tbCorreo" runat="server" CssClass="textEntry" MaxLength="60" 
                                    TabIndex="10" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailRequired15" runat="server" 
                                    ControlToValidate="tbCorreo" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                    ValidationGroup="grupoConsulta"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                                    ControlToValidate="tbCorreo" CssClass="failureNotification" Display="Dynamic" 
                                    Height="20px" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                    ValidationGroup="grupoConsulta" Width="20px"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="EmailRequired16" runat="server" 
                                    ControlToValidate="tbCorreo" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                    ValidationGroup="grupoGeneral"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="EmailRequired17" runat="server" 
                                    ControlToValidate="tbCorreo" CssClass="failureNotification" Display="Dynamic" 
                                    ErrorMessage="Correo Requerido" ToolTip="Correo Requerido" 
                                    ValidationGroup="grupoCosignacion"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label32" runat="server" Text="Número de Proveedor"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                <asp:Label ID="Label39" runat="server" Text="Inicio Periodo de Liquidación"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label50" runat="server" Text="Número Entrada a SAP"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="tbProveedor" runat="server" Height="20px" MaxLength="10" 
                                    TabIndex="3" Width="200px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="regxCantidadArt5" runat="server" 
                                    ControlToValidate="tbProveedor" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{10}" ValidationGroup="grupoConsulta">
                                                <img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="EmailRequired1" runat="server" 
                                    ControlToValidate="tbProveedor" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="Número Requerido" ToolTip="Número Requerido" 
                                    ValidationGroup="grupoConsulta"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="EmailRequired12" runat="server" 
                                    ControlToValidate="tbProveedor" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="Número Requerido" ToolTip="Número Requerido" 
                                    ValidationGroup="grupoGeneral"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                <asp:TextBox ID="txtFechaIni" runat="server" BackColor="White" TabIndex="7" 
                                    Width="100px"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" 
                                    Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgIni" 
                                    TargetControlID="txtFechaIni">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgIni1" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaIni1" runat="server" 
                                    ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta">
                                    <img src="../../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="EmailRequired9" runat="server" 
                                    ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="Centro Requerido" ToolTip="Centro Requerido" 
                                    ValidationGroup="grupoCosignacion"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:TextBox ID="tbEntradaSAP" runat="server" MaxLength="10" TabIndex="11" 
                                    Width="200px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="regxCantidadArt8" runat="server" 
                                    ControlToValidate="tbEntradaSAP" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{10}" ValidationGroup="grupoConsulta">
                                                <img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="EmailRequired18" runat="server" 
                                    ControlToValidate="tbEntradaSAP" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="Número Requerido" ToolTip="Número Requerido" 
                                    ValidationGroup="grupoConsulta"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label33" runat="server" Text="Número del Documento de Compras"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                <asp:Label ID="Label40" runat="server" Text="Fin Periodo de Liquidación"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label53" runat="server" Text="Moneda"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="tbNumDocto" runat="server" MaxLength="10" TabIndex="4" 
                                    Width="200px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="regxCantidadArt6" runat="server" 
                                    ControlToValidate="tbNumDocto" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{10}" ValidationGroup="grupoConsulta">
                                                <img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="EmailRequired2" runat="server" 
                                    ControlToValidate="tbNumDocto" CssClass="failureNotification" Display="Dynamic" 
                                    ErrorMessage="Número Requerido" ToolTip="Número Requerido." 
                                    ValidationGroup="grupoConsulta"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White" TabIndex="8" 
                                    Width="100px"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" 
                                    Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgFin" 
                                    TargetControlID="txtFechaFin">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgFin1" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaFin1" runat="server" 
                                    ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta">
                                    <img src="../../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="EmailRequired10" runat="server" 
                                    ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="Centro Requerido" ToolTip="Centro Requerido" 
                                    ValidationGroup="grupoCosignacion"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:DropDownList ID="ddlmoneda" runat="server" TabIndex="12" Width="200px">
                                    <asp:ListItem Value="MXN">Pesos</asp:ListItem>
                                    <asp:ListItem Value="USD">Dólar Americano</asp:ListItem>
                                    <asp:ListItem Value="EUR">Euros</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label51" runat="server" Text="Indicador de Retención"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="Label52" runat="server" Text="Porcentajes"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label42" runat="server" Text="Número de Empleado" 
                                    Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlretencion1" runat="server" TabIndex="13" Width="200px">
                                                <asp:ListItem Value="1">Honorarios</asp:ListItem>
                                                <asp:ListItem Value="2">Arrendamientos</asp:ListItem>
                                                <asp:ListItem Value="3">Fletes</asp:ListItem>
                                                <asp:ListItem Value="4">Honorarios 1%</asp:ListItem>
                                                <asp:ListItem Value="5">Honorarios 2%</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbRetencion1" runat="server" MaxLength="13" TabIndex="14" 
                                                Width="75px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlretencion2" runat="server" TabIndex="15" Width="200px">
                                                <asp:ListItem Value="1">Honorarios</asp:ListItem>
                                                <asp:ListItem Value="2">Arrendamientos</asp:ListItem>
                                                <asp:ListItem Value="3">Fletes</asp:ListItem>
                                                <asp:ListItem Value="4">Honorarios 1%</asp:ListItem>
                                                <asp:ListItem Value="5">Honorarios 2%</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbRetencion2" runat="server" MaxLength="13" TabIndex="16" 
                                                Width="75px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlretencion3" runat="server" TabIndex="17" Width="200px">
                                                <asp:ListItem Value="1">Honorarios</asp:ListItem>
                                                <asp:ListItem Value="2">Arrendamientos</asp:ListItem>
                                                <asp:ListItem Value="3">Fletes</asp:ListItem>
                                                <asp:ListItem Value="4">Honorarios 1%</asp:ListItem>
                                                <asp:ListItem Value="5">Honorarios 2%</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbRetencion3" runat="server" MaxLength="13" TabIndex="18" 
                                                Width="75px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:TextBox ID="tbNumeroEmpleado" runat="server" MaxLength="8" ReadOnly="True" 
                                    TabIndex="9" Visible="False" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style2">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </fieldset></td>
</tr>
    <tr>
        <td>
            <table align="right">
                <tr>
                    <td>
                        <asp:Button ID="Button1" runat="server" CssClass="botonGrande" Height="30px" 
                            onclick="Button1_Click" TabIndex="19" Text="Guardar Addenda" 
                            Width="150px" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnCerrar" runat="server" CssClass="botonGrande" Height="30px" 
                            TabIndex="20" Text="Cerrar Página" Width="150px" 
                            onclick="btnCerrar_Click" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
</table>

    <br />
    <br />
    </ContentTemplate>
    </asp:UpdatePanel>
     </form>
</body>
</html>


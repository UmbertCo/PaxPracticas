<%--<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webAddendaFemsa.aspx.cs" Inherits="Addendas_Ejemplo" %>--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webAddendaFemsa.aspx.cs" Inherits="Addendas_Ejemplo" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

    <link href="../../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/menu_style.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />

    <%------------------------JQuery----------------------------%>
    <script src='<%= ResolveUrl("~/Scripts/jquery-1.11.3.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/Scripts/jquery-1.11.3.min.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/Scripts/alerts/jquery.alerts.js") %>' type="text/javascript"></script>
    <%------------------------JQuery----------------------------%>

</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        <asp:Label runat="server" ID="lblTitulo" Text="Addenda FEMSA" />
    </h2>
                <fieldset class="register" style="width:890px;">
                    <legend><asp:Literal runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" ID="Literal1" /></legend>
    <table>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Version"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label8" runat="server" Text="Número de Remisión"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="tbVersion" runat="server" TabIndex="1" Width="35px" 
                    MaxLength="2"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:TextBox ID="tbNumeroRemision" runat="server" Width="200px" TabIndex="8" 
                    MaxLength="16"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Clase de documento"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label9" runat="server" Text="Número de Empleado"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddltipodocto" runat="server" TabIndex="2" Width="200px">
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
            <td>
                <asp:TextBox ID="tbNumeroEmpleado" runat="server" Width="200px" ReadOnly="True" 
                    TabIndex="9" MaxLength="8"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Sociedad"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label10" runat="server" Text="Centro"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="tbSociedad" runat="server" Width="200px" TabIndex="3" 
                    MaxLength="4"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:TextBox ID="tbCentro" runat="server" Width="200px" TabIndex="10" 
                    MaxLength="4"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Número de Proveedor"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label11" runat="server" Text="Inicio Periodo de Liquidación"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="tbProveedor" runat="server" Height="20px" Width="200px" 
                    MaxLength="10" TabIndex="4"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regxCantidadArt" runat="server" 
                                                ControlToValidate="tbProveedor" CssClass="failureNotification" 
                                                Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ValidationExpression="^[0-9]{10}" 
                                                ValidationGroup="grupoConsulta">
                                                <img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                                <asp:TextBox ID="txtFechaIni" runat="server" BackColor="White" Width="100px" 
                                    TabIndex="11" ></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaIni" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgIni">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgIni" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaIni" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img src="../../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvFechaIni" runat="server" 
                                    ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                    ValidationGroup="grupoConsulta" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" Width="16px" >*</asp:RequiredFieldValidator>
                            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Número del Documento de Compras"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label12" runat="server" Text="Fin Periodo de Liquidación"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="tbNumDocto" runat="server" Width="200px" MaxLength="10" 
                    TabIndex="5"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regxCantidadArt0" runat="server" 
                                                ControlToValidate="tbNumDocto" CssClass="failureNotification" 
                                                Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ValidationExpression="^[0-9]{10}" 
                                                ValidationGroup="grupoConsulta">
                                                <img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                                <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White" Width="100px" 
                                    TabIndex="12"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgFin">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgFin" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaFin" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img src="../../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" 
                                    ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                    ValidationGroup="grupoConsulta" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" Width="16px" >*</asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Moneda"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label14" runat="server" Text="Correo Electrónico"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlmoneda" runat="server" TabIndex="6" Width="200px">
                    <asp:ListItem Value="MXN">Pesos</asp:ListItem>
                    <asp:ListItem Value="USD">Dólar Americano</asp:ListItem>
                    <asp:ListItem Value="EUR">Euros</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:TextBox ID="tbCorreo" runat="server" CssClass="textEntry" MaxLength="60" 
                    TabIndex="13"></asp:TextBox>

                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                    ControlToValidate="tbCorreo" CssClass="failureNotification" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                    ValidationGroup="grupoConsulta" Display="Dynamic"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>


                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="tbCorreo" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    Width="20px" ValidationGroup="grupoConsulta" 
                    CssClass="failureNotification" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"  Display="Dynamic"><img 
                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>

            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="Número Entrada a SAP"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="tbEntradaSAP" runat="server" Width="200px" MaxLength="10" 
                    TabIndex="7"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regxCantidadArt1" runat="server" 
                                                ControlToValidate="tbEntradaSAP" CssClass="failureNotification" 
                                                Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ValidationExpression="^[0-9]{10}" 
                                                ValidationGroup="grupoConsulta">
                                                <img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                <asp:Label ID="Label13" runat="server" Text="Indicador de Retención"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                <asp:Label ID="Label15" runat="server" Text="Porcentajes"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                <asp:DropDownList ID="ddlretencion1" runat="server" TabIndex="14" Width="200px">
                    <asp:ListItem Value="1">Honorarios</asp:ListItem>
                    <asp:ListItem Value="2">Arrendamientos</asp:ListItem>
                    <asp:ListItem Value="3">Fletes</asp:ListItem>
                    <asp:ListItem Value="4">Honorarios 1%</asp:ListItem>
                    <asp:ListItem Value="5">Honorarios 2%</asp:ListItem>
                </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                <asp:TextBox ID="tbRetencion1" runat="server" Width="75px" TabIndex="15" MaxLength="13"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                <asp:DropDownList ID="ddlretencion2" runat="server" TabIndex="16" Width="200px">
                    <asp:ListItem Value="1">Honorarios</asp:ListItem>
                    <asp:ListItem Value="2">Arrendamientos</asp:ListItem>
                    <asp:ListItem Value="3">Fletes</asp:ListItem>
                    <asp:ListItem Value="4">Honorarios 1%</asp:ListItem>
                    <asp:ListItem Value="5">Honorarios 2%</asp:ListItem>
                </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                <asp:TextBox ID="tbRetencion2" runat="server" Width="75px" TabIndex="17" MaxLength="13"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                <asp:DropDownList ID="ddlretencion3" runat="server" TabIndex="18" Width="200px">
                    <asp:ListItem Value="1">Honorarios</asp:ListItem>
                    <asp:ListItem Value="2">Arrendamientos</asp:ListItem>
                    <asp:ListItem Value="3">Fletes</asp:ListItem>
                    <asp:ListItem Value="4">Honorarios 1%</asp:ListItem>
                    <asp:ListItem Value="5">Honorarios 2%</asp:ListItem>
                </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                <asp:TextBox ID="tbRetencion3" runat="server" Width="75px" TabIndex="19" MaxLength="13"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td align="right">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        </table>
                </fieldset>
                <br />
<table align="right">
<tr>
<td>
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                    Text="Guardar Addenda" ValidationGroup="grupoConsulta" TabIndex="20" 
                    CssClass="botonGrande" Height="30px" />
    </td>
<td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td>
</tr>
</table>

    <br />
    <br />
&nbsp;
</asp:Content>



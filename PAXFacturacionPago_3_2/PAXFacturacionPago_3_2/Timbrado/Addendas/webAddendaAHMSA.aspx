<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webAddendaAHMSA.aspx.cs" Inherits="Timbrado_Addendas_webAddendaAHMSA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">

        .fontText
        {
            font: 8pt verdana;
        }
        .style1
        {
            height: 23px;
        }
        
        .anchoCol1
        {
            width:25%;
            height: 23px;  
        }
        .anchoCol2
        {
            width:35%; 
            height: 23px; 
        }
        .anchoCol3
        {
            width:25%; 
            height: 23px; 
        }
        .anchoCol4
        {
            width:25%; 
            height: 23px; 
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
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
        <fieldset class ="register" style="width:700px; ";>
                    <legend>
                        <asp:Literal ID="ltEncabezadoAHMSA" runat="server" Text="Addenda Altos Hornos de México" />
                    </legend>
                        <asp:Panel ID="pnlAddendaAHM" runat ="server">
                        <asp:UpdatePanel ID="udpDocumento" runat="server">
                        <ContentTemplate>
                        <asp:Panel ID="pnlDocumento" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTipoDocumento" runat="server" Text="Tipo Documento" />
                                </td>
                                <td>
                                    <asp:Label ID="lblClase" runat="server" Text="Clase" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlTipoDocumento" runat="server" 
                                        OnSelectedIndexChanged="ddlTipoDocumento_SelectedIndexChanged" 
                                        AutoPostBack="True"/>    
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlClase" runat="server" 
                                        OnSelectedIndexChanged="ddlClase_SelectedIndexChanged" 
                                        AutoPostBack="True"/>    
                                </td>
                            </tr>
                            </table>
                            <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNumSociedad" runat="server" Text="Núm. Sociedad" />
                                </td>
                                <td>
                                    <asp:Label ID="lblNumDivision" runat="server" Text="Núm. División" />
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlNumSociedad" runat="server" 
                                        onselectedindexchanged="ddlNumSociedad_SelectedIndexChanged" AutoPostBack="true"/>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlNumDivision" runat="server"/>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNumProveedor" runat="server" Text="Núm. Proveedor" />
                                </td>
                                <td>
                                    <asp:Label ID="lblCorreo" runat="server" Text="Correo Electrónico" />
                                </td>
                                <td>
                                    <asp:Label ID="lblMoneda" runat="server" Text="Modena" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNumProveedor" runat="server" MaxLength="10" />
                                    <asp:RequiredFieldValidator ID="rfvNumProveedor" runat="server"
                                     ControlToValidate="txtNumProveedor" ValidationGroup="vgAddenda">
                                     <img src="../../Imagenes/error_sign.gif" /> 
                                     </asp:RequiredFieldValidator>
                                      <asp:RegularExpressionValidator ID="revNumero" runat="server"
                                    ControlToValidate="txtNumProveedor" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="Debe ser un valor numérico" 
                                    ValidationExpression="[0-9]+" 
                                    ValidationGroup="vgAddenda">
                                    <img src="../../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCorreo" runat="server" />
                                    <asp:RequiredFieldValidator ID="rvCorreo" runat="server"
                                     ControlToValidate="txtCorreo" ValidationGroup="vgAddenda"
                                     CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="El correo es requerido" >
                                     <img src="../../Imagenes/error_sign.gif" /> 
                                     </asp:RequiredFieldValidator>
                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                    ValidationGroup="vgAddenda" Width="131px">
                                    <img src="../../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMoneda" runat="server" />
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Panel runat="server" ID="pnlDetalles">
                           
                            <table>
                                <tr>
                                    <td>
                                    <asp:UpdatePanel ID="udpServicios" runat="server">
                                    <ContentTemplate>
                                     <asp:Panel runat="server" ID="pnlServicios" BorderStyle="Solid" BorderWidth="1">
                                     <table>
                                     <tr>
                                     <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblServicios" Text="Pedidos"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                <asp:Panel ID="pnlTrvServicios" runat="server" Width="160px" Height="210px" 
                                                ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                                                    <asp:TreeView ID="trvServicios" runat="server" BackColor="White" Height="200px" 
                                                        ImageSet="Arrows" ShowLines="True" Width="150px" >
                                                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                        <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" 
                                                            HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                                        <ParentNodeStyle Font-Bold="False" />
                                                        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" 
                                                            HorizontalPadding="0px" VerticalPadding="0px" />
                                                    </asp:TreeView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>

                                        </table>
                                    </td>

                                    <td valign="top">
                                        <table>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:Label ID="lblNumServicio" runat="server" Text="Núm. Pedido" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtNumServicio" runat="server" MaxLength="10" />
                                                    <asp:RegularExpressionValidator ID="revNumServicio" runat="server"
                                    ControlToValidate="txtNumServicio" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="Debe ser un valor numérico" 
                                    ValidationExpression="[0-9]+" 
                                    ValidationGroup="vgServicios">
                                    <img src="../../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAgrServicio" runat="server" Text="<<" CssClass="botonGrande" 
                                                        onclick="btnAgrServicio_Click" ValidationGroup="vgServicios"/>
                                                    <asp:Button ID="btnRemServicio" runat="server" Text=">>" CssClass="botonGrande" 
                                                        onclick="btnRemServicio_Click"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:Label ID="lblNumRecepcion" runat="server" Text="Núm. Recepción" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtNumRecepcion" runat="server" MaxLength="10"/>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                    ControlToValidate="txtNumRecepcion" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="Debe ser un valor numérico" 
                                    ValidationExpression="[0-9]+" 
                                    ValidationGroup="vgRecepcion">
                                    <img src="../../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAgrRecepcion" runat="server" Text="<<" 
                                                        CssClass="botonGrande" onclick="btnAgrRecepcion_Click" ValidationGroup="vgRecepcion"/>
                                                    <asp:Button ID="btnRemRecepcion" runat="server" Text=">>" 
                                                        CssClass="botonGrande" onclick="btnRemRecepcion_Click"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    
                                </tr>
                            </table>
                                </asp:Panel>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                                </td>
                                <td>
                                    </td>
                                    <td valign="top">
                                        <asp:Panel ID="pnlDetalleDatos" runat="server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblHojaServicio" runat="server" Text="Núm. Hoja de Servicio" />     
                                                    </td>
                                                    <td>
                                                        </td>
                                                    <td>
                                                        <asp:Label ID="lblNumTransporte" runat="server" Text="Núm. Transporte" />     
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtHojaServicio" runat="server" MaxLength="10" />
                                                        </td>
                                                        <td>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                                            ControlToValidate="txtHojaServicio" CssClass="failureNotification" Display="Dynamic" 
                                                            ToolTip="Debe ser un valor numérico" 
                                                            ValidationExpression="[0-9]+" 
                                                            ValidationGroup="vgRecepcion">
                                                            <img src="../../Imagenes/error_sign.gif" />
                                                            </asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNumTransporte" runat="server" MaxLength="10" />
                                                        </td>
                                                        <td>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                                            ControlToValidate="txtNumTransporte" CssClass="failureNotification" Display="Dynamic" 
                                                            ToolTip="Debe ser un valor numérico" 
                                                            ValidationExpression="[0-9]+" 
                                                            ValidationGroup="vgRecepcion">
                                                            <img src="../../Imagenes/error_sign.gif" />
                                                            </asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNumCtaxPag" runat="server" Text="Núm. Cta. Por Pagar" />

                                                    </td>
                                                    <td>
                                                        </td>
                                                    <td>
                                                        <asp:Label ID="lblEjCtaxPag" runat="server" Text="Ejercicio Cta. Por Pagar" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtNumCtaxPag" runat="server" MaxLength="10" />
                                                        </td>
                                                        <td>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                                                            ControlToValidate="txtNumCtaxPag" CssClass="failureNotification" Display="Dynamic" 
                                                            ToolTip="Debe ser un valor numérico" 
                                                            ValidationExpression="[0-9]+" 
                                                            ValidationGroup="vgRecepcion">
                                                            <img src="../../Imagenes/error_sign.gif" />
                                                            </asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEjCtaxPag" runat="server" MaxLength="4" />
                                                        </td>
                                                        <td>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                                            ControlToValidate="txtEjCtaxPag" CssClass="failureNotification" Display="Dynamic" 
                                                            ToolTip="Debe ser un valor numérico" 
                                                            ValidationExpression="[0-9]+" 
                                                            ValidationGroup="vgRecepcion">
                                                            <img src="../../Imagenes/error_sign.gif" />
                                                            </asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                         <asp:Label ID="lblFechaIni" runat="server" 
                                                        Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                                        AssociatedControlID="txtFechaIni"></asp:Label>
                                                    </td>
                                                    <td>
                                                        
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFechaFin" runat="server" 
                                                        Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                                        AssociatedControlID="txtFechaFin"></asp:Label>

                                                    </td>
                                                    <td>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style2">
                                                   
                                                    <asp:TextBox ID="txtFechaIni" runat="server" BackColor="White" Width="100px" ></asp:TextBox>
                                                    
                                                    <asp:Image ID="imgIni" runat="server" 
                                                        ImageUrl="../../Imagenes/icono_calendario.gif" />
                                                        </td>
                                                        <td>
                                                    <asp:RegularExpressionValidator ID="revFechaIni" runat="server" Display="Dynamic"
                                                        ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                                        ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                                        ValidationGroup="vgAddenda" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                                        <img src="../../Imagenes/error_sign.gif" />
                                                        </asp:RegularExpressionValidator>
                                                    <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" 
                                                        Enabled="True" TargetControlID="txtFechaIni" Format="dd/MM/yyyy" 
                                                        PopupButtonID="imgIni"></cc1:CalendarExtender>
                                                </td>
                                                <td>
                                                    
                                                    <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White" Width="100px"></asp:TextBox>
                                                    
                                                    <asp:Image ID="imgFin" runat="server" 
                                                        ImageUrl="../../Imagenes/icono_calendario.gif" />
                                                        </td>
                                                        <td>
                                                    <asp:RegularExpressionValidator ID="revFechaFin" runat="server" Display="Dynamic"
                                                        ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                                        ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                                        ValidationGroup="vgAddenda" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                                        <img src="../../Imagenes/error_sign.gif" />
                                                        </asp:RegularExpressionValidator>
                                                    <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" 
                                                        Enabled="True" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" 
                                                        PopupButtonID="imgFin"></cc1:CalendarExtender>
                                                </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    <asp:UpdatePanel ID="udpAnexos" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlAnexos" runat="server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAnexos" runat="server" Text="Anexos"/>
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtAnexo" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAgrAnexo" runat="server" Text="Agregar" 
                                                            CssClass="botonGrande" onclick="btnAgrAnexo_Click"/>
                                                        <asp:Button ID="btnQuitarAnexo" runat="server" Text="Quitar"
                                                            CssClass="botonGrande" onclick="btnQuitarAnexo_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:ListBox ID="lsbAnexos" runat="server" Width="250px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td></td>
                                    <td valign="bottom">
                                    <asp:UpdatePanel ID="udpGuardar" runat="server">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnGuardar" runat="server" CssClass="botonGrande" Height="30px" 
                                                        TabIndex="7" Text="Guardar Addenda" 
                                                        Width="150px" ValidationGroup="vgAddenda" onclick="btnGuardar_Click"/>
                                                </td>
                                                <td>
                                                    &nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCerrar" runat="server" CssClass="botonGrande" Height="30px" 
                                                        TabIndex="8" Text="Cerrar Página" Width="150px" 
                                                        onclick="btnCerrar_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                </table>
                            
                        </asp:Panel>
                        </asp:Panel>
        </fieldset>
    </div>
    </form>
</body>
</html>

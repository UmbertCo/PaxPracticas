<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webAddendaBeatriz.aspx.cs" Inherits="Timbrado_Addendas_webAddendaBeatriz" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server"> </asp:ScriptManager>
    <div>

        <table>
            <tr>
                <td>
                <fieldset class ="register" style="width:500px";>
                    <legend>
                        <asp:Literal ID="LtEncabezadoAceros" runat="server" Text="ADDENDA CARGO EXPRESS" />
                    </legend>
                        <asp:Panel ID="pnlAdendaAcerosyEquiposMineros" runat ="server">
                            <table >
                                <tr>
                                    <td align="right" >
                                        &nbsp;</td>
                                    <td align ="right" >
                                       <asp:label id="idFechEmb" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechEmbar %>"></asp:label></td>
                                    <td>
                                        <asp:TextBox ID="txtFechaGuia" runat="server" BackColor="White" Width="100px" 
                                            TabIndex="1" ></asp:TextBox>
                                        <asp:Image ID="imgGuia" runat="server" 
                                            ImageUrl="~/Imagenes/icono_calendario.gif" />
                                            <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" 
                                                Enabled="True" TargetControlID="txtFechaGuia" Format="dd/MM/yyyy" 
                                                PopupButtonID="imgGuia"></cc1:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="revFechaguia" runat="server" Display="Dynamic"
                                            ControlToValidate="txtFechaGuia" CssClass="failureNotification" 
                                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                        <img src="../../Imagenes/error_sign.gif"/>
                                        </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvFechaguia" runat="server" 
                                            ControlToValidate="txtFechaGuia" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ValidationGroup="validaAdenda">
                                            <img src="../../Imagenes/error_sign.gif" />
                                            </asp:RequiredFieldValidator>
                                        
                                    </td>
                                    <td >
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        &nbsp;</td>
                                    <td align="right">
                                       <asp:Label ID="lblFechPago" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblFechpago %>"></asp:Label></td><%-- Fecha de pago--%>
                                    <td>
                                        <asp:TextBox ID="txtFechaPago" runat="server" BackColor="White" Width="100px" 
                                            TabIndex="2"></asp:TextBox>
                                        <asp:Image ID="imgpago" runat="server" 
                                            ImageUrl="~/Imagenes/icono_calendario.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                            Enabled="True" TargetControlID="txtFechaPago" Format="dd/MM/yyyy" 
                                            PopupButtonID="imgpago"></cc1:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="revFechapago" runat="server" Display="Dynamic"
                                            ControlToValidate="txtFechaPago" CssClass="failureNotification" 
                                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                        <img src="../../Imagenes/error_sign.gif"/>
                                        </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvFechapago" runat="server" 
                                            ControlToValidate="txtFechaPago" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ValidationGroup="validaAdenda">
                                            <img src="../../Imagenes/error_sign.gif" />
                                            </asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        &nbsp;</td>
                                    <td align="right">

                                     <asp:Label ID="lblNoguia" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblNoGuia %>"></asp:Label></td>    <%--No. Guía--%>
                                    <td>
                                        <asp:TextBox ID="txtNoGuia" runat="server" TabIndex="3"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                            ControlToValidate="txtNoGuia" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ValidationGroup="validaAdenda">
                                            <img src="../../Imagenes/error_sign.gif" />
                                            </asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td align="right">
                                        &nbsp;</td>
                                </tr>
                                
                                <tr>
                                    <td align="right">
                                        &nbsp;</td>
                                    <td align="right">
                                      <asp:Label ID="lblOrdCom" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblOrdComp %>"></asp:Label></td>  <%--Orden de Compra--%>
                                    <td>
                                        <asp:TextBox ID="txtOrdenCompra" runat="server" TabIndex="4"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                            ControlToValidate="txtOrdenCompra" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ValidationGroup="validaAdenda">
                                            <img src="../../Imagenes/error_sign.gif" />
                                            </asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td  align="right">
                                     <asp:Label ID="lblOrigen" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblOri %>"></asp:Label></td><%--Origen--%>
                                    <td>
                                        <asp:TextBox ID="txtOrigen" runat="server" TabIndex="5"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                            ControlToValidate="txtOrigen" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ValidationGroup="validaAdenda">
                                            <img src="../../Imagenes/error_sign.gif" />
                                            </asp:RequiredFieldValidator>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                    <td align ="right">
                                     <asp:Label ID="lblDest" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblDest %>"></asp:Label></td> <%--Destino--%>
                                    <td >
                                        <asp:TextBox ID="txtDestino" runat="server" TabIndex="6"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                            ControlToValidate="txtDestino" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ValidationGroup="validaAdenda">
                                            <img src="../../Imagenes/error_sign.gif" />
                                            </asp:RequiredFieldValidator>
                                    </td>
                                    <td >
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td >
                                        &nbsp;</td>
                                    <td align = "right">
                                       <asp:Label ID="lblUni" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblUnid %>"></asp:Label></td>  <%--Unidad--%>
                                    <td >
                                        <asp:TextBox ID="txtUnidad" runat="server" TabIndex="7"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                            ControlToValidate="txtUnidad" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ValidationGroup="validaAdenda">
                                            <img src="../../Imagenes/error_sign.gif" />
                                            </asp:RequiredFieldValidator>
                                    </td>
                                    <td >
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        
                                    </td>
                                    <td >
                                        &nbsp;</td>
                                        <td >
                                        &nbsp;</td> 
                                        <td >
                                            &nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                </fieldset>
                </td>
                </tr>
                <tr>
                
                    <td align="right">
                        <div align="right" style="height:40px;">
                            <table align="right">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnGuardar" runat="server" CssClass="botonGrande" Height="30px" 
                                            TabIndex="8" Text="<%$ Resources:resCorpusCFDIEs, btnGuarAddenda %>"
                                            Width="150px" ValidationGroup="validaAdenda" onclick="btnGuardar_Click"/>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCerrar" runat="server" CssClass="botonGrande" Height="30px" 
                                            TabIndex="9" Text="<%$ Resources:resCorpusCFDIEs, btnCerraAddenda %>" Width="150px" />
                                    </td>
                                    <td>
                                    </td>
                              </tr>
                           </table>                
                        </div>            
                    </td>  
                </tr>
        </table>
    </div>
    </form>
</body>
</html>

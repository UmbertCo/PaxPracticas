<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webAddendaAcerosyEquiposMineros.aspx.cs" Inherits="Timbrado_Addendas_webAddendaAcerosyEquiposMineros" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
<script type="text/javascript" language="javascript">

    function fnMostrar() {
        var vCondPago = document.getElementById('<%= ddlCondPago.ClientID %>');
        var vlblDiasCredito = document.getElementById('<%= lblDiasdeCredito.ClientID %>');
        var vDiasCredito = document.getElementById('<%= ddlDiasdeCredito.ClientID %>');

        if (vCondPago != null) {
            if (vCondPago.selectedIndex == 1) {
                vlblDiasCredito.setAttribute('style', 'visibility:block');
                vDiasCredito.setAttribute('style', 'visibility:block');
                
                return;
            }
        }
        
        vlblDiasCredito.setAttribute('style', 'visibility:hidden');
        vDiasCredito.setAttribute('style', 'visibility:hidden');
    }


</script>

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
    <div>
        <table>
            <tr>
                <td>
                <fieldset class ="register" style="width:500px";>
                    <legend>
                        <asp:Literal ID="LtEncabezadoAceros" runat="server" Text="ADENDA ACEROS Y EQUIPOS MINEROS" />
                    </legend>
                        <asp:Panel ID="pnlAdendaAcerosyEquiposMineros" runat ="server">
                            <table >
                                <tr>
                                    <td align="right" class="anchoCol1">
                                        <asp:Label ID="lblNumOrden" runat="server" CssClass ="fontText"
                                        Text="Numero de Orden: "></asp:Label>
                                    </td>
                                    <td class="anchoCol2">
                                        <asp:TextBox ID="txtNumIOrden" runat="server" CssClass="fontText"
                                            TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfNumIOrden" runat="server" 
                                            ControlToValidate="txtNumIOrden" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ValidationGroup="validaAdenda"> 
                                                <img src="../../Imagenes/error_sign.gif" />
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="anchoCol3">
                                        &nbsp;
                                    </td>
                                    <td class="anchoCol4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right"  class="anchoCol1">
                                        <asp:Label ID="lblCondPago" runat="server" class="fontText" 
                                        Text="Condiciones de pago: "></asp:Label>
                                         
                                    </td>
                                    <td class="anchoCol2">
                                        <asp:DropDownList ID="ddlCondPago" runat="server" CssClass="fontText" 
                                        TabIndex="2">
                                        <asp:ListItem Selected="True">Contado</asp:ListItem>
                                        <asp:ListItem>Crédito</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td   class="anchoCol3">
                                        <asp:Label ID="lblDiasdeCredito" runat="server" CssClass="fontText"
                                         Text="Días de Crédito: " ></asp:Label>
                                    </td>
                                    <td class="anchoCol4">
                                        <asp:DropDownList ID="ddlDiasdeCredito" runat="server" CssClass="fontText" 
                                            TabIndex="3">
                                            <asp:ListItem Selected="True">8</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>30</asp:ListItem>
                                            <asp:ListItem>45</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="anchoCol1">
                                        <asp:Label ID="lblFechaVencimiento0" runat="server" CssClass="fontText"
                                        Text="Fecha de Vencimiento (dd/mm/yyyy): "></asp:Label>
                                    </td>
                                    <td class="anchoCol2">
                                        <asp:TextBox ID="txtFechaVencimiento" runat="server" CssClass="fontText">
                                        </asp:TextBox>
                                        
                                        <asp:RequiredFieldValidator ID="rfFechaVencimiento" runat="server" 
                                            ControlToValidate="txtFechaVencimiento" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ValidationGroup="validaAdenda"> 
                                                <img src="../../Imagenes/error_sign.gif" />
                                        </asp:RequiredFieldValidator>
<%--                                        <asp:RangeValidator ID="RangeValidator1" runat="server"
                                            ControlToValidate="txtFechaVencimiento" 
                                            Type="Date" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>"
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>"
                                            ValidationGroup="validaAdenda" MaximumValue="01-01-2100" 
                                            MinimumValue="01-01-1900">
                                                <img src="../../Imagenes/error_sign.gif" />
                                            </asp:RangeValidator>--%>
                                        <asp:RegularExpressionValidator ID="revFechaVencimiento" runat="server" Display="Dynamic"
                                            ControlToValidate="txtFechaVencimiento" CssClass="failureNotification" 
                                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                            ValidationGroup="validaAdenda" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                                <img src="../../Imagenes/error_sign.gif" />
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td class="anchoCol3">
                                        &nbsp;</td>
                                    <td class="anchoCol4">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td   class="anchoCol1">
                                        &nbsp;</td>
                                    <td class="anchoCol2">
                                        &nbsp;</td>
                                    <td class="anchoCol3">
                                        &nbsp;</td>
                                    <td class="anchoCol4">
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
                                            TabIndex="7" Text="Guardar Addenda" 
                                            Width="150px" ValidationGroup="validaAdenda" onclick="btnGuardar_Click"/>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCerrar" runat="server" CssClass="botonGrande" Height="30px" 
                                            TabIndex="8" Text="Cerrar Página" Width="150px" />
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

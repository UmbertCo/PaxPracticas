<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webComplementosTerceros.aspx.cs" Inherits="Timbrado_Complementos_webComplementosTerceros" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">


.failureNotification
{
    font-size: 1.2em;
    color: Red;
    background-image: url('../../Imagenes/error_sign.gif');
    background-repeat:no-repeat;
    display:inline-block;
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
   
       <script type="text/javascript" language="javascript">
       //Impuestos Retenciones
           function enableCheckISR() {
               if (document.getElementById('<%=cbIVARet.ClientID%>').checked == false) {
                   if (document.getElementById('<%=cbISRRet.ClientID%>').checked == false) {
                       document.getElementById('<%=cbISRRet.ClientID%>').checked = true;
                   }
                   ValidatorEnable(document.getElementById('<%= rvfImpIvaRetReq.ClientID %>'), false);
                   document.getElementById('<%= tbimporterIVARet.ClientID %>').value = 0;
               }
               else {
                   ValidatorEnable(document.getElementById('<%= rvfImpIvaRetReq.ClientID %>'), true);
               }
           }

           function enableCheckIVARet() {
               if (document.getElementById('<%=cbISRRet.ClientID%>').checked == false) {
                   if (document.getElementById('<%=cbIVARet.ClientID%>').checked == false) {
                       document.getElementById('<%=cbIVARet.ClientID%>').checked = true;
                   }
                   ValidatorEnable(document.getElementById('<%= rvfImpRetISR.ClientID %>'), false);
                   document.getElementById('<%= tbimporteISRRet.ClientID %>').value = 0;
               }
               else {
                   ValidatorEnable(document.getElementById('<%= rvfImpRetISR.ClientID %>'), true);
               }
           }
           //Impuestos Traslados
           function enableCheckIEPSTra() {
               if (document.getElementById('<%=cbIVATra.ClientID%>').checked == false) {
                   if (document.getElementById('<%=cbIEPSTra.ClientID%>').checked == false) {
                       document.getElementById('<%=cbIEPSTra.ClientID%>').checked = true;
                   }
                   ValidatorEnable(document.getElementById('<%= rvfImpIvaTraReq.ClientID %>'), false);
                   ValidatorEnable(document.getElementById('<%= rvftasaIvaReq.ClientID %>'), false);

                   document.getElementById('<%= tbtasaIVATra.ClientID %>').value = 0;
                   document.getElementById('<%= tbimporteIVATra.ClientID %>').value = 0;

               }
               else {
                   ValidatorEnable(document.getElementById('<%= rvfImpIvaTraReq.ClientID %>'), true);
                   ValidatorEnable(document.getElementById('<%= rvftasaIvaReq.ClientID %>'), true);
               }
           }

           function enableCheckIVATra() {
               if (document.getElementById('<%=cbIEPSTra.ClientID%>').checked == false) {
                   if (document.getElementById('<%=cbIVATra.ClientID%>').checked == false) {
                       document.getElementById('<%=cbIVATra.ClientID%>').checked = true;
                   }
                   ValidatorEnable(document.getElementById('<%= rvfImpIEPSTraReq.ClientID %>'), false);
                   ValidatorEnable(document.getElementById('<%= rvftasaIEPSReq.ClientID %>'), false);

                   document.getElementById('<%= tbtasaIEPSTra.ClientID %>').value = 0;
                   document.getElementById('<%= tbimporteIEPSTra.ClientID %>').value = 0;
               }
               else {
                   ValidatorEnable(document.getElementById('<%= rvfImpIEPSTraReq.ClientID %>'), true);
                   ValidatorEnable(document.getElementById('<%= rvftasaIEPSReq.ClientID %>'), true);
               }
           }
           //Habilitar/Deshabilitar validadores
           function enableValidador() {

               enableCheckISR();
               enableCheckIVARet();
               enableCheckIEPSTra();
               enableCheckIVATra();

           }
</script>

    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
         <asp:Panel ID="Panel1" runat="server" Width="1200px">
                              <table>
                                  <tr>
                                      <td valign="top" style="width:600px">
                                          <fieldset style="width: 560px; height: 240px">
                                              <legend>
                                                  <asp:Literal ID="Literal9" runat="server" Text="Información General"></asp:Literal>
                                              </legend>
                                              <table>
                                                  <tr>
                                                      <td align="left" class="style2">
                                                          <asp:Label ID="Label141" runat="server" CssClass="fontText" 
                                                              Text="<%$ Resources:resCorpusCFDIEs, lblVersion %>"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          </td>
                                                      <td align="left">
                                                          <asp:Label ID="Label142" runat="server" CssClass="fontText" Text="RFC"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          <asp:Label ID="Label168" runat="server" CssClass="fontText" Text="Nombre" 
                                                              Visible="False"></asp:Label>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:TextBox ID="tbVersionTerceros" runat="server" CssClass="fontText" 
                                                              ReadOnly="True" TabIndex="1" Width="50px">1.1</asp:TextBox>
                                                      </td>
                                                      <td align="left">
                                                         </td>
                                                      <td align="left">
                                                          <asp:TextBox ID="tbRFCTerceros" runat="server" CssClass="fontText" TabIndex="2" 
                                                              Width="150px"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon30" runat="server" 
                                                              ControlToValidate="tbRFCTerceros" CssClass="failureNotification" 
                                                              ErrorMessage="RFC Requerido" Height="22px" ToolTip="RFC Requerido" 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                          <asp:RegularExpressionValidator ID="regxRFC6" runat="server" 
                                                              ControlToValidate="tbRFCTerceros" CssClass="failureNotification" 
                                                              Height="22px" ToolTip="RFC incompleto" 
                                                              ValidationExpression="^([A-ZÑ\s]{3,4})\d{6}([A-Zñ\w]{3})$" 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RegularExpressionValidator>
                                                      </td>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtnombreT" runat="server" CssClass="fontText" TabIndex="3" 
                                                              Width="150px" Visible="False"></asp:TextBox>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" class="style2">
                                                          <asp:Label ID="Label101" runat="server" CssClass="fontText" 
                                                              Text="Impuesto Retención"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          </td>
                                                      <td align="left">
                                                          <asp:Label ID="Label100" runat="server" CssClass="fontText" 
                                                              Text="Importe Retención"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:CheckBox ID="cbIVARet" runat="server" Checked="True" TabIndex="4" 
                                                              Text="IVA" />
                                                      </td>
                                                      <td align="left">
                                                          </td>
                                                      <td align="left">
                                                          <asp:TextBox ID="tbimporterIVARet" runat="server" CssClass="fontText" 
                                                              TabIndex="5" Width="150px">0</asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfImpIvaRetReq" runat="server" 
                                                              ControlToValidate="tbimporterIVARet" CssClass="failureNotification" 
                                                              ErrorMessage="Importe Requerido" Height="22px" ToolTip="Importe requerido." 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:CheckBox ID="cbISRRet" runat="server" TabIndex="6" Text="ISR" />
                                                      </td>
                                                      <td align="left">
                                                         </td>
                                                      <td align="left">
                                                          <asp:TextBox ID="tbimporteISRRet" runat="server" CssClass="fontText" 
                                                              TabIndex="7" Width="150px">0</asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfImpRetISR" runat="server" 
                                                              ControlToValidate="tbimporteISRRet" CssClass="failureNotification" 
                                                              ErrorMessage="Importe Requerido" Height="22px" ToolTip="Importe requerido." 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:Label ID="Label154" runat="server" CssClass="fontText" 
                                                              Text="Impuesto Traslado"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          </td>
                                                      <td align="left">
                                                          <asp:Label ID="Label153" runat="server" CssClass="fontText" 
                                                              Text="Importe Traslado"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          <asp:Label ID="Label155" runat="server" CssClass="fontText" Text="Tasa"></asp:Label>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:CheckBox ID="cbIVATra" runat="server" Checked="True" TabIndex="8" 
                                                              Text="IVA" />
                                                      </td>
                                                      <td align="left">
                                                          </td>
                                                      <td align="left">
                                                          <asp:TextBox ID="tbimporteIVATra" runat="server" CssClass="fontText" 
                                                              TabIndex="9" Width="150px">0</asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfImpIvaTraReq" runat="server" 
                                                              ControlToValidate="tbimporteIVATra" CssClass="failureNotification" 
                                                              ErrorMessage="Importe Requerido" Height="22px" ToolTip="Importe requerido." 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          <asp:TextBox ID="tbtasaIVATra" runat="server" CssClass="fontText" TabIndex="10" 
                                                              Width="75px">0</asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvftasaIvaReq" runat="server" 
                                                              ControlToValidate="tbtasaIVATra" CssClass="failureNotification" 
                                                              ErrorMessage="Tasa requerida" Height="22px" ToolTip="Tasa requerida." 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:CheckBox ID="cbIEPSTra" runat="server" TabIndex="11" Text="IEPS" />
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:TextBox ID="tbimporteIEPSTra" runat="server" CssClass="fontText" 
                                                              TabIndex="12" Width="150px">0</asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfImpIEPSTraReq" runat="server" 
                                                              ControlToValidate="tbimporteIEPSTra" CssClass="failureNotification" 
                                                              ErrorMessage="Importe Requerido" Height="22px" ToolTip="Importe requerido." 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          <asp:TextBox ID="tbtasaIEPSTra" runat="server" CssClass="fontText" 
                                                              TabIndex="13" Width="75px">0</asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvftasaIEPSReq" runat="server" 
                                                              ControlToValidate="tbtasaIEPSTra" CssClass="failureNotification" 
                                                              ErrorMessage="Tasa requerida" Height="22px" ToolTip="Tasa requerida." 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                              </table>
                                          </fieldset>
                                          <fieldset class="register" style="width:560px">
                                              <legend>
                                                  <asp:Literal ID="Literal5" runat="server" Text="Información Aduanera"></asp:Literal>
                                              </legend>
                                              <table>
                                                  <tr>
                                                      <td align="left" class="style2">
                                                          <asp:Label ID="Label133" runat="server" CssClass="fontText" Text="Número"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:Label ID="Label134" runat="server" CssClass="fontText" Text="Fecha"></asp:Label>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtnumeroT" runat="server" TabIndex="14"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon24" runat="server" 
                                                              ControlToValidate="txtnumeroT" CssClass="failureNotification" 
                                                              ErrorMessage="Número requerido" Height="22px" ToolTip="Número requerido" 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtFechaIniT" runat="server" BackColor="White" 
                                                              CssClass="fontText" TabIndex="15" Width="150px"></asp:TextBox>
                                                          <cc1:CalendarExtender ID="txtFechaIniT_CalendarExtender" runat="server" 
                                                              Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgIni" 
                                                              TargetControlID="txtFechaIniT">
                                                          </cc1:CalendarExtender>
                                                          <asp:Image ID="imgIni0" runat="server" 
                                                              ImageUrl="~/Imagenes/icono_calendario.gif" />
                                                          <asp:RegularExpressionValidator ID="revFechaIni0" runat="server" 
                                                              ControlToValidate="txtFechaIniT" CssClass="failureNotification" 
                                                              Display="Dynamic" Height="16px" 
                                                              ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                                              ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RegularExpressionValidator>
                                                          <asp:RequiredFieldValidator ID="rfvFechaIni4" runat="server" 
                                                              ControlToValidate="txtFechaIniT" CssClass="failureNotification" Height="16px" 
                                                              ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" class="style2">
                                                          <asp:Label ID="Label135" runat="server" CssClass="fontText" Text="Aduana"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtaduanaT" runat="server" TabIndex="16"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon25" runat="server" 
                                                              ControlToValidate="txtaduanaT" CssClass="failureNotification" 
                                                              ErrorMessage="Aduana Requerida" Height="22px" ToolTip="Aduana requerida." 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                              </table>
                                          </fieldset>
                                          <fieldset class="register" style="width:560px; height:102px;">
                                              <legend>
                                                  <asp:Literal ID="Literal11" runat="server" Text="Cuenta Predial"></asp:Literal>
                                              </legend>
                                              <table>
                                                  <tr>
                                                      <td align="left" class="style2">
                                                          <asp:Label ID="Label167" runat="server" CssClass="fontText" Text="Número"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" valign="top">
                                                          <asp:TextBox ID="txtPredialT" runat="server" Width="150px" TabIndex="17"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon29" runat="server" 
                                                              ControlToValidate="txtPredialT" CssClass="failureNotification" 
                                                              ErrorMessage="Cuenta requerida" Height="22px" ToolTip="Cuenta requerida" 
                                                              Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" class="style2">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" class="style2">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                              </table>
                                          </fieldset>
                                          </td>
                                      <td>
                                          </td>
                                      <td valign="top">
                                          <fieldset class="register" style="height: 240px; width: 600px">
                                              <legend>
                                                  <asp:Literal ID="Literal10" runat="server" Text="Información Fiscal Tercero"></asp:Literal>
                                              </legend>
                                              <table style="width: 577px">
                                                  <tr>
                                                      <td align="left">
                                                          <asp:Label ID="Label157" runat="server" CssClass="fontText" Text="Calle"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:Label ID="Label158" runat="server" CssClass="fontText" Text="Referencia"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:Label ID="Label163" runat="server" CssClass="fontText" Text="Colonia"></asp:Label>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtcalleT" runat="server" TabIndex="18"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon14" runat="server" 
                                                              ControlToValidate="txtcalleT" CssClass="failureNotification" 
                                                              ErrorMessage="Calle Requerida" Height="22px" ToolTip="Calle requerida." 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtReferenciaT" runat="server" TabIndex="21"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon16" runat="server" 
                                                              ControlToValidate="txtReferenciaT" CssClass="failureNotification" 
                                                              ErrorMessage="Referencia Requerida" Height="22px" 
                                                              ToolTip="Referencia requerida." ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtColoniaT" runat="server" TabIndex="25"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon17" runat="server" 
                                                              ControlToValidate="txtColoniaT" CssClass="failureNotification" 
                                                              ErrorMessage="Colonia Requerida" Height="22px" ToolTip="Colonia requerida." 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:Label ID="Label159" runat="server" CssClass="fontText" 
                                                              Text="Número Exterior"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:Label ID="Label160" runat="server" CssClass="fontText" Text="Municipio"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:Label ID="Label165" runat="server" CssClass="fontText" Text="Localidad"></asp:Label>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtNumExtT" runat="server" TabIndex="19"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon18" runat="server" 
                                                              ControlToValidate="txtNumExtT" CssClass="failureNotification" 
                                                              ErrorMessage="Numero Requerido" Height="22px" ToolTip="Numero requerido" 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtMunicipioT" runat="server" TabIndex="22"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon19" runat="server" 
                                                              ControlToValidate="txtMunicipioT" CssClass="failureNotification" 
                                                              ErrorMessage="Municipio requerido" Height="22px" ToolTip="Municipio requerido" 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtLocalidadT" runat="server" TabIndex="26"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon20" runat="server" 
                                                              ControlToValidate="txtLocalidadT" CssClass="failureNotification" 
                                                              ErrorMessage="Localidad Requerida" Height="22px" ToolTip="Localidad Requerida" 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:Label ID="Label161" runat="server" CssClass="fontText" 
                                                              Text="Número Interior"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;
                                                          <br />
                                                      </td>
                                                      <td align="left">
                                                          <asp:Label ID="Label162" runat="server" CssClass="fontText" Text="Estado"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:Label ID="Label166" runat="server" CssClass="fontText" 
                                                              Text="Código Postal"></asp:Label>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtNumIntT" runat="server" TabIndex="20"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon21" runat="server" 
                                                              ControlToValidate="txtNumIntT" CssClass="failureNotification" 
                                                              ErrorMessage="Numero Requerido" Height="22px" ToolTip="Numero Requerido" 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtEstadoT" runat="server" TabIndex="23"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon22" runat="server" 
                                                              ControlToValidate="txtEstadoT" CssClass="failureNotification" 
                                                              ErrorMessage="Estado requerido" Height="22px" ToolTip="Estado requerido" 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtCodigoT" runat="server" TabIndex="27" MaxLength="5"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rfvCodigoPostal" runat="server" 
                                                              ControlToValidate="txtCodigoT" CssClass="failureNotification" Display="Dynamic" 
                                                              Height="22px" ToolTip="<%$ Resources:resCorpusCFDIEs, valCodigoPostal %>" 
                                                              ValidationGroup="TercerosGuardar"><img 
                                                              src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                                      </td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:Label ID="Label164" runat="server" CssClass="fontText" Text="País"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtPaisT" runat="server" TabIndex="24"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon23" runat="server" 
                                                              ControlToValidate="txtPaisT" CssClass="failureNotification" 
                                                              ErrorMessage="Pais requerido" Height="22px" ToolTip="Pais requerido" 
                                                              ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                              </table>
                                          </fieldset>
<%--                                          <fieldset class="register" style="width: 600px; height: 275px;">
                                              <legend>
                                                  <asp:Literal ID="Literal6" runat="server" Text="Parte" Visible="False"></asp:Literal>
                                              </legend>--%>
                                              <table style="visibility:hidden;">
                                                  <tr>
                                                      <td align="left">
                                                          <asp:Label ID="Label136" runat="server" CssClass="fontText" Text="Cantidad" 
                                                              Visible="False"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" valign="top">
                                                          <asp:TextBox ID="txtCantidadT" runat="server" Width="50px" TabIndex="22" 
                                                              Visible="False"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon27" runat="server" 
                                                              ControlToValidate="txtCantidadT" CssClass="failureNotification" 
                                                              ErrorMessage="Cantidad requerida" Height="22px" 
                                                              ToolTip="Cantidad requerida" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" class="style2">
                                                          <asp:Label ID="Label138" runat="server" CssClass="fontText" Text="Descripción" 
                                                              Visible="False"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtDescripcionT" runat="server" Height="58px" 
                                                              TextMode="MultiLine" Width="300px" TabIndex="23" Visible="False"></asp:TextBox>
                                                          <asp:RequiredFieldValidator ID="rvfNoAutDon28" runat="server" 
                                                              ControlToValidate="txtDescripcionT" CssClass="failureNotification" 
                                                              ErrorMessage="Descripción requerida" Height="22px" 
                                                              ToolTip="Descripción requerida" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                  </tr>
                                              </table>
                                      <%--    </fieldset>--%>
                                          </td>
                                  </tr>
                                  <tr>
                                      <td class="style6" valign="top">
                                          &nbsp;</td>
                                      <td>
                                          &nbsp;</td>
                                      <td align="right">
                                          <asp:Button ID="btnTerceros2" runat="server" CssClass="botonEstilo" 
                                              OnClick="btnTerceros_Click" TabIndex="28" 
                                              Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                                              ValidationGroup="TercerosGuardar" Height="30px" Width="80px" 
                                              OnClientClick="enableValidador();" />
                                          &nbsp; &nbsp;
                                          <asp:Button ID="btnCerrar" runat="server" CssClass="botonGrande" TabIndex="29" 
                                              Text="Cerrar Página" Height="30px" Width="150px" />
                                      </td>
                                      <td align="right" valign="top">
                                          &nbsp;</td>
                                  </tr>
                              </table>
                          </asp:Panel>  
    </form>
</body>
</html>

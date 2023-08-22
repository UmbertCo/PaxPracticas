<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webComplementosDonatarias.aspx.cs" Inherits="Timbrado_Complementos_webComplementosDonatarias" %>
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

        .fontText
        {}
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
<table style="height: 124px; width: 760px"><tr><td>
                                          <fieldset class="register" 
        style="width: 800px; height: 300px;">
                                              <legend>
                                                  <asp:Literal ID="Literal6" runat="server" Text="Donatarias"></asp:Literal>
                                              </legend>
                                              <table>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:Label ID="Label2" runat="server" CssClass="fontText" 
                                          Text="<%$ Resources:resCorpusCFDIEs, lblVersion %>"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:Label ID="Label4" runat="server" CssClass="fontText" 
                                          Text="<%$ Resources:resCorpusCFDIEs, lblFecha %>"></asp:Label></td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" valign="top">
                                                          <asp:TextBox ID="tbVersionDon" runat="server" CssClass="fontText" 
                                              ReadOnly="True" TabIndex="1" Width="50px"></asp:TextBox>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtFechaIni" runat="server" BackColor="White" 
                                              CssClass="fontText" TabIndex="3" Width="150px"></asp:TextBox><cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" 
                                              Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgIni" 
                                              TargetControlID="txtFechaIni"></cc1:CalendarExtender><asp:Image ID="imgIni" runat="server" 
                                              ImageUrl="~/Imagenes/icono_calendario.gif" /><asp:RegularExpressionValidator ID="revFechaIni" runat="server" 
                                              ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                              Display="Dynamic" Height="16px" 
                                              ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                              ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                              ValidationGroup="DonativasGuardar" Width="16px">*</asp:RegularExpressionValidator><asp:RequiredFieldValidator ID="rfvFechaIni" runat="server" 
                                              ControlToValidate="txtFechaIni" CssClass="failureNotification" Height="16px" 
                                              ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" 
                                              ValidationGroup="DonativasGuardar" Width="16px">*</asp:RequiredFieldValidator></td>
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
                                                      <td align="left">
                                                          <asp:Label ID="Label3" runat="server" CssClass="fontText" 
                                              Text="<%$ Resources:resCorpusCFDIEs, lblAutorizacion %>" Width="98px" Height="38px"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:Label ID="Label5" runat="server" CssClass="fontText" 
                                              Text="<%$ Resources:resCorpusCFDIEs, lblLeyendaDonativa %>"></asp:Label></td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" valign="top">
                                                          <asp:TextBox ID="tbAutorizacionDon" runat="server" CssClass="fontText" 
                                              TabIndex="2" Width="300px"></asp:TextBox><asp:RequiredFieldValidator ID="rvfNoAutDon" runat="server" 
                                              ControlToValidate="tbAutorizacionDon" CssClass="failureNotification" 
                                              ErrorMessage="No. Autorización requerido" 
                                              ToolTip="<%$ Resources:resCorpusCFDIEs, rfvCodigo %>" 
                                              ValidationGroup="DonativasGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left" rowspan="3">
                                                          <asp:TextBox ID="tbLeyendaDon" runat="server" CssClass="fontText" Height="85px" 
                                              TabIndex="4" TextMode="MultiLine" Width="400px"></asp:TextBox><asp:RequiredFieldValidator ID="rvftbLeyendaDon" runat="server" 
                                              ControlToValidate="tbLeyendaDon" CssClass="failureNotification" 
                                              ErrorMessage="No. Autorización requerido" 
                                              ToolTip="<%$ Resources:resCorpusCFDIEs, rfvCodigo %>" 
                                              ValidationGroup="DonativasGuardar" Width="16px">*</asp:RequiredFieldValidator></td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" valign="top">
                                                          Donativo</td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <%--<td align="left">
                                                          &nbsp;</td>--%>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" valign="top">
                                                        <asp:DropDownList ID="ddlTipoDonativo" runat="server" 
                                                                CssClass="fontText" TabIndex="20" 
                                                                Width="300px">
                                                        </asp:DropDownList></td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <%--<td align="left">
                                                          &nbsp;</td>--%>
                                                  </tr>
                                              </table>
                                          </fieldset></td><td>&nbsp;</td><td>&nbsp;</td></tr><tr>
    <td align="right" valign="top"><asp:Button ID="btnDonativas" runat="server" CssClass="botonEstilo" 
                                              OnClick="btnDonativas_Click" TabIndex="5" 
                                              Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                                              ValidationGroup="DonativasGuardar" 
            Height="37px" Width="80px" />&nbsp;&nbsp;&nbsp;
                                          <asp:Button ID="btnCerrar" runat="server" 
            CssClass="botonGrande" TabIndex="6" 
                                              Text="Cerrar Página" Height="35px" 
            Width="150px" />
                                      </td><td></td><td>&nbsp;</td></tr></table>
    </form>
</body>
</html>

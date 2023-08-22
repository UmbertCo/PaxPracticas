<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webComplementoLeyendasFiscales.aspx.cs" Inherits="Timbrado_Complementos_webComplementoLeyendasFiscales" ValidateRequest="false" %>
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

        .style2
        {
            height: 23px;
        }
    </style>
</head>
<body style="background-color: #FFFFFF">


    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <link href="../../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/menu_style.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />

    <%------------------------JQuery----------------------------%>
    <script src='<%= ResolveUrl("~/Scripts/jquery-1.11.3.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/Scripts/jquery-1.11.3.min.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/Scripts/alerts/jquery.alerts.js") %>' type="text/javascript"></script>
    <%------------------------JQuery----------------------------%>

    <table style="height: 124px; width: 831px"><tr><td>
                                          <fieldset class="register" 
            style="width: 800px; height: 300px;">
                                              <legend>
                                                  <asp:Literal ID="Literal6" runat="server" Text="Leyendas Fiscales"></asp:Literal>
                                              </legend>
                                              <table>
                                                  <tr>
                                                      <td align="left">
                                                          <asp:Label ID="Label93" runat="server" CssClass="fontText" 
                                          Text="<%$ Resources:resCorpusCFDIEs, lblVersion %>"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:Label ID="Label94" runat="server" CssClass="fontText" Text="Norma"></asp:Label></td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" valign="top">
                                                          <asp:TextBox ID="tbVersionLeyFis" runat="server" CssClass="fontText" 
                                              ReadOnly="True" TabIndex="1" Width="50px">1.0</asp:TextBox>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:TextBox ID="txtnormaLeyFis" runat="server" BackColor="White" 
                                          CssClass="fontText" TabIndex="3" Width="150px"></asp:TextBox>
                                          <cc1:FilteredTextBoxExtender ID="fenormaLeyFis" runat="server" Enabled="true" TargetControlID="txtnormaLeyFis"
                                   FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" FilterMode="ValidChars" ValidChars="á,é,ú,í,ó,ñ,,,, ,.,:,;,?,¿,!,¡,+,-,*,/,_,#,$,%,(,),[,],{,}">
                                </cc1:FilteredTextBoxExtender>
<asp:RequiredFieldValidator ID="rfvFechaIni3" runat="server" 
                                          ControlToValidate="txtnormaLeyFis" CssClass="failureNotification" 
                                          ToolTip="La norma es requerida" ValidationGroup="leyendafiscalGuardar"></asp:RequiredFieldValidator></td>
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
                                                          <asp:Label ID="Label95" runat="server" CssClass="fontText" 
                                          Text="Disposición Fiscal"></asp:Label>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:Label ID="Label73" runat="server" CssClass="fontText" 
                                          Text="<%$ Resources:resCorpusCFDIEs, lblLeyendaDonativa %>"></asp:Label></td>
                                                  </tr>
                                                  <tr>
                                                      <td align="left" valign="top">
                                                          <asp:TextBox ID="tbDiposicionFiscal" runat="server" CssClass="fontText" 
                                          TabIndex="2" Width="300px"></asp:TextBox>
                                          <cc1:FilteredTextBoxExtender ID="feDiposicionFiscal" runat="server" Enabled="true"
                                    TargetControlID="tbDiposicionFiscal" FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" FilterMode="ValidChars" ValidChars="á,é,ú,í,ó,ñ,,,, ,.,:,;,?,¿,!,¡,+,-,*,/,_,#,$,%,(,),[,],{,}">
                                </cc1:FilteredTextBoxExtender>
<asp:RequiredFieldValidator ID="rvfNoAutDon0" runat="server" 
                                          ControlToValidate="tbDiposicionFiscal" CssClass="failureNotification" 
                                          ErrorMessage="No. Autorización requerido" Height="22px" 
                                          ToolTip="<%$ Resources:resCorpusCFDIEs, rfvCodigo %>" 
                                          ValidationGroup="leyendafiscalGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                      </td>
                                                      <td align="left">
                                                          &nbsp;</td>
                                                      <td align="left">
                                                          <asp:TextBox ID="tbLeyendaFis" runat="server" CssClass="fontText" Height="85px" 
                                          TabIndex="4" TextMode="MultiLine" Width="400px"></asp:TextBox>
                                          <cc1:FilteredTextBoxExtender ID="feLeyendaFis" runat="server" Enabled="true"
                                    TargetControlID="tbLeyendaFis" FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" FilterMode="ValidChars" ValidChars="á,é,ú,í,ó,ñ,,,, ,.,:,;,?,¿,!,¡,+,-,*,/,_,#,$,%,(,),[,],{,}">
                                </cc1:FilteredTextBoxExtender>
<asp:RequiredFieldValidator ID="rvftbLeyendaDon0" runat="server" 
                                          ControlToValidate="tbLeyendaFis" CssClass="failureNotification" 
                                          ErrorMessage="Leyenda Requerida" Height="22px" ToolTip="Leyenda Requerida" 
                                          ValidationGroup="leyendafiscalGuardar" Width="16px">*</asp:RequiredFieldValidator></td>
                                                  </tr>
                                              </table>
                                          </fieldset></td><td></td><td>&nbsp;</td></tr><tr><td></td><td></td><td align="right"></td></tr><tr>
        <td align="right"><asp:Button ID="btnLFicales" runat="server" CssClass="botonEstilo" 
                                          OnClick="btnLFicales_Click" TabIndex="5" 
                                          Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                                          ValidationGroup="leyendafiscalGuardar" 
                Height="30px" Width="80px" />&nbsp;&nbsp;&nbsp;&nbsp;
                                          <asp:Button ID="btnCerrar" runat="server" 
                CssClass="botonGrande" TabIndex="6" 
                                              Text="Cerrar Página" Height="30px" 
                Width="150px" />
                                      </td><td></td><td align="right">&nbsp;</td></tr></table>
    </form>
</body>
</html>

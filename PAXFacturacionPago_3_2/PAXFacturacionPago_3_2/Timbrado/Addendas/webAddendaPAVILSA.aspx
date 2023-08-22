<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webAddendaPAVILSA.aspx.cs" Inherits="Timbrado_Addendas_webAddendaPAVILSA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">

        .fontText
        {
            font: 8pt verdana;
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
    <div>
    <table>
        <tr>
            <td>
          <fieldset class="register" style="width:700px;">
       <legend>
         <asp:Literal ID="Literal2" runat="server" 
             Text="ADENDA PAVILSA" />
         </legend>
     <asp:Panel runat="server" ID="pnlAddenda">
    <table>
        <tr>
            <td align="right">
            
                              <asp:Label ID="lblMoneda" runat="server" CssClass="fontText" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblMoneda %>"></asp:Label>
            
            </td>
            <td>
            
                                            <asp:DropDownList ID="ddlMonedaArt" runat="server" CssClass="fontText" 
                                                TabIndex="1" Width="63px">
                                                <asp:ListItem Selected="True">MXN</asp:ListItem>
                                                <asp:ListItem>USD</asp:ListItem>
                                                <asp:ListItem>EUR</asp:ListItem>
                                            </asp:DropDownList>
            
            </td>
            <td>
            
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
            
                <asp:Label ID="lblNoCompañia" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNoCompañia %>" CssClass="fontText"></asp:Label>
            
            </td>
            <td>
            
                <asp:TextBox ID="txtNoComp" runat="server" CssClass="fontText" TabIndex="2"></asp:TextBox>
            
                <asp:RequiredFieldValidator ID="rfNoComp" runat="server" 
                    ControlToValidate="txtNoComp" CssClass="failureNotification" Display="Dynamic" 
                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                    ValidationGroup="validaAdenda"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
            
            </td>
            <td>
            
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
            
                <asp:Label ID="lblNoProveedor" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNoProveedor %>" CssClass="fontText"></asp:Label>
            
            </td>
            <td>
            
                <asp:TextBox ID="txtNoProv" runat="server" CssClass="fontText" TabIndex="3"></asp:TextBox>
            
                <asp:RequiredFieldValidator ID="rfNoProv" runat="server" 
                    ControlToValidate="txtNoProv" CssClass="failureNotification" 
                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                    ValidationGroup="validaAdenda"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
            
            </td>
            <td>
            
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
            
                <asp:Label ID="lblNoOrdenCompra" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNoOrdenCompra %>" 
                    CssClass="fontText"></asp:Label>
            
            </td>
            <td>
            
                <asp:TextBox ID="txtOrdCom" runat="server" CssClass="fontText" TabIndex="4" 
                    Width="500px"></asp:TextBox>
            
            </td>
            <td>
            
                <asp:RequiredFieldValidator ID="rfOrdCom" runat="server" 
                    ControlToValidate="txtOrdCom" CssClass="failureNotification" 
                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                    ValidationGroup="validaAdenda"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
            
                &nbsp;</td>
            <td>
            
                <asp:Label ID="lblOrdenEje" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblOrdenEje %>" 
                    CssClass="fontText"></asp:Label>
            
            </td>
            <td>
            
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
            
                <asp:Label ID="lblSubTotal" runat="server" Text="SubTotal $" CssClass="fontText"></asp:Label>
            
            </td>
            <td>
            
                <asp:TextBox ID="txtSubTot" runat="server" BorderStyle="None" 
                    CssClass="fontText" ReadOnly="True" TabIndex="5"></asp:TextBox>
            
            </td>
            <td>
            
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
            
                <asp:Label ID="lblTotal" runat="server" Text="Total $" CssClass="fontText"></asp:Label>
            
            </td>
            <td>
            
                <asp:TextBox ID="txtTotal" runat="server" BorderStyle="None" 
                    CssClass="fontText" ReadOnly="True" TabIndex="6"></asp:TextBox>
            
            </td>
            <td>
            
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

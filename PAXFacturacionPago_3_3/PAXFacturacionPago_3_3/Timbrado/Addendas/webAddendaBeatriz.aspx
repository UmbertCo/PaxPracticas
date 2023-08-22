<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webAddendaBeatriz.aspx.cs" Inherits="Timbrado_Addendas_webAddendaBeatriz" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <%------------------------Bootstrap----------------------------%>
    <script src='<%# ResolveUrl ("~/Scripts/jquery-2.2.4.js") %>' type="text/javascript"></script>
    <script src='<%# ResolveUrl ("~/Scripts/bootstrap.js") %>' type="text/javascript"></script>
    <script src='<%# ResolveUrl ("~/Scripts/bootstrap-colorpicker.js") %>' type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootbox.js/4.3.0/bootbox.min.js" type="text/javascript"> </script>
    <link href="~/Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/webMaster_Style.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap-colorpicker.css" rel="stylesheet"/>
    <script src='<%# ResolveUrl ("~/Scripts/JSPax.js") %>' type="text/javascript"></script>
    <%------------------------JQuery----------------------------%>
    <script src='<%# ResolveUrl ("~/Scripts/jquery.ui.core.js") %>' type="text/javascript"></script>
    <link href="Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <%------------------------JQuery----------------------------%>
    <link href="~/Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/menu_style.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Dynamic_Style.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"> </asp:ScriptManager>
        <div class="container">
             <div class="well">
                 <nav class="navbar navbar-default" role="navigation">
                              <div class="navbar-header">
                                <a class="TituloInicio" href="#"><asp:Literal ID="lblTitulo" runat="server" Text="ADDENDA CARGO EXPRESS" /></a>
                              </div>
                            </nav>
                 <div class="panel-body">
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:label id="idFechEmb" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechEmbar %>"></asp:label>
                                <asp:TextBox ID="txtFechaGuia" runat="server" CssClass="form-control" 
                                    TabIndex="1" ></asp:TextBox>
                                <asp:Image ID="imgGuia" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                    <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" 
                                        Enabled="True" TargetControlID="txtFechaGuia" Format="dd/MM/yyyy" 
                                        PopupButtonID="imgGuia"></cc1:CalendarExtender>
                                <asp:RegularExpressionValidator ID="revFechaguia" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaGuia" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                <span class="tooltiptext"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" /></span>
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvFechaguia" runat="server" 
                                    ControlToValidate="txtFechaGuia" Display="Dynamic" 
                                    ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                    ValidationGroup="validaAdenda">
                                    <span class="tooltiptext"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valValor %>" /></span>
                                    </asp:RequiredFieldValidator>
                            </div>
                        </div>
                     </div>
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblFechPago" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblFechpago %>"></asp:Label>
                                <asp:TextBox ID="txtFechaPago" runat="server" CssClass="form-control" 
                                    TabIndex="2"></asp:TextBox>
                                <asp:Image ID="imgpago" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaPago" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgpago"></cc1:CalendarExtender>
                                <asp:RegularExpressionValidator ID="revFechapago" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaPago" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                <span class="tooltiptext"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" /></span>
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvFechapago" runat="server" 
                                    ControlToValidate="txtFechaPago" Display="Dynamic" 
                                    ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                    ValidationGroup="validaAdenda">
                                    <span class="tooltiptext"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valValor %>" /></span>
                                    </asp:RequiredFieldValidator>
                            </div>
                        </div>
                     </div>
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblNoguia" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblNoGuia %>"></asp:Label>
                                <asp:TextBox ID="txtNoGuia" runat="server" TabIndex="3" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                     </div>
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblOrdCom" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblOrdComp %>"></asp:Label>
                                <asp:TextBox ID="txtOrdenCompra" runat="server" TabIndex="4" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                     </div>
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                 <asp:Label ID="lblOrigen" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblOri %>"></asp:Label>
                                <asp:TextBox ID="txtOrigen" runat="server" TabIndex="5" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="txtOrigen" Display="Dynamic" 
                                    ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                    ValidationGroup="validaAdenda">
                                    <span class="tooltiptext"><asp:Literal ID="Literal5" runat="server" Text="El campo origen requerido." /></span>
                                    </asp:RequiredFieldValidator>
                            </div>
                        </div>
                     </div>
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblDest" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblDest %>"></asp:Label>
                                <asp:TextBox ID="txtDestino" runat="server" TabIndex="6" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                    ControlToValidate="txtDestino" Display="Dynamic" 
                                    ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                    ValidationGroup="validaAdenda">
                                    <span class="tooltiptext"><asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valValor %>" /></span>
                                    </asp:RequiredFieldValidator>
                            </div>
                        </div>
                     </div>
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblUni" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblUnid %>"></asp:Label>
                                <asp:TextBox ID="txtUnidad" runat="server" TabIndex="7" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                    ControlToValidate="txtUnidad" Display="Dynamic" 
                                    ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                    ValidationGroup="validaAdenda">
                                    <span class="tooltiptext"><asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valValor %>" /></span>
                                    </asp:RequiredFieldValidator>
                            </div>
                        </div>
                     </div>
                     <div class="row pull-right">
                         <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary-red" 
                            TabIndex="8" Text="<%$ Resources:resCorpusCFDIEs, btnGuarAddenda %>"
                            ValidationGroup="validaAdenda" onclick="btnGuardar_Click"/>
                        <asp:Button ID="btnCerrar" runat="server" CssClass="btn btn-primary-red" 
                            TabIndex="9" Text="<%$ Resources:resCorpusCFDIEs, btnCerraAddenda %>" />
                     </div>
                 </div>
            </div>
        </div>
    </form>
</body>
</html>

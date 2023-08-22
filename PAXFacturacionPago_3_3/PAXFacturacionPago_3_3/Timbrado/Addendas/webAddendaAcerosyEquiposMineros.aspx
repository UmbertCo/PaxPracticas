<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webAddendaAcerosyEquiposMineros.aspx.cs" Inherits="Timbrado_Addendas_webAddendaAcerosyEquiposMineros" %>

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
    <%--<link href="Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />--%>
    <%------------------------JQuery----------------------------%>
    <link href="~/Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/menu_style.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Dynamic_Style.css" rel="stylesheet" type="text/css" />
    <link href="~/Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
             <div class="well">
                 <nav class="navbar navbar-default" role="navigation">
                              <div class="navbar-header">
                                <a class="TituloInicio" href="#"><asp:Literal ID="lblTitulo" runat="server" Text="ADENDA ACEROS Y EQUIPOS MINEROS" /></a>
                              </div>
                            </nav>
                 <div class="panel-body">
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblNumOrden" runat="server" CssClass ="fontText"
                                Text="Numero de Orden: "></asp:Label>
                                <asp:TextBox ID="txtNumIOrden" runat="server" CssClass="form-control"
                                    TabIndex="1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfNumIOrden" runat="server" 
                                    ControlToValidate="txtNumIOrden" Display="Dynamic" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                    ValidationGroup="validaAdenda"> 
                                    <span class="tooltiptext"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valValor %>" /></span>
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                     </div>
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                 <asp:Label ID="lblCondPago" runat="server" class="fontText" 
                                        Text="Condiciones de pago: "></asp:Label>
                                 <asp:DropDownList ID="ddlCondPago" runat="server" CssClass="form-control" 
                                        TabIndex="2">
                                        <asp:ListItem Selected="True">Contado</asp:ListItem>
                                        <asp:ListItem>Crédito</asp:ListItem>
                                        </asp:DropDownList>
                            </div>
                        </div>
                         <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblDiasdeCredito" runat="server" CssClass="fontText"
                                         Text="Días de Crédito: " ></asp:Label>
                                <asp:DropDownList ID="ddlDiasdeCredito" runat="server" CssClass="form-control" 
                                            TabIndex="3">
                                            <asp:ListItem Selected="True">8</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>30</asp:ListItem>
                                            <asp:ListItem>45</asp:ListItem>
                                        </asp:DropDownList>
                            </div>
                        </div>
                     </div>
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblFechaVencimiento0" runat="server" CssClass="fontText"
                                        Text="Fecha de Vencimiento (dd/mm/yyyy): "></asp:Label>
                                <asp:TextBox ID="txtFechaVencimiento" runat="server" CssClass="form-control">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfFechaVencimiento" runat="server" 
                                            ControlToValidate="txtFechaVencimiento" Display="Dynamic" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                                            ValidationGroup="validaAdenda"> 
                                            <span class="tooltiptext"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valValor %>" /></span>
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revFechaVencimiento" runat="server" Display="Dynamic"
                                            ControlToValidate="txtFechaVencimiento" CssClass="failureNotification" 
                                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                            ValidationGroup="validaAdenda" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                            <span class="tooltiptext"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" /></span>
                                        </asp:RegularExpressionValidator>
                            </div>
                        </div>
                     </div>
                     <div class="row pull-right">
                         <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary-red"
                              TabIndex="7" Text="Guardar Addenda" ValidationGroup="validaAdenda" onclick="btnGuardar_Click"/>
                        <asp:Button ID="btnCerrar" runat="server" CssClass="btn btn-primary-red"
                             TabIndex="8" Text="Cerrar Página"/>
                     </div>
                 </div>
            </div>
        </div>
    </form>
</body>
</html>

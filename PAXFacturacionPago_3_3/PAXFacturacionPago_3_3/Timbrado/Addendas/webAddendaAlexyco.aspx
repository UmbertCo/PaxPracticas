<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webAddendaAlexyco.aspx.cs" Inherits="Timbrado_Addendas_webAddendaAlexyco" %>

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
        <div class="container">
             <div class="well">
                 <nav class="navbar navbar-default" role="navigation">
                              <div class="navbar-header">
                                <a class="TituloInicio" href="#"><asp:Literal ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblAddendaAlexyco %>" /></a>
                              </div>
                            </nav>
                 <div class="panel-body">
                     <div class="row">
                         <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblOrdenDeCompraAlexyco" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblOrdenDeCompraAlexyco %>" ></asp:Label>
                                <asp:TextBox ID="txtOrdenDeCompraAlexyco" runat="server" MaxLength="14" CssClass="form-control"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revOrden" runat="server" 
                                    ControlToValidate="txtOrdenDeCompraAlexyco" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valNumeroDeArchivoAlexyco %>" 
                                    ValidationExpression="^[A-Za-z0-9. ]{0,14}$" 
                                    ValidationGroup="AddendaAlexycoValidationGroup">
                                    <span class="tooltiptext"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valNumeroDeArchivoAlexyco %>" /></span>
                                </asp:RegularExpressionValidator>
                            </div>
                         </div>
                         <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblRamaCentroAlexyco" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRamaCentroAlexyco %>"></asp:Label>
                                <asp:TextBox ID="txtRamaCentroAlexyco" runat="server" MaxLength="7" CssClass="form-control"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revRama" runat="server" 
                                    ControlToValidate="txtRamaCentroAlexyco" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valNumeroDeArchivoAlexyco %>" 
                                    ValidationExpression="^[A-Za-z0-9. ]{0,7}$" 
                                    ValidationGroup="AddendaAlexycoValidationGroup">
                                    <span class="tooltiptext"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valNumeroDeArchivoAlexyco %>" /></span>
                                </asp:RegularExpressionValidator>
                            </div>
                         </div>
                     </div>
                     <div class="row">
                         <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblNumeroDeArchivoAlexyco" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNumeroDeArchivoAlexyco %>"></asp:Label>
                                <asp:TextBox ID="txtNumeroDeArchivoAlexyco" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revNumeroDeArchivoAlexyco" runat="server" 
                                    ControlToValidate="txtNumeroDeArchivoAlexyco" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valNumeroDeArchivoAlexyco %>" 
                                    ValidationExpression="^[0-9]{1,16}$" 
                                    ValidationGroup="AddendaAlexycoValidationGroup">
                                    <span class="tooltiptext"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valNumeroDeArchivoAlexyco %>" /></span>
                                </asp:RegularExpressionValidator>
                            </div>
                        </div>
                         <div class="col-lg-5 col-md-5 col-sm-5 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblReferenciaDeTransporteAlexyco" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblReferenciaDeTransporteAlexyco %>"></asp:Label>
                                <asp:TextBox ID="txtReferenciaDeTransporteAlexyco" runat="server" MaxLength="7" CssClass="form-control"></asp:TextBox>
								<asp:RegularExpressionValidator ID="revReferencia" runat="server" 
                                    ControlToValidate="txtReferenciaDeTransporteAlexyco" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valNumeroDeArchivoAlexyco %>" 
                                    ValidationExpression="^[A-Za-z0-9. ]{0,7}$" 
                                    ValidationGroup="AddendaAlexycoValidationGroup">
                                    <span class="tooltiptext"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valNumeroDeArchivoAlexyco %>" /></span>
                                </asp:RegularExpressionValidator>
                            </div>
                        </div>
                     </div>
                     <div class="row pull-right">
                         <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary-red" 
                            TabIndex="7" Text="Guardar Addenda" 
                            ValidationGroup="AddendaAlexycoValidationGroup" 
                            OnClick="btnGuardar_Click"/>
                        <asp:Button ID="btnCerrar" runat="server" CssClass="btn btn-primary-red" 
                            TabIndex="8" Text="Cerrar Página" 
							onclick="btnCerrar_Click" />
                     </div>
                 </div>
            </div>
       </div>
    </form>
</body>
</html>

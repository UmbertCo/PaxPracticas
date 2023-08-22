<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webAddendaAHMSA.aspx.cs" Inherits="Timbrado_Addendas_webAddendaAHMSA" %>

<!DOCTYPE html>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
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
    <%--<script src='<%# ResolveUrl ("~/Scripts/jquery.ui.draggable.js") %>' type="text/javascript"></script>--%>
    <%--<script src='<%# ResolveUrl ("~/Scripts/alerts/jquery.alerts.js") %>' type="text/javascript"></script>--%>
<%--    <link href="~/Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />--%>
    <%------------------------JQuery----------------------------%>
    <link href="~/Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/menu_style.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Dynamic_Style.css" rel="stylesheet" type="text/css" />
    <%--<link href="~/Styles/StyleSheetConfig.css" rel="stylesheet" type="text/css" />--%>
    <link href="~/Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="container">
             <div class="well">
                 <nav class="navbar navbar-default" role="navigation">
                              <div class="navbar-header">
                                <a class="TituloInicio"href="#"><asp:Literal ID="lblTitulo" runat="server" Text="Addenda Altos Hornos de México" /></a>
                              </div>
                            </nav>
                 <div class="panel-body">
                     <asp:UpdatePanel ID="udpDocumento" runat="server">
                        <ContentTemplate>
                             <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                                    <div class="form-group">
                                        <asp:Label ID="lblTipoDocumento" runat="server" Text="Tipo Documento" />
                                        <asp:DropDownList ID="ddlTipoDocumento" runat="server" CssClass="form-control"
                                                    OnSelectedIndexChanged="ddlTipoDocumento_SelectedIndexChanged" 
                                                    AutoPostBack="True"/> 
                                    </div>
                                </div>
                                 <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                                     <div class="form-group">
                                         <asp:Label ID="lblClase" runat="server" Text="Clase" />
                                         <asp:DropDownList ID="ddlClase" runat="server" CssClass="form-control"
                                                    OnSelectedIndexChanged="ddlClase_SelectedIndexChanged" 
                                                    AutoPostBack="True"/>  
                                     </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                                    <div class="form-group">
                                        <asp:Label ID="lblNumSociedad" runat="server" Text="Núm. Sociedad" />
                                        <asp:DropDownList ID="ddlNumSociedad" runat="server" CssClass="form-control"
                                                    onselectedindexchanged="ddlNumSociedad_SelectedIndexChanged" AutoPostBack="true"/>
                                    </div>
                                </div>
                                 <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                                     <div class="form-group">
                                         <asp:Label ID="lblNumDivision" runat="server" Text="Núm. División" />
                                         <asp:DropDownList ID="ddlNumDivision" runat="server" CssClass="form-control"/>
                                     </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                                    <div class="form-group">
                                        <asp:Label ID="lblNumProveedor" runat="server" Text="Núm. Proveedor" />
                                        <asp:TextBox ID="txtNumProveedor" runat="server" MaxLength="10" CssClass="form-control"/>
                                                <asp:RequiredFieldValidator ID="rfvNumProveedor" runat="server"
                                                 ControlToValidate="txtNumProveedor" ValidationGroup="vgAddenda">
                                                  <span class="tooltiptext"><asp:Literal ID="Literal1" runat="server" Text="El numero de proveedor es requerido" /></span> 
                                                 </asp:RequiredFieldValidator>
                                                  <asp:RegularExpressionValidator ID="revNumero" runat="server"
                                                ControlToValidate="txtNumProveedor" Display="Dynamic" 
                                                ToolTip="Debe ser un valor numérico" 
                                                ValidationExpression="[0-9]+" 
                                                ValidationGroup="vgAddenda">
                                                <span class="tooltiptext"><asp:Literal ID="Literal2" runat="server" Text="Debe ser un valor numérico" /></span> 
                                                </asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                 <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                                     <div class="form-group">
                                         <asp:Label ID="lblCorreo" runat="server" Text="Correo Electrónico" />
                                         <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control"/>
                                                <asp:RequiredFieldValidator ID="rvCorreo" runat="server"
                                                 ControlToValidate="txtCorreo" ValidationGroup="vgAddenda" Display="Dynamic" 
                                                ToolTip="El correo es requerido" >
                                                 <span class="tooltiptext"><asp:Literal ID="Literal3" runat="server" Text="El correo es requerido" /></span>
                                                 </asp:RequiredFieldValidator>
                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                ValidationGroup="vgAddenda" Width="131px">
                                                <span class="tooltiptext"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" /></span>
                                                </asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                                    <div class="form-group">
                                        <asp:Label ID="lblMoneda" runat="server" Text="Modena" />
                                        <asp:DropDownList ID="ddlMoneda" runat="server" CssClass="form-control"/>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12"> 
                            <asp:UpdatePanel ID="udpServicios" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12"> 
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lblServicios" Text="Pedidos"/>
                                                <asp:TreeView ID="trvServicios" runat="server" BackColor="White" Height="200px" 
                                                    ImageSet="Arrows" ShowLines="True">
                                                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                    <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" 
                                                        HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                                    <ParentNodeStyle Font-Bold="False" />
                                                    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" 
                                                        HorizontalPadding="0px" VerticalPadding="0px" />
                                                </asp:TreeView>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12"> 
                                            <div class="form-group">
                                                <asp:Label ID="lblNumServicio" runat="server" Text="Núm. Pedido" />
                                                <asp:TextBox ID="txtNumServicio" runat="server" MaxLength="10" CssClass="form-control"/>
                                                <asp:RegularExpressionValidator ID="revNumServicio" runat="server"
                                                    ControlToValidate="txtNumServicio" CssClass="failureNotification" Display="Dynamic" 
                                                    ToolTip="Debe ser un valor numérico" 
                                                    ValidationExpression="[0-9]+" 
                                                    ValidationGroup="vgServicios">
                                                    <span class="tooltiptext"><asp:Literal ID="Literal5" runat="server" Text="Debe ser un valor numérico" /></span>
                                                </asp:RegularExpressionValidator>
                                                <asp:Button ID="btnAgrServicio" runat="server" Text="<<" CssClass="btn btn-primary-red" 
                                                        onclick="btnAgrServicio_Click" ValidationGroup="vgServicios"/>
                                                <asp:Button ID="btnRemServicio" runat="server" Text=">>" CssClass="btn btn-primary-red" 
                                                        onclick="btnRemServicio_Click"/>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblNumRecepcion" runat="server" Text="Núm. Recepción" />
                                                <asp:TextBox ID="txtNumRecepcion" runat="server" MaxLength="10" CssClass="form-control"/>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                    ControlToValidate="txtNumRecepcion" CssClass="failureNotification" Display="Dynamic" 
                                                    ToolTip="Debe ser un valor numérico" 
                                                    ValidationExpression="[0-9]+" 
                                                    ValidationGroup="vgRecepcion">
                                                    <span class="tooltiptext"><asp:Literal ID="Literal6" runat="server" Text="Debe ser un valor numérico" /></span>
                                                </asp:RegularExpressionValidator>
                                                <asp:Button ID="btnAgrRecepcion" runat="server" Text="<<" 
                                                        CssClass="btn btn-primary-red" onclick="btnAgrRecepcion_Click" ValidationGroup="vgRecepcion"/>
                                                <asp:Button ID="btnRemRecepcion" runat="server" Text=">>" 
                                                        CssClass="btn btn-primary-red" onclick="btnRemRecepcion_Click"/>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                         <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12"> 
                              <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12"> 
                                        <div class="form-group">
                                            <asp:Label ID="lblHojaServicio" runat="server" Text="Núm. Hoja de Servicio" /> 
                                            <asp:TextBox ID="txtHojaServicio" runat="server" MaxLength="10" CssClass="form-control"/>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                                ControlToValidate="txtHojaServicio" Display="Dynamic" 
                                                ToolTip="Debe ser un valor numérico" 
                                                ValidationExpression="[0-9]+" 
                                                ValidationGroup="vgRecepcion">
                                                <span class="tooltiptext"><asp:Literal ID="Literal7" runat="server" Text="Debe ser un valor numérico" /></span>
                                            </asp:RegularExpressionValidator>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label ID="lblNumCtaxPag" runat="server" Text="Núm. Cta. Por Pagar" />
                                            <asp:TextBox ID="txtNumCtaxPag" runat="server" MaxLength="10" CssClass="form-control"/>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                                                ControlToValidate="txtNumCtaxPag" Display="Dynamic" 
                                                ToolTip="Debe ser un valor numérico" 
                                                ValidationExpression="[0-9]+" 
                                                ValidationGroup="vgRecepcion">
                                                <span class="tooltiptext"><asp:Literal ID="Literal8" runat="server" Text="Debe ser un valor numérico" /></span>
                                            </asp:RegularExpressionValidator>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label ID="lblFechaIni" runat="server" 
                                                        Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                                        AssociatedControlID="txtFechaIni"></asp:Label>
                                            <asp:TextBox ID="txtFechaIni" runat="server" BackColor="White" CssClass="form-control"></asp:TextBox>
                                            <asp:Image ID="imgIni" runat="server" 
                                                ImageUrl="../../Imagenes/icono_calendario.gif" />
                                            <asp:RegularExpressionValidator ID="revFechaIni" runat="server" Display="Dynamic"
                                                ControlToValidate="txtFechaIni" 
                                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                                ValidationGroup="vgAddenda" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                                <span class="tooltiptext"><asp:Literal ID="Literal9" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" /></span>
                                                </asp:RegularExpressionValidator>
                                            <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" 
                                                Enabled="True" TargetControlID="txtFechaIni" Format="dd/MM/yyyy" 
                                                PopupButtonID="imgIni"></cc1:CalendarExtender>
                                        </div>
                                    </div>
                                  <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12"> 
                                        <div class="form-group">
                                            <asp:Label ID="lblNumTransporte" runat="server" Text="Núm. Transporte" /> 
                                            <asp:TextBox ID="txtNumTransporte" runat="server" MaxLength="10" CssClass="form-control"/>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                                ControlToValidate="txtNumTransporte" Display="Dynamic" 
                                                ToolTip="Debe ser un valor numérico" 
                                                ValidationExpression="[0-9]+" 
                                                ValidationGroup="vgRecepcion">
                                                <span class="tooltiptext"><asp:Literal ID="Literal10" runat="server" Text="Debe ser un valor numérico" /></span>
                                            </asp:RegularExpressionValidator>
                                        </div>
                                      <div class="form-group">
                                          <asp:Label ID="lblEjCtaxPag" runat="server" Text="Ejercicio Cta. Por Pagar" />
                                          <asp:TextBox ID="txtEjCtaxPag" runat="server" MaxLength="4" CssClass="form-control"/>
                                          <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                                ControlToValidate="txtEjCtaxPag" Display="Dynamic" 
                                                ToolTip="Debe ser un valor numérico" 
                                                ValidationExpression="[0-9]+" 
                                                ValidationGroup="vgRecepcion">
                                                <span class="tooltiptext"><asp:Literal ID="Literal11" runat="server" Text="Debe ser un valor numérico" /></span>
                                           </asp:RegularExpressionValidator>
                                        </div>
                                      <div class="form-group">
                                          <asp:Label ID="lblFechaFin" runat="server" 
                                                        Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                                        AssociatedControlID="txtFechaFin"></asp:Label>
                                          <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White" CssClass="form-control"></asp:TextBox>
                                            <asp:Image ID="imgFin" runat="server" 
                                                ImageUrl="../../Imagenes/icono_calendario.gif" />
                                            <asp:RegularExpressionValidator ID="revFechaFin" runat="server" Display="Dynamic"
                                                ControlToValidate="txtFechaFin"
                                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                                ValidationGroup="vgAddenda" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                                <span class="tooltiptext"><asp:Literal ID="Literal12" runat="server" Text="Debe ser un valor numérico" /></span>
                                                </asp:RegularExpressionValidator>
                                            <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" 
                                                Enabled="True" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" 
                                                PopupButtonID="imgFin"></cc1:CalendarExtender>
                                        </div>
                                    </div>
                             </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <div class="form-group">
                                <asp:Label ID="lblAnexos" runat="server" Text="Anexos"/>
                                <asp:TextBox ID="txtAnexo" runat="server" CssClass="form-control"/>
                                <asp:Button ID="btnAgrAnexo" runat="server" Text="Agregar" 
                                    CssClass="btn btn-primary-red" onclick="btnAgrAnexo_Click"/>
                                <asp:Button ID="btnQuitarAnexo" runat="server" Text="Quitar"
                                    CssClass="btn btn-primary-red" onclick="btnQuitarAnexo_Click" />
                                <asp:ListBox ID="lsbAnexos" runat="server" CssClass="form-control"/>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <br />
                        </div>
                    </div>
                     <div class="row pull-right">
                         <div class="form-group">
                                <asp:UpdatePanel ID="udpGuardar" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary-red" 
                                            TabIndex="7" Text="Guardar Addenda" 
                                            ValidationGroup="vgAddenda" onclick="btnGuardar_Click"/>
                                        <asp:Button ID="btnCerrar" runat="server" CssClass="btn btn-primary-red" 
                                            TabIndex="8" Text="Cerrar Página" 
                                            onclick="btnCerrar_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                     </div>
                 </div>
            </div>
        </div>
    </form>
</body>
</html>

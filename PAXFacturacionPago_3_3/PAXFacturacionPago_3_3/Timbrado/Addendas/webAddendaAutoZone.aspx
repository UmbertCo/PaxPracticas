<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webAddendaAutoZone.aspx.cs" Inherits="Timbrado_Addendas_webAddendaAutoZone" %>

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
                                <a class="TituloInicio" href="#"><asp:Literal ID="lblTitulo" runat="server" Text="ADENDA ACEROS Y EQUIPOS MINEROS" /></a>
                              </div>
                            </nav>
                 <div class="panel-body">
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblVersion" runat="server" Text="Versión:"></asp:Label>
                                <asp:TextBox ID="txtVersion" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvVersion" runat="server" 
                                    ControlToValidate="txtVersion"
                                    ValidationGroup="AddendaDataZoneValidationGroup" 
                                    ToolTip="El campo Versión es requerido">
                                    <span class="tooltiptext"><asp:Literal ID="Literal1" runat="server" Text="El campo Versión es requerido" /></span>
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblVendorId" runat="server" Text="Vendor ID:"></asp:Label>
                                <asp:TextBox ID="txtVendorId" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvVendorId" runat="server" 
                                    ControlToValidate="txtVendorId" 
                                    ValidationGroup="AddendaDataZoneValidationGroup" 
                                    ToolTip="El campo Vendor ID es requerido">
                                    <span class="tooltiptext"><asp:Literal ID="Literal2" runat="server" Text="El campo Vendor ID es requerido" /></span>
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblDeptId" runat="server" Text="Dept ID:"></asp:Label>
                                <asp:TextBox ID="txtDeptId" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDeptId" runat="server" 
                                    ControlToValidate="txtDeptId" 
                                    ValidationGroup="AddendaDataZoneValidationGroup" 
                                    ToolTip="El campo Dept ID es requerido">
                                    <span class="tooltiptext"><asp:Literal ID="Literal3" runat="server" Text="El campo Dept ID es requerido" /></span>
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblBuyer" runat="server" Text="Buyer:"></asp:Label>
                                <asp:TextBox ID="txtBuyer" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvBuyer" runat="server" 
                                    ControlToValidate="txtBuyer"
                                    ValidationGroup="AddendaDataZoneValidationGroup" 
                                    ToolTip="El campo Buyer es requerido">
                                    <span class="tooltiptext"><asp:Literal ID="Literal4" runat="server" Text="El campo Buyer es requerido" /></span>
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"> 
                            <div class="form-group">
                                <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                    ControlToValidate="txtEmail" 
                                    ValidationGroup="AddendaDataZoneValidationGroup" 
                                    ToolTip="El campo Email es requerido">
                                    <span class="tooltiptext"><asp:Literal ID="Literal5" runat="server" Text="El campo Email es requerido" /></span>
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                                    ControlToValidate="txtEmail" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                    ValidationGroup="AddendaDataZoneValidationGroup">
                                    <span class="tooltiptext"><asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" /></span>
                                </asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                     <div class="row pull-right">
                         <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary-red" 
                            TabIndex="7" Text="Guardar Addenda" 
                            ValidationGroup="AddendaDataZoneValidationGroup" 
                            OnClick="btnGuardar_Click"/>
                        <asp:Button ID="btnCerrar" runat="server" CssClass="btn btn-primary-red" 
                            TabIndex="8" Text="Cerrar Página"/>
                     </div>
                </div>
            </div>
       </div>
    </form>
</body>
</html>

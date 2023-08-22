<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Addendas.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Addendas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%------------------------JQuery----------------------------%>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <link href="../../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <%------------------------JQuery----------------------------%>
    <link href="../../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/menu_style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" style="background-color: #FFFFFF">
        <div>
            <table>
                <tr>
                    <td>
                        <fieldset class ="register" style="width:500px";>
                            <legend>
                                <asp:Literal ID="LtEncabezadDataZone" runat="server" Text="ADENDA AUTOZONE" />
                            </legend>
                            <asp:Panel ID="pnlAdendaDataZone" runat ="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblVersion" runat="server" Text="Versión:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVersion" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvVersion" runat="server" 
                                                ControlToValidate="txtVersion" CssClass="failureNotification" 
                                                ValidationGroup="AddendaDataZoneValidationGroup" 
                                                ToolTip="El campo Versión es requerido" Width="16px" >
                                                <img alt="" src="Imagenes/error_sign.gif" />
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblVendorId" runat="server" Text="Vendor ID:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVendorId" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvVendorId" runat="server" 
                                                ControlToValidate="txtVendorId" CssClass="failureNotification" 
                                                ValidationGroup="AddendaDataZoneValidationGroup" 
                                                ToolTip="El campo Vendor ID es requerido" Width="16px" >
                                                <img alt="" src="Imagenes/error_sign.gif" />
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDeptId" runat="server" Text="Dept ID:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDeptId" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDeptId" runat="server" 
                                                ControlToValidate="txtDeptId" CssClass="failureNotification" 
                                                ValidationGroup="AddendaDataZoneValidationGroup" 
                                                ToolTip="El campo Dept ID es requerido" Width="16px" >
                                                <img alt="" src="Imagenes/error_sign.gif" />
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBuyer" runat="server" Text="Buyer:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBuyer" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBuyer" runat="server" 
                                                ControlToValidate="txtBuyer" CssClass="failureNotification" 
                                                ValidationGroup="AddendaDataZoneValidationGroup" 
                                                ToolTip="El campo Buyer es requerido" Width="16px" >
                                                <img alt="" src="Imagenes/error_sign.gif" />
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                                ControlToValidate="txtEmail" CssClass="failureNotification" 
                                                ValidationGroup="AddendaDataZoneValidationGroup" 
                                                ToolTip="El campo Email es requerido" Width="16px" >
                                                <img alt="" src="Imagenes/error_sign.gif" />
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                                                ControlToValidate="txtEmail" CssClass="failureNotification" 
                                                Display="Dynamic" ToolTip="Formato de correo incorrecto" 
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                ValidationGroup="AddendaDataZoneValidationGroup" Width="16px">
                                                <img alt="" src="Imagenes/error_sign.gif" />
                                            </asp:RegularExpressionValidator>
                                        </td>
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
                                            Width="150px" ValidationGroup="AddendaDataZoneValidationGroup" 
                                            OnClick="btnGuardar_Click"/>
                                    </td>
                                    <td>
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


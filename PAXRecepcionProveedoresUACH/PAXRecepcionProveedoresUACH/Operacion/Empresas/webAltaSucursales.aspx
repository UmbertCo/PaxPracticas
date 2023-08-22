<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="webAltaSucursales.aspx.cs" Inherits="Operacion_Empresas_webAltaSucursales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <script type="text/javascript">
        var validationText = '<%=Resources.resCorpusCFDIEs.lblEliminarConfirmarFacultad%>';
    </script>
    <link href="../../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <%--~--%>
    <style type="text/css">
        .modal
        {
            padding: 10px 10px 10px 10px;
            border: 1px solid #333333;
            background-color: White;
        }
        .modal p
        {
            width: 600px;
            text-align: right;
        }
        .modal div
        {
            width: 600px;
            vertical-align: top;
        }
        .modal div p
        {
            text-align: left;
        }
        .imagenModal
        {
            height: 15px;
            cursor: pointer;
        }
        .modalBackground
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .textBox
        {
            width: 180px;
        }
        .style4
        {
            width: 239px;
        }
        .style6
        {
            width: 308px;
        }
        .style7
        {
            width: 318px;
        }
    </style>
    <%--<style type="text/css">
        .style2
        {
            width: 81px;
        }
        .style4
        {
            width: 318px;
        }
        .style5
        {
            height: 30px;
        }
    </style>--%>
    <script language="javascript" type="text/javascript">
        function fntraerEnter(keyStroke) {
            isNetscape = (document.layers);
            eventChooser = (isNetscape) ? keyStroke.which : event.keyCode;
            if (eventChooser == 13) {
                return false;
            }
        }
        document.onkeypress = fntraerEnter; 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <br />
    <br />
    <table align="left">
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblProveedorSucursales %>"></asp:Label>
                </h2>
            </td>
        </tr>
    </table>
    <%--style="margin: 0px auto;"--%>
    <table style="margin: 0px auto; width: 983px; border: solid 1px #C0C0C0;" class="bodyMain">
        <%--<link href="Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />--%>
        <tr>
            <td>
                <asp:UpdatePanel ID="udpEmpresas" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <table>
                                        <tr align="left">
                                            <%--<td></td>--%>
                                            <td>
                                                <asp:Label ID="lblNombreEmpresa" runat="server" AssociatedControlID="ddlEmpresas"
                                                    Text="<%$ Resources:resCorpusCFDIEs, lblNombreEmpresa %>" Font-Bold="False"></asp:Label>
                                            </td>
                                            <tr>
                                                <%--<td></td>--%>
                                                <td>
                                                    <asp:DropDownList ID="ddlEmpresas" runat="server" Width="465px" OnSelectedIndexChanged="ddlEmpresas_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <%--<td></td>--%>
                                                <td>
                                                    <asp:Label ID="lblSucursal" runat="server" AssociatedControlID="ddlEmpresas" Text="<%$ Resources:resCorpusCFDIEs, lblSucursal %>"
                                                        Font-Bold="False"></asp:Label>
                                                </td>
                                                <tr>
                                                    <%--<td></td>--%>
                                                    <td>
                                                        <asp:DropDownList ID="ddlSucursales" runat="server" Width="465px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnNuevo" runat="server" CssClass="botonEstilo" Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>"
                                                            OnClick="btnNuevo_Click" Width="80px" TabIndex="0" />
                                                        <asp:Button ID="btnBorrar" runat="server" CssClass="botonEstilo" OnClick="btnBorrar_Click"
                                                            Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" Width="80px" TabIndex="1" OnClientClick="return confirm(validationText)"/>
                                                        <asp:Button ID="btnEditar" runat="server" CssClass="botonEstilo" OnClick="btnEditar_Click"
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" Width="80px" TabIndex="2" />
                                                        <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" OnClick="btnNCancelar_Click"
                                                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" TabIndex="3" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="120px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                
                                            </tr>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td align="left" class="style4">
                                                <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre" Text="<%$ Resources:resCorpusCFDIEs, lblNombre %>"
                                                    Font-Bold="False"></asp:Label>
                                            </td>
                                            <td align="left" class="style7">
                                                <asp:Label ID="lblColonia" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>"
                                                    AssociatedControlID="txtColonia" Font-Bold="False"></asp:Label>
                                            </td>
                                            <td align="left" rowspan="10">
                                                <table>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblingresa" runat="server" Font-Bold="False" Text="<%$ Resources:resCorpusCFDIEs, lblCorreosAcuse %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblcorreo" runat="server" Font-Bold="False" Text="<%$ Resources:resCorpusCFDIEs, lblEmailCorreo %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtCorreo" runat="server" CssClass="textBox" 
                                                                ValidationGroup="RegisterUserValidationGroup"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtCorreo"
                                                                CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>"
                                                                ValidationGroup="CorreoValidadorGroup"><img 
                                                                src="../../Imagenes/error_sign.gif" />
                                                            </asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCorreo"
                                                                CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                                ValidationGroup="CorreoValidadorGroup"><img 
                                                                src="../../Imagenes/error_sign.gif" />
                                                            </asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table align="right">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAgregarMail" runat="server" CssClass="botonEstilo" Height="27px"
                                                                            OnClick="btnAgregarMail_Click" Text="<%$ Resources:resCorpusCFDIEs, lblAgregar %>"
                                                                            ValidationGroup="CorreoValidadorGroup" Width="80px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnQuitarMail" runat="server" CssClass="botonEstilo" Height="27px"
                                                                            OnClick="btnQuitarMail_Click" Text="<%$ Resources:resCorpusCFDIEs, lblQuitar %>"
                                                                            Width="80px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:ListBox ID="lbEmailsAcuses" runat="server" Height="100px" Width="490px"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Button ID="btnGuardarEmpresa" runat="server" CssClass="botonEstilo" Height="27px"
                                                                OnClick="btnGuardarEmpresa_Click" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                                                                ValidationGroup="RegisterUserValidationGroup" Width="80px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <tr>
                                                <td align="left" class="style4">
                                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="textBox" 
                                                        ValidationGroup="RegisterUserValidationGroup"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNombre"
                                                        CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>"
                                                        ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" ValidationGroup="RegisterUserValidationGroup">
                                                        <img src="../../Imagenes/error_sign.gif" /> 
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="style7">
                                                    <asp:TextBox ID="txtColonia" runat="server" CssClass="textBox"></asp:TextBox>
                                                </td>
                                                <tr>
                                                    <td align="left" class="style4">
                                                        <asp:Label ID="lblPais" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"
                                                            AssociatedControlID="ddlPais" Font-Bold="False"></asp:Label>
                                                    </td>
                                                    <td align="left" class="style7">
                                                        <asp:Label ID="lblCalle" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>"
                                                            AssociatedControlID="txtCalle" Font-Bold="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="style4">
                                                        <asp:DropDownList ID="ddlPais" runat="server" AutoPostBack="true" 
                                                            OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" CssClass="textBox">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="left" class="style7">
                                                        <asp:TextBox ID="txtCalle" runat="server" CssClass="textBox" 
                                                            ValidationGroup="RegisterUserValidationGroup"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvCalle" runat="server" ControlToValidate="txtCalle"
                                                            CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>"
                                                            ValidationGroup="RegisterUserValidationGroup" Width="16px"><img
                                                            src="../../Imagenes/error_sign.gif" />
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <tr>
                                                        <td align="left" class="style4">
                                                            <asp:Label ID="lblEstado" runat="server" AssociatedControlID="ddlEstado" Font-Bold="False"
                                                                Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"></asp:Label>
                                                        </td>
                                                        <td align="left" class="style7">
                                                            <asp:Label ID="lblNoExterior" runat="server" AssociatedControlID="txtNoExterior"
                                                                Font-Bold="False" Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="style4">
                                                        <asp:DropDownList ID="ddlEstado" runat="server" AutoPostBack="true" 
                                                            OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" CssClass="textBox">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="left" class="style7">
                                                        <asp:TextBox ID="txtNoExterior" runat="server" CssClass="textBox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="style4">
                                                        <asp:Label ID="lblMunicipio" runat="server" AssociatedControlID="ddlMunicipio" Font-Bold="False"
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>"></asp:Label>
                                                    </td>
                                                    <td align="left" class="style7">
                                                        <asp:Label ID="lblNoInterior" runat="server" AssociatedControlID="txtNoInterior"
                                                            Font-Bold="False" Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="style4">
                                                        <asp:DropDownList ID="ddlMunicipio" runat="server" CssClass="textBox">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="left" class="style7">
                                                        <asp:TextBox ID="txtNoInterior" runat="server" CssClass="textBox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="style4">
                                                        <asp:Label ID="lblLocalidad" runat="server" AssociatedControlID="txtLocalidad" Font-Bold="False"
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>"></asp:Label>
                                                    </td>
                                                    <td align="left" class="style7">
                                                        <asp:Label ID="lblCodigoPostal" runat="server" AssociatedControlID="txtCodigoPostal"
                                                            Font-Bold="False" Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="style4">
                                                        <asp:TextBox ID="txtLocalidad" runat="server" CssClass="textBox"></asp:TextBox>
                                                    </td>
                                                    <td align="left" class="style7">
                                                        <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="textBox" 
                                                            ValidationGroup="RegisterUserValidationGroup"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="revCodigoPostal" runat="server" ControlToValidate="txtCodigoPostal"
                                                            CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, lblCodigoPostalErroneo %>"  ValidationExpression="\d{5}"
                                                            ValidationGroup="RegisterUserValidationGroup">
                                                            <img src="../../Imagenes/error_sign.gif" />
                                                        </asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                            </tr>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        </td>
                        <td align="left">
                        </td>
                        </tr> </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlEmpresas" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="udpSucursales" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td colspan="2" align="left">
                                    <table>
                                    </table>
                                </td>
                       
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:CheckBox ID="chbUnica" runat="server" Visible="False" />
                                </td>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="udpCorreos" runat="server">
                                            <ContentTemplate>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnAgregarMail" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                    </td>
                                </tr>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarEmpresa" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="webOperacionProveedores.aspx.cs" Inherits="Operacion_Proveedores_webOperacionProveedores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript" language="javascript">
    function copiarContenido() {
        document.getElementById('<%=txtUsuario.ClientID %>').value = document.getElementById('<%=txtRfc.ClientID %>').value;
    }
</script>

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
        .style7
        {
            width: 300px;
        }
        .style11
        {
            width: 400px;
            height: 33px;
        }
        .style12
        {
            width: 109px;
        }
    </style>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <link href="../../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <table>
        <tr>
            <td align="left">
                <h2>
                    <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloProveedores %>"></asp:Label>
                </h2>
            </td>
        </tr>
    </table>
    <table style="border: solid 1px #C0C0C0;" class="bodyMain">
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="upProveedores">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server">
                            <fieldset class="register" style="border: none">
                                <%--<table class="bodyMain">--%>
                                <table style="height: 100%; width: 100%">
                                    <tr>
                                        <td>
                                            <table align="left" style="width: 468px">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre" Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                                                        <asp:TextBox ID="txtNombre" runat="server" TabIndex="1" CssClass="style7"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNombre"
                                                            CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>"
                                                            ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" ValidationGroup="RegisterUserValidationGroup"><img 
                                                src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblCalle" runat="server" AssociatedControlID="txtCalle" Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>"></asp:Label>
                                                        <asp:TextBox ID="txtCalle" runat="server" CssClass="style7" TabIndex="2"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvCalle" runat="server" ControlToValidate="txtCalle"
                                                            CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>"
                                                            ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                                    src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                                    </td>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblRfc" runat="server" AssociatedControlID="txtRfc" Text="<%$ Resources:resCorpusCFDIEs, lblRfc %>"></asp:Label>
                                                            <asp:TextBox ID="txtRfc" Enabled="false" runat="server" CssClass="style7" 
                                                                TabIndex="3"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvRfc" runat="server" ControlToValidate="txtRfc"
                                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtRFC %>"
                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, txtRFC %>" ValidationGroup="RegisterUserValidationGroup"><img alt
                                                src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="regxRFC" runat="server" ControlToValidate="txtRfc"
                                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxRFC %>"
                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, regxRFC %>" ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"
                                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>

                                                        </td>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblCodigo" runat="server" AssociatedControlID="txtCodigo" Text="<%$ Resources:resCorpusCFDIEs, lblCodigo %>"></asp:Label>
                                                                <asp:TextBox ID="txtCodigo" runat="server" CssClass="style7" TabIndex="4"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblPais" runat="server" AssociatedControlID="ddlPais" Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"></asp:Label>
                                                                <asp:DropDownList ID="ddlPais" runat="server" AutoPostBack="true" CssClass="style7"
                                                                    OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" TabIndex="5" 
                                                                    Width="300px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblEstado" runat="server" AssociatedControlID="ddlEstado" Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"></asp:Label>
                                                                <asp:DropDownList ID="ddlEstado" runat="server" AutoPostBack="true" CssClass="style7"
                                                                    OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" TabIndex="6" 
                                                                    Width="300px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblMunicipio" runat="server" AssociatedControlID="ddlMunicipio" Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>"></asp:Label>
                                                                <asp:DropDownList ID="ddlMunicipio" runat="server" CssClass="style7" TabIndex="7"
                                                                    Width="300px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblLocalidad" runat="server" AssociatedControlID="txtLocalidad" Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>"></asp:Label>
                                                                <asp:TextBox ID="txtLocalidad" runat="server" CssClass="style7" TabIndex="8"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblColonia" runat="server" AssociatedControlID="txtColonia" Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>"></asp:Label>
                                                                <asp:TextBox ID="txtColonia" runat="server" CssClass="style7" TabIndex="9"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblNoExterior" runat="server" AssociatedControlID="txtNoExterior"
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>"></asp:Label>
                                                                <asp:TextBox ID="txtNoExterior" runat="server" CssClass="style7" TabIndex="10"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </tr>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table align="left">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblNoInterior" runat="server" AssociatedControlID="txtNoInterior"
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>"></asp:Label>
                                                        <asp:TextBox ID="txtNoInterior" runat="server" TabIndex="11" CssClass="style7"></asp:TextBox>
                                                    </td>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblCodigoPostal" runat="server" AssociatedControlID="txtCodigoPostal"
                                                                Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>"></asp:Label>
                                                            <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="style7" 
                                                                TabIndex="12"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="revCodigoPostal" runat="server" ControlToValidate="txtCodigoPostal"
                                                                CssClass="failureNotification" ToolTip="Código postal erroneo" ValidationExpression="\d{5}"
                                                                ValidationGroup="RegisterUserValidationGroup">
                                    <img src="../../Imagenes/error_sign.gif" />
                                                            </asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblCorreo" runat="server" AssociatedControlID="txtCorreo" Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>" />
                                                            <asp:TextBox ID="txtCorreo" runat="server" CssClass="style7" TabIndex="13"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtCorreo"
                                                                CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>"
                                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                            src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCorreo"
                                                                CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="RegisterUserValidationGroup"><img 
                                            src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblTelefono" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTelefono %>"
                                                                AssociatedControlID="txtTelefono" />
                                                            <asp:TextBox ID="txtTelefono" runat="server" TabIndex="14" CssClass="style7"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" ControlToValidate="txtTelefono"
                                                                CssClass="failureNotification" Display="Dynamic" ToolTip="Telefono Requerido"
                                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                            src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblContacto" runat="server" AssociatedControlID="txtContacto" Text="<%$ Resources:resCorpusCFDIEs, lblContacto %>"></asp:Label>
                                                            <asp:TextBox ID="txtContacto" runat="server" CssClass="style7" TabIndex="15"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtContacto"
                                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="Contacto requerido"
                                                                ToolTip="Contacto requerido" ValidationGroup="RegisterUserValidationGroup"><img 
                                                src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="left">
                                                            <h2>
                                                                <asp:Literal ID="ltrDatosUsuario" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosUsuario %>" />
                                                            </h2>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
                                                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="style7" MaxLength="50" 
                                                                TabIndex="16"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="regxNueva" runat="server" ControlToValidate="txtUsuario"
                                                                CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtUsuario %>"
                                                                ValidationExpression="(?=^.{8,}$).*$" ValidationGroup="RegisterUserValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /> 
                                                            </asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>" ID="lblPassword"
                                                                runat="server" AssociatedControlID="txtPassword" />
                                                            <asp:TextBox ID="txtPassword" runat="server" CssClass="register" TabIndex="17" TextMode="Password"
                                                                Width="300px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword"
                                                                CssClass="failureNotification" ErrorMessage="Contraseña requerida." ToolTip="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>"
                                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                                        src="../../Imagenes/error_sign.gif" />
                                                            </asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="regExPassword" runat="server" ControlToValidate="txtPassword"
                                                                CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, msgPassIncorrecto %>"
                                                                ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"
                                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                            src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label Text="<%$ Resources:resCorpusCFDIEs, lblConfirmaNueva %>" ID="lblConfirmarPassword"
                                                                runat="server" AssociatedControlID="txtConfirmarPassword" />
                                                            <asp:TextBox ID="txtConfirmarPassword" runat="server" CssClass="passwordEntry" TabIndex="18"
                                                                TextMode="Password" Width="300px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="txtConfirmarPassword"
                                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="Es requerida la confirmación de contraseña."
                                                                ToolTip="Es requerida la confirmación de contraseña." ValidationGroup="RegisterUserValidationGroup"><img 
                                                        src="../../Imagenes/error_sign.gif" />
                                                            </asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="txtPassword"
                                                                ControlToValidate="txtConfirmarPassword" CssClass="failureNotification" Display="Dynamic"
                                                                ErrorMessage="No coinciden las contraseñas." ValidationGroup="RegisterUserValidationGroup"><img 
                                                        src="../../Imagenes/error_sign.gif" />
                                                            </asp:CompareValidator>
                                                            <p>
                                                            </p>
                                                        </td>
                                                    </tr>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table align="center">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto">
                                                <h2>
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSeleccioneSucursal %>" />
                                                </h2>
                                                <asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" CellSpacing="1"
                                                    DataKeyNames="id_sucursal" GridLines="None" Height="100px" OnRowDataBound="GrvModulos_RowDataBound"
                                                    OnSelectedIndexChanged="GrvModulos_SelectedIndexChanged" TabIndex="17" Visible="False"
                                                    Width="522px">
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblidsucursal" runat="server" Text='<%# Bind("id_sucursal") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblSucursal %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="empresa_sucursal" runat="server" Text='<%# Bind("empresa_sucursal") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Factura Única Mes" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbUnica" runat="server" Checked='<%# Bind("factura_unica_mes") %>'
                                                                    Enabled="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbSeleccion" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                                                    </EmptyDataTemplate>
                                                    <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                                                    <HeaderStyle BackColor="#880085" Font-Bold="True" ForeColor="#E7E7FF" />
                                                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                                    <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                    <SortedAscendingHeaderStyle BackColor="#594B9C" />
                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                    <SortedDescendingHeaderStyle BackColor="#33276A" />
                                                </asp:GridView>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Button ID="btnNuevo" runat="server" CssClass="botonEstilo" OnClick="btnNuevo_Click"
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" Width="80px" />
                                                        <asp:Button ID="btnBorrar" runat="server" CssClass="botonEstilo" OnClick="btnBorrar_Click"
                                                            OnClientClick="return confirm('¿Desea eliminar el proveedor seleccionado?');"
                                                            Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" Width="80px" />
                                                        <asp:Button ID="btnEditar" runat="server" CssClass="botonEstilo" OnClick="btnEditar_Click"
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" Width="80px" />
                                                        <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" OnClick="btnNCancelar_Click"
                                                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" />
                                                        <asp:Button ID="btnGuardarEmpresa" runat="server" CssClass="botonEstilo" OnClick="btnGuardarEmpresa_Click"
                                                            TabIndex="18" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" ValidationGroup="RegisterUserValidationGroup"
                                                            Width="80px" />
                                                    </td>
                                                </tr>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <h2>
                                                <asp:Label ID="lblConsultaProveedores" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblConsultaProveedores %>"></asp:Label>
                                            </h2>
                                        </td>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblRazonSocialConsulta" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="txtRazonSocialConsulta" runat="server" OnTextChanged="txtRazonSocialConsulta_TextChanged"
                                                    CssClass="style7"></asp:TextBox>
                                            </td>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblRfcConsulta" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRfc %>"></asp:Label>
                                                    <br />
                                                    <asp:TextBox ID="txtRfcConsulta" runat="server" CssClass="style7"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblCodigoProveedor" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCodigoProveedor %>"></asp:Label>
                                                    <br />
                                                    <asp:TextBox ID="txtCodigoProveedor" runat="server" CssClass="style7"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnConsulta" runat="server" CssClass="botonEstilo" 
                                                        Text="<%$ Resources:resCorpusCFDIEs,  lblSubConsulta %>" 
                                                        OnClick="btnConsulta_Click" Height="27px" />
                                                </td>
                                            </tr>

                                            </table>
                                            <table>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gvProveedores" runat="server" AutoGenerateColumns="false" AutoGenerateSelectButton="false"
                                                        BackColor="White" BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3"
                                                        CellSpacing="1" GridLines="None" OnRowDataBound="gvProveedores_RowDataBound"
                                                        OnSelectedIndexChanged="gvProveedores_SelectedIndexChanged" Style="margin-left: 0px">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbSelect" runat="server" CommandName="Select" Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionar %>"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblCodigo %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvCodigo" runat="server" Text='<%# Bind("codigo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvIdProveedor" runat="server" Text='<%# Bind("id_proveedor") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblRazonSocial %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvNombre" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblRfc %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvRfc" runat="server" Text='<%# Bind("rfc") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblContacto %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvContacto" runat="server" Text='<%# Bind("contacto") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEmailCorreo %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvEmail" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblTelefono %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvTelefono" runat="server" Text='<%# Bind("telefono") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblUser %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGvUsuario" runat="server" Text='<%# Bind("usuario") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                                                        </EmptyDataTemplate>
                                                        <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                                                        <HeaderStyle BackColor="#880085" Font-Bold="True" ForeColor="#E7E7FF" />
                                                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                                        <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#594B9C" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#33276A" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table align="right">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAnterior" runat="server" CssClass="botonEstilo" Text="<%$ Resources:resCorpusCFDIEs, lbAnterior %>"
                                                                    Visible="False" OnClick="btnAnterior_Click" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnSiguiente" runat="server" CssClass="botonEstilo" Text="<%$ Resources:resCorpusCFDIEs, lblSiguiente %>"
                                                                    Visible="False" OnClick="btnSiguiente_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        
                                        </table>
                                <table>
                                    <div>
                                    </div>
                                </table>
                                <table>
                                </table>
                                <td>
                                </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <tr>
                                        <td align="center">
                                        </td>
                                    </tr>
                                </tr>
                                </td> </td>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <td>
                                </td>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <tr>
                                            <td>
                                            </td>
                                            <td align="center">
                                            </td>
                                            <td class="style12">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td align="center">
                                            </td>
                                            <td class="style12">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td align="center">
                                            </td>
                                            <td class="style12">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </td>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </tr>
                                </tr> </table>
                                <tr>
                                </tr>
                                <tr>
                                    <td align="left">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                    <td align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="center">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <table>
                                        </table>
                                    </td>
                                    <td valign="top">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="1" align="right">
                                    </td>
                                </tr>
                                </table> </td> </tr>
                                <tr>
                                    <td align="right" class="style11" colspan="2" valign="top">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                    </td>
                                </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvProveedores" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnGuardarEmpresa" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>

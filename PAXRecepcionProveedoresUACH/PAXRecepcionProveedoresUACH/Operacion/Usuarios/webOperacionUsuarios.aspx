<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="webOperacionUsuarios.aspx.cs" Inherits="Operacion_Usuarios_webOperacionUsuarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <link href="../../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
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
            width:200px;
        }
        .style26
        {
            height: 35px;
        }
        .style27
        {
            width: 431px;
        }
        .style28
        {
            width: 411px;
        }
        .style29
        {
            height: 36px;
        }
        .style30
        {
            width: 120px;
        }
        .style32
        {
            height: 36px;
            width: 183px;
        }
        .style33
        {
            height: 35px;
            width: 183px;
        }
        .style34
        {
            width: 183px;
        }
    </style>
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            //Se Ociltan los divs para dar de alta sucursales y perfiles
            $('#divNuevaEmpresa').hide();
            $('#divNuevaSucursal').hide();
            $('#divNuevoPerfil').hide();

            //Efectos de transision entre los paneles
            $("#btnNuevaEmpresa").click(function () {
                $("#divElegirEmpresa").hide("slide", { direction: "left" });
                $("#divNuevaEmpresa").delay(400).show("slide", { direction: "left" });
            });

            $('#btnCancelNuevaEmpresa').click(function () {
                $('#divNuevaEmpresa').hide("slide", { direction: "left" });
                $('#divElegirEmpresa').delay(400).show("slide", { direction: "left" });
            });

            $('#btnNuevaSucursal').click(function () {
                $('#divDatosSucursal').hide("slide", { direction: "left" });
                $('#divNuevaSucursal').delay(400).show("slide", { direction: "left" });
            });

            $('#btnCancelNuevaSuc').click(function () {
                $('#divNuevaSucursal').hide("slide", { direction: "left" });
                $('#divDatosSucursal').delay(400).show("slide", { direction: "left" });
            });

            $('#inNuevoPerfil').click(function () {
                $('#divSelectPerfil').hide("slide", { direction: "left" });
                $('#divNuevoPerfil').delay(400).show("slide", { direction: "left" });
            });

            $('#inCancelarNuevoPerfil').click(function () {
                $('#divNuevoPerfil').hide("slide", { direction: "left" });
                $('#divSelectPerfil').delay(400).show("slide", { direction: "left" });
            });
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(requestHandler);
        });

        function requestHandler(sender, args) {

            if (args.get_error() == undefined) {

                $('#divNuevaEmpresa').hide();
                $('#divNuevaSucursal').hide();
                $('#divNuevoPerfil').hide();

                $("#btnNuevaEmpresa").click(function () {
                    $("#divElegirEmpresa").hide("slide", { direction: "left" });
                    $("#divNuevaEmpresa").delay(400).show("slide", { direction: "left" });
                });

                $('#btnCancelNuevaEmpresa').click(function () {
                    $('#divNuevaEmpresa').hide("slide", { direction: "left" });
                    $('#divElegirEmpresa').delay(400).show("slide", { direction: "left" });
                });

                $('#btnNuevaSucursal').click(function () {
                    $('#divDatosSucursal').hide("slide", { direction: "left" });
                    $('#divNuevaSucursal').delay(400).show("slide", { direction: "left" });
                });

                $('#btnCancelNuevaSuc').click(function () {
                    $('#divNuevaSucursal').hide("slide", { direction: "left" });
                    $('#divDatosSucursal').delay(400).show("slide", { direction: "left" });
                });
                $('#inNuevoPerfil').click(function () {
                    $('#divSelectPerfil').hide("slide", { direction: "left" });
                    $('#divNuevoPerfil').delay(400).show("slide", { direction: "left" });
                });

                $('#inCancelarNuevoPerfil').click(function () {
                    $('#divNuevoPerfil').hide("slide", { direction: "left" });
                    $('#divSelectPerfil').delay(400).show("slide", { direction: "left" });
                });
            }
            else
                alert(args.get_error()); // Do something

            setScrollPos();
        }
        function inNuevoPerfil_onclick() {

        }

        function inNuevoPerfil_onclick() {

        }

        function inCancelarNuevoPerfil_onclick() {

        }
        function saveScrollPos() {
            document.getElementById("<%=scrollPos%>").value =
                document.getElementById("divScroll").scrollTop;
        }
        function setScrollPos() {
            document.getElementById("divScroll").scrollTop =
                document.getElementById("<%=scrollPos%>").value;
        }
    </script>
    <br />
    <br />
    <br />
    <table>
        <tr>
            <td align="left">
                <h2>
                    <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varCatalogoUsuarios %>"></asp:Label>
                </h2>
            </td>
        </tr>
    </table>
    <table style="border: solid 1px #C0C0C0;" class="bodyMain">
        <tr>
            <td>
                <div style="text-align: center;">
                    <asp:UpdateProgress ID="uppConsultas" runat="server">
                        <ProgressTemplate>
                            <img alt="" src="../../Imagenes/imgAjaxLoader.gif" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <br />
                <asp:UpdatePanel ID="udpEstructura" runat="server">
                    <ContentTemplate>
                        <fieldset style="border:none">
                            <legend>
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, mnuUsuarios %>" /></legend>
                            <table>
                                <tr>
                                    <td>
                                        <table width="870">
                                            <tr>
                                                <td class="style28">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="linkModalPerfiles" runat="server" CssClass="botonEstilo" Height="27px"
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lblPerfiles %>" Width="80px" />
                                                                <cc1:ModalPopupExtender ID="linkModal_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                                                                    DynamicServicePath="" Enabled="True" PopupControlID="pnlPerfiles" TargetControlID="linkModalPerfiles">
                                                                </cc1:ModalPopupExtender>
                                                                <asp:Button ID="linkModalEstatus" runat="server" CssClass="botonEstilo" Height="27px"
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lblEstatus %>" Width="80px" />
                                                                <cc1:ModalPopupExtender ID="ModalPopupExtenderEstatus" runat="server" BackgroundCssClass="modalBackground"
                                                                    DynamicServicePath="" Enabled="True" PopupControlID="pnlEstatus" TargetControlID="linkModalEstatus">
                                                                </cc1:ModalPopupExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButtonList ID="rblUsuarios" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"
                                                                    OnSelectedIndexChanged="rblUsuarios_SelectedIndexChanged" Font-Bold="False" ForeColor="Black">
                                                                   <%--  <asp:ListItem Selected="false" Value="rdTodos" Text="<%$ Resources:resCorpusCFDIEs, VarDropTodos %>" />--%>
                                                                    <asp:ListItem Selected="true" Value="rdUsuario" Text="<%$ Resources:resCorpusCFDIEs, mnuUsuarios %>" />
                                                                   <%--  <asp:ListItem Selected="false" Enabled="false" Value="rdProveedor" Text="<%$ Resources:resCorpusCFDIEs, lblProveedores %>" />--%>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            <table>
                                                            <tr>
                                                            <td>
                                                                <asp:Label ID="lblBuscarUsuarios" runat="server" ForeColor="Black" 
                                                                   Text="<%$ Resources:resCorpusCFDIEs, lblUser %>" />
                                                                </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBuscarUsuarios" runat="server" CssClass="textBox" />
                                                                </td>
                                                            <td>
                                                                <asp:Button ID="btnBuscarUsuarios" runat="server" CssClass="botonEstilo" 
                                                                    Height="36px" OnClick="btnBuscarUsuarios_Click" Text="<%$ Resources:resCorpusCFDIEs, lblBuscar %>" Width="79px" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                            </td>
                                                     
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input type="hidden" id="scrollPos2" name="scrollPos" value="0" runat="server" />
                                                                <div id="divScroll"  onscroll="saveScrollPos();" style="overflow: auto; width: 345px;
                                                                    height: 200px">
                                                                    &nbsp;<br />
                                                                    <asp:TreeView ID="trvEstructura" runat="server" BorderStyle="Solid" 
                                                                        BorderWidth="1px" ImageSet="News" NodeIndent="10" 
                                                                        OnSelectedNodeChanged="trvEstructura_SelectedNodeChanged" ShowLines="True" 
                                                                        Width="300px">
                                                                        <HoverNodeStyle Font-Underline="True" />
                                                                        <NodeStyle Font-Names="Century Gothic" Font-Size="11pt" ForeColor="Blue" 
                                                                            HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                                                        <ParentNodeStyle Font-Bold="False" />
                                                                        <RootNodeStyle ImageUrl="~/Imagenes/logo_pax_40.png" />
                                                                        <SelectedNodeStyle BackColor="#FF3300" Font-Underline="True" 
                                                                            HorizontalPadding="0px" VerticalPadding="0px" />
                                                                    </asp:TreeView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAnterior" Visible="false" runat="server" CssClass="botonEstilo" 
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lbAnterior %>" 
                                                                    onclick="btnAnterior_Click" Height="27px" />
                                                                <asp:Button ID="btnSiguiente"  runat="server" CssClass="botonEstilo" 
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lblSiguiente %>" 
                                                                    onclick="btnSiguiente_Click" Height="27px" />
                                                            </td>
                                                        </tr>
                                                        
                                                    </table>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td align="left" class="style34">
                                                                <asp:Label ID="lblNodoSel" Style="font-weight: 700" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNodoSel %>"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblSelVal" Style="font-weight: 700" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNinguno %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="font-weight: 700" class="style34">
                                                                <asp:Label ID="lblProveedor" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblProvider %>"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblProvUsuario" Style="font-weight: 700" runat="server" Text="Ninguno"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="style32">
                                                                <asp:Label ID="lblNombreNodo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNodoNombre %>"
                                                                    Font-Bold="False" ForeColor="Black"></asp:Label>
                                                            </td>
                                                            <td align="left" class="style29">
                                                                <asp:TextBox ID="txtNombreNodo" TabIndex="0" runat="server" CssClass="style1" 
                                                                    ValidationGroup="RegisterUserValidationGroup"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegxNombre" runat="server" ControlToValidate="txtNombreNodo"
                                                                    CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtUsuario %>"
                                                                    ValidationExpression="(?=^.{8,}$).*$" ValidationGroup="AgregarNodo">
                                                                    <img src="../../Imagenes/error_sign.gif" /> 
                                                                </asp:RegularExpressionValidator>
                                                                <asp:Button ID="btnAgregar" runat="server" CssClass="botonEstilo" Height="27px" OnClick="btnAgregar_Click"
                                                                    Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" ValidationGroup="AgregarNodo"
                                                                    Width="80px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="style34">
                                                                <asp:Label ID="lblCorreo" runat="server" AssociatedControlID="txtCorreo" Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"
                                                                    Font-Bold="False" ForeColor="Black"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtCorreo" TabIndex="1" runat="server" CssClass="textBox" MaxLength="50" 
                                                                    ValidationGroup="RegisterUserValidationGroup"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCorreo"
                                                                    CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"
                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="RegisterUserValidationGroup">
                                                                    <img src="../../Imagenes/error_sign.gif" />
                                                                </asp:RegularExpressionValidator>
                                                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtCorreo"
                                                                    CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>"
                                                                    ValidationGroup="RegisterUserValidationGroup"><img src="../../Imagenes/error_sign.gif" />
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="style34">
                                                                <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"
                                                                    Font-Bold="False" ForeColor="Black"></asp:Label>
                                                            </td>
                                                            <td colspan="2" align="left">
                                                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="textBox" MaxLength="50" 
                                                                    TabIndex="2" ValidationGroup="RegisterUserValidationGroup"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="regxNueva0" runat="server" ControlToValidate="txtUsuario"
                                                                    CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtUsuario %>"
                                                                    ValidationExpression="(?=^.{8,}$).*$" ValidationGroup="RegisterUserValidationGroup">
                                                                    <img src="../../Imagenes/error_sign.gif" /> 
                                                                </asp:RegularExpressionValidator>
                                                                <asp:RequiredFieldValidator ID="UserNameRequired0" runat="server" ControlToValidate="txtUsuario"
                                                                    CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>"
                                                                    ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" ValidationGroup="RegisterUserValidationGroup"><img
                                                                    src="../../Imagenes/error_sign.gif" />
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="style34">
                                                                <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>"
                                                                    Font-Bold="False" ForeColor="Black" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtPassword" TabIndex="3"  runat="server" CssClass="textBox" 
                                                                    TextMode="Password" ValidationGroup="RegisterUserValidationGroup"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtPassword"
                                                                    CssClass="failureNotification" Display="Dynamic" Height="16px" ToolTip="Contraseña incompleta"
                                                                    ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"
                                                                    ValidationGroup="ChangeUserPasswordValidationGroup"><img alt 
                                            src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="style33">
                                                                <asp:Label ID="lblConfirmarPassword" runat="server" AssociatedControlID="txtConfirmarPassword"
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lblConfirmaNueva %>" Font-Bold="False" ForeColor="Black" />
                                                            </td>
                                                            <td align="left" class="style26">
                                                                <asp:TextBox ID="txtConfirmarPassword" TabIndex="4"  runat="server" CssClass="textBox" 
                                                                    TextMode="Password" ValidationGroup="RegisterUserValidationGroup"></asp:TextBox>
                                                                <asp:CompareValidator ID="PasswordCompare0" runat="server" ControlToCompare="txtPassword"
                                                                    ControlToValidate="txtConfirmarPassword" CssClass="failureNotification" Display="Dynamic"
                                                                    ErrorMessage="No coinciden las contraseñas." ValidationGroup="RegisterUserValidationGroup"><img 
                        alt src="../../Imagenes/error_sign.gif" />
                                                                </asp:CompareValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="style34">
                                                                <asp:Label ID="lblPerfilAsignar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblPerfiles %>"
                                                                    Font-Bold="False" ForeColor="Black"></asp:Label>
                                                                <br />
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ddlPerfilAsignar" runat="server" AutoPostBack="True" DataTextField="desc_perfil"
                                                                    DataValueField="id_perfil" TabIndex="5" Height="20px" Width="200px" 
                                                                    CssClass="textBox">
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="lblPerfilProveedor" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="2" class="style29">
                                                                <asp:Button ID="btnNuevo" runat="server" TabIndex="6" CssClass="botonEstilo" OnClick="btnNuevo_Click"
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" />
                                                                <asp:Button ID="btnGuardarActualizar" TabIndex="7" runat="server" CssClass="botonEstilo"
                                                                    OnClick="btnGuardarActualizar_Click" 
                                                                    Text="<%$ Resources:resCorpusCFDIEs, btnGuardar  %>" 
                                                                    ValidationGroup="RegisterUserValidationGroup" />
                                                                <asp:Button ID="btnEditar" TabIndex="8" runat="server" CssClass="botonEstilo" OnClick="btnEditar_Click"
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" />
                                                                <asp:Button ID="btnBorrar" TabIndex="9" runat="server" CssClass="botonEstilo" OnClick="btnBorrar_Click"
                                                                    
                                                                    OnClientClick="return confirm('¿Desea eliminar el usuario seleccionado?');" 
                                                                    Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" />
                                                                <asp:Button ID="btnNCancelar" TabIndex="10" runat="server" CssClass="botonEstilo"
                                                                    OnClick="btnNCancelar_Click" 
                                                                    Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:HiddenField ID="hdnSelVal" runat="server" />
                                                                <asp:HiddenField ID="hdnSel" runat="server" />
                                                                <asp:HiddenField ID="hdnValuePath" runat="server" />
                                                                <asp:HiddenField ID="hdIdEstructura" runat="server" Visible="False" />
                                                                <asp:Button ID="btnCancelar" runat="server" CssClass="botonEstilo" Height="27px"
                                                                    OnClick="btnCancelar_Click" TabIndex="15" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>"
                                                                    Visible="False" Width="80px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label1" runat="server" Font-Bold="False" ForeColor="Black" Text="<%$ Resources:resCorpusCFDIEs, lblAdministrarFacultades %>"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblAdministrarEstatus" runat="server" Font-Bold="False" ForeColor="Black"
                                                        Text="<%$ Resources:resCorpusCFDIEs, lblAdministrarEstatus %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style28">
                                                    <div style="overflow: auto; height: 200px; width: 432px;">
                                                        <br />
                                                        <asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                            BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" CellSpacing="1"
                                                            DataKeyNames="id_sucursal" GridLines="None" OnRowDataBound="GrvModulos_RowDataBound"
                                                            OnSelectedIndexChanged="GrvModulos_SelectedIndexChanged" Visible="False" Width="317px">
                                                            <Columns>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblidsucursal" runat="server" Text='<%# Bind("id_sucursal") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblFacultad %>">
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
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="overflow: auto; height: 200px; width: 467px;">
                                                        <br />
                                                        <asp:GridView ID="gvEstatus" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                            BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" CellSpacing="1"
                                                            DataKeyNames="id_status" GridLines="None" Visible="True" Width="317px">
                                                            <Columns>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblidstatus" runat="server" Text='<%# Bind("id_status") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="nombre" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbSeleccion" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
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
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="pnlPerfiles" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" BackColor="White">
                            <div style="overflow: auto; width: 581px; height: 400px">
                                <table style="margin: 15px 15px 15px 15px;">
                                    <tr>
                                        <td align="left" class="style30">
                                            <asp:Label ID="lblPerfil" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblPerfiles %>"
                                                Font-Bold="False" ForeColor="Black"></asp:Label>
                                            <br />
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlPerfil" runat="server" AutoPostBack="True" DataTextField="desc_perfil"
                                                DataValueField="id_perfil" OnDataBound="ddlPerfil_DataBound" OnSelectedIndexChanged="ddlPerfil_SelectedIndexChanged"
                                                TabIndex="5" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style30">
                                            <asp:Label ID="lblNombrePerfil" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNombre %>"
                                                Font-Bold="False" ForeColor="Black"></asp:Label>
                                        </td>
                                        <td align="left" class="style27">
                                            <asp:TextBox ID="txtNuevoPerfil" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:GridView ID="GrvModulos" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" CellSpacing="1"
                                                DataKeyNames="id_modulo" GridLines="None" OnRowDataBound="GrvModulos_RowDataBound"
                                                OnSelectedIndexChanged="GrvModulos_SelectedIndexChanged" Visible="False" Width="335px">
                                                <Columns>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblidmodulo" runat="server" Text='<%# Bind("id_modulo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblmodulos %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmodulo" runat="server" Text='<%# Bind("nombre_modulo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbSeleccion" runat="server" />
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
                                            <br />
                                            <asp:Label runat="server" ID="errorPerfil" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="style26">
                                            <asp:Button ID="btnNuevoPerfil" runat="server" CssClass="botonEstilo" Height="27px"
                                                OnClick="btnNuevoPerfil_Click" Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>"
                                                Width="82px" />
                                            <asp:Button ID="btnGuardarNuevoPerfil" runat="server" CssClass="botonEstilo" Height="27px"
                                                OnClick="btnGuardarNuevoPerfil_Click" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar  %>"
                                                Width="82px" />
                                            <asp:Button ID="btnEditarPerfil" runat="server" CssClass="botonEstilo" Height="27px"
                                                OnClick="btnEditarPerfil_Click" Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>"
                                                Width="82px" />
                                            <asp:Button ID="btnEliminarPerfil" runat="server" CssClass="botonEstilo" Height="27px"
                                                OnClick="btnEliminarPerfil_Click" Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>"
                                                Width="82px" />
                                            <asp:Button ID="btnCancelarPerfil" runat="server" CssClass="botonEstilo" Height="27px"
                                                OnClick="btnCancelarPerfil_Click" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>"
                                                Width="82px" />
                                            <asp:Button ID="btnSalirPerfil" runat="server" CssClass="botonEstilo" Height="27px"
                                                Text="<%$ Resources:resCorpusCFDIEs, mnuSalir %>" Width="82px" OnClick="btnSalirPerfil_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="pnlEstatus" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel2" runat="server" BackColor="White">
                            <div style="overflow: auto; width: 573px; height: 360px">
                                <table style="margin: 15px 15px 15px 15px;">
                                    <tr>
                                        <td align="left" class="style30">
                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEstatus %>"
                                                Font-Bold="False" ForeColor="Black"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlEstatus" runat="server" AutoPostBack="True" DataTextField="nombre"
                                                DataValueField="id_status" TabIndex="5" Width="200px" OnSelectedIndexChanged="ddlEstatus_SelectedIndexChanged1">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="style30">
                                            <asp:Label ID="lblNombreEstatus" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNombre %>"
                                                Font-Bold="False" ForeColor="Black"></asp:Label>
                                        </td>
                                        <td align="left" class="style27">
                                            <asp:TextBox ID="txtNuevoEstatus" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Panel runat="server" ID="pnlEstatusGrid" ScrollBars="Vertical" Height="170px"
                                                Width="400px">
                                                <asp:GridView ID="GrvEstatus" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" CellSpacing="1"
                                                    DataKeyNames="id_status" GridLines="None" Visible="False" Width="335px">
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblidestatus" runat="server" Text='<%# Bind("id_status") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatusComprobante %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblnombre" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
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
                                                <asp:Label runat="server" ID="errorEstatus" ForeColor="Red"></asp:Label>
                                                <br />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" class="style26">
                                            <asp:Button ID="btnNuevoEstatus" runat="server" CssClass="botonEstilo" Height="27px"
                                                Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" Width="82px" OnClick="btnNuevoEstatus_Click" />
                                            <asp:Button ID="btnGuardarNuevoEstatus" runat="server" CssClass="botonEstilo" Height="27px"
                                                Text="<%$ Resources:resCorpusCFDIEs, btnGuardar  %>" Width="82px" OnClick="btnGuardarNuevoEstatus_Click" />
                                            <asp:Button ID="btnEditarEstatus" runat="server" Visible="false" CssClass="botonEstilo"
                                                Height="27px" Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" Width="82px"
                                                OnClick="btnEditarEstatus_Click" />
                                            <asp:Button ID="btnEliminarEstatus" runat="server" CssClass="botonEstilo" Height="27px"
                                                Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" Width="82px" OnClick="btnEliminarEstatus_Click" />
                                            <asp:Button ID="btnCancelarEstatus" runat="server" CssClass="botonEstilo" Height="27px"
                                                Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="82px" OnClick="btnCancelarEstatus_Click" />
                                            <asp:Button ID="btnSalirEstatus" runat="server" CssClass="botonEstilo" Height="27px"
                                                Text="<%$ Resources:resCorpusCFDIEs, mnuSalir %>" Width="82px" OnClick="btnSalirEstatus_Click" />
                                        </td>
                                    </tr>
                                  
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
    </table>
</asp:Content>

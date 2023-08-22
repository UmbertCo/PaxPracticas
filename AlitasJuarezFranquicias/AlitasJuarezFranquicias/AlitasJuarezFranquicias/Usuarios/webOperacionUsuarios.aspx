<%@ Page Title="Usuarios" Theme="Alitas" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="webOperacionUsuarios.aspx.cs" Inherits="Operacion_Usuarios_webOperacionUsuarios" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <script src="../Scripts/progressbar.js" type="text/javascript"></script>
    <link href="../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<link href="../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginReq);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);


        function beginReq(sender, args) {
            // muestra el popup 
            $find(ModalProgress).show();

        }

        function endReq(sender, args) {
            //  esconde el popup 
            $find(ModalProgress).hide();
        }

        function fntraerEnter(keyStroke) {
            isNetscape = (document.layers);
            eventChooser = (isNetscape) ? keyStroke.which : event.keyCode;
            if (eventChooser == 13) {
                return false;
            }
        }
        document.onkeypress = fntraerEnter; 
   
    </script>
    <script runat="server">   
        public void Page_PreInit()
        {
            if (Session["theme"] == null)
            {
                this.Theme = "Alitas";
            }
            else
            {
                this.Theme = Convert.ToString(Session["theme"]);
            }
        }
    </script>
    <meta id="RefreshPeriod" runat="server" http-equiv="refresh" />
    <div style="text-align: center;">
        <asp:UpdateProgress ID="uppConsultas" runat="server">
            <ProgressTemplate>
                <img alt="" src="../Imagenes/imgAjaxLoader.gif" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="udpEstructura" runat="server">
        <ContentTemplate>
        <center>
            <table style="width:952px; text-align:left; background-color:Black">
                <tr>
                    <td class="Titulo">
                    <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varCatalogoUsuarios %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:952px; height:0.5px; background-color:#fff000"></td>
                </tr>
                <tr>
                    <td class="Subtitulos">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, mnuUsuarios %>" />
                    </td>
                </tr>
            </table>
        </center>
        <center>
            <table class="background_tablas_transparente" style="width:952px">
                <tr>
                    <td>
                    <fieldset class="register" style="width: 890px; border-color:transparent;">
                <table>
                    <tr>
                        <td>
                            <table width="870">
                                <tr>
                                    <td>
                                        <asp:TreeView ID="trvEstructura" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                            ImageSet="News" OnSelectedNodeChanged="trvEstructura_SelectedNodeChanged" Width="300px"
                                            NodeIndent="10" ShowLines="True">
                                            <HoverNodeStyle Font-Underline="True" />
                                            <NodeStyle Font-Names="Verdana" Font-Size="11pt" ForeColor="Black" HorizontalPadding="5px"
                                                NodeSpacing="0px" VerticalPadding="0px" />
                                            <ParentNodeStyle Font-Bold="False" />
                                            <RootNodeStyle />
                                            <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px"
                                                BackColor="#ECECEC" />
                                        </asp:TreeView>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblNodoSel" runat="server" SkinId="labelLarge" Style="font-weight: 700" Text="<%$ Resources:resCorpusCFDIEs, lblNodoSel %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSelVal" SkinId="labelLarge" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNinguno %>"></asp:Label>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblNombreNodo" runat="server" SkinId="labelLarge" Style="font-weight: 700" Text="<%$ Resources:resCorpusCFDIEs, lblNodoNombre %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNombreNodo" runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegxNombre" runat="server" ControlToValidate="txtNombreNodo"
                                                        CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtUsuario %>"
                                                        ValidationExpression="(?=^.{8,}$).*$" ValidationGroup="AgregarNodo"><img 
                                            src="../Imagenes/error_sign.gif" /> 
                                                    </asp:RegularExpressionValidator>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAgregar" runat="server" CssClass="botonEstilo" OnClick="btnAgregar_Click"
                                                        Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" ValidationGroup="AgregarNodo"
                                                        Width="80px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="hdnValuePath" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:HiddenField ID="hdnSel" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:HiddenField ID="hdnSelVal" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </fieldset>
                    </td>
                </tr>
            </table>
        </center>
        <center>
            <table class="background_tablas_transparente" style="width:952px">
                <tr>
                    <td align="right" style="width: 890px;">
                        <asp:Button ID="btnNuevo" runat="server" CssClass="botonEstilo" OnClick="btnNuevo_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" Width="80px" ValidationGroup="validationgrupBotones" />
                        <asp:Button ID="btnBorrar" runat="server" CssClass="botonEstilo" OnClick="btnBorrar_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" Width="80px" ValidationGroup="validationgrupBotones" />
                        <asp:Button ID="btnEditar" runat="server" CssClass="botonEstilo" OnClick="btnEditar_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" Width="80px" ValidationGroup="validationgrupBotones" />
                        <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" OnClick="btnNCancelar_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" ValidationGroup="validationgrupBotones" />
                    </td>
                </tr>
            </table>
            </center>
            <cc1:ModalPopupExtender ID="modalGenerando" runat="server" BackgroundCssClass="modalBackground"
                PopupControlID="pnlGenerando" PopupDragHandleControlID="" TargetControlID="pnlGenerando">
            </cc1:ModalPopupExtender>
            <script type="text/javascript" language="javascript">
                var ModalProgress = '<%= modalGenerando.ClientID %>';
            </script>
            <asp:Panel ID="pnlGenerando" runat="server" Width="300px" CssClass="modal" BorderStyle="Solid"
                BorderWidth="1px">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varProcesando %>" Font-Names="Arial" ForeColor="Black" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdateProgress ID="updGenera" runat="server">
                                <ProgressTemplate>
                                    <img alt="" src="../Imagenes/imgAjaxLoader.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upFormulario" runat="server">
        <ContentTemplate>
        <center>
            <table style="width:952px; text-align:left; background-color:Black">
                <tr>
                    <td class="Subtitulos">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosUsuario %>" /></legend>
                    </td>
                </tr>
            </table>
        </center>
        <center>
            <table class="background_tablas_transparente" style="width:952px">
                <tr>
                    <td>
                    <asp:Panel ID="pnlFormulario" runat="server" Height="450px" Width="930px">
                <div class="accountInfo">
                    <fieldset class="register" style="height: 450px; width: 890px; border-color:transparent;">
                        <table>
                            <tr>
                                <td style="vertical-align: top; width: 400px;">
                                    <p>
                                        <asp:Label ID="lblNombreCompleto" runat="server" AssociatedControlID="txtNombre" SkinId="labelLarge"
                                            Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                                        <asp:TextBox ID="txtNombre" runat="server" CssClass="textEntry" MaxLength="255" TabIndex="1"
                                            Width="300px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired0" runat="server" ControlToValidate="txtNombre"
                                            CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>"
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" ValidationGroup="RegisterUserValidationGroup"
                                            Height="21px" Width="16px">*</asp:RequiredFieldValidator>
                                    </p>
                                    <p>
                                        <asp:Label ID="lblUsuario" runat="server" SkinId="labelLarge" AssociatedControlID="txtUsuario" Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
                                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry" MaxLength="50" TabIndex="2"
                                            Width="300px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUsuario"
                                            CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>"
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" ValidationGroup="RegisterUserValidationGroup"><img 
                                src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regxNueva" runat="server" ControlToValidate="txtUsuario"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtUsuario %>"
                                            ValidationExpression="(?=^.{8,}$).*$" ValidationGroup="RegisterUserValidationGroup"><img 
                                src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                    </p>
                                    <p>
                                        <asp:Label ID="lblCorreo" runat="server" SkinId="labelLarge" AssociatedControlID="txtCorreo" Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"></asp:Label>
                                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="textEntry" MaxLength="50" TabIndex="3"
                                            Width="300px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtCorreo"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>"
                                            ValidationGroup="RegisterUserValidationGroup"><img 
                                src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCorreo"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="RegisterUserValidationGroup"
                                            Width="300px"><img 
                                src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                    </p>
                                    <p>
                                        <asp:Label ID="lblTelefono" runat="server" SkinId="labelLarge" AssociatedControlID="txtTelefono" Text="<%$ Resources:resCorpusCFDIEs, lblTelefono %>"></asp:Label>
                                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="textEntry" MaxLength="50"
                                            TabIndex="4" Width="300px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCorreo"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, vrTelefonomsj %>"
                                            ValidationGroup="RegisterUserValidationGroup"><img 
                                src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                ValidationGroup="RegisterUserValidationGroup" Width="131px"><img 
                                src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>--%>
                                    </p>
                                    <p id="pChkCambio" runat="server" visible="false" class="pCambioContrase">
                                        <asp:CheckBox ID="chkCambioContrasena" runat="server" AutoPostBack="True" OnCheckedChanged="chkCambioContrasena_CheckedChanged" />
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs,hedPass %>"></asp:Label>
                                    </p>
                                    <p>
                                        <asp:Label ID="lblPassword" SkinId="labelLarge" runat="server" AssociatedControlID="txtPassword" Text="<%$ Resources:resCorpusCFDIEs,lblContrasenia %>"></asp:Label>
                                        <asp:TextBox CausesValidation="False"  ID="txtPassword" runat="server" CssClass="passwordEntry" TextMode="Password"
                                            Width="300px" TabIndex="5"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword" 
                                    CssClass="failureNotification" ErrorMessage="Contraseña requerida." ToolTip="Contraseña requerida." 
                                    ValidationGroup="RegisterUserValidationGroup"><img src="../Imagenes/error_sign.gif" />
                            </asp:RequiredFieldValidator>--%>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPassword"
                                            ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"
                                            ValidationGroup="ChangeUserPasswordValidationGroup" CssClass="failureNotification"
                                            ToolTip="Contraseña incompleta" Display="Dynamic" Height="16px"><img src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="txtPassword"
                                            CssClass="failureNotification" ValidationGroup="RegisterUserValidationGroup" ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>"
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>" Display="Dynamic"><img src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                    </p>
                                    <p>
                                        <asp:Label ID="lblConfirmarPassword" SkinId="labelLarge" runat="server" AssociatedControlID="txtConfirmarPassword"
                                            Text="<%$ Resources:resCorpusCFDIEs,lblConfContrasena %>"></asp:Label>
                                        <asp:TextBox  ID="txtConfirmarPassword" runat="server" CssClass="passwordEntry" TextMode="Password"
                                            Width="300px" TabIndex="6"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ControlToValidate="txtConfirmarPassword" CssClass="failureNotification" Display="Dynamic" 
                                    ErrorMessage="Es requerida la confirmación de contraseña." ID="ConfirmPasswordRequired" runat="server" 
                                    ToolTip="Es requerida la confirmación de contraseña." ValidationGroup="RegisterUserValidationGroup"><img src="../Imagenes/error_sign.gif" />
                            </asp:RequiredFieldValidator>--%>
                                        <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="txtPassword"
                                            ControlToValidate="txtConfirmarPassword" CssClass="failureNotification" Display="Dynamic"
                                            ErrorMessage="No coinciden las contraseñas." ValidationGroup="RegisterUserValidationGroup"><img src="../Imagenes/error_sign.gif" /> </asp:CompareValidator>
                                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="txtConfirmarPassword"
                                            CssClass="failureNotification" ValidationGroup="RegisterUserValidationGroup" ToolTip="<%$ Resources:resCorpusCFDIEs, valConfirmaNueva %>"
                                            Display="Dynamic" ErrorMessage=""><img src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                    </p>
                                    <asp:HiddenField ID="hdIdEstructura" runat="server" Visible="False" />
                                    <br />
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPerfiles" SkinId="labelLarge" runat="server" AssociatedControlID="ddlPerfil" Text="<%$ Resources:resCorpusCFDIEs, lblPerfiles %>"> </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlPerfil" runat="server" AutoPostBack="True" DataTextField="desc_perfil"
                                                    DataValueField="id_perfil" OnDataBound="ddlPerfil_DataBound" OnSelectedIndexChanged="ddlPerfil_SelectedIndexChanged"
                                                    TabIndex="8" Width="300px" Enabled="False">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="PanelGrid" runat="server" Height="300px" ScrollBars="Auto" Width="300px">
                                                    <asp:GridView ID="GrvModulos" runat="server" AutoGenerateColumns="False" SkinID="SkinGridView"                                                        CellPadding="4"
                                                        DataKeyNames="id_modulo" GridLines="Horizontal" Visible="False" Width="300px"
                                                        Enabled="False">
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
                                                            <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                                                        </EmptyDataTemplate>
                                                        <%-- 
                                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                                        <HeaderStyle BackColor="#0073aa" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle BackColor="#0073aa" ForeColor="White" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="White" ForeColor="#333333" />
                                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                        <SortedAscendingHeaderStyle BackColor="#487575" />
                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                        <SortedDescendingHeaderStyle BackColor="#275353" />
                                                        --%>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                    <br />
                </div>
            </asp:Panel>
                    </td>
                </tr>
            </table>
        </center>
        <center>
            <table class="background_tablas_transparente" style="width:952px">
                <tr>
                    <td align="right" style="width: 890px;">
            <p style="text-align: right;">
                <asp:Button ID="btnGuardarActualizar" runat="server" CommandName="MoveNext" CssClass="botonEstilo"
                    OnClick="btnGuardarActualizar_Click" TabIndex="9" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                    ValidationGroup="RegisterUserValidationGroup" />
                <asp:Button ID="btnCancelar" runat="server" CssClass="botonEstilo" OnClick="btnCancelar_Click"
                    TabIndex="10" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Visible="False" />
            </p>
            </td>
            </tr>
            </table>
        </center>
            <center>
                <table style="width:952px; text-align:left; background-color:Black">
                    <tr>
                        <td class="Subtitulos">
                        <asp:Literal ID="Literal5" runat="server" Text="Sucursales" /></legend>
                        </td>
                    </tr>
                </table>
            </center>
            <center>
                <table class="background_tablas_transparente" style="width:952px">
                    <tr>
                        <td>
            <fieldset id="Fieldset1" runat="server" class="register" style="width: 890px; border-color:transparent;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblNombreSucursal" SkinId="labelLarge" runat="server" AssociatedControlID="grvSucursales"
                                Text="<%$ Resources:resCorpusCFDIEs, lblSucursalesAsignadas %>"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlSucursales" runat="server" AutoPostBack="True" DataTextField="nombre"
                                DataValueField="id_estructura" Enabled="False" OnDataBound="ddlSucursales_DataBound"
                                OnSelectedIndexChanged="ddlSucursales_SelectedIndexChanged" TabIndex="7" Width="300px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Image Visible="false" ID="imgFlecha" ImageUrl="../Imagenes/left-arrow-icon.jpg"
                                Height="40px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="300px">
                                <asp:GridView ID="grvSucursales" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#0073aa" BorderStyle="Double" BorderWidth="3px" CellPadding="4"
                                    DataKeyNames="id_estructura" Enabled="False" GridLines="Horizontal" Visible="True"
                                    Width="300px" SkinID="SkinGridView" >
                                    <Columns>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblidsucursal" runat="server" Text='<%# Bind("id_estructura") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblSucursal %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsucursal" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbSeleccion" runat="server" AutoPostBack="true" OnCheckedChanged="cbSeleccion_OnCheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                                    </EmptyDataTemplate>
                                   <%--<FooterStyle BackColor="White" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#0073aa" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#0073aa" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="White" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#0073aa" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                    <SortedAscendingHeaderStyle BackColor="#487575" />
                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                    <SortedDescendingHeaderStyle BackColor="#275353" />--%>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </fieldset>
                        </td>
                    </tr>
                </table>
            </center>
            <br />
            <%-- Modalpoup Avisos --%>
            <asp:LinkButton ID="lkbAviso" runat="server"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="mpeAvisos" runat="server" TargetControlID="lkbAviso"
                PopupControlID="pnlAvisos" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlAvisos" runat="server" CssClass="modal">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="TablaBackGround">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <table class="TablaBackGround">
                                <tr>
                                    <td>
                                        <img alt="" class="imgInformacion" src="../Imagenes/info_ico.png" />
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblAviso" runat="server" SkinID="labelLarge"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAviso" runat="server" CssClass="botonEstilo"  Text="OK" ValidationGroup="validationgrupBotones" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <%--ModalPoup ErrorLog--%>
            <br />
            <asp:LinkButton ID="lkbErrorLog" runat="server"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="mpeErrorLog" runat="server" TargetControlID="lkbErrorLog"
                PopupControlID="pnlErrorLog" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlErrorLog" runat="server" CssClass="modal">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="TablaBackGround">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <table class="TablaBackGround">
                                <tr>
                                    <td>
                                        <img alt="" class="imgInformacion" src="../Imagenes/info_ico.png" />
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblErrorLog" runat="server" SkinID="labelLarge"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnErrorLog" runat="server" CssClass="botonEstilo" Text="OK" ValidationGroup="validationgrupBotones" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

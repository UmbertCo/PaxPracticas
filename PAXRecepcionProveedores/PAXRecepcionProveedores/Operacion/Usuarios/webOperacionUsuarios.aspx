<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="webOperacionUsuarios.aspx.cs" Inherits="PAXRecepcionProveedores.Operacion.Usuarios.webOperacionUsuarios" 
    MaintainScrollPositionOnPostback="true"   %>
     <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
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
        #divSelectPerfil
    {
        height: 255px;
    }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">

    function fntraerEnter(keyStroke) {
        isNetscape = (document.layers);
        eventChooser = (isNetscape) ? keyStroke.which : event.keyCode;
        if (eventChooser == 13) {
            return false;
        }
    }
    document.onkeypress = fntraerEnter;


        $(document).ready(function () {
           
//            $('#inNuevoPerfil').click(function mostrarPerfil() {
//                $('#divSelectPerfil').hide("slide", { direction: "left" });
//                $('#divNuevoPerfil').delay(400).show("slide", { direction: "left" });
//            });

//            $('#inCancelarNuevoPerfil').click(function () {
//                $('#divNuevoPerfil').hide("slide", { direction: "left" });
//                $('#divSelectPerfil').delay(400).show("slide", { direction: "left" });
//            });
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(requestHandler);
        });

        function requestHandler(sender, args) {

//            if (args.get_error() == undefined) {

//                
//                $('#divNuevoPerfil').hide();

//              
//                $('#inNuevoPerfil').click(function () {
//                    $('#divSelectPerfil').hide("slide", { direction: "left" });
//                    $('#divNuevoPerfil').delay(400).show("slide", { direction: "left" });
//                });

//                $('#inCancelarNuevoPerfil').click(function () {
//                    $('#divNuevoPerfil').hide("slide", { direction: "left" });
//                    $('#divSelectPerfil').delay(400).show("slide", { direction: "left" });
//                });
//            }
//            else
//                alert(args.get_error()); // Do something
            
            
            setScrollPos();

        }
        function inNuevoPerfil_onclick() {

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
    <br /><br /><br />
    <h2>
        <asp:Label ID="lblTitulo" runat="server" 
            Text="<%$ Resources:resCorpusCFDIEs, varCatalogoUsuarios %>" Font-Bold="True" 
            ForeColor="#8B181B"></asp:Label>
    </h2>
    
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
            <fieldset class="register" style="width: 890px;">
                <%--<legend>
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, mnuUsuarios %>" /></legend>--%>
                <table style="height: 225px">
                    <tr>
                        <td>
                            <table width="870">
                                <tr>
                                    <td valign="top">
                                    <table>
                                    <tr>
                                        <td>
                                        
                                        <asp:RadioButtonList ID="rblUsuarios" runat="server" AutoPostBack="True" 
                                                RepeatDirection="Horizontal" OnSelectedIndexChanged="rblUsuarios_SelectedIndexChanged" >
                                           <asp:ListItem Selected="True" Value="rdTodos"  Text="<%$ Resources:resCorpusCFDIEs, VarDropTodos %>"/>
                                            <asp:ListItem Selected="false" Value="rdUsuario"  Text="<%$ Resources:resCorpusCFDIEs, mnuUsuarios %>"/>
                                            <asp:ListItem Selected="false" Value="rdProveedor"  Text="<%$ Resources:resCorpusCFDIEs, lblProveedores %>"/>
                                        </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                            <tr>
                                                            <td>
                                                                <asp:Label ID="lblBuscarUsuarios" runat="server" ForeColor="Black" 
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>" />
                                                                </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBuscarUsuarios" runat="server" CssClass="textBox" />
                                                                </td>
                                                            <td>
                                                                <asp:Button ID="btnBuscarUsuarios" runat="server" CssClass="botonEstiloVentanas" 
                                                                    Height="36px" OnClick="btnBuscarUsuarios_Click" Text="<%$ Resources:resCorpusCFDIEs, btnBuscar %>" Width="79px" />
                                                                </td>
                                                                </tr>
                                                                <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblLineasPagina" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblLineasPagina %>" ></asp:Label>
                                                <asp:DropDownList ID="ddlLineasPagina" runat="server" >
                                                    <asp:ListItem Selected="true" Text="10" />
                                                    <asp:ListItem Text="20" />
                                                    <asp:ListItem Text="30" />
                                                    <asp:ListItem Text="40" />
                                                    <asp:ListItem Text="50" />
                                                </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td>
                                    <input type="hidden" id="scrollPos2" name="scrollPos" value="0" runat="server"/>
                                    <%--<asp:Panel ScrollBars="Vertical" ID="pnlUsuarios" runat="server" Height="150px">--%>
                                    <div id="divScroll" onscroll="saveScrollPos();" style="overflow:auto; width: 345px; height:200px;">
                                    
                                        <asp:TreeView ID="trvEstructura" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                            ImageSet="News" OnSelectedNodeChanged="trvEstructura_SelectedNodeChanged" Width="300px"
                                            NodeIndent="10" ShowLines="True" OnClientClick="javascript:keepScroll()" >
                                            <HoverNodeStyle Font-Underline="True" />
                                            <NodeStyle Font-Names="Century Gothic" Font-Size="11pt" ForeColor="Blue" HorizontalPadding="5px"
                                                NodeSpacing="0px" VerticalPadding="0px" />
                                            <ParentNodeStyle Font-Bold="False" />
                                            <RootNodeStyle ImageUrl="~/Imagenes/logo_pax_40.png" />
                                            <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px"
                                                BackColor="#FF3300" />
                                        </asp:TreeView>
                                       
                                        </div>
                                        <%--</asp:Panel>--%>
                                        </td>
                                        </tr>
                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAnterior" Visible="false" runat="server" CssClass="botonEstiloVentanas" 
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lbAnterior %>" 
                                                                    onclick="btnAnterior_Click" />
                                                                <asp:Button ID="btnSiguiente"  runat="server" CssClass="botonEstiloVentanas" 
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lblSiguiente %>" 
                                                                    onclick="btnSiguiente_Click" />
                                                            </td>
                                                        </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table>
                                        <tr>
                                            <td><asp:Label ID="lblNodoSel" runat="server" Style="font-weight: 700" Text="<%$ Resources:resCorpusCFDIEs, lblNodoSel %>"></asp:Label></td>
                                            <td><asp:Label ID="lblSelVal" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNinguno %>"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="lblNombreNodo" runat="server" Style="font-weight: 700" Text="<%$ Resources:resCorpusCFDIEs, lblNodoNombre %>"></asp:Label></td>
                                            <td><asp:TextBox ID="txtNombreNodo" runat="server"></asp:TextBox></td>
                                            <td><asp:RegularExpressionValidator ID="RegxNombre" runat="server" ControlToValidate="txtNombreNodo"
                                                        CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtUsuario %>"
                                                        ValidationExpression="(?=^.{8,}$).*$" ValidationGroup="AgregarNodo" Width="20px"><img 
                                                        src="../../Imagenes/error_sign.gif" /> 
                                                    </asp:RegularExpressionValidator></td>
                                            <td><asp:Button ID="btnAgregar" runat="server" CssClass="botonEstiloVentanas" OnClick="btnAgregar_Click"
                                                        Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" ValidationGroup="AgregarNodo"
                                                        Width="80px" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:HiddenField ID="hdnValuePath" runat="server" /></td>
                                            <td><asp:HiddenField ID="hdnSel" runat="server" /></td>
                                            <td><asp:HiddenField ID="hdnSelVal" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="lblProveedor" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblProveedorAsociado %>" Style="font-weight: 700"></asp:Label></td>
                                            <td><asp:Label ID="lblProvUsuario" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNinguno %>"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                            <div >
                                                 <p>
                                    <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
                                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry" MaxLength="50" TabIndex="2"
                                            Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUsuario"
                                            CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>"
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" ValidationGroup="RegisterUserValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regxNueva" runat="server" ControlToValidate="txtUsuario"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtUsuario %>"
                                            ValidationExpression="(?=^.{8,}$).*$" ValidationGroup="RegisterUserValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /> 
                                        </asp:RegularExpressionValidator>
                                    </p>
                                    <p>
                                    <asp:Label ID="lblCorreo" runat="server" AssociatedControlID="txtCorreo" Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"></asp:Label>
                                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="textEntry" MaxLength="50" TabIndex="3"
                                            Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtCorreo"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>"
                                            ValidationGroup="RegisterUserValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCorreo"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="RegisterUserValidationGroup"
                                            Width="131px"><img 
                            src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                    </p>
                                    <p>
                                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="<%$ Resources:resCorpusCFDIEs, lblContrasenaCorreo %>"></asp:Label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="passwordEntry" TextMode="Password" Width="200px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                    ControlToValidate="txtPassword" ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"
                                    ValidationGroup="ChangeUserPasswordValidationGroup" 
                                    CssClass="failureNotification" 
                                    ToolTip="Contraseña incompleta" Display="Dynamic" Height="16px"><img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" 
                                     CssClass="failureNotification" ErrorMessage="Contraseña requerida." ToolTip="Contraseña requerida." 
                                     ValidationGroup="RegisterUserValidationGroup"><img src="../../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                                    </p>
                                    <p>
                                    <asp:Label ID="lblConfirmarPassword" runat="server" AssociatedControlID="txtConfirmarPassword" Text="<%$ Resources:resCorpusCFDIEs, lblConfirmaNueva %>"></asp:Label>
                                <asp:TextBox ID="txtConfirmarPassword" runat="server" CssClass="passwordEntry" TextMode="Password" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ControlToValidate="txtConfirmarPassword" CssClass="failureNotification" Display="Dynamic" 
                                     ErrorMessage="Es requerida la confirmación de contraseña." ID="rfvConfirmarPassword" runat="server" 
                                     ToolTip="Es requerida la confirmación de contraseña." ValidationGroup="RegisterUserValidationGroup"><img src="../../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmarPassword" 
                                     CssClass="failureNotification" Display="Dynamic" ErrorMessage="No coinciden las contraseñas."
                                     ValidationGroup="RegisterUserValidationGroup"><img src="../../Imagenes/error_sign.gif" />
                                </asp:CompareValidator>
                                    </p>
                                    </div>
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
            <table>
                <tr>
                    <td align="right" style="width: 890px;">
                        <asp:Button ID="btnNuevo" runat="server" CssClass="botonEstiloVentanas" OnClick="btnNuevo_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" Width="80px" />
                            <asp:Button ID="btnGuardarActualizar" runat="server" CommandName="MoveNext" CssClass="botonEstiloVentanas"
                        OnClick="btnGuardarActualizar_Click" TabIndex="14" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                        ValidationGroup="RegisterUserValidationGroup" Enabled="false" />
                         <asp:Button ID="btnEditar" runat="server" CssClass="botonEstiloVentanas" OnClick="btnEditar_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" Width="80px" />
                        <asp:Button ID="btnBorrar" runat="server" CssClass="botonEstiloVentanas" OnClick="btnBorrar_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" Width="80px" 
                            OnClientClick="return confirm('¿Desea eliminar el usuario seleccionado?');"/>
                       
                        <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstiloVentanas" OnClick="btnNCancelar_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" />
                            
                        <%--<asp:Button ID="btnCancelar" runat="server" CssClass="botonEstiloVentanas" OnClick="btnCancelar_Click"
                        TabIndex="15" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Visible="False" />--%>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upFormulario" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlFormulario" runat="server" Height="528px">
                
                    <fieldset class="register" style="height: 450px; width: 890px;">
                        <legend>
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosUsuario %>" /></legend>
                        <table>
                            <tr>
                                <td align="left" style="vertical-align: top; width: 400px;">
                                <asp:HiddenField ID="hdIdEstructura" runat="server" Visible="False" />
                                   <%-- <p>
                                        <asp:Label ID="lblNombreCompleto" runat="server" AssociatedControlID="txtNombre"
                                            Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                                        <asp:TextBox ID="txtNombre" runat="server" CssClass="textEntry" MaxLength="255" TabIndex="1"
                                            Width="200px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired0" runat="server" ControlToValidate="txtNombre"
                                            CssClass="failureNotification" ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>"
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" ValidationGroup="RegisterUserValidationGroup"
                                            Height="21px" Width="16px">*</asp:RequiredFieldValidator>
                                    </p>--%>
                                    <table>
                                    <tr>
                                    <td valign="top">
                                    <table style="height: 314px">
                                        <tr>
                                            <td>
                                                <p>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblNombreSucursal0" runat="server" AssociatedControlID="ddlPerfil"
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblPerfiles %>"/>                    
                                                        </td>
                                                    
                                                        <td>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="inNuevoPerfil" runat="server" style="width: 80px" Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" 
                                                                class="botonEstiloVentanas" />
                                                        </td>
                                                    </tr>
                                                </table>
                                    
                                                            
                                    </p>
                                    <p align="left">
                                        
                                        <asp:DropDownList ID="ddlPerfil" runat="server" AutoPostBack="True" DataTextField="desc_perfil"
                                                                    DataValueField="id_perfil" OnDataBound="ddlPerfil_DataBound" OnSelectedIndexChanged="ddlPerfil_SelectedIndexChanged"
                                                                    TabIndex="5" Width="300px">
                                                                </asp:DropDownList>
                                                                
                                                                <asp:DropDownList ID="ddlPerfilProveedor" runat="server" Enabled="false" Visible="false"  Width="300px">
                                                                    <asp:ListItem Selected="true" Text="Proveedor" Value="0"/>
                                                                </asp:DropDownList>
                                                                <%--<asp:Label ID="lblPerfilProveedor" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varProveedor %>" Visible="false" />--%>
                                                                
                                                                <cc1:ModalPopupExtender ID="mdlPerfiles" runat="server" BackgroundCssClass="modalBackground"
                                                                    DynamicServicePath="" Enabled="True" PopupControlID="pnlNuevoPerfil" TargetControlID="inNuevoPerfil">
                                                                </cc1:ModalPopupExtender>
                                    </p>
                                    
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="PanelGrid" runat="server" Height="285px" ScrollBars="Auto" Width="360px">
                                                        <asp:GridView ID="GrvModulos" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                            BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3"
                                                            DataKeyNames="id_modulo" GridLines="None" OnRowDataBound="GrvModulos_RowDataBound"
                                                            OnSelectedIndexChanged="GrvModulos_SelectedIndexChanged" Visible="False" 
                                                            Width="335px" CellSpacing="1">
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
                                                               <%-- <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbSeleccion" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                                                            </EmptyDataTemplate>
                                                            <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                                            <HeaderStyle BackColor="Maroon" Font-Bold="True" ForeColor="#E7E7FF" />
                                            <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                            <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                                            <SelectedRowStyle BackColor="#FF121C" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                            <SortedAscendingHeaderStyle BackColor="#D30000" />
                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                            <SortedDescendingHeaderStyle BackColor="Maroon" />
                                                        </asp:GridView>
                                                        </p>
                                                    </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                   
                                <%--<p>--%>
                                <%--<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword" 
                                     CssClass="failureNotification" ErrorMessage="Contraseña requerida." ToolTip="Contraseña requerida." 
                                     ValidationGroup="RegisterUserValidationGroup"><img src="../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>--%>
                                
                                   <%-- </p>--%>
                                </td>
                                 <td align="left" valign="top">
                                    <table>
                                        <tr>
                                            <td align="left">
                                                 <br />
                                                    <p>
                                                    <asp:Label ID="lblSucursales" runat="server" AssociatedControlID="ddlPerfil"
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblSucursal%>"></asp:Label>
                                                 </p>
                                                    <p>
                                                    
                                                    <div style="overflow:auto; width: 450px; height:200px">
                                                        <asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                            BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3"
                                                            DataKeyNames="id_sucursal" GridLines="None" OnRowDataBound="GrvModulos_RowDataBound"
                                                            OnSelectedIndexChanged="GrvModulos_SelectedIndexChanged" Visible="False" 
                                                            Width="400px" CellSpacing="1" Height="185px">
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
                                                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblUnica %>" HeaderStyle-Width="100px">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbUnica" runat="server" Enabled="false" 
                                                                        Checked='<%# Bind("factura_unica_mes") %>' />
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
                                            <HeaderStyle BackColor="Maroon" Font-Bold="True" ForeColor="#E7E7FF" />
                                            <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                            <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                                            <SelectedRowStyle BackColor="#FF121C" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                            <SortedAscendingHeaderStyle BackColor="#D30000" />
                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                            <SortedDescendingHeaderStyle BackColor="Maroon" />
                                                        </asp:GridView>
                                                        </div>
                                                        
                                            </td>
                                        </tr>
                                    </table>
                                   
                                    <br />
                                    <br />
                               </td>
                               </tr>
                               </table>
                                
                </td>  </tr>
                
                 </table> </fieldset> 
               <%-- <p style="text-align: right;">
                    &nbsp;</p>
                <p style="text-align: right;">
                    &nbsp;</p>
                <p style="text-align: right;">
                    &nbsp;</p>
                <p style="text-align: right;">
                    &nbsp;</p>
                <p style="text-align: right;">--%>
                
                    
                    
                
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="pnlNuevoPerfil" runat="server">
        <ContentTemplate>
         
         
        <asp:Panel ID="Panel1" runat="server" Height="361px" Width="486px" 
                BackColor="White" BorderStyle="Double" >
               
            <div id="divNuevoPerfil" >
            <br />
            
            <table>
            <tr>
            <td>
                                                    
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblPerfilEditar" runat="server" AssociatedControlID="txtNuevoPerfil" Text="<%$ Resources:resCorpusCFDIEs, lblPerfiles %>"></asp:Label>
                            
                        </td>
                        <td>&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlPerfilEditar" runat="server"  /></td>
                        </tr>
                        <tr>
                        <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbNuevoPerfil" runat="server" AssociatedControlID="txtNuevoPerfil" Text="<%$ Resources:resCorpusCFDIEs, lblNombre %>"></asp:Label>
                            
                        </td>
                        <td>&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtNuevoPerfil" runat="server" Width="250px" Enabled="false"></asp:TextBox></td>
                        </tr>
                        <tr>
                        <td colspan="2">
                        &nbsp;&nbsp;
                        <center>
                            <asp:GridView ID="gvModulosNuevoPerfil" runat="server" 
                                AutoGenerateColumns="False" BackColor="White"
                                BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3"
                                DataKeyNames="id_modulo" GridLines="None" OnRowDataBound="GrvModulos_RowDataBound"
                                OnSelectedIndexChanged="GrvModulos_SelectedIndexChanged" Visible="False" 
                                Width="335px" CellSpacing="1" Enabled="false">
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
                                <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                <HeaderStyle BackColor="Maroon" Font-Bold="True" ForeColor="#E7E7FF" />
                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                <SelectedRowStyle BackColor="#FF121C" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#D30000" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="Maroon" />
                            </asp:GridView>
                            </center>
                            </td>
                            </tr>
                            <tr>
                            <td colspan="2">
                            <center>
                            <table style="width: 500px;">
                                <tr>
                                    <td align="center">
                                    <asp:Button ID="btnNuevoPerfil" CssClass="botonEstiloVentanas"
                                                runat="server" Width="80px" Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" 
                                            onclick="btnNuevoPerfil_Click"  />
                                            <asp:Button ID="btnGuardarNuevoPerfil" CssClass="botonEstiloVentanas"
                                                runat="server" Width="80px" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                                            onclick="btnGuardarNuevoPerfil_Click" Enabled="false" />
                                            <asp:Button ID="btnEditarPerfil" CssClass="botonEstiloVentanas"
                                                runat="server" Width="80px" Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" onclick="btnEditarPerfil_Click" 
                                                />
                                            <asp:Button ID="btnBorrarPerfil" CssClass="botonEstiloVentanas"
                                                runat="server" Width="80px" Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" onclick="btnBorrarPerfil_Click" 
                                                OnClientClick="return confirm('¿Desea eliminar el perfil seleccionado?');"/>
                                                                    
                                            <asp:Button ID="btnCancelarPerfil" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" runat="server"
                                                CssClass="botonEstiloVentanas" onclick="btnCancelarPerfil_Click" />
                                                                        
                                    </td>
                                </tr>
                            </table>
                            </center>
                            </td>
                            </tr>
                            <tr>
                            <td>
                            <center>
                                <asp:Label ID="lblMensajePerfil" runat="server" ForeColor="Red"/>
                            </center>
                            <br /><br />
                            </td>
                            </tr>
                        </table>
                    </div> </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

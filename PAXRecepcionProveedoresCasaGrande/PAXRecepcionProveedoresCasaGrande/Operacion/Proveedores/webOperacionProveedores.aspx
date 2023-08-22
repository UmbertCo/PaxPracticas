<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webOperacionProveedores.aspx.cs" Inherits="Operacion_Proveedores_webOperacionProveedores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">
        .style2
        {
            width: 955px;
        }
        .style3
        {
            width: 155px;
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
</script> 
<br /><br /><br />
     <h2>
        <asp:Label ID="lblTitulo" runat="server" 
             Text="<%$ Resources:resCorpusCFDIEs, lblCatalogoProveedores %>" 
             Font-Bold="True" ForeColor="#8B181B"></asp:Label>
    </h2>
    <table style="border: 2px ridge #8B181B" class="bodyMain">
    <tr>
        <td>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <%--<asp:Panel ID="pnlProvs" runat="server" ScrollBars="Vertical" Height="100px">--%>
    
    <br />
    
    <div>
        <table style="width:100%; height:100%;" >
                <tr>
                    <td class="style2">
                        <table width: 941px;">
                            <tr>
                       <td rowspan="50" width="120px">
                                &nbsp;</td>
                                <td align="left">
                                    *<asp:Label ID="lblNombre" runat="server" 
                                    AssociatedControlID="txtNombre" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                                </td>
                                <td></td>
                                <td rowspan="50" width="120px">
                                &nbsp;</td>
                                 <td align="left">
                                    *<asp:Label ID="lblContacto" runat="server" 
                                    AssociatedControlID="txtContacto" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblContacto %>"></asp:Label>
                                </td>
                                <td></td>
                                <td></td><td></td><td rowspan="19" width="120px"></td>
                            </tr>
                            <tr>
                        
                                <td align="left">
                                    <asp:TextBox ID="txtNombre" runat="server" Width="220px" TabIndex="1"></asp:TextBox>
                                    
                        
                                </td>
                                <td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                ControlToValidate="txtNombre" CssClass="failureNotification" Display="Dynamic" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                                src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator></td>
                                <td align="left">
                                    <asp:TextBox ID="txtContacto" runat="server" Width="220px" TabIndex="2"></asp:TextBox>
                                    
                                </td>
                                <td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                ControlToValidate="txtContacto" CssClass="failureNotification" Display="Dynamic" 
                                                ErrorMessage="Contacto requerido" 
                                                ToolTip="Contacto requerido" 
                                                ValidationGroup="RegisterUserValidationGroup">
                                                <img src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator></td><td></td><td></td>
                                <tr>
                                    <td align="left"><asp:Label ID="lblPais" runat="server"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblPais %>" 
                                        AssociatedControlID="ddlPais" ></asp:Label></td>
                                        <td></td>
                                    <td align="left"><asp:Label ID="lblEstado" runat="server"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>" 
                                        AssociatedControlID="ddlEstado" ></asp:Label></td>
                                        <td></td><td></td><td></td>
                                </tr>
                                <tr>
                                    <td align="left"><asp:DropDownList ID="ddlPais" runat="server" Width="223px" 
                                    OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" AutoPostBack="true" TabIndex="3">
                                    </asp:DropDownList></td>
                                    <td></td>
                                    <td align="left"><asp:DropDownList ID="ddlEstado" runat="server" Width="223px" 
                                    OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" AutoPostBack="true" TabIndex="4">
                                    </asp:DropDownList></td>
                                    <td></td><td></td><td></td>
                                </tr>
                                <tr>
                                    <td align="left"><asp:Label ID="lblMunicipio" runat="server"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>" 
                                        AssociatedControlID="ddlMunicipio" ></asp:Label></td>
                                        <td></td>
                                    <td align="left"><asp:Label ID="lblLocalidad" runat="server"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>" 
                                        AssociatedControlID="txtLocalidad" ></asp:Label></td>
                                        <td></td><td></td><td></td>
                                </tr>
                                <tr>
                                    <td align="left"><asp:DropDownList ID="ddlMunicipio" runat="server" Width="223px" TabIndex="5" >
                                    </asp:DropDownList></td>
                                    <td></td>
                                    <td align="left"><asp:TextBox ID="txtLocalidad" runat="server" Width="220px" TabIndex="6"></asp:TextBox></td>
                                    <td></td><td></td><td></td>
                                </tr>
                                <tr>
                                    <td align="left">*<asp:Label ID="lblColonia" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>"
                                     AssociatedControlID="txtColonia"></asp:Label></td>
                                     <td></td>
                                    <td align="left">*<asp:Label ID="lblCalle" runat="server" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>" 
                                        AssociatedControlID="txtCalle"></asp:Label></td>
                                        <td></td><td></td><td></td>
                                </tr>
                                <tr>
                                    <td align="left"><asp:TextBox ID="txtColonia" runat="server" Width="220px" TabIndex="7"></asp:TextBox>
                                    
                                    </td>
                                    <td><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                    ControlToValidate="txtColonia" CssClass="failureNotification" Display="Dynamic" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtColonia %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, txtColonia %>" 
                                    ValidationGroup="RegisterUserValidationGroup"><img 
                                    src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator></td>  
                                    <td align="left"><asp:TextBox ID="txtCalle" runat="server" Width="220px" TabIndex="8"></asp:TextBox></td>
                                    <td align="left"><asp:RequiredFieldValidator ID="rfvCalle" runat="server" 
                                        ControlToValidate="txtCalle" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>" 
                                        ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                                    src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator></td>
                                                    <td></td><td></td>
                                </tr>
                                <tr>
                                    <td align="left"><asp:Label ID="lblNoExterior" runat="server"  
                                        Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>" 
                                        AssociatedControlID="txtNoExterior"></asp:Label></td>
                                        <td></td>
                                    <td align="left"><asp:Label ID="lblCodigoPostal" runat="server"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>" 
                                        AssociatedControlID="txtCodigoPostal" ></asp:Label></td>
                                        <td></td><td></td><td></td>
                                </tr>
                                <tr>
                                    <td align="left"><asp:TextBox ID="txtNoExterior" runat="server" TabIndex="9" Width="220px"></asp:TextBox></td>
                                    <td></td>
                                    <td align="left"><asp:TextBox ID="txtCodigoPostal" runat="server" TabIndex="11" Width="220px"></asp:TextBox></td>
                                    <td align="left"><asp:RegularExpressionValidator ID="revCodigoPostal" runat="server" 
                                    ControlToValidate="txtCodigoPostal" CssClass="failureNotification" 
                                    ToolTip="Código postal erroneo" ValidationGroup="RegisterUserValidationGroup"
                                     ValidationExpression="\d{5}">
                                    <img src="../../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator></td>
                                    <td></td><td></td>
                                </tr>
                                <tr>
                                    <td align="left"><asp:Label ID="lblNoInterior" runat="server"  
                                        Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>" 
                                        AssociatedControlID="txtNoInterior"></asp:Label></td>
                                        <td></td>
                                    <td align="left">*<asp:Label ID="lblCorreo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>" AssociatedControlID="txtCorreo"/></td>
                                    <td></td><td></td><td></td>
                                </tr>
                                <tr>
                                    <td align="left"><asp:TextBox ID="txtNoInterior" runat="server" TabIndex="10" Width="220px"></asp:TextBox></td>
                                    <td></td>
                                    <td align="left"><asp:TextBox ID="txtCorreo" runat="server" Width="220px" TabIndex="12"></asp:TextBox></td>
                                    <td align="left"><asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                            ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                            ValidationGroup="RegisterUserValidationGroup"><img 
                                            src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator></td>
                                    <td align="left"><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                            ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                            ValidationGroup="RegisterUserValidationGroup" Width="131px"><img 
                                            src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator></td>
                                            <td></td>
                                </tr>
                                <tr>
                                    <td align="left">*<asp:Label ID="lblTelefono" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTelefono %>" AssociatedControlID="txtTelefono" /></td>
                                    <td></td>
                                    <td align="left"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosUsuario %>"/></td>
                                    <td></td><td></td><td></td>
                                </tr>
                                <tr>
                                <td align="left"><asp:TextBox ID="txtTelefono" runat="server"  Width="220px" TabIndex="13"></asp:TextBox></td>
                                <td align="left"> <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" 
                                            ControlToValidate="txtTelefono" CssClass="failureNotification" Display="Dynamic" 
                                            ToolTip="Telefono Requerido" 
                                            ValidationGroup="RegisterUserValidationGroup"><img 
                                            src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator> </td>
                                <td align="left"><asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label></td>
                                        <td></td><td></td><td></td>
                                </tr>
                                <tr>
                                <td></td>
                                <td></td>
                                <td align="left"><asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry" MaxLength="50" TabIndex="14"
                                             Width="220px"></asp:TextBox></td>
                                <td align="left"><asp:RegularExpressionValidator ID="regxNueva" runat="server" ControlToValidate="txtUsuario"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtUsuario %>"
                                            ValidationExpression="(?=^.{8,}$).*$" ValidationGroup="RegisterUserValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /> 
                                        </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvUsuario" ControlToValidate="txtUsuario" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="Campo Requerido." 
                                            ValidationGroup="RegisterUserValidationGroup"><img src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator> 
                                        </td>
                                        <td></td><td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td align="left"><asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="<%$ Resources:resCorpusCFDIEs, lblContrasenaCorreo %>"></asp:Label></td>
                                    <td></td><td></td><td></td>
                                </tr>
                                    <tr>
                                    <td></td>
                                    <td></td>
                                    <td align="left"><asp:TextBox ID="txtPassword" runat="server" CssClass="passwordEntry" 
                                            TextMode="Password" TabIndex="15" Width="220px"></asp:TextBox></td>
                                    <td><asp:CompareValidator ID="PasswordCompare" runat="server" 
                                            ControlToCompare="txtPassword" ControlToValidate="txtConfirmarPassword" 
                                            CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="No coinciden las contraseñas." 
                                            ValidationGroup="RegisterUserValidationGroup"><img src="../../Imagenes/error_sign.gif" />
                                </asp:CompareValidator>
                                <asp:RequiredFieldValidator runat="server" ID="rfvPassword" ControlToValidate="txtPassword" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="Campo requerido." 
                                            ValidationGroup="RegisterUserValidationGroup"><img src="../../Imagenes/error_sign.gif"  /></asp:RequiredFieldValidator> 
                                </td><td></td><td></td>
                                </tr>
                                    <tr>
                                    <td></td>
                                    <td></td>
                                    <td align="left"><asp:Label ID="lblConfirmarPassword" runat="server" AssociatedControlID="txtConfirmarPassword" Text="<%$ Resources:resCorpusCFDIEs, lblConfirmaNueva %>"></asp:Label></td>
                                    <td></td><td></td><td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td align="left"><asp:TextBox ID="txtConfirmarPassword" runat="server" CssClass="passwordEntry" 
                                            TextMode="Password"    Width="220px" TabIndex="16"></asp:TextBox></td>
                                    <td><asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                    ControlToValidate="txtPassword" ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"
                                    ValidationGroup="RegisterUserValidationGroup" 
                                    CssClass="failureNotification" 
                                    ToolTip="Contraseña incompleta" Display="Dynamic" Height="16px"><img src="../Imagenes/error_sign.gif" width="20px"/> </asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvConfirmarPassword" ControlToValidate="txtConfirmarPassword" CssClass="failureNotification" Display="Dynamic" 
                                            ErrorMessage="Campo requerido." 
                                            ValidationGroup="RegisterUserValidationGroup"><img src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator> 
                                    </td><td></td><td></td>
                                </tr>
                            </tr>
                        </table>
                        <table align="center" 
                            style="border-top-style: ridge; border-width: 1px; border-color: #8B181B">
                            <tr>
                                <td align="center"><asp:Label ID="lblSeleccionSucursal" runat="server" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionSucursal %>" Font-Bold="True" 
                                        ForeColor="#8B181B" />
                                    <div style="overflow:auto; width: 630px; height:200px">
                                        <asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False" 
                                            BackColor="White" BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" 
                                            CellPadding="3" CellSpacing="1" DataKeyNames="id_sucursal" GridLines="None" 
                                            OnRowDataBound="GrvModulos_RowDataBound" 
                                            OnSelectedIndexChanged="GrvModulos_SelectedIndexChanged" Visible="False" 
                                            Width="600px" TabIndex="17" Height="1px">
                                            <Columns>
                                                <asp:TemplateField Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblidsucursal" runat="server" Text='<%# Bind("id_sucursal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblSucursal %>" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="empresa_sucursal" runat="server" 
                                                            Text='<%# Bind("empresa_sucursal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblUnica %>" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbUnica" runat="server" Enabled="false" 
                                                        Checked='<%# Bind("factura_unica_mes") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbSeleccion" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Literal ID="Literal3" runat="server" 
                                                    Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
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
                                    </div></td>
                            </tr>
                        </table>
                
                    </td>
                    <td>
                
                    </td>
                </tr>
        
        
                <tr>
                    <td align="left" class="style2">
                                </td>
                                <td>
                                 
                                </td>
                            </tr>
                        <tr>
                            
                            <td align="right">
                            <div>
        <asp:Button ID="btnNuevo" runat="server" CssClass="botonEstiloVentanas" OnClick="btnNuevo_Click"
            Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" Width="80px" />
            <asp:Button ID="btnGuardarEmpresa" runat="server" CssClass="botonEstiloVentanas" 
                                    onclick="btnGuardarEmpresa_Click" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                                    ValidationGroup="RegisterUserValidationGroup" Width="80px" TabIndex="18" />
            <asp:Button ID="btnEditar" runat="server" CssClass="botonEstiloVentanas" OnClick="btnEditar_Click"
            Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" Width="80px" />
        <asp:Button ID="btnBorrar" runat="server" CssClass="botonEstiloVentanas" OnClick="btnBorrar_Click"
            Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" Width="80px" 
            OnClientClick="return confirm('¿Desea eliminar el proveedor seleccionado?');"/>
        
             
        <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstiloVentanas" OnClick="btnNCancelar_Click"
            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" />
    </div>
                               
                            </td>
                </tr>
                        </table>
    </div>
    <table align="center" 
            style="border-top-style: ridge; border-width: 1px; border-color: #8B181B">
    <tr>
        <td>
            <asp:Label ID="lblNombreBuscar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNombre %>" />    
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtNombreBuscar" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
      
            
            
                <asp:Label ID="lblLineasPagina" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblLineasPagina %>" ></asp:Label>
                                                <asp:DropDownList ID="ddlLineasPagina" runat="server" >
                                                    <asp:ListItem Selected="true" Text="10" />
                                                    <asp:ListItem Text="20" />
                                                    <asp:ListItem Text="30" />
                                                    <asp:ListItem Text="40" />
                                                    <asp:ListItem Text="50" />
                                                </asp:DropDownList>
            <asp:Button ID="btnBuscar" runat="server" CssClass="botonEstiloVentanas" 
                Text="<%$ Resources:resCorpusCFDIEs, btnBuscar %>" onclick="btnBuscar_Click" />
        </td>
       
    </tr>
    <tr>
    <td align="left">
    
    <%--<input type="hidden" id="scrollPos2" name="scrollPos" value="0" runat="server"/>--%>
    <table align="center">
        <tr>
        <%--<td rowspan="50" width="60px">
                                &nbsp;</td>--%>
            <td align="left">
                <div>
    <asp:GridView ID="gvProveedores" runat="server" BackColor="White" 
         BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" 
         CellSpacing="1" GridLines="None"  AutoGenerateColumns="false" 
         OnSelectedIndexChanged="gvProveedores_SelectedIndexChanged" 
            OnRowDataBound="gvProveedores_RowDataBound" Width="816px"  HorizontalAlign="Left">
         <Columns>
         <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton ID="SelectButton" runat="server" CommandName="Select" Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionar %>" />
            </ItemTemplate>
         </asp:TemplateField>
            <asp:TemplateField Visible="False">
                <ItemTemplate>
                    <asp:Label ID="lblGvIdProveedor" runat="server" Text='<%# Bind("id_proveedor") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="lblGvNombre" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblContacto %>" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="lblGvContacto" runat="server" Text='<%# Bind("contacto") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblCorreo %>" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="lblGvEmail" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblTelefono %>" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="lblGvTelefono" runat="server" Text='<%# Bind("telefono") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblUser %>" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="lblGvUsuario" runat="server" Text='<%# Bind("usuario") %>'></asp:Label>
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
    
    </td>
    </tr>
    </table>
    <table  align="right">
    
                        <tr>
                            <td>
                                <asp:Button ID="btnAnterior" runat="server" CssClass="botonEstiloVentanas" Text="<%$ Resources:resCorpusCFDIEs, lbAnterior %>"
                                    Visible="False" OnClick="btnAnterior_Click" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnSiguiente" runat="server" CssClass="botonEstiloVentanas" Text="<%$ Resources:resCorpusCFDIEs, lblSiguiente %>"
                                    Visible="False" OnClick="btnSiguiente_Click" />
                            </td>
                        </tr>
                    </table>
    <%--</asp:Panel>--%>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvProveedores" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="btnGuardarEmpresa" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>
        
        </td>
    </tr>
    </table>
</asp:Content>


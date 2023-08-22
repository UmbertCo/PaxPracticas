<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionUsuarios.aspx.cs" Inherits="Operacion_Usuarios_webOperacionUsuarios" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varCatalogoUsuarios %>"></asp:Label>
    </h2>
    <div style="text-align:center;" >
            <asp:UpdateProgress ID="uppConsultas" runat="server"  >
                    <progresstemplate>
                        <img alt="" 
                    src="../../Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>
    </div>
    <br />
    <asp:UpdatePanel ID="udpEstructura" runat="server">
    <ContentTemplate>
    <fieldset class="register" style=" width:890px;">
    <legend><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, mnuUsuarios %>" /></legend>
    <table>
        <tr>
            <td>
                <table width="870">
                    <tr>
                        <td>
                            <asp:TreeView ID="trvEstructura" runat="server" BorderStyle="Solid" 
                                BorderWidth="1px" ImageSet="News" 
                                onselectednodechanged="trvEstructura_SelectedNodeChanged" 
                                Width="300px" NodeIndent="10" ShowLines="True">
                                <HoverNodeStyle Font-Underline="True" />
                                <NodeStyle Font-Names="Century Gothic" Font-Size="11pt" ForeColor="Blue" 
                                    HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                <ParentNodeStyle Font-Bold="False" />
                                <RootNodeStyle ImageUrl="~/Imagenes/logo_pax_40.png" />
                                <SelectedNodeStyle Font-Underline="True" 
                                    HorizontalPadding="0px" VerticalPadding="0px" BackColor="#FF3300" />
                            </asp:TreeView>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNodoSel" runat="server" style="font-weight: 700" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblNodoSel %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSelVal" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblNinguno %>"></asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNombreNodo" runat="server" style="font-weight: 700" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblNodoNombre %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNombreNodo" runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegxNombre" runat="server" 
                                            ControlToValidate="txtNombreNodo" CssClass="failureNotification" Display="Dynamic" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, txtUsuario %>" 
                                            ValidationExpression="(?=^.{8,}$).*$" 
                                            ValidationGroup="AgregarNodo"><img 
                                            src="../../Imagenes/error_sign.gif" /> 
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAgregar" runat="server" CssClass="botonEstilo" 
                                            onclick="btnAgregar_Click" Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
                                            ValidationGroup="AgregarNodo" Width="80px" />
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

    <table>
    <tr>
        <td align="right" style="width:890px;">
         <asp:Button ID="btnNuevo" runat="server" CssClass="botonEstilo" 
         onclick="btnNuevo_Click" Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" Width="80px" />
         <asp:Button ID="btnBorrar" runat="server" CssClass="botonEstilo" 
         onclick="btnBorrar_Click" Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" 
         Width="80px" />
         <asp:Button ID="btnEditar" runat="server" CssClass="botonEstilo" 
         onclick="btnEditar_Click" Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
         Width="80px" />
         <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" 
         onclick="btnNCancelar_Click" 
         Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" />        
        </td>
    </tr>
    </table>    

    </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upFormulario" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlFormulario" runat="server">
            <div class="accountInfo">
            <fieldset class="register" style=" height:450px; width:890px;">
            <legend><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosUsuario %>" /></legend>
            
            <table><tr><td style="vertical-align:top; width:400px;">
                    <p>
                        <asp:Label ID="lblNombreCompleto" runat="server" 
                            AssociatedControlID="txtNombre" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="textEntry" MaxLength="255" 
                            TabIndex="1" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired0" runat="server" 
                            ControlToValidate="txtNombre" CssClass="failureNotification" 
                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                            ValidationGroup="RegisterUserValidationGroup" Height="21px" Width="16px">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label>
                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry" MaxLength="50" 
                            TabIndex="2" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                            ControlToValidate="txtUsuario" CssClass="failureNotification" Display="Dynamic" 
                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                            ValidationGroup="RegisterUserValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regxNueva" runat="server" 
                            ControlToValidate="txtUsuario" CssClass="failureNotification" Display="Dynamic" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, txtUsuario %>" 
                            ValidationExpression="(?=^.{8,}$).*$" 
                            ValidationGroup="RegisterUserValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /> 
                        </asp:RegularExpressionValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblCorreo" runat="server" AssociatedControlID="txtCorreo" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"></asp:Label>
                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="textEntry" MaxLength="50" 
                            TabIndex="3" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                            ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                            ValidationGroup="RegisterUserValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                            ValidationGroup="RegisterUserValidationGroup" Width="131px"><img 
                            src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                    </p>
                    <asp:HiddenField ID="hdIdEstructura" runat="server" Visible="False" />
            </td><td>
                    <p>
                        <asp:Label ID="lblNombreSucursal" runat="server" 
                            AssociatedControlID="ddlSucursales" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>"></asp:Label>
                        <asp:DropDownList ID="ddlSucursales" runat="server" DataTextField="nombre" 
                            DataValueField="id_estructura" TabIndex="4" Width="300px">
                        </asp:DropDownList>
                    </p>
                    <p>
                        <asp:Label ID="lblNombreSucursal0" runat="server" 
                            AssociatedControlID="ddlSucursales" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblPerfiles %>"></asp:Label>
                    </p>
                    <p>
                        <asp:DropDownList ID="ddlPerfil" runat="server" AutoPostBack="True" 
                            DataTextField="desc_perfil" DataValueField="id_perfil" 
                            ondatabound="ddlPerfil_DataBound" 
                            onselectedindexchanged="ddlPerfil_SelectedIndexChanged" TabIndex="5" 
                            Width="300px">
                        </asp:DropDownList>
                    </p>
                    &nbsp;&nbsp;
                    <asp:Panel ID="PanelGrid" runat="server" Height="285px" ScrollBars="Auto" 
                        Width="360px">
                        <asp:GridView ID="GrvModulos" runat="server" AutoGenerateColumns="False" 
                            BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                            CellPadding="4" DataKeyNames="id_modulo" GridLines="Horizontal" 
                            onrowdatabound="GrvModulos_RowDataBound" 
                            onselectedindexchanged="GrvModulos_SelectedIndexChanged" Visible="False" 
                            Width="335px">
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
                                <asp:Literal ID="Literal3" runat="server" 
                                    text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                            </EmptyDataTemplate>
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
                        </asp:GridView>
                    </asp:Panel>
                    <br />
                    <br />
            </td></tr></table>
            </fieldset>
            </div>
            <p style="text-align:right;" >
                &nbsp;</p>
                <p style="text-align:right;">
                    &nbsp;</p>
                <p style="text-align: right;">
                    &nbsp;</p>
                <p style="text-align: right;">
                    &nbsp;</p>
                <p style="text-align: right;">
                    <asp:Button ID="btnGuardarActualizar" runat="server" CommandName="MoveNext" 
                        CssClass="botonEstilo" onclick="btnGuardarActualizar_Click" TabIndex="14" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                        ValidationGroup="RegisterUserValidationGroup" />
                    <asp:Button ID="btnCancelar" runat="server" CssClass="botonEstilo" 
                        onclick="btnCancelar_Click" TabIndex="15" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Visible="False" />
                </p>
            </asp:Panel>
            </ContentTemplate>
    </asp:UpdatePanel>
<br />
</asp:Content>


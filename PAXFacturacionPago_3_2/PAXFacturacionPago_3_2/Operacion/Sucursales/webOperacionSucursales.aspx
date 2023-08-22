<%@ Page Title="Estructuras" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionSucursales.aspx.cs" Inherits="Operacion_Sucursales_webOperacionSucursales" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloSucursales %>"></asp:Label>
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
    <legend><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEstructura %>" /></legend>
    <table>
        <tr>
            <td>
                <table width="870">
                    <tr>
                        <td colspan="0" rowspan="0">
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
                                            Text="<%$ Resources:resCorpusCFDIEs, lblNodoSelEstructura %>"></asp:Label>
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
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAgregar" runat="server" CssClass="botonEstilo" 
                                            onclick="btnAgregar_Click" Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
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
                    <tr>
                        <td colspan="0">
                            <table>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                        <td>
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
           Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" 
             onclick="btnNuevo_Click" Width="80px" />
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
            <div class="">
            <fieldset class="register" style=" height:350px; width:890px;">
            <legend><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubSucursales %>" /></legend>
            <table><tr><td style="vertical-align:top; width:400px;">
                <asp:HiddenField ID="hdIdEstructura" runat="server" Visible="False" />
                    <p>
                        <asp:Label ID="lblSucursal" runat="server" AssociatedControlID="txtSucursal" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>" ></asp:Label>
                        <asp:TextBox ID="txtSucursal" runat="server" CssClass="textEntry" 
                            MaxLength="255" TabIndex="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSucursal" runat="server" 
                            ControlToValidate="txtSucursal" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                            ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblPais" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblPais %>" 
                            AssociatedControlID="ddlPais" ></asp:Label>
                        <asp:DropDownList ID="ddlPais" runat="server" Width="300px" AutoPostBack="True" 
                            DataTextField="pais" DataValueField="id_pais" 
                            onselectedindexchanged="ddlPais_SelectedIndexChanged" TabIndex="4"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvPais" runat="server" 
                            ControlToValidate="ddlPais" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valPais %>" 
                            ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                    </p><p>
                        <asp:Label ID="lblEstado" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>" 
                            AssociatedControlID="ddlEstado" ></asp:Label>
                            <asp:DropDownList ID="ddlEstado" runat="server" Width="300px" 
                            DataTextField="estado" DataValueField="id_estado" TabIndex="5"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvEstado" runat="server" 
                            ControlToValidate="ddlEstado" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valEstado %>" 
                            ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblMunicipio" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>" 
                            AssociatedControlID="txtMunicipio" ></asp:Label>
                        <asp:TextBox ID="txtMunicipio" runat="server" CssClass="textEntry" 
                            MaxLength="255" TabIndex="6"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMunicipio" runat="server" 
                            ControlToValidate="txtMunicipio" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valMunicipio %>" 
                            ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                    </p><p>
                        <asp:Label ID="lblLocalidad" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>" 
                            AssociatedControlID="txtLocalidad" ></asp:Label>
                        <asp:TextBox ID="txtLocalidad" runat="server" CssClass="textEntry" 
                            MaxLength="255" TabIndex="7"></asp:TextBox>
                    </p>
            </td><td style="vertical-align:top; width:400px;">
                    <p>
                        <asp:Label ID="lblCalle" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>" 
                            AssociatedControlID="txtCalle"></asp:Label>
                        <asp:TextBox ID="txtCalle" runat="server" CssClass="textEntry" MaxLength="255" 
                            TabIndex="8"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCalle" runat="server" 
                            ControlToValidate="txtCalle" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>" 
                            ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblNoExterior" runat="server"  
                            Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>" 
                            AssociatedControlID="txtNoExterior"></asp:Label>
                        <asp:TextBox ID="txtNoExterior" runat="server" CssClass="textEntry" 
                            MaxLength="10" TabIndex="9"></asp:TextBox>
                    </p>
                    <p>
                        <asp:Label ID="lblNoInterior" runat="server"  
                            Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>" 
                            AssociatedControlID="txtNoInterior"></asp:Label>
                        <asp:TextBox ID="txtNoInterior" runat="server" CssClass="textEntry" 
                            MaxLength="10" TabIndex="10"></asp:TextBox>
                    </p>
                    <p>
                        <asp:Label ID="lblColonia" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>" 
                            AssociatedControlID="txtColonia"></asp:Label>
                        <asp:TextBox ID="txtColonia" runat="server" CssClass="textEntry" 
                            MaxLength="255" TabIndex="11"></asp:TextBox>
                    </p>
                    <p>
                        <asp:Label ID="lblReferenciaDom" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblReferenciaDom %>" 
                            AssociatedControlID="txtReferencia" ></asp:Label>
                        <asp:TextBox ID="txtReferencia" runat="server" CssClass="textEntry" 
                            MaxLength="255" TabIndex="12"></asp:TextBox>
                    </p>
                    <p>
                        <asp:Label ID="lblCodigoPostal" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>" 
                            AssociatedControlID="txtCodigoPostal" ></asp:Label>
                        <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="textEntry" 
                            MaxLength="5" TabIndex="13"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtCodigoPostal_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" 
                            TargetControlID="txtCodigoPostal">
                        </cc1:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="rfvCodigoPostal" runat="server" 
                            ControlToValidate="txtCodigoPostal" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCodigoPostal %>" 
                            ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                    </p>
            </td></tr></table>
            </fieldset>
            </div>
            <p style="text-align:right;" >
                <asp:Button ID="btnGuardarActualizar" runat="server" CommandName="MoveNext" 
                ValidationGroup="RegisterUserValidationGroup" 
                Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                    TabIndex="14" onclick="btnGuardarActualizar_Click" 
                    CssClass="botonEstilo" />
                <asp:Button ID="btnCancelar" runat="server" 
                Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                    onclick="btnCancelar_Click" TabIndex="15" Visible="False" 
                    CssClass="botonEstilo" />
            </p>
            </asp:Panel>
            </ContentTemplate>
    </asp:UpdatePanel>
    <div style="padding-left:2px;">
        <asp:UpdatePanel ID="upBorrados" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlBorrados" runat="server" Visible="False">
            <fieldset class="register" style=" height:50px; width:890px;">
            <legend><asp:Literal  runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblReactivarSucursal %>" /></legend>
                <p>
                    <asp:DropDownList ID="ddlSucursalesBorradas" runat="server" Width="300px" 
                        DataTextField="nombre" DataValueField="id_estructura" TabIndex="16">
                    </asp:DropDownList>
                </p>
            </fieldset>
            <p style="text-align:right;" >
                <asp:Button ID="btnReactivar" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnReactivar %>" 
                        onclick="btnReactivar_Click" TabIndex="17" CssClass="botonEstilo" />
            </p>
            </asp:Panel>
        </ContentTemplate>
        </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="updGuardar" runat="server" Visible="False">
        <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal" Width="920px" >
                <asp:GridView ID="gdvSucursales" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" GridLines="Horizontal" Width="920px" 
                    DataKeyNames="id_estructura" 
                    onselectedindexchanged="gdvSucursales_SelectedIndexChanged" 
                    onrowdeleting="gdvSucursales_RowDeleting" AllowPaging="True" 
                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                    onpageindexchanging="gdvSucursales_PageIndexChanging" BackColor="White">
                    <Columns>
                        <asp:CommandField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                            SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>"
                            ShowSelectButton="True" HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:CommandField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblSucursal" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="300px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblCalle %>" 
                            Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblCalle" runat="server" Text='<%# Bind("calle") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>" 
                            Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblNoExterior" runat="server" Text='<%# Bind("numero_exterior") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField  HeaderText="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>" 
                            Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblNoInterior" runat="server" Text='<%# Bind("numero_interior") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblColonia %>" 
                            Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblColonia" runat="server" Text='<%# Bind("colonia") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblReferenciaDom %>" 
                            Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblReferencia" runat="server" Text='<%# Bind("referencia") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>" 
                            Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblLocalidad" runat="server" Text='<%# Bind("localidad") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblMunicipio" runat="server" Text='<%# Bind("municipio") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstado %>" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblEstado" runat="server" Text='<%# Bind("estado") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblPais %>" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblPais" runat="server" Text='<%# Bind("pais") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>" HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblCodigoPostal" runat="server" Text='<%# Bind("codigo_postal") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="True" DeleteText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                            HeaderStyle-HorizontalAlign="Left" Visible="False" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:CommandField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:literal ID="Literal2" runat="server" text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                    </EmptyDataTemplate>
                    <FooterStyle BackColor="White" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5A737E" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="White" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#92BA41" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#5A737E" />
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<br />
    
<br />
</asp:Content>


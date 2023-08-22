<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionClienteSuc.aspx.cs" Inherits="Operacion_Clientes_webOperacionClienteSuc" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloSucReceptores %>"></asp:Label>
    </h2>
    <asp:UpdatePanel ID="upFormulario" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hdIdEstructura" runat="server" Visible="False"></asp:HiddenField>
        <asp:Panel ID="pnlFormulario" runat="server" >
        <fieldset class="register" style=" height:350px; width:890px;">
        <legend><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubReceptorSuc %>" /></legend>
            <table>
            <tr>
            <td style="width:400px; vertical-align:top;" >
                <p>
                    <asp:Label ID="lblDropRfc" runat="server" AssociatedControlID="txtReceptor" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblReceptor %>"></asp:Label>
                    <asp:TextBox ID="txtReceptor" runat="server" CssClass="textEntry" 
                        Enabled="False"></asp:TextBox>
                    &nbsp;</p><p>
                    <asp:Label ID="lblSucursal" runat="server" AssociatedControlID="txtSucursal" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblSucursal %>"></asp:Label>
                    <asp:TextBox ID="txtSucursal" runat="server" CssClass="textEntry" 
                        MaxLength="255" TabIndex="6"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSucursal" runat="server" 
                        ControlToValidate="txtSucursal" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator></p><p>
                    <asp:Label ID="lblPais" runat="server" AssociatedControlID="txtPais" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"></asp:Label>
                    <asp:TextBox ID="txtPais" runat="server" CssClass="textEntry" 
                        MaxLength="255" TabIndex="7"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPais" runat="server" 
                        ControlToValidate="txtPais" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valPais %>" 
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator></p><p>
                    <asp:Label ID="lblEstado" runat="server" AssociatedControlID="txtEstado" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"></asp:Label>
                    <asp:TextBox ID="txtEstado" runat="server" CssClass="textEntry" 
                        MaxLength="255" TabIndex="8"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEstado" runat="server" 
                        ControlToValidate="txtEstado" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valEstado %>" 
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator></p><p>
                    <asp:Label ID="lblMunicipio" runat="server"
                        Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>" 
                        AssociatedControlID="txtMunicipio" ></asp:Label>
                    <asp:TextBox ID="txtMunicipio" runat="server" CssClass="textEntry" 
                        MaxLength="255" TabIndex="9"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMunicipio" runat="server" 
                        ControlToValidate="txtMunicipio" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valMunicipio %>" 
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator></p><p>
                    <asp:Label ID="lblLocalidad" runat="server"
                        Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>" 
                        AssociatedControlID="txtLocalidad" ></asp:Label>
                    <asp:TextBox ID="txtLocalidad" runat="server" CssClass="textEntry" 
                        MaxLength="255" TabIndex="10"></asp:TextBox></p>
                    
                </td>
                <td style="width:400px; vertical-align:top;">
                <p>
                    <asp:Label ID="lblCalle" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>" 
                        AssociatedControlID="txtCalle"></asp:Label><asp:TextBox ID="txtCalle" 
                        runat="server" CssClass="textEntry" MaxLength="255" 
                        TabIndex="11"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCalle" runat="server" 
                        ControlToValidate="txtCalle" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>" 
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator></p><p>
                    <asp:Label ID="lblNoExterior" runat="server"  
                        Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>" 
                        AssociatedControlID="txtNoExterior"></asp:Label>
                    <asp:TextBox ID="txtNoExterior" runat="server" CssClass="textEntry" 
                        MaxLength="10" TabIndex="12"></asp:TextBox></p><p>
                    <asp:Label ID="lblNoInterior" runat="server"  
                        Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>" 
                        AssociatedControlID="txtNoInterior"></asp:Label>
                    <asp:TextBox ID="txtNoInterior" runat="server" CssClass="textEntry" 
                        MaxLength="10" TabIndex="13"></asp:TextBox></p><p>
                    <asp:Label ID="lblColonia" runat="server"
                        Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>" 
                        AssociatedControlID="txtColonia"></asp:Label><asp:TextBox ID="txtColonia" 
                        runat="server" CssClass="textEntry" 
                        MaxLength="255" TabIndex="14"></asp:TextBox></p><p>
                    <asp:Label ID="LablblCodigoPostalel5" runat="server"
                        Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>" 
                        AssociatedControlID="txtCodigoPostal" ></asp:Label>
                    <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="textEntry" 
                        MaxLength="5" TabIndex="15"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" TargetControlID="txtCodigoPostal">
                        </cc1:FilteredTextBoxExtender>
                        </p>
                </td></tr>
                </table>
            </fieldset>
            </asp:Panel>
            <asp:UpdatePanel ID="udpControles" runat="server">
                    <ContentTemplate>
                    <p style="text-align:right;" >
                        <asp:Button ID="btnRegresar" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, btnRegresar %>" 
                            onclick="btnRegresar_Click" CssClass="botonEstilo" />
                        <asp:Button ID="btnActualizarDomicilio" runat="server"
                        ValidationGroup="RegisterUserValidationGroup" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                        onclick="btnActualizarDomicilio_Click" TabIndex="16" CssClass="botonEstilo" />
                        <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>"
                         Visible="False" onclick="btnCancelar_Click" TabIndex="17" 
                            CssClass="botonEstilo" />
                    </p>
                    </ContentTemplate>
                    </asp:UpdatePanel>
            </ContentTemplate>
            </asp:UpdatePanel>
            <br />

            <asp:UpdatePanel ID="updGuardar" runat="server">
            <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal" Width="920px" >
            <asp:GridView ID="gdvSucursales" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" GridLines="Horizontal" Width="920px" 
                DataKeyNames="id_estructura" 
                onselectedindexchanged="gdvSucursales_SelectedIndexChanged" 
                onrowdeleting="gdvSucursales_RowDeleting" AllowPaging="True" 
                BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                onpageindexchanging="gdvSucursales_PageIndexChanging" BackColor="White" 
                    AllowSorting="True" onsorting="gdvSucursales_Sorting">
                <Columns>
                    <asp:CommandField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                        SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>"
                        ShowSelectButton="True" HeaderStyle-HorizontalAlign="Left" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:CommandField>
                    <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblSucursal %>" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblSucursal" runat="server" Text='<%# Bind("nombre") %>'></asp:Label></ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="300px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblCalle %>" HeaderStyle-HorizontalAlign="Left"
                        Visible="False" SortExpression="calle">
                        <ItemTemplate>
                            <asp:Label ID="lblCalle" runat="server" Text='<%# Bind("calle") %>'></asp:Label></ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>" HeaderStyle-HorizontalAlign="Left"
                        Visible="False" >
                        <ItemTemplate>
                            <asp:Label ID="lblNoExterior" runat="server" Text='<%# Bind("numero_exterior") %>'></asp:Label></ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>" HeaderStyle-HorizontalAlign="Left"
                        Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblNoInterior" runat="server" Text='<%# Bind("numero_interior") %>'></asp:Label></ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblColonia %>" HeaderStyle-HorizontalAlign="Left"
                        Visible="False" SortExpression="colonia">
                        <ItemTemplate>
                            <asp:Label ID="lblColonia" runat="server" Text='<%# Bind("colonia") %>'></asp:Label></ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>" HeaderStyle-HorizontalAlign="Left"
                        Visible="False" SortExpression="localidad">
                        <ItemTemplate>
                            <asp:Label ID="lblLocalidad" runat="server" Text='<%# Bind("localidad") %>'></asp:Label></ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>" HeaderStyle-HorizontalAlign="Left" SortExpression="municipio"> 
                        <ItemTemplate>
                            <asp:Label ID="lblMunicipio" runat="server" Text='<%# Bind("municipio") %>'></asp:Label></ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstado %>" HeaderStyle-HorizontalAlign="Left" SortExpression="estado">
                        <ItemTemplate>
                            <asp:Label ID="lblEstado" runat="server" Text='<%# Bind("estado") %>'></asp:Label></ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblPais %>" HeaderStyle-HorizontalAlign="Left" SortExpression="pais">
                        <ItemTemplate>
                            <asp:Label ID="lblPais" runat="server" Text='<%# Bind("pais") %>'></asp:Label></ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField><asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>" HeaderStyle-HorizontalAlign="Right" SortExpression="codigo_postal">
                        <ItemTemplate>
                            <asp:Label ID="lblCodigoPostal" runat="server" Text='<%# Bind("codigo_postal") %>'></asp:Label></ItemTemplate>
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField><asp:CommandField ShowDeleteButton="True" DeleteText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" />
                </Columns>
                <EmptyDataTemplate>
                    <asp:literal ID="Literal1" runat="server" text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                </EmptyDataTemplate>
                <FooterStyle BackColor="White" ForeColor="#5A737E" />
                <HeaderStyle BackColor="#5A737E" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="White" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#92BA41" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#487575" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#275353" />
            </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


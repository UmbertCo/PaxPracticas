<%@ Page Title="Clientes" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionClientes.aspx.cs" Inherits="Operacion_Clientes_webOperacionClientes" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloReceptores %>"></asp:Label>
    </h2>
    <asp:UpdatePanel ID="upRFC" runat="server">
    <ContentTemplate>
     <asp:HiddenField ID="hdIdRfc" runat="server" Visible="False"></asp:HiddenField>
            <asp:Panel runat="server" ID="pnlRFC" >
                <table>
                    <tr>
                        <td>
                            <fieldset class="register" style=" height:300px; width:400px;">
                                <legend>
                                    <asp:Literal ID="Literal2" runat="server" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblSubReceptor %>" />
                                </legend>
                                <p>
                                    <asp:Label ID="lblRFC" runat="server" AssociatedControlID="txtRFC" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>"></asp:Label>
                                    <asp:TextBox ID="txtRFC" runat="server" CssClass="textEntry" MaxLength="13" 
                                        TabIndex="1" ToolTip="AAA000000AAA"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regxRFC" runat="server" 
                                        ControlToValidate="txtRFC" CssClass="failureNotification" Display="Dynamic" 
                                        ToolTip="RFC incompleto" 
                                        ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]" 
                                        ValidationGroup="grupoRfc"><img src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfvRfc" runat="server" 
                                        ControlToValidate="txtRFC" CssClass="failureNotification" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, valRfc %>" ValidationGroup="grupoRfc" 
                                        Width="16px">*</asp:RequiredFieldValidator>
                                </p>
                                <p>
                                    <asp:Label ID="lblRazonSocial" runat="server" 
                                        AssociatedControlID="txtRazonSocial" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                                    <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="textEntry" 
                                        MaxLength="255" TabIndex="2"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvRazonSocial" runat="server" 
                                        ControlToValidate="txtRazonSocial" CssClass="failureNotification" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, valRazonSocial %>" 
                                        ValidationGroup="grupoRfc" Width="16px">*</asp:RequiredFieldValidator>
                                </p>

                                <p>
                                    
                                    <asp:Label ID="lblCorreo" runat="server" 
                                        AssociatedControlID="txtCorreo" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"></asp:Label>
                                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="textEntry" 
                                        MaxLength="255" TabIndex="2"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regxCorreo" runat="server" 
                                        ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, valCorreo %>" 
                                        ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" 
                                        ValidationGroup="grupoRfc" Width="16px"><img 
                                        src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                </p>

                                <p>
                                    <asp:Label ID="lblNombreSucursal" runat="server" 
                                        AssociatedControlID="ddlSucursales" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>"></asp:Label>
                                    <asp:DropDownList ID="ddlSucursales" runat="server" AutoPostBack="True" 
                                        DataTextField="nombre" DataValueField="id_estructura" 
                                        onselectedindexchanged="ddlSucursales_SelectedIndexChanged" TabIndex="3" 
                                        Width="300px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvSucursales" runat="server" 
                                        ControlToValidate="ddlSucursales" CssClass="failureNotification" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                        ValidationGroup="grupoSeriesFolios" Width="16px">*</asp:RequiredFieldValidator>
                                </p>
                            </fieldset></td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRFCInformacion %>"></asp:Label>
                            <br />
                            <asp:Label ID="Label116" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblTituloCapturaCorreo %>"></asp:Label>
                        </td>
                    </tr>
                </table>
&nbsp;</asp:Panel>
            <p style="width:430px; text-align:right;">
                <asp:Button ID="btnGuardar" runat="server" CommandName="MoveNext" 
                ValidationGroup="grupoRfc" 
                Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" TabIndex="4" 
                    onclick="btnGuardar_Click" CssClass="botonEstilo" />
                <asp:Button ID="btnCancelarRfc" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                    onclick="btnCancelarRfc_Click" Visible="False" TabIndex="5" 
                    CssClass="botonEstilo" />
            </p>
            <br />
            <asp:GridView ID="gdvReceptores" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" GridLines="Horizontal" Width="800px" 
                DataKeyNames="id_rfc_receptor" 
                onselectedindexchanged="gdvReceptores_SelectedIndexChanged" 
                onrowdeleting="gdvReceptores_RowDeleting" AllowPaging="True" 
                BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                onpageindexchanging="gdvReceptores_PageIndexChanging" 
            BackColor="White" AllowSorting="True" OnSorting="gdvReceptores_Sorting" >
                <Columns>
                    <asp:CommandField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                        SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>"
                        ShowSelectButton="True" >
                    <ItemStyle Width="50" HorizontalAlign="Left" />
                    </asp:CommandField>
                    <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblAddSucursal %>" HeaderStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblVer %>"  
                                NavigateUrl='<%# Eval("id_rfc_receptor", "~/Operacion/Clientes/webOperacionClienteSuc.aspx?gis={0}") + Eval("nombre_receptor", "&nrs={0}") %>'>hpkAddSucursal</asp:HyperLink>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="120" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="rfc_receptor" 
                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblRfc %>" ItemStyle-Width="100" 
                        HeaderStyle-HorizontalAlign="Left" SortExpression="rfc_receptor"  >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="nombre_receptor" 
                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblNombreRazon %>" 
                        HeaderStyle-HorizontalAlign="Left" SortExpression="nombre_receptor" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:CommandField ShowDeleteButton="True" DeleteText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" >
                        <ItemStyle Width="50" HorizontalAlign="Left" />
                    </asp:CommandField >
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
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


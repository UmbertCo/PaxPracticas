<%@ Page Title="Documentos e Impuestos" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionDocImpuestos.aspx.cs" Inherits="Operacion_DocumentosImpuestos_webOperacionDocImpuestos" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloDocumentos %>"></asp:Label>
    </h2>

    <table>
    <tr>
    <td style="vertical-align:top;">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="">
                <fieldset class="register" style=" width:400px;">
                    <legend><asp:Literal runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubDocumentos %>" /></legend>
                        <p>
                            <asp:Label ID="lblTipoDocumento" runat="server" 
                                AssociatedControlID="ddlTipoDocumento" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblTipoDocumento %>"></asp:Label>
                            <asp:DropDownList ID="ddlTipoDocumento" runat="server" AutoPostBack="True" 
                                DataTextField="nombre" DataValueField="id_tipo_documento" TabIndex="1" 
                                Width="300px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvTipoDoc" runat="server" 
                                ControlToValidate="ddlTipoDocumento" CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valTipoDocumento %>" 
                                ValidationGroup="grupoDocumentosImp"></asp:RequiredFieldValidator>
                        </p>
                        <p>
                            <asp:Label ID="lblEfectoDocumento" runat="server" 
                                AssociatedControlID="ddlEfecto" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblEfectoDocumento %>"></asp:Label>
                            <asp:DropDownList ID="ddlEfecto" runat="server" DataTextField="efecto" 
                                DataValueField="efecto" onselectedindexchanged="ddlEfecto_SelectedIndexChanged" 
                                TabIndex="2" Width="300px" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEfecto" runat="server" 
                                ControlToValidate="ddlEfecto" CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valEfectoDocumento %>" 
                                ValidationGroup="grupoDocumentosImp"></asp:RequiredFieldValidator>
                        </p>
                        <p>
                            <asp:Label ID="lblImpuesto" runat="server" AssociatedControlID="ddlImpuesto" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblImpuesto %>"></asp:Label>
                            <asp:DropDownList ID="ddlImpuesto" runat="server" DataTextField="abreviacion" 
                                DataValueField="id_impuesto" TabIndex="2" Width="300px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvImpuesto" runat="server" 
                                ControlToValidate="ddlImpuesto" CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valImpuesto %>" 
                                ValidationGroup="grupoDocumentosImp"></asp:RequiredFieldValidator>
                        </p>
                </fieldset>
            </div>
            <p style="text-align:right;">
                            <asp:Button ID="btnGuardar" runat="server" CommandName="MoveNext" 
                                onclick="btnGuardar_Click" TabIndex="5" 
                                Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                                ValidationGroup="grupoDocumentosImp" CssClass="botonEstilo" />
                        </p>
        </ContentTemplate>
    </asp:UpdatePanel>
    </td>
    <td style="vertical-align:top; padding-top:40px;">
    <asp:UpdatePanel ID="updGuardar" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlGrid" runat="server">
                <asp:GridView ID="gdvImpuestos" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" GridLines="Horizontal" 
                    onrowdeleting="gdvImpuestos_RowDeleting" Width="460px" 
                    DataKeyNames="id_estructura,id_tipo_documento,id_impuesto" 
                    AllowPaging="True" BorderColor="#336666" BorderStyle="Double" 
                    BorderWidth="3px" onpageindexchanging="gdvImpuestos_PageIndexChanging" 
                    onselectedindexchanged="gdvImpuestos_SelectedIndexChanged" 
                    BackColor="White">
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>"
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:CommandField>
                        <asp:BoundField DataField="nombre" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblTipoDocumento %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="abreviacion" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblImpuesto %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="efecto" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblEfectoDocumento %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="tasa" HeaderStyle-HorizontalAlign="Right"
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblTasaImpuesto %>" >
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:CommandField ShowDeleteButton="True" DeleteText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>"
                             HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:CommandField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Literal runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
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
    </td>
    </tr>
</table>
</asp:Content>


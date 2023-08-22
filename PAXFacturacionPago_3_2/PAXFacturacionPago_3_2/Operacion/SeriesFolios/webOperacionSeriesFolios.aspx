<%@ Page Title="Series y Folios" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionSeriesFolios.aspx.cs" Inherits="Operacion_SeriesFolios_webOperacionSeriesFolios" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloSeries %>"></asp:Label>
    </h2>
    <div style="text-align:center;" >
            <asp:UpdateProgress ID="uppConsultas" runat="server" >
                    <progresstemplate>
                        <img alt="" 
                    src="../../Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>
            </div>
    <table>
        <tr>
        <td style="vertical-align:top;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="">
                <fieldset class="register" style=" height:250px; width:400px;">
                <legend><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubSeries %>" /></legend>
                    <p>
                        <asp:Label ID="lblNombreSucursal" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>" 
                            AssociatedControlID="ddlSucursales" ></asp:Label>
                        <asp:DropDownList ID="ddlSucursales" runat="server" Width="300px" AutoPostBack="True" 
                            DataTextField="nombre" DataValueField="id_estructura" TabIndex="1" 
                            onselectedindexchanged="ddlSucursales_SelectedIndexChanged"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvSucursal" runat="server" 
                            ControlToValidate="ddlSucursales" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                            ValidationGroup="grupoSeriesFolios"></asp:RequiredFieldValidator>
                    </p><p>
                        <asp:Label ID="lblTipoDocumento" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblTipoDocumento %>" 
                            AssociatedControlID="ddlTipoDocumento" ></asp:Label>
                            <asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="300px" 
                            DataTextField="nombre" DataValueField="id_tipo_documento" TabIndex="2" 
                            AutoPostBack="True" 
                            onselectedindexchanged="ddlTipoDocumento_SelectedIndexChanged"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTipoDoc" runat="server" 
                            ControlToValidate="ddlTipoDocumento" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valTipoDocumento %>" 
                            ValidationGroup="grupoSeriesFolios"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblSerie" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblSerie %>" 
                            AssociatedControlID="txtSerie" ></asp:Label>
                        <asp:TextBox ID="txtSerie" runat="server" CssClass="textEntry" 
                            MaxLength="25" TabIndex="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSerie" runat="server" 
                            ControlToValidate="txtSerie" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSerie %>" 
                            ValidationGroup="grupoSeriesFolios"></asp:RequiredFieldValidator>
                    </p><p>
                        <asp:Label ID="lblFolio" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblFolio %>" 
                            AssociatedControlID="txtFolio" ></asp:Label>
                        <asp:TextBox ID="txtFolio" runat="server" CssClass="textEntry" 
                            MaxLength="5" TabIndex="4"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtFolio_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFolio">
                        </cc1:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="rfvFolio" runat="server" 
                            ControlToValidate="txtFolio" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valFolio %>" 
                            ValidationGroup="grupoSeriesFolios"></asp:RequiredFieldValidator>
                    </p>
                </fieldset>
            </div>
             <p style="text-align:right;">
                <asp:Button ID="btnGuardar" runat="server" CommandName="MoveNext" 
                ValidationGroup="grupoSeriesFolios" 
                Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                    TabIndex="5" onclick="btnGuardar_Click" CssClass="botonEstilo" />
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>
    </td>
    <td style="vertical-align:top; padding-top:40px;">
        <asp:UpdatePanel ID="updGuardar" runat="server">
            <ContentTemplate>
                <asp:Panel runat="server">
                    <asp:GridView ID="gdvSeries" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" GridLines="Horizontal" 
                        DataKeyNames="id_serie" onrowdeleting="gdvSeries_RowDeleting" 
                        AllowPaging="True" BorderColor="#336666" BorderStyle="Double" 
                        BorderWidth="3px" onpageindexchanging="gdvSeries_PageIndexChanging" 
                        BackColor="White">
                        <Columns>
                            <asp:BoundField DataField="serie"  HeaderStyle-HorizontalAlign="Left"
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblSerie %>" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="folio" HeaderStyle-HorizontalAlign="Right"
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblFolio %>" >
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Width="150px" />
                            </asp:BoundField>
                            <asp:CommandField ShowDeleteButton="True" DeleteText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>"
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" HeaderStyle-HorizontalAlign="Left" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
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
<br />
<br />
</asp:Content>


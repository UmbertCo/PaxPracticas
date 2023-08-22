<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webReporteCreditosHistorico.aspx.cs" Inherits="Consultas_webReporteCreditosHistorico" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 218px;
        }
        .sinBorde
        {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
   <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblHistorico %>"></asp:Label>
   </h2>
                   <fieldset class="register" style="width:890px; height: 229px;">
                    <legend><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" /></legend>
                    <table class="style1">
                        <tr>
                            <td class="style2">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblUser %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LalblNombreSucursalbel3" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblSucursal %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:DropDownList ID="ddlUsuario" runat="server" Width="150px" 
                                    AutoPostBack="True" onselectedindexchanged="ddlUsuario_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSucursales" runat="server" DataTextField="nombre" 
                                    DataValueField="id_estructura" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblFechaIni" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                    AssociatedControlID="txtFechaInicio"></asp:Label>
                                <asp:TextBox ID="txtFechaInicio" runat="server" BackColor="White" 
                                    Width="150px" ></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaInicio" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgIni">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgIni" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaIni" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaInicio" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img 
                                    src="http://localhost:3373/PAXFacturacionPago_3_2/Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Label ID="lblFechaFin" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                    AssociatedControlID="txtFechaFin"></asp:Label>
                                <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White" 
                                    Width="150px"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgFin">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgFin" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaFin" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img 
                                    src="http://localhost:3373/PAXFacturacionPago_3_2/Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                &nbsp;
                                </td>
                            <td align="right">
                    <asp:Button ID="btnConsultar" runat="server" CssClass="botonEstilo" 
                        onclick="btnConsultar_Click" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                        ValidationGroup="grupoConsulta" Width="80px" />
                        &nbsp;
                        <asp:Button ID="btnExportar" runat="server" CssClass="botonGrande" 
                        onclick="btnExportar_Click" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblExcel %>" 
                        ValidationGroup="grupoConsulta" Visible="False" Height="32px" 
                        Width="155px" Enabled="False" />
                                <br />
                            </td>
                        </tr>
                    </table>
                </fieldset>
    <br />
     <asp:Panel runat="server" CssClass="sinBorde" ID="Panel234" Width="887px">
     <asp:GridView ID="gdvHistorico" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" GridLines="Horizontal" Width="99%" AllowPaging="True"
                    DataKeyNames="id_estructura" 
                    onpageindexchanging="gdvHistorico_PageIndexChanging" BackColor="White" 
                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" PageSize="15" 
                    >
                    <Columns>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblorigen" runat="server" Text='<%# Bind("origen") %>'></asp:Label>
                            </ItemTemplate>
                             <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblOrigen %>">
                            <ItemTemplate>
                                <asp:Label ID="lbldescorigen" runat="server" 
                                    Text='<%# Bind("descripcion_origen") %>'></asp:Label>
                            </ItemTemplate>
                             <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblidtipodoc" runat="server" 
                                    Text='<%# Bind("id_tipodocumento") %>'></asp:Label>
                            </ItemTemplate>
                             <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblTipoDocumento %>">
                            <ItemTemplate>
                                <asp:Label ID="lbldocumento" runat="server" Text='<%# Bind("documento") %>'></asp:Label>
                            </ItemTemplate>
                             <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>                    
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, varUUID %>">
                            <ItemTemplate>
                                <asp:Label ID="lblUUID" runat="server" 
                                    Text='<%# Bind("UUID") %>'></asp:Label>
                            </ItemTemplate>
                             <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblSerie %>">
                            <ItemTemplate>
                                <asp:Label ID="lblserie" runat="server" Text='<%# Bind("serie") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblFolio %>">
                            <ItemTemplate>
                                <asp:Label ID="lblfolio" runat="server" Text='<%# Bind("folio") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblestatus" runat="server" Text='<%# Bind("estatus") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus %>">
                            <ItemTemplate>
                                <asp:Label ID="lbldescestatus" runat="server" 
                                    Text='<%# Bind("descripcion_estatus") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblidestructura" runat="server" 
                                    Text='<%# Bind("id_estructura") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstructura %>">
                            <ItemTemplate>
                                <asp:Label ID="lblestructura" runat="server" Text='<%# Bind("estructura") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblidcreditos" runat="server" Text='<%# Bind("id_creditos") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblCargo %>">
                            <ItemTemplate>
                                <asp:Label ID="lblcargo" runat="server" Text='<%# Bind("cargo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblAbono %>">
                            <ItemTemplate>
                                <asp:Label ID="lblabono" runat="server" Text='<%# Bind("Abono") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblCreditosRest %>">
                            <ItemTemplate>
                                <asp:Label ID="lblcreditosrestantes" runat="server" 
                                    Text='<%# Bind("creditos_restantes") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="fecha" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblFechaOperacion %>">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
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
         <br />
         <br />
         <br />
     </asp:Panel>

    </asp:Content>


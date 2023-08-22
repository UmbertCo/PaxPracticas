<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webConsultaMovimientosClientesDistribuidor.aspx.cs" Inherits="Consultas_webConsultaMovimientosClientesDistribuidor" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

    <script type="text/javascript">
        function OnKeyPress(args) { if (args.keyCode == Sys.UI.Key.esc) { $find("ModalDescarga").hide(); } }
    </script>

    <style type="text/css">
        .style2
        {
            width: 264px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
                <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloDistribuidorConsulta %>"></asp:Label>
            </h2>
            <br />
                   <fieldset class="register">
                    <legend><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" /></legend>
                    <table class="style1">
                        <thead>
                            <tr>
                                <td>
                                     <asp:Label ID="Label3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblUserDistr %>"></asp:Label>  
                                </td> 
                            </tr>
                        </thead>
                        <tr>
                            <td>
                                <asp:TextBox runat ="server" ID= "txtUDistrib" Enabled = "false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblUser %>"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:DropDownList ID="ddlUsuario" runat="server" Width="250px" Enabled = true>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Checkbox runat="server" ID="Uchkbox" 
                                 Checked="false" OnCheckedChanged ="Uchkbox_CheckedChanged"  AutoPostBack="True"></asp:Checkbox>
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
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" > <img 
                                    src="http://localhost:3373/PAXFacturacionPago_3_2/Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
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
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" > <img 
                                    src="http://localhost:3373/PAXFacturacionPago_3_2/Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                &nbsp;
                                </td>
                            <td align="right">
                        &nbsp;
                                <br />
                            </td>
                        </tr>
                    </table>
                </fieldset><br />
                   <table align="right">
                       <tr>
                           <td>
                                <asp:Button ID="btnConsultar" runat="server" CssClass="botonEstilo" 
                                    onclick="btnConsultar_Click" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                                    ValidationGroup="grupoConsulta" Width="80px" />
                           </td>
                           <td>
                                <asp:Button ID="btnExportar" runat="server" CssClass="botonGrande" 
                                    onclick="btnExportar_Click" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblExcel %>" 
                                    ValidationGroup="grupoConsulta" Visible="False" Height="32px" 
                                    Width="155px" Enabled="True" />
                          </td>
                        </tr>
                   </table>
                   <br />
                <br />
                   
                   <asp:Panel ID="PanelTimbrados" runat="server" Visible="False">
                       <fieldset class="register" 
        id="Timbres0" runat="server">
                           <legend>
                               <asp:Literal ID="Literal8" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblTimbrados %>" />
                           </legend>
                           <table>
                               <tr>
                                   <td>
                                        <asp:GridView ID="gvdDetalleDoc1" runat="server" AutoGenerateColumns="False" 
                                            Height="50px" PageSize="7"  CellPadding="4"  GridLines="Both"
                                            Width="100%" BackColor="White" 
                                            BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" >
                                            <Columns>
                                               <asp:BoundField DataField="Accion" 
                                                    HeaderText="<%$ Resources:resCorpusCFDIEs, lblTipoDocumento %>" 
                                                    HeaderStyle-HorizontalAlign ="Left" />
                                               <asp:BoundField DataField="Conteo" 
                                                    HeaderText="<%$ Resources:resCorpusCFDIEs, lblasignarcreditos %>" 
                                                    HeaderStyle-HorizontalAlign ="Left" />
                                            </Columns>
                                            <HeaderStyle BackColor="#5A737E" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                       &nbsp;</td>
                               </tr>
                               <tr>
                                   <td>
                                       <asp:GridView ID="gdvTimbrados" runat="server" AutoGenerateColumns="False" 
                       Height="50px" Width="700px" AllowPaging="True" 
                       onpageindexchanging="gdvTimbrados_PageIndexChanging">
                                           <Columns>
                                               <asp:BoundField DataField="nombre" 
                               HeaderText="<%$ Resources:resCorpusCFDIEs, lblTipoDocumento %>" 
                               HeaderStyle-HorizontalAlign ="Left" />
                                               <asp:BoundField DataField="UUID" 
                               HeaderText="<%$ Resources:resCorpusCFDIEs, varUUID %>" 
                               HeaderStyle-HorizontalAlign ="Left" />
                                               <asp:BoundField DataField="serie" 
                               HeaderText="<%$ Resources:resCorpusCFDIEs, lblSerie %>" 
                               HeaderStyle-HorizontalAlign ="Left" />
                                               <asp:BoundField DataField="folio" 
                               HeaderText="<%$ Resources:resCorpusCFDIEs, lblFolio %>" 
                               HeaderStyle-HorizontalAlign ="Left" />
                                               <asp:BoundField DataField="fecha_documento" DataFormatString="{0:d}" 
                               HeaderText="<%$ Resources:resCorpusCFDIEs, lblFecha %>" 
                               HeaderStyle-HorizontalAlign ="Left" />
                                           </Columns>
                                           <EmptyDataTemplate>
                                               <asp:Literal ID="Literal9" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
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
                                   </td>
                               </tr>
                           </table>
                       </fieldset>
                       </asp:Panel>
</asp:Content>


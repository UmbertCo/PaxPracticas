<%@ Page Title="" Language="C#" MasterPageFile="~/Publica.master" AutoEventWireup="true" CodeFile="webErroresConocidos.aspx.cs" Inherits="Pantallas_Problemas_webErroresConocidos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css" >
    .contenedor
    {
        width:600px;
        text-align:left;
    }
    .contenedor select, input[type='text']
    {
     }
        .style2
        {
        }
        .style3
        {
            width: 124px;
        }
        .style6
        {
            width: 168px;
        }
        .style8
        {
            width: 118px;
        }
        .style9
        {
            width: 210px;
        }
        .style10
        {
            width: 205px;
        }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    

    <br />
    <asp:Label ID="lblencconerr" runat="server" Font-Bold="True" Font-Size="Large" 
        Text="<%$ Resources:resCorpusCFDIEs, lblencconerr %>"></asp:Label>

    <table style="width: 100%; height: 104px;">
        <tr>
            <td>
&nbsp;<asp:Button ID="btnNuevoProblema" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnNuevoProblema%>" Width="172px" 
                    onclick="btnNuevoProblema_Click" Visible="False" CssClass="botonGrande" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td class="style8" align="right">
                            <asp:Label ID="lblestatusb" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblestatusb%>"></asp:Label>
                        </td>
                        <td class="style9">
                            <asp:DropDownList ID="ddlestatus" runat="server" Height="20px" Width="210px" 
                                Enabled="False">
                                <asp:ListItem Value="N">Seleccione Opción</asp:ListItem>
                                <asp:ListItem Value="A">Abierto</asp:ListItem>
                                <asp:ListItem Value="P">En Proceso</asp:ListItem>
                                <asp:ListItem Value="C" Selected="True">Cerrado</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style3" align="right">
                            <asp:Label ID="lblusuariosopb" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblusuariosopb%>"></asp:Label>
                        </td>
                        <td class="style10">
                            <asp:DropDownList ID="ddlusuariosop" runat="server" Height="20px" Width="210px" 
                                ondatabound="ddlusuariosop_DataBound">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style8" align="right">
                            <asp:Label ID="lbltipoincp" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lbltipoincp %>"></asp:Label>
                        </td>
                        <td class="style9">
                            <asp:DropDownList ID="ddltipoinc" runat="server" Height="20px" Width="210px" 
                                ondatabound="ddltipoinc_DataBound">
                            </asp:DropDownList>
                        </td>
                        <td class="style3" align="right">
                            <asp:Label ID="lblticket0" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblticket %>"></asp:Label>
                        </td>
                        <td class="style10">
                            <asp:DropDownList ID="ddlticket" runat="server" Height="20px" Width="210px" 
                                ondatabound="ddltipoinc_DataBound">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style8" align="right">
                            &nbsp;</td>
                        <td colspan="3">
                            <asp:Label ID="lblFechas" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechas%>"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="<%$ Resources:resCorpusCFDIEs, btnExcel%>" 
                                Width="172px" Enabled="False" CssClass="botonGrande" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style8" align="right">
                            <asp:Label ID="lblfecharegb" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblfecharegb%>"></asp:Label>
                        </td>
                        <td class="style9">
                            <table>
                                <tr>
                                    <td>
                            <asp:TextBox ID="txtfechareg" runat="server" Height="20px" Width="210px"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtfechareg_CalendarExtender" runat="server" 
                                TargetControlID="txtfechareg" PopupButtonID="imgfechain">
                            </cc1:CalendarExtender>
                                    </td>
                                    <td>
                            <asp:Image ID="imgfechain" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="style3" align="right">
                            <asp:Label ID="lblfecharegb0" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblfecharegb0%>"></asp:Label>
                        </td>
                        <td class="style10">
                            <table>
                                <tr>
                                    <td>
                            <asp:TextBox ID="txtfechareg2" runat="server" Height="20px" Width="210px"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtfechareg2_CalendarExtender" runat="server" 
                                TargetControlID="txtfechareg2" PopupButtonID="imgfechafin">
                            </cc1:CalendarExtender>
                                    </td>
                                    <td>
                            <asp:Image ID="imgfechafin" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:Button ID="btnBuscarB" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnBuscarB%>" Width="172px" 
                    onclick="btnBuscarB_Click" CssClass="botonGrande" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" Height="309px" ScrollBars="Auto">
                    <asp:GridView ID="gdvIncidencias" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" GridLines="Horizontal" Width="862px" 
                DataKeyNames="id_problema" 
                BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" PageSize="1000" 
                    onrowdatabound="gdvIncidencias_RowDataBound" 
                    onselectedindexchanged="gdvIncidencias_SelectedIndexChanged1" 
                    AllowSorting="True" onsorting="gdvIncidencias_Sorting" BackColor="White">
                        <Columns>
                            <asp:CommandField HeaderText="" 
                        SelectText="Revisar"
                        ShowSelectButton="True" HeaderStyle-HorizontalAlign="Left" 
                        Visible="False" >
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:CommandField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblticket %>" 
                        HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblticket" runat="server" Text='<%# Bind("ticket_problema") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblDescripcion %>" HeaderStyle-HorizontalAlign="Left" 
                        SortExpression="descripcion">
                                <ItemTemplate>
                                    <asp:Label ID="lbldescripcion" runat="server" Text='<%# Bind("descripcion") %>' Width="250px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblfechareg %>" SortExpression="fecha_alta">
                                <ItemTemplate>
                                    <asp:Label ID="lblfechareg" runat="server" 
                                Text='<%# Bind("fecha_alta","{0:d}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lbltipoincp %>" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblincidente" runat="server" 
                                Text='<%# Bind("id_tipo_incidente") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblurgencia" runat="server" 
                                Text='<%# Bind("urgencia") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lbltipoincp %>" 
                        SortExpression="id_tipo_incidente">
                                <ItemTemplate>
                                    <asp:Label ID="lbltipoincidente" runat="server" 
                                Text='<%# Bind("tipo_incidente") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblSolucionSoporte %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblsolucionsoporte" runat="server" 
                                Text='<%# Bind("solucion_soporte") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblHorasRestantes %>" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblTiempoR" runat="server" Font-Bold="True" Font-Size="Medium" 
                                Text='<%# Bind("hora") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblidincidencia" runat="server" 
                                Text='<%# Bind("id_problema") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblestatus" runat="server" Text='<%# Bind("id_estatus") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:literal ID="Literal1" runat="server" 
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
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>




</asp:Content>


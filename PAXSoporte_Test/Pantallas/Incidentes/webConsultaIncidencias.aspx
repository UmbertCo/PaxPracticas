<%@ Page Title="Consulta de Incidencias" Language="C#" MasterPageFile="~/Publica.master" AutoEventWireup="true" CodeFile="webConsultaIncidencias.aspx.cs" Inherits="Pantallas_Incidentes_webConsultaIncidencias" EnableEventValidation = "false" %>

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
        .style3
        {
            width: 124px;
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
        .style11
        {
            width: 98%;
        }
        .style33
        {
            width: 190px;
        }
        .style24
        {
            width: 96px;
        }
        .style25
        {
            width: 98px;
        }
        .style32
        {
            width: 301px;
        }
        .style16
        {
            width: 260px;
        }
        .style26
        {
            width: 100%;
        }
        .style30
        {
            width: 87px;
        }
        .style29
        {
            width: 89px;
        }
        .style19
        {
            color: #FFFFFF;
        }
        .style37
        {
            height: 12px;
            width: 16px;
        }
        .style38
        {
            height: 12px;
            width: 310px;
        }
        .style42
        {
            width: 310px;
        }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

 
   
    <br />
    <asp:Label ID="lblencconinc" runat="server" Font-Bold="True" Font-Size="Large" 
        Text="<%$ Resources:resCorpusCFDIEs, lblencconinc %>"></asp:Label>

    <br />
    <br />

    <table style="width: 100%; height: 104px;">
        <tr>
            <td>
&nbsp;<asp:Button ID="btnNuevaIncidencia" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnNuevaIncidencia%>" Width="172px" 
                    onclick="btnNuevaIncidencia_Click1" CssClass="botonGrande" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Timer 
                    ID="Timer2" runat="server" ontick="Timer2_Tick">
                </asp:Timer>
                &nbsp; &nbsp;&nbsp;&nbsp;</td>
        </tr>
        <tr>
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
                            <asp:DropDownList ID="ddlestatus" runat="server" Height="20px" Width="210px">
                                <asp:ListItem Value="N">Seleccione Opción</asp:ListItem>
                                <asp:ListItem Value="A" Selected="True">Abierto</asp:ListItem>
                                <asp:ListItem Value="P">En Proceso</asp:ListItem>
                                <asp:ListItem Value="C">Cerrado</asp:ListItem>
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
                            <asp:Button ID="btnBuscarB" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnBuscarB%>" Width="172px" 
                    onclick="btnBuscarB_Click" CssClass="botonGrande" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style8" align="right">
                            <asp:Label ID="lbltipoincb" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lbltipoincb%>"></asp:Label>
                        </td>
                        <td class="style9">
                            <asp:DropDownList ID="ddltipoinc" runat="server" Height="20px" Width="210px" 
                                ondatabound="ddltipoinc_DataBound">
                            </asp:DropDownList>
                        </td>
                        <td class="style3" align="right">
                            <asp:Label ID="lblticketprob" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblticketprob%>"></asp:Label>
                        </td>
                        <td class="style10">
                            <asp:DropDownList ID="txtticketpro" runat="server" Height="20px" Width="210px" 
                                ondatabound="txtticketpro_DataBound">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style8" align="right">
                            <asp:Label ID="lblticket" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblticket %>"></asp:Label>
                        </td>
                        <td class="style9">
                            <asp:DropDownList ID="ddlticket" runat="server" Height="20px" Width="210px" 
                                ondatabound="ddltipoinc_DataBound">
                            </asp:DropDownList>
                        </td>
                        <td class="style3" align="right">
                            &nbsp;</td>
                        <td class="style10">
                            &nbsp;</td>
                        <td>
                            <asp:Button ID="btncolor" runat="server" onclick="Button1_Click" 
                                Text="<%$ Resources:resCorpusCFDIEs, btncolor%>" Width="172px" 
                                CssClass="botonGrande" />
                        </td>
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
                            <table class="style11">
                                <tr>
                                    <td>
                            <asp:TextBox ID="txtfechareg" runat="server" Height="20px" Width="210px"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtfechareg_CalendarExtender" runat="server" 
                                TargetControlID="txtfechareg" PopupButtonID="imgfechain" Format="dd/MM/yyyy">
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
                            <table class="style11">
                                <tr>
                                    <td>
                            <asp:TextBox ID="txtfechareg2" runat="server" Height="20px" Width="210px"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtfechareg2_CalendarExtender" runat="server" 
                                TargetControlID="txtfechareg2" PopupButtonID="imgfechafin" Format="dd/MM/yyyy">
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
                            <asp:Button ID="btnProblema" runat="server" Width="172px" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnProblema%>" 
                    onclick="btnProblema_Click" CssClass="botonGrande" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" Height="350px" ScrollBars="Auto">
                    <asp:GridView ID="gdvIncidencias" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" GridLines="Horizontal" Width="847px" 
                DataKeyNames="id_incidente" 
                BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" PageSize="1000" 
                    onrowdatabound="gdvIncidencias_RowDataBound" 
                    onselectedindexchanged="gdvIncidencias_SelectedIndexChanged1" 
                    AllowSorting="True" onrowcommand="gdvIncidencias_RowCommand" 
                    onsorting="gdvIncidencias_Sorting" BackColor="White">
                        <Columns>
                            <asp:CommandField HeaderText="" 
                        SelectText="<%$ Resources:resCorpusCFDIEs, lblcheck %>"
                        ShowSelectButton="True" HeaderStyle-HorizontalAlign="Left" >
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:CommandField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblticket %>" 
                        HeaderStyle-HorizontalAlign="Left" SortExpression="ticket">
                                <ItemTemplate>
                                    <asp:Label ID="lblticket" runat="server" Text='<%# Bind("ticket") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblDescripcion %>" HeaderStyle-HorizontalAlign="Left" 
                        SortExpression="Descripcion">
                                <ItemTemplate>
                                    <asp:Label ID="lbldescripcion" runat="server" Text='<%# Bind("descripcion") %>' 
                                        Width="250px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblfechareg %>" SortExpression="fecha_registro">
                                <ItemTemplate>
                                    <asp:Label ID="lblfechareg" runat="server" Text='<%# Bind("fecha_registro") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lbltipoincb %>" Visible="False">
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
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lbltipoincb %>" 
                        SortExpression="id_tipo_incidente">
                                <ItemTemplate>
                                    <asp:Label ID="lbltipoincidente" runat="server" 
                                Text='<%# Bind("tipo_incidente") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblHorasRestantes %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblTiempoR" runat="server" Font-Bold="True" Font-Size="Medium" 
                                Text='<%# Bind("hora") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbProblema" runat="server" Checked="<%# false %>" 
                                Enabled="<%# true %>" Visible="<%# true %>" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblidincidencia" runat="server" 
                                Text='<%# Bind("id_incidente") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblticketprob %>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblticketproblema" runat="server" CommandName="Problema" 
                                Text='<%# Bind("ticket_problema") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblestatus" runat="server" Text='<%# Bind("estatus") %>'></asp:Label>
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
        </tr>
        <tr>
            <td>

              
          
            
              
            </td>
        </tr>
        <tr>
            <td>
                 
                <asp:Panel ID="Panel12" runat="server" BackColor="White" BorderColor="Black" 
                    BorderStyle="Double" Width="544px">
                    <table class="style11">
                        <tr>
                            <td bgcolor="#004D71" class="style38">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td bgcolor="#004D71" class="style37">
                                <asp:ImageButton ID="imgbtnCerrar" runat="server" 
                                    ImageUrl="~/Imagenes/error_sign.gif" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style42">
                                <asp:Label ID="lblticket0" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblticket %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style42">
                                <table class="style26">
                                    <tr>
                                        <td>
                                            Se pinta en base al estatus, se manejan 2 colores:</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table class="style11">
                                                <tr>
                                                    <td class="style24">
                                                        <asp:Panel ID="Panel4" runat="server" BorderColor="#999999" BorderStyle="Solid" 
                                                            Height="20px" HorizontalAlign="Center" Width="79px">
                                                            Sin Atender</asp:Panel>
                                                    </td>
                                                    <td class="style25">
                                                        <asp:Panel ID="Panel2" runat="server" BackColor="#FFFF99" BorderColor="#999999" 
                                                            BorderStyle="Solid" Height="20px" HorizontalAlign="Center" Width="79px">
                                                            En Proceso</asp:Panel>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="Panel3" runat="server" BackColor="#00CC66" BorderColor="#999999" 
                                                            BorderStyle="Solid" Height="20px" HorizontalAlign="Center" Width="79px">
                                                            Terminado</asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="style42">
                                <table class="style26">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbltipoincb2" runat="server" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lbltipoincb%>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Se pinta en base a la urgencia:</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table class="style26">
                                                <tr>
                                                    <td class="style30">
                                                        <asp:Panel ID="Panel22" runat="server" BorderColor="#999999" 
                                                            BorderStyle="Solid" Height="20px" HorizontalAlign="Center" Width="79px">
                                                            Baja</asp:Panel>
                                                    </td>
                                                    <td class="style29">
                                                        <asp:Panel ID="Panel23" runat="server" BackColor="#FFFF99" 
                                                            BorderColor="#999999" BorderStyle="Solid" Height="20px" 
                                                            HorizontalAlign="Center" Width="79px">
                                                            Media</asp:Panel>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="Panel24" runat="server" BackColor="#CC0000" 
                                                            BorderColor="#999999" BorderStyle="Solid" Height="20px" 
                                                            HorizontalAlign="Center" style="margin-left: 0px" Width="79px">
                                                            <span class="style19">Alta</span></asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="style42">
                                <table style="width: 519px">
                                    <tr>
                                        <td>
                                            Horas:</td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table class="style11">
                                                <tr>
                                                    <td class="style33">
                                                        &nbsp;Menor o igual a 2</td>
                                                </tr>
                                                <tr>
                                                    <td class="style33">
                                                        <asp:Panel ID="Panel25" runat="server" BackColor="#CC0000" 
                                                            BorderColor="#999999" BorderStyle="Solid" Height="20px" 
                                                            HorizontalAlign="Center" Width="79px">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table class="style11">
                                                <tr>
                                                    <td class="style32">
                                                        Mayor a 2 y menor o igual a 6</td>
                                                </tr>
                                                <tr>
                                                    <td class="style32">
                                                        <asp:Panel ID="Panel26" runat="server" BackColor="#FFFF99" 
                                                            BorderColor="#999999" BorderStyle="Solid" Height="20px" 
                                                            HorizontalAlign="Center" Width="79px">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table class="style11">
                                                <tr>
                                                    <td class="style16">
                                                        Si se encuentra terminado</td>
                                                </tr>
                                                <tr>
                                                    <td class="style16">
                                                        <asp:Panel ID="Panel27" runat="server" BackColor="#00CC66" 
                                                            BorderColor="#999999" BorderStyle="Solid" Height="20px" 
                                                            HorizontalAlign="Center" Width="79px">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="style42">
                                <asp:Label ID="lblticketprob0" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblticketprob%>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style42">
                                <table class="style26">
                                    <tr>
                                        <td>
                                            Se pinta en base al estatus, se manejan 2 colores:</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table class="style11">
                                                <tr>
                                                    <td class="style24">
                                                        <asp:Panel ID="Panel28" runat="server" BorderColor="#999999" 
                                                            BorderStyle="Solid" Height="20px" HorizontalAlign="Center" Width="79px">
                                                            Sin Atender</asp:Panel>
                                                    </td>
                                                    <td class="style25">
                                                        <asp:Panel ID="Panel29" runat="server" BackColor="#FFFF99" 
                                                            BorderColor="#999999" BorderStyle="Solid" Height="20px" 
                                                            HorizontalAlign="Center" Width="79px">
                                                            En Proceso</asp:Panel>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="Panel30" runat="server" BackColor="#00CC66" 
                                                            BorderColor="#999999" BorderStyle="Solid" Height="20px" 
                                                            HorizontalAlign="Center" Width="79px">
                                                            Terminado</asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                 
            </td>
        </tr>
        <tr>
            <td>
                  
               <cc1:ModalPopupExtender ID="mpePanel" runat="server" 
                    backgroundcssclass="modalBackground" popupcontrolid="Panel12" 
                    popupdraghandlecontrolid="" targetcontrolid="lnkModal" CancelControlID="imgbtnCerrar">
                </cc1:ModalPopupExtender>
                <asp:LinkButton ID="lnkModal" runat="server"></asp:LinkButton>
            </td>
        </tr>
    </table>




</asp:Content>


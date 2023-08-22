<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webConsultaComprobantes.aspx.cs" Inherits="Operacion_Comprobantes_webConsultaComprobantes" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css" >
    div.textos input[type='text']
    {
        width:300px;
    }
    div.textos select
    {
        width:300px;
    }
    .sinBorde img
    {
        border-style:none;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.5/themes/base/jquery-ui.css"
        type="text/css" media="all" />
    <link rel="stylesheet" href="http://static.jquery.com/ui/css/demo-docs-theme/ui.theme.css"
        type="text/css" media="all" />
    <%--<script type="text/javascript">
        $(function () {
            $("#<%=txtFechaInicio.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%=txtFechaFin.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            
        })
    </script>--%>
    <script type="text/javascript" language="javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginReq);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);

        //                function startProgressBar() {
        //                    divProgressBar.style.visibility = "visible";
        //                    pMessage.style.visibility = "visible";

        //                    progress_update();
        //                }


        function beginReq(sender, args) {
            // muestra el popup 
            $find(ModalProgress).show();
            $find(ModalProgress)._backgroundElement.style.zIndex += 10;
            $find(ModalProgress)._foregroundElement.style.zIndex += 10;
        }

        function endReq(sender, args) {
            //  esconde el popup 
            $find(ModalProgress).hide();
        }

       


            </script>
             <style type="text/css" >
                
                .modal
                {
                    padding: 10px 10px 10px 10px;
                    border:1px solid #333333;
                    background-color:White;
                }
                .modal p
                {
                    width:600px;
                    text-align:right;
                }
                .modal div
                {
                    width:600px;
                    vertical-align:top;
                }
                .modal div p
                {
                    text-align:left;
                }
                .imagenModal
                {
                    height:15px;
                    cursor:pointer;
                }
                .modalBackground
                {
                    background-color: #666699;
                    filter: alpha(opacity=50);
                    opacity: 0.7;
                }
                 .style3
                 {
                     width: 205px;
                 }
                 .style4
                 {
                     width: 797px;
                 }
                </style>
    <br /><br /><br /><br />
    <h2>
        <asp:Label ID="lblTitulo" runat="server" 
            Text="<%$ Resources:resCorpusCFDIEs, lblTituloConsultas %>" Font-Bold="True" 
            ForeColor="#9C1518"></asp:Label>
    </h2>
    <table style="border: 2px ridge #9C1518; width: 973px;" class="bodyMainComprobantes">
        <tr>
            <td>
                <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td align="left">
                                    <asp:RadioButtonList ID="rbTipoBusqueda" RepeatDirection="Horizontal" runat="server" Width="300px">
                                    <asp:ListItem Value="0" Text="<%$ Resources:resCorpusCFDIEs, lblFechaDoc %>" Selected="True"/>
                                    <asp:ListItem Value="1" Text="<%$ Resources:resCorpusCFDIEs, lblFechaValidacion %>" />
                                    </asp:RadioButtonList>       
                                </td>
                            </tr>
                            </table>
                            <asp:Panel ID="pnlFiltros" runat="server" DefaultButton="btnBuscar">
                            <table>
                            <tr>
                            <td valign="top">
                            <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFechaInicio" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" ></asp:Label>
                                </td>
                                    <td class="style3">
                                        <asp:TextBox ID="txtFechaInicio" runat="server" Width="220px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Image ID="imgFIni" runat="server" 
                                        ImageUrl="~/Imagenes/icono_calendario.gif" />
                                            <cc1:CalendarExtender ID="ceFechaInicio" runat="server" 
                                                Enabled="True" TargetControlID="txtFechaInicio" Format="dd/MM/yyyy" 
                                                    PopupButtonID="imgFIni">
                                            </cc1:CalendarExtender>
                             </td>
                             <td></td>
                                                            <td> 
                                                                <asp:Label ID="lblFechaFin" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" ></asp:Label>
                                                            </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFechaFin" runat="server" Width="220px"></asp:TextBox>
                                                                    </td>
                                                                        <td>
                                                                            <asp:Image ID="imgFin" runat="server" 
                                                                                ImageUrl="~/Imagenes/icono_calendario.gif" />
                                                                                    <cc1:CalendarExtender ID="ceFechaFin" runat="server" 
                                                                                        Enabled="True" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" 
                                                                                            PopupButtonID="imgFin">
                                                                                                </cc1:CalendarExtender>
                                                                            </td>
                                                                                <td rowspan="4" width="120px">
                                                                                
                                                                                   
                                                                                    
                                                                                
                                                                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRFCEmisor" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRFCEmisor %>" ></asp:Label>
                                </td>
                                    <td class="style3">
                                        <asp:TextBox ID="txtRfcEmisor" runat="server" Width="220px" />
                                        </td>
                                        <td></td><td></td>
                                        <td>
                                    <asp:Label ID="lblRfcReceptor" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRFCReceptor %>" Width="100px" ></asp:Label>
                                    </td><td>
                                        <asp:TextBox ID="txtRfcReceptor" runat="server" Width="220px" />
                                            </td><td></td>
                            </tr>
                                <tr>
                                    <td>
                                            <asp:Label ID="lblVersion" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblVersion %>" ></asp:Label>
                                    </td>
                                        <td>
                                                <asp:DropDownList ID="ddlVersion" runat="server" Width="223px">
                                                                                                </asp:DropDownList>
                                            </td>
                                                <td></td><td></td>
                                                
                                                <td>
                                                </td><td></td>
                                </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                                <td>
                                                    
                                        </td>
                                                    <td></td><td></td><td></td><td></td>
                                    </tr>
                                        
                        </table>
                        </td>
                        <td valign="top">
                            <asp:Panel ID="pnlSucursales" runat="server" ScrollBars="Auto" Width="250px" Height="180px">
                                                                                
                                                                                    <asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False" 
                                                                                        BackColor="White" BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" 
                                                                                        CellPadding="3" CellSpacing="1" DataKeyNames="id_sucursal" GridLines="None" 
                                                                                        Visible="False" Width="197px">
                                                                                        <Columns>
                                                                                            <asp:TemplateField Visible="False">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblidsucursal" runat="server" Text='<%# Bind("id_sucursal") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblSucursal %>">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="empresa_sucursal" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
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
                                                                                                Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                                                                                        </EmptyDataTemplate>
                                                                                        <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                                                                                        <HeaderStyle BackColor="Maroon" Font-Bold="True" ForeColor="#E7E7FF" />
                                                                                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                                                                        <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                                                                                        <SelectedRowStyle BackColor="#FF121C" Font-Bold="True" ForeColor="White" />
                                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                                        <SortedAscendingHeaderStyle BackColor="#D30000" />
                                                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                                                        <SortedDescendingHeaderStyle BackColor="Maroon" />
                                                                                    </asp:GridView>
                                                                                </asp:Panel> 
                        </td>
                        </tr>
                        </table>
                        </asp:Panel>
                            <table>
                            <tr>
                                            <td class="style4" align="right">
                                            <asp:Label ID="lblLineasPagina" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblLineasPagina %>" ></asp:Label>
                                                <asp:DropDownList ID="ddlLineasPagina" runat="server" >
                                                    <asp:ListItem Selected="true" Text="10" />
                                                    <asp:ListItem Text="20" />
                                                    <asp:ListItem Text="30" />
                                                    <asp:ListItem Text="40" />
                                                    <asp:ListItem Text="50" />
                                                </asp:DropDownList>
                                              
                                             </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upDescargas" runat="server">
                                                    <ContentTemplate>
                                                    <table>
                                                    <tr>
                                                   
                                                    <td>
                                                        <asp:Button ID="btnBuscar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, btnBuscar %>"
                                                            OnClick="btnBuscar_Click" CssClass="botonEstiloVentanas" Width="80px" />
                                                </td>
                                                 <td>
                                                        <asp:Button ID="btnDescargar" runat="server" CssClass="botonEstiloVentanas" OnClick="btnDescargar_Click"
                                                            Text="<%$ Resources:resCorpusCFDIEs, btnDescargar %>" ValidationGroup="grupoConsulta"
                                                            Visible="False" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExportar" runat="server" CssClass="botonGrande" OnClick="btnExportar_Click"
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblExcel %>" ValidationGroup="grupoConsulta"
                                                            Visible="False" Width="200px" Height="50px" />
                                                    </td>
                                                </tr>
                                                </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            
                                                   
                                                
                                              </td>
                                        </tr>
                            </table>
                         
                    </td>
                </tr>
            </table>
            </td>
            </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="udpBuscar" runat="server">
                    <ContentTemplate>
                    <table >
                    <tr>
                    <td align="right">
                      <asp:CheckBox ID="cbSeleccionar" runat="server" AutoPostBack="True" 
                                                    OnCheckedChanged="cbSeleccionar_CheckedChanged" 
                                                    Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionarTodo %>" TextAlign="Left" 
                                                    Visible="false" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:GridView ID="gvResultado" runat="server" 
                            
                            OnPageIndexChanged="gvResultado_PageIndexChanged" BorderColor="White"
                            OnPageIndexChanging="gvResultado_PageIndexChanging"
                            OnRowEditing="gvResultado_RowEditing"  
                            OnRowCancelingEdit="gvResultado_RowCancelingEdit" 
                            OnRowUpdating="gvResultado_RowUpdating"
                            Width="800px" BackColor="White" BorderStyle="Ridge" BorderWidth="2px" 
                            CellPadding="3" CellSpacing="1" GridLines="None">
                            <Columns>
                            <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Text="<%$ Resources:resCorpusCFDIEs,  lblEditar %>" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" Text="<%$ Resources:resCorpusCFDIEs,  btnActualizar %>" />
                                            <asp:LinkButton ID="Cancel" runat="server" CommandName="Cancel" Text="<%$ Resources:resCorpusCFDIEs,  lblCancelar %>" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblXML" runat="server"  Text="<%$ Resources:resCorpusCFDIEs,  varXML %>" HeaderStyle-HorizontalAlign="Left" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hpPDF" runat="server" Target="_blank"
                                            NavigateUrl='<%# Eval("id_comprobante", "~/Consultas/webDescargarXML.aspx?idcomprobante={0}") %>'> 
                                            <asp:Image ID="imgPDF" runat="server" ImageUrl="~/Imagenes/xml_mediano.png" Width="30" BorderStyle="None" />
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="Label2" runat="server" Text="PDF" HeaderStyle-HorizontalAlign="Left" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hpXML" runat="server" Target="_blank"
                                            NavigateUrl='<%# Eval("id_comprobante", "~/Consultas/webDescargaPDF.aspx?idcomprobante={0}") %>'>
                                            <asp:Image ID="imgXML" runat="server" ImageUrl="~/Imagenes/logo_pdf.png" Width="30"  BorderStyle="None" />
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="id_comprobante" HeaderText="ID" Visible="false" ReadOnly="true" />
                                
                                <%-- <asp:BoundField DataField="rfc_emisor" HeaderText="RFC Emisor" ReadOnly="true" />--%>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblidcomprobante" runat="server" Text='<%# Bind("id_comprobante") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="nombre_emisor" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblCorreoEmisor %>" ReadOnly="true"  HeaderStyle-Width="200px" />
                                <%--<asp:BoundField DataField="rfc_receptor" HeaderText="RFC Receptor" ReadOnly="true"/>--%>
                                <asp:BoundField DataField="nombre_receptor" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblReceptor %>" ReadOnly="true" HeaderStyle-Width="300px"/>
                                <asp:BoundField DataField="serie" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblSerie %>" ReadOnly="true" />
                                <asp:BoundField DataField="folio" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblFolio %>" ReadOnly="true" />
                                <asp:BoundField DataField="uuid" HeaderText="UUID" Visible="false" ReadOnly="true" />
                                <asp:BoundField DataField="fecha_documento" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblFechaDoc %>" ReadOnly="true" />
                                <asp:BoundField DataField="fecha_validacion" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblFechaValidacion %>" ReadOnly="true" />
                                <asp:BoundField DataField="sucursal" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblSucursal %>" ReadOnly="true" />
                                <asp:BoundField DataField="version" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblVersion %>" Visible="false" ReadOnly="true" />
                                <asp:BoundField DataField="total" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblTotal %>" Visible="true" ReadOnly="true" />
                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs,  lblValido %>">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chbValido" runat="server" Checked='<%# Bind("valido") %>' Enabled="false"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs,  lblEstatus %>" ControlStyle-Width="100px"  >
                                    <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                                        </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlStatus" runat="server" 
                                        OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs,  lblFechaPago %>" >
                                    <ItemTemplate>
                                            <asp:Label ID="lblFechaPago" runat="server" Text='<%# Bind("fecha_pago","{0:dd/MM/yyyy}") %>'  Width="100px"></asp:Label>
                                        </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtFechaPago" runat="server" Enabled="false" Width="100px"/>
                                        <asp:Image ID="imgIni" runat="server" 
                                            ImageUrl="~/Imagenes/icono_calendario.gif"  Visible="false" Width="18px" Height="18px" />
                                        <cc1:CalendarExtender ID="ceFechaPago" runat="server" 
                                            Enabled="True" TargetControlID="txtFechaPago" Format="dd/MM/yyyy" 
                                            PopupButtonID="imgIni">
                                        </cc1:CalendarExtender>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblSeleccionar %>">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSeleccion" Visible="true" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                            <HeaderStyle BackColor="Maroon" Font-Bold="True" ForeColor="#E7E7FF" />
                            <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                            <SelectedRowStyle BackColor="#FF121C" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#D30000" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="Maroon" />
                        </asp:GridView>
                        </td>
                        </tr>
                        <tr>
                      <td>
                        <table align="right">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnAnterior" runat="server" CssClass="botonEstilo" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lbAnterior %>" onclick="btnAnterior_Click" 
                                            Visible="False" /></td>
                                    <td align="right">
                                        <asp:Button ID="btnSiguiente" runat="server" CssClass="botonEstilo" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblSiguiente %>" 
                                            onclick="btnSiguiente_Click" Visible="False" />
                                    </td>
                                </tr> 
                            </table>
                            </td>
                             </tr>
                        </table>
                         <script type="text/javascript" language="javascript">
                             var ModalProgress = '<%= modalGenerando.ClientID %>';         
                        </script>
                        <asp:Panel ID="pnlGenerando" runat="server" Width="300px" 
                        CssClass="modal" BorderStyle="Solid" BorderWidth="1px">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varProcesando %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdateProgress ID="updGenera" runat="server">
                                            <progresstemplate>
                                                <img alt="" src="../../Imagenes/imgAjaxLoader.gif" />
                                            </progresstemplate>
                                        </asp:UpdateProgress>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

            <cc1:modalpopupextender id="modalGenerando" runat="server" backgroundcssclass="modalBackground" 
            popupcontrolid="pnlGenerando" popupdraghandlecontrolid=""
                targetcontrolid="pnlGenerando" EnableViewState="false" DropShadow="true">
            </cc1:modalpopupextender>
                   </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click"/>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    <asp:Label ID="lMensajeError" runat="server" Visible="false"></asp:Label>
    </table>
</asp:Content>

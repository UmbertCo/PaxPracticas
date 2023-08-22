<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="webConsultaComprobantes.aspx.cs" Inherits="Operacion_Comprobantes_webConsultaComprobantes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        div.textos input[type='text']
        {
            width: 300px;
        }
        div.textos select
        {
            width: 300px;
        }
        .sinBorde img
        {
            border-style: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
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
    <style type="text/css">
        .modal
        {
            padding: 10px 10px 10px 10px;
            border: 1px solid #333333;
            background-color: White;
        }
        .modal p
        {
            width: 600px;
            text-align: right;
        }
        .modal div
        {
            width: 600px;
            vertical-align: top;
        }
        .modal div p
        {
            text-align: left;
        }
        .imagenModal
        {
            height: 15px;
            cursor: pointer;
        }
        .modalBackground
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .style4
        {
            width: 226px;
        }
        .style5
        {
            height: 36px;
        }
    </style>
    <br />
    <br />
    <br />
    <br />
    <h2 align="left">
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloConsultas %>"></asp:Label>
    </h2>
    <table style="border: solid 1px #C0C0C0;" class="bodyMain">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rbTipoBusqueda" RepeatDirection="Horizontal" runat="server">
                                                        <asp:ListItem Value="0" Text="<%$ Resources:resCorpusCFDIEs, lblFechaDoc %>" />
                                                        <asp:ListItem Value="1" Text="<%$ Resources:resCorpusCFDIEs, lblFechaValidacion %>" />
                                                    </asp:RadioButtonList>
                                                </td>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td width="120px" align="left">
                                                        <asp:Label ID="lblFechaInicio" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>"
                                                            Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="style4" Width="220px"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="ceFechaInicio" runat="server" Enabled="True" TargetControlID="txtFechaInicio"
                                                            Format="dd/MM/yyyy" PopupButtonID="imgFIni">
                                                        </cc1:CalendarExtender>
                                                        <asp:Image ID="imgFIni" runat="server" ImageUrl="~/Imagenes/icono_calendario.gif" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td width="120px" align="left">
                                                        <asp:Label ID="lblFechaFin" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>"
                                                            Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtFechaFin" runat="server" CssClass="style4" Width="220px"></asp:TextBox>
                                                        <asp:Image ID="imgFin" runat="server" ImageUrl="~/Imagenes/icono_calendario.gif" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td width="120px" align="left">
                                                        <asp:Label ID="lblRFCEmisor" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>"
                                                            Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtRfcEmisor" runat="server" CssClass="style4" Width="220px" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td width="120px" align="left">
                                                        <asp:Label ID="lblRfcReceptor" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRFCReceptor %>"
                                                            Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtRfcReceptor" runat="server" CssClass="style4" Width="220px" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td width="120px" align="left">
                                                        <asp:Label ID="lblVersion" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblVersion %>"
                                                            Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlVersion" runat="server" CssClass="style4" Width="220px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <div style="overflow: auto;">
                                            <asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" CellSpacing="1"
                                                DataKeyNames="id_sucursal" GridLines="None" OnRowDataBound="GrvModulos_RowDataBound"
                                                OnSelectedIndexChanged="GrvModulos_SelectedIndexChanged" Visible="False" Width="400px">
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
                                                    <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                                                </EmptyDataTemplate>
                                                <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                                                <HeaderStyle BackColor="#880085" Font-Bold="True" ForeColor="#E7E7FF" />
                                                <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                                <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                                                <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                <SortedAscendingHeaderStyle BackColor="#594B9C" />
                                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                <SortedDescendingHeaderStyle BackColor="#33276A" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                    </td>
                                    <td align="right" class="style5">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btnDescargar" runat="server" CssClass="botonEstilo" OnClick="btnDescargar_Click"
                                                    Text="<%$ Resources:resCorpusCFDIEs, btnDescargar %>" ValidationGroup="grupoConsulta"
                                                    Visible="False" Height="27px" />
                                                <asp:Button ID="btnBuscar" runat="server" Text="<%$ Resources:resCorpusCFDIEs,  lblSubConsulta %>"
                                                    OnClick="btnBuscar_Click" ValidationGroup="grupoConsulta" 
                                                    CssClass="botonEstilo" Height="27px" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                  
                </table>
              
                <table width="830px">
                    <tr>
                    <td align="right">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblTamañoPagina" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblResultadosXpagina %>"></asp:Label>
                                <asp:DropDownList ID="ddlTamañoPagina" runat="server" Width="50px">
                                </asp:DropDownList>
                                <asp:CheckBox ID="cbSeleccionar" runat="server" AutoPostBack="True" 
                                    OnCheckedChanged="cbSeleccionar_CheckedChanged" 
                                    
    Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionarTodo %>" TextAlign="Left" 
                                    Visible="true" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<center>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="udpBuscar" ID="updProgress">
                <ProgressTemplate>
                    <img alt="" src="../../Imagenes/imgAjaxLoader.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            </center>--%>
                            <asp:UpdatePanel ID="udpBuscar" runat="server">
                                <ContentTemplate>
                                    <table width="850px">
                                        
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvResultado" runat="server" OnPageIndexChanged="gvResultado_PageIndexChanged"
                                                    BorderColor="White" OnPageIndexChanging="gvResultado_PageIndexChanging" OnRowEditing="gvResultado_RowEditing"
                                                    OnRowCancelingEdit="gvResultado_RowCancelingEdit" OnRowUpdating="gvResultado_RowUpdating"
                                                    Width="869px" BackColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3"
                                                    CellSpacing="1" GridLines="None" AutoGenerateColumns="False">
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
                                                                <asp:Label ID="lblXML" runat="server" Text="<%$ Resources:resCorpusCFDIEs,  varXML %>"
                                                                    HeaderStyle-HorizontalAlign="Left" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hpPDF" runat="server" Target="_blank" NavigateUrl='<%# Eval("id_comprobante", "~/Consultas/webDescargarXML.aspx?idcomprobante={0}") %>'>
                                                                    <asp:Image ID="imgPDF" runat="server" ImageUrl="~/Imagenes/xml_mediano.png" Width="30"
                                                                        BorderStyle="None" />
                                                                </asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblPDF" runat="server" Text="<%$ Resources:resCorpusCFDIEs,  varPDF %>"
                                                                    HeaderStyle-HorizontalAlign="Left" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hpXML" runat="server" Target="_blank" NavigateUrl='<%# Eval("id_comprobante", "~/Consultas/webDescargaPDF.aspx?idcomprobante={0}") %>'>
                                                                    <asp:Image ID="imgXML" runat="server" ImageUrl="~/Imagenes/logo_pdf.png" Width="30"
                                                                        BorderStyle="None" />
                                                                </asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="id_comprobande" HeaderText="ID" Visible="False" ReadOnly="true" />
                                                        <%-- <asp:BoundField DataField="rfc_emisor" HeaderText="RFC Emisor" ReadOnly="true" />--%>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblidcomprobante" runat="server" Text='<%# Bind("id_comprobante") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="nombre_emisor" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblCorreoEmisor %>"
                                                            ReadOnly="true" />
                                                        <%--<asp:BoundField DataField="rfc_receptor" HeaderText="RFC Receptor" ReadOnly="true"/>--%>
                                                        <asp:BoundField DataField="nombre_receptor" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblReceptor %>"
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="serie" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblSerie %>"
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="folio" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblFolio %>"
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="uuid" HeaderText="UUID" Visible="false" ReadOnly="true" />
                                                        <asp:BoundField DataField="fecha_documento" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblFechaDoc %>"
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="fecha_validacion" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblFechaValidacion %>"
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="sucursal" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblSucursal %>"
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="version" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblVersion %>"
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="total" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblTotal %>"
                                                            Visible="true" ReadOnly="true" />
                                                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs,  lblValido %>">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chbValido" runat="server" Checked='<%# Bind("valido") %>' Enabled="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs,  lblEstatus %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs,  lblFechaPago %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFechaPago" runat="server" Text='<%# Bind("fecha_pago","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtFechaPago" runat="server" Enabled="false" Width="200px" />
                                                                <asp:Image ID="imgIni" runat="server" ImageUrl="~/Imagenes/icono_calendario.gif" />
                                                                <cc1:CalendarExtender ID="ceFechaPago" runat="server" Enabled="True" TargetControlID="txtFechaPago"
                                                                    Format="dd/MM/yyyy" PopupButtonID="imgIni">
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
                                                    <HeaderStyle BackColor="#880085" Font-Bold="True" ForeColor="#E7E7FF" />
                                                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                                    <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                    <SortedAscendingHeaderStyle BackColor="#594B9C" />
                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                    <SortedDescendingHeaderStyle BackColor="#33276A" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    <table align="right" style="height: 40px; width: 180px">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAnterior" runat="server" CssClass="botonEstilo" Text="<%$ Resources:resCorpusCFDIEs, lbAnterior %>"
                                                    OnClick="btnAnterior_Click" Visible="False" Height="28px" Width="80px" />
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnSiguiente" runat="server" CssClass="botonEstilo" Text="<%$ Resources:resCorpusCFDIEs, lblSiguiente %>"
                                                    OnClick="btnSiguiente_Click" Visible="False" Height="32px" Width="83px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <script type="text/javascript" language="javascript">
                                        var ModalProgress = '<%= modalGenerando.ClientID %>';         
                                    </script>
                                    <asp:Panel ID="pnlGenerando" runat="server" CssClass="modal" BorderStyle="Solid"
                                        BorderWidth="1px" Width="362px">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varProcesando %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdateProgress ID="updGenera" runat="server">
                                                        <ProgressTemplate>
                                                            <img alt="" src="../../Imagenes/imgAjaxLoader.gif" />
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <cc1:ModalPopupExtender ID="modalGenerando" runat="server" BackgroundCssClass="modalBackground"
                                        PopupControlID="pnlGenerando" PopupDragHandleControlID="" TargetControlID="pnlGenerando"
                                        EnableViewState="false" DropShadow="true">
                                    </cc1:ModalPopupExtender>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Label ID="lMensajeError" runat="server" Visible="false"></asp:Label>
</asp:Content>

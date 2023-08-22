<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webDescargaComprobantes.aspx.cs" Inherits="Operacion_Clientes_webDescargaComprobantes" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
                    width:479px;
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
                .imgInformacion
    {
        height: 80px;
        width: 80px;
    }
                .style3
    {
        width: 209px;
    }
    .style4
    {
        width: 122px;
    }
                </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br /><br />
    <table align="center">
        <tr>
            <td>
            <asp:Label ID="Label1" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblTituloDescargaFacturas %>" Style="font-family: 'Avenir LT Std 35 Light'" Font-Names="Avenir LT Std 35 Light" 
                Font-Size="X-Large"> </asp:Label>
            </td>
        </tr>
    </table>
    <br />
<div>
    <table align="center">
        <tr>
            <td>
                <asp:Label ID="lblMensajeFacturas" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblMensajeFactura %>" Font-Bold="True" 
                    ForeColor="#8B181B"/>
            </td>
        </tr>
    </table>
</div>
<asp:UpdatePanel ID="upComprobantes" runat="server">
<ContentTemplate>
<div style="border: 2px ridge #8B181B; width: 643px; margin-left: 147px;" 
        class="bodyMainEmpresas2" >
    <table align="center">
        <tr>
            <td align="left" class="style4">
                    <asp:Label runat="server" ID="lblSucursal" Text="<%$Resources:resCorpusCFDIEs, lblSucursal %>"/>
                
            </td>
        </tr>
        <tr>
            <td align="left" class="style4" colspan="4">
                    <asp:DropDownList ID="ddlSucursal" runat="server" Height="22px" Width="340px" />
            </td>
        </tr>
        <tr>
            <td align="left" class="style4">
                    <asp:Label ID="lblRfc" runat="server" Text="<%$Resources:resCorpusCFDIEs, lblRfcReceptor %>" />
                
            </td>

            <td></td>
                    <td> <asp:Label ID="lblTotal" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTotalFactura %>" /> </td>
        </tr>    
        <tr>
            <td align="left" class="style4">
                    <asp:TextBox ID="txtRfc" runat="server" Width="140px"></asp:TextBox>
                    </td>
                    <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                        ControlToValidate="txtRfc" CssClass="failureNotification" Display="Dynamic" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtRFC %>" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, txtRFC %>" 
                                        ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                        ControlToValidate="txtRfc" CssClass="failureNotification" Display="Dynamic" 
                                        ErrorMessage ="<%$ Resources:resCorpusCFDIEs, regxRFC %>" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxRFC %>" 
                                        ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]" 
                                        ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left">
                                          <asp:TextBox ID="txtTotalFactura" runat="server" Width="140px" />
                                          </td>
                    <td>
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                     ControlToValidate="txtTotalFactura" CssClass="failureNotification" Display="Dynamic" 
                    ErrorMessage ="<%$ Resources:resCorpusCFDIEs, valTotalNumerico %>" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, valTotalNumerico %>" 
                    ValidationExpression="^[0-9]+(\.[0-9]{2})?$" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                        ControlToValidate="txtTotalFactura" CssClass="failureNotification" Display="Dynamic" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valTotalRequerido %>" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, valTotalRequerido %>" 
                                        ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td align="left">
                                        
            </td>
        </tr>    
        <tr>
            <td class="style4">
                    <asp:Label ID="lblSerie" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSerie %>" />
            </td>
            <td>
            </td>
                    <td>
               
               <asp:Label ID="lblFolio" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFolio %>" />
            </td>
        </tr>
         <tr>
            <td class="style4">
                    <asp:TextBox ID="txtSerie" runat="server" Width="140px"/>

            </td>

            <td>
            </td>
                    <td>
             
             <asp:TextBox ID="txtFolio" runat="server" Width="140px"/>
                </td>
                <td>
            <asp:RegularExpressionValidator ID="revNumero" runat="server"
                                                            ControlToValidate="txtFolio" CssClass="failureNotification" Display="Dynamic" 
                                                            ErrorMessage ="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                            ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                            ValidationExpression="[0-9]+" 
                                                            ValidationGroup="RegisterUserValidationGroup"><img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                        ControlToValidate="txtFolio" CssClass="failureNotification" Display="Dynamic" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFolio %>" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, valFolio %>" 
                                        ValidationGroup="RegisterUserValidationGroup">
                                        <img src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>--%>
               
            </td>
        </tr>
        <%--<tr>
            <td class="style4">
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varUUID %>" />
            
            </td>
            
        </tr>
        <tr>
            <td class="style4">
            <asp:TextBox ID="txtUUID" runat="server" MaxLength="128" Width="163px"></asp:TextBox>
            </td>
                    
        </tr>--%>
        <tr>
            <td class="style4">
                    
                    <asp:Label ID="lblFecha" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechaFormato %>" />     
            </td>
        </tr>
        <tr>
            <td class="style4">
                
                <asp:TextBox ID="txtFecha" runat="server" Width="157px" Height="19px"></asp:TextBox>
            </td>
                    <td>
            <asp:Image ID="imgFIni" runat="server" 
                                            ImageUrl="~/Imagenes/icono_calendario.gif" />
                            <cc1:CalendarExtender ID="ceFecha" runat="server" 
                                Enabled="True" TargetControlID="txtFecha" Format="dd/MM/yyyy" 
                                PopupButtonID="imgFIni">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvFecha" runat="server" 
                                        ControlToValidate="txtFecha" CssClass="failureNotification" Display="Dynamic" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" 
                                        ValidationGroup="RegisterUserValidationGroup">
                                        <img src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="right" >
                <asp:Button ID="btnBuscar" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnBuscar %>" 
                    CssClass="botonEstiloVentanas" onclick="btnBuscar_Click" ValidationGroup="RegisterUserValidationGroup" />
            </td>
        </tr>
    </table>
</div>
</ContentTemplate>

</asp:UpdatePanel>
<asp:LinkButton ID="lkbConsultaCFDI" runat="server"></asp:LinkButton>
<cc1:ModalPopupExtender ID="mpeConsultaCFDI" runat="server"
    TargetControlID ="lkbConsultaCFDI" 
    PopupControlID ="pnlConsultaCFDI" 
    BackgroundCssClass ="modalBackground">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlConsultaCFDI" runat="server"
          BorderStyle="Solid" BorderWidth ="1px" Width="950px" BackColor="White" >
          <asp:UpdatePanel ID="udpConsultaCFDI" runat="server">
          <ContentTemplate>
            <table >
                 <tr>
                        <td>
                            <img alt="" class="imgInformacion" 
                             src="../../Imagenes/Informacion.png" />    
                        </td>
                        <td align="center">
                           <asp:Label ID="lblAviso" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblAvisoCFDI %>" style="font-size: small; font-family: 'Century Gothic'" ForeColor="#0085A6" ></asp:Label>
                        </td>
                        <td>    
                
                        </td>
                     </tr>
                     <tr>
                     <td>
                     </td>
                        <td align="right">
                            <asp:CheckBox ID="cbSeleccionar" runat="server" AutoPostBack="True" 
                                                    OnCheckedChanged="cbSeleccionar_CheckedChanged" 
                                                    Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionarTodo %>" TextAlign="Left" 
                                                     />
                        </td>
                     </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td> 
                    <asp:GridView ID="gvComprobantes" runat="server" 
                            BorderColor="White"
                            Width="800px" BackColor="White" BorderStyle="Ridge" BorderWidth="2px" 
                            CellPadding="3" CellSpacing="1" GridLines="None" AutoGenerateColumns="false">
                            <Columns>
                            <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblidcomprobante" runat="server" Text='<%# Bind("id_comprobante_cliente") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblXML" runat="server"  Text="<%$ Resources:resCorpusCFDIEs,  varXML %>" HeaderStyle-HorizontalAlign="Left" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hpPDF" runat="server" Target="_blank"
                                            NavigateUrl='<%# Eval("id_comprobante_cliente", "~/Consultas/webDescargarXML.aspx?idcomprobante={0}&tipocomprobante=1") %>'> 
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
                                            NavigateUrl='<%# Eval("id_comprobante_cliente", "~/Consultas/webDescargaPDF.aspx?idcomprobante={0}&tipocomprobante=1") %>'>
                                            <asp:Image ID="imgXML" runat="server" ImageUrl="~/Imagenes/logo_pdf.png" Width="30"  BorderStyle="None" />
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="id_comprobante_cliente" HeaderText="ID" Visible="false" ReadOnly="true" />
                                <%-- <asp:BoundField DataField="rfc_emisor" HeaderText="RFC Emisor" ReadOnly="true" />--%>
                               <%-- <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblidcomprobante" runat="server" Text='<%# Bind("id_comprobante") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="nombre_emisor" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblCorreoEmisor %>" ReadOnly="true" />
                                <%--<asp:BoundField DataField="rfc_receptor" HeaderText="RFC Receptor" ReadOnly="true"/>--%>
                                <asp:BoundField DataField="nombre_receptor" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblReceptor %>" ReadOnly="true" />
                                <asp:BoundField DataField="serie" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblSerie %>" ReadOnly="true" />
                                <asp:BoundField DataField="folio" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblFolio %>" ReadOnly="true" />
                                <asp:BoundField DataField="fecha_documento" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblFechaDoc %>" ReadOnly="true" />
                                <asp:BoundField DataField="total" HeaderText="<%$ Resources:resCorpusCFDIEs,  lblTotal %>" Visible="true" ReadOnly="true" />
                                
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
                        <%--<div  align="center">
                                <table>
                                    <tr>    
                                        <td>
                                            <asp:HyperLink ID="hpPDF" runat="server" Target="_blank"><asp:Image ID="imgPDF" runat ="server" ImageUrl="~/Imagenes/logo_pdf.png" Height="44px" Width="44px" BorderWidth="0" /></asp:HyperLink>
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="hpXML" runat="server" Target="_blank"><asp:Image ID="imgXML" runat="server" ImageUrl="~/Imagenes/xml_mediano.png" Height="48px" Width="48" BorderWidth="0" /></asp:HyperLink>                    
                                        </td>
                                    </tr>
                                </table>
                        </div>--%>
                    </td>
                    <td>    
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                       </td>
                     
                     <td>
                     <table width="100%">
                        <tr>
                            <td align="left"style=" width:600px">
                                <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" />           
                            </td>
                            <td align="right">
                            <asp:Button ID="btnDescargar" runat="server" CssClass="botonEstiloVentanas"
                            Height="32px" Width="77px" Text ="<%$ Resources:resCorpusCFDIEs, btnDescargar %>" onclick="btnDescargar_Click"/>        
                                <asp:Button ID="btnConsultaCDFI" runat="server" CssClass="botonEstiloVentanas"
                            Height="32px" Width="77px" Text ="<%$ Resources:resCorpusCFDIEs, mnuSalir %>" onclick="btnConsultaCDFI_Click"/>
                            
                            </td>
                        </tr>
                     </table>
                     
                        
                            
                     </td>
                </tr>
            </table>
            </ContentTemplate>

</asp:UpdatePanel>
            </asp:Panel>



</asp:Content>


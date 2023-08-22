<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="webDescargaComprobantes.aspx.cs" Inherits="PAXRecepcionProveedores.Operacion.Clientes.webDescargaComprobantes" %>
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
    <br /><br /><br /><br />
<div>
    <table>
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
<div style="border-style: ridge; border-width: 2px; border-color: #8B181B; width=100%; height=100%">
    <table>
        <tr>
            <td align="left" class="style4">
                    <asp:Label runat="server" ID="lblSucursal" Text="<%$Resources:resCorpusCFDIEs, lblSucursal %>"/>
                
            </td>
        </tr>
        <tr>
            <td align="left" class="style4">
                    <asp:DropDownList ID="ddlSucursal" runat="server" Height="22px" Width="340px" />
            </td>
        </tr>
        <tr>
            <td align="left" class="style4">
                    <asp:Label ID="lblRfc" runat="server" Text="<%$Resources:resCorpusCFDIEs, lblRfcReceptor %>" />
                
            </td>
            <td> <asp:Label ID="lblFolio" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFolio %>" /> </td>
        </tr>    
        <tr>
            <td align="left" class="style4">
                    <asp:TextBox ID="txtRfc" runat="server" Width="140px"></asp:TextBox>
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
                                        <td align="left"><asp:TextBox ID="txtFolio" runat="server" Width="140px"/></td>
                                        <td align="left"><asp:RegularExpressionValidator ID="revNumero" runat="server"
                                                            ControlToValidate="txtFolio" CssClass="failureNotification" Display="Dynamic" 
                                                            ErrorMessage ="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                            ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                            ValidationExpression="[0-9]+" 
                                                            ValidationGroup="RegisterUserValidationGroup"><img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator></td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                        ControlToValidate="txtFolio" CssClass="failureNotification" Display="Dynamic" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtRFC %>" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, txtRFC %>" 
                                        ValidationGroup="RegisterUserValidationGroup">
                                        <img src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                        
            </td>
        </tr>    
        <tr>
            <td class="style4">
                    <asp:Label ID="lblSerie" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSerie %>" />
            </td>
            <td>
              <asp:Label ID="lblFecha" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechaFormato %>" />      
            </td>
        </tr>
         <tr>
            <td class="style4">
                    <asp:TextBox ID="txtSerie" runat="server" Width="140px"/>

            </td>
            <td>
                <asp:TextBox ID="txtFecha" runat="server" Width="200px"></asp:TextBox>
            </td>
            <td><asp:Image ID="imgFIni" runat="server" 
                                            ImageUrl="~/Imagenes/icono_calendario.gif" />
                            <cc1:CalendarExtender ID="ceFecha" runat="server" 
                                Enabled="True" TargetControlID="txtFecha" Format="dd/MM/yyyy" 
                                PopupButtonID="imgFIni">
                            </cc1:CalendarExtender></td></td>
        </tr>
        <tr>
            <td class="style4">
            <asp:Label ID="lblTotal" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTotalFactura %>" />
            </td>
            <td>
        </tr>
        <tr>
            <td class="style4">
                 <asp:TextBox ID="txtTotalFactura" runat="server" Width="140px" />
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                     ControlToValidate="txtTotalFactura" CssClass="failureNotification" Display="Dynamic" 
                    ErrorMessage ="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                    ValidationExpression="^[0-9]+(\.[0-9]{2})?$" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                        ControlToValidate="txtTotalFactura" CssClass="failureNotification" Display="Dynamic" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtRFC %>" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, txtRFC %>" 
                                        ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style4">
                    
                    
            </td>
        </tr>
        <tr>
            <td class="style4">
                
                
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="btnBuscar" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnBuscar %>" 
                    CssClass="botonEstiloVentanas" onclick="btnBuscar_Click" ValidationGroup="RegisterUserValidationGroup" />
            </td>
        </tr>
    </table>
</div>
<asp:LinkButton ID="lkbConsultaCFDI" runat="server"></asp:LinkButton>
<cc1:ModalPopupExtender ID="mpeConsultaCFDI" runat="server"
    TargetControlID ="lkbConsultaCFDI" 
    PopupControlID ="pnlConsultaCFDI" 
    BackgroundCssClass ="modalBackground">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlConsultaCFDI" runat="server"
         CssClass ="modal" BorderStyle="Solid" BorderWidth ="1px" Width="606px" >
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
                        &nbsp;</td>
                    <td> 
                        <div  align="center">
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
                        </div>
                    </td>
                    <td>    
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                     
                     <td align="center">
                        <asp:Button ID="btnConsultaCDFI" runat="server" CssClass="botonEstiloVentanas"
                            Height="32px" Width="77px" Text ="OK" onclick="btnConsultaCDFI_Click"/>
                     </td>
                </tr>
            </table>
            </asp:Panel>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
    <asp:AsyncPostBackTrigger ControlID="btnConsultaCDFI" EventName="Click" />
</Triggers>
</asp:UpdatePanel>


</asp:Content>

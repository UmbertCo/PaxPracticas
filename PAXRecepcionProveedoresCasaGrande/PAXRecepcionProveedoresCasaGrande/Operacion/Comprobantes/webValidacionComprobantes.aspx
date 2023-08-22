<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webValidacionComprobantes.aspx.cs" Inherits="Operacion_Comprobantes_webValidacionComprobantes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

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
         //         $find(ModalProgress)._backgroundElement.style.zIndex += 10;
         //         $find(ModalProgress)._foregroundElement.style.zIndex += 10;
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
                .style2
                {
                    width: 28px;
                }
                .modalBackground
                {
                    background-color: #666699;
                    filter: alpha(opacity=50);
                    opacity: 0.7;
                }
                </style>
    <br /><br /><br /><br />
    <h2>
        <asp:Label ID="lblTitulo" runat="server"  
            Text="<%$ Resources:resCorpusCFDIEs, lblValidacion %>" Font-Bold="True" 
            ForeColor="#9C1518"></asp:Label>
    </h2>
    <table width="100%"; height="100%" style="border: 2px ridge #9C1518" class="bodyMainEmpresas">
        <tr>
            <td>
            <div>
    <asp:Label ID="lblSucursal" runat="server" AssociatedControlID="ddlSucursales" Text="<%$ Resources:resCorpusCFDIEs, lblSucursal %>" />
    <p>
        <asp:DropDownList ID="ddlSucursales" runat="server" Width="350px" />
        </p>
    </div>
   <%-- <center>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="udpValidacion" ID="updProgress">
                <ProgressTemplate>
                    <img alt="" src="../../Imagenes/imgAjaxLoader.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            </center>--%>
   
    <p><asp:Label runat="server" ID="Label1" AssociatedControlID="fuPdf"  Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionaZIP %>"/><br />
    <asp:Label runat="server" ID="lblMaxZip"  Text=""/></p>
    <table>
        <tr>
            <td>
                <asp:FileUpload ID="fuComprobantes" runat="server" Width="320px" />
                </td>
                <td>
                <asp:Button ID="btnSubir" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubir %>" OnClick="btnSubir_Click" CssClass="botonEstiloVentanas" />
            </td>
        </tr>
        <tr>
            <td>
                <p><asp:Label runat="server" ID="lblfuPdf" AssociatedControlID="fuPdf" Text="<%$ Resources:resCorpusCFDIEs, lblPDF %>"/></p>
                <p><asp:FileUpload ID="fuPdf" runat="server" Width="320px"/></p>
            </td>
        </tr>
        </table>
         <asp:UpdatePanel ID="udpValidacion" runat="server" >
    
    <ContentTemplate>
        <table>
        <tr>
            <td>
                <asp:GridView ID="gvComprobantes" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="White" Width="600px" BorderStyle="Ridge" BorderWidth="2px" 
                    CellPadding="3" CellSpacing="1" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="nombre_xml"  HeaderText="<%$ Resources:resCorpusCFDIEs, varXML %>" />
                        <asp:BoundField DataField="nombre_pdf" HeaderText="<%$ Resources:resCorpusCFDIEs, varPDF %>" />
                        <asp:TemplateField  HeaderText="<%$ Resources:resCorpusCFDIEs, lblValidar %>" Visible="False">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked="False" Enabled="false" /> 
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblValido %>">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("valido") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("valido") %>' Enabled="false"/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                       <asp:BoundField DataField="error"  HeaderText="<%$ Resources:resCorpusCFDIEs, lblCorreoMensaje %>" />
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
            <td align="right" valign="top">
                <asp:Button ID="btnValidar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblValidar %>" 
                    CssClass="botonEstiloVentanas" onclick="btnValidar_Click" />
            </td>
            <td align="right" valign="top">
                <asp:Button ID="btnLimpiar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, btnLimpiar %>" 
                    CssClass="botonEstiloVentanas" onclick="btnLimpiar_Click" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label>
    </ContentTemplate>
    <Triggers>
       <%-- <asp:PostBackTrigger ControlID="btnSubir"/>--%>
        <asp:AsyncPostBackTrigger ControlID="btnValidar" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>
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
                targetcontrolid="pnlGenerando" EnableViewState="False" DropShadow="true">
            </cc1:modalpopupextender>
            </td>
        </tr>
</table>    
</asp:Content>


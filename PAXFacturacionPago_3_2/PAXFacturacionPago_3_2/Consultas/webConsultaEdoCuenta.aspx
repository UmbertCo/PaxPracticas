<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webConsultaEdoCuenta.aspx.cs" Inherits="Consultas_webConsultaEdoCuenta" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<style type="text/css" >
    div.textos input[type='text']
    {
        width:300px;
    }
    div.textos select
    {
    }
    .sinBorde img
    {
        border-style:none;
    }
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript" language="javascript">

    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginReq);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);

    function beginReq(sender, args) {
        // muestra el popup 
        $find(ModalProgress).show();
    }

    function endReq(sender, args) {
        //  esconde el popup 
        $find(ModalProgress).hide();
    } 

            </script>

            <style type="text/css" >
                #sugerencias_rfc
                {
                    position:absolute;
                    float:left;
                    border: 1px solid black;
                    background-color:white;
                    width:300px;
                }
                #sugerencias_rfc a
                {
                    color:#333333;
                    text-decoration:none;
                }
                #sugerencias_rfc a.opSelecionada
                {
                    background-color:#CCCCCC;
                    color:navy;
                    width:300px;
                    display:inline-block;
                }
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
                .style3
                {
                    width: 174px;
                }
                .style4
                {
                    width: 185px;
                }
            </style>

            <h2>
                <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloConsultasEdoCuenta %>"></asp:Label>
            </h2>
            <div style="text-align:center;" >
            <asp:UpdateProgress ID="uppConsultas" runat="server" AssociatedUpdatePanelID="udpControles">
                    <progresstemplate>
                        <img alt="" 
                    src="../Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>
            </div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td width="350px">

                    </td>
                    <td align="right" width="560px">
                        <asp:UpdatePanel ID="UpdatelblCreditos" runat="server">
                        <ContentTemplate>
                         <asp:Label ID="lblCredito" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblCreditos %>" Font-Bold="True"></asp:Label>
                        <asp:Label ID="lblCredValor" runat="server" ></asp:Label>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <div class="accountInfo textos" style="width:900px; height:150px;">
             <fieldset class="register" style="width:890px;">
                    <legend><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" /></legend>
                     <table class="style1">
                        <tr>
                            <td class="style4">
                            
                                <asp:Label ID="lblCliAfi" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCliAli %>"></asp:Label>
                            
                            </td>
                            <td class="style3">
                            
                                &nbsp;</td>
                            <td>
                            
                                &nbsp;</td>
                            <td>
                            
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style4">
                            
                                <asp:DropDownList ID="ddlCliAfi" runat="server" Height="18px" Width="260px" 
                                    DataTextField="clave_usuario" DataValueField="id_usuario" TabIndex="1">
                                </asp:DropDownList>
                            
                            </td>
                            <td class="style3">
                            
                                &nbsp;</td>
                            <td>
                            
                                &nbsp;</td>
                            <td>
                            
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style4">
                            
                                <asp:Label ID="lblFechaIni" runat="server"
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                    AssociatedControlID="txtFechaIni"></asp:Label>
                            
                            </td>
                            <td class="style3">
                            
                                <asp:Label ID="lblFechaFin" runat="server"
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                    AssociatedControlID="txtFechaFin"></asp:Label>
                            
                            </td>
                            <td>
                            
                            </td>
                            <td>
                            
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">
                            
                                <asp:TextBox ID="txtFechaIni" runat="server" BackColor="White" Width="100px" 
                                    TabIndex="2" ></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaIni" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgIni">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgIni" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaIni" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img src="../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvFechaIni" runat="server" 
                                    ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                    ValidationGroup="grupoConsulta" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" Width="16px" >*</asp:RequiredFieldValidator>
                            
                            </td>
                            <td class="style3">
                            
                                <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White" Width="100px" 
                                    TabIndex="3"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgFin">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgFin" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaFin" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img src="../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" 
                                    ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                    ValidationGroup="grupoConsulta" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" Width="16px" >*</asp:RequiredFieldValidator>
                            
                            </td>
                            <td>
                            
                            </td>
                            <td>
                            
                            </td>
                        </tr>
                     </table>
             </fieldset>

            </div> 
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <p style="text-align:right;" >
                    <asp:Button ID="btnConsultar" runat="server" CssClass="botonEstilo" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                        ValidationGroup="grupoConsulta" onclick="btnConsultar_Click" 
                        TabIndex="4" />
                    <asp:Button ID="btnDescargar" runat="server" CssClass="botonEstilo" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnDescargar %>" 
                        ValidationGroup="grupoConsulta" Visible="False" Enabled="False" />
                    <asp:Button ID="btnCancelar" runat="server" CssClass="botonEstilo" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                        ValidationGroup="grupoConsulta" Visible="False" />
                    <asp:Button ID="btnExportar" runat="server" CssClass="botonGrande" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblExcel %>" 
                        ValidationGroup="grupoConsulta" Visible="False" Enabled="False" />
                </p>
                </ContentTemplate>
            </asp:UpdatePanel>
         <asp:UpdatePanel ID="udpControles" runat="server">
            <ContentTemplate>  
            <br />
            <asp:Panel runat="server" CssClass="sinBorde" ID="Panel234">
                <asp:GridView ID="gdvCreditos" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" GridLines="Horizontal" Width="100%" AllowPaging="True" 
                    DataKeyNames="fecha" BackColor="White" 
                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                    onpageindexchanging="gdvCreditos_PageIndexChanging" 
                    onselectedindexchanged="gdvCreditos_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="clave_usuario"
                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblClienteNomCol %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                         <ItemStyle Width="150px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="fecha"
                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblFecha %>" 
                            HeaderStyle-HorizontalAlign="Left" DataFormatString=" {0:dd/MM/yyyy}" >
                        <HeaderStyle HorizontalAlign="Left" />
                         <ItemStyle Width="150px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="cantidad" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblCantidad %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="80px" />
                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="saldo" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblSaldo%>" 
                            HeaderStyle-HorizontalAlign="Right" >
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle Width="100px" />
                        <ItemStyle HorizontalAlign="Right" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="estatus" 
                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus%>" 
                        HeaderStyle-HorizontalAlign="Right">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle Width="150px" />
                        <ItemStyle HorizontalAlign="Right" Width="150px" />
                        </asp:BoundField>
<%--                        <asp:TemplateField Visible="true" 
                        HeaderText = "<%$ Resources:resCorpusCFDIEs, lblDetalles%>"
                        HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:LinkButton ID="LBVerDetalle" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblVer %>" 
                                    onclick="LBVerDetalle_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Width="80px" />    
                        </asp:TemplateField>--%>
                        <asp:CommandField ShowSelectButton="True" 
                         HeaderText="<%$ Resources:resCorpusCFDIEs, lblDetalles%>"
                        SelectText="<%$ Resources:resCorpusCFDIEs, lblVer%>"
                         HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px" />
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
            </asp:Panel>

            <asp:Panel ID="pnlDetalle" runat="server" Height="480px" Width="752px" 
                CssClass="modal" BorderStyle="Solid" BorderWidth="2px" BackColor="White">
                <fieldset>
                <legend><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloConsultaEdoCuentaDet %>" /></legend>
                <table>
                    <tr>
                        <td>

                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                          
                            <asp:GridView ID="gdvDetalle" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" BackColor="White" BorderColor="#336666" 
                                BorderStyle="Double" BorderWidth="3px" CellPadding="4" DataKeyNames="fecha" 
                                GridLines="Horizontal" onpageindexchanging="gdvDetalle_PageIndexChanging" 
                                Width="720px" style="z-index: -1">
                                <Columns>
                                    <asp:BoundField DataField="fecha" DataFormatString=" {0:dd/MM/yyyy}" 
                                        HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblFecha %>">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fecha" DataFormatString=" {0:t}"
                                        HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblHora %>">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="80px" />
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="generado_por" HeaderStyle-HorizontalAlign="Right" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblGeneradoPor%>">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle Width="100px" />
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                    </asp:BoundField>
                                             <asp:BoundField DataField="serie" HeaderStyle-HorizontalAlign="Right" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblSerie%>">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle Width="80px" />
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                    </asp:BoundField>
                                             <asp:BoundField DataField="folio" HeaderStyle-HorizontalAlign="Right" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblFolio%>">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle Width="80px" />
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="estatus" HeaderStyle-HorizontalAlign="Right" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus%>">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle Width="150px" />
                                    <ItemStyle HorizontalAlign="Right" Width="150px" />
                                    </asp:BoundField>                                   
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Literal ID="Literal3" runat="server" 
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
                    <tr>
                        <td align="right">



                        </td>
                    </tr>
                </table>
                <table align="right">
                    <tr>
                        <td align="right">
                                <p align="right">
                              <asp:Button ID="btnAcepDetalle" runat="server" CssClass="botonEstilo"  
                                Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" 
                                ValidationGroup="creditos" onclick="btnAcepDetalle_Click" />              
                                </p>                        
                        </td>
                    </tr>
                
                </table>

                </fieldset>
                </asp:Panel>

                <cc1:modalpopupextender id="modalDetalle" 
                runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlDetalle" popupdraghandlecontrolid=""
                    targetcontrolid="lnkDetalle" CancelControlID="btnAcepDetalle">
                </cc1:modalpopupextender>
                <asp:LinkButton ID="lnkDetalle" runat="server"></asp:LinkButton>

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
                                    <img alt="" src="../Imagenes/imgAjaxLoader.gif" />
                                </progresstemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <cc1:modalpopupextender id="modalGenerando" runat="server" backgroundcssclass="modalBackground" 
            popupcontrolid="pnlGenerando" popupdraghandlecontrolid=""
                targetcontrolid="pnlGenerando">
            </cc1:modalpopupextender>

            <script type="text/javascript" language="javascript">
                var ModalProgress = '<%= modalGenerando.ClientID %>';         
            </script>

                      <asp:Panel ID="pnlRevisaCreditos" runat="server" Height="130px" Width="579px" 
                CssClass="modal" BorderStyle="Solid" BorderWidth="1px">

                <table align="center">
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label7" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblMsgCreditos %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <img alt="" src="../Imagenes/Información.png" width="44" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnAceptCred" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" 
                                CssClass="botonEstilo" />
                        </td>
                    </tr>
                </table>
                </asp:Panel>

            <cc1:modalpopupextender id="modalCreditos" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlRevisaCreditos" popupdraghandlecontrolid=""
                targetcontrolid="lnkRevisaCreditos">
            </cc1:modalpopupextender>

            <asp:LinkButton ID="lnkRevisaCreditos" runat="server"></asp:LinkButton>


            </ContentTemplate>
         </asp:UpdatePanel>
</asp:Content>


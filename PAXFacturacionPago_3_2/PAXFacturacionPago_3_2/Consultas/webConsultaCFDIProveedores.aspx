<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webConsultaCFDIProveedores.aspx.cs" Inherits="Consultas_webConsultaCFDIProveedores" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

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
    .modal
    {}
    .style2
    {
        width: 318px;
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
            </style>

            <h2>
                <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloConsultas %>"></asp:Label>
            </h2>
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
            <div class="accountInfo textos" style="width:900px;">
                <fieldset class="register" style="width:890px;">
                    <legend><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" /></legend>
                    <table class="style1">
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblReceptor" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblProvider %>" 
                                    AssociatedControlID="ddlReceptor" Height="18px" Width="142px"></asp:Label>
                                <asp:DropDownList ID="ddlReceptor" runat="server" 
                                    DataTextField="razon_social" DataValueField="rfc">
                                </asp:DropDownList>
                            </td>
                            <td>
                              
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
                                <asp:Label ID="lblReceptor0" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaDoc %>" 
                                    AssociatedControlID="ddlReceptor" Height="18px" Width="142px"></asp:Label>
                                </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblFechaIni" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                    AssociatedControlID="txtFechaIni"></asp:Label>
                                <asp:TextBox ID="txtFechaIni" runat="server" BackColor="White" Width="100px" ></asp:TextBox>
                                <asp:Image ID="imgIni" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaIni" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img src="../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaIni" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgIni">
                                </cc1:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblFechaFin" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                    AssociatedControlID="txtFechaFin"></asp:Label>
                                <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White" Width="100px"></asp:TextBox>
                                <asp:Image ID="imgFin" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaFin" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img src="../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgFin">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblReceptor1" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaRec %>" 
                                    AssociatedControlID="ddlReceptor" Height="18px" Width="142px"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblFechaIni0" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                    AssociatedControlID="txtFechaIni"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblFechaFin0" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                    AssociatedControlID="txtFechaFin"></asp:Label>
                                </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:TextBox ID="txtFechaIniRec" runat="server" BackColor="White" 
                                    Width="100px" ></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFechaIniRec_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaIniRec" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgIni0">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgIni0" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaIni0" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaIniRec" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img src="../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFechaFinRec" runat="server" BackColor="White" Width="100px"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFechaFinRec_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaFinRec" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgFin0">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgFin0" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaFin0" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaFinRec" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img src="../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <asp:UpdateProgress ID="uppConsultas" runat="server" 
                    AssociatedUpdatePanelID="UpdatePanel1" >
                    <progresstemplate>
                        <img alt="" 
                    src="../Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                
                <p style="text-align:right;" >
                    <asp:Button ID="btnConsultar" runat="server" CssClass="botonEstilo" 
                        onclick="btnConsultar_Click" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                        ValidationGroup="grupoConsulta" />
                    <asp:Button ID="btnDescargar" runat="server" CssClass="botonEstilo" 
                        onclick="btnDescargar_Click" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnDescargar %>" 
                        ValidationGroup="grupoConsulta" Visible="False" />
                    <asp:Button ID="btnExportar" runat="server" CssClass="botonGrande" 
                        onclick="btnExportar_Click" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblExcel %>" 
                        ValidationGroup="grupoConsulta" Visible="False" />
                </p>
                      
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnDescargar" />
                    <asp:PostBackTrigger ControlID="btnExportar" />
                </Triggers>
            </asp:UpdatePanel>
        <asp:UpdatePanel ID="udpControles" runat="server">
        <ContentTemplate>  
            &nbsp;<table style="width: 929px">
                <tr>
                    <td align="right">
                        <table align="right">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cbPaginado" runat="server" AutoPostBack="True" 
                                        oncheckedchanged="cbPaginado_CheckedChanged" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblPaginado %>" TextAlign="Left" 
                                        Visible="False" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cbSeleccionar" runat="server" AutoPostBack="True" 
                                        oncheckedchanged="cbSeleccionar_CheckedChanged" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionarTodo %>" TextAlign="Left" 
                                        Visible="False" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" CssClass="sinBorde" ID="Panel234" >
                <asp:GridView ID="gdvComprobantes" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" GridLines="Horizontal" Width="100%" 
                    DataKeyNames="id_cfd" 
                     BackColor="White" 
                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                    onrowcommand="gdvComprobantes_RowCommand" AllowPaging="True" 
                    onpageindexchanging="gdvComprobantes_PageIndexChanging">
                    <Columns>
                        <asp:ButtonField ButtonType="Image" CommandName="xml" 
                            ImageUrl="~/Imagenes/xml_mediano.png">
                        <ControlStyle Height="30px" Width="30px" />
                        <ItemStyle Height="20px" Width="20px" />
                        </asp:ButtonField>
                        <asp:ButtonField ButtonType="Image" CommandName="pdf" 
                            ImageUrl="~/Imagenes/logo_pdf.png">
                        <ControlStyle Height="30px" Width="30px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="serie" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblSerie %>" HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="folio" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblFolio %>" HeaderStyle-HorizontalAlign="Right" >
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="razon_social" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblReceptor %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="total" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblImporte %>" HeaderStyle-HorizontalAlign="Right"
                            DataFormatString="{0:c}" >
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="fecha_documento" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblFechaDoc %>" HeaderStyle-HorizontalAlign="Left"
                            DataFormatString="{0:d}" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="fecha_recepcion" 
                         HeaderText="<%$ Resources:resCorpusCFDIEs, lblFechaRec %>" HeaderStyle-HorizontalAlign="Left"
                            DataFormatString="{0:d}" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="estatus" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblid_cfd" runat="server" Text='<%# Bind("id_cfd") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="motivo_rechazo">
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSeleccion" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
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
                            <img alt="" src="../Imagenes/Informacion.png" width="44" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnAceptCred" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" 
                                CssClass="botonEstilo" onclick="btnAceptCred_Click" />
                        </td>
                    </tr>
                </table>
                </asp:Panel>

            <cc1:modalpopupextender id="modalRevisaCreditos" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlRevisaCreditos" popupdraghandlecontrolid=""
                targetcontrolid="lnkRevisaCreditos">
            </cc1:modalpopupextender>

            <asp:LinkButton ID="lnkRevisaCreditos" runat="server"></asp:LinkButton>

            <br />

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

                <div style="text-align:center;">
                </div>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


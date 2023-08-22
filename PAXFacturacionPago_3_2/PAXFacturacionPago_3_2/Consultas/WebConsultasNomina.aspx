<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebConsultasNomina.aspx.cs" Inherits="Consultas_WebConsultasNomina" MasterPageFile="~/webOperatorMaster.master" %>

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
    .modal
        {
            padding: 10px 10px 10px 10px;
            border:1px solid #333333;
            background-color:White;
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

                //                function startProgressBar() {
                //                    divProgressBar.style.visibility = "visible";
                //                    pMessage.style.visibility = "visible";

                //                    progress_update();
                //                }


                function beginReq(sender, args) {
                    // muestra el popup 
                    $find(ModalProgress).show();
                }

                function endReq(sender, args) {
                    //  esconde el popup 
                    $find(ModalProgress).hide();
                }

                function clickOnce(btn, msg) {
                    // Comprobamos si se está haciendo una validación
                    if (typeof (Page_ClientValidate) == 'function') {
                        // Si se está haciendo una validación, volver si ésta da resultado false
                        if (Page_ClientValidate() == false) { return false; }
                    }

                    // Asegurarse de que el botón sea del tipo button, nunca del tipo submit
                    if (btn.getAttribute('type') == 'button') {
                        // El atributo msg es totalmente opcional. 
                        // Será el texto que muestre el botón mientras esté deshabilitado
                        if (!msg || (msg = 'undefined')) { msg = 'Loading...'; }

                        btn.value = msg;

                        // La magia verdadera :D
                        btn.disabled = true;
                    }

                    return true;
                }

                function mostrarProgressBar() {
                    $find(ModalProgress).show();
                }

                function ocultarProgressBar() {
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
                <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloConsultasNomina %>"></asp:Label>
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
                <tr>
                    <td width="350px" align="left" colspan="2" style="width: 910px">

                        <br />
                            <asp:Label ID="lblSesion" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, varAlmacenMeses %>" 
                            style="font-weight: 700; color: #3A5C6B;"></asp:Label>

                    </td>
                </tr>
            </table>
    <asp:UpdatePanel ID="upnlDatos" runat="server">
        <ContentTemplate>
        
            <div class="accountInfo textos" style="width:900px;height:260px;">
                <fieldset class="register" style="width:890px;">
                    <legend><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" /></legend>
                    <table style="width: 860px">
                        <tr>
                            <td>
                                <asp:Label ID="LalblNombreSucursalbel3" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblSucursal %>" 
                                    AssociatedControlID="ddlSucursales"></asp:Label>
                                <asp:DropDownList ID="ddlSucursales" runat="server" DataTextField="nombre" 
                                    DataValueField="id_estructura" 
                                   
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblEstatus" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblEstatus %>" 
                                    AssociatedControlID="ddlEstatus"></asp:Label>
                                <asp:DropDownList ID="ddlEstatus" runat="server" DataTextField="estatus" 
                                    DataValueField="clave">
                                </asp:DropDownList>
                        
                                 
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblEmpleado" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEmpleado %>" AssociatedControlID="txtEmpleado"></asp:Label>
                                <asp:TextBox ID= "txtEmpleado" runat="server"  MaxLength = "128" Text=""></asp:TextBox>
                                 <asp:ImageButton ID="imgbBuscarEmpleado" runat="server" AlternateText="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaReceptor %>" 
                                CssClass="imagenModal" Height="20px" ImageUrl="~/Imagenes/lupa.png" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaReceptor %>" 
                                Width="20px" />
                            <cc1:ModalPopupExtender ID="linkModal_ModalPopupExtender" runat="server" 
                                BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True" 
                                PopupControlID="pnlBusquedaEmpleados" TargetControlID="imgbBuscarEmpleado">
                            </cc1:ModalPopupExtender>
                            </td> 
                            <td>
                              <asp:Label ID="Label4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>" AssociatedControlID="txtEmpleado"></asp:Label>
                              <asp:TextBox ID= "txtRFC" runat="server" ValidationGroup="grupoRfc"  MaxLength = "128"></asp:TextBox>
                                 <asp:RegularExpressionValidator ID="regxRFC" runat="server" 
                                        ControlToValidate="txtRFC" CssClass="failureNotification" Display="Dynamic" 
                                        ToolTip="RFC incompleto" 
                                        ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]" 
                                        ValidationGroup="grupoRfc"><img src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                         <asp:RequiredFieldValidator ID="rfvRfc" runat="server" 
                                        ControlToValidate="txtRFC" CssClass="failureNotification" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, valRfc %>" ValidationGroup="grupoRfc" 
                                        Width="16px">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPeriodos" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblPeriodos %>">
                                </asp:Label>
                                <br />
                                <asp:DropDownList ID="ddlPeriodos" runat="server" DataTextField="Descripcion" 
                                    DataValueField="IdTipoPeriodo" TabIndex="20" Width="300px" 
                                    OnDataBound="ddlPeriodos_DataBound">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
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
                                <asp:RequiredFieldValidator ID="rfvFechaIni" runat="server" 
                                    ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                    ValidationGroup="grupoConsulta" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" Width="16px" >*
                                </asp:RequiredFieldValidator>
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
                                <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" 
                                    ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                    ValidationGroup="grupoConsulta" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" Width="16px" >*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        
                        
                    </table>
                </fieldset>
            </div>

            </ContentTemplate>
    </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <p style="text-align:right;" >
                    <asp:Button ID="btnConsultar" runat="server" CssClass="botonEstilo" 
                     
                        Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                        ValidationGroup="grupoConsulta" Width="80px" 
                        onclick="btnConsultar_Click" />
                    <asp:Button ID="btnDescargar" runat="server" CssClass="botonEstilo" 
                        
                        Text="<%$ Resources:resCorpusCFDIEs, btnDescargar %>" 
                        ValidationGroup="grupoConsulta" Visible="False" Width="80px" 
                        onclick="btnDescargar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" CssClass="botonEstilo" 
                       
                        Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                        ValidationGroup="grupoConsulta" Visible="False" Width="80px" 
                        onclick="btnCancelar_Click" />
                    <asp:Button ID="btnExportar" runat="server" CssClass="botonGrande" 
                       
                        Text="<%$ Resources:resCorpusCFDIEs, lblExcel %>" 
                        ValidationGroup="grupoConsulta" Visible="False" Height="32px" 
                        Width="155px" onclick="btnExportar_Click" />

                    <table align="right">
				<tr valign="top">
					<td align="left"><p id="pMessage" style="VISIBILITY:hidden;POSITION:relative">
				<b><asp:Label ID="LabelProgress" runat="server" Text ="<%$ Resources:resCorpusCFDIEs, lblProgressBar %>">
                </asp:Label>               
                </b>
			</p></td>
				<td colspan="2">
				<div style="BORDER-RIGHT:black 1px solid; PADDING-RIGHT:2px; BORDER-TOP:black 1px solid; PADDING-LEFT:2px; FONT-SIZE:12pt; VISIBILITY:hidden; PADDING-BOTTOM:2px; BORDER-LEFT:black 1px solid; WIDTH:112px; PADDING-TOP:2px; BORDER-BOTTOM:black 1px solid; POSITION:relative"
				id="divProgressBar">
				<span id="progress1">&nbsp;&nbsp;</span> <span id="progress2">&nbsp;&nbsp;</span>
				<span id="progress3">&nbsp;&nbsp;</span> <span id="progress4">&nbsp;&nbsp;</span>
				<span id="progress5">&nbsp;&nbsp;</span> <span id="progress6">&nbsp;&nbsp;</span>
				<span id="progress7">&nbsp;&nbsp;</span> <span id="progress8">&nbsp;&nbsp;</span>
				<span id="progress9">&nbsp;&nbsp;</span>
			</div>
				</td>
				</tr>
			</table>
                </p>
                      <asp:Panel ID="pnlCreditos" runat="server" Height="161px" Width="579px" 
                CssClass="modal" BorderStyle="Solid" BorderWidth="2px" BackColor="White">

                <table align="center">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblDetalle" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblMotivoCancelacion %>" 
                                Font-Bold="True"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="txtcomentario" runat="server" Height="51px" 
                                TextMode="MultiLine" Width="497px"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="rfvcomentario" runat="server" 
                                ControlToValidate="txtcomentario" CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, varComentarioCancelacion %>" 
                                ValidationGroup="creditos"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnAcepCreditos" runat="server"  
                                Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" CssClass="botonEstilo" 
                              ValidationGroup="creditos" onclick="btnAcepCreditos_Click" />
                            <asp:Button ID="btnCancelarCreditos" runat="server" CssClass="botonEstilo" 
                               
                                Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" />
                        </td>
                    </tr>
                </table>
                </asp:Panel>

                <cc1:modalpopupextender id="modalCreditos" 
                runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlCreditos" popupdraghandlecontrolid=""
                    targetcontrolid="lnkCreditos" CancelControlID="btnCancelarCreditos">
                </cc1:modalpopupextender>

                 <asp:LinkButton ID="lnkCreditos" runat="server"></asp:LinkButton>
                      
                </ContentTemplate>
                <Triggers>
                    <%--<asp:PostBackTrigger ControlID="btnDescargar" />--%>
                    <asp:PostBackTrigger ControlID="btnCancelar" />
                    <asp:PostBackTrigger ControlID="btnExportar" />
                </Triggers>
            </asp:UpdatePanel>
        <asp:UpdatePanel ID="udpControles" runat="server">
        <ContentTemplate>  
            <br />
            <table style="width: 929px">
                <tr>
                    <td align="right">
                        <table align="right">
                            <tr>
                                <td>
                                    &nbsp;</td>
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
            <br />
            <asp:Panel runat="server" CssClass="sinBorde" ID="Panel234">
                <asp:GridView ID="gdvComprobantes" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" GridLines="Horizontal" 
                     Width="100%"
                    DataKeyNames="id_cfd" 
                   BackColor="White" 
                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                    style="margin-left: 0px" 
                    onselectedindexchanged="gdvComprobantes_SelectedIndexChanged"  >
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varPDF %>" HeaderStyle-HorizontalAlign="Left" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="hpPDF" runat="server" Target="_blank"
                                    NavigateUrl='<%# Eval("id_cfd", "~/Consultas/webConsultasGeneradorNominaPDF.aspx?nic={0}") + "&doc=" + HttpUtility.UrlEncode(Eval("documento", "{0}"))%>'> <asp:Image ID="imgPDF" runat="server" ImageUrl="~/Imagenes/logo_pdf.png" Width="30" /></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varXML %>" HeaderStyle-HorizontalAlign="Left" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="hpXML" runat="server" Target="_blank"
                                    NavigateUrl='<%# Eval("id_cfd", "~/Consultas/webConsultasGeneradorXML.aspx?nic={0}") %>'> <asp:Image ID="imgXML" runat="server" ImageUrl="~/Imagenes/xml_mediano.png" Width="30" /></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/logo_email.png" 
                            ShowSelectButton="True" HeaderText="<%$ Resources:resCorpusCFDIEs, varEmail %>" />
                    
                        <asp:BoundField DataField="UUID"
                        HeaderText="<%$ Resources:resCorpusCFDIEs, varUUID %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />                       
                        </asp:BoundField>

                        <asp:BoundField DataField="Empleado" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblEmpleado %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="35px" />
                        <ItemStyle HorizontalAlign="Right" Width="35px" />
                        </asp:BoundField>

                       
                        <asp:BoundField DataField="Sucursal" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblSucursal %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Departamento" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblDepartamento %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Periodo" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblPeriodoBusqueda %>" 
                            HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Importe" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblImporte %>" HeaderStyle-HorizontalAlign="Right"
                            DataFormatString="{0:c}" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Fecha" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblFecha %>" HeaderStyle-HorizontalAlign="Left"
                            DataFormatString="{0:d}" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="70px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Estatus" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus %>" 
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign ="Center" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Correo" Visible="false" /> 
                          

                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="hpCancelar" runat="server" CausesValidation="False" 
                                     CommandArgument='<%# Eval("id_cfd") %>' Visible='<%# Convert.ToString(Eval("estatus")) == "A" %>' onclick="hpCancelar_Click"
                                    Text="<%$ Resources:resCorpusCFDIEs, lblCancelar %>"  ></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCancelar %>" />
                            </HeaderTemplate>
                            <ItemStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                         <HeaderTemplate>
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionar %>" HeaderStyle-HorizontalAlign="Left" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSeleccion" runat="server" 
                                    Visible='<%# Convert.ToString(Eval("estatus")) == "A" %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                   <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblid_cfd" runat="server" Text='<%# Bind("id_cfd") %>'></asp:Label>
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
                <table align="right" style="width: 100%">
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td align="right">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAnterior" runat="server" CssClass="botonEstilo" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lbAnterior %>" 
                                            Visible="False" onclick="btnAnterior_Click" /></td>
                                    <td align="right">
                                        <asp:Button ID="btnSiguiente" runat="server" CssClass="botonEstilo" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblSiguiente %>" 
                                           Visible="False" onclick="btnSiguiente_Click" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCantidadPaginas" Width="50px" runat="server" 
                                            Visible="false" AutoPostBack="True" 
                                            onselectedindexchanged="ddlCantidadPaginas_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <br />
            <asp:Panel ID="pnlRevisaCreditos" runat="server" Height="130px" Width="579px" 
                CssClass="modal" BorderStyle="Solid" BorderWidth="1px">

                <table align="center">
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label121" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, varSinCancelacion %>" 
                                Visible="False"></asp:Label>
                            <asp:Label ID="Label7" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblMsgCreditos %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <img alt="" src="../Imagenes/Informacion.png" width="44" />
                            <asp:Label ID="lblCosCre" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblMsgCredInsuf %>"></asp:Label>
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

            <cc1:modalpopupextender id="modalRevisaCreditos" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlRevisaCreditos" popupdraghandlecontrolid=""
                targetcontrolid="lnkRevisaCreditos">
            </cc1:modalpopupextender>

            <asp:LinkButton ID="lnkRevisaCreditos" runat="server"></asp:LinkButton>

            <br />
            <br />

            <br />

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

            <asp:Panel ID="pnlEnvioCorreo" DefaultButton="lbEnvioCorreo" runat="server" Height="330px" Width="730px" 
                CssClass="modal" BorderStyle="Solid" BorderWidth="1px">

                <table>
                    <tr>
                        <td align="left">
                            <table>
                                <tr>
                                    <td colspan="2">
                                    
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td rowspan="2" width="580px">
                                                    <asp:Label ID="Label116" runat="server" 
                                                        Text="<%$ Resources:resCorpusCFDIEs, lblTituloEnvioCorreo %>"></asp:Label>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:RadioButtonList ID="rdlArchivo" runat="server" Height="62px" Width="50px">
                                                        <asp:ListItem>1.-</asp:ListItem>
                                                        <asp:ListItem>2.-</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgPDF0" runat="server" ImageUrl="~/Imagenes/logo_pdf.png" 
                                                        Width="30" />
                                                    +<asp:Image ID="imgXML0" runat="server" ImageUrl="~/Imagenes/xml_mediano.png" 
                                                        Width="30" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgZip" runat="server" ImageUrl="~/Imagenes/ZIP.png" 
                                                        Width="30" />
                                                </td>
                                            </tr>
                                        </table>
                                    
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                    
                                        <asp:Label ID="Label117" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreoEmisor %>"></asp:Label>
                                    
                                    </td>
                                    <td>
                                    
                                        <asp:TextBox ID="txtCorreoEmisor" ToolTip="En caso de capturar varios correos separarlos por una coma y sin espacios &quot;,&quot; (ejem@sen.com,ejem2@sen.com, ...)" runat="server" Width="650px"></asp:TextBox>
                                    
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                            ControlToValidate="txtCorreoEmisor" CssClass="failureNotification" 
                                            Display="Dynamic" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                            ValidationGroup="EnvioCorreoValidationGroup"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                            ControlToValidate="txtCorreoEmisor" CssClass="failureNotification" 
                                            Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                            ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" 
                                            ValidationGroup="EnvioCorreoValidationGroup" Width="16px"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    
                                        <asp:Label ID="Label118" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEmpleado %>"></asp:Label>
                                    
                                    </td>
                                    <td>
                                    
                                        <asp:TextBox ID="txtCorreoCliente" runat="server" Width="650px" 
                                            ToolTip="En caso de capturar varios correos separarlos por una coma y sin espacios &quot;,&quot; (ejem@sen.com,ejem2@sen.com, ...)"></asp:TextBox>
                                    
                                    </td>
                                    <td>
                                        <asp:Image ID="linkModal" runat="server" AlternateText="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaCorreo %>"
                                            CssClass="imagenModal" ImageUrl="~/Imagenes/lupa.png" ToolTip="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaCorreo %>" />
                                      
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCorreoCliente"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"
                                            ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*"
                                            ValidationGroup="EnvioCorreoValidationGroup" Width="16px"><img 
                                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    
                                        <asp:Label ID="Label119" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreoCC %>"></asp:Label>
                                    
                                    </td>
                                    <td>
                                    
                                        <asp:TextBox ID="txtCorreoCC" runat="server" Width="650px" 
                                            ToolTip="En caso de capturar varios correos separarlos por una coma y sin espacios &quot;,&quot; (ejem@sen.com,ejem2@sen.com, ...)"></asp:TextBox>
                                    
                                    </td>
                                    <td>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                            ControlToValidate="txtCorreoCC" CssClass="failureNotification" 
                                            Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                            ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" 
                                            ValidationGroup="EnvioCorreoValidationGroup" Width="16px"><img 
                                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                        
                                     </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                    
                                        <asp:Label ID="Label120" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreoMensaje %>"></asp:Label>
                                    
                                    </td>
                                    <td>
                                    
                                        <asp:TextBox ID="txtCorreoMsj" runat="server" Height="100px" 
                                            TextMode="MultiLine" Width="650px"></asp:TextBox>
                                    
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                    
                                    </td>
                                    <td align="right">
                                    
                                        <asp:Button ID="btnAceptarCor" runat="server" CssClass="botonEstilo" 
                                          
                                            Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" 
                                            ValidationGroup="EnvioCorreoValidationGroup" UseSubmitBehavior="false" 
                                            OnClientClick="clickOnce(this, 'Procesando')" onclick="btnAceptarCor_Click"/>
                                        <asp:Button ID="btnCancelarCor" runat="server" CssClass="botonEstilo" 
                                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" onclick="btnCancelarCor_Click" 
                                            />
                                    
                                    </td>
                                    <td>
                                       </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="center" >
                               <asp:UpdateProgress ID="updProgress" runat="server">
                    <progresstemplate>
                        <img alt="" 
                    src="../Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
                </asp:UpdateProgress>
                          </td>
                    </tr>
                </table>
                </asp:Panel>

            <cc1:modalpopupextender id="mpeEnvioCorreo" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlEnvioCorreo" popupdraghandlecontrolid=""
                targetcontrolid="lbEnvioCorreo">
            </cc1:modalpopupextender>

            <asp:LinkButton ID="lbEnvioCorreo" runat="server"></asp:LinkButton>

            <script type="text/javascript" language="javascript">
                var ModalProgress = '<%= modalGenerando.ClientID %>';         
            </script>
            


        </ContentTemplate>
    </asp:UpdatePanel>
   


   <asp:UpdatePanel ID="upnlBusquedaEmpleados" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlBusquedaEmpleados" runat="server" CssClass="modal">
                <div style="width:680px;">
                    <fieldset style="width:620px;">
                        <legend>
                            <asp:Literal ID="lblTituloBusquedaEmpleado" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaEmpleado %>" />
                        </legend>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRfcEmpleadoBusqueda" AssociatedControlID="txtRfcEmpleadoBusqueda" runat="server" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblNumeroEmpleadoBusqueda" AssociatedControlID="txtNumeroEmpleadoBusqueda" runat="server" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblNumeroEmpleado %>">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtRfcEmpleadoBusqueda" runat="server" CssClass="textEntry" 
                                        MaxLength="15">
                                    </asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumeroEmpleadoBusqueda" runat="server" CssClass="textEntry" 
                                        MaxLength="15">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNombreEmpleadoBusqueda" AssociatedControlID="txtNombreEmpleadoBusqueda" 
                                        runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNombreEmpleado %>">
                                    </asp:Label>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNombreEmpleadoBusqueda" runat="server" CssClass="textEntry" 
                                        MaxLength="20">
                                    </asp:TextBox>
                                </td>
                                <td>
                            
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <p>
                    <asp:Button ID="btnBuscaEmpleados" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                        CssClass="botonEstilo" onclick="btnBuscaEmpleados_Click"  />
                        <asp:Button ID="btnCancelarModal" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                        CssClass="botonEstilo" onclick="btnCancelarModal_Click"   />
                </p>
                <asp:GridView ID="gdvEmpleados" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" GridLines="Horizontal" Width="600px" 
                    DataKeyNames="Id_Empleado,TipoRegimen,Banco,RiesgoPuesto,nombre" AllowPaging="True" 
                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" PageSize="5" OnPageIndexChanging="gdvEmpleados_PageIndexChanging"
                    BackColor="White" OnSelectedIndexChanged="gdvEmpleados_SelectedIndexChanged" 
                    >
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" SelectText="<%$ Resources:resCorpusCFDIEs, lblSeleccionar %>" />
                            <asp:BoundField DataField="NumEmpleado" 
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblNumeroEmpleado %>" 
                                HeaderStyle-HorizontalAlign="Left" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RFC" 
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblRfc %>" ItemStyle-Width="100" 
                                HeaderStyle-HorizontalAlign="Left"  >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="nombre" 
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblNombreEmpleado %>" 
                                HeaderStyle-HorizontalAlign="Left" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:literal ID="Literal1" runat="server" text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
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
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                <br />
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="imgbBuscarEmpleado" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

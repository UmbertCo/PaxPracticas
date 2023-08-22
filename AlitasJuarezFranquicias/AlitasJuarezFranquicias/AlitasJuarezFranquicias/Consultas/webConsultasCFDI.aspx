<%@ Page Title="Consultas" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="webConsultasCFDI.aspx.cs" Inherits="Consultas_webConsultasCFDI" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <%------------------------JQuery----------------------------%>
    <%--<script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script src="../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery.ui.core.js" type="text/javascript"></script>
<script src="../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
<script src="../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
<script src="../Scripts/progressbar.js" type="text/javascript"></script>
<link href="../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />--%>
    <%------------------------JQuery----------------------------%>
    <%--<link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />--%>
    <link href="../App_Themes/Alitas/tema_dinamico.css" rel="stylesheet" type="text/css" />
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
        
        #sugerencias_rfc
        {
            position: absolute;
            float: left;
            border: 1px solid black;
            background-color: white;
            width: 300px;
        }
        #sugerencias_rfc a
        {
            color: #333333;
            text-decoration: none;
        }
        #sugerencias_rfc a.opSelecionada
        {
            background-color: #CCCCCC;
            color: navy;
            width: 300px;
            display: inline-block;
        }
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
        .style2
        {
            width: 28px;
        }
    </style>
    <script runat="server">   
        public void Page_PreInit()
        {
            if (Session["theme"] == null)
            {
                this.Theme = "Alitas";
            }
            else
            {
                this.Theme = Convert.ToString(Session["theme"]);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
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


    </script>
    <div style="text-align: center;">
        <asp:UpdateProgress ID="uppConsultas" runat="server" AssociatedUpdatePanelID="udpControles">
            <ProgressTemplate>
                <img alt="" src="../Imagenes/imgAjaxLoader.gif" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <center>
            <table style="width:952px; text-align:left; background-color:Black">
                <tr>
                    <td class="Titulo">
            <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloConsultas %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:952px; height:0.5px; background-color:#fff000"></td>
                </tr>
                <tr>
                    <td class="Subtitulos">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" />
                    </td>
                </tr>
            </table>
        </center>
    <center>
        <table class="background_tablas_transparente" style="width:952px">
            <tr>
                <td>
    <div class="accountInfo textos" style="width: 900px; height: 240px;">
        <fieldset class="register" style="height: 200px; width: 890px; border-color:transparent;">
            <table style="width: 860px">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblNombreSucursal" SkinId="labelLarge" runat="server" AssociatedControlID="ddlSucursales"
                            Text="<%$ Resources:resCorpusCFDIEs, lblSucursal %>"></asp:Label>
                        <asp:DropDownList ID="ddlSucursales" runat="server" DataTextField="nombre" DataValueField="id_estructura"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlSucursales_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblNumTicket" runat="server" SkinId="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblNumTicket %>"
                            AssociatedControlID="txtNumTicket"></asp:Label>
                        <asp:TextBox ID="txtNumticket" runat="server" MaxLength="128"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblUUID" runat="server" SkinId="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, varUUID %>"
                            AssociatedControlID="txtUUID"></asp:Label>
                        <asp:TextBox ID="txtUUID" runat="server" MaxLength="128"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblRfc" runat="server" SkinId="labelLarge" Text="RFC" AssociatedControlID="ddlrfcRecptor"></asp:Label>
                        <asp:DropDownList ID="ddlrfcRecptor" runat="server" AutoPostBack="false" DataTextField="rfc_receptor"
                            DataValueField="rfc_receptor">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFechaIni" runat="server" SkinId="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>"
                                        AssociatedControlID="txtFechaIni"></asp:Label>
                                    <asp:TextBox ID="txtFechaIni" runat="server" BackColor="White" Width="100px"></asp:TextBox>
                                    <asp:Image ID="imgIni" runat="server" ImageUrl="~/Imagenes/icono_calendario.gif" />
                                    <asp:RegularExpressionValidator ID="revFechaIni" runat="server" Display="Dynamic"
                                        ControlToValidate="txtFechaIni" CssClass="failureNotification" ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$"
                                        ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>">
                                        <img src="../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                    <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFechaIni" Format="dd/MM/yyyy" PopupButtonID="imgIni">
                                    </cc1:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvFechaIni" runat="server" ControlToValidate="txtFechaIni"
                                        CssClass="failureNotification" ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>"
                                        Width="16px">*</asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblFechaFin" runat="server" SkinId="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>"
                                        AssociatedControlID="txtFechaFin"></asp:Label>
                                    <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White" Width="100px"></asp:TextBox>
                                    <asp:Image ID="imgFin" runat="server" ImageUrl="~/Imagenes/icono_calendario.gif" />
                                    <asp:RegularExpressionValidator ID="revFechaFin" runat="server" Display="Dynamic"
                                        ControlToValidate="txtFechaFin" CssClass="failureNotification" ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$"
                                        ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>">
                                    <img src="../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                                    <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFechaFin" Format="dd/MM/yyyy" PopupButtonID="imgFin">
                                    </cc1:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ControlToValidate="txtFechaFin"
                                        CssClass="failureNotification" ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>"
                                        Width="16px">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:Label ID="lblEstatus" runat="server" SkinId="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblEstatus %>"
                            AssociatedControlID="ddlEstatus"></asp:Label>
                        <asp:DropDownList ID="ddlEstatus" runat="server" DataTextField="estatus" DataValueField="clave">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
            <p style="text-align: right;">
                <asp:Button ID="btnConsultar" runat="server" CssClass="botonEstilo" OnClick="btnConsultar_Click"
                    Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" ValidationGroup="grupoConsulta"
                    Width="80px" />
                <asp:Button ID="btnDescargar" runat="server" CssClass="botonEstilo" OnClick="btnDescargar_Click"
                    Text="<%$ Resources:resCorpusCFDIEs, btnDescargar %>" ValidationGroup="grupoConsulta"
                    Visible="False" Width="80px" />
                <asp:Button ID="btnCancelar" runat="server" CssClass="botonEstilo" OnClick="btnCancelar_Click"
                    Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" ValidationGroup="grupoConsulta"
                    Visible="False" Width="80px" />
                <asp:Button ID="btnExportar" runat="server" CssClass="botonEstiloGrande" OnClick="btnExportar_Click"
                    Text="Exportar" ValidationGroup="grupoConsulta"
                    Visible="False" Width="101px" />
                <table align="right">
                    <tr valign="top">
                        <td align="left">
                            <p id="pMessage" style="visibility: hidden; position: relative">
                                <b>
                                    <asp:Label ID="LabelProgress" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblProgressBar %>">
                                    </asp:Label>
                                </b>
                            </p>
                        </td>
                        <td colspan="2">
                            <div style="border-right: black 1px solid; padding-right: 2px; border-top: black 1px solid;
                                padding-left: 2px; font-size: 12pt; visibility: hidden; padding-bottom: 2px;
                                border-left: black 1px solid; width: 112px; padding-top: 2px; border-bottom: black 1px solid;
                                position: relative" id="divProgressBar">
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
                </td>
            </tr>
        </table>
    </center>
    <%--<table cellpadding="0" cellspacing="0">
        <tr>
            <td width="350px">
            </td>
            <td align="right" width="560px">
            </td>
        </tr>
    </table>--%>
            <cc1:ModalPopupExtender ID="modalCreditos" runat="server" BackgroundCssClass="modalBackground"
                PopupControlID="pnlCreditos" PopupDragHandleControlID="" TargetControlID="lnkCreditos"
                CancelControlID="btnCancelarCreditos">
            </cc1:ModalPopupExtender>
            <asp:LinkButton ID="lnkCreditos" runat="server"></asp:LinkButton>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDescargar" />
            <asp:PostBackTrigger ControlID="btnCancelar" />
            <asp:PostBackTrigger ControlID="btnExportar" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="udpControles" runat="server">
        <ContentTemplate>
            <table style="width: 920px">
                <tr>
                    <td align="right">
                        <table align="right">
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cbSeleccionar" CssClass="labelCaptura" runat="server" AutoPostBack="True"
                                        OnCheckedChanged="cbSeleccionar_CheckedChanged" Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionarTodo %>"
                                        TextAlign="Left" Visible="False" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" CssClass="sinBorde" ID="Panel234">
                <asp:GridView ID="gdvComprobantes" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    GridLines="Horizontal" Width="100%" DataKeyNames="id_cfd,UUID,rfc" OnPageIndexChanging="gdvComprobantes_PageIndexChanging"
                    SkinID="SkinGridView" OnSelectedIndexChanged="gdvComprobantes_SelectedIndexChanged">
                    <%-- onrowdeleting="grvDetalles_RowDeleting"--%>
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varPDF %>"
                                    HeaderStyle-HorizontalAlign="Left" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="hpPDF" runat="server" Target="_blank" NavigateUrl='<%# Eval("id_cfd", "~/Consultas/webConsultaPDF.aspx?idcfd={0}") %>'>
                                    <asp:Image ID="imgPDF" runat="server" ImageUrl="~/Imagenes/logo_pdf.png" Width="30" /></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varXML %>"
                                    HeaderStyle-HorizontalAlign="Left" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="hpXML" runat="server" Target="_blank" NavigateUrl='<%# Eval("id_cfd", "~/Consultas/webConsultasGeneradorXML.aspx?idcfd={0}") %>'>
                                    <asp:Image ID="imgXML" runat="server" ImageUrl="~/Imagenes/xml_mediano.png" Width="30" /></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/logo_email.png" ShowSelectButton="True"
                            HeaderText="<%$ Resources:resCorpusCFDIEs, varEmail %>" />
                        <asp:BoundField DataField="UUID" HeaderText="<%$ Resources:resCorpusCFDIEs, varUUID %>"
                            HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="noTicket" HeaderText="No Ticket" HeaderStyle-HorizontalAlign="Left"
                            Visible="True">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="35px" />
                            <ItemStyle HorizontalAlign="Right" Width="40px" />
                        </asp:BoundField>
                         
                        <asp:BoundField DataField="serie" HeaderText="<%$ Resources:resCorpusCFDIEs, lblSerie %>"
                            HeaderStyle-HorizontalAlign="Left" Visible="False">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="35px" />
                            <ItemStyle HorizontalAlign="Right" Width="35px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="folio" HeaderText="<%$ Resources:resCorpusCFDIEs, lblFolio %>"
                            HeaderStyle-HorizontalAlign="Right" Visible="False">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="razon_social" HeaderText="<%$ Resources:resCorpusCFDIEs, lblReceptor %>"
                            HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="sucursal" HeaderText="<%$ Resources:resCorpusCFDIEs, lblSucursal %>"
                            HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="total" HeaderText="<%$ Resources:resCorpusCFDIEs, lblImporte %>"
                            HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="fecha" HeaderText="<%$ Resources:resCorpusCFDIEs, lblFecha %>"
                            HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:d}">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="estatus" HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus %>"
                            HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="hpCancelar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_cfd") %>'
                                    Visible='<%# Convert.ToString(Eval("estatus")) == "G" ||Convert.ToString(Eval("estatus")) == "A" %>'
                                    Text="<%$ Resources:resCorpusCFDIEs, lblCancelar %>" OnClick="hpCancelar_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCancelar %>" />
                            </HeaderTemplate>
                            <ItemStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSeleccion" runat="server" Visible='<%# (Convert.ToString(Eval("estatus")) == "A" || Convert.ToString(Eval("estatus")) == "G") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblid_cfd" runat="server" Text='<%# Bind("id_cfd") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblUUID" runat="server" Text='<%# Bind("UUID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblrfcReceptor" runat="server" Text='<%# Bind("rfc") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Literal runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                    </EmptyDataTemplate>
                   
                    <%--
                    <FooterStyle BackColor="White" ForeColor="#5A737E" />
                    <HeaderStyle BackColor="#0073aa" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#0073aa" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="White" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#BCBCBC" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#0073aa" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#0073aa" />--%>
                    
                    <%--/*275353*/--%>
                </asp:GridView>
                <table align="right" style="width: 814px">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAnterior" runat="server" CssClass="botonEstilo" Text="<%$ Resources:resCorpusCFDIEs, lbAnterior %>"
                                            OnClick="btnAnterior_Click" Visible="False" />
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnSiguiente" runat="server" CssClass="botonEstilo" Text="<%$ Resources:resCorpusCFDIEs, lblSiguiente %>"
                                            OnClick="btnSiguiente_Click" Visible="False" />
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
            <asp:Panel ID="pnlCreditos" runat="server" BackColor="White" BorderStyle="Solid"
                BorderWidth="2px" CssClass="modal" Height="161px" Width="579px">
                <table align="center">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblDetalle" runat="server" SkinId="labelXX-Large" Text="<%$ Resources:resCorpusCFDIEs, lblMotivoCancelacion %>"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="txtcomentario" runat="server" Height="51px" TextMode="MultiLine"
                                Width="497px"></asp:TextBox>
                            <%--<br />--%>
                            <asp:RequiredFieldValidator ID="rfvcomentario" runat="server" ControlToValidate="txtcomentario"
                                CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, varComentarioCancelacion %>"
                                ValidationGroup="creditos" Width="16px">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnAcepCreditos" runat="server" CssClass="botonEstilo" OnClick="btnAcepCreditos_Click"
                                Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" ValidationGroup="creditos" />
                            <asp:Button ID="btnCancelarCreditos" runat="server" CssClass="botonEstilo" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlRevisaCreditos" runat="server" Height="130px" Width="579px" CssClass="modal"
                BorderStyle="Solid" BorderWidth="1px">
                <table align="center">
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label121" runat="server" SkinId="labelLarge"  Text="<%$ Resources:resCorpusCFDIEs, varSinCancelacion %>"
                                Visible="False"></asp:Label>
                            <asp:Label ID="Label7" runat="server" SkinId="labelLarge"  Text="<%$ Resources:resCorpusCFDIEs, lblMsgCreditos %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <img alt="" src="../Imagenes/info_ico.png" width="44" />
                            <asp:Label ID="lblCosCre" SkinId="labelLarge" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblMsgCredInsuf %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnAceptCred" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>"
                                CssClass="botonEstilo" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="modalRevisaCreditos" runat="server" BackgroundCssClass="modalBackground"
                PopupControlID="pnlRevisaCreditos" PopupDragHandleControlID="" TargetControlID="lnkRevisaCreditos">
            </cc1:ModalPopupExtender>
            <asp:LinkButton ID="lnkRevisaCreditos" runat="server" Font-Names="Arial"></asp:LinkButton>
            <br />
            <br />
            <br />
            <br />
            <asp:Panel ID="pnlGenerando" runat="server" Width="300px" CssClass="modal" BorderStyle="Solid"
                BorderWidth="1px">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label3" SkinId="labelLarge"  runat="server" Text="<%$ Resources:resCorpusCFDIEs, varProcesando %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdateProgress ID="updGenera" runat="server">
                                <ProgressTemplate>
                                    <img alt="" src="../Imagenes/imgAjaxLoader.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="modalGenerando" runat="server" BackgroundCssClass="modalBackground"
                PopupControlID="pnlGenerando" PopupDragHandleControlID="" TargetControlID="pnlGenerando">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlEnvioCorreo" runat="server" Height="330px" Width="751px" CssClass="modal"
                BorderStyle="Solid" BorderWidth="1px">
                <table>
                    <tr>
                        <td align="left">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td rowspan="2" width="580px">
                                                    <asp:Label ID="Label116" SkinId="labelXX-Large" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloEnvioCorreo %>"></asp:Label>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:RadioButtonList ID="rdlArchivo" CssClass="labelCaptura" runat="server" Height="62px"
                                                        Width="50px">
                                                        <asp:ListItem>1.-</asp:ListItem>
                                                        <asp:ListItem>2.-</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgPDF0" runat="server" ImageUrl="~/Imagenes/logo_pdf.png" Width="30" />
                                                    +<asp:Image ID="imgXML0" runat="server" ImageUrl="~/Imagenes/xml_mediano.png" Width="30" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgZip" runat="server" ImageUrl="~/Imagenes/ZIP.png" Width="30" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label SkinId="labelLarge" ID="Label117" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreoEmisor %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCorreoEmisor" runat="server" Width="650px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtCorreoEmisor"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>"
                                            ValidationGroup="EnvioCorreoValidationGroup"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCorreoEmisor"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"
                                            ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" ValidationGroup="EnvioCorreoValidationGroup"
                                            Width="16px"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label SkinId="labelLarge" ID="Label118" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreoCliente %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCorreoCliente" runat="server" Width="650px" ToolTip="En caso de capturar varios correos separarlos por una coma y sin espacios &quot;,&quot; (ejem@sen.com,ejem2@sen.com, ...)"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCorreoCliente"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"
                                            ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*"
                                            ValidationGroup="EnvioCorreoValidationGroup" Width="16px"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label119" SkinId="labelLarge" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreoCC %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCorreoCC" runat="server" Width="650px" ToolTip="En caso de capturar varios correos separarlos por una coma y sin espacios &quot;,&quot; (ejem@sen.com,ejem2@sen.com, ...)"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtCorreoCC"
                                            CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"
                                            ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*"
                                            ValidationGroup="EnvioCorreoValidationGroup" Width="16px"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="Label120" SkinId="labelLarge" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreoMensaje %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCorreoMsj" runat="server" Height="100px" TextMode="MultiLine"
                                            Width="650px"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnAceptarCor" runat="server" CssClass="botonEstilo" OnClick="btnAceptarCor_Click"
                                            Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" ValidationGroup="EnvioCorreoValidationGroup"
                                            UseSubmitBehavior="false" OnClientClick="clickOnce(this, 'Procesando')" />
                                        <asp:Button ID="btnCancelarCor" runat="server" CssClass="botonEstilo" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>"
                                            OnClick="btnCancelarCor_Click" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="center">
                            <asp:UpdateProgress ID="updProgress" runat="server">
                                <ProgressTemplate>
                                    <img alt="" src="../Imagenes/imgAjaxLoader.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="mpeEnvioCorreo" runat="server" BackgroundCssClass="modalBackground"
                PopupControlID="pnlEnvioCorreo" PopupDragHandleControlID="" TargetControlID="lbEnvioCorreo">
            </cc1:ModalPopupExtender>
            <asp:LinkButton ID="lbEnvioCorreo" runat="server" Font-Names="Arial"></asp:LinkButton>
            <script type="text/javascript" language="javascript">
                var ModalProgress = '<%= modalGenerando.ClientID %>';
            </script>
            <br />
            <%-- Modalpoup Avisos --%>
            <asp:LinkButton ID="lkbAviso" runat="server" Font-Names="Arial"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="mpeAvisos" runat="server" TargetControlID="lkbAviso"
                PopupControlID="pnlAvisos" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlAvisos" runat="server" CssClass="modal">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="TablaBackGround">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <img alt="" class="imgInformacion" src="../Imagenes/info_ico.png" />
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblAviso" runat="server" SkinID="labelLarge"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAviso" runat="server" CssClass="botonEstilo" Text="OK" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <%--ModalPoup ErrorLog--%>
            <asp:LinkButton ID="lkbErrorLog" runat="server" Font-Names="Arial"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="mpeErrorLog" runat="server" TargetControlID="lkbErrorLog"
                PopupControlID="pnlErrorLog" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlErrorLog" runat="server" CssClass="modal">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="TablaBackGround">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <img alt="" class="imgInformacion" src="../Imagenes/info_ico.png" />
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblErrorLog" runat="server" SkinID="labelLarge"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnErrorLog" runat="server" CssClass="botonEstilo" Text="OK" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

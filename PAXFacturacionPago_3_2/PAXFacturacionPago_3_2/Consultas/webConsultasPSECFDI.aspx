<%@ Page Title="Consulta Comprobantes" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webConsultasPSECFDI.aspx.cs" Inherits="Consultas_PSECFDI_webConsultasPSECFDI" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 450px;
        }
        .style4
        {
            width: 200px;
        }
        .modal
        {}
        .style13
        {
            width: 345px;
        }
        .style17
        {
            width: 65px;
            height: 30px;
        }
        .style18
        {
            height: 30px;
        }
        .style20
        {
            height: 13px;
        }
        .style21
        {
            width: 65px;
            height: 32px;
        }
        .style22
        {
            height: 32px;
        }
        .style23
        {
            width: 65px;
            height: 33px;
        }
        .style24
        {
            height: 33px;
        }
    </style>
        <%--<asp:PostBackTrigger ControlID="btnDescargar" />--%>
        <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
        <script src="../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.ui.core.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
        <script src="../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
        <script src="../Scripts/progressbar.js" type="text/javascript"></script>
        <link href="../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    
        <%------------------------JQuery----------------------------%>

        <link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
         <link href="../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
     <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloConsultas %>"></asp:Label>
    </h2>
    <asp:UpdatePanel runat="server">  
    <ContentTemplate>
    <script type="text/javascript">

        function closePopup() {
            $find('mpeShowTotales').hide();
        }

        function openPopup() {
            $find('mpeShowTotales').show();
        }

        function handleChange1(cb) {

            if (cb.checked == true) {
                var chkStatus1 = document.getElementById("MainContent_chbNumeroTotAcumEmisores");
                var chkStatus2 = document.getElementById("MainContent_chbNumTotSolicitEmi");
                chkStatus1.checked = false;
                chkStatus2.checked = false;

            } else {
                
               
            }
        }

        function handleChange2(cb) {

            if (cb.checked == true) {
                var chkStatus1 = document.getElementById("MainContent_chbNumeroTotalAcum");
                var chkStatus2 = document.getElementById("MainContent_chbNumTotSolicitEmi");
                chkStatus1.checked = false;
                chkStatus2.checked = false;

            } else {
                

            }
        }

        function handleChange3(cb) {

            if (cb.checked == true) {
                var chkStatus1 = document.getElementById("MainContent_chbNumeroTotalAcum");
                var chkStatus2 = document.getElementById("MainContent_chbNumeroTotAcumEmisores");
                chkStatus1.checked = false;
                chkStatus2.checked = false;

            } else {
               

            }
        }

        function checkDate(sender, args) {

            //var fechae = document.getElementById('MainContent_fechaProd').value;
            var d = new Date('<%=ConfigurationManager.AppSettings["FechaProduccion"].ToString() %>');

            if (sender._selectedDate < d) {
                alert("No se puede seleccionar una fecha anterior al inicio de operaciones");
                sender._selectedDate = new Date();
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }

    </script>
        <asp:Panel ID="pPSECFDI" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="style2" style="width:100%; text-align:center;">
                    <fieldset class="register" style="width:95%; height:300px; text-align:left; ">
                        <legend>
                         <asp:Literal ID="Literal2" runat="server" text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" />
                        </legend>
                            <table width="100%" style="height: 45px; align:top">
                            <tr>
                            <td class="style20">
                            <asp:Label ID="lblFechaIniL" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>"></asp:Label>
                            </td>
                            <td class="style20">
                            <asp:Label ID="lblFechaFinL" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>"></asp:Label>
                            </td>
                            </tr>
                                <tr>
                                    <td class="style4">
                                        <asp:TextBox ID="txtFechaIni" runat="server" BackColor="White" Width="100px" 
                                            TabIndex="2" Text="14/07/2015"></asp:TextBox>
                                        <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" 
                                            Enabled="True" TargetControlID="txtFechaIni" Format="dd/MM/yyyy" 
                                            PopupButtonID="imgIni" OnClientDateSelectionChanged="checkDate">
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
                                        <p></p>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White" Width="100px" 
                                            TabIndex="3" Text=""></asp:TextBox>
                                        <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" 
                                            Enabled="True" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" 
                                            PopupButtonID="imgFin" OnClientDateSelectionChanged="checkDate">
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
                                        <p></p>
                                    </td>
                                </tr> 
                            </table>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                             <tr>
                                    <td class="style21" align="center">
                                    <p></p>
                                         <p>
                                        </p>
                                         <input id="chbNumeroTotalAcum" type="checkbox" onchange="handleChange1(this);" runat="server" />
                                    </td>

                                    <td class="style22">
                                    <p></p>
                                        <p>
                                        </p>
                                    <asp:Label ID="lblNumTot" runat="server" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblPSECFDITotalCFDI %>"></asp:Label>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="style23" align="center">
                                        <input id="chbNumeroTotAcumEmisores" type="checkbox" onchange="handleChange2(this);" runat="server" />
                                    </td>
                                    <td class="style24">
                                        <asp:Label ID="lblNumTotAcumEmi" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblPSECFDITotalEmisores %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                        <td class="style17" align="center">
                                            <input id="chbNumTotSolicitEmi" type="checkbox" onchange="handleChange3(this);" runat="server" />
                                        </td>
                                        <td class="style18">
                                            <asp:Label ID="lblNumTotSolicitEmi" runat="server" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblPSECFDITotalReceptores %>"></asp:Label>
                                        </td>
                                </tr>
                            </table> 
                     </fieldset>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hidForModel" runat="server" />
        </asp:Panel>


        <asp:Panel ID="pnlTotalesShow" runat="server" CssClass="modal" 
            BackColor="#FFFFFF" BorderColor="Black"
                BorderStyle="Solid" Height="120px" Width="571px">
             <div class="TotalesPopup">
             <p></p>
                <div class="PopupBody">
                    <table border="1" cellpadding="0" cellspacing="0" width="80%" align="center">
                        <tr bgcolor="#CCCCCC">
                            <td class="style13">
                                <asp:Label ID="TotalesDe" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="AcumuladoDe" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblPSECFDITotalAcumulados %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style13">
                                <asp:Label ID="Totales" runat="server" Text=""></asp:Label>

                            </td>
                            <td>
                                <asp:Label ID="Acumulado" runat="server" Text="0"></asp:Label>

                            </td>
                        </tr>
                    </table>
                    <p align="right">
                        <asp:Button ID="btnOkay" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" 
                                CssClass="botonEstilo"/>

                        <asp:Button ID="btnDescargarTXT" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDescargar %>"
                            onclick="btnDescargarTXT_Click" Visible="False" 
                            OnClientClick="closePopup();" CssClass="botonEstilo"/>
                    </p>
                </div>
                <div class="Controls">
                    </div>
             </div>
         </asp:Panel>


            <cc1:modalpopupextender id="mpeShowTotales" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlTotalesShow" popupdraghandlecontrolid=""
                targetcontrolid="hidForModel" OkControlID="btnOkay">
            </cc1:modalpopupextender>

     </ContentTemplate>
     </asp:UpdatePanel>
    <p align="right">
             <asp:Button ID="Button1" runat="server" CssClass="botonEstilo" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                        ValidationGroup="grupoConsulta" 
                        TabIndex="4" onclick="btnDescargar_Click"/>
            </p>
</asp:Content>
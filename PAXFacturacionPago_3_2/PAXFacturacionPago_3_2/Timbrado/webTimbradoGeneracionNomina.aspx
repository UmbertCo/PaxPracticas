<%@ Page  Title="Timbrado" Language="C#" MasterPageFile="~/webOperatorMaster.master" CodeFile="webTimbradoGeneracionNomina.aspx.cs"  Inherits="Timbrado_webTimbradoGeneracionNomina"  AutoEventWireup="true" ValidateRequest="false"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <script type="text/javascript" language="javascript">

        function fnCalcularDiasPagados() 
        {
            var sFechaInicial = document.getElementById('<%=txtFechaInicialPago.ClientID %>').value;
            var sFechaFinal = document.getElementById('<%=txtFechaFinalPago.ClientID %>').value;
            var gdvPagosNomina = document.getElementById('<%=gdvPagosNomina.ClientID %>');

            if (!sFechaInicial || (sFechaInicial != $.trim(sFechaInicial)))
            {
                return;
            }

            if (!sFechaFinal || (sFechaFinal != $.trim(sFechaFinal)))
            {
                return;
            }

            if (!gdvPagosNomina) 
            {
                return;
            }

            var cFechaInicio = sFechaInicial.split("/");
            var cFechaFinal = sFechaFinal.split("/");

            var dFechaInicial = new Date(Date.parse(cFechaInicio[2] + "/" + cFechaInicio[1] + "/" + cFechaInicio[0]));
            var dFechaFinal = new Date(Date.parse(cFechaFinal[2] + "/" + cFechaFinal[1] + "/" + cFechaFinal[0]));

            if(dFechaInicial > dFechaFinal)
            {
                return;
            }

            var nDias = Math.floor((dFechaFinal - dFechaInicial) / (1000 * 60 * 60 * 24)) + 1;

            for (var i = 1; i < gdvPagosNomina.rows.length; i++) 
            {
                var inputs = gdvPagosNomina.rows[i].getElementsByTagName("input");
                for (var j = 0; j < inputs.length; j++) 
                {
                    if (inputs[j].type == "text") 
                    {
                        inputs[j].value = nDias;
                    }
                }
            }
        }
 
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

        function clickOnce(btn, msg) {
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
        .fontText
        {
            font: 8pt verdana;
        }
        .style7
        {
            width: 1223px;
        }

        .style8
        {
            height: 21px;
        }

        </style>


    <h2 >
         <asp:Label ID="lblCFDI" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCFDIPantallaNomina %>"></asp:Label>
    </h2>
    


    <asp:UpdatePanel ID="updGuardar" runat="server">
        <ContentTemplate>        
              <table cellpadding="0" cellspacing="0">
                <tr>
                    <td width="300px">
                    </td>
                    <td align="right" width="560px">
                       <asp:Label ID="lblCredito" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblCreditos %>" Font-Bold="True"></asp:Label>
                        <asp:Label ID="lblCredValor" runat="server" ></asp:Label>
                    </td>
                </tr>
            </table>              
              
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updMail" runat="server">
        <ContentTemplate>        
             <br/>
            <asp:Panel ID="pnlEnvioCorreo" runat="server" Height="390px" Width="730px" CssClass="modal" BorderStyle="Solid" BorderWidth="1px">
                <table>
                    <tr>
                        <td align="center">
                        <table>
                            <tr>
                                <td>
                                  <img alt="" src="../Imagenes/Informacion.png" style="height: 24px; width: 29px" />
                                </td>
                                <td>
                                     <asp:Label ID="Label46" runat="server" Text="<%$ Resources:resCorpusCFDIEs, msgCompGenerado %>"></asp:Label>                                 
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Label ID="lblRetSat" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <hr style="width: 700px;"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table>
                                <tr>
                                    <td colspan="2">
                                    
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="580px" rowspan="2">
                                                    <asp:Label ID="Label116" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloEnvioCorreo %>"></asp:Label>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:RadioButtonList ID="rdlArchivo" runat="server" Height="62px" Width="50px">
                                                        <asp:ListItem Selected="True">1.-</asp:ListItem>
                                                        <asp:ListItem>2.-</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgPDF" runat="server" ImageUrl="~/Imagenes/logo_pdf.png" Width="30" />
                                                    +<asp:Image ID="imgXML" runat="server" ImageUrl="~/Imagenes/xml_mediano.png" Width="30" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgZip" runat="server" ImageUrl="~/Imagenes/ZIP.png" Width="30" />
                                                </td>
                                            </tr>
                                        </table>                                    
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>                                   
                                        <asp:Label ID="Label117" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblCorreoEmisor %>" Visible="False"></asp:Label>                                    
                                    </td>
                                    <td>                                    
                                        <asp:TextBox ID="txtCorreoEmisor" runat="server" Width="650px" Visible="False"></asp:TextBox>                                    
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                            ControlToValidate="txtCorreoEmisor" CssClass="failureNotification" Display="Dynamic" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                            ValidationGroup="EnvioCorreoValidationGroup" Visible="False"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                            ControlToValidate="txtCorreoEmisor" CssClass="failureNotification" 
                                            Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                            ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" 
                                            ValidationGroup="EnvioCorreoValidationGroup" Width="16px" Visible="False"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>                                    
                                        <asp:Label ID="Label118" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblCorreoCliente %>" Visible="False"></asp:Label>                                    
                                    </td>
                                    <td>                                    
                                        <asp:TextBox ID="txtCorreoCliente" runat="server" Width="650px" 
                                            
                                            ToolTip="En caso de capturar varios correos separarlos por una coma &quot;,&quot; (ejem@sen.com, ejem2@sen.com, ...)" 
                                            Visible="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                            ControlToValidate="txtCorreoCliente" CssClass="failureNotification" 
                                            Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                            ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" 
                                            ValidationGroup="EnvioCorreoValidationGroup" Width="16px" Visible="False"><img 
                                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                     </td>
                                </tr>
                                <tr>
                                    <td>                                    
                                        <asp:Label ID="Label119" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreoCC %>"></asp:Label>                                    
                                    </td>
                                    <td>                                    
                                        <asp:TextBox ID="txtCorreoCC" runat="server" Width="650px" 
                                            ToolTip="En caso de capturar varios correos separarlos por una coma &quot;,&quot; (ejem@sen.com, ejem2@sen.com, ...)"></asp:TextBox>
                                    
                                    </td>
                                    <td>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                                            ControlToValidate="txtCorreoCC" CssClass="failureNotification" 
                                            Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
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
                                        <asp:TextBox ID="txtCorreoMsj" runat="server" Height="100px" TextMode="MultiLine" Width="650px"></asp:TextBox>
                                    
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>                                    
                                    </td>
                                    <td align="right">                                    
                                        <asp:Button ID="btnAceptarCor" runat="server" CssClass="botonEstilo" 
                                            onclick="btnAceptarCor_Click" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" 
                                            ValidationGroup="EnvioCorreoValidationGroup" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" />
                                        <asp:Button ID="btnCancelarCor" runat="server" CssClass="botonEstilo" 
                                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                                            onclick="btnCancelarCor_Click" OnClientClick="this.disabled=true;" UseSubmitBehavior="false"/>                                    
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                          </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:modalpopupextender id="mpeEnvioCorreo" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlEnvioCorreo" popupdraghandlecontrolid="" targetcontrolid="lbEnvioCorreo">
            </cc1:modalpopupextender>
            <asp:LinkButton ID="lbEnvioCorreo" runat="server"></asp:LinkButton>
            <br />

            <asp:Panel ID="pnlGenerando" runat="server" Width="300px" CssClass="modal" BorderStyle="Solid" BorderWidth="1px">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblDetalle" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varProcesando %>"></asp:Label>
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
            <cc1:modalpopupextender id="modalGenerando" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlGenerando" targetcontrolid="pnlGenerando">
            </cc1:modalpopupextender>
            <script type="text/javascript" language="javascript">
                var ModalProgress = '<%= modalGenerando.ClientID %>';         
            </script>            
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updDetalles" runat="server" >
        <ContentTemplate>
            <asp:Panel ID="pnlBuscar" runat="server" GroupingText="<%$ Resources:resCorpusCFDIEs, lblDetalles %>" Width="930px">
                <table>
                        <tr>
                            <td>                
                                   
                                <asp:Label ID="lblSucursal" runat="server" CssClass="fontText"  Font-Bold="True" Text="<%$ Resources:resCorpusCFDIEs, lblSucursal %>"></asp:Label>          
                            </td>
                            <td>
                                 <asp:Label ID="lblPeriodo" runat="server"  CssClass="fontText"  Font-Bold="True"  Text="<%$ Resources:resCorpusCFDIEs, lblPeriodos %>"></asp:Label>  
                            </td>
                            <td>
                                <asp:Label ID="lblNominasPagadas" runat="server" CssClass="fontText"  
                                    Font-Bold="True" Text="<%$ Resources:resCorpusCFDIEs, lblNominasPagadas %>"></asp:Label>
                            </td>
                            <td> 
                                <asp:Label ID="lblEstatusEmpleado" runat="server" CssClass="fontText"  
                                    Font-Bold="True" Text="<%$ Resources:resCorpusCFDIEs, lblEstatusEmpleado %>"></asp:Label>             
                            </td>
                            <td rowspan="2" valign="bottom">              
                                 <asp:Button ID="btnConsultar" runat="server" CssClass="botonEstilo"                          
                                    Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" ValidationGroup="ConsultaValidationGroup"
                                    Width="80px" OnClick="btnConsultar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>                
                                <asp:DropDownList ID="ddlSucursal" runat="server" CssClass="fontText" 
                                    Width="180px" DataTextField="nombre" DataValueField="id_estructura" 
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlSucursal_SelectedIndexChanged">
                                </asp:DropDownList>    
                                <asp:RequiredFieldValidator ID="rfvEstructura" runat="server" 
                                    ControlToValidate="ddlSucursal" CssClass="failureNotification" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                    ValidationGroup="ConsultaValidationGroup" Width="16px"> 
                                    <img alt="" src="../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>           
                            </td>
                            <td>

                                <asp:DropDownList ID="ddlPeriodos" runat="server" CssClass="fontText"  
                                    Width="180px" DataTextField="Descripcion" 
                                    DataValueField="IdTipoPeriodo" 
                                    OnSelectedIndexChanged="ddlPeriodos_SelectedIndexChanged" 
                                    AutoPostBack="True">
                                </asp:DropDownList> 
                                <asp:RequiredFieldValidator ID="rfvPeriodos" runat="server" 
                                    ControlToValidate="ddlPeriodos" CssClass="failureNotification" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valPeriodo %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valPeriodo %>" 
                                    ValidationGroup="ConsultaValidationGroup" Width="16px"> 
                                    <img alt="" src="../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlNominasPagadas" runat="server" CssClass="fontText" 
                                    Width="180px" DataTextField="ClaveNomina" DataValueField="ClaveNomina" 
                                    OnDataBound="ddlNominasPagadas_DataBound">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="fontText" 
                                    Width="180px" OnSelectedIndexChanged="ddlEstatus_SelectedIndexChanged">
                                    <asp:ListItem Value="A">Activo</asp:ListItem>
                                    <asp:ListItem Value="I">Inactivo</asp:ListItem>
                                </asp:DropDownList>  
                                <asp:RequiredFieldValidator ID="rfvEstaus" runat="server" 
                                    ControlToValidate="ddlEstatus" CssClass="failureNotification" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valEstatus %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valEstatus %>" 
                                    ValidationGroup="ConsultaValidationGroup" Width="16px"> 
                                    <img alt="" src="../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="pnlDatosNomina" runat="server" GroupingText="<%$ Resources:resCorpusCFDIEs, lblDatosNomina %>">
                <table width="800px">
                    <tr>
                        <td style="width: 140px;">
                            <asp:Label ID="lblFechaPago" runat="server" CssClass="fontText" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblFechpago %>">
                            </asp:Label>
                        </td>
                        <td style="width: 140px;">
                            <asp:Label ID="lblFechaInicialPago" runat="server" CssClass="fontText" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblFechaInicialPago %>">
                            </asp:Label>
                        </td>
                        <td style="width: 140px;">
                            <asp:Label ID="lblFechaFinalPago" runat="server" CssClass="fontText" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblFechaFinalPago %>">
                            </asp:Label>
                        </td>
                        <td style="width: 140px;">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtFechaPago" runat="server" BackColor="White"
                                TabIndex="15" Width="80px">
                            </asp:TextBox>
                            <asp:Image ID="imgFechaPago" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" />
                            <cc1:CalendarExtender ID="txtFechaPagoCalendarExtender" runat="server" 
                                Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgFechaPago" 
                                TargetControlID="txtFechaPago">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvFechaPago" runat="server" 
                                ControlToValidate="txtFechaPago" CssClass="failureNotification" 
                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFechaPago %>" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaPago %>" 
                                ValidationGroup="NominaValidationGroup" Width="16px"> 
                                <img alt="" src="../Imagenes/error_sign.gif" />
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revFechaPago" runat="server" 
                                ControlToValidate="txtFechaPago" CssClass="failureNotification" 
                                Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                ValidationGroup="NominaValidationGroup">
                                <img alt="" src="../Imagenes/error_sign.gif" />
                            </asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaInicialPago" runat="server" BackColor="White" 
                                TabIndex="16" Width="80px">
                            </asp:TextBox>
                            <cc1:CalendarExtender ID="FechaInicialPago_CalendarExtender" runat="server" 
                                Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgFechaInicialPago" 
                                TargetControlID="txtFechaInicialPago" OnClientDateSelectionChanged="fnCalcularDiasPagados">
                            </cc1:CalendarExtender>
                            <asp:Image ID="imgFechaInicialPago" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" />
                            <asp:RequiredFieldValidator ID="rfvFechaInicialPago" runat="server" 
                                ControlToValidate="txtFechaInicialPago" CssClass="failureNotification" 
                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFechaInicialPago %>" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaInicialPago %>" 
                                ValidationGroup="NominaValidationGroup" Width="16px"> 
                                <img alt="" src="../Imagenes/error_sign.gif" />
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revFechaInicialPago" runat="server" 
                                ControlToValidate="txtFechaInicialPago" CssClass="failureNotification" 
                                Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                ValidationGroup="NominaValidationGroup">
                                <img alt="" src="../Imagenes/error_sign.gif" />
                            </asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaFinalPago" runat="server" BackColor="White" 
                                TabIndex="17" Width="80px">
                            </asp:TextBox>
                            <cc1:CalendarExtender ID="FechaFinalPago_CalendarExtender" runat="server" 
                                Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgFechaFinalPago" 
                                TargetControlID="txtFechaFinalPago" OnClientDateSelectionChanged="fnCalcularDiasPagados">
                            </cc1:CalendarExtender>
                            <asp:Image ID="imgFechaFinalPago" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" />
                            <asp:RequiredFieldValidator ID="rfvFechaFinalPago" runat="server" 
                                ControlToValidate="txtFechaFinalPago" CssClass="failureNotification" 
                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFechaFinalPago %>" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaFinalPago %>" 
                                ValidationGroup="NominaValidationGroup" Width="16px"> 
                                <img alt="" src="../Imagenes/error_sign.gif" />
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revFechaFinalPago" runat="server" 
                                ControlToValidate="txtFechaFinalPago" CssClass="failureNotification" 
                                Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                ValidationGroup="NominaValidationGroup">
                                <img alt="" src="../Imagenes/error_sign.gif" />
                            </asp:RegularExpressionValidator>
                            <asp:CompareValidator ID="cvFechaFinalPago" runat="server" ControlToValidate="txtFechaFinalPago"
                                ControlToCompare="txtFechaInicialPago" Operator="GreaterThanEqual"
                                Display="Dynamic" CssClass="failureNotification" Type="Date"
                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFechaFinalMayor %>" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaFinalMayor %>" 
                                ValidationGroup="NominaValidationGroup">
                                <img alt="" src="../Imagenes/error_sign.gif" />
                            </asp:CompareValidator>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
            
            <table style="width: 929px">
                <tr>
                    <td align="right">
                        <table align="right">
                            <tr>
                                <td class="style8">
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cbSeleccionar" runat="server" AutoPostBack="True"                                        
                                        Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionarTodo %>" 
                                        TextAlign="Left" oncheckedchanged="cbSeleccionar_CheckedChanged" 
                                        Visible="False" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Panel runat="server" CssClass="sinBorde" ID="pnlGenerar">
                <asp:GridView ID="gdvPagosNomina" runat="server" AutoGenerateColumns="False" 
                    CellPadding="2" 
                    Width="100%" 
                    BackColor="White" 
                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                    DataKeyNames="id_estructura,Id_Nomina,Id_PagoNomina,Id_Empleado,Periodo,Correo" 
                    OnRowDataBound="gdvPagosNomina_RowDataBound" ShowFooter="True" OnRowDeleting="gdvPagosNomina_RowDeleting"                   
                    >
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:LinkButton ID="hpEditar" runat="server" CausesValidation="False" 
                                    CommandArgument='<%# Eval("Id_PagoNomina") %>'
                                    Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>"
                                    OnClick="hpEditar_Click" Visible='<%# Convert.ToString(Eval("Opcion")) == "lblEditar" %>'>
                                </asp:LinkButton>
                                <asp:LinkButton ID="hpVer" runat="server" CausesValidation="False" 
                                    CommandArgument='<%# Eval("Id_PagoNomina") %>'
                                    Text="<%$ Resources:resCorpusCFDIEs, lblVer %>" 
                                    OnClick="hpVer_Click" Visible='<%# Convert.ToString(Eval("Opcion")) == "lblVer" %>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="40px" />
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/error_sign.gif" 
                            DeleteText="" 
                            ShowDeleteButton="True" ItemStyle-Width="10px">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>
                        <asp:BoundField DataField="NumEmpleado"
                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblNumeroEmpleado %>">
                            <HeaderStyle HorizontalAlign="Center" />                      
                            <ItemStyle HorizontalAlign="Left" Width="50px" />                                   
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaPago" DataFormatString="{0:d}"
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblFechPago %>">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="fecha_documento" DataFormatString="{0:d}"
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblFecha %>">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Nombre" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblNodoNombre %>">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" Width="150px" />  
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Total" >
                            <ItemTemplate >
                                <asp:Label ID="lblTotal" runat="server" Text='<%# String.Format("{0:c6}", Eval("Total"))%>'  />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTotalSum" runat="server" />
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Width="70px" />
                            <ItemStyle HorizontalAlign="Right" Width="70px" />  
                            <FooterStyle HorizontalAlign="Right" Width="70px" Font-Bold="true" />  
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblNumeroDiasPagados %>" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Label ID="lblNumeroDiasPagados" runat="server" Text='<%# Eval("NumDiasPagados") %>'
                                    Visible='<%# Convert.ToString(Eval("Timbrado")) == "1" %>' />
                                <asp:TextBox ID="txtNumeroDiasPagados" runat="server" Width="50px"
                                    Visible='<%# Convert.ToString(Eval("Timbrado")) == "0" %>' />
                                <cc1:FilteredTextBoxExtender ID="ftbeNumeroDiasPagados" runat="server" 
                                    FilterType="Numbers, Custom" TargetControlID="txtNumeroDiasPagados" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="revNumeroDiasPagados" runat="server" 
                                    ControlToValidate="txtNumeroDiasPagados" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="NominaValidationGroup" Width="16px"> 
                                    <img alt="" src="../Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>
                            </ItemTemplate>
                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblSeleccionar %>">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSeleccion" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="5px" />
                            <ItemStyle HorizontalAlign="center" Width="5px" />  
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">  
                            <ItemTemplate>
                                <asp:Literal ID="ltlXML" runat="server" >
                                 </asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">  
                            <ItemTemplate>
                                <asp:Literal ID="ltlIdCfd" runat="server" >
                                 </asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblSeleccion" runat="server" Text="Timbrado"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbTimbrado" runat="server" Enabled="False"
                                    Checked='<%# Convert.ToString(Eval("Timbrado")) == "1" %>' />
                            </ItemTemplate>
                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
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
            <table style="width: 929px">
                <tr>
                    <td class="style7" align="right">
                        </td>
                    <td align="right">
                        <asp:Button ID="btnExcel" runat="server" CssClass="botonGrande" TabIndex="54" Text="<%$ Resources:resCorpusCFDIEs, btnExcel %>" 
                            Height="30px" Width="150px" onclick="btnExcel_Click" OnClientClick="this.disabled=true;" UseSubmitBehavior="false"/>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnCrear" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, btnTimbrar %>" 
                            TabIndex="55" 
                            CssClass="botonEstilo" OnClick="btnCrear_Click" 
                            UseSubmitBehavior="false" ValidationGroup="NominaValidationGroup" />
                            
                            
                    </td>
                    <td>
                        <asp:Button ID="btnGenerar" runat="server" CssClass="botonGrande" TabIndex="54" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnGeneracionNomina %>" 
                        Height="30px" Width="150px" OnClick="btnGenerar_Click" UseSubmitBehavior="false" />
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="4">
                    <asp:ValidationSummary ID="vsCrearFactura" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, varValidacionError %>" 
                             ValidationGroup="NominaValidationGroup" ShowMessageBox="True" ShowSummary="False" />
                    </td>
                </tr>
            </table>
            <br />
            </asp:Panel>

            <br />
            <asp:Panel ID="pnlCreditos" runat="server" Height="142px" Width="579px" CssClass="modal" BorderStyle="Solid" BorderWidth="1px">
                <table align="center">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblHeaderMsg" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblMsgCreditos %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDetailMsg" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varSinGeneracionyTimbradoNomina %>" 
                                Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <img alt="" src="../Imagenes/Informacion.png" width="44" />
                            <asp:Label ID="lblCosCre" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblMsgCredInsuf %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnAcepCreditos" runat="server" onclick="btnAcepCreditos_Click" Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" CssClass="botonEstilo" />
                        </td>
                    </tr>
                </table>
                </asp:Panel>
            <cc1:modalpopupextender id="modalCreditos" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlCreditos" popupdraghandlecontrolid="" targetcontrolid="lnkCreditos">
            </cc1:modalpopupextender>
            <asp:LinkButton ID="lnkCreditos" runat="server"></asp:LinkButton>
            <asp:Panel ID="pnlAvisoCorreo" runat="server" Height="142px" Width="579px" CssClass="modal" BorderStyle="Solid" BorderWidth="1px">
                <table align="center" width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblAvisoEnvioCorreo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, msgEnvioCorreo %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">

                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <img alt="" src="../Imagenes/Informacion.png" width="44" />
                            <asp:Label ID="lblAvisoCorreo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, msgEnvioCorreo %>"></asp:Label>
                            <br />
                            <asp:Label ID="lblAvisoCorreoNoEnviado" runat="server" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnAvisoCorreo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" CssClass="botonEstilo" />
                        </td>
                    </tr>
                </table>
                </asp:Panel>
            <cc1:modalpopupextender id="mpeAvisoCorreo" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlAvisoCorreo" popupdraghandlecontrolid="" targetcontrolid="lbAvisoCorreo">
            </cc1:modalpopupextender>
            <asp:LinkButton ID="lbAvisoCorreo" runat="server"></asp:LinkButton>
            <asp:Panel ID="pnlConfirmacionTimbrar" runat="server" Height="142px" Width="579px" CssClass="modal" BorderStyle="Solid" BorderWidth="1px">
                <table align="center" width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblAvisoConfimacionTimbrar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, msgConfimacionTimbradoNomina %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">

                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <img alt="" src="../Imagenes/Informacion.png" width="44" />
                            <asp:Label ID="lblAvisoConfimacion" runat="server" Text="<%$ Resources:resCorpusCFDIEs, msgConfimacionTimbradoNominaMensaje %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnAceptarConfirmacionTimbrado" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" CssClass="botonEstilo" UseSubmitBehavior="false"
                                OnClick="btnAceptarConfirmacionTimbrado_Click" OnClientClick="clickOnce(this, 'Procesando')" />
                            <asp:Button ID="bntCancelarConfimacionTimbrado" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCancelar %>" CssClass="botonEstilo" />
                        </td>
                    </tr>
                </table>
                </asp:Panel>
            <cc1:modalpopupextender id="mpeConfirmacionTimbrar" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlConfirmacionTimbrar" popupdraghandlecontrolid="" targetcontrolid="lbConfirmacionTimbrar">
            </cc1:modalpopupextender>
            <asp:LinkButton ID="lbConfirmacionTimbrar" runat="server"></asp:LinkButton>
            <asp:Panel ID="pnlGenerandoTimbrado" runat="server" Width="300px" CssClass="modal" BorderStyle="Solid" BorderWidth="1px">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varProcesando %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdateProgress ID="uppTimbrado" runat="server">
                                <progresstemplate>
                                    <img alt="" src="../Imagenes/imgAjaxLoader.gif" />
                                </progresstemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:modalpopupextender id="ModalGenerandoTimbrado" runat="server" backgroundcssclass="modalBackground" 
                popupcontrolid="pnlGenerandoTimbrado" targetcontrolid="pnlGenerandoTimbrado">
            </cc1:modalpopupextender>
            <script type="text/javascript" language="javascript">
                var ModalProgress = '<%= ModalGenerandoTimbrado.ClientID %>';         
            </script> 
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCrear" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

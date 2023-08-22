<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/webOperatorMaster.master" CodeFile="webTimbradoRegistroNomina.aspx.cs" Inherits="Timbrado_webTimbradoRegistroNomina" ValidateRequest="false" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

    <style type="text/css">
        .style2
        {
            height: 19px;
        }
        .fontText
        {
            font: 8pt verdana;    
        }
        .style3
        {
            height: 19px;
            width: 212px;
        }
        .style4
        {
            width: 212px;
        }
        .style6
        {
            height: 28px;
        }
        .modal
        {
            padding: 10px 10px 10px 10px;
            border:1px solid #333333;
            background-color:White;
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

    <h2>
        <asp:Label ID="lblCFDI" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCFDINominaPantalla %>"></asp:Label>
    </h2>
    <asp:UpdatePanel ID="updGuardar" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlSucEmisor" runat="server" GroupingText="<%$ Resources:resCorpusCFDIEs, lblDatosFisc %>" 
                  Width="930px">
                  <table>
                    <tr>
                        <td class="style18">
                            <asp:Label ID="lblNombreSucursal" runat="server" CssClass="fontText" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblSucursal %>" Font-Bold="True"> 
                            </asp:Label>
                        </td>
                        <td>
                         </td>
                        <td>
                            <asp:Label ID="lblDireccionFis" runat="server" CssClass="fontText"> 
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style18">
                            <asp:DropDownList ID="ddlSucursalesFis" runat="server" AutoPostBack="True" 
                                CssClass="fontText" DataTextField="nombre" DataValueField="id_estructura" 
                                TabIndex="1" Width="300px" 
                                OnSelectedIndexChanged="ddlSucursalesFis_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            </td>
                        <td>
                            <asp:Label ID="lblUbicacionFis" runat="server" CssClass="fontText">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style18">
                        
                            </td>
                        <td>
                            </td>
                        <td>
                            <asp:Label ID="lblRFCFis" runat="server" CssClass="fontText">
                            </asp:Label>
                        </td>
                    </tr>
                      <tr>
                          <td colspan="2">
                              <asp:CheckBox ID="cbAgrExpEn" runat="server" AutoPostBack="true" 
                                  CssClass="fontText" TabIndex="2" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblAgrExpEn %>" 
                                  OnCheckedChanged="cbAgrExpEn_CheckedChanged" />
                          </td>
                          <td>
                          </td>
                      </tr>
                </table>                
            </asp:Panel>
            <asp:Panel ID="pnlExtender" runat="server" Width="930px"  Height="20px" CssClass="collapsePanelHeader">
                <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                    <div style="float: left; margin-left: 20px;">
                        <asp:Label ID="lblExpedidoEn" runat="server"> </asp:Label>
                    </div>
                <div style="float: right; vertical-align: middle;">
                    <asp:ImageButton ID="imgExpedidoEnExpander" runat="server" 
                        ImageUrl="~/Imagenes/expand_blue.jpg" AlternateText="(Expedido En...)" 
                        Visible="False"/>
                </div>
              </div>
           </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="cpeExpedidoEn" runat="server" 
                    TargetControlID="pnlExpedidoEn"
                    ExpandControlID="pnlExtender"
                    CollapseControlID="pnlExtender" 
                    TextLabelID="lblExpedidoEn"
                    ImageControlID="imgExpedidoEnExpander"    
                    ExpandedText=""
                    CollapsedText=""
                    ExpandedImage="~/Imagenes/collapse_blue.jpg"
                    CollapsedImage="~/Imagenes/expand_blue.jpg"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanelDemo" Enabled="True">
            </cc1:CollapsiblePanelExtender>
            <asp:Panel ID="pnlExpedidoEn" runat="server" GroupingText="<%$ Resources:resCorpusCFDIEs, lblExpedidoEn %>" 
                Width="930px" BackColor="#F0F0F0">
                   <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSucursalExpedidoEn" runat="server" CssClass="fontText"
                                    AssociatedControlID="ddlSucursales" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblSucursal %>"> 
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:Label ID="lblDomicilioExpedidoEn" runat="server" 
                                    CssClass="fontText"> 
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlSucursales" runat="server" AutoPostBack="True" CssClass="fontText"
                                    DataTextField="nombre" DataValueField="id_estructura" TabIndex="3" 
                                    Width="300px" OnSelectedIndexChanged="ddlSucursales_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvSucursales" runat="server" 
                                    ControlToValidate="ddlSucursales" CssClass="failureNotification" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                    ValidationGroup="NominaValidationGroup" Width="16px"> 
                                    <img alt="" src="../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="lblUbicacionExpedidoEn" runat="server" CssClass="fontText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                </td>
                            <td>
                                </td>
                            <td>
                                <asp:Label ID="lblRFCExpedidoEn" runat="server" CssClass="fontText">
                                </asp:Label>
                            </td>
                        </tr>
                    </table>              
            </asp:Panel>
            <asp:Panel ID="pnlLugExp" runat="server" GroupingText="<%$ Resources:resCorpusCFDIEs, lblLugarExp %>" 
                Width="930px">
                <table>
                <tr>
                    <td>
                        <asp:Label ID="lblPais" runat="server" AssociatedControlID="ddlPaisLugExp" 
                            CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"> 
                        </asp:Label>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblEstado" runat="server" AssociatedControlID="ddlEdoLugExp" 
                            CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"> 
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlPaisLugExp" runat="server" AutoPostBack="True" 
                            CssClass="fontText" DataTextField="pais" DataValueField="id_pais" 
                            TabIndex="4" Width="300px" 
                            OnSelectedIndexChanged="ddlPaisLugExp_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        
                        </td>
                    <td>
                        <asp:DropDownList ID="ddlEdoLugExp" runat="server" CssClass="fontText" 
                            DataTextField="estado" DataValueField="id_estado" TabIndex="5" 
                            Width="300px">
                        </asp:DropDownList>
                    </td>
                </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlDatosGenerales" runat="server" GroupingText="<%$ Resources:resCorpusCFDIEs, lblDatosGen %>" 
                  Width="930px">
                  <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblMetodoPago" runat="server" 
                                    AssociatedControlID="ddlSucursales" 
                                    Text="Método de Pago" CssClass="fontText"></asp:Label>
                            </td>
                            <td>
                                 </td>
                            <td colspan="2">
                                <asp:Label ID="lblNumeroCuenta" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblNumeroCuenta %>" CssClass="fontText">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblMoneda" runat="server" CssClass="fontText" Text="Moneda"> 
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlMetodoPago" runat="server" CssClass="fontText"
                                    TabIndex="6" Width="250px" AutoPostBack="True" 
                                    OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged">
                                    <asp:ListItem>No Aplica</asp:ListItem>
                                    <asp:ListItem>Efectivo</asp:ListItem>
                                    <asp:ListItem>Transferencias Electrónicas de Fondos</asp:ListItem>
                                    <asp:ListItem>Cheques</asp:ListItem>
                                    <asp:ListItem>Tarjetas de débito</asp:ListItem>
                                    <asp:ListItem>Tarjetas de crédito</asp:ListItem>
                                    <asp:ListItem>Tarjetas de servicio</asp:ListItem>
                                    <asp:ListItem>Tarjetas de monedero electrónico</asp:ListItem>
                                    <asp:ListItem>No identificado</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                 </td>
                            <td>
                                <asp:TextBox ID="txtNumeroCuenta" runat="server" TabIndex="7" Width="200px" 
                                    CssClass="fontText"> 
                                </asp:TextBox>
                            </td> 
                             <td>
                                <asp:RegularExpressionValidator ID="regxNumeroCuenta" runat="server"
                                    ControlToValidate ="txtNumeroCuenta" ValidationExpression = ".{4}.*"
                                    CssClass="failureNotification"
                                    ToolTip = "<%$ Resources:resCorpusCFDIEs, valLongNumeroDeCuenta %>"                                
                                    ErrorMessage = "<%$ Resources:resCorpusCFDIEs, valLongNumeroDeCuenta %>"                                
                                    ValidationGroup="NominaValidationGroup" width ="16px"> 
                                    <img alt="" src="../Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMoneda" runat="server" AutoPostBack="True" 
                                    CssClass="fontText" TabIndex="7" Width="200px" 
                                    OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged">
                                    <asp:ListItem Value="MXN">Pesos Mexicanos</asp:ListItem>
                                    <asp:ListItem Value="USD">Dólares</asp:ListItem>
                                    <asp:ListItem Value="XEU">Euros</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblRegimenFiscal" runat="server" CssClass="fontText" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblRegimenfiscal %>"> 
                                </asp:Label>
                            </td>
                            <td>
                                 </td>
                            <td>
                                <asp:Label ID="lblFormaPago" runat="server" AssociatedControlID="ddlFormaPago" 
                                    CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblformadepago %>"> 
                                </asp:Label>
                            </td>
                            <td>
                                 </td>
                            <td>
                                 <asp:Label ID="lblTipoCambio" runat="server" CssClass="fontText" 
                                     Text="<%$ Resources:resCorpusCFDIEs, lblTipoCambio %>">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtRegimenfiscal" runat="server" CssClass="fontText" 
                                    TabIndex="9" Width="250px"> 
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvRegimenFiscal" runat="server" 
                                    ControlToValidate="txtRegimenfiscal" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valRegimenFiscal%>" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valRegimenFiscal%>" 
                                    ValidationGroup="NominaValidationGroup" Width="16px"> 
                                    <img alt="" src="../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFormaPago" runat="server" AutoPostBack="True" 
                                    CssClass="fontText" TabIndex="10" Width="250px">
                                    <asp:ListItem Selected="True">Pago en una sola exhibición.</asp:ListItem>
                                </asp:DropDownList>
                                </td>
                            <td>
                                </td>
                            <td>
                                 <asp:TextBox ID="txtTipoCambio" runat="server" CssClass="fontText" 
                                    TabIndex="11" Width="80px">0</asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTipoCambio" runat="server" 
                                    ControlToValidate="txtTipoCambio" CssClass="failureNotification" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valTipoCambio %>" Height="18px" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valTipoCambio %>" 
                                    ValidationGroup="NominaValidationGroup" Width="16px"> 
                                    <img alt="" src="../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
              </asp:Panel>
            <%--<asp:UpdateProgress ID="udpEmisor" runat="server" AssociatedUpdatePanelID="updGuardar">
                <progresstemplate>
                    <img alt="" 
                src="../Imagenes/imgAjaxLoader.gif" />
                </progresstemplate>
        </asp:UpdateProgress>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlEmpleado" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlFormulario" runat="server"  GroupingText="<%$ Resources:resCorpusCFDIEs, lblDatosEmpleado %>" 
                Width="930px">
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="style2">
                                        <asp:Label ID="lblNombreEmpleado" runat="server" 
                                            AssociatedControlID="txtNombreEmpleado" CssClass="fontText" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblNombreEmpleado %>">

                                        </asp:Label>
                                    </td>
                                    <td class="style3">
                                        </td>
                                    <td class="style2">
                                        
                                        </td>
                                    <td class="style2">
                                        &nbsp;</td>
                                    <td class="style2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtNombreEmpleado" runat="server" CssClass="fontText" 
                                            MaxLength="200" TabIndex="12" Width="395px" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="linkModal" runat="server" TabIndex="13"
                                            AlternateText="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaReceptor %>" 
                                            CssClass="imagenModal" Height="20px" ImageUrl="~/Imagenes/lupa.png" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaReceptor %>" 
                                            Width="20px" />
                                        <cc1:ModalPopupExtender ID="linkModal_ModalPopupExtender" runat="server" 
                                            BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True" 
                                            PopupControlID="pnlBusquedaEmpleados" TargetControlID="linkModal">
                                        </cc1:ModalPopupExtender>
                                        <asp:RequiredFieldValidator ID="rfvNombreEmpleado" runat="server" 
                                            ControlToValidate="txtNombreEmpleado" CssClass="failureNotification" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valRazonSocial %>" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, valRazonSocial %>" 
                                            ValidationGroup="NominaValidationGroup" Width="16px">
                                            <img alt="" src="../Imagenes/error_sign.gif" />    
                                        </asp:RequiredFieldValidator>
                                        </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRfcEmpleado" runat="server" CssClass="fontText"
                                           Text="<%$ Resources:resCorpusCFDIEs, lblRfc %>">
                                        </asp:Label>
                                    </td>
                                    <td class="style4">
                                        <asp:Label ID="lblRfcEmpleadoDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRiesgoPuesto" runat="server" CssClass="fontText" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblRiesgoPuesto %>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRiesgoPuestoDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="style6">
                                        <asp:Label ID="lblCURP" runat="server" CssClass="fontText"
                                            Text="<%$ Resources:resCorpusCFDIEs, lblCURP %>">
                                        </asp:Label>
                                    </td>
                                    <td class="style6">
                                        <asp:Label ID="lblCURPDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                    </td>
                                    <td class="style6">
                                    </td>
                                    <td class="style6">
                                        <asp:Label ID="lblTipoContrato" runat="server" CssClass="fontText" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblTipoContrato %>">
                                        </asp:Label>
                                    </td>
                                    <td class="style6">
                                        <asp:Label ID="lblTipoContratoDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNumeroSeguridadSocial" runat="server" CssClass="fontText"
                                            Text="<%$ Resources:resCorpusCFDIEs, lblNumeroSeguridadSocial %>">
                                        </asp:Label>
                                    </td>
                                    <td class="style4">
                                        <asp:Label ID="lblNumeroSeguridadSocialDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                     </td>
                                    <td>
                                        
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTipoJornada" runat="server" CssClass="fontText" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblTipoJornada %>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTipoJornadaDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRegistroPatronal" runat="server" CssClass="fontText"
                                            Text="<%$ Resources:resCorpusCFDIEs, lblRegistroPatronal %>">
                                        </asp:Label>
                                    </td>
                                    <td class="style4">
                                        <asp:Label ID="lblRegistroPatronalDescripion" runat="server" CssClass="fontText">

                                        </asp:Label>
                                     </td>
                                    <td>

                                    </td>
                                    <td>
                                        <asp:Label ID="lblFechaInicioRelacionLaboral" runat="server" 
                                            CssClass="fontText" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFechaInicioRelacionLaboral %>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFechaInicioRelacionLaboralDescripion" runat="server" 
                                            CssClass="fontText">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNumeroEmpleado" runat="server" CssClass="fontText"
                                           Text="<%$ Resources:resCorpusCFDIEs, lblNumeroEmpleado %>">
                                        </asp:Label>
                                    </td>
                                    <td class="style4">
                                        <asp:Label ID="lblNumeroEmpleadoDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                     </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSalarioBase" runat="server" CssClass="fontText" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblSalarioBase %>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSalarioBaseDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTipoRegimen" runat="server" CssClass="fontText"
                                            Text="<%$ Resources:resCorpusCFDIEs, lblTipoRegimen %>">
                                        </asp:Label>
                                    </td>
                                    <td class="style4">
                                        <asp:Label ID="lblTipoRegimenDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSalarioDiarioIntegrado" runat="server" CssClass="fontText" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblSalarioDiarioIntegrado %>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSalarioDiarioIntegradoDescripion" runat="server" 
                                            CssClass="fontText">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDepartamento" runat="server" CssClass="fontText"
                                            Text="<%$ Resources:resCorpusCFDIEs, lblDepartamento %>">
                                        </asp:Label>
                                    </td>
                                    <td class="style4">
                                        <asp:Label ID="lblDepartamentoDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                     </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCLABE" runat="server" CssClass="fontText" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblCLABE %>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCLABEDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPuesto" runat="server" CssClass="fontText" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblPuesto %>">
                                        </asp:Label>
                                    </td>
                                    <td class="style4">
                                        <asp:Label ID="lblPuestoDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBanco" runat="server" CssClass="fontText" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblBanco %>">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBancoDescripion" runat="server" CssClass="fontText">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAntiguedad" runat="server" 
                                            AssociatedControlID="txtAntiguedad" CssClass="fontText" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblAntiguedad %>">
                                        </asp:Label>
                                    </td>
                                    <td class="style4">
                                        <asp:TextBox ID="txtAntiguedad" runat="server" CssClass="fontText" 
                                            MaxLength="9" TabIndex="14" Width="80px">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlConceptosDeduccionesPercepciones" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConceptosPercepciones" runat="server" Width="930px"
                CssClass="fontText" GroupingText="<%$ Resources:resCorpusCFDIEs, lblConceptos %>">
                <table style="width: 888px">
                    <tr style="height:50px;">
                        <td>
                            <asp:Label ID="lblTipo" runat="server" CssClass="fontText" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblTipo %>"> </asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTipoPercepcion" runat="server" CssClass="fontText" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblConceptoPercepcion %>"> </asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblClavePercepcion" runat="server" CssClass="fontText" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblClavePercepcion %>"> </asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblConceptoPercepcion" runat="server" CssClass="fontText" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblDescripcion %>"> </asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblImporteGravadoPercepcion" runat="server" CssClass="fontText" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblImporteGravado %>"> </asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblImporteExentoPercepcion" runat="server" CssClass="fontText" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblImporteExento %>"> </asp:Label>
                        </td>
                        <td rowspan="2">
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlTipo" runat="server" 
                                DataTextField="Descripcion" DataValueField="IdTipo"
                                CssClass="fontText" TabIndex="19" Width="100px"  
                                AutoPostBack="True" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipoDeduccionPercepcion" runat="server" 
                                DataTextField="Descripcion" DataValueField="Id_TipoPercepDedu"
                                CssClass="fontText" TabIndex="20" Width="200px" 
                                OnSelectedIndexChanged="ddlTipoDeduccionPercepcion_SelectedIndexChanged" 
                                AutoPostBack="True" OnDataBound="ddlTipoDeduccionPercepcion_DataBound">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvTipoDeduccionPercepcion" runat="server" 
                                ControlToValidate="ddlTipoDeduccionPercepcion" CssClass="failureNotification" 
                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valTipoPercepcion%>" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valTipoPercepcion%>" 
                                ValidationGroup="validarPercepcion" Width="16px"
                                InitialValue="0"> <img alt="" src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaveConcepto" runat="server" CssClass="fontText" 
                                TabIndex="21" Width="50px"> </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvClaveConcepto" runat="server" 
                                ControlToValidate="txtClaveConcepto" CssClass="failureNotification" 
                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valClavePercepcion%>" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valClavePercepcion%>" 
                                ValidationGroup="validarPercepcion" Width="16px"> <img alt="" src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtConcepto" runat="server" MaxLength="100" 
                                CssClass="fontText" TabIndex="22" Width="170px"> </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvConceptoP" runat="server" 
                                ControlToValidate="txtConcepto" CssClass="failureNotification" 
                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valConceptoPercepcion%>" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valConceptoPercepcion%>" 
                                ValidationGroup="validarPercepcion" Width="16px"> <img alt="" src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtImporteGravado" runat="server" CssClass="fontText" 
                                TabIndex="23" Width="60px">0</asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvImporteGravado" runat="server" 
                                ControlToValidate="txtImporteGravado" CssClass="failureNotification" 
                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valImporteGravado%>" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valImporteGravado%>" 
                                ValidationGroup="validarPercepcion" Width="16px"> <img alt="" src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="ftbeImporteGravado" runat="server" 
                                FilterType="Numbers, Custom" TargetControlID="txtImporteGravado" ValidChars=".">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtImporteExento" runat="server" CssClass="fontText" 
                                TabIndex="24" Width="60px">0</asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvImporteExento" runat="server" 
                                ControlToValidate="txtImporteExento" CssClass="failureNotification" 
                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valImporteExento%>" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valImporteExento%>" 
                                ValidationGroup="validarPercepcion" Width="16px"> <img alt="" src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            <cc1:FilteredTextBoxExtender ID="ftbImporteExento" runat="server" 
                                FilterType="Numbers, Custom" TargetControlID="txtImporteExento" ValidChars=".">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:HiddenField ID="hfId_Registro" runat="server" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlColapsableIncapacidades" runat="server" Width="890px" Height="23px" CssClass="collapsePanelHeader">
                                <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                                    <div style="float: left; margin-left: 20px;">
                                            <asp:Label ID="lblColapsableIncapacidades" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblIncapacidades %>"> </asp:Label>
                                    </div>
                                    <div style="float: right; vertical-align: middle;">
                                        <asp:ImageButton ID="imgbColapsableIncapacidades" runat="server" 
                                            ImageUrl="~/Imagenes/expand_blue.jpg" AlternateText="(Incapacidades...)" 
                                            Visible="true"/>
                                    </div>
                                </div>
                            </asp:Panel>
                            <cc1:CollapsiblePanelExtender ID="cpeConceptosIncapacidades" runat="server" 
                                TargetControlID="pnlConceptosIncapacidades"
                                ExpandControlID="pnlColapsableIncapacidades"
                                CollapseControlID="pnlColapsableIncapacidades" 
                                TextLabelID="lblColapsableIncapacidades"
                                ImageControlID="imgbColapsableIncapacidades"    
                                ExpandedText=""
                                CollapsedText=""
                                ExpandedImage="~/Imagenes/collapse_blue.jpg"
                                CollapsedImage="~/Imagenes/expand_blue.jpg"
                                SuppressPostBack="true"
                                SkinID="CollapsiblePanelDemo" Enabled="True">
                            </cc1:CollapsiblePanelExtender>
                            <asp:Panel ID="pnlConceptosIncapacidades" Width="890px" runat="server" BackColor="#F0F0F0" CssClass="fontText">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDiasIncapacidad" runat="server" CssClass="fontText" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblDiasIncapacidad %>"> </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTipoIncapacidad" runat="server" CssClass="fontText" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblTipoIncapacidad %>"> </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDescuento" runat="server" CssClass="fontText" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblDescuentoSinPorc %>"> </asp:Label>
                                        </td>
                                        <td rowspan="2">
                                            <asp:Button ID="btnAgregarIncapacidad" runat="server" CssClass="botonChico" 
                                                Height="20px" OnClick="btnAgregarIncapacidad_Click" TabIndex="30"
                                                Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" Width="80px"
                                                ValidationGroup="validarIncapacidad" />
                                            <asp:Button ID="btnCancelarIncapacidad" runat="server" CssClass="botonChico" 
                                                Height="20px" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                                                ValidationGroup="validarPercepcion" Width="80px" TabIndex="31"
                                                onclick="btnCancelarIncapacidad_Click" Visible="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDiasIncapacidad" runat="server" CssClass="fontText" 
                                                TabIndex="27" Width="80px"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDiasIncapacidad" runat="server" 
                                                ControlToValidate="txtDiasIncapacidad" CssClass="failureNotification" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valDiasIncapacidad%>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valDiasIncapacidad%>" 
                                                ValidationGroup="validarIncapacidad" Width="16px"> <img alt="" src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoIncapacidad" runat="server" DataValueField="Clave" DataTextField="Descripcion" 
                                                CssClass="fontText" TabIndex="28" Width="200px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTipoIncapacidad" runat="server" 
                                                ControlToValidate="ddlTipoIncapacidad" CssClass="failureNotification" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valTipoIncapacidad%>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valTipoIncapacidad%>" 
                                                ValidationGroup="validarIncapacidad" Width="16px"> <img alt="" src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescuentoIncapacidad" runat="server" CssClass="fontText" 
                                                TabIndex="29" Width="80px">0</asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDescuentoIncapacidad" runat="server" 
                                                ControlToValidate="txtDescuentoIncapacidad" CssClass="failureNotification" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valDescuentoIncapacidad%>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valDescuentoIncapacidad%>" 
                                                ValidationGroup="validarIncapacidad" Width="16px"> <img alt="" src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                            <cc1:FilteredTextBoxExtender ID="ftbDescuentoIncapacidad" runat="server" 
                                                FilterType="Numbers, Custom" TargetControlID="txtDescuentoIncapacidad" ValidChars=".">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gdvIncapacidades" runat="server" AutoGenerateColumns="False" 
                                            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                            Width="600px" AllowPaging="false" 
                                            DataKeyNames="Id_Incapacidad,Id_PercepcionDeduccion,Tipo"
                                            OnRowDeleting="gdvIncapacidades_RowDeleting" 
                                            OnSelectedIndexChanged="gdvIncapacidades_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:CommandField ButtonType="Image" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                                                        SelectImageUrl="~/Imagenes/edit_16X16.png" SelectText="" 
                                                        ShowSelectButton="True">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:CommandField>
                                                    <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/error_sign.gif" 
                                                        DeleteText="" HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                                                        ShowDeleteButton="True">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:CommandField>
                                                    <asp:BoundField DataField="DiasIncapacidad" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblDiasIncapacidad %>">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TipoIncapacidad" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblTipoIncapacidad %>" ItemStyle-Width="100">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Descuento" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblDescuentoSinPorc %>">
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <asp:literal ID="Literal1" runat="server" text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                                                </EmptyDataTemplate>
                                                <HeaderStyle BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                                            </asp:GridView>
                                            <asp:HiddenField ID="hfId_Incapacidad" runat="server" />
                                            <asp:HiddenField ID="hfId_RegistroIncapacidad" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlColapsableHorasExtra" runat="server" Width="890px" Height="23px" CssClass="collapsePanelHeader">
                                <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                                    <div style="float: left; margin-left: 20px;">
                                            <asp:Label ID="lblColapsableHorasExtra" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblHorasExtra %>"> </asp:Label>
                                    </div>
                                    <div style="float: right; vertical-align: middle;">
                                        <asp:ImageButton ID="imgbColapsableHorasExtra" runat="server" 
                                            ImageUrl="~/Imagenes/expand_blue.jpg" AlternateText="(Horas Extra...)" 
                                            Visible="true"/>
                                    </div>
                                </div>
                            </asp:Panel>
                            <cc1:CollapsiblePanelExtender ID="cpeConceptosHorasExtra" runat="server" 
                                TargetControlID="pnlConceptosHorasExtra"
                                ExpandControlID="pnlColapsableHorasExtra"
                                CollapseControlID="pnlColapsableHorasExtra" 
                                TextLabelID="pnlColapsableHorasExtra"
                                ImageControlID="imgbColapsableHorasExtra"    
                                ExpandedText=""
                                CollapsedText=""
                                ExpandedImage="~/Imagenes/collapse_blue.jpg"
                                CollapsedImage="~/Imagenes/expand_blue.jpg"
                                SuppressPostBack="true"
                                SkinID="CollapsiblePanelDemo" Enabled="True">
                            </cc1:CollapsiblePanelExtender>
                            <asp:Panel ID="pnlConceptosHorasExtra" Width="890px" runat="server" BackColor="#F0F0F0" CssClass="fontText">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDiasHoraExtra" runat="server" CssClass="fontText" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblDias %>"> </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTipoHoras" runat="server" CssClass="fontText" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblTipoHoras %>"> </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHorasExtra" runat="server" CssClass="fontText" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblHorasExtra %>"> </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblImportePagado" runat="server" CssClass="fontText" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblImportePagado %>"> </asp:Label>
                                        </td>
                                        <td rowspan="2">
                                            <asp:Button ID="btnAgregarHorasExtra" runat="server" CssClass="botonChico" 
                                                Height="20px" OnClick="btnAgregarHorasExtra_Click" TabIndex="36"
                                                Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
                                                ValidationGroup="validarHorasExtra" Width="80px" />
                                            <asp:Button ID="btnCancelarHoraExtra" runat="server" CssClass="botonChico" 
                                                Height="20px" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                                                ValidationGroup="validarPercepcion" Width="80px" TabIndex="37"
                                                onclick="btnCancelarHoraExtra_Click" Visible="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDiasHorasExtra" runat="server" CssClass="fontText" 
                                                TabIndex="32" Width="80px"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDiasHorasExtra" runat="server" 
                                                ControlToValidate="txtDiasHorasExtra" CssClass="failureNotification" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valDias%>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valDias%>" 
                                                ValidationGroup="validarHorasExtra" Width="16px"> <img alt="" src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoHorasExtra" runat="server" 
                                                CssClass="fontText" TabIndex="33" Width="200px">
                                                <asp:ListItem Value="Dobles">Dobles</asp:ListItem>
                                                <asp:ListItem Value="Triples">Triples</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTipoHorasExtra" runat="server" 
                                                ControlToValidate="ddlTipoHorasExtra" CssClass="failureNotification" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valTipoHorasExtra%>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valTipoHorasExtra%>" 
                                                ValidationGroup="validarHorasExtra" Width="16px"> <img alt="" src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHorasExtra" runat="server" CssClass="fontText" 
                                                TabIndex="34" Width="80px"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvHorasExtra" runat="server" 
                                                ControlToValidate="txtHorasExtra" CssClass="failureNotification" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valHorasExtra%>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valHorasExtra%>" 
                                                ValidationGroup="validarHorasExtra" Width="16px"> <img alt="" src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                            <cc1:FilteredTextBoxExtender ID="ftbHorasExtra" runat="server" 
                                                FilterType="Numbers" TargetControlID="txtHorasExtra">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtImportePagadoHorasExtra" runat="server" CssClass="fontText" 
                                                TabIndex="35" Width="80px">0</asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvImportePagadoHorasExtra" runat="server" 
                                                ControlToValidate="txtImportePagadoHorasExtra" CssClass="failureNotification" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valImportePagado%>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valImportePagado%>" 
                                                ValidationGroup="validarHorasExtra" Width="16px"> <img alt="" src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                            <cc1:FilteredTextBoxExtender ID="ftbImportePagadoHorasExtra" runat="server" 
                                                FilterType="Numbers, Custom" TargetControlID="txtImportePagadoHorasExtra" ValidChars=".">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gdvHorasExtra" runat="server" AutoGenerateColumns="False" 
                                            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                            Width="500px" AllowPaging="false" DataKeyNames="Id_HoraExtra"
                                            OnRowDeleting="gdvHorasExtra_RowDeleting" 
                                            OnSelectedIndexChanged="gdvHorasExtra_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:CommandField ButtonType="Image" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                                                        SelectImageUrl="~/Imagenes/edit_16X16.png" SelectText="" 
                                                        ShowSelectButton="True">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:CommandField>
                                                    <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/error_sign.gif" 
                                                        DeleteText="" HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                                                        ShowDeleteButton="True">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:CommandField>
                                                    <asp:BoundField DataField="Dias" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblDias %>">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TipoHoras" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblTipoHoras %>" ItemStyle-Width="100">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="HorasExtra" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblHorasExtra %>">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ImportePagado" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblImportePagado %>">
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <asp:literal ID="Literal1" runat="server" text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                                                </EmptyDataTemplate>
                                                <HeaderStyle BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                                            </asp:GridView>
                                            <asp:HiddenField ID="hfId_Hora_Extra" runat="server" />
                                            <asp:HiddenField ID="hfId_RegistroHoraExtra" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table width="100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnAgregarDetalle" runat="server" CssClass="botonEstilo" 
                            OnClick="btnAgregarDetalle_Click" TabIndex="25"
                            Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
                            ValidationGroup="validarPercepcion" Width="80px" />
                        <asp:Button ID="btnCancelarDetalle" runat="server" CssClass="botonEstilo" 
                             TabIndex="26" Visible="false"
                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                            ValidationGroup="validarPercepcion" Width="80px" 
                            OnClick="btnCancelarDetalle_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="930px">
        <tr>
            <td style="width:50%; vertical-align:top">
                <asp:UpdatePanel ID="unplDetallePercepciones" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gdvConceptosPercepciones" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" DataKeyNames="Id_PercepDedu,Id_Tipo,Id_TipoPercepDedu" GridLines="Horizontal" 
                        Width="100%"  AllowPaging="false" Caption="<%$ Resources:resCorpusCFDIEs, lblPercepciones %>"
                        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                        BackColor="White" OnRowDeleting="gdvConceptosPercepciones_RowDeleting" 
                            OnSelectedIndexChanged="gdvConceptosPercepciones_SelectedIndexChanged">
                            <Columns>
                                <asp:CommandField ButtonType="Image" 
                                    HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                                    SelectImageUrl="~/Imagenes/edit_16X16.png" SelectText="" 
                                    ShowSelectButton="True">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>
                                <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/error_sign.gif" 
                                    DeleteText="" HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                                    ShowDeleteButton="True">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>
                                <asp:BoundField DataField="Clave" 
                                    HeaderText="<%$ Resources:resCorpusCFDIEs, lblClavePercepcion %>" ItemStyle-Width="100">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Concepto" 
                                    HeaderText="<%$ Resources:resCorpusCFDIEs, lblConceptoPercepcion %>">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="250px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ImporteGravado" 
                                    HeaderText="<%$ Resources:resCorpusCFDIEs, lblImporteGravado %>">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ImporteExento" 
                                    HeaderText="<%$ Resources:resCorpusCFDIEs, lblImporteExento %>">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:literal ID="Literal1" runat="server" text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                            </EmptyDataTemplate>
                            <HeaderStyle BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="vertical-align:top">
                <asp:UpdatePanel ID="unplDetalleDeducciones" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gdvConceptosDeducciones" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" DataKeyNames="Id_PercepDedu,Id_Tipo,Id_TipoPercepDedu" GridLines="Horizontal" 
                        Width="100%"  AllowPaging="false" Caption="<%$ Resources:resCorpusCFDIEs, lblDeducciones %>"
                        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                        BackColor="White" onrowdeleting="gdvConceptosDeducciones_RowDeleting" 
                            onselectedindexchanged="gdvConceptosDeducciones_SelectedIndexChanged">
                            <Columns>
                                <asp:CommandField ButtonType="Image" 
                                    HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                                    SelectImageUrl="~/Imagenes/edit_16X16.png" SelectText="" 
                                    ShowSelectButton="True">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>
                                <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/error_sign.gif" 
                                    DeleteText="" HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                                    ShowDeleteButton="True">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>
                                <asp:BoundField DataField="Clave" 
                                    HeaderText="<%$ Resources:resCorpusCFDIEs, lblClavePercepcion %>" ItemStyle-Width="100">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Concepto" 
                                    HeaderText="<%$ Resources:resCorpusCFDIEs, lblConceptoPercepcion %>">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="250px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ImporteGravado" 
                                    HeaderText="<%$ Resources:resCorpusCFDIEs, lblImporteGravado %>">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ImporteExento" 
                                    HeaderText="<%$ Resources:resCorpusCFDIEs, lblImporteExento %>">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:literal ID="Literal1" runat="server" text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                            </EmptyDataTemplate>
                            <HeaderStyle BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upnlTotalesDeduccionesPercepciones" runat="server">
        <ContentTemplate>
            <table style="width:930px">
                <tr>
                    <td style="width:50%">
                        <table id="tblPercepcionesTotales" runat="server" style="width:100%;" rules="none" border="1" visible="false">
                            <tr>
                                <td style="width:74%">
                                    <asp:Label ID="lblTotalesPercepciones" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTotal %>"
                                        Visible="false">
                                    </asp:Label>
                                </td>
                                <td style="width:13%; text-align:right;">
                                    <asp:Label ID="lblTotalGravadoPercepciones" runat="server" Text="0" Visible="false">
                                    </asp:Label>
                                </td>
                                <td style="width:13%; text-align:right;">
                                    <asp:Label ID="lblTotalExentoPercepciones" runat="server" Text="0" Visible="false">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width:50%">
                        <table style="width:100%" id="tblDeduccionesTotales" runat="server" rules="none" border="1" visible="false">
                            <tr>
                                <td style="width:74%">
                                    <asp:Label ID="lblTotalesDeducciones" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTotal %>"
                                        Visible="false">
                                    </asp:Label>
                                </td>
                                <td style="width:13%; text-align:right;">
                                    <asp:Label ID="lblTotalGravadoDeducciones" runat="server" Text="0" Visible="false">
                                    </asp:Label>
                                </td>
                                <td style="width:13%; text-align:right;">
                                    <asp:Label ID="lblTotalExentoDeducciones" runat="server" Text="0" Visible="false">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlTotales" runat="server">
        <ContentTemplate>
            <table style="width:930px">
                <tr>
                    <td style="text-align:right;">
                        <asp:Label ID="lblSubtotal" runat="server" style="font-weight: 700" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblSubtotal %>"  Visible="False" Width="100px">
                        </asp:Label>
                    </td>
                    <td style="width:150px; text-align:right;">
                        <asp:Label ID="lblDetSubtotal" runat="server" Visible="False">
                    </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;">
                        <asp:Label ID="lblDetDescuento" runat="server" style="font-weight: 700" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblDescuentoSinPorc %>"  Visible="False" Width="100px">
                        </asp:Label>
                    </td>
                    <td style="width:150px; text-align:right;">
                        <asp:Label ID="lblDetDescuentoVal" runat="server" Visible="False">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;">
                        <asp:Label ID="lblISR" runat="server" style="font-weight: 700" Text="<%$ Resources:resCorpusCFDIEs, lblISR %>" 
                            Visible="False" Width="100px"> 
                        </asp:Label>
                    </td>
                    <td style="width:150px; text-align:right;">
                        <asp:Label ID="lblISRVal" runat="server" Visible="False"> 
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;">
                        <asp:Label ID="lblTotal" runat="server" style="font-weight: 700" Text="<%$ Resources:resCorpusCFDIEs, lblTotal %>" 
                            Visible="False" Width="100px"> 
                        </asp:Label>
                    </td>
                    <td style="width:150px; text-align:right;">
                        <asp:Label ID="lblTotalVal" runat="server" Visible="False"> 
                        </asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:Label ID="lblNumerosLetras" runat="server" style="font-weight: 700"> </asp:Label>
    <asp:UpdatePanel ID="upnlRegistrarNomina" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnNuevo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, btnNuevo %>" 
                         TabIndex="38" CssClass="botonEstilo" OnClick="btnNuevo_Click" />
                        <asp:Button ID="btnCrear" runat="server" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                         TabIndex="39" CssClass="botonEstilo" OnClick="btnCrear_Click" ValidationGroup="NominaValidationGroup" />
                        <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                         TabIndex="40" CssClass="botonEstilo" OnClick="btnCancelar_Click" />
                        <asp:Button ID="btnGenerarNomina" runat="server" Text="<%$ Resources:resCorpusCFDIEs, btnRegresar %>" 
                         TabIndex="41" CssClass="botonEstilo" Visible="false"
                            OnClick="btnGenerarNomina_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsNominaRegistro" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, varValidacionError %>" 
                             ValidationGroup="NominaValidationGroup" ShowMessageBox="True" ShowSummary="False" />
                    </td>
                </tr>
            </table>
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
                                        MaxLength="15" TabIndex="42">
                                    </asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumeroEmpleadoBusqueda" runat="server" CssClass="textEntry" 
                                        MaxLength="15" TabIndex="44">
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
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNombreEmpleadoBusqueda" runat="server" CssClass="textEntry" 
                                        MaxLength="20" TabIndex="43">
                                    </asp:TextBox>
                                </td>
                                <td>
                            
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <p>
                    <asp:Button ID="btnConsulta" runat="server" TabIndex="45"
                        Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                        CssClass="botonEstilo" OnClick="btnConsulta_Click" />
                        <asp:Button ID="btnCancelarModal" runat="server" TabIndex="46"
                        Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                        CssClass="botonEstilo" OnClick="btnCancelarModal_Click" />
                </p>
                <asp:GridView ID="gdvEmpleados" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" GridLines="Horizontal" Width="600px" 
                    DataKeyNames="Id_Empleado,TipoRegimen,Banco,RiesgoPuesto" AllowPaging="True" 
                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" PageSize="5" 
                    BackColor="White" OnPageIndexChanging="gdvEmpleados_PageIndexChanging" 
                    OnSelectedIndexChanged="gdvEmpleados_SelectedIndexChanged">
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
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="linkModal" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
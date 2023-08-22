<%@ Page Title="Timbrado" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webTimbradoGeneracion.aspx.cs" Inherits="Timbrado_webTimbradoGeneracion" ValidateRequest="false" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit.HTMLEditor" tagprefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <script type="text/javascript" language="javascript">



        function calcularCantidadArt() {
            var nPrecio = document.getElementById('<%=txtPrecioArt.ClientID %>').value;
            var nCantidad = document.getElementById('<%=txtCantidadArt.ClientID %>').value;

            document.getElementById('<%=txtImporteArt.ClientID %>').value = 0;

            if (nPrecio.value == 0 || nCantidad.value == 0) {
                document.getElementById('<%=txtImporteArt.ClientID %>').value = 0;

            }
            else {
                document.getElementById('<%=txtImporteArt.ClientID %>').value = parseFloat(nPrecio) * parseFloat(nCantidad);

            }

            function calcularPrecioArt() {
                var nPrecio = document.getElementById('<%=txtPrecioArt.ClientID %>').value;
                var nCantidad = document.getElementById('<%=txtCantidadArt.ClientID %>').value;

                document.getElementById('<%=txtImporteArt.ClientID %>').value = 0;

                if (nPrecio.value == 0 || nCantidad.value == 0) {
                    document.getElementById('<%=txtImporteArt.ClientID %>').value = 0;

                }
                else {
                    document.getElementById('<%=txtImporteArt.ClientID %>').value = parseFloat(nPrecio) * parseFloat(nCantidad);

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

        function onOff(validatorId, activar) {
            var validator = document.getElementById(validatorId);
            ValidatorEnable(validator, activar);
        }

        function url() {
            var pag = document.getElementById('<%= ddlAddenda.ClientID %>');
            var parametro = "";
            if (pag.value == 'Addendas/webAddendaFIDEAPECH.aspx') {
                var tot = document.getElementById('<%= lblTotalVal.ClientID%>').innerHTML;
                if (tot != null) {
                    parametro = '?totFact=' + tot.replace('$', '');
                }
            }
            else {
                parametro = "";
            }

            hidden = open(pag.value + parametro, 'NewWindow', 'top=0,left=0,width=1150,height=670,status=yes,resizable=yes,scrollbars=yes');
        }

        function urlComplemento() {
            var pag = document.getElementById('<%= ddlComplemento.ClientID %>');
            hidden = open(pag.value, 'NewWindow', 'top=0,left=0,width=1200,height=700,status=yes,resizable=yes,scrollbars=yes');
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

        .style9
        {
            width: 19px;
        }
        
        .style15
        {
            width: 176px;
        }

        .style18
        {
            width: 170px;
        }
        
        .style21
        {
            width: 16px;
        }
        .style22
        {
            width: 188px;
        }

        </style>

    <h2 >
        <asp:Label ID="lblCFDI" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCFDIPantalla %>"></asp:Label>
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

              
            <asp:Panel ID="pnlSucEmisor" runat="server" GroupingText="<%$ Resources:resCorpusCFDIEs, lblDatosFisc %>" 
                  Width="930px">
                  <table>
                    <tr>
                        <td class="style18">
                        
                            <asp:Label ID="lblNombreSucursal0" runat="server" CssClass="fontText" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblSucursal %>" Font-Bold="True"></asp:Label>
                        
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp; </td>
                        <td>
                            <asp:Label ID="lblDireccionFis" runat="server" CssClass="fontText"></asp:Label>
                            </td>
                    </tr>
                    <tr>
                        <td class="style18">
                        
                            <asp:DropDownList ID="ddlSucursalesFis" runat="server" AutoPostBack="True" 
                                CssClass="fontText" DataTextField="nombre" DataValueField="id_estructura" 
                                onselectedindexchanged="ddlSucursalesFis_SelectedIndexChanged" TabIndex="2" 
                                Width="300px">
                            </asp:DropDownList>
                        
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Label ID="lblUbicacionFis" runat="server" CssClass="fontText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style18">
                        
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Label ID="lblRFCFis" runat="server" CssClass="fontText"></asp:Label>
                        </td>
                    </tr>
                      <tr>
                          <td colspan="2">
                              <asp:CheckBox ID="cbAgrExpEn" runat="server" AutoPostBack="true" 
                                  CssClass="fontText" oncheckedchanged="cbAgrExpEn_CheckedChanged" TabIndex="1" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblAgrExpEn %>" />
                          </td>
                          <td>
                              &nbsp;</td>
                      </tr>
                </table>                
 
                
            </asp:Panel>

            <asp:Panel ID="pnlExtender" runat="server" Width="930px"  Height="15px" CssClass="collapsePanelHeader">
              <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                
                <div style="float: left; margin-left: 20px;">
                    <asp:Label ID="lblExpedidoEn" runat="server"></asp:Label>
                </div>
                <div style="float: right; vertical-align: middle;">
                    <asp:ImageButton ID="Image1" runat="server" 
                        ImageUrl="~/Imagenes/expand_blue.jpg" AlternateText="(Expedido En...)" 
                        Visible="False"/>
                </div>
              </div>
           </asp:Panel>

          <cc1:CollapsiblePanelExtender ID="cpeExpedidoEn" runat="server" 
                    TargetControlID="pnlExpedidoEn"
                    ExpandControlID="pnlExtender"
                    CollapseControlID="pnlExtender" 
                    TextLabelID="Label1"
                    ImageControlID="Image1"    
                    ExpandedText=""
                    CollapsedText=""
                    ExpandedImage="~/Imagenes/collapse_blue.jpg"
                    CollapsedImage="~/Imagenes/expand_blue.jpg"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanelDemo" Enabled="True">
         </cc1:CollapsiblePanelExtender>
         
            <%--Inicia Expedido En--%>
            <asp:Panel ID="pnlExpedidoEn" runat="server" 
                  GroupingText="<%$ Resources:resCorpusCFDIEs, lblExpedidoEn %>" Width="930px" 
                  BackColor="#F0F0F0">
                   <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblNombreSucursal" runat="server" CssClass="fontText"
                                    AssociatedControlID="ddlSucursales" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblSucursal %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCalle0" runat="server" AssociatedControlID="txtCalle" 
                                    CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>"></asp:Label>
                            </td>
                            <td>
                                 </td>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                
                                <asp:DropDownList ID="ddlSucursales" runat="server" AutoPostBack="True" CssClass="fontText"
                                    DataTextField="nombre" DataValueField="id_estructura" 
                                    onselectedindexchanged="ddlSucursales_SelectedIndexChanged" TabIndex="2" 
                                    Width="300px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvSucursales" runat="server" 
                                    ControlToValidate="ddlSucursales" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCalleExpEn" runat="server" CssClass="fontText" 
                                    MaxLength="255" TabIndex="7" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCalleExpEn" runat="server" 
                                    ControlToValidate="txtCalleExpEn" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valCalleEmisor %>" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valCalleEmisor %>" 
                                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                 &nbsp;</td>
                            <td>
                                <asp:CheckBox ID="cbOtraUbi" runat="server" AutoPostBack="True" 
                                    oncheckedchanged="cbOtraUbi_CheckedChanged" TabIndex="3" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblOtraUbicacion %>" 
                                    CssClass="fontText" />
                            </td>
                        </tr>
                        <tr ID="RenlblPaisEmi" runat="server">
                            <td>
                                <asp:Label ID="lblPais0" runat="server" AssociatedControlID="txtPais" 
                                    CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"></asp:Label>
                                </td>
                            <td>
                                 <asp:Label ID="lblColoniaEmisor" runat="server" 
                                     AssociatedControlID="txtMunicipio" CssClass="fontText" 
                                     Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>"></asp:Label>
                            </td>
                            <td>
                                 </td>
                            <td>
                                <asp:Label ID="lblNoInteriorEmisor" runat="server" 
                                    AssociatedControlID="txtMunicipio" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>" CssClass="fontText"></asp:Label>
                            </td>
                        </tr>
                        <tr ID="RenddlPaisEmi" runat="server">
                            <td>
                                <asp:DropDownList ID="ddlPaisExpEn" runat="server" AutoPostBack="True" 
                                    CssClass="fontText" DataTextField="pais" DataValueField="id_pais" 
                                    onselectedindexchanged="ddlPaisEmisor_SelectedIndexChanged" TabIndex="3" 
                                    Width="300px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPaisExpEn" runat="server" 
                                    ControlToValidate="ddlPaisExpEn" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valPaisEmisor%>" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valPaisEmisor%>" 
                                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <%--CssClass="textEntry"--%>
                                <asp:TextBox ID="txtColoniaExpEn" runat="server" CssClass="fontText" 
                                    MaxLength="255" TabIndex="8" Width="200px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoInteriorExpEn" runat="server" CssClass="fontText" 
                                    MaxLength="255" TabIndex="11" Width="146px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="RenlblEdoEmi" runat="server">
                            <td>
                                 <asp:Label ID="lblEstado0" runat="server" AssociatedControlID="txtEstado" 
                                     CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"></asp:Label>
                                 </td>
                            <td>
                                <asp:Label ID="lblReferenciaEmisor" runat="server" 
                                    AssociatedControlID="txtMunicipio" CssClass="fontText" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblReferenciaDom %>"></asp:Label>
                            </td>
                            <td >
                            </td>
                            <td>
                                <asp:Label ID="lblNoExteriorEmisor" runat="server" 
                                    AssociatedControlID="txtMunicipio" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>" CssClass="fontText"></asp:Label>
                            </td>
                        </tr>
                        <tr id="RenddlEdoEmi" runat="server">
                            <td>
                                 <asp:DropDownList ID="ddlEstadoExpEn" runat="server" CssClass="fontText" 
                                     DataTextField="estado" DataValueField="id_estado" TabIndex="4" 
                                     Width="300px">
                                 </asp:DropDownList>
                                 <asp:RequiredFieldValidator ID="rfvEstadoExpEn" runat="server" 
                                     ControlToValidate="ddlEstadoExpEn" CssClass="failureNotification" 
                                     ToolTip="<%$ Resources:resCorpusCFDIEs, valEstadoEmisor %>" 
                                     ErrorMessage="<%$ Resources:resCorpusCFDIEs, valEstadoEmisor %>" 
                                     ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                                 </td>
                            <td>
                                 <%--CssClass="textEntry"--%>
                                 <cc1:FilteredTextBoxExtender ID="txtCodigoPostalExpEn_FilteredTextBoxExtender" 
                                     runat="server" FilterType="Numbers" TargetControlID="txtCodigoPostalExpEn">
                                 </cc1:FilteredTextBoxExtender>
                                 <asp:TextBox ID="txtReferenciaExpEn" runat="server" CssClass="fontText" 
                                     MaxLength="255" TabIndex="9" Width="200px"></asp:TextBox>
                                 </td>
                            <td>
                                 </td>
                            <td>
                                <asp:TextBox ID="txtNoExteriorExpEn" runat="server" CssClass="fontText" 
                                    MaxLength="255" TabIndex="12" Width="146px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="RenlblMpoEmi">
                          <td>

                                 <asp:Label ID="lblMunicipio0" runat="server" AssociatedControlID="txtMunicipio" 
                                     CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>"></asp:Label>

                                 </td>
                            <td>

                                 <asp:Label ID="LablblCodigoPostalel6" runat="server" 
                                     AssociatedControlID="txtCodigoPostal" CssClass="fontText" 
                                     Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>"></asp:Label>

                                 </td>
                            <td>
                                 </td>
                            <td>
                                </td>
                        </tr>
                        <tr id="RentxtMpoEmi" runat="server">
                            <td>
                                <asp:TextBox ID="txtMunicipioExpEn" runat="server" CssClass="fontText" 
                                    MaxLength="255" TabIndex="5" Width="200px"></asp:TextBox>
                                <%--CssClass="textEntry"--%>
                                <asp:RequiredFieldValidator ID="rfvMpoExpEn" runat="server" 
                                    ControlToValidate="txtMunicipioExpEn" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valMunicipioEmisor %>" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valMunicipioEmisor %>" 
                                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCodigoPostalExpEn" runat="server" CssClass="fontText" 
                                    MaxLength="5" TabIndex="10" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCPExpEn" runat="server" 
                                    ControlToValidate="txtCodigoPostalExpEn" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valCPEmisor %>" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valCPEmisor %>" 
                                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                 </td>
                            <td>
                                <asp:CheckBox ID="cbSerieFolioMatriz" runat="server" 
                                    text = "Serie y Folio de Sucursal Matriz" 
                                    oncheckedchanged="cbSerieFolioMatriz_CheckedChanged" AutoPostBack="True"/>
                            </td>
                        </tr>
                        <tr runat="server" id="RenlblLocalEmi">
                            <td>

                                <asp:Label ID="lblLocalidadEmisor" runat="server" 
                                    AssociatedControlID="txtMunicipio" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>" CssClass="fontText"></asp:Label>

                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                 </td>
                            <td>
                                </td>
                        </tr>
                        <tr runat="server" id="RentxtLocalEmi">
                            <td>

                                <asp:TextBox ID="txtLocalidadExpEn" runat="server" CssClass="fontText" 
                                    MaxLength="255" TabIndex="6" Width="198px"></asp:TextBox>

                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
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
                                 </td>
                            <td>
                                </td>
                        </tr>
                    </table>              
            </asp:Panel>
            <%--Termina Expedido En--%>
            <br />
            <%--Inicia Lugar Expedicion--%>
            <asp:Panel ID="pnlLugExp" runat="server" GroupingText="<%$ Resources:resCorpusCFDIEs, lblLugarExp %>" 
                  Width="930px">
            <table>
            <tr>
                <td>
                
                    <asp:Label ID="lblPais1" runat="server" AssociatedControlID="txtPais" 
                        CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"></asp:Label>
                
                </td>
                <td>
                
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblEstado1" runat="server" AssociatedControlID="txtEstado" 
                        CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                
                    <asp:DropDownList ID="ddlPaisLugExp" runat="server" AutoPostBack="True" 
                        CssClass="fontText" DataTextField="pais" DataValueField="id_pais" 
                        onselectedindexchanged="ddlPaisEmisor_SelectedIndexChanged" TabIndex="13" 
                        Width="300px">
                    </asp:DropDownList>
                
                </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                    <asp:DropDownList ID="ddlEdoLugExp" runat="server" CssClass="fontText" 
                        DataTextField="estado" DataValueField="id_estado" TabIndex="14" 
                        Width="300px">
                    </asp:DropDownList>
                </td>
            </tr>
            </table>
            </asp:Panel> 
            <%--Termina Lugar Expedicion--%>


            <asp:Panel ID="pnlSucEmisor0" runat="server" GroupingText="<%$ Resources:resCorpusCFDIEs, lblDatosGen %>" 
                  Width="930px">
                  <table runat="server" id="tblVersion32">
                        <tr>
                            <td>
                                <asp:Label ID="lblMetodoPago" runat="server" 
                                    AssociatedControlID="ddlSucursales" 
                                    Text="Método de Pago" CssClass="fontText"></asp:Label>
                            </td>
                            <td>
                                 </td>
                            <td colspan="2">
                                <asp:Label ID="lblNumeroCuent" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblNumeroCuenta %>" CssClass="fontText"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblformaPago" runat="server" 
                                    AssociatedControlID="ddlSucursales" Text="<%$ Resources:resCorpusCFDIEs, lblformadepago %>"
                                    CssClass="fontText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlMetodoPago" runat="server" CssClass="fontText"
                                    OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged" TabIndex="0" 
                                    Width="250px" AutoPostBack="True">
                                    <asp:ListItem>No Aplica</asp:ListItem>
                                    <asp:ListItem>Efectivo</asp:ListItem>
                                    <asp:ListItem>Transferencias Electrónicas de Fondos</asp:ListItem>
                                    <asp:ListItem>Cheques</asp:ListItem>
                                    <asp:ListItem>Tarjetas de débito</asp:ListItem>
                                    <asp:ListItem>Tarjetas de crédito</asp:ListItem>
                                    <asp:ListItem>Tarjetas de servicio</asp:ListItem>
                                    <asp:ListItem>Tarjetas de monedero electrónico</asp:ListItem>
                                    <asp:ListItem>No identificado</asp:ListItem>
                                    <asp:ListItem>NA</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                 </td>
                            <td>

                                <asp:TextBox ID="txtNumeroCuenta" runat="server" TabIndex="16" Width="200px" 
                                    CssClass="fontText"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbNumeroCuenta" runat="server" 
                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                    TargetControlID="txtNumeroCuenta" 
                                    InvalidChars="°!&quot;#$%&amp;/()=?¡|¿'´+*{[}]-_.:,;@¬\~^`" 
                                    ValidChars="0-9,A-Z">
                                </cc1:FilteredTextBoxExtender>
                            </td> 
                             <td>
                                <asp:RegularExpressionValidator ID="regxCantidadArt4" runat="server"
                                ControlToValidate ="txtNumeroCuenta" ValidationExpression = ".{4}.*"
                                CssClass="failureNotification"
                                ToolTip = "<%$ Resources:resCorpusCFDIEs, valLongNumeroDeCuenta %>"                                
                                ErrorMessage = "<%$ Resources:resCorpusCFDIEs, valLongNumeroDeCuenta %>"                                
                                ValidationGroup="RegisterUserValidationGroup" width ="16px">*</asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="fontText"
                                    onselectedindexchanged="ddlFormaPago_SelectedIndexChanged" TabIndex="17" 
                                    Width="250px" AutoPostBack="True">
                                    <asp:ListItem Selected="True">Pago en una sola exhibición.</asp:ListItem>
                                    <asp:ListItem>Pago en Parcialidades</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNumeroCuent0" runat="server" CssClass="fontText" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblRegimenfiscal %>"></asp:Label>
                            </td>
                            <td>
                                 </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                 </td>
                            <td align="right">
                                 <asp:ImageButton ID="imgbtnPagPar" runat="server" 
                                     ImageUrl="~/Imagenes/modificar.gif" onclick="imgbtnPagPar_Click" 
                                     ToolTip="<%$ Resources:resCorpusCFDIEs, varEditarPagoParcial %>" 
                                     ErrorMessage="<%$ Resources:resCorpusCFDIEs, varEditarPagoParcial %>" 
                                     Visible="False" TabIndex="18" />
                                 </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtRegimenfiscal" runat="server" CssClass="fontText" 
                                    TabIndex="19" Width="250px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvCodigo2" runat="server" 
                                    ControlToValidate="txtRegimenfiscal" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valRegimenFiscal%>" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valRegimenFiscal%>" 
                                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                </td>
                            <td>
                                </td>
                            <td>
                                 </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                 <asp:Label ID="lblTipoDoc" runat="server" CssClass="fontText" 
                                     Text="<%$ Resources:resCorpusCFDIEs, lblTipoDocumento %>"></asp:Label>
                                 </td>
                            <td class="style9">
                                 </td>
                            <td class="style22">
                                 <asp:Label ID="lblMoneda" runat="server" Text="Moneda" CssClass="fontText"></asp:Label>
                                 </td> 

                            <td class="style9">
                                &nbsp;</td>

                            <td>
                                 <asp:Label ID="lblTipoDoc2" runat="server" CssClass="fontText" 
                                     Text="<%$ Resources:resCorpusCFDIEs, lblComplemento %>"></asp:Label>
                                 </td>
                        </tr>           
                        <tr>
                            <td>
                                 <asp:DropDownList ID="ddlTipoDoc" runat="server" AutoPostBack="True" 
                                     CssClass="fontText" DataTextField="nombre" DataValueField="id_efecto" 
                                     onselectedindexchanged="ddlTipoDoc_SelectedIndexChanged" TabIndex="20" 
                                     Width="250px">
                                 </asp:DropDownList>
                                 </td>
                            <td class="style9">
                                 </td>
                            <td class="style22">
                                 <asp:DropDownList ID="ddlMoneda" runat="server" AutoPostBack="True" 
                                     CssClass="fontText" onselectedindexchanged="ddlMoneda_SelectedIndexChanged" 
                                     TabIndex="21" Width="200px">
                                     <asp:ListItem Value="MXN">Pesos Mexicanos</asp:ListItem>
                                     <asp:ListItem Value="USD">Dólares</asp:ListItem>
                                     <asp:ListItem Value="XEU">Euros</asp:ListItem>
                                 </asp:DropDownList>
                                 </td>
                            <td class="style9">
                                &nbsp;</td>
                            <td>
                                 <asp:DropDownList ID="ddlComplemento" runat="server" CssClass="fontText" 
                                     TabIndex="22" Width="250px" AutoPostBack="True" 
                                     onselectedindexchanged="ddlComplemento_SelectedIndexChanged">
                                 </asp:DropDownList>
                                 </td>
                        </tr> 
                        <tr>
                            <td>
                                 </td>
                            <td class="style9">
                                  </td>
                            <td class="style22">
                                 </td>
                            <td class="style21">
                                &nbsp;</td>
                            <td align="right">
                                 <asp:ImageButton ID="imgbtnComplemento1" runat="server" 
                                     ImageUrl="~/Imagenes/modificar.gif" onclick="imgbtnComplemento_Click" 
                                     ToolTip="<%$ Resources:resCorpusCFDIEs, VarComplementoSeleccionado %>" 
                                     ErrorMessage="<%$ Resources:resCorpusCFDIEs, VarComplementoSeleccionado %>" 
                                     Enabled="False" />
                                 <asp:ImageButton ID="imgbtnCambiarComplemento1" runat="server" 
                                     ImageUrl="~/Imagenes/unlock.gif" onclick="imgbtnCambiarComplemento_Click" 
                                     style="height: 16px" 
                                     ToolTip="<%$ Resources:resCorpusCFDIEs, VarComplementoDesbloquea %>" />
                                 </td>
                        </tr>
                        <tr>
                            <td>
                                 <asp:Label ID="lblSerie" runat="server" CssClass="fontText" Text="Serie"></asp:Label>
                                 </td>
                            <td class="style9">
                                  </td>
                            <td class="style22">
                                 <asp:Label ID="blFolio" runat="server" CssClass="fontText" Text="Folio"></asp:Label>
                                 </td>
                            <td class="style21">
                                &nbsp;</td>
                            <td>
                                 
                                 </td>
                        </tr>                                                                                
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlSerie" runat="server" AutoPostBack="True" 
                                    CssClass="fontText" DataTextField="Serie" DataValueField="id_Serie" 
                                    onselectedindexchanged="ddlSerie_SelectedIndexChanged" TabIndex="23" 
                                    Width="120px">
                                </asp:DropDownList>
                            </td>
                            <td class="style9">
                                </td>
                            <td class="style22">
                                <asp:TextBox ID="txtFolio" runat="server" CssClass="fontText" Enabled="False" 
                                    TabIndex="23" Width="120px"></asp:TextBox>
                            </td>
                            <td class="style21">
                                &nbsp;</td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDescuentoGlobal" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDescuentoGlobal %>" CssClass="fontText">
                                </asp:Label>
                            </td>
                            <td class="style9">
                                &nbsp;</td>
                            <td class="style22">
                                <asp:Label ID="lblTipoCambio" runat="server" 
                                     Text="<%$ Resources:resCorpusCFDIEs, lblTipoCambio %>" CssClass="fontText"></asp:Label>
                            </td>
                            <td class="style21">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtDescuentoGlobal" runat="server" Text="0" 
                                    CssClass="fontText" AutoPostBack="True" TabIndex="25"
                                    OnTextChanged="txtDescuentoGlobal_TextChanged"></asp:TextBox>
                                <asp:CheckBox ID="chkDescuentoGlobal" runat="server" Checked="false" Text="%"
                                   TabIndex="26">
                                </asp:CheckBox>
                                <cc1:FilteredTextBoxExtender ID="ftbeDescuentoGlobal" runat="server" 
                                    FilterType="Numbers, Custom" TargetControlID="txtDescuentoGlobal" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="revDescuentoGlobal" runat="server" 
                                    ControlToValidate="txtDescuentoGlobal" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validarImpCompl" Width="16px"> 
                                    <img alt="" src="../Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>
                            </td>
                            <td class="style9">
                                &nbsp;</td>
                            <td class="style22">
                                <asp:TextBox ID="txttipoCambio" runat="server" CssClass="fontText" 
                                    TabIndex="27" Width="80px">0</asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCodigo1" runat="server" 
                                    ControlToValidate="txttipoCambio" CssClass="failureNotification" Height="18px" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valTipoCambio %>" 
                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valTipoCambio %>" 
                                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                            </td>
                            <td class="style21">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
              </asp:Panel>

            <tr>
                <td>
            <asp:UpdateProgress ID="udpEmisor" runat="server">
                    <progresstemplate>
                        <img alt="" 
                    src="../Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgbtnComplemento1" />
            <asp:PostBackTrigger ControlID="imgbtnPagPar" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upFormulario" runat="server" >
        <ContentTemplate>

            <asp:Panel ID="pnlFormulario" runat="server"  GroupingText="<%$ Resources:resCorpusCFDIEs, lblDatosCliente %>" 
                        Width="930px">
            <table>
            <tr>
                <td>
                    <asp:Label ID="lblRazonReceptor" runat="server" 
                        AssociatedControlID="txtRazonReceptor" CssClass="fontText" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblCalle" runat="server" AssociatedControlID="txtCalle" 
                        CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtRazonReceptor" runat="server" CssClass="fontText" 
                        MaxLength="200" TabIndex="28" Width="300" Enabled="False"></asp:TextBox>
                </td>
                <td>
                    <asp:Image ID="linkModal" runat="server" 
                        AlternateText="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaReceptor %>" 
                        CssClass="imagenModal" ImageUrl="~/Imagenes/lupa.png" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaReceptor %>" />
                    <cc1:ModalPopupExtender ID="linkModal_ModalPopupExtender" runat="server" 
                        BackgroundCssClass="modalBackground" 
                        DynamicServicePath="" Enabled="True" PopupControlID="pnlBusqueda" 
                        TargetControlID="linkModal">
                    </cc1:ModalPopupExtender>
                    <asp:RequiredFieldValidator ID="rfvRazonReceptor" runat="server" 
                        ControlToValidate="txtRazonReceptor" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valRazonSocial %>" 
                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valRazonSocial %>" 
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtCalle" runat="server" CssClass="fontText" MaxLength="255" 
                        TabIndex="35" Width="300px" Enabled="False"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvCalle" runat="server" 
                        ControlToValidate="txtCalle" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>" 
                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valCalle %>" 
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRfcReceptor" runat="server" 
                        AssociatedControlID="txtRfcReceptor" CssClass="fontText" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblRfc %>"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblNoExterior" runat="server" 
                        AssociatedControlID="txtNoExterior" CssClass="fontText" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtRfcReceptor" runat="server" CssClass="fontText" 
                        MaxLength="15" TabIndex="29" Width="300px" Enabled="False"></asp:TextBox>
                </td>
                <td>
                    <asp:RegularExpressionValidator ID="regxRFC" runat="server" 
                        ControlToValidate="txtRfcReceptor" CssClass="failureNotification" 
                        Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valRfcCadena %>" 
                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valRfcCadena %>" 
                        
                        ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]" 
                        ValidationGroup="RegisterUserValidationGroup"><img 
                        src="../Imagenes/error_sign.gif" />*</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvRFCReceptor" runat="server" 
                        ControlToValidate="txtRfcReceptor" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valRfc %>" 
                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valRfc %>" 
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtNoExterior" runat="server" CssClass="fontText" 
                        MaxLength="10" TabIndex="36" Width="300px" Enabled="False"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSucursal" runat="server" AssociatedControlID="ddlSucursal" 
                        CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>"></asp:Label>
                </td>
                <td>
                 </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblNoInterior" runat="server" 
                        AssociatedControlID="txtNoInterior" CssClass="fontText" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlSucursal" runat="server" 
                        CssClass="fontText" DataTextField="nombre" DataValueField="id_estructura" TabIndex="30" 
                        Width="300px" AutoPostBack="True" 
                        onselectedindexchanged="ddlSucursal_SelectedIndexChanged1">
                    </asp:DropDownList>
                </td>
                <td>
                 </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtNoInterior" runat="server" CssClass="fontText" 
                        MaxLength="10" TabIndex="37" Width="300px" Enabled="False"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPais" runat="server" AssociatedControlID="txtPais" 
                        CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"></asp:Label>
                </td>
                <td>
                 </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblColonia" runat="server" AssociatedControlID="txtColonia" 
                        CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtPais" runat="server" CssClass="fontText" MaxLength="255" 
                        TabIndex="31" Width="300px" Enabled="False"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvPais" runat="server" 
                        ControlToValidate="txtPais" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valPais %>" 
                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valPais %>"
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                 </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtColonia" runat="server" CssClass="fontText" MaxLength="255" 
                        TabIndex="38" Width="300px" Enabled="False"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEstado" runat="server" AssociatedControlID="txtEstado" 
                        CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"></asp:Label>
                </td>
                <td>
                 </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                    <asp:Label ID="LablblCodigoPostalel5" runat="server" 
                        AssociatedControlID="txtCodigoPostal" CssClass="fontText" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtEstado" runat="server" CssClass="fontText" MaxLength="255" 
                        TabIndex="32" Width="300px" Enabled="False"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvEstado" runat="server" 
                        ControlToValidate="txtEstado" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valEstado %>" 
                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valEstado %>" 
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                 </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="fontText" 
                        MaxLength="5" TabIndex="39" Width="300px" Enabled="False"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                        FilterType="Numbers" TargetControlID="txtCodigoPostal">
                    </cc1:FilteredTextBoxExtender>
                </td>
                <td>
                    </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMunicipio" runat="server" AssociatedControlID="txtMunicipio" 
                        CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>"></asp:Label>
                </td>
                <td>
                 </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                </td>
                <td>
                   </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtMunicipio" runat="server" CssClass="fontText" 
                        MaxLength="255" TabIndex="33" Width="300px" Enabled="False"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvMunicipio" runat="server" 
                        ControlToValidate="txtMunicipio" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valMunicipio %>" 
                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valMunicipio %>"
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                 </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLocalidad" runat="server" AssociatedControlID="txtLocalidad" 
                        CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>"></asp:Label>
                </td>
                <td>
                 </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtLocalidad" runat="server" CssClass="fontText" 
                        MaxLength="255" TabIndex="34" Width="300px" Enabled="False"></asp:TextBox>
                </td>
                <td>
                 </td>
                <td>
                    &nbsp;
                    &nbsp;</td>
                <td>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            </table>
                </asp:Panel>

                </td>
            </tr>

        </table>

            <asp:UpdateProgress ID="updProgress" runat="server">
                    <progresstemplate>
                        <img alt="" 
                    src="../Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>
   
                     </ContentTemplate>
    </asp:UpdatePanel>



    <asp:UpdatePanel ID="upArticulos" runat="server" >
        <ContentTemplate>
              <asp:Panel ID="pnlRegistrosArticulos" runat="server"
                  GroupingText="<%$ Resources:resCorpusCFDIEs, lblCptoArticulos %>" Width="930px">
                  <table>
                      <tr>
                          <td>
                              <asp:Label ID="lblCodigo" runat="server" CssClass="fontText" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblCodigo %>"></asp:Label>
                          </td>
                          <td>
                          <asp:Image ID="linkModalArt" runat="server" 
                                  AlternateText="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaArticulo %>" 
                                  CssClass="imagenModal" ImageUrl="~/Imagenes/lupa.png" 
                                  ToolTip="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaArticulo %>" />
                             </td>
                          <td>
                              <asp:Label ID="Label70" runat="server" CssClass="fontText" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblMedida %>"></asp:Label>
                          </td>
                          <td>
                             </td>
                          <td>
                              <asp:Label ID="lblDescripcion1" runat="server" CssClass="fontText" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblDescripcion %>"></asp:Label>
                          </td>
                          <td>
                             </td>
                          <td>
                              <asp:Label ID="lblPrecio1" runat="server" CssClass="fontText" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblPrecio %>"></asp:Label>
                          </td>
                          <td>
                              <asp:Label ID="Label75" runat="server" CssClass="fontText" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblMoneda %>"></asp:Label>
                          </td>
                          <td>
                              <asp:Label ID="Label71" runat="server" CssClass="fontText" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblIva %>"></asp:Label>
                          </td>
                          <td>
                              <asp:Label ID="lblCantidad5" runat="server" CssClass="fontText" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblCantidad %>"></asp:Label>
                          </td>
                          <td>
                             </td>
                          <td>
                            <asp:Label ID="lblImporte0" runat="server" CssClass="fontText" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblImporte %>">
                            </asp:Label>
                        </td>
                      </tr>
                      <tr valign="top" style="height:40px">
                          <td width="185">
                              <asp:TextBox ID="txtCodigoArt" runat="server" CssClass="fontText" TabIndex="40" 
                                  Width="50px"></asp:TextBox>
                                  

                          </td>
                          <td width="370">
                              <asp:RequiredFieldValidator ID="rfvCodigoArt" runat="server" 
                                  ControlToValidate="txtCodigoArt" CssClass="failureNotification" 
                                  ErrorMessage="<%$ Resources:resCorpusCFDIEs, rfvCodigo %>" 
                                  ToolTip="<%$ Resources:resCorpusCFDIEs, rfvCodigo %>" 
                                  ValidationGroup="sucursalValidationArticulo" Width="16px">*</asp:RequiredFieldValidator>
                                  
                          </td>
                          <td width="185">
                              <asp:TextBox ID="txtMedidaArt" runat="server" CssClass="fontText" TabIndex="41" 
                                  Width="60px"></asp:TextBox>
                          </td>
                          <td width="185">
                              <asp:RequiredFieldValidator ID="rfvUnidadArt" runat="server" 
                                  ControlToValidate="txtMedidaArt" CssClass="failureNotification" 
                                  ErrorMessage="<%$ Resources:resCorpusCFDIEs, rfvUnidad %>" 
                                  ToolTip="<%$ Resources:resCorpusCFDIEs, rfvUnidad %>" 
                                  ValidationGroup="sucursalValidationArticulo" Width="16px">*</asp:RequiredFieldValidator>
                          </td>
                          <td width="185" rowspan="4" style="height:80px">
                              <asp:TextBox ID="txtDescripcionArt" runat="server" CssClass="fontText" 
                                  Height="140px" Width="248px" TabIndex="42" 
                                  TextMode="MultiLine"></asp:TextBox>

                          </td>
                          <td valign="top">
                              
                              <cc1:ModalPopupExtender ID="linkModalArt_ModalPopupExtender" runat="server" 
                                  BackgroundCssClass="modalBackground"  
                                  DynamicServicePath="" Enabled="True" PopupControlID="pnlArticulos" 
                                  TargetControlID="linkModalArt">
                              </cc1:ModalPopupExtender>
                              <asp:RequiredFieldValidator ID="revDescArt" runat="server" 
                                  ControlToValidate="txtDescripcionArt" CssClass="failureNotification" 
                                  ErrorMessage="<%$ Resources:resCorpusCFDIEs, revDesc %>" 
                                  ToolTip="<%$ Resources:resCorpusCFDIEs, revDesc %>" 
                                  ValidationGroup="sucursalValidationArticulo"><img 
                                src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                               </td>
                          <td>
                                 <table>
                                    <tr>
                                        <td valign="top">
                                            <asp:TextBox ID="txtPrecioArt" runat="server" 
                                                CssClass="fontText" 
                                                TabIndex="43" Width="63px">0</asp:TextBox>
                                        </td>
                                        <td>
                                        
                                            <asp:RequiredFieldValidator ID="rfPrecioArt" runat="server" 
                                                ControlToValidate="txtPrecioArt" CssClass="failureNotification" 
                                                Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, revPrecio %>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, revPrecio %>" 
                                                ValidationGroup="sucursalValidationArticulo"> <img src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="regxPrecioArt" runat="server" 
                                                ControlToValidate="txtPrecioArt" CssClass="failureNotification" 
                                                Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ValidationExpression="^([+-?\d+[0-9]{1,9})(\.[0-9]{1,6})?$" 
                                                ValidationGroup="sucursalValidationArticulo"> <img src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                        
                                        </td>
                                    </tr>
                                    <tr style="height: 40px" valign="bottom">
                                        <td>
                                            <asp:Label ID="lblISRArt" runat="server" CssClass="fontText" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblIsr %>"></asp:Label>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="bottom">
                                            <asp:TextBox ID="txtISRArt" runat="server" CssClass="fontText" 
                                                TabIndex="48" Width="63px">0</asp:TextBox>
                                        </td>
                                        <td>
                                        
                                            <asp:RegularExpressionValidator ID="regISRart" runat="server" 
                                                ControlToValidate="txtISRArt" CssClass="failureNotification" Display="Dynamic" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                ValidationGroup="sucursalValidationArticulo" Width="16px"><img 
                                src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                        
                                        </td>
                                    </tr>
                                </table>
                          </td>
                          <td>
                                 <table>
                                    <tr>
                                        <td valign="top">
                                            <asp:DropDownList ID="ddlMonedaArt" runat="server" CssClass="fontText" 
                                                TabIndex="44" Width="63px">
                                                <asp:ListItem Selected="True">MXN</asp:ListItem>
                                                <asp:ListItem>USD</asp:ListItem>
                                                <asp:ListItem>EUR</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 42px" valign="bottom">
                                        <td>
                                            <asp:Label ID="lblIvaRetArt" runat="server" CssClass="fontText" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblIvaRetenido %>"></asp:Label>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="bottom">                                        
                                            <asp:TextBox ID="txtIvaRetArt" runat="server" CssClass="fontText" TabIndex="49" 
                                                Width="63px">0</asp:TextBox>                                        
                                        </td>
                                        <td>
                                        
                                            <asp:RegularExpressionValidator ID="regIvaRetArt" runat="server" 
                                                ControlToValidate="txtIvaRetArt" CssClass="failureNotification" Display="Dynamic" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                ValidationGroup="sucursalValidationArticulo"><img 
                                src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                        
                                        </td>
                                    </tr>
                                </table>
                          </td>
                          <td style="width:100px">
                                <table>
                                    <tr>
                                        <td valign="top">
                                            <asp:DropDownList ID="ddlIVAArt" runat="server" CssClass="fontText" 
                                                TabIndex="45" Width="69px" DataTextField="DesIva" DataValueField="ValIva">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr style="height: 42px" valign="bottom">
                                        <td>
                                             <asp:Label ID="lblConceptoDescuento" runat="server" CssClass="fontText" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblDescuento %>">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="bottom">
                                            <asp:TextBox ID="txtDescuentoArt" runat="server" CssClass="fontText" 
                                                TabIndex="51" Width="54px">0
                                            </asp:TextBox>                                      
                                            <asp:RegularExpressionValidator ID="regxDescuentoArticulo" runat="server" 
                                                ControlToValidate="txtDescuentoArt" CssClass="failureNotification" 
                                                Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                ValidationGroup="sucursalValidationArticulo">
                                                <img alt="" src="../Imagenes/error_sign.gif" /> 
                                            </asp:RegularExpressionValidator>       
                                        </td>
                                    </tr>
                                </table>
                          </td>
                          <td>
                                 <table>
                                    <tr>
                                        <td valign="top">
                                             <asp:TextBox ID="txtCantidadArt" runat="server" 
                                                CssClass="fontText" TabIndex="46" Width="54px">0</asp:TextBox>                                     
                                        </td>
                                    </tr>
                                    <tr style="height: 40px" valign="bottom">
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="bottom">         
                                              &nbsp;</td>
                                    </tr>
                                </table>
                          </td>
                          <td>
                                 <table style="height:100px">
                                    <tr>
                                        <td valign="top">
                                            <asp:RequiredFieldValidator ID="rfCantidadArt" runat="server" 
                                                ControlToValidate="txtCantidadArt" CssClass="failureNotification" 
                                                Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                                ValidationGroup="sucursalValidationArticulo"> <img src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="regxCantidadArt" runat="server" 
                                                ControlToValidate="txtCantidadArt" CssClass="failureNotification" 
                                                Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                ValidationGroup="sucursalValidationArticulo"> <img src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="bottom" style="height:33px">

                                        </td>
                                    </tr>
                                </table>
                          </td>
                          <td>
                            <table style="height:100px">
                                <tr>
                                    <td valign="top">
                                        <asp:TextBox ID="txtImporteArt" runat="server" CssClass="fontText" 
                                            Enabled="False" ReadOnly="True" TabIndex="47" Width="55px">0
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="height:45px">
                                    <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        
                                    </td>
                                </tr>
                            </table>
                          </td>
                      </tr>
                      <tr valign="top">
                          <td width="185">
                              &nbsp;</td>
                          <td width="185">
                              &nbsp;</td>
                          <td width="185">
                              &nbsp;</td>
                          <td width="185">
                              &nbsp;</td>
                          <td valign="top">
                              &nbsp;</td>
                          <td valign="bottom">
                              
                          </td>
                          <td valign="bottom">
                              
                          </td>
                          <td align="left" colspan="3" valign="bottom">
                              &nbsp;</td>
                          <td align="center" valign="bottom">
                              &nbsp;</td>
                      </tr>
                      <tr valign="top">
                          <td width="185">
                              &nbsp;</td>
                          <td width="185">
                              &nbsp;</td>
                          <td width="185">
                              &nbsp;</td>
                          <td width="185">
                              &nbsp;</td>
                          <td valign="top">
                              &nbsp;</td>
                          <td>

                          </td>
                          <td>

                          </td>
                          <td align="center" colspan="3" valign="bottom">
                              
                          </td>
                          <td align="center" valign="bottom">
                              &nbsp;</td>
                      </tr>
                      <tr valign="top">
                          <td width="185">
                              &nbsp;</td>
                          <td width="185">
                              &nbsp;</td>
                          <td width="185">
                              &nbsp;</td>
                          <td width="185">
                              &nbsp;</td>
                          <td valign="top">
                              &nbsp;</td>
                          <td colspan="2">
                              
                          </td>
                          <td align="center" valign="bottom">
                              &nbsp;</td>
                          <td align="center" colspan="3" valign="bottom">
                            <asp:CheckBox ID="cbAgrCatArt" runat="server" CssClass="fontText" TabIndex="52" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblAgrArtCat %>" />
                          </td>
                      </tr>
                  </table>
                  <table>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" Width="900px"  Height="23px" CssClass="collapsePanelHeader">
                                <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                                    <div style="float: left; margin-left: 20px;">
                                        <asp:Label ID="Label2" Text="<%$ Resources:resCorpusCFDIEs, lblIeps %>" runat="server"></asp:Label>
                                    </div>
                                    <div style="float: right; vertical-align: middle;">
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Imagenes/expand_blue.jpg"  Visible="True"/>
                                    </div>
                                </div>
                            </asp:Panel>

                            <cc1:CollapsiblePanelExtender ID="cpeIEPS" runat="server" 
                                TargetControlID="Panel2"
                                ExpandControlID="Panel1"
                                CollapseControlID="Panel1" 
                                TextLabelID="Label47"
                                ImageControlID="Image2"    
                                ExpandedText=""
                                CollapsedText=""
                                ExpandedImage="~/Imagenes/collapse_blue.jpg"
                                CollapsedImage="~/Imagenes/expand_blue.jpg"
                                SuppressPostBack="true"
                                SkinID="CollapsiblePanelDemo" 
                                Enabled="true"
                                Collapsed = "true">
                            </cc1:CollapsiblePanelExtender>

                            <asp:Panel ID="Panel2" runat="server"  GroupingText="<%$ Resources:resCorpusCFDIEs, lblInformacionGeneral %>" Width="900px" 
                            BackColor="#F0F0F0" CssClass="fontText" ScrollBars="Auto">

                                <div>
                                    <table cellspacing="10">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label72" runat="server" CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblIeps %>">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlIEPS" runat="server" CssClass="fontText"
                                                TabIndex="0" Width="350px"  OnSelectedIndexChanged="ddlIEPS_SelectedIndexChanged" EnableViewState ="true" >
                                                 <%--Los siguiuentes opciones son con traslado !--%>
                                                <asp:ListItem Value = "0" Text = "Seleccione" Selected ="True"></asp:ListItem>
                                                <asp:ListItem Value = "1" Text = "Bebidas con contenido alcohólico y cerveza"></asp:ListItem>
                                                <asp:ListItem Value = "2" Text = "Bebidas energetizantes"></asp:ListItem>
                                                <asp:ListItem Value = "3" Text = "Bebidas saborizadas"></asp:ListItem>
                                                <asp:ListItem Value = "4" Text = "Plaguicidas"></asp:ListItem>
                                                <asp:ListItem Value = "5" Text = "Alimentos no básicos"></asp:ListItem>
                                                <%--Los siguiuentes opciones son sin traslado !--%>
                                                <asp:ListItem Value = "6" Text = "Alcohol, alcohol desnaturalizado y mieles incristalizables 50%"></asp:ListItem>
                                                <asp:ListItem Value = "7" Text = "Tabacos Labrados"></asp:ListItem>
                                                <asp:ListItem Value = "8" Text = "Gasolinas"></asp:ListItem>
                                                <asp:ListItem Value = "9" Text = "Diesel"></asp:ListItem>
                                                <asp:ListItem Value = "10" Text = "Combustibles fósiles"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvIEPS" runat="server" 
                                                ControlToValidate="ddlIEPS" CssClass="failureNotification" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valIEPS %>" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valIEPS %>" 
                                                ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblIeps %>">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIEPS" runat="server" CssClass="fontText" TabIndex="50" Width="100px">0
                                                </asp:TextBox>    
                                                <cc1:FilteredTextBoxExtender ID="fteIEPS" runat="server" FilterType="Numbers, Custom" TargetControlID="txtIEPS" ValidChars=".,%">
                                                </cc1:FilteredTextBoxExtender>
                                                 <asp:RegularExpressionValidator ID="revIEPS" runat="server" 
                                                    ControlToValidate="txtIEPS" CssClass="failureNotification" Display="Dynamic" 
                                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                    ValidationGroup="sucursalValidationArticulo">
                                                    <img alt="" src="../Imagenes/error_sign.gif" /> 
                                                </asp:RegularExpressionValidator> 
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:CheckBox ID="cbIEPS" runat="server" Checked="false" CssClass="fontText" TextAlign="Left" TabIndex="50" Text="<%$ Resources:resCorpusCFDIEs, lblImporte %>">
                                                </asp:CheckBox> 
                                            </td>
                                        </tr>
                                    </table>    
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Panel ID="pnlExtenderImp" runat="server" Width="900px"  Height="23px" CssClass="collapsePanelHeader">
                             <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                
                              <div style="float: left; margin-left: 20px;">
                                <asp:Label ID="lblImpuestosCompl" 
                                      Text="<%$ Resources:resCorpusCFDIEs, lblTitImpCompl %>" runat="server"></asp:Label>
                              </div>
                              <div style="float: right; vertical-align: middle;">
                                <asp:ImageButton ID="Imagen2" runat="server" 
                                    ImageUrl="~/Imagenes/expand_blue.jpg" AlternateText="(Expedido En...)" Visible="True"/>
                              </div>
                           </div>
                        </asp:Panel>

          <cc1:CollapsiblePanelExtender ID="cpeImpuestosCompl" runat="server" 
                    TargetControlID="pnlImpuestosCompl"
                    ExpandControlID="pnlExtenderImp"
                    CollapseControlID="pnlExtenderImp" 
                    TextLabelID="Label47"
                    ImageControlID="Image2"    
                    ExpandedText=""
                    CollapsedText=""
                    ExpandedImage="~/Imagenes/collapse_blue.jpg"
                    CollapsedImage="~/Imagenes/expand_blue.jpg"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanelDemo" Enabled="true">
         </cc1:CollapsiblePanelExtender>

                        <asp:Panel ID="pnlImpuestosCompl" runat="server"  
                                GroupingText="<%$ Resources:resCorpusCFDIEs, lblTitImpCompl %>" Width="900px" 
                                BackColor="#F0F0F0" CssClass="fontText" ScrollBars="Auto">
                                <div align="left">
                                <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkAgregarImporte" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblAgregarImporte %>" 
                                            Checked="false" OnCheckedChanged="chkAgregarImporte_CheckedChanged" 
                                            AutoPostBack="True"/>
                                    </td>
                                    <td>
                                    
                                    </td>
                                    <td>
                                    
                                    </td>
                                    <td>
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblImpLoc" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblImpCompl %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblImpTasa" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTasa %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblImpImpo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblImpImporte %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblImpTipo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTipo %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtImpLoc" runat="server" CssClass="fontText"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="revImporteLoc" runat="server" 
                                            ControlToValidate="txtImpLoc" CssClass="failureNotification" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, revImporteLoc %>" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, revImporteLoc %>" 
                                            ValidationGroup="validarImpCompl" Display="Dynamic">
                                            <img src="../Imagenes/error_sign.gif" />
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtImpTasa" runat="server" CssClass="fontText" Width="60px">0</asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                                ControlToValidate="txtImpTasa" CssClass="failureNotification" Display="Dynamic" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                ValidationGroup="validarImpCompl" Width="16px"> <img alt="" src="../Imagenes/error_sign.gif" />
                                        </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvImpTasa" runat="server" 
                                                ControlToValidate="txtImpTasa" CssClass="failureNotification" 
                                                Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, valTasaReq %>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valTasaReq %>" 
                                                ValidationGroup="validarImpCompl"> <img src="../Imagenes/error_sign.gif" />
                                        </asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="ftbeImpTasa" runat="server" 
                                            FilterType="Numbers, Custom" TargetControlID="txtImpTasa" ValidChars=".">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtImpImporte" runat="server" Enabled="false" CssClass="fontText" Text="0">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvImpImporte" runat="server" 
                                            ControlToValidate="txtImpImporte" CssClass="failureNotification" 
                                            Display="Dynamic"
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, varImpImpoReq %>"
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, varImpImpoReq %>" 
                                            ValidationGroup="validarImpCompl"> 
                                            <img src="../Imagenes/error_sign.gif" />
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revImpImporte" runat="server" 
                                            ControlToValidate="txtImpImporte" CssClass="failureNotification" Display="Dynamic" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                            ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                            ValidationGroup="validarImpCompl" Width="16px"> 
                                            <img alt="" src="../Imagenes/error_sign.gif" />
                                        </asp:RegularExpressionValidator>
                                        <cc1:FilteredTextBoxExtender ID="ftbeImpImporte" runat="server" 
                                            FilterType="Numbers, Custom" TargetControlID="txtImpImporte" ValidChars=".">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlImpTipo" runat="server" CssClass="fontText">
                                            <asp:ListItem Value="T">Traslado</asp:ListItem>
                                            <asp:ListItem Value="R">Retención</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAgrImpCompl" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" CssClass="botonChico" 
                                            Height="20px" onclick="btnAgrImpCompl_Click" Width="80px" 
                                            ValidationGroup="validarImpCompl" />
                                            
                                    </td>
                                    <td>
                                    <asp:ValidationSummary ID="vsImpCompl" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, varValidacionError %>" 
                             ValidationGroup="validarImpCompl" ShowMessageBox="True" ShowSummary="False" />
                                    </td>
                                </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView ID="grvImpuestoCompl" runat="server" AutoGenerateColumns="False" 
                                                BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                                                DataKeyNames="id_Impuesto" onrowdatabound="grvImpuestoCompl_RowDataBound" 
                                                onrowdeleting="grvImpuestoCompl_RowDeleting" 
                                                onselectedindexchanged="grvImpuestoCompl_SelectedIndexChanged" 
                                                Width="100%">
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
                                                    <asp:BoundField DataField="id_Registros" HeaderText="id_Registros" 
                                                        Visible="False" />
                                                    <asp:BoundField DataField="Descripcion" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblImpCompl %>" />
                                                    <asp:BoundField DataField="Tasa" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblTasa %>" />
                                                    <asp:BoundField DataField="Importe" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblImporte %>" />
                                                    <asp:BoundField DataField="Tipo" 
                                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblTipo %>">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    No hay Impuestos Capturados Para el Artículo
                                                </EmptyDataTemplate>
                                                <HeaderStyle BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                                            </asp:GridView>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                                </div>
                        </asp:Panel>

                        </td>
                    </tr>
                  </table>
                  <%--Tabla de informacion Aduanal--%>
                 <table>
                    <tr>
                        <td>                <%--panelExtender--%>
                           <asp:Panel ID="pnlExtenderInfAduanal" runat="server" Width="900px"  Height="23px" CssClass="collapsePanelHeader">
                                 <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                                    <div style="float: left; margin-left: 20px;">
                                        <asp:Label ID="lblInformacionAduanal" Text="<%$ Resources:resCorpusCFDIEs,lblTiInfoAduanal %>" runat="server"> </asp:Label>
                                    </div>
                                    <div style="float: right; vertical-align: middle;">
                                        <asp:ImageButton ID="Image3" runat="server" 
                                            ImageUrl="~/Imagenes/expand_blue.jpg" AlternateText="(Expedido En...)" Visible="True"/>
                                    </div>
                                  </div>
                            </asp:Panel>
                  <cc1:CollapsiblePanelExtender ID="cpeInformacionAduanal" runat="server" 
                            TargetControlID="pnlInformacionAduanal"
                            ExpandControlID="pnlExtenderInfAduanal"
                            CollapseControlID="pnlExtenderInfAduanal" 
                            TextLabelID="Label48"
                            ImageControlID="Image3"    
                            ExpandedText=""
                            CollapsedText=""
                            ExpandedImage="~/Imagenes/collapse_blue.jpg"
                            CollapsedImage="~/Imagenes/expand_blue.jpg"
                            SuppressPostBack="true"
                            SkinID="CollapsiblePanelDemo" Enabled="True">
                 </cc1:CollapsiblePanelExtender>

                        <asp:Panel ID="pnlInformacionAduanal" runat="server"  
                                GroupingText="<%$ Resources:resCorpusCFDIEs, lblTiInfoAduanal %>" Width="900px" 
                                BackColor="#F0F0F0" CssClass="fontText" ScrollBars="Auto">
                                <div align="left">
                                <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblInfAdu" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblAduana %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDocAdu" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDocumentoAduanal %>"></asp:Label>
                                    </td>  
                                    <td>
                                        </td> 
                                    <td>
                                        <asp:Label ID="lblFecha" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFecha %>"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtAduana" runat="server" CssClass="fontText"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="revNombreAduana" runat="server" 
                                            ControlToValidate="txtAduana" CssClass="failureNotification" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, revNombreAduana %>" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, revNombreAduana %>" 
                                            ValidationGroup="validarInfoAduanal" Display="Dynamic"><img 
                                src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDocAduana" runat="server" CssClass="fontText"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="revDocAduanal" runat="server" 
                                                ControlToValidate="txtDocAduana" CssClass="failureNotification" 
                                                Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, revDocAduanal %>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, revDocAduanal %>" 
                                                ValidationGroup="validarInfoAduanal"><img alt=""  src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                    </td>  
                                    <td>
                                        </td>
                                        <%--Fecha Aduana--%>
                                    <td>
                                        <asp:TextBox ID="txtFechaAduana" runat="server" BackColor="White" 
                                            Width="100px" ></asp:TextBox>
                                        <asp:Image ID="imgIni" runat="server" 
                                            ImageUrl="~/Imagenes/icono_calendario.gif" />
                                        <asp:RegularExpressionValidator ID="revFecha" runat="server" Display="Dynamic"
                                            ControlToValidate="txtFechaAduana" CssClass="failureNotification" 
                                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                            ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>"
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" > <img src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                        <cc1:CalendarExtender ID="txtFecha_CalendarExtender_Adu" runat="server" 
                                            Enabled="True" TargetControlID="txtFechaAduana" Format="dd/MM/yyyy" 
                                            PopupButtonID="imgIni">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvFecha" runat="server" 
                                            ControlToValidate="txtFechaAduana" CssClass="failureNotification" 
                                            ValidationGroup="grupoConsulta" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>"
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>"
                                             Width="16px" >*</asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAgegarInfoAduanal" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" CssClass="botonChico" 
                                            Height="20px"  Width="80px" 
                                            ValidationGroup="validarInfoAduanal" onclick="btnAgegarInfoAduanal_Click" />
                                    </td>
                                    <td>
                                        <asp:ValidationSummary ID="vsInfoAduanal" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, varValidacionError %>" 
                             ValidationGroup="validarInfoAduanal" ShowMessageBox="True" ShowSummary="False" />
                                    </td>
                                </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView ID="grvInfAduanal" runat="server" AutoGenerateColumns="False" 
                                                BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                                                Width="100%" DataKeyNames="id_aduana" 
                                                onrowdeleting="grvInfAduanal_RowDeleting" 
                                                onselectedindexchanged="grvInfAduanal_SelectedIndexChanged">
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
                                                    <asp:TemplateField HeaderText="id_registros" Visible="False">
                                                        <EditItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("id_registros") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("id_registros") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Aduana" ReadOnly="True" 
                                                     HeaderText ="<%$ Resources:resCorpusCFDIEs,lblNomAduana %>"/>
                                                    <asp:BoundField DataField="DocAduana" ReadOnly="True"
                                                     HeaderText = "<%$ Resources:resCorpusCFDIEs,lblNoDocAduanal %>"/>
                                                    <asp:BoundField DataField="FechaAduana" ReadOnly="True"
                                                     HeaderText ="<%$ Resources:resCorpusCFDIEs, lblFecha %>" 
                                                        DataFormatString="{0:d}"/>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    No hay Información aduanal Para el Artículo
                                                </EmptyDataTemplate>
                                                <HeaderStyle BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                                            </asp:GridView>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                                </div>
                        </asp:Panel>
                       </td>
                    </tr>
                  </table>
                  <%--fin tabla de Informacion Aduanal--%>

                  <%-- Tabla complemento terceros --%>
                  <table>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlExtenderTerceros" runat="server" Width="900px"  Height="23px" CssClass="collapsePanelHeader">
                                <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                
                                    <div style="float: left; margin-left: 20px;">
                                        <asp:Label ID="lblComplTerceros" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblTitComplTerceros %>" runat="server"></asp:Label>
                                    </div>
                                    <div style="float: right; vertical-align: middle;">
                                <asp:ImageButton ID="btnImgComplTerceros" runat="server" 
                                    ImageUrl="~/Imagenes/expand_blue.jpg" AlternateText="(Complemento Concepto Terceros...)" Visible="True"/>
                                    </div>
                                </div>
                            </asp:Panel>

                              <cc1:CollapsiblePanelExtender ID="cpeComplementoTerceros" runat="server" 
                                        TargetControlID="pnlComplTerceros"
                                        ExpandControlID="pnlExtenderTerceros"
                                        CollapseControlID="pnlExtenderTerceros" 
                                        TextLabelID="Label49"
                                        ImageControlID="btnImgComplTerceros"    
                                        ExpandedText=""
                                        CollapsedText=""
                                        ExpandedImage="~/Imagenes/collapse_blue.jpg"
                                        CollapsedImage="~/Imagenes/expand_blue.jpg"
                                        SuppressPostBack="true"
                                        SkinID="CollapsiblePanelDemo" Enabled="true">
                             </cc1:CollapsiblePanelExtender>

                            <asp:Panel ID="pnlComplTerceros" runat="server"  
                                    GroupingText="<%$ Resources:resCorpusCFDIEs, lblTitComplTerceros %>" Width="900px" 
                                    BackColor="#F0F0F0" CssClass="fontText" ScrollBars="Auto">
                                    <div align="left">
                                    <table>
                                      <tr>
                                          <td valign="top" style="width:430px">
                                              <fieldset style="width: 430px; height: 290px">
                                                  <legend>
                                                      <asp:Literal ID="Literal9" runat="server" Text="Información General"></asp:Literal>
                                                  </legend>
                                                  <table>
                                                      <tr>
                                                          <td align="left" class="style2">
                                                              <asp:Label ID="Label141" runat="server" CssClass="fontText" 
                                                                  Text="<%$ Resources:resCorpusCFDIEs, lblVersion %>"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              </td>
                                                          <td align="left">
                                                              <asp:Label ID="Label142" runat="server" CssClass="fontText" Text="RFC"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              <asp:Label ID="Label168" runat="server" CssClass="fontText" Text="Nombre o Razón Social"></asp:Label>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:TextBox ID="tbVersionTerceros" runat="server" CssClass="fontText" 
                                                                  ReadOnly="True" Width="50px">1.1</asp:TextBox>
                                                          </td>
                                                          <td align="left">
                                                             </td>
                                                          <td align="left">
                                                              <asp:TextBox ID="tbRFCTerceros" runat="server" CssClass="fontText"
                                                                  Width="150px"></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfNoAutDon30" runat="server"
                                                                  ControlToValidate="tbRFCTerceros" CssClass="failureNotification"
                                                                  ErrorMessage="RFC Requerido" Height="22px" ToolTip="RFC Requerido" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                              <asp:RegularExpressionValidator ID="regxRFC6" runat="server" 
                                                                  ControlToValidate="tbRFCTerceros" CssClass="failureNotification" 
                                                                  Height="22px" ToolTip="RFC incompleto" 
                                                                  ErrorMessage="RFC incompleto" 
                                                                  ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RegularExpressionValidator>
                                                          </td>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtnombreT" runat="server" CssClass="fontText" 
                                                                  Width="150px"></asp:TextBox>
                                                          </td>
                                                      </tr>
                                                       <tr>
                                                          <td align="left">
                                                              <asp:Label ID="Label154" runat="server" CssClass="fontText">Impuesto<br />Traslado</asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              </td>
                                                          <td align="left">
                                                              <asp:Label ID="Label153" runat="server" CssClass="fontText" 
                                                                  Text="Importe Traslado"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              <asp:Label ID="Label155" runat="server" CssClass="fontText" Text="Tasa"></asp:Label>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:CheckBox ID="cbIVATra" runat="server" Checked="True"
                                                                  Text="IVA"
                                                                  oncheckedchanged="cbIVATra_CheckedChanged" AutoPostBack="True" />
                                                          </td>
                                                          <td align="left">
                                                              </td>
                                                          <td align="left">
                                                              <asp:TextBox ID="tbimporteIVATra" runat="server" CssClass="fontText" 
                                                                  Width="150px">0</asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfImpIvaTraReq" runat="server"
                                                                  ControlToValidate="tbimporteIVATra" CssClass="failureNotification" 
                                                                  ErrorMessage="Importe Requerido" Height="22px" ToolTip="Importe requerido." 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                              <asp:RegularExpressionValidator ID="rvfImpIvaTraNum" runat="server" 
                                                                  ControlToValidate="tbimporteIVATra" CssClass="failureNotification"
                                                                  ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                  ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                  ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px"><img 
                                                                  src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                                          </td>
                                                          <td align="left">
                                                              <asp:TextBox ID="tbtasaIVATra" runat="server" CssClass="fontText"
                                                                  Width="75px">0</asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvftasaIvaReq" runat="server"
                                                                  ControlToValidate="tbtasaIVATra" CssClass="failureNotification" 
                                                                  ErrorMessage="Tasa requerida" Height="22px" ToolTip="Tasa requerida." 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                              <asp:RegularExpressionValidator ID="rvftasaIvaNum" runat="server" 
                                                                  ControlToValidate="tbtasaIVATra" CssClass="failureNotification"
                                                                  ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                  ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                  ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px"><img 
                                                                  src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:CheckBox ID="cbIEPSTra" runat="server" Text="IEPS" 
                                                                  oncheckedchanged="cbIEPSTra_CheckedChanged" AutoPostBack="True" />
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:TextBox ID="tbimporteIEPSTra" runat="server" CssClass="fontText" 
                                                                   Width="150px">0</asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfImpIEPSTraReq" runat="server"
                                                                  ControlToValidate="tbimporteIEPSTra" CssClass="failureNotification" 
                                                                  ErrorMessage="Importe Requerido" Height="22px" ToolTip="Importe requerido." 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                              <asp:RegularExpressionValidator ID="rvfImpIEPSTraNum" runat="server" 
                                                                  ControlToValidate="tbimporteIEPSTra" CssClass="failureNotification"
                                                                  ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                  ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                  ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px"><img 
                                                                  src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>

                                                          </td>
                                                          <td align="left">
                                                              <asp:TextBox ID="tbtasaIEPSTra" runat="server" CssClass="fontText" 
                                                                  Width="75px">0</asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvftasaIEPSReq" runat="server"
                                                                  ControlToValidate="tbtasaIEPSTra" CssClass="failureNotification" 
                                                                  ErrorMessage="Tasa requerida" Height="22px" ToolTip="Tasa requerida." 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                              <asp:RegularExpressionValidator ID="rvftasaIEPSNum" runat="server" 
                                                                  ControlToValidate="tbtasaIEPSTra" CssClass="failureNotification"
                                                                  ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                  ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                  ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px"><img 
                                                                  src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left" class="style2">
                                                              <asp:Label ID="Label101" runat="server" CssClass="fontText">Impuesto<br />Retención</asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              </td>
                                                          <td align="left">
                                                              <asp:Label ID="Label100" runat="server" CssClass="fontText" 
                                                                  Text="Importe Retención"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:CheckBox ID="cbIVARet" runat="server"
                                                                  oncheckedchanged="cbIVARet_CheckedChanged" AutoPostBack="True" Text="IVA"/>
                                                          </td>
                                                          <td align="left">
                                                              </td>
                                                          <td align="left">
                                                              <asp:TextBox ID="tbimporterIVARet" runat="server" CssClass="fontText" 
                                                                   Width="150px">0</asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfImpIvaRetReq" runat="server"
                                                                  ControlToValidate="tbimporterIVARet" CssClass="failureNotification" 
                                                                  ErrorMessage="Importe Requerido" Height="22px" ToolTip="Importe requerido." 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                              <asp:RegularExpressionValidator ID="rvfImpIvaRetNum" runat="server" 
                                                                  ControlToValidate="tbimporterIVARet" CssClass="failureNotification"
                                                                  ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                  ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                  ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px"><img 
                                                                  src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:CheckBox ID="cbISRRet" runat="server" Text="ISR" 
                                                                  oncheckedchanged="cbISRRet_CheckedChanged" AutoPostBack="True" />
                                                          </td>
                                                          <td align="left">
                                                             </td>
                                                          <td align="left">
                                                              <asp:TextBox ID="tbimporteISRRet" runat="server" CssClass="fontText" 
                                                                   Width="150px">0</asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfImpRetISR" runat="server"
                                                                  ControlToValidate="tbimporteISRRet" CssClass="failureNotification" 
                                                                  ErrorMessage="Importe Requerido" Height="22px" ToolTip="Importe requerido." 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                              <asp:RegularExpressionValidator ID="rvfImpRetISRNum" runat="server" 
                                                                  ControlToValidate="tbimporteISRRet" CssClass="failureNotification"
                                                                  ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                  ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                  ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px"><img 
                                                                  src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                      </tr>
                                                  </table>
                                              </fieldset>
                                              
                                              </td>
                                          <td>
                                              </td>
                                          <td valign="top"  style="width: 380px">
                                              <fieldset style="height: 290px; width: 380px">
                                                  <legend>
                                                      <asp:Literal ID="Literal10" runat="server" Text="Información Fiscal Tercero"></asp:Literal>
                                                  </legend>
                                                  <table>
                                                      <tr>
                                                        <td colspan="3" align="left">
                                                            <asp:CheckBox ID="cbInfoFiscalT" runat="server" AutoPostBack="true" 
                                                                Checked="false" oncheckedchanged="cbInfoFiscalT_CheckedChanged" 
                                                                Text="Agregar información fiscal" />
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:Label ID="Label157" runat="server" CssClass="fontText" Text="Calle"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:Label ID="Label165" runat="server" CssClass="fontText" Text="Localidad"></asp:Label>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtcalleT" runat="server"></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfNoAutDon14" runat="server" 
                                                                  ControlToValidate="txtcalleT" CssClass="failureNotification" 
                                                                  ErrorMessage="Calle Requerida" Height="22px" ToolTip="Calle requerida." 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtLocalidadT" runat="server"></asp:TextBox>
                                                              <%--<asp:RequiredFieldValidator ID="rvfNoAutDon20" runat="server" 
                                                                  ControlToValidate="txtLocalidadT" CssClass="failureNotification" 
                                                                  ErrorMessage="Localidad Requerida" Height="22px" ToolTip="Localidad Requerida" 
                                                                  ValidationGroup="TercerosInfoFiscal" Width="16px">*</asp:RequiredFieldValidator>--%>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:Label ID="Label159" runat="server" CssClass="fontText" 
                                                                  Text="Número Exterior"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:Label ID="Label160" runat="server" CssClass="fontText" Text="Municipio"></asp:Label>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtNumExtT" runat="server"></asp:TextBox>
                                                              <%--<asp:RequiredFieldValidator ID="rvfNoAutDon18" runat="server" 
                                                                  ControlToValidate="txtNumExtT" CssClass="failureNotification" 
                                                                  ErrorMessage="Numero Requerido" Height="22px" ToolTip="Numero requerido" 
                                                                  ValidationGroup="TercerosInfoFiscal" Width="16px">*</asp:RequiredFieldValidator>--%>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtMunicipioT" runat="server" ></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfNoAutDon19" runat="server" 
                                                                  ControlToValidate="txtMunicipioT" CssClass="failureNotification" 
                                                                  ErrorMessage="Municipio requerido" Height="22px" ToolTip="Municipio requerido" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:Label ID="Label161" runat="server" CssClass="fontText" 
                                                                  Text="Número Interior"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;
                                                              <br />
                                                          </td>
                                                          <td align="left">
                                                              <asp:Label ID="Label162" runat="server" CssClass="fontText" Text="Estado"></asp:Label>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtNumIntT" runat="server"></asp:TextBox>
                                                              <%--<asp:RequiredFieldValidator ID="rvfNoAutDon21" runat="server" 
                                                                  ControlToValidate="txtNumIntT" CssClass="failureNotification" 
                                                                  ErrorMessage="Numero Requerido" Height="22px" ToolTip="Numero Requerido" 
                                                                  ValidationGroup="TercerosInfoFiscal" Width="16px">*</asp:RequiredFieldValidator>--%>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtEstadoT" runat="server"></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfNoAutDon22" runat="server" 
                                                                  ControlToValidate="txtEstadoT" CssClass="failureNotification" 
                                                                  ErrorMessage="Estado requerido" Height="22px" ToolTip="Estado requerido" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:Label ID="Label163" runat="server" CssClass="fontText" Text="Colonia"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:Label ID="Label164" runat="server" CssClass="fontText" Text="País"></asp:Label>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtColoniaT" runat="server" ></asp:TextBox>
                                                              <%--<asp:RequiredFieldValidator ID="rvfNoAutDon17" runat="server" 
                                                                  ControlToValidate="txtColoniaT" CssClass="failureNotification" 
                                                                  ErrorMessage="Colonia Requerida" Height="22px" ToolTip="Colonia requerida." 
                                                                  ValidationGroup="TercerosInfoFiscal" Width="16px">*</asp:RequiredFieldValidator>--%>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtPaisT" runat="server"></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfNoAutDon23" runat="server" 
                                                                  ControlToValidate="txtPaisT" CssClass="failureNotification" 
                                                                  ErrorMessage="Pais requerido" Height="22px" ToolTip="Pais requerido" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                             
                                                              <asp:Label ID="Label158" runat="server" CssClass="fontText" Text="Referencia"></asp:Label>
                                                             
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:Label ID="Label166" runat="server" CssClass="fontText" 
                                                                  Text="Código Postal"></asp:Label>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              
                                                              <asp:TextBox ID="txtReferenciaT" runat="server"></asp:TextBox>
                                                              <%--<asp:RequiredFieldValidator ID="rvfNoAutDon16" runat="server" 
                                                                  ControlToValidate="txtReferenciaT" CssClass="failureNotification" 
                                                                  ErrorMessage="Referencia Requerida" Height="22px" 
                                                                  ToolTip="Referencia requerida." ValidationGroup="TercerosInfoFiscal" Width="16px">*</asp:RequiredFieldValidator>--%>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtCodigoT" runat="server" MaxLength="5"></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rfvCodigoPostal" runat="server" 
                                                                  ControlToValidate="txtCodigoT" CssClass="failureNotification" Display="Dynamic" 
                                                                  Height="22px" ToolTip="<%$ Resources:resCorpusCFDIEs, valCodigoPostal %>" 
                                                                  ErrorMessage="<%$ Resources:resCorpusCFDIEs, valCodigoPostal %>" 
                                                                  ValidationGroup="TercerosGuardar"><img 
                                                                  src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                                              <cc1:FilteredTextBoxExtender ID="filterCP" runat="server" FilterType="Numbers" 
                                                                    TargetControlID="txtCodigoT">
                                                              </cc1:FilteredTextBoxExtender>
                                                          </td>
                                                      </tr>
                                                  </table>
                                              </fieldset>
    <%--                                          <fieldset class="register" style="width: 600px; height: 275px;">
                                                  <legend>
                                                      <asp:Literal ID="Literal6" runat="server" Text="Parte" Visible="False"></asp:Literal>
                                                  </legend>--%>
                                                  
                                                  <%--<table style="visibility:hidden;">
                                                      <tr>
                                                          <td align="left">
                                                              <asp:Label ID="Label136" runat="server" CssClass="fontText" Text="Cantidad" 
                                                                  Visible="False"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left" valign="top">
                                                              <asp:TextBox ID="txtCantidadT" runat="server" Width="50px" TabIndex="22" 
                                                                  Visible="False"></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfNoAutDon27" runat="server" 
                                                                  ControlToValidate="txtCantidadT" CssClass="failureNotification" 
                                                                  ErrorMessage="Cantidad requerida" Height="22px" 
                                                                  ToolTip="Cantidad requerida" Width="16px">*</asp:RequiredFieldValidator>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left" class="style2">
                                                              <asp:Label ID="Label138" runat="server" CssClass="fontText" Text="Descripción" 
                                                                  Visible="False"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtDescripcionT" runat="server" Height="58px" 
                                                                  TextMode="MultiLine" Width="300px" TabIndex="23" Visible="False"></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfNoAutDon28" runat="server" 
                                                                  ControlToValidate="txtDescripcionT" CssClass="failureNotification" 
                                                                  ErrorMessage="Descripción requerida" Height="22px" 
                                                                  ToolTip="Descripción requerida" Width="16px">*</asp:RequiredFieldValidator>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                      </tr>
                                                  </table>--%>
                                          <%--    </fieldset>--%>
                                              </td>
                                      </tr>
                                      <tr>
                                        <td colspan="3">
                                            <fieldset style="width:840px">    
                                                  <legend>
                                                      <asp:Literal ID="Literal4" runat="server" Text="Otra información"></asp:Literal>
                                                  </legend>
                                                  <asp:RadioButtonList ID="rblComplTerceros" runat="server" 
                                                        RepeatDirection="Horizontal" AutoPostBack="true" Width="400px"
                                                      onselectedindexchanged="rblComplTerceros_SelectedIndexChanged">
                                                        <asp:ListItem Text="Ninguno" Value="0" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Información Aduanera" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Cuenta Predial" Value="2"></asp:ListItem>
                                                  </asp:RadioButtonList>
                                                  <asp:Panel ID="pnlInfoAduaneraT" runat="server" Visible="false">
                                                     <fieldset class="register" style="width:600px">
                                                  <legend>
                                                      <asp:Literal ID="Literal5" runat="server" Text="Información Aduanera"></asp:Literal>
                                                  </legend>
                                                  <table>
                                                      <tr>
                                                          <td align="left" class="style2">
                                                              <asp:Label ID="Label135" runat="server" CssClass="fontText" Text="Aduana"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:Label ID="Label133" runat="server" CssClass="fontText" Text="Documento Aduanal"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:Label ID="Label134" runat="server" CssClass="fontText" Text="Fecha"></asp:Label>
                                                          </td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtaduanaT" runat="server"></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfAduanaT" runat="server" 
                                                                  ControlToValidate="txtaduanaT" CssClass="failureNotification" 
                                                                  ErrorMessage="Aduana requerida" Height="22px" ToolTip="Número requerido" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px" Enabled="false">*</asp:RequiredFieldValidator>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtnumeroT" runat="server"></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfNoAutDon24" runat="server" 
                                                                  ControlToValidate="txtnumeroT" CssClass="failureNotification" 
                                                                  ErrorMessage="Número requerido" Height="22px" ToolTip="Número requerido" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              <asp:TextBox ID="txtFechaIniT" runat="server" BackColor="White" 
                                                                  CssClass="fontText" Width="100px"></asp:TextBox>
                                                              <cc1:CalendarExtender ID="txtFechaIniT_CalendarExtender" runat="server" 
                                                                  Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgIni0" 
                                                                  TargetControlID="txtFechaIniT">
                                                              </cc1:CalendarExtender>
                                                              <asp:Image ID="imgIni0" runat="server" 
                                                                  ImageUrl="~/Imagenes/icono_calendario.gif" />
                                                              <asp:RegularExpressionValidator ID="revFechaIni0" runat="server" 
                                                                  ControlToValidate="txtFechaIniT" CssClass="failureNotification" 
                                                                  Display="Dynamic" Height="16px" 
                                                                  ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                                                  ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                                                  ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RegularExpressionValidator>
                                                              <asp:RequiredFieldValidator ID="rfvFechaIni4" runat="server" 
                                                                  ControlToValidate="txtFechaIniT" CssClass="failureNotification" Height="16px" 
                                                                  ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" 
                                                                  ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" 
                                                                  ValidationGroup="TercerosGuardar" Width="16px">*</asp:RequiredFieldValidator>
                                                          </td>                                                 
                                                      </tr>
                                                      <tr>
                                                          <td align="left">
                                                              &nbsp;
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                      </tr>
                                                  </table>
                                              </fieldset>
                                                  </asp:Panel>
                                                  <asp:Panel ID="pnlCtaPredialT" runat="server" Visible="false">
                                                    <fieldset class="register" style="width:430px;">
                                                  <legend>
                                                      <asp:Literal ID="Literal11" runat="server" Text="Cuenta Predial"></asp:Literal>
                                                  </legend>
                                                  <table>
                                                      <tr>
                                                          <td align="left" class="style2">
                                                              <asp:Label ID="Label167" runat="server" CssClass="fontText" Text="Número"></asp:Label>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left" valign="top">
                                                              <asp:TextBox ID="txtPredialT" runat="server" Width="150px"></asp:TextBox>
                                                              <asp:RequiredFieldValidator ID="rvfNoAutDon29" runat="server" 
                                                                  ControlToValidate="txtPredialT" CssClass="failureNotification" 
                                                                  ErrorMessage="Cuenta requerida" Height="22px" ToolTip="Cuenta requerida" 
                                                                  Width="16px" ValidationGroup="TercerosGuardar">*</asp:RequiredFieldValidator>
                                                          </td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                      </tr>
                                                      <tr>
                                                          <td align="left" class="style2">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                          <td align="left">
                                                              &nbsp;</td>
                                                      </tr>
                                                  </table>
                                              </fieldset>
                                                  </asp:Panel>
                                              </fieldset>
                                        </td>
                                      </tr>
                                      <tr>
                                          <td class="style6" valign="top">
                                              
                                              &nbsp;</td>
                                          <td>
                                              &nbsp;</td>
                                          <td align="right">
                                          <asp:ValidationSummary ID="vsTercerosGuardar" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, varValidacionError %>" 
                             ValidationGroup="TercerosGuardar" ShowMessageBox="True" ShowSummary="False" />
                                              <asp:Button ID="btnAgregarTerceros" runat="server" CssClass="botonChico" 
                                                  Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>"
                                                  ValidationGroup="TercerosGuardar" Height="20px" Width="80px" 
                                                  onclick="btnAgregarTerceros_Click" />
                                              &nbsp; &nbsp;

                                          </td>

                                      </tr>
                                      <tr> 
                                        <td colspan="3">
                                            <div style="width:860px; overflow:auto">
                                                <asp:GridView ID="grvComplTerceros" runat="server" AutoGenerateColumns="false" 
                                                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                                                    DataKeyNames="id_complTerceros" onrowdatabound="grvComplTerceros_RowDataBound" 
                                                    onrowdeleting="grvComplTerceros_RowDeleting" 
                                                    onselectedindexchanged="grvComplTerceros_SelectedIndexChanged">
                                                    <%--onrowdeleting="grvInfAduanal_RowDeleting" 
                                                    onselectedindexchanged="grvInfAduanal_SelectedIndexChanged"--%>
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
                                                        <asp:TemplateField HeaderText="id_registros" Visible="False">
                                                            <EditItemTemplate>
                                                                <asp:Label ID="Label169" runat="server" Text='<%# Eval("id_registros") %>'></asp:Label>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label170" runat="server" Text='<%# Bind("id_registros") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="Aduana" 
                                                            HeaderText="<%$ Resources:resCorpusCFDIEs,lblNomAduana %>" ReadOnly="True" />
                                                        <asp:BoundField DataField="DocAduana" 
                                                            HeaderText="<%$ Resources:resCorpusCFDIEs,lblNoDocAduanal %>" ReadOnly="True" />
                                                        <asp:BoundField DataField="FechaAduana" DataFormatString="{0:d}" 
                                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblFecha %>" ReadOnly="True" />--%>
                                                        <asp:BoundField DataField="rfc" HeaderText="RFC" ReadOnly="true" />
                                                        <asp:BoundField DataField="nombre" HeaderText="Nombre" ReadOnly="true" />
                                                        <asp:BoundField DataField="impuestoTrasIVA" HeaderText="Impuesto Tras" 
                                                            ReadOnly="true" Visible="false" />
                                                        <asp:BoundField DataField="importeTrasIVA" HeaderText="IVA Tras" 
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="tasaTrasIVA" HeaderText="IVA Tras %" 
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="impuestoTrasIEPS" HeaderText="Impuesto IEPS" 
                                                            ReadOnly="true" Visible="false" />
                                                        <asp:BoundField DataField="importeTrasIEPS" HeaderText="IEPS" ReadOnly="true" />
                                                        <asp:BoundField DataField="tasaTrasIEPS" HeaderText="IEPS %" ReadOnly="true" />
                                                        <asp:BoundField DataField="impuestoRetIVA" HeaderText="Impuesto IVA Ret" 
                                                            ReadOnly="true" Visible="false" />
                                                        <asp:BoundField DataField="importeRetIVA" HeaderText="IVA Ret" 
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="impuestoRetISR" HeaderText="Impuesto ISR" 
                                                            ReadOnly="true" Visible="false" />
                                                        <asp:BoundField DataField="importeRetISR" HeaderText="ISR" ReadOnly="true" />
                                                        <asp:BoundField DataField="calle" HeaderText="Calle" ReadOnly="true" />
                                                        <asp:BoundField DataField="noExterior" HeaderText="No. Ext" ReadOnly="true" />
                                                        <asp:BoundField DataField="noInterior" HeaderText="No. Int" ReadOnly="true" />
                                                        <asp:BoundField DataField="colonia" HeaderText="Colonia" ReadOnly="true" />
                                                        <asp:BoundField DataField="referencia" HeaderText="Referencia" 
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="localidad" HeaderText="Localidad" ReadOnly="true" />
                                                        <asp:BoundField DataField="municipio" HeaderText="Municipio" ReadOnly="true" />
                                                        <asp:BoundField DataField="estado" HeaderText="Estado" ReadOnly="true" />
                                                        <asp:BoundField DataField="pais" HeaderText="País" ReadOnly="true" />
                                                        <asp:BoundField DataField="codigoPostal" HeaderText="Código Postal" 
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="aduanaInfoAd" HeaderText="Aduana" ReadOnly="true" />
                                                        <asp:BoundField DataField="numeroInfoAd" HeaderText="No. Doc. Aduana" 
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="fechaInfoAd" HeaderText="Fecha Aduana" 
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="numeroCtaPred" HeaderText="Cta. Predial" 
                                                            ReadOnly="true" />
                                                        <asp:BoundField DataField="id_complTerceros" HeaderText="complTerceros" 
                                                            ReadOnly="true" Visible="false" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        No hay complemento de terceros para el Artículo
                                                    </EmptyDataTemplate>
                                                    <HeaderStyle BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                                                </asp:GridView>
                                            </div> 
                                        </td>
                                      </tr>
                                  </table>
                                    </div>
                            </asp:Panel>

                        </td>
                    </tr>
                  </table>
                  <%-- Fin Tabla complemento terceros --%>
              </asp:Panel>
              <table>
                  <tr>
                      <td width="800">
                           </td>
                      <td>
                          <asp:Button ID="btnAgregarArt" runat="server" CssClass="botonEstilo" 
                              onclick="btnAgregarArt_Click" TabIndex="53" 
                              Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
                              ValidationGroup="sucursalValidationArticulo" />
                              <asp:ValidationSummary ID="vsSucursalValidationArticulo" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, varValidacionError %>" 
                             ValidationGroup="sucursalValidationArticulo" ShowMessageBox="True" ShowSummary="False" />
                      </td>
                  </tr>
              </table>
            <%--cuarto--%>
                     </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upDetalles" runat="server" >
        <ContentTemplate>
            <asp:Panel ID="pnlRegistro" runat="server" GroupingText="<%$ Resources:resCorpusCFDIEs, lblDetalles %>"
                Width="930px">
                <table class="style1">
                    <tr>
                        <td>
                            <asp:GridView ID="grvDetalles" runat="server" AutoGenerateColumns="False" 
                                onrowdeleting="grvDetalles_RowDeleting" Width="860px" 
                                onselectedindexchanged="grvDetalles_SelectedIndexChanged" 
                                DataKeyNames="id_Registros" BorderColor="#999999" BorderStyle="Solid" 
                                BorderWidth="1px" onrowdatabound="grvDetalles_RowDataBound">
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" ButtonType="Image" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>"  SelectImageUrl="~/Imagenes/edit_16X16.png" SelectText="" >
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:CommandField>
                                    <asp:CommandField ShowDeleteButton="True" ButtonType="Image" 
                                        DeleteImageUrl="~/Imagenes/error_sign.gif" DeleteText="" HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" >
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:CommandField>
                                    <asp:BoundField DataField="id_Registros" HeaderText="id_Registros" 
                                        Visible="False" />
                                    <asp:BoundField DataField="Codigo" HeaderText="<%$ Resources:resCorpusCFDIEs, lblCodigo %>" />
                                    <asp:BoundField DataField="Unidad" HeaderText="<%$ Resources:resCorpusCFDIEs, lblUnidad %>" />
                                    <asp:BoundField DataField="Descripcion" HeaderText="<%$ Resources:resCorpusCFDIEs, lblDescripcion %>" />
                                    <asp:BoundField DataField="PrecioUnitario" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblPrecio %>" >
                                    <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cantidad" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblCantidad %>" 
                                        DataFormatString="" >
                                    <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Importe" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblImporte %>" >
                                    <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No hay Artículos Capturados
                                </EmptyDataTemplate>
                                <HeaderStyle BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" >
                            <table style="width:820px;">
                                <tr>
                                    <td style="text-align:right;">
                                        <table>
                                            <tr>
                                                <td style="width:670px; text-align:right;">
                                                    <asp:Label ID="lblSubtotal" runat="server" style="font-weight: 700" 
                                                        Text="<%$ Resources:resCorpusCFDIEs, lblSubtotal %>"  Visible="False" Width="100px"></asp:Label>
                                                </td>
                                                <td style="width:150px; text-align:right;">
                                                    <asp:Label ID="lblDetSubtotal" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:670px; text-align:right;">
                                                    <asp:Label ID="lblDetDescuento" runat="server" style="font-weight: 700" 
                                                        Text="<%$ Resources:resCorpusCFDIEs, lblDescuentoSinPorc %>"  Visible="False" Width="100px"></asp:Label>
                                                </td>
                                                <td style="width:150px; text-align:right;">
                                                    <asp:Label ID="lblDetDescuentoVal" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                            <asp:DataList ID="dtsTotales" runat="server" 
                                                onitemcommand="dtsTotales_ItemCommand" Width="820px">
                                                <ItemTemplate>
                                                    <table style="width:820px; text-align:right;" >
                                                        <tr>
                                                            <td style="width:340px;">
                                                                 
                                                            </td>
                                                            <td style="width:20px;">
                                                                <asp:ImageButton ID="imgModificar" runat="server" 
                                                                    ImageUrl="~/Imagenes/edit_16X16.png" onclick="imgModificar_Click" 
                                                                    CommandArgument='<%# Eval("efecto") %>' Visible="False" />
                                                            </td>
                                                            <td style="width:60px; text-align:left;" >
                                                                <asp:Label ID="lblTasa" runat="server" style="font-weight: 700" Text="<%$ Resources:resCorpusCFDIEs, lblTasaImpuesto %>" ></asp:Label>
                                                            </td>
                                                            <td style="width:80px; text-align:left;">
                                                                <asp:Label ID="lblTasaVal" runat="server" Text='<%# Eval("tasa") %>' ></asp:Label>
                                                            </td>
                                                            <td style="width:80px; text-align:left;">
                                                                <asp:Label ID="lblTipoImpDoc" runat="server" Text='<%# Eval("efecto") %>' ></asp:Label>
                                                            </td>
                                                            <td style="width:80px; text-align:right; padding-right:3px;">
                                                                <asp:Label ID="lblTipoImpuesto" runat="server" style="font-weight: 700" 
                                                                    Text='<%# Eval("abreviacion") %>'></asp:Label>
                                                            </td>
                                                            <td style="width:150px; text-align:right;">
                                                                <asp:Label ID="lblCalculo" runat="server" Text='<%# Eval("calculo") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        <table>
                                            <tr>
                                                <td style="width:670px; text-align:right;">
                                                    <asp:Label ID="lblTotal" runat="server" style="font-weight: 700" Text="<%$ Resources:resCorpusCFDIEs, lblTotal %>" 
                                                        Visible="False" Width="100px"></asp:Label>
                                                </td>
                                                <td style="width:150px; text-align:right;">
                                                    <asp:Label ID="lblTotalVal" runat="server" 
                                                        Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                
                                                    &nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Label ID="lblNumerosLetras" runat="server" style="font-weight: 700"></asp:Label>
                         </td>
                    </table>
            </asp:Panel>

            <asp:Panel ID="pnlModal" runat="server" BackColor="#CCCCCC" BorderColor="Black" 
                BorderStyle="Solid" Height="90px" Width="300px">
                <table style="width:300px; padding-left:10px;">
                    <tr>
                        <td>
                        <asp:Label ID="lblImpuesto" runat="server" AssociatedControlID="ddlCambioImpuesto"
                                Text="<%$ Resources:resCorpusCFDIEs, lblImpuesto %>" style="color: #000000"></asp:Label>
                                <br />
                        <asp:DropDownList ID="ddlCambioImpuesto" runat="server" 
                                DataTextField="abreviacion" DataValueField="tasa" Width="200px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvTasa" runat="server" 
                                ControlToValidate="ddlCambioImpuesto" CssClass="failureNotification"
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valTasaReq %>" 
                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, valTasaReq %>" 
                                ValidationGroup="groupImp"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;">
                            <asp:Button ID="btnAceptar" runat="server" onclick="btnAceptar_Click" Text="<%$ Resources:resCorpusCFDIEs, btnActualizar %>"
                                ValidationGroup="groupImp" Width="80px" />
                                <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <cc1:modalpopupextender id="mpePanel" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlModal" popupdraghandlecontrolid=""
                targetcontrolid="lnkModal">
                </cc1:modalpopupextender>

            <asp:LinkButton ID="lnkModal" runat="server"></asp:LinkButton>
            <asp:LinkButton ID="lnkModalArt" runat="server"></asp:LinkButton>
   

            <asp:HiddenField ID="hdnItemSel" runat="server" Visible="false"  /> 

            <asp:HiddenField ID="hdnItemMod" runat="server" Visible="false" />

             <asp:HiddenField ID="hdMostrarModal" runat="server" Value="0" />

             <asp:UpdateProgress ID="updProgCrear" runat="server">
                <progresstemplate>
                    <img alt="" 
                    src="../Imagenes/imgAjaxLoader.gif" />
                </progresstemplate>
            </asp:UpdateProgress>

            <table style="width: 929px">
                <tr>
                    <td class="style7" align="right">
                        <asp:DropDownList ID="ddlAddenda" runat="server" 
                            onselectedindexchanged="ddlAddenda_SelectedIndexChanged" Width="200px" 
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnAdenda" runat="server" CssClass="botonGrande" TabIndex="54" Text="<%$ Resources:resCorpusCFDIEs, btnInsertarAddenda %>" 
                            Height="30px" Width="150px" onclick="btnAdenda_Click2" />
                    </td>
                    <td align="right">
                        <asp:Button ID="btnCrear" runat="server" onclick="btnCrear_Click" Text="<%$ Resources:resCorpusCFDIEs, btnCrearComprobante %>" 
                            ValidationGroup="RegisterUserValidationGroup" TabIndex="55" 
                            CssClass="botonEstilo" />
                            
                            
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="3">
                    <asp:ValidationSummary ID="vsCrearFactura" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, varValidacionError %>" 
                             ValidationGroup="RegisterUserValidationGroup" ShowMessageBox="True" ShowSummary="False" />
                    </td>
                </tr>
            </table>
            
            <asp:Panel ID="pnlBusqueda" runat="server" CssClass="modal">
                <div style="width:680px;">
                    <fieldset style="width:590px;">
                    <legend><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaReceptor %>" /></legend>
                        <p>
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubtituloBusReceptores %>" Width="580px" />
                        </p>
                        <p>
                            <asp:Label ID="lblRFC" AssociatedControlID="txtRfcConsulta" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>"></asp:Label>
                                <br />
                            <asp:TextBox ID="txtRfcConsulta" runat="server" CssClass="textEntry" 
                                MaxLength="15"></asp:TextBox>
                        </p>
                        <p>
                            <asp:Label ID="lblRazon" AssociatedControlID="txtRazonSocialConsulta" 
                                runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRazonSocial %>"></asp:Label>
                                <br />
                            <asp:TextBox ID="txtRazonSocialConsulta" runat="server" CssClass="textEntry" 
                                MaxLength="50" ></asp:TextBox>
                        </p>
                    </fieldset>
                </div>
                <p>
                    <asp:Button ID="btnConsulta" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                        onclick="btnConsulta_Click" CssClass="botonEstilo" />
                        <asp:Button ID="btnCancelarModal" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                        CssClass="botonEstilo" onclick="btnCancelarModal_Click" 
                        />
                </p>
                <asp:GridView ID="gdvReceptores" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" GridLines="Horizontal" Width="600px" 
                        DataKeyNames="id_rfc_receptor,rfc_receptor,nombre_receptor" AllowPaging="True" 
                        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                        onpageindexchanging="gdvReceptores_PageIndexChanging" 
                    onselectedindexchanged="gdvReceptores_SelectedIndexChanged" PageSize="5" 
                    BackColor="White">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" SelectText="<%$ Resources:resCorpusCFDIEs, lblSeleccionar %>" />
                            <asp:BoundField DataField="rfc_receptor" 
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblRfc %>" ItemStyle-Width="100" 
                                HeaderStyle-HorizontalAlign="Left"  >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="nombre_receptor" 
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblRazonSocial %>" 
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
            <br />
                      <asp:Panel ID="pnlArticulos" runat="server" CssClass="modal">
                <div style="width:680px;">
                    <fieldset style="width:590px;">
                    <legend><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaArticulo %>" /></legend>
                        <p>
                           <asp:Label ID="Label29" AssociatedControlID="txtArtConsulta" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblCodigo %>"></asp:Label>
                                <br />
                            <asp:TextBox ID="txtArtConsulta" runat="server" CssClass="textEntry" 
                                MaxLength="20"></asp:TextBox>
                        </p>
                        <p>
                            <asp:Label ID="Label42" AssociatedControlID="txtDesArtConsulta" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblDescripcion %>"></asp:Label>
                                <br />
                            <asp:TextBox ID="txtDesArtConsulta" runat="server" CssClass="textEntry"></asp:TextBox>
                        </p>
                    </fieldset>
                </div>
                <p>
                    <asp:Button ID="btnConsultarArt" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                        onclick="btnConsultarArt_Click" CssClass="botonEstilo" />
                        <asp:Button ID="btnCancelarArt" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                        CssClass="botonEstilo" onclick="btnCancelarArt_Click" 
                        />
                </p>
                <asp:GridView ID="gvArticulos" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" GridLines="Horizontal" Width="890px" 
                        DataKeyNames="id_articulo" AllowPaging="True" 
                        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                    BackColor="White" 
                              onpageindexchanging="gvArticulos_PageIndexChanging" 
                              onselectedindexchanged="gvArticulos_SelectedIndexChanged" 
                              onrowdatabound="gvArticulos_RowDataBound">
                        <Columns>
                         <asp:CommandField SelectText="<%$ Resources:resCorpusCFDIEs, lblSeleccionar %>"
                        ShowSelectButton="True" HeaderStyle-HorizontalAlign="Left" >
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:CommandField>
                             <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbleidArticulo" runat="server" Text='<%# Bind("id_articulo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblDescripcion %>" 
                        HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lbledescripcion" runat="server" 
                                        Text='<%# Bind("descripcion") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Width="300px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblMedida %>" 
                                HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblemedida" runat="server" Text='<%# Bind("medida") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="precio" DataFormatString="{0:C6}" 
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblPrecio %>" />
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblIva %>" 
                                HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lbleiva" runat="server" 
                                Text='<%# Bind("iva") %>'></asp:Label>
                                </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblIeps %>" 
                                HeaderStyle-HorizontalAlign="Left" Visible="false"> 
                                <ItemTemplate>
                                    <asp:Label ID="lbleieps" runat="server" Text='<%# Bind("ieps") %>'></asp:Label>
                                </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblIsr %>" 
                                HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lbleisr" runat="server" Text='<%# Bind("isr") %>'></asp:Label>
                                </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblIvaRetenido %>" 
                                HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lbleivaretenido" runat="server" Text='<%# Bind("ivaretenido") %>'></asp:Label>
                                </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblMoneda %>" 
                                HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblemoneda" runat="server" Text='<%# Bind("moneda") %>'></asp:Label>
                                </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>" 
                                HeaderStyle-HorizontalAlign="Left" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbleidestructura" runat="server" 
                                        Text='<%# Bind("id_estructura") %>'></asp:Label>
                                </ItemTemplate>

                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus %>" 
                                HeaderStyle-HorizontalAlign="Left" Visible="False">
                            
                                <ItemTemplate>
                                    <asp:Label ID="lbleestatus" runat="server" Text='<%# Bind("estatus") %>'></asp:Label>
                                </ItemTemplate>

                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                             
                             <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblCodigo %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblclaveart" runat="server" Text='<%# Bind("clave") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
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
                        <br />
            <asp:Panel ID="pnlCreditos" runat="server" Height="142px" Width="579px" 
                CssClass="modal" BorderStyle="Solid" BorderWidth="1px">

                <table align="center">
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label7" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblMsgCreditos %>"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label121" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, varSinGeneracionytimbrado %>" 
                                Visible="False"></asp:Label>
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
                            <asp:Button ID="btnAcepCreditos" runat="server" onclick="btnAcepCreditos_Click" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" CssClass="botonEstilo" />
                        </td>
                    </tr>
                </table>
                </asp:Panel>

            <cc1:modalpopupextender id="modalCreditos" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlCreditos" popupdraghandlecontrolid=""
                targetcontrolid="lnkCreditos">
            </cc1:modalpopupextender>

            <asp:LinkButton ID="lnkCreditos" runat="server"></asp:LinkButton>

            <br />

               <asp:Panel ID="pnlEnvioCorreo" runat="server" Height="390px" Width="730px" 
                CssClass="modal" BorderStyle="Solid" BorderWidth="1px">

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
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Label ID="lblRetSat" runat="server" 
                                        Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>

                        </td>
                    </tr>
                    <tr>
                        <td>

                            <hr style="width: 700px;" />

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
                                                    <asp:Image ID="imgPDF" runat="server" ImageUrl="~/Imagenes/logo_pdf.png" 
                                                        Width="30" />
                                                    +<asp:Image ID="imgXML" runat="server" ImageUrl="~/Imagenes/xml_mediano.png" 
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
                                    
                                        <asp:TextBox ID="txtCorreoEmisor" runat="server" Width="650px"></asp:TextBox>
                                    
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                            ControlToValidate="txtCorreoEmisor" CssClass="failureNotification" 
                                            Display="Dynamic" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                            ValidationGroup="EnvioCorreoValidationGroup"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                            ControlToValidate="txtCorreoEmisor" CssClass="failureNotification" 
                                            Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                            ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" 
                                            ValidationGroup="EnvioCorreoValidationGroup" Width="16px"><img 
                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    
                                        <asp:Label ID="Label118" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreoCliente %>"></asp:Label>
                                    
                                    </td>
                                    <td>
                                    
                                        <asp:TextBox ID="txtCorreoCliente" runat="server" Width="650px" 
                                            ToolTip="En caso de capturar varios correos separarlos por una coma &quot;,&quot; (ejem@sen.com, ejem2@sen.com, ...)"></asp:TextBox>
                                    
                                    </td>
                                    <td>

                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                            ControlToValidate="txtCorreoCliente" CssClass="failureNotification" 
                                            Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
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
                                            onclick="btnAceptarCor_Click" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" 
                                            ValidationGroup="EnvioCorreoValidationGroup" />
                                        <asp:Button ID="btnCancelarCor" runat="server" CssClass="botonEstilo" 
                                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                                            onclick="btnCancelarCor_Click" />
                                    
                                    </td>
                                    <td>
                                       </td>
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

            <cc1:modalpopupextender id="mpeEnvioCorreo" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlEnvioCorreo" popupdraghandlecontrolid=""
                targetcontrolid="lbEnvioCorreo">
            </cc1:modalpopupextender>

            <asp:LinkButton ID="lbEnvioCorreo" runat="server"></asp:LinkButton>

            <br />
            <asp:Panel ID="pnlPagPar" runat="server" CssClass="modal" Width="460px">
                <div style="width:450px;">
                    <fieldset style="width:420px;">
                    <legend><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloPagoParcial %>" /></legend>
                        <table>
                            <tr>
                                <td colspan="3">
                                <table>
                                <tr>
                                    <td>
                                      <asp:CheckBox ID="cbPagParPri" runat="server" 
                                        oncheckedchanged="cbPagParPri_CheckedChanged" 
                                        Text="Generar documento por primera vez" AutoPostBack="True" 
                                            CssClass="fontText" />                                  
                                    </td>
                                    <td>
                                        &nbsp;
                                        &nbsp;</td>
                                    <td>
                                        <asp:Label ID="Label45" runat="server" Text="Parcialidades" CssClass="fontText"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumParcialidad" runat="server" Width="30px" 
                                            CssClass="fontText">0</asp:TextBox>

                                            <cc1:FilteredTextBoxExtender ID="fteNumParcialidad" runat="server" 
                                            FilterType="Custom, Numbers, UppercaseLetters" TargetControlID="txtNumParcialidad" 
                                            InvalidChars="°!&quot;#$%&amp;/()=?¡|¿'´+*{[}]-_.:,;@¬\~^`" 
                                            ValidChars="0-9,A-Z">
                                            </cc1:FilteredTextBoxExtender>   
                                    </td>
                                    <td>
                                            <asp:RequiredFieldValidator ID="rfvNumPar" runat="server" 
                                            ControlToValidate="txtNumParcialidad" CssClass="failureNotification" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, valParcialidades %>" 
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, valParcialidades %>" 
                                            ValidationGroup="ValidationGroupPagoParcial" Width="16px">*</asp:RequiredFieldValidator>                                    
                                    </td>
                                </tr>
                                </table>

                                
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                        
                                            <asp:Label ID="Label114" runat="server" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblPagParDe %>" CssClass="fontText"></asp:Label>
                                        
                                        </td>
                                        <td>
                                        
                                            <asp:TextBox ID="txtPagParDe" runat="server" Width="30px" CssClass="fontText">0</asp:TextBox>

                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                            FilterType="Custom, Numbers, UppercaseLetters" TargetControlID="txtPagParDe" 
                                            InvalidChars="°!&quot;#$%&amp;/()=?¡|¿'´+*{[}]-_.:,;@¬\~^`" 
                                            ValidChars="0-9,A-Z">
                                            </cc1:FilteredTextBoxExtender>                                       
                                        </td>
                                        <td>
                                      <asp:RequiredFieldValidator ID="rfvParDe" runat="server" 
                                                ControlToValidate="txtPagParDe" CssClass="failureNotification" 
                                                Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, valPagParDe %>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valPagParDe %>" 
                                                ValidationGroup="ValidationGroupPagoParcial"><img 
                                                src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>                                        
                                        </td>
                                        <td>
                                        
                                            <asp:Label ID="Label115" runat="server" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblPagParA %>" CssClass="fontText"></asp:Label>
                                        
                                        </td>
                                        <td>
                                        
                                            <asp:TextBox ID="txtPagParA" runat="server" Width="30px" CssClass="fontText">0</asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" 
                                            FilterType="Custom, Numbers, UppercaseLetters" TargetControlID="txtPagParA" 
                                            InvalidChars="°!&quot;#$%&amp;/()=?¡|¿'´+*{[}]-_.:,;@¬\~^`" 
                                            ValidChars="0-9,A-Z">
                                            </cc1:FilteredTextBoxExtender>                                         
                                        </td>
                                        <td>
                                          <asp:RequiredFieldValidator ID="rfvParA" runat="server" 
                                                ControlToValidate="txtPagParA" CssClass="failureNotification" 
                                                Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, valPagParDe %>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valPagParDe %>" 
                                                ValidationGroup="ValidationGroupPagoParcial"><img 
                                src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
   
                                        </td>
                                    </tr>
                                </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label43" runat="server" AssociatedControlID="txtFolFisOri" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblFolioFiscalOrig %>" 
                                        CssClass="fontText"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td class="style15">
                                    <asp:Label ID="Label111" runat="server" AssociatedControlID="txtFecFolFisOri" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblFechaFolioFiscalOrig %>" 
                                        CssClass="fontText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                
                                    <asp:TextBox ID="txtFolFisOri" runat="server" CssClass="fontText" 
                                        Width="200px"></asp:TextBox>
                                
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvFolFisOri" runat="server" 
                                        ControlToValidate="txtFolFisOri" CssClass="failureNotification" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, valFolioFiscalOrig %>" 
                                        ValidationGroup="ValidationGroupPagoParcial" Width="16px">*</asp:RequiredFieldValidator>
                                </td>
                                <td>
                                
                                    <asp:TextBox ID="txtFecFolFisOri" runat="server" CssClass="fontText" 
                                        MaxLength="0" Width="100px"></asp:TextBox>
                                <asp:Image ID="imgPagPar" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <cc1:CalendarExtender ID="txtFecha_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFecFolFisOri" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgPagPar">
                                </cc1:CalendarExtender>                                
                                    <asp:RegularExpressionValidator ID="revFecFolFisOri" runat="server" 
                                        ControlToValidate="txtFecFolFisOri" CssClass="failureNotification" 
                                        Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                        ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                        ValidationGroup="grupoConsulta"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <img src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfvFecFolOri" runat="server" 
                                        ControlToValidate="txtFecFolFisOri" CssClass="failureNotification" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" 
                                        ValidationGroup="grupoConsulta" Width="16px">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                
                                    <asp:Label ID="Label44" runat="server" AssociatedControlID="txtSerFolFisOri" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblSerieFolioFiscalOrig %>" 
                                        CssClass="fontText"></asp:Label>
                                
                                </td>
                                <td>
                                   </td>
                                <td class="style15">
                                
                                    <asp:Label ID="Label112" runat="server" AssociatedControlID="txtMonFolFisOri" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblMontoFolioFiscalOrig %>" 
                                        CssClass="fontText"></asp:Label>
                                
                                </td>
                            </tr>
                            <tr>
                                <td>
                                
                                    <asp:TextBox ID="txtSerFolFisOri" runat="server" CssClass="fontText" 
                                        Width="200px"></asp:TextBox>
                                
                                </td>
                                <td>
<%--                                    <asp:RequiredFieldValidator ID="rfvSerFolFisOri" runat="server" 
                                        ControlToValidate="txtSerFolFisOri" CssClass="failureNotification" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, valSerieFolioFiscalOrig %>" 
                                        ValidationGroup="ValidationGroupPagoParcial" Width="16px">*</asp:RequiredFieldValidator>--%>
                                </td>
                                <td class="style15">
                                
                                    <asp:TextBox ID="txtMonFolFisOri" runat="server" CssClass="fontText" 
                                        Width="100px">0</asp:TextBox>                                
                                    <asp:RequiredFieldValidator ID="rfvMonFolFisOri" runat="server" 
                                        ControlToValidate="txtMonFolFisOri" CssClass="failureNotification" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, valMontoFolioFiscalOrig %>" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, valMontoFolioFiscalOrig %>" 
                                        ValidationGroup="ValidationGroupPagoParcial" Width="16px">*</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regxMonFolFisOri" runat="server" 
                                        ControlToValidate="txtMonFolFisOri" CssClass="failureNotification" 
                                        Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                        ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                        ValidationGroup="sucursalValidationArticulo"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <img src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Button ID="btnAceptarPar" runat="server" CssClass="botonEstilo" 
                                        onclick="btnAceptarPar_Click" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" 
                                        ValidationGroup="ValidationGroupPagoParcial" />
                                        <asp:ValidationSummary ID="vsPagpParcial" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, varValidacionError %>" 
                             ValidationGroup="ValidationGroupPagoParcial" ShowMessageBox="True" ShowSummary="False" />
                                </td>
                                <td class="style15">
                                    <asp:Button ID="btnCancelarPar" runat="server" CssClass="botonEstilo" 
                                        onclick="btnCancelarPar_Click" 
                                        Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>

                </div>
            </asp:Panel>

                <cc1:modalpopupextender id="mpePagoParcial" runat="server" backgroundcssclass="modalBackground"    popupcontrolid="pnlPagPar" popupdraghandlecontrolid=""
                targetcontrolid="lnkModalPagPar" CancelControlID="btnCancelarPar">
                </cc1:modalpopupextender>

                     <asp:LinkButton ID="lnkModalPagPar" runat="server"></asp:LinkButton>

            <%--quinto--%>
                     </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upComplentos" runat="server" >
        <ContentTemplate>
              <%--sexto--%>
                       </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upAdenda" runat="server" >
        <ContentTemplate>
              <br />
              <asp:Panel ID="PnModalAdenda" runat="server" CssClass="modal2" Width="658px">
                  <br />
                  <table>
                      <tr>
                          <td>
                              <asp:Label ID="Label106" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNamespace %>"></asp:Label>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:TextBox ID="txtNamespace" runat="server" Width="400px"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="Label105" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblxsdAddenda %>"></asp:Label>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:TextBox ID="txtxsd" runat="server" Width="400px"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:Label ID="lblAdenda" runat="server" 
                                  Text="<%$ Resources:resCorpusCFDIEs, lblAdenda %>"></asp:Label>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:TextBox ID="txtAdenda" runat="server" Height="300px" TextMode="MultiLine" 
                                  Width="638px" ValidationGroup="Adenda"></asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <asp:RequiredFieldValidator ID="rvftbAdenda" runat="server" 
                                  ControlToValidate="txtAdenda" CssClass="failureNotification" 
                                  ErrorMessage="Información requerida" ToolTip="Capturar Adenda." 
                                  ValidationGroup="Adenda" Width="20px">*</asp:RequiredFieldValidator>
                          </td>
                      </tr>
                      <tr>
                          <td>
                                                                    
                              <asp:Button ID="BotinGuardar" runat="server" CssClass="botonEstilo" 
                                  onclick="BotinGuardar_Click" 
                                  Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                                  ValidationGroup="Adenda" />
                               <asp:Button ID="BotinCancelar" runat="server" CssClass="botonEstilo" 
                                  Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                                  />
                          </td>
                      </tr>
                  </table>
                  <br />
                  <br />
                  <cc1:ModalPopupExtender ID="mpeAdenda" runat="server" 
                      backgroundcssclass="modalBackground" CancelControlID="BotinCancelar" 
                      popupcontrolid="PnModalAdenda" popupdraghandlecontrolid="" 
                      targetcontrolid="LnkModalAdenda">
                  </cc1:ModalPopupExtender>
                  <asp:LinkButton ID="LnkModalAdenda" runat="server"></asp:LinkButton>
                  </asp:Panel>


            <br />

         
         <asp:Panel ID="PanelComplemento1" runat="server" Height="142px" Width="579px" 
                CssClass="modal" BorderStyle="Solid" BorderWidth="1px">

                <table align="center">
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <img alt="" src="../Imagenes/Informacion.png" width="44" />
                                 
                        </td>
                        <td align="left" valign="top">
                            <asp:Label ID="Label3" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblmsjComplementos %>"></asp:Label>
                            <asp:ImageButton ID="ImageButton1" runat="server" Enabled="False" 
                                ImageUrl="~/Imagenes/modificar.gif" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, VarComplementoSeleccionado %>" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                         </td>
                        <td align="center">
                            <asp:Button ID="Button1" runat="server" CssClass="botonEstilo" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" />
                        </td>
                    </tr>
                </table>
                </asp:Panel>

                <asp:LinkButton ID="lnkComplemento" runat="server"></asp:LinkButton>
                <cc1:modalpopupextender id="mpeComplemento" runat="server" backgroundcssclass="modalBackground" popupcontrolid="PanelComplemento1" popupdraghandlecontrolid=""
                targetcontrolid="lnkComplemento">
            </cc1:modalpopupextender>            

            <asp:Panel ID="pnlGenerando" runat="server" Width="300px" 
            CssClass="modal" BorderStyle="Solid" BorderWidth="1px">
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

            <cc1:modalpopupextender id="modalGenerando" runat="server" backgroundcssclass="modalBackground" 
            popupcontrolid="pnlGenerando" popupdraghandlecontrolid=""
                targetcontrolid="pnlGenerando">
            </cc1:modalpopupextender>

            <script type="text/javascript" language="javascript">
                var ModalProgress = '<%= modalGenerando.ClientID %>';         
            </script>
            <asp:Panel ID="PanelMensaje" runat="server" CssClass="modal"
                BorderStyle="Solid" BorderWidth="1px">
                <table align="center">
                    <tr>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <img alt="" src="../Imagenes/Informacion.png" width="44" />
                        </td>
                        <td align="left" valign="top">
                            <asp:Label ID="LblMensaje" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                        <td align="center">
                            <asp:Button ID="BtnMensaje" runat="server" CssClass="botonEstilo" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" 
                                onclick="BtnMensaje_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalMensaje" runat="server" BackgroundCssClass="modalBackground"
                PopupControlID="PanelMensaje" PopupDragHandleControlID="" TargetControlID="BtnMensaje" CancelControlID="BtnMensaje">
            </cc1:ModalPopupExtender>
        </ContentTemplate>



    </asp:UpdatePanel>



</asp:Content>


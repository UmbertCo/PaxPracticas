<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="webAltaSucursales.aspx.cs" Inherits="PAXRecepcionProveedores.Operacion.Empresas.webAltaSucursales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style2
        {
            width: 81px;
        }
        .style4
        {
            width: 318px;
        }
        .style5
        {
            width: 824px;
        }
        .style6
        {
            height: 19px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function fntraerEnter(keyStroke) {
            isNetscape = (document.layers);
            eventChooser = (isNetscape) ? keyStroke.which : event.keyCode;
            if (eventChooser == 13) {
                return false;
            }
        }
        document.onkeypress = fntraerEnter; 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br /><br /><br />
    <h2>
        <asp:Label ID="lblTitulo" runat="server" 
            Text="<%$ Resources:resCorpusCFDIEs, lblAltaSucursales %>" Font-Bold="True" 
            ForeColor="#8B181B"></asp:Label>
    </h2>
    <table style="border: 2px ridge #800000">
        <tr>
            <td class="style5">
            <asp:UpdatePanel ID="udpEmpresas" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td align="left">
                           <asp:Label ID="lblNombreEmpresa" runat="server" AssociatedControlID="ddlEmpresas" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblNombreEmpresa %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:DropDownList ID="ddlEmpresas" runat="server"  Width="470px" OnSelectedIndexChanged="ddlEmpresas_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="ddlEmpresas" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblSucursal %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:DropDownList ID="ddlSucursales" runat="server" Width="470px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                    <table align="right">
                    <tr>
                        <td rowspan="50" width="220px">
                             &nbsp;</td>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnNuevo" runat="server" CssClass="botonEstiloVentanas" 
                                    onclick="btnNuevo_Click" TabIndex="4" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" Width="80px" />
                                <asp:Button ID="btnBorrar" runat="server" CssClass="botonEstiloVentanas" 
                                    onclick="btnBorrar_Click" 
                                    OnClientClick="return confirm('¿Desea eliminar la empresa seleccionada?');" 
                                    TabIndex="5" Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" Width="80px" />
                                <asp:Button ID="btnEditar" runat="server" CssClass="botonEstiloVentanas" 
                                    onclick="btnEditar_Click" TabIndex="6" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" Width="80px" />
                                <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstiloVentanas" 
                                    onclick="btnNCancelar_Click" TabIndex="7" 
                                    Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" />
                            </td>
                        </tr>
                    </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlEmpresas" EventName="SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    <tr>
    <td class="style5">
    <asp:UpdatePanel ID="udpSucursales" runat="server">
    <ContentTemplate>
    <table style="margin: 0px auto;">
        <tr>
            <td colspan="2" align="left">
                <table align="left">
                    <tr>
                        <td align="left">
                            *<asp:Label ID="lblNombre" runat="server" 
                            AssociatedControlID="txtNombre" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblNombre %>"></asp:Label>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>
                                *<asp:Label ID="lblCalle0" runat="server" AssociatedControlID="txtCalle" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td align="left" rowspan="10">
                            <table>
                                <tr>
                                    <td align="left"> <asp:Label ID="lblCorreosAcuse" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblCorreosAcuse %>" Width="400px"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="left">*<asp:Label ID="lblCorreoElectronico" runat="server" 
                                                AssociatedControlID="txtNoExterior" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"></asp:Label> </td>
                                </tr>
                                    <tr>
                                        <td><asp:TextBox ID="txtCorreo" runat="server" Width="220px"></asp:TextBox>
                                        
                                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                                ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                                ValidationGroup="CorreoValidadorGroup"><img 
                                                src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                ValidationGroup="CorreoValidadorGroup" Width="131px"><img 
                            src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Button ID="btnAgregarMail" runat="server" CssClass="botonEstiloVentanas" 
                                                    onclick="btnAgregarMail_Click" 
                                                    Text="<%$ Resources:resCorpusCFDIEs, lblAgregar %>" 
                                                    ValidationGroup="CorreoValidadorGroup" Width="80px" />
                                                <asp:Button ID="btnQuitarMail" runat="server" CssClass="botonEstiloVentanas" 
                                                    onclick="btnQuitarMail_Click" 
                                                    Text="<%$ Resources:resCorpusCFDIEs, lblQuitar %>" Width="80px" />
                                            </td>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lbEmailsAcuses" runat="server" Height="77px" Width="394px">
                                                        </asp:ListBox>
                                                     </td>
                                                     <tr>
                                                        <td align="right"> <asp:Button ID="btnGuardarEmpresa" runat="server" 
                                                                CssClass="botonEstiloVentanas" onclick="btnGuardarEmpresa_Click" 
                                                                Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                                                                ValidationGroup="RegisterUserValidationGroup" Width="80px" /> </td>
                                                     </tr>
                                                </tr>
                                        </tr>
                                    </tr>
                            </table>
                                
                            </td>
                        </td>
                    </tr>
                    <tr>
                        
                        <td align="left">
                            <asp:TextBox ID="txtNombre" runat="server" Width="220px"></asp:TextBox>
                            <td align="left"><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                        ControlToValidate="txtNombre" CssClass="failureNotification" Display="Dynamic" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                                        ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator></td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtCalle" runat="server" Width="220px"></asp:TextBox>
                            </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvCalle" runat="server" 
                                                ControlToValidate="txtCalle" CssClass="failureNotification" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>" 
                                                ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                            src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                            </td>
                                        
                                </td>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblPais" runat="server" AssociatedControlID="ddlPais" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"></asp:Label>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td align="left">
                                            <asp:Label ID="lblNoExterior" runat="server" 
                                                AssociatedControlID="txtNoExterior" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>"></asp:Label>
                                        </td>
                                            <td></td>
                                            
                                    </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlPais" runat="server" AutoPostBack="true" 
                                                    OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" Width="223px">
                                                </asp:DropDownList>
                                            </td>
                                        <td></td>
                                        <td></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtNoExterior" runat="server" Width="220px"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            
                                        </tr>
                                                <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblEstado" runat="server" AssociatedControlID="ddlEstado" 
                                                        Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"></asp:Label>
                                                    </td>
                                                <td></td>
                                                <td></td>
                                                <td align="left">  
                                                    <asp:Label ID="lblNoInterior" runat="server" 
                                                        AssociatedControlID="txtNoInterior" 
                                                        Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>"></asp:Label>
                                                    <td>
                                                        
                                                    </td>
                                                
                                                </tr>
                                                     <tr>
                                                        <td align="left">                            
                                                            <asp:DropDownList ID="ddlEstado" runat="server" AutoPostBack="true" 
                                                                OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" Width="223px">
                                                            </asp:DropDownList>
                        </td>
                                                        <td></td>
                                                        <td></td>
                                                        <td align="left">                            
                                                            <asp:TextBox ID="txtNoInterior" runat="server" Width="220px"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        
                                                        </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblMunicipio" runat="server" AssociatedControlID="ddlMunicipio" 
                                                                            Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>"></asp:Label>
                                                                    </td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblCodigoPostal1" runat="server" 
                                                                            AssociatedControlID="txtCodigoPostal" 
                                                                            Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>"></asp:Label>
                                                                    </td>
                                                                    <td></td>
                                                                    </tr>       
                                                                        <tr>
                                                                            <td align="left"> 
                                                                                <asp:DropDownList ID="ddlMunicipio" runat="server" Width="223px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td></td>
                                                                            <td></td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtCodigoPostal" runat="server" Width="220px"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:RegularExpressionValidator ID="revCodigoPostal" runat="server" 
                                                                                    ControlToValidate="txtCodigoPostal" CssClass="failureNotification" 
                                                                                    ToolTip="Código postal erroneo" ValidationExpression="\d{5}" 
                                                                                    ValidationGroup="RegisterUserValidationGroup">
                            <img src="../../Imagenes/error_sign.gif" />
                            </asp:RegularExpressionValidator>
                                                                            </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblLocalidad" runat="server" AssociatedControlID="txtLocalidad" 
                                                                                        Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>"></asp:Label>
                                                                                </td>
                                                                                <td>&nbsp;</td>
                                                                                <td>&nbsp;</td>
                                                                                <td>
                                                                                    <asp:Label ID="lblTipoPlantilla" runat="server" 
                                                                                        Text="<%$ Resources:resCorpusCFDIEs, lblTipoPlantilla %>" />
                                                                                </td>
                                                                                <td></td>
                                                                            </tr>
                                                                              
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        
                                                                                        <asp:TextBox ID="txtLocalidad" runat="server" Width="220px"></asp:TextBox>
                                                                                        
                                                                                    </td>
                                                                                        <td></td>
                                                                                        <td></td>
                                                                                        <td align="left">
                                                                                            <asp:DropDownList ID="ddlTipoPlantilla" runat="server" />
                                                                                    </td>
                                                                                        <td>&nbsp;</td>
                                                                                </tr>   
                                                                                    <tr>
                                                                                        <td>
                                                                                            *<asp:Label ID="lblColonia" runat="server" AssociatedControlID="txtColonia" 
                                                                                                Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>"></asp:Label>
                                                                                        </td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td align="left">
                                                                                            <asp:CheckBox ID="chbUnica" runat="server" 
                                                                                                Text="<%$ Resources:resCorpusCFDIEs, lblUnica %>" />
                                                                                        </td>
                                                                                        <td>&nbsp;</td>
                                                                                    </tr>   
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <asp:TextBox ID="txtColonia" runat="server" Width="220px" ></asp:TextBox>
                                                                                                
                                                                                            </td>
                                                                                            <td>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                                                                ControlToValidate="txtColonia" CssClass="failureNotification" Display="Dynamic" 
                                                                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtColonia %>" 
                                                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, txtColonia %>" 
                                                                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                                                                                src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                            <%--<td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td align="left">
                                                                                                &nbsp;</td>
                                                                                            <td>&nbsp;</td>--%>
                                                                                        </tr>   
                                                                                            <%--<tr>
                                                                                            <td align="left">
                                                                                                &nbsp;</td>
                                                                                            <td>
                                                                                                &nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                        </tr>   
                                                                                                <tr>
                                                                                            <td class="style6"></td>
                                                                                            <td class="style6"></td>
                                                                                            <td class="style6"></td>
                                                                                            <td class="style6"></td>
                                                                                            <td class="style6"></td>
                                                                                        </tr>   --%>
                    </tr>
                </table>
                
            </td>
            
        </tr>
        
        
        <%--<tr>
            <td>
                <table style="width: 622px">
                    <tr>
                        <td colspan="2" style="width: 250px">
                            
                        </td>
                        <td colspan="2" style="width: 250px">
                            
                        </td>
                        
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;</td>
                        <td colspan="2">
                            &nbsp;</td>
                        
                    </tr>
                    <tr>
                    <td colspan="2">
                            
                        </td>
                        <td colspan="2">
                          
                        </td>
                    </tr>
                    <tr>
                    <td colspan="2">
                            &nbsp;</td>
                        <td colspan="2">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            
                        </td>
                        <td colspan="3">
                           
                        </td>
                    </tr>
                    <tr>
                        <td> 
                            
                           
                            
                            
                        </td>
                        <td colspan="3">
                            
                             
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td class="style2">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td class="style2">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        </tr>
                        <tr>
                        <td>
                            &nbsp;</td>
                        </tr>
                        <tr>
                        <td>
                            &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="udpCorreos" runat="server">
                    <ContentTemplate>
                    
                <table width="100%">
                    <tr>
                        <td class="style4">
                            
                        </td>
                        <td>
                            <table width="90px">
                                <tr>
                                    <td align="center">
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
                
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAgregarMail" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;</td>
        </tr>--%>
    </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnGuardarEmpresa" EventName="Click" />
    </Triggers>
    </asp:UpdatePanel>
    </td>
    </tr>
    </table>
    
</asp:Content>

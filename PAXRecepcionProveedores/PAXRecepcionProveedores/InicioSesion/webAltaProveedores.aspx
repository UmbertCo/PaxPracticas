<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="webAltaProveedores.aspx.cs" Inherits="PAXRecepcionProveedores.InicioSesion.webAltaProveedores" %>
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
    function fntraerEnter(keyStroke) {
        isNetscape = (document.layers);
        eventChooser = (isNetscape) ? keyStroke.which : event.keyCode;
        if (eventChooser == 13) {
            return false;
        }
    }
    document.onkeypress = fntraerEnter; 
    


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
    <h2>
        <asp:Label ID="lblTitulo" runat="server" 
            Text="<%$ Resources:resCorpusCFDIEs, lblRegistroProveedores %>" 
            Font-Bold="True" ForeColor="#8B181B"></asp:Label>
    </h2>
    <h5>
        <asp:Literal ID="ltrMensaje" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblIngreseDatos %>"/>
    </h5>
    <asp:UpdatePanel ID="upAltaProveedor" runat="server">
        <ContentTemplate>
        <table style="border: 2px ridge #8B181B; width: 852px; width=100%; height=100%">
            <tr>
                <td align="left">
                *<asp:Label ID="lblNombre" runat="server" 
                                    AssociatedControlID="txtNombre" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                </td>
                <td></td>
                <td>*<asp:Label ID="lblContacto" runat="server" 
                                    AssociatedControlID="txtContacto" 
                                    Text="Contacto"></asp:Label></td>
            </tr>
                <tr>
                    <td align="left">
                    <asp:TextBox ID="txtNombre" runat="server" Width="220px" TabIndex="1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                ControlToValidate="txtNombre" CssClass="failureNotification" Display="Dynamic" 
                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                                src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                    <td> <asp:TextBox ID="txtContacto" runat="server" Width="220px" TabIndex="2"></asp:TextBox> </td>
                    <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                ControlToValidate="txtContacto" CssClass="failureNotification" Display="Dynamic" 
                                                ErrorMessage="Contacto requerido" 
                                                ToolTip="Contacto requerido" 
                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                                src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                    </td>
                    </tr>
                    <tr>
                    
                        <td align="left"> <asp:Label ID="lblPais" runat="server" AssociatedControlID="ddlPais" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"></asp:Label> </td>
                        <td></td>
                        <td align="left"><asp:Label ID="lblEstado" runat="server" AssociatedControlID="ddlEstado" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"></asp:Label>
                        </td>
                        </tr>
                            <tr>
                                <td align="left"> <asp:DropDownList ID="ddlPais" runat="server" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" TabIndex="3" 
                                Width="223px">
                            </asp:DropDownList></td>
                                <td></td>
                                <td align="left"> 
                                <asp:DropDownList ID="ddlEstado" runat="server" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" TabIndex="4" 
                                Width="223px">
                            </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                            <td align="left"><asp:Label ID="lblMunicipio" runat="server" AssociatedControlID="ddlMunicipio" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>"></asp:Label></td>
                            <td></td>
                            <td align="left"> <asp:Label ID="lblLocalidad" runat="server" AssociatedControlID="txtLocalidad" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>"></asp:Label> </td>
                            </tr>
                                <tr>
                                    <td align="left"> <asp:DropDownList ID="ddlMunicipio" runat="server" TabIndex="5" Width="223px">
                            </asp:DropDownList></td>
                                    <td></td>
                                    <td align="left"> <asp:TextBox ID="txtLocalidad" runat="server" TabIndex="6" Width="220px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left">*<asp:Label ID="lblColonia" runat="server" AssociatedControlID="txtColonia" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>"></asp:Label></td>
                                    <td></td>
                                    <td align="left">*<asp:Label ID="lblCalle" runat="server" AssociatedControlID="txtCalle" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>"></asp:Label></td>
                                </tr>
                                    <tr>
                                    <td align="left"> <asp:TextBox ID="txtColonia" runat="server" TabIndex="7" Width="220px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                                                                ControlToValidate="txtColonia" CssClass="failureNotification" Display="Dynamic" 
                                                                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtColonia %>" 
                                                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, txtColonia %>" 
                                                                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                                                                                src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                    </td>
                                    <td></td>
                                    <td align="left"><asp:TextBox ID="txtCalle" runat="server" TabIndex="8" Width="220px"></asp:TextBox></td>
                                    <td align="left"> <asp:RequiredFieldValidator ID="rfvCalle" runat="server" 
                                ControlToValidate="txtCalle" CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>" 
                                ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator></td>
                                    </tr>
                                        <tr>
                                            <td align="left"> <asp:Label ID="lblNoExterior" runat="server" 
                                AssociatedControlID="txtNoExterior" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>"></asp:Label></td>
                                            <td></td>
                                            <td align="left"><asp:Label ID="lblCodigoPostal" runat="server" 
                                AssociatedControlID="txtCodigoPostal" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>"></asp:Label></td>
                                        </tr>
                                            <tr>
                                                <td align="left"> <asp:TextBox ID="txtNoExterior" runat="server" TabIndex="9" Width="220px"></asp:TextBox></td>
                                                <td></td>
                                                <td align="left"><asp:TextBox ID="txtCodigoPostal" runat="server" TabIndex="11" Width="220px"></asp:TextBox></td>
                                                <td align="left"><asp:RegularExpressionValidator ID="revCodigoPostal" runat="server" 
                                ControlToValidate="txtCodigoPostal" CssClass="failureNotification" 
                                ToolTip="Código postal erroneo" ValidationExpression="\d{5}" 
                                ValidationGroup="RegisterUserValidationGroup">
                                    <img src="../../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator></td>
                                            </tr>
                                                <tr>
                                                    <td align="left"><asp:Label ID="lblNoInterior" runat="server" 
                                                        AssociatedControlID="txtNoInterior" 
                                                        Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>"></asp:Label></td>
                                                    <td></td>
                                                    <td align="left">*<asp:Label ID="lblTelefono" runat="server" AssociatedControlID="txtTelefono" 
                                                                        Text="Telefono" /></td>
                                                </tr>
                                                    <tr>
                                                    <td align="left"><asp:TextBox ID="txtNoInterior" runat="server" TabIndex="10" Width="220px"></asp:TextBox></td>
                                                    <td></td>
                                                    <td align="left"> <asp:TextBox ID="txtTelefono" runat="server" TabIndex="13" Width="220px"></asp:TextBox></td>
                                                    <td align="left"> <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" 
                                                          ControlToValidate="txtTelefono" CssClass="failureNotification" 
                                                          Display="Dynamic" ToolTip="Telefono Requerido" 
                                                            ValidationGroup="RegisterUserValidationGroup"><img src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator> </td>
                                                    </tr>
                                                        <tr>
                                                            <td align="left">*<asp:Label ID="lblCorreo" runat="server" AssociatedControlID="txtCorreo" 
                                                                    Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>" /> </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                            <tr>
                                                            <td align="left"><asp:TextBox ID="txtCorreo" runat="server" TabIndex="12" Width="220px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                    ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                    ValidationGroup="RegisterUserValidationGroup"><img 
                                            src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td align="left">  </td>
                                                            <td></td>
                                                            </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblRecibiraCorreo" runat="server"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblRecibiraCorreo %>" ForeColor="#8B181B" Font-Bold="True"/> </td>
                                                                    <td></td>
                                                                    <td>  </td>
                                                                         <td></td>
                                                                         </tr>
                                                                                <tr>
                                                                                    <td align="left">*<asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" 
                                                                                        Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label></td>
                                                                                </tr>
                                                                                    <tr>
                                                                                    <td align="left"><asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry" MaxLength="50" TabIndex="14"
                                                                                            Width="220px"></asp:TextBox>
                                                                                            
                                                                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUsuario"
                                                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>"
                                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" ValidationGroup="RegisterUserValidationGroup"><img 
                                                                                src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator></td>
                                                                                            <td></td>
                                                                                    </tr>
                                                                                        <tr>
                                                                                            <td align="left"> <asp:Label ID="lblSeleccionSucursal" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionSucursal %>" /></td>
                                                                                        </tr>
                                                                                            <tr>
                                                                                                <td colspan="4">
                                                                                                
                                        <div style="overflow:auto; width: 550px; height:200px">
                                        <asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False" 
                                            BackColor="White" BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" 
                                            CellPadding="3" CellSpacing="1" DataKeyNames="id_sucursal" GridLines="None" 
                                            OnRowDataBound="GrvModulos_RowDataBound" 
                                            OnSelectedIndexChanged="GrvModulos_SelectedIndexChanged" Visible="False" 
                                            Width="500px" TabIndex="17">
                                            <Columns>
                                                <asp:TemplateField Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblidsucursal" runat="server" Text='<%# Bind("id_sucursal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblSucursal %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="empresa_sucursal" runat="server" 
                                                            Text='<%# Bind("empresa_sucursal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblUnica %>">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbUnica" runat="server" Enabled="false" 
                                                                        Checked='<%# Bind("factura_unica_mes") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbSeleccion" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <asp:Literal ID="Literal3" runat="server" 
                                                    Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                                            </EmptyDataTemplate>
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
                                    </div>
                                    </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="4"> <asp:Button ID="btnGuardarEmpresa" runat="server" CssClass="botonEstilo" 
                                    onclick="btnGuardarEmpresa_Click" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                                    ValidationGroup="RegisterUserValidationGroup" Width="80px" TabIndex="18" /></td>
                                    </tr>
                                                                                          
        </table>
                            
                                
                                        
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGuardarEmpresa" EventName="Click" />
            <%--<asp:PostBackTrigger ControlID="ddlEstado"/>
            <asp:PostBackTrigger ControlID="ddlPais" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

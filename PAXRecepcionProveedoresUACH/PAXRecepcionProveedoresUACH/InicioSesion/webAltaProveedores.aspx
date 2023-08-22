<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="webAltaProveedores.aspx.cs" Inherits="InicioSesion_webAltaProveedores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
<script type="text/javascript" language="javascript">
    function copiarContenido() {
        document.getElementById('<%=txtUsuario.ClientID %>').value = document.getElementById('<%=txtRfc.ClientID %>').value;
    }
</script>

    <style type="text/css">
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
        .modalBackground
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .style7
        {
            width: 200px;
        }
        .style4
        {
            width: 404px;
            height: 369px;
        }
        .style10
        {
            width: 405px;
            height: 369px;
        }
        .style11
        {
            width: 509px;
        }
    </style>
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <link href="../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <table style="border: solid 1px #C0C0C0;" class="bodyMain">
        <tr>
            <td>
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
                <table>
                    <tr>
                        <td>
                            <h2>
                                <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRegistroProv %>"></asp:Label>
                            </h2>
                            <h5>
                                <asp:Literal Visible="false" ID="ltrMensaje" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosRegistro %>" />
                            </h5>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="upAltaProveedor" runat="server">
                    <ContentTemplate>
                        <fieldset class="register" style="border: none">
                            <legend>
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubProveedorSuc %>" /></legend>
                            <table style="height: 418px; width: 730px;">
                                <tr>
                                    <td style="vertical-align: top;" class="style4">
                                        <p align="left">
                                            <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre" Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"
                                                CssClass="style1"></asp:Label>
                                            <asp:TextBox ID="txtNombre" runat="server" TabIndex="1" CssClass="style7"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNombre"
                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>"
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" ValidationGroup="RegisterUserValidationGroup"><img 
                                                src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblRfc" runat="server" AssociatedControlID="txtRFC" Text="<%$ Resources:resCorpusCFDIEs, lblRfc %>"
                                                CssClass="style1"></asp:Label>
                                            <asp:TextBox ID="txtRfc" runat="server"  TabIndex="2" 
                                                CssClass="style7" ontextchanged="txtRfc_TextChanged"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvRfc" runat="server" ControlToValidate="txtRfc"
                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtRfc %>"
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, txtRfc %>" ValidationGroup="RegisterUserValidationGroup"><img alt
                                                src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="regxRFC" runat="server" ControlToValidate="txtRfc"
                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxRFC %>"
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, regxRFC %>" ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"
                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblContacto" runat="server" AssociatedControlID="txtContacto" Text="<%$ Resources:resCorpusCFDIEs, lblContacto %>"
                                                CssClass="style1"></asp:Label>
                                            <asp:TextBox ID="txtContacto" runat="server" TabIndex="2" CssClass="style7"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtContacto"
                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="Contacto requerido"
                                                ToolTip="Contacto requerido" ValidationGroup="RegisterUserValidationGroup"><img 
                                                src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblPais" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"
                                                AssociatedControlID="ddlPais" CssClass="style1"></asp:Label>
                                            <asp:DropDownList ID="ddlPais" runat="server" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged"
                                                AutoPostBack="true" TabIndex="3" CssClass="style7">
                                            </asp:DropDownList>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblEstado" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"
                                                AssociatedControlID="ddlEstado"></asp:Label>
                                            <asp:DropDownList ID="ddlEstado" runat="server" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged"
                                                AutoPostBack="true" TabIndex="4" CssClass="style7">
                                            </asp:DropDownList>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblMunicipio" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>"
                                                AssociatedControlID="ddlMunicipio"></asp:Label>
                                            <asp:DropDownList ID="ddlMunicipio" runat="server" TabIndex="5" CssClass="style7">
                                            </asp:DropDownList>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblLocalidad" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>"
                                                AssociatedControlID="txtLocalidad"></asp:Label>
                                            <asp:TextBox ID="txtLocalidad" runat="server" TabIndex="6" CssClass="style7"></asp:TextBox>
                                        </p>
                                        <p align="left">
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUsuario"
                                    CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>"
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" ValidationGroup="RegisterUserValidationGroup"><img 
                                                 src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>--%>
                                            <%--  <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtUsuario"
                                    CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtUsuario %>"
                                    ValidationExpression="(?=^.{8,}$).*$" ValidationGroup="RegisterUserValidationGroup"
                                    Height="16px"><img src="../Imagenes/error_sign.gif" /> 
                                </asp:RegularExpressionValidator> --%>
                                            <asp:Label ID="lblCodigo" runat="server" AssociatedControlID="txtCodigo" CssClass="style1"
                                                Text="<%$ Resources:resCorpusCFDIEs, lblCodigo %>"></asp:Label>
                                            <asp:TextBox ID="txtCodigo" runat="server" CssClass="style7" TabIndex="8"></asp:TextBox>
                                        </p>
                                    </td>
                                    <td style="vertical-align: top;" class="style10">
                                        <p align="left">
                                            <asp:Label ID="lblCalle" runat="server" AssociatedControlID="txtCalle" CssClass="style1"
                                                Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>"></asp:Label>
                                            <asp:TextBox ID="txtCalle" runat="server" TabIndex="9" CssClass="style7"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCalle"
                                                CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>"
                                                ValidationGroup="RegisterUserValidationGroup" Width="16px"><img alt
                                                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblNoExterior" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>"
                                                AssociatedControlID="txtNoExterior"></asp:Label>
                                            <asp:TextBox ID="txtNoExterior" runat="server" TabIndex="10" CssClass="style7"></asp:TextBox>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblNoInterior" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>"
                                                AssociatedControlID="txtNoInterior"></asp:Label>
                                            <asp:TextBox ID="txtNoInterior" runat="server" TabIndex="11" CssClass="style7"></asp:TextBox>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblColonia" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>"
                                                AssociatedControlID="txtColonia"></asp:Label>
                                            <asp:TextBox ID="txtColonia" runat="server" TabIndex="12" CssClass="style7"></asp:TextBox>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblCodigoPostal" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>"
                                                AssociatedControlID="txtCodigoPostal"></asp:Label>
                                            <asp:TextBox ID="txtCodigoPostal" runat="server" TabIndex="13" CssClass="style7"
                                                ValidationGroup="RegisterUserValidationGroup"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCodigoPostal"
                                                CssClass="failureNotification" ToolTip="Código postal erroneo" ValidationGroup="RegisterUserValidationGroup"
                                                ValidationExpression="\d{5}">
                                    <img src="../Imagenes/error_sign.gif" />
                                            </asp:RegularExpressionValidator>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblCorreo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEmailCorreo %>"
                                                AssociatedControlID="txtCorreo" />
                                            <asp:TextBox ID="txtCorreo" runat="server" TabIndex="14" CssClass="style7" ValidationGroup="RegisterUserValidationGroup"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtCorreo"
                                                CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>"
                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                            src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtCorreo"
                                                CssClass="failureNotification" Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="RegisterUserValidationGroup"><img 
                                            src="../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblTelefono" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTelefono %>"
                                                AssociatedControlID="txtTelefono" />
                                            <asp:TextBox ID="txtTelefono" runat="server" TabIndex="15" CssClass="style7" ValidationGroup="RegisterUserValidationGroup"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtTelefono"
                                                CssClass="failureNotification" Display="Dynamic" ToolTip="Telefono Requerido"
                                                ValidationGroup="RegisterUserValidationGroup"><img 
                                            src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                        </p>
                                        <p align="left">
                                            <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" Text="<%$ Resources:resCorpusCFDIEs, lblUser %>"></asp:Label>
                                            <asp:TextBox ID="txtUsuario" Enabled="false" runat="server" CssClass="style7" MaxLength="50" 
                                                TabIndex="7" ReadOnly="True"></asp:TextBox>
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="txtTelefono" Text="<%$ Resources:resCorpusCFDIEs, lblSeleccioneSucursal %>" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Panel ID="pnlgvSucursales" runat="server" Width="460px">
                                                        <asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                            BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" CellSpacing="1"
                                                            DataKeyNames="id_sucursal" GridLines="None" OnPageIndexChanging="gvSucursales_PageIndexChanging"
                                                            OnRowDataBound="GrvModulos_RowDataBound" OnSelectedIndexChanged="GrvModulos_SelectedIndexChanged"
                                                            PageSize="3" TabIndex="17" Visible="False" Width="358px">
                                                            <Columns>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblidsucursal" runat="server" Text='<%# Bind("id_sucursal") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblSucursal %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="empresa_sucursal" runat="server" Text='<%# Bind("empresa_sucursal") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Factura Única Mes" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbUnica" runat="server" Checked='<%# Bind("factura_unica_mes") %>'
                                                                            Enabled="false" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbSeleccion" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                                                            </EmptyDataTemplate>
                                                            <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                                                            <HeaderStyle BackColor="#880085" Font-Bold="True" ForeColor="#E7E7FF" />
                                                            <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                                                            <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                                                            <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                            <SortedAscendingHeaderStyle BackColor="#594B9C" />
                                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                            <SortedDescendingHeaderStyle BackColor="#33276A" />
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnGuardarEmpresa" runat="server" CssClass="botonEstilo" OnClick="btnGuardarEmpresa_Click"
                                            TabIndex="18" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" ValidationGroup="RegisterUserValidationGroup"
                                            Width="104px" Height="28px" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td align="left">
                                        <script language="javascript" type="text/javascript">

                                            var ModalProgress = '<%= modalGenerando.ClientID %>';         
                                        </script>
                                        <asp:Panel ID="pnlGenerando" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                            CssClass="modal" Width="300px">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="style11">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varProcesando %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style11">
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
                                            DropShadow="true" EnableViewState="False" PopupControlID="pnlGenerando" PopupDragHandleControlID=""
                                            TargetControlID="pnlGenerando">
                                        </cc1:ModalPopupExtender>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>

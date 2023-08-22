<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/webOperatorMaster.master" CodeFile="webOperacionNominaEmpleados.aspx.cs" Inherits="Operacion_Empleados_webOperacionNominaEmpleados" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloEmpleado %>"></asp:Label>
    </h2>
    <div style="text-align:center;">
        <asp:UpdateProgress ID="uppConsultas" runat="server" AssociatedUpdatePanelID="upDatosEmpleado">
            <progresstemplate>
                <img alt="" src="../../Imagenes/imgAjaxLoader.gif" />
            </progresstemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upDatosEmpleado" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdId_Empleado" runat="server" Visible="False"></asp:HiddenField>
            <asp:Panel ID="pnlDatosEmpleado" runat="server">
                <fieldset class="register" style="width:890px;">
                    <legend>
                        <asp:Literal ID="litDatosEmpleado" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblDatosEmpleado %>" />
                    </legend>
                    <table>
                        <tr>
                            <td style="width:300px;">
                                <asp:Label ID="lblRFC" runat="server" AssociatedControlID="txtRFC" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>">
                                </asp:Label>
                            </td>
                            <td style="width:32px;">
                            </td>
                            <td style="width:300px;">
                                <asp:Label ID="lblFechaInicioRelacionLaboral" runat="server" 
                                    AssociatedControlID="txtFechaInicioRelacionLaboral" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaInicioRelacionLaboral %>">
                                </asp:Label>
                            </td>
                            <td style="width:32px;">
                            </td>
                            <td rowspan="22">
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtRFC" runat="server" CssClass="textEntry" MaxLength="13" 
                                    TabIndex="1" ToolTip="AAA000000AAA" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:RegularExpressionValidator ID="regxRFC" runat="server" 
                                    ControlToValidate="txtRFC" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxRFC %>" 
                                    ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]" 
                                    ValidationGroup="grupoEmpleado">
                                    <img alt="" src="../../Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvRfc" runat="server" 
                                    ControlToValidate="txtRFC" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valRfc %>" 
                                    ValidationGroup="grupoEmpleado" Width="16px">
                                    <img alt="" src="../../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFechaInicioRelacionLaboral" runat="server" BackColor="White" 
                                    Width="100px" TabIndex="11" Enabled="false">
                                </asp:TextBox>
                                <asp:Image ID="imgIni" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" />
                                <asp:RegularExpressionValidator ID="revFechaInicioRelacionLaboral" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaInicioRelacionLaboral" CssClass="failureNotification" 
                                    ValidationExpression="^(?:(?:0?[1-9]|1\d|2[0-8])(\/|-)(?:0?[1-9]|1[0-2]))(\/|-)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(?:(?:31(\/|-)(?:0?[13578]|1[02]))|(?:(?:29|30)(\/|-)(?:0?[1,3-9]|1[0-2])))(\/|-)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(29(\/|-)0?2)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$" 
                                    ValidationGroup="grupoEmpleado" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>">
                                    <img alt="" src="../Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>
                                <cc1:CalendarExtender ID="FechaInicioRelacionLaboralCalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaInicioRelacionLaboral" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgIni">
                                </cc1:CalendarExtender>
                            </td>
                            <td>
                       
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNombre" runat="server" 
                                    AssociatedControlID="txtNombre" Enabled="false" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblNombreEmpleado %>">
                                </asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblPuesto" runat="server" AssociatedControlID="txtPuesto" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblPuesto %>">
                                </asp:Label>
                             </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="textEntry" 
                                    MaxLength="255" TabIndex="2" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                    ControlToValidate="txtNombre" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valRazonSocial %>" 
                                    ValidationGroup="grupoEmpleado" Width="16px">
                                <img alt="" src="../../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPuesto" runat="server" CssClass="textEntry" 
                                    MaxLength="100" TabIndex="12" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblRegistroPatronal" runat="server" AssociatedControlID="txtCorreo" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblRegistroPatronal %>">
                                </asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblTipoContrato" runat="server" 
                                    AssociatedControlID="txtTipoContrato" Text="<%$ Resources:resCorpusCFDIEs, lblTipoContrato %>">
                                </asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtRegistroPatronal" runat="server" CssClass="textEntry" 
                                    MaxLength="20" TabIndex="3" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:TextBox ID="txtTipoContrato" runat="server" CssClass="textEntry" 
                                    MaxLength="100" TabIndex="13" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNumeroEmpleado" runat="server" 
                                    AssociatedControlID="txtNumeroEmpleado" Text="<%$ Resources:resCorpusCFDIEs, lblNumeroEmpleado %>">
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:Label ID="lblTipoJornada" runat="server" 
                                    AssociatedControlID="txtTipoJornada" Text="<%$ Resources:resCorpusCFDIEs, lblTipoJornada %>">
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtNumeroEmpleado" runat="server" CssClass="textEntry" 
                                    MaxLength="15" TabIndex="4" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvNumeroEmpleado" runat="server" 
                                    ControlToValidate="txtNumeroEmpleado" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valNumeroEmpleado %>" 
                                    ValidationGroup="grupoEmpleado" Width="16px">
                                <img alt="" src="../../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTipoJornada" runat="server" CssClass="textEntry" 
                                    MaxLength="100" TabIndex="14" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCURP" runat="server" 
                                    AssociatedControlID="txtCURP" Text="<%$ Resources:resCorpusCFDIEs, lblCURP %>">
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:Label ID="lblSalarioBase" runat="server" 
                                    AssociatedControlID="txtSalarioBase" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblSalarioBase %>">
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtCURP" runat="server" CssClass="textEntry" 
                                    MaxLength="18" TabIndex="5" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvCURP" runat="server" 
                                    ControlToValidate="txtCURP" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valCURP %>" 
                                    ValidationGroup="grupoEmpleado" Width="16px">
                                <img alt="" src="../../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revCURP" runat="server" 
                                    ControlToValidate="txtCURP" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCURP %>" 
                                    ValidationExpression="[A-Z][A,E,I,O,U,X][A-Z]{2}[0-9]{2}[0-1][0-9][0-3][0-9][M,H][A-Z]{2}[B,C,D,F,G,H,J,K,L,M,N,Ñ,P,Q,R,S,T,V,W,X,Y,Z]{3}[0-9,A-Z][0-9]" 
                                    ValidationGroup="grupoEmpleado">
                                    <img alt="" src="../../Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSalarioBase" runat="server" CssClass="textEntry" 
                                    Enabled="false" MaxLength="255" TabIndex="16">
                                </asp:TextBox>
                            </td>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTipoRegimen" runat="server" 
                                    AssociatedControlID="txtNumeroEmpleado" Text="<%$ Resources:resCorpusCFDIEs, lblTipoRegimen %>">
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:Label ID="lblSalarioDiarioIntegrado" runat="server" 
                                    AssociatedControlID="txtSalarioDiarioIntegrado" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblSalarioDiarioIntegrado %>"></asp:Label>
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlTipoRegimen" runat="server" 
                                    DataTextField="Descripcion" DataValueField="Clave" TabIndex="6" 
                                    Width="300px" Enabled="false" OnDataBound="ddlTipoRegimen_DataBound">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvTipoRegimen" runat="server" 
                                    ControlToValidate="ddlTipoRegimen" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valTipoRegimen %>" 
                                    ValidationGroup="grupoEmpleado" Width="16px" InitialValue="0">
                                <img alt="" src="../../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSalarioDiarioIntegrado" runat="server" CssClass="textEntry" 
                                    Enabled="false" MaxLength="255" TabIndex="16"></asp:TextBox>
                            </td>
                            <td>
                                <cc1:FilteredTextBoxExtender ID="ftbeSalarioBase" runat="server" 
                                    FilterType="Numbers, Custom" TargetControlID="txtSalarioBase" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNumeroSeguridadSocial" runat="server" 
                                    AssociatedControlID="txtNumeroSeguridadSocial" Text="<%$ Resources:resCorpusCFDIEs, lblNumeroSeguridadSocial %>">
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:Label ID="lblRiesgoPuesto" runat="server" 
                                    AssociatedControlID="ddlRiesgoPuesto" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblRiesgoPuesto %>">
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtNumeroSeguridadSocial" runat="server" CssClass="textEntry" 
                                    MaxLength="15" TabIndex="7" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:DropDownList ID="ddlRiesgoPuesto" runat="server" 
                                    DataTextField="Descripcion" DataValueField="Clave" Enabled="false" 
                                    ondatabound="ddlRiesgoPuesto_DataBound" TabIndex="17" Width="300px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <cc1:FilteredTextBoxExtender ID="ftbSalarioDiarioIntegrado" runat="server" 
                                    FilterType="Numbers, Custom" TargetControlID="txtSalarioDiarioIntegrado" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDepartamento" runat="server" 
                                    AssociatedControlID="txtDepartamento" Text="<%$ Resources:resCorpusCFDIEs, lblDepartamento %>">
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:Label ID="lblCorreo" runat="server" AssociatedControlID="txtCorreo" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>">
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtDepartamento" runat="server" CssClass="textEntry" 
                                    MaxLength="100" TabIndex="8" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="textEntry" Enabled="false" 
                                    MaxLength="255" TabIndex="19">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:RegularExpressionValidator ID="regxCorreo" runat="server" 
                                    ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valCorreo %>" 
                                    ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" 
                                    ValidationGroup="grupoEmpleado" Width="16px">
                                    <img alt="" src="../../Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCLABE" runat="server" AssociatedControlID="txtCLABE" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblCLABE %>">
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:Label ID="lblNombreSucursal" runat="server" 
                                    AssociatedControlID="ddlSucursales" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>">
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtCLABE" runat="server" CssClass="textEntry" MaxLength="18" 
                                    TabIndex="9" Enabled="false">
                                </asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbeCLABE" runat="server" 
                                    FilterType="Numbers" TargetControlID="txtCLABE">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:DropDownList ID="ddlSucursales" runat="server" DataTextField="nombre" AutoPostBack="true"
                                    DataValueField="id_estructura" Enabled="false" TabIndex="20" Width="300px" 
                                    OnSelectedIndexChanged="ddlSucursales_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvSucursales" runat="server" 
                                    ControlToValidate="ddlSucursales" CssClass="failureNotification" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                    ValidationGroup="grupoEmpleado" Width="16px">
                                    <img alt="" src="../../Imagenes/error_sign.gif" />
                                </asp:RequiredFieldValidator>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBanco" runat="server" AssociatedControlID="ddlBanco" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblBanco %>">
                                </asp:Label>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblPeriodos %>">

                        </asp:Label>
                            </td>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlBanco" runat="server"
                                    DataTextField="NombreCorto" DataValueField="Clave" TabIndex="10" 
                                    Width="300px" Enabled="false" ondatabound="ddlBanco_DataBound">
                                </asp:DropDownList>
                            </td>
                            <td>
                                </td>
                            <td>
                                <asp:DropDownList ID="ddlPeriodos" runat="server" DataTextField="Descripcion" 
                                    DataValueField="IdTipoPeriodo" Enabled="false" TabIndex="20" Width="300px" 
                                    OnDataBound="ddlPeriodos_DataBound">
                                </asp:DropDownList>
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
                                <asp:Label ID="lblEstatus" runat="server" AssociatedControlID="ddlEstatus" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblEstatus %>">
                                </asp:Label>
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
                                <asp:DropDownList ID="ddlEstatus" runat="server" DataTextField="Descripcion" 
                                    DataValueField="Estatus" Enabled="false" TabIndex="20" Width="300px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </fieldset>
                <p style="text-align:right;">
                    <asp:Button ID="btnNuevo" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnNuevo %>" TabIndex="30"
                        CssClass="botonEstilo" OnClick="btnNuevo_Click" />
                    <asp:Button ID="btnGuardar" runat="server"  ValidationGroup="grupoEmpleado" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" TabIndex="31" Enabled="false"
                        CssClass="botonEstilo" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                        Visible="true" TabIndex="32" CssClass="botonEstilo" 
                        OnClick="btnCancelar_Click" />
                </p>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlBusqueda" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlBusqueda" runat="server" GroupingText="<%$ Resources:resCorpusCFDIEs, lblBuscar %>">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblNumeroEmpleadoBusqueda" runat="server" 
                                AssociatedControlID="txtNumeroEmpleadoBusqueda" Text="<%$ Resources:resCorpusCFDIEs, lblNumeroEmpleado %>">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNumeroEmpleadoBusqueda" runat="server" CssClass="textEntry" 
                                MaxLength="20" TabIndex="33" Enabled="true">
                             </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRFCBusqueda" runat="server" AssociatedControlID="txtRFCBusqueda" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRFCBusqueda" runat="server" CssClass="textEntry" MaxLength="13" 
                                TabIndex="34" ToolTip="AAA000000AAA" Enabled="true">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbnlNombreBusqueda" runat="server" 
                                AssociatedControlID="txtNombre" Enabled="false" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblNombreEmpleado %>">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNombreBusqueda" runat="server" CssClass="textEntry" 
                                MaxLength="255" TabIndex="35" Enabled="true">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSucursal" runat="server" 
                                AssociatedControlID="ddlSucursalBusqueda" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSucursalBusqueda" runat="server" 
                                DataTextField="nombre" DataValueField="id_estructura" TabIndex="20" 
                                Width="300px" Enabled="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:Button ID="btnConsultar" runat="server" CssClass="botonEstilo" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                                Width="80px" OnClick="btnConsultar_Click" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="text-align:right;">
                            <asp:Label ID="lblNumeroRegistros" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblNumRegPag%>"
                                Visible="false">
                            </asp:Label>
                            <asp:DropDownList ID="ddlNumeroRegistros" runat="server" 
                                Visible="false" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlNumeroRegistros_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gdvEmpleados" runat="server" AutoGenerateColumns="False" 
                                CellPadding="4" GridLines="Horizontal" Width="800px" 
                                DataKeyNames="Id_Empleado"  AllowPaging="False" 
                                BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                                BackColor="White" AllowSorting="True" 
                                OnSorting="gdvEmpleados_Sorting" 
                                OnSelectedIndexChanged="gdvEmpleados_SelectedIndexChanged">
                                <Columns>
                                    <asp:CommandField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                                        SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>"
                                        ShowSelectButton="True">
                                    <ItemStyle Width="50" HorizontalAlign="Left" />
                                    </asp:CommandField>
                                    <asp:BoundField DataField="RFC" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblRfc %>" ItemStyle-Width="100" 
                                        HeaderStyle-HorizontalAlign="Left" SortExpression="RFC">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nombre" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblNombreEmpleado %>" 
                                        HeaderStyle-HorizontalAlign="Left" SortExpression="Nombre">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NumEmpleado" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblNumeroEmpleado %>" 
                                        HeaderStyle-HorizontalAlign="Left" SortExpression="NumEmpleado">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DescripcionEstatus" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus %>" 
                                        HeaderStyle-HorizontalAlign="Left" SortExpression="DescripcionEstatus">
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
                            <table align="right">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnAnterior" runat="server" CssClass="botonEstilo" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lbAnterior %>" 
                                            OnClick="btnAnterior_Click" Visible="false" />
                                        <asp:Button ID="btnSiguiente" runat="server" CssClass="botonEstilo" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblSiguiente %>" 
                                            OnClick="btnSiguiente_Click" Visible="false" />
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
<%@ Page Title="" Theme="Alitas" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webOperacionDatosFiscales.aspx.cs" Inherits="Cuenta_WebOperacionDatosFiscales" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

 <link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
 <link href="../App_Themes/Alitas/tema_dinamico.css" rel="stylesheet" type="text/css" />

 
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<link href="../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">

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

    function fntraerEnter(keyStroke) {
        isNetscape = (document.layers);
        eventChooser = (isNetscape) ? keyStroke.which : event.keyCode;
        if (eventChooser == 13) {
            return false;
        }
    }
    document.onkeypress = fntraerEnter; 
   
 </script> 
 <asp:UpdatePanel runat="server">
 <ContentTemplate>
 <center>
    <table style="width:952px; text-align:left; background-color:Black">
        <tr>
            <td class="Titulo">
            <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, hedDatosFisc %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:952px; height:0.5px; background-color:#fff000"></td>
        </tr>
        <tr>
            <td class="Subtitulos">
            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFranquicia %>" />
            </td>
        </tr>
    </table>
 </center>
 <center>
    <table class="background_tablas_transparente" style="width:952px">
        <tr>
            <td>
    <fieldset class="register" style=" width:890px; border-color:transparent;">
    <table width="885">
            <tr>
                <td colspan="0" rowspan="0" align="left">
                    <asp:TreeView ID="trvEstructura" runat="server" BorderStyle="Solid" SkinId="SkinTreeView"
                        BorderWidth="1px" ImageSet="News" 
                        Width="300px" NodeIndent="10" ShowLines="True" 
                        onselectednodechanged="trvEstructura_SelectedNodeChanged" TabIndex="23">
                        <HoverNodeStyle Font-Underline="True" />
                        <NodeStyle Font-Names="Verdana" Font-Size="11pt" ForeColor="Black" HorizontalPadding="5px"
                                                NodeSpacing="0px" VerticalPadding="0px" />
                        <ParentNodeStyle Font-Bold="False" />
                        <RootNodeStyle />
                        <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px"
                                           BackColor="#ECECEC" />
                    </asp:TreeView>
                </td>
               <td>
               <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblNodoSel" runat="server" SkinId="labelLarge" style="font-weight: 700" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblFranquicia %>"></asp:Label> <%--lblNodoSelEstructura--%>
                        </td>
                        <td>
                            <asp:Label ID="lblSelVal" runat="server" SkinId="labelLarge" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblNinguno %>"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblNombreNodo" runat="server" SkinId="labelLarge" style="font-weight: 700" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblNodoNombre %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNombreNodo" runat="server" MaxLength="255"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNombreNodo" runat="server" 
                                ControlToValidate="txtNombreNodo" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, lblCampoObligatorio %>" 
                                ValidationGroup="AgregarNodoValidationGroup" Width="16px" 
                                Display="Dynamic">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Button ID="btnAgregar" runat="server" CssClass="botonEstilo" 
                                Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
                                Width="80px" TabIndex="1" onclick="btnAgregar_Click" 
                                ValidationGroup="AgregarNodoValidationGroup" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdnValuePath" runat="server" />
                        </td>
                        <td>
                            <asp:HiddenField ID="hdnSel" runat="server" />
                        </td>
                        <td>
                            <asp:HiddenField ID="hdnSelVal" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">

                        </td>
                    </tr>
                </table>
                <%--<table width="100%" align="right" style="height: 158px">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblVerNombreMostrar" runat="server" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:ImageMap ID="immImagenMostrar" runat="server" Height="136px" Width="284px" 
                                TabIndex="2"></asp:ImageMap>        
                        </td>
                    </tr>
                </table>--%>
               </td>
            </tr>
        </table>
    </fieldset>
    <p align="right">
        <asp:Button ID="btnNuevo" runat="server" CssClass="botonEstilo" 
           Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" 
            Width="80px" TabIndex="2" onclick="btnNuevo_Click" />
            <asp:Button ID="btnBorrar" runat="server" CssClass="botonEstilo" 
            Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" 
             Width="80px" TabIndex="3" onclick="btnBorrar_Click" />
            <asp:Button ID="btnEditar" runat="server" CssClass="botonEstilo" 
            Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
            Width="80px" TabIndex="4" onclick="btnEditar_Click" />
            <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" 
            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" 
                TabIndex="5" onclick="btnNCancelar_Click" />
    </p>
            
            </td>
        </tr>
    </table>
 </center>
 <center>
    <table style="width:952px; text-align:left; background-color:Black">
        <tr>
            <td class="Subtitulos">
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubCuenta %>" />
            </td>
        </tr>
    </table>
 </center>
 <center>
    <table class="background_tablas_transparente" style="width:952px">
        <tr>
            <td>
            <div>
    <fieldset class="register" style=" height:600px; width:890px; border-color:transparent;" >
    <table>
    <tr>
    <td style="width:390px; vertical-align:top;" >
            <p>
                <asp:Label ID="lblRazonSocial" runat="server" AssociatedControlID="txtRazonSocial" SkinId="labelLarge"
                    Text="<%$ Resources:resCorpusCFDIEs, lblRazonSocial %>"></asp:Label>
                <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="textEntry" 
                    MaxLength="255" Enabled="False" Width="300px" ReadOnly="True"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="Label1" runat="server" AssociatedControlID="txtIDSucursal" SkinId="labelLarge"
                     Text="<%$ Resources:resCorpusCFDIEs, lblIdSucursal %>" ></asp:Label>
                <asp:TextBox ID="txtIdSucursal" runat="server" CssClass="textEntry" 
                     MaxLength="255" TabIndex="6" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvIdSucursal" runat="server" 
                    ControlToValidate="txtIdsucursal" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                    ValidationGroup="RegisterUserValidationGroup" Width="16px" Display="Dynamic"><img 
                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revIdSucursal" runat="server" 
                    ControlToValidate="txtIdSucursal" CssClass="failureNotification" Display="Dynamic" 
                    ToolTip="Debe ser un número" 
                    ValidationExpression="^[0-9]*" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
            </p>
            <p>
                <asp:Label ID="lblFranquicia" runat="server" AssociatedControlID="txtFranquicia" SkinId="labelLarge"
                    Text="<%$ Resources:resCorpusCFDIEs, lblFranquicia %>"></asp:Label>
                <asp:TextBox ID="txtFranquicia" runat="server" CssClass="textEntry" 
                    MaxLength="255" TabIndex="7" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSucursal" runat="server" 
                    ControlToValidate="txtFranquicia" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
            </p>
            
            <p>
                <asp:Label ID="lblPais" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblPais %>" SkinId="labelLarge"
                    AssociatedControlID="ddlPais"></asp:Label>
                <asp:DropDownList ID="ddlPais" runat="server" Width="300px" AutoPostBack="True" 
                    DataTextField="pais" DataValueField="id_pais" 
                    TabIndex="9" onselectedindexchanged="ddlPais_SelectedIndexChanged"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvPais" runat="server" 
                    ControlToValidate="ddlPais" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valPais %>" 
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="lblEstado" runat="server"
                    Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>" SkinId="labelLarge"
                    AssociatedControlID="ddlEstado" ></asp:Label>
                    <asp:DropDownList ID="ddlEstado" runat="server" Width="300px" 
                    DataTextField="estado" DataValueField="id_estado" TabIndex="10"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvEstado" runat="server" 
                    ControlToValidate="ddlEstado" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valEstado %>" 
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="lblMunicipio" runat="server"
                    Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>" SkinId="labelLarge"
                    AssociatedControlID="txtMunicipio" ></asp:Label>
                <asp:TextBox ID="txtMunicipio" runat="server" CssClass="textEntry" 
                    MaxLength="255" TabIndex="11" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMunicipio" runat="server" 
                    ControlToValidate="txtMunicipio" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valMunicipio %>" 
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
            </p><p>
                <asp:Label ID="lblLocalidad" runat="server"
                    Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>" SkinId="labelLarge"
                    AssociatedControlID="txtLocalidad" ></asp:Label>
                <asp:TextBox ID="txtLocalidad" runat="server" CssClass="textEntry" 
                    MaxLength="255" TabIndex="12" Width="300px"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblRegimenFiscal" runat="server" SkinId="labelLarge"
                    Text="<%$ Resources:resCorpusCFDIEs, lblRegimenfiscal %>" 
                    AssociatedControlID="txtRegimenFiscal" ></asp:Label>
                <asp:TextBox ID="txtRegimenFiscal" runat="server" CssClass="textEntry" 
                    TabIndex="13" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvRegimenFiscal" runat="server" 
                    ControlToValidate="txtRegimenFiscal" CssClass="failureNotification" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, valRegimenFiscal %>"
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="lblCalle" runat="server" SkinId="labelLarge"
                    Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>" 
                    AssociatedControlID="txtCalle"></asp:Label>
                <asp:TextBox ID="txtCalle" runat="server" CssClass="textEntry" MaxLength="255" 
                    TabIndex="14" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCalle" runat="server" 
                    ControlToValidate="txtCalle" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>" 
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
            </p>
    </td>
    <td style="vertical-align:top; width:390px;" >
            
            <p>
                <asp:Label ID="lblNoExterior" runat="server" SkinId="labelLarge"  
                    Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>" 
                    AssociatedControlID="txtNoExterior"></asp:Label>
                <asp:TextBox ID="txtNoExterior" runat="server" CssClass="textEntry" 
                    MaxLength="10" TabIndex="15" Width="300px"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblNoInterior" runat="server" SkinId="labelLarge"  
                    Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>" 
                    AssociatedControlID="txtNoInterior"></asp:Label>
                <asp:TextBox ID="txtNoInterior" runat="server" CssClass="textEntry" 
                    MaxLength="10" TabIndex="16" Width="300px"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblColonia" runat="server" SkinId="labelLarge"
                    Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>" 
                    AssociatedControlID="txtColonia"></asp:Label>
                <asp:TextBox ID="txtColonia" runat="server" CssClass="textEntry" 
                    MaxLength="255" TabIndex="17" Width="300px"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblReferenciaDom" runat="server" SkinId="labelLarge"
                    Text="<%$ Resources:resCorpusCFDIEs, lblReferenciaDom %>" 
                    AssociatedControlID="txtReferencia" ></asp:Label>
                <asp:TextBox ID="txtReferencia" runat="server" CssClass="textEntry" 
                    MaxLength="255" TabIndex="13" Width="300px"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="LablblCodigoPostalel5" runat="server" SkinId="labelLarge"
                    Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>" 
                    AssociatedControlID="txtCodigoPostal" ></asp:Label>
                <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="textEntry" 
                    MaxLength="5" TabIndex="18" Width="300px"></asp:TextBox>
                <%--<cc1:filteredtextboxextender ID="txtCodigoPostal_fteSoloNumeros" runat="server" 
                    Enabled="True" FilterType="Numbers" TargetControlID="txtCodigoPostal">
                </cc1:filteredtextboxextender>--%>
                <asp:RequiredFieldValidator ID="rfvCodigoPostal" runat="server" 
                    ControlToValidate="txtCodigoPostal" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCodigoPostal %>" 
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
            </p>
            
            <p> 
                <asp:Label ID="lblRegla" runat ="server" SkinId="labelLarge" Text ="<%$  Resources:resCorpusCFDIEs,lblReglaNe%>"></asp:Label>
                <br />
                <asp:DropDownList ID="ddlReglaNegocio" runat="server"
                        DataTextField="Regla" DataValueField="idRegla" AutoPostBack="True" 
                    TabIndex="19" Width="300px">
                </asp:DropDownList>
            </p>
            <p>
                <asp:Label ID="lblRfc" runat="server" SkinId="labelLarge" Text="<%$ Resources:resCorpusCFDIEs,lblRFC %>"
                    AssociatedControlID="ddlRfc" ></asp:Label>
                <asp:DropDownList ID="ddlRfc" runat="server" Height="19px" Width="300px" 
                    DataTextField="rfc" DataValueField="id_rfc" TabIndex="20" 
                    onselectedindexchanged="ddlRfc_SelectedIndexChanged" AutoPostBack="True" >
                </asp:DropDownList>
            </p>
            
            <p>
                <asp:Label ID="lblNombreSucursal0" runat="server" SkinId="labelLarge"
                    Text="<%$ Resources:resCorpusCFDIEs, lbllogo %>"></asp:Label>
                <br />
                <%--<asp:ImageButton ID="imgLogo" runat="server" Height="22px" ImageUrl="~/Imagenes/lupa.png" Width="22px" 
                    TabIndex="16" Enabled="false" />--%>
                <asp:FileUpload ID="fupLogo" runat="server" TabIndex="21"/>
            </p>
            <p>
                <asp:Label ID="lblimgTicket" runat="server" SkinId="labelLarge"
                    Text="<%$ Resources:resCorpusCFDIEs, lblImgTicket %>"></asp:Label>
                <br />
                <asp:FileUpload ID="fupImgTicket" runat="server" TabIndex="22"/>
            </p>
    </td>
    </tr>
    </table>
    </fieldset>
    </div>
            </td>
        </tr>
    </table>
 </center>
    <center>
    <table class="background_tablas_transparente" style="width:952px">
        <tr>
            <td align="right">
                <asp:Button ID="btnActualizarDomicilio" runat="server" CommandName="MoveNext"  CssClass="botonEstilo" 
                    ValidationGroup="RegisterUserValidationGroup" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                    TabIndex="23" onclick="btnActualizarDomicilio_Click" />
            </td>
        </tr>
    </table>
    </center>
    <%--<p style=" padding-left:780px;">
        
    </p>--%>
<cc1:modalpopupextender id="modalGenerando" runat="server" backgroundcssclass="modalBackground" 
popupcontrolid="pnlGenerando" popupdraghandlecontrolid=""
    targetcontrolid="pnlGenerando">
</cc1:modalpopupextender>
<script type="text/javascript" language="javascript">
    var ModalProgress = '<%= modalGenerando.ClientID %>';
            </script>
<asp:Panel ID="pnlGenerando" runat="server" Width="300px" 
    CssClass="modal" BorderStyle="Solid" BorderWidth="1px">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="Label3" SkinId="labelLarge" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varProcesando %>"></asp:Label>
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
<%-- Modalpoup Avisos --%> 
<asp:LinkButton ID="lkbAviso" runat="server"></asp:LinkButton>
<cc1:ModalPopupExtender ID="mpeAvisos" runat="server"
    TargetControlID ="lkbAviso" 
    PopupControlID ="pnlAvisos" 
    BackgroundCssClass ="modalBackground">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlAvisos" runat="server"
CssClass ="modal" >
        <table cellpadding="0" cellspacing="0">
        <tr>
            <td class="TablaBackGround">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <br />
                <table>
                    <tr>
                    <td>
                        <img alt="" class="imgInformacion" src="../Imagenes/Informacion.png" />
                    </td>
                    <td align="center">
                        <asp:Label ID="lblAviso" runat="server" SkinID="labelLarge"></asp:Label>
                    </td>
                    <td>    
                
                    </td>
                    </tr>
                    <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                    
                    </td>
                    <td>
                        <asp:Button ID="btnAviso" runat="server" CssClass="botonEstilo" Text ="OK" />
                    </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="btnActualizarDomicilio" />
</Triggers>
 </asp:UpdatePanel>
</asp:Content>


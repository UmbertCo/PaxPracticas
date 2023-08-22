<%@ Page Title="Estructuras" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webOperacionSucursales.aspx.cs" Inherits="Operacion_Sucursales_webOperacionSucursales" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
     <%--<asp:FileUpload ID="fupLogo" runat="server" TabIndex="15" />--%>
<script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script src="../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery.ui.core.js" type="text/javascript"></script>
<script src="../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
<script src="../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
<script src="../Scripts/progressbar.js" type="text/javascript"></script>
<link href="../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    
<%--</ContentTemplate>--%>

<link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
<link href="../App_Themes/Alitas/tema_dinamico.css" rel="Stylesheet" type="text/css" />

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

     <style type="text/css">
         .botonEstilo
         {}
     </style>

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
    <div style="text-align:center;" >
            <asp:UpdateProgress ID="uppConsultas" runat="server"  >
                    <progresstemplate>
                        <img alt="" 
                    src="../Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>
            </div>
    <center>
        <table style="width:952px; text-align:left; background-color:Black">
            <tr>
                <td class="Titulo">
                <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloSucursales %>"></asp:Label>
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
                <td colspan="0" rowspan="0" align="left" width="25px">
                <div style="overflow:scroll; height:220px;">
                   <asp:TreeView ID="trvEstructura" runat="server" BorderStyle="Solid" 
                        BorderWidth="1px" ImageSet="News" 
                        onselectednodechanged="trvEstructura_SelectedNodeChanged" 
                        Width="300px" NodeIndent="10" ShowLines="True" TabIndex="1">
                        <HoverNodeStyle Font-Underline="True" />
                        <NodeStyle Font-Names="Verdana" Font-Size="11pt" ForeColor="Black" 
                            HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                        <ParentNodeStyle Font-Bold="False" />
                        <RootNodeStyle />
                        <SelectedNodeStyle Font-Underline="True" 
                            HorizontalPadding="0px" VerticalPadding="0px" BackColor="#ECECEC" />
                    </asp:TreeView>
                </div>
                </td>
               <td valign ="top">
                <table width="100%" align="right" style="height: 158px">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblVerNombreMostrar" SkinId="labelLarge" runat="server" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:ImageMap ID="immImagenMostrar" runat="server" Height="136px" Width="284px"></asp:ImageMap>        
                        </td>
                    </tr>
                </table>
               </td>
            </tr>
        </table>
    </fieldset>
                </td>
            </tr>
        </table>
    </center>
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
<center>
    <table class="background_tablas_transparente" style="width:952px">
    <tr>
       <td>
       <table>
        <tr>
           <td>
              <asp:Label ID="lblNodoSel" runat="server" SkinId="labelLarge" style="font-weight: 700" 
                Text="<%$ Resources:resCorpusCFDIEs,lblFranquicia %>"></asp:Label>
           </td>
           <td width="1024px">
              <asp:Label ID="lblSelVal" runat="server" SkinId="labelLarge" 
                Text="<%$ Resources:resCorpusCFDIEs, lblNinguno %>" Width="100px"></asp:Label>
           </td>
           <td>
               &nbsp;</td>
        </tr>
        <%--<tr>
           <td>
               <asp:Label ID="lblNombreNodo" runat="server" style="font-weight: 700" 
                Text="<%$ Resources:resCorpusCFDIEs, lblNodoNombre %>"></asp:Label>
           </td>
           <td>
               <asp:TextBox ID="txtNombreNodo" runat="server" TabIndex="2"></asp:TextBox>
           </td>
           <td>
                <asp:Button ID="btnAgregar" runat="server" CssClass="botonEstilo" 
                    onclick="btnAgregar_Click" Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
                       Width="80px" TabIndex="3" />
            </td>
        </tr>--%>
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
     </table>
        </td>
        <td align="right" style="width:860px;">
           <asp:Button ID="btnNuevo" runat="server" CssClass="botonEstilo" 
           Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
             onclick="btnNuevo_Click" Width="80px" TabIndex="4" />
            <asp:Button ID="btnBorrar" runat="server" CssClass="botonEstilo" 
             onclick="btnBorrar_Click" Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" 
             Width="80px" TabIndex="5" />
            <asp:Button ID="btnEditar" runat="server" CssClass="botonEstilo" 
            onclick="btnEditar_Click" Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
            Width="80px" TabIndex="6" />
            <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" 
            onclick="btnNCancelar_Click" 
             Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" 
                TabIndex="7" />        
        </td>
    </tr>
    </table>
</center>
<center>
    <table style="width:952px; text-align:left; background-color:Black">
        <tr>
            <td class="Subtitulos">
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubSucursales %>" />
            </td>
        </tr>
    </table>
</center>
<center>
    <table class="background_tablas_transparente" style="width:952px">
        <tr>
            <td>
            <asp:Panel ID="pnlFormulario" runat="server" >
            <div>
            <fieldset class="register" style=" height:450px; width:890px; border-color:transparent;">
            <table>
                <tr>
                    <td style="vertical-align:top; width:400px;">
                        <asp:HiddenField ID="hdIdEstructura" runat="server" />
                        <%--<p>
                            <asp:Label ID="lblRegla" runat ="server" Text ="Regla de Negocio"></asp:Label>
                            <asp:DropDownList ID="ddlReglaNegocio" runat="server"
                                DataTextField="Regla" DataValueField="idRegla">
                            </asp:DropDownList>
                        </p>--%>
                        <p>
                            <asp:Label ID="lblNumTienda" runat="server" AssociatedControlID="txtNumTienda" SkinId="labelLarge"
                                Text="<%$ Resources:resCorpusCFDIEs, lblNoSucursal %>" ></asp:Label>
                            
                            <asp:TextBox ID="txtNumTienda" runat="server" CssClass="textEntry" 
                                MaxLength="255" TabIndex="8" Width="300px" ></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNumTienda" runat="server" 
                                ControlToValidate="txtNumTienda" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                            src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        </p>
                        <%--<p>
                            <asp:Label ID="lblIdSucursal" runat="server" AssociatedControlID="txtIDSucursal" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblIdSucursal %>" ></asp:Label>
                            <asp:TextBox ID="txtIdSucursal" runat="server" CssClass="textEntry" 
                                MaxLength="255" TabIndex="8" Width="120px"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ControlToValidate="txtIdsucursal" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                            src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ControlToValidate="txtIdSucursal" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="Debe ser un número" 
                                    ValidationExpression="^[0-9]*" 
                                    ValidationGroup="RegisterUserValidationGroup"><img 
                                            src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                        </p>--%>
                        <p>
                            <asp:Label ID="lblSucursal" runat="server" AssociatedControlID="txtSucursal" SkinId="labelLarge"
                                Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>" ></asp:Label>
                            <asp:TextBox ID="txtSucursal" runat="server" CssClass="textEntry" 
                                MaxLength="255" TabIndex="9" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvSucursal" runat="server" 
                                ControlToValidate="txtSucursal" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                            src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        </p>
                        <p>
                            <asp:Label ID="lblPais" runat="server" SkinId="labelLarge"
                                Text="<%$ Resources:resCorpusCFDIEs, lblPais %>" 
                                AssociatedControlID="ddlPais" ></asp:Label>
                            <asp:DropDownList ID="ddlPais" runat="server" Width="300px" AutoPostBack="True" 
                                DataTextField="pais" DataValueField="id_pais" 
                                onselectedindexchanged="ddlPais_SelectedIndexChanged" TabIndex="10"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvPais" runat="server" 
                                ControlToValidate="ddlPais" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valPais %>" 
                                ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                            src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        </p><p>
                            <asp:Label ID="lblEstado" runat="server" SkinId="labelLarge"
                                Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>" 
                                AssociatedControlID="ddlEstado" ></asp:Label>
                                <asp:DropDownList ID="ddlEstado" runat="server" Width="300px" 
                                DataTextField="estado" DataValueField="id_estado" TabIndex="11"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEstado" runat="server" 
                                ControlToValidate="ddlEstado" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valEstado %>" 
                                ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                            src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        </p>
                        <p>
                            <asp:Label ID="lblMunicipio" runat="server" SkinId="labelLarge"
                                Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>" 
                                AssociatedControlID="txtMunicipio" ></asp:Label>
                            <asp:TextBox ID="txtMunicipio" runat="server" CssClass="textEntry" 
                                MaxLength="255" TabIndex="12" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvMunicipio" runat="server" 
                                ControlToValidate="txtMunicipio" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valMunicipio %>" 
                                ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                            src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        </p>
                        <p>
                            <asp:Label ID="lblLocalidad" runat="server" SkinId="labelLarge"
                                Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>" 
                                AssociatedControlID="txtLocalidad" ></asp:Label>
                            <asp:TextBox ID="txtLocalidad" runat="server" CssClass="textEntry" 
                                MaxLength="255" TabIndex="13" Width="300px"></asp:TextBox>
                         </p>
                         <p>
                            <asp:Label ID="lblCalle" runat="server"  SkinId="labelLarge"
                                Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>" 
                                AssociatedControlID="txtCalle"></asp:Label>
                            <asp:TextBox ID="txtCalle" runat="server" CssClass="textEntry" MaxLength="255" 
                                TabIndex="15" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCalle" runat="server" 
                                ControlToValidate="txtCalle" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>" 
                                ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                            src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        </p>
                         
                    </td>
                    <td style="vertical-align:top; width:400px;">
                        
                        <p>
                            <asp:Label ID="lblNoExterior" runat="server" SkinId="labelLarge" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>" 
                                AssociatedControlID="txtNoExterior"></asp:Label>
                            <asp:TextBox ID="txtNoExterior" runat="server" CssClass="textEntry" 
                                MaxLength="10" TabIndex="16" Width="300px"></asp:TextBox>
                        </p>
                        <p>
                            <asp:Label ID="lblNoInterior" runat="server"  SkinId="labelLarge" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>" 
                                AssociatedControlID="txtNoInterior"></asp:Label>
                            <asp:TextBox ID="txtNoInterior" runat="server" CssClass="textEntry" 
                                MaxLength="10" TabIndex="17" Width="300px"></asp:TextBox>
                        </p>
                        <p> 
                            <asp:Label ID="lblColonia" runat="server" SkinId="labelLarge"
                                Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>" 
                                AssociatedControlID="txtColonia"></asp:Label>
                            <asp:TextBox ID="txtColonia" runat="server" CssClass="textEntry" 
                                MaxLength="255" TabIndex="18" Width="300px"></asp:TextBox>
                        </p>
                        <p>
                            <asp:Label ID="lblReferenciaDom" runat="server" SkinId="labelLarge"
                                Text="<%$ Resources:resCorpusCFDIEs, lblReferenciaDom %>" 
                                AssociatedControlID="txtReferencia" ></asp:Label>
                            <asp:TextBox ID="txtReferencia" runat="server" CssClass="textEntry" 
                                MaxLength="255" TabIndex="19" Width="300px"></asp:TextBox>
                        </p>
                        <p>
                            <asp:Label ID="lblCodigoPostal" runat="server" SkinId="labelLarge"
                                Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>" 
                                AssociatedControlID="txtCodigoPostal" ></asp:Label>
                            <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="textEntry" 
                                MaxLength="5" TabIndex="20" Width="300px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="txtCodigoPostal_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" 
                                TargetControlID="txtCodigoPostal">
                            </cc1:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="rfvCodigoPostal" runat="server" 
                                ControlToValidate="txtCodigoPostal" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCodigoPostal %>" 
                                ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                            src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        </p>
                        <p align="left">
                          <asp:Label ID="lblLogoMostrar" runat="server" SkinId="labelLarge"
                                       Text="<%$ Resources:resCorpusCFDIEs, lbllogo %>" 
                                CssClass="fontText"></asp:Label>
                            <br />
                            <asp:FileUpload ID="fupLogo" runat="server" TabIndex="21" 
                                    ToolTip ="<%$ Resources:resCorpusCFDIEs, lblArchivoCargado  %>" />
                            <asp:RegularExpressionValidator ID="revImagen" runat="server" 
                            ControlToValidate="fupLogo" CssClass="failureNotification" Display="Dynamic" 
                            ToolTip="El archivo no corresponde a una imagen" 
                            ValidationExpression="^.*\.(jpg)$" 
                            ValidationGroup="RegisterUserValidationGroup"><img 
                                src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                        </p>
                        <table id="tblSeriesFolios" runat="server" visible ="false">
                            <tr>
                                <td style="margin-left: 80px">
                                    <asp:Label ID="lblSerie" runat="server" SkinId="labelLarge" Text="Serie"></asp:Label>
                                </td>
                                <td></td>
                                <td>
                                    <asp:Label ID="blFolio" runat="server" SkinId="labelLarge" Text="Folio"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlSerie" runat="server" 
                                        CssClass="fontText" DataTextField="Serie" DataValueField="id_Serie" 
                                        TabIndex="14" Width="160px" 
                                        onselectedindexchanged="ddlSerie_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>       
                                </td>
                                <td></td>
                                <td>
                                    <asp:TextBox ID="txtFolio" runat="server" 
                                    CssClass="fontText" Enabled="False" Width="120px" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <%--<p>
                            <asp:Label ID="lblRfc" runat="server" Text="<%$ Resources:resCorpusCFDIEs,lblRFC %>"
                                AssociatedControlID="ddlRfc" ></asp:Label>
                            <asp:DropDownList ID="ddlRfc" runat="server" Height="19px" Width="300px" 
                                DataTextField="rfc" DataValueField="id_rfc" TabIndex="22" >
                            </asp:DropDownList>
                        </p>--%>
                    </td>
                </tr>
            </table>
            </fieldset>
            </div>
            <p style="text-align:right;" >
                <asp:Button ID="btnGuardarActualizar" runat="server" CommandName="MoveNext" 
                ValidationGroup="RegisterUserValidationGroup" 
                Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                    TabIndex="22" onclick="btnGuardarActualizar_Click" 
                    CssClass="botonEstilo" />
                <asp:Button ID="btnCancelar" runat="server" 
                Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                    onclick="btnCancelar_Click" TabIndex="23" Visible="False" 
                    CssClass="botonEstilo" />
            </p>
            </asp:Panel>
            
            </td>
        </tr>
    </table>
</center>
            <%--<asp:UpdatePanel id="updFormulario" runat ="server">
            <ContentTemplate>--%>
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
                   <table class="TablaBackGround">
                     <tr>
                        <td>
                            <img alt="" class="imgInformacion" src="../Imagenes/info_ico.png" />
                        </td>
                        <td align="center">
                           <asp:Label ID="lblAviso" runat="server" SkinID="labelMedium"></asp:Label>
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
     <%--ModalPoup ErrorLog--%>
    <asp:LinkButton ID="lkbErrorLog" runat="server"></asp:LinkButton>
        <cc1:ModalPopupExtender ID="mpeErrorLog" runat="server"
         TargetControlID ="lkbErrorLog" 
         PopupControlID ="pnlErrorLog" 
         BackgroundCssClass ="modalBackground">
        </cc1:ModalPopupExtender>
   <asp:Panel ID="pnlErrorLog" runat="server" CssClass ="modal" >
         <table cellpadding="0" cellspacing="0">
           <tr>
               <td class="TablaBackGround">
                   &nbsp;</td>
           </tr>
           <tr>
               <td> 
               <br />
                    <table class="TablaBackGround">
                         <tr>
                            <td>
                                <img alt="" class="imgInformacion" 
                                 src="../Imagenes/info_ico.png" />   
                            </td>
                            <td align="center">
                                <asp:Label ID="lblErrorLog" runat="server" SkinID="labelMedium"></asp:Label>
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
                                <asp:Button ID="btnErrorLog" runat="server" CssClass="botonEstilo" Text ="OK"/>
                            </td>
                         </tr>
                    </table>    
               </td>
           </tr>
       </table>
    </asp:Panel>
    </ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="btnGuardarActualizar" />
</Triggers>
</asp:UpdatePanel>

<%--        Modal Franquicias comentado 18/04/2013 Marco Santana 
<asp:LinkButton ID="lkbTienda" runat="server"></asp:LinkButton>
        <cc1:ModalPopupExtender ID="mpeNewTienda" runat="server"
         TargetControlID ="lkbTienda" 
         PopupControlID ="pnlNewTienda" 
         BackgroundCssClass="modalBackground"> 
        </cc1:ModalPopupExtender>
   <asp:Panel ID="pnlNewTienda" runat="server"  GroupingText="<%$ Resources:resCorpusCFDIEs, vrAgreFranqui %>" BackColor="White"
    CssClass="modal" >
         <table>
             <tr>
                <td>
                   <asp:Label ID="lblNomTienda" runat="server" Text ="<%$  Resources:resCorpusCFDIEs, lblNomTienda   %>"  ></asp:Label>
                <br />
                    <asp:TextBox ID="txtTienda" runat="server" CssClass="textEntry" Width="120px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTienda" runat="server" 
                         ControlToValidate="txtTienda" CssClass="failureNotification" Display="Dynamic" 
                         ErrorMessage="Campo requerido." ToolTip="<%$ Resources:resCorpusCFDIEs, lblCampoObligatorio %>" 
                         ValidationGroup="ValidationGroup" Width="20px" ><img src="../Imagenes/error_sign.gif"/></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Label ID="lblRegla" runat ="server" Text ="<%$  Resources:resCorpusCFDIEs,lblReglaNe%>"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddlReglaNegocio" runat="server"
                         DataTextField="Regla" DataValueField="idRegla" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
             </tr>
             <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="txtIDSucursal" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblIdSucursal %>" ></asp:Label>
                    <br />
                            <asp:TextBox ID="txtIdSucursal" runat="server" CssClass="textEntry" 
                                MaxLength="255" TabIndex="8" Width="120px"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvIdSucursal" runat="server" 
                                ControlToValidate="txtIdsucursal" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                ValidationGroup="ValidationGroup" Width="16px" Display="Dynamic"><img 
                                            src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revIdSucursal" runat="server" 
                                    ControlToValidate="txtIdSucursal" CssClass="failureNotification" Display="Dynamic" 
                                    ToolTip="Debe ser un número" 
                                    ValidationExpression="^[0-9]*" 
                                    ValidationGroup="ValidationGroup"><img 
                                            src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                </td>
                <td>
                    <asp:Label ID="lblimgTicket" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblImgTicket %>"></asp:Label>
                    <br />
                    <asp:FileUpload ID="fupImgTicket" runat="server" />
                </td>
             </tr>
             <tr>
             <td>
                 <asp:Label ID="lblimgLogo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblImgLogo %>"></asp:Label>
                 <br />
                 <asp:FileUpload ID="fupimgPlantillaLogo" runat="server" TabIndex="15" ToolTip ="<%$ Resources:resCorpusCFDIEs, lblArchivoCargado  %>" />
                            <asp:RegularExpressionValidator ID="revfupimgLogo" runat="server" 
                            ControlToValidate="fupimgPlantillaLogo" CssClass="failureNotification" Display="Dynamic" 
                            ToolTip="El archivo no corresponde a una imagen" 
                            ValidationExpression="^.*\.(jpg)$" 
                            ValidationGroup="RegisterUserValidationGroup"><img 
                                src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
             </td>
                <td align="right">
                    <asp:Button ID="btnAgregarTienda" runat="server" CssClass="botonEstilo"
                     Height="32px" Width="77px" 
                        Text ="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
                        ValidationGroup="ValidationGroup" />
                    <asp:Button ID="btnCancelarTienda" runat="server" CssClass="botonEstilo"
                     Height="32px" Width="77px" 
                        Text ="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                        />
                </td>
             </tr>
         </table>
    </asp:Panel>--%>

</asp:Content>


<%@ Page Title="Datos" Language="C#" MasterPageFile="~/webGlobalMaster.master" AutoEventWireup="true" CodeFile="webInicioSesionRegDatos.aspx.cs" Inherits="InicioSesion_webInicioSesionRegDatos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" 
TagPrefix="asp" %> 
   
   
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />

<style type="text/css" >
    div.textos input[type='text']
    {
        width:300px;
    }
    div.textos select
    {
        width:300px;
    }
    .sinBorde img
    {
        border-style:none;
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

    function clickOnce(btn, msg) {
        // Comprobamos si se está haciendo una validación
        if (typeof (Page_ClientValidate) == 'function') {
            // Si se está haciendo una validación, volver si ésta da resultado false
            if (Page_ClientValidate() == false) { return false; }
        }

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
        .fontText
        {
            font: 8pt verdana;
        }
    .style2
    {
        height: 92px;
    }
    .style4
    {
        width: 420px;
    }
    .style5
    {
        width: 348px;
    }
    </style>

<center>

     <h2>
        <asp:Label ID="lblDatosFiscales" runat="server" 
            Text="<%$ Resources:resCorpusCFDIEs, lblDatosFiscales %>"></asp:Label>
    </h2>
    <br />
     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
         <ContentTemplate>
             <asp:Label ID="lblVersion" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblVersion %>"></asp:Label>
             <asp:DropDownList ID="drpVersion" runat="server" 
                 onselectedindexchanged="drpVersion_SelectedIndexChanged" 
                 DataTextField="version" DataValueField="id_version" AutoPostBack="True">
             </asp:DropDownList>

    
    <table >
    
    <tr>
    <td class="style2">
    <asp:Panel ID="pnlDatos" runat="server"  Width="850px" GroupingText="<%$ Resources:resCorpusCFDIEs, lblDatosFisc %>" BorderStyle="None"> 
        <table style="width: 825px">
        <tr>
        <td align="left" colspan="2">
            
                <asp:Label ID="lblRazonSocial" runat="server" AssociatedControlID="txtRazonSocial" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>" 
                    CssClass="fontText"></asp:Label>
        </td>
      </tr>
      <tr>
      <td colspan="3" align="left">
                <asp:TextBox ID="txtRazonSocial" CssClass="fontText" runat="server"
                    TabIndex="1" Width="674px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvRazonSocial" runat="server" 
                    ControlToValidate="txtRazonSocial" CssClass="failureNotification" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, txtRazonSocial %>" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
            
        </td>
    </tr>
        <tr>
        <td align="left" class="style5">
                <asp:Label ID="lblRFC" runat="server" AssociatedControlID="txtRFC" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>" CssClass="fontText"></asp:Label>
        </td>
        <td align="left" class="style4">
            <asp:Label ID="lblSucursal" runat="server" AssociatedControlID="txtRazonSocial" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblSucursalMatriz %>" Width="346px" CssClass="fontText"></asp:Label>
        </td>
        </tr>
        <tr>
        <td align="left" class="style5">
             <asp:TextBox ID="txtRFC" runat="server" CssClass="fontText" TabIndex="2" 
                    ToolTip="AAA000000AAA" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRFC" runat="server" 
                    ControlToValidate="txtRFC" CssClass="failureNotification" Display="Dynamic" 
                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, txtRFC %>" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="regxRFC" runat="server" 
                    ControlToValidate="txtRFC" CssClass="failureNotification" Display="Dynamic" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxRFC %>" 
                    ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
        </td>
        <td align="left" class="style4">
            <asp:TextBox ID="txtSucursal" runat="server" CssClass="fontText" TabIndex="3" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSucursal" runat="server" 
                    ControlToValidate="txtSucursal" CssClass="failureNotification" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
        </td>
        </tr>
        <tr>
            <td align="left" class="style5">
                <asp:UpdatePanel ID="upLocacion" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblPais" runat="server" AssociatedControlID="ddlPais" Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"
                            CssClass="fontText"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlPais" runat="server" AutoPostBack="True" DataTextField="pais"
                            DataValueField="id_pais" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged"
                            Width="300px" TabIndex="4" CssClass="fontText">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvPais" runat="server" ControlToValidate="ddlPais"
                            CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valPais %>"
                            ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        <br />
                        <asp:Label ID="lblEstado" runat="server" AssociatedControlID="ddlEstado" Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"
                            CssClass="fontText"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlEstado" runat="server" DataTextField="estado" DataValueField="id_estado"
                            Width="300px" TabIndex="5" CssClass="fontText">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvEstado" runat="server" ControlToValidate="ddlEstado"
                            CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valEstado %>"
                            ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td align="left" valign="top" class="style4">
                <asp:Label ID="lblMunicipio" runat="server" AssociatedControlID="txtMunicipio" Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>"
                    CssClass="fontText"></asp:Label>
                <br />
                <asp:TextBox ID="txtMunicipio" runat="server" CssClass="fontText" TabIndex="6" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMunicipio" runat="server" ControlToValidate="txtMunicipio"
                    CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valMunicipio %>"
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                
                <br />
                
                <asp:Label ID="lblLocalidad" runat="server" AssociatedControlID="txtLocalidad" Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>"
                    CssClass="fontText"></asp:Label>
                <br />
                <asp:TextBox ID="txtLocalidad" runat="server" CssClass="fontText" TabIndex="7" Width="300px"></asp:TextBox>
            </td>
        </tr>
           <tr>
            <td align="left" class="style5">
                <asp:Label ID="lblCalle" runat="server" AssociatedControlID="txtCalle" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>" Width="100px" 
                    CssClass="fontText" TabIndex="8"></asp:Label>
            </td>
            <td align="left" class="style4">
                <asp:Label ID="lblColonia" runat="server" AssociatedControlID="txtColonia"
                    Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>" CssClass="fontText"></asp:Label>
            </td>
           </tr>
           <tr>
            <td align="left" class="style5">
                <asp:TextBox ID="txtCalle" runat="server" CssClass="fontText" TabIndex="8" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCalle" runat="server" 
                    ControlToValidate="txtCalle" CssClass="failureNotification" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
            </td>
            <td align="left" class="style4">
                 <asp:TextBox ID="txtColonia" runat="server" CssClass="fontText" TabIndex="9" Width="300px"></asp:TextBox>
            </td>
           </tr>
            <tr>
                <td align="left" class="style5">
                     <asp:Label ID="lblNoExterior" runat="server"   AssociatedControlID="txtNoExterior"
                    Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>" CssClass="fontText"></asp:Label>
                </td>
                <td align="left" class="style4">
                    <asp:Label ID="lblNoInterior" runat="server"  AssociatedControlID="txtNoInterior"
                    Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>" CssClass="fontText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" class="style5">
                     <asp:TextBox ID="txtNoExterior" runat="server" CssClass="fontText" 
                    TabIndex="10" Width="300px"></asp:TextBox>
                </td>
                <td align="left" class="style4">
                    <asp:TextBox ID="txtNoInterior" runat="server" CssClass="fontText" 
                    TabIndex="11" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" class="style5">
                     <asp:Label ID="LablblCodigoPostalel5" runat="server" AssociatedControlID="txtCodigoPostal"
                    Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>" 
                    CssClass="fontText" ></asp:Label>
                </td>
                <td align="left" class="style4">
                     <asp:Label ID="lblReferenciaDom" runat="server" AssociatedControlID="txtReferencia"
                    Text="<%$ Resources:resCorpusCFDIEs, lblReferenciaDom %>" 
                    CssClass="fontText" ></asp:Label>           
                </td>
            </tr>

            <tr>
                <td align="left" class="style5">
                     <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="fontText" 
                    MaxLength="5" TabIndex="13" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCodigoPostal" runat="server" 
                    ControlToValidate="txtCodigoPostal" CssClass="failureNotification" 
                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valCodigoPostal %>" 
                    ValidationGroup="RegisterUserValidationGroup"><img src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                <cc1:FilteredTextBoxExtender ID="filterCP" runat="server" FilterType="Numbers" 
                    TargetControlID="txtCodigoPostal">
                </cc1:FilteredTextBoxExtender>
                </td>
                <td align="left" class="style4">
                <asp:TextBox ID="txtReferencia" runat="server" CssClass="fontText" 
                    TabIndex="14" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" class="style5">
                    <asp:Label ID="lblRegimenFiscal" runat="server"
                    Text="<%$ Resources:resCorpusCFDIEs, lblRegimenfiscal %>" 
                    AssociatedControlID="txtRegimenFiscal" CssClass="fontText" ></asp:Label>
                </td>
                <td align="left" class="style4">
                 <asp:Label ID="lblLogo" runat="server" 
                           Text="<%$ Resources:resCorpusCFDIEs, lbllogo %>" 
                    CssClass="fontText"></asp:Label>


            
                </td>
            </tr>
            <tr>
                <td align="left" class="style5">
                    <asp:TextBox ID="txtRegimenFiscal" runat="server" CssClass="fontText" 
                    TabIndex="14" Width="300px">No Aplica</asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvRegimenFiscal" runat="server" 
                    ControlToValidate="txtRegimenFiscal" CssClass="failureNotification" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, valRegimenFiscal %>"
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                </td>
                <td align="left" class="style4"> 
                
             <asp:FileUpload ID="fupLogo" runat="server" TabIndex="15" />
                </td>
            </tr>
            </table>
             </asp:Panel> 
                     
    </td>
    </tr>
    <tr>
    <td  class="style2">
        <asp:Panel ID="pnlDatosCert" runat="server"  Width="850px" GroupingText="<%$ Resources:resCorpusCFDIEs, lblSellosDigitales %>"> 
        <table width="800px">
            <tr>
                <td align="left" >
                     <asp:Label ID="lblKey" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblKey %>" CssClass="fontText"></asp:Label>
                </td>
                <td align="left">
                  <asp:Label ID="lblCer" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblCer %>" CssClass="fontText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                <asp:FileUpload ID="fupKey" runat="server" TabIndex="16" CssClass="fontText" />
                <asp:RequiredFieldValidator ID="rfvKey" runat="server" 
                    ControlToValidate="fupKey" CssClass="failureNotification" Display="Dynamic" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, valKey %>" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                        src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="regxKey" runat="server" 
                    ControlToValidate="fupKey" CssClass="failureNotification" Display="Dynamic" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxKey %>" 
                    ValidationExpression="^.*\.(key)$" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                        src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                </td>
                <td align="left">
                <asp:FileUpload ID="fupCer" runat="server" TabIndex="17" CssClass="fontText" />
                <asp:RequiredFieldValidator ID="rfvCer" runat="server" 
                    ControlToValidate="fupCer" CssClass="failureNotification" Display="Dynamic" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, valCer %>" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                        src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="valCer" runat="server" 
                    ControlToValidate="fupCer" CssClass="failureNotification" Display="Dynamic" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, rfvCer %>" 
                    ValidationExpression="^.*\.(cer)$" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                        src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="left">
                     <asp:Label ID="lblContrasenia" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>" 
                    CssClass="fontText"></asp:Label>
                </td>
                <td>
                
                </td>
            </tr>
            <tr>
                <td align="left">
                <asp:TextBox ID="txtPass" runat="server" TextMode="Password" TabIndex="18" 
                    CssClass="fontText" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPass" runat="server" 
                    ControlToValidate="txtPass" CssClass="failureNotification" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, valContrasenia %>" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                </td>
                <td>
                </td>
                
            </tr>

        </table>
        </asp:Panel>
                    
         
    </td>
    </tr>
    <tr>
        <td class="style2" >
            <asp:Panel ID="pnlDist" runat="server" Width="850px" GroupingText="<%$ Resources:resCorpusCFDIEs, lblContratacion %>">
                <table width="800px">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblNumDist" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblNumeroDistribuidor %>" 
                    CssClass="fontText"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="txtNumDistribuidor" runat="server" CssClass="fontText" 
                    TabIndex="19" Width="300px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            (<asp:Label runat="server" ID="lblDistribuidorAviso" Text="<%$ Resources:resCorpusCFDIEs, lblDistribuidorAviso %>" />)
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    </table>
     </ContentTemplate>
     </asp:UpdatePanel>
             <br />
  <%--           <table cellpadding="0" cellspacing="0" 
                 style="border: thin solid #CCCCCC; width: 731px" ID="tbl3_2" runat="server" visible="false"  >
                 <tr>
                     <td align="left" width="360px" height="50px">
                         <asp:Label ID="lblExpedio" runat="server" 
                             Text="<%$ Resources:resCorpusCFDIEs, varExpedidoEn %>" style="font-weight: 700"></asp:Label>
                         <br />
                     </td>
                     <td align="left">
                         &nbsp;</td>
                 </tr>
                 <tr>
                     <td align="left" width="360px">
                        <p>
                         <asp:Label ID="lblPais0" runat="server" AssociatedControlID="ddlPais0" 
                             CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"></asp:Label>
                         </p>
                         <asp:DropDownList ID="ddlPais0" runat="server" AutoPostBack="True" 
                             CssClass="fontText" DataTextField="pais" DataValueField="id_pais" 
                             onselectedindexchanged="ddlPais_SelectedIndexChanged" TabIndex="18" 
                             Width="200px">
                         </asp:DropDownList>
                     </td>
                     <td align="left">
                        <p>
                         <asp:Label ID="lblCalle0" runat="server" AssociatedControlID="txtCalleEmisor" 
                             CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>"></asp:Label>
                         </p>
                         <asp:TextBox ID="txtCalleEmisor" runat="server" CssClass="fontText" 
                             MaxLength="255" TabIndex="21" Width="150px"></asp:TextBox>
                     </td>
                 </tr>
                 <tr>
                     <td align="left">
                        <p>
                         <asp:Label ID="lblEstado0" runat="server" AssociatedControlID="ddlEstado0" 
                             CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"></asp:Label>
                         </p>
                         <asp:DropDownList ID="ddlEstado0" runat="server" CssClass="fontText" 
                             DataTextField="estado" DataValueField="id_estado" TabIndex="19" 
                             Width="200px">
                         </asp:DropDownList>
                     </td>
                     <td align="left">
                        <p>
                         <asp:Label ID="LablblCodigoPostalel6" runat="server" 
                             AssociatedControlID="txtCodigoPostalEmisor" CssClass="fontText" 
                             Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>"></asp:Label>
                         </p>
                         <asp:TextBox ID="txtCodigoPostalEmisor" runat="server" CssClass="fontText" 
                             MaxLength="5" TabIndex="22" Width="150px"></asp:TextBox>
                         <cc1:FilteredTextBoxExtender ID="txtCodigoPostalEmisor_FilteredTextBoxExtender" 
                             runat="server" FilterType="Numbers" TargetControlID="txtCodigoPostalEmisor">
                         </cc1:FilteredTextBoxExtender>
                     </td>
                 </tr>
                 <tr>
                     <td align="left">
                        <p>
                         <asp:Label ID="lblMunicipio0" runat="server" AssociatedControlID="txtMunicipioEmisor" 
                             CssClass="fontText" Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>"></asp:Label>
                         </p>
                         <asp:TextBox ID="txtMunicipioEmisor" runat="server" CssClass="fontText" 
                             MaxLength="255" TabIndex="20" Width="200px"></asp:TextBox>
                         <br />
                     </td>
                     <td>
                         &nbsp;</td>
                 </tr>
                 <tr>
                     <td align="left">
                         &nbsp;</td>
                     <td>
                         &nbsp;</td>
                 </tr>
             </table>--%>
             <br />


     <br />

</center>
         <asp:UpdatePanel ID="UpdatePanel3" runat="server">
             <ContentTemplate>
                 <table>
                     <tr>
                         <td width="700">
                             &nbsp;</td>
                         <td>
                             <asp:Button ID="btnActualizarDomicilio" runat="server" CommandName="MoveNext" 
                                 CssClass="botonEstilo" onclick="btnActualizarDomicilio_Click" TabIndex="20" 
                                 Text="<%$ Resources:resCorpusCFDIEs, btnSiguiente %>" 
                                 ValidationGroup="RegisterUserValidationGroup" UseSubmitBehavior="false" 
                                 OnClientClick="clickOnce(this, 'Procesando')"  />
                         </td>
                     </tr>
                 </table>
             </ContentTemplate>
             <triggers>
                 <asp:PostBackTrigger ControlID="btnActualizarDomicilio" />
             </triggers>
     </asp:UpdatePanel>
     <br />

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
                            <img alt="" src="../Imagenes/imgAjaxLoader.gif" />
                        </progresstemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <cc1:modalpopupextender id="modalGenerando" runat="server" backgroundcssclass="modalBackground" 
    popupcontrolid="pnlGenerando" targetcontrolid="pnlGenerando">
    </cc1:modalpopupextender>

    <script type="text/javascript" language="javascript">
        var ModalProgress = '<%= modalGenerando.ClientID %>';         
    </script>



   
 </asp:Content>
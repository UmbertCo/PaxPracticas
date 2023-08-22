<%@ Page Title="" Theme="Alitas" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webOperacionRfc.aspx.cs" Inherits="Operacion_RFC_webOperacionRfc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" 
TagPrefix="asp" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <%------------------------JQuery----------------------------%>
<script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script src="../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery.ui.core.js" type="text/javascript"></script>
<script src="../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
<script src="../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
<script src="../Scripts/progressbar.js" type="text/javascript"></script>
<link href="../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
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
<%------------------------JQuery----------------------------%>

<link href="../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
<link href="../App_Themes/Alitas/tema_dinamico.css" rel="stylesheet" type="text/css" />

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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server" >
<link href="../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    
    <script language="javascript" type="text/javascript" >

        function Check(vChkId) {
            var vStatus = '0';
            var vCnt = 0;
            if (vChkId.checked == true) {
                for (j = 0; j < MainContent_gdvRFCs.length; j++) {
                    if (MainContent_gdvRFCs.elements[j].type == "checkbox") {
                        if (document.getElementById(MainContent_gdvRFCs.elements[j].id).checked == true) {
                            vCnt = parseInt(vCnt, 10) + 1;
                        }
                    }
                }
            }
            if (vCnt > 1) {
                vChkId.checked = false;
            }
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

        function fntraerEnter(keyStroke) {
            isNetscape = (document.layers);
            eventChooser = (isNetscape) ? keyStroke.which : event.keyCode;
            if (eventChooser == 13) {
                return false;
            }
        }
        document.onkeypress = fntraerEnter; 

</script>
        <div style="text-align:center;" >
            <asp:UpdateProgress ID="uppConsultas" runat="server"  >
                    <progresstemplate>
                        <img alt="" 
                    src="../../Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>
    </div>
<asp:Panel ID="pnlFormulario" runat="server">
<%--<ContentTemplate>--%>
<center>
    <table style="width:952px; text-align:left; background-color:Black">
        <tr>
            <td class="Titulo">
            <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varCatalogoRFC %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:952px; height:0.5px; background-color:#fff000"></td>
        </tr>
        <tr>
                <td class="Subtitulos">
                <asp:Literal ID="LiteralRfc" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosRFC %>" SkinId="labelMedium" />
                </td>
            </tr>
    </table>
</center>
    <center>
        <table class="background_tablas_transparente" style="width:952px">
            <tr>
                <td>
<div>
<fieldset style="border-color:transparent"> 
    <table style="text-align:left">
        <tr>
            <td align="left">
            <asp:Label ID="lblVersion0"  runat="server" Text="Version 3.2" SkinId="labelLarge"></asp:Label>
            </td>
        </tr>
    </table>
<table style="height: 302px" align="left">  
    <tr>
        <td> 
            <asp:Label ID="lblRFC" runat="server" AssociatedControlID="txtRFC" 
                Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>" SkinId="labelLarge"></asp:Label>
        </td>
    </tr>
    <tr>
        <td> 
            <asp:TextBox ID="txtRFC" runat="server" CssClass="textEntry" Enabled="False" 
                TabIndex="1" ToolTip="AAA000000AAA"></asp:TextBox>
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
            <td> 
                <asp:Label ID="lblCer0" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblCer %>" SkinId="labelLarge"></asp:Label>
        </td>
    </tr>
    <tr>
        <td> 
            <asp:Label ID="lblNombreCompleto" runat="server" 
                AssociatedControlID="txtNombre" 
                Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>" SkinId="labelLarge"></asp:Label>
            </td>
            <td>
                <asp:FileUpload ID="fupCer" runat="server" Enabled="False" TabIndex="15" />
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
        <td valign="middle">
            <asp:TextBox ID="txtNombre" runat="server" CssClass="textEntry" Enabled="False" 
                MaxLength="255" TabIndex="1" Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="UserNameRequired0" runat="server" 
                ControlToValidate="txtNombre" CssClass="failureNotification" 
                ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                ValidationGroup="RegisterUserValidationGroup" Width="20px">*</asp:RequiredFieldValidator>
        </td>
        <td>
            <asp:Label ID="lblKey" runat="server" 
                Text="<%$ Resources:resCorpusCFDIEs, lblKey %>" SkinId="labelLarge"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            
        </td>
        <td>
            <asp:FileUpload ID="fupKey" runat="server" Enabled="False" TabIndex="14" />
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
    </tr>
    <tr>
        <td>
            
        </td>
        <td>
            <asp:Label ID="lblContrasenia" runat="server" SkinId="labelLarge" 
                Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            
        </td>
        <td>
            <asp:TextBox ID="txtPass" runat="server" Enabled="False" TabIndex="16" 
                    TextMode="Password"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
        </td>
     </tr>
 </table>
</fieldset>
    <table align="right">
        <tr>
            <td valign="top">
                <p>
                    <asp:Button ID="btnNuevoUsuario" runat="server" CssClass="botonEstiloGrande" 
                        onclick="btnNuevoUsuario_Click" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblNuevoRFC%>"/>
                    <asp:Button ID="btnGuardarActualizar" runat="server" CssClass="botonEstilo" 
                        onclick="btnGuardarActualizar_Click" 
                        onclientclick="clickOnce(this, 'Procesando')" TabIndex="14" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" UseSubmitBehavior="False" 
                        ValidationGroup="RegisterUserValidationGroup" Visible="False"/>
                    <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" 
                        onclick="btnNCancelar_Click" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>"/>
                </p>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <br />
    <table>
        <tr>
            <td>
                <asp:GridView ID="gdvRFCs" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#0073aa" BorderStyle="Double" BorderWidth="3px" 
                    CellPadding="4" DataKeyNames="id_rfc,rfc" GridLines="Horizontal" 
                    onrowcommand="gdvRFCs_RowCommand" onrowdatabound="gdvRFCs_RowDataBound" 
                    onrowdeleting="gdvRFCs_RowDeleting" 
                    onselectedindexchanged="gdvRFCs_SelectedIndexChanged" Width="799px" SkinID="SkinGridView">
                    <Columns>
                        <asp:CommandField HeaderStyle-HorizontalAlign="Left" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                            SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                            ShowSelectButton="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:CommandField>
                        <%--<asp:ButtonField ButtonType="Image" CommandName="img" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lbllogo %>" 
                            ImageUrl="~/Imagenes/lupa.png">
                        <ControlStyle Height="20px" Width="20px" />
                        <ItemStyle Height="20px" Width="20px" />
                        </asp:ButtonField>--%>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblRFC %>">
                            <ItemTemplate>
                                <asp:Label ID="lblrcf" runat="server" Text='<%# Bind("rfc") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="300px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblRazonSocial %>">
                            <ItemTemplate>
                                <asp:Label ID="lblrazonsocial" runat="server" 
                                    Text='<%# Bind("razon_social") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus %>">
                            <ItemTemplate>
                                <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblidcontribuyente" runat="server" 
                                    Text='<%# Bind("id_contribuyente") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblidrfc" runat="server" Text='<%# Bind("id_rfc") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblidversion" runat="server" Text='<%# Bind("id_version") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstructura %>">
                            <ItemTemplate>
                                <asp:Label ID="lblnombre" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblidestructura" runat="server" 
                                    Text='<%# Bind("id_estructura") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:CommandField ShowDeleteButton="True" />
                        <asp:CheckBoxField />
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSeleccion" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblidpadre" runat="server" Text='<%# Bind("id_padre") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblidusuario" runat="server" Text='<%# Bind("id_usuario") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Literal ID="Literal1" runat="server" 
                            text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                    </EmptyDataTemplate>
                  <%--   <FooterStyle BackColor="White" ForeColor="#333333" />
                    <HeaderStyle BackColor="#0073aa" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#0073aa" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="White" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />--%>
                </asp:GridView>
            </td>
        </tr>
    </table>
    </div>
                
                </td>
            </tr>
        </table>
    </center>
            
<%-- Modalpoup Avisos --%> 
<asp:LinkButton ID="lkbAviso" runat="server"></asp:LinkButton>
<cc1:ModalPopupExtender ID="mpeAvisos" runat="server"
    TargetControlID ="lkbAviso" 
    PopupControlID ="pnlAvisos" 
    BackgroundCssClass ="modalBackground">
</cc1:ModalPopupExtender>
   <asp:Panel ID="pnlAvisos" runat="server" CssClass ="modal" >
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
<%--ModalPoup ErrorLog--%>
<asp:LinkButton ID="lkbErrorLog" runat="server"></asp:LinkButton>
<cc1:ModalPopupExtender ID="mpeErrorLog" runat="server"
    TargetControlID ="lkbErrorLog" 
    PopupControlID ="pnlErrorLog" 
    BackgroundCssClass ="modalBackground">
</cc1:ModalPopupExtender>
   <asp:Panel ID="pnlErrorLog" runat="server" CssClass ="modal">
     <table cellpadding="0" cellspacing="0" >
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
                               <asp:Label ID="lblErrorLog" runat="server" 
                                    SkinID="labelLarge"></asp:Label>
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
</asp:Panel>
<br />
</asp:Content>


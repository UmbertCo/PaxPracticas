<%@ Page Title="" Theme="Alitas" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webOperacionPublicidad.aspx.cs" Inherits="Cuenta_webOperacionPublicidad" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <script src="../Scripts/progressbar.js" type="text/javascript"></script>
    <link href="../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />

    <link href ="../Styles/menu_style.css" rel="Stylesheet" type="text/css" /> 
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
<asp:UpdatePanel ID="udpPublicidad" runat ="server">
<ContentTemplate>
<center>
    <table style="width:952px; text-align:left; background-color:Black">
        <tr>
            <td class="Titulo">
            <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs,lblCataPubli %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:952px; height:0.5px; background-color:#fff000"></td>
        </tr>
        <tr>
            <td class="Subtitulos">
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs,lgPublicidad %>" />
            </td>
        </tr>
    </table>
</center>
<center>
    <table class="background_tablas_transparente" style="width:952px">
        <tr>
            <td>
    <fieldset class="register" style="width:890px; border-color:transparent;">
     <div>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td width="454px" style="height: 260px">
                                <div style="overflow: auto; width: 454px; height: 330px">
                                <asp:GridView ID="dgvPublicidad" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="4" GridLines="Horizontal" 
                                    Width="100%" SkinId="SkinGridView"
                                    DataKeyNames="id_Publicidad,Titulo" 
                                    onselectedindexchanged="dgvPublicidad_SelectedIndexChanged" AllowPaging="True" 
                                        onpageindexchanging="dgvPublicidad_PageIndexChanging" PageSize="5">
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="10px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgSeleccionar" runat="server" CausesValidation="False" 
                                                        CommandName="Select" ImageUrl="../Imagenes/seleccionar.jpg" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="idPublicidad" Visible="False">
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblTitulo %>">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Titulo") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Titulo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ControlStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText ="<%$ Resources:resCorpusCFDIEs, lblgSeleccionPublicidad %>">
                                        <ItemTemplate>
                                                    <asp:CheckBox ID="cbSeleccion" runat="server" Enabled="False" />
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                <%-- <FooterStyle BackColor="White" ForeColor="#5A737E" />
                                <HeaderStyle BackColor="#0073aa" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#0073aa" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="White" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#BCBCBC" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                <SortedAscendingHeaderStyle BackColor="#0073aa" /> 
                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                <SortedDescendingHeaderStyle BackColor="#0073aa" />--%>
                                </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="style2">
                                <asp:HiddenField ID="hdnSelVal" runat="server" />
                            </td>
                            <td>
                                <asp:HiddenField ID="hdnSel" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <div>
                    <center>
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbldescpu" SkinId="labelLarge" runat="server" Text="<%$ Resources:resCorpusCFDIEs, msjDesc %>"></asp:Label> 
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Image ID="imgPublicidad" runat="server"  Height ="150px" Width="75px" /> 
                                </td>
                            </tr>
                        </table>
                       </center>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </fieldset>
            </td>
        </tr>
    </table>
</center>
<center>
    <table class="background_tablas_transparente" style="width:952px">
        <tr>
            <td align="right">
            <p style="text-align:right;" >
        <asp:Button ID="btnNuevo" runat="server" CssClass="botonEstilo" 
           Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" 
             Width="80px" onclick="btnNuevo_Click" />
            <asp:Button ID="btnBorrar" runat="server" CssClass="botonEstilo" 
             Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" 
             Width="80px" onclick="btnBorrar_Click" />
            <asp:Button ID="btnEditar" runat="server" CssClass="botonEstilo" 
            Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
            Width="80px" onclick="btnEditar_Click"  />
            <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" 
             Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" 
            onclick="btnNCancelar_Click" />
    </p>
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
    <table style="width:952px; text-align:left; background-color:Black">
        <tr>
            <td class="Titulo">
            <asp:Literal ID="ltlPublicidad" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lgDatosPublicidad %>" />
            </td>
        </tr>
    </table>
</center>
<center>
    <table class="background_tablas_transparente" style="width:952px">
        <tr>
            <td>
    <fieldset class="register" style=" width:890px; border-color:transparent;">
    <div>
    <table>
        <tr>
            <td> 
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblTit" SkinId="labelLarge" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTitulo %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSelPublic" SkinId="labelLarge" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSelPubli %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtTitulo" runat ="server" Width="292px" MaxLength="99"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" 
                                        ControlToValidate="txtTitulo" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, lblCampoObligatorio %>" 
                                        ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        </td>
                        <td align="right">
                            <asp:CheckBox ID="ckbSeleccion" runat="server" Text="." />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDesc" SkinId="labelLarge" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDescripcion %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDescrib" runat="server" Height="101px" MaxLength="253" 
                                TextMode="MultiLine" Width="292px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfcDescripcion" runat="server" 
                                        ControlToValidate="txtDescrib" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, lblCampoObligatorio %>" 
                                        ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                                    src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>    
                        <td>
                            <asp:Label ID="lblImgMos" SkinId="labelLarge" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSelecImagen %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:FileUpload id="fupImgPublicidad" runat="server" />
                            <%--<asp:RequiredFieldValidator ID="rfcimgPublicidad" runat="server" 
                                ControlToValidate="fupImgPublicidad" CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblCampoObligatorio %>" 
                                ValidationGroup="RegisterUserValidationGroup" Width="16px"><img 
                                src="../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>--%>
                            <%--<asp:RegularExpressionValidator ID="revImagen" runat="server" 
                                ControlToValidate="fupImgPublicidad" CssClass="failureNotification" Display="Dynamic" 
                                ToolTip="El archivo no corresponde a una imagen" 
                                ValidationExpression="^.*\.(gif)$" 
                                ValidationGroup="RegisterUserValidationGroup"><img 
                                src="../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>--%>
                        </td>
                    </tr>
                </table>   
            </td>
            <td>
                <table>
                    <tr>
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
      </table>
    </div>
    </fieldset>
            </td>
        </tr>
    </table>
</center>
<center>
    <table class="background_tablas_transparente" style="width:952px">
        <tr>
            <td align="right">
            <p style="text-align:right;" >
                <asp:Button ID="btnGuardarActualizar" runat="server" CommandName="MoveNext" 
                ValidationGroup="RegisterUserValidationGroup" 
                Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                    TabIndex="23"  
                    CssClass="botonEstilo" onclick="btnGuardarActualizar_Click" />
                <asp:Button ID="btnCancelar" runat="server" 
                Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                    TabIndex="24" Visible="False" 
                    CssClass="botonEstilo" />
            </p>
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
   <asp:Panel ID="pnlAvisos" runat="server"
     CssClass ="modal" >
         <table cellpadding="0" cellspacing="0">
           <tr>
               <td>
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
                            <asp:Button ID="btnAviso" runat="server" CssClass="botonEstilo"
                             Text ="OK" />
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
</asp:Content>


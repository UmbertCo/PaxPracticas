<%@ Page Title="Registro Usuarios de Soporte" Language="C#" MasterPageFile="~/Publica.master" AutoEventWireup="true" CodeFile="webUsuariosGeneracion.aspx.cs" Inherits="Pantallas_Usuarios_webUsuariosGeneracion" EnableEventValidation = "false" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../../WebControls/usrGlobalPwd.ascx" tagname="usrGlobalPwd" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
      <%------------------------JQuery----------------------------%>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <link href="../../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <%------------------------JQuery----------------------------%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
   

    <asp:Label ID="lblenccatusu" runat="server" Font-Bold="True" Font-Size="Large" 
        Text="<%$ Resources:resCorpusCFDIEs, lblenccatusu %>"></asp:Label>

    <br />

    <table style="width: 100%;">
        <tr>
            <td class="style4">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style5">
                <asp:Label ID="lblNombre" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNombreSop%>"></asp:Label>
            </td>
            <td class="style3">
                <asp:Label ID="lblCorreo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreoUsuarioSop%>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style5">
                <asp:TextBox ID="txtNombre" runat="server" Width="393px" Enabled="False"></asp:TextBox>
            </td>
            <td class="style3">
                <asp:TextBox ID="txtCorreo" runat="server" TabIndex="1" Width="280px" 
                    Enabled="False"></asp:TextBox>
                            
                              <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                    ControlToValidate="txtCorreo" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    Width="131px" ValidationGroup="RegisterUserValidationGroup" 
                    CssClass="failureNotification" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"  Display="Dynamic"><img src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                    ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style5">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style5">
                <asp:Label ID="lblUsuario" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblUsuarioSop%>"></asp:Label>
            </td>
            <td class="style3">
                <asp:Button ID="btnNuevoUsuario" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnNuevoUsuario%>" Width="172px" 
                    onclick="btnNuevoUsuario_Click" CssClass="botonGrande" />
            </td>
        </tr>
        <tr>
            <td class="style5">
                <asp:TextBox ID="txtUsuario" runat="server" TabIndex="2" Width="280px" 
                    Enabled="False"></asp:TextBox>
            </td>
            <td class="style3">
                <asp:Button ID="btnCambiarPwd" runat="server" onclick="btnCambiarPwd_Click" 
                    TabIndex="6" Text="<%$ Resources:resCorpusCFDIEs, btnCambiarPwd%>" 
                    Width="172px" Enabled="False" CssClass="botonGrande" />
            </td>
        </tr>
        <tr>
            <td class="style5">
                &nbsp;</td>
            <td class="style3">
                            <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="<%$ Resources:resCorpusCFDIEs, btnExcel%>" 
                                Width="172px" CssClass="botonGrande" />
            </td>
        </tr>
        <tr>
            <td class="style5">
                <asp:Label ID="lblIncidencia" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblIncidenciaSop%>"></asp:Label>
            </td>
            <td class="style3">
                <asp:Button ID="btnGuardar" runat="server" onclick="btnGuardar_Click" 
                    TabIndex="6" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar%>" 
                    Width="172px" Enabled="False" CssClass="botonGrande" />
            </td>
        </tr>
        <tr>
            <td class="style5">
                <asp:CheckBoxList ID="ddlIncidencia" runat="server" BorderColor="#999999" 
                    BorderStyle="Double" TabIndex="5">
                </asp:CheckBoxList>
            </td>
            <td class="style3">
                <asp:TextBox ID="txtPassword" runat="server" CssClass="passwordEntry" 
                    TabIndex="3" Width="280px" TextMode="Password" Visible="False"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                ControlToValidate="txtPassword" CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valTipoDocumento %>" 
                                ValidationGroup="grupoDocumentosImp" Visible="False"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style4">
                &nbsp;</td>
            <td>
                <asp:Label ID="lblConfirmarPwd" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblConfirmarPwdSop %>" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style4">
                <asp:Label ID="lblPerfil" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblPerfil %>" Visible="False"></asp:Label>
                <asp:DropDownList ID="ddlPerfil" runat="server" Height="23px" Width="280px" 
                    TabIndex="5" Visible="False">
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txtConfirmaPwd" runat="server" CssClass="passwordEntry" 
                    TabIndex="4" Width="280px" TextMode="Password" Visible="False"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
        </tr>
        <tr>
            <td class="style4">
                </td>
            <td>
                </td>
        </tr>

        <tr>
            <td  align="left" colspan="2">
     

            <%--    <asp:Panel 
                    ID="Panel1" runat="server" Height="176px" ScrollBars="Vertical" 
                    Width="817px">--%>
                    <asp:GridView ID="gdvUsuarios" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" GridLines="Horizontal" Width="799px" 
                DataKeyNames="id_usuario_soporte" 
                onselectedindexchanged="gdvUsuarios_SelectedIndexChanged" 
                onrowdeleting="gdvUsuarios_RowDeleting" 
                BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                onpageindexchanging="gdvUsuarios_PageIndexChanging" Enabled="False" 
                        BackColor="White" onrowdatabound="gdvUsuarios_RowDataBound">
                        <Columns>
                            <asp:CommandField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                        SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>"
                        ShowSelectButton="True" HeaderStyle-HorizontalAlign="Left" >
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:CommandField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblnombreusuarioSop %>" 
                        HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblnombreusuario" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Width="300px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblUsuarioSop %>" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblusuariosopg" runat="server" Text='<%# Bind("usuario") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblCorreoSop %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblidusuario" runat="server" 
                                Text='<%# Bind("id_usuario_soporte") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True" DeleteText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" />
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:literal ID="Literal1" runat="server" 
                        text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                        </EmptyDataTemplate>
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="White" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                        <SortedAscendingHeaderStyle BackColor="#487575" />
                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                        <SortedDescendingHeaderStyle BackColor="#275353" />
                    </asp:GridView>
               <%-- </asp:Panel>--%>
                
            </td>
        </tr>

        <tr>
            <td  align="center" colspan="2">
     

                <asp:LinkButton ID="lnkModal" runat="server"></asp:LinkButton>
                  
               <cc1:ModalPopupExtender ID="mpePanel" runat="server" 
                    backgroundcssclass="modalBackground" popupcontrolid="Panel12" 
                    popupdraghandlecontrolid="" targetcontrolid="lnkModal" CancelControlID="imgbtnCerrar">
                </cc1:ModalPopupExtender>
            &nbsp;</td>
        </tr>

        <tr>
            <td  align="center" colspan="2">
 <asp:Panel ID="Panel12" runat="server" Width="435px" BackColor="White" BorderColor="Black" 
                    BorderStyle="Double">
     <table bgcolor="White">
         <tr>
             <td bgcolor="#004D71" align="right" valign="top">
                 
                 <asp:ImageButton ID="imgbtnCerrar" runat="server" 
                     ImageUrl="~/Imagenes/error_sign.gif" />
             </td>
         </tr>
         <tr>
             <td>
                 <div class="accountInfo">
                     <fieldset class="changePassword" 
                         style="width:400px; height:200px; text-align: left;">
                         <legend>
                             <asp:Literal ID="Literal2" runat="server" 
                                 Text="<%$ Resources:resCorpusCFDIEs, lblSubUsrPass %>" />
                         </legend>
                         <p>
                             <asp:Label ID="lblContraseniaAnterior" runat="server" 
                                 AssociatedControlID="txtContraseniaAnterior" 
                                 Text="<%$ Resources:resCorpusCFDIEs, lblContraseniaAnterior %>"></asp:Label>
                             <asp:TextBox ID="txtContraseniaAnterior" runat="server" 
                                 CssClass="passwordEntry" MaxLength="50" TextMode="Password" Width="200px"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" 
                                 ControlToValidate="txtContraseniaAnterior" CssClass="failureNotification" 
                                 ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaAnterior %>" 
                                 ToolTip="<%$ Resources:resCorpusCFDIEs, valContraseniaAnterior %>" 
                                 ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                         </p>
                         <p>
                             <asp:Label ID="lblContraseniaNueva" runat="server" 
                                 AssociatedControlID="txtContraseniaNueva" 
                                 Text="<%$ Resources:resCorpusCFDIEs, lblContraseniaNueva %>"></asp:Label>
                             <asp:TextBox ID="txtContraseniaNueva" runat="server" CssClass="passwordEntry" 
                                 MaxLength="50" TextMode="Password" Width="200px"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" 
                                 ControlToValidate="txtContraseniaNueva" CssClass="failureNotification" 
                                 Display="Dynamic" 
                                 ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>" 
                                 ToolTip="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>" 
                                 ValidationGroup="ChangeUserPasswordValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="regxNueva" runat="server" 
                                 ControlToValidate="txtContraseniaNueva" CssClass="failureNotification" 
                                 Display="Dynamic" Height="16px" ToolTip="Contraseña incompleta" 
                                 ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$" 
                                 ValidationGroup="ChangeUserPasswordValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                         </p>
                         <p>
                             <asp:Label ID="lblConfirmaNueva" runat="server" 
                                 AssociatedControlID="txtConfirmaNueva" 
                                 Text="<%$ Resources:resCorpusCFDIEs, lblConfirmaNueva %>"></asp:Label>
                             <asp:TextBox ID="txtConfirmaNueva" runat="server" CssClass="passwordEntry" 
                                 MaxLength="50" TextMode="Password" Width="200px"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" 
                                 ControlToValidate="txtConfirmaNueva" CssClass="failureNotification" 
                                 Display="Dynamic" ErrorMessage="" 
                                 ToolTip="<%$ Resources:resCorpusCFDIEs, valConfirmaNueva %>" 
                                 ValidationGroup="ChangeUserPasswordValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                             <asp:CompareValidator ID="NewPasswordCompare" runat="server" 
                                 ControlToCompare="txtContraseniaNueva" ControlToValidate="txtConfirmaNueva" 
                                 CssClass="failureNotification" Display="Dynamic" ErrorMessage="" 
                                 ToolTip="<%$ Resources:resCorpusCFDIEs, valConNewConf %>" 
                                 ValidationGroup="ChangeUserPasswordValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /></asp:CompareValidator>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                 ControlToValidate="txtConfirmaNueva" CssClass="failureNotification" 
                                 Display="Dynamic" ToolTip="Contraseña incompleta." 
                                 ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$" 
                                 ValidationGroup="ChangeUserPasswordValidationGroup"><img 
                            src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                         </p>
                     </fieldset>
                 </div>
                
                 <asp:Button ID="btnValidar" runat="server" CommandName="ChangePassword" 
                     CssClass="botonEstilo" onclick="btnValidar_Click" 
                     Text="<%$ Resources:resCorpusCFDIEs, btnSiguiente %>" 
                     ValidationGroup="ChangeUserPasswordValidationGroup" />
             </td>
         </tr>
     </table>
              </asp:Panel>
            </td>
        </tr>

        </table>

</asp:Content>


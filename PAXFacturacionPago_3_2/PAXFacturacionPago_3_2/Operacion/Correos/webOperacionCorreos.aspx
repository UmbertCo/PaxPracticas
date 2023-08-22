<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionCorreos.aspx.cs" Inherits="Operacion_Correos_webOperacionCorreos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

  <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varCatalogoRFC %>"></asp:Label>

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
                <asp:Label ID="lblCorreo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEmailCorreo%>"></asp:Label>
            </td>
            <td class="style3">
                <asp:Label ID="lblCorreo0" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblContrasenaCorreo%>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style5">
                <asp:TextBox ID="txtCorreo" runat="server" TabIndex="1" Width="280px"></asp:TextBox>
                            
                              <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                    ControlToValidate="txtCorreo" 
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="RegisterUserValidationGroup" 
                    CssClass="failureNotification" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>"  Display="Dynamic"><img src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                    ControlToValidate="txtCorreo" CssClass="failureNotification" Display="Dynamic" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
            </td>
            <td class="style3">
                <asp:TextBox ID="txtPass" runat="server" TabIndex="2" Width="280px" 
                    TextMode="Password"></asp:TextBox>
                            
                <asp:RequiredFieldValidator ID="EmailRequired0" runat="server" 
                    ControlToValidate="txtPass" CssClass="failureNotification" Display="Dynamic" 
                    ToolTip="Contraseña Requerida." 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style5">
                <asp:Label ID="lblEstatus" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblEstatus %>"></asp:Label>
                </td>
            <td class="style3">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style5">
                <asp:DropDownList ID="ddlEstatus" runat="server" Height="23px" Width="150px" 
                    TabIndex="3">
                    <asp:ListItem Selected="True" Value="A">Activo</asp:ListItem>
                    <asp:ListItem Value="B">Baja</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="style3">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style5">
                <asp:Label ID="lblEstatus0" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>"></asp:Label>
                </td>
            <td class="style3">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style5">
                <asp:TextBox ID="txtRFC" runat="server" TabIndex="4" Width="280px"></asp:TextBox>
                            
                <asp:RequiredFieldValidator ID="valRFC" runat="server" 
                    ControlToValidate="txtRFC" CssClass="failureNotification" Display="Dynamic" 
                    ErrorMessage="<%$ Resources:resCorpusCFDIEs, valContraseniaNueva %>" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, txtRFC %>" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="regxRFC" runat="server" 
                    ControlToValidate="txtRFC" CssClass="failureNotification" Display="Dynamic" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxRFC %>" 
                    ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A-Z]" 
                    ValidationGroup="RegisterUserValidationGroup"><img 
                    src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
            </td>
            <td class="style3">
                            &nbsp;</td>
        </tr>
        <tr>
            <td class="style5">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style5">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style4">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr align="left">
            <td class="style4">
                &nbsp;</td>
            <td align="left">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnNuevoCorreo" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo%>" Width="90px" 
                    onclick="btnNuevoUsuario_Click" CssClass="botonEstilo" TabIndex="5" 
                    Height="35px" />
            &nbsp;<asp:Button ID="btnCambiarPwd" runat="server" onclick="btnCambiarPwd_Click" 
                    TabIndex="6" Text="<%$ Resources:resCorpusCFDIEs, lblCambiar%>" 
                    Width="150px" Enabled="False" CssClass="botonGrande" Height="35px" />
                            <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="<%$ Resources:resCorpusCFDIEs, btnExcel%>" 
                                Width="160px" CssClass="botonGrande" Visible="False" 
                    TabIndex="7" Height="35px" />
                <asp:Button ID="btnGuardar" runat="server" onclick="btnGuardar_Click" 
                    TabIndex="8" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar%>" 
                    Width="90px" Enabled="False" CssClass="botonEstilo" 
                    ValidationGroup="RegisterUserValidationGroup" Height="35px" />
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
                    <asp:GridView ID="gdvCorreos" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" GridLines="Horizontal" Width="799px" 
                DataKeyNames="idCorreo" 
                onselectedindexchanged="gdvCorreos_SelectedIndexChanged" 
                onrowdeleting="gdvCorreos_RowDeleting" 
                BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                onpageindexchanging="gdvCorreos_PageIndexChanging" Enabled="False" 
                        BackColor="White" onrowdatabound="gdvCorreos_RowDataBound">
                        <Columns>
                            <asp:CommandField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                        SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>"
                        ShowSelectButton="True" HeaderStyle-HorizontalAlign="Left" >
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:CommandField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblidCorreo" runat="server" Text='<%# Bind("idCorreo") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEmailCorreo %>" 
                        HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblCorreoElectronico" runat="server" 
                                        Text='<%# Bind("CorreoElectronico") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Width="300px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblContrasenaCorreo %>" 
                                HeaderStyle-HorizontalAlign="Left" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblcontrasenia" runat="server" Text='<%# Bind("Password") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("Estatus") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblidContribuyente" runat="server" 
                                Text='<%# Bind("idContribuyente") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True" DeleteText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:CommandField>
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


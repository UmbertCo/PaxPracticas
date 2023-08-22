<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionRfc.aspx.cs" Inherits="Operacion_RFC_webOperacionRfc" %>

   
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varCatalogoRFC %>"></asp:Label>
    </h2>
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
</script>

        <div style="text-align:center;" >
            <asp:UpdateProgress ID="uppConsultas" runat="server"  >
                    <progresstemplate>
                        <img alt="" 
                    src="../../Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>
    </div>
    <br />

  <%--  <asp:UpdatePanel ID="upFormulario" runat="server">
        <ContentTemplate>--%>
            <asp:Panel ID="pnlFormulario" runat="server">
            <div>
            <fieldset>
            <legend><asp:Literal ID="LiteralRfc" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosRFC %>" /></legend>
                <p></p>
            <table><tr><td valign="top">

                     <p align="left">
                        <asp:Label ID="lblRFC" runat="server" AssociatedControlID="txtRFC" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>"></asp:Label>
                    </p>
                    <p align="left">
                        <asp:TextBox ID="txtRFC" runat="server" CssClass="textEntry" TabIndex="1" 
                            ToolTip="AAA000000AAA" Enabled="False"></asp:TextBox>
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
                    </p>
                    <p>
                        <asp:Label ID="lblNombreCompleto" runat="server" 
                            AssociatedControlID="txtNombre" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                    </p>
                     <p>
                         <asp:TextBox ID="txtNombre" runat="server" CssClass="textEntry" Enabled="False" 
                             MaxLength="255" TabIndex="1" Width="300px"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="UserNameRequired0" runat="server" 
                             ControlToValidate="txtNombre" CssClass="failureNotification" 
                             ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                             ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                             ValidationGroup="RegisterUserValidationGroup" Width="20px">*</asp:RequiredFieldValidator>
                     </p>
                     <p>
                         <asp:Label ID="lblNombreSucursal2" runat="server" 
                             AssociatedControlID="ddlSucursales" 
                             Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>"></asp:Label>
                     </p>
                     <p>
                         <asp:DropDownList ID="ddlSucursales" runat="server" AutoPostBack="True" 
                             DataTextField="nombre" DataValueField="id_estructura" Enabled="False" 
                             onselectedindexchanged="ddlSucursales_SelectedIndexChanged" TabIndex="4" 
                             Width="300px">
                         </asp:DropDownList>
                     </p>
                     <p>
                         <asp:Label ID="lblNombreSucursal3" runat="server" 
                             Text="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>"></asp:Label>
                     </p>
                     <p>
                         <asp:DropDownList ID="ddlUsuarios" runat="server" DataTextField="clave_usuario" 
                             DataValueField="id_contribuyente" Enabled="False" TabIndex="4" Width="300px">
                         </asp:DropDownList>
                     </p>
                     <p>
                         <asp:Label ID="lblVersion0" runat="server" 
                             Text="<%$ Resources:resCorpusCFDIEs, lblVersion %>"></asp:Label>
                     </p>
                     <p>
                         <asp:DropDownList ID="drpVersion" runat="server" AutoPostBack="True" 
                             DataTextField="version" DataValueField="id_version" Enabled="False">
                         </asp:DropDownList>
                     </p>
            </td><td>
                
                   <p>
                       &nbsp;</p>
                    <p>
                        <asp:Label ID="lblCer0" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblCer %>"></asp:Label>
                    </p>
                    <p>
                        <asp:FileUpload ID="fupCer" runat="server" Enabled="False" TabIndex="15" />
                        <asp:RequiredFieldValidator ID="rfvCer" runat="server" 
                            ControlToValidate="fupCer" CssClass="failureNotification" Display="Dynamic" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, valCer %>" 
                            ValidationGroup="RegisterUserValidationGroup"><img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="valCer" runat="server" 
                            ControlToValidate="fupCer" CssClass="failureNotification" Display="Dynamic" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, rfvCer %>" 
                            ValidationExpression="^.*\.(cer)$" 
                            ValidationGroup="RegisterUserValidationGroup"><img 
                        src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                    </p>
                   <p>
                       <asp:Label ID="lblKey" runat="server" 
                           Text="<%$ Resources:resCorpusCFDIEs, lblKey %>"></asp:Label>
                    </p>
                    <p>
                        <asp:FileUpload ID="fupKey" runat="server" TabIndex="14" Enabled="False" />
                        <asp:RequiredFieldValidator ID="rfvKey" runat="server" 
                            ControlToValidate="fupKey" CssClass="failureNotification" Display="Dynamic" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, valKey %>" 
                            ValidationGroup="RegisterUserValidationGroup"><img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regxKey" runat="server" 
                            ControlToValidate="fupKey" CssClass="failureNotification" Display="Dynamic" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, regxKey %>" 
                            ValidationExpression="^.*\.(key)$" 
                            ValidationGroup="RegisterUserValidationGroup"><img 
                        src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                    </p>
                   <p>
                       <asp:Label ID="lblContrasenia" runat="server" 
                           Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>"></asp:Label>
                    </p>
                    <p>
                        <asp:TextBox ID="txtPass" runat="server" TabIndex="16" TextMode="Password" 
                            Enabled="False"></asp:TextBox>
                    </p>
                   <p>
                       <asp:Label ID="lblNombreSucursal0" runat="server" 
                           Text="<%$ Resources:resCorpusCFDIEs, lbllogo %>"></asp:Label>
                    </p>
                       <p>
                           <asp:FileUpload ID="fupLogo" runat="server" Enabled="False" TabIndex="15" />
                       </p>
                       <p>
                           &nbsp;</p>
                       </td></tr>
                </table>
            
            </fieldset>
                <table align="right">
                    <tr>
                        <td valign="top">
                            <p>
                                <asp:Button ID="btnNuevoUsuario" runat="server" CssClass="botonEstilo" 
                                    Height="36px" onclick="btnNuevoUsuario_Click" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblNuevoRFC%>" Width="80px" />
                                <asp:Button ID="btnGuardarActualizar" runat="server" CssClass="botonEstilo" 
                                    Height="36px" onclick="btnGuardarActualizar_Click" 
                                    onclientclick="clickOnce(this, 'Procesando')" TabIndex="14" 
                                    Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" UseSubmitBehavior="False" 
                                    ValidationGroup="RegisterUserValidationGroup" Visible="False" Width="80px" />
                                <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" 
                                    onclick="btnNCancelar_Click" 
                                    Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" />
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
                                BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                                CellPadding="4" DataKeyNames="id_rfc" GridLines="Horizontal" 
                                onrowcommand="gdvRFCs_RowCommand" onrowdatabound="gdvRFCs_RowDataBound" 
                                onrowdeleting="gdvRFCs_RowDeleting" 
                                onselectedindexchanged="gdvRFCs_SelectedIndexChanged" Width="799px">
                                <Columns>
                                    <asp:CommandField HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                                        SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                                        ShowSelectButton="True">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:CommandField>
                                    <asp:ButtonField ButtonType="Image" CommandName="img" 
                                        HeaderText="<%$ Resources:resCorpusCFDIEs, lbllogo %>" 
                                        ImageUrl="~/Imagenes/lupa.png">
                                    <ControlStyle Height="20px" Width="20px" />
                                    <ItemStyle Height="20px" Width="20px" />
                                    </asp:ButtonField>
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
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblidcontribuyente" runat="server" 
                                                Text='<%# Bind("id_contribuyente") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
                                    <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstructura %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblnombre" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblidestructura" runat="server" 
                                                Text='<%# Bind("id_estructura") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowDeleteButton="True" />
                                    <asp:CheckBoxField />
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbSeleccion" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblidpadre" runat="server" Text='<%# Bind("id_padre") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblidusuario" runat="server" Text='<%# Bind("id_usuario") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Literal ID="Literal1" runat="server" 
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
                        </td>
                    </tr>
                </table>
            </div>
            <p style="text-align:right;" >
                &nbsp;</p>
            </asp:Panel>
           <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
<br />
</asp:Content>


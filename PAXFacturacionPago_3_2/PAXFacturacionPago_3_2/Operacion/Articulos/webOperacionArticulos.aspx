<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionArticulos.aspx.cs" Inherits="Operacion_Articulos_webOperacionArticulos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">

        .fontText
        {
            font: 8pt verdana;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

  <h2>
       <asp:Label ID="lblTitulo" runat="server" 
           Text="<%$ Resources:resCorpusCFDIEs, lblArticulosCat %>"></asp:Label>
    </h2>
      
     <script language="javascript" type="text/javascript" >
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
                <fieldset class="register" style=" width:862px;" 
        __designer:mapid="6d5">
                    <legend __designer:mapid="6d6">
                        <asp:Literal ID="Literal2" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, mnuArticulos %>" />
                    </legend>

    <table>
        
        <tr>
            <td>
                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        
        <tr>
            <td>
                <asp:DropDownList ID="ddlSucursales" runat="server" Width="200px" TabIndex="1" 
                    DataTextField="nombre" DataValueField="id_estructura" AutoPostBack="True" 
                    onselectedindexchanged="ddlSucursales_SelectedIndexChanged">
                </asp:DropDownList>

            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        
        <tr>
            <td>
                <asp:Label ID="Label12" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblCodigo %>"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        
        <tr>
            <td>
                <asp:TextBox ID="txtClave" runat="server" TabIndex="2" Enabled="False" 
                    MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="revPrecio3" runat="server" 
                                ControlToValidate="txtClave" CssClass="failureNotification" 
                                ErrorMessage="Clave requerida." ToolTip="Clave requerida." 
                                ValidationGroup="sucursalValidationGroup" Display="Dynamic"> <img src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>

            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDescripcion %>"></asp:Label>
            </td>
            <td>
                </td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblMedida %>"></asp:Label>
            </td>
            <td>
                </td>
            <td>
                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblPrecio %>"></asp:Label>
            </td>
            <td>
                </td>
            <td>
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblIva %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtDescripcion" runat="server" Width="290px" TabIndex="3" 
                    Enabled="False" Height="80px" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="revPrecio2" runat="server" 
                                ControlToValidate="txtDescripcion" CssClass="failureNotification" 
                                ErrorMessage="Descripción requerida." ToolTip="Descripción requerida." 
                                ValidationGroup="sucursalValidationGroup" Display="Dynamic"> <img src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>

            </td>
            <td>
               </td>
            <td valign="top">
                <asp:TextBox ID="txtMedida" runat="server" TabIndex="4" Enabled="False" 
                    MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="revPrecio1" runat="server" 
                                ControlToValidate="txtMedida" CssClass="failureNotification" 
                                ErrorMessage="Medida requerida." ToolTip="Medida requerida." 
                                ValidationGroup="sucursalValidationGroup" Display="Dynamic"> <img src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>

            </td>
            <td>
                </td>
            <td valign="top">
                <asp:TextBox ID="txtPrecio" runat="server" TabIndex="5" Enabled="False"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="revPrecio" runat="server" 
                                ControlToValidate="txtPrecio" CssClass="failureNotification" 
                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, revPrecio %>" ToolTip="<%$ Resources:resCorpusCFDIEs, revPrecio %>" 
                                ValidationGroup="sucursalValidationGroup" Display="Dynamic"> <img src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>

                            <asp:RegularExpressionValidator ID="regxPrecio" runat="server" 
                                ControlToValidate="txtPrecio" ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$"
                                ValidationGroup="sucursalValidationGroup" 
                                CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                    Display="Dynamic"><img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
            </td>
            <td>
               </td>
            <td valign="top">

                            <asp:DropDownList ID="txtIVACon" runat="server" TabIndex="6" Width="150px" 
                                CssClass="fontText" Enabled="False">
                                <asp:ListItem Selected="True">16.00</asp:ListItem>
                                <asp:ListItem>11.00</asp:ListItem>
                                <asp:ListItem>0.00</asp:ListItem>
                                <asp:ListItem Value="Exento">Exento</asp:ListItem>
                            </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                </td>
            <td>
                </td>
            <td>
                </td>
            <td>
                </td>
            <td>
                </td>
            <td>
                </td>
            <td>
                </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblIeps %>" Visible="false"></asp:Label>
            </td>
            <td>
                </td>
            <td>
                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblIsr %>"></asp:Label>
            </td>
            <td>
               </td>
            <td>
                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblIvaRetenido %>"></asp:Label>
            </td>
            <td>
                </td>
            <td>
                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblMoneda %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>

                            <asp:DropDownList ID="txtIEPS" runat="server" TabIndex="6" Width="150px" 
                                CssClass="fontText" Enabled="False" Visible="false">
                                <asp:ListItem>0.360000</asp:ListItem>
                                <asp:ListItem>0.439200</asp:ListItem>
                                <asp:ListItem>0.298800</asp:ListItem>
                                <asp:ListItem Selected="True">0</asp:ListItem>
                            </asp:DropDownList>

<%--                            <asp:RegularExpressionValidator ID="regxPrecio1" runat="server" 
                                ControlToValidate="txtIEPS" ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,2})?$"
                                ValidationGroup="sucursalValidationGroup" 
                                CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                    Display="Dynamic"><img src="../../Imagenes/error_sign.gif" /> 
                            </asp:RegularExpressionValidator>--%>
                        </td>
            <td>
                </td>
            <td>
                <asp:TextBox ID="txtISR" runat="server" TabIndex="8" Enabled="False"></asp:TextBox>

                            <asp:RegularExpressionValidator ID="regxPrecio2" runat="server" 
                                ControlToValidate="txtISR" ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$"
                                ValidationGroup="sucursalValidationGroup" 
                                CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                    Display="Dynamic"><img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
            </td>
            <td>
                </td>
            <td>
                <asp:TextBox ID="txtIVARet" runat="server" TabIndex="9" Enabled="False"></asp:TextBox>

                            <asp:RegularExpressionValidator ID="regxPrecio3" runat="server" 
                                ControlToValidate="txtIVARet" ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$"
                                ValidationGroup="sucursalValidationGroup" 
                                CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                    Display="Dynamic"><img src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
            </td>
            <td>
                </td>
            <td>
                                <asp:DropDownList ID="ddlMoneda" runat="server" Width="150px" 
                    TabIndex="10" Enabled="False">
                                    <asp:ListItem Value="MXN">Pesos Mexicanos</asp:ListItem>
                                    <asp:ListItem Value="USD">Dólares</asp:ListItem>
                                    <asp:ListItem Value="XEU">Euros</asp:ListItem>
                                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                </td>
            <td>
                &nbsp;</td>
            <td>
                </td>
            <td>
                </td>
            <td>
                </td>
            <td>
                </td>
        </tr>
        <tr>
            <td>
                <%--<asp:Label ID="lblISH" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblISH %>" 
                    AssociatedControlID="txtISH" Visible="False"></asp:Label>--%>
            </td>
            <td>
                &nbsp;</td>
            <td>
<%--                <asp:Label ID="lblCNG" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblCNG %>" 
                    AssociatedControlID="txtCNG" Visible="False"></asp:Label>--%>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <%--<asp:TextBox ID="txtISH" runat="server" TabIndex="11" Enabled="False" 
                    EnableTheming="True" Visible="False"></asp:TextBox>--%>

                            </td>
            <td>
                &nbsp;</td>
            <td>
          <%--      <asp:TextBox ID="txtCNG" runat="server" TabIndex="12" Enabled="False" 
                    Visible="False"></asp:TextBox>--%>

                            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>

                </fieldset></p>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="630">
                            <asp:Button ID="btnExcel" runat="server" onclick="btnExcel_Click" Text="<%$ Resources:resCorpusCFDIEs, btnExcel%>" 
                                Width="154px" CssClass="botonGrande" Visible="False" 
                    TabIndex="16" Height="27px" />
                        </td>
                        <td>
                <asp:Button ID="btnNuevoCorreo" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo%>" Width="81px" 
                    onclick="btnNuevoUsuario_Click" CssClass="botonEstilo" TabIndex="13" Height="27px" />
                        </td>
                        <td>
                           <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" 
                               onclick="btnNCancelar_Click" 
                               Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                    Width="81px" TabIndex="14" Height="27px" />
                        </td>
                        <td>
                <asp:Button ID="btnGuardar" runat="server" onclick="btnGuardar_Click" 
                    TabIndex="15" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar%>" 
                    Width="81px" Enabled="False" CssClass="botonEstilo" 
                    ValidationGroup="sucursalValidationGroup" 
                    onclientclick="clickOnce(this, 'Procesando')" UseSubmitBehavior="False" 
                                Height="27px" />
                        </td>
                    </tr>
    </table>

    <br />

    <table>
        <tr>
            <td>
                    <asp:GridView ID="gdvArticulos" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" GridLines="Horizontal" Width="890px" 
                DataKeyNames="id_articulo" 
                BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                        BackColor="White" onpageindexchanging="gdvArticulos_PageIndexChanging" 
                        onrowdeleting="gdvArticulos_RowDeleting" 
                        onselectedindexchanged="gdvArticulos_SelectedIndexChanged" 
                        AllowPaging="True" TabIndex="15" AllowSorting="True" 
                        onsorting="gdvArticulos_Sorting"               

                        >
                        
                        <Columns>
                            <asp:CommandField SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>"
                        ShowSelectButton="True" HeaderStyle-HorizontalAlign="Left" >
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:CommandField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblCodigo %>" SortExpression="clave">
                                <ItemTemplate>
                                    <asp:Label ID="lblclaveart" runat="server" Text='<%# Bind("clave") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbleidArticulo" runat="server" Text='<%# Bind("id_articulo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblDescripcion %>" 
                        HeaderStyle-HorizontalAlign="Left" SortExpression="descripcion">
                                <ItemTemplate>
                                    <asp:Label ID="lbledescripcion" runat="server" 
                                        Text='<%# Bind("descripcion") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Width="300px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblMedida %>" 
                                HeaderStyle-HorizontalAlign="Left" SortExpression="medida">
                                <ItemTemplate>
                                    <asp:Label ID="lblemedida" runat="server" Text='<%# Bind("medida") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="precio" DataFormatString="{0:C2}" 
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblPrecio %>"  SortExpression="precio"/>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblIva %>" 
                                HeaderStyle-HorizontalAlign="Left" SortExpression="iva">
                                <ItemTemplate>
                                    <asp:Label ID="lbleiva" runat="server" 
                                Text='<%# Bind("iva") %>'></asp:Label>
                                </ItemTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblIeps %>" 
                                HeaderStyle-HorizontalAlign="Left" SortExpression ="ieps" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbleieps" runat="server" Text='<%# Bind("ieps") %>'></asp:Label>
                                </ItemTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblIsr %>" 
                                HeaderStyle-HorizontalAlign="Left" SortExpression="isr">
                                <ItemTemplate>
                                    <asp:Label ID="lbleisr" runat="server" Text='<%# Bind("isr") %>'></asp:Label>
                                </ItemTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblIvaRetenido %>" 
                                HeaderStyle-HorizontalAlign="Left" SortExpression="ivaretenido">
                                <ItemTemplate>
                                    <asp:Label ID="lbleivaretenido" runat="server" Text='<%# Bind("ivaretenido") %>'></asp:Label>
                                </ItemTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblMoneda %>" 
                                HeaderStyle-HorizontalAlign="Left" SortExpression="moneda">
                                <ItemTemplate>
                                    <asp:Label ID="lblemoneda" runat="server" Text='<%# Bind("moneda") %>'></asp:Label>
                                </ItemTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>" 
                                HeaderStyle-HorizontalAlign="Left" Visible="False" SortExpression="id_estructura">
                                <ItemTemplate>
                                    <asp:Label ID="lbleidestructura" runat="server" 
                                        Text='<%# Bind("id_estructura") %>'></asp:Label>
                                </ItemTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus %>" 
                                HeaderStyle-HorizontalAlign="Left" Visible="False" SortExpression="estatus">
                            
                                <ItemTemplate>
                                    <asp:Label ID="lbleestatus" runat="server" Text='<%# Bind("estatus") %>'></asp:Label>
                                </ItemTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True" DeleteText="<%$ Resources:resCorpusCFDIEs, btnPassBajaProc %>"/>
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
                        <SortedAscendingHeaderStyle BackColor="#487575" CssClass="sortasc-header" 
                            HorizontalAlign="Left" />                        
                        <SortedDescendingHeaderStyle BackColor="#275353" CssClass="sortdesc-header" 
                            HorizontalAlign="Left" />
                    </asp:GridView>
               </td>
        </tr>
    </table>

</asp:Content>


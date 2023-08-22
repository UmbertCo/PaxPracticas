<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionAsignaCreditosDistribuidores.aspx.cs" Inherits="Operacion_Distribuidores_webOperacionAsignaCreditosDistribuidores" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">  

    <style type="text/css">
        .style2
        {
            width: 583px;
        }
    </style>
   

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

 <script type="text/javascript" language="javascript">

     function confirmation() 
     {
         var agree = confirm('¿Está usted seguro de querer eliminar este registro?');

         if (agree)
             return true;
         else
             return false;
     }

     function confirmationGuardar() {
         var agree = confirm('¿La información que capturo es correcta?');

         if (agree)
             return true;
         else
             return false;
     }
 </script>
    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, LblDistribuidorAsigna %>"></asp:Label>
    </h2>
    <asp:UpdatePanel ID="upRFC" runat="server">
    <ContentTemplate>
     <asp:HiddenField ID="hdIdRfc" runat="server" Visible="False"></asp:HiddenField>
            <asp:Panel runat="server" ID="pnlDistribuidor" >
                <table width="925px">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCreditosDistDisp" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblCreditos %>"></asp:Label>
                            &nbsp;<asp:Label ID="lbCreditosDisponibles" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCodDistDes" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblCodDisDes %>"></asp:Label>
                            <asp:Label ID="lblCodDis" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                </table>
                &nbsp;<br />
            </asp:Panel>
        <fieldset>
            <legend>
                <asp:Literal ID="Literal3" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, LblDistribuidores %>" />
            </legend>
            <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                Width="824px">
                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, lblCreditosUser %>">
                    <ContentTemplate>
                        <p>
                        </p>
                        <p>
                            <asp:Label ID="lblUsuario" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>"></asp:Label>
                        </p>
                        <p>
                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry" 
                                Enabled="False" MaxLength="255" ReadOnly="True" TabIndex="1" 
                                ToolTip="AAA000000AAA"></asp:TextBox>
                        </p>
                        <p>
                            <asp:Label ID="lblEmail" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblEmailCorreo %>"></asp:Label>
                        </p>
                        <p>
                            <asp:TextBox ID="txtemail" runat="server" CssClass="textEntry" Enabled="False" 
                                MaxLength="255" ReadOnly="True" TabIndex="2"></asp:TextBox>
                        </p>
                        <p>
                            <asp:Label ID="lblAcceso" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblAcceso %>"></asp:Label>
                        </p>
                        <p>
                            <asp:DropDownList ID="ddlAcceso" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="ddlAcceso_SelectedIndexChanged" TabIndex="3" 
                                Width="300px">
                                <asp:ListItem Selected="True" Value="R">Restringido</asp:ListItem>
                                <asp:ListItem Value="L">Libre</asp:ListItem>
                            </asp:DropDownList>
                        </p>

                        <p>
                            &nbsp;</p>
                        <p>
                            <asp:Label ID="lblServicios" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblservicios %>" Visible="False"></asp:Label>
                        </p>

                        <asp:GridView ID="grdServiciosAsig" runat="server" AutoGenerateColumns="False" 
                            Width="808px">
                            <Columns>
                                <asp:BoundField DataField="id_servicios">
                                <ItemStyle Width="25px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Descripcion" HeaderText="<%$ Resources:resCorpusCFDIEs, lblDescripcion %>" />
                                <asp:BoundField DataField="Precio" HeaderText="<%$ Resources:resCorpusCFDIEs, lblcreditosxservicio %>" />
                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblasignarcreditos %>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbImporte" runat="server" Width="100px" AutoPostBack="True" 
                                            ontextchanged="tbImporte_TextChanged" MaxLength="9"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="regxPrecioArt3" runat="server" 
                                            ControlToValidate="tbImporte" CssClass="failureNotification" Display="Dynamic" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                            ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                            ValidationGroup="AgregarCreditos">
                                                <img 
    src="../../Imagenes/error_sign.gif" /> 
                                            </asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblPrecioUnit %>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPrecio" runat="server" AutoPostBack="True" 
                                            ontextchanged="txtPrecio_TextChanged" Width="100px" MaxLength="9"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="regxPrecioArt2" runat="server" 
                                            ControlToValidate="txtPrecio" CssClass="failureNotification" Display="Dynamic" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                            ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                            ValidationGroup="AgregarCreditos">
                                                <img 
    src="../../Imagenes/error_sign.gif" /> 
                                            </asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lbltotcreditos %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#5A737E" />
                            <HeaderStyle BackColor="#5A737E" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#92BA41" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                        <br />
                        <br />
                        <p text-align:right;"="">
                            <table style="width: 808px">
                                <tr>
                                    <td width="603">
                                        &nbsp;</td>
                                    <td align="right">
                                        <asp:Label ID="lblUsuario3" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblCreditosUser %>" 
                                            ViewStateMode="Enabled" Visible="False"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcreditos" runat="server" ReadOnly="True" Visible="False" 
                                            Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        &nbsp;</td>
                                    <td>
                                        <asp:Button ID="btnGuardar" runat="server" CommandName="MoveNext" 
                                            CssClass="botonEstilo" Height="35px" OnClick="btnGuardar_Click" TabIndex="6" 
                                            Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                                            ValidationGroup="AgregarCreditos" Visible="False" Width="80px" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancelarRfc" runat="server" CssClass="botonEstilo" 
                                            Height="35px" OnClick="btnCancelarRfc_Click" TabIndex="7" 
                                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Visible="False" 
                                            Width="80px" />
                                    </td>
                                </tr>
                            </table>
                        </p>
                        <p text-align:right;"="">
                            &nbsp;</p>
                        <br />
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, btnActualizarServicios %>">
                    <ContentTemplate>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblServicios1" runat="server" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblservicios %>"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cbServicios" runat="server">
                                    </asp:CheckBoxList>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnActualiza" runat="server" CssClass="botonGrande" 
                                        Height="25px" OnClick="btnActualiza_Click" 
                                        Text="<%$ Resources:resCorpusCFDIEs, btnActualizarServicios %>" Width="160px" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
            <br />
        </fieldset><br />
             <table>
                 <tr>
                     <td>
                         <table cellpadding="0" cellspacing="0" class="style1">
                             <tr>
                                 <td>
                                     <asp:Label ID="lblUsuarioFiltro" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>"></asp:Label>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtUsuarioFiltro" runat="server" TabIndex="8" Width="200px"></asp:TextBox>
                                     <br />
                                 </td>
                                 <td>
                                     &nbsp;</td>
                             </tr>
                             <tr>
                                 <td>
                                     <asp:Label ID="lblCorreoFiltro" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEmailCorreo %>"></asp:Label>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtCorreoFiltro" runat="server" TabIndex="9" Width="300px"></asp:TextBox>
                                     <br />
                                 </td>
                                 <td>
                                     &nbsp;</td>
                             </tr>
                             <tr>
                                 <td>
                                     <asp:Label ID="lblAccesoFiltro" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblAcceso %>"></asp:Label>
                                 </td>
                                 <td>
                                     <asp:DropDownList ID="ddlAccesoFiltro" runat="server" AutoPostBack="True" 
                                         TabIndex="10" Width="200px">
                                         <asp:ListItem Selected="True" Value="T">Todos</asp:ListItem>
                                         <asp:ListItem Value="R">Restringido</asp:ListItem>
                                         <asp:ListItem Value="L">Libre</asp:ListItem>
                                     </asp:DropDownList>
                                 </td>
                                 <td>
                                     <asp:Button ID="btnFiltrar" runat="server" onclick="btnFiltrar_Click" 
                                         Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                                         CssClass="botonEstilo" TabIndex="11" />
                                 </td>
                             </tr>
                         </table>
                     </td>
                 </tr>
                 <tr>
                     
                     <td>
                         <asp:GridView ID="gdvClientes" runat="server" AllowPaging="True" 
                             AutoGenerateColumns="False" BackColor="White" BorderColor="#336666" 
                             BorderStyle="Double" BorderWidth="3px" CellPadding="4" 
                             DataKeyNames="id_usuario" GridLines="Horizontal" 
                             OnPageIndexChanging="gdvClientes_PageIndexChanging" 
                             OnRowDataBound="gdvClientes_RowDataBound" 
                             OnRowDeleting="gdvClientes_RowDeleting" 
                             OnSelectedIndexChanged="gdvClientes_SelectedIndexChanged" Width="530px" 
                             PageSize="20">
                             <Columns>
                                 <asp:CommandField SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                                     ShowSelectButton="True">
                                 <ItemStyle HorizontalAlign="Left" Width="50px" />
                                 </asp:CommandField>
                                 <asp:TemplateField Visible="False">
                                     <ItemTemplate>
                                         <asp:Label ID="lblidusuario" runat="server" Text='<%# Bind("id_usuario") %>'></asp:Label>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>">
                                     <ItemTemplate>
                                         <asp:Label ID="lblclaveusuario" runat="server" 
                                             Text='<%# Bind("clave_usuario") %>'></asp:Label>
                                     </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Left" />
                                     <ItemStyle HorizontalAlign="Left" />
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEmailCorreo %>">
                                     <ItemTemplate>
                                         <asp:Label ID="lblemailusuario" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                                     </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Left" />
                                     <ItemStyle HorizontalAlign="Left" />
                                 </asp:TemplateField>
                                 <asp:TemplateField Visible="False">
                                     <ItemTemplate>
                                         <asp:Label ID="lblidestructura" runat="server" 
                                             Text='<%# Bind("id_estructura") %>'></asp:Label>
                                     </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Left" />
                                     <ItemStyle HorizontalAlign="Left" />
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblAcceso %>">
                                     <ItemTemplate>
                                         <asp:Label ID="lblaccesousuario" runat="server" Text='<%# Bind("acceso") %>'></asp:Label>
                                     </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Left" />
                                     <ItemStyle HorizontalAlign="Left" />
                                 </asp:TemplateField>
                                 <asp:BoundField DataField="creditos" DataFormatString="{0:N2}" 
                                     HeaderText="<%$ Resources:resCorpusCFDIEs, lblCreditosUser %>">
                                 <HeaderStyle HorizontalAlign="Right" />
                                 <ItemStyle HorizontalAlign="Right" />
                                 </asp:BoundField>
                                 <asp:TemplateField>
                                     <ItemTemplate>
                                         <asp:LinkButton ID="LnkCancelar" runat="server" 
                                             CommandArgument='<%# Bind("id_usuario") %>' ForeColor="Black" 
                                             onclick="LnkCancelar_Click" 
                                             Text="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" CommandName="Delete"></asp:LinkButton>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:CommandField DeleteText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                                     ShowDeleteButton="True" Visible="False">
                                 <ItemStyle HorizontalAlign="Left" Width="50px" />
                                 </asp:CommandField>
                             </Columns>
                             <EmptyDataTemplate>
                                 <asp:Literal ID="Literal1" runat="server" 
                                     text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                             </EmptyDataTemplate>
                             <FooterStyle BackColor="White" ForeColor="#5A737E" />
                             <HeaderStyle BackColor="#5A737E" Font-Bold="True" ForeColor="White" />
                             <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                             <RowStyle BackColor="White" ForeColor="#333333" />
                             <SelectedRowStyle BackColor="#92BA41" Font-Bold="True" ForeColor="White" />
                             <SortedAscendingCellStyle BackColor="#E9E7E2" />
                             <SortedAscendingHeaderStyle BackColor="#506C8C" />
                             <SortedDescendingCellStyle BackColor="#FFFDF8" />
                             <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                         </asp:GridView>
                     </td>
                 </tr>
        </table>
        <br />
        <br />
        <br />
        <br />
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webGlobalCreditos.aspx.cs" Inherits="webGlobalCreditos" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style3
        {
            width: 111px;
        }
        .style4
        {
            width: 400px;
        }
        .style2
        {
        }
        .style5
        {
            width: 152px;
        }
        .style6
        {
            width: 41px;
            height: 34px;
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
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <script type="text/javascript" language="javascript">
    


        function confirmationGuardar() {
            var agree = confirm('¿La información que capturo es correcta?');

            if (agree)
                return true;
            else
                return false;
        }
 </script>
    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblAsignacionCreditos %>"></asp:Label>
    </h2>
    <asp:UpdatePanel runat="server" ID="updPanel1">
    <ContentTemplate>
 
                <fieldset style="width:890px;">
                    <legend><asp:Literal runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblBuscarCreditos %>" ID="Literal1" /></legend>
                    <br />
        <table cellpadding="0" cellspacing="0" width="700">
            <tr>
                <td class="style3">
                    <asp:Label ID="lblUsuario" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblclaveusuario %>"></asp:Label>
                </td>
                <td align="left" class="style4">
                    <asp:TextBox ID="txtUsuario" runat="server" style="margin-left: 0px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvClave" runat="server" 
                            ControlToValidate="txtUsuario" CssClass="failureNotification" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, valParametro %>" 
                            ValidationGroup="buscarCreditos" Width="16px">*</asp:RequiredFieldValidator>
                    </td>
                <td>
                    <asp:Label ID="lblBuscarUsu" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblclaveusuario %>"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBuscarUsu" runat="server" style="margin-left: 0px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    &nbsp;</td>
                <td class="style4">
                    &nbsp;</td>
                <td align="right">
                    &nbsp;</td>
                <td align="right">
                    <asp:Button ID="btnBuscarUsu" runat="server" CssClass="botonEstilo" 
                        Height="35px" onclick="btnBuscarUsu_Click" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" Width="80px" />
                </td>
            </tr>
            <tr>
                <td class="style3">
                    <asp:Label ID="lblCreditos" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCreditosUser %>"></asp:Label>
                </td>
                <td align="left" class="style4">
                    <asp:Label ID="lblValCred" runat="server" Font-Bold="True"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style3">
                    &nbsp;</td>
                <td class="style4">
                    &nbsp;</td>
                <td class="style3">
                    <img alt="" class="style6" 
                        src="Imagenes/la-flecha-verde-de-la-izquierda-icono-8326-96.png" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style3">
                    <asp:Label ID="lblServicios" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblservicios %>"></asp:Label>
                </td>
                <td class="style4">
                    <asp:GridView ID="grdServicios" runat="server" AutoGenerateColumns="False" 
                        Width="298px">
                        <Columns>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <asp:BoundField DataField="Precio" HeaderText="Costo" />
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
                </td>
                <td align="left">
                    &nbsp;</td>
                <td align="left">
                    <asp:GridView ID="grdBusqueda" runat="server" AutoGenerateColumns="False" 
                        Width="298px">
                        <Columns>
                            <asp:BoundField DataField="clave_usuario" HeaderText="Descripcion" />
                            <asp:BoundField DataField="estatus" HeaderText="Estatus" />
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
                </td>
            </tr>
            <tr>
                <td class="style3">
                    &nbsp;</td>
                <td class="style4">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style3">
                    &nbsp;</td>
                <td class="style4">
                    &nbsp;</td>
                <td align="right">
                    &nbsp;</td>
                <td align="right">
                    &nbsp;</td>
            </tr>
        </table>
                </fieldset><table>
                    <tr>
                        <td width="800">
                        </td>
                        <td>
                            <asp:Button ID="btnBuscar" runat="server" CssClass="botonEstilo" Height="35px" 
                                onclick="btnBuscar_Click" Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                                ValidationGroup="buscarCreditos" Width="80px" />
                        </td>
                    </tr>
                </table>
                <br /><fieldset>
                    <legend>
                        <asp:Literal runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblAgregarCreditos %>" ID="Literal2" /></legend>
                    <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                        Width="870px">
                        <cc1:TabPanel runat="server" HeaderText="Créditos" ID="TabPanel1">

                            <HeaderTemplate>
                                Créditos
                            </HeaderTemplate>

                            <ContentTemplate>
                                <br />
                                <table cellpadding="0" cellspacing="0" width="700">
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblUsuario1" runat="server" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblclaveusuario %>"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtUsuario0" runat="server" ReadOnly="True" 
                                                style="margin-left: 0px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvClave0" runat="server" 
                                                ControlToValidate="txtUsuario0" CssClass="failureNotification" 
                                                ToolTip="<%$ Resources:resCorpusCFDIEs, valParametro %>" 
                                                ValidationGroup="AgregarCreditos" Width="16px">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            <asp:Label ID="lblUsuario4" runat="server" 
                                                Text="<%$ Resources:resCorpusCFDIEs, lblservicios %>"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style2" colspan="2">
                                            <asp:GridView ID="grdServiciosAsig" runat="server" AutoGenerateColumns="False" 
                                                Width="808px">
                                                <Columns>
                                                    <asp:BoundField DataField="id_servicios" >
                                                    <ItemStyle Width="25px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                                    <asp:BoundField DataField="Precio" HeaderText="Crédito por servicio" />
                                                    <asp:TemplateField HeaderText="Créditos a asignar">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="tbImporte" runat="server" Width="100px" AutoPostBack="True" 
                                                                ontextchanged="tbImporte_TextChanged"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="regxPrecioArt0" runat="server" 
                                                                ControlToValidate="tbImporte" CssClass="failureNotification" Display="Dynamic" 
                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                                ValidationGroup="AgregarCreditos">
                                                <img 
    src="Imagenes/error_sign.gif" /> 
                                            </asp:RegularExpressionValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Precio unitario">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtPrecio" runat="server" AutoPostBack="True" 
                                                                ontextchanged="txtPrecio_TextChanged" Width="100px"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="regxPrecioArt2" runat="server" 
                                                                ControlToValidate="txtPrecio" CssClass="failureNotification" Display="Dynamic" 
                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                                                ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                                                ValidationGroup="AgregarCreditos">
                                                <img 
    src="Imagenes/error_sign.gif" /> 
                                            </asp:RegularExpressionValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total créditos">
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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            &nbsp;</td>
                                        <td align="right">
                                            <table>
                                                <tr>
                                                    <td width="500">
                                                        &nbsp;</td>
                                                    <td>
                                                        <asp:Label ID="lblUsuario3" runat="server" 
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblCreditosUser %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCreditos" runat="server" ReadOnly="True" Width="120px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style5">
                                            &nbsp;</td>
                                        <td align="right">
                                            <asp:Button ID="btnAgregar" runat="server" CssClass="botonEstilo" 
                                                Enabled="False" Height="35px" onclick="btnAgregar_Click" 
                                                Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
                                                ValidationGroup="AgregarCreditos" Width="80px" 
                                                onclientclick="return confirmationGuardar();" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>

                        </cc1:TabPanel>
                        <cc1:TabPanel runat="server" HeaderText="Actualizar Servicios" ID="TabPanel2">

                            <ContentTemplate>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblUsuario5" runat="server" 
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
                                            &nbsp;</td>
                                        <td>
                                            <asp:Button ID="btnActualiza" runat="server" CssClass="botonGrande" 
                                                Height="25px" OnClick="btnActualiza_Click" 
                                                Text="<%$ Resources:resCorpusCFDIEs, btnActualizarServicios %>" 
                                                ValidationGroup="buscarCreditos" Width="160px" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>

                        </cc1:TabPanel>
                    </cc1:TabContainer>
                    <br />

                    <asp:Panel ID="pnlCreditos" runat="server" Height="142px" Width="579px" 
                        CssClass="modal" BorderStyle="Solid" BorderWidth="1px">

                        <table align="center">
                            <tr>
                                <td align="center">


                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label121" runat="server" 
                                        Text="<%$ Resources:resCorpusCFDIEs, varSinGeneracionytimbrado %>" 
                                        Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <img alt="" src="Imagenes/Informacion.png" width="44" />
                                    <asp:Label ID="lblCosCre" runat="server" 
                                        Text="El usuario no cuenta con los permisos para accesar a este recurso."></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnAcepCreditos" runat="server" onclick="btnAcepCreditos_Click" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" CssClass="botonEstilo" />
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>

                    <cc1:modalpopupextender id="modalCreditos" runat="server" backgroundcssclass="modalBackground" popupcontrolid="pnlCreditos" popupdraghandlecontrolid=""
                        targetcontrolid="lnkCreditos">
                    </cc1:modalpopupextender>

                    <asp:LinkButton ID="lnkCreditos" runat="server"></asp:LinkButton>



                </fieldset>
      </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


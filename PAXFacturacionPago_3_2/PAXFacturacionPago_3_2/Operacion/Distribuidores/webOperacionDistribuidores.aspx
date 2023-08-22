<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionDistribuidores.aspx.cs" Inherits="Operacion_Distribuidores_webOperacionDistribuidores" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style3
        {
            width: 247px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDistribuidoresCat %>"></asp:Label>
    </h2>
    <asp:UpdatePanel ID="upRFC" runat="server">
    <ContentTemplate>
            <asp:Panel runat="server" ID="pnlRFC" HorizontalAlign="Left" >
            <fieldset class="register">
                <table align="left">
                    <tr>
                        <td class="style3">
                            <asp:Label ID="lblCorreo0" runat="server" AssociatedControlID="txtCorreo" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblEmailCorreo %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCorreo1" runat="server" AssociatedControlID="txtCorreo" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblNumeroDistribuidor %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtCorreo" runat="server" CssClass="textEntry" MaxLength="50" 
                                TabIndex="1" AutoPostBack="True" ontextchanged="txtCorreo_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtConsecutivo" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style3">
                            <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style3">
                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="textEntry" MaxLength="50" 
                                TabIndex="2" AutoPostBack="True" ontextchanged="txtUsuario_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbCertificado" runat="server" TabIndex="3" 
                                 />
                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCertificadoUtiliza %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style3">
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechaInicio %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFechaFinal %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            <asp:TextBox ID="txtFechaIniRec" runat="server" BackColor="White" Width="100px" 
                                TabIndex="4"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtFechaIniRec_CalendarExtender" runat="server" 
                                Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgIni0" 
                                TargetControlID="txtFechaIniRec">
                            </cc1:CalendarExtender>
                            <asp:Image ID="imgIni0" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" />
                            <asp:RegularExpressionValidator ID="revFechaIni" runat="server" 
                                ControlToValidate="txtFechaIniRec" CssClass="failureNotification" 
                                Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                ValidationGroup="RegisterUserValidationGroup">
                                    <img src="../../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvFechaIni" runat="server" 
                                ControlToValidate="txtFechaIniRec" CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" 
                                ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaFinRec" runat="server" BackColor="White" Width="100px" 
                                TabIndex="5"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtFechaFinRec_CalendarExtender" runat="server" 
                                Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgFin0" 
                                TargetControlID="txtFechaFinRec">
                            </cc1:CalendarExtender>
                            <asp:Image ID="imgFin0" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" />
                            <asp:RegularExpressionValidator ID="revFechaIni0" runat="server" 
                                ControlToValidate="txtFechaFinRec" CssClass="failureNotification" 
                                Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                ValidationGroup="RegisterUserValidationGroup">
                                    <img src="../../Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvFechaIni0" runat="server" 
                                ControlToValidate="txtFechaFinRec" CssClass="failureNotification" 
                                ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" 
                                ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </fieldset>
            </asp:Panel>
            <p style="width:430px; text-align:right;">
                <asp:Button ID="btnNuevoDist" runat="server" CssClass="botonEstilo" 
                    Height="36px" onclick="btnNuevoDist_Click" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo%>" Width="103px" 
                    TabIndex="8" />
                <asp:Button ID="btnCancelarRfc" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                    onclick="btnCancelarRfc_Click" TabIndex="7" 
                    CssClass="botonEstilo" />
                <asp:Button ID="btnGuardar" runat="server" CommandName="MoveNext" 
                ValidationGroup="RegisterUserValidationGroup" 
                Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" TabIndex="6" 
                    onclick="btnGuardar_Click" CssClass="botonEstilo" Enabled="False" />
            </p>
            <asp:Panel ID="PnlGrid" runat="server" Height="250px" ScrollBars="Auto">
                <asp:GridView ID="gdvDistribuidores" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                    CellPadding="4" DataKeyNames="id_distribuidor" GridLines="Horizontal" 
                    onrowdeleting="gdvDistribuidores_RowDeleting" 
                    onselectedindexchanged="gdvDistribuidores_SelectedIndexChanged" Width="900px">
                    <Columns>
                        <asp:CommandField 
                            SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                            ShowSelectButton="True">
                        <ItemStyle HorizontalAlign="Left" Width="50" />
                        </asp:CommandField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblNumeroDistribuidor %>">
                            <ItemTemplate>
                                <asp:Label ID="lbliddistribuidor" runat="server" 
                                    Text='<%# Bind("id_distribuidor") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblUsuarioDist %>">
                            <ItemTemplate>
                                <asp:Label ID="lblidusuario" runat="server" Text='<%# Bind("id_usuario") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>">
                            <ItemTemplate>
                                <asp:Label ID="lblcveusuario" runat="server" 
                                    Text='<%# Bind("clave_usuario") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblNumeroDistribuidor %>">
                            <ItemTemplate>
                                <asp:Label ID="lblnumerodist" runat="server" Text='<%# Bind("numero_dist") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEmailCorreo %>">
                            <ItemTemplate>
                                <asp:Label ID="lblcorreo" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblFechaInicio %>">
                            <ItemTemplate>
                                <asp:Label ID="lblFechainicio" runat="server" 
                                    Text='<%# Bind("fecha_inicio") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblFechaFinal %>">
                            <ItemTemplate>
                                <asp:Label ID="lblFechafin" runat="server" Text='<%# Bind("fecha_fin") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus %>">
                            <ItemTemplate>
                                <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblCertificadoUtiliza %>">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbCertifica" runat="server" 
                                    Checked='<%# Bind("certificado") %>' Enabled="False" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:CommandField DeleteText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>"                             
                            ShowDeleteButton="True">
                        <ItemStyle HorizontalAlign="Left" Width="50" />
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
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
            </asp:Panel>
            <br />
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


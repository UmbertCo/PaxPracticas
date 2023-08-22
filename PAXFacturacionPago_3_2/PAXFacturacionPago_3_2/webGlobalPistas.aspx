<%@ Page Title="webGlobalPistas" Language="C#" MasterPageFile="~/webGlobalMaster.master" AutoEventWireup="true" CodeFile="webGlobalPistas.aspx.cs" Inherits="webGlobalPistas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

            <h2>
                <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloPistas %>"></asp:Label>
            </h2>
            <br />
            <cc1:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="0" 
                Height="700px" Width="940px">
                <cc1:TabPanel runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, lblAplicativo %>" ID="TabPanel1">

                    <ContentTemplate>

                    <div class="accountInfo textos" style="width:900px;">
                        <fieldset class="register" style="width:890px;">
                            <legend><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" /></legend>
                            <table class="style1">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblReceptor" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>" 
                                            AssociatedControlID="ddlReceptor"></asp:Label>
                                        <asp:DropDownList ID="ddlReceptor" runat="server" 
                                            DataTextField="clave_usuario" DataValueField="id_usuario">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblLeyenda" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblLeyenda %>" style="font-weight: 700"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAccion" runat="server" AssociatedControlID="txtAccion" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFiltro1 %>"></asp:Label>
                                        <asp:TextBox ID="txtAccion" runat="server" MaxLength="999" Width="300px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="txtFolioIni_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterMode="InvalidChars" 
                                            InvalidChars="&quot;&lt;&gt;!#$%&amp;?\¬&quot;" TargetControlID="txtAccion">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAccion2" runat="server" AssociatedControlID="txtAccion2" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFiltro2 %>"></asp:Label>
                                        <asp:TextBox ID="txtAccion2" runat="server" MaxLength="999" Width="300px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="txtAccion2_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterMode="InvalidChars" 
                                            InvalidChars="&quot;&lt;&gt;!#$%&amp;?\¬&quot;" TargetControlID="txtAccion2">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAccion3" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFiltro3 %>" 
                                            AssociatedControlID="txtAccion3"></asp:Label>
                                        <asp:TextBox ID="txtAccion3" runat="server" MaxLength="999" Width="300px"></asp:TextBox>
                                        <cc1:filteredtextboxextender ID="txtAccion3_filteredtextboxextender" 
                                            runat="server" Enabled="True" 
                                            TargetControlID="txtAccion3" FilterMode="InvalidChars" 
                                            InvalidChars="&quot;&lt;&gt;!#$%&amp;?\¬&quot;">
                                        </cc1:filteredtextboxextender>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFechaIni" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                            AssociatedControlID="txtFechaIni"></asp:Label>
                                        <asp:TextBox ID="txtFechaIni" runat="server" BackColor="White" Width="100px" ></asp:TextBox>
                                        <asp:Image ID="imgIni" runat="server" 
                                            ImageUrl="~/Imagenes/icono_calendario.gif" />
                                        <asp:RegularExpressionValidator ID="revFechaIni" runat="server" Display="Dynamic"
                                            ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                            ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                            <img src="Imagenes/error_sign.gif" />
                                            </asp:RegularExpressionValidator>
                                        <cc1:calendarextender ID="txtFechaIni_CalendarExtender" runat="server" 
                                            Enabled="True" TargetControlID="txtFechaIni" Format="dd/MM/yyyy" 
                                            PopupButtonID="imgIni">
                                        </cc1:calendarextender>
                                        <asp:RequiredFieldValidator ID="rfvFechaIni" runat="server" 
                                            ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                            ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFechaFin" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                            AssociatedControlID="txtFechaFin"></asp:Label>
                                        <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White" Width="100px"></asp:TextBox>
                                        <asp:Image ID="imgFin" runat="server" 
                                            ImageUrl="~/Imagenes/icono_calendario.gif" />
                                        <asp:RegularExpressionValidator ID="revFechaFin" runat="server" Display="Dynamic"
                                            ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                            ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                            <img src="Imagenes/error_sign.gif" />
                                            </asp:RegularExpressionValidator>
                                        <cc1:calendarextender ID="txtFechaFin_CalendarExtender" runat="server" 
                                            Enabled="True" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" 
                                            PopupButtonID="imgFin">
                                        </cc1:calendarextender>
                                        <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" 
                                            ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                            ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>

                    <div style="text-align:center;" >
                    <asp:UpdateProgress ID="uppConsultas" runat="server" AssociatedUpdatePanelID="udpConsultas" >
                            <progresstemplate>
                                <img alt="" 
                            src="Imagenes/imgAjaxLoader.gif" />
                            </progresstemplate>
                    </asp:UpdateProgress>
                    </div>
                    <br />
                    <asp:UpdatePanel ID="udpConsultas" runat="server">
                        <ContentTemplate>
                        <p style="text-align:right;" >
                            <asp:Button ID="btnConsultar" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                                onclick="btnConsultar_Click" ValidationGroup="grupoConsulta" 
                                CssClass="botonEstilo" />
                        </p>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="udpControles" runat="server">
                        <ContentTemplate>  
                            <br />
                            <asp:Panel ID="Panel1" runat="server" CssClass="sinBorde" 
                                ScrollBars="Vertical" Height="300px" >
                                <asp:GridView ID="gdvComprobantes" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="4" GridLines="Horizontal" Width="100%" 
                                    DataKeyNames="id_Usuario" BackColor="White" 
                                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                                    AllowSorting="True" EnableSortingAndPagingCallbacks="True" 
                                    onsorting="gdvComprobantes_Sorting" PageSize="100">
                                    <Columns>
                                        <asp:BoundField DataField="clave_usuario" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>" 
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="clave_usuario" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="fecha_accion" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblFecha %>" 
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="fecha_accion" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="180px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="accion" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblAccion %>" 
                                            HeaderStyle-HorizontalAlign="Right" SortExpression="accion" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
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
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    </ContentTemplate>

                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, lblBaseDatos %>">

                    <ContentTemplate>



                    <div class="accountInfo textos" style="width:900px;">
                        <fieldset class="register" style="width:890px;">
                            <legend><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" /></legend>
                            <table class="style1">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblUsuario" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>" 
                                            AssociatedControlID="ddlUsuario"></asp:Label>
                                        <asp:DropDownList ID="ddlUsuario" runat="server" 
                                            DataTextField="Usuario" DataValueField="id_Usuario">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblInstancia" runat="server" 
                                            AssociatedControlID="lblInstanciaVal" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblInstancia %>"></asp:Label>
                                        <asp:Label ID="lblInstanciaVal" runat="server" style="font-weight: 700"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblLeyendaBd" runat="server" style="font-weight: 700" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblLeyenda %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblConsulta" runat="server" AssociatedControlID="txtConsulta" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFiltro1 %>"></asp:Label>
                                        <asp:TextBox ID="txtConsulta" runat="server" MaxLength="999" Width="300px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Filteredtextboxextender1" runat="server" 
                                            Enabled="True" FilterMode="InvalidChars" 
                                            InvalidChars="&quot;&lt;&gt;!#$%&amp;?\¬&quot;" TargetControlID="txtConsulta">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblConsulta2" runat="server" AssociatedControlID="txtConsulta2" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFiltro2 %>"></asp:Label>
                                        <asp:TextBox ID="txtConsulta2" runat="server" MaxLength="999" Width="300px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Filteredtextboxextender2" runat="server" 
                                            Enabled="True" FilterMode="InvalidChars" 
                                            InvalidChars="&quot;&lt;&gt;!#$%&amp;?\¬&quot;" TargetControlID="txtConsulta2">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblConsulta3" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFIltro3 %>" 
                                            AssociatedControlID="txtConsulta3"></asp:Label>
                                        <asp:TextBox ID="txtConsulta3" runat="server" MaxLength="999" Width="300px"></asp:TextBox>
                                        <cc1:filteredtextboxextender ID="Filteredtextboxextender3" 
                                            runat="server" Enabled="True" 
                                            TargetControlID="txtConsulta3" FilterMode="InvalidChars" 
                                            InvalidChars="&quot;&lt;&gt;!#$%&amp;?\¬&quot;">
                                        </cc1:filteredtextboxextender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblArchivo" runat="server" AssociatedControlID="lblArchivoVal" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblArchivo %>"></asp:Label>
                                        <asp:Label ID="lblArchivoVal" runat="server" style="font-weight: 700"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFecha" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                            AssociatedControlID="txtFechaIniBd"></asp:Label>
                                        <asp:TextBox ID="txtFechaIniBd" runat="server" BackColor="White" Width="100px" ></asp:TextBox>
                                        <asp:Image ID="Image1" runat="server" 
                                            ImageUrl="~/Imagenes/icono_calendario.gif" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
                                            ControlToValidate="txtFechaIniBd" CssClass="failureNotification" 
                                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                            ValidationGroup="grupoBD" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                            <img src="Imagenes/error_sign.gif" />
                                            </asp:RegularExpressionValidator>
                                        <cc1:calendarextender ID="txtFechaIniBd_Calendarextender" runat="server" 
                                            Enabled="True" TargetControlID="txtFechaIniBd" Format="dd/MM/yyyy" 
                                            PopupButtonID="Image1">
                                        </cc1:calendarextender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                            ControlToValidate="txtFechaIniBd" CssClass="failureNotification" 
                                            ValidationGroup="grupoBD" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFechaFinBd" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                            AssociatedControlID="txtFechaFinBd"></asp:Label>
                                        <asp:TextBox ID="txtFechaFinBd" runat="server" BackColor="White" Width="100px"></asp:TextBox>
                                        <asp:Image ID="Image2" runat="server" 
                                            ImageUrl="~/Imagenes/icono_calendario.gif" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic"
                                            ControlToValidate="txtFechaFinBd" CssClass="failureNotification" 
                                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                            ValidationGroup="grupoBD" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                            <img src="Imagenes/error_sign.gif" />
                                            </asp:RegularExpressionValidator>
                                        <cc1:calendarextender ID="txtFechaFinBd_Calendarextender" runat="server" 
                                            Enabled="True" TargetControlID="txtFechaFinBd" Format="dd/MM/yyyy" 
                                            PopupButtonID="Image2">
                                        </cc1:calendarextender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                            ControlToValidate="txtFechaFinBd" CssClass="failureNotification" 
                                            ValidationGroup="grupoBD" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div style="text-align:center;" >
                    <asp:UpdateProgress ID="updBD" runat="server" AssociatedUpdatePanelID="updConsultasBD" >
                            <progresstemplate>
                                <img alt="" 
                            src="Imagenes/imgAjaxLoader.gif" />
                            </progresstemplate>
                    </asp:UpdateProgress>
                    </div>
                    <br />
                    <asp:UpdatePanel ID="updConsultasBD" runat="server">
                        <ContentTemplate>
                        <p style="text-align:right;" >
                            <asp:Button ID="btnConsultaBD" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                                ValidationGroup="grupoBD" 
                                CssClass="botonEstilo" onclick="btnConsultaBD_Click" />
                        </p>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="updGrid" runat="server">
                        <ContentTemplate>  
                            <br />
                            <asp:Panel ID="Panel2" runat="server" CssClass="sinBorde" 
                                ScrollBars="Vertical" Height="300px" >
                                <asp:GridView ID="grvDatosBd" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="4" GridLines="Horizontal" Width="100%" BackColor="White" 
                                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                                    AllowSorting="True" EnableSortingAndPagingCallbacks="True" 
                                    onsorting="grvDatosBd_Sorting" PageSize="100">
                                    <Columns>
                                        <asp:BoundField DataField="usuario" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>" 
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="usuario" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="objeto" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblObjeto %>" 
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="objeto" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="fecha_evento" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblFecha %>" 
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="fecha_evento" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="180px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="consulta" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblAccion %>" 
                                            HeaderStyle-HorizontalAlign="Right" SortExpression="consulta" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
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
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    </ContentTemplate>

                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, lblSO %>">

                 <ContentTemplate>



                    <div class="accountInfo textos" style="width:900px;">
                        <fieldset class="register" style="width:890px;">
                            <legend><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" /></legend>
                            <table class="style1">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblUsuarioSO" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>" 
                                            AssociatedControlID="ddlUsuarioSO"></asp:Label>
                                        <asp:DropDownList ID="ddlUsuarioSO" runat="server" 
                                            DataTextField="username" DataValueField="id_Usuario">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTipo" runat="server" AssociatedControlID="ddlTipo" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblTipo %>"></asp:Label>
                                        <asp:DropDownList ID="ddlTipo" runat="server" DataTextField="entrytype" 
                                            DataValueField="id_entrytype">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSource" runat="server" AssociatedControlID="ddlTipo" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblOrigen %>"></asp:Label>
                                        <asp:DropDownList ID="ddlSource" runat="server" DataTextField="source" 
                                            DataValueField="id_source">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblInstanciaSO" runat="server" AssociatedControlID="ddlInstancia" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblInstancia %>"></asp:Label>
                                        <asp:DropDownList ID="ddlInstancia" runat="server" DataTextField="source" 
                                            DataValueField="id_source">
                                            <asp:ListItem>APOLO</asp:ListItem>
                                            <asp:ListItem>DISCOVERY</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblLeyendaSO" runat="server" style="font-weight: 700" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblLeyenda %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAccionSO" runat="server" AssociatedControlID="txtConsultaSO" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFiltro1 %>"></asp:Label>
                                        <asp:TextBox ID="txtConsultaSO" runat="server" MaxLength="999" Width="300px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Filteredtextboxextender4" runat="server" 
                                            Enabled="True" FilterMode="InvalidChars" 
                                            InvalidChars="&quot;&lt;&gt;!#$%&amp;?\¬&quot;" TargetControlID="txtConsultaSO">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAccion2SO" runat="server" 
                                            AssociatedControlID="txtConsulta2SO" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFiltro2 %>"></asp:Label>
                                        <asp:TextBox ID="txtConsulta2SO" runat="server" MaxLength="999" Width="300px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Filteredtextboxextender5" runat="server" 
                                            Enabled="True" FilterMode="InvalidChars" 
                                            InvalidChars="&quot;&lt;&gt;!#$%&amp;?\¬&quot;" 
                                            TargetControlID="txtConsulta2SO">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAccion3SO" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFiltro3 %>" 
                                            AssociatedControlID="txtConsulta3SO"></asp:Label>
                                        <asp:TextBox ID="txtConsulta3SO" runat="server" MaxLength="999" Width="300px"></asp:TextBox>
                                        <cc1:filteredtextboxextender ID="Filteredtextboxextender6" 
                                            runat="server" Enabled="True" 
                                            TargetControlID="txtConsulta3SO" FilterMode="InvalidChars" 
                                            InvalidChars="&quot;&lt;&gt;!#$%&amp;?\¬&quot;">
                                        </cc1:filteredtextboxextender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" AssociatedControlID="lblArchivoValSO" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblArchivo %>"></asp:Label>
                                        <asp:Label ID="lblArchivoValSO" runat="server" style="font-weight: 700"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                            AssociatedControlID="txtFechaIniSO"></asp:Label>
                                        <asp:TextBox ID="txtFechaIniSO" runat="server" BackColor="White" Width="100px" ></asp:TextBox>
                                        <asp:Image ID="imgIniSO" runat="server" 
                                            ImageUrl="~/Imagenes/icono_calendario.gif" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="Dynamic"
                                            ControlToValidate="txtFechaIniSO" CssClass="failureNotification" 
                                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                            ValidationGroup="grupoSO" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                            <img src="Imagenes/error_sign.gif" />
                                            </asp:RegularExpressionValidator>
                                        <cc1:calendarextender ID="txtFechaIniSO_Calendarextender" runat="server" 
                                            Enabled="True" TargetControlID="txtFechaIniSO" Format="dd/MM/yyyy" 
                                            PopupButtonID="imgIniSO">
                                        </cc1:calendarextender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                            ControlToValidate="txtFechaIniSO" CssClass="failureNotification" 
                                            ValidationGroup="grupoSO" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                            AssociatedControlID="txtFechaFinSO"></asp:Label>
                                        <asp:TextBox ID="txtFechaFinSO" runat="server" BackColor="White" Width="100px"></asp:TextBox>
                                        <asp:Image ID="imgFinSO" runat="server" 
                                            ImageUrl="~/Imagenes/icono_calendario.gif" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="Dynamic"
                                            ControlToValidate="txtFechaFinSO" CssClass="failureNotification" 
                                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                            ValidationGroup="grupoSO" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                            <img src="Imagenes/error_sign.gif" />
                                            </asp:RegularExpressionValidator>
                                        <cc1:calendarextender ID="txtFechaFinSO_Calendarextender" runat="server" 
                                            Enabled="True" TargetControlID="txtFechaFinSO" Format="dd/MM/yyyy" 
                                            PopupButtonID="imgFinSO">
                                        </cc1:calendarextender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                            ControlToValidate="txtFechaFinSO" CssClass="failureNotification" 
                                            ValidationGroup="grupoSO" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div style="text-align:center;" >
                    <asp:UpdateProgress ID="updProgress" runat="server" AssociatedUpdatePanelID="udpConsultaSO" >
                            <progresstemplate>
                                <img alt="" 
                            src="Imagenes/imgAjaxLoader.gif" />
                            </progresstemplate>
                    </asp:UpdateProgress>
                    </div>
                    <br />
                    <asp:UpdatePanel ID="udpConsultaSO" runat="server">
                        <ContentTemplate>
                        <p style="text-align:right;" >
                            <asp:Button ID="btnConsultaSO" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                                ValidationGroup="grupoSO" 
                                CssClass="botonEstilo" onclick="btnConsultaSO_Click" />
                        </p>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="updGridSO" runat="server">
                        <ContentTemplate>  
                            <br />
                            <asp:Panel ID="Panel3" runat="server" CssClass="sinBorde" 
                                ScrollBars="Vertical" Height="300px" >
                                <asp:GridView ID="grvDatosSO" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="4" GridLines="Horizontal" Width="100%" BackColor="White" 
                                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                                    AllowSorting="True" EnableSortingAndPagingCallbacks="True" 
                                    onsorting="grvDatosSO_Sorting" PageSize="100">
                                    <Columns>
                                        <asp:BoundField DataField="username" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>" 
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="username" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="source" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblObjeto %>" 
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="source" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="timegenerated" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblFecha %>" 
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="timegenerated" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="180px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="message" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblAccion %>" 
                                            HeaderStyle-HorizontalAlign="Right" SortExpression="message" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
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
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    </ContentTemplate>

                </cc1:TabPanel>
            </cc1:TabContainer>



</asp:Content>


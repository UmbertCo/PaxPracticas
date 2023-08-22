<%@ Page Title="" Language="C#" MasterPageFile="~/webGlobalMaster.master" AutoEventWireup="true" CodeFile="webGlobalSOAP.aspx.cs" Inherits="webGlobalSOAP" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varConSOAP %>"></asp:Label>
    </h2>
    <div class="accountInfo textos" style="width:900px;" __designer:mapid="c4">
        <fieldset class="register" style="width:890px;" __designer:mapid="c5">
            <legend __designer:mapid="c6">
                <asp:Literal runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblSubConsulta %>" ID="Literal4"></asp:Literal>
            </legend>
            <table class="style1" __designer:mapid="c8">
                <tr __designer:mapid="c9">
                    <td __designer:mapid="ca">
                        <asp:Label runat="server" AssociatedControlID="ddlUsuarioSO" 
                            Text="<%$ Resources:resCorpusCFDIEs, varUUID %>" ID="lblUsuarioSO"></asp:Label>
                        <asp:DropDownList runat="server" DataTextField="uuid" 
                            DataValueField="id_UUID" ID="ddlUsuarioSO">
                        </asp:DropDownList>
                    </td>
                    <td __designer:mapid="cd">
                        <asp:Label runat="server" AssociatedControlID="ddlTipo" 
                            Text="<%$ Resources:resCorpusCFDIEs, varSOAP %>" ID="lblTipo"></asp:Label>
                        <asp:DropDownList runat="server" DataTextField="entrytype" 
                            DataValueField="id_entrytype" ID="ddlTipo">
                            <asp:ListItem>Request</asp:ListItem>
                            <asp:ListItem>Response</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td __designer:mapid="cd">
                        <asp:Label runat="server" AssociatedControlID="ddlTipo" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblEfectoDocumento %>" ID="lblTipo0"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlOrigenAcuses" AutoPostBack="True" 
                            onselectedindexchanged="ddlOrigenAcuses_SelectedIndexChanged">
                            <asp:ListItem>SAT</asp:ListItem>
                            <asp:ListItem>PAC</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr __designer:mapid="c9">
                    <td __designer:mapid="ca">
                        <asp:Label runat="server" AssociatedControlID="ddlUsuarioSO" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblOrigen %>" ID="lblOrigen"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlOrigen">
                            <asp:ListItem>Envio</asp:ListItem>
                            <asp:ListItem>Cancelacion</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td __designer:mapid="cd">
                        &nbsp;</td>
                    <td __designer:mapid="cd">
                        <asp:Label runat="server" AssociatedControlID="ddlTipo" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblCodigo %>" ID="lblTipo1"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlCodigo" 
                            onselectedindexchanged="ddlOrigenAcuses_SelectedIndexChanged">
                            <asp:ListItem>Todos</asp:ListItem>
                            <asp:ListItem>201</asp:ListItem>
                            <asp:ListItem>202</asp:ListItem>
                            <asp:ListItem>203</asp:ListItem>
                            <asp:ListItem>205</asp:ListItem>
                            <asp:ListItem>301</asp:ListItem>
                            <asp:ListItem>302</asp:ListItem>
                            <asp:ListItem>303</asp:ListItem>
                            <asp:ListItem>304</asp:ListItem>
                            <asp:ListItem>305</asp:ListItem>
                            <asp:ListItem>306</asp:ListItem>
                            <asp:ListItem>307</asp:ListItem>
                            <asp:ListItem>308</asp:ListItem>
                            <asp:ListItem>401</asp:ListItem>
                            <asp:ListItem>402</asp:ListItem>
                            <asp:ListItem>403</asp:ListItem>
                            <asp:ListItem>0</asp:ListItem>
                            <asp:ListItem>96</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr __designer:mapid="ed">
                    <td __designer:mapid="ee">
                        <asp:Label runat="server" AssociatedControlID="txtFechaIniSO" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" ID="Label9"></asp:Label>
                        <asp:TextBox runat="server" BackColor="White" Width="100px" ID="txtFechaIniSO"></asp:TextBox>
                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" 
                            PopupButtonID="imgIniSO" Enabled="True" TargetControlID="txtFechaIniSO" 
                            ID="txtFechaIniSO_Calendarextender">
                        </cc1:CalendarExtender>
                        <asp:Image runat="server" ImageUrl="~/Imagenes/icono_calendario.gif" 
                            ID="imgIniSO"></asp:Image>
                        <asp:RegularExpressionValidator runat="server" 
                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                            ControlToValidate="txtFechaIniSO" Display="Dynamic" ValidationGroup="grupoSO" 
                            CssClass="failureNotification" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                            ID="RegularExpressionValidator3">
                                            <img src="Imagenes/error_sign.gif" />
                                            </asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFechaIniSO" 
                            ValidationGroup="grupoSO" CssClass="failureNotification" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" 
                            ID="RequiredFieldValidator3"></asp:RequiredFieldValidator>
                    </td>
                    <td __designer:mapid="f5">
                        <asp:Label runat="server" AssociatedControlID="txtFechaFinSO" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" ID="Label10"></asp:Label>
                        <asp:TextBox runat="server" BackColor="White" Width="100px" ID="txtFechaFinSO"></asp:TextBox>
                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" 
                            PopupButtonID="imgFinSO" Enabled="True" TargetControlID="txtFechaFinSO" 
                            ID="txtFechaFinSO_Calendarextender">
                        </cc1:CalendarExtender>
                        <asp:Image runat="server" ImageUrl="~/Imagenes/icono_calendario.gif" 
                            ID="imgFinSO"></asp:Image>
                        <asp:RegularExpressionValidator runat="server" 
                            ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                            ControlToValidate="txtFechaFinSO" Display="Dynamic" ValidationGroup="grupoSO" 
                            CssClass="failureNotification" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" 
                            ID="RegularExpressionValidator4">
                                            <img src="Imagenes/error_sign.gif" />
                                            </asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFechaFinSO" 
                            ValidationGroup="grupoSO" CssClass="failureNotification" 
                            ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" 
                            ID="RequiredFieldValidator4"></asp:RequiredFieldValidator>
                    </td>
                    <td __designer:mapid="f5">
                        &nbsp;</td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div align="center">
                        <asp:UpdateProgress runat="server" 
            AssociatedUpdatePanelID="udpConsultaSO" ID="updProgress"><ProgressTemplate>
                                    <img alt="" 
                                src="Imagenes/imgAjaxLoader.gif" />
                            
    </ProgressTemplate>
    </asp:UpdateProgress>

    </div>

                    <asp:UpdatePanel runat="server" ID="udpConsultaSO"><ContentTemplate>
                        <p style="text-align:right;" >
                            <asp:Button ID="btnConsulta" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                                ValidationGroup="grupoSO" 
                                CssClass="botonEstilo" onclick="btnConsultaSO_Click" />
                            <asp:Button ID="btnExportar" runat="server" CssClass="botonGrande" 
                                onclick="btnExportar_Click" Text="<%$ Resources:resCorpusCFDIEs, lblExcel %>" 
                                ValidationGroup="grupoConsulta" Visible="False" />
                        </p>
                        
</ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExportar" />
                        </Triggers>
</asp:UpdatePanel>

                    <asp:UpdatePanel runat="server" ID="updGridSO"><ContentTemplate>
  
                            <br />
                            <asp:Panel ID="Panel3" runat="server" CssClass="sinBorde" 
                                ScrollBars="Vertical" Height="300px" >
                                <asp:GridView ID="grvDatosSO" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="4" GridLines="Horizontal" Width="100%" BackColor="White" 
                                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                                    AllowSorting="True" EnableSortingAndPagingCallbacks="True" 
                                    onsorting="grvDatosSO_Sorting" PageSize="100">
                                    <Columns>
                                        <asp:BoundField DataField="uuid" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, varUUID %>" 
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="uuid" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="acuse" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblObjeto %>" 
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="acuse" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="id_codigo" 
                                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblAccion %>" 
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="id_codigo" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbSeleccion" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Literal ID="Literal2" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
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


                    </asp:Content>


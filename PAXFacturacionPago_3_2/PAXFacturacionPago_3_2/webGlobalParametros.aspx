<%@ Page Title="" Language="C#" MasterPageFile="~/webGlobalMaster.master" AutoEventWireup="true" CodeFile="webGlobalParametros.aspx.cs" Inherits="webGlobalParametros" ValidateRequest="false"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <%------------------------JQuery----------------------------%>
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <link href="Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <%------------------------JQuery----------------------------%>
    <style type="text/css">
        .textEntry
        {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varCatParametros %>"></asp:Label>
    </h2>
    <div style="text-align:center;" >
            <asp:UpdateProgress ID="uppConsultas" runat="server"  >
                    <progresstemplate>
                        <img alt="" 
                    src="Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>
            </div>
    <asp:UpdatePanel ID="upFormulario" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlFormulario" runat="server" Height="463px">
            <div class="accountInfo">
            <fieldset class="register" style=" height:348px; width:890px;">
            <legend><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDatosPar %>" /></legend>
                <asp:HiddenField ID="hdIdEstructura" runat="server" Visible="False" />
            <table><tr><td style="vertical-align:top; width:400px;">
                    <p>
                        <asp:Label ID="lblParametro" runat="server" AssociatedControlID="txtParametro" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblParametro %>" ></asp:Label>
                        <asp:TextBox ID="txtParametro" runat="server" CssClass="textEntry" 
                            MaxLength="255" TabIndex="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSucursal" runat="server" 
                            ControlToValidate="txtParametro" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valParametro %>" 
                            ValidationGroup="RegisterUserValidationGroup"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblValor" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblValor %>" 
                            AssociatedControlID="txtValor" ></asp:Label>
                        <asp:TextBox ID="txtValor" runat="server" CssClass="textEntry" 
                            MaxLength="999999999" TabIndex="6" Height="115px" TextMode="MultiLine" 
                            Width="598px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtValor" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                            ValidationGroup="RegisterUserValidationGroup"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblEstatus" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblEstatus %>" 
                            AssociatedControlID="ddlEstatus" ></asp:Label>
                        <asp:DropDownList ID="ddlEstatus" runat="server" Width="100px" 
                            DataTextField="estatus" DataValueField="id_estatus" TabIndex="4">
                            <asp:ListItem Value="A">Activo</asp:ListItem>
                            <asp:ListItem Value="B">Baja</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvPais" runat="server" 
                            ControlToValidate="ddlEstatus" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, lblEstatus %>" 
                            ValidationGroup="RegisterUserValidationGroup"></asp:RequiredFieldValidator>
                    </p>
            </td><td style="vertical-align:top; width:400px;">
                </td></tr></table>
            </fieldset>
            </div>
            <p style="text-align:right;" >
                <asp:Button ID="btnGuardarActualizar" runat="server" CommandName="MoveNext" 
                ValidationGroup="RegisterUserValidationGroup" 
                Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                    TabIndex="14" onclick="btnGuardarActualizar_Click" 
                    CssClass="botonEstilo" />
                <asp:Button ID="btnCancelar" runat="server" 
                Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" 
                    onclick="btnCancelar_Click" TabIndex="15" Visible="False" 
                    CssClass="botonEstilo" />
            </p>
            </asp:Panel>
            </ContentTemplate>
    </asp:UpdatePanel>
    <div style="padding-left:2px;">

    <br />
    <asp:UpdatePanel ID="updGuardar" runat="server">
        <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal" Width="920px" >
                <asp:GridView ID="gdvDatos" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" GridLines="Horizontal" Width="920px" 
                    DataKeyNames="id_Parametro" 
                    onrowdeleting="gdvDatos_RowDeleting" AllowPaging="True" 
                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                    onpageindexchanging="gdvDatos_PageIndexChanging" BackColor="White" 
                    onselectedindexchanged="gdvDatos_SelectedIndexChanged">
                    <Columns>
                        <asp:CommandField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                            SelectText="<%$ Resources:resCorpusCFDIEs, lblEditar %>"
                            ShowSelectButton="True" HeaderStyle-HorizontalAlign="Left" >
                        <HeaderStyle HorizontalAlign="Left" />
                        </asp:CommandField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblParametro %>" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblParametro" runat="server" Text='<%# Bind("Parametro") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblValor %>" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblValor" runat="server" Text='<%# Bind("Valor") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:resCorpusCFDIEs, lblEstatus %>" HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="lblEstatus" runat="server" Text='<%# Bind("estatus") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:literal ID="Literal2" runat="server" text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                    </EmptyDataTemplate>
                    <FooterStyle BackColor="White" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5A737E" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="White" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#92BA41" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#5A737E" />
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<br />
    
<br />
</asp:Content>


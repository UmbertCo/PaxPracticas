<%@ Page Title="" Theme="Alitas" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webOperacionSeriesFolios.aspx.cs" Inherits="Cuenta_webOperacionSeriesFolios" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<link href="../App_Themes/Alitas/tema_dinamico.css" rel="Stylesheet" type="text/css" />

<script language="javascript" type="text/javascript">

    function fntraerEnter(keyStroke) {
        isNetscape = (document.layers);
        eventChooser = (isNetscape) ? keyStroke.which : event.keyCode;
        if (eventChooser == 13) {
            return false;
        }
    }
    document.onkeypress = fntraerEnter; 
   
 </script> 

     <script runat="server">   
        public void Page_PreInit()
        {
            if (Session["theme"] == null)
            {
                this.Theme = "Alitas";
            }
            else
            {
                this.Theme = Convert.ToString(Session["theme"]);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <div style="text-align:center;" >
            <asp:UpdateProgress ID="uppConsultas" runat="server" >
                    <progresstemplate>
                        <img alt="" 
                    src="../Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>
            </div>
            <center>
                <table style="width:952px; text-align:left; background-color:Black">
                    <tr>
                        <td class="Titulo">
                        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloSeries %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                            <td style="width:952px; height:0.5px; background-color:#fff000"></td>
                        </tr>
                    <tr>
                        <td class="Subtitulos">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubSeries %>" />
                        </td>
                    </tr>
                </table>
            </center>
            <center>
    <table class="background_tablas_transparente" style="width:952px">
        <tr>
        <td style="vertical-align:top;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="" style="width: 486px">
                <fieldset class="register" style=" height:250px; width:400px; border-color:transparent;">
                    <p>
                        <asp:Label ID="lblNombreSucursal" runat="server" SkinID="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>"
                            AssociatedControlID="ddlSucursales"></asp:Label>
                        <asp:DropDownList ID="ddlSucursales" runat="server" Width="300px" AutoPostBack="True" 
                            DataTextField="nombre" DataValueField="id_estructura" TabIndex="1" 
                            onselectedindexchanged="ddlSucursales_SelectedIndexChanged"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvSucursal" runat="server" 
                            ControlToValidate="ddlSucursales" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                            ValidationGroup="grupoSeriesFolios" Width="16px"> <img src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                    </p><p>
                        <asp:Label ID="lblTipoDocumento" runat="server" SkinID="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblTipoDocumento %>"
                            AssociatedControlID="ddlTipoDocumento"></asp:Label>
                            <asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="300px" 
                            DataTextField="nombre" DataValueField="id_tipo_documento" TabIndex="2" 
                            AutoPostBack="True" 
                            onselectedindexchanged="ddlTipoDocumento_SelectedIndexChanged" ></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTipoDoc" runat="server" 
                            ControlToValidate="ddlTipoDocumento" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valTipoDocumento %>" 
                            ValidationGroup="grupoSeriesFolios" Width="16px"> <img src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblSerie" runat="server" SkinID="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblSerie %>"
                            AssociatedControlID="txtSerie"></asp:Label>
                        <asp:TextBox ID="txtSerie" runat="server" Width="300px" CssClass="textEntry" 
                            MaxLength="25" TabIndex="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSerie" runat="server" 
                            ControlToValidate="txtSerie" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSerie %>" 
                            ValidationGroup="grupoSeriesFolios" Width="16px"> <img src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                    </p><p>
                        <asp:Label ID="lblFolio" runat="server" SkinID="labelLarge" Text="<%$ Resources:resCorpusCFDIEs, lblFolio %>"
                            AssociatedControlID="txtFolio"></asp:Label>
                        <asp:TextBox ID="txtFolio" runat="server" Width="300px" CssClass="textEntry" 
                            MaxLength="5" TabIndex="4"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="txtFolio_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFolio">
                        </cc1:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="rfvFolio" runat="server" 
                            ControlToValidate="txtFolio" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valFolio %>" 
                            ValidationGroup="grupoSeriesFolios" Width="16px"> <img src="../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                    </p>
                </fieldset>
            </div>
            <table align="right">
                <tr>
                    <td>
                    <asp:Button ID="btnGuardar" runat="server" CommandName="MoveNext" 
                ValidationGroup="grupoSeriesFolios" 
                Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                    TabIndex="5" CssClass="botonEstilo" onclick="btnGuardar_Click" />    
                    </td>
                </tr>
            </table>
            <%--Modal--%>
<asp:LinkButton ID="lkbAviso" runat="server"></asp:LinkButton>
<cc1:ModalPopupExtender ID="mpeAvisos" runat="server"
TargetControlID ="lkbAviso" 
PopupControlID ="pnlAvisos" 
BackgroundCssClass ="modalBackground">
</cc1:ModalPopupExtender>
<br />
<asp:Panel ID="pnlAvisos" runat="server" CssClass ="modal" >
    <table cellpadding="0" cellspacing="0" >
        <tr>
            <td class="TablaBackGround">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <br />
                <table class="TablaBackGround">
                    <tr>
                    <td>
                        <img alt="" class="imgInformacion" 
                            src="../Imagenes/info_ico.png" />    
                    </td>
                    <td align="center">
                        <asp:Label ID="lblAviso" runat="server" SkinID="labelMedium"></asp:Label>
                    </td>
                    <td>    
                
                    </td>
                    </tr>
                    <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                    
                    </td>
                    <td>
                        <asp:Button ID="btnAviso" runat="server" CssClass="botonEstilo" Text ="OK"/>
                    </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
    </td>
    <td style="vertical-align:top; padding-top:40px;">
        <asp:UpdatePanel ID="updGuardar" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="360px">
                    <asp:GridView ID="gdvSeries" runat="server" AutoGenerateColumns="False" 
                        BackColor="White" BorderColor="#0073aa" BorderStyle="Double" BorderWidth="3px" 
                        GridLines ="Horizontal" Width="335px"
                        DataKeyNames="id_serie" AllowPaging="True" 
                        onpageindexchanging="gdvSeries_PageIndexChanging" 
                        onrowdeleting="gdvSeries_RowDeleting" SkinID="SkinGridView" >
                        <Columns>
                            <asp:BoundField DataField="serie"  HeaderStyle-HorizontalAlign="Left"
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblSerie %>" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="folio" HeaderStyle-HorizontalAlign="left"
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblFolio %>" >
                            <HeaderStyle HorizontalAlign="left" />
                            <ItemStyle HorizontalAlign="left" Width="150px" />
                            </asp:BoundField>
                            <asp:CommandField ShowDeleteButton="True" DeleteText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>"
                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" HeaderStyle-HorizontalAlign="Left" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                            </asp:CommandField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                        </EmptyDataTemplate>
                          <%--<FooterStyle BackColor="White" ForeColor="#333333" />
                            <HeaderStyle BackColor="#0073aa" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#0073aa" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />--%>
                            </asp:GridView>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        </td>
        </tr>
    </table>
            
            </center>
<br />
<br />

</asp:Content>


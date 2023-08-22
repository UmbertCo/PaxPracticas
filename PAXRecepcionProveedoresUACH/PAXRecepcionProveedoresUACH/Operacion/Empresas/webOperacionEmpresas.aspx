<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="webOperacionEmpresas.aspx.cs" Inherits="Operacion_Empresas_webOperacionEmpresas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <script src="../../Scripts/progressbar.js" type="text/javascript"></script>
    <script type="text/javascript">
        var validationText = '<%=Resources.resCorpusCFDIEs.lblEliminarConfirmarEmpresa%>';
    </script>
    <link href="../../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style4
        {
            width: 635px;
        }
        .style5
        {
            width: 569px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <br />
    <br />
    <table>
        <tr>
            <td>
                <h2>
                    <asp:Label ID="lblTituloEmpresas" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloEmpresas %>"
                        Font-Size="X-Large"></asp:Label>
                </h2>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upAltaEmpresa" runat="server">
        <ContentTemplate>
            <table style="margin: 0px auto; width: 983px; border: solid 1px #C0C0C0;" class="bodyMain">
                <tr>
                    <td align="left">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="fupLogo" Text="<%$ Resources:resCorpusCFDIEs, lblListaEmpresas %>"
                            Font-Bold="False" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:DropDownList ID="ddlEmpresas" runat="server" Width="326px" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button ID="btnNuevo" runat="server" CssClass="botonEstilo" Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>"
                            OnClick="btnNuevo_Click" Width="80px" TabIndex="4" Height="27px" />
                        <asp:Button ID="btnBorrar" runat="server" CssClass="botonEstilo" OnClick="btnBorrar_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" Width="80px" TabIndex="5"
                            
                            OnClientClick="return confirm(validationText)"
                            Height="27px" />
                        <asp:Button ID="btnEditar" runat="server" CssClass="botonEstilo" OnClick="btnEditar_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" Width="80px" TabIndex="6"
                            Height="27px" />
                        <asp:Button ID="btnGuardar" runat="server" CssClass="botonEstilo" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                            OnClick="btnGuardar_Click" Height="27px" Width="80px" 
                            ValidationGroup="RegisterEnterpriseValidationGroup" />
                        <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" OnClick="btnNCancelar_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" TabIndex="7"
                            Height="27px" />
                    </td>
                </tr>
            </table>
            <table style="margin: 0px auto; width: 983px; border: solid 1px #C0C0C0;" class="bodyMain">
                <tr>
                    <td align="left" class="style5">
                        <asp:Label ID="lblEmpresa" AssociatedControlID="txtNombreEmpresa" runat="server"
                            Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>" Font-Bold="False"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblRFC" runat="server" AssociatedControlID="txtRFC" Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>"
                            Font-Bold="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style5">
                        <asp:TextBox ID="txtNombreEmpresa" runat="server" Width="413px" Enabled="false" 
                            ValidationGroup="RegisterEnterpriseValidationGroup"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNombreEmpresa" runat="server" ControlToValidate="txtNombreEmpresa"
                            CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>"
                            ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtRfc" runat="server" Enabled="false" 
                            ValidationGroup="RegisterEnterpriseValidationGroup"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valRFC" runat="server" ControlToValidate="txtRfc"
                            CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtRFC %>"
                            ToolTip="<%$ Resources:resCorpusCFDIEs, txtRFC %>" ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regxRFC" runat="server" ControlToValidate="txtRfc"
                            CssClass="failureNotification" Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxRFC %>"
                            ToolTip="<%$ Resources:resCorpusCFDIEs, regxRFC %>" ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"
                            ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style5">
                        <asp:Label ID="lblLogo" runat="server" AssociatedControlID="fupLogo" Text="<%$ Resources:resCorpusCFDIEs, lblLogoImg %>"
                            Font-Bold="False" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style5">
                        <asp:FileUpload ID="fupLogo" runat="server" Width="358px" Enabled="false" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBorrar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnEditar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnNCancelar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnGuardar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

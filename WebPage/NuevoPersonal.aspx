<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true"  validateRequest="false" CodeBehind="NuevoPersonal.aspx.cs" Inherits="WebPage.WebForm2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="formas">
    <div>
<h1 align="center"><asp:Literal ID="ltlTitulo" runat="server" Text="<%$Resources:Resource_es,lblAgregarPersonal%>"></asp:Literal></h1>
</div>
<div>

    
        <p>
            <asp:Literal ID="ltlNombre" runat="server" Text="<%$Resources:Resource_es,lblNombre%>"></asp:Literal>
                <asp:TextBox ID="txtNombre" CssClass="txt" runat="server" Width="550px" MaxLength="49" 
                    TabIndex="1"></asp:TextBox>
           
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtNombre" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="ValidarNuevo"></asp:RequiredFieldValidator>
                       </p>
            <p>
            <asp:Literal ID="ltlApPaterno" runat="server" Text="<%$Resources:Resource_es,lblApPaterno%>"></asp:Literal> 
            
                <asp:TextBox ID="txtApPaterno" CssClass="txt" runat="server" Width="481px" MaxLength="49" 
                    TabIndex="2"></asp:TextBox>
            
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtApPaterno" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="ValidarNuevo"></asp:RequiredFieldValidator>
            </p>
            <p>
            <asp:Literal ID="ltlApMaterno" runat="server" Text="<%$Resources:Resource_es,lblApMaterno%>"></asp:Literal>
            
                <asp:TextBox CssClass="txt" ID="txtApMaterno" runat="server" Width="480px" MaxLength="49" 
                    TabIndex="3"></asp:TextBox>
            
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="txtApMaterno" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="ValidarNuevo"></asp:RequiredFieldValidator>
            </p>
            <p>
            <asp:Literal ID="ltlDireccion" runat="server" Text="<%$Resources:Resource_es,lblDireccion%>"></asp:Literal>
            
                <asp:TextBox CssClass="txt" ID="txtDireccion" runat="server" Width="542px" MaxLength="49" 
                    TabIndex="4"></asp:TextBox>
            
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="txtDireccion" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="ValidarNuevo"></asp:RequiredFieldValidator>
            </p>
            <p>
            <asp:Literal ID="ltlTelefono" runat="server" Text="<%$Resources:Resource_es,lblTelefono%>"></asp:Literal>
           
                <asp:TextBox CssClass="txt" ID="txtTelefono" runat="server" Width="405px" MaxLength="49" 
                    TabIndex="5"></asp:TextBox>
            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="txtTelefono" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="ValidarNuevo"></asp:RequiredFieldValidator><small>
            <asp:Literal ID="ltlFormato" runat="server" Text="<%$Resources:Resource_es,lblFormatoTelefono%>"></asp:Literal></small>
                <asp:RegularExpressionValidator ID="revTelefono" runat="server" 
                    ErrorMessage="Telefono Invalido" ControlToValidate="txtTelefono" ValidationGroup="ValidarNuevo"
                    ForeColor="Red" ValidationExpression="\d{3}\-\d{7}"></asp:RegularExpressionValidator>
            </p>
            <p>
            <asp:Literal ID="ltlCorreo" runat="server" Text="<%$Resources:Resource_es,lblCorreo%>"></asp:Literal>
            
                <asp:TextBox CssClass="txt" ID="txtCorreo" runat="server" Width="415px" MaxLength="50" 
                    TabIndex="6"></asp:TextBox>
           
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ControlToValidate="txtCorreo" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="ValidarNuevo"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revCorreo" runat="server" 
                    ErrorMessage="Direccion no Valida" ControlToValidate="txtCorreo" ValidationGroup="ValidarNuevo" 
                    ForeColor="Red" 
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </p>
            <p>
            <asp:Literal ID="ltlPuesto" runat="server" Text="<%$Resources:Resource_es,lblPuesto%>"></asp:Literal>
            
                <asp:DropDownList ID="ddlPuesto" runat="server" TabIndex="7">
                </asp:DropDownList>
            </p>
            
                <asp:LinkButton ID="lkbNuevo" runat="server" TabIndex="9"></asp:LinkButton>
                <asp:LinkButton ID="lklConfirmar" runat="server" TabIndex="10"></asp:LinkButton>
            

            <p style="text-align:right; background:none; border:none;">
                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn"
                    onclick="btnNuevo_Click1" ValidationGroup="ValidarNuevo" TabIndex="8" />
                    </p>
            
    
</div>
<cc1:ModalPopupExtender ID="mdlNuevoPersonal" runat="server"
     TargetControlID = "lkbNuevo"
     PopupControlID = "pnlenviar" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlenviar" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblagregar" runat="server" Text="<%$Resources:Resource_es,lblagregar%>" ></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnOK" runat="server" Text="Aceptar" CssClass="btn"
                        onclick="btnOK_Click" TabIndex="10" />
                 <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn"
                        onclick="btnCancelar_Click" TabIndex="11" />
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:LinkButton ID="lkbAvisoUsuario" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mdlAvisoUsuario" runat="server"
     TargetControlID = "lkbAvisoUsuario"
     PopupControlID = "pnlAvisaUsuario" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlAvisaUsuario" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblExiste" runat="server" Text="<%$Resources:Resource_es,lblExiste%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnAvisoOk" CssClass="btn" runat="server" Text="Si" 
                    onclick="btnAvisoOk_Click" TabIndex="12" />
             </td>
        </tr>
    </table>
    </asp:Panel>
    </div>
</asp:Content>

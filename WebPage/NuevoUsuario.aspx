<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true" validateRequest="false" CodeBehind="NuevoUsuario.aspx.cs" Inherits="WebPage.WebForm1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style4
        {
            width: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="formas">
    
<asp:LinkButton ID="lkbRegistroDeUsuario" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlRegistroDeUsuario" runat="server"
     TargetControlID = "lkbRegistroDeUsuario"
     PopupControlID = "pnlRegistroDeUsuario" DropShadow ="false"
     BackgroundCssClass="modalBackground">        
</cc1:modalpopupextender>
    <asp:Panel ID="pnlRegistroDeUsuario" runat="server" 
    CssClass="modalPopup" Width="816px">
    <div>
        <center><h1><asp:Literal ID="ltlTitulo" runat="server" Text="<%$Resources:Resource_es,lblReUsuario%>"></asp:Literal></h1></center>
    </div>
        <br />
<table>
<tr>
    <td align ="right">Tipo de cuenta
        <asp:DropDownList ID="ddltypoPerfil" runat="server"
         AutoPostBack = "true">
         <asp:ListItem Selected ="True" Value="User">usuario</asp:ListItem>
         <asp:ListItem  Value="Admin">Administrador</asp:ListItem>
        
        </asp:DropDownList>
    </td>    
</tr>
<tr>
    <td>
        <p>
            <asp:Literal ID="ltlUsuario" runat="server" Text="<%$Resources:Resource_es,lblUsuario%>"></asp:Literal>
            <asp:TextBox ID = "txtUsuario" CssClass="txt" runat="server" MaxLength="20" Width="180px" 
                    TabIndex="1"></asp:TextBox>
            <asp:RequiredFieldValidator ID="revUsuario" runat="server" 
                    ControlToValidate="txtUsuario" ForeColor="Red" Text="*" 
                    ValidationGroup="vldNuevo"></asp:RequiredFieldValidator>
        </p>
    </td>
</tr>
<tr>
    <td>
        <p>
            <asp:Literal ID="ltlContrasena" runat="server" Text="<%$Resources:Resource_es,lblContrasena%>" ></asp:Literal>
                <asp:TextBox ID="txtpass" CssClass="txt" runat="server" TextMode="Password" MaxLength="20" 
                Width="180px" TabIndex="2"></asp:TextBox>
            <asp:RequiredFieldValidator ID="revContrasena" runat="server" 
                ControlToValidate="txtpass" ForeColor="Red" Text="*" 
                ValidationGroup="vldNuevo"></asp:RequiredFieldValidator>
        </p>
    </td>
</tr>
<tr>
    <td>
        <p>
            <asp:Literal ID="ltlConfContraseña" runat="server" Text="<%$Resources:Resource_es,lblConfContrasena%>"></asp:Literal> 
            <asp:TextBox ID="txtConfirpass" CssClass="txt" runat="server" TextMode="Password" 
                MaxLength="20" Width="180px" TabIndex="3"></asp:TextBox>
            <asp:RequiredFieldValidator ID="revConfirContraseña" runat="server" 
                ControlToValidate="txtConfirpass" ForeColor="Red" Text="*" 
                ValidationGroup="vldNuevo"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="covContrasena" runat="server" 
                ControlToCompare="txtpass" ControlToValidate="txtConfirpass" 
                ErrorMessage="<%$Resources:Resource_es,lblErrorPassNew%>" ForeColor="Red" 
                ValidationGroup="vldNuevo"></asp:CompareValidator>
        </p>
    </td>
</tr>
</table>
    <br />
    <p style="text-align:right; background:none; border:none;">
            <asp:Button ID="btnNuevo" CssClass="btn" runat="server" Text="Nuevo" 
                onclick="btnNuevo_Click" ValidationGroup="vldNuevo" TabIndex="4" />
    </p>
    
    </asp:Panel>
    
  <asp:LinkButton ID="lklConfirmar" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlContinuar" runat="server"
     TargetControlID = "lklConfirmar"
     PopupControlID = "pnlConfirmar" DropShadow ="false"
     BackgroundCssClass="modalBackground">        
</cc1:modalpopupextender>
    <asp:Panel ID="pnlConfirmar" runat="server" 
    CssClass="modalPopup" Width="293px">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblContinuar" runat="server"  Text="<%$Resources:Resource_es,lblContinuar%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td class="style5" align="center">
                <asp:Button ID="btnContinuar" runat="server" Text="Continuar" 
                    onclick="btnContinuar_Click" />
                <asp:Button ID="btnSalir" runat="server" Text="Salir" 
                    onclick="btnSalir_Click" />
            </td>
        </tr>
    </table>
    </asp:Panel>
    </div>
</asp:Content>

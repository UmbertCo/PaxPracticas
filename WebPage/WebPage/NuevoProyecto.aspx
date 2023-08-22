<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true" validateRequest="false" CodeBehind="NuevoProyecto.aspx.cs" Inherits="PAXActividades.WebForm4" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="formas">
<div>
<h1 align="center">
    <asp:Literal ID="ltlTitulo" runat="server" Text="<%$Resources:Resource_es,lblNuevoProyecto%>"></asp:Literal></h1>
</div>

<p>
                <asp:Literal ID="ltlNombreProyecto" runat="server" Text="<%$Resources:Resource_es,lblNombreProyecto%>"></asp:Literal>
            
                <asp:TextBox ID="txtProyecto" runat="server" MaxLength="100" CssClass="txt"
                    ValidationGroup="valdgroupnuevo" TabIndex="1" Width="300px"></asp:TextBox>
            
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtProyecto" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="valdgroupnuevo"></asp:RequiredFieldValidator>
            </p>

<p>
                <asp:Literal ID="ltlDescripcion" runat="server" Text="<%$Resources:Resource_es,lblDescripcion%>"></asp:Literal>
            <br />

                <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" CssClass="txt"
                 ValidationGroup="valdgroupnuevo" 
                TabIndex="2" Width="510px" MaxLength="200" Height="106px"></asp:TextBox>
            
                <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" 
                    ControlToValidate="txtDescripcion" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="valdgroupnuevo"></asp:RequiredFieldValidator>
           </p>
<p style="text-align:right; background:none; border:none;">
                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn"
                    onclick="btnNuevo_Click" ValidationGroup="valdgroupnuevo" TabIndex="3" />
                    </p>
            
    <asp:LinkButton ID="lkbNuevo" runat="server"></asp:LinkButton>
<cc1:modalpopupextender ID="mdlNuevoProyecto" runat="server"
     TargetControlID = "lkbNuevo"
     PopupControlID = "pnlNuvoProyecto" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlNuvoProyecto" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblAgregarProyecto" runat="server" Text="<%$Resources:Resource_es,lblAgreProyecto%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td class="style5" align="center"><asp:Button ID="btnOK" runat="server" Text="Aceptar" CssClass="btn"
                    onclick="btnOK_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn"
                    onclick="btnCancelar_Click"/>
            </td>
        </tr>
    </table>
   </asp:Panel>
  <asp:LinkButton ID="LkAviso" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="MdlAviso" 
    runat="server" 
    TargetControlID="LkAviso" PopupControlID="pnlAvisoCambio2"
    BackgroundCssClass="modalBackground">
  </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlAvisoCambio2" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>                
                <asp:Label ID="lblcambio2" runat="server" Text=""></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="Button1" runat="server" Text="Aceptar" CssClass="btn" />
            </td>
        </tr>
    </table>
    </asp:Panel>
    </div>
</asp:Content>

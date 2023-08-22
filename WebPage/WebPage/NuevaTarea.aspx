<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true" validateRequest="false" CodeBehind="NuevaTarea.aspx.cs" Inherits="PAXActividades.WebForm8" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="formas">
<div align="center">
    <table width="100%">
        <tr>
            <td align="center">
            <h1>
                <asp:Label ID="lblAgreTarea" runat="server" Text="<%$Resources:Resource_es,lblAgreTarea%>"></asp:Label> 
            </h1>
            </td>
        </tr>
    </table>

</div>
    <p>
             <asp:Literal ID="ltlTarea" runat="server" Text="<%$Resources:Resource_es,lblTarea%>"></asp:Literal>
             
             <asp:TextBox ID="txtTarea" runat="server" TabIndex="1" MaxLength="100" 
                 CssClass="txt" Width="300px"></asp:TextBox>
            
            <asp:RequiredFieldValidator ID="rfvTarea" runat="server" 
                    ControlToValidate="txtTarea" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="NuevaTarea"></asp:RequiredFieldValidator>
           </p>
        <p>
                <asp:Literal ID="ltlDescripcion" runat="server" Text="<%$Resources:Resource_es,lblDescripcion%>"></asp:Literal>
                <br />
                <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" CssClass="txt"
                Height="106px" Width="510px" TabIndex="2" MaxLength="200"></asp:TextBox>
               
                    <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" 
                    ControlToValidate="txtDescripcion" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="NuevaTarea"></asp:RequiredFieldValidator>
                </p>
    <p style="text-align:right; background:none; border:none;">
           
                <asp:Button ID="btnNueva" runat="server" Text="Nueva" 
                    ValidationGroup="NuevaTarea" onclick="btnNueva_Click" TabIndex="3" CssClass="btn" />
            </p>
<asp:LinkButton ID="lkbNuevo" runat="server"></asp:LinkButton>
<cc1:modalpopupextender ID="mdlNuevaTarea" runat="server"
     TargetControlID = "lkbNuevo"
     PopupControlID = "pnlNuevaTarea" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlNuevaTarea" runat="server" 
    CssClass="modalPopup">
    <table width= "100%">
        <tr>
            <td align= "center">
            <h3>
                <asp:Label ID="lblmdlAgregarTarea" runat="server" Text="<%$Resources:Resource_es,lblmdlAgreTarea%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center"><asp:Button ID="btnOK" runat="server" Text="Si" CssClass="btn"
                    onclick="btnOK_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"  CssClass="btn"/>
            </td>
        </tr>
    </table>
   </asp:Panel>
   
<asp:LinkButton ID="lkbAviso" runat="server"></asp:LinkButton>
<cc1:modalpopupextender ID="mdlAviso" 
runat="server" 
TargetControlID="lkbAviso" PopupControlID="pnlAviso"
BackgroundCssClass="modalBackground">
</cc1:modalpopupextender>
<asp:Panel ID="pnlAviso" runat="server" 
CssClass="modalPopup">
<table width="100%">
    <tr>
        <td align="center">
        <h3>            
            <asp:Label ID="lblmdlAviso" runat="server" Text="<%$Resources:Resource_es,lblmdlTareaAgre%>"></asp:Label> 
        </h3>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Button ID="btnAvisoOk" runat="server" Text="Aceptar"  CssClass="btn"
                onclick="btnAvisoOk_Click"/>
        </td>
    </tr>
</table>
</div>
</asp:Panel>
</asp:Content>

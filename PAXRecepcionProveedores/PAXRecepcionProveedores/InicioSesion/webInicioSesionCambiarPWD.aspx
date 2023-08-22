<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="webInicioSesionCambiarPWD.aspx.cs" Inherits="PAXRecepcionProveedores.InicioSesion.webInicioSesionCambiarPWD" %>
<%@ Register src="../WebControls/usrGlobalPwd.ascx" tagname="usrGlobalPwd" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<br /><br /><br />
    <center>
    <%--<h2>
        <asp:Label ID="lblCambiar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCambiar %>"> ></asp:Label>
    </h2>--%>
    <p>
        <asp:Label ID="lblCambiarDetalle" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCambiarDetalle %>"></asp:Label>
    </p>
    <p>
        <asp:Label ID="lblRestricciones" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRestricciones %>"></asp:Label>
    </p>
    <div>
    <uc1:usrGlobalPwd ID="usrGlobalPwd" runat="server" /> 
    </div>
   <%--<asp:Panel ID="pnlcambioPSW" runat="server">
   
        
        </asp:Panel>--%>
   <br />
   </center>
</asp:Content>

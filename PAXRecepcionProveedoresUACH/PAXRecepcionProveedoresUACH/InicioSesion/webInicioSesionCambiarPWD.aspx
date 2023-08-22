<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="webInicioSesionCambiarPWD.aspx.cs" Inherits="InicioSesion_webInicioSesionCambiarPWD" %>
<%@ Register src="../WebControls/usrGlobalPwd.ascx" tagname="usrGlobalPwd" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<br />
<br />
 <center style="width: 965px">
    <%--<h2>
        <asp:Label ID="lblCambiar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCambiar %>"> ></asp:Label>
    </h2>--%>
        <asp:Label ID="lblCambiarDetalle" runat="server" 
         Text="<%$ Resources:resCorpusCFDIEs, lblCambiarDetalle %>" 
         Font-Size="Small"></asp:Label>
        <asp:Label ID="lblRestricciones" runat="server" 
         Text="<%$ Resources:resCorpusCFDIEs, lblRestricciones %>" 
         Font-Size="Small"></asp:Label>
   <div> 
        <uc1:usrGlobalPwd ID="usrGlobalPwd" runat="server"  />
   </div>
    <br />
    </center>
</asp:Content>


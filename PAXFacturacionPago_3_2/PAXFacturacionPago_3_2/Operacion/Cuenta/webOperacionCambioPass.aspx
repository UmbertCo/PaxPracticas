<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionCambioPass.aspx.cs" Inherits="Operacion_Cuenta_webOperacionCambioPass" %>

<%@ Register src="../../WebControls/usrGlobalPwd.ascx" tagname="usrGlobalPwd" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, hedPass %>"></asp:Label>
    </h2>
    <p>
        <asp:Label ID="lblRestricciones" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRestricciones %>"></asp:Label>
    </p>
    <uc1:usrGlobalPwd ID="usrGlobalPwd1" runat="server" bTerminarSesion="True" TextoBoton="<%$ Resources:resCorpusCFDIEs, btnActualizar %>" 
        sRedireccion="~/InicioSesion/webInicioSesionLogin.aspx" />
</asp:Content>


<%@ Page Title="Cambiar" Language="C#" MasterPageFile="~/webGlobalMaster.master" AutoEventWireup="true"
    CodeFile="webInicioSesionCambiarPWD.aspx.cs" Inherits="Account_ChangePassword" %>

<%@ Register src="../WebControls/usrGlobalPwd.ascx" tagname="usrGlobalPwd" tagprefix="uc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <center>
    <h2>
        <asp:Label ID="lblCambiar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCambiar %>"> ></asp:Label>
    </h2>
    <p>
        <asp:Label ID="lblCambiarDetalle" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCambiarDetalle %>"></asp:Label>
    </p>
    <p>
        <asp:Label ID="lblRestricciones" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRestricciones %>"></asp:Label>
    </p>
   
        <uc1:usrGlobalPwd ID="usrGlobalPwd" runat="server" />
   
    <br />
    </center>
    </asp:Content>
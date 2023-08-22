<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="GenerarAddenda.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Addendas.GenerarAddenda" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <asp:Button ID="btnCapturarAddenda" runat="server" Text="Capturar Addenda" />
        <asp:Button ID="btnPegarAddenda" runat="server" Text="Pegar Addenda" 
            onclick="btnPegarAddenda_Click"  />
    </div>
</asp:Content>

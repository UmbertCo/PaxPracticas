<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="webTimbradoGeneracion.aspx.cs" Inherits="webTimbradoGeneracion" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" language="javascript">

    function fnAbrirVentanaComplemento() {
        hidden = open("webComplementos.aspx", 'NewWindow', 'top=0,left=0,width=1200,height=700,status=yes,resizable=yes,scrollbars=yes');
    }    

</script>

    <h2>
        Welcome to ASP.NET!
    </h2>
    <p>
        To learn more about ASP.NET visit <a href="http://www.asp.net" title="ASP.NET Website">www.asp.net</a>.
    </p>
    <p>
        <button id="btnAbrirComplemento" class="clone" onclick="fnAbrirVentanaComplemento();" >Agregar</button>         
    </p>
</asp:Content>

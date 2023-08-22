<%@ Page Title="Contratar" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Contratar.aspx.cs" Inherits="WebServidores._Contratar" %>

<asp:Content runat="server" ContentPlaceHolderID="Cencabezado">

    <style type="text/css">
        .anterior
        {
            position:relative;
            top: 270px;
            left: 600px;
            z-index: 2;
        }
        .siguiente
        {
            position:relative;
            top: 270px;
            left: 660px;
            z-index: 2;
        }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Cmenu"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Canuncio">
    <ul class="slides">
        <li>
            <img Class="IMGanuncio" src="/Imagenes/paqueteEconomy.png" /><a href="Comprar.aspx?paquete=economico"><img Class="IMGboton" src="/Imagenes/Contratar.png" /></a>
        </li>
        <li>
            <img Class="IMGanuncio" src="/Imagenes/paquetePremium.png" /><a href="Comprar.aspx?paquete=premium"><img Class="IMGboton" src="/Imagenes/Contratar.png" /></a>
        </li>
        <li>
            <img Class="IMGanuncio" src="/Imagenes/paqueteUltimate.png" /><a href="Comprar.aspx?paquete=ultimate"><img Class="IMGboton" src="/Imagenes/Contratar.png" /></a>
        </li>
        <a href="#" class="anterior" ><img src="/Imagenes/izq.png"></img></a>
        <a href="#" class="siguiente" ><img src="/Imagenes/derecha.png"></img></a>
    </ul>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Cofrecemos">
    <br />
        <div id="lblBrindamos">
            <img src="/Imagenes/brindamos.png"/>
        </div>
    <asp:Table ID="Table1" runat="server" Height="210px" Width="841px">  
        <asp:TableRow ID="TableRow2" runat="server" ForeColor="White" CssClass="tablaBrindamos">  
                <asp:TableCell width="10px"></asp:TableCell>  
                <asp:TableCell width="170px"><a href="soporte.aspx"><img src="/Imagenes/soporte.png"/></a></asp:TableCell>  
                <asp:TableCell width="21px"></asp:TableCell>  
                <asp:TableCell width="170px"><a href="acceso.aspx"><img src="/Imagenes/acceso.png"/></a></asp:TableCell>  
                <asp:TableCell width="21px"></asp:TableCell> 
                <asp:TableCell width="170px"><a href="orientacion.aspx"><img src="/Imagenes/orientacion.png"/></a></asp:TableCell>  
                <asp:TableCell width="21px"></asp:TableCell> 
                <asp:TableCell width="170px"><a href="disponibilidad.aspx"><img src="/Imagenes/disponibilidad.png"/></a></asp:TableCell>  
        </asp:TableRow>  
    </asp:Table>
    </asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Cfooter"></asp:Content>

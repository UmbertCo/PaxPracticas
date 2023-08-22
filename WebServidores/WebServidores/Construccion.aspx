<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Construccion.aspx.cs" Inherits="WebServidores._Default" %>

<asp:Content runat="server" ContentPlaceHolderID="Cencabezado">

     <style type="text/css">
        .trabajando
        {
            z-index: 2;
            position: relative;
            float: left;
            left: 70px;
            top: 50px;
        }
        
        .mensaje
        {
            font-size: 12pt;
            position: relative;
            bottom: 140px;
        }
        
        #contenidoTrabajando
        {
            position: relative;
            bottom: 260px;
            left: 180px;
            font-weight: bold;
            z-index: 2;
        }
        .IMGanuncio
        {
            z-index: 1;
            position: relative;
            right: 120px;
        }
    </style>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Cmenu"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Canuncio"><img Class="IMGanuncio" src="/Imagenes/fondo.png" /><img Class="trabajando" src="/Imagenes/trabajando.png" />
<div id="contenidoTrabajando">Lo sentimos pero el contenido de esta pagina aun está en construcción. Por favor regresa <br />para recibir más información sobre nuestros productos y servicios.</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Cofrecemos">
    <br />
    <div id="lblBrindamos">
        <img src="/Imagenes/brindamos.png"/>
    </div>
    <asp:Table ID="Table1" runat="server" Height="210px" Width="841px">  
        <asp:TableRow ID="TableRow2" runat="server" ForeColor="White" CssClass="tablaBrindamos">  
                <asp:TableCell width="10px"></asp:TableCell>  
                <asp:TableCell width="170px"><a href="Construccion.aspx"><img src="/Imagenes/soporte.png"/></a></asp:TableCell>  
                <asp:TableCell width="21px"></asp:TableCell>  
                <asp:TableCell width="170px"><a href="Construccion.aspx"><img src="/Imagenes/acceso.png"/></a></asp:TableCell>  
                <asp:TableCell width="21px"></asp:TableCell> 
                <asp:TableCell width="170px"><a href="Construccion.aspx"><img src="/Imagenes/orientacion.png"/></a></asp:TableCell>  
                <asp:TableCell width="21px"></asp:TableCell> 
                <asp:TableCell width="170px"><a href="Construccion.aspx"><img src="/Imagenes/disponibilidad.png"/></a></asp:TableCell>  
        </asp:TableRow>  
    </asp:Table>
    </asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Cfooter"></asp:Content>

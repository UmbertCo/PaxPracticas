<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebServidores._Default" %>

<asp:Content runat="server" ContentPlaceHolderID="Cencabezado"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Cmenu"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Canuncio"><img Class="IMGanuncio" src="/Imagenes/Anuncio.png" /><a href="Contratar.aspx"><img Class="IMGboton" src="/Imagenes/Vermas.png" /></a></asp:Content>
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

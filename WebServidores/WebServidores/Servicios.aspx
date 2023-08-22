<%@ Page Title="Servicios" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Servicios.aspx.cs" Inherits="WebServidores._Servicios" %>

<asp:Content runat="server" ContentPlaceHolderID="Cencabezado">
    <style type="text/css">
        .extras
        {
            position: relative;
            right: 55px;
            z-index: 1;
        }        
        .botonMax
        {
            position: relative;
            right: 120px;
            z-index: 2;
            bottom: 10px;
        }
        .botonMin
        {
            position: relative;
            right: 120px;
            z-index: 2;
            bottom: 181px;
        }
        .lblProductos
        {
            font-weight:bold; 
            font-size: 16pt;
            color: Black;
            z-index: 3;
        }
        .lblError
        {
            font-weight:bold; 
            font-size: 14pt;
            color: Black;
            z-index: 2;
            text-shadow: -3px 0 white, 0 3px white, 3px 0 white, 0 -3px white;
            margin-left: 5px;
        }
        #productosExtra
        {
            position: relative;
            bottom: 150px;
            right: 30px;
            z-index: 3;
        }
        #contenedorIzq
        {
            z-index: 2;
            bottom: 520px;
            left: 20px;
            position: relative;
            width: 1000px;
        }
        .txtCampos
        {
            width:200px;
        }
        .lblCampos
        {
            font-size: 14pt;
            position: relative;
            bottom: 30px;
        }
        .txtArea
        {
            border-top-left-radius: 10px;
            border-top-right-radius: 10px;
            border-bottom-left-radius: 10px;
            border-bottom-right-radius: 10px;
            border: 1px solid black;
            height: 98px;
            padding-left: 10px;
            margin-top: 5px;
            width:450px;
        }
        .fila
        {
            margin-bottom: 20px;
        }
        #botonLimpiar
        {
            z-index: 2;
            top: 20px;
            left: 720px;
            position: relative;
            width:200px;
        }
        #botonEnviar
        {
            z-index: 2;
            bottom: 48px;
            left: 940px;
            position: relative;
            width:200px;
        }
        .lblPrecios
        {
            font-weight:bold; 
            font-size: 16pt;
            color: Black;
            z-index: 3;
            text-shadow: -3px 0 white, 0 3px white, 3px 0 white, 0 -3px white;
            position: relative;
            bottom: 30px;
        }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Canuncio"><img Class="IMGanuncio" src="/Imagenes/fondoContacto.png" />
    <div id="contenedorIzq">
    <asp:Table ID="Table1" runat="server">  
        <asp:TableRow runat="server" ForeColor="White">  
                <asp:TableCell width="180px" >
                <img src="Imagenes/ssl.png" /><br /><br /><br />
                 </asp:TableCell>  
                <asp:TableCell width="900px">
                    <asp:label CssClass="lblCampos" runat="server" text="Certificado SSL"></asp:label><br />
                    <asp:label CssClass="lblPrecios" runat="server" text="Indispensable para sitios que manejan información<br/>personal, que desean reforzar la seguridad de sus datos<br/>y mejorar la confianza en sus usuarios"></asp:label>
                </asp:TableCell>  
         </asp:TableRow>
         <asp:TableRow runat="server" ForeColor="White">  
                <asp:TableCell >
                <img src="Imagenes/IP.png" /><br /><br /><br />
                 </asp:TableCell>  
                <asp:TableCell>
                    <asp:label CssClass="lblCampos" runat="server" text="IP dedicada"></asp:label><br />
                    <asp:label CssClass="lblPrecios" runat="server" text="Necesaria para instalar un Certificado de Seguridad,<br/>además de que permite realizar conexiones remotas<br/>con mayor seguridad."></asp:label>
                </asp:TableCell>  
         </asp:TableRow>
         <asp:TableRow runat="server" ForeColor="White">  
                <asp:TableCell >
                <img src="Imagenes/creacion.png" />
                 </asp:TableCell>  
                <asp:TableCell>
                    <asp:label CssClass="lblCampos" runat="server" text="Creación"></asp:label><br />
                    <asp:label CssClass="lblPrecios" runat="server" text="Tu sitio web a la medida. Creamos según tus requerimientos<br/>para que unas tu empresa al mundo del internet."></asp:label>
                </asp:TableCell>  
         </asp:TableRow>
    </asp:Table>
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Cfooter"></asp:Content>
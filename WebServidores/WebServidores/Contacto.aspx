<%@ Page Title="Contacto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Contacto.aspx.cs" Inherits="WebServidores._Contacto" %>

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
            bottom: 510px;
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
            font-size: 12pt;
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
            margin-left: 5px;
        }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Canuncio"><img Class="IMGanuncio" src="/Imagenes/fondoContacto.png" />
    <div id="contenedorIzq">
        <div id="botonLimpiar">
           <asp:imagebutton Class="IMGboton" ImageUrl="/Imagenes/Limpiar.png" 
                runat="server" ID="btnLimpiar" onclick="Limpiar"/>  
        </div>
        <div id="botonEnviar">
           <asp:imagebutton Class="IMGboton" ImageUrl="/Imagenes/Enviar.png" runat="server" 
                ID="btnEnviar" onclick="Enviar"/>  
        </div>
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Cfooter"></asp:Content>
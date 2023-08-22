<%@ Page Title="Comprar" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Comprar.aspx.cs" Inherits="WebServidores._Comprar" %>

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
            left: 820px;
            z-index: 2;
            bottom: 50px;
        }
        .botonMin
        {
            position: relative;
            left: 820px;
            z-index: 2;
            bottom: 210px;
        }
        .lblProductos
        {
            font-weight:bold; 
            font-size: 16pt;
            color: Black;
            z-index: 3;
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
        #productosExtra
        {
            position: relative;
            bottom: 200px;
            right: 30px;
            z-index: 3;
        }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Cmenu"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Canuncio"><img Class="IMGanuncio" src="/Imagenes/fondo.png" />
    <div id="contenedoresIzq">
        <asp:label Class="lblCampos" runat="server" text="Paquete:"></asp:label><br />
        <asp:textbox CssClass="txtCampos" runat="server" ID="txtPaquete" Enabled="False"></asp:textbox><br /><br />
        <asp:label Class="lblCampos" runat="server" text="Precio:"></asp:label><br />
        <asp:textbox CssClass="txtCampos" runat="server" ID="txtPrecio" Enabled="False"></asp:textbox><br /><br />
        <asp:label Class="lblCampos" runat="server" text="Meses:"></asp:label><br />
        <asp:textbox CssClass="txtCampos" runat="server" ID="txtMeses" Enabled="False"></asp:textbox>
    </div>
    <div id="contenedoresDer">
        <asp:label Class="lblCampos" runat="server" text="Descripción:"></asp:label><br />
        <asp:textbox CssClass="txtArea" runat="server" ID="txtDescripcion" Rows="5" TextMode="MultiLine" Enabled="False"></asp:textbox><br /><br />
        <asp:label Class="lblCampos" runat="server" text="Total:"></asp:label><br />
        <asp:textbox CssClass="txtCampos" runat="server" ID="txtTotal" Enabled="False" 
            AutoPostBack="True"></asp:textbox>
    </div>
    <div id="botonDerecho">
        <a href="Contratar.aspx"><img Class="IMGboton" src="/Imagenes/Comprar.png" /></a>
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Cofrecemos">
    <br />
        <asp:panel id="panelMin" runat="server" Visible="true">
            <img Class="extras" src="/Imagenes/eresumido.png"/>
            <asp:imagebutton Class="botonMax" ImageUrl="/Imagenes/btnMax.png" runat="server" onclick="maximizar" ID="btnMax" Visible="true"/>  
        </asp:panel>
        <asp:panel id="panelMax" runat="server" Visible="false">
            <img Class="extras" src="/Imagenes/eextendido.png"/>
            <asp:imagebutton Class="botonMin" ImageUrl="/Imagenes/btnMin.png" runat="server" onclick="minimizar" ID="btnMin" Visible="false"/>
            <div id="productosExtra">
                <asp:checkbox runat="server" ID="chkServidor" oncheckedchanged="sumarFTP" AutoPostBack="True"/><asp:label Class="lblProductos" runat="server" text=" Servidor FTP:" Visible="true"/>
                <asp:label Class="lblPrecios" runat="server" text="  $50.00"></asp:label><br /><br />

                <asp:checkbox runat="server" ID="chkPosicionamiento" AutoPostBack="True" oncheckedchanged="sumarPosicionamiento"/>
                <asp:label Class="lblProductos" runat="server" 
                    text=" Posicionar mi contenido en google" ToolTip="Se de los primeros resultados en la busqueda de google."/>
                <asp:label Class="lblPrecios" runat="server" text="  $599.00"></asp:label><br /><br />
                <asp:checkbox runat="server" ID="chkKit" AutoPostBack="True" oncheckedchanged="sumarKit"/><asp:label Class="lblProductos" runat="server" text=" Kit de aplicaciones web" Visible="true"/>
                <asp:label Class="lblPrecios" runat="server" text="  $99.00"></asp:label><br /><br />
            </div>
        </asp:panel>
    </asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Cfooter"></asp:Content>
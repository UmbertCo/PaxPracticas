<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/webOperatorMaster.master" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">
<div class="container">
        <h1>
            <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblBienvenido %>" CssClass="TituloInicio"></asp:Label>
            <hr class="hrbottom" style="padding-bottom: 0px; margin-bottom: 0px;"/>
        </h1>
    </div>
     <div class="container">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12"> 
                <asp:Label ID="lblBienvendia" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblBienvenida %>" 
                    style="font-family: 'Century Gothic'">
                </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12"> 
                <asp:Label ID="lblProveedor" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblProveedor %>" 
                    style="font-family: 'Century Gothic'">
                </asp:Label><br />
                <asp:Label id="lblPermisos" runat="server" Visible="false" >
                </asp:Label>
            </div>
        </div>
        <nav class="navbar navbar-default-pax" role="navigation">
            <div class="navbar-header">
            <a class="navbar-brand" style="color:white;" href="#"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblAvisos %>" /></a>
            </div>
        </nav> 
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12"> 
                <asp:Label id="lblFielSat" runat="server" Visible="true" Text="Se les informa que a partir del día 15 de Julio del 2016, con el Fundamento legal del Artículo 29- A fracción VII, inciso c, del Código Fiscal de la Federación, y regla 2.7.1.32, tercer y cuarto párrafos de la Segunda Resolución de Modificaciones a la Resolución Miscelánea Fiscal para 2016
Los contribuyentes Emisores, deberán “especificar en el CFDI la clave correspondiente del concepto con que fue realizado el pago al método de pago, de conformidad con el Catálogo publicado en el portal del SAT” (Servicio de Administración Tributaria).
http://www.sat.gob.mx/informacion_fiscal/preguntas_frecuentes/Paginas/default.aspx
">
                </asp:Label>
            </div>
        </div>
     </div>
</asp:Content>
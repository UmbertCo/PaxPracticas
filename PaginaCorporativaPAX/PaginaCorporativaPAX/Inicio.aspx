<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Inicio.aspx.cs" Inherits="Inicio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="StyleSheet.css" rel="Stylesheet" type="text/css" />
    <link rel="shortcut icon" href="imagenes/fav.ico" />
  
    <style type="text/css">
        .style8
        {
            text-align: center;
            height: 41px;
            width: 300px;
            border-style: none;
            border-color: inherit;
            border-width: 0;
            background-image: url('imagenes/fondo2.jpg');
            background-repeat: no-repeat;
            background-position: left center;
        }
    </style>

  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
    <script runat="server" type="text/C#">
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static AjaxControlToolkit.Slide[] GetSlides()
        {
            return new AjaxControlToolkit.Slide[] {
            new  AjaxControlToolkit.Slide("imagenes/facturacion-electronica.jpg", "quienes", "quienes"),
            new  AjaxControlToolkit.Slide("imagenes/servidor_web.jpg", "mona", "imagen"),
            new  AjaxControlToolkit.Slide("imagenes/desarrollo-sw.jpg", "logo", "logo")};
        }
      
   </script>
<table class="hojappal" align="center" style="background-position: center center">
        <tr>
            <td  align ="center">

                <asp:Image ID ="Foto" runat ="server" />
                <cc1:SlideShowExtender ID="SlideShowExtender1" runat="server" 
                TargetControlID="Foto" 
                SlideShowServiceMethod="GetSlides" 
                AutoPlay="true" 
                ImageTitleLabelID="imageTitle" 
                ImageDescriptionLabelID="imageDescription"          
                PreviousButtonID="prevButton" 
                Loop="true" PlayInterval="7000" />
            </td>
        </tr>
</table>

<center>
        <table border="0" cellpadding="0" cellspacing="0">  
            <tr>
                <td class="fondo2">
                
                <asp:Label ID="lblTituloQuienesPAX" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblQuienesPAX %>" Font-Bold="False" 
                Font-Size="Small" style="color: #46626E; font-family: 'Century Gothic'; text-align: center; font-weight: 700; font-size: medium;" 
                    Height="15px" Width="260px"></asp:Label>
               </td>
            <td class="fondo2"  >
                                           
                <asp:Label ID="lblTituloNoticias" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloNoticias %>" Font-Bold="False" 
                Font-Size="Small" style="color: #46626E; font-family: 'Century Gothic'; text-align: center; font-size: medium; font-weight: 700;" 
                    Height="15px" Width="260px"></asp:Label>
              </td>
            <td class="style8" valign="middle">
            
                   
               <asp:Label ID="lblTituloClientes" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloclientesQue %>" Font-Bold="False" 
                Font-Size="Small" style="color: #46626E; font-family: 'Century Gothic'; text-align: center; font-size: medium; font-weight: 700;" 
                    Height="15px" Width="260px"></asp:Label>
                    </td>
        
        </tr>
        <tr>
            <td class="fondo2">
                <asp:Image ID="imgQuienesPAX" runat="server" Height="70px" 
                    ImageUrl="~/imagenes/Quienes.jpg" Width="152px" BorderColor="White" 
                    BorderStyle="Solid" />
                &nbsp;</td>

            <td class="fondo2">
                <asp:Image ID="imgNoticias" runat="server" ImageUrl="~/imagenes/Newspaper-icon.png" 
                    Height="70px" Width="152px" BorderStyle="Solid" />
                &nbsp;</td>
            <td class="style8" >
                <asp:ImageButton
                        ID="imgLogoPAX" runat="server" ImageUrl="~/imagenes/top1.jpg" 
                    BorderColor="White" BorderStyle="Solid" Height="70px" 
                    PostBackUrl="~/Clientes.aspx" Width="152px" />
              </td>
        </tr>
        <tr>
            <td align="center" valign="top" class="fondo2" width="320px">
                <br />
                <asp:Label ID="lblQuienesPAX" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblQuienesSomos %>" Font-Bold="False" 
                Font-Size="Small" style="color: #46626E; font-family: 'Century Gothic'; text-align: justify;" 
                    Height="50px" Width="260px"></asp:Label>
             
                  <br />
             
                 
             
                  <a href="QuienEsPAXFacturacion.aspx" style="text-decoration:none">
                   <asp:Label ID="lblVerMasPAX" runat="server" 
                   Text="<%$ Resources:resCorpusCFDIEs, lblvermas %>" Font-Size="X-Small" 
                   Font-Underline="True" 
                    style="font-family: 'Century Gothic'; color: #82B520; font-size: small" 
                    Width="260px"></asp:Label>
                  </a>

       
            </td>
            <td class="fondo2"    align="center" valign="middle" width="320px">
                     <br />
                     <asp:Label ID="lblNoticias" runat="server" 
                    Text = "<%$ Resources:resCorpusCFDIEs, lblNoticias %>" Font-Bold="False" 
                    Font-Size="Small" style="color: #46626E; font-family: 'Century Gothic'; text-align: justify; " 
                         Height="50px" Width="280px"></asp:Label>
                     <br />
                     <a href="LeerMasNoticias.aspx" style="text-decoration:none">
                 <asp:Label ID="lblVerMasnoticias" runat="server" 
                 Text="<%$ Resources:resCorpusCFDIEs, lblleermas %>" Font-Size="X-Small" 
                  Font-Underline="True" 
                  style="font-family: 'Century Gothic'; font-size: small; color: #82B520"></asp:Label>
               </a>
                     <br />
                 
                     <a href="webLogin.aspx?componente=webNoticias.aspx" style="text-decoration:none">
                        <asp:Label ID="Label32" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblAgregarNot %>" Font-Size="X-Small" 
                            Font-Bold="False" Font-Underline="True" 
                            style="color: #82B520; font-family: 'Century Gothic'; font-size: small; text-align: right;" 
                         Width="280px"></asp:Label>
                    </a>
                     <br />
                     <br />
               </td>
            <td class="fondo2" width="320px" valign="top">
           
                    <br />
           
                    <asp:Label ID="lblClientes" runat="server" 
                    Text = "<%$ Resources:resCorpusCFDIEs, lblLogos %>" Font-Bold="False" 
                    Font-Size="Small" style="color: #46626E; font-family: 'Century Gothic'; text-align: justify;" 
                    Height="50px" Width="260px"></asp:Label>
                   
                                 
              </td>
              
        </tr>
    </table>
    </center>
</asp:Content>


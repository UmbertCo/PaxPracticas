<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Inicio-Desarrollo.aspx.cs" Inherits="Inicio_Desarrollo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="StyleSheet.css" rel="Stylesheet" type="text/css" />
    <link rel="shortcut icon" href="imagenes/fav.ico" />
  
    <style type="text/css">
        .style9
    {
        height: 18px;
    }
        .style11
    {
        width: 153px;
    }
    .style12
    {
        width: 152px;
    }
        .style13
        {
            width: 50px;
        }
        .style14
        {
            height: 28px;
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
            new  AjaxControlToolkit.Slide("imagenes/app-Escritorio.jpg", "quienes", "quienes"),
            new  AjaxControlToolkit.Slide("imagenes/app-Moviles.jpg", "mona", "imagen"),
            new  AjaxControlToolkit.Slide("imagenes/app-Web.jpg", "mona", "imagen"),
            new  AjaxControlToolkit.Slide("imagenes/seguridad-App.jpg", "mona", "imagen"),
            new  AjaxControlToolkit.Slide("imagenes/Servicio-Windows", "mona", "imagen"),
            new  AjaxControlToolkit.Slide("imagenes/Web-Service.jpg", "logo", "logo")};
        }
      
   </script>
<table class="hojaPrincipal">
        <tr>
            <td  align ="center" class="style9">

                <asp:Image ID ="Foto" runat ="server" Width="720px" Height="347px" />
                <cc1:SlideShowExtender ID="SlideShowExtender1" runat="server" 
                TargetControlID="Foto" 
                SlideShowServiceMethod="GetSlides" 
                AutoPlay="true" 
                ImageTitleLabelID="imageTitle" 
                ImageDescriptionLabelID="imageDescription"          
                PreviousButtonID="prevButton" 
                Loop="true" PlayInterval="2000" />
            </td>
            <td class="style9"></td>
        </tr>
</table>

 <center>
      <img src="imagenes/linea.jpg" width="945" height="2" />
 </center>
     <table  width="945px" >
    
        <tr>
        
            <td width="130px">
                &nbsp;</td>
            <td rowspan="4" valign="top" style="text-align: center">
                
             <asp:Label ID="Label1" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloDesarrolloDeSW %>" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: center; font-weight: 700; font-size: large;" 
                    Height="50px" Width="260px"></asp:Label>
                <br />
                <asp:ImageButton ID="ImageButton1" runat="server" 
                    ImageUrl="~/Imagenes/devolped_soft.png" 
                    BorderColor="#395C6C" BorderStyle="Solid" Height="91px" Width="120px" 
                    PostBackUrl="~/DesarrolloSW.aspx" BackColor="White" />

                     <br />

                     <br />
             
                     <asp:Label ID="Label2" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloDesarrolloSW %>" Font-Bold="False" 
                Font-Size="Small" style="color: #006699; font-family: 'Century Gothic'; text-align: center; font-size: medium;" 
                    Height="20px" Width="260px"></asp:Label>
                   
                     <br />
                   
                     <a href="DesarrolloSW.aspx" style="text-decoration:none">
                   <asp:Label ID="Label31" runat="server" 
                   Text="<%$ Resources:resCorpusCFDIEs, lblvermas %>" Font-Size="X-Small" 
                   Font-Underline="True" 
                    
                    style="font-family: 'Century Gothic'; color: #82B520; font-size: small; text-align: right;"></asp:Label>
                &nbsp;</td>
            <td rowspan="4" width="80px">
                &nbsp;</td>

            <td rowspan="4" valign="top" style="text-align: center">
              
             <asp:Label ID="Label3" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloContacto %>" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: center; font-weight: 700; font-size: large;" 
                    Height="50px" Width="260px"></asp:Label>
                <br />
                <asp:ImageButton ID="ImageButton2" runat="server" 
                    ImageUrl="~/Imagenes/mona-Chica.jpg" 
                    BorderColor="#395C6C" BorderStyle="Solid" Height="110px" Width="100px" 
                    PostBackUrl="~/Contacto.aspx" BackColor="White" />

                     <br />

                     <br />
             
                     <asp:Label ID="Label4" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lbltxtContacto %>" Font-Bold="False" 
                Font-Size="Small" style="color: #006699; font-family: 'Century Gothic'; text-align: center; font-size: medium;" 
                    Height="20px" Width="260px"></asp:Label>
                   
                     <br />
                   
                     <a href="Contacto.aspx" style="text-decoration:none">
                   <asp:Label ID="Label5" runat="server" 
                   Text="<%$ Resources:resCorpusCFDIEs, lblvermas %>" Font-Size="X-Small" 
                   Font-Underline="True" 
                    
                    style="font-family: 'Century Gothic'; color: #82B520; font-size: small; text-align: right;"></asp:Label>
                &nbsp;</td>

                &nbsp;
            <td rowspan="4" class="style12">
                &nbsp;</td>
            <td height="120px">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style14">
                </td>
            <td class="style14">
                </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="style13">
                &nbsp;</td>
            <td class="style11">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td class="style12">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>



</asp:Content>



  


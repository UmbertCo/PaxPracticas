<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="QuienEsPAXFacturacion.aspx.cs" Inherits="QuienEsPAXFacturacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <link rel="shortcut icon" href="imagenes/fav.ico" />
    <style type="text/css">
        #FlashID5
        {
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="hojaPrincipal">
        <tr>
            <td style="text-align: center" height="70px" valign="bottom">
                    

               <asp:Label ID="lblTituloQuienesPAXFacturacion" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloQuienesPAXFacturacion %>" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: left; font-size: xx-large; font-weight: 700;" 
                Height="40px" Width="700px"></asp:Label>

                &nbsp;</td>
        </tr>
        <tr>
            <td valign="bottom" 
                width="825px" style="text-align: center" height="170px">                
               <asp:Label ID="lblQuienesPAXFacturacion" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblQuienesPAXFacturacion %>" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: justify; font-size: large; " 
                Height="150px" Width="700px"></asp:Label>

               </td>
        </tr>
        <tr>
            <td style="text-align: center">

                     <br />
                     <iframe width="560" height="315" src="http://www.youtube.com/embed/WnMUbXZumNU" frameborder="0" allowfullscreen></iframe>

                <br />
                     <br />
                     <br />
                <br />
                &nbsp;<br />
            </td>
            
        </tr>
    </table>
</asp:Content>


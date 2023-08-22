<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ComprasOnLine.aspx.cs" Inherits="ComprasOnLine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="shortcut icon" href="imagenes/fav.ico" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="hojaPrincipal">
        <tr>
            <td style="text-align: center" width="825px">

                

                <asp:Label ID="lblTituloComprasOnLine" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloComprasOnLine %>" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: left; font-size: xx-large; font-weight: 700;" 
                Height="70px" Width="700px"></asp:Label>
                &nbsp;</td>
        </tr>
        <tr>
            <td width="825px" style="text-align: center">
              
               <asp:Label ID="lblcomprasOnLine" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblcomprasOnLine %>" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: justify; font-size: large; " 
                Height="100px" Width="700px"></asp:Label>


                <br />
                <a href="https://ventas.paxfacturacion.com.mx:444/" 
                   style="text-decoration:none; text-align: center;" target="_blank">
                   <asp:Label ID="lblirCompras" runat="server" 
                   Text="<%$ Resources:resCorpusCFDIEs, lbliraComprasOnLine %>" Font-Size="X-Small" 
                   Font-Underline="True" 
                    
                    style="font-family: 'Century Gothic'; color: #82B520; font-size: small; text-align: right;" 
                    Width="400px"></asp:Label>
                  </a>

                <br />

                &nbsp;</td>
        </tr>
        <tr>
            <td height="200px" width="825px" 
                style="text-align: center;">
                <asp:Image ID="Tarjetas" runat="server" ImageUrl="~/imagenes/OnLine.jpg" 
                    Width="300px" style="margin-right: 0px" />
               
                <br />
                <br />
                <br />
               
            </td>
        </tr>
    </table>
</asp:Content>


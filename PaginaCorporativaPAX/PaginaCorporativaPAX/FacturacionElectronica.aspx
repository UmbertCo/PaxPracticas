<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FacturacionElectronica.aspx.cs" Inherits="FacturacionElectronica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="shortcut icon" href="imagenes/fav.ico" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="hojaPrincipal">
        <tr>
            <td class="style9">
                </td>
            <td class="style9">
                </td>
            <td class="style9">
                </td>
            <td class="style9">
                </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="Titulo-Facturacion" valign="top" width="550px">
                <asp:Label ID="lblTituloFacturacion" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloFacturacion %>" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: left; font-weight: 700; font-size: xx-large;" 
                Height="38px" Width="520px"></asp:Label>
                &nbsp;</td>
                <td class="SAT-Grande" rowspan="2" width="500px">
                </td>

        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="center" valign="top" width="570px">

                <br />

                <asp:Label ID="lblTextFacturacion" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblFacturacionElectronica %>" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: justify; font-size: large; " 
                Height="260px" Width="520px"></asp:Label>
               
              
              

                
                </td>

            
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td style="text-align: center">
            
                  <a href="http://www.paxfacturacion.com" 
                   style="text-decoration:none; text-align: center;" target="_blank">
                   <asp:Label ID="lbliraFacturacion" runat="server" 
                   Text="<%$ Resources:resCorpusCFDIEs, lbliraFacturacion %>" Font-Size="X-Small" 
                   Font-Underline="True" 
                    
                    style="font-family: 'Century Gothic'; color: #82B520; font-size: small; text-align: right;" 
                    Width="520px" Height="40px"></asp:Label>
                  </a>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>


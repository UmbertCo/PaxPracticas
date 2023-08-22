<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Servidores.aspx.cs" Inherits="Servidores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="shortcut icon" href="imagenes/fav.ico" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <table class="hojaPrincipal">
        <tr>
            <td>
                &nbsp;</td>
            <td colspan="2" height="70px" width="600px">

                &nbsp;<asp:Label ID="lblTituloServidores" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloServidores %>" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: left; font-size: xx-large; font-weight: 700;" 
                Height="30px" Width="450px"></asp:Label>
                &nbsp;</td>
            <td class="Servidores" rowspan="5" width="500px">
                &nbsp;</td>
        </tr>
        <tr>
            <td width="50px">
                &nbsp;</td>
            <td colspan="2" rowspan="4" style="text-align: left" valign="top" width="600px">

                <asp:Label ID="lblServidores" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblServidores %>" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: justify; font-size: large; " 
                Height="180px" Width="420px"></asp:Label>

                <br />
                <a href="http://servidores.paxfacturacion.com.mx/"
                   style="text-decoration:none; text-align: center;" target="_blank">
                   <asp:Label ID="lbliraServidores" runat="server" 
                   Text="<%$ Resources:resCorpusCFDIEs, lbliraServidores %>" Font-Size="X-Small" 
                   Font-Underline="True" 
                    
                    style="font-family: 'Century Gothic'; color: #82B520; font-size: small; text-align: right;" 
                    Width="400px" Height="40px"></asp:Label>
                  </a>


             </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td >
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td style="text-align: center">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>


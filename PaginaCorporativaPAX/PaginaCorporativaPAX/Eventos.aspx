<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Eventos.aspx.cs" Inherits="Eventos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <link rel="shortcut icon" href="imagenes/fav.ico" />
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="hojaPrincipal">
        <tr>
            <td>
                </td>
            <td valign="middle" height="20px" style="text-align: center" width="800px">
                 <asp:Label ID="lblTituloEventos" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloEventos %>" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: left; font-size: xx-large; font-weight: 700;" 
                Height="50px" Width="700px"></asp:Label>
                &nbsp;</td>
            <%--<td  rowspan="3">
                &nbsp;</td>--%>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td rowspan="4" valign="middle" 
                width="800px" style="text-align: center">
                
               
                <asp:Label ID="lblTextEventos" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTextEventos %>" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: justify; font-size: large; margin-bottom: 0px;" 
                Height="100px" Width="700px"></asp:Label>

                   <br />

                   <a href="http://eventos.paxfacturacion.mx/" 
                   style="text-decoration:none; text-align: center;" target="_blank">
                   <asp:Label ID="lbliraEventos" runat="server" 
                   Text="<%$ Resources:resCorpusCFDIEs, lbliraEventos %>" Font-Size="X-Small" 
                   Font-Underline="True" 
                    
                    style="font-family: 'Century Gothic'; color: #82B520; font-size: small; text-align: right;" 
                    Width="430px"></asp:Label>
                  </a>
                
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
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
            <td>
                &nbsp;</td>
            <td class="Evento" height="190px">
                &nbsp;</td>
            
            
            
        </tr>
    </table>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Valores.aspx.cs" Inherits="Valores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <link rel="shortcut icon" href="imagenes/fav.ico" />
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="hojaPrincipal">
        <tr>
<%--            <td style="text-align: left">
                &nbsp;</td>--%>
            <td style="text-align: center" height="60px">


                <asp:Label ID="lblTituloValores" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloValores %>" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: left; font-size: xx-large; font-weight: 700;" 
                Height="60px" Width="520px"></asp:Label>

                &nbsp;</td>
            <td  class="" rowspan="3">
                &nbsp;</td>
        </tr>
        <tr>

            <td rowspan="2" valign="top" width="630" height="300px" align="left" 
                style="text-align: right">

                <br />

                <asp:Label ID="lblTextValores" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTextValores %>" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: left; font-size: small; " 
                Height="320px" Width="570px"></asp:Label>

                
                &nbsp;</td>

            <td  height="370px" width="300px">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/imagenes/Valores.jpg" 
                    Height="350px" Width="300px" />

                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
      
    </table>
</asp:Content>


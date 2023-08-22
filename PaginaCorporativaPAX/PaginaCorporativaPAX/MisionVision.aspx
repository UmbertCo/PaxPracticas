<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MisionVision.aspx.cs" Inherits="MisionVision" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <link rel="shortcut icon" href="imagenes/fav.ico" />
    <style type="text/css">
        .style8
        {
            width: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="hojaPrincipal">
        <tr>
            <td rowspan="2">
                &nbsp;</td>
            <td class="style8">



                &nbsp;</td>
            <td rowspan="2">
                &nbsp;</td>
            <td>


                &nbsp;</td>
            <td rowspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td >
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblTituloMision" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloMision %>" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: left; font-size: xx-large; font-weight: 700;" 
                Height="40px" Width="400px"></asp:Label>
                &nbsp;</td>
            <td  class="Mision"rowspan="4">

                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td rowspan="3" width="450px" valign="top" align="right" >

               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

               <asp:Label ID="lblTextVision" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTextVision %>" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: justify; font-size: medium; " 
                Height="220px" Width="400px"></asp:Label>
                &nbsp;</td>
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
            <td>
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
        <tr>
            <td>
                &nbsp;</td>
            <td class="Vision" height="300px">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td style="text-align: left" valign="top" height="320px">

                  <asp:Label ID="lblTituloVision" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloVision %>" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: left; font-size: xx-large; font-weight: 700;" 
                Height="60px" Width="410px"></asp:Label>


                  <br />


                <asp:Label ID="lblTextMision" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTextMision %>" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: justify; font-size: medium; " 
                Height="100px" Width="410px"></asp:Label>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>


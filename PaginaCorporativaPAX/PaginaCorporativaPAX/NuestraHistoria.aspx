<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NuestraHistoria.aspx.cs" Inherits="NuestraHistoria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="shortcut icon" href="imagenes/fav.ico" />
    <style type="text/css">
        .style8
        {
            width: 503px;
        }
    </style>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="hojaPrincipal">
        <tr>
            <td>
                &nbsp;</td>
            <td style="text-align: left" class="style8">




                &nbsp;</td>
            <%--<td>
                &nbsp;</td>--%>
<%--            <td>
                &nbsp;</td>--%>
<%--            <td>
                &nbsp;</td>--%>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <%--<td>
                &nbsp;</td>--%>
            <td height="10px" class="style8">

               <asp:Label ID="lblTituloNuestraHistoria" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloNuestraHistoria %>" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: left; font-size: xx-large; font-weight: 700;" 
                Height="55px" Width="400px"></asp:Label>
                &nbsp;</td>
            <td class="NuestraHistoria" rowspan="4" width="200px" valign="top">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td valign="top" class="style8" >

               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

               <asp:Label ID="lblNuestraHistoria" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblNuestraHistoria %>" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: justify; font-size: large; " 
                Height="270px" Width="400px"></asp:Label>

                &nbsp;</td>
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
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="style8">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="style8">
                &nbsp;</td>
            <td>
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


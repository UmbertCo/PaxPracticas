<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Clientes.aspx.cs" Inherits="Clientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <link rel="shortcut icon" href="imagenes/fav.ico" />
    <style type="text/css">
    .style8
    {
        width: 245px;
    }
        .style9
        {
            height: 23px;
        }
        .style10
        {
            height: 23px;
            width: 318px;
        }
        .style11
        {
            background-image: url('imagenes/suspiros22.jpg');
            background-repeat: no-repeat;
            background-position: center center;
            width: 318px;
            height: 100px;
        }
        .style12
        {
            background-image: url('imagenes/foxconn22.jpg');
            background-repeat: no-repeat;
            background-position: center center;
            width: 318px;
            height: 100px;
        }
        .style13
        {
            background-image: url('imagenes/interceramic22.jpg');
            background-repeat: no-repeat;
            background-position: center center;
            width: 318px;
            height: 100px;
        }
        .style14
        {
            width: 318px;
        }
        .style15
        {
            background-image: url('imagenes/interceramic22.jpg');
            background-repeat: no-repeat;
            background-position: center center;
            width: 245px;
            height: 100px;
        }
        .style16
        {
            background-image: url('imagenes/balatas.jpg');
            background-repeat: no-repeat;
            background-position: center center;
            width: 245px;
            height: 100px;
        }
        .style17
        {
            background-image: url('imagenes/uach22.jpg');
            background-repeat: no-repeat;
            background-position: center center;
            width: 245px;
            height: 130px;
        }
        .style18
        {
            background-image: url('imagenes/foxconn22.jpg');
            background-repeat: no-repeat;
            background-position: center center;
            width: 227px;
            height: 100px;
        }
        .style19
        {
            background-image: url('imagenes/suspiros22.jpg');
            background-repeat: no-repeat;
            background-position: center center;
            width: 227px;
            height: 100px;
        }
        .style20
        {
            background-position: center center;
            background-image: url('imagenes/subway22.jpg');
            background-repeat: no-repeat;
            width: 227px;
            height: 100px;
        }
        .style21
        {
            width: 227px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="hojaPrincipal">
        <tr>
            <td>
                &nbsp;</td>
            <td colspan="3" style="text-align: left">
                
               &nbsp;&nbsp;&nbsp;&nbsp;
                
               <asp:Label ID="TituloClientes" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloClientes %>" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: left; font-weight: 700; font-size: xx-large;" 
                Height="30px" Width="530px"></asp:Label>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style9">
                </td>
            <td colspan="2" style="text-align: left" class="style9" >
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label ID="ClientesQue" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblClientesQue %>" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: left; font-weight: 700; font-size: large;" 
                Height="20px" Width="340px"></asp:Label>
                </td>
            <td class="style10">
                </td>
            <td class="style9" >
                </td>
            <td class="style9" >
                </td>
        </tr>
        <tr>
            <td >
                </td>
            <td class="style18">
                </td>
            <td class="style15">
                </td>
            <td class="style11">
                </td>
            <td class="logo6">
                </td>
            <td >
                </td>
        </tr>
        <tr>
            <td >
                </td>
            <td class="style19">
                </td>
            <td class="style16" >
                </td>
            <td class="style12">
                </td>
            <td class="logo3" >
                </td>
            <td >
                </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="style20">
                &nbsp;</td>
            <td class="style17">
                &nbsp;</td>
            <td class="style13">
                &nbsp;</td>
            <td class="logo4">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="style21">
                &nbsp;</td>
            <td class="style8">
                &nbsp;</td>
            <td class="style14">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>


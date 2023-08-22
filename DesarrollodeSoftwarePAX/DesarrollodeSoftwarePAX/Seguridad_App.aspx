<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Seguridad_App.aspx.cs" Inherits="Seguridad_App" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link rel="shortcut icon" href="imagenes/fav.ico" />
    <style type="text/css">
        .style1
        {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="hojaPrincipal">
        <tr>
            <td>
                &nbsp;</td>
            <td class="style1" height="30px">

                          <asp:Label ID="Label1" runat="server" 
              Text = "<%$ Resources:resCorpusCFDIEs, lblTituloSeguridadApp %>" Font-Bold="True" 
              Font-Size="XX-Large" ForeColor="#395C6C" Font-Names="Arial" 
              style="color: #A5D10F; text-align: left;" Width="440px" Height="30px"></asp:Label>
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td class="Seguridad_App" rowspan="5" height="200px" width="480px">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td rowspan="4" align="center" style="text-align: center" 
                valign="top" width="490px" height="230px">

                                            <br />

              <asp:Label ID="Label212" runat="server" 
              Text = "<%$ Resources:resCorpusCFDIEs, lblTextoSeguridadApp %>" Font-Bold="False" 
              Font-Names="Arial" Font-Size="Small" ForeColor="#395C6C" 
              Font-Underline="False" 
                    style="text-align: justify; font-family: 'Century Gothic';" Width="400px"></asp:Label>

             

                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                </td>
            <td class="style2">
                </td>
            <td>
                </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td  height="50px" style="text-align: center">
                <asp:ImageButton ID="ImageButton1" runat="server" 
                    ImageUrl="~/Imagenes/1Contactanos.png" PostBackUrl="~/Contacto.aspx" />
                <br />
                <a href="Contacto.aspx" style="text-decoration:none">
            <asp:Label ID="Label8" runat="server" 
               Text="<%$ Resources:resCorpusCFDIEs, lblClickAqui %>" 
              Font-Bold="True" Font-Size="Small" ForeColor="#395C6C" 
                    style="font-family: 'Century Gothic'" Font-Underline="True"></asp:Label>
              </a>
                <br />
                <br />
            </td>
            <td class="style2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table> 
</asp:Content>


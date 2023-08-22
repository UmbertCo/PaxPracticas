<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WebTerminosCondiciones.aspx.cs" Inherits="WebTerminosCondiciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <link rel="shortcut icon" href="imagenes/fav.ico" />
    <style type="text/css">
        .ManoFondo
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div align="center">
<table class="hojaPrincipal">
    <tbody align="center" valign="top">
<td class ="ManoFondo">

    <table style="width: 965px; height: 233px" >
        <tr>
            <td align="left"  style="text-align: left">

                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                <asp:Label ID="lblTituloTerminosycondiciones" runat="server" Font-Bold="True" Font-Size="X-Large" 
                    ForeColor="#A5D10F" Text="<%$ Resources:resCorpusCFDIEs, lblTerminos %>" 
                    style="font-family: 'Century Gothic'; text-align: left;" Width="500px"></asp:Label>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td align="right" valign="top">
                <table style="height: 12px; width: 955px">
                    <tr>
                        <td align="left" class="style8">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
                        <td align="left" valign="top">
                            &nbsp;</td>
                        <td valign="top" align="left">
                <asp:Label ID="lblTxtTerminosyCondiciones" runat="server" ForeColor="#395C6C" Font-Bold="False" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblTerminosyCondiciones %>" 
                                style="font-family: 'Century Gothic'; font-size: small"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </td>
    </table>
    </div>

</asp:Content>


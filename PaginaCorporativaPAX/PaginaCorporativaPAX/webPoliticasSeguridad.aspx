<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="webPoliticasSeguridad.aspx.cs" Inherits="webPoliticasSeguridad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <link rel="shortcut icon" href="imagenes/fav.ico" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div align="center">
<table class="hojaPrincipal">
    <tbody align="center" valign="top">
<td class ="ManoFondo">

    <table style="width: 965px; height: 233px" >
        <tr>
            <td align="left">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblTituloPoliticasdePrivacidad" runat="server" Font-Bold="True" Font-Size="X-Large" 
                    ForeColor="#A5D10F" Text="<%$ Resources:resCorpusCFDIEs, lblPolitica %>" 
                    style="text-align: left; font-family: 'Century Gothic'" Width="400px"></asp:Label>
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
                <asp:Label ID="lbltxtPoliticasdePrivacidad" runat="server" ForeColor="#395C6C" Font-Bold="False" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblpoliticasdesc %>" 
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


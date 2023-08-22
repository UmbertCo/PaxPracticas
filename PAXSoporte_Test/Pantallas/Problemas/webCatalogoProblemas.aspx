<%@ Page Title="" Language="C#" MasterPageFile="~/Publica.master" AutoEventWireup="true" CodeFile="webCatalogoProblemas.aspx.cs" Inherits="Pantallas_Problemas_webCatalogoProblemas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css" >
    .contenedor
    {
        width:600px;
        text-align:left;
    }
    .contenedor select, input[type='text']
    {
     }
     .style6
    {
    }
    .style7
    {
        width: 364px;
    }
    .style8
    {
        width: 50px;
    }
    .style22
    {
        width: 100%;
    }
     .style24
    {
        width: 93px;
    }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
   

    <br />
    <asp:Label ID="lblenccatpro" runat="server" Font-Bold="True" Font-Size="Large" 
        Text="<%$ Resources:resCorpusCFDIEs, lblenccatpro %>"></asp:Label>

    <br />

    <br />

    <table style="width: 99%;">
        <tr>
            <td class="style7">
                <asp:Label ID="lblTipoProblema" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblTipoProblema %>"></asp:Label>
            </td>
            <td class="style8">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style7">
                <asp:DropDownList ID="ddlIncidencia" runat="server" Height="20px" Width="259px">
                </asp:DropDownList>
            </td>
            <td class="style8">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style7">
                &nbsp;</td>
            <td class="style8">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style7">
                <asp:Label ID="lblDescripcionin" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDescripcionin%>"></asp:Label>
            </td>
            <td class="style8">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style6" colspan="2">
                <asp:TextBox ID="txtDescripcionI" runat="server" Height="96px" TextMode="MultiLine" 
                    Width="451px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style7">
                &nbsp; </td>
            <td class="style8">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style7">
                <table class="style22" __designer:mapid="9cb">
                    <tr __designer:mapid="9cc">
                        <td class="style24" __designer:mapid="9cd">
                            <asp:Label runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lbldatosprueba %>" ID="lbldatosprueba"></asp:Label>
                        </td>
                        <td __designer:mapid="9cf">
                            <asp:FileUpload runat="server" Height="23px" Width="219px" ID="fuPruebas">
                            </asp:FileUpload>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="style8">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style7">
                &nbsp;</td>
            <td class="style8">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style7">
                <asp:Button ID="button1" runat="server" style="margin-left: 0px" Text="<%$ Resources:resCorpusCFDIEs, btnGuardarSop%>" 
                    Width="85px" onclick="button1_Click" CssClass="botonEstilo" OnClientClick="this.disabled=true;"  UseSubmitBehavior="false" />
            </td>
            <td class="style8">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style7">
                &nbsp;</td>
            <td class="style8">
                &nbsp;</td>
        </tr>
    </table>

</asp:Content>


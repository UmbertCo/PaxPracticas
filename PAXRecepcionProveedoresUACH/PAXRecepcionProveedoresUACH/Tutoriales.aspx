<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Tutoriales.aspx.cs" Inherits="Tutoriales" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

    <style type="text/css">
        .style2
        {
            width: 100px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="border: solid 1px #C0C0C0;" class="bodyMain">
        <tr>
            <td class="style2">
            </td>
            <td>
                <asp:Label ID="TextBox2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRegistroProveedores %>"
                    Font-Bold="true"></asp:Label>
                <br />
                <br />
                <asp:Literal runat="server" ID="MyYoutubeVideo" />
            </td>
            <td>
                <h4>
                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTutorialRegistroProveedores %>"></asp:Label>
                    
                    
                </h4>
            </td>
        </tr>
        <tr>
            <td class="style2">
            </td>
            <td>
                <asp:Label runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCatalogoProveedores %>" Font-Bold="true"></asp:Label>
                <br />
                <br />
                <asp:Literal runat="server" ID="MyYoutubeVideo1" />
            </td>
            <td>
                <h4>
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTutorialCatalogoProveedores %>"></asp:Label>
                    
                    
                </h4>
            </td>
        </tr>
        <tr>
            <td class="style2">
            </td>
            <td>
                <asp:Label ID="TextBox1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblValidacionFacturas %>"  Font-Bold="true"></asp:Label>
                <br />
                <br />
                <asp:Literal runat="server" ID="MyYoutubeVideo2" />
            </td>
            <td>
                <h4>
                  <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTutorialValidacionComprobantes %>"></asp:Label> 
                    
                   
                </h4>
            </td>
        </tr>
        <tr>
            <td class="style2">
            </td>
             <td>
                <asp:Label ID="TextBox4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblConsultaFacturas %>"  Font-Bold="true"></asp:Label>
                <br />
                <br />
                <asp:Literal runat="server" ID="MyYoutubeVideo4" />
            </td>
            <td>
                <h4>
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTutorialConsultaComprobantes %>"></asp:Label>
                
                  
                </h4>
            </td>
        </tr>
        <tr>
            <td class="style2">
            </td>
           <td>
                <asp:Label ID="TextBox3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRecuperaCuenta %>"  Font-Bold="true"></asp:Label>
                <br />
                <br />
                <asp:Literal runat="server" ID="MyYoutubeVideo3" />
            </td>
            <td>
                <h4>
                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTutorialCambioContraseña %>"></asp:Label>
                    
                </h4>
            </td>
        </tr>
    </table>
   

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OficinaVirtual.aspx.cs" Inherits="OficinaVirtual" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="shortcut icon" href="imagenes/fav.ico" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="hojaPrincipal">
        <tr>
            <td colspan="3" style="text-align: left">

                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                <asp:Label ID="lblTituloOficinaVirtual" runat="server" 
                Text = "<%$ Resources:resCorpusCFDIEs, lblTituloOficinaVirtual %>" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: left; font-size: xx-large; font-weight: 700;" 
                Height="40px" Width="400px"></asp:Label>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="3" style="text-align: center" width="830px">
               
                <asp:Label ID="lbl0180000PAX00" runat="server" 
               Text = "<%$ Resources:resCorpusCFDIEs, lblTelefonoPAX %>" Font-Bold="True" 
                Font-Size="XX-Large" ForeColor="#395C6C" Font-Names="Arial" Height="50px"></asp:Label>

                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: center">
                <asp:Label ID="lblChihuahua" runat="server" 
                Text = "Cd. Chihuahua, Chihuahua" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: center; font-size: small; font-weight: 700;" 
                Height="20px" Width="320px"></asp:Label>

                &nbsp;</td>
            <td>

                            <asp:Label ID="lblMexico" runat="server" 
                Text = "Cd.México, D.F." Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: center; font-size: small; font-weight: 700;" 
                Height="20px" Width="300px"></asp:Label>
                &nbsp;</td>
            <td>

                 <asp:Label ID="lblJuarez" runat="server" 
                Text = "Cd.Juárez, Chihuahua" Font-Bold="False" 
                Font-Size="Small" style="color: #A6D110; font-family: 'Century Gothic'; text-align: center; font-size: small; font-weight: 700;" 
                Height="20px" Width="320px"></asp:Label>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: center">                                                                                             

              <a href="http://maps.google.com.mx/maps/place?q=Parque+de+Innovaci%C3%B3n+y+Transferencia+de+Tecnolog%C3%ADa,+Heroico+Colegio+Militar+4709,+Bodegas+del+Estado,+Nombre+De+Dios,+31300+Chihuahua&amp;hl=es&amp;ie=UTF8&amp;ftid=0x86ea4376f60fc76b:0xab78fb18d989c09" 
                    target="_blank">
               <img alt="" src="imagenes/PIT2.png" style="height:170px; width:240px" border="0" /></a>&nbsp;</td>
            <td style="text-align: center">

                <a href="https://maps.google.com.mx/maps?ie=UTF8&cid=7941369580978594460&q=World+Trade+Center+M%C3%A9xico&iwloc=A&gl=MX&hl=es" 
                    target="_blank">
               <img alt="" src="imagenes/WTC.png" style="height:170px; width:240px" 
                    border="0" /></a>&nbsp;</td>
            <td style="text-align: center">
                <a href="https://maps.google.com.mx/maps/place?q=Plaza+nogales+cd+juarez&hl=es&ftid=0x86e758a0351ece4d:0xaed0cb3c46cdb25d" 
                    target="_blank">
               <img alt="" src="imagenes/Cd. Juarez.png" style="height:170px; width:240px" border="0" /></a>&nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: center">

                <br />

                <asp:Label ID="lblPIT2" runat="server" 
                Text = "Parque de Inovación y Transferencia de Tecnología(PIT2)" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: center; font-size: small; " 
                Height="40px" Width="320px"></asp:Label>

                <br />

                <asp:Label ID="lblTelPIT2" runat="server" 
                Text = "Tel.(614) 4 24 04 44" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: center; font-size: small; " 
                Height="20px" Width="320px"></asp:Label>
                &nbsp;</td>
            <td style="text-align: center">

                            <asp:Label ID="lblWTC" runat="server" 
                Text = "World Trade Center(WTC)" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: center; font-size: small; margin-top: 0px;" 
                Height="20px" Width="300px"></asp:Label>

                                <br />

                                <asp:Label ID="lblTelWTC" runat="server" 
                Text = "Tel.(55) 9000 26 90" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: center; font-size: small; " 
                Height="20px" Width="300px"></asp:Label>
                &nbsp;</td>
            <td style="text-align: center">

                                        <asp:Label ID="lblPlazaNogales" runat="server" 
                Text = "Plaza Nogales" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: center; font-size: small; margin-top: 0px;" 
                Height="20px" Width="300px"></asp:Label>

                                            <br />

                                            <asp:Label ID="lblTelPlazaNogales" runat="server" 
                Text = "Tel. (656) 616 16 44" Font-Bold="False" 
                Font-Size="Small" style="color: #395C6C; font-family: 'Century Gothic'; text-align: center; font-size: small; margin-top: 0px;" 
                Height="20px" Width="300px"></asp:Label>
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
    </table>
</asp:Content>


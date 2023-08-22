<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Contacto.aspx.cs" Inherits="Contacto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link rel="shortcut icon" href="imagenes/fav.ico" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table class="hojaPrincipal">
    <tr>
      <td valign="top">
          <table style="width: 914px; height: 97px">
              <tr>
                  <td align="left">
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      <asp:Label ID="Label5" runat="server" 
                          Text="<%$ Resources:resCorpusCFDIEs, lblContacto %>" Font-Bold="True" 
                          Font-Size="XX-Large" ForeColor="#A5D10F" 
                          style="font-family: 'Century Gothic'"></asp:Label>
                      <br />
                  </td>
                  <td>
                      &nbsp;</td>
              </tr>
              <tr>
                  <td align="left">
                      <%--<asp:Label ID="Label6" runat="server" 
                          Text="<%$ Resources:resCorpusCFDIEs, lblContacto1 %>" Font-Bold="True" 
                          Font-Size="Medium" ForeColor="#395C6C" Visible="False"></asp:Label>--%>
                  </td>
                  <td>
                      &nbsp;</td>
              </tr>
              <tr>
                  <td align="left" >
                    <%--  <asp:Label ID="Label7" runat="server" 
                          Text="<%$ Resources:resCorpusCFDIEs, lblContacto2 %>" Font-Bold="True" 
                          Font-Size="Medium" ForeColor="#395C6C" Visible="False"></asp:Label>--%>
                  </td>
                  <td >
                      </td>
              </tr>
              <tr>
                  <td valign="top" align="left">
                      <table style="width: 600px" align="right" >
                          <tr>
                              <td  class="correo" align="right" width="55px">
                       
                                  <asp:Image ID="Image3" runat="server" Height="50px" 
                                      ImageUrl="~/Imagenes/contactomail.png" Width="60px" />
                              </td>
                              <td align="left" class="style8" >
                       
                                  <asp:Label ID="Label8" runat="server" 
                                      Text="<%$ Resources:resCorpusCFDIEs, lblContacto3 %>" Font-Bold="True" 
                                      ForeColor="#395C6C" Width="400px" 
                                      style="font-family: 'Century Gothic'; font-size: medium"></asp:Label>
                       
                                 </td>
                          </tr>
                          <tr>
                              <td  colspan="2" align="left" >
                                 
                                 
                                  <asp:Label ID="Label9" runat="server" 
                                      Text="<%$ Resources:resCorpusCFDIEs, lblContacto5 %>" ForeColor="#395C6C" 
                                      Width="500px" style="font-family: 'Century Gothic'; font-size: small"></asp:Label>
                              </td>
                          </tr>
                          <tr>
                              <td class="style10" colspan="2" >
                                      
                          
                                      
                                  <a href="mailto:info@paxfacturacion.com?subject=Servicio al cliente&amp;body=Envíe un correo electrónico al servicio al cliente para obtener asistencia." 
                                      style="color: #395C6C; text-decoration: underline;">
                                      <asp:Label ID="lblCorreo" runat="server" 
                                      Text="<%$ Resources:resCorpusCFDIEs, lblContacto9 %>" 
                                      
                                      style="font-family: 'Century Gothic'; font-size: small; text-align: right;" 
                                      Width="510px" Font-Underline="True"></asp:Label></a>

                              </td>
                          </tr>
                          <tr align="left">
                              <td  align="left" width="55px" >
                       
                                  <asp:Image ID="Image4" runat="server" Height="50px" 
                                      ImageUrl="~/Imagenes/telefono.jpg" Width="60px" />
                              </td>
                              <td align="left" class="style8" >
                       
                                  <asp:Label ID="Label10" runat="server" 
                                      Text="<%$ Resources:resCorpusCFDIEs, lblContacto4 %>" Font-Bold="True" 
                                      ForeColor="#395C6C" 
                                      style="font-family: 'Century Gothic'; font-size: medium"></asp:Label>
                       
                                 </td>
                          </tr>
                          <tr>
                              <td colspan="2" align="left" class="style9" >
                                  
                                  <asp:Label ID="Label11" runat="server" 
                                      Text="<%$ Resources:resCorpusCFDIEs, lblContacto6 %>" ForeColor="#395C6C" 
                                      Width="500px" style="font-family: 'Century Gothic'; font-size: small"></asp:Label>
                              </td>
                          </tr>
                          <tr>
                              <td  colspan="2" align="justify" class="style8" >
                                 <asp:Label 
                                      ID="Label12" runat="server" 
                                      Text="<%$ Resources:resCorpusCFDIEs, lblTelefono %>" Font-Bold="True" 
                                      Font-Size="Medium" ForeColor="#395C6C" 
                                      style="font-family: 'Century Gothic'; text-align: right;" Width="510px"></asp:Label>
                                  <br />
                                 
                             <asp:Label ID="lblOficina" runat="server" 
                            Text = "<%$ Resources:resCorpusCFDIEs, lblTelefonoOficina %>" Font-Bold="True" 
                            Font-Names="Arial" Font-Size="Medium" ForeColor="#395C6C" 
                                      style="font-family: 'Century Gothic'; text-align: right;" Width="510px"></asp:Label>
                              </td>
                          </tr>
                          <tr align="left">
                              <td  align="right" width="55px">
                       
                                  <asp:Image ID="Image5" runat="server" Height="50px" 
                                      ImageUrl="~/Imagenes/en-@-icon_30-2672.jpg" Width="60px" />
                              </td>
                              <td  align="left" class="style8">
                       
                                  <asp:Label ID="Label13" runat="server" 
                                      Text="<%$ Resources:resCorpusCFDIEs, lblContacto7 %>" Font-Bold="True" 
                                      ForeColor="#395C6C" 
                                      style="font-family: 'Century Gothic'; font-size: medium"></asp:Label>
                       
                              </td>
                          </tr>
                          <tr>
                              <td  align="left" colspan="2" class="style10">
                                 
                                  <asp:Label ID="Label14" runat="server" 
                                      Text="<%$ Resources:resCorpusCFDIEs, lblContacto8 %>" ForeColor="#395C6C" 
                                      Width="500px" style="font-family: 'Century Gothic'; font-size: small"></asp:Label>
                              </td>
                          </tr>
                          <tr>
                              <td  align="right" colspan="2" width="510px">
                              <!-- BoldChat Live Chat Button HTML v3.00 (Type=Web,ChatButton=My Chat Button,ChatWindow=My Chat Window,Website=- None -,Department=- None -) -->
                                <div style="text-align: right; white-space: nowrap;">
                                 <a href="https://livechat.boldchat.com/aid/984004916369670794/bc.chat?cwdid=6063724607315866159" 
                                 target="_blank" onclick="window.open((window.pageViewer && pageViewer.link || function(link){return link;})(this.href + (this.href.indexOf('?')>=0 ? '&' : '?') + 'url=' + escape(document.location.href)), 'Chat984004916369670794', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=640,height=480');return false;">
                                  <div align="right" style="width: 537px"><script language="JavaScript" type="text/javascript">                                                         document.write(unescape('%3Cimg alt="Live chat by BoldChat" src="' + (('https:' == document.location.protocol) ? 'https:' : 'http:') + '//cbi.boldchat.com/aid/984004916369670794/bc.cbi?cbdid=8074818285341542082" border="0" /%3E')); </script></div>
                                 </a>
                                </div>
                                <!-- /BoldChat Live Chat Button HTML v3.00 -->
                                  <%--<asp:HyperLink ID="HyperLink4" runat="server" 
                          Text="<%$ Resources:resCorpusCFDIEs, lblContacto10 %>" ForeColor="#395C6C" 
                          NavigateUrl="http://ventas.paxfacturacion.com.mx"></asp:HyperLink>--%>
                              </td>
                          </tr>
                      </table>
                  </td>
                  <td valign="baseline" align="left">
                      &nbsp;<asp:Image ID="Image2" runat="server" Height="306px" 
                          ImageUrl="~/Imagenes/MonaContacto.jpg" Width="254px" />
                  </td>
              </tr>
              </table>
        </td>
    </tr>
  </table> 

</asp:Content>


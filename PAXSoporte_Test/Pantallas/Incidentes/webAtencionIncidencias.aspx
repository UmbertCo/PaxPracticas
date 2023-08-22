<%@ Page Title="" Language="C#" MasterPageFile="~/Publica.master" AutoEventWireup="true" CodeFile="webAtencionIncidencias.aspx.cs" Inherits="Pantallas_Incidentes_webAtencionIncidenciast" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
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
     .style11
    {
        width: 329px;
        height: 24px;
    }
    .style13
    {
        width: 329px;
    }
    .style14
    {
        height: 433px;
    }
     .style15
    {
        width: 96px;
    }
    .style16
    {
        width: 121px;
    }
     .style17
    {
        width: 83%;
    }
    .style18
    {
        width: 119px;
    }
     .style19
    {
        width: 120px;
    }
     .style20
    {
        width: 155px;
    }
     .style22
    {
        width: 100%;
    }
     .style24
    {
        width: 93px;
    }
     .style25
    {
        width: 182px;
    }
     .style26
    {
        width: 237px;
    }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <br />
    <asp:Label ID="lblencatninc" runat="server" Font-Bold="True" Font-Size="Large" 
        Text="<%$ Resources:resCorpusCFDIEs, lblencatninc %>"></asp:Label>

    <br />

    <table style="width:100%; height: 599px;">
        <tr>
            <td>
                                                                                            <table 
                    style="width:100%;">
                    <tr>
                        <td align="right" valign="top" class="style16">
                                        <asp:Label runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblTicketText %>" ID="lblTicketText"></asp:Label>

                                    </td>
                        <td class="style15">
                            <asp:Label runat="server" Text="Label" Font-Bold="True" Font-Size="Medium" 
                                ID="lblTicket"></asp:Label>
                        </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="style16">
                            <asp:Label runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblusuariorep %>" ID="lblusuariorep2"></asp:Label>
                        </td>
                        <td class="style15">
                            <asp:TextBox runat="server" ReadOnly="True" Height="20px" Width="255px" 
                                ID="txtUsuario"></asp:TextBox>
                        </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="style16">
                            <asp:Label runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblfechareg %>" 
                                ID="lblfechareg"></asp:Label>
                        </td>
                        <td class="style15">
                            <asp:Label ID="txtFecha" runat="server"></asp:Label>
                        </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="style16">
                            <asp:Label runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblfechater %>" 
                                ID="lblfechater"></asp:Label>
                        </td>
                        <td class="style15">
                            <asp:Label ID="txtfechater" runat="server"></asp:Label>
                        </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="style16">
                            <asp:Label runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblDescripcionI %>" ID="lblDescripcion1"></asp:Label>
                        </td>
                        <td class="style15">
                            <asp:TextBox runat="server" TextMode="MultiLine" ReadOnly="True" Height="75px" 
                                Width="394px" ID="txtDescripcion" style="margin-bottom: 0px"></asp:TextBox>
                        </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="style16">
                             </td>
                        <td class="style15">
                             </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" colspan="2">
                            <asp:Label runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblReasignaDesc %>" 
                                ID="lblleyenda" Font-Bold="False" Font-Italic="True"></asp:Label>
                        </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="style16">
                             </td>
                        <td class="style15">
                             </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="style16">
                            <asp:Label runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblIncidenciaSop %>" ID="lbltipoinc"></asp:Label>
                        </td>
                        <td class="style15" valign="top">
                             <asp:DropDownList runat="server" Height="23px" TabIndex="5" 
                                Width="280px" ID="ddlIncidencia">
                            </asp:DropDownList>
                          
                            <asp:Button runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, btnReasignar %>" Width="118px" 
                                ID="btnReasignar0" OnClick="btnReasignar_Click"></asp:Button>
                        </td>
                        <td>
                             </td>
                    </tr>
                    </table>
                                                                            
                 </td>
            <td>
                 </td>
        </tr>
        <tr>
            <td class="style14">
                <cc1:TabContainer ID="tbInformacionIncidencias" runat="server" 
                    ActiveTabIndex="3" Height="477px" Width="524px">
                
                    <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Atención">
                        <HeaderTemplate>
                            Atención
                        </HeaderTemplate>
                        <ContentTemplate>
                            
                                                                                   
                            <table style="width:100%; height: 242px;">
                                
                                <caption>
                                                                   
                                    <caption>
                                                                           
                                        <tr>
                                            <td valign="top">
                                                 <table class="style17">
                                                    <tr>
                                                        <td align="left" class="style20">
                                                            <asp:Label ID="lblfecharatn" runat="server" 
                                                                Text="<%$ Resources:resCorpusCFDIEs, lblfecharatn %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="txtfechaatn" runat="server">---</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <tr>
                                                <td>
                                                     </td>
                                                <tr>
                                                    <td>
                                                         <asp:Label ID="lblCorreoI" runat="server" 
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblCorreoI %>"></asp:Label>
                                                                                           
                                                    </td>
                                                    <caption>
                                                                                           
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtCorreo" runat="server" Height="93px" 
                                                                    style="margin-bottom: 0px" TextMode="MultiLine" Width="394px"></asp:TextBox>
                                                                                         </td>
                                                            <caption>
                                                                                                   
                                                                <tr>
                                                                    <td>
                                                                                                              <asp:Button ID="btnEnviaCorreo" 
                                                                            runat="server" Height="26px" OnClick="btnEnviaCorreo_Click" 
                                                                            Text="<%$ Resources:resCorpusCFDIEs, btnEnviaCorreo %>" Width="213px" CssClass="botonEstilo" />
                                                                    </td>
                                                                    <caption>
                                                                                                       
                                                                    </caption>
                                                                </tr>
                                                                <caption>
                                                                                                   
                                                                    <caption>
                                                                                                           
                                                                        <tr>
                                                                            <td>
                                                                                                                    
                                                                            </td>
                                                                            <caption>
                                                                                                                   
                                                                                <tr>
                                                                                    <td>
                                                                                                                                 </td>
                                                                                    <caption>
                                                                                                                           
                                                                                        <tr>
                                                                                            <td>
                                                                                                                                         </td>
                                                                                            <caption>
                                                                                                                               
                                                                                            </caption>
                                                                                        </tr>
                                                                                    </caption>
                                                                                </tr>
                                                                            </caption>
                                                                        </tr>
                                                                    </caption>
                                                                </caption>
                                                            </caption>
                                                        </tr>
                                                        <caption>
                                                            <caption>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <caption>
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <caption>
                                                                                <tr>
                                                                                    <td>
                                                                                    </td>
                                                                                    <caption>
                                                                                    </caption>
                                                                                </tr>
                                                                            </caption>
                                                                        </tr>
                                                                    </caption>
                                                                </tr>
                                                            </caption>
                                                        </caption>
                                                    </caption>
                                                </tr>
                                            </tr>
                                        </tr>
                                    </caption>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </caption>
                                </tr>
                                </tr>
                                </tr>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </tr>
                                </tr>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </tr>
                                </tr>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                            </table>
                            
                        
                            
                        </ContentTemplate>
                        
                    </cc1:TabPanel>
                   
                    <cc1:TabPanel ID="TabPanel9" runat="server" HeaderText ="SAT">
                   <HeaderTemplate>
                   SAT
                   </HeaderTemplate>
                   <ContentTemplate>
                   
                       <table style="width:100%;">
                           <tr>
                               <td>
                                   <table class="style22">
                                       <tr>
                                           <td class="style20">
                                               <asp:Label ID="lblfechaautnot" runat="server" 
                                                   Text="<%$ Resources:resCorpusCFDIEs, lblfechaautnot %>"></asp:Label>
                                           </td>
                                           <td>
                                               <asp:Label ID="txtfechanoti" runat="server">---</asp:Label>
                                           </td>
                                       </tr>
                                   </table>
                               </td>
                               <td>
                                    </td>
                               <td>
                                    </td>
                               <tr>
                                   <td>
                                   </td>
                                   <td>
                                   </td>
                                   <td>
                                   </td>
                                   <tr>
                                       <td>
                                           <asp:Label ID="lbldescSAT" runat="server" 
                                               Text="<%$ Resources:resCorpusCFDIEs, lbldescSAT %>"></asp:Label>
                                       </td>
                                       <td>
                                       </td>
                                       <td>
                                       </td>
                                       <tr>
                                           <td>
                                               <asp:TextBox ID="tbNotificacion" runat="server" Height="93px" 
                                                   TextMode="MultiLine" Width="394px"></asp:TextBox>
                                           </td>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td>
                                               <asp:Label ID="lblplanaccion" runat="server" 
                                                   Text="<%$ Resources:resCorpusCFDIEs, lblplanaccion %>"></asp:Label>
                                           </td>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td>
                                               <table class="style22">
                                                   <tr>
                                                       <td class="style26">
                                                           <asp:FileUpload ID="fuSat" runat="server" Height="23px" Width="219px" />
                                                       </td>
                                                       <td>
                                                           <asp:Label ID="lblarchivo" runat="server"></asp:Label>
                                                       </td>
                                                       <td>
                                                           <asp:CheckBox ID="cbAutorizaSAT" runat="server" AutoPostBack="True" 
                                                               OnCheckedChanged="cbAutorizaSAT_CheckedChanged" Text="Autorizado" />
                                                       </td>
                                                   </tr>
                                               </table>
                                           </td>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td>
                                               <table class="style17">
                                                   <tr>
                                                       <td class="style19">
                                                           <asp:Label ID="lblfechanot" runat="server" 
                                                               Text="<%$ Resources:resCorpusCFDIEs, lblfechanot %>"></asp:Label>
                                                       </td>
                                                       <td>
                                                           <asp:Label ID="txtfechanot" runat="server">---</asp:Label>
                                                       </td>
                                                   </tr>
                                               </table>
                                           </td>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td>
                                               <asp:Label ID="lblautorizador" runat="server" 
                                                   Text="<%$ Resources:resCorpusCFDIEs, lblautorizador %>"></asp:Label>
                                           </td>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td>
                                               <asp:TextBox ID="tbAutorizador" runat="server" Height="18px" Width="394px"></asp:TextBox>
                                           </td>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td>
                                               <asp:Button ID="btnSAT" runat="server" OnClick="btnSAT_Click" 
                                                   Text="<%$ Resources:resCorpusCFDIEs, btnSAT %>" Width="126px" 
                                                   CssClass="botonEstilo" Enabled="False" />
                                               <asp:Button ID="btnAutorizaSat" runat="server" Enabled="False" 
                                                   OnClick="btnAutorizaSat_Click" 
                                                   Text="<%$ Resources:resCorpusCFDIEs, btnAutorizaSat %>" Width="126px" 
                                                   CssClass="botonEstilo" />
                                           </td>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                           <td>
                                           </td>
                                       </tr>
                                   </tr>
                               </tr>
                           </tr>
                       </table>
                   
                   </ContentTemplate>

                    </cc1:TabPanel>

                    <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Solución Soporte">
                        <HeaderTemplate>
                            Solución Soporte
                        </HeaderTemplate>
                        <ContentTemplate>
                            
                                                                                   
                            <table style="width:100%;">
                                
                                <caption>
                                                                   
                                    <caption>
                                                                           
                                        <tr>
                                            <td>
                                                <table class="style17">
                                                    <tr>
                                                        <td class="style25">
                                                            <asp:Label ID="lblfechasop" runat="server" 
                                                                Text="<%$ Resources:resCorpusCFDIEs, lblfechasop %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="txtfechasop" runat="server">---</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <tr>
                                                <td>
                                                     </td>
                                                <tr>
                                                    <td>
                                                         <asp:Label ID="lblSolucionSoporte" runat="server" 
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblSolucionSoporte %>"></asp:Label>
                                                                                           
                                                    </td>
                                                    <caption>
                                                                                           
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtSolucionSoporte" runat="server" Height="93px" TabIndex="2" 
                                                                    TextMode="MultiLine" Width="394px"></asp:TextBox>
                                                                                              </td>
                                                            <caption>
                                                                                                   
                                                                <tr>
                                                                    <td>
                                                                                                                   <asp:Button ID="btnGuardarS0" 
                                                                            runat="server" Height="26px" OnClick="btnGuardar_Click" TabIndex="3" 
                                                                            Text="<%$ Resources:resCorpusCFDIEs, btnGuardarS %>" Width="187px" CssClass="botonGrande" Enabled="False" />
                                                                    </td>
                                                                    <caption>
                                                                                                       
                                                                    </caption>
                                                                </tr>
                                                                <caption>
                                                                                                   
                                                                    <caption>
                                                                                                           
                                                                        <tr>
                                                                            <td valign="top">
                                                                                                                    
                                                                            </td>
                                                                            <caption>
                                                                                                                   
                                                                                <tr>
                                                                                    <td>
                                                                                                                                 </td>
                                                                                    <caption>
                                                                                                                           
                                                                                        <tr>
                                                                                            <td>
                                                                                                                                         </td>
                                                                                            <caption>
                                                                                                                               
                                                                                            </caption>
                                                                                        </tr>
                                                                                        <caption>
                                                                                                                           
                                                                                            <caption>
                                                                                                                                   
                                                                                                <tr>
                                                                                                    <td>
                                                                                                                                                      
                                                                                                                                           
                                                                                                    </td>
                                                                                                    <caption>
                                                                                                                                           
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                                                         </td>
                                                                                                            <caption>
                                                                                                                                                   
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                                                                 </td>
                                                                                                                    <caption>
                                                                                                                                                       
                                                                                                                    </caption>
                                                                                                                </tr>
                                                                                                            </caption>
                                                                                                        </tr>
                                                                                                    </caption>
                                                                                                    <caption>
                                                                                                    </caption>
                                                                                                </tr>
                                                                                            </caption>
                                                                                        </caption>
                                                                                    </caption>
                                                                                </tr>
                                                                            </caption>
                                                                        </tr>
                                                                    </caption>
                                                                </caption>
                                                            </caption>
                                                        </tr>
                                                    </caption>
                                                </tr>
                                            </tr>
                                        </tr>
                                    </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </tr>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </caption>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </caption>
                            </table>
                            
                        
                            
                        </ContentTemplate>
                        
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="Pruebas">
                        <ContentTemplate>
                            
                                                                                   <table style="width: 97%; height: 187px;">
                                
                                <caption>
                                                               
                                    <caption>
                                                                           
                                        <tr>
                                            <td class="style13">
                                                <table class="style17">
                                                    <tr>
                                                        <td class="style18">
                                                            <asp:Label ID="lblfechaprue" runat="server" 
                                                                Text="<%$ Resources:resCorpusCFDIEs, lblfechaprue %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="txtfechaprue" runat="server">---</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <tr>
                                                <td class="style13">
                                                     </td>
                                                <tr>
                                                    <td class="style13">
                                                         <asp:Label ID="lblSolucionSoporte0" runat="server" 
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblSolucionSoporte %>"></asp:Label>
                                                                                           
                                                    </td>
                                                    <caption>
                                                                                           
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtSolucionSoporteP" runat="server" Height="93px" 
                                                                    ReadOnly="True" TabIndex="2" TextMode="MultiLine" Width="394px"></asp:TextBox>
                                                                                 </td>
                                                            <caption>
                                                                                                   
                                                                <caption>
                                                                                                   
                                                                </caption>
                                                                <caption>
                                                                                                   
                                                                    <caption>
                                                                                                           
                                                                        <caption>
                                                                                                               
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblSolucionPruebas0" runat="server" 
                                                                                        Text="<%$ Resources:resCorpusCFDIEs, lblSolucionPruebas %>"></asp:Label>
                                                                                                                          
                                                                                    <asp:CheckBox ID="cbAprobado" runat="server" Text="<%$ Resources:resCorpusCFDIEs, cbAprobado %>" />
                                                                                                        </td>
                                                                                <caption>
                                                                                                                       
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtSolucionPruebas" runat="server" Height="93px" TabIndex="2" 
                                                                                                TextMode="MultiLine" Width="394px"></asp:TextBox>
                                                                                                                         </td>
                                                                                        <caption>
                                                                                                                           
                                                                                        </caption>
                                                                                    </tr>
                                                                                    <caption>
                                                                                                                       
                                                                                        <caption>
                                                                                                                               
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table class="style22">
                                                                                                        <tr>
                                                                                                            <td class="style24">
                                                                                                                <asp:Label ID="lbldatosprueba" runat="server" 
                                                                                                                    Text="<%$ Resources:resCorpusCFDIEs, lbldatosprueba %>"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:FileUpload ID="fuPruebas" runat="server" Height="23px" Width="219px" />
                                                                                                                <asp:Label ID="lblpruebaa" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                                <tr>
                                                                                                    <td class="style11">
                                                                                                                                             
                                                                                                        <asp:Button ID="btnGuardarSolucionPrueba" runat="server" Height="26px" 
                                                                                                            OnClick="btnGuardarSolucionPrueba_Click" TabIndex="3" 
                                                                                                            Text="<%$ Resources:resCorpusCFDIEs, btnGuardarSolucionPrueba %>" 
                                                                                                            Width="170px" CssClass="botonGrande" Enabled="False" />
                                                                                                    </td>
                                                                                                    <caption>
                                                                                                                                           
                                                                                                        <caption>
                                                                                                                                               
                                                                                                            <caption>
                                                                                                                                               
                                                                                                            </caption>
                                                                                                        </caption>
                                                                                                    </caption>
                                                                                                </tr>
                                                                                            </tr>
                                                                                        </caption>
                                                                                    </caption>
                                                                                </caption>
                                                                            </tr>
                                                                        </caption>
                                                                    </caption>
                                                                </caption>
                                                            </caption>
                                                        </tr>
                                                    </caption>
                                                </tr>
                                            </tr>
                                        </tr>
                                    </caption>
                                </caption>
                                                                                       </caption>
                                                                                       </caption>
                                                                                       </caption>
                                                                                       </tr>
                                                                                       </caption>
                                                                                       </tr>
                                                                                       </tr>
                                                                                       </tr>
                                                                                       </caption>
                                                                                       </caption>
                                                                                       </tr>
                                                                                       </caption>
                                                                                       </caption>
                                                                                       </caption>
                                                                                       </caption>
                                                                                       </tr>
                                                                                       </caption>
                                                                                       </tr>
                                                                                       </tr>
                                                                                       </tr>
                                                                                       </caption>
                                                                                       </caption>
                                                                                       </tr>
                                                                                       </caption>
                                                                                       </caption>
                                                                                       </caption>
                                                                                       </caption>
                                                                                       </tr>
                                                                                       </caption>
                                                                                       </tr>
                                                                                       </tr>
                                                                                       </tr>
                                                                                       </caption>
                                                                                       </caption>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </tr>
                                </tr>
                                </caption>
                                </caption>
                                </tr>
                                <caption>
                                                                   
                                </caption>
                                                                                       <caption>
                                                                                           <caption>
                                                                                               <caption>
                                                                                                   <caption>
                                                                                                   </caption>
                                                                                               </caption>
                                                                                           </caption>
                                                                                       </caption>
                                                                                       </caption>
                                </caption>
                                </tr>
                                </tr>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </tr>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </caption>
                                <caption>
                                                                   
                                    <caption>
                                                                           
                                        <caption>
                                                                               
                                            <caption>
                                                                                   
                                                <caption>
                                                                                   
                                                </caption>
                                            </caption>
                                        </caption>
                                    </caption>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </caption>
                                </caption>
                                <caption>
                                                                   
                                    <caption>
                                                                           
                                        <caption>
                                                                               
                                            <caption>
                                                                                   
                                                <caption>
                                                                                   
                                                </caption>
                                            </caption>
                                        </caption>
                                    </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                <caption>
                                                               
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                            </table>
                            
                        
                            
                        </ContentTemplate>
                        
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel5" runat="server" HeaderText="Solución Usuario">
                        <HeaderTemplate>
                            Solución Usuario
                        </HeaderTemplate>
                        <ContentTemplate>
                            
                                                          
                            <table style="width:100%;">
                                
                                <caption>
                                                                   
                                    <caption>
                                                                           
                                        <tr>
                                            <td>
                                                <table class="style17">
                                                    <tr>
                                                        <td class="style25">
                                                            <asp:Label ID="lblfechausu" runat="server" 
                                                                Text="<%$ Resources:resCorpusCFDIEs, lblfechausu %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="txtfechausu" runat="server">---</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <tr>
                                                <td>
                                                     </td>
                                                <tr>
                                                    <td>
                                                         <asp:Label ID="lblSolucionUsuario" runat="server" 
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblSolucionUsuario %>"></asp:Label>
                                                                                           
                                                    </td>
                                                    <caption>
                                                                                           
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtSolucionUsuario" runat="server" Height="75px" TabIndex="1" 
                                                                    TextMode="MultiLine" Width="394px"></asp:TextBox>
                                                                                         </td>
                                                            <caption>
                                                                                                   
                                                                <tr>
                                                                    <td>
                                                                                       <asp:Button ID="btnGuardarSolUsuario0" runat="server" 
                                                                            Height="26px" OnClick="btnGuardarSolUsuario_Click" TabIndex="3" 
                                                                            Text="<%$ Resources:resCorpusCFDIEs, btnGuardarI %>" Width="170px" 
                                                                                           CssClass="botonGrande" Enabled="False" />
                                                                             <asp:Button ID="btnTerminar" runat="server" Enabled="False" 
                                                                            OnClick="btnTerminar_Click" TabIndex="3" 
                                                                            Text="<%$ Resources:resCorpusCFDIEs, btnTerminar %>" Width="134px" 
                                                                            Height="25px" CssClass="botonGrande" />
                                                                               </td>
                                                                    <caption>
                                                                                                       
                                                                    </caption>
                                                                    <caption>
                                                                                                       
                                                                        <caption>
                                                                                                               
                                                                            <tr>
                                                                                <td>
                                                                                                                        
                                                                                </td>
                                                                                <caption>
                                                                                                                       
                                                                                    <tr>
                                                                                        <td>
                                                                                                                                     </td>
                                                                                        <caption>
                                                                                                                               
                                                                                            <tr>
                                                                                                <td>
                                                                                                                                             </td>
                                                                                                <caption>
                                                                                                                                   
                                                                                                </caption>
                                                                                            </tr>
                                                                                            <caption>
                                                                                                                               
                                                                                                <caption>
                                                                                                                                       
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                                                                                      
                                                                                                        </td>
                                                                                                        <caption>
                                                                                                                                               
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                                                             </td>
                                                                                                                <caption>
                                                                                                                                                       
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                                                                     </td>
                                                                                                                        <caption>
                                                                                                                                                           
                                                                                                                        </caption>
                                                                                                                    </tr>
                                                                                                                </caption>
                                                                                                            </tr>
                                                                                                        </caption>
                                                                                                    </tr>
                                                                                                </caption>
                                                                                            </caption>
                                                                                        </caption>
                                                                                    </tr>
                                                                                </caption>
                                                                            </tr>
                                                                        </caption>
                                                                    </caption>
                                                                    <caption>
                                                                    </caption>
                                                                </tr>
                                                            </caption>
                                                        </tr>
                                                    </caption>
                                                </tr>
                                            </tr>
                                        </tr>
                                    </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </tr>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </tr>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </tr>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </caption>
                                </caption>
                                </caption>
                                </tr>
                                </caption>
                                </tr>
                                </tr>
                                </tr>
                            </table>
                            
                        
                            
                        </ContentTemplate>
                        




                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
            <td class="style14">
                </td>
        </tr>
        </table>
</asp:Content>


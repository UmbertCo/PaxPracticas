<%@ Page Title="" Language="C#" MasterPageFile="~/Publica.master" AutoEventWireup="true" CodeFile="webAtencionProblemas.aspx.cs" Inherits="Pantallas_Problemas_webAtencionProblemas" %>
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
        .style1
        {
            width: 237px;
        }
        .style2
        {
            width: 76px;
        }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <br />
    <asp:Label ID="lblencatnpro" runat="server" Font-Bold="True" Font-Size="Large" 
        Text="<%$ Resources:resCorpusCFDIEs, lblencatnpro %>"></asp:Label>

    <table style="width:100%; height: 816px;">
        <tr>
            <td>
                                                                                            <table 
                    style="width:100%;">
                    <tr>
                        <td align="right" valign="top">
                                        <asp:Label runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblTicketText %>" ID="lblTicketText"></asp:Label>

                                    </td>
                        <td>
                            <asp:Label runat="server" Text="Label" Font-Bold="True" Font-Size="Medium" 
                                ID="lblTicket"></asp:Label>
                        </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblusuariorep %>" ID="lblusuariorep2"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox runat="server" ReadOnly="True" Height="20px" Width="255px" 
                                ID="txtUsuario"></asp:TextBox>
                        </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblfechareg %>" 
                                ID="lblfechareg"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="txtFecha" runat="server"></asp:Label>
                        </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblfechater %>" 
                                ID="lblfechater"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="txtfechater" runat="server"></asp:Label>
                        </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblDescripcionI %>" ID="lblDescripcion1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" TextMode="MultiLine" ReadOnly="True" Height="75px" 
                                Width="394px" ID="txtDescripcion" style="margin-bottom: 0px"></asp:TextBox>
                        </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                             </td>
                        <td>
                             </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" colspan="2">
                            <asp:Label runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblreastickpro %>" 
                                ID="lblleyenda" Font-Bold="False" Font-Italic="True"></asp:Label>
                        </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" >
                             </td>
                        <td>
                             </td>
                        <td>
                             </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblIncidenciaSop %>" ID="lbltipoinc"></asp:Label>
                        </td>
                        <td valign="top">
                             <asp:DropDownList runat="server" Height="23px" TabIndex="5" 
                                Width="280px" ID="ddlIncidencia">
                            </asp:DropDownList>
                          
                              <asp:Button runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, btnReasignar %>" Width="118px" 
                                ID="btnReasignar" OnClick="btnReasignar_Click" CssClass="botonEstilo"></asp:Button>
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
            <td>
                <cc1:TabContainer ID="tbInformacionIncidencias" runat="server" 
                    ActiveTabIndex="3" Height="475px" Width="526px">

                   <cc1:TabPanel ID="TabPanel10" runat="server" HeaderText="Atención">
                   <HeaderTemplate>Atención</HeaderTemplate>
                   <ContentTemplate><table><tr><td><table><tr><td align="left"><asp:Label ID="lblfecharatn" runat="server" 
                                                        Text="<%$ Resources:resCorpusCFDIEs, lblfecharatn %>"></asp:Label></td><td><asp:Label ID="txtfechaatn" runat="server">---</asp:Label></td></tr></table></td>
                       <tr>
                           <td>
                           </td>
                           <tr>
                               <td>
                                   <asp:Label ID="lblCorreoP" runat="server" 
                                       Text="<%$ Resources:resCorpusCFDIEs, lblCorreoP %>"></asp:Label>
                               </td>
                               <tr>
                                   <td>
                                       <asp:TextBox ID="txtCorreo" runat="server" Height="93px" 
                                           style="margin-bottom: 0px" TextMode="MultiLine" Width="394px"></asp:TextBox>
                                   </td>
                               </tr>
                               <caption>
                                   <tr>
                                       <td>
                                           <asp:Button ID="btnEnviaCorreo" runat="server" CssClass="botonEstilo" 
                                               Height="26px" OnClick="btnEnviaCorreo_Click" 
                                               Text="<%$ Resources:resCorpusCFDIEs, btnEnviaCorreo %>" Width="204px" />
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
                                                           <caption>
                                                           </caption>
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
                                           </tr>
                                       </caption>
                                   </caption>
                               </caption>
                           </tr>
                           <caption>
                           </caption>
                           </caption>
                           </caption>
                       </tr>
                       </caption>
                       </tr></caption></caption>
                       <caption>
                       </caption>
                       </caption></tr></caption></tr></tr></tr></caption></caption></caption></tr></caption></tr>
                       </tr>
                       </tr>
                       </caption></tr>
                       </caption>
                       </caption>
                       </caption>
                       </tr>
                       </caption>
                       </tr></caption></caption>
                       </caption>
                       </tr></caption></table></ContentTemplate>
                   </cc1:TabPanel>

                    <cc1:TabPanel ID="TabPanel9" runat="server" HeaderText ="SAT">
                   <HeaderTemplate>SAT</HeaderTemplate>
                   <ContentTemplate><table style="width:100%;"><tr><td><table><tr><td><asp:Label ID="lblfechaautnot" runat="server" 
                                                   Text="<%$ Resources:resCorpusCFDIEs, lblfechaautnot %>"></asp:Label></td><td><asp:Label ID="txtfechanoti" runat="server">---</asp:Label></td></tr></table></td><td></td><td></td><tr><td></td><td></td><td></td><tr><td><asp:Label ID="lbldescSAT" runat="server" 
                                               Text="<%$ Resources:resCorpusCFDIEs, lbldescSAT %>"></asp:Label></td><td></td><td></td><tr><td><asp:TextBox ID="tbNotificacion" runat="server" Height="93px" 
                                                   TextMode="MultiLine" Width="394px"></asp:TextBox></td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr><tr><td><asp:Label ID="lblplanaccion" runat="server" 
                                                   Text="<%$ Resources:resCorpusCFDIEs, lblplanaccion %>"></asp:Label></td><td></td><td></td></tr><tr><td><table><tr><td class="style1"><asp:FileUpload ID="fuSat" runat="server" Height="23px" Width="219px" /></td><td class="style2"><asp:Label ID="lblarchivo" runat="server"></asp:Label></td><td>
                       <asp:CheckBox ID="cbAutorizaSAT" runat="server" Text="Autorizado" 
                           AutoPostBack="True" oncheckedchanged="cbAutorizaSAT_CheckedChanged" /></td></tr></table></td><td></td><td></td></tr><tr><td><table><tr><td><asp:Label ID="lblfechanot" runat="server" 
                                                               Text="<%$ Resources:resCorpusCFDIEs, lblfechanot %>"></asp:Label></td><td><asp:Label ID="txtfechanot" runat="server">---</asp:Label></td></tr></table></td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr><tr><td><asp:Label ID="lblautorizador" runat="server" 
                                                   Text="<%$ Resources:resCorpusCFDIEs, lblautorizador %>"></asp:Label></td><td></td><td></td></tr><tr><td>
                       <asp:TextBox ID="tbAutorizador" runat="server" Height="18px" Width="394px" 
                           Enabled="False"></asp:TextBox></td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr><tr><td>
                       <asp:Button ID="btnSAT" runat="server" OnClick="btnSAT_Click" 
                                                   Text="<%$ Resources:resCorpusCFDIEs, btnSAT %>" 
                           Width="126px" CssClass="botonEstilo" Enabled="False" />
                       <asp:Button ID="btnAutorizaSat" 
                           runat="server" OnClick="btnAutorizaSat_Click" 
                                                   
                           Text="<%$ Resources:resCorpusCFDIEs, btnAutorizaSat %>" Width="126px" 
                           CssClass="botonEstilo" Enabled="False" /></td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr></tr></tr></tr></table></ContentTemplate>

                    </cc1:TabPanel>

                    <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Solución Soporte">
                        <HeaderTemplate>Solución Soporte</HeaderTemplate>
                        <ContentTemplate><table style="width:100%;"><caption><caption><tr><td><table><tr><td><asp:Label ID="lblfechasop" runat="server" 
                                                                Text="<%$ Resources:resCorpusCFDIEs, lblfechasop %>"></asp:Label></td><td><asp:Label ID="txtfechasop" runat="server">---</asp:Label></td></tr></table></td><tr><td></td><tr><td><asp:Label ID="lblSolucionSoporte" runat="server" 
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblSolucionSoporte %>"></asp:Label></td><caption><tr><td><asp:TextBox ID="txtSolucionSoporte" runat="server" Height="93px" TabIndex="2" 
                                                                    TextMode="MultiLine" Width="394px"></asp:TextBox></td><caption><tr><td>
                                    <asp:Button ID="btnGuardarS0" 
                                                                            runat="server" Height="26px" 
                                        OnClick="btnGuardar_Click" TabIndex="3" 
                                                                            
                                        Text="<%$ Resources:resCorpusCFDIEs, btnGuardarS %>" Width="187px" 
                                        CssClass="botonGrande" Enabled="False" /></td><caption></caption></tr><caption><caption><tr><td valign="top"></td><caption><tr><td></td><caption><tr><td></td><caption></caption></tr><caption><caption><tr><td></td><caption><tr><td></td><caption><tr><td></td><caption></caption></tr></caption><caption></caption></tr></caption></tr></caption></caption></caption></tr></caption></tr></caption></caption></caption></tr></caption></tr></tr></tr></caption></tr></caption></caption></caption></caption></caption></table></ContentTemplate>
                        
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="Pruebas">
                        <ContentTemplate><table><caption><caption>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
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
                                    <td>
                                    </td>
                                    <tr>
                                        <td>
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
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <caption>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblSolucionPruebas0" runat="server" 
                                                                                Text="<%$ Resources:resCorpusCFDIEs, lblSolucionPruebas %>"></asp:Label>
                                                                            <asp:CheckBox ID="cbAprobado" runat="server" Text="Aprobado" />
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
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
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
                                                                                            <td>
                                                                                                <asp:Button ID="btnGuardarSolucionPrueba" runat="server" CssClass="botonGrande" 
                                                                                                    Height="26px" OnClick="btnGuardarSolucionPrueba_Click" TabIndex="3" 
                                                                                                    Text="<%$ Resources:resCorpusCFDIEs, btnGuardarSolucionPrueba %>" 
                                                                                                    Width="170px" Enabled="False" />
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
                                                            </tr>
                                                        </caption>
                                                    </caption>
                                                </caption>
                                            </tr>
                                        </caption>
                                    </tr>
                                </tr>
                            </tr>
                            </caption></caption>
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
                            </tr>
                            </caption></caption></caption></tr></caption></tr></caption></caption></caption></tr></caption></tr></caption></tr></tr></tr></caption></caption></caption><caption></caption>
                            <caption>
                                <caption>
                                    <caption>
                                    </caption>
                                    </tr>
                                </caption>
                                </tr>
                            </caption>
                            </tr>
                            </caption>
                            </caption></tr></caption></caption></caption></tr></caption></tr></tr></tr></caption></caption></caption></tr></caption></tr></caption></tr></caption><caption><caption><caption><caption><caption></caption></caption></caption></caption></caption></caption></caption></tr></caption></tr></caption></caption><caption><caption><caption><caption><caption></caption></caption></caption></caption></caption></tr></caption></tr></caption></caption></caption></caption></tr></caption></tr></caption></tr></caption></caption></caption></tr></caption></tr></caption></caption></caption></table></ContentTemplate>
                        
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel5" runat="server" HeaderText="Solución Usuario">
                        <HeaderTemplate>Solución Usuario</HeaderTemplate>
                        <ContentTemplate><table style="width:100%;"><caption><caption><tr><td><table><tr><td><asp:Label ID="lblfechausu" runat="server" 
                                                                Text="<%$ Resources:resCorpusCFDIEs, lblfechausu %>"></asp:Label></td><td><asp:Label ID="txtfechausu" runat="server">---</asp:Label></td></tr></table></td><tr><td></td><tr><td><asp:Label ID="lblSolucionUsuario" runat="server" 
                                                            Text="<%$ Resources:resCorpusCFDIEs, lblSolucionUsuario %>"></asp:Label></td><caption><tr><td><asp:TextBox ID="txtSolucionUsuario" runat="server" Height="75px" TabIndex="1" 
                                                                    TextMode="MultiLine" Width="394px"></asp:TextBox></td><caption><tr><td>
                                    <asp:Button ID="btnGuardarSolUsuario0" runat="server" 
                                                                            Height="26px" 
                                        OnClick="btnGuardarSolUsuario_Click" TabIndex="3" 
                                                                            
                                        Text="<%$ Resources:resCorpusCFDIEs, btnGuardarI %>" Width="170px" 
                                        CssClass="botonGrande" Enabled="False" /><asp:Button ID="btnTerminarP" runat="server" Enabled="False" 
                                                                            OnClick="btnTerminar_Click" TabIndex="3" 
                                                                            
                                        Text="<%$ Resources:resCorpusCFDIEs, btnTerminarP %>" Width="126px" 
                                        CssClass="botonGrande" /></td><caption></caption><caption><caption><tr><td></td><caption><tr><td></td><caption><tr><td></td><caption></caption></tr><caption><caption><tr><td></td><caption><tr><td></td><caption><tr><td></td><caption></caption></tr></caption></tr><caption></caption></caption></tr></caption></caption></caption></tr></caption></tr></caption></caption></tr></caption></tr></caption></tr></tr></tr></caption>
                            </tr>
                            </caption></tr></tr></tr></caption></caption></caption></tr>
                            </caption>
                            </tr>
                            </caption>
                            </tr></caption></caption></caption></caption>
                            </caption>
                            </caption>
                            </caption>
                            </tr></caption></tr></caption>
                            </caption>
                            </caption>
                            </caption></caption></caption></tr></caption></tr></caption></caption></caption></caption>
                            </tr>
                            </caption>
                            </tr>
                            </caption>
                            </tr>
                            </caption>
                            </caption>
                            </caption>
                            </tr>
                            </table></ContentTemplate>
                        




                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
            <td>
                </td>
        </tr>
        </table>
</asp:Content>


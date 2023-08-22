<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="webOperacionPlantillasCorreos.aspx.cs" Inherits="PAXRecepcionProveedores.Operacion.Plantillas.webOperacionPlantillasCorreos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script language="javascript" type="text/javascript">
//    function cpeColorEncabezado_SelectionChanged() {
//        document.getElementById("txtColorEncabezado").style.color = "#" + document.getElementById("txtColorEncabezado").value;
    //    }
//    function colorChanged(sender) {
//        sender.get_element().value =
//       "#" + sender.get_selectedColor();
//    }

//    function colorChanged(sender) {
//        document.getElementById("txtColorEncabezado").value =
//       "#" + sender.get_selectedColor();
//        document.getElementById("lblColorEncabezado").value =
//       "#" + sender.get_selectedColor();
    //    }
        function fntraerEnter(keyStroke) {
            isNetscape = (document.layers);
            eventChooser = (isNetscape) ? keyStroke.which : event.keyCode;
            if (eventChooser == 13) {
                return false;
            }
        }
        document.onkeypress = fntraerEnter; 
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<br /><br /><br />
<h2>
        <asp:Label ID="lblTitulo" runat="server" 
            Text="<%$ Resources:resCorpusCFDIEs, lblEdicionPlantilla %>" Font-Bold="True" 
            ForeColor="#8B181B"></asp:Label>
    </h2>
    <table style="border: 2px ridge #8B181B">
        <tr>
            <td>
            <asp:UpdatePanel ID="udpPlantilla" runat="server">
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblPlantillas" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varPlantillas %>" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlPlantillas" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantillas_SelectedIndexChanged" />
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblAsunto" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblAsunto %>" />
                            </td>
                            <td>
                                <asp:Label ID="lblMensaje" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreoMensaje %>" />
                            </td>
                    </tr>
                    <tr>
                            <td>
                                <asp:TextBox ID="txtAsunto" runat="server" Enabled="false" Width="342px"/>
                            </td>
                            <td rowspan="3">
                                <asp:TextBox ID="txtMensaje" runat="server" TextMode="MultiLine" 
                                    Height="67px" Enabled="false" Width="375px" style="margin-top: 0px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblImagenLogo" runat="server" Text="Logo" />
                                <asp:CheckBox ID="cbLogoEliminar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:FileUpload ID="fupImagenLogo"  runat="server" Enabled="false" 
                                    Width="329px"/>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="lblImagenFirma" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblImagenFirma %>" />
                                <asp:CheckBox ID="cbFirmaEliminar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" Enabled="false" />
                            </td>
                            <td>
                                <asp:Label ID="lblFirma" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblFirma %>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:FileUpload ID="fupImagenFirma"  runat="server" Enabled="false" 
                                    Width="329px"/>
                            </td>
                            <td rowspan="3">
                                <asp:TextBox ID="txtFirma" runat="server" TextMode="MultiLine" Height="59px" 
                                    Enabled="false" Width="371px" />

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblColorEncabezado" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblColorEncabezado %>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtColorEncabezado" runat="server" Enabled="false" />
                                <cc1:ColorPickerExtender runat="server" 
                                  ID="cpeColorEncabezado"
                                  TargetControlID="txtColorEncabezado" PopupButtonID="imgColorPicker" 
                                   Enabled="false" SampleControlID="lblColorMuestra" />
                                  <asp:Image ID="imgColorPicker" runat="server" ImageUrl="~/Imagenes/cp_button.png"  />
                                  <asp:Label ID="lblColorMuestra" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEjemplo %>" BorderColor="Black" />
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnEditar" runat="server" CssClass="botonEstilo" 
                                onclick="btnEditar_Click" Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                                Width="80px" TabIndex="6" />
                                <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" 
                                onclick="btnNCancelar_Click" 
                                 Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" 
                                    TabIndex="7" />  
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblPlantillaVista" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblVistaPreviaPlantilla %>"/>

                </td>
             </tr>
             <tr>
                <td>
                    <asp:Panel ID="pnlPlantilla" runat="server" BorderColor="Black" 
                        BorderStyle="Solid" Width="980px">
                    <table>
                        <tr>
                            <td>
                                <b><asp:Label ID="lblAsunto2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblAsunto %>" />: </b>
                                <asp:Label ID="lblAsuntoVista" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <asp:Image ID="imgLogo" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltPlantilla" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <asp:Image ID="imgFirma" runat="server" />
                            </td>
                        </tr>
                    </table>
                    </asp:Panel>
                </td>
             </tr>
        </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlPlantillas" EventName="SelectedIndexChanged" />
        <asp:PostBackTrigger ControlID="btnEditar" />
    </Triggers>
    </asp:UpdatePanel>

            </td>
        </tr>
    </table>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="webOperacionPlantillasCorreos.aspx.cs" Inherits="Operacion_Plantillas_webOperacionPlantillasCorreos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
    <br />
    <br />
    <br />
        <table>
            <tr>
                <td align="left">
                <h2>
                    <asp:Label ID="lblTitulo" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, varCatalogoPlantillas %>"></asp:Label>
               </h2> 
                </td>
            </tr>
        </table>
<table style="margin: 0px auto; width: 983px; border: solid 1px #C0C0C0;" class="bodyMain">
    <tr> 
        <td>
                    <asp:UpdatePanel ID="udpPlantilla" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblPlantillas" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, varPlantillas %>" Font-Bold="False" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:DropDownList ID="ddlPlantillas" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantillas_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td align="left">
                                <br />
                                    <asp:Label ID="lblAsunto" runat="server" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblAsunto %>" Font-Bold="False" />
                                </td>
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td align="left">
                                    <asp:Label ID="lblMensaje" runat="server" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblCorreoMensaje %>" 
                                        Font-Bold="False" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtAsunto" runat="server" Enabled="false" Width="342px" />
                                </td>
                                <td></td>
                                <td rowspan="3">
                                    <asp:TextBox ID="txtMensaje" runat="server" TextMode="MultiLine" Height="68px" Enabled="false"
                                        Width="375px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblImagenLogo" runat="server" Text="Logo" Font-Bold="False" />
                                    <asp:CheckBox ID="cbLogoEliminar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:FileUpload ID="fupImagenLogo" runat="server" Enabled="false" Width="329px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                <br />
                                    <asp:Label ID="lblImagenFirma" runat="server"  
                                        Text="<%$ Resources:resCorpusCFDIEs, lblImagenFirma %>" 
                                        Font-Bold="False" />
                                    <asp:CheckBox ID="cbFirmaEliminar" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" Enabled="false" />
                                </td>
                                <td></td>
                                <td align="left">
                                    <asp:Label ID="lblFirma" runat="server"  
                                        Text="<%$ Resources:resCorpusCFDIEs, lblImagenFirma %>" 
                                        Font-Bold="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:FileUpload ID="fupImagenFirma" runat="server" Enabled="false" Width="329px" />
                                </td>
                                <td></td>
                                <td rowspan="3">
                                    <asp:TextBox ID="txtFirma" runat="server" TextMode="MultiLine" Height="59px" Enabled="false"
                                        Width="371px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                <br />
                                    <asp:Label ID="lblColorEncabezado" runat="server"  
                                        Text="<%$ Resources:resCorpusCFDIEs, lblColorEncabezado %>" 
                                        Font-Bold="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="txtColorEncabezado" runat="server" Enabled="false" />
                                    <cc1:ColorPickerExtender runat="server" ID="cpeColorEncabezado" TargetControlID="txtColorEncabezado"
                                        PopupButtonID="imgColorPicker" Enabled="false" SampleControlID="lblColorMuestra" />
                                    <asp:Image ID="imgColorPicker" runat="server" ImageUrl="~/Imagenes/cp_button.png" />
                                    <asp:Label ID="lblColorMuestra" runat="server"  
                                        Text="<%$ Resources:resCorpusCFDIEs, lblEjemplo %>" BorderColor="Black" 
                                        Font-Bold="False" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button ID="btnEditar" runat="server" CssClass="botonEstilo" OnClick="btnEditar_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" Width="80px" 
                            TabIndex="6" Height="27px" />
                        <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstilo" OnClick="btnNCancelar_Click"
                            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" 
                            TabIndex="7" Height="27px" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblPlantillaVista" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblVistaPlantilla %>" 
                            Font-Bold="False" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Panel ID="pnlPlantilla" runat="server" BorderColor="Black" BorderStyle="Solid">
                            <table>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblAsunto2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblAsunto %>" />:&nbsp;
                                        <asp:Label ID="lblAsuntoVista" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Image ID="imgLogo" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Literal ID="ltPlantilla" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
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


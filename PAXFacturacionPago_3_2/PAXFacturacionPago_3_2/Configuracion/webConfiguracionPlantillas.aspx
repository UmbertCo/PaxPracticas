<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webConfiguracionPlantillas.aspx.cs" Inherits="Configuracion_webConfiguracionPlantillas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 256px;
        }
        .style3
        {
            width: 320px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblConfigPlantillas %>"></asp:Label>

    </h2>
  
    <asp:UpdatePanel ID="UpdPlantilla" runat="server">
        <ContentTemplate>
            <table style="width: 906px">
                <tr>
                    <td class="style2" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2" colspan="2">
                        <asp:Label ID="Label3" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblEstructura %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style2" colspan="2">
                        <asp:DropDownList ID="ddlSucursales" runat="server" AutoPostBack="True" 
                            DataTextField="nombre" DataValueField="id_estructura" 
                            onselectedindexchanged="ddlSucursales_SelectedIndexChanged" TabIndex="1" 
                            Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style2" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <fieldset class="register" style="width:890px;">
                            <legend>
                                <asp:Literal ID="Literal1" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblColor %>" />
                            </legend>
                            <table align="left">
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:Label ID="Label2" runat="server" 
                                            Text="<%$ Resources:resCorpusCFDIEs, lblColor %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" width="40">
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Imagenes/Circulo Negro.png" />
                                    </td>
                                    <td align="center" width="40">
                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/Circulo Azul.png" />
                                    </td>
                                    <td align="center" width="40">
                                        <asp:Image ID="Image4" runat="server" ImageUrl="~/Imagenes/Circulo Verde.png" />
                                    </td>
                                    <td align="center" width="40">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/Circulo Rojo.png" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" width="40">
                                        <asp:RadioButton ID="rbNegro" runat="server" Checked="True" GroupName="Radio" 
                                            TabIndex="2" AutoPostBack="True" 
                                            oncheckedchanged="rbNegro_CheckedChanged" />
                                    </td>
                                    <td align="center" width="40">
                                        <asp:RadioButton ID="rbAzul" runat="server" GroupName="Radio" TabIndex="3" 
                                            AutoPostBack="True" oncheckedchanged="rbAzul_CheckedChanged" />
                                    </td>
                                    <td align="center" width="40">
                                        <asp:RadioButton ID="rbVerde" runat="server" GroupName="Radio" TabIndex="4" 
                                            AutoPostBack="True" oncheckedchanged="rbVerde_CheckedChanged" />
                                    </td>
                                    <td align="center" width="40">
                                        <asp:RadioButton ID="rbRojo" runat="server" GroupName="Radio" TabIndex="5" 
                                            AutoPostBack="True" oncheckedchanged="rbRojo_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset></td>
                </tr>
                <tr>
                    <td align="center" valign="top">
                        <fieldset class="register" style="width:890px;">
                            <legend>
                                <asp:Literal ID="Literal2" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, varPlantillas %>" />
                            </legend>
                            <table align="left">
                                <tr align="left">
                                    <td valign="top">
                                        <asp:Image ID="imgSinlogo" runat="server" Height="196px" 
                                            ImageUrl="~/Imagenes/logo_sinlogo.png" />
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td valign="top">
                                        <asp:Image ID="imgLogo" runat="server" Height="196px" 
                                            ImageUrl="~/Imagenes/logo_logo.png" />
                                    </td>
                                    <td valign="top">
                                        &nbsp;</td>
                                    <td valign="top">
                                        <asp:Image ID="imgPersonal" runat="server" Height="196px" 
                                            ImageUrl="~/Imagenes/logo_personalizado.png" Visible="False" Width="171px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:RadioButton ID="rbSinLogo" runat="server" Checked="True" GroupName="Logo" 
                                            TabIndex="6" Text="Genérica" />
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td align="center">
                                        <asp:RadioButton ID="rbConLogo" runat="server" GroupName="Logo" TabIndex="7" 
                                            Text="Logo" />
                                    </td>
                                    <td align="center">
                                        &nbsp;</td>
                                    <td align="center">
                                        <asp:RadioButton ID="rbPersonal" runat="server" GroupName="Logo" TabIndex="8" 
                                            Text="Personalizada" Visible="False" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset></td>
                    <td align="center">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" align="right" valign="top">
                        <asp:Button ID="btnGuardar" runat="server" CssClass="botonEstilo" 
                            onclick="btnGuardar_Click" TabIndex="9" 
                            Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" />
                    </td>
                </tr>
                <tr>
                    <td class="style2" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2" colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
            <br />
            &nbsp;
        </ContentTemplate>

    </asp:UpdatePanel>
    







        








</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="webNoticias.aspx.cs" Inherits="PaginaComercial.webNoticias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


 <link rel="shortcut icon" href="imagenes/fav.ico" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <table align="center">
        <tr>
            <td>
                <asp:Label ID="lblDescripcionin" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblDescripcionin%>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:TextBox ID="txtDescripcionI" runat="server" Height="68px" 
                    TextMode="MultiLine" Width="494px" MaxLength="140" Enabled="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="Medium" 
                    ForeColor="#CC0000" Visible="False" 
                    Text="<%$ Resources:resCorpusCFDIEs, varAceptar%>" 
                    style="font-family: 'Century Gothic'; font-size: small"></asp:Label>
                <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="Medium" 
                    ForeColor="#CC0000" Visible="False" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblNoticiasError%>" 
                    style="font-family: 'Century Gothic'; font-size: small"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                <asp:Button ID="btnNuevo" runat="server" 
                    onclick="button2_Click" style="margin-left: 0px" 
                    Text="Nuevo" Width="158px" Height="27px" CssClass="botonGrande" Font-Bold="True" />
                        </td>
                        <td>
                <asp:Button ID="btnGuardar" runat="server" 
                    onclick="button1_Click" style="margin-left: 0px" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnGuardarSop%>" Width="158px" 
                    CssClass="botonGrande" Font-Bold="True" Height="29px" />
                        </td>
                        <td>
                 <asp:Button ID="btnCancelar" runat="server" 
                    onclick="button3_Click" style="margin-left: 0px" 
                    Text="Cancelar" Width="158px" Height="27px" CssClass="botonGrande" Font-Bold="True" />
                        </td>
                        <td>
                            <asp:Button ID="btnSalir" runat="server" Text="Salir" CssClass="botonGrande" 
                                Height="27px" onclick="Button4_Click" onclientclick="~/Inicio.aspx" 
                                PostBackUrl="~/Inicio.aspx" Width="158px" />
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                    <asp:GridView ID="gvPreguntas" runat="server" 
    AutoGenerateColumns="False" DataKeyNames="idNoticia" Font-Bold="True" 
                    onselectedindexchanged="gvPreguntas_SelectedIndexChanged" 
    Width="644px" BackColor="White" BorderColor="#336666" 
    BorderStyle="Double" BorderWidth="3px" CellPadding="4" 
    onrowdeleting="gvPreguntas_RowDeleting" PageSize="5" 
                        style="font-family: 'Century Gothic'; font-size: small">
                        <Columns>
                            <asp:CommandField SelectText="Editar" 
                            ShowSelectButton="True">
                            <ControlStyle Width="55px" />
                            <ItemStyle HorizontalAlign="Center" Width="55px" />
                            </asp:CommandField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdNoticia" runat="server" Text='<%# bind("idNoticia") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="25px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlPregunta" runat="server" CssClass="link_hyper" 
                                    ForeColor="#395C6C" Text='<%# Bind("descripcion") %>' Font-Bold="False"></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="fecha" DataFormatString="{0:d}">
                            <ItemStyle ForeColor="#395C6C" />
                            </asp:BoundField>
                            <asp:CommandField DeleteText="Eliminar" 
                            ShowDeleteButton="True">
                            <ControlStyle Width="55px" />
                            <ItemStyle HorizontalAlign="Center" Width="55px" />
                            </asp:CommandField>
                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <HeaderStyle BackColor="#395C6C" Font-Bold="True" 
                        ForeColor="White" />
                        <PagerStyle BackColor="#395C6C" ForeColor="White" 
                        HorizontalAlign="Center" />
                        <RowStyle BackColor="White" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" 
                        ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                        <SortedAscendingHeaderStyle BackColor="#487575" />
                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                        <SortedDescendingHeaderStyle BackColor="#275353" />
                    </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;</td>
        </tr>
    </table>
    <table align ="center">
    <tr>
      <td><div align="center"><img src="linea.jpg" width="750" height="2" /></div></td>
    </tr>
  </table>
</asp:Content>


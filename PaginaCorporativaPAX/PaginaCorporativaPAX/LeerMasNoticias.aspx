<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LeerMasNoticias.aspx.cs" Inherits="LeerMasNoticias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <link rel="shortcut icon" href="imagenes/fav.ico" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <table class=hojaPrincipal" width="950" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td  align="left" width="950px" style="text-align: left">
         
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         
          <asp:Label ID="lblLeerMas" runat="server" 
              Text="<%$ Resources:resCorpusCFDIEs, lblleermas %>" Font-Bold="True" 
              Font-Size="XX-Large" ForeColor="#A5D10F" 
              style="font-family: 'Century Gothic'"></asp:Label>
          <br />
        bsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          <asp:Label ID="lblNoticias" runat="server" 
              Text="<%$ Resources:resCorpusCFDIEs, lblTitNoticias %>" Font-Bold="True" 
              Font-Size="X-Large" ForeColor="#395C6C" 
              style="font-family: 'Century Gothic'"></asp:Label>
          <br />
          <br />
        </td>
    </tr>
  </table>
  <table width="750" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td class="bordesRedondos"><div align="left">
        <table border="0" cellspacing="0" cellpadding="0" style="width: 398px">
          <tr>
            <td>&nbsp;</td>
          </tr>
        </table>
        <table border="0" cellspacing="0" cellpadding="0" style="width: 741px">
          <tr>
          <td valign="50px">
              &nbsp;&nbsp;
              <br />
              <br />
              <br />
              <br />
              <br />
              <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
            <td width="900px">
                <asp:GridView ID="gvPreguntas" runat="server" AutoGenerateColumns="False" 
                    GridLines="None" DataKeyNames="idNoticia" Font-Bold="True" 
                    ForeColor="#333333" 
                    onselectedindexchanged="gvPreguntas_SelectedIndexChanged" Width="900px" 
                    CellPadding="4" style="font-family: 'Century Gothic'; font-size: small">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblIdNoticia" runat="server" Text='<%# bind("idNoticia") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink ID="hlNoticia" runat="server" CssClass="link_hyper" 
                                    ForeColor="#395C6C" Text='<%# Bind("descripcion") %>' Font-Bold="False"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#7C6F57" />
                    <FooterStyle BackColor="#395C6C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#395C6C" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#E3EAEB" />
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                </asp:GridView>
            </td>
          </tr>
        </table>
        <table width="400" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td>&nbsp;</td>
          </tr>
    </table>
      </div></td>
    </tr>
  </table>

</asp:Content>


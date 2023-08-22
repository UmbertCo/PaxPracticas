<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webBusquedaReceptores.aspx.cs" Inherits="Timbrado_webBusquedaReceptores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <h2>
            <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloBusquedaReceptor %>"></asp:Label>
        </h2>
        <div class="accountInfo textos" style="width:600px;">
            <fieldset class="register" style="width:600px;">
                <p>
                <asp:Label runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubtituloBusReceptores %>" />
                </p>
                <p>
                    <asp:Label ID="lblRFC" AssociatedControlID="txtRFC" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>"></asp:Label>
                    <asp:TextBox ID="txtRFC" runat="server" CssClass="textEntry" MaxLength="15"></asp:TextBox>
                </p>
                <p>
                    <asp:Label ID="lblRazon" AssociatedControlID="txtRazonSocial" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRazonSocial %>"></asp:Label>
                    <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="textEntry" 
                        MaxLength="50" ></asp:TextBox>
                </p>
            </fieldset>
        </div>
        <p style="width:600px; text-align:right;">
            <asp:Button ID="btnConsulta" runat="server" 
                Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                onclick="btnConsulta_Click" />
                <asp:Button ID="btnCancelar" runat="server" 
                Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" onclick="btnCancelar_Click" 
                />
        </p>
        <asp:GridView ID="gdvReceptores" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" ForeColor="#333333" GridLines="None" Width="600px" 
                DataKeyNames="id_rfc_receptor,rfc_receptor,nombre_receptor" AllowPaging="True" 
                BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
                onpageindexchanging="gdvReceptores_PageIndexChanging">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <%--<asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="hpkSelecionar" runat="server" CausesValidation="False" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionar %>" 
                                onclick="hpkSelecionar_Click"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:TemplateField>--%>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="btnSeleccion" runat="server" onclick="hpkSeleccion_Click" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblSeleccionar %>" />
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="rfc_receptor" 
                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblRfc %>" ItemStyle-Width="100" 
                        HeaderStyle-HorizontalAlign="Left"  >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="nombre_receptor" 
                        HeaderText="<%$ Resources:resCorpusCFDIEs, lblRazonSocial %>" 
                        HeaderStyle-HorizontalAlign="Left" >
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <EmptyDataTemplate>
                    <asp:literal ID="Literal1" runat="server" text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                </EmptyDataTemplate>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


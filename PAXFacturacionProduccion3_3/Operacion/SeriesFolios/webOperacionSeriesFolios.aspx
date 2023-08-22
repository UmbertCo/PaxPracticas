<%@ Page Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionSeriesFolios.aspx.cs" Inherits="Operacion_SeriesFolios_webOperacionSeriesFolios" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="container">
        <h1>
            <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloSeries %>" CssClass="TituloInicio"></asp:Label>
            <hr class="hrbottom" style="padding-bottom: 0px; margin-bottom: 0px;"/>
        </h1>
       
    </div>
    <div class="container">
        <div class="well">
            <div class="panel-body">
                <div class="row">
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12"> 
                        <asp:UpdatePanel ID="updSerieFolio" runat="server">
                            <ContentTemplate>
                                <%--Campo Nombre sucursal--%>
                                <div class="form-group">
                                    <asp:Label ID="lblNombreSucursal" runat="server"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>" 
                                        AssociatedControlID="ddlSucursales" ></asp:Label>
                                    <asp:DropDownList ID="ddlSucursales" runat="server" CssClass="form-control" AutoPostBack="True" 
                                        DataTextField="nombre" DataValueField="id_estructura" TabIndex="1" 
                                        onselectedindexchanged="ddlSucursales_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvSucursal" runat="server" 
                                        ControlToValidate="ddlSucursales" ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                                        ValidationGroup="grupoSeriesFolios"><span class="tooltiptext"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valSucursal %>" /></span></asp:RequiredFieldValidator>
                                </div>

                                <%--Campo Tipo documento --%>
                                <div class="form-group">
                                    <asp:Label ID="lblTipoDocumento" runat="server"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblTipoComprobante %>" 
                                        AssociatedControlID="ddlTipoDocumento" ></asp:Label>
                                        <asp:DropDownList ID="ddlTipoDocumento" runat="server" CssClass="form-control" 
                                        DataTextField="Descripcion" DataValueField="id_tipocomprobante" TabIndex="2" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="ddlTipoDocumento_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvTipoDoc" runat="server" 
                                        ControlToValidate="ddlTipoDocumento" ToolTip="<%$ Resources:resCorpusCFDIEs, valTipoDocumento %>" 
                                        ValidationGroup="grupoSeriesFolios"><span class="tooltiptext"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valTipoDocumento %>" /></span></asp:RequiredFieldValidator>
                                </div>

                                <%--Campo Serie --%>
                                <div class="form-group">
                                    <asp:Label ID="lblSerie" runat="server"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblSerie %>" 
                                        AssociatedControlID="txtSerie" ></asp:Label>
                                    <asp:TextBox ID="txtSerie" runat="server" CssClass="form-control" 
                                        MaxLength="25" TabIndex="3"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSerie" runat="server" 
                                        ControlToValidate="txtSerie" ToolTip="<%$ Resources:resCorpusCFDIEs, valSerie %>" 
                                        ValidationGroup="grupoSeriesFolios"><span class="tooltiptext"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valSerie %>" /></span></asp:RequiredFieldValidator>
                                </div>

                                <%--Campo Folio --%>
                                <div class="form-group">
                                    <asp:Label ID="lblFolio" runat="server"
                                        Text="<%$ Resources:resCorpusCFDIEs, lblFolio %>" 
                                        AssociatedControlID="txtFolio" ></asp:Label>
                                    <asp:TextBox ID="txtFolio" runat="server" CssClass="form-control" 
                                        MaxLength="5" TabIndex="4"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="txtFolio_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFolio">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="rfvFolio" runat="server" 
                                        ControlToValidate="txtFolio" ToolTip="<%$ Resources:resCorpusCFDIEs, valFolio %>" 
                                        ValidationGroup="grupoSeriesFolios"><span class="tooltiptext"><asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:resCorpusCFDIEs, valFolio %>" /></span></asp:RequiredFieldValidator>
                                </div>
                            <%--</fieldset>--%>
                                <div class="form-group pull-right">
                                    <asp:Button ID="btnGuardar" runat="server" CommandName="MoveNext" 
                                    ValidationGroup="grupoSeriesFolios" 
                                    Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>"
                                        TabIndex="5" onclick="btnGuardar_Click" CssClass="btn btn-primary-red" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12"> 
                        <asp:UpdatePanel ID="updGuardar" runat="server">
                            <ContentTemplate>
                                <div class="table-responsive" style="overflow:auto;">
                                    <asp:GridView ID="gdvSeries" runat="server" AutoGenerateColumns="False" 
                                        CellPadding="4" GridLines="Horizontal" 
                                        DataKeyNames="id_serie" onrowdeleting="gdvSeries_RowDeleting" 
                                        AllowPaging="True" CssClass="table-bordered table"
                                        onpageindexchanging="gdvSeries_PageIndexChanging" BackColor="White">
                                        <Columns>
                                            <asp:BoundField DataField="serie"  HeaderStyle-HorizontalAlign="Left"
                                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblSerie %>" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="150px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="folio" HeaderStyle-HorizontalAlign="Right"
                                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblFolio %>" >
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="150px" />
                                            </asp:BoundField>
                                            <asp:CommandField ShowDeleteButton="True" DeleteText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>"
                                                HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" HeaderStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:CommandField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                                        </EmptyDataTemplate>
                                        <FooterStyle BackColor="White" ForeColor="#5A737E" />
                                        <HeaderStyle BackColor="#5A737E" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="White" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                        <SortedAscendingHeaderStyle BackColor="#487575" />
                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                        <SortedDescendingHeaderStyle BackColor="#275353" />
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
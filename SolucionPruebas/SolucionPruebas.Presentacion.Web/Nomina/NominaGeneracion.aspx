<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="NominaGeneracion.aspx.cs" Inherits="SolucionPruebas.Presentacion.Web.Nomina.NominaGeneracion" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:GridView ID="gdvPagosNomina" runat="server" AutoGenerateColumns="False" 
        CellPadding="2" GridLines="Both" Width="100%" BackColor="White" 
        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
        DataKeyNames="id_estructura,Id_Nomina,Id_PagoNomina,Id_Empleado">
        <Columns>
            <asp:TemplateField ItemStyle-Width="20px">
                <ItemTemplate>
                    <asp:CheckBox ID="cbSeleccion" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="NumEmpleado"
            HeaderText="NumeroEmpleado" >
            <HeaderStyle HorizontalAlign="Center" />                      
            <ItemStyle Width="90px" />
            <ItemStyle HorizontalAlign="Left" Width="90px" />                                   
            </asp:BoundField>
            <asp:BoundField DataField="Nombre" 
                HeaderText="Nombre" >
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle Width="100px" />
            <ItemStyle HorizontalAlign="Left" Width="100px" />  
            </asp:BoundField>
            <asp:BoundField DataField="Departamento" 
                HeaderText="Departamento" >
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle Width="100px" />
            <ItemStyle HorizontalAlign="Left" Width="100px" />  
            </asp:BoundField>
            <asp:BoundField DataField="Puesto" 
                HeaderText="Puesto" >
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle Width="100px" />
            <ItemStyle HorizontalAlign="Left" Width="100px" />  
            </asp:BoundField>
            <asp:BoundField DataField="Total" 
                HeaderText="Total">
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle Width="100px" />
            <ItemStyle HorizontalAlign="Left" Width="100px" />  
            </asp:BoundField>
        </Columns>
        <EmptyDataTemplate>
            <asp:Literal ID="Literal1" runat="server" Text="GridVacio" />
        </EmptyDataTemplate>
        <FooterStyle BackColor="White" ForeColor="#5A737E" />
        <HeaderStyle BackColor="#5A737E" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="White" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#92BA41" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F7F7F7" />
        <SortedAscendingHeaderStyle BackColor="#487575" />
        <SortedDescendingCellStyle BackColor="#E5E5E5" />
        <SortedDescendingHeaderStyle BackColor="#275353" />
    </asp:GridView>
    <asp:Button ID="btnGenerar" runat="server" Text="Generar Nomina" 
        onclick="btnGenerar_Click" />
</asp:Content>

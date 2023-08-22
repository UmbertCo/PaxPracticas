<%@ Page Title="" Language="C#" MasterPageFile="~/Privada.master" AutoEventWireup="true" CodeFile="webBusquedaIncidentes.aspx.cs" Inherits="Pantallas_Problemas_webBusquedaIncidentes" %>

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
        width:200px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div class="accountInfo textos" style="width:600px;">
                <fieldset class="register" style="width:600px;">
                    <legend>Parámetros de la búsqueda</legend>
                    <p>
                        Los resultados mostrados por la búsqueda estaran límitados a los correspondientes a su usuario.
                        Se puede incluir más de una palabra clave, separando cada una con un espacio en 
                        blanco.</p>
                    <p>
                    <table class="contenedor">
                        <tr>
                            <td>
                                <asp:Label ID="lblEstatus" runat="server" 
                                    Text="Estatus del incidente" 
                                    AssociatedControlID="ddlEstatus"></asp:Label>
                                <asp:DropDownList ID="ddlEstatus" runat="server">
                                    <asp:ListItem Selected="True" Value="-1">Todos</asp:ListItem>
                                    <asp:ListItem Value="A">Abierto</asp:ListItem>
                                    <asp:ListItem Value="E">En solución</asp:ListItem>
                                    <asp:ListItem Value="S">Solucionado</asp:ListItem>
                                    <asp:ListItem Value="C">Cerrado</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFechaIni" runat="server" 
                                    Text="Fecha inicial" 
                                    AssociatedControlID="txtFechaIni"></asp:Label>
                                <asp:TextBox ID="txtFechaIni" runat="server" BackColor="White" ></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaIni" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                                <asp:RegularExpressionValidator ID="revFechaIni" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" ToolTip="La fecha no tiene el formato correcto" > <img src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvFechaIni" runat="server" 
                                    ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                    ValidationGroup="grupoConsulta" ToolTip="La fecha inicial es requerida" ></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="lblFechaFin" runat="server" 
                                    Text="Fecha final" 
                                    AssociatedControlID="txtFechaFin"></asp:Label>
                                <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaFin" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                                <asp:RegularExpressionValidator ID="revFechaFin" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" ToolTip="La fecha no tiene el formato correcto" > <img src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" 
                                    ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                    ValidationGroup="grupoConsulta" ToolTip="La fecha final es requerida" ></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblUrgencia" runat="server" 
                                    Text="Urgencia del incidente" 
                                    AssociatedControlID="ddlUrgencia"></asp:Label>
                                <asp:DropDownList ID="ddlUrgencia" runat="server">
                                    <asp:ListItem Selected="True" Value="-1">Todos</asp:ListItem>
                                    <asp:ListItem Value="0">Baja urgencia</asp:ListItem>
                                    <asp:ListItem Value="1">Urgencia moderada</asp:ListItem>
                                    <asp:ListItem Value="2">Alta urgencia</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblImpacto" runat="server" 
                                    Text="Impacto del incidente" 
                                    AssociatedControlID="ddlImpacto"></asp:Label>
                                <asp:DropDownList ID="ddlImpacto" runat="server">
                                    <asp:ListItem Selected="True" Value="-1">Todos</asp:ListItem>
                                    <asp:ListItem Value="0">Sin riesgo</asp:ListItem>
                                    <asp:ListItem Value="1">Bajo riesgo</asp:ListItem>
                                    <asp:ListItem Value="2">Riesgo moderado</asp:ListItem>
                                    <asp:ListItem Value="3">Alto riesgo</asp:ListItem>
                                    <asp:ListItem Value="4">Sin catalogar</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblPalabrasClave" runat="server" 
                                    Text="Palabras clave" 
                                    AssociatedControlID="txtPalabrasClave"></asp:Label>
                                <asp:TextBox ID="txtPalabrasClave" runat="server" BackColor="White" 
                                    Width="500px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    </p>
                </fieldset>
            </div>
            <p style="width:600px; text-align:right;">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                    onclick="btnBuscar_Click" />
            </p>
    <asp:UpdatePanel ID="udpResultados" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gdvIncidentes" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" ForeColor="#333333" GridLines="None"  Width="100%" AllowPaging="True" 
                DataKeyNames="ticket" >
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField HeaderText="TICKET" DataField="ticket" />
                    <asp:BoundField HeaderText="DESCRIPCIÓN" DataField="descripcion" />
                    <asp:BoundField HeaderText="REGISTRO" DataField="fecha_registro" />
                    <asp:BoundField HeaderText="ATENCIÓN" DataField="fecha_atencion" />
                    <asp:BoundField HeaderText="SOLUCIÓN" DataField="fecha_solucion" />
                    <asp:BoundField HeaderText="URGENCIA" DataField="urgencia" />
                    <asp:BoundField HeaderText="IMPACTO" DataField="impacto" />
                </Columns>
                <EmptyDataTemplate>
                    <asp:Literal ID="Literal1" runat="server" Text="No hay registros" />
                </EmptyDataTemplate>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


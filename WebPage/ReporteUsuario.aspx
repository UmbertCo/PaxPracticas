<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true" validateRequest="false" CodeBehind="ReporteUsuario.aspx.cs" Inherits="WebPage.ReporteUsuario" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="formas">
<div>
<h1><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Resource_es,lblReporteAct%>"></asp:Literal></h1>
<p>
        <asp:Literal ID="ltlProyecto" runat="server" Text="<%$Resources:Resource_es,lblProyecto%>"></asp:Literal>
       
        <asp:DropDownList ID="ddlProyectos" runat="server" AutoPostBack="True" 
            TabIndex="2" onselectedindexchanged="ddlProyectos_SelectedIndexChanged">
        </asp:DropDownList>
    </p>
    <p>
            <asp:CheckBox ID="chkReporte" runat="server" Text="Reporte completo" 
                oncheckedchanged="chkReporte_CheckedChanged" AutoPostBack="True" />
       </p>
</div>
<div>
  <asp:Panel ID="Panel1" runat="server">
    <p> 
        
        <asp:Literal ID="ltlFechain" runat="server" Text="<%$Resources:Resource_es,lblFechain%>"></asp:Literal>
         <asp:Label ID="lblFormFechaIni" runat="server" Text="<%$Resources:Resource_es,lblFormatoFecha%>"></asp:Label>
        
        <asp:TextBox ID="txtFechain" runat="server" TabIndex="3" MaxLength="10"></asp:TextBox>
        <asp:RegularExpressionValidator ID="revFechaini" runat="server" 
            ErrorMessage="*" ControlToValidate="txtFechain" ForeColor="Red" 
            ValidationExpression="\d{2}\/\d{2}\/\d{4}" ValidationGroup="ValidarFechas">
        </asp:RegularExpressionValidator>
         <asp:Image ID="imgCalendarioin" runat="server" 
            ImageUrl="~/Imagenes/Calendario.jpg" />
     &nbsp
                 <asp:Literal ID="ltlFechafin" runat="server" Text="<%$Resources:Resource_es,lblFechafin%>"></asp:Literal>
                 <asp:Label ID="lblFormFechaFin" runat="server" Text="<%$Resources:Resource_es,lblFormatoFecha%>"></asp:Label>
            
        <asp:TextBox ID="txtFechafin" runat="server" TabIndex="4" MaxLength="10" ></asp:TextBox> 
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
            ErrorMessage="*" ControlToValidate="txtFechafin" ForeColor="Red" 
            ValidationExpression="\d{2}\/\d{2}\/\d{4}" ValidationGroup="ValidarFechas">
        </asp:RegularExpressionValidator>
        <asp:Image ID="imgcalendariofin" runat="server" 
            ImageUrl="~/Imagenes/Calendario.jpg" />
    
</p>
    </asp:Panel>
<table>
    <tr>
      <td>
          <cc1:CalendarExtender ID="clFechaIni" runat="server" 
              PopupButtonID="imgCalendarioin" TargetControlID="txtFechain" 
              Format="dd/MM/yyyy">
          </cc1:CalendarExtender>
      </td>
      <td>
          <cc1:CalendarExtender ID="clFechaFin" runat="server" 
              PopupButtonID="imgCalendariofin" TargetControlID="txtFechafin" 
              Format="dd/MM/yyyy">
          </cc1:CalendarExtender>
      </td>
    </tr>
  </table>
  </div>
<table>
    <tr>
        <td>
        <div>
    <table> 
        <tr>
            <td>
                <asp:GridView ID="gdvReporte"  CssClass="tables"
            DataKeyNames="Nombre,Tarea,Inicio,Pausa,DescripcionPausa,Terminar,DescripcionTerminar"
            runat = "server" AutoGenerateColumns = "False" 
            
            BackColor="White" CellPadding="4" 
                    >
        <Columns>
            <asp:BoundField HeaderText="Nombre" DataField="Nombre" Visible="False" />
            <asp:BoundField DataField="Tarea" HeaderText="Tarea">
            </asp:BoundField>
            <asp:BoundField DataField="Inicio" HeaderText="Inicio de Tarea" />
            <asp:BoundField DataField="Pausa" HeaderText ="hora de pausa" />
            <asp:BoundField DataField="DescripcionPausa" 
                HeaderText="Descripcion de pausa" />
            <asp:BoundField DataField="Terminar" HeaderText="hora finalizada" />
            <asp:BoundField DataField="DescripcionTerminar" 
                HeaderText="Descripcion terminada" />
        </Columns>
        </asp:GridView>
                <br />
                <br />
            </td>
        </tr>
    </table>
</div>
        </td>
    </tr>
</table>

<p style="background:none; border:none; text-align:right;">
            <asp:Button ID="mostrar" runat="server" Text="Reporte" onclick="mostrar_Click" CssClass="btn"
                TabIndex="5" ValidationGroup="ValidarFechas" />
        
            <asp:Button ID="btnExportar" runat="server" Text="Exportar" CssClass="btn"
                onclick="btnExportar_Click" TabIndex="6" />
        </p>

<asp:LinkButton ID="lkbSeleccionDeReporte" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlSeleccion" runat="server"
     TargetControlID = "lkbSeleccionDeReporte"
     PopupControlID = "pnlReporte" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlReporte" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center" class="style6">
            <h3>
                <asp:Label ID="lblSelec" runat="server" Text="<%$Resources:Resource_es,lblSelec%>"></asp:Label>
            </h3>
            </td>
        </tr>
        </table>
        <table width="100%">
        <tr>
            <td align="center">
                <asp:Button ID="btnRerporte" runat="server" Text="Si" CssClass="btn"
                    onclick="btnRerporte_Click"/>
            </td>
        </tr>
    </table>
    </asp:Panel>

    <asp:LinkButton ID="lkbAviso" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlAviso" runat="server"
     TargetControlID = "lkbAviso"
     PopupControlID = "pnlAvisoReporte" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlAvisoReporte" runat="server"
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblMensaje" runat="server" Text="<%$Resources:Resource_es,lblSelecFecha%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnAvisoReporteOK" runat="server" Text="Si" CssClass="btn"/>
            </td>
        </tr>
    </table>
    </asp:Panel>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true"  validateRequest="false" CodeBehind="AsignarTarea.aspx.cs" Inherits="PAXActividades.NuevaTarea" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .style6
        {
            width: 274px;
        }
        .style7
        {
            width: 80px;
        }
        </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="formas">
<div align="center">
    <h1>
        <asp:Literal ID="ltlTituloTareas" runat="server" Text="<%$Resources:Resource_es,lblTituloTareas %>"></asp:Literal>
    </h1>
</div>
<div id="Principal">
<p>       <span class="green">1 </span><asp:Literal ID="ltlCatProyecto" runat="server" Text="<%$Resources:Resource_es,lblCatProyecto%>"></asp:Literal>
            
                <asp:DropDownList ID="ddlProyectos" runat="server" TabIndex="1" 
                    AutoPostBack="True">
                </asp:DropDownList>
            </p>
           <p>
                <span class="green">2 </span><asp:Literal ID="ltlCatPersonal" runat="server" Text="<%$Resources:Resource_es,lblCatPersonal%>"></asp:Literal>
                
                <asp:DropDownList ID="ddlPersonal" runat="server" TabIndex="2" 
                    AutoPostBack="True">
                </asp:DropDownList>
           </p>
                <p>
                <span class="green">3 </span><asp:Literal ID="ltlTiempo" runat="server" Text="<%$Resources:Resource_es,lblTiempo%>"></asp:Literal>
                
                <asp:TextBox ID="txtHoras" runat="server" Width="68px" TabIndex="3" class="txt"
                MaxLength="2"></asp:TextBox>
                <asp:Literal ID="ltlRango" runat="server" Text="<%$Resources:Resource_es,lblRango%>"></asp:Literal>
                
                <asp:RequiredFieldValidator ID="rfvHorasVacio" runat="server" 
                ControlToValidate="txtHoras" Text="*" ForeColor="Red" 
                ValidationGroup="vldNueva"></asp:RequiredFieldValidator>

             <cc1:filteredtextboxextender ID="fteHoras" runat="server" 
            TargetControlID="txtHoras" FilterType="Numbers" ValidChars="+-=/*().">
            </cc1:filteredtextboxextender >
        
        <asp:RangeValidator ID="rvHoras" runat="server" ControlToValidate="txtHoras" 
            ErrorMessage="Rango de Numero no validos" ForeColor="Red" MaximumValue="72" 
            MinimumValue="1" ValidationGroup="vldNueva" Type="Integer">
        </asp:RangeValidator>
   </p>
    
    <p><span class="green">4 </span><asp:Literal ID="lblAsignarTarea" runat="server" Text="<%$Resources:Resource_es,lblAsignarTarea%>"></asp:Literal>
    <span style="font-weight:normal; "><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Resource_es,lblEligeTarea%>"></asp:Literal></span>
    </p>
    <div style="overflow: auto; width: 869px; height: 206px;" >
        <asp:GridView ID="gdvTareas" runat="server" AutoGenerateColumns="False" CssClass="tables"
            onselectedindexchanged="gdvTareas_SelectedIndexChanged" 
            DataKeyNames="IdTarea,Tarea" Width= "500px"  
            Height="200px">
            <Columns>
                <asp:TemplateField ShowHeader="False" ItemStyle-Width="10px">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                            CommandName="Select" ImageUrl="Imagenes/selecc.jpg" Text="Select" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IdTarea" Visible="False">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("IdTarea") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("IdTarea") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tarea">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Tarea") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Tarea") %>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Descripcion">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" 
                            Text='<%# Bind("DescripcionTarea") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("DescripcionTarea") %>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
                <table class="style1">
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnNuevaTarea" runat="server" onclick="btnNuevaTarea_Click" 
                                TabIndex="4" Text="Asignar tarea" ValidationGroup="vldNueva" CssClass="btn2" />
                        </td>
                    </tr>
                </table>
                <br />
                <p>
                <span class="green">5 </span><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Resource_es,lblVerificaTarea%>"></asp:Literal>
                </p>
         <table width="100%">
             <tr>
                 <td class="style6">
                    <div>
                     <asp:Button ID="btnTareaAsig" runat="server" 
                     TabIndex="4" Text="Tareas Asignadas" CssClass="btn2" 
                            onclick="btnTareaAsig_Click" />
                    </div>
                 </td>
             </tr>
         </table>
    </div>
<br />
<%--<hr style="height: -12px; width: 922px" />--%>
    <asp:LinkButton ID="lkbNuevo" runat="server" CssClass="btn"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlNuevaTarea" runat="server"
     TargetControlID = "lkbNuevo"
     PopupControlID = "pnlenviar" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlenviar" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblmdlAgregarTarea" runat="server" Text="<%$Resources:Resource_es,lblmdlAgreTarea%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnOK" runat="server" Text="Si" CssClass="btn"
                    onclick="btnOK_Click"/>
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn"
                    onclick="btnCancelar_Click"/>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:LinkButton ID="lkbAvisoTarea" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlAvisoTarea" runat="server"
     TargetControlID = "lkbAvisoTarea"
     PopupControlID = "pnlAvisoTarea" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlAvisoTarea" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblmdlAvisoTarea" runat="server" Text="<%$Resources:Resource_es,lblmdlSelecTarea%>"></asp:Label> 
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center"><asp:Button ID="btnSelecTareaAceptar" runat="server" Text="Aceptar" CssClass="btn"/>
            </td>
        </tr>
    </table>
    </asp:Panel>

    <asp:LinkButton ID="lkbexiste" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlexiste" runat="server"
     TargetControlID = "lkbexiste"
     PopupControlID = "pnlexiste" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlexiste" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblmdlExiste" runat="server" Text="<%$Resources:Resource_es,lblmdTareaExiste%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center"><asp:Button ID="btnexisteOk" runat="server" Text="Aceptar" CssClass="btn"
                    onclick="btnexisteOk_Click"/>
            </td>
        </tr>
    </table>
    </asp:Panel>

    <asp:LinkButton ID="lkbAviso" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlAviso" runat="server"
     TargetControlID = "lkbAviso"
     PopupControlID = "pnlAviso" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlAviso" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblmdlAvisoTareaAsig" runat="server" Text="<%$Resources:Resource_es,lblmdlTareaAsig%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center"><asp:Button ID="btnAceptar" runat="server" Text="Aceptar" 
                    CssClass="btn" onclick="btnAceptar_Click"                    />
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:LinkButton ID="lkbEditar" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlModificarTarea" runat="server"
     TargetControlID = "lkbEditar"
     PopupControlID = "pnlModificarTarea" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlModificarTarea" runat="server" 
    CssClass="modalPopup">
    <table>
    <tr>
        <td> <asp:Label ID="lblmodlpersonal" runat="server" Text="<%$Resources:Resource_es,lblPersonal%>"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="ddlSelecPersonal" runat="server" 
                onselectedindexchanged="ddlSelecPersonal_SelectedIndexChanged" 
                AutoPostBack="True">
            </asp:DropDownList>
        </td>
    </tr>
        <tr>
            <td>
                <div style="overflow: auto">
                     <asp:GridView ID="gdvTareasAsignadas" runat="server" CssClass="tables"
                         AutoGenerateColumns="False" 
                         onselectedindexchanged="gdvTareasAsignadas_SelectedIndexChanged" 
                         DataKeyNames="IdAsig_Tarea,IdProyecto,Horas,IdTarea,id_estatus">
                         <Columns>
                             <asp:CommandField ShowSelectButton="True" />
                             <asp:BoundField DataField="IdAsig_Tarea" HeaderText="IdAsig_Tarea" 
                                 Visible="False" />
                             <asp:BoundField HeaderText="IdProyecto" Visible="False" 
                                 DataField="IdProyecto" />
                             <asp:BoundField DataField="NomProyecto" HeaderText="Proyecto" />
                             <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                             <asp:BoundField DataField="Horas" HeaderText="Horas" />
                             <asp:BoundField DataField="id_estatus" HeaderText="id_estatus" 
                                 Visible="False" />
                             <asp:BoundField DataField="IdTarea" HeaderText="IdTarea" Visible="False" />
                             <asp:BoundField DataField="Descricpion" HeaderText="Estatus" />
                         </Columns>
                     </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnEditCancelar" runat="server" Text="Cancelar" CssClass="btn"/>
            </td>
        </tr>
    </table>
    </asp:Panel>
    </div>
</asp:Content>

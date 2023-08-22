<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true"  validateRequest="false" CodeBehind="ActTarea.aspx.cs" Inherits="WebPage.ActTarea" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="formas" style=" padding-bottom:10px">
<div align="center">
  <h1>
    <asp:Label ID="lblActuTarea" runat="server" Text="<%$Resources:Resource_es,lblActTarea%>"></asp:Label>
  </h1>
</div>   
<div style="width: 921px; overflow: scroll; height: 426px;">
<table>
    <tr>
        <td align="center">
            <asp:GridView ID="gdvTareas" runat="server" AutoGenerateColumns="False" CssClass="tables"
            DataKeyNames="IdTarea,Tarea,DescripcionTarea" 
            onselectedindexchanged="gdvTareas_SelectedIndexChanged" BorderWidth="1px" 
            Height="254px" Width="100%">
            <Columns>
                <asp:CommandField ShowSelectButton="True" ButtonType="Image" 
                    SelectImageUrl="Imagenes/edit.png" HeaderText="Editar" />
                <asp:BoundField DataField="IdTarea" HeaderText="Id Tarea" Visible="False"/>
                <asp:TemplateField HeaderText="Tarea">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Tarea") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                    <div style="width:350px; overflow:hidden;">
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Tarea") %>'></asp:Label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle BackColor="#AAA9A9" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Descripcion">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" 
                            Text='<%# Bind("DescripcionTarea") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                    <div style="width:350px; overflow:hidden;">
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("DescripcionTarea") %>'></asp:Label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle BackColor="#AAA9A9" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>    
        </td>
    </tr>
</table>
</div>
<br />
    <asp:LinkButton ID="lkbCancelar" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlConfirmarCancelacion" runat="server"
     TargetControlID = "lkbCancelar"
     PopupControlID = "pnlConfirmarCancelacion" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlConfirmarCancelacion" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align ="center">
            <h3>
                <asp:Label ID="lbmdlCancelar" runat="server" Text="<%$Resources:Resource_es,lblCancelar%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
            <asp:Button ID="btnOK" runat="server" Text="Si" CssClass="btn"
                    onclick="btnOK_Click"/>
                <asp:Button ID="btnNo" runat="server" Text="No" CssClass="btn"/>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:LinkButton ID="lkbActualizar" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlActualizar" runat="server"
     TargetControlID = "lkbActualizar"
     PopupControlID = "pnlConfirmarActualizacion" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlConfirmarActualizacion" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align ="center">
            <h3>
                <asp:Label ID="lblmdlActualizarTarea" runat="server" Text="<%$Resources:Resource_es,lblmdlActTaraSelec%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnSi" runat="server" Text="SI" onclick="btnSi_Click" CssClass="btn"/>
                <asp:Button ID="btnNos" runat="server" Text="No" onclick="btnNos_Click" CssClass="btn"/>
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
                <asp:Label ID="lblmdlAvisoTarea" runat="server" Text="<%$Resources:Resource_es,lblmdlAvisoTarea%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnAvisoOk" runat="server" Text="Si" CssClass="btn" />
            </td>
        </tr>
    </table>
    </asp:Panel>
<asp:LinkButton ID="lkbBaja" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlBaja" runat="server"
     TargetControlID = "lkbBaja"
     PopupControlID = "pnlConfirmarBaja" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlConfirmarBaja" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
          <td align="center"> 
            <h3>
                <asp:Label ID="lblConfirmarBaja" runat="server" Text="<%$Resources:Resource_es,lblConfirmarBaja%>" ></asp:Label>
            </h3>
          </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnBajaSi" runat="server" Text="SI" onclick="btnBajaSi_Click" CssClass="btn" />
                <asp:Button ID="btnBajaNo" runat="server" Text="No" CssClass="btn"/>
            </td>
        </tr>
    </table>
    </asp:Panel>
<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>--%>
    <asp:LinkButton ID="lkbMensaje" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlMensaje" 
    runat="server" 
    TargetControlID="lkbMensaje"
    PopupControlID="pnlMensaje"
    BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlMensaje" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
     <tr>
       <td align="center">
       <h3>
           <asp:Label ID="lblMensaje" runat="server"></asp:Label>
       </h3>
       </td>
     </tr>
     <tr>
       <td align="center">
           <asp:Button ID="bs" runat="server" Text="Si" CssClass="btn" />
       </td>
     </tr>
    </table>
    </asp:Panel>
<%--</ContentTemplate>

</asp:UpdatePanel>--%>

<asp:LinkButton ID="lkbActualizarTarea" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlActualizarTarea" 
    runat="server" 
    TargetControlID="lkbActualizarTarea"
    PopupControlID="pnlActualizarTarea"
    BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlActualizarTarea" runat="server" 
    CssClass="modalPopup">
     <table>
        <tr>
            <td>
             <asp:Literal ID="ltlTarea" runat="server" Text="<%$Resources:Resource_es,lblTarea%>"></asp:Literal>
             </td>
             <td>
             <asp:TextBox ID="txtTarea" runat="server" TabIndex="1" Height="20px" 
                     MaxLength="100"></asp:TextBox>
            </td>
            <td>
            <asp:RequiredFieldValidator ID="rfvTarea" runat="server" 
                    ControlToValidate="txtTarea" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="vldActualizar"></asp:RequiredFieldValidator>
            </td>
        </tr>
        </table>
        <table>
        <tr>
            <td><asp:Literal ID="ltlDescripcion" runat="server" Text="<%$Resources:Resource_es,lblDescripcion%>"></asp:Literal>
            <br />
                <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" 
                Height="132px" Width="486px" TabIndex="2" MaxLength="8000"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" 
                ControlToValidate="txtDescripcion" ErrorMessage="*" ForeColor="Red" 
                ValidationGroup="vldActualizar"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <table align="right">
            <tr>
                <td>
                    <asp:Button ID="btnBaja" runat="server" Text="Baja" CssClass="btn"
                        style="margin-left: 0px" onclick="btnBaja_Click" 
                        TabIndex="3" />
                </td>
                <td>
                    <asp:Button ID="btnCancelar" runat="server" onclick="btnCancelar_Click" CssClass="btn"
                        Text="Cancelar" TabIndex="4" />
                </td>
                <td>
                    <asp:Button ID="btnActualizar" runat="server" onclick="btnActualizar_Click" CssClass="btn"
                        Text="Actualizar" ValidationGroup="vldActualizar" TabIndex="5" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true"  validateRequest="false" CodeBehind="ActProyecto.aspx.cs" Inherits="WebPage.WebForm5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />   
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="formas" style="padding-bottom:10px;">
<div align="center">
    <h1>
        <asp:Literal ID="ltlTitulo" runat="server" Text="<%$Resources:Resource_es,lblActProyecto%>"></asp:Literal></h1>
    </div>
<div style="width: 921px; overflow: scroll; height: 426px;">
<table>
<tr>
    <td align="center">
        <asp:GridView ID="gdvProyectos" runat="server" AutoGenerateColumns="False" CssClass="tables"
        onselectedindexchanged="gdvProyectos_SelectedIndexChanged" 
        DataKeyNames="IdProyecto,NomProyecto,DescProyecto" CellPadding="4" 
         GridLines="Horizontal" BackColor="#AAA9A9" 
         BorderColor="#AAA9A9" BorderStyle="Double" BorderWidth="1px" 
        Height="254px" Width="100%">
        <Columns>
            <asp:CommandField ButtonType="Image" SelectImageUrl="Imagenes/edit.png" 
                ShowSelectButton="True" HeaderText="Editar">
            </asp:CommandField>
            
            <asp:TemplateField HeaderText="IdProyecto" Visible="False">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("IdProyecto") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("IdProyecto") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nombre">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("NomProyecto") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <div style="width:200px; overflow:hidden"><asp:Label ID="Label1" runat="server" Text='<%# Bind("NomProyecto") %>'></asp:Label></div>
                </ItemTemplate>
                <ItemStyle/>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Descripcion">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("DescProyecto") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                   <div style="width:200px; overflow:hidden"><asp:Label ID="Label2" runat="server" Text='<%# Bind("DescProyecto") %>'></asp:Label></div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
            <FooterStyle BackColor="333" />
            <HeaderStyle Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#d0d0d0" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="White" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#666464" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F7F7F7" />
            <SortedAscendingHeaderStyle BackColor="#487575" />
            <SortedDescendingCellStyle BackColor="#E5E5E5" />
            <SortedDescendingHeaderStyle BackColor="#CCCCCC" />
    </asp:GridView>
    </td>
</tr>
</table>
</div>
<asp:LinkButton ID="lkbNuevo" runat="server"></asp:LinkButton>
<cc1:ModalPopupExtender ID="mdlActualizar" runat="server"
     TargetControlID = "lkbNuevo"
     PopupControlID = "pnlActualizar" DropShadow ="false"
     BackgroundCssClass="modalBackground">
 </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlActualizar" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align ="center">
            <h3>
                <asp:Label ID="lblmdlActualizar" runat="server" Text="<%$Resources:Resource_es,lblmdlActProyecto%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align = "center"> 
            <asp:Button ID="btnSi" runat="server" Text="Si" onclick="btnSi_Click" CssClass="btn" />
                <asp:Button ID="btnNo" runat="server" Text="No" onclick="btnNo_Click" CssClass="btn"  />
            </td>
        </tr>
    </table>
    </asp:Panel>
     <asp:LinkButton ID="lkbAviso" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mdlAviso" runat="server"
     TargetControlID = "lkbAviso"
     PopupControlID = "pnlAviso" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlAviso" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align ="center">
            <h3>
                <asp:Label ID="lblmdlAviso" runat="server" Text="<%$Resources:Resource_es,lblmdlAvisoProyecto%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnAvisoOk" runat="server" Text="Aceptar" CssClass="btn"
                    onclick="btnAvisoOk_Click"/>
            </td>
        </tr>
    </table>
  </asp:Panel>
    <asp:LinkButton ID="lkbCancelar" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mdlConfirmarCancelacion" runat="server"
     TargetControlID = "lkbCancelar"
     PopupControlID = "pnlConfirmarCancelacion" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlConfirmarCancelacion" runat="server" 
    CssClass="modalPopup" Width= "220px">
    <table width="100%">
        <tr>
            <td align = "center">
            <h3>
                <asp:Label ID="lblmdlCancelar" runat="server" Text="<%$Resources:Resource_es,lblCancelar%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
            <asp:Button ID="btnCancelarSi" runat="server" 
                    Text="Si" onclick="btnCancelarSi_Click" CssClass="btn" />
                <asp:Button ID="btnCancelarNo" runat="server" Text="No" CssClass="btn" />
            </td>
        </tr>
    </table>
    </asp:Panel>
<asp:LinkButton ID="lkbBaja" runat="server"></asp:LinkButton>
<cc1:ModalPopupExtender ID="mdlBaja" runat="server"
     TargetControlID = "lkbBaja"
     PopupControlID = "pnlBaja" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlBaja" runat="server" 
    CssClass="modalPopup">
    <table width="100%" >
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblmdlBaja" runat="server" Text="<%$Resources:Resource_es,lblmdlBaja%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
            <asp:Button ID="btnBajaSi" runat="server" 
                    Text="Si" onclick="btnBajaSi_Click" CssClass="btn"/>
                <asp:Button ID="btnBajaNo" runat="server" Text="No" CssClass="btn"  />
            </td>
        </tr>
    </table>
    </asp:Panel>
  <asp:LinkButton ID="lkconfirmarcambio" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlAvisoCambio" runat="server"
     TargetControlID = "lkconfirmarcambio"
     PopupControlID = "pnlAvisoCambio" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlAvisoCambio" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblcambio" runat="server" Text=""></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
               <asp:Button ID="Button1" runat="server" Text="Aceptar" CssClass="btn"/>
            </td>
        </tr>
    </table>
    </asp:Panel>


    <asp:LinkButton ID="lkEditarProyecto" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlEditarProyecto" runat="server"
     TargetControlID = "lkEditarProyecto"
     PopupControlID = "pnlEditarProyecto" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:modalpopupextender>
    <asp:Panel ID="pnlEditarProyecto" runat="server" 
    CssClass="modalPopup" Height="197px">
    <div>
<br />
    <table>
        <tr>
            <td>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Resource_es,lblNombreProyecto%>"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtNomProyecto" runat="server" Width="180px" MaxLength="100" 
                    TabIndex="1"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="revNombreProyecto" runat="server" 
                    ControlToValidate="txtNomProyecto" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="vldActualizar"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <div>
        <asp:Literal ID="ltlDescripcion" runat="server" Text="<%$Resources:Resource_es,lblDescripcion%>"></asp:Literal>
    </div>
        <div>

            <table >
                <tr>
                    <td >
                        <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" 
                        Width="350px" TabIndex="2" MaxLength="200" Height="50px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="revDescripcion" runat="server" 
                            ControlToValidate="txtDescripcion" ErrorMessage="*" ForeColor="Red" 
                            ValidationGroup="vldActualizar"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>

        </div>
        <div>

            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblIdProyecto" runat="server"  Visible="False"></asp:Label>
                    </td>
                </tr>
             </table>
             <table align="right">
                <tr>
                    <td>
                        <asp:Button ID="btnBaja" runat="server" Text="Baja" onclick="btnBaja_Click" 
                            TabIndex="3" CssClass="btn"/>
                    </td>
                    <td >
                        <asp:Button ID="btnCandcelar" runat="server" Text="Cancelar" 
                            onclick="btnCandcelar_Click" TabIndex="4" CssClass="btn" />
                    </td>
                    <td>
                        <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" 
                            onclick="btnActualizar_Click" ValidationGroup="vldActualizar" 
                            TabIndex="5" CssClass="btn"/>
                    </td>
                </tr>
                </table>
        </div>
</div>
    </asp:Panel>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true" validateRequest="false" CodeBehind="ActPersonal.aspx.cs" Inherits="WebPage.WebForm3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
            width: 81px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="formas">
<div>
<h1 align="center"><asp:Literal ID="ltlTitulo" runat="server" Text="<%$Resources:Resource_es,lblActualizaPersonal%>" /></h1>
</div>
<div style="width: 921px; overflow: scroll; height: 426px;">
    <table align="center" style="overflow: auto">
        <tr>
            <td align="center">
                <asp:GridView ID="gdvPersonal" runat="server" CssClass="tables"
                DataKeyNames="IdPersonal,Nombre,Direccion,ApPaterno,ApMaterno,Correo,Telefono,Puesto,IdPuesto" 
                onselectedindexchanged="gdvPersonal_SelectedIndexChanged" 
                    AutoGenerateColumns="False" Height="254px" Width="100%" >
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="Imagenes/edit.png" ShowSelectButton="True" HeaderText="Editar"/>
                        
                        <asp:BoundField DataField="IdPersonal" HeaderText="idpersonal" 
                            Visible="False" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" ControlStyle-Width="50px">
                        <ControlStyle Width="50px"></ControlStyle>
                        <HeaderStyle BackColor="#aaa9a9" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ApPaterno" HeaderText="Apellido Paterno">
                        <HeaderStyle BackColor="#aaa9a9" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ApMaterno" HeaderText="Apellido Materno">
                        <HeaderStyle BackColor="#aaa9a9" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Direccion" HeaderText="Direccion">
                        <HeaderStyle BackColor="#aaa9a9" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Telefono" HeaderText="Telefono">
                        <HeaderStyle BackColor="#aaa9a9" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Correo" HeaderText="Correo">
                        <HeaderStyle BackColor="#aaa9a9" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Puesto" HeaderText="Puesto">
                        <HeaderStyle BackColor="#aaa9a9" />
                        </asp:BoundField>
                        <asp:BoundField DataField="estatus" HeaderText="Estatus" Visible="False" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    </div>
    <hr />
    <asp:LinkButton ID="lkbCancelar" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mdlConfirmarCancelacion" runat="server"
     TargetControlID = "lkbCancelar"
     PopupControlID = "pnlConfirmarCancelacion" DropShadow ="false" 
     BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlConfirmarCancelacion" runat="server" 
         CssClass="modalPopup" >
    <table width = "100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblCancelar" runat="server" Text="<%$Resources:Resource_es,lblCancelar%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center"><asp:Button ID="btnOK" runat="server" Text="Si" CssClass="btn"
                    onclick="btnOK_Click" />
                <asp:Button ID="btnNo" runat="server" Text="No" CssClass="btn"/>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:LinkButton ID="lkbActualizar" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mdlActualizar" runat="server"
     TargetControlID = "lkbActualizar"
     PopupControlID = "pnlConfirmarActualizacion" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlConfirmarActualizacion" runat="server" 
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblActualizarPersonal" runat="server" Text="<%$Resources:Resource_es,lblActualizarPersonal%>"></asp:Label>
            </h3>
            </td>
        </tr>

        <tr>
            <td align="center">
                <asp:Button ID="btnSi" runat="server" Text="Si" onclick="btnSi_Click" CssClass="btn"/>
                <asp:Button ID="btnNos" runat="server" Text="No" onclick="btnNos_Click" CssClass="btn" />
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
            <td align = "center">
            <h3>
                <asp:Label ID="lblSelecPersonal" runat="server" Text="<%$Resources:Resource_es,lblActualizarPersonal%>"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align= "center">
                <asp:Button ID="btnAvisoOk" runat="server" Text="Aceptar" CssClass="btn"
                    onclick="btnAvisoOk_Click"/>
            </td>
        </tr>
    </table>
    </asp:Panel>
<asp:LinkButton ID="lkbBaja" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mdlBaja" runat="server"
     TargetControlID = "lkbBaja"
     PopupControlID = "pnlConfirmarBaja" DropShadow ="false"
     BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlConfirmarBaja" runat="server"
    CssClass="modalPopup">
    <table width="100%">
        <tr>
            <td align= "center">
            <h3>
                <asp:Label ID="lblmdlConfirmarBaja" runat="server" Text="<%$Resources:Resource_es,lblmdlConfirmarBaja%>"></asp:Label> 
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnBajaSi" runat="server" Text="Si" onclick="btnBajaSi_Click" CssClass="btn"/>
                <asp:Button ID="btnBajaNo" runat="server" Text="No" CssClass="btn"/>
            </td>
        </tr>
    </table>
    </asp:Panel>


     <asp:LinkButton ID="LkAviso" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="ModalPopupExtender1" 
    runat="server" 
    TargetControlID="LkAviso" PopupControlID="pnlAvisoCambio2"
    BackgroundCssClass="modalBackground">

    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlAvisoCambio2" runat="server" 
    CssClass="modalPopup">
    <table width ="100%">
        <tr>
            <td align= "center">
            <h3>
                <asp:Label ID="lblcambio2" runat="server" Text=""></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="Button1" runat="server" Text="Si" CssClass="btn" />
            </td>
        </tr>
    </table>
    </asp:Panel>
    </div>

     <asp:LinkButton ID="LinkButtonEditar" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="ModalPopupEditar" 
    runat="server" 
    TargetControlID="LinkButtonEditar" PopupControlID="PanelEditar"
    BackgroundCssClass="modalBackground">

    </cc1:ModalPopupExtender>
    <asp:Panel ID="PanelEditar" runat="server"
    CssClass="modalPopup" Height="260" Width="842px">
          <table align="center">
            <tr>
                <td>
                    <asp:Literal ID="ltlNombre" runat="server" Text="<%$Resources:Resource_es,lblNombre%>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtNombre" runat="server" Width="350px" MaxLength="20" 
                        TabIndex="1"></asp:TextBox>
                </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="revNombre" runat="server" 
                    ControlToValidate="txtNombre" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="vldActualizar"></asp:RequiredFieldValidator>
            </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltlApPaterno" runat="server" Text="<%$Resources:Resource_es,lblApPaterno%>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtApPaterno" runat="server" Width="350px" MaxLength="20" 
                        TabIndex="2"></asp:TextBox>
                </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="revApPaterno" runat="server" 
                    ControlToValidate="txtApPaterno" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="vldActualizar"></asp:RequiredFieldValidator>
            </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltlApMaterno" runat="server" Text="<%$Resources:Resource_es,lblApMaterno%>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtApMaterno" runat="server" Width="350px" MaxLength="20" 
                        TabIndex="3"></asp:TextBox>
                </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="revApMaterno" runat="server" 
                    ControlToValidate="txtApMaterno" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="vldActualizar"></asp:RequiredFieldValidator>
            </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltlDireccion" runat="server" Text="<%$Resources:Resource_es,lblDireccion%>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDireccion" runat="server" Width="350px" MaxLength="100" 
                        TabIndex="4"></asp:TextBox>
                </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="revDireccion" runat="server" 
                    ControlToValidate="txtDireccion" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="vldActualizar"></asp:RequiredFieldValidator>
            </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltlTelefono" runat="server" Text="<%$Resources:Resource_es,lblTelefono%>"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtTelefono" runat="server" Width="250px" MaxLength="20" TabIndex="5" 
                        ></asp:TextBox>
                </td>
            <td align="left">
               <small> <asp:Literal ID="ltlFormato" runat="server" Text="<%$Resources:Resource_es,lblFormatoTelefono%>"></asp:Literal></small>
                <asp:RequiredFieldValidator ID="revTelefono" runat="server" 
                    ControlToValidate="txtTelefono" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="vldActualizar"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revTelefonoformato" runat="server" 
                    ErrorMessage="Telefono Invalido" ControlToValidate="txtTelefono" 
                    ForeColor="Red" ValidationExpression="\d{3}\-\d{7}" 
                    ValidationGroup="vldActualizar"></asp:RegularExpressionValidator>
            </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltlCorreo" runat="server" Text="<%$Resources:Resource_es,lblCorreo%>"></asp:Literal>
                </td>
                <td class="style7">
                    <asp:TextBox ID="txtCorreo" runat="server" Width="350px" MaxLength="100" 
                        TabIndex="6"></asp:TextBox>
                </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="revCorreo" runat="server" 
                    ControlToValidate="txtCorreo" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="vldActualizar"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revCorreoformato" runat="server" 
                    ErrorMessage="Direccion no Valida" ControlToValidate="txtCorreo" 
                    ForeColor="Red" 
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                    ValidationGroup="vldActualizar"></asp:RegularExpressionValidator>
            </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="ltlPuesto" runat="server" Text="<%$Resources:Resource_es,lblPuesto%>"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPuesto" runat="server" Width="180px" TabIndex="7">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblestatus" runat="server" TabIndex="11"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            </table>

            <table align="right">
            <tr>
                <td>
                    <asp:Button ID="btnBaja" CssClass="btn" runat="server" Text="Baja" 
                        onclick="btnBaja_Click" TabIndex="8" />
                </td>
                <td>
                    <asp:Button ID="btnCancelar" CssClass="btn" runat="server" onclick="btnCancelar_Click" 
                        Text="Cancelar" TabIndex="9" />
                </td>
                <td>
                    <asp:Button ID="btnActualizar" CssClass="btn" runat="server" onclick="btnActualizar_Click" 
                        Text="Actualizar" ValidationGroup="vldActualizar" TabIndex="10" />
                </td>
            </tr>
        </table>
        <asp:Label ID="lblIdpersonal" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lblpuesto" runat="server" Visible="false"></asp:Label>  
    </asp:Panel>
</asp:Content>

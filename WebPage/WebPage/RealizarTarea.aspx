<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true"  validateRequest="false" CodeBehind="RealizarTarea.aspx.cs" Inherits="PAXActividades.RealizarTarea" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="formas">
    <div id="divmarco">
<h1 align="center">
    <asp:Literal ID="ltlTituloRealizarTarea" runat="server"></asp:Literal>Realizar Tarea</h1>
    
    
          <div>
          <asp:UpdatePanel ID="upd_pnl_gdvtareas" runat="server">
                 <ContentTemplate>
                     <p>
                  <asp:Label ID="lblProyecto" runat="server" Text="<%$Resources:Resource_es,lblProyecto%>"> 
                  </asp:Label><asp:DropDownList ID="ddlProyectos" runat="server" 
                    onselectedindexchanged="ddlProyectos_SelectedIndexChanged" AutoPostBack="True" 
                         TabIndex="1">
                    </asp:DropDownList>

                    </p>
                    <br />

                     <div style="overflow: auto" >
                    <asp:GridView ID="gdvTareas" runat="server" AutoGenerateColumns="False" CssClass="tables"
                    DataKeyNames="IdAsig_Tarea,IdTarea,Tarea,Descricpion"  
                     Width="100%" onselectedindexchanged="gdvTareas_SelectedIndexChanged" 
                             Height="217px">
                     <Columns>
                         <asp:CommandField ButtonType="Image" SelectImageUrl="Imagenes/selecc.jpg" 
                             ShowSelectButton="True" >
                         <HeaderStyle Width="80px" />
                         </asp:CommandField>
                       <asp:BoundField DataField="IdAsig_Tarea" HeaderText="Id_asig_Tarea" 
                             Visible="False" >
                       </asp:BoundField>
                         <asp:BoundField DataField="IdTarea" HeaderText="IdTareat" Visible="False" />
                         
                       <asp:BoundField DataField="Tarea" HeaderText="Tareas" >
                       <HeaderStyle Width="250px" BackColor="#aaa9a9" Wrap="True" />
                       </asp:BoundField>
                         <asp:BoundField DataField="DescripcionTarea" HeaderText="Descripcion" >
                         <HeaderStyle BackColor="#AAA9A9" Width="300px" />
                         </asp:BoundField>
                         <asp:BoundField DataField="Horas" HeaderText="Horas">
                         <HeaderStyle Width="20px" />
                         </asp:BoundField>
                         <asp:BoundField DataField="Descricpion" HeaderText="Estatus" >
                         <HeaderStyle Width="100px" BackColor="#aaa9a9" />
                         </asp:BoundField>
                     </Columns>
                       
                    </asp:GridView>
                    </div>
                    <asp:Label ID="lblTareaSel" runat="server" Text="" ></asp:Label>
                    <br />
                     <table>
                 <tr>
                     <td >
                         <asp:Button ID="btninicio" runat="server" onclick="btninicio_Click" CssClass="btn"
                             Text="Inicio" TabIndex="2" />
                     </td>
                     <td >
                         <asp:Button ID="btnpausar" runat="server" onclick="btnpausar_Click" CssClass="btn"
                             Text="Pausar" TabIndex="3" />
                     </td>
                     <td >
                         <asp:Button ID="btnterminar" runat="server" onclick="btnterminar_Click" CssClass="btn"
                             Text="Terminar" TabIndex="4" />
                     </td>
                 </tr>
             </table>
                    <asp:LinkButton ID="lkbRealizartarea" runat="server"></asp:LinkButton>
                     <cc1:ModalPopupExtender ID="mdlConfirmarTarea" runat="server" 
                         DropShadow="false" PopupControlID="pnlConfirmarTarea" 
                         TargetControlID="lkbRealizartarea"
                         BackgroundCssClass="modalBackground">
                     </cc1:ModalPopupExtender>
                     <asp:Panel ID="pnlConfirmarTarea" runat="server" 
                     CssClass="modalPopup">
                         <table width="100%">
                             <tr>
                                <td align="center">
                                <h3>
                                    <asp:Label ID="lblmdlConfirmarTarea" runat="server" Text="<%$Resources:Resource_es,lblmdlconfTarea%>"></asp:Label>
                                </h3>
                                <h3>
                                <asp:Label ID="lblSelNoTarea" runat="server"></asp:Label>
                                </h3>
                                </td>
                             </tr>
                             <tr>
                                 <td align="center">
                                     &nbsp;<asp:Button ID="btnConfirmarSi" runat="server" Text="Si" CssClass="btn"
                                         onclick="btnConfirmarSi_Click" />
                                     <asp:Button ID="btnConfirmarNo" runat="server" Text="No" CssClass="btn"
                                         onclick="btnConfirmarNo_Click"/>
                                 </td>
                             </tr>
                         </table>
                     </asp:Panel>
             <br />
            <asp:LinkButton ID="lkbReporte" runat="server"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="mdlReporte" runat="server"
             TargetControlID = "lkbReporte"
             PopupControlID = "pnlReporte" DropShadow ="false"
             BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlReporte" runat="server" 
            CssClass="modalPopup">
            <table width="100%" >
                <tr>
                <td>
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblReporte" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                    <td align="center">
                        <asp:TextBox ID="txtReporte" runat="server" TextMode="MultiLine" 
                                 Width="600px" Height="83px"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="rfvDescPausa" runat="server" 
                            ControlToValidate="txtReporte" ErrorMessage="*" ForeColor="Red" 
                            ValidationGroup="validardescrip"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="center" >
                    <asp:Button ID="btnPausaCancelar" runat ="server" Text= "Cancelar" CssClass = "btn" />
                        <asp:Button ID="btnReporte" runat="server" Text="Si" onclick="btnReporte_Click" CssClass="btn"
                            ValidationGroup="validardescrip" />
                    </td>
                </tr>
                </table>
                </td>
                </tr>
            </table>
            </asp:Panel>
             <br />
            <asp:LinkButton ID="lkbNotificacion" runat="server"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="mdlNotificacion" runat="server"
            TargetControlID = "lkbNotificacion"
            PopupControlID = "pnlNotificacion" DropShadow ="false"
            BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlNotificacion" runat="server" 
            CssClass="modalPopup">
            <table width="100%">
            <tr>
                <td align="center">
                <h3>
                    <asp:Label ID="lblAviso" runat="server" Text=""></asp:Label>
                </h3>
                </center>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnNotifiOk" runat="server" Text="Si" CssClass="btn"/>
                </td>
            </tr>
            </table>
            </asp:Panel>
            <br />
            <asp:LinkButton ID="lkbAviso" runat="server"></asp:LinkButton>
            <cc1:ModalPopupExtender ID="mdlAviso" runat="server"
             TargetControlID = "lkbAviso"
             PopupControlID = "pnlAviso" DropShadow ="false"
             BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlAviso" runat="server" 
            CssClass="modalPopup">
            <table width ="100%" >
                <tr>
                    <td align="center">
                    <h3>
                        <asp:Label ID="lblmdlAviso" runat="server" Text="<%$Resources:Resource_es,lblmdlAvisoTarea%>"></asp:Label>
                    </h3>
                    </td>
                </tr>
                <tr>
                    <td  align="center">
                        <asp:Button ID="btnAvisoOk" runat="server" Text="Si" CssClass="btn"/>
                    </td>
                </tr>
            </table>
            </asp:Panel>
           </ContentTemplate>
          </asp:UpdatePanel>
            </div>
            
</div>
</div>
</asp:Content>

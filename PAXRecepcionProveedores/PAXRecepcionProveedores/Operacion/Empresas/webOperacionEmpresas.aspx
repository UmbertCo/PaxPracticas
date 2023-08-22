<%@ Page Title="Empresas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="webOperacionEmpresas.aspx.cs" Inherits="PAXRecepcionProveedores.Operacion.Empresas.WebForm1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="~/Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script src="~/Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
<script src="~/Scripts/jquery.ui.core.js" type="text/javascript"></script>
<script src="~/Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
<script src="~/Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
<script src="~/Scripts/progressbar.js" type="text/javascript"></script>
<link href="~/Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <script language="javascript" type="text/javascript">
     function fntraerEnter(keyStroke) {
         isNetscape = (document.layers);
         eventChooser = (isNetscape) ? keyStroke.which : event.keyCode;
         if (eventChooser == 13) {
             return false;
         }
     }
     document.onkeypress = fntraerEnter; 
    </script>
<br /><br /><br />
    <h2>
        <asp:Label ID="lblTitulo" runat="server" 
            Text="<%$ Resources:resCorpusCFDIEs, lblEmpresas %>" Font-Bold="True" 
            ForeColor="#8B181B"></asp:Label>
    </h2>

    <div style="border-style: ridge; border-width: 2px; border-color: #8B181B">
   
        <asp:UpdatePanel ID="upAltaEmpresa" runat="server" >
            <ContentTemplate>
                 <table style="margin: 0px auto; width: 800px; ">
                    <tr >
                        <td align="left">
                           <asp:Label ID="Label1" runat="server" AssociatedControlID="ddlEmpresas" Text="<%$ Resources:resCorpusCFDIEs, lblListaEmpresas %>"/>
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="left">
                            <asp:DropDownList ID="ddlEmpresas" runat="server"  Width="326px" />          
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                             <asp:Button ID="btnNuevo" runat="server" CssClass="botonEstiloVentanas" 
                               Text="<%$ Resources:resCorpusCFDIEs, lblNuevoCorreo %>" 
                                 onclick="btnNuevo_Click" Width="80px" TabIndex="4" />
                                  <asp:Button ID="btnGuardar" runat="server" CssClass="botonEstiloVentanas" 
                                onclick="btnGuardar_Click" Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                                Width="80px" TabIndex="6" ValidationGroup="RegisterUserValidationGroup" />
                                 <asp:Button ID="btnEditar" runat="server" CssClass="botonEstiloVentanas" 
                                onclick="btnEditar_Click" Text="<%$ Resources:resCorpusCFDIEs, lblEditar %>" 
                                Width="80px" TabIndex="6" />
                                <asp:Button ID="btnBorrar" runat="server" CssClass="botonEstiloVentanas" 
                                 onclick="btnBorrar_Click" Text="<%$ Resources:resCorpusCFDIEs, btnBorrar %>" 
                                 Width="80px" TabIndex="5"  />
                               
                               
                                <asp:Button ID="btnNCancelar" runat="server" CssClass="botonEstiloVentanas" 
                                onclick="btnNCancelar_Click" 
                                 Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="80px" 
                                    TabIndex="7" />    
                        </td>
                    </tr>

                </table>
                <table style="margin: 0px auto; width: 800px; ">
                    <tr>
                        <td>
                            <asp:Label ID="lblEmpresa" AssociatedControlID="txtNombreEmpresa" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                        </td>
                        <td>
                             <asp:Label ID="lblRFC" runat="server" AssociatedControlID="txtRFC" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNombreEmpresa" runat="server" Width="326px" Enabled="false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNombreEmpresa" runat="server" 
                                        ControlToValidate="txtNombreEmpresa" CssClass="failureNotification" Display="Dynamic" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, txtNombre %>" 
                                        ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../../Imagenes/error_sign.gif" />
                                         </asp:RequiredFieldValidator>
                                         
                        </td>
                        <td>
                            <asp:TextBox ID="txtRfc" runat="server" Width="140px" Enabled="false"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valRFC" runat="server" 
                                        ControlToValidate="txtRfc" CssClass="failureNotification" Display="Dynamic" 
                                        ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtRFC %>" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, txtRFC %>" 
                                        ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../../Imagenes/error_sign.gif" /> </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regxRFC" runat="server" 
                                        ControlToValidate="txtRfc" CssClass="failureNotification" Display="Dynamic" 
                                        ErrorMessage ="<%$ Resources:resCorpusCFDIEs, regxRFC %>" 
                                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxRFC %>" 
                                        ValidationExpression="[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]" 
                                        ValidationGroup="RegisterUserValidationGroup"><img 
                                        src="../../Imagenes/error_sign.gif" /> </asp:RegularExpressionValidator>
                         </td>
                    </tr>
                    <tr>
                    <td>
                        <asp:Label ID="lblLogo" runat="server" AssociatedControlID="fupLogo" Text="<%$ Resources:resCorpusCFDIEs, lblLogo %>"/>
                    </td>
                </tr>
                <tr>
                    <td>
                    <asp:FileUpload ID="fupLogo" runat="server" Width="358px" Enabled="false" />
                    </td>
                </tr>
                </table>
                <br />
                <br />
               
                
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnBorrar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnEditar" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnGuardar" />
            </Triggers>
        </asp:UpdatePanel>
        
    </div>
    

</asp:Content>

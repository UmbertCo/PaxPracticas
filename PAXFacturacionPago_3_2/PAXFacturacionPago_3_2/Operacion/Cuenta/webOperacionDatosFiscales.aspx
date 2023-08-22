<%@ Page Title="Cuenta" Language="C#" MasterPageFile="~/webOperatorMaster.master"  AutoEventWireup="true" CodeFile="webOperacionDatosFiscales.aspx.cs" Inherits="webOperacionDatosFiscales" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../../WebControls/usrGlobalPwd.ascx" tagname="usrGlobalPwd" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">


    <link href="../../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />

    <style type="text/css">

        .fontText
        {
            font: 8pt verdana;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ContentTemplate>
    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, hedDatosFisc %>"></asp:Label>
    </h2>

    <asp:HiddenField ID="hdIdRfc" runat="server" Visible="False"></asp:HiddenField>
    <asp:HiddenField ID="hdIdEstructura" runat="server" Visible="False"></asp:HiddenField>

    <div class="">
    <fieldset class="register" style=" height:450px; width:830px;" >

    <legend><asp:Literal runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubCuenta %>" /></legend>

    <table>
    <tr>
    <td style="width:400px; vertical-align:top;" >
            <p>
                <asp:Label ID="lblRFC" runat="server" AssociatedControlID="txtRFC" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblRFC %>"></asp:Label>
                <asp:TextBox ID="txtRFC" runat="server" CssClass="textEntry" TabIndex="1" 
                    MaxLength="50" Enabled="False"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblRazonSocial" runat="server" AssociatedControlID="txtRazonSocial" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblNombreCompleto %>"></asp:Label>
                <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="textEntry" 
                    ReadOnly="True" TabIndex="2" MaxLength="255" Enabled="False"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblSucursal" runat="server" AssociatedControlID="txtSucursal" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblNombreSucursal %>"></asp:Label>
                <asp:TextBox ID="txtSucursal" runat="server" CssClass="textEntry" 
                    MaxLength="255" TabIndex="3"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSucursal" runat="server" 
                    ControlToValidate="txtSucursal" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valSucursal %>" 
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
            </p>
         
                <p>
                    <asp:Label ID="lblPais" runat="server"
                        Text="<%$ Resources:resCorpusCFDIEs, lblPais %>" 
                        AssociatedControlID="ddlPais" ></asp:Label>
                    <asp:DropDownList ID="ddlPais" runat="server" Width="300px" AutoPostBack="True" 
                        DataTextField="pais" DataValueField="id_pais" 
                        onselectedindexchanged="ddlPais_SelectedIndexChanged" TabIndex="4"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvPais" runat="server" 
                        ControlToValidate="ddlPais" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valPais %>" 
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                </p><p>
                    <asp:Label ID="lblEstado" runat="server"
                        Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>" 
                        AssociatedControlID="ddlEstado" ></asp:Label>
                        <asp:DropDownList ID="ddlEstado" runat="server" Width="300px" 
                        DataTextField="estado" DataValueField="id_estado" TabIndex="5"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvEstado" runat="server" 
                        ControlToValidate="ddlEstado" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valEstado %>" 
                        ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
                </p>
           
            <p>
                <asp:Label ID="lblMunicipio" runat="server"
                    Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>" 
                    AssociatedControlID="txtMunicipio" ></asp:Label>
                <asp:TextBox ID="txtMunicipio" runat="server" CssClass="textEntry" 
                    MaxLength="255" TabIndex="6"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMunicipio" runat="server" 
                    ControlToValidate="txtMunicipio" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valMunicipio %>" 
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
            </p><p>
                <asp:Label ID="lblLocalidad" runat="server"
                    Text="<%$ Resources:resCorpusCFDIEs, lblLocalidad %>" 
                    AssociatedControlID="txtLocalidad" ></asp:Label>
                <asp:TextBox ID="txtLocalidad" runat="server" CssClass="textEntry" 
                    MaxLength="255" TabIndex="7"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblRegimenFiscal" runat="server"
                    Text="<%$ Resources:resCorpusCFDIEs, lblRegimenfiscal %>" 
                    AssociatedControlID="txtRegimenFiscal" ></asp:Label>
                <asp:TextBox ID="txtRegimenFiscal" runat="server" CssClass="textEntry" 
                    TabIndex="8"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvRegimenFiscal" runat="server" 
                    ControlToValidate="txtRegimenFiscal" CssClass="failureNotification" 
                    ToolTip="<%$ Resources:resCorpusCFDIEs, valRegimenFiscal %>"
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
            </p>
    </td>
    <td style="vertical-align:top; width:400px;" >
            <p>
                <asp:Label ID="lblCalle" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>" 
                    AssociatedControlID="txtCalle"></asp:Label>
                <asp:TextBox ID="txtCalle" runat="server" CssClass="textEntry" MaxLength="255" 
                    TabIndex="9"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCalle" runat="server" 
                    ControlToValidate="txtCalle" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCalle %>" 
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="lblNoExterior" runat="server"  
                    Text="<%$ Resources:resCorpusCFDIEs, lblNoExterior %>" 
                    AssociatedControlID="txtNoExterior"></asp:Label>
                <asp:TextBox ID="txtNoExterior" runat="server" CssClass="textEntry" 
                    MaxLength="10" TabIndex="10"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblNoInterior" runat="server"  
                    Text="<%$ Resources:resCorpusCFDIEs, lblNoInterior %>" 
                    AssociatedControlID="txtNoInterior"></asp:Label>
                <asp:TextBox ID="txtNoInterior" runat="server" CssClass="textEntry" 
                    MaxLength="10" TabIndex="11"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblColonia" runat="server"
                    Text="<%$ Resources:resCorpusCFDIEs, lblColonia %>" 
                    AssociatedControlID="txtColonia"></asp:Label>
                <asp:TextBox ID="txtColonia" runat="server" CssClass="textEntry" 
                    MaxLength="255" TabIndex="12"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblReferenciaDom" runat="server"
                    Text="<%$ Resources:resCorpusCFDIEs, lblReferenciaDom %>" 
                    AssociatedControlID="txtReferencia" ></asp:Label>
                <asp:TextBox ID="txtReferencia" runat="server" CssClass="textEntry" 
                    MaxLength="255" TabIndex="13"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="LablblCodigoPostalel5" runat="server"
                    Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>" 
                    AssociatedControlID="txtCodigoPostal" ></asp:Label>
                <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="textEntry" 
                    MaxLength="5" TabIndex="14"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="txtCodigoPostal_fteSoloNumeros" runat="server" 
                    Enabled="True" FilterType="Numbers" TargetControlID="txtCodigoPostal">
                </cc1:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="rfvCodigoPostal" runat="server" 
                    ControlToValidate="txtCodigoPostal" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCodigoPostal %>" 
                    ValidationGroup="RegisterUserValidationGroup" Width="16px">*</asp:RequiredFieldValidator>
            </p>
            <p>
                       <asp:Label ID="lblNombreSucursal0" runat="server" 
                           Text="<%$ Resources:resCorpusCFDIEs, lbllogo %>"></asp:Label>
            </p>
            <p>
                <asp:ImageButton ID="imgLogo" runat="server" Height="22px" 
                    ImageUrl="~/Imagenes/lupa.png" onclick="imgLogo_Click" Width="22px" 
                    TabIndex="16" />
&nbsp;
                           <asp:FileUpload ID="fupLogo" runat="server" TabIndex="15" />
            </p>
    </td>
    </tr>
    </table>
    </fieldset>
        <br />
<%--             <table cellpadding="0" cellspacing="0" 
                 style="border: thin solid #CCCCCC; width: 856px" ID="tbl3_2" 
                 runat="server" visible="false"   >
                 <tr >
                     <td align="left" width="20px" height="50px" >
                         &nbsp;</td>
                     <td align="left" width="400px" height="50px" >
                         <asp:Label ID="lblExpedio" runat="server" 
                             Text="<%$ Resources:resCorpusCFDIEs, varExpedidoEn %>" 
                             style="font-weight: 700"></asp:Label>
                         <br  />
                     </td>
                     <td align="left" >
                     </td>
                 </tr>
                 <tr >
                     <td align="left" >
                         &nbsp;</td>
                     <td align="left" width="360px" >
                         <asp:Label ID="lblPais0" runat="server" AssociatedControlID="ddlPais0" 
                             Text="<%$ Resources:resCorpusCFDIEs, lblPais %>"></asp:Label>
                         <br  />
                         <asp:DropDownList ID="ddlPais0" runat="server" AutoPostBack="True" 
                             DataTextField="pais" DataValueField="id_pais" 
                             onselectedindexchanged="ddlPais_SelectedIndexChanged" TabIndex="4" 
                             Width="200px">
                         </asp:DropDownList>
                     </td>
                     <td align="left" >
                         <asp:Label ID="lblCalle0" runat="server" AssociatedControlID="txtCalleEmisor" 
                             Text="<%$ Resources:resCorpusCFDIEs, lblCalle %>"></asp:Label>
                         <br  />
                         <asp:TextBox ID="txtCalleEmisor" runat="server" CssClass="textEntry" 
                             MaxLength="255" TabIndex="15" Width="150px"></asp:TextBox>
                     </td>
                 </tr>
                 <tr >
                     <td align="left">
                         &nbsp;</td>
                     <td align="left">
                         <asp:Label ID="lblEstado0" runat="server" AssociatedControlID="ddlEstado0" 
                             Text="<%$ Resources:resCorpusCFDIEs, lblEstado %>"></asp:Label>
                         <br  />
                         <asp:DropDownList ID="ddlEstado0" runat="server" 
                             DataTextField="estado" DataValueField="id_estado" TabIndex="5" 
                             Width="200px">
                         </asp:DropDownList>
                     </td>
                     <td align="left" >
                         <asp:Label ID="LablblCodigoPostalel6" runat="server" 
                             AssociatedControlID="txtCodigoPostalEmisor" 
                             Text="<%$ Resources:resCorpusCFDIEs, lblCodigoPostal %>"></asp:Label>
                         <br />
                         <asp:TextBox ID="txtCodigoPostalEmisor" runat="server" CssClass="textEntry" 
                             MaxLength="5" TabIndex="16" Width="150px"></asp:TextBox>
                         <cc1:FilteredTextBoxExtender ID="txtCodigoPostalEmisor_FilteredTextBoxExtender" 
                             runat="server" FilterType="Numbers" 
                             TargetControlID="txtCodigoPostalEmisor">
                         </cc1:FilteredTextBoxExtender>
                     </td>
                 </tr>
                 <tr>
                     <td align="left">
                         &nbsp;</td>
                     <td align="left">
                         <asp:Label ID="lblMunicipio0" runat="server" 
                             AssociatedControlID="txtMunicipioEmisor" 
                             Text="<%$ Resources:resCorpusCFDIEs, lblMunicipio %>"></asp:Label>
                         <br />
                         <asp:TextBox ID="txtMunicipioEmisor" runat="server" CssClass="textEntry" 
                             MaxLength="255" TabIndex="14" Width="200px"></asp:TextBox>
                         <br />
                     </td>
                     <td>
                     </td>
                 </tr>
                 <tr>
                     <td align="left">
                         &nbsp;</td>
                     <td align="left">
                         &nbsp;</td>
                     <td>
                         &nbsp;</td>
                 </tr>
             </table>--%>
    </div>


            <p style=" padding-left:780px;">
                <asp:Button ID="btnActualizarDomicilio" runat="server" CommandName="MoveNext"  CssClass="botonEstilo" 
                ValidationGroup="RegisterUserValidationGroup" 
                Text="<%$ Resources:resCorpusCFDIEs, btnActualizar %>"
                onclick="btnActualizarDomicilio_Click" TabIndex="17" />
            </p>
         
    
    </ContentTemplate>
</asp:Content>


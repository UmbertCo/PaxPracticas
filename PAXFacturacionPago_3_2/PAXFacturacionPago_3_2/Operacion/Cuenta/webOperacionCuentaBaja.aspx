<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionCuentaBaja.aspx.cs" Inherits="Operacion_Cuenta_webOperacionCuentaBaja" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="../../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <asp:UpdatePanel ID="upBaja" runat="server">
    <ContentTemplate>
        <h2>
            <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloCuenta %>"></asp:Label>
        </h2>
        <div style="text-align:center;" >
            <asp:UpdateProgress ID="uppSoporte" runat="server" AssociatedUpdatePanelID="upBaja" >
                    <progresstemplate>
                        <img alt="" 
                    src="../../Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>
            </div>
        <div class="">
            <fieldset class="register" style="width:400px; height:150px; background:white;">
            <legend><asp:Literal runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubBajaCuenta %>" /></legend>
                <p>
                    <asp:Label ID="lblBajaCuenta" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblBajaCuenta %>"></asp:Label>
                </p>
                <p>
                    <asp:Label ID="lblContraseniaBaja" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>" 
                        AssociatedControlID="txtPassBaja"></asp:Label>
                    <asp:TextBox ID="txtPassBaja" runat="server" TextMode="Password" Width="300px" ValidationGroup="grupoPassBaja" 
                        TabIndex="1" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvBaja" runat="server" 
                        ControlToValidate="txtPassBaja" ValidationGroup="grupoPassBaja" 
                        CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valConfirmaNueva %>"></asp:RequiredFieldValidator>
                        <div style=" visibility:collapse;">
                    <asp:TextBox ID="TextBox1" runat="server" BorderStyle="None" Height="0px" Width="0px"></asp:TextBox>
                    </div>
                    <p>
                    </p>
                    <p style="text-align:right;">
                        &nbsp;</p>
                    <p>
                    </p>
                    <p>
                    </p>
                    <p>
                    </p>
                    <p>
                    </p>
                </p>
            </fieldset>
        </div>
        <p style="padding-left:340px;">
            <asp:Button ID="btnPassBaja" runat="server" ValidationGroup="grupoPassBaja" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnPassBajaProc %>"  
                TabIndex="2" OnClientClick="return bajaUsuario();" 
                onclick="btnPassBajaModal_Click" CssClass="botonEstilo" Width="86px" 
                        ></asp:Button>
        </p>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


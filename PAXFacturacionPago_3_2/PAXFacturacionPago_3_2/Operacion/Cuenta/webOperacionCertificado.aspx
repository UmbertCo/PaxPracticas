<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionCertificado.aspx.cs" Inherits="Operacion_Cuenta_webOperacionDatosFiscales" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="../../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <h2>
        <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, hedCertificados %>"></asp:Label>
    </h2>

    <asp:HiddenField ID="hdIdRfc" runat="server" Visible="False"></asp:HiddenField>
    <asp:HiddenField ID="hdIdEstructura" runat="server" Visible="False"></asp:HiddenField>
    <asp:HiddenField ID="hdRfc" runat="server" Visible="False"></asp:HiddenField>
    <%--<asp:UpdatePanel ID="udpCer" runat="server">
        <ContentTemplate>--%>
            <div class="">
                <fieldset class="register" style="height:200px; width:400px;">
                    <legend>
                        <asp:Literal ID="Literal1" runat="server" 
                Text="<%$ Resources:resCorpusCFDIEs, lblSubCuentaCertificados %>" />
                    </legend>
                    <p>
                        <asp:Label ID="lblKey" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblKey %>" 
                        AssociatedControlID="fupKey"></asp:Label>
                        <asp:FileUpload ID="fupKey" runat="server" Width="300px" TabIndex="15"  />
                        <asp:RequiredFieldValidator ID="rfvKey" runat="server" 
                        ControlToValidate="fupKey" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valKey %>" 
                        ValidationGroup="grupoCertificado"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblCer" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblCer %>" 
                        AssociatedControlID="fupCer"></asp:Label>
                        <asp:FileUpload ID="fupCer" runat="server" Width="300px" TabIndex="16" />
                        <asp:RequiredFieldValidator ID="rfvCer" runat="server" 
                        ControlToValidate="fupCer" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valCer %>" 
                        ValidationGroup="grupoCertificado"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblContrasenia" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>" 
                        AssociatedControlID="txtPass"></asp:Label>
                        <asp:TextBox ID="txtPass" runat="server" TextMode="Password" Width="300px" 
                        TabIndex="17" MaxLength="250"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPass" runat="server" 
                        ControlToValidate="txtPass" CssClass="failureNotification" ToolTip="<%$ Resources:resCorpusCFDIEs, valContrasenia %>" 
                        ValidationGroup="grupoCertificado"></asp:RequiredFieldValidator>
                    </p>
                </fieldset>
            </div>

        <p style="padding-left:350px;" >
        <asp:Button ID="btnActualizarCertificados" runat="server" CommandName="MoveNext" 
        ValidationGroup="grupoCertificado" 
        Text="<%$ Resources:resCorpusCFDIEs, btnActualizar %>" 
        onclick="btnActualizarCertificados_Click" TabIndex="18" CssClass="botonEstilo" />
        </p>

       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    
    </asp:Content>


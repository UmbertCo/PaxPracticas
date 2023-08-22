<%@ Page Title="" Language="C#" MasterPageFile="~/webOperatorMaster.master" AutoEventWireup="true" CodeFile="webOperacionGeneraContrasena.aspx.cs" Inherits="Operacion_Distribuidores_Default" %><%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
                

                    <h2>
        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, hedPass %>"></asp:Label>
    </h2>
                <fieldset class="register" style=" height:250px; width:800px;">
                                <legend>
                                    <asp:Literal ID="Literal2" runat="server" 
                                        Text="<%$ Resources:resCorpusCFDIEs, lblContrasenaCorreo %>" />
                                </legend>

                                                <p>
                    <asp:Label ID="lblTitulo" runat="server" 
                              Text="<%$ Resources:resCorpusCFDIEs, varPas64 %>"></asp:Label>
                </p>
                <p>
                    <asp:Label ID="lblContrasena" runat="server" 
                              Text="<%$ Resources:resCorpusCFDIEs, lblContrasenaCorreo %>"></asp:Label>
                </p>
                <p>
                    <asp:TextBox ID="txtValue" runat="server" CssClass="textEntry" MaxLength="255" 
                              TabIndex="2" TextMode="Password"></asp:TextBox>
                </p>
                <p>
                    <asp:Label ID="lblBase64" runat="server" 
                              Text="<%$ Resources:resCorpusCFDIEs, varPas64Val %>"></asp:Label>
                </p>
                <p>
                    <asp:TextBox ID="txtValue64" runat="server" CssClass="textEntry" 
                                MaxLength="255" ReadOnly="True" TabIndex="2" Width="786px"></asp:TextBox>
                </p>
                <p>
                    <asp:Button ID="btnGenerar" runat="server" CommandName="MoveNext" 
                                CssClass="botonEstilo" Height="35px" OnClick="btnGenerar_Click" TabIndex="6" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                                ValidationGroup="AgregarCreditos" Width="80px" />
                </p>

                            </fieldset>


</asp:Content>


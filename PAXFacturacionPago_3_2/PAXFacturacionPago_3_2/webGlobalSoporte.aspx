<%@ Page Title="Soporte" Language="C#" MasterPageFile="~/webGlobalMaster.master" AutoEventWireup="true" CodeFile="webGlobalSoporte.aspx.cs" Inherits="webGlobalSoporte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">


            <h2>
                <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblTituloSoporte %>"></asp:Label>
            </h2>
            <div style="text-align:center;" >
          <%--  <asp:UpdateProgress ID="uppSoporte" runat="server" AssociatedUpdatePanelID="upSoporte" >
                    <progresstemplate>
                        <img alt="" 
                    src="Imagenes/imgAjaxLoader.gif" />
                    </progresstemplate>
            </asp:UpdateProgress>--%>
            </div>
            <div class="">
                <fieldset class="register" style=" width:890px;">
                <legend><asp:Literal runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSubSoporte %>" /></legend>
                    <p>
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSoporteAgradecimiento %>"
                             ></asp:Label>&nbsp;
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSoporteLeyenda %>"
                             ></asp:Label>
                    </p>
                    <p>
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSoporteAsunto %>"
                             AssociatedControlID="ddlAsunto" ></asp:Label>
                        <asp:DropDownList ID="ddlAsunto" runat="server" 
                            DataTextField="tipo_incidente" DataValueField="id_tipo_incidente" 
                            Height="19px" Width="280px">
                        </asp:DropDownList>
                    </p>
                    <p>
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblSoporteDescipcion %>"
                             AssociatedControlID="txtDescripcion" ></asp:Label>
                        <asp:TextBox ID="txtDescripcion" runat="server" Height="150px" 
                            TextMode="MultiLine" Width="700px" MaxLength="4000"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" ValidationGroup="grupoSoporte"  ToolTip="<%$ Resources:resCorpusCFDIEs, valSoporteDescripcion %>"
                            ControlToValidate="txtDescripcion" CssClass="failureNotification" ></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="lblArchivo" runat="server" AssociatedControlID="fuAsunto" 
                            Text="<%$ Resources:resCorpusCFDIEs, lbldatosprueba %>"></asp:Label>
                    </p>
                    <p>
                        <asp:FileUpload ID="fuAsunto" runat="server" />
                    </p>
                    <p>
                        &nbsp;</p>
                </fieldset>
            </div>
            <p style="text-align:right;" >
                <asp:Button ID="btnEnviar" runat="server" ValidationGroup="grupoSoporte" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnEnviar %>" 
                    onclick="btnEnviar_Click1" CssClass="botonEstilo" />
            </p>
     
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true"  validateRequest="false" CodeBehind="Login.aspx.cs" Inherits="WebPage.WebForm6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id= "marco">
<div class="gaffete">
    
    <div class="form">
                <p class="Label"> <asp:Literal ID="ltlUsuario" runat="server" Text="<%$Resources:Resource_es,lblUsuario%>"></asp:Literal></p>
            
                <asp:TextBox ID="txtUsuario" CssClass="inputLogin" runat="server" 
                    TabIndex="1"></asp:TextBox>
            
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtUsuario" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="validalogin"></asp:RequiredFieldValidator>
           
             <p class="Label" style="margin-top:5px;"><asp:Literal ID="ltlContrasena" runat="server" Text="<%$Resources:Resource_es,lblContrasena%>"></asp:Literal></p>
             
                <asp:TextBox ID="txtContraseña" runat="server" CssClass="inputLogin"
                    TextMode="Password" TabIndex="2"></asp:TextBox>
            
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtContraseña" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="validalogin"></asp:RequiredFieldValidator>
           <br />
                <asp:Label ID="lblerror" runat="server" class="leyendaError"
                    Text="<%$ Resources:Resource_es,lblErrorUser %>" Font-Size="10pt" 
                    ForeColor="Red" Visible="False"></asp:Label>
            
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" class="btnLogin"
                    onclick="btnAceptar_Click" ValidationGroup="validalogin" TabIndex="3" />
            
                <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="Label2" runat="server" Visible="false"></asp:Label>
                </div>
            
</div>
</div>
</asp:Content>




<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="practicandoASP.cuentas.login" MasterPageFile="~/Site1.master" %>

<asp:Content ID="encabezado" runat="server" ContentPlaceHolderID="headContenido">
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        
        #contenido
        {  
            height: 300px;
        }
        
        #usuario
        {
            position: relative;
            left: 70px;
            width: 170px;
            float: left;
        }
        
        #campos
        {
            left: 50px;
            position: relative;
        }             
    </style>
    <title>Acceso a cuenta</title>
</asp:Content>
<asp:Content ID="MContenido" runat="server" ContentPlaceHolderID="MContenido">
        <div id = "contenido">

        <div id="usuario">
            <img src="../Imagenes/user.jpg"" style="height: 139px" />
        </div>
        <div id="campos">
            Usuario:<br />
            <asp:TextBox ID="txtUsuario" runat="server" CssClass="textbox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtUsuario" ErrorMessage="RequiredFieldValidator" 
                Font-Bold="True" ForeColor="Red">Ingresa tu usuario.</asp:RequiredFieldValidator>
            <br />
            <br />
            Contraseña:<br />
            <asp:TextBox ID="txtPassword" runat="server" CssClass="textbox" 
                TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="txtPassword" ErrorMessage="RequiredFieldValidator" 
                Font-Bold="True" ForeColor="Red">Ingresa tu contraseña.</asp:RequiredFieldValidator>
            <br />
            <br />
            <asp:Button ID="btnEntrar" runat="server" Text="Entrar" 
                onclick="btnEntrar_Click" />
        </div>
        </div>
</asp:Content>

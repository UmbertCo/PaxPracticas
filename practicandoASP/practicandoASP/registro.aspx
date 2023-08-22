<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registro.aspx.cs" Inherits="practicandoASP.registro" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 365px;
        }
        .style3
        {
            width: 10px;
        }
        #contenido
        {
            margin-top: 20px;
            width: 78%;
            margin-left: 130px; 
            height: 350px;
        } 
       #form1
        {
            height: 650px; 
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <div id="navegacionMenu">
                    <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" CssClass="NavegacionMenu" >
                        <Items>
                            <asp:MenuItem Text="INICIO" Value="INICIO" NavigateUrl="index.aspx"></asp:MenuItem>
                            <asp:MenuItem Text="MIS FAVORITOS" Value="MIS FAVORITOS">
                                <asp:MenuItem NavigateUrl="http://www.google.com.mx" Text="Google" 
                                    Value="Google"></asp:MenuItem>
                            </asp:MenuItem>
                        </Items>
                    </asp:Menu>
                </div>
        <a href="../index.aspx"><img src="Imagenes/bannerIndex.jpg" border="0" 
                style="width: 860px" /></a>
        <br />
        
        <br />
        <br />
    </div>
    <div id="contenido">
  
            <table class="style1">
                <tr>
                    <td class="style2">
    
            Usuario:
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtUsuario" ErrorMessage="RequiredFieldValidator" 
                            Font-Bold="True" ForeColor="Red">Debe ingresar un usuario.</asp:RequiredFieldValidator>
                        <br />
            <asp:TextBox ID="txtUsuario" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
            Password:
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="txtPass" ErrorMessage="RequiredFieldValidator" 
                            Font-Bold="True" ForeColor="Red">Debe ingresar una contraseña.</asp:RequiredFieldValidator>
                        <br />
            <asp:TextBox ID="txtPass" runat="server" ontextchanged="TextBox2_TextChanged" 
                            CssClass="textbox" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        <strong>
                        <br />
                        Datos personales</strong><br />
                        <br />
            Nombre(s):      <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="txtNombre" ErrorMessage="RequiredFieldValidator" 
                            Font-Bold="True" ForeColor="Red">Debe ingresar un nombre.</asp:RequiredFieldValidator>
            
            <br />
            <asp:TextBox ID="txtNombre" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        <br />
                        <br />
                        <br />
            Apellidos:  <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                            ControlToValidate="txtApellidos" ErrorMessage="RequiredFieldValidator" 
                            Font-Bold="True" ForeColor="Red">Debe ingresar sus apellidos.</asp:RequiredFieldValidator>
                         <br />
            <asp:TextBox ID="txtApellidos" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
        
                        <br />
        
            Edad:   <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                            ControlToValidate="txtEdad" ErrorMessage="RequiredFieldValidator" 
                            Font-Bold="True" ForeColor="Red">Debe ingresar su edad.</asp:RequiredFieldValidator>
                    <br />
            <asp:TextBox ID="txtEdad" runat="server" TextMode="Number" CssClass="textbox"></asp:TextBox>
                        <br />
                    </td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        <br />
            Telefono:   <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                            ControlToValidate="txtTelefono" ErrorMessage="RequiredFieldValidator" 
                            Font-Bold="True" ForeColor="Red">Debe ingresar su telefono.</asp:RequiredFieldValidator>
                        <br />
            <asp:TextBox ID="txtTelefono" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        <br />
            Sexo:<br />
            <asp:RadioButton ID="rdbMasculino" runat="server" Checked="True" 
                Text="Masculino" />
    &nbsp;<asp:RadioButton ID="rdbFemenino" runat="server" GroupName="rblSexo" 
                Text="Femenino" />
                    </td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        
            <br />
            <asp:Button ID="btnRegistrarme" runat="server" Text="Registrarme" 
                onclick="btnRegistrarme_Click" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnLimpiar" runat="server" onclick="btnLimpiar_Click" Text="Limpiar" />
        </div>
    </div>
    </form>
</body>
</html>

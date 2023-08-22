<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webInicio.aspx.cs" Inherits="webInicio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bienvenida</title>
    <style type="text/css">
        .style1
        {
            width: 1024px;
            height: 768px;
        }
        
        .page
        {
             background-position: center;
             
             background-repeat: no-repeat;
             background-attachment: scroll;
             background-image: url('mnu_images/entrada1024.jpg');
         width: 1300px;
            height: 768px;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div  class="page" >
        <br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
        <br /><br /><br /><br /><br /><br /><br /><br />
        
        <table>
            <tr><td style="width:260px"></td>
            <td>
            <div style="width:362px; height:269px">
            <asp:LinkButton ID="lnkCliente" runat="server" PostBackUrl="~/Operacion/Clientes/webDescargaComprobantes.aspx" >
                <img src="mnu_images/btn.jpg" border="0" />
                </asp:LinkButton>
            </div>
            </td>
            <td style="width:50px"></td>
            <td>
            <div style="width:362px; height:269px">
            <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/InicioSesion/webInicioSesionLogin.aspx">
                <img src="mnu_images/btn1.jpg" border="0" />
                </asp:LinkButton>
            </div>
            </td>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            
        </table>
        </div>
    </form>
</body>
</html>
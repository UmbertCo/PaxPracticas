<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="prueba.aspx.cs" Inherits="practicandoASP.prueba" MasterPageFile="~/Site1.master" %>

<asp:Content ID="encabezado" runat="server" ContentPlaceHolderID="headContenido">
</asp:Content>
    <asp:Content runat="server" ContentPlaceHolderID="MContenido">
            <div id="contenido">
        <table>
            <td>
            <div style="width:362px; height:269px">
            <asp:LinkButton ID="lnkCliente" runat="server" PostBackUrl="~/cuentas/login.aspx" >
                <img src="mnu_images/btn1.jpg" border="0" />
                </asp:LinkButton>
            </div>
            </td>
            <td style="width:50px"></td>
            <td>
            <div style="width:362px; height:269px">
            <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="registro.aspx">
                <img src="mnu_images/btn.jpg" border="0" />
                </asp:LinkButton>
            </div>    
        </table>
        </div>
        </asp:Content>
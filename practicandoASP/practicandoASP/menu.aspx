<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu.aspx.cs" Inherits="practicandoASP.menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/menu_style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>  
        <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" StaticPopOutImageUrl="~/mnu_images/drop-arrow.gif" DynamicPopOutImageUrl="~/mnu_images/right-arrow.gif">
        <LevelMenuItemStyles>
            <asp:MenuItemStyle CssClass="level1"/>
            <asp:MenuItemStyle CssClass="level2" />
            <asp:MenuItemStyle CssClass="level3" />
        </LevelMenuItemStyles>  
        <StaticHoverStyle BackColor="red" />
        <DynamicHoverStyle BackColor="blue" />
            <Items>
                <asp:MenuItem Text="Inicio" Value="Inicio"></asp:MenuItem>
                <asp:MenuItem Text="Cuenta" Value="Cuenta">
                    <asp:MenuItem Text="Iniciar sesión" Value="Iniciar sesión">
                        <asp:MenuItem Text="Cosa random" Value="Cosa random"/>
                        <asp:MenuItem Text="Cosa random2" Value="Cosa random2"/>
                    </asp:MenuItem>
                    <asp:MenuItem Text="Registro" Value="Registro"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="Otro item" Value="Otro item"></asp:MenuItem>
                <asp:MenuItem Text="Random" Value="Random"></asp:MenuItem>
            </Items>
        </asp:Menu>
    
    </div>
    </form>
</body>
</html>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="encabezado.ascx.cs" Inherits="practicandoASP.encabezado1" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <div class="header">
        <a href="../index.aspx"><img src="Imagenes/bannerIndex.jpg" border="0" /></a>
        <br />
        
        <br />
        <br />
    </div>
</head>
<body>
    <form id="form1" runat="server">

    <div>
        <div id="navegacionMenu">
            <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" CssClass="NavegacionMenu" >
                <Items>
                    <asp:MenuItem Text="INICIO" Value="INICIO"></asp:MenuItem>
                    <asp:MenuItem Text="MIS FAVORITOS" Value="MIS FAVORITOS">
                        <asp:MenuItem NavigateUrl="http://www.google.com.mx" Text="Google" 
                            Value="Google"></asp:MenuItem>
                    </asp:MenuItem>
                </Items>
            </asp:Menu>
        </div>
    </div>
    </form>
</body>
</html>


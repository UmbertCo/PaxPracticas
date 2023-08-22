<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="webGlobalError.aspx.cs" Inherits="webGlobalError" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">

 p.MsoNormal
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:10.0pt;
	margin-left:0cm;
	line-height:115%;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";
	}
        .style2
        {
            width: 299px;
            height: 298px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" Runat="Server">
    <table align="center" width="800">
        <tr>
            <td align="center">
                <p class="MsoNormal" 
                    style="font-size: medium; line-height: 115%; font-family: &quot;Segoe UI&quot;,&quot;sans-serif&quot;; color: dimgray">
                    <asp:Label ID="lblError" 
                        runat="server" Text="<%$ Resources:Resources, lblErroProblema %>" 
                        style="font-weight: 700"></asp:Label>
                </p>
                <p class="MsoNormal" 
                    style="font-size: medium; line-height: 115%; font-family: &quot;Segoe UI&quot;,&quot;sans-serif&quot;; color: dimgray">
                    <asp:Label 
                        ID="lblErrorDet" runat="server" 
                        Text="<%$ Resources:Resources, lblErrorDet %>" style="font-weight: 700"></asp:Label>
                </p>
                <p class="MsoNormal" 
                    style="font-size: medium; line-height: 115%; font-family: &quot;Segoe UI&quot;,&quot;sans-serif&quot;; color: dimgray">
                    <asp:Label ID="lblErrorDesc" runat="server" style="font-weight:700"></asp:Label>
                </p>
            </td>
        </tr>
        <tr>
            <td align="center">
                <img alt="warning" class="style2" src="Imagenes/warning.png" /></td>
        </tr>
        <tr>
            <td align="center" >
                <asp:HyperLink ID="lnkHome" runat="server" 
                    NavigateUrl="~/InicioSesion/webInicioSesionLogin.aspx" Text="<%$ Resources:Resources, hlkRegresar %>"></asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>


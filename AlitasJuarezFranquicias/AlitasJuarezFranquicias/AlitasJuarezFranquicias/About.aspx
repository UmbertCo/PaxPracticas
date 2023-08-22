<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="About.aspx.cs" Inherits="About" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        #FlashID5
        {
            height: 448px;
            width: 619px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table align="center" >
        <tr>
            <td align="center">
                <p class="MsoNormal" 
                    style="font-size: medium; line-height: 115%; font-family: &quot;Segoe UI&quot;,&quot;sans-serif&quot;; color: dimgray">
                    <asp:Label ID="lblCuentaCreada" runat="server"></asp:Label>
                </p>
                <p class="MsoNormal" 
                    style="font-size: medium; line-height: 115%; font-family: &quot;Segoe UI&quot;,&quot;sans-serif&quot;; color: dimgray">
                    <asp:Label  ID="lblCuentaDetalle" runat="server" ></asp:Label>
                </p>
                
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Image ID="imgAviso" runat ="server"  Visible="false" />
                                <%--panel de video de ayuda --%>
                
            </td>
        </tr>
        <tr>
            <td align="center" >
                <asp:HyperLink ID="lnkHome" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, hlkRegresar %>"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <%--modal de Video de Ayuda--%>
    <%--<asp:LinkButton ID="lbkVideoAyuda" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mpeVideoAyuda" runat="server"
     TargetControlID ="lbkVideoAyuda" 
     PopupControlID ="pnlVideoAyuda" 
      BackgroundCssClass ="modalBackground">
    </cc1:ModalPopupExtender>--%>
    <%--video ayuda--%>
</asp:Content>

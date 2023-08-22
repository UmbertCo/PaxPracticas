<%@ Page Title="" Language="C#" MasterPageFile="~/Paginaprincipal.Master" AutoEventWireup="true" CodeBehind="wfError.aspx.cs" Inherits="WebPage.wfError" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:LinkButton ID="lkbError" runat="server"></asp:LinkButton>
    <cc1:modalpopupextender ID="mdlError" runat="server"
     TargetControlID = "lkbError"
     PopupControlID = "pnlError" DropShadow ="false"
     BackgroundCssClass="modalBackground">        
</cc1:modalpopupextender>
    <asp:Panel ID="pnlError" runat="server" 
    CssClass="modalPopup" Width="291px">
    <table width="100%">
        <tr>
            <td align="center">
            <h3>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </h3>
            </td>
        </tr>
        <tr>
            <td class="style5" align="center">
                <asp:Button ID="btnError" runat="server" Text="Ok" />
            </td>
        </tr>
    </table>
    </asp:Panel>
</asp:Content>

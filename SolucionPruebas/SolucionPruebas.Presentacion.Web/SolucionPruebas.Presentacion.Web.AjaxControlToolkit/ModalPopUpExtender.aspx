<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModalPopUpExtender.aspx.cs" Inherits="ModalPopUpExtender" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style>
        .modalBackground {
	        background-color:Gray;
	        filter:alpha(opacity=70);
	        opacity:0.7;
        }


        .modalPopup {
	        background-color:#FFD9D5;
	        border-width:3px;
	        border-style:solid;
	        border-color:Gray;
	        padding:3px;
	        width:250px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:LinkButton ID="LinkButton1" runat="server"><%=DateTime.Now.ToString() %></asp:LinkButton>
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Height="200px" Width="300px"
            Style="display: none">
            123123312<asp:LinkButton ID="LinkButton2" runat="server">LinkButton</asp:LinkButton>
            <asp:Button ID="Button1" runat="server" Text="Close" />
            <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" Height="100px" Width="150px"
                Style="display: none">
                333333
                <asp:Button ID="Button2" runat="server" Text="Close" />
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="LinkButton2"
                PopupControlID="Panel2" CancelControlID="Button2" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="LinkButton1"
            PopupControlID="Panel1" CancelControlID="Button1" BackgroundCssClass="modalBackground">
        </cc1:ModalPopupExtender>
    </form>
</body>
</html>

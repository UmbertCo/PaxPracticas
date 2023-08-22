<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TwoModalPopUpExtenderUpdatePanel.aspx.cs" Inherits="TwoModalPopUpExtenderUpdatePanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="ModelPopupRelation.ascx" TagName="ModelPopupRelation" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        /*css for ModalPoup Extender backgroud */
        .ModalPoupBackgroundCssClass
        {
            background-color: Black;
            filter: alpha(opacity=20);
            opacity: 0.2;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <%--First model Popup Starts from Here--%>
                <asp:Button ID="btntargetControlOfmpeFirstDialogBox" runat="Server" Text="" Style="display: none;" />
                <cc1:ModalPopupExtender ID="mpeFirstDialogBox" runat="server" TargetControlID="btntargetControlOfmpeFirstDialogBox"
                    PopupControlID="pnlFirstDialogBox" CancelControlID="btnCloseFirstDialogBox" BackgroundCssClass="ModalPoupBackgroundCssClass"
                    BehaviorID="mpeFirstDialogBox">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlFirstDialogBox" runat="server" BorderStyle="Solid" BorderWidth="1" Style="width: 700px; background-color: White; 
                    display: none; height: 400px; z-index:10000">
                    <asp:Panel ID="pnlFirstDialogBoxHeader" runat="server" Width="100%" BackColor="Brown"
                        HorizontalAlign="Right">
                        <asp:Button ID="btnCloseFirstDialogBox" runat="server" Text="Close DialogBox" />
                    </asp:Panel>
                    <asp:Panel ID="pnlFirstDialogBoxDetail" runat="server" Width="99%" HorizontalAlign="left">
                        <h1>
                            This is the first DialogBox</h1>
                        <br />
                        <%--this button opens the Second dialogbox--%>
                        <asp:Button ID="btnOpenSecondDialogBox" runat="server" Text="Open Second DialogBox"
                            OnClick="btnOpenSecondDialogBox_Click" />
                    </asp:Panel>
                </asp:Panel>
                <%--First model Popup Ends from Here--%>
                <%--Second model Popup Starts from Here--%>
                <asp:Button ID="btntargetControlOfmpeSecondDialogBox" runat="Server" Text="" Style="display: none;" />
                <cc1:ModalPopupExtender ID="mpeSecondDialogBox" runat="server" TargetControlID="btntargetControlOfmpeSecondDialogBox"
                    PopupControlID="pnlSecondDialogBox" CancelControlID="btnCloseSecondDialogBox"
                    BackgroundCssClass="ModalPoupBackgroundCssClass" BehaviorID="mpeSecondDialogBox">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlSecondDialogBox" runat="server"  BorderStyle="Solid" BorderWidth="1" Style="width: 500px; background-color: White;
                    display: none; z-index:10001">
                    <asp:Panel ID="pnlSecondDialogBoxHeader" runat="server" Width="100%" BackColor="Brown"
                        HorizontalAlign="Right">
                        <asp:Button ID="btnCloseSecondDialogBox" runat="server" Text="Close DialogBox" />
                    </asp:Panel>
                    <asp:Panel ID="pnlSecondDialogBoxDetail" runat="server" Width="99%" HorizontalAlign="left">
                        <h1>
                            This is the Second DialogBox</h1>                  
                    </asp:Panel>
                </asp:Panel>
                <%--Second model Popup Ends from Here--%>
                <%--Button opens the First Dialog box --%>
                <asp:Button ID="btnOpenFirstDialogBox" runat="server" Text="Open First DialogBox"
                    OnClick="btnOpenFirstDialogBox_Click" />
                <%-- One ModelPopupRelation Control is required to set one relation between two  model popup dialog boxes(means ModalPopupExtender). You can add more according to requirement --%>
               <uc1:ModelPopupRelation ID="ModelPopupRelation1" runat="server" ParentModelPopupID="mpeFirstDialogBox"
                ChildModelPopupID="mpeSecondDialogBox" Start="true" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TwoModalPopUpExtender.aspx.cs" Inherits="TwoModalPopUpExtender" %>
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
    <form id="form1" runat="server" style="z-index:98">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <asp:LinkButton ID="lnkDummy" runat="server">Ejemplo</asp:LinkButton>

        <cc1:modalpopupextender id="ModalPopupExtender1" runat="server" backgroundcssclass="modalBackground"
                BehaviorID="pnlPopup" cancelcontrolid="btnHide" popupcontrolid="pnlPopup" targetcontrolid="lnkDummy">
        </cc1:modalpopupextender>

        <asp:Panel ID="pnlPopup" runat="server" Style="display: none; background-color:#FFD9D5; border-style:solid; z-index:10000;" Height="200px" Width="300px">

            <div class="modal-content" style="z-index:10001;">
              <div class="modal-header">

                <h4>Pendaftaran</h4>

              </div>
              <div class="modal-body" style="">


              </div>
              <div class="modal-footer">
                <asp:Button ID="btnAceptar" runat="server" Cssclass="btn btn-danger" Text="Tutup" />
                <asp:Button ID="btnHide" runat="server" Cssclass="btn btn-danger" Text="Tutup" />
              </div>
            </div>
        </asp:Panel>

        <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnAceptar" BehaviorID="panelLogmasuk"
                PopupControlID="panelLogmasuk" CancelControlID="Button3" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>

        <asp:Panel ID="panelLogmasuk" runat="server" Style="display: none; background-color:#FFD9D5; border-style:solid; z-index:10002;" Height="100px" Width="150px">

            <div class="modal-content" style="z-index:10003;">
              <div class="modal-header">

                <h4>Log masuk</h4>

              </div>
              <div class="modal-body" style="">
              log masuk
              </div>
              <div class="modal-footer">
                <asp:Button ID="Button3" runat="server" Cssclass="btn btn-danger" Text="Tutup" />
              </div>
            </div>
        </asp:Panel>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webAddendaFIDEAPECH.aspx.cs" Inherits="Timbrado_Addendas_webAddendaFIDEAPECH" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" language="javascript">
        function calcularCantidadPago() {
            var nImporte1 = document.getElementById('<%=txtImporte1.ClientID %>').value;
            var nImporte2 = document.getElementById('<%=txtImporte2.ClientID %>').value;
            var nImporte3 = document.getElementById('<%=txtImporte3.ClientID %>').value;
            var nImporte4 = document.getElementById('<%=txtImporte4.ClientID %>').value;
            var nTotal = 0;
            var nNum = 0;

            document.getElementById('<%=txtCanPag.ClientID %>').value = 0;

            if (nImporte1.value == 0 || nImporte2.value == 0 || nImporte3.value == 0 || nImporte4.value == 0) {
                document.getElementById('<%=txtCanPag.ClientID %>').value = 0;

            }
            else {
                nTotal = parseFloat(nImporte1) + parseFloat(nImporte2) + parseFloat(nImporte3) + parseFloat(nImporte4);

                document.getElementById('<%=txtCanPag.ClientID %>').value = nTotal;

            }
        }

        function redondear(cantidad, decimales) {
            var cantidad = parseFloat(cantidad);
            var decimales = parseFloat(decimales);
            decimales = (!decimales ? 2 : decimales);
            return Math.round(cantidad * Math.pow(10, decimales)) / Math.pow(10, decimales);
        }

        function calcularCapitalCto1() {
            var dCapital = document.getElementById('<%=txtCap1Cpto1.ClientID %>').value;
            var dIntNor = document.getElementById('<%=txtNor1Cpto1.ClientID %>').value;
            var dIntMor = document.getElementById('<%=txtMor1Cpto1.ClientID %>').value;
            var dSubTot =  parseFloat(dCapital) +  parseFloat(dIntNor) +  parseFloat(dIntMor);
            dSubTot = redondear(dSubTot, 2);

            document.getElementById('<%=txtSubTot1Cpto1.ClientID %>').value = dSubTot;
        }

        function calcularCapitalCto2() {
            var dCapital = document.getElementById('<%=txtCap1Cpto2.ClientID %>').value;
            var dIntNor = document.getElementById('<%=txtNor1Cpto2.ClientID %>').value;
            var dIntMor = document.getElementById('<%=txtMor1Cpto2.ClientID %>').value;
            var dSubTot = parseFloat(dCapital) + parseFloat(dIntNor) + parseFloat(dIntMor);
            dSubTot = redondear(dSubTot, 2);

            document.getElementById('<%=txtSubTot1Cpto2.ClientID %>').value = dSubTot;
        }

        function calcularCapitalCto3() {
            var dCapital = document.getElementById('<%=txtCap1Cpto3.ClientID %>').value;
            var dIntNor = document.getElementById('<%=txtNor1Cpto3.ClientID %>').value;
            var dIntMor = document.getElementById('<%=txtMor1Cpto3.ClientID %>').value;
            var dSubTot = parseFloat(dCapital) +  parseFloat(dIntNor) +  parseFloat(dIntMor);
            dSubTot = redondear(dSubTot, 2);

            document.getElementById('<%=txtSubTot1Cpto3.ClientID %>').value = dSubTot;
        }

        function calcularCapitalCto4() {
            var dCapital = document.getElementById('<%=txtCap1Cpto4.ClientID %>').value;
            var dIntNor = document.getElementById('<%=txtNor1Cpto4.ClientID %>').value;
            var dIntMor = document.getElementById('<%=txtMor1Cpto4.ClientID %>').value;
            var dSubTot = parseFloat(dCapital) + parseFloat(dIntNor) + parseFloat(dIntMor);
            dSubTot = redondear(dSubTot, 2);

            document.getElementById('<%=txtSubTot1Cpto4.ClientID %>').value = dSubTot;
        }

        function calcularSaldoCredito1(){
            
            var dSalCre = document.getElementById('<%=txtSalCapCre1.ClientID %>').value;
            dSalCre = redondear(dSalCre, 2);
            if (document.getElementById('<%=gvConceptosCto1.ClientID %>') == null) {
                document.getElementById('<%=txtSalCap1.ClientID %>').value = dSalCre;
            }
            else {
                var nRen = document.getElementById('<%=gvConceptosCto1.ClientID %>').rows.length;
                if (nRen == 1) {
                    document.getElementById('<%=txtSalCap1.ClientID %>').value = dSalCre
                }
            }
        }

        function calcularSaldoCredito2() {

            var dSalCre = document.getElementById('<%=txtSalCapCre2.ClientID %>').value;
            dSalCre = redondear(dSalCre, 2);
            if (document.getElementById('<%=gvConceptosCto2.ClientID %>') == null) {
                document.getElementById('<%=txtSalCap2.ClientID %>').value = dSalCre;
            }
            else {
                var nRen = document.getElementById('<%=gvConceptosCto2.ClientID %>').rows.length;
                if (nRen == 1) {
                    document.getElementById('<%=txtSalCap2.ClientID %>').value = dSalCre
                }
            }
        }

        function calcularSaldoCredito3() {

            var dSalCre = document.getElementById('<%=txtSalCapCre3.ClientID %>').value;
            dSalCre = redondear(dSalCre, 2);
            if (document.getElementById('<%=gvConceptosCto3.ClientID %>') == null) {
                document.getElementById('<%=txtSalCap3.ClientID %>').value = dSalCre;
            }
            else {
                var nRen = document.getElementById('<%=gvConceptosCto3.ClientID %>').rows.length;
                if (nRen == 1) {
                    document.getElementById('<%=txtSalCap3.ClientID %>').value = dSalCre
                }
            }
        }


        function calcularSaldoCredito4() {            
            var dSalCre = document.getElementById('<%=txtSalCapCre4.ClientID %>').value;
            dSalCre = redondear(dSalCre, 2);
            if (document.getElementById('<%=gvConceptosCto4.ClientID %>') == null) {
                document.getElementById('<%=txtSalCap4.ClientID %>').value = dSalCre;
            }
            else {
                var nRen = document.getElementById('<%=gvConceptosCto4.ClientID %>').rows.length;
                if (nRen == 1) {
                    document.getElementById('<%=txtSalCap4.ClientID %>').value = dSalCre
                }
            }
        }

        function validaEnter() {
            if (event.keyCode == 13) {
                var pnlCto1 = $find("cpeContrato1");
                pnlCto1._doOpen();
            }
        }
//        function Moneda(input){
//            var num = input.value.replace(/\./g,"");
//            if(!isNaN(num)){
//            num = num.toString().split("").reverse().join("").replace(/(?=\d*\.?)(\d{3})/g,"$1.");
//            num = num.split("").reverse().join("").replace(/^[\.]/,"");
//            input.value = num;
//            }else{
//                input.value = input.value.replace(/[^\d\.]*/g,"");
//                }
//         }
        
    </script>
    </head>

<body style="background-color: #FFFFFF">
    
    <link href="../../Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/menu_style.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />

    <%------------------------JQuery----------------------------%>
    <script src='<%= ResolveUrl("~/Scripts/jquery-1.11.3.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/Scripts/jquery-1.11.3.min.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/Scripts/alerts/jquery.alerts.js") %>' type="text/javascript"></script>
    <%------------------------JQuery----------------------------%>

    <form id="form1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div>
    <table>
        <tr>
            <td>
      <fieldset class="register" style="width:950px;">
       <legend>
         <asp:Literal ID="Literal2" runat="server" 
             Text="ADENDA FIDEAPECH" />
         </legend>
         <%--Contrato 1--%>
            <asp:Panel ID="pnlContrato1" runat="server" Width="930px"  Height="23px" CssClass="collapsePanelHeader">
              <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                
                <div style="float: left; margin-left: 20px;">
                    <asp:Label ID="lblContratoUno" runat="server">Contrato 1</asp:Label>
                </div>
                <div style="float: right; vertical-align: middle;">
                    <asp:ImageButton ID="Image1" runat="server" 
                         ImageUrl="~/Imagenes/expand_blue.jpg" AlternateText="(Contrato 1...)" Visible="True"/>
                </div>
              </div>
           </asp:Panel>
                     <cc1:CollapsiblePanelExtender ID="cpeContrato1" runat="server" 
                    TargetControlID="pnlContrato1Ext"
                    ExpandControlID="pnlContrato1"
                    CollapseControlID="pnlContrato1" 
                    TextLabelID="lblContratoUno"
                    ImageControlID="Image1"    
                    ExpandedText=""
                    CollapsedText=""
                    ExpandedImage="~/Imagenes/collapse_blue.jpg"
                    CollapsedImage="~/Imagenes/expand_blue.jpg"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanelDemo" Enabled="True">
         </cc1:CollapsiblePanelExtender>
            <asp:Panel ID="pnlContrato1Ext" runat="server" 
                  GroupingText="Contrato 1" Width="930px" BackColor="#F0F0F0">
         <table>
            <tr>
                <td>                
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblContratoCpto1" runat="server" Text="No. Contrato"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                
                    <asp:Label ID="lblImporteCpto1" runat="server" Text="Importe"></asp:Label>
                
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td colspan="6">
                    <asp:Label ID="lblSalAntCpto1" runat="server" 
                        Text="Saldo de capital antes del pago crédito"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblSalCapCpto2" runat="server" Text="Saldo de capital"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCto1" runat="server" Text="1" Font-Bold="True"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtNoContrato1" runat="server" TabIndex="1" Width="90px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfContrato1" runat="server" 
                        ControlToValidate="txtNoContrato1" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtImporte1" runat="server" TabIndex="2" Width="90px">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfImporte1" runat="server" 
                        ControlToValidate="txtImporte1" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regxImporte1" runat="server" 
                        ControlToValidate="txtImporte1" CssClass="failureNotification" 
                        Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                        ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                        ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td valign="top">
                    <asp:TextBox ID="txtSalCapCre1" runat="server" TabIndex="3" Width="90px">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfSalCapCre1" runat="server" 
                        ControlToValidate="txtSalCapCre1" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regxSalCapCre1" runat="server" 
                        ControlToValidate="txtSalCapCre1" CssClass="failureNotification" 
                        Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                        ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                        ValidationGroup="validaAgrega1"> <img src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtSalCap1" runat="server" TabIndex="4" Width="90px">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfSalCap1" runat="server" 
                        ControlToValidate="txtSalCap1" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td valign="top">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td colspan="29">
                    <table>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblFechaCpto1" runat="server" Text="Fecha"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblDiasCpto1" runat="server" Text="Días"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblCapitalCpto1" runat="server" Text="Capital"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblIntNorCpto1" runat="server" Text="Int. Norm."></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblTasaCpto1" runat="server" Text="Tasa %"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblIntMorCpto1" runat="server" Text="Int. Morat"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblTasa2Cpto1" runat="server" Text="Tasa %"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblSubTotCpto1" runat="server" Text="Sub-Total"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtFec1Cpto1" runat="server" Width="90px" TabIndex="5"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfFecha1" runat="server" 
                                    ControlToValidate="txtFec1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDia1Cpto1" runat="server" Width="60px" TabIndex="6"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfDias1" runat="server" 
                                    ControlToValidate="txtDia1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCap1Cpto1" runat="server" Width="90px" TabIndex="7">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfCapital1" runat="server" 
                                    ControlToValidate="txtCap1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxCapital1" runat="server" 
                                    ControlToValidate="txtCap1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega1"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNor1Cpto1" runat="server" Width="90px" TabIndex="8">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfNorm1" runat="server" 
                                    ControlToValidate="txtNor1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxNorm1" runat="server" 
                                    ControlToValidate="txtNor1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega1"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTas1_1Cpto1" runat="server" Width="60px" TabIndex="9">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfTasa1Cpto1" runat="server" 
                                    ControlToValidate="txtTas1_1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxTasa1Cpto1" runat="server" 
                                    ControlToValidate="txtTas1_1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega1"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMor1Cpto1" runat="server" Width="80px" TabIndex="10">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfMoral1" runat="server" 
                                    ControlToValidate="txtMor1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxMor1" runat="server" 
                                    ControlToValidate="txtMor1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega1"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTas2_1Cpto1" runat="server" Width="60px" TabIndex="11">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfTasa2Cpto1" runat="server" 
                                    ControlToValidate="txtTas2_1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxTasa2Cpto1" runat="server" 
                                    ControlToValidate="txtTas2_1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega1"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubTot1Cpto1" runat="server" Width="80px" TabIndex="12" ReadOnly="false">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfSubTot1" runat="server" 
                                    ControlToValidate="txtSubTot1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega1"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxSubTot1" runat="server" 
                                    ControlToValidate="txtSubTot1Cpto1" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega1"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Button ID="btnAgrCpto1" runat="server" CssClass="botonChico" Height="20px" 
                                    onclick="btnAgrCpto1_Click" Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
                                    ValidationGroup="validaAgrega1" Width="80px" TabIndex="13" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td align="right">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td colspan="29">
                    <asp:GridView ID="gvConceptosCto1" runat="server" AutoGenerateColumns="False" Width="860px"
                                DataKeyNames="id_Concepto" BorderColor="#999999" BorderStyle="Solid" 
                                BorderWidth="1px" onrowdeleting="gvConceptosCto1_RowDeleting" 
                        TabIndex="14" onrowdatabound="gvConceptosCto1_RowDataBound" >
                        <Columns>
                            <asp:CommandField ShowDeleteButton="True" ButtonType="Image" 
                                        DeleteImageUrl="~/Imagenes/error_sign.gif" DeleteText="" HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" >
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:CommandField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                            <asp:BoundField DataField="Dias" HeaderText="Dias" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Capital" HeaderText="Capital" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IntNorm" HeaderText="Int. Norm." >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TasaUno" HeaderText="Tasa %" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IntMorat" HeaderText="Int. Moral" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TasaDos" HeaderText="Tasa %" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SubTotal" HeaderText="Sub-Total" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                       <EmptyDataTemplate>
                                    No hay Conceptos Capturados
                       </EmptyDataTemplate>
                       <HeaderStyle BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:GridView>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="32" style="background-color: #F0F0F0">
                <hr />
                </td>
            </tr>
         </table>
         </asp:Panel>
<%--Contrato 2--%>
            <asp:Panel ID="pnlContrato2" runat="server" Width="930px"  Height="23px" CssClass="collapsePanelHeader">
              <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                
                <div style="float: left; margin-left: 20px;">
                    <asp:Label ID="lblContratoDos" runat="server">Contrato 2</asp:Label>
                </div>
                <div style="float: right; vertical-align: middle;">
                    <asp:ImageButton ID="Image2" runat="server" 
                         ImageUrl="~/Imagenes/expand_blue.jpg" AlternateText="(Contrato 2...)" Visible="True"/>
                </div>
              </div>
           </asp:Panel>
                     <cc1:CollapsiblePanelExtender ID="cpeContrato2" runat="server" 
                    TargetControlID="pnlContrato2Ext"
                    ExpandControlID="pnlContrato2"
                    CollapseControlID="pnlContrato2" 
                    TextLabelID="lblContratoDos"
                    ImageControlID="Image2"    
                    ExpandedText=""
                    CollapsedText=""
                    ExpandedImage="~/Imagenes/collapse_blue.jpg"
                    CollapsedImage="~/Imagenes/expand_blue.jpg"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanelDemo" Enabled="True">
         </cc1:CollapsiblePanelExtender>
  <asp:Panel ID="pnlContrato2Ext" runat="server" GroupingText="Contrato 2" Width="930px" BackColor="#F0F0F0">
    <table>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblContrato2" runat="server" Text="No. Contrato"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblImporte2" runat="server" Text="Importe"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td valign="top" colspan="6">
                    <asp:Label ID="lblSalCapCre2" runat="server" 
                        Text="Saldo de capital antes del pago crédito"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblSalCap2" runat="server" Text="Saldo de capital"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>                
                
                    <asp:Label ID="lblCto2" runat="server" Text="2" Font-Bold="True"></asp:Label>
                
                </td>
                <td>
                    &nbsp;</td>
                <td>
                
                    <asp:TextBox ID="txtNoContrato2" runat="server" TabIndex="15" Width="90px"></asp:TextBox>
                
                </td>
                <td>
                    &nbsp;</td>
                <td>
                
                    <asp:TextBox ID="txtImporte2" runat="server" TabIndex="16" Width="90px">0</asp:TextBox>
                
                    <asp:RequiredFieldValidator ID="rfImporte2" runat="server" 
                        ControlToValidate="txtImporte2" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega2"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                
                    <asp:RegularExpressionValidator ID="regxImporte2" runat="server" 
                        ControlToValidate="txtImporte2" CssClass="failureNotification" 
                        Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                        ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                        ValidationGroup="validaAgrega2"> <img src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                
                </td>
                <td>
                    &nbsp;</td>
                <td>
                   </td>
                <td valign="top">
                    <asp:TextBox ID="txtSalCapCre2" runat="server" TabIndex="17" Width="90px">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfSalCapCre2" runat="server" 
                        ControlToValidate="txtSalCapCre2" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega2"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regxSalCapCre2" runat="server" 
                        ControlToValidate="txtSalCapCre2" CssClass="failureNotification" 
                        Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                        ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                        ValidationGroup="validaAgrega2"> <img src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                </td>
                <td>
                  </td>
                <td>
                   </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtSalCap2" runat="server" TabIndex="18" Width="90px">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfSalCap2" runat="server" 
                        ControlToValidate="txtSalCap2" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega2"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>                
                
                </td>
                <td>
                    &nbsp;</td>
                <td>
                
                </td>
                <td>
                    &nbsp;</td>
                <td>
                
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td colspan="29">
                    <table>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Fecha"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Días"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Capital"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Int. Norm."></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Tasa %"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="Int. Morat"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Tasa %"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Sub-Total"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtFec1Cpto2" runat="server" Width="90px" TabIndex="19"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfFecha2" runat="server" 
                                    ControlToValidate="txtFec1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega2"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDia1Cpto2" runat="server" Width="60px" TabIndex="20"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfDias2" runat="server" 
                                    ControlToValidate="txtDia1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega2"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCap1Cpto2" runat="server" Width="90px" TabIndex="21">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfCapital2" runat="server" 
                                    ControlToValidate="txtCap1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega2"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxCapital2" runat="server" 
                                    ControlToValidate="txtCap1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega2"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNor1Cpto2" runat="server" Width="90px" TabIndex="22">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfNorm2" runat="server" 
                                    ControlToValidate="txtNor1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega2"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxNorm2" runat="server" 
                                    ControlToValidate="txtNor1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega2"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTas1_1Cpto2" runat="server" Width="60px" TabIndex="23">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfTasa1Cpto2" runat="server" 
                                    ControlToValidate="txtTas1_1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega2"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxTasa1Cpto2" runat="server" 
                                    ControlToValidate="txtTas1_1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega2"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMor1Cpto2" runat="server" Width="80px" TabIndex="24">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfMoral2" runat="server" 
                                    ControlToValidate="txtMor1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega2"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxMor2" runat="server" 
                                    ControlToValidate="txtMor1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega2"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTas2_1Cpto2" runat="server" Width="60px" TabIndex="25">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfTasa2Cpto2" runat="server" 
                                    ControlToValidate="txtTas2_1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega2"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxTasa2Cpto2" runat="server" 
                                    ControlToValidate="txtTas2_1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega2"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubTot1Cpto2" runat="server" Width="80px" TabIndex="26">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfSubTot2" runat="server" 
                                    ControlToValidate="txtSubTot1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega2"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxSubTot2" runat="server" 
                                    ControlToValidate="txtSubTot1Cpto2" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega2"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Button ID="btnAgrCpto2" runat="server" CssClass="botonChico" Height="20px" 
                                    onclick="btnAgrCpto2_Click" Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
                                    ValidationGroup="validaAgrega2" Width="80px" TabIndex="27" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td align="right">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>

                    </table>                       
                </td>
                <td>
                    &nbsp;</td>             
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td colspan="29">
                    <asp:GridView ID="gvConceptosCto2" runat="server" AutoGenerateColumns="False" 
                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                        DataKeyNames="id_Concepto" onrowdeleting="gvConceptosCto2_RowDeleting" 
                        Width="860px" TabIndex="27" onrowdatabound="gvConceptosCto2_RowDataBound">
                        <Columns>
                            <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/error_sign.gif" 
                                DeleteText="" HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                                ShowDeleteButton="True">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:CommandField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                            <asp:BoundField DataField="Dias" HeaderText="Dias" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Capital" HeaderText="Capital">
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IntNorm" HeaderText="Int. Norm." >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TasaUno" HeaderText="Tasa %" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IntMorat" HeaderText="Int. Moral" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TasaDos" HeaderText="Tasa %" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SubTotal" HeaderText="Sub-Total">
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            No hay Conceptos Capturados
                        </EmptyDataTemplate>
                        <HeaderStyle BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:GridView>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="32" style="background-color: #F0F0F0">
                  <hr />
                  </td>
            </tr>    
    </table>
        </asp:Panel>

    <%--Contrato 3--%>
            <asp:Panel ID="pnlContrato3" runat="server" Width="930px"  Height="23px" CssClass="collapsePanelHeader">
              <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                
                <div style="float: left; margin-left: 20px;">
                    <asp:Label ID="lblContratoTres" runat="server">Contrato 3</asp:Label>
                </div>
                <div style="float: right; vertical-align: middle;">
                    <asp:ImageButton ID="ImageButton1" runat="server" 
                         ImageUrl="~/Imagenes/expand_blue.jpg" AlternateText="(Contrato 3...)" Visible="True"/>
                </div>
              </div>
           </asp:Panel>
                     <cc1:CollapsiblePanelExtender ID="cpeContrato3" runat="server" 
                    TargetControlID="pnlContrato3Ext"
                    ExpandControlID="pnlContrato3"
                    CollapseControlID="pnlContrato3" 
                    TextLabelID="lblContratoTres"
                    ImageControlID="Image3"    
                    ExpandedText=""
                    CollapsedText=""
                    ExpandedImage="~/Imagenes/collapse_blue.jpg"
                    CollapsedImage="~/Imagenes/expand_blue.jpg"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanelDemo" Enabled="True">
         </cc1:CollapsiblePanelExtender>
  <asp:Panel ID="pnlContrato3Ext" runat="server" GroupingText="Contrato 3" Width="930px" BackColor="#F0F0F0">
    <table>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblContrato3" runat="server" Text="No. Contrato"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblImporte3" runat="server" Text="Importe"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td colspan="6">
                    <asp:Label ID="lblSalCapCre3" runat="server" 
                        Text="Saldo de capital antes del pago crédito"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblSalCap3" runat="server" Text="Saldo de capital"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCto3" runat="server" Font-Bold="True" Text="3"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtNoContrato3" runat="server" TabIndex="28" Width="90px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtImporte3" runat="server" TabIndex="29" Width="90px">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfImporte3" runat="server" 
                        ControlToValidate="txtImporte3" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega3"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regxImporte3" runat="server" 
                        ControlToValidate="txtImporte3" CssClass="failureNotification" 
                        Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                        ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                        ValidationGroup="validaAgrega3"> <img src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtSalCapCre3" runat="server" TabIndex="30" Width="90px">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfSalCapCre3" runat="server" 
                        ControlToValidate="txtSalCapCre3" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega3"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regxSalCapCre3" runat="server" 
                        ControlToValidate="txtSalCapCre3" CssClass="failureNotification" 
                        Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                        ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                        ValidationGroup="validaAgrega3"> <img src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtSalCap3" runat="server" TabIndex="31" Width="90px">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfSalCap3" runat="server" 
                        ControlToValidate="txtSalCap3" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega3"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td colspan="29">
                <table>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblFecha3" runat="server" Text="Fecha"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="lblDias3" runat="server" Text="Días"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Capital"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="Int. Norm."></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Tasa %"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label19" runat="server" Text="Int. Morat"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="Tasa %"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label21" runat="server" Text="Sub-Total"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtFec1Cpto3" runat="server" Width="90px" TabIndex="32"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfFecha3" runat="server" 
                                    ControlToValidate="txtFec1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega3"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDia1Cpto3" runat="server" Width="60px" TabIndex="33"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfDias3" runat="server" 
                                    ControlToValidate="txtDia1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega3"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCap1Cpto3" runat="server" Width="90px" TabIndex="34">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfCapital3" runat="server" 
                                    ControlToValidate="txtCap1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega3"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxCapital3" runat="server" 
                                    ControlToValidate="txtCap1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega3"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNor1Cpto3" runat="server" Width="90px" TabIndex="35">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfNorm3" runat="server" 
                                    ControlToValidate="txtNor1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega3"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxNorm3" runat="server" 
                                    ControlToValidate="txtNor1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega3"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTas1_1Cpto3" runat="server" Width="60px" TabIndex="36">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfTasa1Cpto3" runat="server" 
                                    ControlToValidate="txtTas1_1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega3"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxTasa1Cpto3" runat="server" 
                                    ControlToValidate="txtTas1_1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega3"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMor1Cpto3" runat="server" Width="80px" TabIndex="37">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfMoral3" runat="server" 
                                    ControlToValidate="txtMor1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega3"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxMor3" runat="server" 
                                    ControlToValidate="txtMor1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega3"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTas2_1Cpto3" runat="server" Width="60px" TabIndex="38">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfTasa2Cpto3" runat="server" 
                                    ControlToValidate="txtTas2_1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega3"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxTasa2Cpto3" runat="server" 
                                    ControlToValidate="txtTas2_1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega3"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubTot1Cpto3" runat="server" Width="80px" TabIndex="39">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfSubTot3" runat="server" 
                                    ControlToValidate="txtSubTot1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega3"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxSubTot3" runat="server" 
                                    ControlToValidate="txtSubTot1Cpto3" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega3"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Button ID="btnAgrCpto3" runat="server" CssClass="botonChico" Height="20px" 
                                    onclick="btnAgrCpto3_Click" Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
                                    ValidationGroup="validaAgrega3" Width="80px" TabIndex="40" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td align="right">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>

                    </table>                   
                </td>
                <td>
                    &nbsp;</td>                
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td colspan="29">
                    <asp:GridView ID="gvConceptosCto3" runat="server" AutoGenerateColumns="False" 
                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                        DataKeyNames="id_Concepto" Width="860px" 
                        onrowdeleting="gvConceptosCto3_RowDeleting" TabIndex="40" 
                        onrowdatabound="gvConceptosCto3_RowDataBound">
                        <Columns>
                            <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/error_sign.gif" 
                                DeleteText="" HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                                ShowDeleteButton="True">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:CommandField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                            <asp:BoundField DataField="Dias" HeaderText="Dias">
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Capital" HeaderText="Capital">
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IntNorm" HeaderText="Int. Norm.">
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TasaUno" HeaderText="Tasa %">
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IntMorat" HeaderText="Int. Moral">
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TasaDos" HeaderText="Tasa %">
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SubTotal" HeaderText="Sub-Total">
                           <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            No hay Conceptos Capturados
                        </EmptyDataTemplate>
                        <HeaderStyle BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:GridView>
                    </td>
                <td>
                    &nbsp;</td>                     
            </tr>
            <tr>
                <td colspan="32" style="background-color: #F0F0F0">
                    <hr />
                </td>
            </tr>    
    </table>
  </asp:Panel>  

    <%--Contrato 4--%>
                <asp:Panel ID="pnlContrato4" runat="server" Width="930px"  Height="23px" CssClass="collapsePanelHeader">
              <div style="padding:5px; cursor: pointer; vertical-align: middle;">
                
                <div style="float: left; margin-left: 20px;">
                    <asp:Label ID="lblContratoCuatro" runat="server">Contrato 4</asp:Label>
                </div>
                <div style="float: right; vertical-align: middle;">
                    <asp:ImageButton ID="Image4" runat="server" 
                         ImageUrl="~/Imagenes/expand_blue.jpg" AlternateText="(Contrato 4...)" Visible="True"/>
                </div>
              </div>
           </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="cpeContrato4" runat="server" 
                    TargetControlID="pnlContrato4Ext"
                    ExpandControlID="pnlContrato4"
                    CollapseControlID="pnlContrato4" 
                    TextLabelID="lblContratoCuatro"
                    ImageControlID="Image4"    
                    ExpandedText=""
                    CollapsedText=""
                    ExpandedImage="~/Imagenes/collapse_blue.jpg"
                    CollapsedImage="~/Imagenes/expand_blue.jpg"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanelDemo" Enabled="True">
         </cc1:CollapsiblePanelExtender>
  <asp:Panel ID="pnlContrato4Ext" runat="server" GroupingText="Contrato 4" Width="930px" BackColor="#F0F0F0">
    <table>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblContrato4" runat="server" Text="No. Contrato"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblImporte4" runat="server" Text="Importe"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td colspan="6">
                    <asp:Label ID="lblSalCapCre4" runat="server" 
                        Text="Saldo de capital antes del pago crédito"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblSalCap4" runat="server" Text="Saldo de capital"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCto4" runat="server" Font-Bold="True" Text="4"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtNoContrato4" runat="server" TabIndex="41" Width="90px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtImporte4" runat="server" TabIndex="42" Width="90px">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfImporte4" runat="server" 
                        ControlToValidate="txtImporte4" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega4"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regxImporte4" runat="server" 
                        ControlToValidate="txtImporte4" CssClass="failureNotification" 
                        Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                        ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                        ValidationGroup="validaAgrega4"> <img src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtSalCapCre4" runat="server" TabIndex="43" Width="90px">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfSalCapCre4" runat="server" 
                        ControlToValidate="txtSalCapCre4" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega4"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regxSalCapCre4" runat="server" 
                        ControlToValidate="txtSalCapCre4" CssClass="failureNotification" 
                        Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                        ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                        ValidationGroup="validaAgrega4"> <img src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="txtSalCap4" runat="server" TabIndex="44" Width="90px">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfSalCap4" runat="server" 
                        ControlToValidate="txtSalCap4" CssClass="failureNotification" 
                        Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                        ValidationGroup="validaAgrega4"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td colspan="29">
                    <table>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Fecha"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Días"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label22" runat="server" Text="Capital"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label23" runat="server" Text="Int. Norm."></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label24" runat="server" Text="Tasa %"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label25" runat="server" Text="Int. Morat"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label26" runat="server" Text="Tasa %"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label27" runat="server" Text="Sub-Total"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtFec1Cpto4" runat="server" Width="90px" TabIndex="45"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfFecha4" runat="server" 
                                    ControlToValidate="txtFec1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega4"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDia1Cpto4" runat="server" Width="60px" TabIndex="46"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfDias4" runat="server" 
                                    ControlToValidate="txtDia1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega4"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCap1Cpto4" runat="server" Width="90px" TabIndex="47">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfCapital4" runat="server" 
                                    ControlToValidate="txtCap1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega4"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxCapital4" runat="server" 
                                    ControlToValidate="txtCap1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega4"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNor1Cpto4" runat="server" Width="90px" TabIndex="48">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfNorm4" runat="server" 
                                    ControlToValidate="txtNor1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega4"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxNorm4" runat="server" 
                                    ControlToValidate="txtNor1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega4"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTas1_1Cpto4" runat="server" Width="60px" TabIndex="49">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfTasa1Cpto4" runat="server" 
                                    ControlToValidate="txtTas1_1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAdenda"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxTasa1Cpto4" runat="server" 
                                    ControlToValidate="txtTas1_1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega4"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMor1Cpto4" runat="server" Width="80px" TabIndex="50">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfMoral4" runat="server" 
                                    ControlToValidate="txtMor1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega4"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxMor4" runat="server" 
                                    ControlToValidate="txtMor1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega4"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTas2_1Cpto4" runat="server" Width="60px" TabIndex="51">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfTasa2Cpto4" runat="server" 
                                    ControlToValidate="txtTas2_1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega4"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxTasa2Cpto4" runat="server" 
                                    ControlToValidate="txtTas2_1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega4"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubTot1Cpto4" runat="server" Width="80px" TabIndex="52">0</asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfSubTot4" runat="server" 
                                    ControlToValidate="txtSubTot1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ToolTip="<%$ Resources:resCorpusCFDIEs, regxCantidad %>" 
                                    ValidationGroup="validaAgrega4"> <img 
                        src="../../Imagenes/error_sign.gif" /></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regxSubTot4" runat="server" 
                                    ControlToValidate="txtSubTot1Cpto4" CssClass="failureNotification" 
                                    Display="Dynamic" ToolTip="<%$ Resources:resCorpusCFDIEs, regxPrecio %>" 
                                    ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,6})?$" 
                                    ValidationGroup="validaAgrega4"> <img 
                                    src="../../Imagenes/error_sign.gif" /></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Button ID="btnAgrCpto4" runat="server" CssClass="botonChico" Height="20px" 
                                    onclick="btnAgrCpto4_Click" Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
                                    ValidationGroup="validaAgrega4" Width="80px" TabIndex="53" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td align="right">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>

                    </table>    
                  </td>
                <td>
                    &nbsp;</td>                
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td colspan="29">
                    <asp:GridView ID="gvConceptosCto4" runat="server" AutoGenerateColumns="False" 
                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                        DataKeyNames="id_Concepto" Width="860px" TabIndex="54" 
                        onrowdeleting="gvConceptosCto4_RowDeleting" 
                        onrowdatabound="gvConceptosCto4_RowDataBound">
                        <Columns>
                            <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/error_sign.gif" 
                                DeleteText="" HeaderText="<%$ Resources:resCorpusCFDIEs, lblEliminar %>" 
                                ShowDeleteButton="True">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:CommandField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                            <asp:BoundField DataField="Dias" HeaderText="Dias">
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Capital" HeaderText="Capital" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IntNorm" HeaderText="Int. Norm.">
                             <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TasaUno" HeaderText="Tasa %">
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IntMorat" HeaderText="Int. Moral" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TasaDos" HeaderText="Tasa %" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SubTotal" HeaderText="Sub-Total" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            No hay Conceptos Capturados
                        </EmptyDataTemplate>
                        <HeaderStyle BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
                    </asp:GridView>
                   </td>
                <td>
                    &nbsp;</td>               
            </tr>
            <tr>
                <td colspan="32" style="background-color: #F0F0F0">
                    <hr />
                </td>
            </tr>    
    </table>
 </asp:Panel>
    <%--Pie de pagina--%>
    <asp:Panel ID="pnlPie" runat="server"  Width="930px">
        <table> 
            <tr>
                <td colspan="39" style="background-color: #F0F0F0">
                    <hr />
                </td>
            </tr>                                  
            <tr>
                <td colspan="39" style="background-color: #F0F0F0">
                    <table>
                        <tr>
                            <td>
                                &nbsp;&nbsp;</td>
                            <td align="right">
                                <asp:TextBox ID="txtTemp0" runat="server" BorderStyle="None" Width="90px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTemp1" runat="server" BorderStyle="None" Width="40px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td align="left">
                                <asp:Label ID="Label15" runat="server" Text="Capital"></asp:Label>
                            </td>
                            <td>
                                &nbsp;<asp:TextBox ID="txtTemp4" runat="server" BorderStyle="None" Visible="False" 
                                    Width="60px"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;</td>
                            <td align="left">
                                <asp:Label ID="Label16" runat="server" Text="Int. Norm."></asp:Label>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtTemp2" runat="server" BorderStyle="None" Width="60px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label17" runat="server" Text="Int. Morat"></asp:Label>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtTemp3" runat="server" BorderStyle="None" Width="60px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label18" runat="server" Text="Sub-Total"></asp:Label>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label14" runat="server" Text="Totales:"></asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotCap" runat="server" BorderStyle="None" 
                                    Font-Overline="False" ReadOnly="True" Width="90px">0</asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotNor" runat="server" BorderStyle="None" 
                                    Font-Overline="False" ReadOnly="True" Width="90px">0</asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotMor" runat="server" BorderStyle="None" 
                                    Font-Overline="False" ReadOnly="True" Width="90px">0</asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotSubTot" runat="server" BorderStyle="None" 
                                    Font-Overline="False" Width="90px">0</asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;</td>
                        </tr>
                    </table>
                </td>
 
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="39" style="background-color: #F0F0F0">
                <table>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCanPag" runat="server" Text="Cantidad de Pago"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblS" runat="server" Text="$"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCanPag" runat="server" BorderStyle="None" 
                                Font-Overline="False" ReadOnly="True">0</asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="39" style="background-color: #F0F0F0">
                    <asp:Label ID="lblPrograma" runat="server" Text="Programa"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td colspan="29">
                    <asp:TextBox ID="txtPrograma" runat="server" TabIndex="55" Width="840px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        <tr>
            <td colspan="39" style="background-color: #F0F0F0">
                <asp:Label ID="lblObservaciones" runat="server" Text="Observaciones"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td colspan="29">
               
                <asp:TextBox ID="txtObservaciones" runat="server" Width="840px" TabIndex="56"></asp:TextBox>
               
             </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        </table>
        </asp:Panel>
        </fieldset>            
            </td>
        </tr>
        <tr>
            <td align="right">
                <div align="right" style="height:40px;">
                    <table align="right">
                        <tr>
                            <td>
                                <asp:Button ID="btnGuardar" runat="server" CssClass="botonGrande" Height="30px" 
                                   TabIndex="57" Text="Guardar Addenda" 
                                    Width="150px" ValidationGroup="validaAdenda" onclick="btnGuardar_Click" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnCerrar" runat="server" CssClass="botonGrande" Height="30px" 
                                    TabIndex="58" Text="Cerrar Página" Width="150px" />
                            </td>
                            <td>
                            </td>
                      </tr>
                   </table>                
                </div>            
            </td>
        </tr>
    </table>


    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>

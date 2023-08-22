<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="webDescargarComprobantesCliente.aspx.cs" Inherits="Operacion_Comprobantes_webDescargarComprobantesCliente" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <script src="../../Scripts/progressbar.js" type="text/javascript"></script>
    <link href="../../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/menu_style.css" rel="stylesheet" type="text/css" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 195px;
        }
    </style>
    </head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table style="width: 340px">
    <tr>
        <td>
        <%--<asp:Label ID="lblConsulta2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblConsultaExitosa %>"/>--%>
        <asp:Label ID="lblConsulta" runat="server" Text="Los comprobantes están listos para la descarga, desea descargarlos?"/>
        </td>
    </tr>
    <tr>
        <td>
        </td>
    </tr>
    <tr>
    <td>
        &nbsp;</td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="updProgresoDescarga" runat="server" ><%--UpdateMode="Conditional" >--%>
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" class="style1">
                        <tr>
                            <td class="style2">
                                <asp:Button ID="btnDescarga" runat="server" CssClass="botonEstilo" 
                                    Height="30px" onclick="btnDescarga_Click" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" Width="189px"/>
                            </td>
                            <td>
                                <asp:Button ID="btnCerrar" runat="server" CssClass="botonEstilo" 
                                    onclick="btnCerrar_Click" TabIndex="20" 
                                    Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Height="30px" />
                            </td>
                        </tr>
                    </table>
                    <%-- OnClientClick="startProgressBar()"/>--%>
                        &nbsp;
                    <asp:Timer ID="timerDescarga" runat="server" Interval="500" Enabled="false" 
			            ontick="timerDescarga_Tick" />
                    
                    <br />
                    <br />
                    <asp:Label ID="lblProgresoDescarga" runat="server" Text="Procesando: 0%" Visible="false" />

                   <%-- <asp:Label ID="LabelProgress" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblProgressBar %>" 
                                    style="visibility:hidden"></asp:Label>--%>                    
                    <br />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnDescarga" />
                </Triggers>
	        </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
        </td>
    </tr>
    </table>
    </form>
</body>
</html>
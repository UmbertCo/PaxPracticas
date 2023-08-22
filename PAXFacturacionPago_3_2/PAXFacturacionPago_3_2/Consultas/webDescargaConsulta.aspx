<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webDescargaConsulta.aspx.cs" Inherits="Consultas_webDescargaConsulta"  Async="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<link href="../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
<link href="../Styles/menu_style.css" rel="stylesheet" type="text/css" />

 <script type="text/javascript" language="javascript">

  function startProgressBar() {
                    divProgressBar.style.visibility = "visible";
                    pMessage.style.visibility = "visible";
                    progress_update();
                }
 </script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
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
    <asp:Label ID="lblConsulta" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblConsultaExitosa %>"/>
    </td>
    </tr>
    <tr>
    <td>
<%--        <table align="left">
            <tr valign="top">
                <td align="left">
                    <p id="pMessage" style="VISIBILITY:hidden;POSITION:relative">
                        <b>
                        <asp:Label ID="LabelProgress" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblProgressBar %>">
                </asp:Label>
                        </b>
                    </p>
                </td>
                <td colspan="2">
                    <div id="divProgressBar" runat="server"
                        style="BORDER-RIGHT:black 1px solid; PADDING-RIGHT:2px; BORDER-TOP:black 1px solid; PADDING-LEFT:2px; FONT-SIZE:12pt; VISIBILITY:hidden; PADDING-BOTTOM:2px; BORDER-LEFT:black 1px solid; WIDTH:112px; PADDING-TOP:2px; BORDER-BOTTOM:black 1px solid; POSITION:relative">
                        <span id="progress1">&nbsp;&nbsp;</span> <span id="progress2">&nbsp;&nbsp;</span>
                        <span id="progress3">&nbsp;&nbsp;</span> <span id="progress4">&nbsp;&nbsp;</span>
                        <span id="progress5">&nbsp;&nbsp;</span> <span id="progress6">&nbsp;&nbsp;</span>
                        <span id="progress7">&nbsp;&nbsp;</span> <span id="progress8">&nbsp;&nbsp;</span>
                        <span id="progress9">&nbsp;&nbsp;</span>
                    </div>
                </td>
            </tr>
        </table>--%>
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
                    <table cellpadding="0" cellspacing="0" class="style1" width="400px">
                        <tr>
                            <td width="200px">
                                <asp:Button ID="btnDescarga" runat="server" CssClass="botonGrande" 
                                    Height="30px" onclick="btnDescarga_Click" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" Width="200px" />
                            </td>
                            <td>
                                <asp:Button ID="btnCerrar" runat="server" CssClass="botonEstilo" Height="30px" 
                                    onclick="btnCerrar_Click" TabIndex="20" 
                                    Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="87px" />
                            </td>
                        </tr>
                    </table>
                    &nbsp;
                    <asp:Timer ID="timerDescarga" runat="server" Interval="500" Enabled="false" 
			                ontick="timerDescarga_Tick" />    
                    <br />
                    <br />
                    <asp:Label ID="lblProgresoDescarga" runat="server" Text="Procesando: 0%" 
                            Visible="false"></asp:Label>

                    <%-- <asp:Label ID="LabelProgress" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblProgressBar %>" 
                            style="visibility:hidden"></asp:Label>--%>    
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

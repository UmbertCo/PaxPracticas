<%@ Page Language="C#" Theme="Alitas" AutoEventWireup="true" CodeFile="webDescargaConsulta.aspx.cs" Inherits="Consultas_webDescargaConsulta"  Async="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

    <%------------------------JQuery----------------------------%>
<script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script src="../Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery.ui.core.js" type="text/javascript"></script>
<script src="../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
<script src="../Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
<script src="../Scripts/progressbar.js" type="text/javascript"></script>
<link href="../Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />
<link href="../Styles/menu_style.css" rel="stylesheet" type="text/css" />
    
<%------------------------JQuery----------------------------%>
 <script type="text/javascript" language="javascript">

     function startProgressBar() {
         divProgressBar.style.visibility = "visible";
         pMessage.style.visibility = "visible";
         progress_update();
     }
 </script>

     <script runat="server">   
        public void Page_PreInit()
        {
            if (Session["theme"] == null)
            {
                this.Theme = "Alitas";
            }
            else
            {
                this.Theme = Convert.ToString(Session["theme"]);
            }
        }
    </script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    </head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <table>
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
    <%--<asp:Button runat="server" ID="btnDescarga" 
            Text ="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" 
            onclick="btnDescarga_Click" CssClass="botonGrande" Height="30px" 
            Width="200px"/>
    &nbsp;<asp:Button ID="btnCerrar" runat="server" 
            CssClass="botonEstilo" Height="30px" 
                            TabIndex="20" 
            Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>" Width="87px" />--%>
        </td>
    </tr>
    <tr>
    <td>
    <%-- <asp:Label ID="LabelProgress" runat="server" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblProgressBar %>" 
            Visible="False"></asp:Label>--%>
        </td>
    </tr>
    </table>
    </form>
</body>
</html>

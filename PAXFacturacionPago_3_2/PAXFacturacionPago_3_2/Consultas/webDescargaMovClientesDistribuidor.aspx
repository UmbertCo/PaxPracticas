﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webDescargaMovClientesDistribuidor.aspx.cs" Inherits="Consultas_webDescargaMovClientesDistribuidor"  Async="true"%>

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
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
    </head>
<body>
    <form id="form2" runat="server">
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
    </td>
    </tr>
    <tr>
    <td>
        &nbsp;</td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="updProgresoDescarga" runat="server" >
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

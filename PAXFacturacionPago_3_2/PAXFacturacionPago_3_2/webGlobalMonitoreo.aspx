<%@ Page Title="Monitoreo" Language="C#" MasterPageFile="~/webGlobalMaster.master" AutoEventWireup="true" CodeFile="webGlobalMonitoreo.aspx.cs" Inherits="webGlobalMonitoreo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<h2>
    <asp:Label ID="lblTitulo" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varMonitoreo %>"></asp:Label>
</h2>

<br />

    <table cellpadding="0" cellspacing="0">
        <tr>
            <td width="900px">
                &nbsp;</td>
            <td>
                <asp:Button ID="btnMonitoreo" runat="server" CssClass="botonEstilo" 
                    Text="<%$ Resources:resCorpusCFDIEs, varMonitoreo %>" 
                    onclick="btnMonitoreo_Click"  />
            </td>
        </tr>
    </table>

  

    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.ui.core.js" type="text/javascript"></script>
    <script src="Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="Scripts/alerts/jquery.alerts.js" type="text/javascript"></script>
    <link href="Scripts/alerts/jquery.alerts.css" rel="stylesheet" type="text/css" />

    <link href="Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />
    <style type="text/css" >

        .modal
        {
            padding: 10px 10px 10px 10px;
            border:1px solid #333333;
            background-color:White;
        }
        .modal p
        {
            width:600px;
            text-align:right;
        }
        .modal div
        {
            width:600px;
            vertical-align:top;
        }
        .modal div p
        {
            text-align:left;
        }
        .imagenModal
        {
            height:15px;
            cursor:pointer;
        }

    </style>



<cc1:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="4" 
    Height="760px" Width="940px" AutoPostBack="True" 
        onactivetabchanged="TabContainer_ActiveTabChanged">

    <cc1:TabPanel runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, varCPU %>" ID="TabPanel1">

        <ContentTemplate>
<div class="accountInfo textos" style="width:900px;">
    <fieldset class="register" style="width:890px;">
        <legend>
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varCaptura %>" />


        </legend>
        <table class="style1">
            <tr>
                <td>
                    <asp:Label ID="lblReceptor" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, varUbicacion %>" 
                                AssociatedControlID="ddlUbicacion"></asp:Label>


                    <asp:DropDownList ID="ddlUbicacion" runat="server" AutoPostBack="True" onselectedindexchanged="ddlUbicacion_SelectedIndexChanged" 
                                >
                        <asp:ListItem>Apolo</asp:ListItem>
<asp:ListItem Value="Discovery">Discovery</asp:ListItem>
<asp:ListItem>Chihuahua</asp:ListItem>
</asp:DropDownList>


                </td>
                <td>
                    <asp:Label ID="lblDispositivo" runat="server" AssociatedControlID="ddlDispositivo" 
                        Text="<%$ Resources:resCorpusCFDIEs, varDispositivo %>"></asp:Label>


                    <asp:DropDownList ID="ddlDispositivo" runat="server">
                        <asp:ListItem>CPU</asp:ListItem>
</asp:DropDownList>


                </td>
                <td rowspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblOrigen" runat="server" AssociatedControlID="ddlOrigen" 
                        Text="<%$ Resources:resCorpusCFDIEs, varOrigen %>"></asp:Label>


                    <asp:DropDownList ID="ddlOrigen" runat="server">
                        <asp:ListItem>Sistema gráfico de uso del CPU</asp:ListItem>
</asp:DropDownList>


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
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPromedio" runat="server" AssociatedControlID="txtPromedio" 
                        Text="<%$ Resources:resCorpusCFDIEs, varPromedio %>"></asp:Label>


                    <asp:TextBox ID="txtPromedio" runat="server" BackColor="White" Width="100px" 
                        MaxLength="10"></asp:TextBox>





                    <cc1:MaskedEditExtender ID="txtPromedio_MaskedEditExtender" runat="server" 
                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                        Mask="99.99" MaskType="Number" TargetControlID="txtPromedio">
                    </cc1:MaskedEditExtender>
                    %<asp:RequiredFieldValidator ID="rfvFechaIni0" runat="server" 
                        ControlToValidate="txtPromedio" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                        ValidationGroup="grupoCPU"></asp:RequiredFieldValidator>


                </td>
                <td>
                    <asp:Label ID="lblMaximo" runat="server" AssociatedControlID="txtMaximo" 
                        Text="<%$ Resources:resCorpusCFDIEs, varMaximo %>"></asp:Label>


                    <asp:TextBox ID="txtMaximo" runat="server" BackColor="White" Width="100px" 
                        MaxLength="10"></asp:TextBox>


                    <cc1:MaskedEditExtender ID="txtMaximo_MaskedEditExtender" runat="server" 
                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                        Mask="99.99" MaskType="Number" TargetControlID="txtMaximo">
                    </cc1:MaskedEditExtender>
                    %<asp:RequiredFieldValidator ID="rfvFechaIni1" runat="server" 
                        ControlToValidate="txtMaximo" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                        ValidationGroup="grupoCPU"></asp:RequiredFieldValidator>


                </td>
                <td>
                    <asp:Label ID="lblCapacidad" runat="server" 
                        AssociatedControlID="txtCapCpu" Font-Bold="True" 
                        Text="<%$ Resources:resCorpusCFDIEs, varCapacidad %>"></asp:Label>


                    <asp:TextBox ID="txtCapCpu" runat="server" BackColor="White" MaxLength="10" 
                        Width="100px"></asp:TextBox>


                    <cc1:MaskedEditExtender ID="txtCapCpu_MaskedEditExtender" runat="server" 
                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                        Mask="999.99" MaskType="Number" TargetControlID="txtCapCpu">
                    </cc1:MaskedEditExtender>


                    <asp:RequiredFieldValidator ID="rfvFechaIni2" runat="server" 
                        ControlToValidate="txtCapCpu" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                        ValidationGroup="grupoCPU"></asp:RequiredFieldValidator>


                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAdjuntar" runat="server" AssociatedControlID="fuCpu" 
                        Text="<%$ Resources:resCorpusCFDIEs, lbldatosprueba %>"></asp:Label>


                    <asp:FileUpload ID="fuCpu" runat="server" Height="23px" Width="219px" />


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
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFechaIni" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                AssociatedControlID="txtFechaIni"></asp:Label>


                    <asp:TextBox ID="txtFechaIni" runat="server" BackColor="White" Width="100px" ></asp:TextBox>


                    <asp:Image ID="imgIni" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" ></asp:Image>


                    <asp:RegularExpressionValidator ID="revFechaIni" runat="server" Display="Dynamic"
                                ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                <img src="Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>


                    <cc1:calendarextender ID="txtFechaIni_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txtFechaIni" Format="dd/MM/yyyy" 
                                PopupButtonID="imgIni"></cc1:calendarextender>


                    <asp:RequiredFieldValidator ID="rfvFechaIni" runat="server" 
                                ControlToValidate="txtFechaIni" CssClass="failureNotification" 
                                ValidationGroup="grupoCPU" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>


                </td>
                <td>
                    <asp:Label ID="lblFechaFin" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                AssociatedControlID="txtFechaFin"></asp:Label>


                    <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White" Width="100px"></asp:TextBox>


                    <asp:Image ID="imgFin" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" ></asp:Image>


                    <asp:RegularExpressionValidator ID="revFechaFin" runat="server" Display="Dynamic"
                                ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                <img src="Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>


                    <cc1:calendarextender ID="txtFechaFin_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" 
                                PopupButtonID="imgFin"></cc1:calendarextender>


                    <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" 
                                ControlToValidate="txtFechaFin" CssClass="failureNotification" 
                                ValidationGroup="grupoCPU" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>


                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;
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
            </tr>
        </table>
    </fieldset>
</div>
<div style="text-align:center;" >
    <asp:UpdateProgress ID="uppConsultas" runat="server" AssociatedUpdatePanelID="udpConsultas" >
        <progresstemplate>
            <img alt="" 
                src="Imagenes/imgAjaxLoader.gif" />
        
</progresstemplate>
</asp:UpdateProgress>


</div>
<br />
<asp:UpdatePanel ID="udpConsultas" runat="server">
    <ContentTemplate>
        <p style="text-align:right;" >
            <asp:Label ID="lblMsgCPU" runat="server" Visible="False"></asp:Label>
            <asp:Button ID="btnGuardar" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                     ValidationGroup="grupoCPU" 
                    CssClass="botonEstilo" onclick="btnGuardar_Click" />
        </p>
    
</ContentTemplate>
</asp:UpdatePanel>


</ContentTemplate>

</cc1:TabPanel>
    <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, varMemoria %>">

        <ContentTemplate>
<div class="accountInfo textos" style="width:900px;">
    <fieldset class="register" style="width:890px;">
        <legend>
            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varCaptura %>" />
        </legend>
        <table class="style1">
            <tr>
                <td>
                    <asp:Label ID="lblUbMemoria" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, varUbicacion %>" 
                                AssociatedControlID="ddlUbMemoria"></asp:Label>
                    <asp:DropDownList ID="ddlUbMemoria" runat="server" AutoPostBack="True" onselectedindexchanged="ddlUbMemoria_SelectedIndexChanged" 
                                >
                        <asp:ListItem>Apolo</asp:ListItem>
                        <asp:ListItem Value="Discovery">Discovery</asp:ListItem>
                        <asp:ListItem>Chihuahua</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDisMemoria" runat="server" AssociatedControlID="ddlDisMemoria" 
                        Text="<%$ Resources:resCorpusCFDIEs, varDispositivo %>"></asp:Label>
                    <asp:DropDownList ID="ddlDisMemoria" runat="server">
                        <asp:ListItem>MEMORIA</asp:ListItem>
                    </asp:DropDownList>
                </td>
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
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblOrMemoria" runat="server" AssociatedControlID="ddlOrMemoria" 
                        Text="<%$ Resources:resCorpusCFDIEs, varOrigen %>"></asp:Label>
                    <asp:DropDownList ID="ddlOrMemoria" runat="server">
                        <asp:ListItem>Memoria gráfica</asp:ListItem>
                    </asp:DropDownList>
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
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblProMemoria" runat="server" AssociatedControlID="txtPromemoria" 
                        Text="<%$ Resources:resCorpusCFDIEs, varPromedio %>"></asp:Label>
                    <asp:TextBox ID="txtPromemoria" runat="server" BackColor="White" Width="100px" MaxLength="10"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="txtPromemoria_MaskedEditExtender" runat="server" 
                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                        Mask="99.99" MaskType="Number" TargetControlID="txtPromemoria">
                    </cc1:MaskedEditExtender>
                    GB<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                        ControlToValidate="txtPromemoria" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                        ValidationGroup="grupoMem"></asp:RequiredFieldValidator>                
                </td>
                <td>
                    <asp:Label ID="lblMaxMem" runat="server" AssociatedControlID="txtMaxMem" 
                        Text="<%$ Resources:resCorpusCFDIEs, varMaximo %>"></asp:Label>
                    <asp:TextBox ID="txtMaxMem" runat="server" BackColor="White" Width="100px" MaxLength="10"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="txtMaxMem_MaskedEditExtender" runat="server" 
                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                        Mask="99.99" MaskType="Number" TargetControlID="txtMaxMem">
                    </cc1:MaskedEditExtender>
                    GB<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" 
                        ControlToValidate="txtMaxMem" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                        ValidationGroup="grupoMem"></asp:RequiredFieldValidator>  
                </td>
                <td>
                    <asp:Label ID="lblMemCap" runat="server" 
                        AssociatedControlID="txtCapMem" Font-Bold="True" 
                        Text="<%$ Resources:resCorpusCFDIEs, varCapacidad %>"></asp:Label>
                    <asp:TextBox ID="txtCapMem" runat="server" BackColor="White" MaxLength="10" 
                        Width="100px"></asp:TextBox>

                    <cc1:MaskedEditExtender ID="txtCapMem_MaskedEditExtender" runat="server" 
                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                        Mask="99.99" MaskType="Number" TargetControlID="txtCapMem">
                    </cc1:MaskedEditExtender>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" 
                        ControlToValidate="txtCapMem" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                        ValidationGroup="grupoMem"></asp:RequiredFieldValidator>  
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAdjuntar0" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lbldatosprueba %>" 
                                AssociatedControlID="fuCpuMem"></asp:Label>
                    <asp:FileUpload ID="fuCpuMem" runat="server" Height="23px" Width="219px" />
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
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblfechaIniMem" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                AssociatedControlID="txtFechaIniMem"></asp:Label>
                    <asp:TextBox ID="txtFechaIniMem" runat="server" BackColor="White" Width="100px" ></asp:TextBox>
                    <asp:Image ID="imgIniMem" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" >
                    </asp:Image>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
                                ControlToValidate="txtFechaIniMem" CssClass="failureNotification" 
                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                <img src="Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>
                    <cc1:calendarextender ID="txtFechaIniMem_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txtFechaIniMem" Format="dd/MM/yyyy" 
                                PopupButtonID="imgIniMem">
                    </cc1:calendarextender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="txtFechaIniMem" CssClass="failureNotification" 
                                ValidationGroup="grupoMem" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Label ID="Label12" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                AssociatedControlID="txtFechaFinMem"></asp:Label>
                    <asp:TextBox ID="txtFechaFinMem" runat="server" BackColor="White" Width="100px"></asp:TextBox>
                    <asp:Image ID="imgFinMem" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" >
                    </asp:Image>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic"
                                ControlToValidate="txtFechaFinMem" CssClass="failureNotification" 
                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                <img src="Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>
                    <cc1:calendarextender ID="txtFechaFinMem_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txtFechaFinMem" Format="dd/MM/yyyy" 
                                PopupButtonID="imgFinMem">
                    </cc1:calendarextender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ControlToValidate="txtFechaFinMem" CssClass="failureNotification" 
                                ValidationGroup="grupoMem" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;
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
            </tr>
        </table>
    </fieldset>
</div>
<div style="text-align:center;" >
    <asp:UpdateProgress ID="UpdateProgressMem" runat="server" AssociatedUpdatePanelID="udpConsultasMem" >
        <progresstemplate>
            <img alt="" 
                src="Imagenes/imgAjaxLoader.gif" />
        </progresstemplate>
    </asp:UpdateProgress>
</div>
<br />
<asp:UpdatePanel ID="udpConsultasMem" runat="server">
    <ContentTemplate>
        <p style="text-align:right;" >
            <asp:Label ID="lblMsgMemoria" runat="server" Visible="False"></asp:Label>
            <asp:Button ID="btnGuardarMem" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                     ValidationGroup="grupoMem" 
                    CssClass="botonEstilo" onclick="btnGuardarMem_Click" />
        </p>
    </ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>

    

</cc1:TabPanel>
    <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, lblAlmacenamiento %>">

        <ContentTemplate>
<div class="accountInfo textos" style="width:900px;">
    <fieldset class="register" style="width:890px;">
        <legend>
            <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varCaptura %>" />
        </legend>
        <table class="style1">
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, varUbicacion %>" 
                                AssociatedControlID="ddlUbicacionAlmacen"></asp:Label>
                    <asp:DropDownList ID="ddlUbicacionAlmacen" runat="server" AutoPostBack="True" onselectedindexchanged="ddlUbicacionAlmacen_SelectedIndexChanged" 
                                >
                        <asp:ListItem>Apolo</asp:ListItem>
                        <asp:ListItem Value="Discovery">Discovery</asp:ListItem>
                        <asp:ListItem>Chihuahua</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label9" runat="server" AssociatedControlID="ddlDispositivoAlm" 
                        Text="<%$ Resources:resCorpusCFDIEs, varDispositivo %>"></asp:Label>
                    <asp:DropDownList ID="ddlDispositivoAlm" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlDispositivoAlm_SelectedIndexChanged">
                        <asp:ListItem>C</asp:ListItem>
                        <asp:ListItem>D</asp:ListItem>
                        <asp:ListItem>SAN</asp:ListItem>
                        <asp:ListItem>NAS</asp:ListItem>
                    </asp:DropDownList>
                </td>
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
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label10" runat="server" AssociatedControlID="ddlOrigenAlm" 
                        Text="<%$ Resources:resCorpusCFDIEs, varOrigen %>"></asp:Label>
                    <asp:DropDownList ID="ddlOrigenAlm" runat="server">
                        <asp:ListItem>Gráfico de uso de disco</asp:ListItem>
                    </asp:DropDownList>
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
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label13" runat="server" AssociatedControlID="txtPromedioAlm" 
                        Text="<%$ Resources:resCorpusCFDIEs, varPromedio %>"></asp:Label>
                    <asp:TextBox ID="txtPromedioAlm" runat="server" BackColor="White" Width="100px" MaxLength="10"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="txtPromedioAlm_MaskedEditExtender" runat="server" 
                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                        Mask="99.99" MaskType="Number" TargetControlID="txtPromedioAlm">
                    </cc1:MaskedEditExtender>
                    GB<asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" 
                        ControlToValidate="txtPromedioAlm" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                        ValidationGroup="grupoAlm"></asp:RequiredFieldValidator>  
                </td>
                <td>
                    <asp:Label ID="Label14" runat="server" AssociatedControlID="txtMaximoAlm" 
                        Text="<%$ Resources:resCorpusCFDIEs, varMaximo %>"></asp:Label>
                    <asp:TextBox ID="txtMaximoAlm" runat="server" BackColor="White" Width="100px" MaxLength="10"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="txtMaximoAlm_MaskedEditExtender" runat="server" 
                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                        Mask="99.99" MaskType="Number" TargetControlID="txtMaximoAlm">
                    </cc1:MaskedEditExtender>
                    GB<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" 
                        ControlToValidate="txtMaximoAlm" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                        ValidationGroup="grupoAlm"></asp:RequiredFieldValidator>  
                </td>
                <td>
                    <asp:Label ID="Label15" runat="server" 
                        AssociatedControlID="txtCapAlm" Font-Bold="True" 
                        Text="<%$ Resources:resCorpusCFDIEs, varCapacidad %>"></asp:Label>
                    <asp:TextBox ID="txtCapAlm" runat="server" BackColor="White" MaxLength="10" 
                        Width="100px"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="txtCapAlm_MaskedEditExtender" runat="server" 
                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                        Mask="999.99" MaskType="Number" TargetControlID="txtCapAlm">
                    </cc1:MaskedEditExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" 
                        ControlToValidate="txtCapAlm" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                        ValidationGroup="grupoAlm"></asp:RequiredFieldValidator>  
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAdjuntar1" runat="server" AssociatedControlID="fuAlm" 
                        Text="<%$ Resources:resCorpusCFDIEs, lbldatosprueba %>"></asp:Label>
                    <asp:FileUpload ID="fuAlm" runat="server" Height="23px" Width="219px" />
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
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label17" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                AssociatedControlID="txtFechaIniAlm"></asp:Label>
                    <asp:TextBox ID="txtFechaIniAlm" runat="server" BackColor="White" Width="100px" ></asp:TextBox>
                    <asp:Image ID="imgIniAlm" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" >
                    </asp:Image>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="Dynamic"
                                ControlToValidate="txtFechaIniAlm" CssClass="failureNotification" 
                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                <img src="Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>
                    <cc1:calendarextender ID="txtFechaIniAlm_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txtFechaIniAlm" Format="dd/MM/yyyy" 
                                PopupButtonID="imgIniAlm">
                    </cc1:calendarextender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                ControlToValidate="txtFechaIniAlm" CssClass="failureNotification" 
                                ValidationGroup="grupoAlm" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Label ID="Label18" runat="server" 
                                Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                AssociatedControlID="txtFechaFinAlm"></asp:Label>
                    <asp:TextBox ID="txtFechaFinAlm" runat="server" BackColor="White" Width="100px"></asp:TextBox>
                    <asp:Image ID="imgFinAlm" runat="server" 
                                ImageUrl="~/Imagenes/icono_calendario.gif" >
                    </asp:Image>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="Dynamic"
                                ControlToValidate="txtFechaFinAlm" CssClass="failureNotification" 
                                ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                <img src="Imagenes/error_sign.gif" />
                                </asp:RegularExpressionValidator>
                    <cc1:calendarextender ID="txtFechaFinAlm_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txtFechaFinAlm" Format="dd/MM/yyyy" 
                                PopupButtonID="imgFinAlm">
                    </cc1:calendarextender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                ControlToValidate="txtFechaFinAlm" CssClass="failureNotification" 
                                ValidationGroup="grupoAlm" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;
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
            </tr>
        </table>
    </fieldset>
</div>
<div style="text-align:center;" >
    <asp:UpdateProgress ID="UpdateProgressAlm" runat="server" AssociatedUpdatePanelID="udpConsultasAlm" >
        <progresstemplate>
            <img alt="" 
                src="Imagenes/imgAjaxLoader.gif" />
        </progresstemplate>
    </asp:UpdateProgress>
</div>
<br />
<asp:UpdatePanel ID="udpConsultasAlm" runat="server">
    <ContentTemplate>
        <p style="text-align:right;" >
            <asp:Label ID="lblMsgAlm" runat="server" Visible="False"></asp:Label>
            <asp:Button ID="btnGuardaAlm" runat="server" 
                    Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                     ValidationGroup="grupoAlm" 
                    CssClass="botonEstilo" onclick="btnGuardaAlm_Click" />
        </p>
    </ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>

    

</cc1:TabPanel>
    <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, varRed %>">

        <ContentTemplate>
    <div class="accountInfo textos" style="width:900px;">
        <fieldset class="register" style="width:890px;">
            <legend>
                <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, varCaptura %>" />
            </legend>
            <table class="style1">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, varUbicacion %>" 
                                    AssociatedControlID="ddlUbicacionRed"></asp:Label>
                        <asp:DropDownList ID="ddlUbicacionRed" runat="server" AutoPostBack="True" onselectedindexchanged="ddlUbicacionRed_SelectedIndexChanged" 
                                    >
                            <asp:ListItem>Apolo</asp:ListItem>
                            <asp:ListItem Value="Discovery">Discovery</asp:ListItem>
                            <asp:ListItem>Chihuahua</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" AssociatedControlID="ddlDispositivoRed" 
                            Text="<%$ Resources:resCorpusCFDIEs, varDispositivo %>"></asp:Label>
                        <asp:DropDownList ID="ddlDispositivoRed" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlDispositivoRed_SelectedIndexChanged">
                            <asp:ListItem>Red Publica</asp:ListItem>
                            <asp:ListItem>Red Privada</asp:ListItem>
                        </asp:DropDownList>
                    </td>
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
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" AssociatedControlID="ddlOrigenRed" 
                            Text="<%$ Resources:resCorpusCFDIEs, varOrigen %>"></asp:Label>
                        <asp:DropDownList ID="ddlOrigenRed" runat="server">
                            <asp:ListItem>Gráfico de uso fuera del servidorGráfico de uso de disco</asp:ListItem>
                        </asp:DropDownList>
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
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" AssociatedControlID="txtPromedioRed" 
                            Text="<%$ Resources:resCorpusCFDIEs, varPromedio %>"></asp:Label>
                        <asp:TextBox ID="txtPromedioRed" runat="server" BackColor="White" Width="100px" MaxLength="10"></asp:TextBox>
                        <cc1:MaskedEditExtender ID="txtPromedioRed_MaskedEditExtender" runat="server" 
                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                            Mask="99.99" MaskType="Number" TargetControlID="txtPromedioRed">
                        </cc1:MaskedEditExtender>
                        MB<asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" 
                        ControlToValidate="txtPromedioRed" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                        ValidationGroup="grupoRed"></asp:RequiredFieldValidator> 
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" AssociatedControlID="txtMaximoRed" 
                            Text="<%$ Resources:resCorpusCFDIEs, varMaximo %>"></asp:Label>
                        <asp:TextBox ID="txtMaximoRed" runat="server" BackColor="White" Width="100px" MaxLength="10"></asp:TextBox>
                        <cc1:MaskedEditExtender ID="txtMaximoRed_MaskedEditExtender" runat="server" 
                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                            Mask="99.99" MaskType="Number" TargetControlID="txtMaximoRed">
                        </cc1:MaskedEditExtender>
                        MB<asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" 
                        ControlToValidate="txtMaximoRed" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                        ValidationGroup="grupoRed"></asp:RequiredFieldValidator> 
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" 
                            AssociatedControlID="txtRedCap" Font-Bold="True" 
                            Text="<%$ Resources:resCorpusCFDIEs, varCapacidad %>"></asp:Label>
                        <asp:TextBox ID="txtRedCap" runat="server" BackColor="White" MaxLength="10" 
                            Width="100px"></asp:TextBox>
                        <cc1:MaskedEditExtender ID="txtRedCap_MaskedEditExtender" runat="server" 
                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                            Mask="999.99" MaskType="Number" TargetControlID="txtRedCap">
                        </cc1:MaskedEditExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" 
                        ControlToValidate="txtRedCap" CssClass="failureNotification" 
                        ToolTip="<%$ Resources:resCorpusCFDIEs, valValor %>" 
                        ValidationGroup="grupoRed"></asp:RequiredFieldValidator> 
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblAdjuntar2" runat="server" AssociatedControlID="fuRed" 
                            Text="<%$ Resources:resCorpusCFDIEs, lbldatosprueba %>"></asp:Label>
                        <asp:FileUpload ID="fuRed" runat="server" Height="23px" Width="219px" />
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
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label11" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                    AssociatedControlID="txtFechaIniRed"></asp:Label>
                        <asp:TextBox ID="txtFechaIniRed" runat="server" BackColor="White" Width="100px" ></asp:TextBox>
                        <asp:Image ID="imgIniRed" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" >
                        </asp:Image>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaIniRed" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img src="Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                        <cc1:calendarextender ID="txtFechaIniRed_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaIniRed" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgIniRed">
                        </cc1:calendarextender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                    ControlToValidate="txtFechaIniRed" CssClass="failureNotification" 
                                    ValidationGroup="grupoRed" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="Label16" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                    AssociatedControlID="txtFechaFinRed"></asp:Label>
                        <asp:TextBox ID="txtFechaFinRed" runat="server" BackColor="White" Width="100px"></asp:TextBox>
                        <asp:Image ID="imgFinRed" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" >
                        </asp:Image>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaFinRed" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img src="Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                        <cc1:calendarextender ID="txtFechaFinRed_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaFinRed" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgFinRed">
                        </cc1:calendarextender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                    ControlToValidate="txtFechaFinRed" CssClass="failureNotification" 
                                    ValidationGroup="grupoRed" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
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
                </tr>
            </table>
        </fieldset>
    </div>
    <div style="text-align:center;" >
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="udpConsultasRed" >
            <progresstemplate>
                <img alt="" 
                    src="Imagenes/imgAjaxLoader.gif" />
            </progresstemplate>
        </asp:UpdateProgress>
    </div>
    <br />
    <asp:UpdatePanel ID="udpConsultasRed" runat="server">
        <ContentTemplate>
            <p style="text-align:right;" >
                <asp:Label ID="lblMsgRed" runat="server" Visible="False"></asp:Label>
                <asp:Button ID="btnRed" runat="server" 
                        Text="<%$ Resources:resCorpusCFDIEs, btnGuardar %>" 
                         ValidationGroup="grupoRed" 
                        CssClass="botonEstilo" onclick="btnRed_Click" />
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</ContentTemplate>

   
</cc1:TabPanel>
    <cc1:TabPanel ID="TabPanel5" runat="server" HeaderText="<%$ Resources:resCorpusCFDIEs, varConsultaReportes %>">

        <ContentTemplate>
    <div class="accountInfo textos" style="width:900px;">
        <fieldset class="" style="width:890px;">
            <legend>
                <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" />
            </legend>
            <table class="style1">
                <tr>
                    <td>
                        <asp:Label ID="Label8" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, varUbicacion %>" 
                                    AssociatedControlID="ddlUbicacionCon"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlUbicacionCon" runat="server" 
                                    >
                            <asp:ListItem>Apolo</asp:ListItem>
                            <asp:ListItem Value="Discovery">Discovery</asp:ListItem>
                            <asp:ListItem>Chihuahua</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td rowspan="2">
                        <asp:Label ID="Label19" runat="server" AssociatedControlID="chkLista" 
                            Text="<%$ Resources:resCorpusCFDIEs, varDispositivo %>"></asp:Label>
                        <asp:UpdatePanel ID="udpchek" runat="server">
                            <ContentTemplate>
                                <asp:CheckBoxList ID="chkLista" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="chkLista_SelectedIndexChanged" style="text-align: left" 
                                    Width="128px">
                                    <asp:ListItem>CPU</asp:ListItem>
                                    <asp:ListItem>MEMORIA</asp:ListItem>
                                    <asp:ListItem>C</asp:ListItem>
                                    <asp:ListItem>D</asp:ListItem>
                                    <asp:ListItem>SAN</asp:ListItem>
                                    <asp:ListItem>NAS</asp:ListItem>
                                    <asp:ListItem>Red Publica</asp:ListItem>
                                    <asp:ListItem>Red Privada</asp:ListItem>
                                    <asp:ListItem>TODOS</asp:ListItem>
                                </asp:CheckBoxList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="Label27" runat="server" AssociatedControlID="ddlUbicacionCon" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblPistasUsuario %>"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlUsuarios" runat="server" DataTextField="clave_usuario" 
                            DataValueField="id_usuario">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label25" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" 
                                    AssociatedControlID="txtFechaIniCon"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtFechaIniCon" runat="server" BackColor="White" Width="100px" ></asp:TextBox>
                        <asp:Image ID="imgIniCon" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" >
                        </asp:Image>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaIniCon" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img src="Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                        <cc1:calendarextender ID="txtFechaIniCon_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaIniCon" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgIniCon">
                        </cc1:calendarextender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                    ControlToValidate="txtFechaIniCon" CssClass="failureNotification" 
                                    ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="Label26" runat="server" 
                                    Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" 
                                    AssociatedControlID="txtFechaFinCon"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtFechaFinCon" runat="server" BackColor="White" Width="100px"></asp:TextBox>
                        <asp:Image ID="imgFinCon" runat="server" 
                                    ImageUrl="~/Imagenes/icono_calendario.gif" >
                        </asp:Image>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" Display="Dynamic"
                                    ControlToValidate="txtFechaFinCon" CssClass="failureNotification" 
                                    ValidationExpression="^(((0?[1-9]|[12]\d|3[01])[\/](0?[13578]|1[02])[\/](20\d{2}))|((0?[1-9]|[12]\d|30)[\/](0?[13456789]|1[012])[\/](20\d{2}))|((0?[1-9]|1\d|2[0-8])[\/]0?2[\/](20\d{2}))|(29[\/]0?2[\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))$" 
                                    ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFormatoFecha %>" >
                                    <img src="Imagenes/error_sign.gif" />
                                    </asp:RegularExpressionValidator>
                        <cc1:calendarextender ID="txtFechaFinCon_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtFechaFinCon" Format="dd/MM/yyyy" 
                                    PopupButtonID="imgFinCon">
                        </cc1:calendarextender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                                    ControlToValidate="txtFechaFinCon" CssClass="failureNotification" 
                                    ValidationGroup="grupoConsulta" ToolTip="<%$ Resources:resCorpusCFDIEs, valFechaConsulta %>" ></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:UpdatePanel ID="udpExel" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnExcel" runat="server" CssClass="botonGrande" Enabled="False" 
                                    OnClick="btnExcel_Click" Text="<%$ Resources:resCorpusCFDIEs, btnExcel %>" 
                                    Width="172px" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label28" runat="server" AssociatedControlID="txtFechaIniUsu" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>" Visible="False"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtFechaIniUsu" runat="server" BackColor="White" Width="100px" 
                            Visible="False"></asp:TextBox>
                        <cc1:CalendarExtender ID="txtFechaIniUsu_CalendarExtender" runat="server" 
                            Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgIniUsu" 
                            TargetControlID="txtFechaIniUsu">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgIniUsu" runat="server" 
                            ImageUrl="~/Imagenes/icono_calendario.gif" Visible="False" />
                    </td>
                    <td>
                        <asp:Label ID="Label29" runat="server" AssociatedControlID="txtFechaFinUsu" 
                            Text="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>" Visible="False"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtFechaFinUsu" runat="server" BackColor="White" Width="100px" 
                            Visible="False"></asp:TextBox>
                        <cc1:CalendarExtender ID="txtFechaFinUsu_CalendarExtender" runat="server" 
                            Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgFinUsu" 
                            TargetControlID="txtFechaFinUsu">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgFinUsu" runat="server" 
                            ImageUrl="~/Imagenes/icono_calendario.gif" Visible="False" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div style="text-align:center;" >
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="udpConsultasCon" >
            <ProgressTemplate>
                <img alt="" 
                    src="Imagenes/imgAjaxLoader.gif" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="udpConsultasCon" runat="server">
            <ContentTemplate>
            <p style="text-align:right;" >
                <asp:Button ID="btnConusltar" runat="server" CssClass="botonEstilo" 
                    OnClick="btnConusltar_Click" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblConsultar %>" 
                    ValidationGroup="grupoConsulta" />


                    <br />
            <asp:Panel ID="pnlGrid" runat="server" Height="200px" ScrollBars="Vertical" 
                Width="920px">

                <asp:GridView ID="gdvRegistros" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" 
                    CellPadding="4" DataKeyNames="id_Monitoreo" 
                    GridLines="Horizontal" Width="900px">
                    <Columns>
                        <asp:BoundField DataField="ubicacion" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, varUbicacion %>" />
                        <asp:BoundField DataField="tipo_dispositivo" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, varDispositivo %>" />
                        <asp:BoundField DataField="origen" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, varOrigen %>" />
                        <asp:BoundField DataField="fecha_ini" DataFormatString="{0:dd-M-yyyy}" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblFechaIni %>">
                        <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="fecha_fin" DataFormatString="{0:dd-M-yyyy}" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, lblFechaFin %>">
                        <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="promedio" DataFormatString="{0:F2}" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, varPromedio %>" />
                        <asp:BoundField DataField="maximo" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, varMaximo %>" />
                        <asp:BoundField DataField="capacidad" 
                            HeaderText="<%$ Resources:resCorpusCFDIEs, varCapacidad %>" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblArchivo %>" HeaderStyle-HorizontalAlign="Left" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="adjunto" runat="server" Target="_blank"
                                    NavigateUrl='<%# Eval("id_Monitoreo", "~/Consultas/webConsultasMonitoreo.aspx?nic={0}") %>' 
                                    Visible='<%# Convert.ToBoolean(Eval("adjunto")) %>' BorderStyle="None" 
                                    ><asp:Image ID="imgPDF" runat="server" ImageUrl="~/Imagenes/images.jpg" Width="30" BorderStyle="None" /></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Literal ID="Literal7" runat="server" 
                            text="<%$ Resources:resCorpusCFDIEs, lblGridVacio %>" />
                    </EmptyDataTemplate>
                    <FooterStyle BackColor="White" ForeColor="#333333" />
                    <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="White" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />
                </asp:GridView>

                </asp:Panel>

                <p>
                </p>

                <p>
                </p>

                <p>
                </p>

                <p>
                </p>

                <p>
                </p>

                <p>
                </p>

                <p>
                </p>

                <p>
                </p>

                <p>
                </p>

            </p>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    
</ContentTemplate>

   
</cc1:TabPanel>
</cc1:TabContainer>



    <asp:Panel ID="pnlAlarmas" runat="server" Height="250px" Width="579px" 
    CssClass="modal" BorderStyle="Solid" BorderWidth="1px">

    <table align="center">
        <tr>
            <td align="center">
                <asp:Label ID="lblDetalle" runat="server" 
                    ></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" ID="varDetAlarmas" runat="server">
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnAcepAlarms" runat="server" onclick="btnAcepAlarms_Click" 
                    Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" 
                    CssClass="botonEstilo" />
            </td>
        </tr>
    </table>
    </asp:Panel>

    <cc1:modalpopupextender id="modalAlarmas" runat="server" 
                backgroundcssclass="modalBackground" popupcontrolid="pnlAlarmas"
        targetcontrolid="lnkAlarmas" DynamicServicePath="" Enabled="True">
    </cc1:modalpopupextender>

    <asp:LinkButton ID="lnkAlarmas" runat="server"></asp:LinkButton>  

</asp:Content>


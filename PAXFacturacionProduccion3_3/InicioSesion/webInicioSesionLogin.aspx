<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webInicioSesionLogin.aspx.cs" MasterPageFile="~/webGlobalMaster.master" Inherits="InicioSesion_webInicioSesionLogin" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">   
        <div class="container">
           <div class="pull-right">
             <h2>
               <asp:ImageButton ID="imgDescarga" runat="server" ImageUrl="~/Imagenes/manual-usuario.png" onclick="imgDescarga_Click" />
             </h2>
           </div>
            <h1>
                
                <asp:Label ID="lblIncioSesion" runat="server" CssClass="TituloInicio"></asp:Label>
               
                <hr class="hrbottom" style="padding-bottom: 0px; margin-bottom: 0px;"/>
            </h1>
        </div>
    <div class="container">
        <div class="well">

        <center>
            <p>
                <img alt="" class="container-slim" src="../Imagenes/barra_gratuita.png" />
                <h3>
                    <asp:Label ID="lblBienvenida" runat="server" class="Subtitulo"></asp:Label>
                </h3>
            </p>
            <p>
                <span class="Apple-style-span" 
                    style="border-collapse: separate; color: rgb(0, 0, 0); font-family: verdana; font-size: 12px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-border-horizontal-spacing: 0px; -webkit-border-vertical-spacing: 0px; -webkit-text-decorations-in-effect: none; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; ">
                <span class="Apple-style-span" 
                    style="color: rgb(100, 100, 100); font-family: Arial, Helvetica, sans-serif; text-align: left; ">
                <asp:Label ID="lblProveedor" runat="server" 
                    style="font-family: 'Century Gothic'"></asp:Label>
                </span></span>
            </p>
             <img src="../Imagenes/login-icono.png" alt="" id="login-icono"/>
            <p>
               &nbsp;
                <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="updLogin" ID="updProgress">
                   <ProgressTemplate>
                      <img alt="" src="../Imagenes/imgAjaxLoader.gif" />
                   </ProgressTemplate>
                </asp:UpdateProgress>
            </p>
        </center>
         
        <asp:UpdatePanel ID="updLogin" runat="server">
            <ContentTemplate>
              <div class="panel-body">               
                    <div class="row">    
                       <div class="col-xs-12 col-sm-4 col-md-3 col-lg-4"> 
                       </div>         
                        <div class="col-xs-123 col-sm-4 col-md-6 col-lg-4">
                            <div class="form-group">
                                <h4>
                                <asp:Label ID="lblUsuario" CssClass="LabelTexto" runat="server" AssociatedControlID="txtUserName" Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>" style="font-family: 'Century Gothic'"></asp:Label>
                                </h4>                       
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>                 
                                     <asp:RequiredFieldValidator ID="rfvUsaurioRequerido" runat="server" 
                                          ControlToValidate="txtUserName"
                                          ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                          ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                          ValidationGroup="LoginUserValidationGroup">
                                         <span class="tooltiptext"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" /></span>
                                     </asp:RequiredFieldValidator></div></div><div class="col-xs-12 col-sm-4 col-md-3 col-lg-4">
                        </div>
                     </div>    
                    <div class="row">
                       <div class="col-xs-12 col-sm-4 col-md-3 col-lg-4">
                       </div>         
                       <div class="col-xs-12 col-sm-4 col-md-6 col-lg-4">
                            <div class="form-group">
                               <h4>                  
                               <asp:Label ID="lblContrasenia" runat="server" AssociatedControlID="txtPassword" Text="<%$ Resources:resCorpusCFDIEs, lblContrasenia %>" style="font-family: 'Century Gothic'"></asp:Label></h4>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" MaxLength="50" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvContrasenaRequerida" runat="server" 
                                            ControlToValidate="txtPassword"  
                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>" 
                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>" 
                                            ValidationGroup="LoginUserValidationGroup">
                                           <span class="tooltiptext"><asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblContraseniaRequerida %>" /></span>
                                       </asp:RequiredFieldValidator></div></div></div><div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <img alt="SAT" class="Imag" src="../Imagenes/proveedorpac.png" style="width:98px; height:140px;" />
                    </div>
                    </div>
                    <div class="row" >
                        <div class="col-xs-12 col-sm-4 col-md-3 col-lg-4">
                        </div>
                        <div class="col-xs-12 col-sm-4 col-md-6 col-lg-4">
                            <div class="form-group">  
                                 <asp:Button ID="btnEntrar" runat="server" onclick="btnEntrar_Click" Text="<%$ Resources:resCorpusCFDIEs, lblEntrar %>" ValidationGroup="LoginUserValidationGroup" 
                                             style="font-family: 'Century Gothic'" CssClass="btn btn-primary-red center-block BotonSinTop" 
                                             Width="75px" />   
                             </div>
                        </div>
                   </div>
                    <!--Separador-->             
                    <div class="row">
                        <div class="col-xs-1 col-sm-4 col-md-3 col-lg-4">
                        </div>
                        <div class="col-xs-10 col-sm-4 col-md-3 col-lg-4">
                            <div class="form-group">  
                                <center>  
                                <asp:Label ID="lblRegistrate" runat="server" CssClass="text-center" Text="<%$ Resources:resCorpusCFDIEs, lblRegistrate %>" style="font-family: 'Century Gothic'"></asp:Label></center></div></div><div class="col-xs-1 col-sm-4 col-md-3 col-lg-4">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-1 col-sm-4 col-md-3 col-lg-4">
                        </div>
                        <div class="col-xs-10 col-sm-4 col-md-3 col-lg-4">
                             <div class="form-group">
                                 <asp:LinkButton ID="lnkRecuperar" runat="server" onclick="lnkRecMsg_Click" Text="<%$ Resources:resCorpusCFDIEs, hpkRecuperar %>"></asp:LinkButton><a>| </a><asp:HyperLink ID="hpkRegistrar" runat="server"  Text="<%$ Resources:resCorpusCFDIEs, lblRegistrar %>"  NavigateUrl="~/InicioSesion/webInicioSesionRegistro.aspx"></asp:HyperLink></div></div><div class="col-xs-1 col-sm-4 col-md-3 col-lg-4">
                        </div>
                    </div>
              </div>       
                 
             <!--modal fade -->


            <!--Modal de Recupera Contrasenia-->             
             <!--modal fade -->

            <!--Modal de Aviso-->
                <div class="modal fade" id="divModalAviso" tabindex="-1" role="dialog" aria-labelledby="divModalAviso" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <asp:UpdatePanel ID="upModalAviso" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="modal-content">
                                    <div class="modal-header">
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-xs-2 col-sm-2 col-md-2 col-lg-3">
                                            </div>
                                            <div class="col-xs-10 col-sm-10 col-md-4 col-lg-9">
                                                <asp:Label ID="lblAviso" runat="server"></asp:Label></div></div></div><div class="modal-footer">
                                        <asp:Button ID="btnAviso" runat="server" CssClass="btn btn-primary-red" onclick="btnAviso_Click"
                                           Text="<%$ Resources:resCorpusCFDIEs, lblAceptar %>" data-dismiss="modal" aria-hidden="true"/>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

            <!--Modal de Desbloqueo-->             
          
            <div class="modal fade" id="divModalRecupera" tabindex="-1" role="dialog" aria-labelledby="divModalRecupera" aria-hidden="true" data-replace="true">
               <asp:LinkButton ID="lnkRecupera" runat="server">
               </asp:LinkButton><div class="modal-dialog" role="document">
                    <%--<asp:UpdatePanel ID="upModalRecupera" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4>
                                    <asp:Label ID="lblRecuperaCuenta" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblRecuperaCuenta %>" ></asp:Label></h4><hr class="hrbottom" /><h4>
                                         <asp:Label ID="lblRecupera" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblDetRecupera %>"></asp:Label></h4></div><div class="modal-body">
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-xs-1 col-sm-3 col-md-2 col-lg-3">
                                                </div>
                                                <div class="col-xs-10 col-sm-7 col-md-8 col-lg-6">
                                                    <h4>
                                                    <asp:Label ID="lblUsuarioRec" runat="server" AssociatedControlID="txtUsuario" Text="<%$ Resources:resCorpusCFDIEs, lblUsuario %>"></asp:Label></h4><asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox><asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                                            ControlToValidate="txtUsuario"  
                                                            ErrorMessage="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                                            ToolTip="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" 
                                                            ValidationGroup="RegisterUserValidationGroup">
                                                            <span class="tooltiptext"><asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblUsuarioRequerido %>" /></span> 
                                                        </asp:RequiredFieldValidator></div></div></div><div class="row">
                                            <div class="form-group">
                                                <div class="col-xs-1 col-sm-3 col-md-2 col-lg-3">
                                                </div>
                                                <div class="col-xs-10 col-sm-7 col-md-8 col-lg-6">
                                                    <h4>
                                                    <asp:Label ID="lblCorreo" runat="server" AssociatedControlID="txtCorreo" Text="<%$ Resources:resCorpusCFDIEs, lblCorreo %>"></asp:Label></h4><asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox><asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                                                ControlToValidate="txtCorreo" Display="Dynamic" 
                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" 
                                                                ValidationGroup="RegisterUserValidationGroup">
                                                                <span class="tooltiptext"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:resCorpusCFDIEs, lblCorreoRequerido %>" /></span> 
                                                            </asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                                ControlToValidate="txtCorreo" Display="Dynamic" 
                                                                ToolTip="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" 
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                                ValidationGroup="RegisterUserValidationGroup">
                                                                <span class="tooltiptext"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:resCorpusCFDIEs, txtCorreo %>" /></span> 
                                                            </asp:RegularExpressionValidator></div></div></div><div class="row">
                                            <div class="form-group">
                                                <div class="col-xs-1 col-sm-3 col-md-2 col-lg-4">
                                                </div>
                                                <div class="col-xs-10 col-sm-7 col-md-8 col-lg-4">
                                                    <div class="form-group"> 
                                                        <label> Captcha: </label><asp:Image ID="ImageCaptchaRec" runat="server" AlternateText="If you can't read this number refresh your screen" ImageUrl="~/captcha.ashx"/>
                                                        <asp:ImageButton ID="bntRecargaRec" OnClick="bntRecargaRec_Click" runat="server" Height="16px" ImageUrl="~/Imagenes/reload_captcha.png" Width="16px" TabIndex="6"/>
                                                    </div>
                                                        <asp:TextBox ID="txtNumeroRec" runat="server" EnableViewState="False" MaxLength="8" TabIndex="7" CssClass="form-control"></asp:TextBox><asp:RequiredFieldValidator ID="vrfNumeroRec" runat="server" 
                                                                ControlToValidate="txtNumeroRec" Display="Dynamic"
                                                                ErrorMessage="<%$ Resources:resCorpusCFDIEs, txtNumero %>" ToolTip="<%$ Resources:resCorpusCFDIEs, txtNumero %>" 
                                                                ValidationGroup="RegisterUserValidationGroup">
                                                                <span class="tooltiptext"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:resCorpusCFDIEs, txtNumero %>" /></span>
                                                            </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnCrearCuenta" runat="server" CssClass="btn btn-primary-red" ValidationGroup="RegisterUserValidationGroup" onclick="btnRecuperaCuenta_Click" Text="<%$ Resources:resCorpusCFDIEs, btnRecuperaCuenta %>" aria-hidden="true" />            
                                        <asp:Button ID="btnCanRec" runat="server" CssClass="btn btn-primary-red"  onclick="btnCanRec_Click" Text="<%$ Resources:resCorpusCFDIEs, btnCancelar %>"  data-dismiss="modal" aria-hidden="true" />
                                    </div>
                                </div>
                        <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                    </div>
                </div>

        </div> <!--cierra well -->
    </div>

</asp:Content>

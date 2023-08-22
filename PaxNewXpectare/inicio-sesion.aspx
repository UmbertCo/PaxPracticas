<%@ Page Language="C#" AutoEventWireup="true" CodeFile="inicio-sesion.aspx.cs" Inherits="inicio_sesion" MasterPageFile="MasterPage.master"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
 <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/login.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row">
                	<div id="inicio-sesion" class="span12">
                        <div class="separador-login"></div>
                        <h1>Inicio de Sesión</h1>
                        <a href="https://www.paxfacturacion.com.mx:452/InicioSesion/webInicioSesionLogin.aspx?ReturnUrl=%2f" id="manual-usuario"><img src="img/manual-usuario.png" alt="" /></a>
                        <div class="row">
                        	<div class="span12 block-green">  
                                 <h3>Bienvenido al Servicio de Emisión y Timbrado de Facturación Electrónica de PAX Facturación.</h3>
                                 <p> Este servicio se otorga por disposición del SAT ya que somos un Proveedor Autorizado de Certificación. </p>
                            	 <img src="img/login-icono.png" alt="" id="login-icono"/>
                                    <div id="login-form">
                                        <form id="valida-interna">
                                            <fieldset>
                                            	<input type="text" id="usuario" name="usuario" placeholder="Usuario" class="required"/>
                                                <input type="password" placeholder="Contraseña" id="password" name="password" class="campo-texto" />
                                                <input type="submit" value=""/>
                                        <div id="exito-interna">
                                        </div>                              
                                            </fieldset>
                                        </form>
                                    </div><!-- #login-form -->
                                       
                                 <p style="font-size:11px;">Registrarse como nuevo usuario: </p>
								 <p style="color:#309cc0; font-size:11px;"> <a href=""> Registrar </a> |  <a href=""> Recuperar Contraseña </a>  |  <a href=""> Reactivar Cuenta </a></p>
                            	
                                 <img src="img/separador-sesion.png" alt="" id="separador-sesion"/>
                                 
                                 <p style="font-size:11px; line-height:30px;"> <span style="color:#e4583d; margin-right:1px;">*</span> En caso de cerrar mal la página y necesitar continuar usando el sistema existe <br> 
                                 la opción de desbloquear la cuenta por este medio. </p>
								 <p style="color:#309cc0; font-size:11px;"> |<a href="">Desbloquear </a> |</p>                                 
                    
                           </div><!-- .span12 -->
                        </div>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
      </asp:Content>
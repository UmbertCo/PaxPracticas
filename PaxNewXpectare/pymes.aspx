<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pymes.aspx.cs" Inherits="pymes" MasterPageFile="MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
 <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/pymes.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row fix">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Pymes</span></p>
                        <div class="separador"></div>
                        <h1>Pymes</h1>
                        <div class="row">
                        	<div class="span8 block-green">
                                <h2>Si tú tienes software para administrar tu negocio, nosotros podemos ayudarte para que no modifiques tu forma de trabajar y sigas dando tu mismo servicio.</h2>
                                <a href="servicios-complementarios.aspx"><img src="img/nuestros-servicios.png" alt="" style="margin-left:30px;"/></a>
                                <h4>Ventajas y Beneficios</h4>
                                <ul>
                                    <li>Nuestro compromiso con su empresa, es cubrir todas sus necesidades, obteniendo de esta manera su confianza y satisfacción.</li>
                                </ul>
                                <ol style="padding:0 58px;">
                                    <li>Con nosotros siempre obtendrá ATENCIÓN, SERVICIO Y SEGURIDAD.</li>
                                    <li>SEGURIDAD en su información. Contamos con respaldos y accesos restringidos.</li>
                                    <li>Contamos con el SERVICIO y ATENCIÓN personalizada con soporte técnico las 24 horas, los 7 días de la semana y los 365 días del año.</li>
                                    <li>Cuenta con la SEGURIDAD de estar contratando a un equipo capaz de brindar un servicio de alta calidad, disponibilidad y
                                        eficiencia a un BAJO COSTO.</li>
                                    <li>Somos un proveedor autorizado del SAT.</li>
                                </ol>
                                <!-- <a href="#"><img src="img/nuestros-servicios.png" alt="" id="nuestros-servicios"/></a> -->
                            </div><!-- .span8 -->
                            <div class="span4 block-blue">
                            	<a href="registro.aspx"><img src="img/compra-ahora.jpg" alt="" id="compra-ahora"/></a>
                                <a href="servicios-complementarios.aspx"><img src="img/estas-listo.jpg" alt=""/></a>
                            </div><!-- .span4 -->
                        </div>
                    </div><!-- .span12 -->
                </div><!-- .row -->
                <div class="row">
                	<div class="span4">
                    	<iframe id="video" width="370" height="284" src="//www.youtube.com/embed/dmo7QyI5Wu8" frameborder="0" allowfullscreen></iframe>
                    </div><!-- .span4 -->
                    <div class="span4">
                    	<div id="suscribete">
                            <form id="valida-interna">
                                <fieldset>
                                    <input type="email" placeholder="Email" id="email" name="email" class="campo-texto"/>
                                    <input type="submit" value=""/>
                                    <div id="exito-interna"></div>                              
                                </fieldset>
                            </form>
                        </div><!-- #suscribete -->
                    </div><!-- .span4 -->
                    <div class="span4">
                    	<div id="noticias">
                        	<h3>Modificaciones a la Resolución Miscelánea Fiscal para 2013.</h3>
                            <p>A partir de 2014, todos los contribuyentes que utilizan el esquema de Comprobante Fiscal Digital (CFD) deberán utilizar el esquema de Comprobante Fiscal Digital por Internet (CFDI) para la emisión de sus facturas electrónicas. <br/>
                            A partir de 2014 los contribuyentes con ingresos superiores a 250 mil pesos en el año, deberán utilizar el esquema de CFDI para la emisión de sus facturas electrónicas.<a href="#">+ Ver completa</a></p>
                            <div id="acciones">
                            	<a href="#" class="ver-mas">Ver más noticias</a>
                                <a href="#" onclick="share('http://xpectare.com',626,436);"><img src="img/compartir.png" alt=""/></a>
                            </div><!-- #acciones -->
                        </div><!-- #noticias -->
                    </div><!-- .span4 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
    </asp:Content>
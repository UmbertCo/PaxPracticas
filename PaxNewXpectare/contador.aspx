<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/contador.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Contador</span></p>
                        <div class="separador"></div>
                        <h1>Soluciones para Contadores</h1>
                        <div class="row">
                        	<div class="span8 block-green">
                                <h4>Facturación electrónica para ti:</h4>
                                <div class="encabezado-contenido">
                                    <div class="icono-encabezado-contenido">
                                        <img src="img/contador-icon.gif" alt=""/>
                                    </div>
                                    <div class="texto-encabezado-contenido">
                                        <p>Si alguien conoce y experimenta la problemática de la transición de Comprobantes fiscales son los Contadores.<br/><br/>
                                        ¿Por qué no aprovechar ese conocimiento y tener ingresos adicionales?</p>
                                    </div>
                                </div>
                                <div style="clear:both;height:1px;"></div>
                                <ul>
                                    <li>Aprovecha tu cartera de clientes actual y haz negocio con PAX, todos los clientes que actualmente facturan en papel a través de CBB o por CFD a partir de 2014 deben de usar Comprobante Fiscal Digital a través de Internet.</li>
                                    <li>Te ofrecemos la oportunidad hacer negocio con la facturación, y continuar cumpliendo las obligaciones ante el SAT de una manera sencilla y práctica.</li>
                                </ul>
                                 <div id="boton-internas-container">
                                    <a href="#">
                                        <div class="boton-internas">
                                            <a href="guia.aspx"><img src="img/boton-internas.png" alt=""/>
                                            <span id="texto-superior-boton">Te invitamos a empezar a facturar</span><br /><span id="texto-inferior-boton">de inmediato con PAX Facturación</span>
                                        </div>
                                    </a>
                                </div>
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


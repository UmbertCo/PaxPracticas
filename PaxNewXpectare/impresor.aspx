<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
              
        <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/impresor.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Impresor</span></p>
                        <div class="separador"></div>
                        <h1>Soluciones para Impresores</h1>
                        <div class="row">
                        	<div class="span8 block-green">
                                <h4>Facturación electrónica para ti:</h4>
                                <div class="encabezado-contenido">
                                    <div class="icono-encabezado-contenido">
                                        <img src="img/impresor-icon.gif" alt=""/>
                                        <img src="img/qr-code.gif" alt=""/>
                                    </div>
                                    <div class="texto-encabezado-contenido">
                                        <p>Aprovecha tu cartera de clientes actual y haz negocio con PAX, todos los clientes que actualmente factura en papel a través de CBB a partir de 2014 deben de usar Comprobante Fiscal Digital por Internet.<br/><br/>
                                        Te ofrecemos la oportunidad de seguir haciendo negocio con la facturación, y cumpliendo las obligaciones ante el SAT de una manera sencilla y práctica.</p>
                                    </div>
                                </div>
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


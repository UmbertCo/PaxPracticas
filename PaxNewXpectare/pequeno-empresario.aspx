<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pequeno-empresario.aspx.cs" Inherits="Default2" MasterPageFile="MasterPage.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    
            <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/pequeno-empresario.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Pequeño Empresario</span></p>
                        <div class="separador"></div>
                        <h1>Soluciones para Pequeños Empresarios</h1>
                        <div class="row">
                        	<div class="span8 block-green">
                                <h4>Facturación electrónica para ti:</h4>
                                <div class="encabezado-contenido">
                                    <div class="icono-encabezado-contenido">
                                        <img src="img/pequeno-empresario-icon.gif" alt=""/>
                                    </div>
                                    <div class="texto-encabezado-contenido">
                                        <p>Si eres un pequeño empresario o emprendedor sin importar el régimen o tu actividad, y expides facturas a través de un Código Bidimensional o CFD, es momento de cambiar a Comprobante Fiscal Digital por Internet.<br/><br/>
                                        En 2014 todos las empresas sin importar su tamaño deben de usar este método para expedir sus comprobantes fiscales.</p>
                                    </div>
                                </div>
                                <ul>
                                    <li>Te ofrecemos la oportunidad de expedir estos comprobantes para tus clientes de una manera sencilla y práctica.</li>
                                    <li>¿Cómo obtenerlos?
                                        <ol>
                                            <li>Contar con tu Firma Electrónica Avanzada (FIEL) otorgada por el SAT.</li>
                                            <li>Solicitar por lo menos un Certificado de Sello Digital (CSD) al SAT.</li>
                                            <li>Regístrate en PAX Facturación (Regístrate).</li>
                                            <li>Comprar un número de timbres según tus consumos de comprobantes.</li>
                                            <li>Configurar tus datos generales dentro del sistema e inicia a facturar de inmediato.</li>
                                        </ol>
                                    </li>
                                    <li>Si tienes dudas o deseas conocer a detalle los pasos y requisitos consulta nuestra Guía Interactiva.</li>
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
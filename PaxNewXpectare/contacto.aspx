<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="contacto.aspx.cs" Inherits="contacto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
     <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/contacto.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row fix">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Contacto</span></p>
                        <div class="separador"></div>
                        <h1>Contáctanos</h1>
                        <div class="row">
                        	<div class="span8 block-green">
                                <h4 style="width:96.2%;">Envía un Email</h4>
                                <div class="contacto">
                                    <form id="contactoForm">
                                    <fieldset>
                                        <span class="nombre"><input type="text" id="nombre" name="nombre" placeholder="Nombre" class="required"/></span>
                                        <span class="email"><input type="text" id="email" name="email" placeholder="Email" class="email required"/></span>
                                        <span class="comentario"> <textarea id="comentario" name="comentario" placeholder="Comentario" class="required"></textarea></span>
                                        <div style="width:100%;">
                                            <div style="width:270px; margin:0 auto;">
                                                <input type="reset" id="botonborrar" value="Borrar"/>
                                                <input type="submit" id="botonenviar" value="Enviar"/>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <div id="exito"> </div>
                                    </form>
                                </div>
                                <div class="contacto-derecha">
                                    <img src="img/telefono.gif" alt="" id="telefono"/>
                                    <h2 style="font-size:20px; padding:0; text-transform:uppercase; line-height:30px; display:inline;">Llame al<br/>servicio<br/>al cliente</h2>
                                    <p>Comuníquese por teléfono con el servicio al cliente las 24 horas.</p>
                                    <p id="tel-sup">(01 800)</p>
                                    <p id="tel-inf">00 729 00</p>
                                    <p id="tel-sup">(01 800)</p>
                                    <p id="tel-inf">00 PAX 00</p>
                                    <div class="separador-contacto-derecha"></div>
                                    <a href="#" class="chatenlinea"><img src="img/chat-en-linea.png" alt=""/></a>
                                </div>
                                <h4 style="width:96.2%; margin-bottom:40px; display:block; float:left;">Sucursales</h4>
                                <div class="sucursales">
                                    <div class="sucursales-i">
                                        <img src="img/cuu.png" alt=""/>
                                        <h2 style="font-size:20px; padding:0;">Chihuahua Matríz</h2>
                                        <ul style="padding:0 20px;">
                                            <li>
                                                Parque de Innovación y Transferencia de Tecnología Piso 2, Local 5<br/>
                                                Av. Heroico Colegio Militar #4709-2<br/>
                                                Col. Nombre de Dios, Chihuahua, Chih. México C.P. 31105<br/>
                                                <span id="vineta-titulo">Teléfono : (614) 424 04 44</span>
                                            </li>
                                        </ul>
                                        <div class="separador-contenido"></div>
                                        <img src="img/cjs.png" alt=""/>
                                        <h2 style="font-size:20px; padding:0;">Cd. Juárez</h2>
                                        <ul style="padding:0 20px; margin-bottom:62px;">
                                            <li>
                                                Centro Ejecutivo CELU Piso 1, Local 22.<br/>
                                                <span id="Span1">Teléfono : (656) 616 16 44</span>
                                            </li>
                                        </ul>
                                        <div class="separador-contenido"></div>
                                    </div>
                                    <div class="sucursales-d">
                                        <img src="img/df.png" alt=""/>
                                        <h2 style="font-size:20px; padding:0;">Cd. de México, D.F.</h2>
                                        <ul style="padding:0 20px; margin-bottom:108px;">
                                            <li>
                                                World Trade Center México (WTC) Piso 39, Oficina 34.<br/>
                                                <span id="Span2">Teléfono : (55) 9000 26 90</span>
                                            </li>
                                        </ul>
                                        <div class="separador-contenido"></div>
                                        <img src="img/mty.png" alt=""/>
                                        <h2 style="font-size:20px; padding:0;">Monterrey</h2>
                                        <ul style="padding:0 20px;">
                                            <li>
                                                Centro de Innovación y Transferencia Tecnología (CIT2) Pabellón TEC<br/>
                                                Local 38, Oficina 36-E.<br/>
                                                <span id="Span3">Teléfono : (81) 1967 50 85</span>
                                            </li>
                                        </ul>
                                        <div class="separador-contenido"></div>
                                    </div>
                                </div>                          
                                <!-- <a href="#"><img src="img/nuestros-servicios.png" alt="" id="nuestros-servicios"/></a> -->
                            </div><!-- .span8 -->
                            <div class="span4 block-blue">
                            	<a href="#"><img src="img/compra-ahora.jpg" alt="" id="compra-ahora"/></a>
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
                                    <input type="email" placeholder="Email" id="email1" name="email" class="campo-texto"/>
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


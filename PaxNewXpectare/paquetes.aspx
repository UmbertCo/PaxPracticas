<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
     <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/paquetes-pax.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Paquetes</span></p>
                        <div class="separador"></div>
                        <h1>Paquetes</h1>
                        <div class="row">
                        	<div class="span8 block-green" id="paquetes">
                                <div id="contenedor-consumo">
                                    <h4 style="width:96.2%;">Selecciona tu Consumo</h4>
                                    <form>
                                        <input type="checkbox" name="anual" id="anual" value="Anual"/>
                                        <label for="anual"><span></span>Anual (Compra única)</label>
                                        <input type="checkbox" name="mensual" id="mensual" value="Mensual"/>
                                        <label for="mensual"><span></span>Mensual (Conoce los beneficios de nuestro programa de lealtad)</label>
                                    </form>
                                </div>
                                <div id="contenedor-paquetes">
                                    <h4 style="width:96.2%;">Nuestros Paquetes</h4>
                                    <form>
                                        <ul>
                                            <li><a href="#"><img src="img/paquete-1.png" alt=""/></a><input type="checkbox" name="paquete-1" id="paquete-1" value="Paquete 1"/><label for="paquete-1"><span></span></label></li>
                                            <li><a href="#"><img src="img/paquete-2.png" alt=""/></a><input type="checkbox" name="paquete-2" id="paquete-2" value="Paquete 2"/><label for="paquete-2"><span></span></label></li>
                                            <li><a href="#"><img src="img/paquete-3.png" alt=""/></a><input type="checkbox" name="paquete-3" id="paquete-3" value="Paquete 3"/><label for="paquete-3"><span></span></label></li>
                                            <li><a href="#"><img src="img/paquete-4.png" alt=""/></a><input type="checkbox" name="paquete-4" id="paquete-4" value="Paquete 4"/><label for="paquete-4"><span></span></label></li>
                                        </ul>
                                    </form>
                                </div>
                                <div id="contenedor-promocode">
                                    <form id="codigo-de-promo">
                                        <label for="" id="promocode">Código de Promoción</label>
                                        <input type="number" id="Number1" name="codigo-de-promo" placeholder="" class="required"/> <input type="submit" placeholder="" id="activar-codigo" value="Activar"/> <label for="activar-codigo">¡Activar aquí!</label>
                                    </form>
                                </div>
                                <h4 style="width:96.2%;">Paquete Personalizado</h4>
                                <div id="contenedor-personalizado">
                                    <form id="cantidad-de-facturas">
                                        <label for="no-facturas" id="label-facturas">Introduce el número de facturas que necesitas en tu paquete.</label>
                                        <input type="number" id="no-facturas" name="no-facturas" placeholder="" class="required" style=""/> <input type="submit" placeholder="" id="cotizar" value="Cotizar"/>
                                        <label for="costo-total" id="label-costo">Costo total de su paquete:</label>
                                    </form>
                                </div>
                                <!-- <a href="#"><img src="img/nuestros-servicios.png" alt="" id="nuestros-servicios"/></a> -->
                            </div><!-- .span8 -->
                            <div class="span4 block-blue" id="sidebar-paquetes" style="background-color:#eef5f6;">
                                <h4>Forma de Pago</h4>
                            	<form>
                                    <input type="checkbox" name="pago-tarjeta" id="pago-tarjeta" value="Pago de Tarjeta"/>
                                    <label for="pago-tarjeta"><span></span>Pago de Tarjeta</label>
                                    <input type="checkbox" name="pago-banco" id="pago-banco" value="Pago en Banco"/>
                                    <label for="pago-banco"><span></span>Pago en Banco</label>
                                </form>
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


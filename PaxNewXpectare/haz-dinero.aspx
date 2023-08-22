<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="haz-dinero.aspx.cs" Inherits="haz_dinero" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

       <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/haz-dinero.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row fix">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Haz Dinero con PAX</span></p>
                        <div class="separador"></div>
                        <h1>Haz Dinero con Pax</h1>
                        <div class="row">
                        	<div class="span8 block-green">
                             		<h2 style="margin-right:30px">Si quieres tener ingresos adicionales a tus
                                    actividades  actuales  y aprovechar la 
                                    oportunidad de este nuevo negocio, 
                                    contáctanos y empieza a ganar dinero 
                                    en poco tiempo. 
                                    </h2>          
                                    <ul style="padding-bottom:0px; ">
                                        <li style="line-height:30px; ">Nuestro sistema de distribución de facturas electrónicas  es el mejor del mercado, nosotros NO te pedimos de una inversión inicial o de una cantidad exorbitante para adquirir una franquicia. Inicia un negocio de facturación electrónica sin desembolsar un solo peso.</li>         
                                     </ul>  
                                     <h4>Si eres:</h4>                                     
                            	<div id="impresor">
                                	<div class="fondo-haz-dinero">
                    					<img src="img/fondo-impresor.png" alt="" />  
                                        <p class="texto1-haz-dinero">Todos tus clientes de Código bidimensional tienen que cambiar a Factura Electrónica (CFDI, Comprobante Fiscal Digital a través de Internet), aprovecha esta oportunidad e invítalos a cambiar.</p>
              					   </div>  
                                </div>    
                            	<div id="contador">
                                	<div class="fondo-haz-dinero">
                    					<img src="img/fondo-contador.png" alt="" />  
                                        <p class="texto1-haz-dinero"> Ayuda a tus clientes actuales a migrar a factura electrónica y genera un ingreso adicional.</p>
              					   </div>  
                                </div>
                            	<div id="software">
                                	<div class="fondo-haz-dinero">
                    					<img src="img/fondo-desarrollador-sw.png" alt="" />  
                                        <p class="texto1-haz-dinero"> Si cuentas con una aplicación de punto de venta, software administrativo o ERP, integra tu solución con PAX y empieza a ganar dinero.</p>
              					   </div>  
                                </div>  
                            	<div id="contactanos">
                                	<div class="fondo-haz-dinero">
                    					<a href="contacto.aspx"><img src="img/contactanos-negocio.png" alt="" /></a>
              					   </div>  
                                </div>                                  
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


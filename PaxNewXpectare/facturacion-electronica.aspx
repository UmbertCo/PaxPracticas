<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
     <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/facturacion-electronica.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Facturación Electrónica</span></p>
                        <div class="separador"></div>
                        <h1>Facturación Electrónica</h1>
                        <div class="row">
                        	<div class="span8 block-green">
                            	<div id="registrate">
                                	<div class="fondo-facturacion">
                    					<img src="img/facturacion-electronica1.png" alt="" />  
                                        <p class="texto1-facturacion">Si no cuentas con un software de facturación inicia a facturar de inmediato. </br>
                                        <strong style="font-size:14px">Programa de facturación electrónica en línea ¡GRATIS!</strong></p>
                                        
                                        <p class="texto2-facturacion">¡La aplicación más</br>
                                        fácil y segura!</p>                                          
                                        
                                        <p class="texto3-facturacion">Cumple con las disposiciones del SAT y ¡factura electrónicamente!</br>
                                        <strong style="font-size:14px">El mejor servicio, ¡Compruébalo!</strong></p>               
                                    </div>  
                    				<a href="registro.aspx"><img src="img/boton-facturacion.png" alt="" class="boton-facturacion"/></a>
                                </div>    
                            	<div id="aplicaciones">
                                	<div class="fondo-facturacion">
                    					<img src="img/facturacion-electronica2.png" alt="" />  
                                        <p class="texto4-facturacion">Si ya cuentas con un software administrativo y solo requieres el timbrado Fiscal, 
                                        regístrate y adquiere créditos de inmediato.</p>
                                        
                                        <p class="texto5-facturacion">Conoces las</br>
                                        aplicaciones ya</br>
                                        integradas o</br>
                                        integra la tuya
                                        </p>                                          
                                        
                                        <p class="texto6-facturacion"><strong>¡Seguridad y rapidez,</br>
										lo que tu empresa</br>
										necesita!</strong>
                                        </p>               
                                    </div>  
                    				<a href="registro.aspx"><img src="img/boton-facturacion2.png" alt="" class="boton-facturacion2"/></a>
                                </div>       
                            	<div id="compra-linea">
                                	<div class="fondo-facturacion">
                    					<img src="img/facturacion-electronica3.png" alt=""/>  
                                        <p class="texto7-facturacion"><strong>Si ya eres</br>
                                        cliente de</strong></p>
                                        
                                        <p class="texto8-facturacion"><strong> y quieres</br>
                                        adquirir más</br>
                                        timbres</strong></p>                                          
                                        
                                        <p class="texto9-facturacion"><strong>Hazlo aquí</strong></p>               
                                    </div>  
                    				<a href="registro.aspx"><img src="img/boton-facturacion3.png" alt="" class="boton-facturacion2"/></a>
                                </div>              
                            	<div class="row">
                    				<a href="guia.aspx"><img src="img/facturacion-electronica-guia.png" alt="" id="guia"/></a>
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


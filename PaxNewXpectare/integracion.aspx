<%@ Page Language="C#"   MasterPageFile="MasterPage.master"  AutoEventWireup="true" CodeFile="integracion.aspx.cs" Inherits="integracion" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

       <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/integracion-pax.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row fix">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Integración PAX</span></p>
                        <div class="separador"></div>
                        <h1>Integración Pax</h1>
                        <div class="row">
                        	<div class="span8 block-green">
                            
                             		<h2 style="margin-right:30px">Si cuentas con un ERP o Software 
                                    administrativo y te interesa timbrar 
                                    tus documentos fiscales con el PAC más 
                                    rápido y confiable.</h2>
                                    
                                    <ul style="padding-bottom:0px; margin:0; ">
                                        <li style="line-height:30px; ">Tenemos métodos que pueden ayudar a una integración sin dolores de cabeza y hecha por expertos
                                         que conocen de sistemas administrativos sin importar el lenguaje o el paquete que utilices.</li>         
                                     </ul>  
                                     <ol style="padding-bottom:4px; padding-left:62px; margin:0;">
										<li style="list-style:none;">No importa la tecnología en que está desarrollado tenemos un método adecuado para cada aplicación. </li>
										<li style="list-style:none;">Desde servicios de archivos de texto o el XML ya armado.</li>
                                      </ol>

                                     <h4>WEB Service TXT o XML</h4>  
                                     <ul style="padding-bottom:0px; margin:0; ">
                                        <li style="line-height:30px; ">Integración de servicios SOAP, recomendado para su ERP.</li>         
                                     </ul>                                       
                                                                      
                                     <h4> Conector PAX TXT o XML</h4>   
                                     <ul style="padding-bottom:0px; margin:0; ">
                                        <li style="line-height:30px; ">Herramienta de procesamiento alterna para su ERP.</li>
                                     </ul>                                           
                                                            
                            	<div class="row" style="height:208px;">
                    				<a href="contacto.aspx"><img src="img/integracion.png" alt="" id="guia"/>  </a>
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
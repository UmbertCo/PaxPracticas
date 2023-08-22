<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" MasterPageFile="MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
 <section>
        <div class="container">
                <div class="row">
                    <div class="span12"  id="slider">
                        <div class="flexslider">
                          <ul class="slides">
                            <li>
                              <a href="registro.aspx"><img src="img/1.jpg" alt=""/></a>
                            </li>
                            <li>
                              <a href="#"><img src="img/2.jpg" alt=""/></a>
                            </li>
                          </ul>
                        </div><!-- .flexslider -->
                                
                        <div id="suscribete">
                            <form id="valida">
                                <fieldset>
                                    <input type="text" placeholder="Email" id="email" name="email" class="campo-texto emailtexto"/>
                                    <input type="submit" value="Enviar" class="btnEnviar"/>
                             <div id="exito-suscribete"> </div>                              
                                </fieldset>
                            </form>
        
                        </div><!-- #suscribete -->
                     </div><!-- .span12-->
                 </div><!-- #flexslider -->
        	
            	<div class="row">
                	<div id="blocks" class="span8" >
                	<div id="blocks-container">
                    		
                            <a href="facturacion-electronica.aspx" style="display:block;" >
                            	<div class="block block-1 span3">
                                	<p>El más rápido y confiable del mercado, adquiere tus facturas electrónicas.</p>
                                    <img src="img/btnConoce.png" alt=""/>
                                </div><!-- .block -->
                            </a>
                            
                            <a href="integracion.aspx" style="display:block;" >
                                <div class="block block-2 span3">
                                	<p>Incorpora la facturación electrónica a tu sistema administrativo o ERP.</p>
                                    <img src="img/btnVer.png" alt=""/>
                                </div><!-- .block -->
                            </a>
                           
                            
                            <a href="haz-dinero.aspx" style="display:block;">
                                <div class="block block-3 span3">
                                	<p>Conoce las oportunidades que pax tiene para promover y vender nuestros servicios.</p>
                                    <img src="img/btnHaz.png" alt=""/>
                                </div><!-- .block -->
                            </a>
                            
                        
                    </div><!-- .span8 -->
                   </div> 
                    <div class="span4">
                    	<iframe  id="video-interna" width="370" height="284" src="//www.youtube.com/embed/dmo7QyI5Wu8" frameborder="0" allowfullscreen></iframe>
                    </div><!-- .span4 -->
                </div><!-- .row -->
                <div class="row fix">
                    <div class="span12" style="margin-bottom: 30px;">
                        <div id="soluciones"></div>
                    </div>
                    <div class="span12" style="margin-bottom: 30px;">
                        <div class="soluciones-bloques bloque-1">
                           <a href="profesionista.aspx"><img src="img/solucion-1.jpg"></a>
                           <p>Profesionista</p>
                        </div>
                        <div class="soluciones-bloques bloque-2">
                           <a href="arrendador.aspx"><img src="img/solucion-2.jpg"></a>
                           <p>Arrendador</p>
                        </div>
                        <div class="soluciones-bloques bloque-3">
                           <a href="pequeno-empresario.aspx"><img src="img/solucion-3.jpg"></a>
                           <p>Pequeño empresario</p>
                        </div>
                        <div class="soluciones-bloques bloque-4">
                           <a href="autoempleado.aspx"><img src="img/solucion-4.jpg"></a>
                           <p>Autoempleado</p>
                        </div>
                        <div class="soluciones-bloques bloque-5">
                           <a href="impresor.aspx"><img src="img/solucion-5.jpg"></a>
                           <p>Impresor</p>
                        </div>
                        <div class="soluciones-bloques bloque-6">
                           <a href="desarrollador-de-software.aspx"><img src="img/solucion-6.jpg"></a>
                           <p>Desarrollador Software</p>
                        </div>
                        <div class="soluciones-bloques bloque-7">
                           <a href="corporativo.aspx"><img src="img/solucion-7.jpg"></a>
                           <p>Corporativo</p>
                        </div>
                        <div class="soluciones-bloques bloque-8">
                           <a href="contador.aspx"><img src="img/solucion-8.jpg"></a>
                           <p>Contador</p>
                        </div>
                    </div>
                </div><!-- .row -->
                <div class="row">
                	<div class="span4">
                    	<a href="pasos-para-csd.aspx"><img src="img/fiel.png" alt="" id="fiel"/></a>
                    </div><!-- .span4 -->
                    <div class="span4">
                    	<a href="servicios-complementarios.aspx"><img src="img/estas-listo.jpg" alt=""/></a>
                    </div><!-- .span4 -->
                    <div class="span4">
                    	<div id="noticias">
                        	<h3>Modificaciones a la Resolución Miscelánea Fiscal para 2013.</h3>
                            <p>A partir de 2014, todos los contribuyentes que utilizan el esquema de Comprobante Fiscal Digital (CFD) deberán utilizar el esquema de Comprobante Fiscal Digital por Internet (CFDI) para la emisión de sus facturas electrónicas. <a href="#">+ Ver completa</a></p>
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

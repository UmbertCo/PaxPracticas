<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
      <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/corporativo.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Corporativo</span></p>
                        <div class="separador"></div>
                        <h1>Soluciones para Corporativos</h1>
                        <div class="row">
                        	<div class="span8 block-green">
                                <h4>Facturación electrónica para ti:</h4>
                                <div class="encabezado-contenido">
                                    <div class="icono-encabezado-contenido">
                                        <img src="img/corporativo-icon.gif" alt=""/>
                                    </div>
                                    <div class="texto-encabezado-contenido">
                                        <p>Si eres parte de una empresa grande y tu principal problema es contar con una plataforma de timbrado que maneje alto volumen de facturas por día de una manera sencilla y el más rápido del mercado, intégrate con PAX.<br/>
                                        Contamos con el método de integración adecuado para tu ERP o Software Administrativo, nos hemos integrado con las aplicaciones más frecuentes en las grandes empresas.
                                        </p>
                                    </div>
                                </div>
                                 <div style="clear:both;height:1px;"></div>
                                <ul>
                                    <li style="font:15px 'Helvetica85HeavyHeavy'; color:#006080; text-transform:uppercase;">Por mencionar algunas tenemos:</li>
                                </ul>
                                <img src="img/logos1.jpg" alt="" style="margin-left:42px;"/>
                                <p style="font-size:12px; color:#636363; margin-left:42px;">* Consulta nuestra lista completa de aplicaciones integradas.</p>
                                <ul>
                                    <li style="font:15px 'Helvetica85HeavyHeavy'; color:#006080; text-transform:uppercase;">Algunos de nuestros clientes satisfechos con esta integración son:</li>
                                </ul>
                                <img src="img/logos2.jpg" alt="" style="margin:0 0 20px 42px;"/>
                                <img src="img/logos3.jpg" alt="" style="margin:0 0 20px 42px;"/>
                                <img src="img/logos4.jpg" alt="" style="margin:0 0 20px 42px;"/>
                                <ul style="margin-bottom:45px;">
                                    <li>
                                        Nuestros métodos de integración pueden ayudar a hacer más fácil esta transición a CFDI.<br/>
                                        <span id="vineta-titulo">WEB Service TXT o XML</span><br/>
                                        Integración de servicios SOAP, recomendado para su ERP.<br/>
                                        <span id="vineta-titulo">Conector PAX TXT o XML</span><br/>
                                        Herramienta de procesamiento alterna para su ERP.
                                    </li>
                                    <li>Si necesitas <a href="#">soporte para implementar PAX</a> contáctanos o <a href="#">consulta nuestra base de conocimientos</a>.</li>
                                </ul>
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


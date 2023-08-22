<%@ Page Language="C#" AutoEventWireup="true" CodeFile="acerca-de.aspx.cs" Inherits="acerca_de" MasterPageFile="MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
     <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/acerca-de.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Acerca de PAX</span></p>
                        <div class="separador"></div>
                        <h1>Acerca de PAX</h1>
                        <div class="row">
                        	<div class="span8 block-green" style="position:relative;">
                                <h4>Sobre Nosotros</h4>
                                <h2>PAX Facturación es establecida por un grupo de 5 empresas de TI:</h2>
                                    <ol style="padding-bottom:4px; padding-left:72px;">
                                        <li>ADS Systems.</li>
                                        <li>AZUR Sistemas de Información.</li>
                                        <li>FDS Fraire Developer Solutions.</li>
                                        <li>ESN Enterprise Solution.</li>
                                        <li>Con la colaboración de inversionistas de Chihuahua.</li>
                                    </ol>
                                    <ul style="padding-bottom:8px;">
                                        <li>Juntos a inicios de 2011 fundan grupo CORPUS Facturación, con el objetivo de ofrecer  servicios de desarrollo de sistemas administrativos, renta de servidores en la nube y el proporcionar servicios de Facturación Electrónica a todas las personas físicas o morales que cuenten con la necesidad de emitir Comprobantes Fiscales.</li>
                                        <li>Nuestro Corporativo se encuentra en el Parque de Innovación y Transferencia Tecnológica, (PIT2) en la ciudad de Chihuahua. Actualmente contamos con sucursales en el Distrito Federal, Juárez y en Monterrey, así como con representantes y distribuidores en diferentes ciudades de la República Mexicana.</li>
                                        <li>La misión principal de PAX facturación es el timbrado, administración y control de todo comprobante Fiscal Digital por internet (CFDI) emitido y recibido por todo tipo de contribuyente, basado en un servicio personalizado, para el correcto cumplimiento de las normas fiscales actuales.</li>
                                        <li>En PAX estamos comprometidos en ofrecer servicios de Facturación electrónica, desarrollo de software a la medida, así como alojamiento de servidores en la nube, todo esto basado en las necesidades del cliente.</li>
                                    </ul>
                                <h4>Respaldo</h4>
                                <h2>PAX Facturación cuenta con el respaldo del Grupo CORPUS Facturación, el cual esta conformado por 4 empresas con mas de 20 años de experiencia en el mercado de TI.</h2>
                                <ul style="padding-bottom:16px;">
                                    <li>
                                        <span id="vineta-titulo">Infraestructura Tecnológica</span><br/>
                                        Estas empresas proveen el  además de sus experiencia , la infraestructura tecnológica de punta para ofrecer una ventaja competitiva en nuestros centros de datos, en donde se cuenta con un ancho de banda de 1000 Mbps de subida y bajada de datos, lo cual proporciona mejores tiempos de respuesta.
                                        Además contamos con infraestructura de punta, la cual proporciona seguridad en la información, así como estabilidad en el servicio, por medio de dos centros de datos certificados con altos niveles de seguridad.
                                    </li>
                                    <li>
                                        <span id="Span1">Disponibilidad de Servicio</span><br/>
                                        Es la misma infraestructura de seguridad y tecnología de punta de nuestro centro de datos la que nos permite ofrecer un 99.3% de disponibilidad de servicio.
                                    </li>
                                    <li>
                                        <span id="Span2">Personal Calificado</span><br/>
                                        SQL Server<br/>
                                        Visual Studio 2010 .Net<br/>
                                        Windows Server 2008<br/>
                                        Project Management Professional PMP<br/>
                                        ITIL
                                    </li>
                                </ul>
                                <img src="img/logos-acerca-de.png" alt="" style="position:absolute; top:1163px; left:429px;"/>
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
<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/guia-interactiva.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Guía Interactiva</span></p>
                        <div class="separador"></div>
                        <h1>Guía Interactiva</h1>
                        <div class="row">
                        	<div class="span8 block-green">
                                <img src="img/guia-1.png" alt="" style="margin-top:30px;" />
                                <ul id="accordion" class="accordion"><!-- Inicio del acordión -->
                                    <li class="accordion" style="list-style:none;">
                                        <h3>¿No la tienes? Aquí te decimos cómo</h3><!-- Handler -->
                                        <div class="panel"><!-- Primer panel del acordión -->
                                            <ul style="margin-left:-68px;">
                                                <li><span style="font-weight:bold; font-size:15px;">Personas Físicas</span><br/>
                                                    Solicitar una cita en el SAT para la obtención de la FIEL, y presentar la siguiente documentación:
                                                    <ol>
                                                        <li>Dispositivo magnético (usb o disco compacto).</li>
                                                        <li>Original o copia certificada de:</li>
                                                        <li>Acta de nacimiento o carta de naturalización (se deberá presentar solo en los casos en los cuales en el Módulo de Consulta de RENAPO no se cuente con una CURP certificada).Para verificar si cuenta con la CURP certificada consulte la siguiente liga: Modulo RENAPO.</li>
                                                        <li>Documento migratorio vigente (FM2 o FM3), para personas de origen extranjero.</li>
                                                        <li>Identificación oficial vigente.</li>
                                                    </ol>
                                                </li>
                                                <li><span style="font-weight:bold; font-size:15px;">Personas Morales</span><br/>
                                                    Solicitar una cita en el SAT para la obtención de la FIEL, y presentar la siguiente documentación:
                                                    <ol>
                                                        <li>El representante legal deberá contar con el certificado de Firma Electrónica Avanzada "Fiel" vigente, como persona física.</li>
                                                        <li>Llevar el día de su cita lo siguiente:</li>
                                                        <li>Dispositivo magnético (usb o disco compacto).</li>
                                                        <li>Copia certificada de los siguientes documentos:</li>
                                                        <li>Poder general del representante legal para actos de dominio o de administración.(El poder no podrá ser mancomunado ni de carácter especial.)</li>
                                                        <li>Acta constitutiva de la persona moral solicitante.</li>
                                                    </ol>
                                                    Original o copia certificada de Identificación oficial del representante legal de la persona moral solicitante (credencial para votar, pasaporte, cédula profesional, cartilla del servicio militar ó credencial emitida por los gobiernos federal, estatal o municipal que cuente con la fotografía y firma.
                                                </li>
                                            </ul>
                                        </div><!-- /Fin del primer panel del acordión -->
                                    </li>
                                </ul><!-- /Fin del acordión -->
                                <img src="img/guia-2.png" alt=""/>
                                <ul id="accordion-2" class="accordion-2"><!-- Inicio del acordión -->
                                    <li class="accordion-2" style="list-style:none;">
                                        <h3>Consulta los pasos para obtener tus sellos digitales</h3><!-- Handler -->
                                        <div class="panel"><!-- Segundo panel del acordión -->
                                            <ul style="margin-left:-68px;">
                                                <li><span style="font-weight:bold; font-size:15px;">Requisitos</span><br/>
                                                    <ol>
                                                        <li>Para obtener estos CSD es necesario tener la FIEL Vigente.</li>
                                                        <li>Contar con la aplicación del SAT, SOLCEDI (Solicitud de Certificado Digital).</li>
                                                    </ol>
                                                </li>
                                                <li><span style="font-weight:bold; font-size:15px;">Los pasos para obtener el CSD, son los siguientes:</span><br/>
                                                    <div id="contenedor-pasos">
                                                        <div id="guia-paso-1">
                                                            <p class="numero">1</p>
                                                            <p style="margin:0;">Descargar<br/>
                                                               aplicación<br/>
                                                               <strong>SOLCEDI</strong>
                                                            </p>
                                                        </div>
                                                        <div id="guia-paso-2">
                                                            <p class="numero">2</p>
                                                            <p style="margin:0;">Proporcionar<br/>
                                                               archivo de la <strong>FIEL</strong>
                                                            </p>
                                                        </div>
                                                        <div id="guia-paso-3">
                                                            <p class="numero">3</p>
                                                            <p style="margin:0;">Obtener archivos<br/>
                                                               del sello digital
                                                            </p>
                                                        </div>
                                                        <!--
                                                        <div id="guia-paso-4">
                                                            <p class="numero">4</p>
                                                            <p style="margin:0;">Ensobretado de<br/>
                                                               archivos de sellos<br/>
                                                               digitales
                                                            </p>
                                                        </div>
                                                        -->
                                                        <div id="guia-paso-5">
                                                            <p class="numero">4</p>
                                                            <p style="margin:0;">Envío de los<br/> 
                                                               Archivos de<br/>
                                                               certificado al <strong>SAT</strong>
                                                            </p>
                                                        </div>
                                                        <a href="pasos-csd.aspx"><div id="link-a-pasos"></div></a>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div><!-- /Fin del segundo panel del acordión -->
                                    </li>
                                </ul><!-- /Fin del acordión -->
                                <img src="img/guia-3.png" alt="" style="margin-top:10px;"/>
                                <ul>
                                    <li>Solo tienes que capturar tus datos fiscales y tu correo electrónico.<br/>
                                        <a href="registro.aspx"><img src="img/registrate-aqui.png" alt="" style="margin:20px 0 14px -18px;"/></a>
                                    </li>
                                </ul>
                                <img src="img/guia-4.png" alt=""/>
                                <ul>
                                    <li>Adquiere el número de timbres de acuerdo al número de comprobantes fiscales que generes en tu periodo, así podrás controlar tus costos de timbres y siempre tener disponibles para poder hacer las facturas para tus clientes.<br/>
                                        Espera una confirmación de tu compra para poder acceder al sistema de facturación.
                                    </li>
                                </ul>
                                <img src="img/guia-5.png" alt=""/>
                                <ul style="margin-bottom:70px;">
                                    <li>Solo entra al sistema y configura tus datos para poder empezar a facturar.</li>
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


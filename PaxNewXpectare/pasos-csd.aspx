<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
        <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/5pasos.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Pasos para obtener el CSD</span></p>
                        <div class="separador"></div>
                        <h1>Pasos para obtener el CSD</h1>
                        <div class="row">
                        	<div class="span8 block-green" id="pasos-csd">
                                <h2 id="text-sup-pasos" style="margin:25px 0 0 30px; padding-top:2px; font-size:16px; text-transform:uppercase;">Sigue los siguientes pasos y obtén tu CSD</h2>
                                <div id="paso-1">
                                    <img src="img/paso-1.png" alt="" usemap="#Map" style="margin-top:30px;"/>
                                    <map name="Map" id="Map">
                                        <area shape="rect" coords="-5,-15,121,149" href="#" alt="paso-1" class="botonPaso" />
                                        <area shape="rect" coords="120,-2,377,145" href="#" alt="paso-2" class="botonPaso" />
                                        <area shape="rect" coords="376,-1,505,117" href="#" alt="paso-3" class="botonPaso" />
                                        <area shape="rect" coords="504,-1,632,132" href="#" alt="paso-4" class="botonPaso" />
                                        <area shape="rect" coords="631,-2,820,142" href="#" alt="paso-5" class="botonPaso" />
                                    </map>
                                    <img src="img/paso-1-a.png" alt="" style="margin-top:30px;"/>
                                    <p id="titulo-pasos">Proporcionar archivo de la FIEL y obtención de archivos de sello digital</p>
                                    <ul style="margin-bottom:50px;">
                                        <li>Se selecciona la opción de <span id="vineta-titulo">“Solicitud de Certificados de Sello Digital (CSD)”</span>.<br/>
                                            Al dar clic mostrará la pantalla donde deberá pulsar el botón de <span id="Span1">“Examinar”</span> para obtener su certificado de FIEL vigente.<br/>
                                            <img src="img/paso1.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                        <li>Si elige un certificado que no se encuentra vigente se mostrará el siguiente mensaje:
                                            <img src="img/paso1a.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                        <li>Por lo que deberá volver a pulsar el botón “Examinar” y una vez que seleccionó el certificado vigente mostrará una nueva pantalla.</li>
                                    </ul>
                                    <div width="100%;">
                                        <div id="boton-siguiente"><a href="#" alt="paso-2" class="botonPaso toTop"></a></div>
                                    </div>
                                </div>
                                <div id="paso-2">
                                    <img src="img/paso-2.png" alt="" usemap="#Map-2" style="margin-top:30px;"/>
                                    <map name="Map-2" id="Map-2">
                                        <area shape="rect" coords="-5,-15,121,149" href="#" alt="paso-1" class="botonPaso" />
                                        <area shape="rect" coords="120,-2,377,145" href="#" alt="paso-2" class="botonPaso" />
                                        <area shape="rect" coords="376,-1,505,117" href="#" alt="paso-3" class="botonPaso" />
                                        <area shape="rect" coords="504,-1,632,132" href="#" alt="paso-4" class="botonPaso" />
                                        <area shape="rect" coords="631,-2,820,142" href="#" alt="paso-5" class="botonPaso" />
                                    </map>
                                    <img src="img/paso-2-a.png" alt="" style="margin-top:30px;" id="paso-2-top"/>
                                    <p id="P1">Inicie la captura de pasos</p>
                                    <ul>
                                        <li>Capture los siguientes campos:
                                            <ol>
                                                <li>Nombre de la Sucursal o Unidad (máximo 64 caracteres).</li>
                                                <li>Contraseña de la Clave Privada (mínimo 8 caracteres y máximo 256 caracteres).</li>
                                                <li>Confirmación de la Contraseña (la que capturó en el campo anterior).</li>
                                            </ol>
                                            <img src="img/paso2-a.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <p id="P2">Nombre de la sucursal o unidad</p>
                                    <ul>
                                        <li>En caso de tener caracteres no válidos para el nombre, mostrará el siguiente mensaje. Al dar aceptar, la aplicación por funcionalidad no permite avanzar al siguiente campo hasta que el mismo cumpla con las especificaciones.
                                            <img src="img/paso2-b.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <p id="P3">Contraseña de la clave privada</p>
                                    <ul>
                                        <li>En caso de tener caracteres no válidos para el nombre, mostrará el siguiente mensaje. Al dar aceptar, la aplicación por funcionalidad no permite avanzar al siguiente campo hasta que el mismo cumpla con las especificaciones.
                                            <img src="img/paso2-c.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <p id="P4">Confirmación de la contraseña</p>
                                    <ul>
                                        <li>Confirmar la contraseña de la “Clave Privada” la cual debe ser idéntica a la capturada en el campo anterior.<br/>
                                            Si las contraseñas proporcionadas no son iguales, el sistema mostrará el siguiente mensaje; Una vez que tanto la contraseña y la confirmación coincidan, habilitará el botón de “Agregar”.
                                            <img src="img/paso2-d.jpg" alt="" style="margin-top:20px;"/>
                                            <img src="img/paso2-e.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <ul>
                                        <li>Pulsar el botón agregar y mostrará en el recuadro “Solicitudes realizadas”.<br/>
                                            <span style="color:#003e53"><strong>Nota.</strong> Cabe señalar que puede generar hasta 30 solicitudes.</span>
                                            <img src="img/paso2-f.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <ul>
                                        <li>La aplicación cuenta con la opción para eliminar o modificación los datos de una solicitud. Sólo seleccione la sucursal, dé clic derecho y elija lo que requiera hacer:
                                            <img src="img/paso2-g.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <ul>
                                        <li>La opción de modificar, solo puede modificar el nombre de la <span id="Span2">“Sucursal”</span>.
                                            <img src="img/paso2-h.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <ul>
                                        <li>En la opción de eliminar, se borra la solicitud elegida.
                                            <img src="img/paso2-i.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <ul>
                                        <li>Y puede volver a capturar los datos.<br/>
                                            <img src="img/paso2-j.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <ul style="margin-bottom:50px;">
                                        <li>Una vez generada las solicitudes necesarias, pulsar el botón <span id="Span3">“Siguiente”</span>.
                                            <img src="img/paso2-k.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <div width="100%;">
                                        <div id="Div1"><a href="#" alt="paso-3" class="botonPaso toTop"></a></div>
                                    </div>
                                </div>
                                <div id="paso-3">
                                    <img src="img/paso-3.png" alt="" usemap="#Map-3" style="margin-top:30px;"/>
                                    <map name="Map-3" id="Map-3">
                                        <area shape="rect" coords="-5,-15,121,149" href="#" alt="paso-1" class="botonPaso" />
                                        <area shape="rect" coords="120,-2,377,145" href="#" alt="paso-2" class="botonPaso" />
                                        <area shape="rect" coords="376,-1,505,117" href="#" alt="paso-3" class="botonPaso" />
                                        <area shape="rect" coords="504,-1,632,132" href="#" alt="paso-4" class="botonPaso" />
                                        <area shape="rect" coords="631,-2,820,142" href="#" alt="paso-5" class="botonPaso" />
                                    </map>
                                    <img src="img/paso-3-a.png" alt="" style="margin-top:30px;"/>
                                    <ul>
                                        <li>Mostrará la ventana de <span id="Span4">Generación de Claves</span>, es preciso que mueva el ratón hasta que la barra llegue al 100%.
                                            <img src="img/paso3-a.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <ul style="margin-bottom:50px;">
                                        <li>Una vez que el proceso termine, deberá proporcionar la clave privada de su certificado de FIEL vigente como se muestra a continuación.</li>
                                    </ul>
                                    <div width="100%;">
                                        <div id="Div2"><a href="#" alt="paso-4" class="botonPaso toTop"></a></div>
                                    </div>
                                </div>
                                <div id="paso-4">
                                    <img src="img/paso-4.png" alt="" usemap="#Map-4" style="margin-top:30px;"/>
                                    <map name="Map-4" id="Map-4">
                                        <area shape="rect" coords="-5,-15,121,149" href="#" alt="paso-1" class="botonPaso" />
                                        <area shape="rect" coords="120,-2,377,145" href="#" alt="paso-2" class="botonPaso" />
                                        <area shape="rect" coords="376,-1,505,117" href="#" alt="paso-3" class="botonPaso" />
                                        <area shape="rect" coords="504,-1,632,132" href="#" alt="paso-4" class="botonPaso" />
                                        <area shape="rect" coords="631,-2,820,142" href="#" alt="paso-5" class="botonPaso" />
                                    </map>
                                    <img src="img/paso-4-a.png" alt="" style="margin-top:30px;"/>
                                    <p id="P5">Proporcione la clave privada de su certificado de FIEL</p>
                                    <ul>
                                        <li>Cuando el anterior proceso termine, deberá proporcionar la clave privada de su certificado de FIEL vigente como se muestra a continuación.<br/>
                                            <img src="img/paso4-a.jpg" alt="" style="margin-top:20px;"/>
                                            <img src="img/paso4-b.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <ul style="margin-bottom:50px;">
                                        <li>Asimismo, capturar la contraseña de la clave privada.<br/>
                                            <img src="img/paso4-c.jpg" alt="" style="margin-top:20px;"/>
                                            <img src="img/paso4-d.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <div width="100%;">
                                        <div id="Div3"><a href="#" alt="paso-5" class="botonPaso toTop"></a></div>
                                    </div>
                                </div>
                                <div id="paso-5">
                                    <img src="img/paso-5.png" alt="" usemap="#Map-5" style="margin-top:30px;"/>
                                    <map name="Map-5" id="Map-5">
                                        <area shape="rect" coords="-5,-15,121,149" href="#" alt="paso-1" class="botonPaso" />
                                        <area shape="rect" coords="120,-2,377,145" href="#" alt="paso-2" class="botonPaso" />
                                        <area shape="rect" coords="376,-1,505,117" href="#" alt="paso-3" class="botonPaso" />
                                        <area shape="rect" coords="504,-1,632,132" href="#" alt="paso-4" class="botonPaso" />
                                        <area shape="rect" coords="631,-2,820,142" href="#" alt="paso-5" class="botonPaso" />
                                    </map>
                                    <img src="img/paso-5-a.png" alt="" style="margin-top:30px;"/>
                                    <ul>
                                        <li>Al indicar donde se guardará, termina el proceso mostrando la siguiente pantalla. En la parte inferior, se señala la ruta completa en donde quedaron almacenados los archivos.<br/>
                                            <img src="img/paso5-a.jpg" alt="" style="margin-top:20px;"/>
                                        </li>
                                    </ul>
                                    <ul style="margin-bottom:80px;">
                                        <li>Al pulsar el botón <span id="Span5">“Terminar”</span> regresa a la pantalla inicial.</li>
                                    </ul>
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


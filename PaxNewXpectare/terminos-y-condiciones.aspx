<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
             
        <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/terminos-y-condiciones.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Términos y Condiciones</span></p>
                        <div class="separador"></div>
                        <h1>Términos y Condiciones</h1>
                        <div class="row">
                        	<div class="span8 block-green">
                                <h4>Condiciones de Uso</h4>
                                <ul>
                                    <li>Tómese un tiempo para revisar las Condiciones de Uso, porque con la sola utilización del Portal se compromete
                                        sin reservas a cumplirlas.<br/>
                                        PAX se reserva el derecho de modificar sin previo aviso éstas Condiciones de Uso, así como el contrato que estas
                                        suponen, mediante la sola actualización de este texto.<br/>
                                        Todo el material existente en el sitio, que no corresponda a un tercero, constituye propiedad intelectual y exclusiva de PAX.
                                        A título meramente enunciativo, se entenderán incluidos el texto, software, nombres, logotipos, marcas comerciales,
                                        marcas registradas, nombres comerciales, imágenes, fotografías, dibujos, música y videos.<br/>
                                        PAX se reserva todos los derechos sobre el mencionado material, no cede ni transfiere a favor del usuario ningún derecho sobre<br/>
                                        su propiedad intelectual o la de terceros. En consecuencia, el usuario puede descargar el contenido únicamente para su
                                        uso personal, nunca para fines comerciales, siempre y cuando se conserven todos los mensajes de copyright, propiedad
                                        intelectual y marcas comerciales, no se realicen modificaciones a los mismos, no se haga uso de los contenidos de forma
                                        que aparente estar vinculado con alguno de nuestros productos, servicios, eventos o marcas, y no se almacene los contenidos<br/>
                                        en una base de datos, servidor, u ordenador personal para su uso con fines comerciales.<br/>
                                        No obstante, usted no está autorizado, bajo ningún concepto, a copiar, reproducir, publicar, tanto físicamente como
                                        electrónicamente, transmitir o distribuir los contenidos en línea, a no ser que haya solicitado y obtenido, por escrito,
                                        nuestra autorización expresa. Tampoco podrá añadir, eliminar, alterar o tergiversar cualquier contenido de la Web de PAX.
                                        Se prohíbe expresamente cualquier intento de modificar cualquier material en línea, o de forzar o evadir nuestras medidas
                                        de seguridad. El usuario se obliga a usar los contenidos de forma correcta y lícita y no está autorizado a revender o vender
                                        el material ni a utilizar técnicas de ingeniería inversa, descompilar o desensamblar el software, o convertirlo a cualquier
                                        otro formato que permitiera su uso por parte de terceros.
                                    </li>
                                </ul>
                                <h4>Límite de responsabilidad</h4>
                                <ul>
                                    <li>PAX excluye cualquier responsabilidad por los daños y perjuicios de toda naturaleza que puedan deberse a la transmisión,
                                        difusión, almacenamiento, puesta a disposición, recepción, obtención o acceso a los contenidos, y en particular,
                                        aunque no de modo exclusivo, por los daños y perjuicios que puedan deberse a (a) el incumplimiento de la ley, la
                                        moral y las buenas costumbres generalmente aceptadas o el orden público como consecuencia de la transmisión, difusión,
                                        almacenamiento, puesta a disposición, recepción, obtención o acceso a los contenidos;<br/>
                                        (b) la infracción de los derechos de propiedad intelectual e industrial, de compromisos contractuales de cualquier
                                        clase, de los derechos al honor, a la intimidad personal y familiar y a la imagen de las personas, de los derechos
                                        de propiedad y de toda otra naturaleza pertenecientes a un tercero como consecuencia de la transmisión, difusión,
                                        almacenamiento, puesta a disposición, recepción, obtención o acceso a los contenidos; (c) la realización de actos
                                        de competencia desleal y publicidad ilícita como consecuencia de la transmisión, difusión, almacenamiento, puesta
                                        a disposición, recepción, obtención o acceso a los contenidos; (d) la falta de veracidad, exactitud, exhaustividad,
                                        pertinencia y/o actualidad de los contenidos; (e) la inadecuación para cualquier clase de propósito de y la defraudación
                                        de las expectativas generadas por los contenidos; f) los vicios y defectos de toda clase de los contenidos transmitidos,
                                        difundidos, almacenados, puestos a disposición o de otra forma transmitidos o puestos a disposición, recibidos, obtenidos
                                        o a los que se haya accedido a través del portal o de los servicios.
                                    </li>
                                </ul>
                                <h4>Extinción del presente Contrato</h4>
                                <ul style="margin-bottom:60px;">
                                    <li>PAX puede dar por finalizado el presente contrato en cualquier momento, sin necesidad de previo aviso, en el caso de que,
                                        a su propio juicio, usted haya violado alguna de las condiciones del mismo.
                                    </li>
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


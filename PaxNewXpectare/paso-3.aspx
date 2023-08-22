<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
       <section id="main" style="background-color:#f0f0ee;">
        	<div style="width:770px;">
                <div id="paso-1">
                    <img src="img/p3-titulo.png" alt="" style="margin-top:30px;"/>
                    <ul style="margin-bottom:50px;">
                        <li>Ingresa a la pagina del SAT: <a href="http://www.sat.gob.mx/sitio_internet/home.asp" target="_blank">http://www.sat.gob.mx/sitio_internet/home.asp</a> y da clic en <span id="vineta-titulo">"Tramites y Servicios"</span>.<br/>
                            <img src="img/lightbox2-1.png" alt="" style="margin-top:20px;"/>
                        </li>
                        <li>Da clic en <span id="Span1">"Solicitud de Certificados de Sello Digital"</span>.<br/>
                            Puedes autentificarte con la FIEL o con tu RFC y tu contraseña.<br/>
                            <img src="img/lightbox2-2.png" alt="" style="margin-top:20px;"/>
                        </li>
                        <li>Click en <span id="Span2">"Envio de solicitud de certificados de Sello Digital"</span>.<br/>
                            <img src="img/lightbox2-3.png" alt="" style="margin-top:20px;"/>
                        </li>
                        <li>Selecciona tu archivo .sdg a través del botón "Examinar".<br/>
                            <img src="img/lightbox2-4.png" alt="" style="margin-top:20px;"/>
                        </li>
                        <li>Una vez seleccionado tu archivo .sdg click al botón <span id="Span3">"Enviar requerimiento"</span>.<br/>
                            <img src="img/lightbox2-5.png" alt="" style="margin-top:20px;"/>
                        </li>
                        <li>Una vez que se envío el archivo .sdg y fue aceptado por el SAT, descarga tu certificado dando click a <span id="Span4">"Recuperación de certificados"</span>, escriba el RFC y clic en el botón <span id="Span5">"Buscar"</span>.<br/>
                            <img src="img/lightbox2-6.png" alt="" style="margin-top:20px;"/>
                        </li>
                        <li>Se listarán los diferentes certificados con que cuenta el RFC.<br/>
                            <img src="img/lightbox2-7.png" alt="" style="margin-top:20px;"/>
                        </li>
                        <li>El certificado que nos interesa es el de Tipo: <span id="Span6">SELLOS</span>. Da clic al número de serie y guarda el certificado.<br/>
                            <img src="img/lightbox2-8.png" alt="" style="margin-top:20px;"/>
                        </li>
                    </ul>
                </div>
                <!-- <a href="#"><img src="img/nuestros-servicios.png" alt="" id="nuestros-servicios"/></a> -->
            </div>
        </section>
</asp:Content>


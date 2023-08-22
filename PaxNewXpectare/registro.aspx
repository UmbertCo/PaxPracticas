<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
       <section id="banner">
        	<div class="container">
            	<div class="row">
					<div class="span12">
                    	<img src="img/registro-datos.jpg" alt=""/>
                    </div><!-- .span12 -->
                </div><!-- .row -->
            </div><!-- .container -->
        </section>
        
        <section id="main">
        	<div class="container">
            	<div class="row fix">
                	<div class="span12">
                    	<p class="breadcrumb"><a href="/">Home</a> / <span>Regístrate con PAX</span></p>
                        <div class="separador"></div>
                        <h1>Registrate con PAX</h1>
                        <div class="row">
                        	<div class="span8 block-green">
                                <div class="registro" style="padding-bottom:10px;">
                                    <h4 style="width:100%;">Datos de Compra</h4>
                                    <form id="registroForm" action="paquetes.aspx" method="get"><!-- Inicio de la forma de Registro -->
                                        <fieldset>
                                            <label for="nombre-registro">Nombre o Razón Social</label>
                                            <input type="text" id="nombre-registro" name="nombre-registro" placeholder="" class="required"/>
                                            <label for="rfc">RFC</label>
                                            <input type="text" id="rfc" name="rfc" placeholder="" class="required"/>
                                            <label for="sucursal">Nombre de la Sucursal</label>
                                            <input type="text" id="sucursal" name="sucursal" placeholder="" class="required"/>
                                            <label for="calle">Calle</label>
                                            <input type="text" id="calle" name="calle" placeholder="" class="required"/>
                                            <div style="width:100%;">
                                                <div style="float:left; width:50%;">
                                                    <label for="no-ext">No. Ext</label>
                                                    <input type="number" id="no-ext" name="no-ext" placeholder="" class="required"/>
                                                </div>
                                                <div style="float:right; width:50%;">
                                                    <label for="no-int" style="padding-left:15px;">No. Int</label>
                                                    <input style="margin-left:15px;" type="number" id="no-int" name="no-int" placeholder="" class="required"/>
                                                </div>
                                            </div>
                                            <label for="colonia">Colonia</label>
                                            <input type="text" id="colonia" name="colonia" placeholder="" class="required"/>
                                            <label for="co-pos">Código Postal</label>
                                            <input type="text" id="co-pos" name="co-pos" placeholder="" class="required"/>
                                        </fieldset>
                                    <!-- </form> -->
                                </div>
                                <div class="registro-derecha">
                                    <!-- <form id="registroForm-2"> -->
                                        <fieldset>
                                            <label for="reg-fiscal">Régimen Fiscal</label>
                                            <input type="text" id="reg-fiscal" name="reg-fiscal" placeholder="" class="required"/>
                                            <label for="telefono">Teléfono</label>
                                            <input type="tel" id="telefono" name="telefono" placeholder="" class="required"/>
                                            <label for="delegacion">Delegación o Municipio</label>
                                            <input type="text" id="delegacion" name="delegacion" placeholder="" class="required"/>
                                            <label for="localidad">Localidad</label>
                                            <input type="text" id="localidad" name="localidad" placeholder="" class="required"/>
                                            <label for="estado">Estado</label>
                                            <select name="estado" id="estado" style="margin-bottom:20px;">
                                                <option value="Aguascalientes">Aguascalientes</option>
                                                <option value="Baja California">Baja California</option>
                                                <option value="Baja California Sur">Baja California Sur</option>
                                                <option value="Campeche">Campeche</option>
                                                <option value="Chiapas">Chiapas</option>
                                                <option value="Chihuahua">Chihuahua</option>
                                                <option value="Coahuila">Coahuila</option>
                                                <option value="Colima">Colima</option>
                                                <option value="Distrito Federal">Distrito Federal</option>
                                                <option value="Durango">Durango</option>
                                                <option value="Estado de México">Estado de México</option>
                                                <option value="Guanajuato">Guanajuato</option>
                                                <option value="Guerrero">Guerrero</option>
                                                <option value="Hidalgo">Hidalgo</option>
                                                <option value="Jalisco">Jalisco</option>
                                                <option value="Michoacán">Michoacán</option>
                                                <option value="Morelos">Morelos</option>
                                                <option value="Nayarit">Nayarit</option>
                                                <option value="Nuevo León">Nuevo León</option>
                                                <option value="Oaxaca">Oaxaca</option>
                                                <option value="Puebla">Puebla</option>
                                                <option value="Querétaro">Querétaro</option>
                                                <option value="Quintana Roo">Quintana Roo</option>
                                                <option value="San Luis Potosí">San Luis Potosí</option>
                                                <option value="Sinaloa">Sinaloa</option>
                                                <option value="Sonora">Sonora</option>
                                                <option value="Tabasco">Tabasco</option>
                                                <option value="Tamaulipas">Tamaulipas</option>
                                                <option value="Tlaxcala">Tlaxcala</option>
                                                <option value="Veracruz">Veracruz</option>
                                                <option value="Yucatán">Yucatán</option>
                                                <option value="Zacatecas">Zacatecas</option>
                                            </select>
                                            <label for="pais">País</label>
                                            <select name="pais" id="pais">
                                                <option value="México">México</option>
                                            </select>
                                        </fieldset>
                                    <!-- </form> -->
                                </div>
                                <div class="checkbox-dt">
                                    <!-- <form> -->
                                        <input type="checkbox" name="dt" id="dt" value=""/>
                                        <label for="dt"><span></span>Sus datos de compra son diferentes a sus datos fiscales.</label>
                                    <!-- </form> -->
                                </div>
                                <div class="datos-fiscales">
                                    <div class="registro">
                                        <h4 style="width:100%;">Datos Fiscales</h4>
                                        <!-- <form id="registroForm-3"> -->
                                            <fieldset>
                                                <label for="nombre-registro-2">Nombre, Sucursal o Matríz</label>
                                                <input type="text" id="nombre-registro-2" name="nombre-registro-2" placeholder="" class="required"/>
                                                <label for="rfc-2">RFC</label>
                                                <input type="text" id="rfc-2" name="rfc-2" placeholder="" class="required"/>
                                                <label for="razon-social-2">Razón Social</label>
                                                <input type="text" id="razon-social-2" name="razon-social-2" placeholder="" class="required"/>
                                                <label for="calle-2">Calle</label>
                                                <input type="text" id="calle-2" name="calle-2" placeholder="" class="required"/>
                                                <div style="width:100%;">
                                                    <div style="float:left; width:50%;">
                                                        <label for="no-ext-2">No. Ext</label>
                                                        <input type="number" id="no-ext-2" name="no-ext-2" placeholder="" class="required"/>
                                                    </div>
                                                    <div style="float:right; width:50%;">
                                                        <label for="no-int-2" style="padding-left:15px;">No. Int</label>
                                                        <input style="margin-left:15px;" type="number" id="no-int-2" name="no-int-2" placeholder="" class="required"/>
                                                    </div>
                                                </div>
                                                <label for="colonia-2">Colonia</label>
                                                <input type="text" id="colonia-2" name="colonia-2" placeholder="" class="required"/>
                                                <label for="co-pos-2">Código Postal</label>
                                                <input type="text" id="co-pos-2" name="co-pos-2" placeholder="" class="required"/>
                                            </fieldset>
                                        <!-- </form> -->
                                    </div>
                                    <div class="registro-derecha">
                                        <!-- <form id="registroForm-4"> -->
                                            <fieldset>
                                                <label for="telefono-2">Teléfono</label>
                                                <input type="tel" id="telefono-2" name="telefono-2" placeholder="" class="required"/>
                                                <label for="delegacion-2">Delegación o Municipio</label>
                                                <input type="text" id="delegacion-2" name="delegacion-2" placeholder="" class="required"/>
                                                <label for="localidad-2">Localidad</label>
                                                <input type="text" id="localidad-2" name="localidad-2" placeholder="" class="required"/>
                                                <label for="estado-2">Estado</label>
                                                <select name="estado-2" id="estado-2" style="margin-bottom:20px;">
                                                    <option value="Aguascalientes">Aguascalientes</option>
                                                    <option value="Baja California">Baja California</option>
                                                    <option value="Baja California Sur">Baja California Sur</option>
                                                    <option value="Campeche">Campeche</option>
                                                    <option value="Chiapas">Chiapas</option>
                                                    <option value="Chihuahua">Chihuahua</option>
                                                    <option value="Coahuila">Coahuila</option>
                                                    <option value="Colima">Colima</option>
                                                    <option value="Distrito Federal">Distrito Federal</option>
                                                    <option value="Durango">Durango</option>
                                                    <option value="Estado de México">Estado de México</option>
                                                    <option value="Guanajuato">Guanajuato</option>
                                                    <option value="Guerrero">Guerrero</option>
                                                    <option value="Hidalgo">Hidalgo</option>
                                                    <option value="Jalisco">Jalisco</option>
                                                    <option value="Michoacán">Michoacán</option>
                                                    <option value="Morelos">Morelos</option>
                                                    <option value="Nayarit">Nayarit</option>
                                                    <option value="Nuevo León">Nuevo León</option>
                                                    <option value="Oaxaca">Oaxaca</option>
                                                    <option value="Puebla">Puebla</option>
                                                    <option value="Querétaro">Querétaro</option>
                                                    <option value="Quintana Roo">Quintana Roo</option>
                                                    <option value="San Luis Potosí">San Luis Potosí</option>
                                                    <option value="Sinaloa">Sinaloa</option>
                                                    <option value="Sonora">Sonora</option>
                                                    <option value="Tabasco">Tabasco</option>
                                                    <option value="Tamaulipas">Tamaulipas</option>
                                                    <option value="Tlaxcala">Tlaxcala</option>
                                                    <option value="Veracruz">Veracruz</option>
                                                    <option value="Yucatán">Yucatán</option>
                                                    <option value="Zacatecas">Zacatecas</option>
                                                </select>
                                                <label for="pais-2">País</label>
                                                <select name="pais-2" id="pais-2">
                                                    <option value="México">México</option>
                                                </select>
                                            </fieldset>
                                        <!-- </form> -->
                                    </div>
                                </div>
                                <div class="registro">
                                    <h4 style="width:100%;">Datos de Registro</h4>
                                    <!-- <form id="registroForm-5"> -->
                                        <fieldset>
                                            <label for="email-registro">Correo</label>
                                            <input type="email" id="email-registro" name="correo" placeholder="" class="required"/>
                                            <label for="usuario">Usuario</label>
                                            <input type="text" id="usuario" name="usuario" placeholder="" class="required"/>
                                        </fieldset>
                                    <!-- </form> -->
                                </div>
                                <div class="registro-derecha">
                                    <img src="img/codigo-distribuidor.png" alt=""/>
                                    <div class="codigo-de-referencia">
                                        <!-- <form id="registroForm-6"> -->
                                            <fieldset>
                                                <label for="codigo" style="padding:25px 0 0 20px;">Código de Referencia</label>
                                                <input style="margin-left:20px;" type="text" id="codigo" name="codigo" placeholder="" class="required"/>
                                            </fieldset>
                                        <!-- </form> -->
                                    </div>
                                    <img src="img/exito.png" alt="" id="exito"/>
                                </div>
                                <div style="width:100%; float:left;">
                                    <div class="botones-enviar">
                                        <!-- <form> -->
                                            <!-- <img src="img/exito.png" alt="" id="Img1"/> -->
                                            <input type="reset" placeholder="" id="botonborrar-2" value=""/>
                                            <input type="submit" placeholder="" id="botonenviar-2" value=""/>
                                        </form><!-- /Fin de la Forma de Registro -->
                                    </div>
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


﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaxConectorConEsquema.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ConectorPAXMYERS")]
        public string Origen {
            get {
                return ((string)(this["Origen"]));
            }
            set {
                this["Origen"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Factura")]
        public string tipodocto {
            get {
                return ((string)(this["tipodocto"]));
            }
            set {
                this["tipodocto"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.paxfacturacion.com/")]
        public string urlHostComercial {
            get {
                return ((string)(this["urlHostComercial"]));
            }
            set {
                this["urlHostComercial"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("GT")]
        public string TipoServicio {
            get {
                return ((string)(this["TipoServicio"]));
            }
            set {
                this["TipoServicio"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<xsl:stylesheet version=\"2.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" xm" +
            "lns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:fn=\"http://www.w3.org/2005/xpath" +
            "-functions\" xmlns:cfdi=\"http://www.sat.gob.mx/cfd/3\" xmlns:ecc=\"http://www.sat.g" +
            "ob.mx/ecc\" xmlns:psgecfd=\"http://www.sat.gob.mx/psgecfd\" xmlns:divisas=\"http://w" +
            "ww.sat.gob.mx/divisas\" xmlns:detallista=\"http://www.sat.gob.mx/detallista\" xmlns" +
            ":ecb=\"http://www.sat.gob.mx/ecb\" xmlns:implocal=\"http://www.sat.gob.mx/implocal\"" +
            " xmlns:terceros=\"http://www.sat.gob.mx/terceros\" xmlns:iedu=\"http://www.sat.gob." +
            "mx/iedu\" xmlns:ventavehiculos=\"http://www.sat.gob.mx/ventavehiculos\" xmlns:pfic=" +
            "\"http://www.sat.gob.mx/pfic\" xmlns:tpe=\"http://www.sat.gob.mx/TuristaPasajeroExt" +
            "ranjero\" xmlns:leyendasFisc=\"http://www.sat.gob.mx/leyendasFiscales\">   <!-- Con" +
            " el siguiente método se establece que la salida deberá ser en texto -->   <!-- <" +
            "xsl:output method=\"text\" version=\"1.0\" encoding=\"UTF-8\" indent=\"no\"/> -->   <xsl" +
            ":output method=\"text\" version=\"1.0\" encoding=\"UTF-8\" indent=\"no\"/>   <!--    En " +
            "esta sección se define la inclusión de las plantillas de utilerías para colapsar" +
            " espacios   -->   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\Transformacion3_2\\u" +
            "tilerias.xslt\"/>   <!--     En esta sección se define la inclusión de las demás " +
            "plantillas de transformación para     la generación de las cadenas originales de" +
            " los complementos fiscales    -->   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\T" +
            "ransformacion3_2\\ecc.xslt\"/>   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\Transf" +
            "ormacion3_2\\psgecfd.xslt\"/>   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\Transfo" +
            "rmacion3_2\\donat11.xslt\"/>   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\Transfor" +
            "macion3_2\\divisas.xslt\"/>   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\Transform" +
            "acion3_2\\ecb.xslt\"/>   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\Transformacion" +
            "3_2\\detallista.xslt\"/>   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\Transformaci" +
            "on3_2\\implocal.xslt\"/>   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\Transformaci" +
            "on3_2\\terceros11.xslt\"/>   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\Transforma" +
            "cion3_2\\iedu.xslt\"/>   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\Transformacion" +
            "3_2\\ventavehiculos.xslt\"/>   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\Transfor" +
            "macion3_2\\pfic.xslt\"/>   <xsl:include href=\"C:\\ConectorPAXMYERS\\XML\\Transformaci" +
            "on3_2\\TuristaPasajeroExtranjero.xslt\"/>   <xsl:include href=\"C:\\ConectorPAXMYERS" +
            "\\XML\\Transformacion3_2\\leyendasFisc.xslt\"/>   <!-- Aquí iniciamos el procesamien" +
            "to de la cadena original con su | inicial y el terminador || -->   <xsl:template" +
            " match=\"/\">|<xsl:apply-templates select=\"/cfdi:Comprobante\"/>||</xsl:template>  " +
            " <!--  Aquí iniciamos el procesamiento de los datos incluidos en el comprobante " +
            "-->   <xsl:template match=\"cfdi:Comprobante\">    <!-- Iniciamos el tratamiento d" +
            "e los atributos de comprobante -->    <xsl:call-template name=\"Requerido\">     <" +
            "xsl:with-param name=\"valor\" select=\"./@version\"/>    </xsl:call-template>    <xs" +
            "l:call-template name=\"Requerido\">     <xsl:with-param name=\"valor\" select=\"./@fe" +
            "cha\"/>    </xsl:call-template>    <xsl:call-template name=\"Requerido\">     <xsl:" +
            "with-param name=\"valor\" select=\"./@tipoDeComprobante\"/>    </xsl:call-template> " +
            "   <xsl:call-template name=\"Requerido\">     <xsl:with-param name=\"valor\" select=" +
            "\"./@formaDePago\"/>    </xsl:call-template>    <xsl:call-template name=\"Opcional\"" +
            ">     <xsl:with-param name=\"valor\" select=\"./@condicionesDePago\"/>    </xsl:call" +
            "-template>    <xsl:call-template name=\"Requerido\">     <xsl:with-param name=\"val" +
            "or\" select=\"./@subTotal\"/>    </xsl:call-template>    <xsl:call-template name=\"O" +
            "pcional\">     <xsl:with-param name=\"valor\" select=\"./@descuento\"/>    </xsl:call" +
            "-template>    <xsl:call-template name=\"Opcional\">     <xsl:with-param name=\"valo" +
            "r\" select=\"./@TipoCambio\"/>    </xsl:call-template>    <xsl:call-template name=\"" +
            "Opcional\">     <xsl:with-param name=\"valor\" select=\"./@Moneda\"/>    </xsl:call-t" +
            "emplate>    <xsl:call-template name=\"Requerido\">     <xsl:with-param name=\"valor" +
            "\" select=\"./@total\"/>    </xsl:call-template>    <xsl:call-template name=\"Requer" +
            "ido\">     <xsl:with-param name=\"valor\" select=\"./@metodoDePago\"/>    </xsl:call-" +
            "template>    <xsl:call-template name=\"Requerido\">     <xsl:with-param name=\"valo" +
            "r\" select=\"./@LugarExpedicion\"/>    </xsl:call-template>    <xsl:call-template n" +
            "ame=\"Opcional\">     <xsl:with-param name=\"valor\" select=\"./@NumCtaPago\"/>    </x" +
            "sl:call-template>    <xsl:call-template name=\"Opcional\">     <xsl:with-param nam" +
            "e=\"valor\" select=\"./@FolioFiscalOrig\"/>    </xsl:call-template>    <xsl:call-tem" +
            "plate name=\"Opcional\">     <xsl:with-param name=\"valor\" select=\"./@SerieFolioFis" +
            "calOrig\"/>    </xsl:call-template>    <xsl:call-template name=\"Opcional\">     <x" +
            "sl:with-param name=\"valor\" select=\"./@FechaFolioFiscalOrig\"/>    </xsl:call-temp" +
            "late>    <xsl:call-template name=\"Opcional\">     <xsl:with-param name=\"valor\" se" +
            "lect=\"./@MontoFolioFiscalOrig\"/>    </xsl:call-template>    <!--     Llamadas pa" +
            "ra procesar al los sub nodos del comprobante    -->    <xsl:apply-templates sele" +
            "ct=\"./cfdi:Emisor\"/>    <xsl:apply-templates select=\"./cfdi:Receptor\"/>    <xsl:" +
            "apply-templates select=\"./cfdi:Conceptos\"/>    <xsl:apply-templates select=\"./cf" +
            "di:Impuestos\"/>    <xsl:apply-templates select=\"./cfdi:Complemento\"/>   </xsl:te" +
            "mplate>   <!-- Manejador de nodos tipo Emisor -->   <xsl:template match=\"cfdi:Em" +
            "isor\">    <!-- Iniciamos el tratamiento de los atributos del Emisor -->    <xsl:" +
            "call-template name=\"Requerido\">     <xsl:with-param name=\"valor\" select=\"./@rfc\"" +
            "/>    </xsl:call-template>    <xsl:call-template name=\"Opcional\">     <xsl:with-" +
            "param name=\"valor\" select=\"./@nombre\"/>    </xsl:call-template>    <!--     Llam" +
            "adas para procesar al los sub nodos del comprobante    -->    <xsl:apply-templat" +
            "es select=\"./cfdi:DomicilioFiscal\"/>    <xsl:if test=\"./cfdi:ExpedidoEn\">     <x" +
            "sl:call-template name=\"Domicilio\">      <xsl:with-param name=\"Nodo\" select=\"./cf" +
            "di:ExpedidoEn\"/>     </xsl:call-template>    </xsl:if>    <xsl:for-each select=\"" +
            "./cfdi:RegimenFiscal\">     <xsl:call-template name=\"Requerido\">      <xsl:with-p" +
            "aram name=\"valor\" select=\"./@Regimen\"/>     </xsl:call-template>    </xsl:for-ea" +
            "ch>   </xsl:template>   <!-- Manejador de nodos tipo Receptor -->   <xsl:templat" +
            "e match=\"cfdi:Receptor\">    <!-- Iniciamos el tratamiento de los atributos del R" +
            "eceptor -->    <xsl:call-template name=\"Requerido\">     <xsl:with-param name=\"va" +
            "lor\" select=\"./@rfc\"/>    </xsl:call-template>    <xsl:call-template name=\"Opcio" +
            "nal\">     <xsl:with-param name=\"valor\" select=\"./@nombre\"/>    </xsl:call-templa" +
            "te>    <!--     Llamadas para procesar al los sub nodos del Receptor    -->    <" +
            "xsl:if test=\"./cfdi:Domicilio\">     <xsl:call-template name=\"Domicilio\">      <x" +
            "sl:with-param name=\"Nodo\" select=\"./cfdi:Domicilio\"/>     </xsl:call-template>  " +
            "  </xsl:if>   </xsl:template>   <!-- Manejador de nodos tipo Conceptos -->   <xs" +
            "l:template match=\"cfdi:Conceptos\">    <!-- Llamada para procesar los distintos n" +
            "odos tipo Concepto -->    <xsl:for-each select=\"./cfdi:Concepto\">     <xsl:apply" +
            "-templates select=\".\"/>    </xsl:for-each>   </xsl:template>   <!-- Manejador de" +
            " nodos tipo Impuestos -->   <xsl:template match=\"cfdi:Impuestos\">    <xsl:for-ea" +
            "ch select=\"./cfdi:Retenciones/cfdi:Retencion\">     <xsl:apply-templates select=\"" +
            ".\"/>    </xsl:for-each>    <xsl:call-template name=\"Opcional\">     <xsl:with-par" +
            "am name=\"valor\" select=\"./@totalImpuestosRetenidos\"/>    </xsl:call-template>   " +
            " <xsl:for-each select=\"./cfdi:Traslados/cfdi:Traslado\">     <xsl:apply-templates" +
            " select=\".\"/>    </xsl:for-each>    <xsl:call-template name=\"Opcional\">     <xsl" +
            ":with-param name=\"valor\" select=\"./@totalImpuestosTrasladados\"/>    </xsl:call-t" +
            "emplate>   </xsl:template>   <!-- Manejador de nodos tipo Retencion -->   <xsl:t" +
            "emplate match=\"cfdi:Retencion\">    <xsl:call-template name=\"Requerido\">     <xsl" +
            ":with-param name=\"valor\" select=\"./@impuesto\"/>    </xsl:call-template>    <xsl:" +
            "call-template name=\"Requerido\">     <xsl:with-param name=\"valor\" select=\"./@impo" +
            "rte\"/>    </xsl:call-template>   </xsl:template>   <!-- Manejador de nodos tipo " +
            "Traslado -->   <xsl:template match=\"cfdi:Traslado\">    <xsl:call-template name=\"" +
            "Requerido\">     <xsl:with-param name=\"valor\" select=\"./@impuesto\"/>    </xsl:cal" +
            "l-template>    <xsl:call-template name=\"Requerido\">     <xsl:with-param name=\"va" +
            "lor\" select=\"./@tasa\"/>    </xsl:call-template>    <xsl:call-template name=\"Requ" +
            "erido\">     <xsl:with-param name=\"valor\" select=\"./@importe\"/>    </xsl:call-tem" +
            "plate>   </xsl:template>   <!-- Manejador de nodos tipo Complemento -->   <xsl:t" +
            "emplate match=\"cfdi:Complemento\">    <xsl:for-each select=\"./*\">     <xsl:apply-" +
            "templates select=\".\"/>    </xsl:for-each>   </xsl:template>   <!--    Manejador " +
            "de nodos tipo Concepto   -->   <xsl:template match=\"cfdi:Concepto\">    <!-- Inic" +
            "iamos el tratamiento de los atributos del Concepto -->    <xsl:call-template nam" +
            "e=\"Requerido\">     <xsl:with-param name=\"valor\" select=\"./@cantidad\"/>    </xsl:" +
            "call-template>    <xsl:call-template name=\"Requerido\">     <xsl:with-param name=" +
            "\"valor\" select=\"./@unidad\"/>    </xsl:call-template>    <xsl:call-template name=" +
            "\"Opcional\">     <xsl:with-param name=\"valor\" select=\"./@noIdentificacion\"/>    <" +
            "/xsl:call-template>    <xsl:call-template name=\"Requerido\">     <xsl:with-param " +
            "name=\"valor\" select=\"./@descripcion\"/>    </xsl:call-template>    <xsl:call-temp" +
            "late name=\"Requerido\">     <xsl:with-param name=\"valor\" select=\"./@valorUnitario" +
            "\"/>    </xsl:call-template>    <xsl:call-template name=\"Requerido\">     <xsl:wit" +
            "h-param name=\"valor\" select=\"./@importe\"/>    </xsl:call-template>    <!--     M" +
            "anejo de los distintos sub nodos de información aduanera de forma indistinta    " +
            "  a su grado de dependencia    -->    <xsl:for-each select=\".//cfdi:InformacionA" +
            "duanera\">     <xsl:apply-templates select=\".\"/>    </xsl:for-each>    <!-- Llama" +
            "da al manejador de nodos de Cuenta Predial en caso de existir -->    <xsl:if tes" +
            "t=\"./cfdi:CuentaPredial\">     <xsl:apply-templates select=\"./cfdi:CuentaPredial\"" +
            "/>    </xsl:if>    <!-- Llamada al manejador de nodos de ComplementoConcepto en " +
            "caso de existir -->    <xsl:if test=\"./cfdi:ComplementoConcepto\">     <xsl:apply" +
            "-templates select=\"./cfdi:ComplementoConcepto\"/>    </xsl:if>   </xsl:template> " +
            "  <!-- Manejador de nodos tipo Información Aduanera -->   <xsl:template match=\"c" +
            "fdi:InformacionAduanera\">    <!-- Manejo de los atributos de la información adua" +
            "nera -->    <xsl:call-template name=\"Requerido\">     <xsl:with-param name=\"valor" +
            "\" select=\"./@numero\"/>    </xsl:call-template>    <xsl:call-template name=\"Reque" +
            "rido\">     <xsl:with-param name=\"valor\" select=\"./@fecha\"/>    </xsl:call-templa" +
            "te>    <xsl:call-template name=\"Opcional\">     <xsl:with-param name=\"valor\" sele" +
            "ct=\"./@aduana\"/>    </xsl:call-template>   </xsl:template>   <!-- Manejador de n" +
            "odos tipo Información CuentaPredial -->   <xsl:template match=\"cfdi:CuentaPredia" +
            "l\">    <xsl:call-template name=\"Requerido\">     <xsl:with-param name=\"valor\" sel" +
            "ect=\"./@numero\"/>    </xsl:call-template>   </xsl:template>   <!-- Manejador de " +
            "nodos tipo ComplementoConcepto -->   <xsl:template match=\"cfdi:ComplementoConcep" +
            "to\">    <xsl:for-each select=\"./*\">     <xsl:apply-templates select=\".\"/>    </x" +
            "sl:for-each>   </xsl:template>   <!-- Manejador de nodos tipo Domicilio fiscal -" +
            "->   <xsl:template match=\"cfdi:DomicilioFiscal\">    <!-- Iniciamos el tratamient" +
            "o de los atributos del Domicilio Fiscal -->    <xsl:call-template name=\"Requerid" +
            "o\">     <xsl:with-param name=\"valor\" select=\"./@calle\"/>    </xsl:call-template>" +
            "    <xsl:call-template name=\"Opcional\">     <xsl:with-param name=\"valor\" select=" +
            "\"./@noExterior\"/>    </xsl:call-template>    <xsl:call-template name=\"Opcional\">" +
            "     <xsl:with-param name=\"valor\" select=\"./@noInterior\"/>    </xsl:call-templat" +
            "e>    <xsl:call-template name=\"Opcional\">     <xsl:with-param name=\"valor\" selec" +
            "t=\"./@colonia\"/>    </xsl:call-template>    <xsl:call-template name=\"Opcional\"> " +
            "    <xsl:with-param name=\"valor\" select=\"./@localidad\"/>    </xsl:call-template>" +
            "    <xsl:call-template name=\"Opcional\">     <xsl:with-param name=\"valor\" select=" +
            "\"./@referencia\"/>    </xsl:call-template>    <xsl:call-template name=\"Requerido\"" +
            ">     <xsl:with-param name=\"valor\" select=\"./@municipio\"/>    </xsl:call-templat" +
            "e>    <xsl:call-template name=\"Requerido\">     <xsl:with-param name=\"valor\" sele" +
            "ct=\"./@estado\"/>    </xsl:call-template>    <xsl:call-template name=\"Requerido\">" +
            "     <xsl:with-param name=\"valor\" select=\"./@pais\"/>    </xsl:call-template>    " +
            "<xsl:call-template name=\"Requerido\">     <xsl:with-param name=\"valor\" select=\"./" +
            "@codigoPostal\"/>    </xsl:call-template>   </xsl:template>   <!-- Manejador de n" +
            "odos tipo Domicilio -->   <xsl:template name=\"Domicilio\">    <xsl:param name=\"No" +
            "do\"/>    <!-- Iniciamos el tratamiento de los atributos del Domicilio  -->    <x" +
            "sl:call-template name=\"Opcional\">     <xsl:with-param name=\"valor\" select=\"$Nodo" +
            "/@calle\"/>    </xsl:call-template>    <xsl:call-template name=\"Opcional\">     <x" +
            "sl:with-param name=\"valor\" select=\"$Nodo/@noExterior\"/>    </xsl:call-template> " +
            "   <xsl:call-template name=\"Opcional\">     <xsl:with-param name=\"valor\" select=\"" +
            "$Nodo/@noInterior\"/>    </xsl:call-template>    <xsl:call-template name=\"Opciona" +
            "l\">     <xsl:with-param name=\"valor\" select=\"$Nodo/@colonia\"/>    </xsl:call-tem" +
            "plate>    <xsl:call-template name=\"Opcional\">     <xsl:with-param name=\"valor\" s" +
            "elect=\"$Nodo/@localidad\"/>    </xsl:call-template>    <xsl:call-template name=\"O" +
            "pcional\">     <xsl:with-param name=\"valor\" select=\"$Nodo/@referencia\"/>    </xsl" +
            ":call-template>    <xsl:call-template name=\"Opcional\">     <xsl:with-param name=" +
            "\"valor\" select=\"$Nodo/@municipio\"/>    </xsl:call-template>    <xsl:call-templat" +
            "e name=\"Opcional\">     <xsl:with-param name=\"valor\" select=\"$Nodo/@estado\"/>    " +
            "</xsl:call-template>    <xsl:call-template name=\"Requerido\">     <xsl:with-param" +
            " name=\"valor\" select=\"$Nodo/@pais\"/>    </xsl:call-template>    <xsl:call-templa" +
            "te name=\"Opcional\">     <xsl:with-param name=\"valor\" select=\"$Nodo/@codigoPostal" +
            "\"/>    </xsl:call-template>   </xsl:template>  </xsl:stylesheet>")]
        public string cadenaoriginal_3_2 {
            get {
                return ((string)(this["cadenaoriginal_3_2"]));
            }
            set {
                this["cadenaoriginal_3_2"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorMyers\\XML\\XMLGenerado\\")]
        public string RutaDocZips {
            get {
                return ((string)(this["RutaDocZips"]));
            }
            set {
                this["RutaDocZips"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorMyers\\XML\\LogErrores\\")]
        public string LogError {
            get {
                return ((string)(this["LogError"]));
            }
            set {
                this["LogError"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorMyers\\XML\\LogTimbres\\")]
        public string LogTimbrados {
            get {
                return ((string)(this["LogTimbrados"]));
            }
            set {
                this["LogTimbrados"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorMyers\\XML\\Docs")]
        public string rutaDocs {
            get {
                return ((string)(this["rutaDocs"]));
            }
            set {
                this["rutaDocs"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorMyers\\XML\\DocInvalido\\")]
        public string rutaDocInv {
            get {
                return ((string)(this["rutaDocInv"]));
            }
            set {
                this["rutaDocInv"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorMyers\\XML\\TXTGenerado\\")]
        public string rutaTXTGen {
            get {
                return ((string)(this["rutaTXTGen"]));
            }
            set {
                this["rutaTXTGen"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorMyers\\XML\\Imagenes\\")]
        public string imagenes {
            get {
                return ((string)(this["imagenes"]));
            }
            set {
                this["imagenes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorMyers\\XML\\Certificados")]
        public string rutaCertificados {
            get {
                return ((string)(this["rutaCertificados"]));
            }
            set {
                this["rutaCertificados"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorMyers\\XML\\Certificados\\")]
        public string rutaPEM {
            get {
                return ((string)(this["rutaPEM"]));
            }
            set {
                this["rutaPEM"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorMyers\\XML\\TxtDesconexion\\")]
        public string rutaDesconexion {
            get {
                return ((string)(this["rutaDesconexion"]));
            }
            set {
                this["rutaDesconexion"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("PRUEBAS")]
        public string Estatus {
            get {
                return ((string)(this["Estatus"]));
            }
            set {
                this["Estatus"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WSDL_PAX")]
        public string usuario {
            get {
                return ((string)(this["usuario"]));
            }
            set {
                this["usuario"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==")]
        public string password {
            get {
                return ((string)(this["password"]));
            }
            set {
                this["password"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("T")]
        public string Modo {
            get {
                return ((string)(this["Modo"]));
            }
            set {
                this["Modo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10000")]
        public string intervalo {
            get {
                return ((string)(this["intervalo"]));
            }
            set {
                this["intervalo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorMyers\\XML\\EsquemasAdicionales")]
        public string rutaEsquemaFord {
            get {
                return ((string)(this["rutaEsquemaFord"]));
            }
            set {
                this["rutaEsquemaFord"] = value;
            }
        }
    }
}

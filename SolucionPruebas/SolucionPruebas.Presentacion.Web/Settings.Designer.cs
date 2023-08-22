﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolucionPruebas.Presentacion.Web {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"UTF-8\"?><xs:schema targetNamespace=\"http://www.ahms" +
            "a.com/xsd/AddendaAHM1\" xmlns:ahmsa=\"http://www.ahmsa.com/xsd/AddendaAHM1\" xmlns:" +
            "xs=\"http://www.w3.org/2001/XMLSchema\" elementFormDefault=\"qualified\" attributeFo" +
            "rmDefault=\"unqualified\"> <xs:element name=\"AddendaAHM\">  <xs:annotation>   <xs:d" +
            "ocumentation>Estándar para la expresión de datos requeridos por AHMSA.</xs:docum" +
            "entation>  </xs:annotation>  <xs:complexType>   <xs:sequence>    <xs:element nam" +
            "e=\"Documento\">     <xs:annotation>      <xs:documentation>Nodo requerido para ex" +
            "presar la información del documento.</xs:documentation>     </xs:annotation>    " +
            " <xs:complexType>      <xs:sequence>       <xs:element name=\"Encabezado\">       " +
            " <xs:annotation>         <xs:documentation>Nodo para recibir la información requ" +
            "erida por AHMSA para el correcto         registro y trámite de pago del Comproba" +
            "nte Fiscal Digital.</xs:documentation>         </xs:annotation>         <xs:comp" +
            "lexType>          <xs:attribute name=\"NumSociedad\" type=\"xs:string\" use=\"require" +
            "d\">           <xs:annotation>            <xs:documentation>Atributo requerido pa" +
            "ra expresar la Clave de Sociedad            que ampara el comprobante.</xs:docum" +
            "entation>           </xs:annotation>          </xs:attribute>          <xs:attri" +
            "bute name=\"NumDivision\" type=\"xs:string\" use=\"required\">           <xs:annotatio" +
            "n>            <xs:documentation>Atributo requerido para expresar la división SAP" +
            " solo cuando se factura            a la sociedad S003, para las demás sociedades" +
            " dejar en blanco.</xs:documentation>           </xs:annotation>          </xs:at" +
            "tribute>          <xs:attribute name=\"NumProveedor\" type=\"xs:string\" use=\"requir" +
            "ed\">           <xs:annotation>            <xs:documentation>Atributo requerido q" +
            "ue acepta un valor numérico entero superior a 0 para            expresar el núme" +
            "ro con el que se identifica al proveedor en el sistema SAP de AHMSA.</xs:documen" +
            "tation>           </xs:annotation>          </xs:attribute>          <xs:attribu" +
            "te name=\"Correo\" type=\"xs:string\" use=\"required\">           <xs:annotation>     " +
            "       <xs:documentation>Atributo requerido para expresar el correo electrónico " +
            "del contacto del            área de cobranza por parte del proveedor, a quien se" +
            " debe de avisar en caso de encontrar            errores en el comprobante.</xs:d" +
            "ocumentation>           </xs:annotation>          </xs:attribute>          <xs:a" +
            "ttribute name=\"Moneda\" type=\"xs:string\" use=\"required\">           <xs:annotation" +
            ">            <xs:documentation>Atributo requerido para expresar el tipo de moned" +
            "a de los importes que            ampara el comprobante. Este tipo de moneda debe" +
            " ser el mismo que se indica en la orden            de compra/hoja de servicio/tr" +
            "ansporte. </xs:documentation>           </xs:annotation>          </xs:attribute" +
            ">         </xs:complexType>        </xs:element>        <xs:element name=\"Detall" +
            "e\" minOccurs=\"0\" maxOccurs=\"1\">         <xs:annotation>          <xs:documentati" +
            "on>Nodo requerido para expresar la información detallada del Comprobante        " +
            "  Fiscal Digital (Factura Electrónica), que se va a registrar en SAP. Se debe in" +
            "dicar el          número de documento que dio origen al CFD</xs:documentation>  " +
            "       </xs:annotation>         <xs:complexType>          <xs:sequence>         " +
            "  <xs:element name=\"Pedido\" minOccurs=\"1\" maxOccurs=\"20\">            <xs:annotat" +
            "ion>             <xs:documentation>Nodo requerido para expresar el número de ped" +
            "ido que dio             origen al CFD si se trata de una orden de compra (pedido" +
            ").</xs:documentation>            </xs:annotation>            <xs:complexType>   " +
            "          <xs:sequence>              <xs:element name=\"Recepcion\" nillable=\"true" +
            "\" minOccurs=\"0\" maxOccurs=\"20\">               <xs:annotation>                <xs" +
            ":documentation>Nodo opcional para expresar el/los número(s) de acuse de         " +
            "       recepción con el/los que se entrego la mercancía</xs:documentation>      " +
            "         </xs:annotation>              </xs:element>             </xs:sequence> " +
            "            <xs:attribute name=\"Num\" form=\"unqualified\" type=\"xs:string\">       " +
            "       <xs:annotation>               <xs:documentation>Atributo requerido que ac" +
            "epta un valor numérico entero superior               a 0 para expresar el número" +
            " de pedido </xs:documentation>              </xs:annotation>             </xs:at" +
            "tribute>            </xs:complexType>           </xs:element>           <xs:elem" +
            "ent name=\"HojaServicio\" minOccurs=\"0\" maxOccurs=\"1\">            <xs:complexType>" +
            "             <xs:attribute name=\"Num\" type=\"xs:string\">              <xs:annotat" +
            "ion>               <xs:documentation>Atributo requerido que acepta un valor numé" +
            "rico entero superior               a 0 para indicar el número de hoja de servici" +
            "o.</xs:documentation>              </xs:annotation>             </xs:attribute> " +
            "           </xs:complexType>           </xs:element>           <xs:element name=" +
            "\"Transporte\" minOccurs=\"0\" maxOccurs=\"1\">            <xs:complexType>           " +
            "  <xs:attribute name=\"Num\" type=\"xs:string\">              <xs:annotation>       " +
            "        <xs:documentation>Atributo requerido que acepta un valor numérico entero" +
            " superior               a 0 para expresar el número de transporte.</xs:documenta" +
            "tion>              </xs:annotation>             </xs:attribute>            </xs:" +
            "complexType>           </xs:element>           <xs:element name=\"CtaxPag\" minOcc" +
            "urs=\"0\" maxOccurs=\"1\">            <xs:complexType>             <xs:attribute nam" +
            "e=\"Num\" type=\"xs:string\">              <xs:annotation>               <xs:documen" +
            "tation>Atributo requerido que acepta un valor numérico entero superior          " +
            "     a 0 para los comprobantes de la Sociedad S003 – MINOSA, por concepto de Fle" +
            "te.               Expresa el número de cuenta por pagar como fue registrado en p" +
            "roven.</xs:documentation>              </xs:annotation>             </xs:attribu" +
            "te>             <xs:attribute name=\"Ejercicio\" type=\"xs:string\">              <x" +
            "s:annotation>               <xs:documentation>Atributo requerido que acepta un v" +
            "alor numérico entero superior               a 0 para  los comprobantes de la Soc" +
            "iedad S003 – MINOSA, por concepto de Flete.               Expresa el ejercicio f" +
            "iscal al que pertenece la cuenta por pagar.</xs:documentation>              </xs" +
            ":annotation>             </xs:attribute>            </xs:complexType>           " +
            "</xs:element>           <xs:element name=\"Liquidacion\" minOccurs=\"0\" maxOccurs=\"" +
            "1\">            <xs:complexType>             <xs:attribute name=\"FechaInicio\" typ" +
            "e=\"xs:string\">              <xs:annotation>               <xs:documentation>Atri" +
            "buto requerido que acepta una cadena de 10 caracteres alfanuméricos             " +
            "  en el formato DD.MM.AAAA, para expresar la fecha de inicio del período de liqu" +
            "idación que               se factura. .</xs:documentation>              </xs:ann" +
            "otation>             </xs:attribute>             <xs:attribute name=\"FechaFin\" t" +
            "ype=\"xs:string\">              <xs:annotation>               <xs:documentation>At" +
            "ributo requerido que acepta una cadena de 10 caracteres alfanuméricos           " +
            "    en el formato DD.MM.AAAA, para expresar la fecha final del período de liquid" +
            "ación que               se factura. .</xs:documentation>              </xs:annot" +
            "ation>             </xs:attribute>            </xs:complexType>           </xs:e" +
            "lement>          </xs:sequence>         </xs:complexType>        </xs:element>  " +
            "      <xs:element name=\"Anexos\" minOccurs=\"0\" >         <xs:annotation>         " +
            " <xs:documentation>Nodo opcional para expresar el/los nombre(s) de los archivo(s" +
            ") adicionales          al Comprobante Fiscal Digital</xs:documentation>         " +
            "</xs:annotation>\t\t\t\t\t\t\t         <xs:complexType>          <xs:sequence>         " +
            "  <xs:element name=\"Anexo\" type=\"xs:string\" minOccurs=\"0\" maxOccurs=\"5\" />      " +
            "    </xs:sequence>         </xs:complexType>        </xs:element>       </xs:seq" +
            "uence>       <xs:attribute name=\"Tipo\" type=\"xs:string\">        <xs:annotation> " +
            "        <xs:documentation>Atributo requerido que acepta un valor numérico entero" +
            " superior         a 0 que expresa el tipo de comprobante.</xs:documentation>    " +
            "    </xs:annotation>       </xs:attribute>       <xs:attribute name=\"Clase\" type" +
            "=\"xs:string\">        <xs:annotation>         <xs:documentation>Atributo requerid" +
            "o para precisar la clase de comprobante.</xs:documentation>        </xs:annotati" +
            "on>       </xs:attribute>      </xs:complexType>     </xs:element>    </xs:seque" +
            "nce>    <xs:attribute name=\"Version\" form=\"unqualified\" type=\"xs:string\">     <x" +
            "s:annotation>      <xs:documentation>Atributo requerido con valor prefijado a 1." +
            "0 que indica la versión de      la addenda bajo la que se encuentran expresados " +
            "los datos adicionales requeridos por AHMSA</xs:documentation>     </xs:annotatio" +
            "n>    </xs:attribute>   </xs:complexType>  </xs:element> </xs:schema>")]
        public string esquema {
            get {
                return ((string)(this["esquema"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<ahmsa:AddendaAHM Version=""1.0"" xmlns:ahmsa=""http://www.ahmsa.com/xsd/AddendaAHM1"" 
xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
xsi:schemaLocation=""http://www.ahmsa.com/xsd/AddendaAHM1 http://www.ahmsa.com/xsd/AddendaAHM1/AddendaAHM.xsd"">
	<ahmsa:Documento Tipo=""1"" Clase=""PE"">
		<ahmsa:Encabezado 
	NumSociedad=""S003"" 
	NumDivision=""D002"" 
	NumProveedor=""120788"" 
	Correo=""cecilia.espino@eficsa.com"" 
	Moneda=""USD"" />
		<ahmsa:Detalle>
				<ahmsa:Pedido Num=""4501053570"" />
			<ahmsa:HojaServicio Num="""" />
			<ahmsa:Transporte Num="""" />
			<ahmsa:CtaxPag Num=""2150487131"" Ejercicio="""" />
			<ahmsa:Liquidacion FechaInicio="""" FechaFin="""" />
		</ahmsa:Detalle>
	</ahmsa:Documento>
</ahmsa:AddendaAHM>")]
        public string addenda {
            get {
                return ((string)(this["addenda"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\ConectorPAXMYERS\\XML\\Certificados\\aaa010101aaa__csd_01.cer")]
        public string RutaCertificado {
            get {
                return ((string)(this["RutaCertificado"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\ConectorPAXMYERS\\XML\\Certificados\\aaa010101aaa__csd_01.key")]
        public string RutaLlave {
            get {
                return ((string)(this["RutaLlave"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\ConectorPAXMYERS\\XML\\Certificados\\RutaPfx\\")]
        public string RutaPfx {
            get {
                return ((string)(this["RutaPfx"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("12345678a")]
        public string Password {
            get {
                return ((string)(this["Password"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("aaa010101aaa__csd_01.cer")]
        public string NombreCertificado {
            get {
                return ((string)(this["NombreCertificado"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("aaa010101aaa__csd_01.key")]
        public string NombreLlave {
            get {
                return ((string)(this["NombreLlave"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.paxfacturacion.com/")]
        public string urlHostComercial {
            get {
                return ((string)(this["urlHostComercial"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
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
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("es-Mx")]
        public string DefaultLanguage {
            get {
                return ((string)(this["DefaultLanguage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\ConectorPAXMYERS\\XML\\Certificados")]
        public string RutaArchivos {
            get {
                return ((string)(this["RutaArchivos"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\ConectorPAXMYERS\\XML\\Certificados\\contraseña.txt")]
        public string RutaPassword {
            get {
                return ((string)(this["RutaPassword"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\OpenSSL-Win32\\bin\\openssl.exe")]
        public string RutaOpenSSL {
            get {
                return ((string)(this["RutaOpenSSL"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBALZNNBI840E5XGle/3AtijyN7MGtvfiL+0vHyMZ23LLbsFx6w5q/ZzU1L8Z2/XS71KRa3B3Lfi5EBV7sfRrdUxEscCS+naowIdplzhPyTjsm1v3Cs4SH9cVLwuBPjLk5BijpyK99dd2W/FZT+B0z97ojaWDoUais/wfgY8An+oI3AgMBAAECgYAyEeJLYA3KzEZlaoId/WQKXbC5QU78BPZwSd8LI3paZZx6yf0Pc6KVKpaEnAnhFfXBbnMme82cR3JyL+Hsjv1RMxz2aOQFmj1lSJAth3w4aAYx2gZzo0Hqg9Qjn/Bew/ChaDAwvDGqpbVfOeZkVuF36hqdgvXh4nD3POLnoXctgQJBAOrygWhspAauyzBsy8ZhHBS84iwlJbZ64RE+95d3sMGCIuW00+FOUxtD4bz0xbCaV3zkbGAntpjcGx5r1fN2ucMCQQDGowycldpEtqZyvgm2XnIRcHA8U3d71xVMguSQqIHrk+BYBdmdB9A86jhMS4CDDfxQPWT6k3ZlydAdwbFBDBp9AkBVqLDRt7pdzWC7eQ8adtFcJjl3yttjGo3wUbrHeJXzF1VN1o3heUMHj8o/sCZbawo2uLlinVgPh0BD6SEKMOEtAkAEVlUVKjR84Zwaz0l5APDurozU1GG8g4LEi+sfuX40vaLdaStKQXxriBW4nMFumySSP/Tvf77LFDAGJk+PgwslAkEArlH6rpmldpCH70qPwOeqd8a+5qikPTfg5e9z+uEjtT1aA0VksKtKo7lfeCSqV9Xk+5QZ4nZRw6djgyVGmEtdaw==")]
        public string LlavePem {
            get {
                return ((string)(this["LlavePem"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\Desarrollo1\\Desktop\\pem_key.pem")]
        public string RutaLlavePrivadaPEM {
            get {
                return ((string)(this["RutaLlavePrivadaPEM"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\Desarrollo1\\Desktop\\pem_cer.pem")]
        public string RutaCertificadoPEM {
            get {
                return ((string)(this["RutaCertificadoPEM"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\OpenSSL-Win32\\bin\\pem.pfx")]
        public string RutaArchivoPfx {
            get {
                return ((string)(this["RutaArchivoPfx"]));
            }
        }
    }
}
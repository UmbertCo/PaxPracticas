﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaxConectorRetenciones.Properties {
    
    
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorRetenciones\\XML\\XMLGenerado\\")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorRetenciones\\XML\\LogErrores\\")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorRetenciones\\XML\\LogTimbres\\")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorRetenciones\\XML\\Docs")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorRetenciones\\XML\\DocInvalido\\")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("PaxConectorRetenciones")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("Retenciones")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorRetenciones\\XML\\TXTGenerado\\")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorRetenciones\\XML\\Imagenes\\")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("<xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" xmlns:xs=\"http:/" +
            "/www.w3.org/2001/XMLSchema\" xmlns:fn=\"http://www.w3.org/2005/xpath-functions\" xm" +
            "lns:retenciones=\"http://www.sat.gob.mx/esquemas/retencionpago/1\" xmlns:arrendami" +
            "entoenfideicomiso=\"http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoe" +
            "nfideicomiso\" xmlns:dividendos=\"http://www.sat.gob.mx/esquemas/retencionpago/1/d" +
            "ividendos\" xmlns:enajenaciondeacciones=\"http://www.sat.gob.mx/esquemas/retencion" +
            "pago/1/enajenaciondeacciones\" xmlns:fideicomisonoempresarial=\"http://www.sat.gob" +
            ".mx/esquemas/retencionpago/1/fideicomisonoempresarial\" xmlns:intereses=\"http://w" +
            "ww.sat.gob.mx/esquemas/retencionpago/1/intereses\" xmlns:intereseshipotecarios=\"h" +
            "ttp://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios\" xmlns:opera" +
            "cionesderivados=\"http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesderiv" +
            "ados\" xmlns:pagosaextranjeros=\"http://www.sat.gob.mx/esquemas/retencionpago/1/pa" +
            "gosaextranjeros\" xmlns:planesderetiro=\"http://www.sat.gob.mx/esquemas/retencionp" +
            "ago/1/planesderetiro\" xmlns:premios=\"http://www.sat.gob.mx/esquemas/retencionpag" +
            "o/1/premios\" xmlns:sectorfinanciero=\"http://www.sat.gob.mx/esquemas/retencionpag" +
            "o/1/sectorfinanciero\" version=\"2.0\">\r\n<!--\r\n Con el siguiente método se establec" +
            "e que la salida deberá ser en texto \r\n-->\r\n<xsl:output method=\"text\" version=\"1." +
            "0\" encoding=\"UTF-8\" indent=\"no\"/>\r\n<!--\r\n \r\n\tEn esta sección se define la inclus" +
            "ión de las plantillas de utilerías para colapsar espacios\r\n\t\r\n-->\r\n<xsl:include " +
            "href=\"http://www.sat.gob.mx/sitio_internet/cfd/2/cadenaoriginal_2_0/utilerias.xs" +
            "lt\"/>\r\n<!--\r\n \r\n\t\tEn esta sección se define la inclusión de las demás plantillas" +
            " de transformación para \r\n\t\tla generación de las cadenas originales de los compl" +
            "ementos fiscales \r\n\t\r\n-->\r\n<xsl:include href=\"http://www.sat.gob.mx/esquemas/ret" +
            "encionpago/1/arrendamientoenfideicomiso/arrendamientoenfideicomiso.xslt\"/>\r\n<xsl" +
            ":include href=\"http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos/dividen" +
            "dos.xslt\"/>\r\n<xsl:include href=\"http://www.sat.gob.mx/esquemas/retencionpago/1/e" +
            "najenaciondeacciones/enajenaciondeacciones.xslt\"/>\r\n<xsl:include href=\"http://ww" +
            "w.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial/fideicomisonoempr" +
            "esarial.xslt\"/>\r\n<xsl:include href=\"http://www.sat.gob.mx/esquemas/retencionpago" +
            "/1/intereses/intereses.xslt\"/>\r\n<xsl:include href=\"http://www.sat.gob.mx/esquema" +
            "s/retencionpago/1/intereseshipotecarios/intereseshipotecarios.xslt\"/>\r\n<xsl:incl" +
            "ude href=\"http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros/pagos" +
            "aextranjeros.xslt\"/>\r\n<xsl:include href=\"http://www.sat.gob.mx/esquemas/retencio" +
            "npago/1/planesderetiro/planesderetiro.xslt\"/>\r\n<xsl:include href=\"http://www.sat" +
            ".gob.mx/esquemas/retencionpago/1/premios/premios.xslt\"/>\r\n<xsl:include href=\"htt" +
            "p://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados/operacionesc" +
            "onderivados.xslt\"/>\r\n<xsl:include href=\"http://www.sat.gob.mx/esquemas/retencion" +
            "pago/1/sectorfinanciero/sectorfinanciero.xslt\"/>\r\n<!--\r\n Aquí iniciamos el proce" +
            "samiento de la cadena original con su | inicial y el terminador || \r\n-->\r\n<xsl:t" +
            "emplate match=\"/\">\r\n|\r\n<xsl:apply-templates select=\"/retenciones:Retenciones\"/>\r" +
            "\n||\r\n</xsl:template>\r\n<!--\r\n  Aquí iniciamos el procesamiento de los datos inclu" +
            "idos en el comprobante \r\n-->\r\n<xsl:template match=\"retenciones:Retenciones\">\r\n<!" +
            "--\r\n Iniciamos el tratamiento de los atributos de comprobante \r\n-->\r\n<xsl:call-t" +
            "emplate name=\"Requerido\">\r\n<xsl:with-param name=\"valor\" select=\"./@Version\"/>\r\n<" +
            "/xsl:call-template>\r\n<xsl:call-template name=\"Requerido\">\r\n<xsl:with-param name=" +
            "\"valor\" select=\"./@NumCert\"/>\r\n</xsl:call-template>\r\n<xsl:call-template name=\"Op" +
            "cional\">\r\n<xsl:with-param name=\"valor\" select=\"./@FolioInt\"/>\r\n</xsl:call-templa" +
            "te>\r\n<xsl:call-template name=\"Requerido\">\r\n<xsl:with-param name=\"valor\" select=\"" +
            "./@FechaExp\"/>\r\n</xsl:call-template>\r\n<xsl:call-template name=\"Requerido\">\r\n<xsl" +
            ":with-param name=\"valor\" select=\"./@CveRetenc\"/>\r\n</xsl:call-template>\r\n<xsl:cal" +
            "l-template name=\"Opcional\">\r\n<xsl:with-param name=\"valor\" select=\"./@DescRetenc\"" +
            "/>\r\n</xsl:call-template>\r\n<!--\r\n\r\n\t\t\tLlamadas para procesar al los sub nodos de " +
            "la retencion\r\n\t\t\r\n-->\r\n<xsl:apply-templates select=\"./retenciones:Emisor\"/>\r\n<xs" +
            "l:apply-templates select=\"./retenciones:Receptor\"/>\r\n<xsl:apply-templates select" +
            "=\"./retenciones:Periodo\"/>\r\n<xsl:apply-templates select=\"./retenciones:Totales\"/" +
            ">\r\n<xsl:apply-templates select=\"./retenciones:Complemento\"/>\r\n</xsl:template>\r\n<" +
            "!--  Manejador de nodos tipo Emisor  -->\r\n<xsl:template match=\"retenciones:Emiso" +
            "r\">\r\n<!--\r\n Iniciamos el tratamiento de los atributos del Emisor \r\n-->\r\n<xsl:cal" +
            "l-template name=\"Requerido\">\r\n<xsl:with-param name=\"valor\" select=\"./@RFCEmisor\"" +
            "/>\r\n</xsl:call-template>\r\n<xsl:call-template name=\"Opcional\">\r\n<xsl:with-param n" +
            "ame=\"valor\" select=\"./@NomDenRazSocE\"/>\r\n</xsl:call-template>\r\n<xsl:call-templat" +
            "e name=\"Opcional\">\r\n<xsl:with-param name=\"valor\" select=\"./@CURPE\"/>\r\n</xsl:call" +
            "-template>\r\n</xsl:template>\r\n<!--  Manejador de nodos tipo Receptor  -->\r\n<xsl:t" +
            "emplate match=\"retenciones:Receptor\">\r\n<!--\r\n Iniciamos el tratamiento de los at" +
            "ributos del Receptor \r\n-->\r\n<xsl:call-template name=\"Requerido\">\r\n<xsl:with-para" +
            "m name=\"valor\" select=\"./@Nacionalidad\"/>\r\n</xsl:call-template>\r\n<!--\r\n\r\n\t\t\tLlam" +
            "adas para procesar al los sub nodos del Receptor\r\n\t\t\r\n-->\r\n<xsl:if test=\"./reten" +
            "ciones:Nacional\">\r\n<xsl:call-template name=\"Requerido\">\r\n<xsl:with-param name=\"v" +
            "alor\" select=\"./retenciones:Nacional/@RFCRecep\"/>\r\n</xsl:call-template>\r\n</xsl:i" +
            "f>\r\n<xsl:if test=\"./retenciones:Nacional\">\r\n<xsl:call-template name=\"Opcional\">\r" +
            "\n<xsl:with-param name=\"valor\" select=\"./retenciones:Nacional/@NomDenRazSocR\"/>\r\n" +
            "</xsl:call-template>\r\n</xsl:if>\r\n<xsl:if test=\"./retenciones:Nacional\">\r\n<xsl:ca" +
            "ll-template name=\"Opcional\">\r\n<xsl:with-param name=\"valor\" select=\"./retenciones" +
            ":Nacional/@CURPR\"/>\r\n</xsl:call-template>\r\n</xsl:if>\r\n<xsl:if test=\"./retencione" +
            "s:Extranjero\">\r\n<xsl:call-template name=\"Opcional\">\r\n<xsl:with-param name=\"valor" +
            "\" select=\"./retenciones:Extranjero/@NumRegIdTrib\"/>\r\n</xsl:call-template>\r\n</xsl" +
            ":if>\r\n<xsl:if test=\"./retenciones:Extranjero\">\r\n<xsl:call-template name=\"Requeri" +
            "do\">\r\n<xsl:with-param name=\"valor\" select=\"./retenciones:Extranjero/@NomDenRazSo" +
            "cR\"/>\r\n</xsl:call-template>\r\n</xsl:if>\r\n</xsl:template>\r\n<!--  Manejador de nodo" +
            "s tipo Periodo  -->\r\n<xsl:template match=\"retenciones:Periodo\">\r\n<!--\r\n Iniciamo" +
            "s el tratamiento de los atributos del Periodo \r\n-->\r\n<xsl:call-template name=\"Re" +
            "querido\">\r\n<xsl:with-param name=\"valor\" select=\"./@MesIni\"/>\r\n</xsl:call-templat" +
            "e>\r\n<xsl:call-template name=\"Requerido\">\r\n<xsl:with-param name=\"valor\" select=\"." +
            "/@MesFin\"/>\r\n</xsl:call-template>\r\n<xsl:call-template name=\"Requerido\">\r\n<xsl:wi" +
            "th-param name=\"valor\" select=\"./@Ejerc\"/>\r\n</xsl:call-template>\r\n</xsl:template>" +
            "\r\n<!--  Manejador de nodos tipo Totales  -->\r\n<xsl:template match=\"retenciones:T" +
            "otales\">\r\n<xsl:call-template name=\"Requerido\">\r\n<xsl:with-param name=\"valor\" sel" +
            "ect=\"./@montoTotOperacion\"/>\r\n</xsl:call-template>\r\n<xsl:call-template name=\"Req" +
            "uerido\">\r\n<xsl:with-param name=\"valor\" select=\"./@montoTotGrav\"/>\r\n</xsl:call-te" +
            "mplate>\r\n<xsl:call-template name=\"Requerido\">\r\n<xsl:with-param name=\"valor\" sele" +
            "ct=\"./@montoTotExent\"/>\r\n</xsl:call-template>\r\n<xsl:call-template name=\"Requerid" +
            "o\">\r\n<xsl:with-param name=\"valor\" select=\"./@montoTotRet\"/>\r\n</xsl:call-template" +
            ">\r\n<xsl:for-each select=\"./retenciones:ImpRetenidos\">\r\n<xsl:apply-templates sele" +
            "ct=\".\"/>\r\n<xsl:call-template name=\"Opcional\">\r\n<xsl:with-param name=\"valor\" sele" +
            "ct=\"./@BaseRet\"/>\r\n</xsl:call-template>\r\n<xsl:call-template name=\"Opcional\">\r\n<x" +
            "sl:with-param name=\"valor\" select=\"./@Impuesto\"/>\r\n</xsl:call-template>\r\n<xsl:ca" +
            "ll-template name=\"Requerido\">\r\n<xsl:with-param name=\"valor\" select=\"./@montoRet\"" +
            "/>\r\n</xsl:call-template>\r\n<xsl:call-template name=\"Requerido\">\r\n<xsl:with-param " +
            "name=\"valor\" select=\"./@TipoPagoRet\"/>\r\n</xsl:call-template>\r\n</xsl:for-each>\r\n<" +
            "/xsl:template>\r\n<!--  Manejador de nodos tipo Complemento  -->\r\n<xsl:template ma" +
            "tch=\"retenciones:Complemento\">\r\n<xsl:for-each select=\"./*\">\r\n<xsl:apply-template" +
            "s select=\".\"/>\r\n</xsl:for-each>\r\n</xsl:template>\r\n</xsl:stylesheet>")]
        public string cadenaoriginal_RET {
            get {
                return ((string)(this["cadenaoriginal_RET"]));
            }
            set {
                this["cadenaoriginal_RET"] = value;
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorRetenciones\\XML\\Certificados")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PaxConectorRetenciones\\XML\\Certificados\\")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("RETENCIONES")]
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
    }
}

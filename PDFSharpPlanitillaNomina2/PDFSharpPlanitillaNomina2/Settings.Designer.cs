﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PDFSharpPlanitillaNomina2 {
    
    
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
        [global::System.Configuration.DefaultSettingValueAttribute(@"<xsl:stylesheet version=""2.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:fn=""http://www.w3.org/2005/xpath-functions"" xmlns:tfd=""http://www.sat.gob.mx/TimbreFiscalDigital""><xsl:output method=""text"" version=""1.0"" encoding=""UTF-8"" indent=""no""/><xsl:template name=""Requerido""><xsl:param name=""valor""/>|<xsl:call-template name=""ManejaEspacios""><xsl:with-param name=""s"" select=""$valor""/></xsl:call-template></xsl:template><xsl:template name=""ManejaEspacios""><xsl:param name=""s""/><xsl:value-of select=""normalize-space(string($s))""/></xsl:template><xsl:template match=""/"">|<xsl:apply-templates select=""/tfd:TimbreFiscalDigital""/>||</xsl:template><xsl:template match=""tfd:TimbreFiscalDigital""><xsl:call-template name=""Requerido""><xsl:with-param name=""valor"" select=""./@version""/></xsl:call-template><xsl:call-template name=""Requerido""><xsl:with-param name=""valor"" select=""./@UUID""/></xsl:call-template><xsl:call-template name=""Requerido""><xsl:with-param name=""valor"" select=""./@FechaTimbrado""/></xsl:call-template><xsl:call-template name=""Requerido""><xsl:with-param name=""valor"" select=""./@selloCFD""/></xsl:call-template><xsl:call-template name=""Requerido""><xsl:with-param name=""valor"" select=""./@noCertificadoSAT""/></xsl:call-template></xsl:template></xsl:stylesheet>")]
        public string cadenaoriginal_TFD_1_0 {
            get {
                return ((string)(this["cadenaoriginal_TFD_1_0"]));
            }
            set {
                this["cadenaoriginal_TFD_1_0"] = value;
            }
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolucionPruebas.Presentacion.WindowsForms.Properties {
    
    
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
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\Proyectos\\Practicas\\SolucionPruebas\\SolucionPruebas.Presentacion.WindowsForms\\" +
            "Prueba")]
        public string rutaArchivos {
            get {
                return ((string)(this["rutaArchivos"]));
            }
            set {
                this["rutaArchivos"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\Proyectos\\Practicas\\SolucionPruebas\\SolucionPruebas.Presentacion.WindowsForms\\" +
            "Logs")]
        public string rutaLog {
            get {
                return ((string)(this["rutaLog"]));
            }
            set {
                this["rutaLog"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\Proyectos\\Practicas\\SolucionPruebas\\SolucionPruebas.Presentacion.WindowsForms\\" +
            "Errores")]
        public string rutaErrores {
            get {
                return ((string)(this["rutaErrores"]));
            }
            set {
                this["rutaErrores"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\Proyectos\\Practicas\\SolucionPruebas\\SolucionPruebas.Presentacion.WindowsForms\\" +
            "cfdi_prueba.xml")]
        public string nombreArchivo {
            get {
                return ((string)(this["nombreArchivo"]));
            }
            set {
                this["nombreArchivo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\Proyectos\\Practicas\\SolucionPruebas\\SolucionPruebas.Presentacion.WindowsForms\\" +
            "layout_prueba.txt")]
        public string nombreArchivoLayout {
            get {
                return ((string)(this["nombreArchivoLayout"]));
            }
            set {
                this["nombreArchivoLayout"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\Proyectos\\Practicas\\SolucionPruebas\\SolucionPruebas.Presentacion.WindowsForms\\" +
            "Certificados")]
        public string rutaCertificados {
            get {
                return ((string)(this["rutaCertificados"]));
            }
            set {
                this["rutaCertificados"] = value;
            }
        }
    }
}
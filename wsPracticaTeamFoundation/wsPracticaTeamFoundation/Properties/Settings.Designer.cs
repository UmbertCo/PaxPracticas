﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace wsPracticaTeamFoundation.Properties {
    
    
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
        [global::System.Configuration.DefaultSettingValueAttribute("gabriel.reyes@paxfacturacion.com")]
        public string Correo {
            get {
                return ((string)(this["Correo"]));
            }
            set {
                this["Correo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PracticaTeamFoundation\\Entrada\\")]
        public string rutaEntrada {
            get {
                return ((string)(this["rutaEntrada"]));
            }
            set {
                this["rutaEntrada"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Temp\\PracticaTeamFoundation\\")]
        public string rutaXml {
            get {
                return ((string)(this["rutaXml"]));
            }
            set {
                this["rutaXml"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Temp\\")]
        public string rutaTemporal {
            get {
                return ((string)(this["rutaTemporal"]));
            }
            set {
                this["rutaTemporal"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PracticaTeamFoundation\\Salida\\")]
        public string rutaSalida {
            get {
                return ((string)(this["rutaSalida"]));
            }
            set {
                this["rutaSalida"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PracticaTeamFoundation\\Errores\\")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\PracticaTeamFoundation\\Log\\")]
        public string rutaLog {
            get {
                return ((string)(this["rutaLog"]));
            }
            set {
                this["rutaLog"] = value;
            }
        }
    }
}
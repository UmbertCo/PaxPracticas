﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolucionPruebas.WindowsService.EnviadorComprobantes.Properties {
    
    
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
        [global::System.Configuration.DefaultSettingValueAttribute("60000")]
        public string Intervalo {
            get {
                return ((string)(this["Intervalo"]));
            }
            set {
                this["Intervalo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\EnviadorComprobantes\\Log\\")]
        public string Log {
            get {
                return ((string)(this["Log"]));
            }
            set {
                this["Log"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\EnviadorComprobantes\\LogErrores\\")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\EnviadorComprobantes\\Entrada\\")]
        public string RutaArchivos {
            get {
                return ((string)(this["RutaArchivos"]));
            }
            set {
                this["RutaArchivos"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\EnviadorComprobantes\\Certificados\\RutaPfx\\")]
        public string RutaPfx {
            get {
                return ((string)(this["RutaPfx"]));
            }
            set {
                this["RutaPfx"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\EnviadorComprobantes\\Salida\\")]
        public string RutaSalida {
            get {
                return ((string)(this["RutaSalida"]));
            }
            set {
                this["RutaSalida"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("12345678a")]
        public string Password {
            get {
                return ((string)(this["Password"]));
            }
            set {
                this["Password"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\EnviadorComprobantes\\Certificados\\")]
        public string RutaLlave {
            get {
                return ((string)(this["RutaLlave"]));
            }
            set {
                this["RutaLlave"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\EnviadorComprobantes\\Certificados\\")]
        public string RutaCertificado {
            get {
                return ((string)(this["RutaCertificado"]));
            }
            set {
                this["RutaCertificado"] = value;
            }
        }
    }
}

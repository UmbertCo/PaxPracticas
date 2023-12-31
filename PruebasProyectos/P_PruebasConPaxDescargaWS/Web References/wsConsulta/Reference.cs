﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18449.
// 
#pragma warning disable 1591

namespace P_PruebasConPaxDescargaWS.wsConsulta {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="wsPAXSATDescargaSoap", Namespace="http://paxfacturacion.com/")]
    public partial class wsPAXSATDescarga : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback fnConsultaDescargaSATOperationCompleted;
        
        private System.Threading.SendOrPostCallback fnConsultaEnvioSATOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public wsPAXSATDescarga() {
            this.Url = global::P_PruebasConPaxDescargaWS.Properties.Settings.Default.P_PruebasConPaxDescargaWS_wsConsulta_wsPAXSATDescarga;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event fnConsultaDescargaSATCompletedEventHandler fnConsultaDescargaSATCompleted;
        
        /// <remarks/>
        public event fnConsultaEnvioSATCompletedEventHandler fnConsultaEnvioSATCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://paxfacturacion.com/fnConsultaDescargaSAT", RequestNamespace="http://paxfacturacion.com/", ResponseNamespace="http://paxfacturacion.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] fnConsultaDescargaSAT(string psUsuarioPAX, string psContraseña, string psConsultaXML, out string sMensajeEstatus) {
            object[] results = this.Invoke("fnConsultaDescargaSAT", new object[] {
                        psUsuarioPAX,
                        psContraseña,
                        psConsultaXML});
            sMensajeEstatus = ((string)(results[1]));
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void fnConsultaDescargaSATAsync(string psUsuarioPAX, string psContraseña, string psConsultaXML) {
            this.fnConsultaDescargaSATAsync(psUsuarioPAX, psContraseña, psConsultaXML, null);
        }
        
        /// <remarks/>
        public void fnConsultaDescargaSATAsync(string psUsuarioPAX, string psContraseña, string psConsultaXML, object userState) {
            if ((this.fnConsultaDescargaSATOperationCompleted == null)) {
                this.fnConsultaDescargaSATOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfnConsultaDescargaSATOperationCompleted);
            }
            this.InvokeAsync("fnConsultaDescargaSAT", new object[] {
                        psUsuarioPAX,
                        psContraseña,
                        psConsultaXML}, this.fnConsultaDescargaSATOperationCompleted, userState);
        }
        
        private void OnfnConsultaDescargaSATOperationCompleted(object arg) {
            if ((this.fnConsultaDescargaSATCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fnConsultaDescargaSATCompleted(this, new fnConsultaDescargaSATCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://paxfacturacion.com/fnConsultaEnvioSAT", RequestNamespace="http://paxfacturacion.com/", ResponseNamespace="http://paxfacturacion.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string fnConsultaEnvioSAT(string psUsuarioPAX, string psContraseña, string psRFCEmisor, string psReceptor, decimal pnTotal, string psUUID) {
            object[] results = this.Invoke("fnConsultaEnvioSAT", new object[] {
                        psUsuarioPAX,
                        psContraseña,
                        psRFCEmisor,
                        psReceptor,
                        pnTotal,
                        psUUID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void fnConsultaEnvioSATAsync(string psUsuarioPAX, string psContraseña, string psRFCEmisor, string psReceptor, decimal pnTotal, string psUUID) {
            this.fnConsultaEnvioSATAsync(psUsuarioPAX, psContraseña, psRFCEmisor, psReceptor, pnTotal, psUUID, null);
        }
        
        /// <remarks/>
        public void fnConsultaEnvioSATAsync(string psUsuarioPAX, string psContraseña, string psRFCEmisor, string psReceptor, decimal pnTotal, string psUUID, object userState) {
            if ((this.fnConsultaEnvioSATOperationCompleted == null)) {
                this.fnConsultaEnvioSATOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfnConsultaEnvioSATOperationCompleted);
            }
            this.InvokeAsync("fnConsultaEnvioSAT", new object[] {
                        psUsuarioPAX,
                        psContraseña,
                        psRFCEmisor,
                        psReceptor,
                        pnTotal,
                        psUUID}, this.fnConsultaEnvioSATOperationCompleted, userState);
        }
        
        private void OnfnConsultaEnvioSATOperationCompleted(object arg) {
            if ((this.fnConsultaEnvioSATCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fnConsultaEnvioSATCompleted(this, new fnConsultaEnvioSATCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void fnConsultaDescargaSATCompletedEventHandler(object sender, fnConsultaDescargaSATCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fnConsultaDescargaSATCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal fnConsultaDescargaSATCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string sMensajeEstatus {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void fnConsultaEnvioSATCompletedEventHandler(object sender, fnConsultaEnvioSATCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fnConsultaEnvioSATCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal fnConsultaEnvioSATCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591
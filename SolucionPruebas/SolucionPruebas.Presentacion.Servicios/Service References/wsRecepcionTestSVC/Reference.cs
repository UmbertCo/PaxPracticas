﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolucionPruebas.Presentacion.Servicios.wsRecepcionTestSVC {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://test.paxfacturacion.com.mx:453", ConfigurationName="wsRecepcionTestSVC.IwcfRecepcion")]
    public interface IwcfRecepcion {
        
        [System.ServiceModel.OperationContractAttribute(Action="https://test.paxfacturacion.com.mx:453/IwcfRecepcion/fnEnviarXML", ReplyAction="https://test.paxfacturacion.com.mx:453/IwcfRecepcion/fnEnviarXMLResponse")]
        string fnEnviarXML(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IwcfRecepcionChannel : SolucionPruebas.Presentacion.Servicios.wsRecepcionTestSVC.IwcfRecepcion, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IwcfRecepcionClient : System.ServiceModel.ClientBase<SolucionPruebas.Presentacion.Servicios.wsRecepcionTestSVC.IwcfRecepcion>, SolucionPruebas.Presentacion.Servicios.wsRecepcionTestSVC.IwcfRecepcion {
        
        public IwcfRecepcionClient() {
        }
        
        public IwcfRecepcionClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IwcfRecepcionClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IwcfRecepcionClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IwcfRecepcionClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string fnEnviarXML(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion) {
            return base.Channel.fnEnviarXML(psComprobante, psTipoDocumento, pnId_Estructura, sNombre, sContraseña, sVersion);
        }
    }
}
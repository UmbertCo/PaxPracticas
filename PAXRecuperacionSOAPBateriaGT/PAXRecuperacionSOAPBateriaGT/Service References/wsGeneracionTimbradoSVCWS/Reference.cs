﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PAXRecuperacionSOAPBateriaGT.wsGeneracionTimbradoSVCWS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://www.paxfacturacion.com.mx:454", ConfigurationName="wsGeneracionTimbradoSVCWS.IwcfRecepcion")]
    public interface IwcfRecepcion {
        
        [System.ServiceModel.OperationContractAttribute(Action="https://ws.paxfacturacion.com.mx:454/IwcfRecepcion/fnEnviarTXT", ReplyAction="*")]
        string fnEnviarTXT(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion, string sOrigen);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IwcfRecepcionChannel : PAXRecuperacionSOAPBateriaGT.wsGeneracionTimbradoSVCWS.IwcfRecepcion, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IwcfRecepcionClient : System.ServiceModel.ClientBase<PAXRecuperacionSOAPBateriaGT.wsGeneracionTimbradoSVCWS.IwcfRecepcion>, PAXRecuperacionSOAPBateriaGT.wsGeneracionTimbradoSVCWS.IwcfRecepcion {
        
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
        
        public string fnEnviarTXT(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion, string sOrigen) {
            return base.Channel.fnEnviarTXT(psComprobante, psTipoDocumento, pnId_Estructura, sNombre, sContraseña, sVersion, sOrigen);
        }
    }
}

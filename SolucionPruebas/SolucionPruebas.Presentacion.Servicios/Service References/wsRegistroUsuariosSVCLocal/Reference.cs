﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolucionPruebas.Presentacion.Servicios.wsRegistroUsuariosSVCLocal {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://www.paxfacturacion.mx:455", ConfigurationName="wsRegistroUsuariosSVCLocal.IwcfRegistroSVC")]
    public interface IwcfRegistroSVC {
        
        [System.ServiceModel.OperationContractAttribute(Action="https://www.paxfacturacion.mx:455/IwcfRegistroSVC/fnRegistraUsuario", ReplyAction="https://www.paxfacturacion.mx:455/IwcfRegistroSVC/fnRegistraUsuarioResponse")]
        string fnRegistraUsuario(string sNombreUsuario, string sContraseñaUsuario, System.Collections.Generic.List<string> sDatos, string sNombreCertificado, byte[] btCertificado, string sContraseñaCert, string sNombreLlavePriv, byte[] btLLavePrivada, string sUsuarioHijo, string sContraseñaUsuarioHijo, string sEmailUsuHijo);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IwcfRegistroSVCChannel : SolucionPruebas.Presentacion.Servicios.wsRegistroUsuariosSVCLocal.IwcfRegistroSVC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IwcfRegistroSVCClient : System.ServiceModel.ClientBase<SolucionPruebas.Presentacion.Servicios.wsRegistroUsuariosSVCLocal.IwcfRegistroSVC>, SolucionPruebas.Presentacion.Servicios.wsRegistroUsuariosSVCLocal.IwcfRegistroSVC {
        
        public IwcfRegistroSVCClient() {
        }
        
        public IwcfRegistroSVCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IwcfRegistroSVCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IwcfRegistroSVCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IwcfRegistroSVCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string fnRegistraUsuario(string sNombreUsuario, string sContraseñaUsuario, System.Collections.Generic.List<string> sDatos, string sNombreCertificado, byte[] btCertificado, string sContraseñaCert, string sNombreLlavePriv, byte[] btLLavePrivada, string sUsuarioHijo, string sContraseñaUsuarioHijo, string sEmailUsuHijo) {
            return base.Channel.fnRegistraUsuario(sNombreUsuario, sContraseñaUsuario, sDatos, sNombreCertificado, btCertificado, sContraseñaCert, sNombreLlavePriv, btLLavePrivada, sUsuarioHijo, sContraseñaUsuarioHijo, sEmailUsuHijo);
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolucionPruebas.Presentacion.Servicios.CatalogoServicioLocal {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CatalogoServicioLocal.ICatalogoService")]
    public interface ICatalogoService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICatalogoService/fnRegistrarPersona", ReplyAction="http://tempuri.org/ICatalogoService/fnRegistrarPersonaResponse")]
        void fnRegistrarPersona(SolucionPruebas.Entidades.Personas ENPersona, SolucionPruebas.Entidades.Contacto ENContacto, bool pbRequiereTransaccion, bool pbEnTransaccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICatalogoService/fnRegistrarContacto", ReplyAction="http://tempuri.org/ICatalogoService/fnRegistrarContactoResponse")]
        void fnRegistrarContacto(SolucionPruebas.Entidades.Contacto ENContacto, bool pbRequireTransaccion, bool pbRequiereTransaccion, bool pbEnTransaccion);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICatalogoServiceChannel : SolucionPruebas.Presentacion.Servicios.CatalogoServicioLocal.ICatalogoService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CatalogoServiceClient : System.ServiceModel.ClientBase<SolucionPruebas.Presentacion.Servicios.CatalogoServicioLocal.ICatalogoService>, SolucionPruebas.Presentacion.Servicios.CatalogoServicioLocal.ICatalogoService {
        
        public CatalogoServiceClient() {
        }
        
        public CatalogoServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CatalogoServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CatalogoServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CatalogoServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void fnRegistrarPersona(SolucionPruebas.Entidades.Personas ENPersona, SolucionPruebas.Entidades.Contacto ENContacto, bool pbRequiereTransaccion, bool pbEnTransaccion) {
            base.Channel.fnRegistrarPersona(ENPersona, ENContacto, pbRequiereTransaccion, pbEnTransaccion);
        }
        
        public void fnRegistrarContacto(SolucionPruebas.Entidades.Contacto ENContacto, bool pbRequireTransaccion, bool pbRequiereTransaccion, bool pbEnTransaccion) {
            base.Channel.fnRegistrarContacto(ENContacto, pbRequireTransaccion, pbRequiereTransaccion, pbEnTransaccion);
        }
    }
}
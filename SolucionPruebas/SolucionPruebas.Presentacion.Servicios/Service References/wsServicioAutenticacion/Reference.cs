﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="wsServicioAutenticacion.ServiceAuthenticationSoap")]
    public interface ServiceAuthenticationSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento psNombre del espacio de nombres http://tempuri.org/ no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.HelloWorldResponse HelloWorld(SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.HelloWorldRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorld", Namespace="http://tempuri.org/", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.HelloWorldRequestBody Body;
        
        public HelloWorldRequest() {
        }
        
        public HelloWorldRequest(SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.HelloWorldRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class HelloWorldRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string psNombre;
        
        public HelloWorldRequestBody() {
        }
        
        public HelloWorldRequestBody(string psNombre) {
            this.psNombre = psNombre;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorldResponse", Namespace="http://tempuri.org/", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.HelloWorldResponseBody Body;
        
        public HelloWorldResponse() {
        }
        
        public HelloWorldResponse(SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.HelloWorldResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class HelloWorldResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string HelloWorldResult;
        
        public HelloWorldResponseBody() {
        }
        
        public HelloWorldResponseBody(string HelloWorldResult) {
            this.HelloWorldResult = HelloWorldResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServiceAuthenticationSoapChannel : SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.ServiceAuthenticationSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceAuthenticationSoapClient : System.ServiceModel.ClientBase<SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.ServiceAuthenticationSoap>, SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.ServiceAuthenticationSoap {
        
        public ServiceAuthenticationSoapClient() {
        }
        
        public ServiceAuthenticationSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceAuthenticationSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceAuthenticationSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceAuthenticationSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.HelloWorldResponse SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.ServiceAuthenticationSoap.HelloWorld(SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.HelloWorldRequest request) {
            return base.Channel.HelloWorld(request);
        }
        
        public string HelloWorld(string psNombre) {
            SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.HelloWorldRequest inValue = new SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.HelloWorldRequest();
            inValue.Body = new SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.HelloWorldRequestBody();
            inValue.Body.psNombre = psNombre;
            SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.HelloWorldResponse retVal = ((SolucionPruebas.Presentacion.Servicios.wsServicioAutenticacion.ServiceAuthenticationSoap)(this)).HelloWorld(inValue);
            return retVal.Body.HelloWorldResult;
        }
    }
}
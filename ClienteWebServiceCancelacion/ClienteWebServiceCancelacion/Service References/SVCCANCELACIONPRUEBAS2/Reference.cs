﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.36366
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ArrayOfString", Namespace="https://test.paxfacturacion.com.mx:456", ItemName="string")]
    [System.SerializableAttribute()]
    public class ArrayOfString : System.Collections.Generic.List<string> {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://test.paxfacturacion.com.mx:456", ConfigurationName="SVCCANCELACIONPRUEBAS2.wcfCancelaASMXSoap")]
    public interface wcfCancelaASMXSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento sListaUUID del espacio de nombres https://test.paxfacturacion.com.mx:456 no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="https://test.paxfacturacion.com.mx:456/fnCancelarXML", ReplyAction="*")]
        ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.fnCancelarXMLResponse fnCancelarXML(ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.fnCancelarXMLRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnCancelarXMLRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnCancelarXML", Namespace="https://test.paxfacturacion.com.mx:456", Order=0)]
        public ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.fnCancelarXMLRequestBody Body;
        
        public fnCancelarXMLRequest() {
        }
        
        public fnCancelarXMLRequest(ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.fnCancelarXMLRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://test.paxfacturacion.com.mx:456")]
    public partial class fnCancelarXMLRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.ArrayOfString sListaUUID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string psRFC;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string signature;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string sNombre;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string sContrasena;
        
        public fnCancelarXMLRequestBody() {
        }
        
        public fnCancelarXMLRequestBody(ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.ArrayOfString sListaUUID, string psRFC, string signature, string sNombre, string sContrasena) {
            this.sListaUUID = sListaUUID;
            this.psRFC = psRFC;
            this.signature = signature;
            this.sNombre = sNombre;
            this.sContrasena = sContrasena;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnCancelarXMLResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnCancelarXMLResponse", Namespace="https://test.paxfacturacion.com.mx:456", Order=0)]
        public ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.fnCancelarXMLResponseBody Body;
        
        public fnCancelarXMLResponse() {
        }
        
        public fnCancelarXMLResponse(ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.fnCancelarXMLResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://test.paxfacturacion.com.mx:456")]
    public partial class fnCancelarXMLResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fnCancelarXMLResult;
        
        public fnCancelarXMLResponseBody() {
        }
        
        public fnCancelarXMLResponseBody(string fnCancelarXMLResult) {
            this.fnCancelarXMLResult = fnCancelarXMLResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface wcfCancelaASMXSoapChannel : ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.wcfCancelaASMXSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wcfCancelaASMXSoapClient : System.ServiceModel.ClientBase<ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.wcfCancelaASMXSoap>, ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.wcfCancelaASMXSoap {
        
        public wcfCancelaASMXSoapClient() {
        }
        
        public wcfCancelaASMXSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public wcfCancelaASMXSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wcfCancelaASMXSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wcfCancelaASMXSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.fnCancelarXMLResponse ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.wcfCancelaASMXSoap.fnCancelarXML(ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.fnCancelarXMLRequest request) {
            return base.Channel.fnCancelarXML(request);
        }
        
        public string fnCancelarXML(ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.ArrayOfString sListaUUID, string psRFC, string signature, string sNombre, string sContrasena) {
            ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.fnCancelarXMLRequest inValue = new ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.fnCancelarXMLRequest();
            inValue.Body = new ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.fnCancelarXMLRequestBody();
            inValue.Body.sListaUUID = sListaUUID;
            inValue.Body.psRFC = psRFC;
            inValue.Body.signature = signature;
            inValue.Body.sNombre = sNombre;
            inValue.Body.sContrasena = sContrasena;
            ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.fnCancelarXMLResponse retVal = ((ClienteWebServiceCancelacion.SVCCANCELACIONPRUEBAS2.wcfCancelaASMXSoap)(this)).fnCancelarXML(inValue);
            return retVal.Body.fnCancelarXMLResult;
        }
    }
}
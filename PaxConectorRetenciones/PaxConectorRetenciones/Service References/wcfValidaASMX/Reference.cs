﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaxConectorRetenciones.wcfValidaASMX {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://test.paxfacturacion.com:460/", ConfigurationName="wcfValidaASMX.wcfValidaASMXSoap")]
    public interface wcfValidaASMXSoap {
        
        // CODEGEN: Generating message contract since element name psComprobante from namespace http://test.paxfacturacion.com:460/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://test.paxfacturacion.com:460/fnValidaXML", ReplyAction="*")]
        PaxConectorRetenciones.wcfValidaASMX.fnValidaXMLResponse fnValidaXML(PaxConectorRetenciones.wcfValidaASMX.fnValidaXMLRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnValidaXMLRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnValidaXML", Namespace="http://test.paxfacturacion.com:460/", Order=0)]
        public PaxConectorRetenciones.wcfValidaASMX.fnValidaXMLRequestBody Body;
        
        public fnValidaXMLRequest() {
        }
        
        public fnValidaXMLRequest(PaxConectorRetenciones.wcfValidaASMX.fnValidaXMLRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://test.paxfacturacion.com:460/")]
    public partial class fnValidaXMLRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string psComprobante;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string sNombre;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string sContrasena;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string sVersion;
        
        public fnValidaXMLRequestBody() {
        }
        
        public fnValidaXMLRequestBody(string psComprobante, string sNombre, string sContrasena, string sVersion) {
            this.psComprobante = psComprobante;
            this.sNombre = sNombre;
            this.sContrasena = sContrasena;
            this.sVersion = sVersion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnValidaXMLResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnValidaXMLResponse", Namespace="http://test.paxfacturacion.com:460/", Order=0)]
        public PaxConectorRetenciones.wcfValidaASMX.fnValidaXMLResponseBody Body;
        
        public fnValidaXMLResponse() {
        }
        
        public fnValidaXMLResponse(PaxConectorRetenciones.wcfValidaASMX.fnValidaXMLResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://test.paxfacturacion.com:460/")]
    public partial class fnValidaXMLResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fnValidaXMLResult;
        
        public fnValidaXMLResponseBody() {
        }
        
        public fnValidaXMLResponseBody(string fnValidaXMLResult) {
            this.fnValidaXMLResult = fnValidaXMLResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface wcfValidaASMXSoapChannel : PaxConectorRetenciones.wcfValidaASMX.wcfValidaASMXSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wcfValidaASMXSoapClient : System.ServiceModel.ClientBase<PaxConectorRetenciones.wcfValidaASMX.wcfValidaASMXSoap>, PaxConectorRetenciones.wcfValidaASMX.wcfValidaASMXSoap {
        
        public wcfValidaASMXSoapClient() {
        }
        
        public wcfValidaASMXSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public wcfValidaASMXSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wcfValidaASMXSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wcfValidaASMXSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        PaxConectorRetenciones.wcfValidaASMX.fnValidaXMLResponse PaxConectorRetenciones.wcfValidaASMX.wcfValidaASMXSoap.fnValidaXML(PaxConectorRetenciones.wcfValidaASMX.fnValidaXMLRequest request) {
            return base.Channel.fnValidaXML(request);
        }
        
        public string fnValidaXML(string psComprobante, string sNombre, string sContrasena, string sVersion) {
            PaxConectorRetenciones.wcfValidaASMX.fnValidaXMLRequest inValue = new PaxConectorRetenciones.wcfValidaASMX.fnValidaXMLRequest();
            inValue.Body = new PaxConectorRetenciones.wcfValidaASMX.fnValidaXMLRequestBody();
            inValue.Body.psComprobante = psComprobante;
            inValue.Body.sNombre = sNombre;
            inValue.Body.sContrasena = sContrasena;
            inValue.Body.sVersion = sVersion;
            PaxConectorRetenciones.wcfValidaASMX.fnValidaXMLResponse retVal = ((PaxConectorRetenciones.wcfValidaASMX.wcfValidaASMXSoap)(this)).fnValidaXML(inValue);
            return retVal.Body.fnValidaXMLResult;
        }
    }
}

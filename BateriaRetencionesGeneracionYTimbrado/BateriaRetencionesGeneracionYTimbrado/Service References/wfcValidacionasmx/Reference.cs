﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://test.paxfacturacion.com:460/", ConfigurationName="wfcValidacionasmx.wcfValidaASMXSoap")]
    public interface wcfValidaASMXSoap {
        
        // CODEGEN: Generating message contract since element name psComprobante from namespace http://test.paxfacturacion.com:460/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://test.paxfacturacion.com:460/fnValidaXML", ReplyAction="*")]
        BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.fnValidaXMLResponse fnValidaXML(BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.fnValidaXMLRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnValidaXMLRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnValidaXML", Namespace="http://test.paxfacturacion.com:460/", Order=0)]
        public BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.fnValidaXMLRequestBody Body;
        
        public fnValidaXMLRequest() {
        }
        
        public fnValidaXMLRequest(BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.fnValidaXMLRequestBody Body) {
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
        public BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.fnValidaXMLResponseBody Body;
        
        public fnValidaXMLResponse() {
        }
        
        public fnValidaXMLResponse(BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.fnValidaXMLResponseBody Body) {
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
    public interface wcfValidaASMXSoapChannel : BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.wcfValidaASMXSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wcfValidaASMXSoapClient : System.ServiceModel.ClientBase<BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.wcfValidaASMXSoap>, BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.wcfValidaASMXSoap {
        
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
        BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.fnValidaXMLResponse BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.wcfValidaASMXSoap.fnValidaXML(BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.fnValidaXMLRequest request) {
            return base.Channel.fnValidaXML(request);
        }
        
        public string fnValidaXML(string psComprobante, string sNombre, string sContrasena, string sVersion) {
            BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.fnValidaXMLRequest inValue = new BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.fnValidaXMLRequest();
            inValue.Body = new BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.fnValidaXMLRequestBody();
            inValue.Body.psComprobante = psComprobante;
            inValue.Body.sNombre = sNombre;
            inValue.Body.sContrasena = sContrasena;
            inValue.Body.sVersion = sVersion;
            BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.fnValidaXMLResponse retVal = ((BateriaRetencionesGeneracionYTimbrado.wfcValidacionasmx.wcfValidaASMXSoap)(this)).fnValidaXML(inValue);
            return retVal.Body.fnValidaXMLResult;
        }
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PAXConectorFTPGTCFDI33.CancelacionASMX {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ArrayOfString", Namespace="https://www.paxfacturacion.com.mx:476", ItemName="string")]
    [System.SerializableAttribute()]
    public class ArrayOfString : System.Collections.Generic.List<string> {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://www.paxfacturacion.com.mx:476", ConfigurationName="CancelacionASMX.wcfCancelaASMXSoap")]
    public interface wcfCancelaASMXSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento sListaUUID del espacio de nombres https://www.paxfacturacion.com.mx:476 no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="https://www.paxfacturacion.com.mx:476/fnCancelarXML", ReplyAction="*")]
        PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLResponse fnCancelarXML(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento sListaUUID del espacio de nombres https://www.paxfacturacion.com.mx:476 no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="https://www.paxfacturacion.com.mx:476/fnCancelarDistribuidoresXML", ReplyAction="*")]
        PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLResponse fnCancelarDistribuidoresXML(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento sListaUUID del espacio de nombres https://www.paxfacturacion.com.mx:476 no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="https://www.paxfacturacion.com.mx:476/fnCancelarXMLMP", ReplyAction="*")]
        PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLMPResponse fnCancelarXMLMP(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLMPRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento sListaUUID del espacio de nombres https://www.paxfacturacion.com.mx:476 no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="https://www.paxfacturacion.com.mx:476/fnCancelarDistribuidoresXMLMP", ReplyAction="*")]
        PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLMPResponse fnCancelarDistribuidoresXMLMP(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLMPRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnCancelarXMLRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnCancelarXML", Namespace="https://www.paxfacturacion.com.mx:476", Order=0)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLRequestBody Body;
        
        public fnCancelarXMLRequest() {
        }
        
        public fnCancelarXMLRequest(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.paxfacturacion.com.mx:476")]
    public partial class fnCancelarXMLRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaUUID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string psRFCEmisor;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString psRFCReceptor;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaTotales;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string sNombre;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string sContrasena;
        
        public fnCancelarXMLRequestBody() {
        }
        
        public fnCancelarXMLRequestBody(PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaUUID, string psRFCEmisor, PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString psRFCReceptor, PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaTotales, string sNombre, string sContrasena) {
            this.sListaUUID = sListaUUID;
            this.psRFCEmisor = psRFCEmisor;
            this.psRFCReceptor = psRFCReceptor;
            this.sListaTotales = sListaTotales;
            this.sNombre = sNombre;
            this.sContrasena = sContrasena;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnCancelarXMLResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnCancelarXMLResponse", Namespace="https://www.paxfacturacion.com.mx:476", Order=0)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLResponseBody Body;
        
        public fnCancelarXMLResponse() {
        }
        
        public fnCancelarXMLResponse(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.paxfacturacion.com.mx:476")]
    public partial class fnCancelarXMLResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fnCancelarXMLResult;
        
        public fnCancelarXMLResponseBody() {
        }
        
        public fnCancelarXMLResponseBody(string fnCancelarXMLResult) {
            this.fnCancelarXMLResult = fnCancelarXMLResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnCancelarDistribuidoresXMLRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnCancelarDistribuidoresXML", Namespace="https://www.paxfacturacion.com.mx:476", Order=0)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLRequestBody Body;
        
        public fnCancelarDistribuidoresXMLRequest() {
        }
        
        public fnCancelarDistribuidoresXMLRequest(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.paxfacturacion.com.mx:476")]
    public partial class fnCancelarDistribuidoresXMLRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaUUID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string psRFCEmisor;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString psRFCReceptor;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaTotales;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string signature;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string sNombre;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string sContrasena;
        
        public fnCancelarDistribuidoresXMLRequestBody() {
        }
        
        public fnCancelarDistribuidoresXMLRequestBody(PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaUUID, string psRFCEmisor, PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString psRFCReceptor, PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaTotales, string signature, string sNombre, string sContrasena) {
            this.sListaUUID = sListaUUID;
            this.psRFCEmisor = psRFCEmisor;
            this.psRFCReceptor = psRFCReceptor;
            this.sListaTotales = sListaTotales;
            this.signature = signature;
            this.sNombre = sNombre;
            this.sContrasena = sContrasena;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnCancelarDistribuidoresXMLResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnCancelarDistribuidoresXMLResponse", Namespace="https://www.paxfacturacion.com.mx:476", Order=0)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLResponseBody Body;
        
        public fnCancelarDistribuidoresXMLResponse() {
        }
        
        public fnCancelarDistribuidoresXMLResponse(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.paxfacturacion.com.mx:476")]
    public partial class fnCancelarDistribuidoresXMLResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fnCancelarDistribuidoresXMLResult;
        
        public fnCancelarDistribuidoresXMLResponseBody() {
        }
        
        public fnCancelarDistribuidoresXMLResponseBody(string fnCancelarDistribuidoresXMLResult) {
            this.fnCancelarDistribuidoresXMLResult = fnCancelarDistribuidoresXMLResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnCancelarXMLMPRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnCancelarXMLMP", Namespace="https://www.paxfacturacion.com.mx:476", Order=0)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLMPRequestBody Body;
        
        public fnCancelarXMLMPRequest() {
        }
        
        public fnCancelarXMLMPRequest(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLMPRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.paxfacturacion.com.mx:476")]
    public partial class fnCancelarXMLMPRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaUUID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string psRFCEmisor;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string sNombre;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string sContrasena;
        
        public fnCancelarXMLMPRequestBody() {
        }
        
        public fnCancelarXMLMPRequestBody(PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaUUID, string psRFCEmisor, string sNombre, string sContrasena) {
            this.sListaUUID = sListaUUID;
            this.psRFCEmisor = psRFCEmisor;
            this.sNombre = sNombre;
            this.sContrasena = sContrasena;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnCancelarXMLMPResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnCancelarXMLMPResponse", Namespace="https://www.paxfacturacion.com.mx:476", Order=0)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLMPResponseBody Body;
        
        public fnCancelarXMLMPResponse() {
        }
        
        public fnCancelarXMLMPResponse(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLMPResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.paxfacturacion.com.mx:476")]
    public partial class fnCancelarXMLMPResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fnCancelarXMLMPResult;
        
        public fnCancelarXMLMPResponseBody() {
        }
        
        public fnCancelarXMLMPResponseBody(string fnCancelarXMLMPResult) {
            this.fnCancelarXMLMPResult = fnCancelarXMLMPResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnCancelarDistribuidoresXMLMPRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnCancelarDistribuidoresXMLMP", Namespace="https://www.paxfacturacion.com.mx:476", Order=0)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLMPRequestBody Body;
        
        public fnCancelarDistribuidoresXMLMPRequest() {
        }
        
        public fnCancelarDistribuidoresXMLMPRequest(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLMPRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.paxfacturacion.com.mx:476")]
    public partial class fnCancelarDistribuidoresXMLMPRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaUUID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string psRFCEmisor;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string signature;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string sNombre;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string sContrasena;
        
        public fnCancelarDistribuidoresXMLMPRequestBody() {
        }
        
        public fnCancelarDistribuidoresXMLMPRequestBody(PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaUUID, string psRFCEmisor, string signature, string sNombre, string sContrasena) {
            this.sListaUUID = sListaUUID;
            this.psRFCEmisor = psRFCEmisor;
            this.signature = signature;
            this.sNombre = sNombre;
            this.sContrasena = sContrasena;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnCancelarDistribuidoresXMLMPResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnCancelarDistribuidoresXMLMPResponse", Namespace="https://www.paxfacturacion.com.mx:476", Order=0)]
        public PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLMPResponseBody Body;
        
        public fnCancelarDistribuidoresXMLMPResponse() {
        }
        
        public fnCancelarDistribuidoresXMLMPResponse(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLMPResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.paxfacturacion.com.mx:476")]
    public partial class fnCancelarDistribuidoresXMLMPResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fnCancelarDistribuidoresXMLMPResult;
        
        public fnCancelarDistribuidoresXMLMPResponseBody() {
        }
        
        public fnCancelarDistribuidoresXMLMPResponseBody(string fnCancelarDistribuidoresXMLMPResult) {
            this.fnCancelarDistribuidoresXMLMPResult = fnCancelarDistribuidoresXMLMPResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface wcfCancelaASMXSoapChannel : PAXConectorFTPGTCFDI33.CancelacionASMX.wcfCancelaASMXSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wcfCancelaASMXSoapClient : System.ServiceModel.ClientBase<PAXConectorFTPGTCFDI33.CancelacionASMX.wcfCancelaASMXSoap>, PAXConectorFTPGTCFDI33.CancelacionASMX.wcfCancelaASMXSoap {
        
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
        PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLResponse PAXConectorFTPGTCFDI33.CancelacionASMX.wcfCancelaASMXSoap.fnCancelarXML(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLRequest request) {
            return base.Channel.fnCancelarXML(request);
        }
        
        public string fnCancelarXML(PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaUUID, string psRFCEmisor, PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString psRFCReceptor, PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaTotales, string sNombre, string sContrasena) {
            PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLRequest inValue = new PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLRequest();
            inValue.Body = new PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLRequestBody();
            inValue.Body.sListaUUID = sListaUUID;
            inValue.Body.psRFCEmisor = psRFCEmisor;
            inValue.Body.psRFCReceptor = psRFCReceptor;
            inValue.Body.sListaTotales = sListaTotales;
            inValue.Body.sNombre = sNombre;
            inValue.Body.sContrasena = sContrasena;
            PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLResponse retVal = ((PAXConectorFTPGTCFDI33.CancelacionASMX.wcfCancelaASMXSoap)(this)).fnCancelarXML(inValue);
            return retVal.Body.fnCancelarXMLResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLResponse PAXConectorFTPGTCFDI33.CancelacionASMX.wcfCancelaASMXSoap.fnCancelarDistribuidoresXML(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLRequest request) {
            return base.Channel.fnCancelarDistribuidoresXML(request);
        }
        
        public string fnCancelarDistribuidoresXML(PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaUUID, string psRFCEmisor, PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString psRFCReceptor, PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaTotales, string signature, string sNombre, string sContrasena) {
            PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLRequest inValue = new PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLRequest();
            inValue.Body = new PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLRequestBody();
            inValue.Body.sListaUUID = sListaUUID;
            inValue.Body.psRFCEmisor = psRFCEmisor;
            inValue.Body.psRFCReceptor = psRFCReceptor;
            inValue.Body.sListaTotales = sListaTotales;
            inValue.Body.signature = signature;
            inValue.Body.sNombre = sNombre;
            inValue.Body.sContrasena = sContrasena;
            PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLResponse retVal = ((PAXConectorFTPGTCFDI33.CancelacionASMX.wcfCancelaASMXSoap)(this)).fnCancelarDistribuidoresXML(inValue);
            return retVal.Body.fnCancelarDistribuidoresXMLResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLMPResponse PAXConectorFTPGTCFDI33.CancelacionASMX.wcfCancelaASMXSoap.fnCancelarXMLMP(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLMPRequest request) {
            return base.Channel.fnCancelarXMLMP(request);
        }
        
        public string fnCancelarXMLMP(PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaUUID, string psRFCEmisor, string sNombre, string sContrasena) {
            PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLMPRequest inValue = new PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLMPRequest();
            inValue.Body = new PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLMPRequestBody();
            inValue.Body.sListaUUID = sListaUUID;
            inValue.Body.psRFCEmisor = psRFCEmisor;
            inValue.Body.sNombre = sNombre;
            inValue.Body.sContrasena = sContrasena;
            PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarXMLMPResponse retVal = ((PAXConectorFTPGTCFDI33.CancelacionASMX.wcfCancelaASMXSoap)(this)).fnCancelarXMLMP(inValue);
            return retVal.Body.fnCancelarXMLMPResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLMPResponse PAXConectorFTPGTCFDI33.CancelacionASMX.wcfCancelaASMXSoap.fnCancelarDistribuidoresXMLMP(PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLMPRequest request) {
            return base.Channel.fnCancelarDistribuidoresXMLMP(request);
        }
        
        public string fnCancelarDistribuidoresXMLMP(PAXConectorFTPGTCFDI33.CancelacionASMX.ArrayOfString sListaUUID, string psRFCEmisor, string signature, string sNombre, string sContrasena) {
            PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLMPRequest inValue = new PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLMPRequest();
            inValue.Body = new PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLMPRequestBody();
            inValue.Body.sListaUUID = sListaUUID;
            inValue.Body.psRFCEmisor = psRFCEmisor;
            inValue.Body.signature = signature;
            inValue.Body.sNombre = sNombre;
            inValue.Body.sContrasena = sContrasena;
            PAXConectorFTPGTCFDI33.CancelacionASMX.fnCancelarDistribuidoresXMLMPResponse retVal = ((PAXConectorFTPGTCFDI33.CancelacionASMX.wcfCancelaASMXSoap)(this)).fnCancelarDistribuidoresXMLMP(inValue);
            return retVal.Body.fnCancelarDistribuidoresXMLMPResult;
        }
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ArrayOfString", Namespace="http://www.paxfacturacion.com:458/", ItemName="string")]
    [System.SerializableAttribute()]
    public class ArrayOfString : System.Collections.Generic.List<string> {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.paxfacturacion.com:458/", ConfigurationName="wsCancelacionRetencionProduccion.wcfCancelaASMXSoap")]
    public interface wcfCancelaASMXSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento sListaUUID del espacio de nombres http://www.paxfacturacion.com:458/ no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.paxfacturacion.com:458/fnCancelarXML", ReplyAction="*")]
        SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.fnCancelarXMLResponse fnCancelarXML(SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.fnCancelarXMLRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnCancelarXMLRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnCancelarXML", Namespace="http://www.paxfacturacion.com:458/", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.fnCancelarXMLRequestBody Body;
        
        public fnCancelarXMLRequest() {
        }
        
        public fnCancelarXMLRequest(SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.fnCancelarXMLRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.paxfacturacion.com:458/")]
    public partial class fnCancelarXMLRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.ArrayOfString sListaUUID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string psRFC;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public int pnId_Estructura;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string sNombre;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string sContrasena;
        
        public fnCancelarXMLRequestBody() {
        }
        
        public fnCancelarXMLRequestBody(SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.ArrayOfString sListaUUID, string psRFC, int pnId_Estructura, string sNombre, string sContrasena) {
            this.sListaUUID = sListaUUID;
            this.psRFC = psRFC;
            this.pnId_Estructura = pnId_Estructura;
            this.sNombre = sNombre;
            this.sContrasena = sContrasena;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnCancelarXMLResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnCancelarXMLResponse", Namespace="http://www.paxfacturacion.com:458/", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.fnCancelarXMLResponseBody Body;
        
        public fnCancelarXMLResponse() {
        }
        
        public fnCancelarXMLResponse(SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.fnCancelarXMLResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.paxfacturacion.com:458/")]
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
    public interface wcfCancelaASMXSoapChannel : SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.wcfCancelaASMXSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wcfCancelaASMXSoapClient : System.ServiceModel.ClientBase<SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.wcfCancelaASMXSoap>, SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.wcfCancelaASMXSoap {
        
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
        SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.fnCancelarXMLResponse SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.wcfCancelaASMXSoap.fnCancelarXML(SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.fnCancelarXMLRequest request) {
            return base.Channel.fnCancelarXML(request);
        }
        
        public string fnCancelarXML(SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.ArrayOfString sListaUUID, string psRFC, int pnId_Estructura, string sNombre, string sContrasena) {
            SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.fnCancelarXMLRequest inValue = new SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.fnCancelarXMLRequest();
            inValue.Body = new SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.fnCancelarXMLRequestBody();
            inValue.Body.sListaUUID = sListaUUID;
            inValue.Body.psRFC = psRFC;
            inValue.Body.pnId_Estructura = pnId_Estructura;
            inValue.Body.sNombre = sNombre;
            inValue.Body.sContrasena = sContrasena;
            SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.fnCancelarXMLResponse retVal = ((SolucionPruebas.Presentacion.Servicios.wsCancelacionRetencionProduccion.wcfCancelaASMXSoap)(this)).fnCancelarXML(inValue);
            return retVal.Body.fnCancelarXMLResult;
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.paxfacturacion.com:458/", ConfigurationName="wsRecepcionRetencionProduccion.wcfRecepcionASMXSoap")]
    public interface wcfRecepcionASMXSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento psComprobante del espacio de nombres http://www.paxfacturacion.com:458/ no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.paxfacturacion.com:458/fnEnviarXML", ReplyAction="*")]
        SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.fnEnviarXMLResponse fnEnviarXML(SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.fnEnviarXMLRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnEnviarXMLRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnEnviarXML", Namespace="http://www.paxfacturacion.com:458/", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.fnEnviarXMLRequestBody Body;
        
        public fnEnviarXMLRequest() {
        }
        
        public fnEnviarXMLRequest(SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.fnEnviarXMLRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.paxfacturacion.com:458/")]
    public partial class fnEnviarXMLRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string psComprobante;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int psCveRetencion;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public int pnId_Estructura;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string sNombre;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string sContrasena;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string sVersion;
        
        public fnEnviarXMLRequestBody() {
        }
        
        public fnEnviarXMLRequestBody(string psComprobante, int psCveRetencion, int pnId_Estructura, string sNombre, string sContrasena, string sVersion) {
            this.psComprobante = psComprobante;
            this.psCveRetencion = psCveRetencion;
            this.pnId_Estructura = pnId_Estructura;
            this.sNombre = sNombre;
            this.sContrasena = sContrasena;
            this.sVersion = sVersion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnEnviarXMLResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnEnviarXMLResponse", Namespace="http://www.paxfacturacion.com:458/", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.fnEnviarXMLResponseBody Body;
        
        public fnEnviarXMLResponse() {
        }
        
        public fnEnviarXMLResponse(SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.fnEnviarXMLResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.paxfacturacion.com:458/")]
    public partial class fnEnviarXMLResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fnEnviarXMLResult;
        
        public fnEnviarXMLResponseBody() {
        }
        
        public fnEnviarXMLResponseBody(string fnEnviarXMLResult) {
            this.fnEnviarXMLResult = fnEnviarXMLResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface wcfRecepcionASMXSoapChannel : SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.wcfRecepcionASMXSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wcfRecepcionASMXSoapClient : System.ServiceModel.ClientBase<SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.wcfRecepcionASMXSoap>, SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.wcfRecepcionASMXSoap {
        
        public wcfRecepcionASMXSoapClient() {
        }
        
        public wcfRecepcionASMXSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public wcfRecepcionASMXSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wcfRecepcionASMXSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wcfRecepcionASMXSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.fnEnviarXMLResponse SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.wcfRecepcionASMXSoap.fnEnviarXML(SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.fnEnviarXMLRequest request) {
            return base.Channel.fnEnviarXML(request);
        }
        
        public string fnEnviarXML(string psComprobante, int psCveRetencion, int pnId_Estructura, string sNombre, string sContrasena, string sVersion) {
            SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.fnEnviarXMLRequest inValue = new SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.fnEnviarXMLRequest();
            inValue.Body = new SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.fnEnviarXMLRequestBody();
            inValue.Body.psComprobante = psComprobante;
            inValue.Body.psCveRetencion = psCveRetencion;
            inValue.Body.pnId_Estructura = pnId_Estructura;
            inValue.Body.sNombre = sNombre;
            inValue.Body.sContrasena = sContrasena;
            inValue.Body.sVersion = sVersion;
            SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.fnEnviarXMLResponse retVal = ((SolucionPruebas.Presentacion.Servicios.wsRecepcionRetencionProduccion.wcfRecepcionASMXSoap)(this)).fnEnviarXML(inValue);
            return retVal.Body.fnEnviarXMLResult;
        }
    }
}

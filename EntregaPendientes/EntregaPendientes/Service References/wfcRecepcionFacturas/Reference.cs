﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.296
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntregaPendientes.wfcRecepcionFacturas {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ArrayOfString", Namespace="www.paxfacturacion.com", ItemName="string")]
    [System.SerializableAttribute()]
    public class ArrayOfString : System.Collections.Generic.List<string> {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="www.paxfacturacion.com", ConfigurationName="wfcRecepcionFacturas.ServiceNadroSoap")]
    public interface ServiceNadroSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento sNombre del espacio de nombres www.paxfacturacion.com no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="www.paxfacturacion.com/fnRecibeFacturas", ReplyAction="*")]
        EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasResponse fnRecibeFacturas(EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento xmlArray del espacio de nombres www.paxfacturacion.com no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="www.paxfacturacion.com/fnRecibeFacturasArray", ReplyAction="*")]
        EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasArrayResponse fnRecibeFacturasArray(EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasArrayRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnRecibeFacturasRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnRecibeFacturas", Namespace="www.paxfacturacion.com", Order=0)]
        public EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasRequestBody Body;
        
        public fnRecibeFacturasRequest() {
        }
        
        public fnRecibeFacturasRequest(EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="www.paxfacturacion.com")]
    public partial class fnRecibeFacturasRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string sNombre;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string sContrasena;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string hash_peticion;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string hash_timbrado;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string xml_timbrado;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string tipo_documento;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=6)]
        public int id_estructura;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string fecha;
        
        public fnRecibeFacturasRequestBody() {
        }
        
        public fnRecibeFacturasRequestBody(string sNombre, string sContrasena, string hash_peticion, string hash_timbrado, string xml_timbrado, string tipo_documento, int id_estructura, string fecha) {
            this.sNombre = sNombre;
            this.sContrasena = sContrasena;
            this.hash_peticion = hash_peticion;
            this.hash_timbrado = hash_timbrado;
            this.xml_timbrado = xml_timbrado;
            this.tipo_documento = tipo_documento;
            this.id_estructura = id_estructura;
            this.fecha = fecha;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnRecibeFacturasResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnRecibeFacturasResponse", Namespace="www.paxfacturacion.com", Order=0)]
        public EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasResponseBody Body;
        
        public fnRecibeFacturasResponse() {
        }
        
        public fnRecibeFacturasResponse(EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="www.paxfacturacion.com")]
    public partial class fnRecibeFacturasResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool fnRecibeFacturasResult;
        
        public fnRecibeFacturasResponseBody() {
        }
        
        public fnRecibeFacturasResponseBody(bool fnRecibeFacturasResult) {
            this.fnRecibeFacturasResult = fnRecibeFacturasResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnRecibeFacturasArrayRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnRecibeFacturasArray", Namespace="www.paxfacturacion.com", Order=0)]
        public EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasArrayRequestBody Body;
        
        public fnRecibeFacturasArrayRequest() {
        }
        
        public fnRecibeFacturasArrayRequest(EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasArrayRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="www.paxfacturacion.com")]
    public partial class fnRecibeFacturasArrayRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public EntregaPendientes.wfcRecepcionFacturas.ArrayOfString xmlArray;
        
        public fnRecibeFacturasArrayRequestBody() {
        }
        
        public fnRecibeFacturasArrayRequestBody(EntregaPendientes.wfcRecepcionFacturas.ArrayOfString xmlArray) {
            this.xmlArray = xmlArray;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnRecibeFacturasArrayResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnRecibeFacturasArrayResponse", Namespace="www.paxfacturacion.com", Order=0)]
        public EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasArrayResponseBody Body;
        
        public fnRecibeFacturasArrayResponse() {
        }
        
        public fnRecibeFacturasArrayResponse(EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasArrayResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="www.paxfacturacion.com")]
    public partial class fnRecibeFacturasArrayResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool fnRecibeFacturasArrayResult;
        
        public fnRecibeFacturasArrayResponseBody() {
        }
        
        public fnRecibeFacturasArrayResponseBody(bool fnRecibeFacturasArrayResult) {
            this.fnRecibeFacturasArrayResult = fnRecibeFacturasArrayResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServiceNadroSoapChannel : EntregaPendientes.wfcRecepcionFacturas.ServiceNadroSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceNadroSoapClient : System.ServiceModel.ClientBase<EntregaPendientes.wfcRecepcionFacturas.ServiceNadroSoap>, EntregaPendientes.wfcRecepcionFacturas.ServiceNadroSoap {
        
        public ServiceNadroSoapClient() {
        }
        
        public ServiceNadroSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceNadroSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceNadroSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceNadroSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasResponse EntregaPendientes.wfcRecepcionFacturas.ServiceNadroSoap.fnRecibeFacturas(EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasRequest request) {
            return base.Channel.fnRecibeFacturas(request);
        }
        
        public bool fnRecibeFacturas(string sNombre, string sContrasena, string hash_peticion, string hash_timbrado, string xml_timbrado, string tipo_documento, int id_estructura, string fecha) {
            EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasRequest inValue = new EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasRequest();
            inValue.Body = new EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasRequestBody();
            inValue.Body.sNombre = sNombre;
            inValue.Body.sContrasena = sContrasena;
            inValue.Body.hash_peticion = hash_peticion;
            inValue.Body.hash_timbrado = hash_timbrado;
            inValue.Body.xml_timbrado = xml_timbrado;
            inValue.Body.tipo_documento = tipo_documento;
            inValue.Body.id_estructura = id_estructura;
            inValue.Body.fecha = fecha;
            EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasResponse retVal = ((EntregaPendientes.wfcRecepcionFacturas.ServiceNadroSoap)(this)).fnRecibeFacturas(inValue);
            return retVal.Body.fnRecibeFacturasResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasArrayResponse EntregaPendientes.wfcRecepcionFacturas.ServiceNadroSoap.fnRecibeFacturasArray(EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasArrayRequest request) {
            return base.Channel.fnRecibeFacturasArray(request);
        }
        
        public bool fnRecibeFacturasArray(EntregaPendientes.wfcRecepcionFacturas.ArrayOfString xmlArray) {
            EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasArrayRequest inValue = new EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasArrayRequest();
            inValue.Body = new EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasArrayRequestBody();
            inValue.Body.xmlArray = xmlArray;
            EntregaPendientes.wfcRecepcionFacturas.fnRecibeFacturasArrayResponse retVal = ((EntregaPendientes.wfcRecepcionFacturas.ServiceNadroSoap)(this)).fnRecibeFacturasArray(inValue);
            return retVal.Body.fnRecibeFacturasArrayResult;
        }
    }
}

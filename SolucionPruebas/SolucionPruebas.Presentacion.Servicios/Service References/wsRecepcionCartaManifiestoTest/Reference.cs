﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ArrayOfString", Namespace="https://test.paxfacturacion.com.mx:467", ItemName="string")]
    [System.SerializableAttribute()]
    public class ArrayOfString : System.Collections.Generic.List<string> {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://test.paxfacturacion.com.mx:467", ConfigurationName="wsRecepcionCartaManifiestoTest.wcfRecepcionManifiestoSoap")]
    public interface wcfRecepcionManifiestoSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento psCartaManifiesto del espacio de nombres https://test.paxfacturacion.com.mx:467 no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="https://test.paxfacturacion.com.mx:467/fnEnviarManifiestoXML", ReplyAction="*")]
        SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoXMLResponse fnEnviarManifiestoXML(SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoXMLRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento pbDatos del espacio de nombres https://test.paxfacturacion.com.mx:467 no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="https://test.paxfacturacion.com.mx:467/fnEnviarManifiestoDatos", ReplyAction="*")]
        SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoDatosResponse fnEnviarManifiestoDatos(SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoDatosRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnEnviarManifiestoXMLRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnEnviarManifiestoXML", Namespace="https://test.paxfacturacion.com.mx:467", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoXMLRequestBody Body;
        
        public fnEnviarManifiestoXMLRequest() {
        }
        
        public fnEnviarManifiestoXMLRequest(SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoXMLRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://test.paxfacturacion.com.mx:467")]
    public partial class fnEnviarManifiestoXMLRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string psCartaManifiesto;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public byte[] pbPfx;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string psPasswordPfx;
        
        public fnEnviarManifiestoXMLRequestBody() {
        }
        
        public fnEnviarManifiestoXMLRequestBody(string psCartaManifiesto, byte[] pbPfx, string psPasswordPfx) {
            this.psCartaManifiesto = psCartaManifiesto;
            this.pbPfx = pbPfx;
            this.psPasswordPfx = psPasswordPfx;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnEnviarManifiestoXMLResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnEnviarManifiestoXMLResponse", Namespace="https://test.paxfacturacion.com.mx:467", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoXMLResponseBody Body;
        
        public fnEnviarManifiestoXMLResponse() {
        }
        
        public fnEnviarManifiestoXMLResponse(SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoXMLResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://test.paxfacturacion.com.mx:467")]
    public partial class fnEnviarManifiestoXMLResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fnEnviarManifiestoXMLResult;
        
        public fnEnviarManifiestoXMLResponseBody() {
        }
        
        public fnEnviarManifiestoXMLResponseBody(string fnEnviarManifiestoXMLResult) {
            this.fnEnviarManifiestoXMLResult = fnEnviarManifiestoXMLResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnEnviarManifiestoDatosRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnEnviarManifiestoDatos", Namespace="https://test.paxfacturacion.com.mx:467", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoDatosRequestBody Body;
        
        public fnEnviarManifiestoDatosRequest() {
        }
        
        public fnEnviarManifiestoDatosRequest(SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoDatosRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://test.paxfacturacion.com.mx:467")]
    public partial class fnEnviarManifiestoDatosRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.ArrayOfString pbDatos;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public byte[] pbPfx;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string psPasswordPfx;
        
        public fnEnviarManifiestoDatosRequestBody() {
        }
        
        public fnEnviarManifiestoDatosRequestBody(SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.ArrayOfString pbDatos, byte[] pbPfx, string psPasswordPfx) {
            this.pbDatos = pbDatos;
            this.pbPfx = pbPfx;
            this.psPasswordPfx = psPasswordPfx;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnEnviarManifiestoDatosResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnEnviarManifiestoDatosResponse", Namespace="https://test.paxfacturacion.com.mx:467", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoDatosResponseBody Body;
        
        public fnEnviarManifiestoDatosResponse() {
        }
        
        public fnEnviarManifiestoDatosResponse(SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoDatosResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://test.paxfacturacion.com.mx:467")]
    public partial class fnEnviarManifiestoDatosResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fnEnviarManifiestoDatosResult;
        
        public fnEnviarManifiestoDatosResponseBody() {
        }
        
        public fnEnviarManifiestoDatosResponseBody(string fnEnviarManifiestoDatosResult) {
            this.fnEnviarManifiestoDatosResult = fnEnviarManifiestoDatosResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface wcfRecepcionManifiestoSoapChannel : SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.wcfRecepcionManifiestoSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wcfRecepcionManifiestoSoapClient : System.ServiceModel.ClientBase<SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.wcfRecepcionManifiestoSoap>, SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.wcfRecepcionManifiestoSoap {
        
        public wcfRecepcionManifiestoSoapClient() {
        }
        
        public wcfRecepcionManifiestoSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public wcfRecepcionManifiestoSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wcfRecepcionManifiestoSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wcfRecepcionManifiestoSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoXMLResponse SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.wcfRecepcionManifiestoSoap.fnEnviarManifiestoXML(SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoXMLRequest request) {
            return base.Channel.fnEnviarManifiestoXML(request);
        }
        
        public string fnEnviarManifiestoXML(string psCartaManifiesto, byte[] pbPfx, string psPasswordPfx) {
            SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoXMLRequest inValue = new SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoXMLRequest();
            inValue.Body = new SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoXMLRequestBody();
            inValue.Body.psCartaManifiesto = psCartaManifiesto;
            inValue.Body.pbPfx = pbPfx;
            inValue.Body.psPasswordPfx = psPasswordPfx;
            SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoXMLResponse retVal = ((SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.wcfRecepcionManifiestoSoap)(this)).fnEnviarManifiestoXML(inValue);
            return retVal.Body.fnEnviarManifiestoXMLResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoDatosResponse SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.wcfRecepcionManifiestoSoap.fnEnviarManifiestoDatos(SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoDatosRequest request) {
            return base.Channel.fnEnviarManifiestoDatos(request);
        }
        
        public string fnEnviarManifiestoDatos(SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.ArrayOfString pbDatos, byte[] pbPfx, string psPasswordPfx) {
            SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoDatosRequest inValue = new SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoDatosRequest();
            inValue.Body = new SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoDatosRequestBody();
            inValue.Body.pbDatos = pbDatos;
            inValue.Body.pbPfx = pbPfx;
            inValue.Body.psPasswordPfx = psPasswordPfx;
            SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.fnEnviarManifiestoDatosResponse retVal = ((SolucionPruebas.Presentacion.Servicios.wsRecepcionCartaManifiestoTest.wcfRecepcionManifiestoSoap)(this)).fnEnviarManifiestoDatos(inValue);
            return retVal.Body.fnEnviarManifiestoDatosResult;
        }
    }
}

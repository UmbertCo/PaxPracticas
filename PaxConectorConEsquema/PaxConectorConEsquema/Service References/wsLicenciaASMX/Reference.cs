﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaxConectorConEsquema.wsLicenciaASMX {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://192.168.2.13:86", ConfigurationName="wsLicenciaASMX.wsLicenciaASMXSoap")]
    public interface wsLicenciaASMXSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento pidUsuario del espacio de nombres http://192.168.2.13:86 no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.2.13:86/fnObtenerLlaveVersionUsuario", ReplyAction="*")]
        PaxConectorConEsquema.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioResponse fnObtenerLlaveVersionUsuario(PaxConectorConEsquema.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento nVersion del espacio de nombres http://192.168.2.13:86 no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.2.13:86/fnInsertarLlaveVersionUsuario", ReplyAction="*")]
        PaxConectorConEsquema.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioResponse fnInsertarLlaveVersionUsuario(PaxConectorConEsquema.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento sclaveusuario del espacio de nombres http://192.168.2.13:86 no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.2.13:86/fnObtieneUsuario", ReplyAction="*")]
        PaxConectorConEsquema.wsLicenciaASMX.fnObtieneUsuarioResponse fnObtieneUsuario(PaxConectorConEsquema.wsLicenciaASMX.fnObtieneUsuarioRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnObtenerLlaveVersionUsuarioRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnObtenerLlaveVersionUsuario", Namespace="http://192.168.2.13:86", Order=0)]
        public PaxConectorConEsquema.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequestBody Body;
        
        public fnObtenerLlaveVersionUsuarioRequest() {
        }
        
        public fnObtenerLlaveVersionUsuarioRequest(PaxConectorConEsquema.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.2.13:86")]
    public partial class fnObtenerLlaveVersionUsuarioRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string pidUsuario;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string nVersion;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string nOrigen;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public System.DateTime sFecha;
        
        public fnObtenerLlaveVersionUsuarioRequestBody() {
        }
        
        public fnObtenerLlaveVersionUsuarioRequestBody(string pidUsuario, string nVersion, string nOrigen, System.DateTime sFecha) {
            this.pidUsuario = pidUsuario;
            this.nVersion = nVersion;
            this.nOrigen = nOrigen;
            this.sFecha = sFecha;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnObtenerLlaveVersionUsuarioResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnObtenerLlaveVersionUsuarioResponse", Namespace="http://192.168.2.13:86", Order=0)]
        public PaxConectorConEsquema.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioResponseBody Body;
        
        public fnObtenerLlaveVersionUsuarioResponse() {
        }
        
        public fnObtenerLlaveVersionUsuarioResponse(PaxConectorConEsquema.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.2.13:86")]
    public partial class fnObtenerLlaveVersionUsuarioResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fnObtenerLlaveVersionUsuarioResult;
        
        public fnObtenerLlaveVersionUsuarioResponseBody() {
        }
        
        public fnObtenerLlaveVersionUsuarioResponseBody(string fnObtenerLlaveVersionUsuarioResult) {
            this.fnObtenerLlaveVersionUsuarioResult = fnObtenerLlaveVersionUsuarioResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnInsertarLlaveVersionUsuarioRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnInsertarLlaveVersionUsuario", Namespace="http://192.168.2.13:86", Order=0)]
        public PaxConectorConEsquema.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequestBody Body;
        
        public fnInsertarLlaveVersionUsuarioRequest() {
        }
        
        public fnInsertarLlaveVersionUsuarioRequest(PaxConectorConEsquema.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.2.13:86")]
    public partial class fnInsertarLlaveVersionUsuarioRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int pidUsuario;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string nVersion;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string nOrigen;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public System.DateTime sFecha;
        
        public fnInsertarLlaveVersionUsuarioRequestBody() {
        }
        
        public fnInsertarLlaveVersionUsuarioRequestBody(int pidUsuario, string nVersion, string nOrigen, System.DateTime sFecha) {
            this.pidUsuario = pidUsuario;
            this.nVersion = nVersion;
            this.nOrigen = nOrigen;
            this.sFecha = sFecha;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnInsertarLlaveVersionUsuarioResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnInsertarLlaveVersionUsuarioResponse", Namespace="http://192.168.2.13:86", Order=0)]
        public PaxConectorConEsquema.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioResponseBody Body;
        
        public fnInsertarLlaveVersionUsuarioResponse() {
        }
        
        public fnInsertarLlaveVersionUsuarioResponse(PaxConectorConEsquema.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.2.13:86")]
    public partial class fnInsertarLlaveVersionUsuarioResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string fnInsertarLlaveVersionUsuarioResult;
        
        public fnInsertarLlaveVersionUsuarioResponseBody() {
        }
        
        public fnInsertarLlaveVersionUsuarioResponseBody(string fnInsertarLlaveVersionUsuarioResult) {
            this.fnInsertarLlaveVersionUsuarioResult = fnInsertarLlaveVersionUsuarioResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnObtieneUsuarioRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnObtieneUsuario", Namespace="http://192.168.2.13:86", Order=0)]
        public PaxConectorConEsquema.wsLicenciaASMX.fnObtieneUsuarioRequestBody Body;
        
        public fnObtieneUsuarioRequest() {
        }
        
        public fnObtieneUsuarioRequest(PaxConectorConEsquema.wsLicenciaASMX.fnObtieneUsuarioRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.2.13:86")]
    public partial class fnObtieneUsuarioRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string sclaveusuario;
        
        public fnObtieneUsuarioRequestBody() {
        }
        
        public fnObtieneUsuarioRequestBody(string sclaveusuario) {
            this.sclaveusuario = sclaveusuario;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnObtieneUsuarioResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnObtieneUsuarioResponse", Namespace="http://192.168.2.13:86", Order=0)]
        public PaxConectorConEsquema.wsLicenciaASMX.fnObtieneUsuarioResponseBody Body;
        
        public fnObtieneUsuarioResponse() {
        }
        
        public fnObtieneUsuarioResponse(PaxConectorConEsquema.wsLicenciaASMX.fnObtieneUsuarioResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://192.168.2.13:86")]
    public partial class fnObtieneUsuarioResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int fnObtieneUsuarioResult;
        
        public fnObtieneUsuarioResponseBody() {
        }
        
        public fnObtieneUsuarioResponseBody(int fnObtieneUsuarioResult) {
            this.fnObtieneUsuarioResult = fnObtieneUsuarioResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface wsLicenciaASMXSoapChannel : PaxConectorConEsquema.wsLicenciaASMX.wsLicenciaASMXSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wsLicenciaASMXSoapClient : System.ServiceModel.ClientBase<PaxConectorConEsquema.wsLicenciaASMX.wsLicenciaASMXSoap>, PaxConectorConEsquema.wsLicenciaASMX.wsLicenciaASMXSoap {
        
        public wsLicenciaASMXSoapClient() {
        }
        
        public wsLicenciaASMXSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public wsLicenciaASMXSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wsLicenciaASMXSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wsLicenciaASMXSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        PaxConectorConEsquema.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioResponse PaxConectorConEsquema.wsLicenciaASMX.wsLicenciaASMXSoap.fnObtenerLlaveVersionUsuario(PaxConectorConEsquema.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequest request) {
            return base.Channel.fnObtenerLlaveVersionUsuario(request);
        }
        
        public string fnObtenerLlaveVersionUsuario(string pidUsuario, string nVersion, string nOrigen, System.DateTime sFecha) {
            PaxConectorConEsquema.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequest inValue = new PaxConectorConEsquema.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequest();
            inValue.Body = new PaxConectorConEsquema.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequestBody();
            inValue.Body.pidUsuario = pidUsuario;
            inValue.Body.nVersion = nVersion;
            inValue.Body.nOrigen = nOrigen;
            inValue.Body.sFecha = sFecha;
            PaxConectorConEsquema.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioResponse retVal = ((PaxConectorConEsquema.wsLicenciaASMX.wsLicenciaASMXSoap)(this)).fnObtenerLlaveVersionUsuario(inValue);
            return retVal.Body.fnObtenerLlaveVersionUsuarioResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        PaxConectorConEsquema.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioResponse PaxConectorConEsquema.wsLicenciaASMX.wsLicenciaASMXSoap.fnInsertarLlaveVersionUsuario(PaxConectorConEsquema.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequest request) {
            return base.Channel.fnInsertarLlaveVersionUsuario(request);
        }
        
        public string fnInsertarLlaveVersionUsuario(int pidUsuario, string nVersion, string nOrigen, System.DateTime sFecha) {
            PaxConectorConEsquema.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequest inValue = new PaxConectorConEsquema.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequest();
            inValue.Body = new PaxConectorConEsquema.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequestBody();
            inValue.Body.pidUsuario = pidUsuario;
            inValue.Body.nVersion = nVersion;
            inValue.Body.nOrigen = nOrigen;
            inValue.Body.sFecha = sFecha;
            PaxConectorConEsquema.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioResponse retVal = ((PaxConectorConEsquema.wsLicenciaASMX.wsLicenciaASMXSoap)(this)).fnInsertarLlaveVersionUsuario(inValue);
            return retVal.Body.fnInsertarLlaveVersionUsuarioResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        PaxConectorConEsquema.wsLicenciaASMX.fnObtieneUsuarioResponse PaxConectorConEsquema.wsLicenciaASMX.wsLicenciaASMXSoap.fnObtieneUsuario(PaxConectorConEsquema.wsLicenciaASMX.fnObtieneUsuarioRequest request) {
            return base.Channel.fnObtieneUsuario(request);
        }
        
        public int fnObtieneUsuario(string sclaveusuario) {
            PaxConectorConEsquema.wsLicenciaASMX.fnObtieneUsuarioRequest inValue = new PaxConectorConEsquema.wsLicenciaASMX.fnObtieneUsuarioRequest();
            inValue.Body = new PaxConectorConEsquema.wsLicenciaASMX.fnObtieneUsuarioRequestBody();
            inValue.Body.sclaveusuario = sclaveusuario;
            PaxConectorConEsquema.wsLicenciaASMX.fnObtieneUsuarioResponse retVal = ((PaxConectorConEsquema.wsLicenciaASMX.wsLicenciaASMXSoap)(this)).fnObtieneUsuario(inValue);
            return retVal.Body.fnObtieneUsuarioResult;
        }
    }
}

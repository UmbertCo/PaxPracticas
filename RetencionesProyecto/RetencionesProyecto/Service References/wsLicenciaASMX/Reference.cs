﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RetencionesProyecto.wsLicenciaASMX {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://192.168.2.13:86", ConfigurationName="wsLicenciaASMX.wsLicenciaASMXSoap")]
    public interface wsLicenciaASMXSoap {
        
        // CODEGEN: Generating message contract since element name pidUsuario from namespace http://192.168.2.13:86 is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.2.13:86/fnObtenerLlaveVersionUsuario", ReplyAction="*")]
        RetencionesProyecto.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioResponse fnObtenerLlaveVersionUsuario(RetencionesProyecto.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequest request);
        
        // CODEGEN: Generating message contract since element name nVersion from namespace http://192.168.2.13:86 is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.2.13:86/fnInsertarLlaveVersionUsuario", ReplyAction="*")]
        RetencionesProyecto.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioResponse fnInsertarLlaveVersionUsuario(RetencionesProyecto.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequest request);
        
        // CODEGEN: Generating message contract since element name sclaveusuario from namespace http://192.168.2.13:86 is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://192.168.2.13:86/fnObtieneUsuario", ReplyAction="*")]
        RetencionesProyecto.wsLicenciaASMX.fnObtieneUsuarioResponse fnObtieneUsuario(RetencionesProyecto.wsLicenciaASMX.fnObtieneUsuarioRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class fnObtenerLlaveVersionUsuarioRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="fnObtenerLlaveVersionUsuario", Namespace="http://192.168.2.13:86", Order=0)]
        public RetencionesProyecto.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequestBody Body;
        
        public fnObtenerLlaveVersionUsuarioRequest() {
        }
        
        public fnObtenerLlaveVersionUsuarioRequest(RetencionesProyecto.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequestBody Body) {
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
        public RetencionesProyecto.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioResponseBody Body;
        
        public fnObtenerLlaveVersionUsuarioResponse() {
        }
        
        public fnObtenerLlaveVersionUsuarioResponse(RetencionesProyecto.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioResponseBody Body) {
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
        public RetencionesProyecto.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequestBody Body;
        
        public fnInsertarLlaveVersionUsuarioRequest() {
        }
        
        public fnInsertarLlaveVersionUsuarioRequest(RetencionesProyecto.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequestBody Body) {
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
        public RetencionesProyecto.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioResponseBody Body;
        
        public fnInsertarLlaveVersionUsuarioResponse() {
        }
        
        public fnInsertarLlaveVersionUsuarioResponse(RetencionesProyecto.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioResponseBody Body) {
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
        public RetencionesProyecto.wsLicenciaASMX.fnObtieneUsuarioRequestBody Body;
        
        public fnObtieneUsuarioRequest() {
        }
        
        public fnObtieneUsuarioRequest(RetencionesProyecto.wsLicenciaASMX.fnObtieneUsuarioRequestBody Body) {
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
        public RetencionesProyecto.wsLicenciaASMX.fnObtieneUsuarioResponseBody Body;
        
        public fnObtieneUsuarioResponse() {
        }
        
        public fnObtieneUsuarioResponse(RetencionesProyecto.wsLicenciaASMX.fnObtieneUsuarioResponseBody Body) {
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
    public interface wsLicenciaASMXSoapChannel : RetencionesProyecto.wsLicenciaASMX.wsLicenciaASMXSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wsLicenciaASMXSoapClient : System.ServiceModel.ClientBase<RetencionesProyecto.wsLicenciaASMX.wsLicenciaASMXSoap>, RetencionesProyecto.wsLicenciaASMX.wsLicenciaASMXSoap {
        
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
        RetencionesProyecto.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioResponse RetencionesProyecto.wsLicenciaASMX.wsLicenciaASMXSoap.fnObtenerLlaveVersionUsuario(RetencionesProyecto.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequest request) {
            return base.Channel.fnObtenerLlaveVersionUsuario(request);
        }
        
        public string fnObtenerLlaveVersionUsuario(string pidUsuario, string nVersion, string nOrigen, System.DateTime sFecha) {
            RetencionesProyecto.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequest inValue = new RetencionesProyecto.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequest();
            inValue.Body = new RetencionesProyecto.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioRequestBody();
            inValue.Body.pidUsuario = pidUsuario;
            inValue.Body.nVersion = nVersion;
            inValue.Body.nOrigen = nOrigen;
            inValue.Body.sFecha = sFecha;
            RetencionesProyecto.wsLicenciaASMX.fnObtenerLlaveVersionUsuarioResponse retVal = ((RetencionesProyecto.wsLicenciaASMX.wsLicenciaASMXSoap)(this)).fnObtenerLlaveVersionUsuario(inValue);
            return retVal.Body.fnObtenerLlaveVersionUsuarioResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RetencionesProyecto.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioResponse RetencionesProyecto.wsLicenciaASMX.wsLicenciaASMXSoap.fnInsertarLlaveVersionUsuario(RetencionesProyecto.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequest request) {
            return base.Channel.fnInsertarLlaveVersionUsuario(request);
        }
        
        public string fnInsertarLlaveVersionUsuario(int pidUsuario, string nVersion, string nOrigen, System.DateTime sFecha) {
            RetencionesProyecto.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequest inValue = new RetencionesProyecto.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequest();
            inValue.Body = new RetencionesProyecto.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioRequestBody();
            inValue.Body.pidUsuario = pidUsuario;
            inValue.Body.nVersion = nVersion;
            inValue.Body.nOrigen = nOrigen;
            inValue.Body.sFecha = sFecha;
            RetencionesProyecto.wsLicenciaASMX.fnInsertarLlaveVersionUsuarioResponse retVal = ((RetencionesProyecto.wsLicenciaASMX.wsLicenciaASMXSoap)(this)).fnInsertarLlaveVersionUsuario(inValue);
            return retVal.Body.fnInsertarLlaveVersionUsuarioResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RetencionesProyecto.wsLicenciaASMX.fnObtieneUsuarioResponse RetencionesProyecto.wsLicenciaASMX.wsLicenciaASMXSoap.fnObtieneUsuario(RetencionesProyecto.wsLicenciaASMX.fnObtieneUsuarioRequest request) {
            return base.Channel.fnObtieneUsuario(request);
        }
        
        public int fnObtieneUsuario(string sclaveusuario) {
            RetencionesProyecto.wsLicenciaASMX.fnObtieneUsuarioRequest inValue = new RetencionesProyecto.wsLicenciaASMX.fnObtieneUsuarioRequest();
            inValue.Body = new RetencionesProyecto.wsLicenciaASMX.fnObtieneUsuarioRequestBody();
            inValue.Body.sclaveusuario = sclaveusuario;
            RetencionesProyecto.wsLicenciaASMX.fnObtieneUsuarioResponse retVal = ((RetencionesProyecto.wsLicenciaASMX.wsLicenciaASMXSoap)(this)).fnObtieneUsuario(inValue);
            return retVal.Body.fnObtieneUsuarioResult;
        }
    }
}

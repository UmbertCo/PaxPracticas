﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.36366
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WSConsultaAlSuper.wcfConsultaTicketSoap")]
    public interface wcfConsultaTicketSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento no_ticket del espacio de nombres http://tempuri.org/ no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ConsultaTicket", ReplyAction="*")]
        SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.ConsultaTicketResponse ConsultaTicket(SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.ConsultaTicketRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento psFolio del espacio de nombres http://tempuri.org/ no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RegistraTicket", ReplyAction="*")]
        SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.RegistraTicketResponse RegistraTicket(SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.RegistraTicketRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ConsultaTicketRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ConsultaTicket", Namespace="http://tempuri.org/", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.ConsultaTicketRequestBody Body;
        
        public ConsultaTicketRequest() {
        }
        
        public ConsultaTicketRequest(SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.ConsultaTicketRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ConsultaTicketRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string no_ticket;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int no_tienda;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public int pto_venta;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string fecha_ticket;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=4)]
        public double total_ticket;
        
        public ConsultaTicketRequestBody() {
        }
        
        public ConsultaTicketRequestBody(string no_ticket, int no_tienda, int pto_venta, string fecha_ticket, double total_ticket) {
            this.no_ticket = no_ticket;
            this.no_tienda = no_tienda;
            this.pto_venta = pto_venta;
            this.fecha_ticket = fecha_ticket;
            this.total_ticket = total_ticket;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ConsultaTicketResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ConsultaTicketResponse", Namespace="http://tempuri.org/", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.ConsultaTicketResponseBody Body;
        
        public ConsultaTicketResponse() {
        }
        
        public ConsultaTicketResponse(SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.ConsultaTicketResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ConsultaTicketResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ConsultaTicketResult;
        
        public ConsultaTicketResponseBody() {
        }
        
        public ConsultaTicketResponseBody(string ConsultaTicketResult) {
            this.ConsultaTicketResult = ConsultaTicketResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RegistraTicketRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RegistraTicket", Namespace="http://tempuri.org/", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.RegistraTicketRequestBody Body;
        
        public RegistraTicketRequest() {
        }
        
        public RegistraTicketRequest(SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.RegistraTicketRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class RegistraTicketRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string psFolio;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int pnNoTienda;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public int pnPtoVenta;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string psFechaTicket;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=4)]
        public double pnTotalTicket;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string psFolioInterno;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string psUUID;
        
        public RegistraTicketRequestBody() {
        }
        
        public RegistraTicketRequestBody(string psFolio, int pnNoTienda, int pnPtoVenta, string psFechaTicket, double pnTotalTicket, string psFolioInterno, string psUUID) {
            this.psFolio = psFolio;
            this.pnNoTienda = pnNoTienda;
            this.pnPtoVenta = pnPtoVenta;
            this.psFechaTicket = psFechaTicket;
            this.pnTotalTicket = pnTotalTicket;
            this.psFolioInterno = psFolioInterno;
            this.psUUID = psUUID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RegistraTicketResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RegistraTicketResponse", Namespace="http://tempuri.org/", Order=0)]
        public SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.RegistraTicketResponseBody Body;
        
        public RegistraTicketResponse() {
        }
        
        public RegistraTicketResponse(SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.RegistraTicketResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class RegistraTicketResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool RegistraTicketResult;
        
        public RegistraTicketResponseBody() {
        }
        
        public RegistraTicketResponseBody(bool RegistraTicketResult) {
            this.RegistraTicketResult = RegistraTicketResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface wcfConsultaTicketSoapChannel : SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.wcfConsultaTicketSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class wcfConsultaTicketSoapClient : System.ServiceModel.ClientBase<SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.wcfConsultaTicketSoap>, SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.wcfConsultaTicketSoap {
        
        public wcfConsultaTicketSoapClient() {
        }
        
        public wcfConsultaTicketSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public wcfConsultaTicketSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wcfConsultaTicketSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public wcfConsultaTicketSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.ConsultaTicketResponse SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.wcfConsultaTicketSoap.ConsultaTicket(SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.ConsultaTicketRequest request) {
            return base.Channel.ConsultaTicket(request);
        }
        
        public string ConsultaTicket(string no_ticket, int no_tienda, int pto_venta, string fecha_ticket, double total_ticket) {
            SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.ConsultaTicketRequest inValue = new SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.ConsultaTicketRequest();
            inValue.Body = new SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.ConsultaTicketRequestBody();
            inValue.Body.no_ticket = no_ticket;
            inValue.Body.no_tienda = no_tienda;
            inValue.Body.pto_venta = pto_venta;
            inValue.Body.fecha_ticket = fecha_ticket;
            inValue.Body.total_ticket = total_ticket;
            SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.ConsultaTicketResponse retVal = ((SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.wcfConsultaTicketSoap)(this)).ConsultaTicket(inValue);
            return retVal.Body.ConsultaTicketResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.RegistraTicketResponse SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.wcfConsultaTicketSoap.RegistraTicket(SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.RegistraTicketRequest request) {
            return base.Channel.RegistraTicket(request);
        }
        
        public bool RegistraTicket(string psFolio, int pnNoTienda, int pnPtoVenta, string psFechaTicket, double pnTotalTicket, string psFolioInterno, string psUUID) {
            SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.RegistraTicketRequest inValue = new SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.RegistraTicketRequest();
            inValue.Body = new SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.RegistraTicketRequestBody();
            inValue.Body.psFolio = psFolio;
            inValue.Body.pnNoTienda = pnNoTienda;
            inValue.Body.pnPtoVenta = pnPtoVenta;
            inValue.Body.psFechaTicket = psFechaTicket;
            inValue.Body.pnTotalTicket = pnTotalTicket;
            inValue.Body.psFolioInterno = psFolioInterno;
            inValue.Body.psUUID = psUUID;
            SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.RegistraTicketResponse retVal = ((SolucionPruebas.Presentacion.Servicios.WSConsultaAlSuper.wcfConsultaTicketSoap)(this)).RegistraTicket(inValue);
            return retVal.Body.RegistraTicketResult;
        }
    }
}
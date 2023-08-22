﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.7905
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/nomina")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.sat.gob.mx/nomina", IsNullable=false)]
public partial class Nomina {
    
    private NominaPercepciones percepcionesField;
    
    private NominaDeducciones deduccionesField;
    
    private NominaIncapacidad[] incapacidadesField;
    
    private NominaHorasExtra[] horasExtrasField;
    
    private string versionField;
    
    private string registroPatronalField;
    
    private string numEmpleadoField;
    
    private string cURPField;
    
    private int tipoRegimenField;
    
    private string numSeguridadSocialField;
    
    private System.DateTime fechaPagoField;
    
    private System.DateTime fechaInicialPagoField;
    
    private System.DateTime fechaFinalPagoField;
    
    private decimal numDiasPagadosField;
    
    private string departamentoField;
    
    private string cLABEField;
    
    private string bancoField;
    
    private bool bancoFieldSpecified;
    
    private System.DateTime fechaInicioRelLaboralField;
    
    private bool fechaInicioRelLaboralFieldSpecified;
    
    private int antiguedadField;
    
    private bool antiguedadFieldSpecified;
    
    private string puestoField;
    
    private string tipoContratoField;
    
    private string tipoJornadaField;
    
    private string periodicidadPagoField;
    
    private decimal salarioBaseCotAporField;
    
    private bool salarioBaseCotAporFieldSpecified;
    
    private int riesgoPuestoField;
    
    private bool riesgoPuestoFieldSpecified;
    
    private decimal salarioDiarioIntegradoField;
    
    private bool salarioDiarioIntegradoFieldSpecified;
    
    public Nomina() {
        this.versionField = "1.1";
    }
    
    /// <remarks/>
    public NominaPercepciones Percepciones {
        get {
            return this.percepcionesField;
        }
        set {
            this.percepcionesField = value;
        }
    }
    
    /// <remarks/>
    public NominaDeducciones Deducciones {
        get {
            return this.deduccionesField;
        }
        set {
            this.deduccionesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Incapacidad", IsNullable=false)]
    public NominaIncapacidad[] Incapacidades {
        get {
            return this.incapacidadesField;
        }
        set {
            this.incapacidadesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("HorasExtra", IsNullable=false)]
    public NominaHorasExtra[] HorasExtras {
        get {
            return this.horasExtrasField;
        }
        set {
            this.horasExtrasField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Version {
        get {
            return this.versionField;
        }
        set {
            this.versionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string RegistroPatronal {
        get {
            return this.registroPatronalField;
        }
        set {
            this.registroPatronalField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumEmpleado {
        get {
            return this.numEmpleadoField;
        }
        set {
            this.numEmpleadoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CURP {
        get {
            return this.cURPField;
        }
        set {
            this.cURPField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int TipoRegimen {
        get {
            return this.tipoRegimenField;
        }
        set {
            this.tipoRegimenField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumSeguridadSocial {
        get {
            return this.numSeguridadSocialField;
        }
        set {
            this.numSeguridadSocialField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
    public System.DateTime FechaPago {
        get {
            return this.fechaPagoField;
        }
        set {
            this.fechaPagoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
    public System.DateTime FechaInicialPago {
        get {
            return this.fechaInicialPagoField;
        }
        set {
            this.fechaInicialPagoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
    public System.DateTime FechaFinalPago {
        get {
            return this.fechaFinalPagoField;
        }
        set {
            this.fechaFinalPagoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal NumDiasPagados {
        get {
            return this.numDiasPagadosField;
        }
        set {
            this.numDiasPagadosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Departamento {
        get {
            return this.departamentoField;
        }
        set {
            this.departamentoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
    public string CLABE {
        get {
            return this.cLABEField;
        }
        set {
            this.cLABEField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Banco {
        get {
            return this.bancoField;
        }
        set {
            this.bancoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool BancoSpecified {
        get {
            return this.bancoFieldSpecified;
        }
        set {
            this.bancoFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
    public System.DateTime FechaInicioRelLaboral {
        get {
            return this.fechaInicioRelLaboralField;
        }
        set {
            this.fechaInicioRelLaboralField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool FechaInicioRelLaboralSpecified {
        get {
            return this.fechaInicioRelLaboralFieldSpecified;
        }
        set {
            this.fechaInicioRelLaboralFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Antiguedad {
        get {
            return this.antiguedadField;
        }
        set {
            this.antiguedadField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool AntiguedadSpecified {
        get {
            return this.antiguedadFieldSpecified;
        }
        set {
            this.antiguedadFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Puesto {
        get {
            return this.puestoField;
        }
        set {
            this.puestoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string TipoContrato {
        get {
            return this.tipoContratoField;
        }
        set {
            this.tipoContratoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string TipoJornada {
        get {
            return this.tipoJornadaField;
        }
        set {
            this.tipoJornadaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string PeriodicidadPago {
        get {
            return this.periodicidadPagoField;
        }
        set {
            this.periodicidadPagoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal SalarioBaseCotApor {
        get {
            return this.salarioBaseCotAporField;
        }
        set {
            this.salarioBaseCotAporField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool SalarioBaseCotAporSpecified {
        get {
            return this.salarioBaseCotAporFieldSpecified;
        }
        set {
            this.salarioBaseCotAporFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int RiesgoPuesto {
        get {
            return this.riesgoPuestoField;
        }
        set {
            this.riesgoPuestoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool RiesgoPuestoSpecified {
        get {
            return this.riesgoPuestoFieldSpecified;
        }
        set {
            this.riesgoPuestoFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal SalarioDiarioIntegrado {
        get {
            return this.salarioDiarioIntegradoField;
        }
        set {
            this.salarioDiarioIntegradoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool SalarioDiarioIntegradoSpecified {
        get {
            return this.salarioDiarioIntegradoFieldSpecified;
        }
        set {
            this.salarioDiarioIntegradoFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/nomina")]
public partial class NominaPercepciones {
    
    private NominaPercepcionesPercepcion[] percepcionField;
    
    private decimal totalGravadoField;
    
    private decimal totalExentoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Percepcion")]
    public NominaPercepcionesPercepcion[] Percepcion {
        get {
            return this.percepcionField;
        }
        set {
            this.percepcionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal TotalGravado {
        get {
            return this.totalGravadoField;
        }
        set {
            this.totalGravadoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal TotalExento {
        get {
            return this.totalExentoField;
        }
        set {
            this.totalExentoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/nomina")]
public partial class NominaPercepcionesPercepcion {

    private string tipoPercepcionField;
    
    private string claveField;
    
    private string conceptoField;
    
    private decimal importeGravadoField;
    
    private decimal importeExentoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string TipoPercepcion
    {
        get {
            return this.tipoPercepcionField;
        }
        set {
            this.tipoPercepcionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Clave {
        get {
            return this.claveField;
        }
        set {
            this.claveField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Concepto {
        get {
            return this.conceptoField;
        }
        set {
            this.conceptoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal ImporteGravado {
        get {
            return this.importeGravadoField;
        }
        set {
            this.importeGravadoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal ImporteExento {
        get {
            return this.importeExentoField;
        }
        set {
            this.importeExentoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/nomina")]
public partial class NominaDeducciones {
    
    private NominaDeduccionesDeduccion[] deduccionField;
    
    private decimal totalGravadoField;
    
    private decimal totalExentoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Deduccion")]
    public NominaDeduccionesDeduccion[] Deduccion {
        get {
            return this.deduccionField;
        }
        set {
            this.deduccionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal TotalGravado {
        get {
            return this.totalGravadoField;
        }
        set {
            this.totalGravadoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal TotalExento {
        get {
            return this.totalExentoField;
        }
        set {
            this.totalExentoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/nomina")]
public partial class NominaDeduccionesDeduccion {
    
    private string tipoDeduccionField;
    
    private string claveField;
    
    private string conceptoField;
    
    private decimal importeGravadoField;
    
    private decimal importeExentoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string TipoDeduccion
    {
        get {
            return this.tipoDeduccionField;
        }
        set {
            this.tipoDeduccionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Clave {
        get {
            return this.claveField;
        }
        set {
            this.claveField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Concepto {
        get {
            return this.conceptoField;
        }
        set {
            this.conceptoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal ImporteGravado {
        get {
            return this.importeGravadoField;
        }
        set {
            this.importeGravadoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal ImporteExento {
        get {
            return this.importeExentoField;
        }
        set {
            this.importeExentoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/nomina")]
public partial class NominaIncapacidad {
    
    private decimal diasIncapacidadField;
    
    private int tipoIncapacidadField;
    
    private decimal descuentoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal DiasIncapacidad {
        get {
            return this.diasIncapacidadField;
        }
        set {
            this.diasIncapacidadField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int TipoIncapacidad {
        get {
            return this.tipoIncapacidadField;
        }
        set {
            this.tipoIncapacidadField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Descuento {
        get {
            return this.descuentoField;
        }
        set {
            this.descuentoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/nomina")]
public partial class NominaHorasExtra {
    
    private int diasField;
    
    private NominaHorasExtraTipoHoras tipoHorasField;
    
    private int horasExtraField;
    
    private decimal importePagadoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Dias {
        get {
            return this.diasField;
        }
        set {
            this.diasField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public NominaHorasExtraTipoHoras TipoHoras {
        get {
            return this.tipoHorasField;
        }
        set {
            this.tipoHorasField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int HorasExtra {
        get {
            return this.horasExtraField;
        }
        set {
            this.horasExtraField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal ImportePagado {
        get {
            return this.importePagadoField;
        }
        set {
            this.importePagadoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/nomina")]
public enum NominaHorasExtraTipoHoras {
    
    /// <remarks/>
    Dobles,
    
    /// <remarks/>
    Triples,
}

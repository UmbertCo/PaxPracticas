﻿//-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
using System.Xml.Serialization;
//-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlRoot(ElementName = "TimbreFiscalDigital", Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital")]
public partial class TimbreFiscalDigital
{

    private string versionField;

    private string uUIDField;

    private System.DateTime fechaTimbradoField;

    private string selloCFDField;

    private string noCertificadoSATField;

    private string selloSATField;

    public TimbreFiscalDigital()
    {
        this.versionField = "1.0";
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string version
    {
        get
        {
            return this.versionField;
        }
        set
        {
            this.versionField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string UUID
    {
        get
        {
            return this.uUIDField;
        }
        set
        {
            this.uUIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public System.DateTime FechaTimbrado
    {
        get
        {
            return this.fechaTimbradoField;
        }
        set
        {
            this.fechaTimbradoField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string selloCFD
    {
        get
        {
            return this.selloCFDField;
        }
        set
        {
            this.selloCFDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string noCertificadoSAT
    {
        get
        {
            return this.noCertificadoSATField;
        }
        set
        {
            this.noCertificadoSATField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string selloSAT
    {
        get
        {
            return this.selloSATField;
        }
        set
        {
            this.selloSATField = value;
        }
    }
}

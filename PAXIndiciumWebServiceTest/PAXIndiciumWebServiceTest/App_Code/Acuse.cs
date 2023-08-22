using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Acuse
/// </summary>
public partial class Acuse {
        
        private int estatusField; //código del SAT
        
        private System.DateTime fechaCancelacionField; //fecha de la cancelación
        
        private string resultadoField; // descripción del error
        
        private byte[] archivoField; //Archivo de acuse adjunto ejemplo “acuse.xml”
        
        /// <remarks/>
        public int Estatus {
            get {
               return this.estatusField;
            }
            set {
                this.estatusField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime FechaCancelacion {
            get {
                return this.fechaCancelacionField;
            }
            set {
                this.fechaCancelacionField = value;
            }
        }
        
        /// <remarks/>
        public string Resultado {
            get {
                return this.resultadoField;
            }
            set {
                this.resultadoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] Archivo {
            get {
                return this.archivoField;
            }
            set {
                this.archivoField = value;
            }
        }
    }

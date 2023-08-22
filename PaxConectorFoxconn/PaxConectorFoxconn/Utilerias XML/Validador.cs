using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace Revisa_GeneraXML
{
    class Validador
    {
        string sXmlTexto { set; get; }

        string sErr, sWarn;

       public string sErrores { get{return sErr;} }

       public string sWarnings {get{return sWarn;}}

        bool bTerminar = false;

        bool bErr = false, bWar = false ,bBW = false;

       public  bool bErrores { get {return bErr; } }

       public  bool bWarnings { get { return bWar; } }

       public bool bErrorAdvertencia { get { return bBW; } }

        XmlReaderSettings xrsValidador = new XmlReaderSettings();

        /// <summary>
        /// Inicializa el objeto asignando la cadena xml en una variable local y ademas prepara el
        /// validador XmlReaderSettings para su futura validacion en la funcion fnRevisar
        /// </summary>
        /// <param name="sXmlTexto">El texto XML</param>
        public Validador(string sXmlTexto) 
        {
            this.sXmlTexto = sXmlTexto;
            xrsValidador.Schemas.Add(@"http://www.sat.gob.mx/cfd/3", "http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd");
            xrsValidador.ValidationType = ValidationType.Schema;
            xrsValidador.ValidationEventHandler += new ValidationEventHandler(fnxrsValidadorEventHandler);
        
        }

        /// <summary>
        /// Revisa el XML comparandolo con el esquema en busca de errores. Recive un parametro bTerminar
        /// que se encarga de decirle al Validador que Termine de revisar el XML o no en caso de que
        /// exista algun error
        /// </summary>
        /// <param name="bTerminar">Termina de revisar?</param>
        public void fnRevisar( bool bTerminar) 
        {
            this.bTerminar = bTerminar;

            XmlReader xrArchivoValidar = XmlReader.Create(new StringReader(sXmlTexto),xrsValidador);

            while (xrArchivoValidar.Read()) { }
            xrArchivoValidar.Close();

        
        }

        /// <summary>
        /// Revisa XML comparandolo con el esquema en busca de errores
        /// Por default termina de revisar en el momento en que encuentra un error o un warning
        /// </summary>
        public void fnRevisar() { fnRevisar(false); }

        /// <summary>
        /// Evento que se encarga de determinar si existe un error en el XML revisado 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         void fnxrsValidadorEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                sWarn = "WARNING: " + e.Message +"\n";

                bWar = true;
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                sErr = "ERROR: " + e.Message + "\n";

                bErr = true;
            }

            bBW = true;

             if(!bTerminar){return;}
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Revisa_GeneraXML
{
      public  class GeneraXML
    {
        FileInfo fiArchivo { set; get; }
        DirectoryInfo diDirectorio { set; get; }
        StreamReader srLector { set; get; }
        String sTexto { get; set; }
        Hashtable hsLlaves = new Hashtable();
         XmlDocument xdDocumento;

         public XmlDocument xdDoc { get { return xdDocumento; } }

        public String sDocumento { get { return xdDocumento.OuterXml; } }
     
        int nConceptos;
        int nNumeroConceptos { get { return nConceptos; } }

        int nRetenciones;
        int nNumeroRetenciones { get { return nRetenciones; } }

        int nTraslados;
        int nNumeroTraslados { get { return nTraslados; } }

        private bool sXmlcorrecto = true;
        public bool sXmlBienFormado { get { return sXmlBienFormado; } }

        public GeneraXML(string psruta) 
        {
            fiArchivo = new FileInfo(psruta);
            diDirectorio = fiArchivo.Directory;
            srLector = new StreamReader(psruta);
            fnLeerTexo();
            fnObtenerLlaves();
            fnFormarXml();
            
            

        }

        #region Funciones
        /// <summary>
        /// Lee todo el texto del documento proporcionado
        /// </summary>
        private void fnLeerTexo() 
        {

            sTexto = srLector.ReadToEnd();
            srLector.Close();
        
        }

        /// <summary>
        /// Divide la cadena en subcadenas separadas por los caracteres '?' y '/n'
        /// y las almacen en hsLlaves
        /// </summary>
        private void fnObtenerLlaves()
        {
            //Divide los renglones
            string [] sRenglones = sTexto.Split("?\n".ToCharArray());

            
            //quitar elementos vacios
            List<string> lRenglonesAux = new List<string>();
            foreach (string renglon in sRenglones)
            {
                if (!renglon.Equals(""))
                    lRenglonesAux.Add(renglon);            
            }

            sRenglones = lRenglonesAux.ToArray();
            
            //Contador de Conceptos
            int nConceptos = 0;

            //Contador de Retenciones
            int nRetenciones = 0;

            //Contador de Traslados
            int nTraslados = 0;

            for (int i = 0; i < sRenglones.Length; i += 2)
            {

                //Asigna el nombre al nodo del lado izquierdo
                string sNodo = sRenglones[i];

                //Investiga si el nodo es un Concepto en caso de serlo
                //lo contabiliza y evita que exista un error al ser guadado
                //en la hashtable al asignarle un numero despues del sufijo 'cc'
                if (sRenglones[i].Equals("cc"))
                {

                    sNodo = sNodo + (++nConceptos);


                }

                if (sRenglones[i].Equals("ir")) 
                {

                    sNodo = sNodo + (++nRetenciones);
                
                }

                if (sRenglones[i].Equals("it")) 
                {

                    sNodo = sNodo + (++nTraslados);
                
                }

                //Divide los renglones del lado derecho para que queden formados
                //en atributos
                string[] sAtributos = sRenglones[i + 1].Split("|".ToCharArray());


                //Guarda el nodo y sus atributos en una Hashtable 
                hsLlaves.Add(sNodo, sAtributos);

                //Adigna numero de conceptos en la factura para despues acceder a ellos
                this.nConceptos = nConceptos;

                //Asigna numero de Retenciones en la factura para despues acceder a ellos
                this.nRetenciones = nRetenciones;

                //Asigna numero de Traslados en la factura para despues acceder a ellos
                this.nTraslados = nTraslados;
            }
        
        }

        #region Funciones con XML
        /// <summary>
        /// Llena de Atributos un Elemento
        /// </summary>
        /// <param name="pxeElemento">Elemento pasado por referencia</param>
        /// <param name="psAtributos">Arreglo de atributos de la forma {atributo1@valor1.... atributon@valorn}</param>
        private void fnLlenarAtributosElemento(ref XmlElement pxeElemento, string [] psAtributos)
        {

            foreach (string sAtributo in psAtributos) 
            {

                fnAgregarAtributoElemento(ref pxeElemento, sAtributo);        
            
            }
        
        
        }

        /// <summary>
        /// Agrega un solo atributo a un Elemento
        /// </summary>
        /// <param name="pxeElemento">Elemento pasado por referencia</param>
        /// <param name="psAtributo">Atributo</param>
        private void fnAgregarAtributoElemento(ref XmlElement pxeElemento, string psAtributo) 
        {

            string[] sAtributo_separado = psAtributo.Split("@".ToCharArray());

            pxeElemento.SetAttribute(sAtributo_separado[0], sAtributo_separado[1]);
        
        
        
        }

        /// <summary>
        /// Llena de Elementos un elemento superior
        /// </summary>
        /// <param name="pxeElemento">Elemento Padre</param>
        /// <param name="Elementos">Array de elementos hijos</param>
        private void fnLlenarElementos_a_Elemento(ref XmlElement pxeElemento, XmlElement [] pxeElementos) 
        {

             for(int i=0;i<pxeElementos.Length;i++)
            {

                fnAgregarElemento_a_Elemento( ref pxeElemento, ref pxeElementos[i]);
            
            }
        
        }

        /// <summary>
        /// Agrega un solo Elemento al Elemento Superior
        /// </summary>
        /// <param name="pxeElemento">Elemento Padre</param>
        /// <param name="pxeElementoaux">Elemento Hijo</param>
        private void fnAgregarElemento_a_Elemento(ref XmlElement pxeElemento, ref XmlElement pxeElementoaux)
        {

                pxeElemento.AppendChild(pxeElementoaux);
       
        
        }

        /// <summary>
        /// Agrega los conceptos a un elemento padre llamado Conceptos
        /// </summary>
        /// <param name="pxeRetenciones">Elemento Conceptos</param>
        private void fnAgregarConceptos(ref XmlElement pxeConceptos) 
        {

            for (int i = 1; i <= nNumeroConceptos; i++)
            {
                XmlElement xeCC = pxeConceptos.OwnerDocument.CreateElement("cfdi:Concepto", "http://www.sat.gob.mx/cfd/3");

                fnLlenarAtributosElemento(ref xeCC, (string[])hsLlaves["cc" + i]);

                pxeConceptos.AppendChild(xeCC);
            }
        
        
        }

        /// <summary>
        /// Agrega las Retenciones a un elemento padre llamado Retenciones
        /// </summary>
        /// <param name="pxeRetenciones">Elemento Retenciones</param>
        private void fnAgregarRetenciones(ref XmlElement pxeRetenciones)
        {

            for (int i = 1; i <= nNumeroRetenciones; i++)
            {
                XmlElement xeIR = pxeRetenciones.OwnerDocument.CreateElement("cfdi:Retencion", "http://www.sat.gob.mx/cfd/3");

                fnLlenarAtributosElemento(ref xeIR, (string[])hsLlaves["ir" + i]);

                pxeRetenciones.AppendChild(xeIR);
            }


        }

        /// <summary>
        /// Agrega los Traslados a un elemento padre llamado Elementos
        /// </summary>
        /// <param name="pxeTraslados">Elemento Traslados</param>
        private void fnAgregarTraslados(ref XmlElement pxeTraslados)
        {

            for (int i = 1; i <= nNumeroTraslados; i++)
            {
                XmlElement xeIT = pxeTraslados.OwnerDocument.CreateElement("cfdi:Traslado", "http://www.sat.gob.mx/cfd/3");

                fnLlenarAtributosElemento(ref xeIT, (string[])hsLlaves["it" + i]);

                pxeTraslados.AppendChild(xeIT);
            }


        }
        
        /// <summary>
        /// Ordena los elementos y sus atributos en el Documento XML
        /// </summary>
        private void fnFormarXml() 
        {
           

            xdDocumento = new XmlDocument();
                       
            //Crear y llenar Elmento Comprobante
            XmlElement xeComprobante = xdDocumento.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3");

            xdDocumento.AppendChild(xeComprobante);

            xdDocumento.DocumentElement.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

            xdDocumento.DocumentElement.SetAttribute("schemaLocation","http://www.w3.org/2001/XMLSchema-instance",
                "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd");


            xdDocumento.DocumentElement.Attributes["schemaLocation"].Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
            

            #region Emisor
            //--Crear Elemento Emisor
            XmlElement xeRE = xdDocumento.CreateElement("cfdi:Emisor", "http://www.sat.gob.mx/cfd/3");
                
            
            //----Crear y llenar Elemento DomicilioFiscal
            XmlElement xeDE = xdDocumento.CreateElement("cfdi:DomicilioFiscal", "http://www.sat.gob.mx/cfd/3");
                
            //----Agregar Atributos
                  fnLlenarAtributosElemento(ref xeDE, (string [])hsLlaves["de"]);

            //----Crear y llenar Elemento DomicilioFiscal
                  XmlElement xeRF = xdDocumento.CreateElement("cfdi:RegimenFiscal", "http://www.sat.gob.mx/cfd/3");
            
            //----Agregar Atributos DomicilioFiscal
                  fnLlenarAtributosElemento(ref xeRF, (string[])hsLlaves["rf"]);

            //--Llenar de Elementos Emisor
                XmlElement[] xeElementos_Emisor = { xeDE, xeRF };
                fnLlenarElementos_a_Elemento(ref xeRE, xeElementos_Emisor);

            //--Agregar atributos a Emisor
                fnLlenarAtributosElemento(ref xeRE, (string[])hsLlaves["re"]);
            #endregion

            #region Receptor
            //--Crear Elemento Receptor
                XmlElement xeRR = xdDocumento.CreateElement("cfdi:Receptor", "http://www.sat.gob.mx/cfd/3");

            //----Crear y llenar Elemento Domicilio
                XmlElement xeDR = xdDocumento.CreateElement("cfdi:Domicilio", "http://www.sat.gob.mx/cfd/3");

            //----Agregar Atributos Domicilio
                  fnLlenarAtributosElemento(ref xeDR, (string[])hsLlaves["dr"]);
            
            //----Agregar DomicilioFiscal a Receptor
                  xeRR.AppendChild(xeDR);

            //----Agregar Atributos Receptor
                  fnLlenarAtributosElemento(ref xeRR, (string[])hsLlaves["rr"]);

            #endregion

            #region Conceptos
            //--Crear Elemento Emisor
                  XmlElement xeCS = xdDocumento.CreateElement("cfdi:Conceptos", "http://www.sat.gob.mx/cfd/3");
            
            //--Agregar Conceptos
                fnAgregarConceptos(ref xeCS);


            #endregion

            #region Impuestos
            //--Crear Elemento  Impuestos
                XmlElement xeIS = xdDocumento.CreateElement("cfdi:Impuestos", "http://www.sat.gob.mx/cfd/3");

            //----Crear Elemento Retenciones
                  XmlElement xeIR = xdDocumento.CreateElement("cfdi:Retenciones", "http://www.sat.gob.mx/cfd/3");
            
            //----Agregar Elementos a Retenciones
                  fnAgregarRetenciones(ref xeIR);

            //----Crear Elemento Traslados
                  XmlElement xeIT = xdDocumento.CreateElement("cfdi:Traslados", "http://www.sat.gob.mx/cfd/3");
           
            //----Agregar Elementos a Traslados 
                  fnAgregarTraslados(ref xeIT);

            //----Agregar Impuestos Traslados y Retenciones
                 if(nNumeroRetenciones!=0)
                  xeIS.AppendChild(xeIR);
                 if(nNumeroTraslados != 0)
                  xeIS.AppendChild(xeIT);

            ////--Llenar de Elementos Impuestos
            //    XmlElement[] xeElementos_Impuestos = { xeIR, xeIT };
            //    fnLlenarElementos_a_Elemento(ref xeIS, xeElementos_Impuestos);
            #endregion
            

            //Llenar de Elementos Comprobante
            XmlElement[] xeElementos_Comprobante = { xeRE, xeRR, xeCS, xeIS };    
            fnLlenarElementos_a_Elemento(ref xeComprobante, xeElementos_Comprobante);


            //Agregar Atributos Comprobante
            fnLlenarAtributosElemento(ref xeComprobante, (string[])hsLlaves["co"]);
            
//            xdDocumento.AppendChild(xeComprobante);

           

            
        }
        
        /// <summary>
        /// Guarda el Documento XML
        /// </summary>
        /// <param name="psRuta">Ruta donde se guardara el documento XML formado</param>
        public void fnGuardarXML(string psRuta)
        {
            xdDocumento.Save(psRuta);


        }

        #endregion

        #endregion
    }
}

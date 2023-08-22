using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace UtileriasXML.Adendas
{
      public class AdendaFoxconn :AdendaGenerica
    {

          #region Atributos
          XmlDocument _xdAdenda;
          public   XmlDocument xdAdenda 
          {

              get 
              {
              
                  XmlDocument auxAdenda = new XmlDocument();

                  auxAdenda.LoadXml(xeElementoFoxconn.OuterXml);
    
 
                  return auxAdenda; 
              
              
              }
          }

          XmlDocument _xdComprobante;
          public XmlDocument xdComprobante 
          {

              set 
              {

                  _xdComprobante = value;
              
              }
             
          
          }

          public XmlDocument xdComprobanteAdenda 
          {

              get 
              {
                  if (xeElementoFoxconn.HasChildNodes)
                  {

                      XmlDocument xdComprobanteAdenda = new XmlDocument();

                      xdComprobanteAdenda.LoadXml(_xdComprobante.DocumentElement.OuterXml);

                      XmlElement xeAddenda = xdComprobanteAdenda.CreateElement("cfdi", "Addenda", "http://www.sat.gob.mx/cfd/3");

                      xeAddenda.InnerXml = xdAdenda.OuterXml;

                      xdComprobanteAdenda.DocumentElement.AppendChild(xeAddenda);

                      return xdComprobanteAdenda;
                  }
                  else 
                  {


                      return _xdComprobante;
                  
                  }
              }
          
          }


          XmlElement xeElementoFoxconn;

          public string psLayoutEntrante
          {

              set
              {


                  _sLayout = value;

                  fnSepararAdenda();

                  fnGenerarXML();

              }

          }

          bool _bAdendaBienFormada = true;
          public bool bAdendaBienFormada 
          {

              get { return _bAdendaBienFormada; }
          }

          string _sMensajeError = "";
          public string sMensajeError 
          {

              get 
              {
                  
                  return _sMensajeError; }
          }


          string _sNodoError;
          public string sNodoError
          {

              get { return _sNodoError; }
          
          }

          #endregion

          #region Constructores
          /// <summary>
          /// 
          /// </summary>
          public AdendaFoxconn()
              : base()
          { 
          
            init();
              
          }

 


          /// <summary>
          /// 
          /// </summary>
          /// <param name="sLayout"></param>
           public AdendaFoxconn(string sLayout)
               : base(sLayout)  
           {
               init();
               fnSepararAdenda();
           
           }

          /// <summary>
          ///
          /// </summary>
          /// <param name="fsLayout"></param>
           public AdendaFoxconn(FileStream fsLayout)
               : base(fsLayout)
           {
               init();
               fnSepararAdenda();
           }
          #endregion

          #region Funciones

           #region Procedimientos
           /// <summary>
          /// Inicializador de todos los parametros para todos los contructores
          /// </summary>
          private void init()
          {
              _xdAdenda = new XmlDocument();

              xeElementoFoxconn = _xdAdenda.CreateElement("Foxconn");
         
          
          }

          /// <summary>
          /// Funcion que se utiliza para agregar un nodo al XML addenda
          /// </summary>
          /// <param name="sLinea">Arregloc con el indice y los atributos (todos juntos en una sola cadena)</param>
          public override void fnAgregarNodo(string[] sLinea) 
          {

              string[] sNodoEntrante = sLinea;

              if (fnEsAdenda(sNodoEntrante[0]))
              {

                  XmlElement xeElemento = _xdAdenda.CreateElement(fnDameNombreAdenda(sNodoEntrante[0]));

                  fnAgregarAtributosElemento(ref xeElemento, sNodoEntrante[1].Split("|".ToCharArray()));

                  xeElementoFoxconn.AppendChild(xeElemento);

              }
          
          }
          
          /// <summary>
          /// Funcion que se utiliza llaves del XML de la adenda
          /// y arma un documento XML
          /// </summary>
          protected override void fnGenerarXML()
           {
           
               if(!_sLayout.Equals(""))
               {
                    foreach(string linea in _sRenglonesAdenda)
                     {
                       
               
                        string []_auxlinea = linea.Split("?".ToCharArray());
    
                        string sNombre = _auxlinea[0];

                        string sAtributos = _auxlinea[1];
               
                        fnAgregarNodo(new string[]{sNombre,sAtributos});

                    }
               }
           
           
           }

          /// <summary>
          /// Funcion que toma la cadena la adenda de la cadena del Layout
          /// </summary>
          protected override void fnSepararAdenda()
           {
              _sRenglones.AddRange(  _sLayout.Split("\n\r".ToCharArray()));

              _sRenglones = _sRenglones.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
               
              int nTerminaComprobante = fnDondeTerminaComprobante(_sRenglones.ToArray());
              
              string []sAuxRenglones = _sRenglones.ToArray();

             _sRenglonesAdenda.AddRange(sAuxRenglones);

             _sRenglonesAdenda.RemoveRange(0, nTerminaComprobante);

           }

          /// <summary>
          /// Funcion que se utiliza para agregar un nuevo Elemento en el xml de la Adenda
          /// junto con sus atributos
          /// </summary>
          /// <param name="pxeElementoPadre">Elemento padre a agregar</param>
          /// <param name="psAtributos">arreglo de atributos de la forma Nombre@Valor</param>
           public  void fnAgregarAtributosElemento( ref XmlElement pxeElementoPadre, string []psAtributos)
         {


         
             foreach(string sAtributo in psAtributos)
             {
                 string sNombre = "";
                 string sValor="";
                 try
                 {
                     sNombre = sAtributo.Split("@".ToCharArray())[0];


                    string [] sauxValor = sAtributo.Split("@".ToCharArray());

                    for (int i = 1; i < sauxValor.Length; i++)
                    {

                        sValor += sauxValor[i]+"@";
                    
                    }
                   sValor= sValor.Substring(0, sValor.Length - 1);
                 }
                 catch 
                 {
                     _bAdendaBienFormada = false;
                     _sMensajeError += "Layout mal formado Atributo Invalido\n";
                     _sNodoError += pxeElementoPadre.Name+ "\n";
                     
                     return;
                 
                 }
                 try
                 {
                     XmlAttribute xaAtributo = pxeElementoPadre.OwnerDocument.CreateAttribute(sNombre);

                     xaAtributo.Value = sValor;

                     pxeElementoPadre.Attributes.Append(xaAtributo);
                 }
                 catch 
                 {

                     _bAdendaBienFormada = false;
                     _sMensajeError += "Layout mal formado Caracter Invalido";
                     _sNodoError += pxeElementoPadre.Name+"\n";
                 }
             }
         
         }

           #endregion

           #region Metodos

           /// <summary>
          /// Funcion que se encarga de determinar donde termina el comprobante
          /// en el Layout para poder saber donde empieza la Adenda
          /// </summary>
          /// <param name="psRenglones">Arreglo de Renglones</param>
          /// <returns>Numero del ultimo indice que corresponde al comprobante</returns>
           private int fnDondeTerminaComprobante(string []psRenglones) 
           {
               int nFin =0;

               foreach (string sRenglon in psRenglones) 
               {

                   string sIndice = sRenglon.Split("?".ToCharArray())[0]; 
               
                   if(!fnEsComprobante(sIndice))
                       return nFin;

                   nFin++;
               }
               

               return 0;
           }

          /// <summary>
          /// Funcion que transforma las llaves desde el formato del layout a formato XML
          /// </summary>
          /// <param name="psAlias"></param>
          /// <returns></returns>
           private string fnDameNombreAdenda(string psAlias) 
           {

               switch (psAlias)
               {


                   case "txid":
                       return "TaxID";
                   case "mp":
                       return "MetodoPago";
                   case "ad1":
                       return "Impuesto";
                   case "ad2":
                       return "NumeroDocumento";
                   case "ad3":
                       return "Pedimento";
                   case "ad4":
                       return "OrdenCompra";
                   case "ad5":
                       return "Termino";
                   case "ad6":
                       return "Factura";
                   case "st":
                       return "EnvioA";
                   case "drst":
                       return "DireccionEnvio";
                   case "tx":
                       return "TotalTexto";
                   case "so":
                       return "OrdenVenta";
                   case "del":
                       return "Envio";
                   case "bk1":
                       return "NumeroCuenta";
                   case "bk2":
                       return "NombreCuenta";
                   case "bk3":
                       return "NombreBanco";
                   case "bk4":
                       return "ClaveSwift";
                   case "bk5":
                       return "DireccionBanco";
                   case "bk6":
                       return "Contacto";



               }

               if (psAlias.Contains("asn"))
               {
               
                   int nNumero = int.Parse(psAlias.Substring(3));

                   return "NumeroParte" + nNumero.ToString();
               }

               return "";
           }

          /// <summary>
          /// Funcion que se encarga de decir si el indice pasado es del comprobante o no
          /// </summary>
          /// <param name="psIndice">Indice de layout</param>
          /// <returns>cierto si es del comprobante falso si no</returns>
           private bool fnEsComprobante(string psIndice) 
           {

               switch (psIndice)
               {
               
               
                   case "co":
                   case "re":
                   case "de":
                   case "ee":
                   case "rf":
                   case "rr":
                   case "dr":
                   case "cc":
                   case "ir":
                   case "it":
                       return true;


               
               }

               return false;
           }
      
          /// <summary>
          /// Funcion que se encarga de definir si el indice dado es parte de la adenda foxconn
          /// </summary>
          /// <param name="psIndice">Indice de Layout</param>
          /// <returns>cierto si es del comprobante falso si no</returns>
          private bool fnEsAdenda(string psIndice)
          {

              switch(psIndice)
              {
              
                  case "txid":
                  case "mp":
                  case "ad1":
                  case "ad2":
                  case "ad3":
                  case "ad4":
                  case "ad5":
                  case "ad6":
                  case "st":
                  case "drst":
                  case "tx":
                  case "so":
                  case "del":
                  case "bk1":
                  case "bk2":
                  case "bk3":
                  case "bk4":
                  case "bk5":
                  case "bk6":
                      return true;
                      
              
              
              }

              if (psIndice.Contains("asn")) { return true; }

          
              return false;
          }

           #endregion

           #endregion
    }

    }


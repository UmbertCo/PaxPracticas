using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace UtileriasXML.Adendas
{

     public abstract class AdendaGenerica
     {

         #region Atributos
         protected string _sLayout;
         public string SLayout
         {

             get { return _sLayout; }
         
         }
         
         protected List<string> _sRenglones;

         protected List<string> _sRenglonesAdenda;
         #endregion

         #region Constructores
         /// <summary>
         /// Constructor basico 
         /// </summary>
         public AdendaGenerica()
         {



             _sRenglones = new List<string>();

             _sRenglonesAdenda = new List<string>();
         }

         /// <summary>
         /// Constructor que recibe la cadena del Layout
         /// </summary>
         /// <param name="psLayout"></param>
         public AdendaGenerica(string psLayout)
         {

             _sLayout = psLayout;



             _sRenglones = new List<string>();

             _sRenglonesAdenda = new List<string>();
         }

         /// <summary>
         /// Constructor que recibe un Filestream del archivo a analizar
         /// </summary>
         /// <param name="fsLayout">Archivo  (regularmente recibido de System.IO.File)</param>
         public AdendaGenerica(FileStream fsLayout)
         {

             StreamReader _sLector = new StreamReader(fsLayout);

             _sLayout = _sLector.ReadToEnd();

             _sRenglones = new List<string>();

             _sRenglonesAdenda = new List<string>();

         } 
         #endregion

         #region Funciones Abstractas
         /// <summary>
         /// Funcion que se utiliza llaves del XML de la adenda
         /// y arma un documento XML
         /// </summary>
         protected abstract void fnGenerarXML();

         /// <summary>
         /// Funcion que toma la cadena la adenda de la cadena del Layout
         /// </summary>
         protected abstract void fnSepararAdenda(); 

         /// <summary>
         /// Funcoin que recibe Nodos de la Forma Linea {Nodo,Atributos}
         /// </summary>
         /// <param name="sLinea">Arreglo con 2 parametros</param>
         public abstract void fnAgregarNodo(string[] sLinea);
         #endregion
      
    }
}

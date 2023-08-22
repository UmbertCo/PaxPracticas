using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Revisa_GeneraXML;
using Revisa_GeneraXML.Properties;
using Root.Reports;

namespace CRevisa_GeneraXML
{
    class Ejercicios
    {

        public static void fnServicioWeb_mio() 
        {
         

            GeneraXML generador = new GeneraXML(Revisa_GeneraXML.Properties.Settings.Default.sRuta_Busqueda+"b.txt");

            wslModRevisaGenera.wslModRevisaSoapClient wslModValidar = new wslModRevisaGenera.wslModRevisaSoapClient();

            string sRes = wslModValidar.fnRevisaTexto(generador.sDocumento);
        
        }


        public static void crear_PDF()
        {
            Report rReporte = new Report(new PdfFormatter());

            List<Page> paginas = new List<Page>();

            for (int i = 1; i <= 5; i++)
                paginas.Add(new Page(rReporte));
             


           
            //asdasda

            clsCadenaRPdf Cadena = new clsCadenaRPdf(rReporte, "Pearl Jam!!");

            paginas[0].Add(0,Cadena.rHeight , Cadena.fnClonar());
            paginas[3].Add(0, Cadena.rHeight, Cadena.fnClonar());

            rReporte.Save("pdfvacio5hojas.pdf");
        
        }
    }

    /// <summary>
    /// Clase que hereda a RepString para un trato mas automatizado con
    /// las cadenas en libreria Reports
    /// </summary>
    public class clsCadenaRPdf : RepString
    {
        public FontDef fdFuente {  get { return base.fontProp.fontDef; } }

        public FontProp fpPropiedadesFuente {  get { return base.fontProp; } }

        public Report rReporte {  get { return base.fontProp.fontDef.report; } }

        public String sCadena {  get { return base.sText; } }

        /// <summary>
        /// Constructor con Parametros Basicos
        /// por default toma:
        ///     StandardFont: TimesRoman
        ///     Tamaño Fuente: 50.0
        /// </summary>
        /// <param name="preporte"></param>
        /// <param name="psCadena"></param>
        public clsCadenaRPdf(Report preporte,String psCadena)
            : base(new FontProp(new FontDef(preporte,FontDef.StandardFont.TimesRoman),50.0),psCadena) 
        {

        
        }




        /// <summary>
        /// Constructor Base
        /// </summary>
        /// <param name="pfpProp">Propiedades Fuente</param>
        /// <param name="sCadena">Cadena</param>
        public clsCadenaRPdf(FontProp pfpProp,String sCadena)
            :base(pfpProp,sCadena) 
        {
            
        
        
        }

        /// <summary>
        /// Clona el Objeto Cadena
        /// </summary>
        /// <returns>Objeto Cadena</returns>
        public clsCadenaRPdf fnClonar() 
        {

            clsCadenaRPdf srNuevoObjeto = new clsCadenaRPdf(fpPropiedadesFuente, sCadena);

            return srNuevoObjeto;
        }
    
    }
}

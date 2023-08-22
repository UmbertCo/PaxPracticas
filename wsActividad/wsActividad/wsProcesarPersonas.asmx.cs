using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;

namespace wsActividad
{
    /// <summary>
    /// Descripción breve de wsProcesarPersonas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class wsProcesarPersonas : System.Web.Services.WebService
    {

        [WebMethod]
        public string fnProcesarPersona(string sDatos)
        {
            Char cDelimitador = '?';
            Char cDelimitador1 = '@';
            Char cDelimitador2 = '|';
            Char cDelimitador3 = '\n';
            int Itamanio;
            string sPatron;
            string sPatron1;
            string sPatron2;
            string sPatron3;
            string sReemplazo;
            string sReemplazo1;
            string sReemplazo2;
            string sReemplazo3;
            string sReemplazo4;
            string sReemplazo5;
            string sReemplazo6;
            string sReemplazo7;
            string sReemplazo8;
            string sReemplazo9;
            string sReemplazo10;
            string sReemplazo11;
            string sReemplazo12;
            string sReemplazo13;
            string sReemplazo14;
            string sReemplazo15;
            StringBuilder sbConcatena = new StringBuilder();

            sPatron = @"([a-z]*)\?";
            Match sRegex = Regex.Match(sDatos, sPatron, RegexOptions.IgnoreCase);
            sReemplazo = sRegex.Groups[1].Value;
            sRegex = sRegex.NextMatch();
            sReemplazo1 = sRegex.Groups[1].Value;

            sPatron1 = @"\?([a-z]*)\@";
            Match sRegex1 = Regex.Match(sDatos, sPatron1, RegexOptions.IgnoreCase);
            sReemplazo2 = sRegex1.Groups[1].Value;
            sRegex1 = sRegex1.NextMatch();
            sReemplazo3 = sRegex1.Groups[1].Value;

            sPatron2 = @"\|([A-Z a-z]*)\@";
            Match sRegex2 = Regex.Match(sDatos, sPatron2, RegexOptions.IgnoreCase);
            sReemplazo4 = sRegex2.Groups[1].Value;
            sRegex2 = sRegex2.NextMatch();
            sReemplazo5 = sRegex2.Groups[1].Value;
            sRegex2 = sRegex2.NextMatch();
            sReemplazo6 = sRegex2.Groups[1].Value;
            sRegex2 = sRegex2.NextMatch();
            sReemplazo7 = sRegex2.Groups[1].Value;
            sRegex2 = sRegex2.NextMatch();
            sReemplazo8 = sRegex2.Groups[1].Value;
            sRegex2 = sRegex2.NextMatch();
            sReemplazo9 = sRegex2.Groups[1].Value;
            sRegex2 = sRegex2.NextMatch();
            sReemplazo10 = sRegex2.Groups[1].Value;
            sRegex2 = sRegex2.NextMatch();
            sReemplazo11 = sRegex2.Groups[1].Value;
            sRegex2 = sRegex2.NextMatch();
            sReemplazo12 = sRegex2.Groups[1].Value;

            //para buscar numerointerior
            Match sRegex3 = Regex.Match(sDatos, sPatron2, RegexOptions.RightToLeft);
            sRegex3 = sRegex3.NextMatch().NextMatch().NextMatch().NextMatch().NextMatch().NextMatch();
            sReemplazo13 = sRegex3.Groups[1].Value;

            sPatron3 = @"\@([A-Z a-z 0-9]*)";
            Match sRegex4 = Regex.Match(sDatos, sPatron3, RegexOptions.RightToLeft);
            sRegex4 = sRegex4.NextMatch().NextMatch().NextMatch().NextMatch().NextMatch().NextMatch();
            sReemplazo14 = sRegex4.Groups[1].Value;
            sRegex4 = sRegex4.NextMatch().NextMatch().NextMatch().NextMatch().NextMatch().NextMatch().NextMatch().NextMatch()
            .NextMatch().NextMatch().NextMatch().NextMatch().NextMatch().NextMatch();
            sReemplazo15 = sRegex4.Groups[1].Value;

            string[] sSplit = sDatos.Split(cDelimitador, cDelimitador1, cDelimitador2, cDelimitador3);

            foreach (string s in sSplit)
            {
                string sTemporal = s;
                sTemporal = sTemporal.Replace(sReemplazo, "<" + sReemplazo + ">");
                sTemporal = sTemporal.Replace(sReemplazo2, " \n<" + sReemplazo2 + ">");
                sTemporal = sTemporal.Replace(sReemplazo4, "</" + sReemplazo2 + ">" + " \n<" + sReemplazo4 + ">");
                sTemporal = sTemporal.Replace(sReemplazo5, "</" + sReemplazo4 + ">" + " \n<" + sReemplazo5 + ">");
                sTemporal = sTemporal.Replace(sReemplazo1, "</" + sReemplazo5 + ">" + " \n<" + sReemplazo1 + ">");
                sTemporal = sTemporal.Replace(sReemplazo3, "\n<" + sReemplazo3 + ">");
                sTemporal = sTemporal.Replace(sReemplazo6, "</" + sReemplazo3 + ">" + " \n<" + sReemplazo6 + ">");
                sTemporal = sTemporal.Replace(sReemplazo7, "</" + sReemplazo6 + ">" + " \n<" + sReemplazo7 + ">");
                sTemporal = sTemporal.Replace(sReemplazo8, "</" + sReemplazo7 + ">" + " \n<" + sReemplazo8 + ">");
                sTemporal = sTemporal.Replace(sReemplazo9, "</" + sReemplazo8 + ">" + " \n<" + sReemplazo9 + ">");
                sTemporal = sTemporal.Replace(sReemplazo10, "</" + sReemplazo9 + ">" + " \n<" + sReemplazo10 + ">");
                sTemporal = sTemporal.Replace(sReemplazo11, "</" + sReemplazo10 + ">" + " \n<" + sReemplazo11 + ">");
                sTemporal = sTemporal.Replace(sReemplazo12, "</" + sReemplazo11 + ">" + " \n<" + sReemplazo12 + ">");
                sTemporal = sTemporal.Replace(sReemplazo13, "</" + sReemplazo6 + ">" + "\n<" + sReemplazo13 + ">");
                
                sbConcatena.Append(sTemporal);
            }

            //para acomodar las etiquetas que no quedan bien cerradas
            sbConcatena.Replace("<" + sReemplazo + ">", "</" + sReemplazo12 + ">" + "\n</" + sReemplazo1 + ">" + "</" + sReemplazo +  ">" + " \n<" + sReemplazo + ">");
            sbConcatena.Replace(sReemplazo14 + "</" + sReemplazo6 + ">", sReemplazo14 + "</" + sReemplazo13 + ">");
            sbConcatena.Replace(sReemplazo15 + "</" + sReemplazo5 + ">", sReemplazo15 + "</" + sReemplazo4 + ">");

            Itamanio = sReemplazo12.Length + sReemplazo.Length + sReemplazo1.Length;
            sbConcatena = sbConcatena.Remove(0, Itamanio + 10);

            return "<" + sReemplazo + "s>" + sbConcatena + "</" + sReemplazo12 + ">" + "</" + sReemplazo1 + ">" + "</" + sReemplazo + ">" + "</"
                + sReemplazo + "s>";
    
        }
    }
}
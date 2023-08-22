using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Interop.Excel;

namespace SolucionPruebas.Negocios.ExpresionesRegulares
{
    public partial class HerramientasCSharp
    {
        private void HerramientasCSharp_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void btnExpresionRegular_Click(object sender, RibbonControlEventArgs e)
        {
            Worksheet wsHoja = Globals.ThisAddIn.fnObtenerCeldaActiva();

            bool nBandera = true;
            int nRenglon = 1;
            while (nBandera)
            {
                
                string sCURP = wsHoja.Range[string.Format("B{0}", nRenglon)].Value;

                if (string.IsNullOrEmpty(sCURP))
                    nBandera = false;
                else
                    wsHoja.Range[string.Format("C{0}", nRenglon)].Value = fnValidarExpresionRegular(sCURP, "[A-Z][AEIOUX][A-Z]{2}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[MH]([ABCMTZ]S|[BCJMOT]C|[CNPST]L|[GNQ]T|[GQS]R|C[MH]|[MY]N|[DH]G|NE|VZ|DF|SP)[BCDFGHJ-NP-TV-Z]{3}[0-9A-Z][0-9]");

                nRenglon++;
            }            
        }

        public bool fnValidarExpresionRegular(string psValor, string psExpresion)
        {
            bool bRetorno = false;

            if (Regex.IsMatch(psValor, psExpresion))
            {
                bRetorno = true;
            }

            return bRetorno;
        }
    }
}

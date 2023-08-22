using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using System.Globalization;
using RetencionesProyecto.Properties;
using System.IO;

namespace RetencionesProyecto
{
    class fideicomisoNoEmpresarial
    {
         CultureInfo languaje;

        public string Version { get; set; }


        //Importe total de ingresos del periodo
        public string sMontTotEntradasPeriodo;
        public string MontTotEntradasPeriodo
        {
            get { return sMontTotEntradasPeriodo; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotEntradasPeriodo = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Proporcional de Ingresos Acomulables
        public string sPartPropAcumDelFideicom;
        public string PartPropAcumDelFideicom
        {
            get { return sPartPropAcumDelFideicom; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sPartPropAcumDelFideicom = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Proporcion de participacion del fideicomisario
        public string sPropDelMontTot;
        public string PropDelMontTot
        {
            get { return sPropDelMontTot; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sPropDelMontTot = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Concepto de Ingresos
        public string Concepto { get; set; }


        public string sMontTotEgresPeriodo;
        public string MontTotEgresPeriodo
        {
            get { return sMontTotEgresPeriodo; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotEgresPeriodo = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }


        public string sPartPropDelFideicom;
        public string PartPropDelFideicom
        {
            get { return sPartPropDelFideicom; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sPartPropDelFideicom = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Proporcion de participacion del fideicomisario
        public string sPropDelMontTot2;
        public string PropDelMontTot2
        {
            get { return sPropDelMontTot2; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sPropDelMontTot2 = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }


        //Concepto de Egresos
        public string ConceptoS { get; set; }

        //Monto de Retenciones
        public string sMontRetRelPagFideic;
        public string MontRetRelPagFideic
        {
            get { return sMontRetRelPagFideic; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontRetRelPagFideic = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Descripcion de retenciones
        public string DescRetRelPagFideic { get; set; }


        /// <summary>
        /// Crea una nueva instancia de Detalle
        /// </summary>
        /// <param name="navDetalle">Navegador con los datos del concepto</param>
        /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
        public fideicomisoNoEmpresarial(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
        {
            try
            {

                Version = navDetalle.SelectSingleNode("@Version", nsmComprobante).Value;
                MontTotEntradasPeriodo = navDetalle.SelectSingleNode("fideicomisonoempresarial:IngresosOEntradas/@MontTotEntradasPeriodo", nsmComprobante).Value;
                PartPropAcumDelFideicom = navDetalle.SelectSingleNode("fideicomisonoempresarial:IngresosOEntradas/@PartPropAcumDelFideicom", nsmComprobante).Value;
                PropDelMontTot = navDetalle.SelectSingleNode("fideicomisonoempresarial:IngresosOEntradas/@PropDelMontTot", nsmComprobante).Value;
                Concepto = navDetalle.SelectSingleNode("fideicomisonoempresarial:IngresosOEntradas/fideicomisonoempresarial:IntegracIngresos/@Concepto", nsmComprobante).Value;
                MontTotEgresPeriodo = navDetalle.SelectSingleNode("fideicomisonoempresarial:DeduccOSalidas/@MontTotEgresPeriodo", nsmComprobante).Value;
                PartPropDelFideicom = navDetalle.SelectSingleNode("fideicomisonoempresarial:DeduccOSalidas/@PartPropDelFideicom", nsmComprobante).Value;
                PropDelMontTot2 = navDetalle.SelectSingleNode("fideicomisonoempresarial:DeduccOSalidas/@PropDelMontTot", nsmComprobante).Value;
                ConceptoS = navDetalle.SelectSingleNode("fideicomisonoempresarial:DeduccOSalidas/fideicomisonoempresarial:IntegracEgresos/@ConceptoS", nsmComprobante).Value;
                MontRetRelPagFideic = navDetalle.SelectSingleNode("fideicomisonoempresarial:RetEfectFideicomiso/@MontRetRelPagFideic", nsmComprobante).Value;
                DescRetRelPagFideic = navDetalle.SelectSingleNode("fideicomisonoempresarial:RetEfectFideicomiso/@DescRetRelPagFideic", nsmComprobante).Value;

               
            }
            catch (Exception ex)
            {
                Log("No se encontro un dato requerido" + ex.ToString());
            }
        }
        public static void Log(string e)
        {
            string path = Settings.Default.LogError + "LOG.txt";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                TextWriter tw = new StreamWriter(path);
                tw.WriteLine("Error " + e + " / " + System.DateTime.Now.ToString());
                tw.Close();
                tw.Dispose();
            }
            else if (File.Exists(path))
            {
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine("MError " + e + " / " + System.DateTime.Now.ToString());
                tw.Close();
                tw.Dispose();
            }
        }

    }
}

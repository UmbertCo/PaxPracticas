﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.XPath;
using System.Xml;
using PaxConectorRetenciones.Properties;
using System.IO;

namespace PaxConectorRetenciones
{
    class arrendamientoFideicomiso
    {
        CultureInfo languaje;

        public string Version { get; set; }


        //Importe Fiduciario
        public string sPagProvEfecPorFiduc;
        public string PagProvEfecPorFiduc
        {
            get { return sPagProvEfecPorFiduc; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
                sPagProvEfecPorFiduc = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Importe Rendimiento
        public string sRendimFideicom;
        public string RendimFideicom 
        {
            get { return sRendimFideicom; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
                sRendimFideicom = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Importe deducciones
        public string sDeduccCorresp;
        public string DeduccCorresp
        {
            get { return sDeduccCorresp; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
                sDeduccCorresp = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        public string sMontTotRet;
        public string MontTotRet
        {
            get { return sMontTotRet; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotRet = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        public string sMontResFiscDistFibras;
        public string MontResFiscDistFibras
        {
            get { return sMontResFiscDistFibras; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
                sMontResFiscDistFibras = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        public string sMontOtrosConceptDistr;
        public string MontOtrosConceptDistr
        {
            get { return sMontOtrosConceptDistr; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
                sMontOtrosConceptDistr = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        public string DescrMontOtrosConceptDistr { get; set; }


        private string sBaseRet;
        public string BaseRet
        {
            get { return sBaseRet; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
                sBaseRet = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        private string sMontoTotRet;
        public string MontoTotRet
        {
            get { return sMontoTotRet; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontoTotRet = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }



        /// <summary>
        /// Crea una nueva instancia de Detalle
        /// </summary>
        /// <param name="navDetalle">Navegador con los datos del concepto</param>
        /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
        public arrendamientoFideicomiso(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
        {
            try
            {

                Version = navDetalle.SelectSingleNode("@Version", nsmComprobante).Value;
                PagProvEfecPorFiduc = navDetalle.SelectSingleNode("@PagProvEfecPorFiduc", nsmComprobante).Value;
                RendimFideicom = navDetalle.SelectSingleNode("@RendimFideicom", nsmComprobante).Value;
                DeduccCorresp = navDetalle.SelectSingleNode("@DeduccCorresp", nsmComprobante).Value;
                MontoTotRet = navDetalle.SelectSingleNode("@MontoTotRet", nsmComprobante).Value;
                MontResFiscDistFibras = navDetalle.SelectSingleNode("@MontResFiscDistFibras", nsmComprobante).Value;
                MontOtrosConceptDistr = navDetalle.SelectSingleNode("@MontOtrosConceptDistr", nsmComprobante).Value;
                DescrMontOtrosConceptDistr = navDetalle.SelectSingleNode("@DescrMontOtrosConceptDistr", nsmComprobante).Value;
               
            }
            catch (Exception ex)
            {
                Log("No se encontro un dato requerido" + ex.ToString());
            }
        }

        public static void Log(string e)
        {
            DateTime Fecha = DateTime.Today;
            string path = Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
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

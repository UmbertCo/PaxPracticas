//using PAXConectorCapacitacion06.wsXML;
//using PAXConectorCapacitacion06.wsTimbrado;
//using PAXConectorCapacitacion06.wcfRecepcionASMX;
//using PAXConectorCapacitacion06.wsCancela;
using PAXConectorCapacitacion06.wcfCancelaASMX;
using PAXConectorCapacitacion06.wcfRecepcionASMX;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;
using System.Xml.XPath;
using System.Xml.Xsl;


namespace PAX_Conector_Capacitacion06
{
    /// <summary>
    /// Author:		Alejandro Martinez
    /// Date: 		14.04.2021
    /// Description: Clase comun
    /// </summary>
    /// Variables que están incluidas en las funciones
    /// <param name="sSalida">C:\PAXConectorCapacitacion06\Entrada\</param>
    /// <param name="sTxtProcesados">C:\PAXConectorCapacitacion06\TXT Procesados\</param>
    /// <param name="sLog">C:\PAXConectorCapacitacion06\Log\</param>
    /// <param name="sErrores">C:\PAXConectorCapacitacion06\Errores\</param>
    /// <param name="sCertificados">C:\PAXConectorCapacitacion06\Certificados\</param>
    /// <param name="sTemp">C:\PAXConectorCapacitacion06\Temp\</param>

    public class clsComunes
    {
        static clsLog sLog = new clsLog(ConfigurationManager.AppSettings["sLog"]);

        static string sCertificados = ConfigurationManager.AppSettings["sCertificados"].ToString();
        static string sEntrada = ConfigurationManager.AppSettings["sEntrada"].ToString();
        static string sErrores = ConfigurationManager.AppSettings["sErrores"].ToString();
        static string sSalida = ConfigurationManager.AppSettings["sSalida"].ToString();
        static string sTxtProcesados = ConfigurationManager.AppSettings["sTxtProcesados"].ToString();
        static string sCancelaciones = ConfigurationManager.AppSettings["sCancelaciones"].ToString();
        static string sTxtProcesadosCancelaciones = ConfigurationManager.AppSettings["sTxtProcesadosCancelaciones"].ToString();
        static string sSalidaCancelaciones = ConfigurationManager.AppSettings["sSalidaCancelaciones"].ToString();



        public static void fnPrincipal()
        {

            //Carga de TXT para timbrar
            DirectoryInfo disEntrada = new DirectoryInfo(sEntrada);
            foreach (var vArchivo in disEntrada.GetFiles("*.txt", SearchOption.TopDirectoryOnly))
            {
                string sArchivosinExtension = vArchivo.ToString();
                sArchivosinExtension = sArchivosinExtension.Remove(sArchivosinExtension.Length - 4);
                string sArchivoExtXML = sArchivosinExtension + ".xml";
                string sMaloXML = sErrores + sArchivoExtXML;

                if (!File.Exists(sTxtProcesados + vArchivo.Name))
                {

                    string sClavePrivada = fnValidaClave();
                    string sDirCer = fnValidaCerti();
                    string sKey = fnValidaKey();
                    string sFilePath = sEntrada + vArchivo.Name;
                    string sTexto = File.ReadAllText(sFilePath);

                    Comprobante co = new Comprobante();
                    ComprobanteEmisor re = new ComprobanteEmisor();
                    ComprobanteReceptor rr = new ComprobanteReceptor();

                    co.Emisor = re; co.Receptor = rr;
                    //Inicia Regex CO 
                    string sCO = @"(?<=co\?).*?(?=\n)";
                    Regex rCO = new Regex(sCO);
                    Match mCO = rCO.Match(sTexto);
                    string stCO = mCO.Value;
                    //Fin Regex CO


                    //Match Value CO
                    //Match Version
                    string sCOVersion = @"(?<=Version@).*?(?=\|)|(?<=Version@).*?(?=\r)";
                    Regex rSCOVersion = new Regex(sCOVersion);
                    Match mCOVersion = rSCOVersion.Match(stCO);
                    co.Version = mCOVersion.Value;

                    //Match Serie
                    string sCOSerie = @"(?<=Serie@).*?(?=\|)|(?<=Serie@).*?(?=\r)";
                    Regex rSCOSerie = new Regex(sCOSerie);
                    Match mCOSerie = rSCOSerie.Match(stCO);
                    co.Serie = mCOSerie.Value;

                    //Match Folio
                    string sCOFolio = @"(?<=Folio@).*?(?=\|)|(?<=Folio@).*?(?=\r)";
                    Regex rSCOFolio = new Regex(sCOFolio);
                    Match mCOFolio = rSCOFolio.Match(stCO);
                    co.Folio = mCOFolio.Value;

                    //Match Fecha
                    string sCOFecha = @"(?<=Fecha@).*?(?=\|)|(?<=Fecha@).*?(?=\r)";
                    Regex rSCOFecha = new Regex(sCOFecha);
                    Match mCOFecha = rSCOFecha.Match(stCO);
                    co.Fecha = mCOFecha.Value;

                    //Se pospone el Sello

                    //Match FormaPago
                    string sCOFormaPago = @"(?<=FormaPago@).*?(?=\|)|(?<=FormaPago@).*?(?=\r)";
                    Regex rSCOFormaPago = new Regex(sCOFormaPago);
                    Match mCOFormaPago = rSCOFormaPago.Match(stCO);
                    co.FormaPago = mCOFormaPago.Value;

                    //Se pospone numero de certificado 

                    //Se pospone Certificado

                    //Match CondicionesDePago
                    string sCOCondicionesDePago = @"(?<=CondicionesDePago@).*?(?=\|)|(?<=CondicionesDePago@).*?(?=\r)";
                    Regex rSCOCondicionesDePago = new Regex(sCOCondicionesDePago);
                    Match mCOCondicionesDePago = rSCOCondicionesDePago.Match(stCO);
                    co.CondicionesDePago = mCOCondicionesDePago.Value;

                    //Match SubTotal
                    string sCOSubTotal = @"(?<=SubTotal@).*?(?=\|)|(?<=SubTotal@).*?(?=\r)";
                    Regex rSCOSubTotal = new Regex(sCOSubTotal);
                    Match mCOSubtotal = rSCOSubTotal.Match(stCO);
                    string stCOSubTotal = mCOSubtotal.Value;
                    decimal dtCOSubTotal = Convert.ToDecimal(stCOSubTotal);
                    co.SubTotal = dtCOSubTotal;

                    //Match Descuento
                    string sCODescuento = @"(?<=Descuento@).*?(?=\|)|(?<=Descuento@).*?(?=\r)";
                    Regex rSCODescuento = new Regex(sCODescuento);
                    Match mCODescuento = rSCODescuento.Match(stCO);
                    string stCODescuento = mCODescuento.Value;
                    decimal dtCODescuento = Convert.ToDecimal(stCODescuento);
                    co.Descuento = dtCODescuento;

                    //Match TipoCambio
                    string sCOTipoCambio = @"(?<=TipoCambio@).*?(?=\|)|(?<=TipoCambio@).*?(?=\r)";
                    Regex rSCOTipoCambio = new Regex(sCOTipoCambio);
                    Match mCOTipoCambio = rSCOTipoCambio.Match(stCO);
                    string stCOTipoCambio = mCOTipoCambio.Value;
                    decimal dtCOTipoCambio = Convert.ToDecimal(stCOTipoCambio);
                    co.TipoCambio = dtCOTipoCambio;


                    //Match Moneda
                    string sCOMoneda = @"(?<=Moneda@).*?(?=\|)|(?<=Moneda@).*?(?=\r)";
                    Regex rSCOMoneda = new Regex(sCOMoneda);
                    Match mCOMoneda = rSCOMoneda.Match(stCO);
                    co.Moneda = mCOMoneda.Value;

                    //Match Total
                    string sCOTotal = @"(?<=\|Total@).*?(?=\|)|(?<=\|Total@).*?(?=\r)";
                    Regex rSCOTotal = new Regex(sCOTotal);
                    Match mCOTotal = rSCOTotal.Match(stCO);
                    string stCOTotal = mCOTotal.Value;
                    decimal dtCOTotal = Convert.ToDecimal(stCOTotal);
                    co.Total = dtCOTotal;

                    //Match TipoDeComprobante
                    string sCOTipoDeComprobante = @"(?<=TipoDeComprobante@).*?(?=\|)|(?<=TipoDeComprobante@).*?(?=\r)";
                    Regex rSCOTipoDeComprobante = new Regex(sCOTipoDeComprobante);
                    Match mCOTipoDeComprobante = rSCOTipoDeComprobante.Match(stCO);
                    co.TipoDeComprobante = mCOTipoDeComprobante.Value;

                    //Match MetodoPago
                    string sCOMetodoPago = @"(?<=MetodoPago@).*?(?=\|)|(?<=MetodoPago@).*?(?=\r)";
                    Regex rSCOMetodoPago = new Regex(sCOMetodoPago);
                    Match mCOMetodoPago = rSCOMetodoPago.Match(stCO);
                    co.MetodoPago = mCOMetodoPago.Value;


                    //Match LugarExpedicion
                    string sCOLugarExpedicion = @"(?<=LugarExpedicion@).*?(?=\|)|(?<=LugarExpedicion@).*?(?=\r)";
                    Regex rSCOLugarExpedicion = new Regex(sCOLugarExpedicion);
                    Match mCOLugarExpedicion = rSCOLugarExpedicion.Match(stCO);
                    co.LugarExpedicion = mCOLugarExpedicion.Value;

                    //Fin Match Vaue CO

                    // Se pospone confirmacion

                    //Inicia Regex RE
                    string sRE = @"(?<=re\?).*?(?=\n)";
                    Regex rRE = new Regex(sRE);
                    Match mRE = rRE.Match(sTexto);
                    string stRE = mRE.Value;
                    //Fin Regex RE

                    //Match Value RE
                    //Match Rfc
                    string sRERfc = @"(?<=Rfc@).*?(?=\|)|(?<=Rfc@).*?(?=\r)";
                    Regex rSRERfc = new Regex(sRERfc);
                    Match mRERfc = rSRERfc.Match(stRE);
                    re.Rfc = mRERfc.Value;

                    //Match Nombre
                    string sRENombre = @"(?<=Nombre@).*?(?=\|)|(?<=Nombre@).*?(?=\r)";
                    Regex rSRENombre = new Regex(sRENombre);
                    Match mRENombre = rSRENombre.Match(stRE);
                    re.Nombre = mRENombre.Value;

                    //Match RegimenFiscal
                    string sRERegimenFiscal = @"(?<=RegimenFiscal@).*?(?=\|)|(?<=RegimenFiscal@).*?(?=\r)";
                    Regex rSRERegimenFiscal = new Regex(sRERegimenFiscal);
                    Match mRERegimenFiscal = rSRERegimenFiscal.Match(stRE);
                    re.RegimenFiscal = mRERegimenFiscal.Value;

                    //Fin Match Value RE


                    //Inicia Regex RR
                    string sRR = @"(?<=rr\?).*?(?=\n)";
                    Regex rRR = new Regex(sRR);
                    Match mRR = rRR.Match(sTexto);
                    string stRR = mRR.Value;
                    //Fin Regex RR

                    //Match Value RR
                    //Match Rfc
                    string sRRRfc = @"(?<=Rfc@).*?(?=\|)|(?<=Rfc@).*?(?=\r)";
                    Regex rSRRRfc = new Regex(sRRRfc);
                    Match mRRRfc = rSRRRfc.Match(stRR);
                    rr.Rfc = mRRRfc.Value;

                    //Match Nombre
                    string sRRNombre = @"(?<=Nombre@).*?(?=\|)|(?<=Nombre@).*?(?=\r)";
                    Regex rSRRNombre = new Regex(sRRNombre);
                    Match mRRNombre = rSRRNombre.Match(stRR);
                    rr.Nombre = mRRNombre.Value;

                    //Match ResidenciaFiscal
                    string sRRResidenciaFiscal = @"(?<=ResidenciaFiscal@).*?(?=\|)|(?<=ResidenciaFiscal@).*?(?=\r)";
                    Regex rSRRResidenciaFiscal = new Regex(sRRResidenciaFiscal);
                    Match mRRResidenciaFiscal = rSRRResidenciaFiscal.Match(stRR);
                    if (mRRResidenciaFiscal.Success)
                    {
                        rr.ResidenciaFiscal = mRRResidenciaFiscal.Value;
                        //Match NumRegIdTrib
                        string sRRNumRegIdTrib = @"(?<=NumRegIdTrib@).*?(?=\|)|(?<=NumRegIdTrib@).*?(?=\r)";
                        Regex rSRRNumRegIdTrib = new Regex(sRRNumRegIdTrib);
                        Match mRRNumRegIdTrib = rSRRNumRegIdTrib.Match(stRR);
                        if (mRRNumRegIdTrib.Success)
                        { rr.NumRegIdTrib = mRRNumRegIdTrib.Value; }
                    }


                    //Match UsoCFDI
                    string sRRUsoCFDI = @"(?<=UsoCFDI@).*?(?=\|)|(?<=UsoCFDI@).*?(?=\r)";
                    Regex rSRRUsoCFDI = new Regex(sRRUsoCFDI);
                    Match mRRUsoCFDI = rSRRUsoCFDI.Match(stRR);
                    rr.UsoCFDI = mRRUsoCFDI.Value;

                    //Fin Match Value RR

                    //REGEX CC
                    string sCC = @"(?<=cc\?).*?(?=cc\?)|(?<=cc\?).*?(?=im\?)";
                    Regex r = new Regex(sCC, RegexOptions.Singleline);
                    Match m = r.Match(sTexto);
                    //Fin Regex CC

                    List<ComprobanteConcepto> lstConceptos = new List<ComprobanteConcepto>();

                    while (m.Success)
                    {
                        ComprobanteConcepto cc = new ComprobanteConcepto();
                        string stSCC = m.Value;

                        //Match Value CC
                        //Match Clave Producto
                        string sCCClaveProdServ = @"(?<=ClaveProdServ@).*?(?=\|)|(?<=ClaveProdServ@).*?(?=\r)";
                        Regex rCCClaveProdServ = new Regex(sCCClaveProdServ);
                        Match mCCClaveProdServ = rCCClaveProdServ.Match(stSCC);
                        string stCCClaveProdServ = mCCClaveProdServ.Value;
                        cc.ClaveProdServ = stCCClaveProdServ;

                        //Match NoIdentificacion
                        string sCCNoIdentificacion = @"(?<=NoIdentificacion@).*?(?=\|)|(?<=NoIdentificacion@).*?(?=\r)";
                        Regex rCCNoIdentificacion = new Regex(sCCNoIdentificacion);
                        Match mCCNoIdentificacion = rCCNoIdentificacion.Match(stSCC);
                        string stCCNoIdentificacion = mCCNoIdentificacion.Value;
                        cc.NoIdentificacion = stCCNoIdentificacion;

                        //Match Cantidad
                        string sCCCantidad = @"(?<=Cantidad@).*?(?=\|)|(?<=Cantidad@).*?(?=\r)";
                        Regex rCCCantidad = new Regex(sCCCantidad);
                        Match mCCCantidad = rCCCantidad.Match(stSCC);
                        string stCCCantidad = mCCCantidad.Value;
                        decimal dtCCCantidad = Convert.ToDecimal(stCCCantidad);
                        cc.Cantidad = dtCCCantidad;

                        //Match ClaveUnidad
                        string sCCClaveUnidad = @"(?<=ClaveUnidad@).*?(?=\|)|(?<=ClaveUnidad@).*?(?=\r)";
                        Regex rCCClaveUnidad = new Regex(sCCClaveUnidad);
                        Match mCCClaveUnidad = rCCClaveUnidad.Match(stSCC);
                        string stCCClaveUnidad = mCCClaveUnidad.Value;
                        cc.ClaveUnidad = stCCClaveUnidad;

                        //Match Descripcion
                        string sCCDescripcion = @"(?<=Descripcion@).*?(?=\|)|(?<=Descripcion@).*?(?=\r)";
                        Regex rCCDescripcion = new Regex(sCCDescripcion);
                        Match mCCDescripcion = rCCDescripcion.Match(stSCC);
                        string stCCDescripcion = mCCDescripcion.Value;
                        cc.Descripcion = stCCDescripcion;

                        //Match ValorUnitario
                        string sCCValorUnitario = @"(?<=ValorUnitario@).*?(?=\|)|(?<=ValorUnitario@).*?(?=\r)";
                        Regex rCCValorUnitario = new Regex(sCCValorUnitario);
                        Match mCCValorUnitario = rCCValorUnitario.Match(stSCC);
                        string stCCValorUnitario = mCCValorUnitario.Value;
                        decimal dtCCValorUnitario = Convert.ToDecimal(stCCValorUnitario);
                        cc.ValorUnitario = dtCCValorUnitario;

                        //Match Importe
                        string sCCImporte = @"(?<=Importe@).*?(?=\|)|(?<=Importe@).*?(?=\r)";
                        Regex rCCImporte = new Regex(sCCImporte);
                        Match mCCImporte = rCCImporte.Match(stSCC);
                        string stCCImporte = mCCImporte.Value;
                        decimal dtCCImporte = Convert.ToDecimal(stCCImporte);
                        cc.Importe = dtCCImporte;

                        //Match Descuento
                        string sCCDescuento = @"(?<=Descuento@).*?(?=\|)|(?<=Descuento@).*?(?=\r)";
                        Regex rCCDescuento = new Regex(sCCDescuento);
                        Match mCCDescuento = rCCDescuento.Match(stSCC);
                        string stCCDescuento = mCCDescuento.Value;
                        decimal dtCCDescuento = Convert.ToDecimal(stCCDescuento);
                        cc.Descuento = dtCCDescuento;

                        //FIN Match Value CC

                        lstConceptos.Add(cc);


                        ////fin Regex match para trasladados


                        ////Traslados CCIT
                        //Regex match para trasladados
                        string sCCIT = @"(?<=ccit\?).*?(?=\n)";
                        Regex rCCIT = new Regex(sCCIT);
                        Match mCCIT = rCCIT.Match(stSCC);
                        //Regex match para trasladados

                        List<ComprobanteConceptoImpuestosTraslado> lstImpuestosTrasladados = new List<ComprobanteConceptoImpuestosTraslado>();


                        while (mCCIT.Success)
                        {
                            ComprobanteConceptoImpuestosTraslado ccit = new ComprobanteConceptoImpuestosTraslado();
                            string stCCIT = mCCIT.Value;

                            //Match Value CCIT
                            //Match Base
                            string sCCITBase = @"(?<=Base@).*?(?=\|)|(?<=Base@).*?(?=\r)";
                            Regex rCCITBase = new Regex(sCCITBase);
                            Match mCCITBase = rCCITBase.Match(stCCIT);
                            string stCCITBase = mCCITBase.Value;
                            decimal dtCCTIBase = Convert.ToDecimal(stCCITBase);
                            ccit.Base = dtCCTIBase;

                            //Match Impuesto
                            string sCCITImpuesto = @"(?<=Impuesto@).*?(?=\|)|(?<=Impuesto@).*?(?=\r)";
                            Regex rCCITImpuesto = new Regex(sCCITImpuesto);
                            Match mCCITImpuesto = rCCITImpuesto.Match(stCCIT);
                            ccit.Impuesto = mCCITImpuesto.Value;

                            //Match TipoFactor
                            string sCCITTipoFactor = @"(?<=TipoFactor@).*?(?=\|)|(?<=TipoFactor@).*?(?=\r)";
                            Regex rCCITTipoFactor = new Regex(sCCITTipoFactor);
                            Match mCCITTipoFactor = rCCITTipoFactor.Match(stCCIT);
                            ccit.TipoFactor = mCCITTipoFactor.Value;

                            //Match TasaOCuota
                            string sCCITTasaOCuota = @"(?<=TasaOCuota@).*?(?=\|)|(?<=TasaOCuota@).*?(?=\r)";
                            Regex rCCITTasaOCuota = new Regex(sCCITTasaOCuota);
                            Match mCCITTasaOCuota = rCCITTasaOCuota.Match(stCCIT);
                            string stCCITTasaOCuota = mCCITTasaOCuota.Value;
                            decimal dtCCTITasaOCuota = Convert.ToDecimal(stCCITTasaOCuota);
                            ccit.TasaOCuota = dtCCTITasaOCuota;

                            //Match Importe
                            string sCCITImporte = @"(?<=Importe@).*?(?=\|)|(?<=Importe@).*?(?=\r)";
                            Regex rCCITImporte = new Regex(sCCITImporte);
                            Match mCCITImporte = rCCITImporte.Match(stCCIT);
                            string stCCITImporte = mCCITImporte.Value;
                            decimal dtCCTIImporte = Convert.ToDecimal(stCCITImporte);
                            ccit.Importe = dtCCTIImporte;

                            //FIN Match Value CCIT


                            lstImpuestosTrasladados.Add(ccit);

                            cc.Impuestos = new ComprobanteConceptoImpuestos();

                            mCCIT = mCCIT.NextMatch();
                        }

                        //Impuestos retenidos CCIR

                        ////Regex match para retencion
                        string sCCIR = @"(?<=ccir\?).*?(?=\n)";
                        Regex rCCIR = new Regex(sCCIR);
                        Match mCCIR = rCCIR.Match(stSCC);

                        ////fin Regex match para retencion

                        List<ComprobanteConceptoImpuestosRetencion> lstImpuestosRetencion = new List<ComprobanteConceptoImpuestosRetencion>();


                        while (mCCIR.Success)
                        {

                            ComprobanteConceptoImpuestosRetencion ccir = new ComprobanteConceptoImpuestosRetencion();
                            string stSCCIR = mCCIR.Value;

                            //Match Value CCIR
                            //Match Base
                            string sCCIRBase = @"(?<=Base@).*?(?=\|)|(?<=Base@).*?(?=\r)";
                            Regex rCCIRBase = new Regex(sCCIRBase);
                            Match mCCIRBase = rCCIRBase.Match(stSCCIR);
                            string stCCIRBase = mCCIRBase.Value;
                            decimal dtCCIRBase = Convert.ToDecimal(stCCIRBase);
                            ccir.Base = dtCCIRBase;

                            //Match Impuesto
                            string sCCIRImpuesto = @"(?<=Impuesto@).*?(?=\|)|(?<=Impuesto@).*?(?=\r)";
                            Regex rCCIRImpuesto = new Regex(sCCIRImpuesto);
                            Match mCCIRImpuesto = rCCIRImpuesto.Match(stSCCIR);
                            ccir.Impuesto = mCCIRImpuesto.Value;

                            //Match TipoFactor
                            string sCCIRTipoFactor = @"(?<=TipoFactor@).*?(?=\|)|(?<=TipoFactor@).*?(?=\r)";
                            Regex rCCIRTipoFactor = new Regex(sCCIRTipoFactor);
                            Match mCCIRTipoFactor = rCCIRTipoFactor.Match(stSCCIR);
                            ccir.TipoFactor = mCCIRTipoFactor.Value;

                            //Match TasaOCuota
                            string sCCIRTasaOCuota = @"(?<=TasaOCuota@).*?(?=\|)|(?<=TasaOCuota@).*?(?=\r)";
                            Regex rCCIRTasaOCuota = new Regex(sCCIRTasaOCuota);
                            Match mCCIRTasaOCuota = rCCIRTasaOCuota.Match(stSCCIR);
                            string stCCIRTasaOCuota = mCCIRTasaOCuota.Value;
                            decimal dtCCIRTasaOCuota = Convert.ToDecimal(stCCIRTasaOCuota);
                            ccir.TasaOCuota = dtCCIRTasaOCuota;

                            //Match Importe
                            string sCCIRImporte = @"(?<=Importe@).*?(?=\|)|(?<=Importe@).*?(?=\r)";
                            Regex rCCIRImporte = new Regex(sCCIRImporte);
                            Match mCCIRImporte = rCCIRImporte.Match(stSCCIR);
                            string stCCIRImporte = mCCIRImporte.Value;
                            decimal dtCCIRImporte = Convert.ToDecimal(stCCIRImporte);
                            ccir.Importe = dtCCIRImporte;

                            //Fin Match Value CCIR
                            lstImpuestosRetencion.Add(ccir);

                            mCCIR = mCCIR.NextMatch();
                        }

                        cc.Impuestos.Retenciones = lstImpuestosRetencion.ToArray();
                        cc.Impuestos.Traslados = lstImpuestosTrasladados.ToArray();

                        m = m.NextMatch();

                    }

                    co.Conceptos = lstConceptos.ToArray();
                    //FIN CC con CIR y CIT

                    //IM con IT y IR


                    //Regex match para Impuestos
                    string sIM = @"(?<=im\?).*?(?=\n)";
                    Regex rIM = new Regex(sIM);
                    Match mIM = rIM.Match(sTexto);
                    //fin Regex match para Impuestos
                    string stSIM = mIM.Value;
                    ComprobanteImpuestos im = new ComprobanteImpuestos();
                    //Match Value IM
                    //Match TotalImpuestosRetenidos
                    string sCCIRTotalImpuestosRetenidos = @"(?<=TotalImpuestosRetenidos@).*?(?=\|)|(?<=TotalImpuestosRetenidos@).*?(?=\r)";
                    Regex rCCIRTotalImpuestosRetenidos = new Regex(sCCIRTotalImpuestosRetenidos);
                    Match mCCIRTotalImpuestosRetenidos = rCCIRTotalImpuestosRetenidos.Match(stSIM);
                    string stCCIRTotalImpuestosRetenidos = mCCIRTotalImpuestosRetenidos.Value;
                    decimal dtCCIRTotalImpuestosRetenidos = Convert.ToDecimal(stCCIRTotalImpuestosRetenidos);
                    im.TotalImpuestosRetenidos = dtCCIRTotalImpuestosRetenidos;

                    //Match TotalImpuestosTrasladados
                    string sCCIRTotalImpuestosTrasladados = @"(?<=TotalImpuestosTrasladados@).*?(?=\|)|(?<=TotalImpuestosTrasladados@).*?(?=\r)";
                    Regex rCCIRTotalImpuestosTrasladados = new Regex(sCCIRTotalImpuestosTrasladados);
                    Match mCCIRTotalImpuestosTrasladados = rCCIRTotalImpuestosTrasladados.Match(stSIM);
                    string stCCIRTotalImpuestosTrasladados = mCCIRTotalImpuestosTrasladados.Value;
                    decimal dtCCIRTotalImpuestosTrasladados = Convert.ToDecimal(stCCIRTotalImpuestosTrasladados);
                    im.TotalImpuestosTrasladados = dtCCIRTotalImpuestosTrasladados;

                    //Fin Match Value IM

                    //Regex match para Comprobante impuestos retenidos IR
                    string sIR = @"(?<=\nir\?).*?(?=\n)";//@"(<=cir\?)";
                    Regex rIR = new Regex(sIR);
                    Match mIR = rIR.Match(sTexto);
                    //fin Regex match para Comprobante impuestosretenidos IR
                    List<ComprobanteImpuestosRetencion> lstIR = new List<ComprobanteImpuestosRetencion>();
                    while (mIR.Success)
                    {
                        ComprobanteImpuestosRetencion ir = new ComprobanteImpuestosRetencion();
                        string stIR = mIR.Value;

                        //Match Value IR

                        //Match Impuesto
                        string sIMIRImpuesto = @"(?<=Impuesto@).*?(?=\|)|(?<=Impuesto@).*?(?=\r)";
                        Regex rIMIRImpuesto = new Regex(sIMIRImpuesto);
                        Match mIMIRImpuesto = rIMIRImpuesto.Match(stIR);
                        ir.Impuesto = mIMIRImpuesto.Value;

                        //Match Importe
                        string sIMIRImporte = @"(?<=Importe@).*?(?=\|)|(?<=Importe@).*?(?=\r)";
                        Regex rIMIRImporte = new Regex(sIMIRImporte);
                        Match mIMIRImporte = rIMIRImporte.Match(stIR);
                        string stIMIRImporte = mIMIRImporte.Value;
                        decimal dtIMIRImporte = Convert.ToDecimal(stIMIRImporte);
                        ir.Importe = dtIMIRImporte;

                        //Fin Match Value IR
                        lstIR.Add(ir);
                        mIR = mIR.NextMatch();
                    }

                    List<ComprobanteImpuestosTraslado> lstIT = new List<ComprobanteImpuestosTraslado>();

                    //Regex match para Comprobante impuestos trasladados IT
                    string sIT = @"(?<=\nit\?).*?(?=\n)";
                    Regex rIT = new Regex(sIT);
                    Match mIT = rIT.Match(sTexto);
                    //fin Regex match para Comprobante impuestos trasladados IT

                    while (mIT.Success)
                    {
                        string stSIT = mIT.Value;
                        ComprobanteImpuestosTraslado it = new ComprobanteImpuestosTraslado();


                        //Match Value IT

                        //Match Impuesto
                        string sIMITImpuesto = @"(?<=Impuesto@).*?(?=\|)|(?<=Impuesto@).*?(?=\r)";
                        Regex rIMITImpuesto = new Regex(sIMITImpuesto);
                        Match mIMITImpuesto = rIMITImpuesto.Match(stSIT);
                        it.Impuesto = mIMITImpuesto.Value;

                        //Match Importe
                        string sIMITImporte = @"(?<=Importe@).*?(?=\|)|(?<=Importe@).*?(?=\r)";
                        Regex rIMITImporte = new Regex(sIMITImporte);
                        Match mIMITImporte = rIMITImporte.Match(stSIT);
                        string stIMITImporte = mIMITImporte.Value;
                        decimal dtIMITImporte = Convert.ToDecimal(stIMITImporte);
                        it.Importe = dtIMITImporte;

                        //Match TasaOCuota
                        string sIMITTasaOCuota = @"(?<=TasaOCuota@).*?(?=\|)|(?<=TasaOCuota@).*?(?=\r)";
                        Regex rIMITTasaOCuota = new Regex(sIMITTasaOCuota);
                        Match mIMITTasaOCuota = rIMITTasaOCuota.Match(stSIT);
                        string stIMITTasaOCuota = mIMITTasaOCuota.Value;
                        decimal dtIMITTasaOCuota = Convert.ToDecimal(stIMITTasaOCuota);
                        it.TasaOCuota = dtIMITTasaOCuota;

                        //Match TipoFactor
                        string sIMITTipoFactor = @"(?<=TipoFactor@).*?(?=\|)|(?<=TipoFactor@).*?(?=\r)";
                        Regex rIMITTipoFactor = new Regex(sIMITTipoFactor);
                        Match mIMITTipoFactor = rIMITTipoFactor.Match(stSIT);
                        it.TipoFactor = mIMITTipoFactor.Value;

                        //Fin Match Value IT

                        lstIT.Add(it);

                        mIT = mIT.NextMatch();
                    }

                    im.Retenciones = lstIR.ToArray();
                    im.Traslados = lstIT.ToArray();

                    co.Impuestos = im;


                    //FIN IM con IT y IR

                    string sArchivoXML = sEntrada + sArchivoExtXML;


                    try
                    {


                        byte[] bCertificado = File.ReadAllBytes(sDirCer);

                        //Service1Client client = new Service1Client();



                        //Obtener numero de certificado
                        string numeroCertificado, Inicio, Final, Serie;
                        bool bNoCertificado = clsValida.fnLeerCER(sDirCer, out Inicio, out Final, out Serie, out numeroCertificado);
                        //bool bNoCertificado = clsValida.fnLeerCER(bCertificado, out Inicio, out Final, out Serie, out numeroCertificado);
                        //bool bNoCertificado = client.fnLeerCER(out Inicio, out Final, out Serie, out numeroCertificado, bCertificado);
                        if (bNoCertificado == false)
                        {
                            sLog.fnAgregarLog("Se encontraron certificado: " + sDirCer + " incorrecto ");
                            sLog.fnAgregarLog("El servicio fue detenido.");
                            Environment.Exit(1);
                        }
                        sLog.fnAgregarLog("Se esta utilizando el certificado no: " + numeroCertificado + ".");



                        //Llena NoCertificado
                        co.NoCertificado = numeroCertificado;
                        //co.Certificado = client.fnCertificado(sDirCer);
                        co.Certificado = clsValida.Base64_Encode(bCertificado);


                        //Creacion XML
                        fnCreaXML(co, sArchivoXML);


                        string sXMLsinSello = fnXMLString(sArchivoXML);
                        //Datos para fnCrearSello a log
                        //sLog.fnAgregarLog("sXMLsinSello --->  " + sXMLsinSello);
                        //sLog.fnAgregarLog("----------");
                        //sLog.fnAgregarLog("sKey --->  " + sKey);
                        //sLog.fnAgregarLog("----------");
                        //sLog.fnAgregarLog("sClavePrivada --->  " + sClavePrivada);
                        //sLog.fnAgregarLog("----------");
                        //sLog.fnAgregarLog("sArchivoExtXML --->  " + sArchivoExtXML);
                        //sLog.fnAgregarLog("----------");

                        clsCrearSello fnCreaSello  = new clsCrearSello();

                       // co.Sello = client.fnCrearSello(sXMLsinSello, sKey, sClavePrivada, sArchivoExtXML);
                        co.Sello = fnCreaSello.fnCrearSello(sXMLsinSello, sKey, sClavePrivada, sArchivoExtXML);

                    
                        sLog.fnAgregarLog("Se realizo el sellado del XML: " + sArchivoExtXML);
                        //client.Close();
                        fnCreaXML(co, sArchivoXML); // con Sello


                        //Valida xml contra xsd 
                        clsValida clsValidacion = new clsValida();
                        bool bCorrecto = clsValidacion.fnValidarXML(sArchivoXML);


                        if (bCorrecto == true)
                        {
                            sLog.fnAgregarLog("Se agrega XML sellado");

                            string sXML = fnXMLString(sArchivoXML);

                            File.Delete(sArchivoXML);

                            fnXMLaTimbrado(sXML, sArchivoXML, sArchivoExtXML, co, sMaloXML);

                        }

                        else
                        {
                            File.Delete(sArchivoXML);
                            //Agrega icorrecto errores
                            fnCreaXML(co, sMaloXML);
                            sLog.fnAgregarLog("Se encontro incidencia con el sellado de: " + sArchivoExtXML);

                        }

                    }
                    catch (Exception exWS)
                    {
                        File.Delete(sArchivoXML);
                        //Agrega icorrecto errores
                        fnCreaXML(co, sMaloXML);
                        sLog.fnAgregarLog("Se agrega XML con errores");
                        //Que ha sucedido
                        sLog.fnAgregarLog(exWS.Message);
                        // Excepción interna
                        if (exWS.InnerException != null)
                        {
                            sLog.fnAgregarLog("\n" + exWS.InnerException.Message);
                        }
                        //¿Donde?
                        sLog.fnAgregarLog("\n" + exWS.StackTrace);
                    }

                    //Copia txt a carpeta procesados
                    File.Copy(sEntrada + vArchivo.Name, sTxtProcesados + vArchivo.Name);
                    File.SetAttributes(sTxtProcesados + vArchivo.Name, FileAttributes.Normal);

                    if (File.Exists(sTxtProcesados + vArchivo.Name) || File.Exists(sEntrada + vArchivo.Name))
                    {
                        //Borra txt que ya fueron copiados
                        File.Delete(sEntrada + vArchivo.Name);
                    }

                }


            }

            //Carga de TXT para cancelar
            DirectoryInfo disCancelaciones = new DirectoryInfo(sCancelaciones);
            foreach (var vArchivo in disCancelaciones.GetFiles("*.txt", SearchOption.TopDirectoryOnly))
            {
                sLog.fnAgregarLog("Inicia proceso de cancelado");

                if (!File.Exists(sTxtProcesadosCancelaciones + vArchivo.Name))
                {
                    string sFilePath = sCancelaciones + vArchivo.Name;
                    string sTexto = File.ReadAllText(sFilePath);

                    sLog.fnAgregarLog("Se detecta " + vArchivo + " para realizar cancelaciones.");

                    using (StreamReader ReaderObject = new StreamReader(sFilePath))
                    {


                        var sNoLineas = File.ReadAllLines(sFilePath).Length;


                        List<string> lListaUUID = new List<string>();
                        List<string> lpsRFCReceptor = new List<string>();
                        List<string> lsListaTotales = new List<string>();

                        string psRFC = string.Empty;

                        string Line;

                        bool bPipesValidos = false;

                        int i = 0;

                        while ((Line = ReaderObject.ReadLine()) != null)
                        {

                            string[] pipe = Line.Split('|');


                            if (pipe.Length == 4)
                            {
                                sLog.fnAgregarLog("La linea -> " + Line + " <- del archivo " + vArchivo + " a cancelar se cumple con los 4 parametros.");

                                //stListaUUID[i] = pipe[0];
                                lListaUUID.Add(pipe[0]);
                                psRFC = pipe[1];
                                //pstRFCReceptor[i] = pipe[2];
                                lpsRFCReceptor.Add(pipe[2]);
                                //stListaTotales[i] = pipe[3];         
                                lsListaTotales.Add(pipe[3]);

                                bPipesValidos = true;
                                i++;
                            }
                            else
                            {
                                sLog.fnAgregarLog("La linea -> " + Line + " <- del archivo " + vArchivo + " a cancelar no cumple con los 4 parametros. ");
                            }

                        }
                        if (bPipesValidos == true)
                        {

                            string sUsuarioCancelacion = ConfigurationManager.AppSettings["sUsuarioCancelacion"].ToString();
                            string sContraseñaCancelacion = ConfigurationManager.AppSettings["sContraseñaCancelacion"].ToString();
                            try
                            {


                                wcfCancelaASMXSoapClient servicioCancelacion = new wcfCancelaASMXSoapClient();
                                servicioCancelacion.Open();

                                string[] stListaUUID = lListaUUID.ToArray();
                                ArrayOfString sListaUUID = new ArrayOfString();
                                sListaUUID.AddRange(stListaUUID);

                                string[] pstRFCReceptor = lpsRFCReceptor.ToArray();
                                ArrayOfString psRFCReceptor = new ArrayOfString();
                                psRFCReceptor.AddRange(pstRFCReceptor);

                                string[] stListaTotales = lsListaTotales.ToArray();
                                ArrayOfString sListaTotales = new ArrayOfString();
                                sListaTotales.AddRange(stListaTotales);

                                //string sXmlCancelado = string.Empty;


                                try
                                {
                                    string sXmlCancelado = servicioCancelacion.fnCancelarXML(sListaUUID, psRFC, psRFCReceptor, sListaTotales, sUsuarioCancelacion, sContraseñaCancelacion);
                                    servicioCancelacion.Close();

                                    string sArchivosinExtension = vArchivo.ToString();
                                    sArchivosinExtension = sArchivosinExtension.Remove(sArchivosinExtension.Length - 4);
                                    string sArchivoExtXML = sArchivosinExtension + ".xml";

                                    //string s205 = 205.ToString();
                                    //bool b205 = sXmlCancelado.Contains(s205);

                                    //Console.WriteLine(b205);

                                    //Console.ReadKey();

                                    XmlDocument xtDoc = new XmlDocument();
                                    xtDoc.LoadXml(sXmlCancelado);
                                    String scArchivoXML = sSalidaCancelaciones + sArchivoExtXML;
                                    xtDoc.Save(scArchivoXML);

                                    sLog.fnAgregarLog("XMLs Cancelados  --- " + scArchivoXML + " ---");


                                }
                                catch (Exception ex)
                                {
                                    sLog.fnAgregarLog("Se presento incidencia realizado la peticion de cancelacion: ");
                                    //Que ha sucedido
                                    sLog.fnAgregarLog(ex.Message);
                                    // Excepción interna
                                    if (ex.InnerException != null)
                                    {
                                        sLog.fnAgregarLog("\n" + ex.InnerException.Message);
                                    }
                                    //¿Donde?
                                    sLog.fnAgregarLog("\n" + ex.StackTrace);
                                }

                            }
                            catch (Exception ex)
                            {
                                sLog.fnAgregarLog("Se presento incidencia realizado la instancia de cancelacion: ");
                                //Que ha sucedido
                                sLog.fnAgregarLog(ex.Message);
                                // Excepción interna
                                if (ex.InnerException != null)
                                {
                                    sLog.fnAgregarLog("\n" + ex.InnerException.Message);
                                }
                                //¿Donde?
                                sLog.fnAgregarLog("\n" + ex.StackTrace);
                            }


                        }

                    }
                    //Mueve txt a carpeta procesados
                    File.Move(sCancelaciones + vArchivo, sTxtProcesadosCancelaciones + vArchivo);


                }

                sLog.fnAgregarLog("Termina proceso de cancelado");

            }

        }

        /// <summary>
        /// Author:		Alejandro Martinez
        /// Date: 		24.03.2021
        /// Description:	Valida archivo con ClavePrivada
        /// </summary>
        /// Variables que están incluidas en las funciones
        /// <param name="sCertificados">Directorio que contiene Certifificados </param>
        private static string fnValidaClave()
        {
            sLog.fnAgregarLog("Inicio correcto del servicio");

            DirectoryInfo disCer = new DirectoryInfo(sCertificados);

            //Carga certificado
            string sClavePrivada = string.Empty;
            string sClave = string.Empty;

            int iCon = 0;

            foreach (var vClave in disCer.GetFiles("*.txt", SearchOption.TopDirectoryOnly))
            {
                if (File.Exists(sCertificados + vClave.Name))
                {
                    sClave = vClave.Name;
                    sClavePrivada = System.IO.File.ReadAllText(sCertificados + sClave);

                    sLog.fnAgregarLog("Se cargo la contraseña del archivo " + sClave + "  con exito.");
                    //bCer = true;
                    iCon++;
                }
            }
            if (iCon == 0)
            {
                sLog.fnAgregarLog("El contraseña no fue cargada.");
                sLog.fnAgregarLog("El servicio fue detenido.");
                Environment.Exit(1);
            }
            if (iCon > 1)
            {
                sLog.fnAgregarLog("Se encontraron " + iCon + " archivos de texto, favor de dejar solo uno con la contraseña.");
                sLog.fnAgregarLog("El servicio fue detenido.");
                Environment.Exit(1);
            }
            return sClavePrivada;

        }

        /// <summary>
        /// Author:		Alejandro Martinez
        /// Date: 		19.03.2021
        /// Description:	Valida Certificado
        /// </summary>
        /// 
        private static string fnValidaCerti()
        {
            DirectoryInfo disCer = new DirectoryInfo(sCertificados);

            string sCer = string.Empty;
            int iCon = 0;

            foreach (var vCer in disCer.GetFiles("*.cer", SearchOption.TopDirectoryOnly))
            {
                if (File.Exists(sCertificados + vCer.Name))
                {
                    sCer = vCer.Name;
                    sLog.fnAgregarLog("El certificado " + sCer + " fue cargado con exito.");
                    iCon++;
                }
            }
            if (iCon == 0)
            {
                sLog.fnAgregarLog("El certificado no fue cargado.");
                sLog.fnAgregarLog("El servicio fue detenido.");
                Environment.Exit(1);
            }
            if (iCon > 1)
            {
                sLog.fnAgregarLog("Se encontraron " + iCon + " certificados, favor de dejar solo uno.");
                sLog.fnAgregarLog("El servicio fue detenido.");
                Environment.Exit(1);
            }

            string sDirCer = sCertificados + sCer;

            return sDirCer;
        }

        /// <summary>
        /// Author:		Alejandro Martinez
        /// Date: 		19.03.2021
        /// Description:	Valida Key
        /// </summary>
        /// 

        private static string fnValidaKey()
        {
            DirectoryInfo disCer = new DirectoryInfo(sCertificados);
            int iCon = 0;
            //Carga Key
            string sKey = string.Empty;

            foreach (var vCer in disCer.GetFiles("*.key", SearchOption.TopDirectoryOnly))
            {
                if (File.Exists(sCertificados + vCer.Name))
                {
                    sKey = vCer.Name;
                    sLog.fnAgregarLog("La llave " + sKey + " fue cargada con exito.");
                    iCon++;
                }
            }
            if (iCon == 0)
            {
                sLog.fnAgregarLog("La llave no fue cargada.");
                sLog.fnAgregarLog("El servicio fue detenido.");
                Environment.Exit(1);
            }
            if (iCon > 1)
            {
                sLog.fnAgregarLog("Se encontraron " + iCon + " llaves, favor de dejar solo una.");
                sLog.fnAgregarLog("El servicio fue detenido.");
                Environment.Exit(1);
            }

            //string sDirKey = sCertificados + sKey;

            return sKey;
        }


        /// <summary>
        /// Author:		Alejandro Martinez
        /// Date: 		19.03.2021
        /// Description:	Crea XML
        /// </summary>
        /// 
        private static void fnCreaXML(Comprobante co, string sSalida)
        {
            try
            {
                XmlSerializerNamespaces xmlNameSpace = new XmlSerializerNamespaces();
                xmlNameSpace.Add("cfdi", "http://www.sat.gob.mx/cfd/3");
                //xmlNameSpace.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                xmlNameSpace.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                XmlSerializer oXmlSerializar = new XmlSerializer(typeof(Comprobante));
                string sXml = string.Empty;
                XmlDocument doc = new XmlDocument();

                // Crea la declaracion xml 
                //XmlDeclaration xmldecl;
                //xmldecl = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                ////xmldecl.Encoding = "UTF-8";


                // agrega nodo de la declaracion
                XmlElement root = doc.DocumentElement;
                //doc.InsertBefore(xmldecl, root);


                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                //settings.Encoding = new UTF8Encoding(true);
                settings.Encoding = new UTF8Encoding(false);

                //XmlWriter doc = XmlWriter.Create(sSalida, settings);


                using (XmlWriter writer = doc.CreateNavigator().AppendChild())
                {
                    using (XmlWriter writter = XmlWriter.Create(writer))
                    {
                        oXmlSerializar.Serialize(writter, co, xmlNameSpace);
                        sXml = writer.ToString();
                    }

                }


                doc.Save(sSalida);

                using (XmlWriter w = XmlWriter.Create(sSalida, settings))
                {
                    doc.Save(w);
                }

            }
            catch (Exception exCreaXML)
            { //¿Qué ha sucedido?
                sLog.fnAgregarLog(exCreaXML.Message);
                // Excepción interna
                if (exCreaXML.InnerException != null)
                {
                    sLog.fnAgregarLog("\n" + exCreaXML.InnerException.Message);
                }
                //¿Donde?
                sLog.fnAgregarLog("\n" + exCreaXML.StackTrace);
            }

        }

        /// <summary>
        /// Author:		Alejandro Martinez
        /// Date: 		14.04.2021
        /// Description: Recibe XML para convertirlo en string 
        /// </summary>
        private static string fnXMLString(string sArchivoXML)
        {
            XmlDocument tXml = new XmlDocument();
            tXml.Load(sArchivoXML);

            return tXml.OuterXml;
        }

        /// <summary>
        /// Author:		Alejandro Martinez
        /// Date: 		14.04.2021
        /// Description: Envia XML para ser timbrado 
        /// </summary>

        private static void fnXMLaTimbrado(string sXML, string sArchivoXML, string sArchivoExtXML, Comprobante co, string sMaloXML)
        {
            //Timbrado
            string sTipoDocumentoTimbre = ConfigurationManager.AppSettings["sTipoDocumentoTimbre"].ToString();
            int sIdEstructuraTimbre = int.Parse(ConfigurationManager.AppSettings["sIdEstructuraTimbre"]);
            string sUsuarioTimbre = ConfigurationManager.AppSettings["sUsuarioTimbre"];
            string sContraseñaTimbre = ConfigurationManager.AppSettings["sContraseñaTimbre"];
            string sVersionTimbre = ConfigurationManager.AppSettings["sVersionTimbre"];


            //Datos para fnCrearSello a log
            //sLog.fnAgregarLog("sXML --->  " + sXML);
            //sLog.fnAgregarLog("----------");
            //sLog.fnAgregarLog("sTipoDocumentoTimbre --->  " + sTipoDocumentoTimbre);
            //sLog.fnAgregarLog("----------");
            //sLog.fnAgregarLog("sUsuarioTimbre --->  " + sUsuarioTimbre);
            //sLog.fnAgregarLog("----------");
            //sLog.fnAgregarLog("sContraseñaTimbre --->  " + sContraseñaTimbre);
            //sLog.fnAgregarLog("----------");
            //sLog.fnAgregarLog("sVersionTimbre --->  " + sVersionTimbre);
            //sLog.fnAgregarLog("----------");

            //sLog.fnAgregarLog("Sello " + co.Sello);
            //sLog.fnAgregarLog("No Certificado " + co.NoCertificado);
            //sLog.fnAgregarLog("Certificado "+ co.Certificado);

            //wcfRecepcionASMXSoapClient fnTimbra = new wcfRecepcionASMXSoapClient();

            //Console.WriteLine("Inicia SOAP");
            //Console.ReadKey();
            wcfRecepcionASMXSoapClient servicioTimbrado = new wcfRecepcionASMXSoapClient();
            servicioTimbrado.Open();
          
            ////wcfRecepcionASMXSoapClient casa = new wcfRecepcionASMXSoapClient();
            //sLog.fnAgregarLog("-----" + sXML);
            //sLog.fnAgregarLog("-------" + sTipoDocumentoTimbre);
            //sLog.fnAgregarLog("-------" + sIdEstructuraTimbre);
            // sLog.fnAgregarLog("-------" + sUsuarioTimbre);
            // sLog.fnAgregarLog("-------" + sContraseñaTimbre);
            // sLog.fnAgregarLog("-------" + sVersionTimbre);

            // Console.WriteLine("----- PAUSA FORZADA ----");
         
            string sXmlTimbrado = servicioTimbrado.fnEnviarXML(sXML, sTipoDocumentoTimbre, sIdEstructuraTimbre, sUsuarioTimbre, sContraseñaTimbre, sVersionTimbre);
            servicioTimbrado.Close();


            //sXML = sXML.Replace(">", "&gt;");      
            //sXML = sXML.Replace("<", "&lt;");
            Console.WriteLine("----Timbrado---");
            Console.WriteLine(sXmlTimbrado);
            Console.ReadKey();                    
            
            
 

           // string sXmlTimbrado = fnTimbra.fnEnviarXML(sXML, sTipoDocumentoTimbre, sIdEstructuraTimbre, sUsuarioTimbre, sContraseñaTimbre, sVersionTimbre);
            try
            {
                XmlDocument xtDoc = new XmlDocument();
                xtDoc.LoadXml(sXmlTimbrado);
                sArchivoXML = sSalida + sArchivoExtXML;
                xtDoc.Save(sArchivoXML);

                sLog.fnAgregarLog("XML timbrado --- " + sArchivoXML + " ---");

            }
            catch (Exception ex)
            {
                File.Delete(sArchivoXML);
                //Agrega icorrecto errores
                fnCreaXML(co, sMaloXML);
                sLog.fnAgregarLog("No fue posible realizar el timbrado");
                sLog.fnAgregarLog(sXmlTimbrado);
                //Que ha sucedido
                sLog.fnAgregarLog(ex.Message);
                // Excepción interna
                if (ex.InnerException != null)
                {
                    sLog.fnAgregarLog("\n" + ex.InnerException.Message);
                }
                //¿Donde?
                sLog.fnAgregarLog("\n" + ex.StackTrace);
            }
        }



    }

}

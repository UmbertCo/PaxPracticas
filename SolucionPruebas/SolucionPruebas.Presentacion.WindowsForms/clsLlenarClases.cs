using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public class clsLlenarClases
    {
        Comprobante cComprobante;
        ComprobanteEmisor cComprobanteEmisor;
        ComprobanteReceptor cComprobanteReceptor;
        ComprobanteImpuestos cComprobanteImpuestos;
        ComprobanteImpuestosRetencion cComprobanteImpuestosRetencion;
        ComprobanteImpuestosTraslado cComprobanteImpuestosTraslado;
        ComprobanteComplemento cComprobanteComplemento;
        List<ComprobanteEmisorRegimenFiscal> ListaComprobanteEmisorRegimenFiscal = new List<ComprobanteEmisorRegimenFiscal>();
        List<ComprobanteConcepto> ListaComprobanteConcepto = new List<ComprobanteConcepto>();
        List<ComprobanteImpuestosRetencion> ListaComprobanteImpuestosRetencion = new List<ComprobanteImpuestosRetencion>();
        List<ComprobanteImpuestosTraslado> ListaComprobanteImpuestosTraslado = new List<ComprobanteImpuestosTraslado>();

        Nomina cComplementoNomina;
        NominaPercepciones cComplementoNominaPercepciones;
        NominaDeducciones cComplementoNominaDeducciones;
        List<NominaPercepcionesPercepcion> ListaComplementoNominaPercepciones = new List<NominaPercepcionesPercepcion>();
        List<NominaDeduccionesDeduccion> ListaComplementoNominaDeducciones = new List<NominaDeduccionesDeduccion>();
        List<NominaIncapacidad> ListaComplementoNominaIncapcidad = new List<NominaIncapacidad>();
        List<NominaHorasExtra> ListaComplementoNominaHorasExtra = new List<NominaHorasExtra>();

        X509Certificate2 certEmisor = new X509Certificate2();
        bool gnBanderaComplemento = true;
        string gsComplemento = string.Empty;
        OpenSSL_Lib.cSello cSello;

        public clsLlenarClases(string psRutaCertificado, string psPassword, byte[] paLlavePrivada, string psRutaGeneracion)
        {
            certEmisor.Import(psRutaCertificado);
            cSello = new OpenSSL_Lib.cSello(psPassword, paLlavePrivada, psRutaGeneracion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XmlDocument fnFormarComprobante()
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
            string sCadenaOriginalEmisor = string.Empty;
            string sNameSpace = null;
            XmlDocument xdRetorno = new XmlDocument();
            XPathNavigator navNodoTimbre;
            try
            {
                if (ListaComprobanteEmisorRegimenFiscal.Count > 0)
                {
                    cComprobanteEmisor.RegimenFiscal = ListaComprobanteEmisorRegimenFiscal.ToArray();
                }

                if (ListaComprobanteConcepto.Count > 0)
                {
                    cComprobante.Conceptos = ListaComprobanteConcepto.ToArray();
                }

                if (ListaComprobanteImpuestosRetencion.Count > 0)
                {
                    cComprobanteImpuestos.Retenciones = ListaComprobanteImpuestosRetencion.ToArray();
                }

                if (ListaComprobanteImpuestosTraslado.Count > 0)
                {
                    cComprobanteImpuestos.Traslados = ListaComprobanteImpuestosTraslado.ToArray();
                }

                if (gsComplemento.Equals("nom"))
                {
                    XmlDocument xmlComplNomina = new XmlDocument();
                    XmlSerializerNamespaces sns = new XmlSerializerNamespaces();
                    sns.Add("nomina", "http://www.sat.gob.mx/nomina");
                    cComprobanteComplemento = new ComprobanteComplemento();

                    sNameSpace = "nomina" + "|" + "http://www.sat.gob.mx/nomina" + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina11.xsd";

                    //Datos del complemento de Nómina
                    if (ListaComplementoNominaPercepciones.Count > 0)
                    {
                        cComplementoNominaPercepciones.Percepcion = ListaComplementoNominaPercepciones.ToArray();
                        cComplementoNomina.Percepciones = cComplementoNominaPercepciones;
                    }

                    if (ListaComplementoNominaDeducciones.Count > 0)
                    {
                        cComplementoNominaDeducciones.Deduccion = ListaComplementoNominaDeducciones.ToArray();
                        cComplementoNomina.Deducciones = cComplementoNominaDeducciones;
                    }

                    if (ListaComplementoNominaIncapcidad.Count > 0)
                    {
                        cComplementoNomina.Incapacidades = ListaComplementoNominaIncapcidad.ToArray();
                    }

                    if (ListaComplementoNominaHorasExtra.Count > 0)
                    {
                        cComplementoNomina.HorasExtras = ListaComplementoNominaHorasExtra.ToArray();
                    }

                    XmlSerializer serializador = new XmlSerializer(typeof(Nomina));
                    serializador.Serialize(sw, cComplementoNomina, sns);
                    ms.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(ms);
                    xmlComplNomina.LoadXml(sr.ReadToEnd());

                    XmlElement xeComplementoNomina = xmlComplNomina.DocumentElement;
                    XmlElement[] axeComplNomina = new XmlElement[] { xeComplementoNomina };
                    cComprobanteComplemento.Any = axeComplNomina;
                    cComprobante.Complemento = cComprobanteComplemento;
                }

                cComprobante.Emisor = cComprobanteEmisor;
                cComprobante.Receptor = cComprobanteReceptor;
                cComprobante.Impuestos = cComprobanteImpuestos;

                //-----------------------------------------------------------------------------------------------------------------------

                xdRetorno = fnGenerarXML32(cComprobante, sNameSpace);

                navNodoTimbre = xdRetorno.CreateNavigator();
                sCadenaOriginalEmisor = fnConstruirCadenaTimbrado(navNodoTimbre);
                cSello.sCadenaOriginal = sCadenaOriginalEmisor;
                cComprobante.sello = cSello.sSello;

                //Valida sello
                if (!fnVerificarSello(sCadenaOriginalEmisor, cComprobante.sello))
                {
                    throw new Exception("Sello invalido");
                }

                xdRetorno = fnGenerarXML32(cComprobante, sNameSpace);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return xdRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributosVersionSeccion"></param>
        public void fnLlenarClaseComprobante(string[] psAtributosVersionSeccion)
        {
            cComprobante = new Comprobante();

            foreach (string arreglo in psAtributosVersionSeccion)
            {
                if (arreglo.Contains("version"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.version = valores[1];
                }
                if (arreglo.Contains("serie"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.serie = valores[1];
                }
                if (arreglo.Contains("folio"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.folio = valores[1];
                }
                if (arreglo.Split('@')[0].Equals("fecha"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.fecha = Convert.ToDateTime(valores[1]);
                }
                if (arreglo.Contains("formaDePago"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.formaDePago = valores[1];
                }
                if (arreglo.Contains("noCertificado"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.noCertificado = valores[1];
                }
                if (arreglo.Contains("certificado"))
                {
                    cComprobante.certificado = Convert.ToBase64String(certEmisor.GetRawCertData());
                }
                if (arreglo.Contains("condicionesDePago"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.condicionesDePago = valores[1];
                }
                if (arreglo.Contains("subTotal"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.subTotal = Convert.ToDecimal(valores[1]);
                }
                if (arreglo.Contains("descuento"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.descuento = Convert.ToDecimal(valores[1]);
                    cComprobante.descuentoSpecified = true;
                }
                if (arreglo.Contains("motivoDescuento"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.motivoDescuento = valores[1];
                }
                if (arreglo.Contains("TipoCambio"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.TipoCambio = valores[1];
                }
                if (arreglo.Contains("Moneda"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.Moneda = valores[1];
                }
                if (arreglo.Contains("total"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.total = Convert.ToDecimal(valores[1]);
                }
                if (arreglo.Contains("tipoDeComprobante"))
                {
                    string[] valores = arreglo.Split('@');
                    if (valores[1].Equals("egreso"))
                        cComprobante.tipoDeComprobante = ComprobanteTipoDeComprobante.egreso;
                    else
                        cComprobante.tipoDeComprobante = ComprobanteTipoDeComprobante.ingreso;
                }
                if (arreglo.Contains("metodoDePago"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.metodoDePago = valores[1];
                }
                if (arreglo.Contains("LugarExpedicion"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.LugarExpedicion = valores[1];
                }
                if (arreglo.Contains("NumCtaPago"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.NumCtaPago = valores[1];
                }
                if (arreglo.Contains("FolioFiscalOrig"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.FolioFiscalOrig = valores[1];
                }
                if (arreglo.Contains("SerieFolioFiscalOrig"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.SerieFolioFiscalOrig = valores[1];
                }
                if (arreglo.Contains("FechaFolioFiscalOrig"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.FechaFolioFiscalOrig = Convert.ToDateTime(valores[1]);
                    cComprobante.FechaFolioFiscalOrigSpecified = true;
                }
                if (arreglo.Contains("MontoFolioFiscalOrig"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobante.MontoFolioFiscalOrig = Convert.ToDecimal(valores[1]);
                    cComprobante.MontoFolioFiscalOrigSpecified = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        public void fnLlenarClaseEmisor(string[] psAtributos)
        {
            cComprobanteEmisor = new ComprobanteEmisor();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("rfc"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.rfc = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("nombre"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.nombre = fnReplaceCaracters(valores[1]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        public void fnLlenarClaseEmisorDomicilio(string[] psAtributos)
        {
            cComprobanteEmisor.DomicilioFiscal = new t_UbicacionFiscal();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("calle"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.DomicilioFiscal.calle = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("noExterior"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.DomicilioFiscal.noExterior = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("noInterior"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.DomicilioFiscal.noInterior = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("colonia"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.DomicilioFiscal.colonia = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("localidad"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.DomicilioFiscal.localidad = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("referencia"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.DomicilioFiscal.referencia = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("municipio"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.DomicilioFiscal.municipio = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("estado"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.DomicilioFiscal.estado = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("pais"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.DomicilioFiscal.pais = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("codigoPostal"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.DomicilioFiscal.codigoPostal = fnReplaceCaracters(valores[1]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        public void fnLlenarClaseEmisorDomicilioExpedidoEn(string[] psAtributos)
        {
            cComprobanteEmisor.ExpedidoEn = new t_Ubicacion();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("calle"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.ExpedidoEn.calle = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("noExterior"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.ExpedidoEn.noExterior = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("noInterior"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.ExpedidoEn.noInterior = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("colonia"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.ExpedidoEn.colonia = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("localidad"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.ExpedidoEn.localidad = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("referencia"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.ExpedidoEn.referencia = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("municipio"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.ExpedidoEn.municipio = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("estado"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.ExpedidoEn.estado = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("pais"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.ExpedidoEn.pais = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("codigoPostal"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisor.ExpedidoEn.codigoPostal = fnReplaceCaracters(valores[1]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        public void fnLlenarClaseEmisorRegimen(string[] psAtributos)
        {
            ComprobanteEmisorRegimenFiscal cComprobanteEmisorRegimenFiscal = new ComprobanteEmisorRegimenFiscal();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("Regimen"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteEmisorRegimenFiscal.Regimen = fnReplaceCaracters(valores[1]);
                    ListaComprobanteEmisorRegimenFiscal.Add(cComprobanteEmisorRegimenFiscal);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        public void fnLlenarClaseReceptor(string[] psAtributos)
        {
            cComprobanteReceptor = new ComprobanteReceptor();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("rfc"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteReceptor.rfc = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("nombre"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteReceptor.nombre = fnReplaceCaracters(valores[1]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        public void fnLlenarClaseReceptorDomicilio(string[] psAtributos)
        {
            cComprobanteReceptor.Domicilio = new t_Ubicacion();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("calle"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteReceptor.Domicilio.calle = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("noExterior"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteReceptor.Domicilio.noExterior = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("noInterior"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteReceptor.Domicilio.noInterior = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("colonia"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteReceptor.Domicilio.colonia = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("localidad"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteReceptor.Domicilio.localidad = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("referencia"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteReceptor.Domicilio.referencia = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("municipio"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteReceptor.Domicilio.municipio = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("estado"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteReceptor.Domicilio.estado = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("pais"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteReceptor.Domicilio.pais = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("codigoPostal"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteReceptor.Domicilio.codigoPostal = fnReplaceCaracters(valores[1]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        public void fnLlenarClaseConceptos(string[] psAtributos)
        {
            ComprobanteConcepto CComprobanteConcepto = new ComprobanteConcepto();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("cantidad"))
                {
                    string[] valores = arreglo.Split('@');
                    CComprobanteConcepto.cantidad = Convert.ToDecimal(valores[1]);
                }
                if (arreglo.Contains("unidad"))
                {
                    string[] valores = arreglo.Split('@');
                    CComprobanteConcepto.unidad = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("noIdentificacion"))
                {
                    string[] valores = arreglo.Split('@');
                    CComprobanteConcepto.noIdentificacion = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("descripcion"))
                {
                    string[] valores = arreglo.Split('@');
                    CComprobanteConcepto.descripcion = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("valorUnitario"))
                {
                    string[] valores = arreglo.Split('@');
                    CComprobanteConcepto.valorUnitario = Convert.ToDecimal(valores[1]);
                }
                if (arreglo.Contains("importe"))
                {
                    string[] valores = arreglo.Split('@');
                    CComprobanteConcepto.importe = Convert.ToDecimal(valores[1]);
                }
            }

            ListaComprobanteConcepto.Add(CComprobanteConcepto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        public void fnLlenarClaseImpuestos(string[] psAtributos)
        {
            cComprobanteImpuestos = new ComprobanteImpuestos();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("totalImpuestosRetenidos"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteImpuestos.totalImpuestosRetenidos = Convert.ToDecimal(valores[1]);
                    cComprobanteImpuestos.totalImpuestosRetenidosSpecified = true;
                }
                if (arreglo.Contains("totalImpuestosTrasladados"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteImpuestos.totalImpuestosTrasladados = Convert.ToDecimal(valores[1]);
                    cComprobanteImpuestos.totalImpuestosTrasladadosSpecified = true;
                }
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        public void fnLlenarClaseImpuestosRetenidos(string[] psAtributos)
        {
            cComprobanteImpuestosRetencion = new ComprobanteImpuestosRetencion();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("impuesto"))
                {
                    string[] valores = arreglo.Split('@');

                    if (valores[0].Equals("ISR"))
                        cComprobanteImpuestosRetencion.impuesto = ComprobanteImpuestosRetencionImpuesto.ISR;
                    if (valores[0].Equals("IVA"))
                        cComprobanteImpuestosRetencion.impuesto = ComprobanteImpuestosRetencionImpuesto.IVA;
                }
                if (arreglo.Contains("importe"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteImpuestosRetencion.importe = Convert.ToDecimal(valores[1]);
                }
            }

            ListaComprobanteImpuestosRetencion.Add(cComprobanteImpuestosRetencion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        public void fnLlenarClaseImpuestosTrasladados(string[] psAtributos)
        {
            cComprobanteImpuestosTraslado = new ComprobanteImpuestosTraslado();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("impuesto"))
                {
                    string[] valores = arreglo.Split('@');

                    if (valores[0].Equals("IEPS"))
                        cComprobanteImpuestosTraslado.impuesto = ComprobanteImpuestosTrasladoImpuesto.IEPS;
                    if (valores[0].Equals("IVA"))
                        cComprobanteImpuestosTraslado.impuesto = ComprobanteImpuestosTrasladoImpuesto.IVA;
                    if (valores[0].Equals("ISH"))
                        cComprobanteImpuestosTraslado.impuesto = ComprobanteImpuestosTrasladoImpuesto.ISH;
                    //if (valores[0].Equals("ISH"))
                    //    cComprobanteImpuestosTraslado.impuesto = ComprobanteImpuestosTrasladoImpuesto.CargosNoGravables;
                }
                if (arreglo.Contains("importe"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteImpuestosTraslado.importe = Convert.ToDecimal(valores[1]);
                }
                if (arreglo.Contains("tasa"))
                {
                    string[] valores = arreglo.Split('@');
                    cComprobanteImpuestosTraslado.tasa = Convert.ToDecimal(valores[1]);
                }
            }

            ListaComprobanteImpuestosTraslado.Add(cComprobanteImpuestosTraslado);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psIndiceComplemento"></param>
        /// <param name="psIndice"></param>
        /// <param name="psAtributos"></param>
        public void fnLlenarClaseComplemento(string psIndiceComplemento, string psIndice, string[] psAtributos)
        {
            if (!gnBanderaComplemento)
            {
                return;
            }

            switch (psIndiceComplemento)
            {
                case "nom":
                    fnLlenarClaseComplementoNomina(psIndice, psAtributos);
                    gnBanderaComplemento = false;
                    gsComplemento = psIndiceComplemento;
                    break;
            }
        }

        /// <summary>
        /// Función que contruye la cadena original
        /// </summary>
        /// <param name="xml">Documento</param>
        /// <param name="psNombreArchivoXSLT">Nombre del archivo de tranformación</param>
        /// <returns></returns>
        private string fnConstruirCadenaTimbrado(IXPathNavigable xml)
        {
            string sCadenaOriginal = string.Empty;
            MemoryStream ms;
            StreamReader srDll;
            XsltArgumentList args;
            XslCompiledTransform xslt;
            try
            {
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(CaOri.V32));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(xml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);
                sCadenaOriginal = srDll.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception(DateTime.Now + " " + "Error al generar la cadena original." + " " + ex.Message);

            }
            return sCadenaOriginal;
        }

        /// <summary>
        /// Genera el XML en base a la estructura que contiene los datos version 3.2
        /// </summary>
        /// <param name="datos">Estructura que contiene los datos</param>
        /// <returns>XmlDocument con los datos del objeto Comprobante</returns>
        private XmlDocument fnGenerarXML32(Comprobante datos, string tNamespace)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
            XmlDocument xXml = new XmlDocument();
            XmlSerializerNamespaces sns = new XmlSerializerNamespaces();
            sns.Add("cfdi", "http://www.sat.gob.mx/cfd/3");
            string[] pspace = { "" };
            if (!(tNamespace == null))
            {
                pspace = tNamespace.Split('|');
                if (pspace.Length > 1)
                {
                    sns.Add(pspace[0], pspace[1]);
                }
            }
            XmlSerializer serializador = new XmlSerializer(typeof(Comprobante));
            try
            {
                serializador.Serialize(sw, datos, sns);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);

                xXml.LoadXml(sr.ReadToEnd());
                if (!(tNamespace == null))
                {
                    XmlAttribute att = xXml.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                    att.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd" + " " + pspace[1] + " " + pspace[2];
                    xXml.DocumentElement.SetAttributeNode(att);
                }
                else
                {
                    XmlAttribute att = xXml.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                    att.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
                    xXml.DocumentElement.SetAttributeNode(att);
                }

                return xXml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psIndice"></param>
        /// <param name="psAtributos"></param>
        private void fnLlenarClaseComplementoNomina(string psIndice, string[] psAtributos)
        {
            switch (psIndice)
            {
                case "nom":
                    cComplementoNomina = new Nomina();
                    fnLlenarClaseComplementoNominaCabecera(psAtributos);
                    break;

                case "percs":
                    cComplementoNominaPercepciones = new NominaPercepciones();
                    fnLlenarClaseComplementoNominaPercepcion(psAtributos);
                    break;

                case "per":
                    fnLlenarClaseComplementoNominaPercepciones(psAtributos);
                    break;

                case "deducs":
                    cComplementoNominaDeducciones = new NominaDeducciones();
                    fnLlenarClaseComplementoNominaDeduccion(psAtributos);
                    break;

                case "dedu":
                    fnLlenarClaseComplementoNominaDeducciones(psAtributos);
                    break;

                case "inca":
                    fnLlenarClaseComplementoNominaIncapacidades(psAtributos);
                    break;

                case "hora":
                    fnLlenarClaseComplementoNominaHorasExtra(psAtributos);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        private void fnLlenarClaseComplementoNominaCabecera(string[] psAtributos)
        {
            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("Version"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.Version = valores[1];
                }
                if (arreglo.Contains("RegistroPatronal"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.RegistroPatronal = valores[1];
                }
                if (arreglo.Contains("NumEmpleado"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.NumEmpleado = valores[1];
                }
                if (arreglo.Contains("CURP"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.CURP = valores[1];
                }
                if (arreglo.Contains("TipoRegimen"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.TipoRegimen = Convert.ToInt32(valores[1]);
                }
                if (arreglo.Contains("NumSeguridadSocial"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.NumSeguridadSocial = valores[1];
                }
                if (arreglo.Contains("FechaPago"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.FechaPago = Convert.ToDateTime(valores[1]);
                }
                if (arreglo.Contains("FechaInicialPago"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.FechaInicialPago = Convert.ToDateTime(valores[1]);
                }
                if (arreglo.Contains("FechaFinalPago"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.FechaFinalPago = Convert.ToDateTime(valores[1]);
                }
                if (arreglo.Contains("NumDiasPagados"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.NumDiasPagados = Convert.ToDecimal(valores[1]);
                }
                if (arreglo.Contains("Departamento"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.Departamento = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("CLABE"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.CLABE = valores[1];
                }
                if (arreglo.Contains("Banco"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.Banco = fnReplaceCaracters(valores[1]);
                    cComplementoNomina.BancoSpecified = true;
                }
                if (arreglo.Contains("FechaInicioRelLaboral"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.FechaInicioRelLaboral = Convert.ToDateTime(valores[1]);
                    cComplementoNomina.FechaInicioRelLaboralSpecified = true;
                }
                if (arreglo.Contains("Antiguedad"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.Antiguedad = Convert.ToInt32(valores[1]);
                    cComplementoNomina.AntiguedadSpecified = true;
                }
                if (arreglo.Split('@')[0].Equals("Puesto"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.Puesto = fnReplaceCaracters(valores[1]);

                }
                if (arreglo.Contains("TipoContrato"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.TipoContrato = valores[1];
                }
                if (arreglo.Contains("TipoJornada"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.TipoJornada = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("PeriodicidadPago"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.PeriodicidadPago = fnReplaceCaracters(valores[1]);
                }
                if (arreglo.Contains("SalarioBaseCotApor"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.SalarioBaseCotApor = Convert.ToDecimal(valores[1]);
                    cComplementoNomina.SalarioBaseCotAporSpecified = true;
                }
                if (arreglo.Contains("RiesgoPuesto"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.RiesgoPuesto = Convert.ToInt32(valores[1]);
                    cComplementoNomina.RiesgoPuestoSpecified = true;
                }
                if (arreglo.Contains("SalarioDiarioIntegrado"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNomina.SalarioDiarioIntegrado = Convert.ToDecimal(valores[1]);
                    cComplementoNomina.SalarioDiarioIntegradoSpecified = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        private void fnLlenarClaseComplementoNominaPercepcion(string[] psAtributos)
        {
            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("TotalGravado"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaPercepciones.TotalGravado = Convert.ToDecimal(valores[1]);
                }
                if (arreglo.Contains("TotalExento"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaPercepciones.TotalExento = Convert.ToDecimal(valores[1]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        private void fnLlenarClaseComplementoNominaPercepciones(string[] psAtributos)
        {
            NominaPercepcionesPercepcion cComplementoNominaPercepcion = new NominaPercepcionesPercepcion();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("TipoPercepcion"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaPercepcion.TipoPercepcion = valores[1];
                }
                if (arreglo.Contains("Clave"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaPercepcion.Clave = valores[1];
                }
                if (arreglo.Contains("Concepto"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaPercepcion.Concepto = valores[1];
                }
                if (arreglo.Contains("ImporteGravado"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaPercepcion.ImporteGravado = Convert.ToDecimal(valores[1]);
                }
                if (arreglo.Contains("ImporteExento"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaPercepcion.ImporteExento = Convert.ToDecimal(valores[1]);
                }
            }

            ListaComplementoNominaPercepciones.Add(cComplementoNominaPercepcion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        private void fnLlenarClaseComplementoNominaDeduccion(string[] psAtributos)
        {
            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("TotalGravado"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaDeducciones.TotalGravado = Convert.ToDecimal(valores[1]);
                }
                if (arreglo.Contains("TotalExento"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaDeducciones.TotalExento = Convert.ToDecimal(valores[1]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        private void fnLlenarClaseComplementoNominaDeducciones(string[] psAtributos)
        {
            NominaDeduccionesDeduccion cComplementoNominaDeduccion = new NominaDeduccionesDeduccion();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("TipoDeduccion"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaDeduccion.TipoDeduccion = valores[1];
                }
                if (arreglo.Contains("Clave"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaDeduccion.Clave = valores[1];
                }
                if (arreglo.Contains("Concepto"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaDeduccion.Concepto = valores[1];
                }
                if (arreglo.Contains("ImporteGravado"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaDeduccion.ImporteGravado = Convert.ToDecimal(valores[1]);
                }

                if (arreglo.Contains("ImporteExento"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaDeduccion.ImporteExento = Convert.ToDecimal(valores[1]);
                }
            }

            ListaComplementoNominaDeducciones.Add(cComplementoNominaDeduccion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        private void fnLlenarClaseComplementoNominaHorasExtra(string[] psAtributos)
        {
            NominaHorasExtra cComplementoNominaHoraExtra = new NominaHorasExtra();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("Dias"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaHoraExtra.Dias = Convert.ToInt32(valores[1]);
                }
                if (arreglo.Contains("TipoHoras"))
                {
                    string[] valores = arreglo.Split('@');

                    switch (valores[1].ToString())
                    {
                        case "Dobles":
                            cComplementoNominaHoraExtra.TipoHoras = NominaHorasExtraTipoHoras.Dobles;
                            break;
                        case "Triples":
                            cComplementoNominaHoraExtra.TipoHoras = NominaHorasExtraTipoHoras.Triples;
                            break;
                        default:
                            break;
                    }

                }
                if (arreglo.Contains("HorasExtra"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaHoraExtra.HorasExtra = Convert.ToInt32(valores[1]);
                }

                if (arreglo.Contains("ImportePagado"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaHoraExtra.ImportePagado = Convert.ToDecimal(valores[1]);
                }
            }

            ListaComplementoNominaHorasExtra.Add(cComplementoNominaHoraExtra);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psAtributos"></param>
        private void fnLlenarClaseComplementoNominaIncapacidades(string[] psAtributos)
        {
            NominaIncapacidad cComplementoNominaIncapacidad = new NominaIncapacidad();

            foreach (string arreglo in psAtributos)
            {
                if (arreglo.Contains("DiasIncapacidad"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaIncapacidad.DiasIncapacidad = Convert.ToDecimal(valores[1]);
                }
                if (arreglo.Contains("TipoIncapacidad"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaIncapacidad.TipoIncapacidad = Convert.ToInt32(valores[1]);
                }
                if (arreglo.Contains("Descuento"))
                {
                    string[] valores = arreglo.Split('@');
                    cComplementoNominaIncapacidad.Descuento = Convert.ToDecimal(valores[1]);
                }
            }

            ListaComplementoNominaIncapcidad.Add(cComplementoNominaIncapacidad);
        }

        /// <summary>
        /// Anexo 20 Eliminar en la reglas de estructura.
        /// </summary>
        /// <param name="varRep"></param>
        /// <returns></returns>
        private string fnReplaceCaracters(string varRep)
        {
            string sReplace = string.Empty;

            if (varRep.Contains('&'))
            {
                varRep.Replace("&", "&amp;");
            }

            if (varRep.Contains('<'))
            {
                varRep.Replace("<", "&lt;");
            }

            if (varRep.Contains('>'))
            {
                varRep.Replace(">", "&gt;");
            }

            if (varRep.Contains("'"))
            {
                varRep.Replace("'", "&apos;");
            }

            if (varRep.Contains("\""))
            {
                varRep.Replace("\"", "&quot;");
            }

            sReplace = varRep;
            return sReplace;
        }

        /// <summary>
        /// Comprueba que el sello del comprobante refleje los datos de la cadena original
        /// </summary>
        /// <param name="psCadenaOriginal">Cadena original del comprobante</param>
        /// <returns>Booleano indicando si la cadena original corresponde al sello</returns>
        private bool fnVerificarSello(string psCadenaOriginal, string psSello)
        {
            RSACryptoServiceProvider publica = ((RSACryptoServiceProvider)certEmisor.PublicKey.Key);
            try
            {
                //Verificamos que el certificado obtenga el mismo resultado que el del sello
                byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(psCadenaOriginal));
                bool exito = publica.VerifyHash(
                        hash,
                        "sha1",
                        Convert.FromBase64String(psSello));

                return exito;
            }
            catch (Exception)
            {
                return false;
            }
        }        
    }
}

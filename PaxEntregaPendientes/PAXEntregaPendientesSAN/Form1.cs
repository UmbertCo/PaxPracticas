using PAXEntregaPendientesSAN.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Windows.Forms;

namespace PAXEntregaPendientesSAN
{
    public partial class frmEntregaPendientesSAN : Form
    {
        public frmEntregaPendientesSAN()
        {
            InitializeComponent();
        }
        private void btnPrueba_Click(object sender, EventArgs e)
        {
            DateTime dFechaComprobante;
            DateTime dFechaTimbrado;
            string sCadenaOriginalEmisor = string.Empty;
            string sCadenaOriginal = string.Empty;
            string HASHEmisor = string.Empty;
            System.Diagnostics.EventLog eventLogExport = new System.Diagnostics.EventLog();

            int nArchivos = lbComprobantes.Items.Count;

            DataTable dtComprobantes = new DataTable();            
            dtComprobantes.Columns.Add("IdUsuario", typeof(int));
            dtComprobantes.Columns.Add("IdEstructura", typeof(int));
            dtComprobantes.Columns.Add("UUID", typeof(string));
            dtComprobantes.Columns.Add("Usuario", typeof(string));
            dtComprobantes.Columns.Add("Emisor", typeof(string));
            dtComprobantes.Columns.Add("FechaTimbrado", typeof(DateTime));
            dtComprobantes.Columns.Add("Tamanio", typeof(string));
            dtComprobantes.Columns.Add("Existe", typeof(string));
            dtComprobantes.Columns.Add("PAC", typeof(string));
            dtComprobantes.Columns.Add("Version", typeof(string));

            
            foreach (string nombreArchivo in lbComprobantes.Items)
            {
                try
                {

                    string sNombreArchivo = Path.GetFileName(nombreArchivo);
                    string sNombreZip = string.Empty;
                    dFechaTimbrado = DateTime.Now;

                    File.Copy(nombreArchivo, Settings.Default.RutaPendientes + sNombreArchivo, true);

                    var contenidoArchivo = fnRecuperaArchivo(Settings.Default.RutaPendientes + sNombreArchivo);
                    byte[] contenidoArchivoBytes = fnRecuperaArchivoByte(contenidoArchivo, Settings.Default.RutaPendientes + sNombreArchivo);

                    contenidoArchivo.Close();

                    XmlDocument sXmlDocument = new XmlDocument();
                    try
                    {
                        sXmlDocument.Load(Settings.Default.RutaPendientes + sNombreArchivo);
                    }
                    catch (Exception ex)
                    {
                        string sComprobante = File.ReadAllText(Settings.Default.RutaPendientes + sNombreArchivo);
                        int nIndexFinalCfdi = sComprobante.IndexOf("</cfdi:Comprobante>");

                        sComprobante = sComprobante.Substring(0, nIndexFinalCfdi + 19);

                        sXmlDocument.LoadXml(sComprobante);
                    }

                    bool bGrande = false;

                    Double TamañoXML = Convert.ToDouble(contenidoArchivoBytes.Length / 1024.0);

                    if (TamañoXML >= Convert.ToDouble(Settings.Default.LimiteInfGrandes) && TamañoXML <= Convert.ToDouble(Settings.Default.LimiteSupGrandes))
                        bGrande = true;

                    XPathNavigator navNodoTimbre = sXmlDocument.CreateNavigator();

                    XslCompiledTransform xslt;
                    XsltArgumentList args;
                    MemoryStream ms;
                    StreamReader srDll;

                    string sRfcPac = string.Empty;
                    string sUUID = string.Empty;
                    string sRfcEmisor = string.Empty;
                    string sRfcReceptor = string.Empty;
                    string sCadenaTimbre = string.Empty;
                    string sHashTimbre = string.Empty;
                    string sNombreEmisor = string.Empty;
                    string sNombreReceptor = string.Empty;
                    string sMoneda = string.Empty;
                    string sSerie = string.Empty;
                    string sFolio = string.Empty;
                    string sTotal = string.Empty;
                    string sVersion = string.Empty;
                    DateTime dFechaEmision = new DateTime();
                    int nIdEstructura = 0;
                    int nIdUsuario = 0;

                    // Hash Emisor
                    try
                    {
                        xslt = new XslCompiledTransform();
                        //xslt.Load(typeof(CaOri.V3211));
                        xslt.Load(typeof(CaOri.V33));
                        ms = new MemoryStream();
                        args = new XsltArgumentList();
                        xslt.Transform(navNodoTimbre, args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCadenaOriginalEmisor = srDll.ReadToEnd();
                    }
                    catch { }

                    if (!sCadenaOriginalEmisor.Equals("|||"))
                        sVersion = "3.3";
                    else
                    {
                        try
                        {
                            xslt = new XslCompiledTransform();
                            //xslt.Load(typeof(CaOri.V3211));
                            xslt.Load(typeof(CaOri.V40));
                            ms = new MemoryStream();
                            args = new XsltArgumentList();
                            xslt.Transform(navNodoTimbre, args, ms);
                            ms.Seek(0, SeekOrigin.Begin);
                            srDll = new StreamReader(ms);

                            sCadenaOriginalEmisor = srDll.ReadToEnd();
                        }
                        catch { }

                        if (!sCadenaOriginalEmisor.Equals("|||"))
                            sVersion = "4.0";
                    }

                    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(sXmlDocument.NameTable);
                    if (sVersion.Equals("3.3"))
                        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                    else
                        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");

                    nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                    try
                    {
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(Timbrado.V3.TFD11XSLT));
                        ms = new MemoryStream();
                        args = new XsltArgumentList();
                        xslt.Transform(sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);
                        sCadenaTimbre = srDll.ReadToEnd();

                        sCadenaTimbre = "|" + sCadenaTimbre + "||";
                    }
                    catch { }

                    if (!string.IsNullOrEmpty(sCadenaOriginalEmisor))
                        HASHEmisor = GetHASH(sCadenaOriginalEmisor);
                    //MessageBox.Show("HashEmisor: " + HASHEmisor);

                    if (!string.IsNullOrEmpty(sCadenaTimbre))
                        sHashTimbre = GetHASH(sCadenaTimbre).ToString();
                    //MessageBox.Show("HashZip: " + sHashzip);

                    

                    dFechaComprobante = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Fecha", nsmComprobante).ValueAsDateTime;
                    try { dFechaTimbrado = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).ValueAsDateTime; }
                    catch { }
                   

                    try { sSerie = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Serie", nsmComprobante).Value; }
                    catch { }
                    try { sFolio = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Folio", nsmComprobante).Value; }
                    catch { }
                    try { dFechaTimbrado = Convert.ToDateTime(sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value); }
                    catch { }
                    try { sRfcEmisor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@Rfc", nsmComprobante).Value; }
                    catch { }
                    try { sNombreEmisor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@Nombre", nsmComprobante).Value; }
                    catch { }
                    try { sRfcReceptor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@Rfc", nsmComprobante).Value; }
                    catch { }
                    try { sNombreReceptor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@Nombre", nsmComprobante).Value; }
                    catch { }
                    try { dFechaEmision = Convert.ToDateTime(sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Fecha", nsmComprobante).Value); }
                    catch { }
                    try { sTotal = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Total", nsmComprobante).Value; }
                    catch { }
                    try { sMoneda = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobante).Value; }
                    catch { }
                    try { sUUID = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                    catch { }
                    try { sRfcPac = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@RfcProvCertif", nsmComprobante).Value; }
                    catch { }

                    DataTable dtDatosTimbrado = new DataTable();

                    if (!bGrande)
                    {
                        if (sVersion.Equals("3.3"))
                            dtDatosTimbrado = fnObtenerIdUsuarioIdEstructura(sRfcEmisor);
                        else
                            dtDatosTimbrado = fnObtenerIdUsuarioIdEstructura40(sRfcEmisor);
                    }
                    else
                    {
                        if (sVersion.Equals("3.3"))
                            dtDatosTimbrado = fnObtenerIdUsuarioIdEstructuraGrande(sRfcEmisor);
                        else
                            dtDatosTimbrado = fnObtenerIdUsuarioIdEstructuraGrande40(sRfcEmisor);
                    }

                    if (dtDatosTimbrado.Rows.Count <= 0)
                    {
                        if (sVersion.Equals("3.3"))
                            dtDatosTimbrado = fnObtenerIdUsuarioIdEstructuraPAX(sRfcEmisor);
                        else
                            dtDatosTimbrado = fnObtenerIdUsuarioIdEstructuraPAX40(sRfcEmisor);
                    }

                    DataTable dtUsuario = new DataTable();
                    string sUsuario = string.Empty;
                    if (dtDatosTimbrado.Rows.Count > 0)
                    {
                        nIdUsuario = Convert.ToInt32(dtDatosTimbrado.Rows[0]["id_usuario_timbrado"]);
                        nIdEstructura = Convert.ToInt32(dtDatosTimbrado.Rows[0]["id_estructura"]);

                        dtUsuario = fnObtenerUsuario(nIdUsuario);
                        if (dtUsuario.Rows.Count > 0)
                        {
                            sUsuario = dtUsuario.Rows[0]["clave_usuario"].ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(sUsuario))
                    {
                        sUsuario = "0";
                        nIdUsuario = 0;
                    }
                        

                    FileInfo fi = new FileInfo(Settings.Default.RutaPendientes + sNombreArchivo);

                    DataRow drRenglon = dtComprobantes.NewRow();
                    drRenglon["IdUsuario"] = nIdUsuario;
                    drRenglon["Usuario"] = sUsuario;
                    drRenglon["IdEstructura"] = nIdEstructura;
                    drRenglon["UUID"] = sUUID;
                    drRenglon["Emisor"] = sRfcEmisor;
                    drRenglon["FechaTimbrado"] = dFechaTimbrado;
                    drRenglon["Tamanio"] = (fi.Length) / 1024;
                    drRenglon["Version"] = sVersion;

                    //if (nIdUsuario <= 0 || nIdEstructura <= 0)
                    //{
                    //    dtComprobantes.Rows.Add(drRenglon);
                    //    continue;
                    //}

                    if (!bGrande)
                    {
                        if (sVersion.Equals("3.3"))
                        {
                            if (fnExisteComprobanteSAN(sUUID).Rows.Count > 0)
                            {
                                drRenglon["Existe"] = "Existe UUID";
                            }
                            else if (fnExisteComprobante(HASHEmisor, nIdUsuario, "Emisor"))
                            {
                                drRenglon["Existe"] = "Existe HASH Emisor";
                            }
                            else
                            {
                                drRenglon["Existe"] = "No existe";
                            }
                        }
                        else
                        {
                            if (fnExisteComprobanteSAN(sUUID).Rows.Count > 0)
                            {
                                drRenglon["Existe"] = "Existe UUID";
                            }
                            else if (fnExisteComprobante40(HASHEmisor, nIdUsuario, "Emisor"))
                            {
                                drRenglon["Existe"] = "Existe HASH Emisor";
                            }
                            else
                            {
                                drRenglon["Existe"] = "No existe";
                            }
                        }
                    }
                    else
                    {
                        if (sVersion.Equals("3.3"))
                        {
                            if (fnExisteComprobanteSANGrande(sUUID).Rows.Count > 0)
                            {
                                drRenglon["Existe"] = "Existe UUID";
                            }
                            else if (fnExisteComprobanteGrande(HASHEmisor, nIdUsuario, "Emisor"))
                            {
                                drRenglon["Existe"] = "Existe HASH Emisor";
                            }
                            else
                            {
                                drRenglon["Existe"] = "No existe";
                            }
                        }
                        else
                        {
                            if (fnExisteComprobanteSANGrande(sUUID).Rows.Count > 0)
                            {
                                drRenglon["Existe"] = "Existe UUID";
                            }
                            else if (fnExisteComprobanteGrande40(HASHEmisor, nIdUsuario, "Emisor"))
                            {
                                drRenglon["Existe"] = "Existe HASH Emisor";
                            }
                            else
                            {
                                drRenglon["Existe"] = "No existe";
                            }
                        }                        
                    }

                    

                    drRenglon["PAC"] = sRfcPac;

                    //drRenglon["Existe"] = "No existe";

                    dtComprobantes.Rows.Add(drRenglon);

                    File.Delete(Settings.Default.RutaPendientes + sNombreArchivo);
                }
                catch (Exception ex)
                {
                    
                }
            }
           
           
            dgvComprobantes.DataSource = dtComprobantes;
            fnAgregarRowNumber(dgvComprobantes);
        }
        private void btnEntregarArchivos_Click(object sender, EventArgs e)
        {
            DateTime dFechaComprobante;
            DateTime dFechaTimbrado;
            int nContador = 0;
            string sCadenaOriginalEmisor = string.Empty;
            string sCadenaOriginal = string.Empty;
            string HASHEmisor = string.Empty;
            string sEstatus = string.Empty;
            System.Diagnostics.EventLog eventLogExport = new System.Diagnostics.EventLog();

            DataTable dtComprobantes = new DataTable();           
            dtComprobantes.Columns.Add("IdUsuario", typeof(int));
            dtComprobantes.Columns.Add("Usuario", typeof(string));
            dtComprobantes.Columns.Add("IdEstructura", typeof(int));
            dtComprobantes.Columns.Add("UUID", typeof(string));
            dtComprobantes.Columns.Add("Estatus", typeof(string));
            
            txtResultado.Text = string.Empty;
            dgvComprobantes.DataSource = null;

            int nArchivos = lbComprobantes.Items.Count;
            string[,] asArchivos = new string[2, nArchivos];

            foreach (string nombreArchivo in lbComprobantes.Items)
            {
                int nId_Zip = 0;
                string sHashzip = string.Empty;
                string sNombreArchivo = Path.GetFileName(nombreArchivo);
                string sNombreZip = string.Empty;
                string sComprobante = string.Empty;

                File.Copy(nombreArchivo, Settings.Default.RutaPendientes + sNombreArchivo);

                asArchivos[0, nContador] = sNombreArchivo;

                asArchivos[1, nContador] = sNombreZip;

                var contenidoArchivo = fnRecuperaArchivo(Settings.Default.RutaPendientes + sNombreArchivo);
                byte[] contenidoArchivoBytes = fnRecuperaArchivoByte(contenidoArchivo, Settings.Default.RutaPendientes + sNombreArchivo);

                contenidoArchivo.Close();

                XmlDocument sXmlDocument = new XmlDocument();
                try
                {
                    sXmlDocument.Load(Settings.Default.RutaPendientes + sNombreArchivo);
                }
                catch (Exception ex)
                {
                    sComprobante = File.ReadAllText(Settings.Default.RutaPendientes + sNombreArchivo);
                    int nIndexFinalCfdi = sComprobante.IndexOf("</cfdi:Comprobante>");

                    sComprobante = sComprobante.Substring(0, nIndexFinalCfdi + 19);

                    sXmlDocument.LoadXml(sComprobante);
                }

                bool bGrande = false;

                Double TamañoXML = Convert.ToDouble(contenidoArchivoBytes.Length / 1024.0);

                if (TamañoXML >= Convert.ToDouble(Settings.Default.LimiteInfGrandes) && TamañoXML <= Convert.ToDouble(Settings.Default.LimiteSupGrandes))
                    bGrande = true;

                //if (contenidoArchivoBytes.Length > 200)
                //    bGrande = true;                

                XPathNavigator navNodoTimbre = sXmlDocument.CreateNavigator();

                XslCompiledTransform xslt;
                XsltArgumentList args;
                MemoryStream ms;
                StreamReader srDll;

                string sRfcPac = string.Empty;
                string sUUID = string.Empty;
                string sRfcEmisor = string.Empty;
                string sRfcReceptor = string.Empty;
                string sCadenaTimbre = string.Empty;
                string sHashTimbre = string.Empty;
                string sNombreEmisor = string.Empty;
                string sNombreReceptor = string.Empty;
                string sMoneda = string.Empty;
                string sSerie = string.Empty;
                string sFolio = string.Empty;
                string sTotal = string.Empty;
                string sVersion = string.Empty;
                string sTipoDeComprobante = string.Empty;
                DateTime dFechaEmision = new DateTime();
                int nIdEstructura = 0;
                int nIdUsuario = 0;

                // Hash Emisor
                xslt = new XslCompiledTransform();
                //xslt.Load(typeof(CaOri.V3211));
                xslt.Load(typeof(CaOri.V33));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(navNodoTimbre, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);

                sCadenaOriginalEmisor = srDll.ReadToEnd();

                if (!sCadenaOriginalEmisor.Equals("|||"))
                    sVersion = "3.3";
                else
                {
                    xslt = new XslCompiledTransform();
                    //xslt.Load(typeof(CaOri.V3211));
                    xslt.Load(typeof(CaOri.V40));
                    ms = new MemoryStream();
                    args = new XsltArgumentList();
                    xslt.Transform(navNodoTimbre, args, ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    srDll = new StreamReader(ms);

                    sCadenaOriginalEmisor = srDll.ReadToEnd();

                    if (!sCadenaOriginalEmisor.Equals("|||"))
                        sVersion = "4.0";
                }

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(sXmlDocument.NameTable);
                if (sVersion.Equals("3.3"))
                    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                else
                    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");

                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                nsmComprobante.AddNamespace("nomina12", "http://www.sat.gob.mx/nomina12");

                xslt = new XslCompiledTransform();
                xslt.Load(typeof(Timbrado.V3.TFD11XSLT));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);
                sCadenaTimbre = srDll.ReadToEnd();

                sCadenaTimbre = "|" + sCadenaTimbre + "||";

                HASHEmisor = GetHASH(sCadenaOriginalEmisor);
                //MessageBox.Show("HashEmisor: " + HASHEmisor);

                sHashTimbre = GetHASH(sCadenaTimbre).ToString();
                //MessageBox.Show("HashZip: " + sHashzip);

               
                dFechaComprobante = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Fecha", nsmComprobante).ValueAsDateTime;
                dFechaTimbrado = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).ValueAsDateTime;

                try { sSerie = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Serie", nsmComprobante).Value; }
                catch { }
                try { sFolio = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Folio", nsmComprobante).Value; }
                catch { }
                try { dFechaTimbrado = Convert.ToDateTime(sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value); }
                catch { }
                try { sRfcEmisor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@Rfc", nsmComprobante).Value; }
                catch { }
                try { sNombreEmisor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@Nombre", nsmComprobante).Value; }
                catch { }
                try { sRfcReceptor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@Rfc", nsmComprobante).Value; }
                catch { }
                try { sNombreReceptor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@Nombre", nsmComprobante).Value; }
                catch { }
                try { dFechaEmision = Convert.ToDateTime(sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Fecha", nsmComprobante).Value); }
                catch { }
                try { sTotal = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Total", nsmComprobante).Value; }
                catch { }
                try { sMoneda = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobante).Value; }
                catch { }
                try { sUUID = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                catch { }
                try { sRfcPac = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@RfcProvCertif", nsmComprobante).Value; }
                catch { }
                try { sTipoDeComprobante = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@TipoDeComprobante", nsmComprobante).Value; }
                catch { }

                DataTable dtDatosTimbrado = new DataTable();

                if (!bGrande)
                {
                    if (sVersion.Equals("3.3"))
                        dtDatosTimbrado = fnObtenerIdUsuarioIdEstructura(sRfcEmisor);
                    else
                        dtDatosTimbrado = fnObtenerIdUsuarioIdEstructura40(sRfcEmisor);
                }
                else
                {
                    if (sVersion.Equals("3.3"))
                        dtDatosTimbrado = fnObtenerIdUsuarioIdEstructuraGrande(sRfcEmisor);
                    else
                        dtDatosTimbrado = fnObtenerIdUsuarioIdEstructuraGrande40(sRfcEmisor);
                }

                if (dtDatosTimbrado.Rows.Count <= 0)
                {
                    if (sVersion.Equals("3.3"))
                        dtDatosTimbrado = fnObtenerIdUsuarioIdEstructuraPAX(sRfcEmisor);
                    else
                        dtDatosTimbrado = fnObtenerIdUsuarioIdEstructuraPAX40(sRfcEmisor);
                }

                DataTable dtUsuario = new DataTable();
                string sUsuario = string.Empty;

                if (dtDatosTimbrado.Rows.Count > 0)
                {
                    nIdUsuario = Convert.ToInt32(dtDatosTimbrado.Rows[0]["id_usuario_timbrado"]);
                    nIdEstructura = Convert.ToInt32(dtDatosTimbrado.Rows[0]["id_estructura"]);

                    dtUsuario = fnObtenerUsuario(nIdUsuario);
                    if (dtUsuario.Rows.Count > 0)
                    {
                        sUsuario = dtUsuario.Rows[0]["clave_usuario"].ToString();
                    }
                }

                DataRow drRenglon = dtComprobantes.NewRow();
                drRenglon["IdUsuario"] = nIdUsuario;
                drRenglon["IdEstructura"] = nIdEstructura;
                drRenglon["UUID"] = sUUID;
                drRenglon["Usuario"] = sUsuario;

                try
                {
                    string sTipoDeComplemento = fnRecuperaComplemento40(sXmlDocument);

                    sComprobante = sXmlDocument.InnerXml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "").Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");

                    string sInstruccion = string.Empty;

                    if (nIdEstructura.Equals(0))
                        nIdEstructura = 53592;

                    if (nIdUsuario.Equals(0))
                        nIdUsuario = 55179;

                    if (!bGrande)
                    {
                        if (sVersion.Equals("3.3"))
                        {
                            if (nIdUsuario <= 0 || nIdEstructura <= 0)
                            {
                                txtResultado.Text += "Comprobante " + sNombreArchivo + " sin ID Usuario ni ID Estructura." + System.Environment.NewLine;
                                drRenglon["Estatus"] = "No Procesado";
                            }
                            else if (fnExisteComprobante(HASHEmisor, nIdUsuario, "Emisor"))
                            {
                                txtResultado.Text += "Comprobante " + sNombreArchivo + " existente." + System.Environment.NewLine;
                                drRenglon["Estatus"] = "No Procesado";
                            }
                            else if (fnExisteComprobanteSAN(sUUID).Rows.Count > 0)
                            {
                                sInstruccion = "Comprobante existente: Comprobante: {0}, Fecha timbrado: {1}, Hash: {2}";
                                drRenglon["Estatus"] = "No Procesado";
                            }
                            else
                            {
                                fnInsertarComprobanteSAN(sComprobante, 1, 'X', dFechaEmision, nIdEstructura, nIdUsuario, cbOrigen.SelectedItem.ToString(), sHashTimbre, HASHEmisor,
                                    sUUID, dFechaTimbrado, sRfcEmisor, sNombreEmisor, sRfcReceptor, sNombreReceptor, dFechaEmision, sSerie, sFolio, sTotal, sMoneda, Convert.ToString(3));

                                sInstruccion = "Datos Registrados: Comprobante: {0}, Fecha timbrado: {1}, Hash: {2}";
                                drRenglon["Estatus"] = "Procesado";
                            }
                        }
                        else
                        {
                            if (nIdUsuario <= 0 || nIdEstructura <= 0)
                            {
                                txtResultado.Text += "Comprobante " + sNombreArchivo + " sin ID Usuario ni ID Estructura." + System.Environment.NewLine;
                                drRenglon["Estatus"] = "No Procesado";
                            }
                            else if (fnExisteComprobante40(HASHEmisor, nIdUsuario, "Emisor"))
                            {
                                txtResultado.Text += "Comprobante " + sNombreArchivo + " existente." + System.Environment.NewLine;
                                drRenglon["Estatus"] = "No Procesado";
                            }
                            else if (fnExisteComprobanteSAN40(sUUID).Rows.Count > 0)
                            {
                                sInstruccion = "Comprobante existente: Comprobante: {0}, Fecha timbrado: {1}, Hash: {2}";
                                drRenglon["Estatus"] = "No Procesado";
                            }
                            else
                            {
                                fnInsertarComprobanteSAN40(sComprobante, 1, 'X', dFechaEmision, nIdEstructura, nIdUsuario, cbOrigen.SelectedItem.ToString(), sHashTimbre, HASHEmisor,
                                    sUUID, dFechaTimbrado, sRfcEmisor, sNombreEmisor, sRfcReceptor, sNombreReceptor, dFechaEmision, sSerie, sFolio, sTotal, sTipoDeComprobante, sTipoDeComplemento, 
                                    sMoneda, Convert.ToString(3));

                                sInstruccion = "Datos Registrados: Comprobante: {0}, Fecha timbrado: {1}, Hash: {2}";
                                drRenglon["Estatus"] = "Procesado";
                            }
                        }
                    }
                    else
                    {
                        if (sVersion.Equals("3.3"))
                        {
                            if (nIdUsuario <= 0 && nIdEstructura <= 0)
                            {
                                txtResultado.Text += "Comprobante " + sNombreArchivo + " sin ID Usuario ni ID Estructura." + System.Environment.NewLine;
                                drRenglon["Estatus"] = "No Procesado";
                            }
                            else if (fnExisteComprobanteGrande(HASHEmisor, nIdUsuario, "Emisor"))
                            {
                                txtResultado.Text += "Comprobante " + sNombreArchivo + " existente." + System.Environment.NewLine;
                                drRenglon["Estatus"] = "No Procesado";
                            }
                            else if (fnExisteComprobanteSANGrande(sUUID).Rows.Count > 0)
                            {
                                sInstruccion = "Comprobante existente: Comprobante: {0}, Fecha timbrado: {1}, Hash: {2}";
                                drRenglon["Estatus"] = "No Procesado";
                            }
                            else
                            {
                                fnInsertarComprobanteSANGrande(sComprobante, 1, 'X', dFechaEmision, nIdEstructura, nIdUsuario, cbOrigen.SelectedItem.ToString(), sHashTimbre, HASHEmisor,
                                   sUUID, dFechaTimbrado, sRfcEmisor, sNombreEmisor, sRfcReceptor, sNombreReceptor, dFechaEmision, sSerie, sFolio, sTotal, sMoneda, Convert.ToString(3));

                                sInstruccion = "Datos Registrados: Comprobante: {0}, Fecha timbrado: {1}, Hash: {2}";
                                drRenglon["Estatus"] = "Procesado";
                            }
                        }
                        else
                        {
                            if (nIdUsuario <= 0 && nIdEstructura <= 0)
                            {
                                txtResultado.Text += "Comprobante " + sNombreArchivo + " sin ID Usuario ni ID Estructura." + System.Environment.NewLine;
                                drRenglon["Estatus"] = "No Procesado";
                            }
                            else if (fnExisteComprobanteGrande40(HASHEmisor, nIdUsuario, "Emisor"))
                            {
                                txtResultado.Text += "Comprobante " + sNombreArchivo + " existente." + System.Environment.NewLine;
                                drRenglon["Estatus"] = "No Procesado";
                            }
                            else if (fnExisteComprobanteSANGrande40(sUUID).Rows.Count > 0)
                            {
                                sInstruccion = "Comprobante existente: Comprobante: {0}, Fecha timbrado: {1}, Hash: {2}";
                                drRenglon["Estatus"] = "No Procesado";
                            }
                            else
                            {
                                fnInsertarComprobanteSAN40Grande(sComprobante, 1, 'X', dFechaEmision, nIdEstructura, nIdUsuario, cbOrigen.SelectedItem.ToString(), sHashTimbre, HASHEmisor,
                                    sUUID, dFechaTimbrado, sRfcEmisor, sNombreEmisor, sRfcReceptor, sNombreReceptor, dFechaEmision, sSerie, sFolio, sTotal, sTipoDeComprobante, sTipoDeComplemento,
                                    sMoneda, Convert.ToString(3));

                                sInstruccion = "Datos Registrados: Comprobante: {0}, Fecha timbrado: {1}, Hash: {2}";
                                drRenglon["Estatus"] = "Procesado";
                            }
                        }
                    }

                    



                    txtResultado.Text += string.Format(sInstruccion, sNombreArchivo, dFechaTimbrado, HASHEmisor) + System.Environment.NewLine;

                    File.Delete(Settings.Default.RutaPendientes + sNombreArchivo);

                    dtComprobantes.Rows.Add(drRenglon);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error al insertar el comprobante: " + sNombreArchivo + " - " + ex.Message);
                    txtResultado.Text += "Comprobante " + sNombreArchivo + " no existente. Zip:" + nId_Zip + ", Nombre Zip: " + sNombreZip + " FecharTimbrado: " + dFechaTimbrado + " HashEmisor: " + HASHEmisor + System.Environment.NewLine;
                }
                

                nContador++;
            }

            dgvComprobantes.DataSource = dtComprobantes;
        }
        private void btnSeleccionarComprobantes_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdComprobantes = new OpenFileDialog();
            ofdComprobantes.Multiselect = true;
            ofdComprobantes.AddExtension = true;
            ofdComprobantes.DefaultExt = "*.xml";
            ofdComprobantes.ShowDialog();

            if (string.IsNullOrEmpty(ofdComprobantes.FileName))
            {
                btnEntregarArchivos.Enabled = false;
                return;
            }

            lbComprobantes.DataSource = ofdComprobantes.FileNames;

            if (lbComprobantes.Items.Count > 0)
                btnEntregarArchivos.Enabled = true;
        }
        private void frmEntregaPendientesSAN_Load(object sender, EventArgs e)
        {
            btnEntregarArchivos.Enabled = false;
            /*
            DataTable dtElemento = new DataTable();
            dtElemento.Columns.Add("Origen", typeof(string));
            dtElemento.Columns.Add("Descripcion", typeof(string));

            DataRow drRenglonWs = dtElemento.NewRow();
            drRenglonWs["Origen"] = "R";
            drRenglonWs["Descripcion"] = "WebService";
            dtElemento.Rows.Add(drRenglonWs);

            DataRow drRenglonP = dtElemento.NewRow();
            drRenglonP["Origen"] = "C";
            drRenglonP["Descripcion"] = "Portal Cobro";
            dtElemento.Rows.Add(drRenglonP);

            cbOrigen.DataSource = dtElemento;
            */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgv"></param>
        private void fnAgregarRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }

            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        /// <summary>
        /// Metodo que se encarga de revisar si un comprobante ya se encuentra registrado por medio del Hash del Emisor
        /// </summary>
        /// <param name="psHashEmisor">Hash del Emisor</param>
        /// <param name="sNombreArchivoZip">Nombre del Zip</param>
        /// <param name="sNombreXML">Nombre XML</param>
        /// <returns></returns>
        private DataTable fnExisteComprobanteSAN(string psUuid)
        {
            DataTable dtResultado = new DataTable();

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac_Comprobantes_sel_Uuid", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sUuid", psUuid);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {
                            da.Fill(dtResultado);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("No fue posible buscar el hash del comprobante: " + psUuid + " - " + ex.Message);
                }
            }
            return dtResultado;
        }

        /// <summary>
        /// Metodo que se encarga de revisar si un comprobante ya se encuentra registrado por medio del Hash del Emisor
        /// </summary>
        /// <param name="psHashEmisor">Hash del Emisor</param>
        /// <param name="sNombreArchivoZip">Nombre del Zip</param>
        /// <param name="sNombreXML">Nombre XML</param>
        /// <returns></returns>
        private DataTable fnExisteComprobanteSAN40(string psUuid)
        {
            DataTable dtResultado = new DataTable();

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac40_Comprobantes_sel_Uuid", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sUuid", psUuid);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {
                            da.Fill(dtResultado);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("No fue posible buscar el hash del comprobante: " + psUuid + " - " + ex.Message);
                }
            }
            return dtResultado;
        }

        /// <summary>
        /// Metodo que se encarga de revisar si un comprobante ya se encuentra registrado por medio del Hash del Emisor
        /// </summary>
        /// <param name="psHashEmisor">Hash del Emisor</param>
        /// <param name="sNombreArchivoZip">Nombre del Zip</param>
        /// <param name="sNombreXML">Nombre XML</param>
        /// <returns></returns>
        private DataTable fnExisteComprobanteSANGrande(string psUuid)
        {
            DataTable dtResultado = new DataTable();

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.ConexionGrandes)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac_ComprobantesWS_sel_Uuid", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sUuid", psUuid);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {
                            da.Fill(dtResultado);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("No fue posible buscar el hash del comprobante: " + psUuid + " - " + ex.Message);
                }
            }
            return dtResultado;
        }

        /// <summary>
        /// Metodo que se encarga de revisar si un comprobante ya se encuentra registrado por medio del Hash del Emisor
        /// </summary>
        /// <param name="psHashEmisor">Hash del Emisor</param>
        /// <param name="sNombreArchivoZip">Nombre del Zip</param>
        /// <param name="sNombreXML">Nombre XML</param>
        /// <returns></returns>
        private DataTable fnExisteComprobanteSANGrande40(string psUuid)
        {
            DataTable dtResultado = new DataTable();

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.ConexionGrandes)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac40_ComprobantesWS_sel_Uuid", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sUuid", psUuid);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {
                            da.Fill(dtResultado);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("No fue posible buscar el hash del comprobante: " + psUuid + " - " + ex.Message);
                }
            }
            return dtResultado;
        }

        /// <summary>
        /// Metodo que se encarga de revisar si un comprobante ya se encuentra registrado por medio del Hash del Emisor
        /// </summary>
        /// <param name="psHashEmisor">Hash del Emisor</param>
        /// <param name="sNombreArchivoZip">Nombre del Zip</param>
        /// <param name="sNombreXML">Nombre XML</param>
        /// <returns></returns>
        private bool fnExisteComprobante(string psHashEmisor, int pnIdUsuario, string psTipo)
        {
            bool bResultado = false;
            string nComprobante = string.Empty;
            string sHashEmisor = string.Empty;
            try
            {
                nComprobante = fnObtenerHashComprobante(psHashEmisor, pnIdUsuario, psTipo);
                if (!nComprobante.Equals("0"))
                {
                    bResultado = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible buscar el hash del comprobante: " + psHashEmisor + " - " + ex.Message);
            }
            return bResultado;
        }

        /// <summary>
        /// Metodo que se encarga de revisar si un comprobante ya se encuentra registrado por medio del Hash del Emisor
        /// </summary>
        /// <param name="psHashEmisor">Hash del Emisor</param>
        /// <param name="sNombreArchivoZip">Nombre del Zip</param>
        /// <param name="sNombreXML">Nombre XML</param>
        /// <returns></returns>
        private bool fnExisteComprobante40(string psHashEmisor, int pnIdUsuario, string psTipo)
        {
            bool bResultado = false;
            string nComprobante = string.Empty;
            string sHashEmisor = string.Empty;
            try
            {
                nComprobante = fnObtenerHashComprobante40(psHashEmisor, pnIdUsuario, psTipo);
                if (!nComprobante.Equals("0"))
                {
                    bResultado = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible buscar el hash del comprobante: " + psHashEmisor + " - " + ex.Message);
            }
            return bResultado;
        }

        /// <summary>
        /// Metodo que se encarga de revisar si un comprobante ya se encuentra registrado por medio del Hash del Emisor
        /// </summary>
        /// <param name="psHashEmisor">Hash del Emisor</param>
        /// <param name="sNombreArchivoZip">Nombre del Zip</param>
        /// <param name="sNombreXML">Nombre XML</param>
        /// <returns></returns>
        private bool fnExisteComprobanteGrande(string psHashEmisor, int pnIdUsuario, string psTipo)
        {
            bool bResultado = false;
            string nComprobante = string.Empty;
            string sHashEmisor = string.Empty;
            try
            {
                nComprobante = fnObtenerHashComprobanteGrande(psHashEmisor, pnIdUsuario, psTipo);
                if (!nComprobante.Equals("0"))
                {
                    bResultado = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible buscar el hash del comprobante: " + psHashEmisor + " - " + ex.Message);
            }
            return bResultado;
        }

        /// <summary>
        /// Metodo que se encarga de revisar si un comprobante ya se encuentra registrado por medio del Hash del Emisor
        /// </summary>
        /// <param name="psHashEmisor">Hash del Emisor</param>
        /// <param name="sNombreArchivoZip">Nombre del Zip</param>
        /// <param name="sNombreXML">Nombre XML</param>
        /// <returns></returns>
        private bool fnExisteComprobanteGrande40(string psHashEmisor, int pnIdUsuario, string psTipo)
        {
            bool bResultado = false;
            string nComprobante = string.Empty;
            string sHashEmisor = string.Empty;
            try
            {
                nComprobante = fnObtenerHashComprobanteGrande40(psHashEmisor, pnIdUsuario, psTipo);
                if (!nComprobante.Equals("0"))
                {
                    bResultado = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible buscar el hash del comprobante: " + psHashEmisor + " - " + ex.Message);
            }
            return bResultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string GetHASH(string text)
        {
            byte[] hashValue;
            byte[] message = Encoding.UTF8.GetBytes(text);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            } return hex;
        }

        /// <summary>
        /// Agrega el comprobante a la BD
        /// </summary>
        /// <param name="sXML">Comprobante</param>
        /// <param name="nId_tipo_documento">Tipo de documento</param>
        /// <param name="cEstatus">estatus de generacion</param>
        /// <param name="dFecha_Documento">fecha de generacion</param>
        /// <param name="nId_estructura">id de estructura</param>
        /// <param name="nId_usuario_timbrado">id de usuario que genera</param>
        /// <param name="nSerie">Serie a generar el folio</param>
        /// <returns></returns>
        public static int fnInsertarComprobanteSAN(string psComprobante, int pnIdTipoDocumento, char pcEstatus, DateTime pdFechaDocumento, int pnIdEstructura, int pnIdUsuarioTimbrado, string psOrigen,
            string psHASHTimbre, string psHASHEmisor, string psUUID, DateTime pdFechaTimbrado, string psRfcEmisor, string psNombreEmisor, string psRfcReceptor, string psNombreReceptor,
            DateTime pdFechaEmision, string psSerie, string psFolio, string psTotal, string psMoneda, string psIdPac)
        {

            string cadenaCon = Settings.Default.Conexion;
            int nRetorno = 0;
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(cadenaCon)))
            {
                //int nRetorno = 0;
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_mpac_Timbrado_InsertaComprobanteAll_Completo_Ins", con))
                        {

                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("sXML", PAXCrypto.CryptoAES.EncriptaAES(psComprobante));
                            cmd.Parameters.AddWithValue("nId_tipo_documento", pnIdTipoDocumento);
                            cmd.Parameters.AddWithValue("cEstatus", pcEstatus);
                            cmd.Parameters.AddWithValue("dFecha_Documento", pdFechaDocumento);
                            cmd.Parameters.AddWithValue("nId_estructura", pnIdEstructura);
                            cmd.Parameters.AddWithValue("nId_usuario_timbrado", pnIdUsuarioTimbrado);
                            cmd.Parameters.AddWithValue("sOrigen", psOrigen);
                            cmd.Parameters.AddWithValue("sHash", psHASHTimbre.ToUpper());
                            cmd.Parameters.AddWithValue("sDatos", psHASHEmisor.ToUpper());
                            cmd.Parameters.AddWithValue("sUuid", psUUID);
                            cmd.Parameters.AddWithValue("dFecha_Timbrado", pdFechaTimbrado);
                            cmd.Parameters.AddWithValue("sRFC_Emisor", psRfcEmisor);
                            cmd.Parameters.AddWithValue("sNombre_Emisor", psNombreEmisor);
                            cmd.Parameters.AddWithValue("sRFC_Receptor", psRfcReceptor);
                            cmd.Parameters.AddWithValue("sNombre_Receptor", psNombreReceptor);
                            cmd.Parameters.AddWithValue("dFecha_Emision", pdFechaEmision);
                            cmd.Parameters.AddWithValue("nSerie", psSerie);
                            cmd.Parameters.AddWithValue("sSerie", psSerie);
                            cmd.Parameters.AddWithValue("sFolio", psFolio);
                            cmd.Parameters.AddWithValue("nTotal", PAXCrypto.CryptoAES.EncriptaAES(psTotal));
                            cmd.Parameters.AddWithValue("sMoneda", psMoneda);
                            cmd.Parameters.AddWithValue("nIdPac", psIdPac);

                            nRetorno = Convert.ToInt32(cmd.ExecuteScalar());

                            tran.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception("No se ha podido insertar el comprobante: " + ex.Message);
                    }
                    finally
                    {
                        //tran.Commit();
                        con.Close();
                    }
                }
            }
            return nRetorno;
        }

        /// <summary>
        /// Agrega el comprobante a la BD
        /// </summary>
        /// <param name="sXML">Comprobante</param>
        /// <param name="nId_tipo_documento">Tipo de documento</param>
        /// <param name="cEstatus">estatus de generacion</param>
        /// <param name="dFecha_Documento">fecha de generacion</param>
        /// <param name="nId_estructura">id de estructura</param>
        /// <param name="nId_usuario_timbrado">id de usuario que genera</param>
        /// <param name="nSerie">Serie a generar el folio</param>
        /// <returns></returns>
        public static int fnInsertarComprobanteSAN40(string psComprobante, int pnIdTipoDocumento, char pcEstatus, DateTime pdFechaDocumento, int pnIdEstructura, int pnIdUsuarioTimbrado,
            string psOrigen, string psHASHTimbre, string psHASHEmisor, string psUUID, DateTime pdFechaTimbrado, string psRfcEmisor, string psNombreEmisor, string psRfcReceptor, string psNombreReceptor,
            DateTime pdFechaEmision, string psSerie, string psFolio, string psTotal, string psTipoComprobante, string psTipoComplemento, string psMoneda, string psIdPac)
        {

            string cadenaCon = Settings.Default.Conexion;
            int nRetorno = 0;
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(cadenaCon)))
            {
                //int nRetorno = 0;
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_mpac40_Timbrado_InsertaComprobanteAll_Completo_Ins", con))
                        {

                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("sXML", PAXCrypto.CryptoAES.EncriptaAES(psComprobante));
                            cmd.Parameters.AddWithValue("nId_tipo_documento", pnIdTipoDocumento);
                            cmd.Parameters.AddWithValue("cEstatus", pcEstatus);
                            cmd.Parameters.AddWithValue("dFecha_Documento", pdFechaDocumento);
                            cmd.Parameters.AddWithValue("nId_estructura", pnIdEstructura);
                            cmd.Parameters.AddWithValue("nId_usuario_timbrado", pnIdUsuarioTimbrado);
                            cmd.Parameters.AddWithValue("sOrigen", psOrigen);
                            cmd.Parameters.AddWithValue("sHash", psHASHTimbre.ToUpper());
                            cmd.Parameters.AddWithValue("sDatos", psHASHEmisor.ToUpper());
                            cmd.Parameters.AddWithValue("sUuid", psUUID);
                            cmd.Parameters.AddWithValue("dFecha_Timbrado", pdFechaTimbrado);
                            cmd.Parameters.AddWithValue("sRFC_Emisor", psRfcEmisor);
                            cmd.Parameters.AddWithValue("sNombre_Emisor", psNombreEmisor);
                            cmd.Parameters.AddWithValue("sRFC_Receptor", psRfcReceptor);
                            cmd.Parameters.AddWithValue("sNombre_Receptor", psNombreReceptor);
                            cmd.Parameters.AddWithValue("dFecha_Emision", pdFechaEmision);
                            cmd.Parameters.AddWithValue("sSerie", psSerie);
                            cmd.Parameters.AddWithValue("sFolio", psFolio);
                            cmd.Parameters.AddWithValue("nTotal", PAXCrypto.CryptoAES.EncriptaAES(psTotal));
                            cmd.Parameters.AddWithValue("sTipoComprobante", psTipoComprobante);
                            cmd.Parameters.AddWithValue("sTipoComplemento", psTipoComplemento);
                            cmd.Parameters.AddWithValue("sMoneda", psMoneda);
                            cmd.Parameters.AddWithValue("nIdPac", psIdPac);

                            nRetorno = Convert.ToInt32(cmd.ExecuteScalar());

                            tran.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception("No se ha podido insertar el comprobante: " + ex.Message);
                    }
                    finally
                    {
                        //tran.Commit();
                        con.Close();
                    }
                }
            }
            return nRetorno;
        }

        /// <summary>
        /// Agrega el comprobante a la BD
        /// </summary>
        /// <param name="sXML">Comprobante</param>
        /// <param name="nId_tipo_documento">Tipo de documento</param>
        /// <param name="cEstatus">estatus de generacion</param>
        /// <param name="dFecha_Documento">fecha de generacion</param>
        /// <param name="nId_estructura">id de estructura</param>
        /// <param name="nId_usuario_timbrado">id de usuario que genera</param>
        /// <param name="nSerie">Serie a generar el folio</param>
        /// <returns></returns>
        public static int fnInsertarComprobanteSANGrande(string psComprobante, int pnIdTipoDocumento, char pcEstatus, DateTime pdFechaDocumento, int pnIdEstructura, int pnIdUsuarioTimbrado,
            string psOrigen, string psHASHTimbre, string psHASHEmisor, string psUUID, DateTime pdFechaTimbrado, string psRfcEmisor, string psNombreEmisor, string psRfcReceptor,
            string psNombreReceptor, DateTime pdFechaEmision, string psSerie, string psFolio, string psTotal, string psMoneda, string psIdPac)
        {

            string cadenaCon = Settings.Default.ConexionGrandes;
            int nRetorno = 0;
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(cadenaCon)))
            {
                //int nRetorno = 0;
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_mpac_Timbrado_InsertaComprobanteAll_Completo_Ins", con))
                        {

                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("sXML", PAXCrypto.CryptoAES.EncriptaAES(psComprobante));
                            cmd.Parameters.AddWithValue("nId_tipo_documento", pnIdTipoDocumento);
                            cmd.Parameters.AddWithValue("cEstatus", pcEstatus);
                            cmd.Parameters.AddWithValue("dFecha_Documento", pdFechaDocumento);

                            if (!pnIdEstructura.Equals(0))
                                cmd.Parameters.AddWithValue("nId_estructura", pnIdEstructura);

                            cmd.Parameters.AddWithValue("nId_usuario_timbrado", pnIdUsuarioTimbrado);
                            cmd.Parameters.AddWithValue("sOrigen", psOrigen);
                            cmd.Parameters.AddWithValue("sHash", psHASHTimbre.ToUpper());
                            cmd.Parameters.AddWithValue("sDatos", psHASHEmisor.ToUpper());
                            cmd.Parameters.AddWithValue("sUuid", psUUID);
                            cmd.Parameters.AddWithValue("dFecha_Timbrado", pdFechaTimbrado);
                            cmd.Parameters.AddWithValue("sRFC_Emisor", psRfcEmisor);
                            cmd.Parameters.AddWithValue("sNombre_Emisor", psNombreEmisor);
                            cmd.Parameters.AddWithValue("sRFC_Receptor", psRfcReceptor);
                            cmd.Parameters.AddWithValue("sNombre_Receptor", psNombreReceptor);
                            cmd.Parameters.AddWithValue("dFecha_Emision", pdFechaEmision);
                            cmd.Parameters.AddWithValue("nSerie", psSerie);
                            cmd.Parameters.AddWithValue("sSerie", psSerie);
                            cmd.Parameters.AddWithValue("sFolio", psFolio);
                            cmd.Parameters.AddWithValue("nTotal", PAXCrypto.CryptoAES.EncriptaAES(psTotal));
                            cmd.Parameters.AddWithValue("sMoneda", psMoneda);
                            cmd.Parameters.AddWithValue("nIdPac", psIdPac);

                            nRetorno = Convert.ToInt32(cmd.ExecuteScalar());

                            tran.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception("No se ha podido insertar el comprobante: " + ex.Message);
                    }
                    finally
                    {
                        //tran.Commit();
                        con.Close();
                    }
                }
            }
            return nRetorno;
        }

        /// <summary>
        /// Agrega el comprobante a la BD
        /// </summary>
        /// <param name="sXML">Comprobante</param>
        /// <param name="nId_tipo_documento">Tipo de documento</param>
        /// <param name="cEstatus">estatus de generacion</param>
        /// <param name="dFecha_Documento">fecha de generacion</param>
        /// <param name="nId_estructura">id de estructura</param>
        /// <param name="nId_usuario_timbrado">id de usuario que genera</param>
        /// <param name="nSerie">Serie a generar el folio</param>
        /// <returns></returns>
        public static int fnInsertarComprobanteSAN40Grande(string psComprobante, int pnIdTipoDocumento, char pcEstatus, DateTime pdFechaDocumento, int pnIdEstructura, int pnIdUsuarioTimbrado, 
            string psOrigen, string psHASHTimbre, string psHASHEmisor, string psUUID, DateTime pdFechaTimbrado, string psRfcEmisor, string psNombreEmisor, string psRfcReceptor, string psNombreReceptor, 
            DateTime pdFechaEmision, string psSerie, string psFolio, string psTotal, string psTipoComprobante, string psTipoComplemento, string psMoneda, string psIdPac)
        {

            string cadenaCon = Settings.Default.ConexionGrandes;
            int nRetorno = 0;
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(cadenaCon)))
            {
                //int nRetorno = 0;
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_mpac40_Timbrado_InsertaComprobanteAll_Completo_Ins", con))
                        {

                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("sXML", PAXCrypto.CryptoAES.EncriptaAES(psComprobante));
                            cmd.Parameters.AddWithValue("nId_tipo_documento", pnIdTipoDocumento);
                            cmd.Parameters.AddWithValue("cEstatus", pcEstatus);
                            cmd.Parameters.AddWithValue("dFecha_Documento", pdFechaDocumento);

                            if (!pnIdEstructura.Equals(0))
                                cmd.Parameters.AddWithValue("nId_estructura", pnIdEstructura);

                            cmd.Parameters.AddWithValue("nId_usuario_timbrado", pnIdUsuarioTimbrado);
                            cmd.Parameters.AddWithValue("sOrigen", psOrigen);
                            cmd.Parameters.AddWithValue("sHash", psHASHTimbre.ToUpper());
                            cmd.Parameters.AddWithValue("sDatos", psHASHEmisor.ToUpper());
                            cmd.Parameters.AddWithValue("sUuid", psUUID);
                            cmd.Parameters.AddWithValue("dFecha_Timbrado", pdFechaTimbrado);
                            cmd.Parameters.AddWithValue("sRFC_Emisor", psRfcEmisor);
                            cmd.Parameters.AddWithValue("sNombre_Emisor", psNombreEmisor);
                            cmd.Parameters.AddWithValue("sRFC_Receptor", psRfcReceptor);
                            cmd.Parameters.AddWithValue("sNombre_Receptor", psNombreReceptor);
                            cmd.Parameters.AddWithValue("dFecha_Emision", pdFechaEmision);
                            cmd.Parameters.AddWithValue("sSerie", psSerie);
                            cmd.Parameters.AddWithValue("nSerie", Convert.ToInt32(0));
                            cmd.Parameters.AddWithValue("sFolio", psFolio);
                            cmd.Parameters.AddWithValue("nTotal", PAXCrypto.CryptoAES.EncriptaAES(psTotal));
                            cmd.Parameters.AddWithValue("sTipoComprobante", psTipoComprobante);
                            cmd.Parameters.AddWithValue("sTipoComplemento", psTipoComplemento);
                            cmd.Parameters.AddWithValue("sMoneda", psMoneda);
                            cmd.Parameters.AddWithValue("nIdPac", psIdPac);

                            nRetorno = Convert.ToInt32(cmd.ExecuteScalar());

                            tran.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception("No se ha podido insertar el comprobante: " + ex.Message);
                    }
                    finally
                    {
                        //tran.Commit();
                        con.Close();
                    }
                }
            }
            return nRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psNombreArchivo"></param>
        /// <returns></returns>
        private DataTable fnObtenerIdUsuarioIdEstructuraPAX(string psRFCEmisor)
        {
            DataTable dtResultado = new DataTable();

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
            {
                conexion.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("usp_cfd33_Comprobantes_sel_RfcEmisor", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sRfcEmisor", psRFCEmisor);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {

                            da.Fill(dtResultado);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error al buscar el Id Usuario: " + psRFCEmisor + " - " + ex.Message);
                }

            }
            return dtResultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psNombreArchivo"></param>
        /// <returns></returns>
        private DataTable fnObtenerIdUsuarioIdEstructuraPAX40(string psRFCEmisor)
        {
            DataTable dtResultado = new DataTable();

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
            {
                conexion.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("usp_cfd40_Comprobantes_sel_RfcEmisor", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sRfcEmisor", psRFCEmisor);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {

                            da.Fill(dtResultado);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error al buscar el Id Usuario: " + psRFCEmisor + " - " + ex.Message);
                }

            }
            return dtResultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psNombreArchivo"></param>
        /// <returns></returns>
        private DataTable fnObtenerIdUsuarioIdEstructura(string psRFCEmisor)
        {
            DataTable dtResultado = new DataTable();

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
            {
                conexion.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac_Comprobantes_sel_RfcEmisor", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sRfcEmisor", psRFCEmisor);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {

                            da.Fill(dtResultado);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error al buscar el Id Usuario: " + psRFCEmisor + " - " + ex.Message);
                }

            }
            return dtResultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psNombreArchivo"></param>
        /// <returns></returns>
        private DataTable fnObtenerIdUsuarioIdEstructura40(string psRFCEmisor)
        {
            DataTable dtResultado = new DataTable();

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
            {
                conexion.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac40_Comprobantes_sel_RfcEmisor", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sRfcEmisor", psRFCEmisor);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {

                            da.Fill(dtResultado);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error al buscar el Id Usuario: " + psRFCEmisor + " - " + ex.Message);
                }

            }
            return dtResultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psNombreArchivo"></param>
        /// <returns></returns>
        private DataTable fnObtenerIdUsuarioIdEstructuraGrande(string psRFCEmisor)
        {
            DataTable dtResultado = new DataTable();

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.ConexionGrandes)))
            {
                conexion.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac_ComprobantesWS_sel_RfcEmisor", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sRfcEmisor", psRFCEmisor);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {

                            da.Fill(dtResultado);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error al buscar el Id Usuario: " + psRFCEmisor + " - " + ex.Message);
                }

            }
            return dtResultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psNombreArchivo"></param>
        /// <returns></returns>
        private DataTable fnObtenerIdUsuarioIdEstructuraGrande40(string psRFCEmisor)
        {
            DataTable dtResultado = new DataTable();

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.ConexionGrandes)))
            {
                conexion.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac40_ComprobantesWS_sel_RfcEmisor", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sRfcEmisor", psRFCEmisor);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {

                            da.Fill(dtResultado);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error al buscar el Id Usuario: " + psRFCEmisor + " - " + ex.Message);
                }

            }
            return dtResultado;
        }

        /// <summary>
        /// Sobrecarga que realize para poder escribir en el log de xml en caso de error
        /// </summary>
        /// <param name="psHashEmisor"></param>
        /// <param name="sNombreArchivoZip"></param>
        /// <param name="sNombreXML"></param>
        /// <returns></returns>
        private string fnObtenerHashComprobante(string psHashEmisor, int pnIdUsuario, string psTipo)
        {
            string nRetorno = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
                {
                    conexion.Open();
                    // Se busca el comprobante 
                    using (SqlCommand comando = new SqlCommand("usp_mpac_Timbrado_BuscaHASH_XML_Sel", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@sHash", psHashEmisor);
                        comando.Parameters.AddWithValue("@nId_usuario_timbrado", pnIdUsuario);
                        comando.Parameters.AddWithValue("@sTipo", psTipo);
                        nRetorno = Convert.ToString(comando.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible buscar el Hash: " + psHashEmisor + " - " + psHashEmisor + " - " + ex.Message);
            }
            return nRetorno;
        }

        /// <summary>
        /// Sobrecarga que realize para poder escribir en el log de xml en caso de error
        /// </summary>
        /// <param name="psHashEmisor"></param>
        /// <param name="sNombreArchivoZip"></param>
        /// <param name="sNombreXML"></param>
        /// <returns></returns>
        private string fnObtenerHashComprobante40(string psHashEmisor, int pnIdUsuario, string psTipo)
        {
            string nRetorno = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
                {
                    conexion.Open();
                    // Se busca el comprobante 
                    using (SqlCommand comando = new SqlCommand("usp_mpac40_Timbrado_BuscaHASH_XML_Sel", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@sHash", psHashEmisor);
                        comando.Parameters.AddWithValue("@nId_usuario_timbrado", pnIdUsuario);
                        comando.Parameters.AddWithValue("@sTipo", psTipo);
                        nRetorno = Convert.ToString(comando.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible buscar el Hash: " + psHashEmisor + " - " + psHashEmisor + " - " + ex.Message);
            }
            return nRetorno;
        }

        /// <summary>
        /// Sobrecarga que realize para poder escribir en el log de xml en caso de error
        /// </summary>
        /// <param name="psHashEmisor"></param>
        /// <param name="sNombreArchivoZip"></param>
        /// <param name="sNombreXML"></param>
        /// <returns></returns>
        private string fnObtenerHashComprobanteGrande(string psHashEmisor, int pnIdUsuario, string psTipo)
        {
            string nRetorno = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.ConexionGrandes)))
                {
                    conexion.Open();
                    // Se busca el comprobante 
                    using (SqlCommand comando = new SqlCommand("usp_mpac_Timbrado_BuscaHASH_XML_Sel", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@sHash", psHashEmisor);
                        comando.Parameters.AddWithValue("@nId_usuario_timbrado", pnIdUsuario);
                        comando.Parameters.AddWithValue("@sTipo", psTipo);
                        nRetorno = Convert.ToString(comando.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible buscar el Hash: " + psHashEmisor + " - " + psHashEmisor + " - " + ex.Message);
            }
            return nRetorno;
        }

        /// <summary>
        /// Sobrecarga que realize para poder escribir en el log de xml en caso de error
        /// </summary>
        /// <param name="psHashEmisor"></param>
        /// <param name="sNombreArchivoZip"></param>
        /// <param name="sNombreXML"></param>
        /// <returns></returns>
        private string fnObtenerHashComprobanteGrande40(string psHashEmisor, int pnIdUsuario, string psTipo)
        {
            string nRetorno = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.ConexionGrandes)))
                {
                    conexion.Open();
                    // Se busca el comprobante 
                    using (SqlCommand comando = new SqlCommand("usp_mpac40_Timbrado_BuscaHASH_XML_Sel", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@sHash", psHashEmisor);
                        comando.Parameters.AddWithValue("@nId_usuario_timbrado", pnIdUsuario);
                        comando.Parameters.AddWithValue("@sTipo", psTipo);
                        nRetorno = Convert.ToString(comando.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible buscar el Hash: " + psHashEmisor + " - " + psHashEmisor + " - " + ex.Message);
            }
            return nRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psNombreArchivo"></param>
        /// <returns></returns>
        private DataTable fnObtenerUsuario(int pnIdUsuario)
        {
            DataTable dtResultado = new DataTable();

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
            {
                conexion.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("usp_Con_ObtenerClaveUsuario_sel", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("nIdUsuario", pnIdUsuario);

                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {

                            da.Fill(dtResultado);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error al buscar el Id Usuario: " + pnIdUsuario + " - " + ex.Message);
                }

            }
            return dtResultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rutaAbsoluta"></param>
        /// <returns></returns>
        private Stream fnRecuperaArchivo(string rutaAbsoluta)
        {
            return File.OpenRead(rutaAbsoluta);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rutaAbsoluta"></param>
        /// <param name="ruta"></param>
        /// <returns></returns>
        private byte[] fnRecuperaArchivoByte(Stream rutaAbsoluta, string ruta)
        {
            StreamReader sr = new StreamReader(rutaAbsoluta);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            return encoding.GetBytes(sr.ReadToEnd());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xdComprobante"></param>
        /// <returns></returns>
        public static String fnRecuperaComplemento40(XmlDocument xdComprobante)
        {
            String xComplementos = "sincomplemento";
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdComprobante.NameTable);
            try
            {
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");
                nsmComprobante.AddNamespace("cce11", "http://www.sat.gob.mx/ComercioExterior11");
                nsmComprobante.AddNamespace("donat", "http://www.sat.gob.mx/donat");
                nsmComprobante.AddNamespace("divisas", "http://www.sat.gob.mx/divisas");
                nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");
                nsmComprobante.AddNamespace("leyendasFisc", "http://www.sat.gob.mx/leyendasFiscales");
                nsmComprobante.AddNamespace("pfic", "http://www.sat.gob.mx/pfic");
                nsmComprobante.AddNamespace("tpe", "http://www.sat.gob.mx/TuristaPasajeroExtranjero");
                nsmComprobante.AddNamespace("nomina12", "http://www.sat.gob.mx/nomina12");
                nsmComprobante.AddNamespace("registrofiscal", "http://www.sat.gob.mx/registrofiscal");
                nsmComprobante.AddNamespace("pagoenespecie", "http://www.sat.gob.mx/pagoenespecie");
                nsmComprobante.AddNamespace("aerolineas", "http://www.sat.gob.mx/aerolineas");
                nsmComprobante.AddNamespace("valesdedespensa", "http://www.sat.gob.mx/valesdedespensa");
                nsmComprobante.AddNamespace("notariospublicos", "http://www.sat.gob.mx/notariospublicos");
                nsmComprobante.AddNamespace("vehiculousado", "http://www.sat.gob.mx/vehiculousado");
                nsmComprobante.AddNamespace("servicioparcial", "http://www.sat.gob.mx/servicioparcialconstruccion");
                nsmComprobante.AddNamespace("obrasarte", "http://www.sat.gob.mx/arteantiguedades");
                nsmComprobante.AddNamespace("ine", "http://www.sat.gob.mx/ine");
                nsmComprobante.AddNamespace("iedu", "http://www.sat.gob.mx/iedu");
                nsmComprobante.AddNamespace("ventavehiculos", "http://www.sat.gob.mx/ventavehiculos");
                nsmComprobante.AddNamespace("detallista", "http://www.sat.gob.mx/detallista");
                nsmComprobante.AddNamespace("ecc12", "http://www.sat.gob.mx/EstadoDeCuentaCombustible12");
                nsmComprobante.AddNamespace("consumodecombustibles11", "http://www.sat.gob.mx/ConsumoDeCombustibles11");
                nsmComprobante.AddNamespace("gceh", "http://www.sat.gob.mx/GastosHidrocarburos10");
                nsmComprobante.AddNamespace("ieeh", "http://www.sat.gob.mx/IngresosHidrocarburos10");
                nsmComprobante.AddNamespace("cartaporte20", "http://www.sat.gob.mx/CartaPorte20");
                nsmComprobante.AddNamespace("pago20", "http://www.sat.gob.mx/Pagos20");

                XPathNavigator xpnNavegador = xdComprobante.CreateNavigator();

                XPathNodeIterator xpnComplementos = xpnNavegador.Select("/cfdi:Comprobante/cfdi:Complemento/*", nsmComprobante);
                XPathNodeIterator xpnComplementosConcepto = xpnNavegador.Select("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto/cfdi:ComplementoConcepto/*", nsmComprobante);

                while (xpnComplementos.MoveNext())
                {
                    switch (xpnComplementos.Current.Name)
                    {
                        case "cce11:ComercioExterior":
                            if (xComplementos == "sincomplemento") xComplementos = "cce11";
                            else if (!xComplementos.Contains("cce11")) xComplementos += xComplementos = "|cce11"; break;
                        case "donat:Donatarias":
                            if (xComplementos == "sincomplemento") xComplementos = "donat";
                            else if (!xComplementos.Contains("donat")) xComplementos += xComplementos = "|donat"; break;
                        case "divisas:Divisas":
                            if (xComplementos == "sincomplemento") xComplementos = "divisas";
                            else if (!xComplementos.Contains("divisas")) xComplementos += xComplementos = "|divisas"; break;
                        case "implocal:ImpuestosLocales":
                            if (xComplementos == "sincomplemento") xComplementos = "implocal";
                            else if (!xComplementos.Contains("implocal")) xComplementos += xComplementos = "|implocal"; break;
                        case "leyendasFisc:LeyendasFiscales":
                            if (xComplementos == "sincomplemento") xComplementos = "leyendasFisc";
                            else if (!xComplementos.Contains("leyendasFisc")) xComplementos += xComplementos = "|leyendasFisc"; break;
                        case "pfic:PFintegranteCoordinado":
                            if (xComplementos == "sincomplemento") xComplementos = "pfic";
                            else if (!xComplementos.Contains("pfic")) xComplementos += xComplementos = "|pfic"; break;
                        case "tpe:TuristaPasajeroExtranjero":
                            if (xComplementos == "sincomplemento") xComplementos = "tpe";
                            else if (!xComplementos.Contains("tpe")) xComplementos += xComplementos = "|tpe"; break;
                        case "nomina12:Nomina":
                            if (xComplementos == "sincomplemento") xComplementos = "nomina12";
                            else if (!xComplementos.Contains("nomina12")) xComplementos += xComplementos = "|nomina12"; break;
                        case "registrofiscal:CFDIRegistroFiscal":
                            if (xComplementos == "sincomplemento") xComplementos = "registrofiscal";
                            else if (!xComplementos.Contains("registrofiscal")) xComplementos += xComplementos = "|registrofiscal"; break;
                        case "pagoenespecie:PagoEnEspecie":
                            if (xComplementos == "sincomplemento") xComplementos = "pagoenespecie";
                            else if (!xComplementos.Contains("pagoenespecie")) xComplementos += xComplementos = "|pagoenespecie"; break;
                        case "aerolineas:Aerolineas":
                            if (xComplementos == "sincomplemento") xComplementos = "aerolineas";
                            else if (!xComplementos.Contains("aerolineas")) xComplementos += xComplementos = "|aerolineas"; break;
                        case "valesdedespensa:ValesDeDespensa":
                            if (xComplementos == "sincomplemento") xComplementos = "valesdedespensa";
                            else if (!xComplementos.Contains("valesdedespensa")) xComplementos += xComplementos = "|valesdedespensa"; break;
                        case "notariospublicos:NotariosPublicos":
                            if (xComplementos == "sincomplemento") xComplementos = "notariospublicos";
                            else if (!xComplementos.Contains("notariospublicos")) xComplementos += xComplementos = "|notariospublicos"; break;
                        case "vehiculousado:VehiculoUsado":
                            if (xComplementos == "sincomplemento") xComplementos = "vehiculousado";
                            else if (!xComplementos.Contains("vehiculousado")) xComplementos += xComplementos = "|vehiculousado"; break;
                        case "servicioparcial:parcialesconstruccion":
                            if (xComplementos == "sincomplemento") xComplementos = "servicioparcial";
                            else if (!xComplementos.Contains("servicioparcial")) xComplementos += xComplementos = "|servicioparcial"; break;
                        case "obrasarte:obrasarteantiguedades":
                            if (xComplementos == "sincomplemento") xComplementos = "obrasarte";
                            else if (!xComplementos.Contains("obrasarte")) xComplementos += xComplementos = "|obrasarte"; break;
                        case "ine:INE":
                            if (xComplementos == "sincomplemento") xComplementos = "ine";
                            else if (!xComplementos.Contains("ine")) xComplementos += xComplementos = "|ine"; break;
                        case "detallista:detallista":
                            if (xComplementos == "sincomplemento") xComplementos = "detallista";
                            else if (!xComplementos.Contains("detallista")) xComplementos += xComplementos = "|detallista"; break;
                        case "ecc12:EstadoDeCuentaCombustible":
                            if (xComplementos == "sincomplemento") xComplementos = "ecc12";
                            else if (!xComplementos.Contains("ecc12")) xComplementos += xComplementos = "|ecc12"; break;
                        case "consumodecombustibles11:ConsumoDeCombustibles":
                            if (xComplementos == "sincomplemento") xComplementos = "consumodecombustibles11";
                            else if (!xComplementos.Contains("consumodecombustibles11")) xComplementos += xComplementos = "|consumodecombustibles11"; break;
                        case "gceh:GastosHidrocarburos":
                            if (xComplementos == "sincomplemento") xComplementos = "gceh";
                            else if (!xComplementos.Contains("gceh")) xComplementos += xComplementos = "|gceh"; break;
                        case "ieeh:IngresosHidrocarburos":
                            if (xComplementos == "sincomplemento") xComplementos = "ieeh";
                            else if (!xComplementos.Contains("ieeh")) xComplementos += xComplementos = "|ieeh"; break;
                        case "cartaporte20:CartaPorte":
                            if (xComplementos == "sincomplemento") xComplementos = "cartaporte20";
                            else if (!xComplementos.Contains("cartaporte20")) xComplementos += xComplementos = "|cartaporte20"; break;
                        case "pago20:Pagos":
                            if (xComplementos == "sincomplemento") xComplementos = "pago20";
                            else if (!xComplementos.Contains("pago20")) xComplementos += xComplementos = "|pago20"; break;
                    }
                }

                while (xpnComplementosConcepto.MoveNext())
                {
                    switch (xpnComplementosConcepto.Current.Name)
                    {
                        case "iedu:instEducativas":
                            if (xComplementos == "sincomplemento") xComplementos = "iedu";
                            else if (!xComplementos.Contains("iedu")) xComplementos += xComplementos = "|iedu"; break;
                        case "ventavehiculos:VentaVehiculos":
                            if (xComplementos == "sincomplemento") xComplementos = "ventavehiculos";
                            else if (!xComplementos.Contains("ventavehiculos")) xComplementos += "|ventavehiculos"; break;
                    }
                }

            }
            catch { }

            return xComplementos;
        }
    }
}

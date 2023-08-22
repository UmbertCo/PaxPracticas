using ICSharpCode.SharpZipLib.Zip;
using PaxEntregaPendientes.Properties;
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

namespace PaxEntregaPendientes
{
    public partial class frmEntregarPendientes : Form
    {
        public frmEntregarPendientes()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            btnEntregarArchivos.Enabled = false;
        }
        private void btnSeleccionarOrigen_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdCarpeta = new FolderBrowserDialog();
            fbdCarpeta.ShowDialog();

            if (string.IsNullOrEmpty(fbdCarpeta.SelectedPath))
            {
                btnEntregarArchivos.Enabled = false;
                return;
            }

            txtOrigen.Text = fbdCarpeta.SelectedPath;
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

            if (lbZips.Items.Count > 0 && lbComprobantes.Items.Count > 0)
                btnEntregarArchivos.Enabled = true;
        }
        private void btnSeleccionarZip_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdZip = new OpenFileDialog();
            ofdZip.Multiselect = true;
            ofdZip.AddExtension = true;
            ofdZip.DefaultExt = "*.zip";
            ofdZip.ShowDialog();

            if (string.IsNullOrEmpty(ofdZip.FileName))
            {
                btnEntregarArchivos.Enabled = false;
                return;
            }

            lbZips.DataSource = ofdZip.FileNames;

            if (lbZips.Items.Count > 0 && lbComprobantes.Items.Count > 0)
                btnEntregarArchivos.Enabled = true;
        }
        private void btnPrueba_Click(object sender, EventArgs e)
        {
            DateTime dFechaComprobante;
            DateTime dFechaTimbrado;
            int nContador = 0;
            int pnIdUsuario = 0;
            string sCadenaOriginalEmisor = string.Empty;
            string sCadenaOriginal = string.Empty;
            string HASHEmisor = string.Empty;
            System.Diagnostics.EventLog eventLogExport = new System.Diagnostics.EventLog();

            int nArchivos = lbComprobantes.Items.Count;

            DataTable dtComprobantes = new DataTable();

            try { pnIdUsuario = Convert.ToInt32(txtUsuario.Text); }
            catch { pnIdUsuario = 0; }

            if (txtUsuario.Text.Trim().Equals("NADROFACTURACION"))
            {
                dtComprobantes.Columns.Add("Nombre Archivo", typeof(string));
                dtComprobantes.Columns.Add("Origen", typeof(string));
                dtComprobantes.Columns.Add("NombreZip", typeof(string));
                dtComprobantes.Columns.Add("FechaTimbrado", typeof(DateTime));
                dtComprobantes.Columns.Add("Hash", typeof(string));
            }
            else if (txtUsuario.Text.Trim().Equals("Forjadores01") || txtUsuario.Text.Trim().Equals("Guillermo Gama") || txtUsuario.Text.Trim().Equals("INDS920604") ||
                txtUsuario.Text.Trim().Equals("usr_quotimbrado") || txtUsuario.Text.Trim().Equals("uset29tlx") || txtUsuario.Text.Trim().Equals("ML02105752")
                || txtUsuario.Text.Trim().Equals("ML08011387") || txtUsuario.Text.Trim().Equals("profeco_pax") || txtUsuario.Text.Trim().Equals("NOMINAICHISAL")
                || txtUsuario.Text.Trim().Equals("SPF_timbrado") || txtUsuario.Text.Trim().Equals("Boca_Veracruz") || txtUsuario.Text.Trim().Equals("ervinprado")
                || txtUsuario.Text.Trim().Equals("UcacsaFacturacion2015") || txtUsuario.Text.Trim().Equals("seech_jose") || txtUsuario.Text.Trim().Equals("ICHEA2018"))
            {
                dtComprobantes.Columns.Add("Nombre Archivo", typeof(string));
                dtComprobantes.Columns.Add("Origen", typeof(string));
                dtComprobantes.Columns.Add("NombreZip", typeof(string));
                dtComprobantes.Columns.Add("FechaTimbrado", typeof(DateTime));
                dtComprobantes.Columns.Add("Hash", typeof(string));
                //dtComprobantes.Columns.Add("Hash", typeof(string));
            }
            else if (txtUsuario.Text.Trim().Equals("gobfacturacion") || txtUsuario.Text.Trim().Equals("OPNOMCSD") || txtUsuario.Text.Trim().Equals("nomina2016")
                || txtUsuario.Text.Trim().Equals("ICI081106CX7") || txtUsuario.Text.Trim().Equals("emmasal16") || txtUsuario.Text.Trim().Equals("GABRIELA JEFFERY SOSA"))
            {
                dtComprobantes.Columns.Add("Nombre Archivo", typeof(string));
                dtComprobantes.Columns.Add("Origen", typeof(string));
                dtComprobantes.Columns.Add("Id Zip", typeof(int));
                dtComprobantes.Columns.Add("NombreZip", typeof(string));
                dtComprobantes.Columns.Add("FechaTimbrado", typeof(DateTime));
                dtComprobantes.Columns.Add("UUID", typeof(string));
                dtComprobantes.Columns.Add("RFC Receptor", typeof(string));
                dtComprobantes.Columns.Add("ID Organismo", typeof(string));
                dtComprobantes.Columns.Add("CURP", typeof(string));
                dtComprobantes.Columns.Add("Numero Empleado", typeof(string));
                dtComprobantes.Columns.Add("Fecha Pago", typeof(DateTime));
                dtComprobantes.Columns.Add("Hash", typeof(string));
                dtComprobantes.Columns.Add("Version", typeof(string));
            }
            else if (txtUsuario.Text.Trim().Equals("SSC971029MU9"))
            {
                dtComprobantes.Columns.Add("Nombre Archivo", typeof(string));
                dtComprobantes.Columns.Add("Origen", typeof(string));
                dtComprobantes.Columns.Add("Id Zip", typeof(int));
                dtComprobantes.Columns.Add("NombreZip", typeof(string));
                dtComprobantes.Columns.Add("FechaTimbrado", typeof(DateTime));
                dtComprobantes.Columns.Add("UUID", typeof(string));
                dtComprobantes.Columns.Add("RFC Receptor", typeof(string));
                dtComprobantes.Columns.Add("ID Organismo", typeof(string));
                dtComprobantes.Columns.Add("CURP", typeof(string));
                dtComprobantes.Columns.Add("Numero Empleado", typeof(string));
                dtComprobantes.Columns.Add("Fecha Pago", typeof(DateTime));
                dtComprobantes.Columns.Add("Hash", typeof(string));
                dtComprobantes.Columns.Add("Version", typeof(string));
            }
            else
            {
                dtComprobantes.Columns.Add("Nombre Archivo", typeof(string));
                dtComprobantes.Columns.Add("Origen", typeof(string));
                dtComprobantes.Columns.Add("NombreZip", typeof(string));
                dtComprobantes.Columns.Add("FechaTimbrado", typeof(DateTime));
            }
            
            foreach (string nombreArchivo in lbComprobantes.Items)
            {
                string sNombreArchivo = Path.GetFileName(nombreArchivo);
                string sNombreZip = string.Empty;
                dFechaTimbrado = DateTime.Now;
                string sVersion = string.Empty;

                File.Copy(nombreArchivo, Settings.Default.RutaPendientes + sNombreArchivo, false);

                var contenidoArchivo = RecuperaArchivo(Settings.Default.RutaPendientes + sNombreArchivo);
                byte[] contenidoArchivoBytes = RecuperaArchivoByte(contenidoArchivo, Settings.Default.RutaPendientes + sNombreArchivo);

                contenidoArchivo.Close();

                XmlDocument sXmlDocument = new XmlDocument();
                sXmlDocument.Load(Settings.Default.RutaPendientes + sNombreArchivo);
                

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(sXmlDocument.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                try
                {
                    dFechaComprobante = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Fecha", nsmComprobante).ValueAsDateTime;
                    sVersion = "3.3";
                }
                catch
                {
                    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");
                    dFechaComprobante = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Fecha", nsmComprobante).ValueAsDateTime;
                    sVersion = "4.0";
                }

                try { dFechaTimbrado = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).ValueAsDateTime; }
                catch { }

                if (txtUsuario.Text.Trim().Equals("NADROFACTURACION"))
                {
                    XPathNavigator navNodoTimbre = sXmlDocument.CreateNavigator();

                    XslCompiledTransform xslt;
                    XsltArgumentList args;
                    MemoryStream ms;
                    StreamReader srDll;

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
                    HASHEmisor = GetHASH(sCadenaOriginalEmisor);

                    sNombreZip = fnObtenerNombreZip(sNombreArchivo);

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

                    try
                    {
                        DataRow drRenglon = dtComprobantes.NewRow();
                        drRenglon["Nombre Archivo"] = sNombreArchivo;
                        drRenglon["Origen"] = txtOrigen.Text;
                        drRenglon["NombreZip"] = sNombreZip;
                        drRenglon["FechaTimbrado"] = dFechaTimbrado;
                        drRenglon["Hash"] = HASHEmisor;
                        dtComprobantes.Rows.Add(drRenglon);

                        File.Delete(Settings.Default.RutaPendientes + sNombreArchivo);

                        nContador++;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al insertar el comprobante: " + sNombreArchivo + " - " + ex.Message);
                        txtResultado.Text += "Comprobante " + sNombreArchivo + " Nombre Zip: " + sNombreZip + " FecharTimbrado: " + dFechaTimbrado + " HashEmisor: " + HASHEmisor + System.Environment.NewLine;
                    }
                }
                if (txtUsuario.Text.Trim().Equals("Forjadores01") || txtUsuario.Text.Trim().Equals("Guillermo Gama") || txtUsuario.Text.Trim().Equals("INDS920604") ||
                txtUsuario.Text.Trim().Equals("usr_quotimbrado") || txtUsuario.Text.Trim().Equals("uset29tlx") || txtUsuario.Text.Trim().Equals("ML02105752")
                || txtUsuario.Text.Trim().Equals("ML08011387") || txtUsuario.Text.Trim().Equals("profeco_pax") || txtUsuario.Text.Trim().Equals("NOMINAICHISAL")
                || txtUsuario.Text.Trim().Equals("SPF_timbrado") || txtUsuario.Text.Trim().Equals("Boca_Veracruz") || txtUsuario.Text.Trim().Equals("ervinprado")
                || txtUsuario.Text.Trim().Equals("UcacsaFacturacion2015") || txtUsuario.Text.Trim().Equals("seech_jose") || txtUsuario.Text.Trim().Equals("ICHEA2018"))
                {
                    XPathNavigator navNodoTimbre = sXmlDocument.CreateNavigator();

                    XslCompiledTransform xslt;
                    XsltArgumentList args;
                    MemoryStream ms;
                    StreamReader srDll;

                    // Hash Emisor
                    xslt = new XslCompiledTransform();
                    //xslt.Load(typeof(CaOri.V3211));
                    if (sVersion.Equals("3.3"))
                        xslt.Load(typeof(CaOri.V33));
                    else
                        xslt.Load(typeof(CaOri.V40));

                    ms = new MemoryStream();
                    args = new XsltArgumentList();
                    xslt.Transform(navNodoTimbre, args, ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    srDll = new StreamReader(ms);

                    sCadenaOriginalEmisor = srDll.ReadToEnd();
                    HASHEmisor = GetHASH(sCadenaOriginalEmisor);

                    sNombreZip = fnObtenerNombreZip(sNombreArchivo);

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

                    try
                    {
                        DataRow drRenglon = dtComprobantes.NewRow();
                        drRenglon["Nombre Archivo"] = sNombreArchivo;
                        drRenglon["Origen"] = txtOrigen.Text;
                        drRenglon["NombreZip"] = sNombreZip;
                        drRenglon["FechaTimbrado"] = dFechaTimbrado;
                        drRenglon["Hash"] = HASHEmisor;
                        dtComprobantes.Rows.Add(drRenglon);

                        File.Delete(Settings.Default.RutaPendientes + sNombreArchivo);

                        nContador++;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al insertar el comprobante: " + sNombreArchivo + " - " + ex.Message);
                        txtResultado.Text += "Comprobante " + sNombreArchivo + " Nombre Zip: " + sNombreZip + " FecharTimbrado: " + dFechaTimbrado + " HashEmisor: " + HASHEmisor + System.Environment.NewLine;
                    }
                }
                else if (txtUsuario.Text.Trim().Equals("gobfacturacion") || txtUsuario.Text.Trim().Equals("OPNOMCSD") || txtUsuario.Text.Trim().Equals("nomina2016") 
                    || txtUsuario.Text.Trim().Equals("ICI081106CX7") || txtUsuario.Text.Trim().Equals("emmasal16") || txtUsuario.Text.Trim().Equals("GABRIELA JEFFERY SOSA"))
                {
                    XPathNavigator navNodoTimbre = sXmlDocument.CreateNavigator();

                    XslCompiledTransform xslt;
                    XsltArgumentList args;
                    MemoryStream ms;
                    StreamReader srDll;
                    
                    string sUUID = string.Empty;
                    string sRfcReceptor = string.Empty;
                    int nIdOrganismo = 0;
                    string sCURP = string.Empty;
                    string sNumeroEmpleado = string.Empty;
                    DateTime sFechaPago = DateTime.Now;


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
                    HASHEmisor = GetHASH(sCadenaOriginalEmisor);

                    try { sUUID = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                    catch { }
                    try { sRfcReceptor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@Rfc", nsmComprobante).Value; }
                    catch { }
                    try { sCURP = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/nomina12:Receptor/@Curp", nsmComprobante).Value; }
                    catch { }
                    try { sNumeroEmpleado = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/nomina12:Receptor/@NumEmpleado", nsmComprobante).Value; }
                    catch { }
                    try { sFechaPago = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/@FechaPago", nsmComprobante).ValueAsDateTime; }
                    catch { }
                    try { sVersion = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/@Version", nsmComprobante).Value; }
                    catch { }

                    try { nIdOrganismo = Convert.ToInt32(txtOrganismo.Text); }
                    catch { }

                    if (nIdOrganismo.Equals(0))
                    {
                        MessageBox.Show("El ID del Organismo es necesario para la inserción de los comprobantes");
                        return;
                    }

                    sNombreZip = fnObtenerNombreZip(Path.GetFileNameWithoutExtension(sNombreArchivo));

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

                    try
                    {
                        DataRow drRenglon = dtComprobantes.NewRow();
                        drRenglon["Nombre Archivo"] = sNombreArchivo;
                        drRenglon["Origen"] = txtOrigen.Text;
                        drRenglon["NombreZip"] = sNombreZip;
                        drRenglon["FechaTimbrado"] = dFechaTimbrado;
                        drRenglon["UUID"] = sUUID;
                        drRenglon["RFC Receptor"] = sRfcReceptor;
                        drRenglon["ID Organismo"] = nIdOrganismo;
                        drRenglon["CURP"] = sCURP;
                        drRenglon["Numero Empleado"] = sNumeroEmpleado;
                        drRenglon["Fecha Pago"] = sFechaPago;
                        drRenglon["Version"] = sVersion;
                        drRenglon["Hash"] = HASHEmisor;

                        File.Delete(Settings.Default.RutaPendientes + sNombreArchivo);

                        dtComprobantes.Rows.Add(drRenglon);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Error al insertar el comprobante: " + sNombreArchivo + " - " + ex.Message);
                    }
                }
                else if (txtUsuario.Text.Trim().Equals("SSC971029MU9"))
                {
                    XPathNavigator navNodoTimbre = sXmlDocument.CreateNavigator();

                    XslCompiledTransform xslt;
                    XsltArgumentList args;
                    MemoryStream ms;
                    StreamReader srDll;

                    string sUUID = string.Empty;
                    string sRfcReceptor = string.Empty;
                    int nIdOrganismo = 0;
                    string sCURP = string.Empty;
                    string sNumeroEmpleado = string.Empty;
                    DateTime sFechaPago = DateTime.Now;

                    // Hash Emisor
                    xslt = new XslCompiledTransform();
                    //xslt.Load(typeof(CaOri.V3211));
                    if(sVersion.Equals("3.3"))
                        xslt.Load(typeof(CaOri.V33));
                    else
                        xslt.Load(typeof(CaOri.V40));

                    ms = new MemoryStream();
                    args = new XsltArgumentList();
                    xslt.Transform(navNodoTimbre, args, ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    srDll = new StreamReader(ms);

                    sCadenaOriginalEmisor = srDll.ReadToEnd();
                    HASHEmisor = GetHASH(sCadenaOriginalEmisor);

                    try { sUUID = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                    catch { }
                    try { sRfcReceptor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@Rfc", nsmComprobante).Value; }
                    catch { }
                    try { sCURP = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/nomina12:Receptor/@Curp", nsmComprobante).Value; }
                    catch { }
                    try { sNumeroEmpleado = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/nomina12:Receptor/@NumEmpleado", nsmComprobante).Value; }
                    catch { }
                    try { sFechaPago = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/@FechaPago", nsmComprobante).ValueAsDateTime; }
                    catch { }
                    try { sVersion = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/@Version", nsmComprobante).Value; }
                    catch { }

                    try { nIdOrganismo = Convert.ToInt32(txtOrganismo.Text); }
                    catch { }

                    if (nIdOrganismo.Equals(0))
                    {
                        MessageBox.Show("El ID del Organismo es necesario para la inserción de los comprobantes");
                        return;
                    }

    
                    sNombreZip = fnObtenerNombreZip(Path.GetFileNameWithoutExtension(sNombreArchivo));
        

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

                    try
                    {
                        DataRow drRenglon = dtComprobantes.NewRow();
                        drRenglon["Nombre Archivo"] = sNombreArchivo;
                        drRenglon["Origen"] = txtOrigen.Text;
                        drRenglon["NombreZip"] = sNombreZip;
                        drRenglon["FechaTimbrado"] = dFechaTimbrado;
                        drRenglon["UUID"] = sUUID;
                        drRenglon["RFC Receptor"] = sRfcReceptor;
                        drRenglon["ID Organismo"] = nIdOrganismo;
                        drRenglon["CURP"] = sCURP;
                        drRenglon["Numero Empleado"] = sNumeroEmpleado;
                        drRenglon["Fecha Pago"] = sFechaPago;
                        drRenglon["Version"] = sVersion;
                        drRenglon["Hash"] = HASHEmisor;

                        File.Delete(Settings.Default.RutaPendientes + sNombreArchivo);

                        dtComprobantes.Rows.Add(drRenglon);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Error al insertar el comprobante: " + sNombreArchivo + " - " + ex.Message);
                    }
                }
                else
                {
                    sNombreZip = fnObtenerNombreZip(Path.GetFileNameWithoutExtension(sNombreArchivo));

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

                    try
                    {
                        DataRow drRenglon = dtComprobantes.NewRow();
                        drRenglon["Nombre Archivo"] = sNombreArchivo;
                        drRenglon["Origen"] = txtOrigen.Text;
                        drRenglon["NombreZip"] = sNombreZip;
                        drRenglon["FechaTimbrado"] = dFechaTimbrado;
                        dtComprobantes.Rows.Add(drRenglon);

                        File.Delete(Settings.Default.RutaPendientes + sNombreArchivo);

                        nContador++;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al insertar el comprobante: " + sNombreArchivo + " - " + ex.Message);
                        txtResultado.Text += "Comprobante " + sNombreArchivo + " Nombre Zip: " + sNombreZip + " FecharTimbrado: " + dFechaTimbrado + " HashEmisor: " + HASHEmisor + System.Environment.NewLine;
                    }
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
            int pnIdUsuario = 0;
            string sCadenaOriginalEmisor = string.Empty;
            string sCadenaOriginal = string.Empty;
            string HASHEmisor = string.Empty;
            string sEstatus = string.Empty;
            System.Diagnostics.EventLog eventLogExport = new System.Diagnostics.EventLog();

            try { pnIdUsuario = Convert.ToInt32(txtUsuario.Text); }
            catch { pnIdUsuario = 0; }

            DataTable dtComprobantes = new DataTable();
            if (txtUsuario.Text.Trim().Equals("NADROFACTURACION"))
            {
                dtComprobantes.Columns.Add("Nombre Archivo", typeof(string));
                dtComprobantes.Columns.Add("Origen", typeof(string));
                dtComprobantes.Columns.Add("Id Zip", typeof(int));
                dtComprobantes.Columns.Add("NombreZip", typeof(string));
                dtComprobantes.Columns.Add("FechaTimbrado", typeof(DateTime));
                dtComprobantes.Columns.Add("Hash", typeof(string));
            }
            else if (txtUsuario.Text.Trim().Equals("Forjadores01") || txtUsuario.Text.Trim().Equals("Guillermo Gama") || txtUsuario.Text.Trim().Equals("INDS920604") ||
                txtUsuario.Text.Trim().Equals("usr_quotimbrado") || txtUsuario.Text.Trim().Equals("uset29tlx") || txtUsuario.Text.Trim().Equals("ML02105752")
                || txtUsuario.Text.Trim().Equals("ML08011387") || txtUsuario.Text.Trim().Equals("profeco_pax") || txtUsuario.Text.Trim().Equals("NOMINAICHISAL")
                || txtUsuario.Text.Trim().Equals("SPF_timbrado") || txtUsuario.Text.Trim().Equals("Boca_Veracruz") || txtUsuario.Text.Trim().Equals("ervinprado")
                || txtUsuario.Text.Trim().Equals("UcacsaFacturacion2015") || txtUsuario.Text.Trim().Equals("seech_jose") || txtUsuario.Text.Trim().Equals("ICHEA2018"))
            {
                dtComprobantes.Columns.Add("Nombre Archivo", typeof(string));
                dtComprobantes.Columns.Add("Origen", typeof(string));
                dtComprobantes.Columns.Add("Id Zip", typeof(int));
                dtComprobantes.Columns.Add("NombreZip", typeof(string));
                dtComprobantes.Columns.Add("FechaTimbrado", typeof(DateTime));
                dtComprobantes.Columns.Add("Hash", typeof(string));
                //dtComprobantes.Columns.Add("Hash", typeof(string));
            }
            else if (txtUsuario.Text.Trim().Equals("gobfacturacion") || txtUsuario.Text.Trim().Equals("OPNOMCSD") || txtUsuario.Text.Trim().Equals("nomina2016")
                    || txtUsuario.Text.Trim().Equals("ICI081106CX7") || txtUsuario.Text.Trim().Equals("emmasal16") || txtUsuario.Text.Trim().Equals("GABRIELA JEFFERY SOSA"))
            {
                dtComprobantes.Columns.Add("Nombre Archivo", typeof(string));
                dtComprobantes.Columns.Add("Origen", typeof(string));
                dtComprobantes.Columns.Add("Id Zip", typeof(int));
                dtComprobantes.Columns.Add("NombreZip", typeof(string));
                dtComprobantes.Columns.Add("FechaTimbrado", typeof(DateTime));
                dtComprobantes.Columns.Add("UUID", typeof(string));
                dtComprobantes.Columns.Add("RFC Receptor", typeof(string));
                dtComprobantes.Columns.Add("ID Organismo", typeof(string));
                dtComprobantes.Columns.Add("CURP", typeof(string));
                dtComprobantes.Columns.Add("Numero Empleado", typeof(string));
                dtComprobantes.Columns.Add("Fecha Pago", typeof(DateTime));
                dtComprobantes.Columns.Add("Hash", typeof(string));
                dtComprobantes.Columns.Add("Version", typeof(string));
            }
            else if (txtUsuario.Text.Trim().Equals("SSC971029MU9"))
            {
                dtComprobantes.Columns.Add("Nombre Archivo", typeof(string));
                dtComprobantes.Columns.Add("Origen", typeof(string));
                dtComprobantes.Columns.Add("Id Zip", typeof(int));
                dtComprobantes.Columns.Add("NombreZip", typeof(string));
                dtComprobantes.Columns.Add("FechaTimbrado", typeof(DateTime));
                dtComprobantes.Columns.Add("Hash", typeof(string));
            }
            else
            {
                dtComprobantes.Columns.Add("Nombre Archivo", typeof(string));
                dtComprobantes.Columns.Add("Origen", typeof(string));
                dtComprobantes.Columns.Add("Id Zip", typeof(int));
                dtComprobantes.Columns.Add("NombreZip", typeof(string));
                dtComprobantes.Columns.Add("FechaTimbrado", typeof(DateTime));
            }

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
                string sVersion = string.Empty;

                File.Copy(nombreArchivo, Settings.Default.RutaPendientes + sNombreArchivo);
                
                asArchivos[0, nContador] = sNombreArchivo;
                
                asArchivos[1, nContador] = sNombreZip;

                var contenidoArchivo = RecuperaArchivo(Settings.Default.RutaPendientes + sNombreArchivo);
                byte[] contenidoArchivoBytes = RecuperaArchivoByte(contenidoArchivo, Settings.Default.RutaPendientes + sNombreArchivo);

                contenidoArchivo.Close();

                XmlDocument sXmlDocument = new XmlDocument();
                sXmlDocument.Load(Settings.Default.RutaPendientes + sNombreArchivo);

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(sXmlDocument.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                nsmComprobante.AddNamespace("nomina12", "http://www.sat.gob.mx/nomina12");

                try
                {
                    dFechaComprobante = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Fecha", nsmComprobante).ValueAsDateTime;
                    sVersion = "3.3";
                }
                catch
                {
                    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");
                    dFechaComprobante = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Fecha", nsmComprobante).ValueAsDateTime;
                    sVersion = "4.0";
                }
                dFechaTimbrado = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).ValueAsDateTime;

                if (txtUsuario.Text.Trim().Equals("NADROFACTURACION"))
                {
                    XPathNavigator navNodoTimbre = sXmlDocument.CreateNavigator();

                    XslCompiledTransform xslt;
                    XsltArgumentList args;
                    MemoryStream ms;
                    StreamReader srDll;

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
                    HASHEmisor = GetHASH(sCadenaOriginalEmisor);

                    sNombreZip = fnObtenerNombreZip(sNombreArchivo);
                    sHashzip = GetHASH(sNombreZip).ToString();

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

                    nId_Zip = fnObtenerZip(sHashzip);

                    if (nId_Zip.Equals(0))
                        continue;

                    if (fnExisteComprobante(HASHEmisor, sNombreZip, sNombreArchivo))
                    {
                        txtResultado.Text += "Comprobante " + sNombreArchivo + " existente." + System.Environment.NewLine;
                        continue;
                    }
                    else
                    {
                        
                    }

                    try
                    {
                        DataRow drRenglon = dtComprobantes.NewRow();
                        drRenglon["Nombre Archivo"] = sNombreArchivo;
                        drRenglon["Origen"] = txtOrigen.Text;
                        drRenglon["Id Zip"] = nId_Zip;
                        drRenglon["NombreZip"] = sNombreZip;
                        drRenglon["FechaTimbrado"] = dFechaTimbrado;
                        drRenglon["Hash"] = HASHEmisor;

                        //if (sXmlDocument.InnerXml.Contains("'"))
                        //{
                        //    sComprobante = sXmlDocument.InnerXml.Replace("'", "&#39;");
                        //    txtResultado.Text += "Comprobante " + sNombreArchivo + "con comillas. Actualizado";
                        //}
                        //else
                        //    sComprobante = sXmlDocument.InnerXml;

                        sComprobante = sXmlDocument.InnerXml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");


                        if (rbActivo.Checked)
                            sEstatus = "A";
                        else
                            sEstatus = "P";

                        fnInsertarComprobante(sComprobante, txtOrigen.Text, sNombreArchivo, dFechaTimbrado, nId_Zip, HASHEmisor, sEstatus);

                        //fnActualizarComprobante(dFechaTimbrado, nId_Zip, HASHEmisor);

                        string sInstruccion = "Datos Registrados: Comprobante: {0}, Origen: {1}, Zip: {2}, Fecha timbrado: {3}, Hash: {4}";

                        txtResultado.Text += string.Format(sInstruccion, sNombreArchivo, txtOrigen.Text, sNombreZip, dFechaTimbrado, HASHEmisor) + System.Environment.NewLine;

                        File.Delete(Settings.Default.RutaPendientes + sNombreArchivo);

                        dtComprobantes.Rows.Add(drRenglon);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Error al insertar el comprobante: " + sNombreArchivo + " - " + ex.Message);
                        txtResultado.Text += "Comprobante " + sNombreArchivo + " no existente. Zip:" + nId_Zip + ", Nombre Zip: " + sNombreZip + " FecharTimbrado: " + dFechaTimbrado + " HashEmisor: " + HASHEmisor + System.Environment.NewLine;
                    }
                }
                else if (txtUsuario.Text.Trim().Equals("Forjadores01") || txtUsuario.Text.Trim().Equals("Guillermo Gama") || txtUsuario.Text.Trim().Equals("INDS920604") ||
                txtUsuario.Text.Trim().Equals("usr_quotimbrado") || txtUsuario.Text.Trim().Equals("uset29tlx") || txtUsuario.Text.Trim().Equals("ML02105752")
                || txtUsuario.Text.Trim().Equals("ML08011387") || txtUsuario.Text.Trim().Equals("profeco_pax") || txtUsuario.Text.Trim().Equals("NOMINAICHISAL")
                || txtUsuario.Text.Trim().Equals("SPF_timbrado") || txtUsuario.Text.Trim().Equals("Boca_Veracruz") || txtUsuario.Text.Trim().Equals("ervinprado")
                || txtUsuario.Text.Trim().Equals("UcacsaFacturacion2015") || txtUsuario.Text.Trim().Equals("seech_jose") || txtUsuario.Text.Trim().Equals("ICHEA2018"))
                {
                    XPathNavigator navNodoTimbre = sXmlDocument.CreateNavigator();

                    XslCompiledTransform xslt;
                    XsltArgumentList args;
                    MemoryStream ms;
                    StreamReader srDll;

                    // Hash Emisor
                    xslt = new XslCompiledTransform();
                    //xslt.Load(typeof(CaOri.V3211));
                    if(sVersion.Equals("3.3"))
                        xslt.Load(typeof(CaOri.V33));
                    else
                        xslt.Load(typeof(CaOri.V40));

                    ms = new MemoryStream();
                    args = new XsltArgumentList();
                    xslt.Transform(navNodoTimbre, args, ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    srDll = new StreamReader(ms);

                    sCadenaOriginalEmisor = srDll.ReadToEnd();
                    HASHEmisor = GetHASH(sCadenaOriginalEmisor);

                    sNombreZip = fnObtenerNombreZip(sNombreArchivo);
                    sHashzip = GetHASH(sNombreZip).ToString();

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

                    if (sVersion.Equals("3.3"))
                        nId_Zip = fnObtenerZipDatos(sHashzip);
                    else
                        nId_Zip = fnObtenerZipDatos40(sHashzip);

                    if (nId_Zip.Equals(0))
                        continue;

                    //if (fnExisteComprobante(HASHEmisor, sNombreZip, sNombreArchivo))
                    //{
                    //    txtResultado.Text += "Comprobante " + sNombreArchivo + " existente." + System.Environment.NewLine;
                    //    continue;
                    //}
                    //else
                    //{
                    //    //txtResultado.Text += "Comprobante " + sNombreArchivo + " no existente. Zip:" + nId_Zip + ", Nombre Zip: " + sNombreZip + " FecharTimbrado: " + dFechaTimbrado + " HashEmisor: " + HASHEmisor + System.Environment.NewLine;
                    //}

                    try
                    {
                        DataRow drRenglon = dtComprobantes.NewRow();
                        drRenglon["Nombre Archivo"] = sNombreArchivo;
                        drRenglon["Origen"] = txtOrigen.Text;
                        drRenglon["Id Zip"] = nId_Zip;
                        drRenglon["NombreZip"] = sNombreZip;
                        drRenglon["FechaTimbrado"] = dFechaTimbrado;
                        drRenglon["Hash"] = HASHEmisor;

                        string sAddenda = string.Empty;
                        try
                        {
                            sAddenda = sXmlDocument.CreateNavigator().SelectSingleNode("cfdi:Comprobante/cfdi:Addenda", nsmComprobante).OuterXml;
                            sXmlDocument.CreateNavigator().SelectSingleNode("cfdi:Comprobante/cfdi:Addenda", nsmComprobante).DeleteSelf();
                        }
                        catch { }  

                        sComprobante = sXmlDocument.InnerXml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "").Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");

                        if (rbActivo.Checked)
                            sEstatus = "A";
                        else
                            sEstatus = "P";

                        if (sVersion.Equals("3.3"))
                            fnInsertarComprobante(sComprobante, txtOrigen.Text, sNombreArchivo, dFechaTimbrado, nId_Zip, HASHEmisor, txtUsuario.Text, sEstatus, sAddenda, 3);
                        else
                            fnInsertarComprobante40(sComprobante, txtOrigen.Text, sNombreArchivo, dFechaTimbrado, nId_Zip, HASHEmisor, txtUsuario.Text, sEstatus, sAddenda, 3);



                        //fnActualizarComprobante(dFechaTimbrado, nId_Zip, HASHEmisor);

                        string sInstruccion = "Datos Registrados: Comprobante: {0}, Origen: {1}, Zip: {2}, Fecha timbrado: {3}, Hash: {4}";

                        txtResultado.Text += string.Format(sInstruccion, sNombreArchivo, txtOrigen.Text, sNombreZip, dFechaTimbrado, HASHEmisor) + System.Environment.NewLine;

                        File.Delete(Settings.Default.RutaPendientes + sNombreArchivo);

                        dtComprobantes.Rows.Add(drRenglon);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Error al insertar el comprobante: " + sNombreArchivo + " - " + ex.Message);
                        txtResultado.Text += "Comprobante " + sNombreArchivo + " no existente. Zip:" + nId_Zip + ", Nombre Zip: " + sNombreZip + " FecharTimbrado: " + dFechaTimbrado + " HashEmisor: " + HASHEmisor + System.Environment.NewLine;
                    }
                }
                else if (txtUsuario.Text.Trim().Equals("gobfacturacion") || txtUsuario.Text.Trim().Equals("OPNOMCSD") || txtUsuario.Text.Trim().Equals("nomina2016") 
                    || txtUsuario.Text.Trim().Equals("ICI081106CX7") || txtUsuario.Text.Trim().Equals("emmasal16") || txtUsuario.Text.Trim().Equals("GABRIELA JEFFERY SOSA"))
                {
                    XPathNavigator navNodoTimbre = sXmlDocument.CreateNavigator();

                    XslCompiledTransform xslt;
                    XsltArgumentList args;
                    MemoryStream ms;
                    StreamReader srDll;
                    
                    string sUUID = string.Empty;
                    string sRfcReceptor = string.Empty;
                    int nIdOrganismo = 0;
                    string sCURP = string.Empty;
                    string sNumeroEmpleado = string.Empty;
                    DateTime sFechaPago = DateTime.Now;
                    string sVersionCFDI = string.Empty;

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
                    HASHEmisor = GetHASH(sCadenaOriginalEmisor);
                    //MessageBox.Show("HashEmisor: " + HASHEmisor);

                    sNombreZip = fnObtenerNombreZip(Path.GetFileNameWithoutExtension(sNombreArchivo));
                    //MessageBox.Show("NombreZip: " + sNombreZip);

                    sHashzip = GetHASH(sNombreZip).ToString();
                    //MessageBox.Show("HashZip: " + sHashzip);

                    try { sUUID = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                    catch { }
                    try { sRfcReceptor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@Rfc", nsmComprobante).Value; }
                    catch { }
                    try { sCURP = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/nomina12:Receptor/@Curp", nsmComprobante).Value; }
                    catch { }
                    try { sNumeroEmpleado = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/nomina12:Receptor/@NumEmpleado", nsmComprobante).Value; }
                    catch { }
                    try { sFechaPago = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/@FechaPago", nsmComprobante).ValueAsDateTime; }
                    catch { }
                    try { sVersion = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/@Version", nsmComprobante).Value; }
                    catch { }
                    try { sVersionCFDI = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Version", nsmComprobante).Value; }
                    catch { }

                    try { nIdOrganismo = Convert.ToInt32(txtOrganismo.Text); }
                    catch { }

                    if (nIdOrganismo.Equals(0))
                    {
                        MessageBox.Show("El ID del Organismo es necesario para la inserción de los comprobantes");
                        return;
                    }

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

                    nId_Zip = fnObtenerZip(sHashzip);
                    //MessageBox.Show("IdZip: " + nId_Zip.ToString());

                    if (nId_Zip.Equals(0))
                        continue;

                    if (fnExisteComprobante(HASHEmisor, sNombreZip, sNombreArchivo))
                    {
                        //MessageBox.Show("Existe el Comprobante: " + HASHEmisor);
                        txtResultado.Text += "Comprobante " + sNombreArchivo + " existente." + System.Environment.NewLine;
                        continue;
                    }
                    else
                    {
                        //MessageBox.Show("No existe el Comprobante: " + HASHEmisor);
                        //txtResultado.Text += "Comprobante " + sNombreArchivo + " no existente. Zip:" + nId_Zip + ", Nombre Zip: " + sNombreZip + " FecharTimbrado: " + dFechaTimbrado + " HashEmisor: " + HASHEmisor + System.Environment.NewLine;
                    }

                    

                    try
                    {
                        DataRow drRenglon = dtComprobantes.NewRow();
                        drRenglon["Nombre Archivo"] = sNombreArchivo;
                        drRenglon["Origen"] = txtOrigen.Text;
                        drRenglon["Id Zip"] = nId_Zip;
                        drRenglon["NombreZip"] = sNombreZip;
                        drRenglon["FechaTimbrado"] = dFechaTimbrado;
                        drRenglon["Hash"] = HASHEmisor;

                        //if (sXmlDocument.InnerXml.Contains("'"))
                        //{
                        //    sComprobante = sXmlDocument.InnerXml.Replace("'", "&#39;");
                        //    txtResultado.Text += "Comprobante " + sNombreArchivo + "con comillas. Actualizado";
                        //}
                        //else
                        //    sComprobante = sXmlDocument.InnerXml;

                        sComprobante = sXmlDocument.InnerXml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");

                        if (rbActivo.Checked)
                            sEstatus = "A";
                        else
                            sEstatus = "P";

                        fnInsertarComprobante(sComprobante, txtOrigen.Text, sNombreArchivo, dFechaTimbrado, nId_Zip, HASHEmisor, txtUsuario.Text, sEstatus, sUUID, sRfcReceptor, nIdOrganismo,
                            sCURP, sNumeroEmpleado, sFechaPago, HASHEmisor, sVersion, sVersionCFDI);



                        //fnActualizarComprobante(dFechaTimbrado, nId_Zip, HASHEmisor);

                        string sInstruccion = "Datos Registrados: Comprobante: {0}, Origen: {1}, Zip: {2}, Fecha timbrado: {3}, Hash: {4}";

                        txtResultado.Text += string.Format(sInstruccion, sNombreArchivo, txtOrigen.Text, sNombreZip, dFechaTimbrado, HASHEmisor) + System.Environment.NewLine;

                        File.Delete(Settings.Default.RutaPendientes + sNombreArchivo);

                        dtComprobantes.Rows.Add(drRenglon);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Error al insertar el comprobante: " + sNombreArchivo + " - " + ex.Message);
                        txtResultado.Text += "Comprobante " + sNombreArchivo + " no existente. Zip:" + nId_Zip + ", Nombre Zip: " + sNombreZip + " FecharTimbrado: " + dFechaTimbrado + " HashEmisor: " + HASHEmisor + System.Environment.NewLine;
                    }
                }
                else if (txtUsuario.Text.Trim().Equals("SSC971029MU9"))
                {
                    XPathNavigator navNodoTimbre = sXmlDocument.CreateNavigator();

                    XslCompiledTransform xslt;
                    XsltArgumentList args;
                    MemoryStream ms;
                    StreamReader srDll;

                    string sUUID = string.Empty;
                    string sRfcReceptor = string.Empty;
                    int nIdOrganismo = 0;
                    string sCURP = string.Empty;
                    string sNumeroEmpleado = string.Empty;
                    DateTime sFechaPago = DateTime.Now;

                    // Hash Emisor
                    xslt = new XslCompiledTransform();
                    //xslt.Load(typeof(CaOri.V3211));
                    if (sVersion.Equals("3.3"))
                        xslt.Load(typeof(CaOri.V33));
                    else
                        xslt.Load(typeof(CaOri.V40));

                    ms = new MemoryStream();
                    args = new XsltArgumentList();
                    xslt.Transform(navNodoTimbre, args, ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    srDll = new StreamReader(ms);

                    sCadenaOriginalEmisor = srDll.ReadToEnd();
                    HASHEmisor = GetHASH(sCadenaOriginalEmisor);
                    //MessageBox.Show("HashEmisor: " + HASHEmisor);

                    sNombreZip = fnObtenerNombreZip(Path.GetFileNameWithoutExtension(sNombreArchivo));
                    //MessageBox.Show("NombreZip: " + sNombreZip);

                    sHashzip = GetHASH(sNombreZip).ToString();
                    //MessageBox.Show("HashZip: " + sHashzip);

                    try { sUUID = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                    catch { }
                    try { sRfcReceptor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@Rfc", nsmComprobante).Value; }
                    catch { }
                    try { sCURP = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/nomina12:Receptor/@Curp", nsmComprobante).Value; }
                    catch { }
                    try { sNumeroEmpleado = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/nomina12:Receptor/@NumEmpleado", nsmComprobante).Value; }
                    catch { }
                    try { sFechaPago = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/@FechaPago", nsmComprobante).ValueAsDateTime; }
                    catch { }
                    try { sVersion = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/@Version", nsmComprobante).Value; }
                    catch { }

                    try { nIdOrganismo = Convert.ToInt32(txtOrganismo.Text); }
                    catch { }

                    if (nIdOrganismo.Equals(0))
                    {
                        MessageBox.Show("El ID del Organismo es necesario para la inserción de los comprobantes");
                        return;
                    }

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

    
                    nId_Zip = fnObtenerZipConamm(sHashzip);
                    //MessageBox.Show("IdZip: " + nId_Zip.ToString());

                    if (nId_Zip.Equals(0))
                        continue;

                    if (fnExisteComprobanteConamm(HASHEmisor, sNombreZip, sNombreArchivo))
                    {
                        //MessageBox.Show("Existe el Comprobante: " + HASHEmisor);
                        txtResultado.Text += "Comprobante " + sNombreArchivo + " existente." + System.Environment.NewLine;
                        continue;
                    }
                    else
                    {
                        //MessageBox.Show("No existe el Comprobante: " + HASHEmisor);
                        //txtResultado.Text += "Comprobante " + sNombreArchivo + " no existente. Zip:" + nId_Zip + ", Nombre Zip: " + sNombreZip + " FecharTimbrado: " + dFechaTimbrado + " HashEmisor: " + HASHEmisor + System.Environment.NewLine;
                    }



                    try
                    {
                        DataRow drRenglon = dtComprobantes.NewRow();
                        drRenglon["Nombre Archivo"] = sNombreArchivo;
                        drRenglon["Origen"] = txtOrigen.Text;
                        drRenglon["Id Zip"] = nId_Zip;
                        drRenglon["NombreZip"] = sNombreZip;
                        drRenglon["FechaTimbrado"] = dFechaTimbrado;
                        drRenglon["Hash"] = HASHEmisor;

                        //if (sXmlDocument.InnerXml.Contains("'"))
                        //{
                        //    sComprobante = sXmlDocument.InnerXml.Replace("'", "&#39;");
                        //    txtResultado.Text += "Comprobante " + sNombreArchivo + "con comillas. Actualizado";
                        //}
                        //else
                        //    sComprobante = sXmlDocument.InnerXml;

                        sComprobante = sXmlDocument.InnerXml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");

                        if (rbActivo.Checked)
                            sEstatus = "A";
                        else
                            sEstatus = "P";

                        fnInsertarComprobanteConamm(sComprobante, txtOrigen.Text, sNombreArchivo, dFechaTimbrado, nId_Zip, HASHEmisor, txtUsuario.Text, sEstatus, sUUID, sRfcReceptor, nIdOrganismo,
                            sCURP, sNumeroEmpleado, sFechaPago, HASHEmisor, sVersion);



                        //fnActualizarComprobante(dFechaTimbrado, nId_Zip, HASHEmisor);

                        string sInstruccion = "Datos Registrados: Comprobante: {0}, Origen: {1}, Zip: {2}, Fecha timbrado: {3}, Hash: {4}";

                        txtResultado.Text += string.Format(sInstruccion, sNombreArchivo, txtOrigen.Text, sNombreZip, dFechaTimbrado, HASHEmisor) + System.Environment.NewLine;

                        File.Delete(Settings.Default.RutaPendientes + sNombreArchivo);

                        dtComprobantes.Rows.Add(drRenglon);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Error al insertar el comprobante: " + sNombreArchivo + " - " + ex.Message);
                        txtResultado.Text += "Comprobante " + sNombreArchivo + " no existente. Zip:" + nId_Zip + ", Nombre Zip: " + sNombreZip + " FecharTimbrado: " + dFechaTimbrado + " HashEmisor: " + HASHEmisor + System.Environment.NewLine;
                    }
                }
                else
                {
                    sNombreZip = fnObtenerNombreZip(Path.GetFileNameWithoutExtension(sNombreArchivo));
                    sHashzip = GetHASH(sNombreZip).ToString();

                    //MessageBox.Show("Hash ZIP: " + sHashzip);

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

                    nId_Zip = fnObtenerZip(sHashzip);

                    //MessageBox.Show("Zip ID: " + nId_Zip);

                    if (nId_Zip.Equals(0))
                        continue;

                    if (rbActivo.Checked)
                        sEstatus = "P";
                    else
                        sEstatus = "N";

                    try
                    {
                        DataRow drRenglon = dtComprobantes.NewRow();
                        drRenglon["Nombre Archivo"] = sNombreArchivo;
                        drRenglon["Origen"] = txtOrigen.Text;
                        drRenglon["Id Zip"] = nId_Zip;
                        drRenglon["NombreZip"] = sNombreZip;
                        drRenglon["FechaTimbrado"] = dFechaTimbrado;

                        //if (sXmlDocument.InnerXml.Contains("'"))
                        //{
                        //    sComprobante = sXmlDocument.InnerXml.Replace("'", "&#39;");
                        //    txtResultado.Text += "Comprobante " + sNombreArchivo + "con comillas. Actualizado";
                        //}
                        //else
                        //    sComprobante = sXmlDocument.InnerXml;

                        sComprobante = sXmlDocument.InnerXml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");

                        fnInsertarComprobante(sComprobante, txtOrigen.Text, sNombreArchivo, dFechaTimbrado, nId_Zip, sEstatus);

                        //fnActualizarComprobante(dFechaTimbrado, nId_Zip, HASHEmisor);

                        string sInstruccion = "Datos Registrados: Comprobante: {0}, Origen: {1}, Zip: {2}, Fecha timbrado: {3}, Hash: {4}";

                        txtResultado.Text += string.Format(sInstruccion, sNombreArchivo, txtOrigen.Text, sNombreZip, dFechaTimbrado, HASHEmisor) + System.Environment.NewLine;

                        File.Delete(Settings.Default.RutaPendientes + sNombreArchivo);

                        dtComprobantes.Rows.Add(drRenglon);

                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Error al insertar el comprobante: " + sNombreArchivo + " - " + ex.Message);
                        txtResultado.Text += "Comprobante " + sNombreArchivo + " no existente. Zip:" + nId_Zip + ", Nombre Zip: " + sNombreZip + " FecharTimbrado: " + dFechaTimbrado + " HashEmisor: " + HASHEmisor + System.Environment.NewLine;
                    }
                }
                
                nContador++;
            }

            dgvComprobantes.DataSource = dtComprobantes;
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
        /// 
        /// </summary>
        /// <param name="directorioRaiz"></param>
        /// <returns></returns>
        private IList RecuperaListaArchivos(string directorioRaiz)
        {
            IList listaArchivos = Directory.GetFiles(directorioRaiz).ToList();
            return listaArchivos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rutaAbsoluta"></param>
        /// <returns></returns>
        private Stream RecuperaArchivo(string rutaAbsoluta)
        {
            return File.OpenRead(rutaAbsoluta);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rutaAbsoluta"></param>
        /// <param name="ruta"></param>
        /// <returns></returns>
        private byte[] RecuperaArchivoByte(Stream rutaAbsoluta, string ruta)
        {
            StreamReader sr = new StreamReader(rutaAbsoluta);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            return encoding.GetBytes(sr.ReadToEnd());


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toEncode"></param>
        /// <returns></returns>
        private string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psNombreArchivo"></param>
        /// <returns></returns>
        private string fnObtenerNombreZip(string psNombreArchivo)
        {
            string sNombreZip = string.Empty;
            //IList listaZip;
            try
            {
                //listaZip = RecuperaListaArchivos(psNombreZips);

                foreach (string sZip in lbZips.Items)
                {
                    using (FileStream fsSource = new FileStream(sZip, FileMode.Open))
                    {
                        Stream strZip = fsSource;
                        ZipFile archivoZip = new ZipFile(strZip);

                        foreach (ZipEntry zipEntry in archivoZip)
                        {
                            try
                            {
                                if (!zipEntry.IsFile)
                                {
                                    throw new System.Exception("No es archivo");
                                }

                                if (zipEntry.IsDirectory)
                                {
                                    throw new System.Exception("Es carpeta");
                                }

                                if (zipEntry.IsCrypted)
                                {
                                    throw new System.Exception("Archivo encriptado");
                                }


                                if (Path.GetFileNameWithoutExtension(psNombreArchivo).Equals(Path.GetFileNameWithoutExtension(zipEntry.Name)))
                                {
                                    sNombreZip = Path.GetFileName(sZip);
                                    return sNombreZip;
                                }

                            }
                            catch (Exception ex)
                            {
                                throw new System.Exception("Error al buscar en el zip: " + sZip + " " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Ocurrio un error al buscar el zip origen: " + ex.Message);
            }
            return sNombreZip;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psHashZip"></param>
        /// <returns></returns>
        private int fnObtenerZip(string psHashZip)
        {
            int nId_Zip = 0;

            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
            {
                conexion.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("usp_Zip_Buscar_Sel", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sHASH", psHashZip);
                        nId_Zip = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error al buscar el Id ZIP: " + psHashZip + " - " + ex.Message);
                }
                
            }
            return nId_Zip;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psHashZip"></param>
        /// <returns></returns>
        private int fnObtenerZipConamm(string psHashZip)
        {
            int nId_Zip = 0;

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
            {
                conexion.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac_Zip_Buscar_Sel", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sHASH", psHashZip);
                        nId_Zip = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error al buscar el Id ZIP: " + psHashZip + " - " + ex.Message);
                }

            }
            return nId_Zip;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psHashZip"></param>
        /// <returns></returns>
        private int fnObtenerZipDatos(string psHashZip)
        {
            int nId_Zip = 0;

            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
            {
                conexion.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac_Zip_Datos_Buscar_Sel", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sHASH", psHashZip);
                        nId_Zip = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error al buscar el Id ZIP: " + psHashZip + " - " + ex.Message);
                }

            }
            return nId_Zip;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psHashZip"></param>
        /// <returns></returns>
        private int fnObtenerZipDatos40(string psHashZip)
        {
            int nId_Zip = 0;

            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
            {
                conexion.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac40_Zip_Datos_Buscar_Sel", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sHASH", psHashZip);
                        nId_Zip = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error al buscar el Id ZIP: " + psHashZip + " - " + ex.Message);
                }

            }
            return nId_Zip;
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
        /// Metodo que se encarga de registrar un comprobante pendiente en la BD de DBSANJOSE
        /// </summary>
        /// <param name="psComprobante">Comprobante</param>
        /// <param name="psOrigen">Origen</param>
        /// <param name="psNombreArchivo">Nombre del archivo</param>
        /// <param name="dFechaTimbrado">Fecha de timbrado</param>
        /// <param name="pnId_Zip">ID del Zip</param>
        /// <param name="psEstatus">Estatus</param>
        private void fnInsertarComprobante(string psComprobante, string psOrigen, string psNombreArchivo, DateTime dFechaTimbrado, int pnId_Zip, string psEstatus)
        {
            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_Cfd_Comprobante_ins_pendiente", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sXml", psComprobante);
                        command.Parameters.AddWithValue("sOrigen", psOrigen);
                        command.Parameters.AddWithValue("sNombre_archivo", psNombreArchivo);
                        command.Parameters.AddWithValue("dFecha_Timbrado", dFechaTimbrado);
                        command.Parameters.AddWithValue("sEstatus", psEstatus);
                        command.Parameters.AddWithValue("nIdZip", pnId_Zip);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    throw new Exception("No se ha podido insertar el comprobante: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Metodo que se encarga de registrar un comprobante pendiente en la BD de DBSANJOSE
        /// </summary>
        /// <param name="psComprobante">Comprobante</param>
        /// <param name="psOrigen">Origen</param>
        /// <param name="psNombreArchivo">Nombre del archivo</param>
        /// <param name="dFechaTimbrado">Fecha de timbrado</param>
        /// <param name="pnId_Zip">ID del Zip</param>
        /// <param name="psEstatus">Estatus</param>
        private void fnInsertarComprobante(string psComprobante, string psOrigen, string psNombreArchivo, DateTime dFechaTimbrado, int pnId_Zip, string psHashEmisor, string psEstatus)
        {
            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_Cfd_Comprobante_ins_pendiente", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sXml", psComprobante);
                        command.Parameters.AddWithValue("sOrigen", psOrigen);
                        command.Parameters.AddWithValue("sNombre_archivo", psNombreArchivo);
                        command.Parameters.AddWithValue("dFecha_Timbrado", dFechaTimbrado);
                        command.Parameters.AddWithValue("sEstatus", psEstatus);
                        command.Parameters.AddWithValue("nIdZip", pnId_Zip);
                        command.Parameters.AddWithValue("sDatos", psHashEmisor);
                      
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    throw new Exception("No se ha podido insertar el comprobante: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Metodo que se encarga de registrar un comprobante pendiente en la BD de DBSANJOSE
        /// </summary>
        /// <param name="psComprobante">Comprobante</param>
        /// <param name="psOrigen">Origen</param>
        /// <param name="psNombreArchivo">Nombre del archivo</param>
        /// <param name="dFechaTimbrado">Fecha de timbrado</param>
        /// <param name="pnId_Zip">ID del Zip</param>
        /// <param name="psEstatus">Estatus</param>
        private void fnInsertarComprobante(string psComprobante, string psOrigen, string psNombreArchivo, DateTime dFechaTimbrado, int pnId_Zip, string psHashEmisor, string psUsuario, string psEstatus, string psAddenda, int pnIdPac)
        {
            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac_Comprobante_Datos_Ins_pendiente", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sXml", psComprobante);
                        command.Parameters.AddWithValue("sOrigen", psOrigen);
                        command.Parameters.AddWithValue("sNombre_archivo", psNombreArchivo);
                        command.Parameters.AddWithValue("dFecha_Timbrado", dFechaTimbrado);
                        command.Parameters.AddWithValue("sEstatus", psEstatus);
                        command.Parameters.AddWithValue("nIdZip", pnId_Zip);
                        command.Parameters.AddWithValue("sDatos", psHashEmisor);
                        command.Parameters.AddWithValue("nIdPac", pnIdPac);

                        if (!string.IsNullOrEmpty(psAddenda))
                            command.Parameters.AddWithValue("sAddenda", psAddenda);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    throw new Exception("No se ha podido insertar el comprobante: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Metodo que se encarga de registrar un comprobante pendiente en la BD de DBSANJOSE
        /// </summary>
        /// <param name="psComprobante">Comprobante</param>
        /// <param name="psOrigen">Origen</param>
        /// <param name="psNombreArchivo">Nombre del archivo</param>
        /// <param name="dFechaTimbrado">Fecha de timbrado</param>
        /// <param name="pnId_Zip">ID del Zip</param>
        /// <param name="psEstatus">Estatus</param>
        private void fnInsertarComprobante40(string psComprobante, string psOrigen, string psNombreArchivo, DateTime dFechaTimbrado, int pnId_Zip, string psHashEmisor, string psUsuario, string psEstatus, string psAddenda, int pnIdPac)
        {
            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac40_Comprobante_Datos_Ins_pendiente", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sXml", psComprobante);
                        command.Parameters.AddWithValue("sOrigen", psOrigen);
                        command.Parameters.AddWithValue("sNombre_archivo", psNombreArchivo);
                        command.Parameters.AddWithValue("dFecha_Timbrado", dFechaTimbrado);
                        command.Parameters.AddWithValue("sEstatus", psEstatus);
                        command.Parameters.AddWithValue("nIdZip", pnId_Zip);
                        command.Parameters.AddWithValue("sDatos", psHashEmisor);
                        command.Parameters.AddWithValue("nIdPac", pnIdPac);

                        if (!string.IsNullOrEmpty(psAddenda))
                            command.Parameters.AddWithValue("sAddenda", psAddenda);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    throw new Exception("No se ha podido insertar el comprobante: " + ex.Message);
                }
            }
        }


        /// <summary>
        /// Método que se encarga para registrar un comprobante pendiente de la BD de Interceramic
        /// </summary>
        /// <param name="psComprobante">Comprobante</param>
        /// <param name="psNombreArchivo">Nombre Archivo</param>
        /// <param name="dFechaTimbrado">Fecha Timbrado</param>
        /// <param name="pnId_Zip">ID Zip</param>
        /// <param name="psEstatus">Estatus</param>
        private void fnInsertarComprobante(string psComprobante, string psNombreArchivo, DateTime dFechaTimbrado, int pnId_Zip, string psEstatus)
        {
            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_Cfd_Comprobante_ins_pendiente", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sXml", psComprobante);
                        command.Parameters.AddWithValue("sNombre_archivo", psNombreArchivo);
                        command.Parameters.AddWithValue("dFecha_Timbrado", dFechaTimbrado);
                        command.Parameters.AddWithValue("sEstatus", psEstatus);
                        command.Parameters.AddWithValue("nIdZip", pnId_Zip);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    throw new Exception("No se ha podido insertar el comprobante: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psComprobante"></param>
        /// <param name="psOrigen"></param>
        /// <param name="psNombreArchivo"></param>
        /// <param name="dFechaTimbrado"></param>
        /// <param name="pnId_Zip"></param>
        /// <param name="psHashEmisor"></param>
        /// <param name="psUsuario"></param>
        /// <param name="psEstatus"></param>
        /// <param name="psUUID"></param>
        /// <param name="psRFCReceptor"></param>
        /// <param name="pnIdOrganismo"></param>
        /// <param name="psCURP"></param>
        /// <param name="psNumeroEmpleado"></param>
        /// <param name="pdFechaPago"></param>
        /// <param name="psHash"></param>
        /// <param name="psVersion"></param>
        private void fnInsertarComprobante(string psComprobante, string psOrigen, string psNombreArchivo, DateTime dFechaTimbrado, int pnId_Zip, string psHashEmisor, string psUsuario,
            string psEstatus, string psUUID, string psRFCReceptor, int pnIdOrganismo, string psCURP, string psNumeroEmpleado, DateTime pdFechaPago, string psHash, string psVersion, string psVersionCFDI)
        {
            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_Cfd_Comprobante_Ins_pendiente", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sXml", psComprobante);
                        command.Parameters.AddWithValue("sOrigen", psOrigen);
                        command.Parameters.AddWithValue("sNombre_archivo", psNombreArchivo);
                        command.Parameters.AddWithValue("sEstatus", psEstatus);
                        command.Parameters.AddWithValue("nIdZip", pnId_Zip);
                        command.Parameters.AddWithValue("sUUID", psUUID);
                        command.Parameters.AddWithValue("sRfcReceptor", psRFCReceptor);
                        command.Parameters.AddWithValue("nIdOrganismo", pnIdOrganismo);
                        command.Parameters.AddWithValue("sCURP", psCURP);
                        command.Parameters.AddWithValue("sNumEmpleado", psNumeroEmpleado);
                        command.Parameters.AddWithValue("dFechaPago", pdFechaPago);
                        command.Parameters.AddWithValue("sHash", psHash);
                        command.Parameters.AddWithValue("sVersion", psVersion);
                        command.Parameters.AddWithValue("sVersionCFDI", psVersionCFDI);
                        command.Parameters.AddWithValue("dFechaTimbrado", dFechaTimbrado);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    throw new Exception("No se ha podido insertar el comprobante: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psComprobante"></param>
        /// <param name="psOrigen"></param>
        /// <param name="psNombreArchivo"></param>
        /// <param name="dFechaTimbrado"></param>
        /// <param name="pnId_Zip"></param>
        /// <param name="psHashEmisor"></param>
        /// <param name="psUsuario"></param>
        /// <param name="psEstatus"></param>
        /// <param name="psUUID"></param>
        /// <param name="psRFCReceptor"></param>
        /// <param name="pnIdOrganismo"></param>
        /// <param name="psCURP"></param>
        /// <param name="psNumeroEmpleado"></param>
        /// <param name="pdFechaPago"></param>
        /// <param name="psHash"></param>
        /// <param name="psVersion"></param>
        private void fnInsertarComprobanteConamm(string psComprobante, string psOrigen, string psNombreArchivo, DateTime dFechaTimbrado, int pnId_Zip, string psHashEmisor, string psUsuario,
            string psEstatus, string psUUID, string psRFCReceptor, int pnIdOrganismo, string psCURP, string psNumeroEmpleado, DateTime pdFechaPago, string psHash, string psVersion)
        {
            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_mpac40_Comprobante_Datos_Ins", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sXml", PAXCrypto.CryptoAES.EncriptaAES(psComprobante));
                        command.Parameters.AddWithValue("sOrigen", psOrigen);
                        command.Parameters.AddWithValue("sNombre_archivo", psNombreArchivo);
                        //command.Parameters.AddWithValue("dFecha_Timbrado", dFechaTimbrado);
                        command.Parameters.AddWithValue("sEstatus", psEstatus);
                        command.Parameters.AddWithValue("nIdZip", pnId_Zip);
                        command.Parameters.AddWithValue("sUUID", psUUID);
                        command.Parameters.AddWithValue("sRfc_Receptor", psRFCReceptor);
                        command.Parameters.AddWithValue("nIdOrganismo", pnIdOrganismo);
                        command.Parameters.AddWithValue("sCURP", PAXCrypto.CryptoAES.EncriptaAES(psCURP));
                        command.Parameters.AddWithValue("sNumEmpleado", psNumeroEmpleado);
                        command.Parameters.AddWithValue("dFechaPago", pdFechaPago);
                        command.Parameters.AddWithValue("sHash", psHash);

                        command.Parameters.AddWithValue("sVersionCFDI", "4.0");
                        command.Parameters.AddWithValue("nIdPac", 3);
                        command.Parameters.AddWithValue("sAddenda", string.Empty);
                        command.Parameters.AddWithValue("sVersionNomina", "1.2");

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    throw new Exception("No se ha podido insertar el comprobante: " + ex.Message);
                }
            }
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
        public static int fnInsertarComprobanteSAN(
                                                    string psComprobante,
                                                    int pnIdTipoDocumento,
                                                    char pcEstatus,
                                                    DateTime pdFechaDocumento,
                                                    int pnIdEstructura,
                                                    int pnIdUsuarioTimbrado,
                                                    string psOrigen,
                                                    string psHASHTimbre,
                                                    string psHASHEmisor,
                                                    string psUUID,
                                                    DateTime pdFechaTimbrado,
                                                    string psRfcEmisor,
                                                    string psNombreEmisor,
                                                    string psRfcReceptor,
                                                    string psNombreReceptor,
                                                    DateTime pdFechaEmision,
                                                    string psSerie,
                                                    string psFolio,
                                                    string psTotal,
                                                    string psMoneda
                                                )
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
                        using (SqlCommand cmd = new SqlCommand("usp_Timbrado_InsertaComprobanteAll_Completo_Ins", con))
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
        /// <param name="dFechaTimbrado"></param>
        /// <param name="pnId_Zip"></param>
        /// <param name="psHashEmisor"></param>
        private void fnActualizarComprobante(DateTime dFechaTimbrado, int pnId_Zip, string psHashEmisor)
        {
            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = conexion;
                        command.CommandType = CommandType.Text;
                        string sInstruccion = "UPDATE [dbo].[tbl_cfd_Comprobantes_cat] SET fecha_timbrado = '{0}' where datos = '{1}' and id_zip = {2}";
                        command.CommandText = string.Format(sInstruccion, dFechaTimbrado.Year + "-" + dFechaTimbrado.Month + "-" + dFechaTimbrado.Day + " " + dFechaTimbrado.Hour + ":" + dFechaTimbrado.Minute + ":" + dFechaTimbrado.Second + "." + dFechaTimbrado.Millisecond, psHashEmisor, pnId_Zip);
                        //command.CommandText = string.Format(sInstruccion, dFechaTimbrado.Day + "-" + dFechaTimbrado.Month + "-" + dFechaTimbrado.Year + " " + dFechaTimbrado.Hour + ":" + dFechaTimbrado.Minute + ":" + dFechaTimbrado.Second + "." + dFechaTimbrado.Millisecond, psHashEmisor, pnId_Zip);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    throw new Exception("No se ha podido actualizar el comprobante: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Metodo que se encarga de revisar si un comprobante ya se encuentra registrado por medio del Hash del Emisor
        /// </summary>
        /// <param name="psHashEmisor">Hash del Emisor</param>
        /// <param name="sNombreArchivoZip">Nombre del Zip</param>
        /// <param name="sNombreXML">Nombre XML</param>
        /// <returns></returns>
        private bool fnExisteComprobante(string psHashEmisor, string sNombreArchivoZip, string sNombreXML)
        {
            bool bResultado = false;
            string nComprobante = string.Empty;
            string sHashEmisor = string.Empty;
            try
            {
                nComprobante = fnObtenerHashComprobante(psHashEmisor, sNombreArchivoZip, sNombreXML);
                if (!nComprobante.Equals("0"))
                {
                    bResultado = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible buscar el hash del comprobante: " + sNombreXML + " - " + ex.Message);
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
        private bool fnExisteComprobanteConamm(string psHashEmisor, string sNombreArchivoZip, string sNombreXML)
        {
            bool bResultado = false;
            string nComprobante = string.Empty;
            string sHashEmisor = string.Empty;
            try
            {
                nComprobante = fnObtenerHashComprobanteConamm(psHashEmisor, sNombreArchivoZip, sNombreXML);
                if (!nComprobante.Equals("0"))
                {
                    bResultado = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible buscar el hash del comprobante: " + sNombreXML + " - " + ex.Message);
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
        private DataTable fnExisteComprobanteSAN(string psUuid)
        {
            DataTable dtResultado = new DataTable();

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_cfd33_Comprobantes_sel_Uuid", conexion))
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
        /// Sobrecarga que realize para poder escribir en el log de xml en caso de error
        /// </summary>
        /// <param name="psHashEmisor"></param>
        /// <param name="sNombreArchivoZip"></param>
        /// <param name="sNombreXML"></param>
        /// <returns></returns>
        private string fnObtenerHashComprobante(string psHashEmisor, string sNombreArchivoZip, string sNombreXML)
        {
            string nRetorno = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(PAXCrypto.Base64.DesencriptarBase64(Settings.Default.Conexion)))
                {
                    conexion.Open();
                    // Se busca el comprobante 
                    using (SqlCommand comando = new SqlCommand("usp_Comprobantes_BuscaHASH_XML_Sel_pendiente", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@sHash", psHashEmisor);
                        nRetorno = Convert.ToString(comando.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible buscar el Hash: " + sNombreXML + " - " + psHashEmisor + " - " + ex.Message);
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
        private string fnObtenerHashComprobante(string psHashEmisor, int pnIdUsuario, string psTipo)
        {
            string nRetorno = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
                {
                    conexion.Open();
                    // Se busca el comprobante 
                    using (SqlCommand comando = new SqlCommand("usp_Timbrado_BuscaHASH_XML_Sel", conexion))
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
        private string fnObtenerHashComprobanteConamm(string psHashEmisor, string sNombreArchivoZip, string sNombreXML)
        {
            string nRetorno = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
                {
                    conexion.Open();
                    // Se busca el comprobante 
                    using (SqlCommand comando = new SqlCommand("usp_mpac_Comprobantes_BuscaHASH_XML_Sel", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@sHash", psHashEmisor);
                        nRetorno = Convert.ToString(comando.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible buscar el Hash: " + sNombreXML + " - " + psHashEmisor + " - " + ex.Message);
            }
            return nRetorno;
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
    }
}

using PAXEntregaPendientesZip.Properties;
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

namespace PAXEntregaPendientesZip
{
    public partial class frmEntregaPendientes : Form
    {
        public frmEntregaPendientes()
        {
            InitializeComponent();
        }

        private void frmEntregaPendientes_Load(object sender, EventArgs e)
        {
            btnEntregarArchivos.Enabled = false;
        }
        private void btnPrueba_Click(object sender, EventArgs e)
        {
            DateTime dFechaComprobante;
            DateTime dFechaTimbrado;
            int nContador = 0;
            string sCadenaOriginalEmisor = string.Empty;
            string sCadenaOriginal = string.Empty;
            string HASHEmisor = string.Empty;
            System.Diagnostics.EventLog eventLogExport = new System.Diagnostics.EventLog();

            int nArchivos = lbComprobantes.Items.Count;

            DataTable dtComprobantes = new DataTable();
            if (txtUsuario.Text.Trim().Equals("NADROFACTURACION"))
            {
                dtComprobantes.Columns.Add("Nombre Archivo", typeof(string));
                dtComprobantes.Columns.Add("Origen", typeof(string));
                dtComprobantes.Columns.Add("NombreZip", typeof(string));
                dtComprobantes.Columns.Add("FechaTimbrado", typeof(DateTime));
                dtComprobantes.Columns.Add("Hash", typeof(string));
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

                File.Copy(nombreArchivo, Settings.Default.RutaPendientes + sNombreArchivo);

                var contenidoArchivo = RecuperaArchivo(Settings.Default.RutaPendientes + sNombreArchivo);
                byte[] contenidoArchivoBytes = RecuperaArchivoByte(contenidoArchivo, Settings.Default.RutaPendientes + sNombreArchivo);

                contenidoArchivo.Close();

                XmlDocument sXmlDocument = new XmlDocument();
                sXmlDocument.Load(Settings.Default.RutaPendientes + sNombreArchivo);

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(sXmlDocument.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                dFechaComprobante = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).ValueAsDateTime;
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
                    xslt.Load(typeof(CaOri.V3211));
                    ms = new MemoryStream();
                    args = new XsltArgumentList();
                    xslt.Transform(navNodoTimbre, args, ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    srDll = new StreamReader(ms);

                    sCadenaOriginalEmisor = srDll.ReadToEnd();
                    HASHEmisor = GetHASH(sCadenaOriginalEmisor);

                    sNombreZip = Path.GetFileName(lbZips.SelectedValue.ToString());

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
                else
                {
                    sNombreZip = Path.GetFileName(lbZips.SelectedValue.ToString());

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
            if (txtUsuario.Text.Trim().Equals("NADROFACTURACION"))
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

                dFechaComprobante = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).ValueAsDateTime;
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
                    xslt.Load(typeof(CaOri.V3211));
                    ms = new MemoryStream();
                    args = new XsltArgumentList();
                    xslt.Transform(navNodoTimbre, args, ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    srDll = new StreamReader(ms);

                    sCadenaOriginalEmisor = srDll.ReadToEnd();
                    HASHEmisor = GetHASH(sCadenaOriginalEmisor);

                    sNombreZip = Path.GetFileName(lbZips.SelectedValue.ToString());
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

                        sComprobante = sXmlDocument.InnerXml;


                        if (rbActivo.Checked)
                            sEstatus = "A";
                        else
                            sEstatus = "P";

                        fnInsertarComprobante(sComprobante, txtOrigen.Text, sNombreArchivo, dFechaTimbrado, nId_Zip, HASHEmisor, txtUsuario.Text, sEstatus);

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
                    sNombreZip = Path.GetFileName(lbZips.SelectedValue.ToString());
                    sHashzip = GetHASH(sNombreZip).ToString();

                    if (string.IsNullOrEmpty(sNombreZip))
                        continue;

                    nId_Zip = fnObtenerZip(sHashzip);

                    if (nId_Zip.Equals(0))
                        continue;

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

                        if (rbActivo.Checked)
                            sEstatus = "A";
                        else
                            sEstatus = "P";

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

        private IList RecuperaListaArchivos(string directorioRaiz)
        {
            IList listaArchivos = Directory.GetFiles(directorioRaiz).ToList();
            return listaArchivos;
        }

        private Stream RecuperaArchivo(string rutaAbsoluta)
        {
            return File.OpenRead(rutaAbsoluta);
        }

        private byte[] RecuperaArchivoByte(Stream rutaAbsoluta, string ruta)
        {
            StreamReader sr = new StreamReader(rutaAbsoluta);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            return encoding.GetBytes(sr.ReadToEnd());


        }

        private string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        private int fnObtenerZip(string psHashZip)
        {
            int nId_Zip = 0;

            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
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

        private void fnInsertarComprobante(string psComprobante, string psOrigen, string psNombreArchivo, DateTime dFechaTimbrado, int pnId_Zip, string psHashEmisor, string psUsuario, string psEstatus)
        {
            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
            {
                conexion.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = conexion;
                        command.CommandType = CommandType.Text;
                        string sInstruccion = "INSERT INTO [dbo].[tbl_cfd_Comprobantes_cat]	VALUES ('{0}','{1}','{2}','{3}','{4}',{5},'{6}')";
                        command.CommandText = string.Format(sInstruccion, psComprobante, psOrigen, psNombreArchivo, dFechaTimbrado.Year + "-" + dFechaTimbrado.Month + "-" + dFechaTimbrado.Day + " " + dFechaTimbrado.Hour + ":" + dFechaTimbrado.Minute + ":" + dFechaTimbrado.Second + "." + dFechaTimbrado.Millisecond,
                                                psEstatus, pnId_Zip, psHashEmisor);

                        //command.CommandText = string.Format(sInstruccion, psComprobante, psOrigen, psNombreArchivo, dFechaTimbrado.Day + "-" + dFechaTimbrado.Month + "-" + dFechaTimbrado.Year + " " + dFechaTimbrado.Hour + ":" + dFechaTimbrado.Minute + ":" + dFechaTimbrado.Second + "." + dFechaTimbrado.Millisecond, 
                        //                        "P", pnId_Zip, psHashEmisor);

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
        private void fnInsertarComprobante(string psComprobante, string psOrigen, string psNombreArchivo, DateTime dFechaTimbrado, int pnId_Zip, string psEstatus)
        {
            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
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
        /// Método que se encarga para registrar un comprobante pendiente de la BD de Interceramic
        /// </summary>
        /// <param name="psComprobante">Comprobante</param>
        /// <param name="psNombreArchivo">Nombre Archivo</param>
        /// <param name="dFechaTimbrado">Fecha Timbrado</param>
        /// <param name="pnId_Zip">ID Zip</param>
        /// <param name="psEstatus">Estatus</param>
        private void fnInsertarComprobante(string psComprobante, string psNombreArchivo, DateTime dFechaTimbrado, int pnId_Zip, string psEstatus)
        {
            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
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

        private void fnActualizarComprobante(DateTime dFechaTimbrado, int pnId_Zip, string psHashEmisor)
        {
            using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
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
                using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(Settings.Default.Conexion)))
                {
                    conexion.Open();
                    // Se busca el comprobante 
                    using (SqlCommand comando = new SqlCommand("usp_Comprobantes_BuscaHASH_XML_Sel", conexion))
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

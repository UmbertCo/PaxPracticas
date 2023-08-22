using Microsoft.VisualBasic.FileIO;
using SolucionPruebas.Presentacion.WindowsForms1.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace SolucionPruebas.Presentacion.WindowsForms1
{
    public partial class frmBulkLCO : Form
    {
        public string[] asListadoArchivos;
        //DataTable dtLCO = new DataTable();
        System.Windows.Forms.Timer timer;

        delegate void InicializarProgresoDelegado(int pnValor);
        delegate void CambiarEstatusDelegado(string psAccion);
        delegate void CambiarNombreArchivoDelegado(string psNombreArchivo); 

        public frmBulkLCO()
        {
            InitializeComponent();
        }

        private void frmBulkLCO_Load(object sender, EventArgs e)
        {
            lblArchivoValor.Text = string.Empty;
            lblEstatusValor.Text = string.Empty;
            lblInicioValor.Text = string.Empty;
            lblProgresoValor.Text = string.Empty;
        }

        private void btnBulk_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdArchivo = new OpenFileDialog();
            ofdArchivo.Multiselect = true;
            ofdArchivo.ShowDialog();
            
            try
            {
                if (ofdArchivo.FileNames == null)
                    throw new Exception("No se ha seleccionado ningun archivo");

                lblArchivoValor.Text = string.Empty;
                lblEstatusValor.Text = string.Empty;
                lblInicioValor.Text = string.Empty;
                lblProgresoValor.Text = string.Empty;

                lblEstatusValor.BackColor = SystemColors.Control;
                lblEstatusValor.ForeColor = Color.Black;

                btnBulk.Enabled = false;
                btnCancelarBulk.Enabled = true;

                asListadoArchivos = ofdArchivo.FileNames;
                lblArchivoValor.Text = ofdArchivo.FileName;

                GC.Collect();
                GC.WaitForFullGCComplete();

                timer = new System.Windows.Forms.Timer();
                timer.Interval = 750;
                timer.Enabled = false;
                //timer.Tick -= timer_Tick;
                timer.Start();

                //if (messagesNum > oldMessagesNum)
                timer.Tick += new EventHandler(timer_Tick);
                //else
                //    timer.Tick -= timer_Tick;

                bwLCO.WorkerSupportsCancellation = true;
                bwLCO.WorkerReportsProgress = true;
                bwLCO.DoWork += new DoWorkEventHandler(bwLCO_DoWork);
                bwLCO.ProgressChanged += new ProgressChangedEventHandler(bwLCO_ProgressChanged);
                bwLCO.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLCO_RunWorkerCompleted);
                bwLCO.RunWorkerAsync();

                #region Comentada
                //foreach (string sRutaArchivo in ofdArchivo.FileNames)
                //{
                //    DataTable dtLCO = new DataTable();
                //    dtLCO = new DataTable();
                //    dtLCO.Columns.Add("RFC", typeof(string));
                //    dtLCO.Columns.Add("ValidezObligaciones", typeof(byte));
                //    dtLCO.Columns.Add("EstatusCertificado", typeof(char));
                //    dtLCO.Columns.Add("noCertificado", typeof(string));
                //    dtLCO.Columns.Add("FechaFinal", typeof(DateTime));
                //    dtLCO.Columns.Add("FechaInicio", typeof(DateTime));

                //    FileStream inFile = null;
                //    FileStream outFile = null;
                //    CMS cms = null;
                //    Stream archivo = null;
                //    BinaryReader zr = null;

                //    string sNombreArchivo = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(sRutaArchivo));
                //    string sRuta = Path.GetDirectoryName(sRutaArchivo) + @"\";

                //    try
                //    {
                //        using (FileStream fInStream = new FileStream(sRutaArchivo, FileMode.Open, FileAccess.Read))
                //        {
                //            using (GZipStream zipStream = new GZipStream(fInStream, CompressionMode.Decompress))
                //            {
                //                using (FileStream fOutStream = new FileStream(sRuta + sNombreArchivo + ".xml", FileMode.Create, FileAccess.Write))
                //                {
                //                    byte[] tempBytes = new byte[4096];
                //                    int i;
                //                    while ((i = zipStream.Read(tempBytes, 0, tempBytes.Length)) != 0)
                //                    {
                //                        fOutStream.Write(tempBytes, 0, i);
                //                    }
                //                }
                //            }
                //        }

                //        inFile = File.Open(sRuta + sNombreArchivo + ".xml", FileMode.Open);
                //        outFile = File.Create(sRuta + sNombreArchivo + "Desencriptado.txt");

                //        cms = new CMS();
                //        cms.Decode(inFile, outFile);
                //        inFile.Close();
                //        outFile.Close();

                //        archivo = File.Open(sRuta + sNombreArchivo + "Desencriptado.txt", FileMode.Open);
                //        zr = new BinaryReader(archivo);

                //        XmlDocument doc = new XmlDocument();
                //        StringBuilder sb = new StringBuilder();

                //        long largo = archivo.Length;
                //        int parte = 1000000;
                //        bool cortar = true;
                //        string aux = string.Empty;
                //        int index = 0;

                //        while (largo > 0)
                //        {
                //            string linea = aux + System.Text.Encoding.UTF8.GetString(zr.ReadBytes(parte));

                //            if (cortar)
                //            {
                //                linea = linea.Substring(linea.IndexOf("<lco:Con"));
                //                cortar = false;
                //            }

                //            if (largo < parte)
                //            {
                //                linea = linea.Substring(0, linea.IndexOf("LCO>") - 6);
                //            }

                //            index = linea.LastIndexOf("</lco:Contribuyente>");

                //            try
                //            {
                //                aux = linea.Substring(index + 20);
                //                linea = linea.Remove(index + 20);
                //            }
                //            catch { }

                //            linea = linea.Replace("lco:", string.Empty);

                //            doc.LoadXml("<root></root>");
                //            doc.DocumentElement.InnerXml = linea;

                //            XPathNodeIterator nodos = doc.CreateNavigator().Select("/root/Contribuyente");

                //            sb = new StringBuilder();

                //            while (nodos.MoveNext())
                //            {
                //                string rfc = nodos.Current.SelectSingleNode("@RFC").Value;

                //                XPathNodeIterator cert = nodos.Current.Select("Certificado");

                //                while (cert.MoveNext())
                //                {
                //                    DataRow drRenglon = dtLCO.NewRow();
                //                    drRenglon["RFC"] = rfc;
                //                    drRenglon["ValidezObligaciones"] = Convert.ToByte(cert.Current.SelectSingleNode("@ValidezObligaciones").Value);
                //                    drRenglon["EstatusCertificado"] = cert.Current.SelectSingleNode("@EstatusCertificado").Value;
                //                    drRenglon["noCertificado"] = cert.Current.SelectSingleNode("@noCertificado").Value;
                //                    drRenglon["FechaFinal"] = cert.Current.SelectSingleNode("@FechaFinal").Value;
                //                    drRenglon["FechaInicio"] = cert.Current.SelectSingleNode("@FechaInicio").Value;
                //                    dtLCO.Rows.Add(drRenglon);
                //                }
                //            }

                //            largo -= parte;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show("Ocurrio un error al generar los archivos: " + sNombreArchivo + " " + ex.Message);
                //    }
                //    finally
                //    {
                //        if (inFile != null)
                //            inFile.Close();

                //        if (outFile != null)
                //            outFile.Close();

                //        if (zr != null)
                //            zr.Close();

                //        if (archivo != null)
                //            archivo.Close();
                //    }


                //    if (dtLCO.Rows.Count <= 0)
                //        throw new Exception("No hay registros en el DataTable del archivo: " + sNombreArchivo);

                //    pbBulk.Maximum = dtLCO.Rows.Count;

                //    using (SqlConnection conn = new SqlConnection(Settings.Default.conexion))
                //    {
                //        SqlBulkCopy bulkCopy = null;
                //        try
                //        {
                //            using (bulkCopy = new SqlBulkCopy(conn.ConnectionString))
                //            {
                //                bulkCopy.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);

                //                bulkCopy.BulkCopyTimeout = 480;
                //                bulkCopy.BatchSize = 1000;
                //                bulkCopy.NotifyAfter = 200;
                //                bulkCopy.ColumnMappings.Add("RFC", "rfc");
                //                bulkCopy.ColumnMappings.Add("ValidezObligaciones", "validez_obligaciones");
                //                bulkCopy.ColumnMappings.Add("EstatusCertificado", "estatus_certificado");
                //                bulkCopy.ColumnMappings.Add("noCertificado", "no_certificado");
                //                bulkCopy.ColumnMappings.Add("FechaFinal", "fecha_final");
                //                bulkCopy.ColumnMappings.Add("FechaInicio", "fecha_inicio");
                //                bulkCopy.DestinationTableName = "tbl_ctp_LCO_apl_tmp";
                //                bulkCopy.WriteToServer(dtLCO);
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            if (conn != null)
                //            {
                //                conn.Close();
                //            }
                //            throw new Exception("Error al realizar la instrucción Bulk de la LCO en BD: " + ex.Message);
                //        }
                //        finally
                //        {
                //            if (bulkCopy != null)
                //            {
                //                using (bulkCopy as IDisposable)
                //                {

                //                }
                //            }

                //            dtLCO.Rows.Clear();
                //            dtLCO.Clear();
                //            dtLCO = null;

                //            GC.Collect();
                //            GC.WaitForFullGCComplete();
                //        }
                //    } // Fin de la conexión
                //    pbBulk.Value = 0;
                //    MessageBox.Show("Se ha terminado el Bulk del archivo: " + sNombreArchivo);
                //}// fin del for each
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al hacer el Bulk: " + ex.Message);
            }
            finally
            {
                //GC.Collect();
                //GC.WaitForFullGCComplete();
            }
        }

        private void btnCancelarBulk_Click(object sender, EventArgs e)
        {
            if (bwLCO.IsBusy)
            {
                btnCancelarBulk.Enabled = false;
                lblEstatusValor.Text = "Cancelando...";

                // Notify the worker thread that a cancel has been requested.
                // The cancel will not actually happen until the thread in the
                // DoWork checks the bwAsync.CancellationPending flag, for this
                // reason we set the label to "Cancelling...", because we haven't
                // actually cancelled yet.
                bwLCO.CancelAsync();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblEstatusValor.Text))
            {
                if (lblEstatusValor.BackColor == Color.Black)
                {
                    lblEstatusValor.BackColor = Color.Red;
                    lblEstatusValor.ForeColor = Color.Black;
                }
                else
                {
                    lblEstatusValor.BackColor = Color.Black;
                    lblEstatusValor.ForeColor = Color.White;
                }
            }  
        }

        private void OnSqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            bwLCO.ReportProgress(Convert.ToInt32(e.RowsCopied));
        }

        private void bwLCO_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbBulk.Value = e.ProgressPercentage;
            this.lblProgresoValor.Text = (e.ProgressPercentage.ToString());
        }

        private void bwLCO_DoWork(object sender, DoWorkEventArgs e)
        {
            fnSubirLCO(e);
        }

        private void bwLCO_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                //this.lblProgresoValor.Text = "Cancelado!";
                this.lblEstatusValor.Text = "Cancelado!";
            }
            else if (!(e.Error == null))
            {
                //this.lblProgresoValor.Text = ("Error: " + e.Error.Message);
                this.lblEstatusValor.Text = ("Error: " + e.Error.Message);
            }
            else
            {
                this.lblEstatusValor.Text = "Se acabo!";
                //this.lblProgresoValor.Text = "Se acabo!";
            }

            btnBulk.Enabled = true;
            btnCancelarBulk.Enabled = false;

            lblEstatusValor.BackColor = SystemColors.Control;
            lblEstatusValor.ForeColor = Color.Black;
            timer.Enabled = false;
            timer.Stop();

            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        private void fnSubirLCO(DoWorkEventArgs e)
        {
            try
            {
                foreach (string sRutaArchivo in asListadoArchivos)
                {
                    if (bwLCO.CancellationPending)
                    {
                        // Set the e.Cancel flag so that the WorkerCompleted event
                        // knows that the process was cancelled.
                        e.Cancel = true;
                        bwLCO.ReportProgress(0);
                        return;
                    }

                    //BinaryReader zr = null;
                    string sNombreArchivo = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(sRutaArchivo));
                    string sRuta = Path.GetDirectoryName(sRutaArchivo) + @"\";

                    fnCambiarEstatus("Descomprimir " + sNombreArchivo);
                    fnDescomprimirArchivo(sRuta, sRutaArchivo, sNombreArchivo);

                    // Revisamos si se realiza la petición de cancelación
                    if (bwLCO.CancellationPending)
                    {
                        // Set the e.Cancel flag so that the WorkerCompleted event
                        // knows that the process was cancelled.
                        e.Cancel = true;
                        bwLCO.ReportProgress(0);
                        return;
                    }

                    fnCambiarEstatus("Decodificar " + sNombreArchivo);
                    fnDecodificarArchivo(sRuta, sNombreArchivo);

                    // Revisamos si se realiza la petición de cancelación
                    if (bwLCO.CancellationPending)
                    {
                        // Set the e.Cancel flag so that the WorkerCompleted event
                        // knows that the process was cancelled.
                        e.Cancel = true;
                        bwLCO.ReportProgress(0);
                        return;
                    }

                    DataTable dtLCO = new DataTable();
                    fnCambiarEstatus("Generar registros " + sNombreArchivo);
                    dtLCO = fnGeneracionLCODataTable(sRuta, sNombreArchivo);
                    //dtLCO = fnGeneracionLCOXml(sRuta, sNombreArchivo);
                    //dtLCO = fnObtenerDatos(sRuta + sNombreArchivo + "_Desencriptado.txt");

                    if (dtLCO.Rows.Count <= 0)
                        throw new Exception("No hay registros en el DataTable del archivo: " + sNombreArchivo);

                    //if (string.IsNullOrEmpty(doc.InnerXml.ToString()))
                    //    throw new Exception("No hay registros en el string del archivo: " + sNombreArchivo);

                    if (bwLCO.CancellationPending)
                    {
                        // Set the e.Cancel flag so that the WorkerCompleted event
                        // knows that the process was cancelled.
                        e.Cancel = true;
                        bwLCO.ReportProgress(0);
                        return;
                    }

                    fnCambiarEstatus("Escribir BulkCopy " + sNombreArchivo);
                    fnIniciarlizarProgressBar(dtLCO.Rows.Count);
                    fnRegistrarLCO(dtLCO);
                    bwLCO.ReportProgress(0);

                    if (bwLCO.CancellationPending)
                    {
                        // Set the e.Cancel flag so that the WorkerCompleted event
                        // knows that the process was cancelled.
                        e.Cancel = true;
                        bwLCO.ReportProgress(0);
                        return;
                    }
                }// fin del for each
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en el proceso de subida de la LCO: " + ex.Message);
            }
        }

        private void fnIniciarlizarProgressBar(int pnValor)
        {
            if (this.InvokeRequired) //preguntamos si la llamada se hace desde un hilo 
            {
                //si es así entonces volvemos a llamar a CambiarProgreso pero esta vez a través del delegado 
                //instanciamos el delegado indicandole el método que va a ejecutar 
                InicializarProgresoDelegado delegado = new InicializarProgresoDelegado(fnIniciarlizarProgressBar);
                //ya que el delegado invocará a CambiarProgreso debemos indicarle los parámetros 
                object[] parametros = new object[] { pnValor };
                //invocamos el método a través del mismo contexto del formulario (this) y enviamos los parámetros 
                this.Invoke(delegado, parametros);
            }
            else
            {
                //en caso contrario, se realiza el llamado a los controles 
                //lblArchivoValor.Text = psNombreArchivo;
                lblInicioValor.Text = pnValor.ToString() + " registros";
                pbBulk.Maximum = pnValor;
            } 
        }

        /// <summary>
        /// Método que se encarga de cambiar el estatus de la operación
        /// </summary>
        /// <param name="psAccion">Acción se se esta realizando</param>
        private void fnCambiarEstatus(string psAccion)
        {
            if (this.InvokeRequired) //preguntamos si la llamada se hace desde un hilo 
            {
                //si es así entonces volvemos a llamar a CambiarProgreso pero esta vez a través del delegado 
                //instanciamos el delegado indicandole el método que va a ejecutar 
                CambiarEstatusDelegado delegado = new CambiarEstatusDelegado(fnCambiarEstatus);
                //ya que el delegado invocará a CambiarProgreso debemos indicarle los parámetros 
                object[] parametros = new object[] { psAccion };
                //invocamos el método a través del mismo contexto del formulario (this) y enviamos los parámetros 
                this.Invoke(delegado, parametros);
            }
            else
            {
                //en caso contrario, se realiza el llamado a los controles 
                //lblArchivoValor.Text = psArchivo;
                lblEstatusValor.Text = psAccion;
            }
        }

        /// <summary>
        /// Método que se encarga de cambiar el valor del documento que se esta procesando
        /// </summary>
        /// <param name="psNombreArchivo">Nombre del archivo</param>
        private void fnCambiarNombreArchivo(string psNombreArchivo)
        {
            if (this.InvokeRequired) //preguntamos si la llamada se hace desde un hilo 
            {
                //si es así entonces volvemos a llamar a CambiarProgreso pero esta vez a través del delegado 
                //instanciamos el delegado indicandole el método que va a ejecutar 
                CambiarNombreArchivoDelegado delegado = new CambiarNombreArchivoDelegado(fnCambiarEstatus);
                //ya que el delegado invocará a CambiarProgreso debemos indicarle los parámetros 
                object[] parametros = new object[] { psNombreArchivo };
                //invocamos el método a través del mismo contexto del formulario (this) y enviamos los parámetros 
                this.Invoke(delegado, parametros);
            }
            else
            {
                //en caso contrario, se realiza el llamado a los controles 
                lblArchivoValor.Text = psNombreArchivo;
            }
        }

        private DataTable fnObtenerDatos(string psRutaDocumento)
        {
            DataTable dtResultado = new DataTable();
            string sFileName = string.Empty;
            string header = "No";
            string sql = string.Empty;
            string sPathOnly = string.Empty;
            try
            {
                sPathOnly = Path.GetDirectoryName(psRutaDocumento);
                sFileName = Path.GetFileName(psRutaDocumento);

                sql = @"SELECT * FROM [" + sFileName + "]";

                using (OleDbConnection connection = new OleDbConnection(
                        @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sPathOnly +
                        ";Extended Properties=\"Text;HDR=" + header + "\""))
                {
                    using (OleDbCommand command = new OleDbCommand(sql, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            dtResultado = new DataTable();
                            dtResultado.Locale = CultureInfo.CurrentCulture;
                            adapter.Fill(dtResultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al momento de cargar la información: " +  psRutaDocumento + " " + ex.Message);
            }
            return dtResultado;
        }

        /// <summary>
        /// Método que se encarga de descomprimir el archivo
        /// </summary>
        /// <param name="psRuta">Ruta de generación</param>
        /// <param name="psRutaArchivo">Ruta del archivo</param>
        /// <param name="psNombreArchivo">Nombre del archivo</param>
        private void fnDescomprimirArchivo(string psRuta, string psRutaArchivo, string psNombreArchivo)
        {
            
            try
            {
                fnCambiarNombreArchivo(psNombreArchivo);

                using (FileStream fInStream = new FileStream(psRutaArchivo, FileMode.Open, FileAccess.Read))
                {
                    using (GZipStream zipStream = new GZipStream(fInStream, CompressionMode.Decompress))
                    {
                        using (FileStream fOutStream = new FileStream(psRuta + psNombreArchivo + ".xml", FileMode.Create, FileAccess.Write))
                        {
                            byte[] tempBytes = new byte[4096];
                            int i;
                            while ((i = zipStream.Read(tempBytes, 0, tempBytes.Length)) != 0)
                            {
                                fOutStream.Write(tempBytes, 0, i);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No fue posible descomprimir el zip: " + psNombreArchivo + ". Error: " + ex.Message);

                if (File.Exists(psRuta + psNombreArchivo + ".xml"))
                    File.Delete(psRuta + psNombreArchivo + ".xml");
            }
            finally
            {
                GC.Collect();
                GC.WaitForFullGCComplete();
            }
        }

        /// <summary>
        /// Método que se encarga de decodificar el archivo
        /// </summary>
        /// <param name="psRuta">Ruta de generación</param>
        /// <param name="psNombreArchivo">Nombre del archivo</param>
        private void fnDecodificarArchivo(string psRuta, string psNombreArchivo)
        {
            FileStream inFile = null;
            FileStream outFile = null;
            CMS cms = null;
            try
            {
                inFile = File.Open(psRuta + psNombreArchivo + ".xml", FileMode.Open);
                outFile = File.Create(psRuta + psNombreArchivo + "Desencriptado.txt");

                cms = new CMS();
                cms.Decode(inFile, outFile);
            }
            catch (Exception ex)
            {
                if (inFile != null)
                    inFile.Close();

                if (outFile != null)
                    outFile.Close();

                if (File.Exists(psRuta + psNombreArchivo + "Desencriptado.txt"))
                    File.Delete(psRuta + psNombreArchivo + "Desencriptado.txt");

                MessageBox.Show("No fue posible decodificar el zip: " + psNombreArchivo + ". Error: " + ex.Message);
            }
            finally
            {
                if (File.Exists(psRuta + psNombreArchivo + ".xml"))
                    File.Delete(psRuta + psNombreArchivo + ".xml");

                if (inFile != null)
                    inFile.Close();

                if (outFile != null)
                    outFile.Close();

                ((IDisposable)cms).Dispose();

                GC.Collect();
                GC.WaitForFullGCComplete();
            }
        }

        /// <summary>
        /// Generación de la LCO
        /// </summary>
        /// <param name="psRuta">Ruta configurada</param>
        /// <param name="psNombreArchivo">Nombre del archivo</param>
        /// <returns></returns>
        public DataTable fnGeneracionLCODataTable(string psRuta, string  psNombreArchivo)
        {
            BinaryReader zr = null;
            bool cortar = true;
            DataTable dtLCO = new DataTable();
            long largo;
            int index = 0;
            int parte = 1000000;            
            string aux = string.Empty;
            Stream archivo = null;
            StringBuilder sb = new StringBuilder();
            XmlDocument doc = new XmlDocument();
            try
            {
                dtLCO.Columns.Add("RFC", typeof(string));
                dtLCO.Columns.Add("ValidezObligaciones", typeof(byte));
                dtLCO.Columns.Add("EstatusCertificado", typeof(char));
                dtLCO.Columns.Add("noCertificado", typeof(string));
                dtLCO.Columns.Add("FechaFinal", typeof(DateTime));
                dtLCO.Columns.Add("FechaInicio", typeof(DateTime));

                archivo = File.Open(psRuta + psNombreArchivo + "Desencriptado.txt", FileMode.Open);
                zr = new BinaryReader(archivo);
                sb = new StringBuilder();

                largo = archivo.Length;

                // Creación de un datatable como origen de datoss
                while (largo > 0)
                {
                    string linea = aux + System.Text.Encoding.UTF8.GetString(zr.ReadBytes(parte));

                    if (cortar)
                    {
                        linea = linea.Substring(linea.IndexOf("<lco:Con"));
                        cortar = false;
                    }

                    if (largo < parte)
                    {
                        linea = linea.Substring(0, linea.IndexOf("LCO>") - 6);
                    }

                    index = linea.LastIndexOf("</lco:Contribuyente>");

                    try
                    {
                        aux = linea.Substring(index + 20);
                        linea = linea.Remove(index + 20);
                    }
                    catch { }

                    linea = linea.Replace("lco:", string.Empty);

                    doc.LoadXml("<root></root>");
                    doc.DocumentElement.InnerXml = linea;

                    XPathNodeIterator nodos = doc.CreateNavigator().Select("/root/Contribuyente");

                    //sb = new StringBuilder();

                    while (nodos.MoveNext())
                    {
                        string rfc = nodos.Current.SelectSingleNode("@RFC").Value;

                        XPathNodeIterator cert = nodos.Current.Select("Certificado");

                        while (cert.MoveNext())
                        {
                            DataRow drRenglon = dtLCO.NewRow();
                            drRenglon["RFC"] = rfc;
                            drRenglon["ValidezObligaciones"] = Convert.ToByte(cert.Current.SelectSingleNode("@ValidezObligaciones").Value);
                            drRenglon["EstatusCertificado"] = cert.Current.SelectSingleNode("@EstatusCertificado").Value;
                            drRenglon["noCertificado"] = cert.Current.SelectSingleNode("@noCertificado").Value;
                            drRenglon["FechaFinal"] = cert.Current.SelectSingleNode("@FechaFinal").Value;
                            drRenglon["FechaInicio"] = cert.Current.SelectSingleNode("@FechaInicio").Value;
                            dtLCO.Rows.Add(drRenglon);
                        }
                    }

                    largo -= parte;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al generar los archivos: " + psNombreArchivo + " " + ex.Message);
            }
            finally
            {
                if (archivo != null)
                    archivo.Close();

                if (zr != null)
                    zr.Close();

                if (doc != null)
                    doc = null;

                if (sb != null)
                    sb.Clear();

                if (File.Exists(psRuta + psNombreArchivo + ".xml"))
                    File.Delete(psRuta + psNombreArchivo + ".xml");

                if (File.Exists(psRuta + psNombreArchivo + "Desencriptado.txt"))
                    File.Delete(psRuta + psNombreArchivo + "Desencriptado.txt");

                GC.Collect();
                GC.WaitForFullGCComplete();
            }
            return dtLCO;
        }

        /// <summary>
        /// Método que se encarga de generar la LCO en base a un XML y cargarlo en un datatable
        /// </summary>
        /// <param name="psRuta"></param>
        /// <param name="psNombreArchivo"></param>
        /// <returns></returns>
        private DataTable fnGeneracionLCOXml(string psRuta, string psNombreArchivo)
        {
            BinaryReader zr = null;
            bool cortar = true;
            DataSet dsLCO = new DataSet();
            DataTable dtLCO = new DataTable();
            int parte = 1000000;
            int index = 0;
            long largo;            
            string aux = string.Empty;
            StringBuilder sb = new StringBuilder();
            Stream archivo = null;
            StreamWriter sw = null;
            XmlDocument xdLCO = new XmlDocument();
            try
            {
                string archiveIndex = psRuta + psNombreArchivo + "_Desencriptado.xml";
                archiveIndex = archiveIndex.Replace(".XML.gz", "");

                Stream res = File.Create(archiveIndex);
                sw = new StreamWriter(res, Encoding.GetEncoding(1252));

                largo = archivo.Length;

                sw.Write("<LCO>");

                while (largo > 0)
                {
                    string linea = aux + System.Text.Encoding.UTF8.GetString(zr.ReadBytes(parte));

                    if (cortar)
                    {
                        linea = linea.Substring(linea.IndexOf("<lco:Con"));
                        cortar = false;
                    }

                    if (largo < parte)
                    {
                        linea = linea.Substring(0, linea.IndexOf("LCO>") - 6);
                    }

                    index = linea.LastIndexOf("</lco:Contribuyente>");

                    try
                    {
                        aux = linea.Substring(index + 20);
                        linea = linea.Remove(index + 20);
                    }
                    catch { }

                    linea = linea.Replace("lco:", string.Empty);

                    xdLCO.LoadXml("<root></root>");
                    xdLCO.DocumentElement.InnerXml = linea;

                    XPathNodeIterator nodos = xdLCO.CreateNavigator().Select("/root/Contribuyente");

                    while (nodos.MoveNext())
                    {
                        //sb.Append("<Contribuyente ");

                        string rfc = nodos.Current.SelectSingleNode("@RFC").Value;

                        XPathNodeIterator cert = nodos.Current.Select("Certificado");

                        while (cert.MoveNext())
                        {
                            sb.Append("<Contribuyente");
                            sb.Append(" RFC=" + "\"");
                            sb.Append(rfc);
                            sb.Append("\"");
                            sb.Append(" ValidezObligaciones=" + "\"");
                            sb.Append(cert.Current.SelectSingleNode("@ValidezObligaciones").Value);
                            sb.Append("\"");
                            sb.Append(" EstatusCertificado=" + "\"");
                            sb.Append(cert.Current.SelectSingleNode("@EstatusCertificado").Value);
                            sb.Append("\"");
                            sb.Append(" noCertificado=" + "\"");
                            sb.Append(cert.Current.SelectSingleNode("@noCertificado").Value);
                            sb.Append("\"");
                            sb.Append(" FechaFinal=" + "\"");
                            sb.Append(cert.Current.SelectSingleNode("@FechaFinal").Value);
                            sb.Append("\"");
                            sb.Append(" FechaInicio=" + "\"");
                            sb.Append(cert.Current.SelectSingleNode("@FechaInicio").Value);
                            sb.Append("\"");
                            sb.Append(" />");

                            sw.Write(sb.ToString());
                        }
                    }

                    largo -= parte;
                }

                sw.Write("</LCO>");
                sw.Close();
                sw.Dispose();

                dsLCO.ReadXml(psRuta + psNombreArchivo + "_Desencriptado.xml");
                dtLCO = dsLCO.Tables[0];

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al generar los archivos: " + psNombreArchivo + " " + ex.Message);
            }
            finally
            {
                if (archivo != null)
                    archivo.Close();

                if (zr != null)
                    zr.Close();

                if (xdLCO != null)
                    xdLCO = null;

                if (sw != null)
                    sw.Close();

                if (sb != null)
                    sb.Clear();

                if (File.Exists(psRuta + psNombreArchivo + ".xml"))
                    File.Delete(psRuta + psNombreArchivo + ".xml");

                if (File.Exists(psRuta + psNombreArchivo + "Desencriptado.txt"))
                    File.Delete(psRuta + psNombreArchivo + "Desencriptado.txt");

                //if (File.Exists(psRuta + psNombreArchivo + "_Desencriptado.xml"))
                //    File.Delete(psRuta + psNombreArchivo + "_Desencriptado.xml");

                GC.Collect();
                GC.WaitForFullGCComplete();
            }
            return dtLCO;
        }

        /// <summary>
        /// Método que registrar la LCO ya formada en base a un data table
        /// </summary>
        public void fnRegistrarLCO(DataTable pdtLCO)
        {
            using (SqlConnection conn = new SqlConnection(Settings.Default.conexion))
            {
                SqlBulkCopy bulkCopy = null;
                //DataTableReader dtrLCO = dtLCO.CreateDataReader();
                try
                {
                    using (bulkCopy = new SqlBulkCopy(conn.ConnectionString))
                    {
                        bulkCopy.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);

                        bulkCopy.BulkCopyTimeout = 480;
                        bulkCopy.BatchSize = 1000;
                        bulkCopy.NotifyAfter = 200;
                        bulkCopy.ColumnMappings.Add("RFC", "rfc");
                        bulkCopy.ColumnMappings.Add("ValidezObligaciones", "validez_obligaciones");
                        bulkCopy.ColumnMappings.Add("EstatusCertificado", "estatus_certificado");
                        bulkCopy.ColumnMappings.Add("noCertificado", "no_certificado");
                        bulkCopy.ColumnMappings.Add("FechaFinal", "fecha_final");
                        bulkCopy.ColumnMappings.Add("FechaInicio", "fecha_inicio");
                        bulkCopy.DestinationTableName = "tbl_ctp_LCO_apl_tmp";
                        bulkCopy.WriteToServer(pdtLCO);
                        bulkCopy.Close();
                    }
                }
                catch (Exception ex)
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                    throw new Exception("Error al realizar la instrucción Bulk de la LCO en BD: " + ex.Message);
                }
                finally
                {
                    if (bulkCopy != null)
                    {
                        ((IDisposable)bulkCopy).Dispose();

                        //using (bulkCopy as IDisposable)
                        //{

                        //}
                    }

                    pdtLCO.Dispose();
                    pdtLCO.Rows.Clear();
                    pdtLCO.Clear();
                    pdtLCO = null;

                    GC.Collect();
                    GC.WaitForFullGCComplete();
                }
            }
        }
    }
}

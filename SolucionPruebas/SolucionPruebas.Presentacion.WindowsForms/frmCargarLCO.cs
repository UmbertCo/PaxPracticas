using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmCargarLCO : Form
    {
        public frmCargarLCO()
        {
            InitializeComponent();
        }

        private void frmCargarLCO_Load(object sender, EventArgs e)
        {
            
        }

        private void btnCargarLCO_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdArchivo = new OpenFileDialog();
            ofdArchivo.Multiselect = true;
            ofdArchivo.ShowDialog();

            if (string.IsNullOrEmpty(ofdArchivo.FileName))
                return;

            lbArchivos.DataSource = ofdArchivo.FileNames.ToArray();
            lbArchivos.SelectedIndex = -1;
        }

        private void lbArchivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbArchivos.SelectedValue == null)
                    return;

                if (string.IsNullOrEmpty(lbArchivos.SelectedValue.ToString()))
                    return;

                if (Path.GetExtension(lbArchivos.SelectedValue.ToString()).ToLower().Equals(".xml"))
                    fnCargarLCO(lbArchivos.SelectedValue.ToString());
                else
                    fnCrearLCO(lbArchivos.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al momento de cargar el archivo de la LCO: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            DataTable dtLCO = new DataTable();
            DataTable dtResultado = new DataTable();
            try
            {
                dtLCO = (DataTable)dgvLCO.DataSource;

                DataRow[] drResultado = dtLCO.Select("RFC='" + txtRFC.Text + "'");

                if (drResultado.Count() <= 0)
                    return;

                dtResultado.Columns.Add("RFC", typeof(string));
                dtResultado.Columns.Add("ValidezObligaciones", typeof(byte));
                dtResultado.Columns.Add("EstatusCertificado", typeof(char));
                dtResultado.Columns.Add("noCertificado", typeof(string));
                dtResultado.Columns.Add("FechaFinal", typeof(DateTime));
                dtResultado.Columns.Add("FechaInicio", typeof(DateTime));

                for (int i = 0; i < drResultado.Count(); i++)
                {
                    DataRow drRenglon = dtResultado.NewRow();
                    drRenglon["RFC"] = drResultado[i]["RFC"];
                    drRenglon["ValidezObligaciones"] = drResultado[i]["ValidezObligaciones"];
                    drRenglon["EstatusCertificado"] = drResultado[i]["EstatusCertificado"];
                    drRenglon["noCertificado"] = drResultado[i]["noCertificado"];
                    drRenglon["FechaFinal"] = drResultado[i]["FechaFinal"];
                    drRenglon["FechaInicio"] = drResultado[i]["FechaInicio"];

                    dtResultado.Rows.Add(drRenglon);
                }
                dgvResultado.DataSource = dtResultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al momento de filtrar la LCO: " + ex.Message);
            }
        }

        private void fnCrearLCO(string psRuta)
        {
            string sRutaArchivo = string.Empty;
            try
            {
                sRutaArchivo = psRuta;
                string sNombreArchivo = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(sRutaArchivo));
                string sRuta = Path.GetDirectoryName(sRutaArchivo) + @"\";

                fnDescomprimirArchivo(sRuta, sRutaArchivo, sNombreArchivo);

                fnDecodificarArchivo(sRuta, sNombreArchivo);

                DataTable dtLCO = new DataTable();
                dtLCO = fnGeneracionLCODataTable(sRuta, sNombreArchivo);

                dgvLCO.DataSource = dtLCO;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible generar la LCO: " + ex.Message);
            }
        }

        private void fnCargarLCO(string psRuta)
        {
            DataSet dsDocumento = new DataSet();
            try
            {
                dsDocumento.ReadXml(psRuta);
                dgvLCO.DataSource = dsDocumento.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        /// Generación de la LCO
        /// </summary>
        /// <param name="psRuta">Ruta configurada</param>
        /// <param name="psNombreArchivo">Nombre del archivo</param>
        /// <returns></returns>
        public DataTable fnGeneracionLCODataTable(string psRuta, string psNombreArchivo)
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

                archivo = File.Open(psRuta + psNombreArchivo + "Desencriptado.xml", FileMode.Open);
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

                //if (File.Exists(psRuta + psNombreArchivo + "Desencriptado.txt"))
                //    File.Delete(psRuta + psNombreArchivo + "Desencriptado.txt");

                GC.Collect();
                GC.WaitForFullGCComplete();
            }
            return dtLCO;
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
                outFile = File.Create(psRuta + psNombreArchivo + "Desencriptado.xml");

                cms = new CMS();
                cms.Decode(inFile, outFile);
            }
            catch (Exception ex)
            {
                if (inFile != null)
                    inFile.Close();

                if (outFile != null)
                    outFile.Close();

                //if (File.Exists(psRuta + psNombreArchivo + "Desencriptado.txt"))
                //    File.Delete(psRuta + psNombreArchivo + "Desencriptado.txt");

                MessageBox.Show("No fue posible decodificar el zip: " + psNombreArchivo + ". Error: " + ex.Message);
            }
            finally
            {
                //if (File.Exists(psRuta + psNombreArchivo + ".xml"))
                //    File.Delete(psRuta + psNombreArchivo + ".xml");

                if (inFile != null)
                    inFile.Close();

                if (outFile != null)
                    outFile.Close();

                ((IDisposable)cms).Dispose();

                GC.Collect();
                GC.WaitForFullGCComplete();
            }
        }
    }
}

using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms1
{
    public partial class Form1 : Form
    {
        DataSet dsTimbrado;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                fbdCarpeta.ShowDialog();

                if (string.IsNullOrEmpty(fbdCarpeta.SelectedPath))
                    return;

                txtDirectorio.Text = fbdCarpeta.SelectedPath;

                dtResultado.Columns.Add("carpeta", typeof(string));
                dtResultado.Columns.Add("numero", typeof(int));

                List<string> lsCarpetas = Directory.EnumerateFiles(fbdCarpeta.SelectedPath, "*.zip").ToList<string>();

                foreach (string sCarpeta in lsCarpetas)
                {
                    DataRow drRenglon = dtResultado.NewRow();
                    int nNumeroArchivo = 0;

                    using (FileStream fsSource = new FileStream(sCarpeta, FileMode.Open))
                    {
                        Stream strZip = fsSource;
                        ZipFile archivoZip = new ZipFile(strZip);
                        
                        foreach (ZipEntry zipEntry in archivoZip)
                        {
                            nNumeroArchivo += 1;
                        }
                    }

                    drRenglon["carpeta"] = Path.GetFileName(sCarpeta);
                    drRenglon["numero"] = nNumeroArchivo;
                    dtResultado.Rows.Add(drRenglon);
                }

                dgvArchivos.DataSource = dtResultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            dsTimbrado = new DataSet();
            using (SqlConnection scConexion = new SqlConnection())
            {
                scConexion.ConnectionString = "Data Source=DB-DESAROLLO;Initial Catalog=PaxNadrosa;Persist Security Info=True;User ID=Desarrollo;Password=F4cturax10n";
                scConexion.Open();
                try
                {
                    using (SqlCommand scoComando = new SqlCommand())
                    {
                        scoComando.Connection = scConexion;
                        scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                        scoComando.CommandText = "usp_reporte_diario";

                        scoComando.Parameters.AddWithValue("dia", 18);
                        scoComando.Parameters.AddWithValue("mes", 11);
                        scoComando.Parameters.AddWithValue("anio", 2014);
                        scoComando.Parameters.AddWithValue("dia2", 19);
                        scoComando.Parameters.AddWithValue("mes2", 11);
                        scoComando.Parameters.AddWithValue("anio2", 2014);

                        using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                        {
                            sdaAdaptador.Fill(dsTimbrado);
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }

            DataTable dtTimbrado = new DataTable();
            dtTimbrado = dsTimbrado.Tables[0];

            fnCalcularImpuestos(ref dtTimbrado);

        }

        private void fnCalcularImpuestos(ref DataTable dtComprobantes)
        {
            decimal dImporteImpuestoIvaCero = 0;
            decimal dImporteImpuestoIva = 0;
            decimal dImporteImpuestoIeps = 0;
            string sTasa = string.Empty;
            try
            {
                dtComprobantes.Columns.Add("IvaCero", typeof(decimal));
                dtComprobantes.Columns.Add("Iva", typeof(decimal));
                dtComprobantes.Columns.Add("Ieps", typeof(decimal));

                foreach (DataRow Renglon in dtComprobantes.Rows)
                {
                    XmlDocument dxComprobante = new XmlDocument();
                    dxComprobante.LoadXml(Renglon["xml"].ToString());

                    string sImpuesto = string.Empty;

                    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(dxComprobante.NameTable);
                    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                    nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                    XPathNavigator navEncabezado = dxComprobante.CreateNavigator();
                    XPathNodeIterator xpnIterador = navEncabezado.Select("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados", nsmComprobante);

                    //XPathNavigator navEncabezado = dxComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados[1]", );
                    decimal dTasa;
                    dImporteImpuestoIvaCero = 0;
                    dImporteImpuestoIva = 0;
                    dImporteImpuestoIeps = 0;

                    

                    foreach (XPathNavigator xpnTraslados in xpnIterador)
                    {
                        dTasa = 0;
                        XPathNodeIterator xpnIteradorImpuesto = xpnTraslados.Select("cfdi:Traslado", nsmComprobante);
                        foreach (XPathNavigator xpnTrasladosImpuesto in xpnIteradorImpuesto)
                        {
                            try
                            {
                                sImpuesto = xpnTrasladosImpuesto.SelectSingleNode("@impuesto", nsmComprobante).Value.ToString();
                                sTasa = xpnTrasladosImpuesto.SelectSingleNode("@tasa", nsmComprobante).Value.ToString();
                            }
                            catch { }

                            try
                            {
                                dTasa = Convert.ToDecimal(sTasa);
                            }
                            catch { }


                            if (sImpuesto.Equals("IVA") && dTasa.Equals(0))
                            {
                                try { dImporteImpuestoIvaCero += Convert.ToDecimal(xpnTrasladosImpuesto.SelectSingleNode("@importe", nsmComprobante).Value.ToString()); }
                                catch { }
                            }

                            if (sImpuesto.Equals("IVA") && (dTasa.Equals(16.00) || dTasa.Equals(16) || dTasa.Equals(16.0)))
                            {
                                try { dImporteImpuestoIva += Convert.ToDecimal(xpnTrasladosImpuesto.SelectSingleNode("@importe", nsmComprobante).Value.ToString()); }
                                catch { }
                            }

                            if (sImpuesto.Equals("IEPS"))
                            {
                                try { dImporteImpuestoIeps += Convert.ToDecimal(xpnTrasladosImpuesto.SelectSingleNode("@importe", nsmComprobante).Value.ToString()); }
                                catch { }
                            }
                        }
                    }

                    Renglon["IvaCero"] = dImporteImpuestoIvaCero;
                    Renglon["Iva"] = dImporteImpuestoIva;
                    Renglon["Ieps"] = dImporteImpuestoIeps;
                }
            }
            catch (Exception ex)
            {
                //clsLog.EscribirLog("Error al momento generar los importes de los impuestos -" + ex.Message);
            }
        }
    }
}

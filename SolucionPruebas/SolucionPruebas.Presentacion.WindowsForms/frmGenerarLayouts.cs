using SolucionPruebas.Presentacion.WindowsForms.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmGenerarLayouts : Form
    {
        public frmGenerarLayouts()
        {
            InitializeComponent();
        }
        private void btnArchivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog pfdLayout = new OpenFileDialog();

            txtLayout.Text = string.Empty;

            //pfdLayout.Filter = "|.txt";
            pfdLayout.ShowDialog();

            if (string.IsNullOrEmpty(pfdLayout.FileName))
                return;

            txtLayout.Text = pfdLayout.FileName;
        }
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            string sLayout = string.Empty;
            string sFecha = string.Empty;
            int nNumeroRegistros = 0;
            List<string> ListaLayouts = new List<string>();
            OpenFileDialog pfdLayout = new OpenFileDialog();
            try
            {
                if(string.IsNullOrEmpty(txtNumeroLayouts.Text))
                {
                    MessageBox.Show("Seleccionar el número de registros a generar");
                    return;
                }

                try
                {
                    nNumeroRegistros = Convert.ToInt32(txtNumeroLayouts.Text);
                }
                catch
                {
                    MessageBox.Show("El número de registros a generar no es entero");
                    return;
                }

                Stream archivo = File.Open(txtLayout.Text, FileMode.Open);
                StreamReader sr = new StreamReader(archivo);
                sLayout = sr.ReadToEnd();
                archivo.Close();

                GenerarLayouts cGenerarLayouts = new GenerarLayouts(sLayout, nNumeroRegistros);
                Thread hilo = new Thread(new ThreadStart(cGenerarLayouts.fnGenerarLayout));
                hilo.Start();
                hilo.Join();

                //ListaLayouts.Add(sLayout);
                //int div = ListaLayouts.Count() / 5;
                //if (div == 0) div = 1;

                //IEnumerable<List<string>> list1 = ExtensionMethod.Partition<string>(ListaLayouts, div);
                //var threads = new Thread[list1.Count()];
                //int i = 0;
                //foreach (List<string> list2 in list1)
                //{
                //    Worker workerObject = new Worker();
                //    threads[i] = new Thread(() => workerObject.DoWork(list2, nNumeroRegistros));
                //    threads[i].Start();
                //    Thread.Sleep(5);
                //    i++;
                //}

                //foreach (Thread thread in threads)
                //{
                //    thread.Join();
                //}
            }
            catch (Exception ex)
            {
                AccesoDisco.GuardarArchivoTexto(string.Format("{0}\\{1}.txt", Settings.Default.rutaErrores, Guid.NewGuid().ToString("N")), ex.ToString());
            }
        }
        private void btnGenerarRuta_Click(object sender, EventArgs e)
        {
            string sLayout = string.Empty;
            string[] aLayout = null;
            string sFecha = string.Empty;
            string sRfc = string.Empty;
            string[] Files = null;
            try
            {
                Files = Directory.GetFiles(txtRuta.Text);

                foreach (string archivo in Files)
                {
                    Stream sArchivo = File.Open(archivo, FileMode.Open);
                    StreamReader sr = new StreamReader(sArchivo);
                    sLayout = sr.ReadToEnd();
                    sArchivo.Close();

                    for (int i = 0; i < 1; i++)
                    {
                        // Generamos nueva fecha
                        int nIndiceFecha = sLayout.IndexOf("fecha@");
                        int nIndicePipeFecha = sLayout.IndexOf("|", nIndiceFecha);
                        sFecha = sLayout.Substring(nIndiceFecha, nIndicePipeFecha - nIndiceFecha);

                        aLayout = sFecha.Split('@');

                        System.Threading.Thread.Sleep(1000);
                        sLayout = sLayout.Replace(aLayout[1], DateTime.Now.ToString("s"));

                        // Agregamos el rfc del Emisor de Pruebas
                        int nIndiceRfcEmisor = sLayout.IndexOf("re?rfc@");
                        int nIndicePipeRfcEmisor = sLayout.IndexOf("|", nIndiceRfcEmisor);
                        sRfc = sLayout.Substring(nIndiceRfcEmisor, nIndicePipeRfcEmisor - nIndiceRfcEmisor);

                        aLayout = sRfc.Split('@');

                        System.Threading.Thread.Sleep(1000);
                        sLayout = sLayout.Replace(aLayout[1], "AAA010101AAA");

                        AccesoDisco.GuardarArchivoTexto(string.Format("{0}\\{1}.txt", Settings.Default.rutaArchivos, Path.GetFileNameWithoutExtension(archivo)), sLayout);
                        //AccesoDisco.GuardarArchivoTexto(string.Format("{0}\\{1}.txt", Settings.Default.rutaLog, Guid.NewGuid().ToString("N")), sLayout);
                    }
                }
            }
            catch (Exception ex)
            {
                AccesoDisco.GuardarArchivoTexto(string.Format("{0}\\{1}.txt", Settings.Default.rutaErrores, Guid.NewGuid().ToString("N")), ex.ToString());
            }
        }
        private void btnRutaArchivos_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog pfdLayout = new FolderBrowserDialog();

            txtRuta.Text = string.Empty;

            pfdLayout.ShowDialog();

            if (string.IsNullOrEmpty(pfdLayout.SelectedPath))
                return;

            txtRuta.Text = pfdLayout.SelectedPath;
        }
    }

    public class GenerarLayouts
    {
        string gsLayout;
        int gnNumeroRegistros;
        string[] aLayout = null;

        public GenerarLayouts(string psLayout, int pnNumeroRegistros)
        {
            gsLayout = psLayout;
            gnNumeroRegistros = pnNumeroRegistros;
        }

        public void fnGenerarLayout()
        {
            string sFecha = string.Empty;
            for (int i = 0; i < gnNumeroRegistros; i++)
		    {
                int nIndiceFecha = gsLayout.IndexOf("fecha@");
                int nIndicePipeFecha = gsLayout.IndexOf("|", nIndiceFecha);
                sFecha = gsLayout.Substring(nIndiceFecha, nIndicePipeFecha - nIndiceFecha);

                aLayout = sFecha.Split('@');
                gsLayout = gsLayout.Replace(aLayout[1], DateTime.Now.AddDays(-1).AddSeconds(i).ToString("s"));

                AccesoDisco.GuardarArchivoTexto(string.Format("{0}\\{1}.txt", Settings.Default.rutaArchivos, i.ToString() + "-" + Guid.NewGuid().ToString("N")), gsLayout);
		    }  
        }
    }

    public static class ExtensionMethod
    {
        public static IEnumerable<List<string>> Partition<cfdi>(this IList<string> source, Int32 size)
        {
            for (int i = 0; i < Math.Ceiling(source.Count / (Double)size); i++)
                yield return new List<string>(source.Skip(size * i).Take(size));
        }
    }

    public class Worker
    {
        private volatile bool _shouldStop;

        public void DoWork(List<string> list, int gnNumeroRegistros)
        {
            while (!_shouldStop)
            {
                foreach (string gsLayout in list)
                {
                    string[] aLayout = null;
                    string sFecha = string.Empty;
                    try
                    {
                        string sLayout = string.Empty;
                        sLayout = gsLayout;
                        for (int i = 0; i < gnNumeroRegistros; i++)
                        {
                            int nIndiceFecha = sLayout.IndexOf("fecha@");
                            int nIndicePipeFecha = sLayout.IndexOf("|", nIndiceFecha);
                            sFecha = sLayout.Substring(nIndiceFecha, nIndicePipeFecha - nIndiceFecha);

                            aLayout = sFecha.Split('@');
                            sLayout = sLayout.Replace(aLayout[1], DateTime.Now.AddDays(-2).AddSeconds(i).ToString("s"));

                            AccesoDisco.GuardarArchivoTexto(string.Format("{0}\\{1}.txt", Settings.Default.rutaArchivos, Guid.NewGuid().ToString("N")), sLayout);
                        } 
                    }
                    catch (Exception ex)
                    {
                                  
                    }
                } 
                _shouldStop = true;
            }
        } 
    }
}

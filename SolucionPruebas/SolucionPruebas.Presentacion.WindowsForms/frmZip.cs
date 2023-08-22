using Chilkat;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmZip : Form
    {
        public frmZip()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            { 
                fbdArchivo.ShowDialog();
                if (fbdArchivo.SelectedPath.Equals(string.Empty))
                    return;

                ZipOutputStream zosArchivoError;
                zosArchivoError = new ZipOutputStream(File.Create(fbdArchivo.SelectedPath + @"\" + "Archivo.zip"));
                zosArchivoError.SetLevel(6);

                for (int i = 0; i < 2; i++)
                {
                    foreach (var item in lbArchivos.Items)
                    {
                        fnAñadirFicheroaZip(zosArchivoError, "/", item.ToString());
                    }
                }

                zosArchivoError.Finish();
                zosArchivoError.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el archivo zip: " + ex.Message);
            }
        }
        private void btnGenerarZipChilkat_Click(object sender, EventArgs e)
        {
            Chilkat.Zip archivoZip = new Zip();
            bool bResultado = false;
            bool bRecursivo = true;
            Chilkat.StringArray saArchivos = new StringArray();
            try
            {
                fbdArchivo.ShowDialog();
                if (fbdArchivo.SelectedPath.Equals(string.Empty))
                    return;

                bResultado = archivoZip.UnlockComponent("INTERMRSA_78UJEvED0IwK");

                //----------------- Se genera el Zip en base a un directorio

                archivoZip.NewZip(fbdArchivo.SelectedPath + @"\" + "ArchivoChilkatUnDirectorio.zip");
                archivoZip.AppendFromDir = @"C:\Users\Desarrollo1\Documents\Visual Studio 2010\Projects\GeneradorPAXVB\GeneradorPAXVB\bin\Debug\ArchivosGenerados";
                archivoZip.AppendFiles("*.*", bRecursivo);

                bResultado = archivoZip.WriteZipAndClose();
                //-----------------

                

                //----------------- Se genera el Zip en base a una lista de archivos

                archivoZip.NewZip(fbdArchivo.SelectedPath + @"\" + "ArchivoChilkatListaArchivos.zip");
                for (int i = 0; i < 2; i++)
                {
                    foreach (var item in lbArchivos.Items)
                    {
                        saArchivos.Append(item.ToString());
                        //fnAñadirFicheroaZip(zosArchivoError, "/", item.ToString());

                    }
                }

                archivoZip.AppendMultiple(saArchivos, bRecursivo);

                bResultado = archivoZip.WriteZipAndClose();

                //----------------- Se genera el Zip en base a dos directorios       

                archivoZip.NewZip(fbdArchivo.SelectedPath + @"\" + "ArchivoChilkatDosDirectorio.zip");
                archivoZip.AppendFromDir = @"C:\Users\Desarrollo1\Documents\Visual Studio 2010\Projects\GeneradorPAXVB\GeneradorPAXVB\bin\Debug\ArchivosGenerados\Directorio1";
                archivoZip.AppendFiles("*.xml", bRecursivo);
                archivoZip.AppendFromDir = @"C:\Users\Desarrollo1\Documents\Visual Studio 2010\Projects\GeneradorPAXVB\GeneradorPAXVB\bin\Debug\ArchivosGenerados\Directorio2";
                archivoZip.AppendFiles("*.xml", bRecursivo);

                bResultado = archivoZip.WriteZipAndClose();

                //-----------------
             
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el archivo zip: " + ex.Message);
            }
        }
        private void btnChilkatZip_Click(object sender, EventArgs e)
        {
            Chilkat.Zip archivoZip = new Zip();
            bool bResultado = false;
            bool bRecursivo = true;
            Chilkat.StringArray saArchivos = new StringArray();
            try
            {
                bResultado = archivoZip.UnlockComponent("Anything for 30-day trial");

                //----------------- Se genera el Zip en base a un directorio

                fbdArchivo.ShowDialog();
                if (fbdArchivo.SelectedPath.Equals(string.Empty))
                    return;

                archivoZip.NewZip(fbdArchivo.SelectedPath + @"\" + "ArchivoChilkat.zip");

                fbdArchivo.ShowDialog();
                if (fbdArchivo.SelectedPath.Equals(string.Empty))
                    return;

                archivoZip.AppendFromDir = fbdArchivo.SelectedPath;

                archivoZip.AppendFiles("*.*", bRecursivo);

                bResultado = archivoZip.WriteZipAndClose();
                //-----------------
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el archivo zip: " + ex.Message);
            }
        }
        private void btnArchivo1_Click(object sender, EventArgs e)
        {
            ofdArchivo.Multiselect = true;
            ofdArchivo.ShowDialog();

            if (ofdArchivo.FileName.Equals(string.Empty))
                return;

            foreach (var archivo in ofdArchivo.FileNames)
            {
                lbArchivos.Items.Add(archivo.ToString());
            }
        }
        private void btnAchivo2_Click(object sender, EventArgs e)
        {
            ofdArchivo.ShowDialog();
            if (ofdArchivo.FileName.Equals(string.Empty))
                return;

            txtArchivo2.Text = ofdArchivo.FileName;
        }

        /// <summary>
        /// Función que agrega un archivo individual a un fichero ZIP que esta en memoria
        /// </summary>
        /// <param name="zStream">Stream</param>
        /// <param name="psRelativePath">Ruta relativa</param>
        /// <param name="psFile">Nombre del archivo</param>
        private static void fnAñadirFicheroaZip(ICSharpCode.SharpZipLib.Zip.ZipOutputStream zStream, string psRelativePath, string psFile)
        {
            byte[] buffer = new byte[4096];
            string fileRelativePath = (psRelativePath.Length > 1 ? psRelativePath : string.Empty)
                                      + System.IO.Path.GetFileName(psFile);
            ICSharpCode.SharpZipLib.Zip.ZipEntry entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(fileRelativePath);
            zStream.PutNextEntry(entry);

            using (FileStream fs = File.OpenRead(psFile))
            {
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }        
    }
}

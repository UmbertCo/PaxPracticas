using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System.IO;

namespace GeneradorMasivoComprobantes
{
    public class Comprimidor
    {
        public void Comprimir(string directorio, string filtro, string zipFic, bool crearAuto = true)
        {
            // comprimir los ficheros del directorio indicado
            // y guardarlo en el zip
            // en filtro se indicará el filtro a usar para seleccionar los ficheros del directorio
            // si directorio es una cadena vacía, filtro será el fichero a comprimir (sólo ese)
            // si crearAuto = True, zipfile será el directorio en el que se guardará
            // y se generará automáticamente el nombre con la fecha y hora actual
            string[] fileNames = null;
            fileNames = Directory.GetFiles(directorio, filtro);
            ComprimirArchivos(fileNames);

            foreach (string ArchivoXML in fileNames)
                System.IO.File.Delete(ArchivoXML);
        }
        //
        public void ComprimirArchivos(string[] fileNames)
        {
            // comprimir los ficheros del array en el zip indicado
            // si crearAuto = True, zipfile será el directorio en el que se guardará
            // y se generará automáticamente el nombre con la fecha y hora actual
            //Crc32 objCrc32 = new Crc32(); t
            ZipOutputStream strmZipOutputStream = default(ZipOutputStream);

            // si hay que crear el nombre del fichero
            // éste será el path indicado y la fecha actual
            string zipFic = System.Configuration.ConfigurationManager.AppSettings["rutaZips"];

            if (!Directory.Exists(zipFic))
                Directory.CreateDirectory(zipFic);

            zipFic += DateTime.Now.ToString("yyMMddHHmmss") + ".zip";
            
            strmZipOutputStream = new ZipOutputStream(File.Create(zipFic));
            // Compression Level: 0-9
            // 0: no(Compression)
            // 9: maximum compression
            strmZipOutputStream.SetLevel(6);
            //
            string strFile = null;
            foreach (string strFile_loopVariable in fileNames)
            {
                strFile = strFile_loopVariable;
                FileStream strmFile = File.OpenRead(strFile);
                byte[] abyBuffer = new byte[Convert.ToInt32(strmFile.Length - 1) + 1];
                //
                strmFile.Read(abyBuffer, 0, abyBuffer.Length);
                //
                //------------------------------------------------------------------
                // para guardar sin el primer path
                //string sFile = strFile;
                //int i = sFile.IndexOf("\\");
                //if (i > -1)
                //{
                //    sFile = sFile.Substring(i + 1).TrimStart();
                //}
                //------------------------------------------------------------------
                //
                //------------------------------------------------------------------
                // para guardar sólo el nombre del fichero
                // esto sólo se debe hacer si no se procesan directorios
                // que puedan contener nombres repetidos
                string sFile = Path.GetFileName(strFile);
                ZipEntry theEntry = new ZipEntry(sFile);
                //------------------------------------------------------------------
                //
                // se guarda con el path completo
                //ZipEntry theEntry = new ZipEntry(strFile);
                //

                // guardar la fecha y hora de la última modificación
                FileInfo fi = new FileInfo(strFile);
                theEntry.DateTime = fi.LastWriteTime;
                //theEntry.DateTime = DateTime.Now
                //
                theEntry.Size = strmFile.Length;
                strmFile.Close();
                //objCrc32.Reset(); t
                //objCrc32.Update(abyBuffer); t
                //theEntry.Crc = objCrc32.Value; t
                strmZipOutputStream.PutNextEntry(theEntry);
                strmZipOutputStream.Write(abyBuffer, 0, abyBuffer.Length);
            }
            strmZipOutputStream.Finish();
            strmZipOutputStream.Close();
        }
        //
        public void Descomprimir(string directorio, string zipFic = "", bool eliminar = false, bool renombrar = false)
        {
            // descomprimir el contenido de zipFic en el directorio indicado.
            // si zipFic no tiene la extensión .zip, se entenderá que es un directorio y
            // se procesará el primer fichero .zip de ese directorio.
            // si eliminar es True se eliminará ese fichero zip después de descomprimirlo.
            // si renombrar es True se añadirá al final .descomprimido
            if (!zipFic.ToLower().EndsWith(".zip"))
            {
                zipFic = Directory.GetFiles(zipFic, "*.zip")[0];
            }
            // si no se ha indicado el directorio, usar el actual
            if (string.IsNullOrEmpty(directorio))
                directorio = ".";
            //
            ZipInputStream z = new ZipInputStream(File.OpenRead(zipFic));
            ZipEntry theEntry = default(ZipEntry);
            //
            do
            {
                theEntry = z.GetNextEntry();
                if ((theEntry != null))
                {
                    string fileName = directorio + "\\" + Path.GetFileName(theEntry.Name);
                    //
                    // dará error si no existe el path
                    FileStream streamWriter = null;
                    try
                    {
                        streamWriter = File.Create(fileName);
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                        streamWriter = File.Create(fileName);
                    }
                    //
                    int size = 0;
                    byte[] data = new byte[2049];
                    do
                    {
                        size = z.Read(data, 0, data.Length);
                        if ((size > 0))
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break; // TODO: might not be correct. Was : Exit Do
                        }
                    } while (true);
                    streamWriter.Close();
                }
                else
                {
                    break; // TODO: might not be correct. Was : Exit Do
                }
            } while (true);
            z.Close();
            //
            // cuando se hayan extraído los ficheros, renombrarlo
            if (renombrar)
            {
                File.Copy(zipFic, zipFic + ".descomprimido");
            }
            if (eliminar)
            {
                File.Delete(zipFic);
            }
        }
        //
        public ZipEntry[] Contenido(string zipFic)
        {
            // devuelve el contenido del zip indicado
            ZipInputStream strmZipInputStream = new ZipInputStream(File.OpenRead(zipFic));
            ZipEntry objEntry = default(ZipEntry);
            //Dim strOutput As String
            ZipEntry[] files = null;
            int n = -1;
            //Dim strBuilder As System.Text.StringBuilder = New System.Text.StringBuilder(strOutput)
            //
            objEntry = strmZipInputStream.GetNextEntry();
            while ((objEntry == null) == false)
            {
                n = n + 1;
                Array.Resize(ref files, n + 1);
                files[n] = objEntry;
                objEntry = strmZipInputStream.GetNextEntry();
            }
            strmZipInputStream.Close();
            //
            return files;
        }
    }
}

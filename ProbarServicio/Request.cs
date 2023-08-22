using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ProbarServicio
{
    public partial class Request : Form
    {
        public static string sBody;

        public static string sContentLength, sVsDebugger, sContentType, sSOAPA, sHost, sExpect, sEncoding, sConnection;

        public Request()
        {
            InitializeComponent();
            txtBody.Text = sBody;

            string dir = ProbarServicio.Properties.Settings.Default.rutaLogs.ToString();

            

            //while (fnWaitForFile(dir+ "trace.log") == false)
            //{
            //    //Espera a que el archivo sea escrito totalmente en el disco duro.
            //}
            try
            {
                using (Stream s = System.IO.File.Open(dir + "trace.log",
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.ReadWrite))
                {
                    string sArchivo = StreamToString(s);
                    string[] inicio;
                    string[] fin;

                    inicio = sArchivo.Split('{');
                    fin = inicio[1].Split('}');
                    string encabezado = fin[0];
                    s.Close();

                    encabezado = encabezado.Trim();

                    string[] sAtributos = encabezado.Split(':');

                    //recorremos los atributos
                    for (int i = 0; i < sAtributos.Length; i++)
                    {
                        if (sAtributos[i].Equals("Content-Type"))
                            sContentType = sAtributos[i + 1];

                        if (sAtributos[i].Equals("VsDebuggerCausalityData"))
                            sVsDebugger = sAtributos[i + 1];

                        if (sAtributos[i].Equals("SOAPAction"))
                            sSOAPA = sAtributos[i + 1];

                        if (sAtributos[i].Equals("Host"))
                            sHost = sAtributos[i + 1];

                        if (sAtributos[i].Equals("Content-Length"))
                            sContentLength = sAtributos[i + 1];

                        if (sAtributos[i].Equals("Expect"))
                            sExpect = sAtributos[i + 1];

                        if (sAtributos[i].Equals("Accept-Encoding"))
                            sEncoding = sAtributos[i + 1];

                        if (sAtributos[i].Equals("Connection"))
                            sConnection = sAtributos[i + 1];
                    }

                    txtHeader.Text = encabezado;
                }

            }
            catch
            {
            }
        }

        public Request(string Body)
        {
            InitializeComponent();
            sBody = Body;
        }

        /// <summary>
        /// Función que espera a que termine la escritura de un archivo en disco
        /// </summary>
        /// <param name="fullPath">Ruta del archivo</param>
        /// <returns></returns>
        bool fnWaitForFile(string psRutaArchivo)
        {
            int numTries = 0;
            while (true)
            {
                ++numTries;
                try
                {
                    using (FileStream fs = new FileStream(psRutaArchivo, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 100))
                    {
                        fs.ReadByte(); break;
                    }
                }
                catch (Exception)
                {
                    if (numTries > 10)
                    {
                        return false;
                    } System.Threading.Thread.Sleep(500);
                }
            } return true;
        }


        public static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}

using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;

namespace PAX_Conector_Capacitacion06
{
    public class clsLog
    {
      
        private string Path = string.Empty;

        public clsLog(string Path)
        {
            this.Path = Path;
        }

        public void fnAgregarLog(string sLog)
        {
            CreateDirectory();
            string nombre = string.Empty;
            nombre = GetNameFile();
            string cadena = string.Empty;

            cadena += DateTime.Now + " - " +sLog + Environment.NewLine;

            StreamWriter sw = new StreamWriter(Path + "/" + nombre, true);
            sw.Write(cadena);
            sw.Close();

        }

        private string GetNameFile()
        {
            string nombre = string.Empty;

            nombre = "logEntrada_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + ".txt";
           

            return nombre;
        }

        private void CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);


            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}

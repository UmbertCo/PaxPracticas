using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PAXRecuperacionSOAPBateriaGT
{
    public class clsLog
    {
        /// <summary>
        /// Escribe en un archivo log
        /// </summary>
        /// <param name="sMensaje">El Mensaje a escribir</param>
        public static void EscribirLog(string sMensaje)
        {
            string path = PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["LogError"] + "LogError - " + String.Format("{0:dd-MM-yyyy}", DateTime.Now) + ".txt";

            if (!File.Exists(path))
            {
                StreamWriter sr4 = new StreamWriter(path);
                sr4.WriteLine(DateTime.Now + " - " + sMensaje);
                sr4.Close();
            }
            else
            {
                StreamWriter sw4 = new StreamWriter(path, true);
                sw4.WriteLine(DateTime.Now + " - " + sMensaje);
                sw4.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class clsLog
{
    /// <summary>
    /// Función que se encarga de escribir en los logs
    /// </summary>
    /// <param name="sRuta">Ruta del Log</param>
    /// <param name="sMensajeError">Mensaje de error</param>
    public static void Escribir(string sRuta, string sMensajeError)
    {
        DateTime Fecha = DateTime.Today;
        string sPathex;
        try
        {
            sPathex = sRuta + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
            if (!File.Exists(sPathex))
            {
                StreamWriter sr4 = new StreamWriter(sPathex);
                sr4.WriteLine(sMensajeError);
                sr4.Close();
            }
            else
            {
                System.IO.StreamWriter sw4 = new System.IO.StreamWriter(sPathex, true);
                sw4.WriteLine(sMensajeError);
                sw4.Close();
            }
        }
        catch (Exception ex)
        { 
        
        }
    }
}


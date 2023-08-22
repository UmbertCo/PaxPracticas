using System;
using System.IO;
using ConectorPDFSharp.Properties;

public class clsLog
{
    /// <summary>
    /// Escribe en un archivo log
    /// </summary>
    /// <param name="sMensaje">El Mensaje a escribir</param>
    public static void EscribirLog(string sMensaje)
    {
        string path = Settings.Default["LogError"] + "LogError - " + String.Format("{0:dd-MM-yyyy}", DateTime.Now) + ".txt";

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


    public static void EscribirLogCancelacion(string sMensaje)
    {
        string path = Settings.Default["LogCancelacion"] + "LogError - " + String.Format("{0:dd-MM-yyyy}", DateTime.Now) + ".txt";

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


    /// <summary>
    /// Función que espera a que el archivo pueda ser leido para ser procesado.
    /// </summary>
    /// <param name="fullPath">Dirección completa</param>
    /// <returns></returns>
    public static bool fnWaitForFile(string fullPath)
    {
        int numTries = 0;
        while (true)
        {
            ++numTries;
            try
            {
                using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 100))
                {
                    fs.ReadByte();
                    break;
                }
            }
            catch (Exception)
            {
                if (numTries > 10)
                {
                    return false;
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
        return true;
    }
}


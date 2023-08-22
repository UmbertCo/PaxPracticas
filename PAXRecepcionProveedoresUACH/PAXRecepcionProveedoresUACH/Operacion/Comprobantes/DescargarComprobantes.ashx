<%@ WebHandler Language="C#" Class="DescargarComprobantes" %>

using System;
using System.Web;
using System.IO;

public class DescargarComprobantes : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        string Ruta = string.Empty;
        try
        {
            if (context.Request.QueryString["f"] != null && context.Request.QueryString["d"] != null)
            {
                Ruta = clsComun.ObtenerParamentro("RutaDocZips") + context.Request.QueryString["d"].ToString() + "\\" + context.Request.QueryString["f"].ToString() + ".zip";

                FileInfo f = new FileInfo(Ruta);
                string sFileName = f.Name.Replace(f.Extension, "");
                string sRutaCompletaZIP = f.FullName;

                //Leer archivo
                byte[] aContenidoArchivo = File.ReadAllBytes(sRutaCompletaZIP);

                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                context.Response.CacheControl = "public";
                context.Response.ContentType = "application/zip";
                context.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName + ".zip");//Gid + ".zip");
                context.Response.AddHeader("Content-Length", aContenidoArchivo.Length.ToString());
                context.Response.BinaryWrite(aContenidoArchivo);
                //Response.WriteFile(sRutaCompletaZIP);
                //Response.TransmitFile(sRutaCompletaZIP);
                context.Response.Flush();
                //Response.Close();
                context.Response.End();


            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        finally
        {
            if (!String.IsNullOrEmpty(Ruta))
            {
                fnLimpiaCarpetas(Ruta);
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    /// <summary>
    /// Elimina las carpetas temporales que fueron creadas
    /// </summary>
    private void fnLimpiaCarpetas(string sRutaArchivoZip)
    {
        try
        {
            string sFolder = clsComun.ObtenerParamentro("RutaDocZips"); //+ sCarpetaZIP;
            string sCarpetaZIP = Path.GetDirectoryName(sRutaArchivoZip);
            string sFolderXmls = clsComun.ObtenerParamentro("RutaDocXmlZips");

            if (Directory.Exists(sFolder))
            {
                foreach (string d in Directory.GetDirectories(sFolder))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(d);

                    TimeSpan tsDirectorio = new TimeSpan(dirInfo.CreationTime.Ticks);
                    TimeSpan tsActual = new TimeSpan(DateTime.Now.Ticks);

                    //Elimina todas los directorios con:
                    //  - un tiempo de creación mayor a 30 minutos
                    //  - el directorio actual que se está descargando
                    // No se elimina el directorio donde se guardan los PDF y XML (sFolderXmls)
                    if (d.ToUpper() + "\\" != sFolderXmls.ToUpper() && (d.ToUpper() == sCarpetaZIP.ToUpper() || (tsActual.TotalMinutes - tsDirectorio.TotalMinutes) >= 30))
                    {
                        foreach (string file in Directory.GetFiles(d))
                        {
                            File.Delete(file);
                        }
                        Directory.Delete(d, true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

}
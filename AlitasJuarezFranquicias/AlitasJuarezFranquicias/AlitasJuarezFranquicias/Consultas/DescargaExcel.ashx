<%@ WebHandler Language="C#" Class="DescargaExcel" %>

using System;
using System.Web;

public class DescargaExcel : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        string sArchivo = string.Empty;

        try
        {
            sArchivo = (context.Request.QueryString["f"] != null) ? context.Request.QueryString["f"].ToString() : string.Empty;

            String dlDir = @"Exceles/";

            string filename = context.Server.MapPath(dlDir + sArchivo + ".xls");

            if (!String.IsNullOrEmpty(filename))
            {
                String path = filename;

                System.IO.FileInfo toDownload = new System.IO.FileInfo(path);

                if (toDownload.Exists)
                {
                    context.Response.Clear();
                    context.Response.Buffer = true;
                    context.Response.ContentType = "application/vnd.ms-excel";
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
                    context.Response.AddHeader("Content-Length", toDownload.Length.ToString());
                    context.Response.Charset = "UTF-8";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.WriteFile(path);
                    context.Response.Flush();
                    context.Response.End();
                }
            }
        }
        catch (Exception ex)
        {
           //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
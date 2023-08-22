using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Imagen : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int nEStructu = 0;
        try
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["estructura"])))
                nEStructu = Convert.ToInt32(Request.QueryString["estructura"]);

            int nUbicacion = Convert.ToInt32(Request.QueryString["Ubic"]);

            byte[] bytImangen = { };

            switch (nUbicacion)
            {
                case 1:
                    //obtenemos la imagen 
                    bytImangen = clsComun.fnObterimagenNodeHijo(nEStructu, string.Empty);
                    if (bytImangen != null)
                    {
                        Response.ContentType = "image/jpeg";
                        Response.Expires = 0;
                        Response.Buffer = true;
                        Response.Clear();
                        Response.BinaryWrite(bytImangen);
                        Response.End();
                    }
                    break;
                case 2:
                    //obtenemos la imagen 
                    bytImangen = clsComun.fnObterimagenNodeHijo(nEStructu, string.Empty);
                    if (bytImangen != null)
                    {
                        Response.ContentType = "image/jpeg";
                        Response.Expires = 0;
                        Response.Buffer = true;
                        Response.Clear();
                        Response.BinaryWrite(bytImangen);
                        Response.End();
                    }
                    break;
                case 3:
                    bytImangen = clsComun.fnObtenerImgPublic();
                    if (bytImangen != null)
                    {
                        Response.ContentType = "image/jpeg";
                        Response.Expires = 0;
                        Response.Buffer = true;
                        Response.Clear();
                        Response.BinaryWrite(bytImangen);
                        Response.End();
                    }

                    break;

                default:
                    break;

            }
        }
        catch (Exception ex)
        { clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion); }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ImagenTicket : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        //{
        //    int nEStructu = 0;
        //    if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["estructura"])))
        //    nEStructu = Convert.ToInt32(Request.QueryString["estructura"]);
        //    int nUbicacion = Convert.ToInt32(Request.QueryString["Ubic"]);

        //    //switch (nUbicacion)
        //    //{
        //    //    case 2:
        //            //obtenemos la imagen 
        //            byte[] bytImangen = clsComun.fnObterimagenNodeHijo(nEStructu, nUbicacion.ToString());//clsComun.fnObterImagenes(nEStructu);
        //            if (bytImangen != null)
        //            {
        //                Response.ContentType = "image/jpeg";
        //                Response.Expires = 0;
        //                Response.Buffer = true;
        //                Response.Clear();
        //                Response.BinaryWrite(bytImangen);
        //                Response.End();
        //            }
         
        //        //default:
        //        //    break;
        //    //}
        //}
        //catch (Exception ex)
        //{ clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion); }
    }

    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx");
        }

    }
}
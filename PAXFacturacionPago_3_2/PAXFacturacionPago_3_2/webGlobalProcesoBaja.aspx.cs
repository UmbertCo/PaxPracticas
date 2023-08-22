using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;

public partial class webGlobalProcesoBaja : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsOperacionCuenta gDAL = new clsOperacionCuenta();

        string[] psCadena = Request.QueryString["dub"].Split(',');
        byte[] bDatos = new byte[psCadena.Length];

        for(int i = 0; i < psCadena.Length; i++)
        {
            bDatos[i] = Convert.ToByte(psCadena[i]);
        }

        //bDatos = Utilerias.Encriptacion.DES3.Desencriptar(bDatos);        
        bDatos = PAXCrypto.CryptoAES.DesencriptaAESB(bDatos);
        string cadenaDatos = System.Text.Encoding.UTF8.GetString(bDatos);

        string[] sDatos = cadenaDatos.Split(':');

        if (gDAL.fnEliminarCuenta(sDatos[0], sDatos[1], sDatos[2]))
        {
            Response.Write(Resources.resCorpusCFDIEs.varBajaExitosa);
            Response.End();
        }
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
    /// <summary>
    /// Loads the language specific resources
    /// </summary>
    protected override void InitializeCulture()
    {
        if (Session["Culture"] != null)
        {
            string lang = Session["Culture"].ToString();
            if ((lang != null) || (lang != ""))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
            }
        }
        else
        {
            string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }
    }
}
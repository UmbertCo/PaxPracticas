using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Threading;
using System.Data;


public partial class Inicio : System.Web.UI.Page
{

    private PagComercial gPag;
    protected void Page_Load(object sender, EventArgs e)
    {
        gPag = new PagComercial();
        DataTable dtPreguntas = gPag.fnSeleccionaNoticia();
        if (dtPreguntas.Rows.Count > 0)
        {
            DataRow renglon = dtPreguntas.Rows[0];
            lblNoticias.Text = Convert.ToString(renglon["descripcion"]);
        }
    }

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
            string language= "Es-Mx"; //=System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }
    }
}





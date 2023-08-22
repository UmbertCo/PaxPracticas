using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Globalization;

public partial class LeerMasNoticias : System.Web.UI.Page
{
    private PagComercial gPag;
    protected void Page_Load(object sender, EventArgs e)
    {
        fnCargaGrid();
    }
    private void fnCargaGrid()
    {
        gPag = new PagComercial();
        DataTable dtNoticias = gPag.fnSelNoticia();
        gvPreguntas.DataSource = dtNoticias;
        gvPreguntas.DataBind();
    }
    protected void gvPreguntas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        foreach (GridViewRow renglon in gvPreguntas.Rows)
        {
            Label idNoticia;
            idNoticia = ((Label)renglon.FindControl("lblIdNoticia"));
            HyperLink Noticia;
            Noticia = ((HyperLink)renglon.FindControl("hlNoticia"));
            //string codigo = Utilerias.Encriptacion.Classica.Encriptar(Convert.ToString(idPregunta.Text));
            Noticia.NavigateUrl = "webMasNoticia.aspx?idnoticia=" + idNoticia.Text;
            Noticia.Target = "_blank";
        }
    }
    protected void gvPreguntas_SelectedIndexChanged(object sender, EventArgs e)
    {

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
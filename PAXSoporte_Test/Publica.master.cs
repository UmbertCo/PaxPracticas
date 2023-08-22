using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Services;
using System.Configuration;
using System.Web.Configuration;
public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/Pantallas/Login/webInicioSesionLogin.aspx");
        }
        if (!IsPostBack)
        {
            /*string sb = string.Empty;

            sb = "<script type=\"text/javascript\">";
            sb = sb + " function redireccionar() ";
            sb = sb + " { ";

            sb = sb + " if(confirm(\" " + Resources.resCorpusCFDIEs.msgRedireccionar + "\")){ ";
            sb = sb + " location.reload(); ";
            sb = sb + " } ";

            sb = sb + " else{ ";
            sb = sb + " LoggingOut(); ";
            sb = sb + " } ";

            sb = sb + " } ";
            sb = sb + " setTimeout(\"redireccionar()\", 600000); ";
            sb = sb + " </script> ";


            Type cstype = this.GetType();

            ClientScriptManager CSM = Page.ClientScript;
            CSM.RegisterStartupScript(cstype, "Jredirecciona", sb);*/

            ClientScriptManager CSM = Page.ClientScript;

            //Session["Reset"] = true;
            //Configuration config = WebConfigurationManager.OpenWebConfiguration(null);
            //SessionStateSection section = (SessionStateSection)config.GetSection("system.web/sessionState");
            //int timeout = (int)section.Timeout.TotalMinutes * 1000 * 60;
            int timeout = 600000;
            CSM.RegisterStartupScript(this.GetType(), "SessionAlert", "SessionExpireAlert(" + timeout + ");", true);


        }
    }

    protected void HeadLoginStatus_LoggedOut(object sender, EventArgs e)
    {
        Session.Abandon();
        FormsAuthentication.SignOut();
    }

    protected void drpIdioma_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Culture"] = this.drpIdioma.SelectedValue;
        Response.Redirect(Request.RawUrl);
    }
    protected void drpIdioma_PreRender(object sender, EventArgs e)
    {
        if ((!IsPostBack) && (Session["Culture"] != null))
        {
            string lang = Session["Culture"].ToString();
            int index = drpIdioma.Items.IndexOf(drpIdioma.Items.FindByValue(lang));
            drpIdioma.SelectedIndex = index;
        }
    }

    protected void lnkEspañol_Click(object sender, EventArgs e)
    {
        Session["Culture"] = "es-Mx";
        Response.Redirect(Request.RawUrl);
    }
    protected void lnkEnglish_Click(object sender, EventArgs e)
    {
        Session["Culture"] = "en-Us";
        Response.Redirect(Request.RawUrl);
    }
    protected void lnkEnglish_PreRender(object sender, EventArgs e)
    {
        if ((!IsPostBack) && (Session["Culture"] != null))
        {
            string lang = Session["Culture"].ToString();
        }
    }
    protected void lnkEspañol_PreRender(object sender, EventArgs e)
    {
        if ((!IsPostBack) && (Session["Culture"] != null))
        {
            string lang = Session["Culture"].ToString();
        }
    }
   
}

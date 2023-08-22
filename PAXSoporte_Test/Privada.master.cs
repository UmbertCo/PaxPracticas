using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

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
    protected void lnkEnglish_Click(object sender, EventArgs e)
    {
        Session["Culture"] = "en-Us";
        Response.Redirect(Request.RawUrl);
    }
    protected void lnkEspañol_Click(object sender, EventArgs e)
    {
        Session["Culture"] = "es-Mx";
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

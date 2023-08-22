using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webGlobalError : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sQueryString = (Request.QueryString["p"] != null) ? Request.QueryString["p"].ToString().Replace(" ", " ") : string.Empty;
        if (sQueryString == "")
        {
            lblError.Visible = true;
            lblErrorDet.Visible = true;
        }
        else
        {
            lblError.Visible = false;
            lblErrorDet.Text = "ERROR";
            lblErrorDesc.Text = sQueryString;
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
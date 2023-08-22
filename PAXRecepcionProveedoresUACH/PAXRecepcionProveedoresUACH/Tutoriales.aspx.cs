using System;
using System.Globalization;
using System.Threading;

public partial class Tutoriales : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MyYoutubeVideo.Text = "<iframe title=\"YouTube video player\""
          + "width=\"320\" height=\"195\" src=\"http://www.youtube.com/embed/"
          + "" + "PtbHKHNjUDQ" + "\" frameborder=\"0\" allowfullscreen></iframe>";

            MyYoutubeVideo1.Text = "<iframe title=\"YouTube video player\""
          + "width=\"320\" height=\"195\" src=\"http://www.youtube.com/embed/"
          + "" + "jeVmGsQyCrk" + "\" frameborder=\"0\" allowfullscreen></iframe>";

            MyYoutubeVideo2.Text = "<iframe title=\"YouTube video player\""
          + "width=\"320\" height=\"195\" src=\"http://www.youtube.com/embed/"
          + "" + "iVe32ywsIu0" + "\" frameborder=\"0\" allowfullscreen></iframe>";

            MyYoutubeVideo4.Text = "<iframe title=\"YouTube video player\""
          + "width=\"320\" height=\"195\" src=\"http://www.youtube.com/embed/"
          + "" + "fDvR1jml-wo" + "\" frameborder=\"0\" allowfullscreen></iframe>";

            MyYoutubeVideo3.Text = "<iframe title=\"YouTube video player\""
          + "width=\"320\" height=\"195\" src=\"http://www.youtube.com/embed/"
          + "" + "MRGFnNVX2zE" + "\" frameborder=\"0\" allowfullscreen></iframe>";
        }
        else
        {
            InitializeCulture();
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
            string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Twitterizer;
using System.Data;
using Utilerias.SQL;
using System.Data.SqlClient;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Globalization;
using System.Threading;
namespace PaginaComercial
{
    public partial class webNoticias : System.Web.UI.Page
    {
        private PagComercial gPag;
        protected void Page_Load(object sender, EventArgs e)
        {
            fnCargaGrid();
        }

        protected void button1_Click(object sender, EventArgs e)
        {
           
            Label5.Visible = false;
            Label6.Visible = false;
            if (txtDescripcionI.Text != "")
            {
                if (txtDescripcionI.Text.Length <= 140)
                {
                    if (Convert.ToInt32(Session["idNoticia"]) == 0)
                    {
                        gPag = new PagComercial();
                        gPag.fnInsertaNoticia(txtDescripcionI.Text);
                        // PostTwitterUpdate(txtDescripcionI.Text);
                        Label6.Visible = true;
                        btnGuardar.Enabled = false;
                        fnCargaGrid();

                    }
                    else
                    {
                        gPag = new PagComercial();
                        gPag.fnUpdNoticia(Convert.ToInt32(Session["idNoticia"]), txtDescripcionI.Text);
                        Label6.Visible = true;
                        // PostTwitterUpdate(txtDescripcionI.Text);
                        Label6.Visible = true;
                        fnCargaGrid();
                    }
                }
                else
                {
                    Label5.Visible = true;
                }
            }
            else
            {
                Label5.Visible = true;
            }
            txtDescripcionI.Text = "";
        }

        private void fnCargaGrid()
        {
            gPag = new PagComercial();
            DataTable dtNoticias = gPag.fnSelNoticia();
            gvPreguntas.DataSource = dtNoticias;
            gvPreguntas.DataBind();
        }

        public void PostTwitterUpdate(string tweet)
        {

            string TwitterMessage = txtDescripcionI.Text;
            string ConsumerKey = "5AXxAX5XAKahHjNydM3pUw";
            string ConsumerSecret = "8LlJyoZcoqGw4syEbpsiQFYM7XO39u1apcxBROYDY";
            string Token = "360701685-pO0vYwLTvQetM1Lh1wlFNqKV4EGVwSHgSlT2Y2ZG";
            string TokenSecret = "ve38XwtOM0P7vUBut0k3M8PRaIXkilnbQfKk8zaw";

            OAuthTokens tokens = new OAuthTokens();
            tokens.AccessToken = Token;
            tokens.AccessTokenSecret = TokenSecret;
            tokens.ConsumerKey = ConsumerKey;
            tokens.ConsumerSecret = ConsumerSecret;

            TwitterResponse<TwitterStatus> tweetResponse = TwitterStatus.Update(tokens, tweet);

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
        protected void button2_Click(object sender, EventArgs e)
        {
            txtDescripcionI.Text = "";
            txtDescripcionI.Enabled = true;
            Session["idNoticia"] = null;
            Label6.Visible = false;
            Label5.Visible = false;
            btnGuardar.Enabled = true;
        }


        protected void button3_Click(object sender, EventArgs e)
        {
            txtDescripcionI.Text = "";
          
        }


        protected void gvPreguntas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                gPag = new PagComercial();
                int psidNoticia = Convert.ToInt32(e.Keys["idNoticia"].ToString());
                gPag.fnDelNoticia(psidNoticia);
                fnCargaGrid();
                txtDescripcionI.Text = "";
                btnGuardar.Enabled = false;
                txtDescripcionI.Enabled = false;
                Session["idNoticia"] = null;
                Label6.Visible = false;
                Label5.Visible = false;
            }
            catch
            {
            }
        }
        protected void gvPreguntas_SelectedIndexChanged(object sender, EventArgs e)
        {
            gPag = new PagComercial();

            try
            {
                //obtener los valores de la noticia seleccionada en el grid
                GridViewRow gvrFila = (GridViewRow)gvPreguntas.SelectedRow;
                Session["idNoticia"] = Convert.ToInt32(gvPreguntas.SelectedDataKey.Value);
                int idNoticia = Convert.ToInt32(Session["idNoticia"]);
                DataTable dtNoticia = gPag.fnSeleccionaNoticia(idNoticia);
                foreach (DataRow renglon in dtNoticia.Rows)
                {
                    txtDescripcionI.Text = Convert.ToString(renglon["descripcion"]);
                    btnGuardar.Enabled = true;
                    txtDescripcionI.Enabled = true;
                    Label6.Visible = false;
                }
            }
            catch (Exception)
            {
                //false;
            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            Response.Redirect("Inicio.aspx");
        }
}


}
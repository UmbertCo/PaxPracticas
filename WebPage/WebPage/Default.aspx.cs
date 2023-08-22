using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Negocio;

namespace PAXActividades
{
    public partial class WebForm7 : System.Web.UI.Page
    {
        //crea una nueva instancia de la capa logica
        Logica negocio = new Logica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Nombre"] != null)
            {
                //el control contiene el valor de las variables 
                lblUsuario.Text = Session["Nombre"].ToString() + " " + Session["ApPaterno"].ToString();

                //termina la ejecucion del metodo 
                return;
            }

            //el control contiene un texto
            lblUsuario.Text = "Bienvenido ";
        }

        protected void btnAvisoSi_Click(object sender, EventArgs e)
        {

            Response.Redirect("NuevoPersonal.aspx");
        }
    }
}
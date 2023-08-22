using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;

namespace WebPage
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //crea una nueva instancia de la capa logica
        Logica negocio = new Logica();

        protected void Page_Load(object sender, EventArgs e)
        {
            //el control contiene el valor de la variable 
            txtUsuario.Enabled = false;
            txtUsuario.Text = Session["NombrePersonal"].ToString() + "." + Session["ApUsuario"].ToString();
            mdlRegistroDeUsuario.Show();
        }
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            //variables con los valores determinados por los controles
            int idNuevopersonal = Convert.ToInt32(Session["Idpersonal_nuevo"]);
            string usuario = txtUsuario.Text;
            string contrasena = txtpass.Text;
            string tipo = ddltypoPerfil.SelectedValue;

            if (usuario == string.Empty)
            {

            } if (contrasena == string.Empty)
            {

            }

            //la variable contiene el resultado de la funcion que recive parametros
            int idUsuario = negocio.fnAgregaUsuario(usuario, Utilerias.Encriptacion.Classica.Encriptar(contrasena), tipo);//, idNuevopersonal);

            //Realiza la relacion del Personal con el usuario
            negocio.fnRelacionPersonalUsuario(idNuevopersonal, idUsuario);

            //muestra el control Ajax avisando si o no continua con la operacion 
            mdlContinuar.Show();
        }
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            //limpia la variable especificada 
            Session.Remove("Idpersonal_nuevo");

            Response.Redirect("NuevoPersonal.aspx");
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}
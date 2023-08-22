using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
namespace WebPage
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        //crea una nueva instancia de la capa logica
        Logica negocio = new Logica();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            //instrucción controla los errores posibles 
            try
            {
                //variables con los valores determinados por los controles
                string nomproyecto = txtProyecto.Text;
                string descproyecto = txtDescripcion.Text;
                DateTime FechaProyecto = DateTime.Now;
                bool estatus = true;

                //llama una funcion que resive parametros
                negocio.fnAgregarProyecto(nomproyecto, descproyecto, FechaProyecto, estatus);

                //llena los campos de los controles con espacio
                txtDescripcion.Text = "";
                txtProyecto.Text = "";

                //el control contiene un texto espeficicado
                lblcambio2.Text = Resources.Resource_es.lblmdlAvisoProyectoSatisfactorio;

                //muestra el control Ajax de aviso
                MdlAviso.Show();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //el control contiene un texto espeficicado
                lblcambio2.Text = Resources.Resource_es.lblmdlAvisoErrorBajaProyectoAsignado;

                //muestra el control Ajax de aviso
                MdlAviso.Show();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //llena los campos de los controles con espacio
            txtDescripcion.Text = "";
            txtProyecto.Text = "";
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            //muestra el control Ajax confirmando el nuevo proyecto
            mdlNuevoProyecto.Show();
        }
    }
}

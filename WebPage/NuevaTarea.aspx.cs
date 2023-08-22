using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using System.Data;

namespace WebPage
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        //crea una nueva instancia de la capa logica
        Logica negocio = new Logica();

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnNueva_Click(object sender, EventArgs e)
        {
            //muestra el control Ajax para confirmar la nueva tarea
            mdlNuevaTarea.Show();
        }
        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (txtTarea.Text != null || txtDescripcion.Text != null)
            {
                //variables con los valores determinados por los controles
                string tarea = txtTarea.Text;
                string descripciontarea = txtDescripcion.Text;
                DateTime fechatarea = DateTime.Now;
                bool estatus = true;

                //la variable logica contiene el resultado de la funcion que resive parametros
                bool agregar = negocio.fnAgregarTarea(tarea, descripciontarea, fechatarea, estatus);
                if (agregar == true)
                {
                    //muestra el control Ajax aviso
                    mdlAviso.Show();
                }
            }
        }
        protected void btnAvisoOk_Click(object sender, EventArgs e)
        {
            //llena los campos de los controles con espacio
            txtTarea.Text = "";
            txtDescripcion.Text = "";
        }
    }
}
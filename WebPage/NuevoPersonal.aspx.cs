using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;

namespace WebPage
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        //crea una nueva instancia de la capa logica
        Logica negocio = new Logica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //llama el metodo para llenar el combobox con los puestos
                puesto();
            }
        }
        protected void btnOK_Click(object sender, EventArgs e)
        {
            //variables con los valores determinados por los controles
            string Nombre = txtNombre.Text;
            string Direccion = txtDireccion.Text;
            string ApPaterno = txtApPaterno.Text;
            string ApMaterno = txtApMaterno.Text;
            string Correo = txtCorreo.Text;
            string Telefono = txtTelefono.Text;
            int IdPuesto = Convert.ToInt32(ddlPuesto.SelectedValue);
            bool estatus = true;
            DateTime FeAlta = DateTime.Now;
            Session["ApUsuario"] = txtApPaterno.Text;

            //la variable contiene el resultado de la funcion que recive parametros
            int idpersonal = negocio.fnAgregarPersonal(Nombre, ApPaterno, ApMaterno, Direccion, Telefono, Correo, IdPuesto, estatus, FeAlta);

            //llame el metodo que limpia los controles
            limpiar(this);

            //posiciona el cursor en la caja de texto
            txtNombre.Focus();

            //variables obtienen el valor de las variables
            Session["Idpersonal_nuevo"] = idpersonal;
            Session["NombrePersonal"] = Nombre;

            Response.Redirect("NuevoUsuario.aspx");
        }

        protected void btnNuevo_Click1(object sender, EventArgs e)
        {
            if (txtNombre.Text == string.Empty || txtDireccion.Text == string.Empty || txtApPaterno.Text == string.Empty
               || txtApMaterno.Text == string.Empty || txtTelefono.Text == string.Empty || txtCorreo.Text == string.Empty)
            {
                //posiciona el cursor en la caja de texto
                txtNombre.Focus();

                //termina la ejecucion del metodo 
                return;
            }
            if (negocio.busqueda(txtNombre.Text, txtApPaterno.Text) == true)
            {
                //muestra el control Ajax aviso de personal existente
                mdlAvisoUsuario.Show();
            }
            else
            {
                //muestra el control Ajax confirma el nuevo personal
                mdlNuevoPersonal.Show();
            }

        }

        //metodo para llenar los combobox con los puestos
        private void puesto()
        {
            //obtiene el origen de la base de datos
            ddlPuesto.DataSource = negocio.fnMostrarPuestos();

            //establece el valor del campo del origen de la base de datos
            ddlPuesto.DataValueField = "IdPuesto";

            //obtiene el texto del campo del origen de base de datos
            ddlPuesto.DataTextField = "Puesto";

            //enlaza el origen de base de datos al control
            ddlPuesto.DataBind();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //llama el metodo que limpia los controles
            limpiar(this);

            //posiciona el cursor en la caja de texto
            txtNombre.Focus();

        }

        //metodo para limpar los controles
        private void limpiar(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    limpiar(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = "";
                            break;
                    }
                }
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            //posiciona el cursor en la caja de texto
            txtNombre.Focus();
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            Response.Redirect("NuevoUsuario.aspx");
        }

        protected void btnAvisoOk_Click(object sender, EventArgs e)
        {
            //llama el metodo para limpiar los controles
            limpiar(this);
        }
    }
}


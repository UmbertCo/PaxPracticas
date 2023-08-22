using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using System.Drawing;

namespace WebPage
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        //crea una nueva instancia de la capa logica
        Logica negocio = new Logica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fnMostrarPersonal();
            }
        }

        //metodo para llenar el gridview 
        private void fnMostrarPersonal()
        {
            //obtiene el origen de la base de datos
            gdvPersonal.DataSource = negocio.fnMostrarPersonal();

            //enlaza el origen de base de datos al control
            gdvPersonal.DataBind();
        }

        protected void gdvPersonal_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////llama el metodo para llenar un combobox
            puesto();

            //llama el metodo para llenar el gridview
            fnMostrarPersonal();

            //muestra el control Ajax para confirmar la actualizacion 
            mdlActualizar.Show();

            //la variable de tipo gridviewrow obtiene la fila seleccionada del control
            GridViewRow fila = gdvPersonal.SelectedRow;

            //variables obtienen el valor determinado de la fila seleccionada
            int IdPersonal = Convert.ToInt32(gdvPersonal.DataKeys[fila.RowIndex].Values["IdPersonal"]);
            string Nombre = gdvPersonal.DataKeys[fila.RowIndex].Values["Nombre"].ToString();
            string Direccion = Convert.ToString(gdvPersonal.DataKeys[fila.RowIndex].Values["Direccion"]);
            string ApPaterno = Convert.ToString(gdvPersonal.DataKeys[fila.RowIndex].Values["ApPaterno"]);
            string ApMaterno = Convert.ToString(gdvPersonal.DataKeys[fila.RowIndex].Values["ApMaterno"]);
            string Correo = Convert.ToString(gdvPersonal.DataKeys[fila.RowIndex].Values["Correo"]);
            string Telefono = Convert.ToString(gdvPersonal.DataKeys[fila.RowIndex].Values["Telefono"]);
            string Puesto = Convert.ToString(gdvPersonal.DataKeys[fila.RowIndex].Values["Puesto"]);
            int IdPuesto = Convert.ToInt32(gdvPersonal.DataKeys[fila.RowIndex].Values["IdPuesto"]);

            //pinta la fila del color verde
            fila.BackColor = Color.Gray;

            //muestra los valores en los controles determinados 
            lblIdpersonal.Text = Convert.ToString(IdPersonal);
            txtNombre.Text = Nombre;
            txtDireccion.Text = Direccion;
            txtApPaterno.Text = ApPaterno;
            txtApMaterno.Text = ApMaterno;
            txtCorreo.Text = Correo;
            txtTelefono.Text = Telefono;
            lblpuesto.Text = Convert.ToString(Puesto);

            //iguala el valor del combobox con el del grid 
            ddlPuesto.SelectedValue = IdPuesto.ToString();

            //la variable permite guardar el valor del control
            ViewState["idPersonal"] = lblIdpersonal.Text;
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            if (ViewState["idPersonal"] != null)
            {
                //instrucción controla los errores posibles 
                try
                {
                    //variables con los valores determinados por los controles
                    int idpersonal = Convert.ToInt32(ViewState["idPersonal"]);
                    string Nombre = txtNombre.Text;
                    string Direccion = txtDireccion.Text;
                    string ApPaterno = txtApPaterno.Text;
                    string ApMaterno = txtApMaterno.Text;
                    string Correo = txtCorreo.Text;
                    string Telefono = txtTelefono.Text;
                    string puesto = ddlPuesto.Text;
                    bool estatus = true;

                    //llama una funcion que resive parametros
                    negocio.fnActualizarPersonal(idpersonal, Nombre, Direccion, ApPaterno, ApMaterno, Correo, Telefono, estatus, puesto);

                    //llama el metodo para llenar el gridview
                    fnMostrarPersonal();

                    //llama el metodo para limpiar los controles
                    limpiar(this);

                    //limpia todas variables de tipo viewstate
                    ViewState.Clear();

                    //el control imprime un texto espeficicado
                    lblcambio2.Text = Resources.Resource_es.lblmdlAvisoSatisfac;

                    //muestra el control Ajax de aviso
                    ModalPopupExtender1.Show();

                    //muetra el combobox vacio
                    ddlPuesto.SelectedItem.Text = "";

                    //termina la ejecucion del metodo 
                    return;
                }
                //atrapa diferentes clases de Excepciones
                catch
                {
                    //el control imprime un texto espeficicado
                    lblcambio2.Text = Resources.Resource_es.lblmdlAvisoErrorPersonal;

                    //muestra el control Ajax de aviso
                    ModalPopupExtender1.Show();
                }
            }

            //muestra el control Ajax de aviso
            mdlAviso.Show();
        }

        //metodo para llenar el combobox con los puestos
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
            if (ViewState["idPersonal"] != null)
            {
                //muestra el control Ajax para la confirmacion del la cancelacion 
                mdlConfirmarCancelacion.Show();

                //termina la ejecucion del metodo 
                return;
            }

            //muestra el control Ajax de aviso
            mdlAviso.Show();
        }

        //metodo para limpiar los controles 
        private void limpiar(Control parent)
        {
            //repite las intruciones para cada elemento de una coleccion
            foreach (Control c in parent.Controls)
            {

                if (c.Controls.Count > 0)
                {
                    //llama el metodo para limpiar controles
                    limpiar(c);
                }
                else
                {
                    //controla multiples selecciones
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = "";
                            break;
                    }
                }
            }
        }

        protected void btnBaja_Click(object sender, EventArgs e)
        {
            if (ViewState["idPersonal"] == null)
            {
                //muestra el control Ajax de aviso
                mdlAviso.Show();

                //termina la ejecucion del metodo 
                return;
            }

            //muestra el control Ajax para la confirmacion de la baja
            mdlBaja.Show();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            //llama el metodo para limpiar los controles 
            limpiar(this);

            //limpia todas variables de tipo viewstate
            ViewState.Clear();

            //llama el metodo para llenar el gridview
            fnMostrarPersonal();

            //muetra el combobox vacio
            ddlPuesto.SelectedItem.Text = "";
        }

        protected void btnNos_Click(object sender, EventArgs e)
        {
            //llama el metodo para limpiar los controles 
            limpiar(this);

            //limpia todas variables de tipo viewstate
            ViewState.Clear();

            //llama el metodo para llenar el gridview
            fnMostrarPersonal();
        }

        protected void btnAvisoOk_Click(object sender, EventArgs e)
        {
            //llama el metodo para limpiar los controles
            limpiar(this);
        }

        protected void btnBajaSi_Click(object sender, EventArgs e)
        {
            if (ViewState["idPersonal"] == null)
            {
                //muestra el control Ajax de aviso
                mdlAviso.Show();

                //termina la ejecucion del metodo 
                return;
            }

            //instrucción controla los errores posibles 
            try
            {
                //variables con los valores determinados por los controles
                int idpersonal = Convert.ToInt32(lblIdpersonal.Text);
                string Nombre = txtNombre.Text;
                string Direccion = txtDireccion.Text;
                string ApPaterno = txtApPaterno.Text;
                string ApMaterno = txtApMaterno.Text;
                string Correo = txtCorreo.Text;
                string Telefono = txtTelefono.Text;
                string puesto = ddlPuesto.Text;

                //variable logica
                bool estatus = false;

                //llama una funcion con parametros
                negocio.fnActualizarPersonal(idpersonal, Nombre, Direccion, ApPaterno, ApMaterno, Correo, Telefono, estatus, puesto);

                //llama el metodo para llenar el gridview
                fnMostrarPersonal();

                //llama el metodo para limpiar los controles
                limpiar(this);

                //limpia todas variables de tipo viewstate
                ViewState.Clear();

                //el control imprime un texto espeficicado
                lblcambio2.Text = Resources.Resource_es.lblmdlAvisoBajaPersonal;

                //muestra el control Ajax de aviso
                ModalPopupExtender1.Show();

                //muetra el combobox vacio
                ddlPuesto.SelectedItem.Text = "";
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //el control imprime un texto espeficicado
                lblcambio2.Text = Resources.Resource_es.lblmdlAvisoErrorBajaPersonal;

                //muestra el control Ajax de aviso
                ModalPopupExtender1.Show();
            }
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            ModalPopupEditar.Show();
        }
    }
}

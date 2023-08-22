using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using System.Drawing;

namespace PAXActividades
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        //crea una nueva instancia de la capa logica
        Logica negocio = new Logica();
        //carga la pagina
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //llama el metodo para llenar el grid con los proyectos
                mostrar_proyectos();
            }
        }

        //metodo para llenar el gridview 
        private void mostrar_proyectos()
        {
            //obtiene el origen de la base de datos
            gdvProyectos.DataSource = negocio.fnMostrarProyectos();

            //enlaza el origen de base de datos al control
            gdvProyectos.DataBind();
        }

        protected void gdvProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //llama el metodo para llenar el gridview
            mostrar_proyectos();

            //muestra el control Ajax para confirmar la actualizacion 
            mdlActualizar.Show();

            //la variable de tipo gridviewrow obtiene la fila seleccionada del control
            GridViewRow fila = gdvProyectos.SelectedRow;

            //variables obtienen el valor determinado de la fila seleccionada
            int idproyecto = Convert.ToInt32(gdvProyectos.DataKeys[fila.RowIndex].Values["IdProyecto"]);
            string NomProyecto = Convert.ToString(gdvProyectos.DataKeys[fila.RowIndex].Values["NomProyecto"]);
            string DescProyecto = Convert.ToString(gdvProyectos.DataKeys[fila.RowIndex].Values["DescProyecto"]);

            //muestra los valores en los controles determinados 
            lblIdProyecto.Text = Convert.ToString(idproyecto);
            txtNomProyecto.Text = NomProyecto;
            txtDescripcion.Text = DescProyecto;

            //pinta la fila del color verde
            fila.BackColor = Color.Gray;

            //la variable permite guardar el valor del control
            Session["IdProyecto"] = lblIdProyecto.Text;
        }
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            //instrucción controla los errores posibles 
            try
            {
                if (Session["IdProyecto"] != null)
                {
                    //variables con los valores determinados por los controles
                    int id = Convert.ToInt32(lblIdProyecto.Text);
                    string nomproyecto = txtNomProyecto.Text;
                    string descproycto = txtDescripcion.Text;
                    DateTime FechaModif = DateTime.Now;

                    //llama una funcion que resive parametros
                    negocio.Actualizar_proyecto(id, nomproyecto, descproycto, FechaModif);

                    //llama el metodo para llenar el gridview
                    mostrar_proyectos();

                    //llena los campos de los controles con espacio
                    txtDescripcion.Text = "";
                    txtNomProyecto.Text = "";

                    //la variable permite guardar el valor del control
                    Session.Remove("IdProyecto");

                    //el control imprime un texto espeficicado
                    lblcambio.Text = Resources.Resource_es.lblmdlActProyecto;

                    //muestra el control Ajax comfirmando el cambio
                    mdlAvisoCambio.Show();

                    //termina la ejecucion del metodo 
                    return;
                }
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //el control imprime un texto espeficicado
                lblcambio.Text = Resources.Resource_es.lblmdlErrorActProyecto;

                //muestra el control Ajax de notificacion que el cambio no se realizo
                mdlAvisoCambio.Show();
            }
            //muestra el control Ajax aviso
            mdlAviso.Show();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //llena los campos de los controles con espacio
            txtDescripcion.Text = "";
            txtNomProyecto.Text = "";
        }

        protected void btnAvisoOk_Click(object sender, EventArgs e)
        {
            //llena los campos de los controles con espacio
            txtDescripcion.Text = "";
            txtNomProyecto.Text = "";
        }

        protected void btnCandcelar_Click(object sender, EventArgs e)
        {
            if (Session["IdProyecto"] != null)
            {
                //muestra el control Ajax comfirmando de la cancelacion
                mdlConfirmarCancelacion.Show();
            }

            //muestra el control Ajax aviso
            mdlAviso.Show();
        }

        protected void btnBaja_Click(object sender, EventArgs e)
        {
            if (Session["IdProyecto"] != null)
            {
                int IdProyecto = Convert.ToInt32(Session["IdProyecto"]);
                bool existe = negocio.fnChecaProyectoAsignado(IdProyecto);
                if (existe == true)
                {
                    lblcambio.Text = Resources.Resource_es.lblmdlAvisoErrorBajaProyectoAsignado;
                    mdlAvisoCambio.Show();
                    return;
                }
                else
                {
                    //muestra el control Ajax confirma baja 
                    mdlBaja.Show();
                }
            }
            //muestra el control Ajax aviso
            mdlAviso.Show();
        }

        protected void btnBajaSi_Click(object sender, EventArgs e)
        {
            //instrucción controla los errores posibles 
            try
            {
                //variables con los valores determinados por los controles
                int idproyecto = Convert.ToInt32(lblIdProyecto.Text);
                bool estatus = false;

                //llama una funcion que resive parametros
                negocio.baja_proyecto(idproyecto, estatus);

                //llama el metodo para llenar el gridview
                mostrar_proyectos();

                //remueve el valor de la variable especificada
                Session.Remove("IdProyecto");

                //llena los campos de los controles con espacio
                txtDescripcion.Text = "";
                txtNomProyecto.Text = "";

                //el control imprime un texto espeficicado
                lblcambio.Text = Resources.Resource_es.lblmdlAvisoBajaProyecto;

                //muestra el control Ajax confirmando la baja
                mdlAvisoCambio.Show();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //el control imprime un texto espeficicado
                lblcambio.Text = Resources.Resource_es.lblmdlAvisoErrorBajaProyecto;

                //muestra el control Ajax notifica que no se realizo la baja
                mdlAvisoCambio.Show();
            }
        }

        protected void btnCancelarSi_Click(object sender, EventArgs e)
        {
            //remueve el valor de la variable especificada
            Session.Remove("IdProyecto");

            //llena los campos de los controles con espacio
            txtDescripcion.Text = "";
            txtNomProyecto.Text = "";
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            //remueve el valor de la variable especificada
            Session.Remove("IdProyecto");

            //llena los campos de los controles con espacio
            txtDescripcion.Text = "";
            txtNomProyecto.Text = "";

            //llama el metodo para llenar el gridview
            mostrar_proyectos();
        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            mdlEditarProyecto.Show();
        }
    }
}

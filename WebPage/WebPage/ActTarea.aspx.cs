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
    public partial class ActTarea : System.Web.UI.Page
    {
        //crea una nueva instancia de la capa logica
        Logica negocio = new Logica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mostrar_tareas();
            }

        }

        //metodo para llenar el gridview 
        private void mostrar_tareas()
        {
            //obtiene el origen de la base de datos
            gdvTareas.DataSource = negocio.fnMostrarTareas();

            //enlaza el origen de base de datos al control
            gdvTareas.DataBind();
        }
        protected void gdvTareas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //llama el metodo para llenar el gridview
            mostrar_tareas();

            //muestra el control Ajax para confirmar la actualizacion 
            mdlActualizar.Show();

            //la variable de tipo gridviewrow obtiene la fila seleccionada del control
            GridViewRow fila = gdvTareas.SelectedRow;

            //variables obtienen el valor determinado de la fila seleccionada
            ViewState["IdTarea"] = Convert.ToInt32(gdvTareas.DataKeys[fila.RowIndex].Values["IdTarea"]);
            string Tarea = Convert.ToString(gdvTareas.DataKeys[fila.RowIndex].Values["Tarea"]);
            string DescripcionTarea = Convert.ToString(gdvTareas.DataKeys[fila.RowIndex].Values["DescripcionTarea"]);

            //pinta la fila del color verde
            fila.BackColor = Color.Gray;

            //muestra los valores en los controles determinados 
            int IdTarea = Convert.ToInt16(ViewState["IdTarea"]);
            txtTarea.Text = Tarea;
            txtDescripcion.Text = DescripcionTarea;

        }

        protected void btnBaja_Click(object sender, EventArgs e)
        {
            if (ViewState["IdTarea"] != null)
            {
                int IdTarea = Convert.ToInt32(ViewState["IdTarea"]);
                bool existe = negocio.fnChecaTareaAsignada(IdTarea);
                if (existe == true)
                {
                    lblMensaje.Text = Resources.Resource_es.lblmdlAvisoErrorBajaTareaAsignada;
                    mdlMensaje.Show();
                    return;
                }
                else
                {
                    //muestra el control Ajax para la confirmacion de la baja
                    mdlBaja.Show();
                }
            }
            else
            {
                //muestra el control Ajax de aviso
                mdlAviso.Show();
            }
        }
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            //instrucción controla los errores posibles 
            try
            {
                if (ViewState["IdTarea"] != null)
                {
                    if (txtTarea.Text != null || txtDescripcion.Text != null)
                    {
                        //variables con los valores determinados por los controles
                        int IdTarea = Convert.ToInt32(ViewState["IdTarea"]);
                        string Tarea = txtTarea.Text;
                        string descripcion = txtDescripcion.Text;
                        string btnactualizar = "A";

                        //la variable logica contiene el resultado de la funcion que resive parametros
                        bool actualizarr = negocio.fnActualizarTarea(IdTarea, Tarea, descripcion, btnactualizar);

                        if (actualizarr == true)
                        {
                            //el control es visible
                            lblMensaje.Visible = true;

                            //el control contiene un texto 
                            lblMensaje.Text = Resources.Resource_es.lblActualizarMensaje;

                            //muestra el control Ajax mustra un mensaje
                            mdlMensaje.Show();

                            //llama el metodo para llenar el gridview
                            mostrar_tareas();

                            //limpia todas variables de tipo viewstate
                            ViewState.Clear();

                            //llena los campos de los controles con espacio
                            txtTarea.Text = "";
                            txtDescripcion.Text = "";
                        }
                    }

                }
                else
                {
                    //muestra el control Ajax aviso
                    mdlAviso.Show();
                }
            }
            //atrapa diferentes clases de Excepciones
            catch (Exception)
            {
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (ViewState["IdTarea"] != null)
            {
                //muestra el control Ajax para la confirmacion del la cancelacion 
                mdlConfirmarCancelacion.Show();
            }
            else
            {
                //muestra el control Ajax aviso
                mdlAviso.Show();
            }
        }

        protected void btnNos_Click(object sender, EventArgs e)
        {
            //limpia todas variables de tipo viewstate
            ViewState.Clear();

            //llena los campos de los controles con espacio
            txtTarea.Text = "";
            txtDescripcion.Text = "";

            //llama el metodo para llenar el gridview
            mostrar_tareas();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            //limpia todas variables de tipo viewstate
            ViewState.Clear();

            //llena los campos de los controles con espacio
            txtTarea.Text = "";
            txtDescripcion.Text = "";
        }

        protected void btnBajaSi_Click(object sender, EventArgs e)
        {
            //instrucción controla los errores posibles 
            try
            {
                if (ViewState["IdTarea"] != null)
                {
                    if (txtTarea.Text != null || txtDescripcion.Text != null)
                    {
                        //variables con los valores determinados por los controles
                        int IdTarea = Convert.ToInt32(ViewState["IdTarea"]);
                        string Tarea = txtTarea.Text;
                        string descripcion = txtDescripcion.Text;
                        string btnactualizar = "B";

                        //la variable logica contiene el resultado de la funcion que resive parametros
                        bool actualizarr = negocio.fnActualizarTarea(IdTarea, Tarea, descripcion, btnactualizar);
                        if (actualizarr == true)
                        {
                            //el control es visible
                            lblMensaje.Visible = true;

                            //el control contiene un texto 
                            lblMensaje.Text = Resources.Resource_es.lblBajaMensaje;

                            //muestra el control Ajax mustra un mensaje
                            mdlMensaje.Show();

                            //llama el metodo para llenar el gridview
                            mostrar_tareas();

                            //limpia todas variables de tipo viewstate
                            ViewState.Clear();

                            //llena los campos de los controles con espacio
                            txtTarea.Text = "";
                            txtDescripcion.Text = "";
                        }
                    }
                }
            }
            //atrapa diferentes clases de Excepciones
            catch (Exception)
            {
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void btnSi_Click(object sender, EventArgs e)
        {
            mdlActualizarTarea.Show();
        }
    }
}
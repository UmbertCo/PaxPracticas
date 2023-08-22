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
    public partial class NuevaTarea : System.Web.UI.Page
    {
        //crea una nueva instancia de la capa logica
        Logica negocio = new Logica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                //llama el metodo para llenar el grid
                tareas();

                //llama el metodo para llenar el combobox con el personal 
                personal();

                //llama el metodo para llenar el combobox con los proyectos
                proyectos();

                ViewState["nModificar"] = 0;
            }

            if (Session["idPersonal"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        //metod llena el combobox con el personal
        public void personal()
        {
            ddlPersonal.DataSource = negocio.fnMostrarPersonal();
            if (ddlPersonal.SelectedValue == string.Empty)
                ddlPersonal.Items.Add("Sin personal dado de alta");

            //establece el valor del campo del origen de la base de datos
            ddlPersonal.DataValueField = "IdPersonal";

            //obtiene el texto del campo del origen de base de datos
            ddlPersonal.DataTextField = "Nombre";

            //enlaza el origen de base de datos al control
            ddlPersonal.DataBind();
        }

        //metodo llena el combobox con los proyectos
        public void proyectos()
        {
            ddlProyectos.DataSource = negocio.fnMostrarProyectos();

            //establece el valor del campo del origen de la base de datos
            ddlProyectos.DataValueField = "IdProyecto";

            //obtiene el texto del campo del origen de base de datos
            ddlProyectos.DataTextField = "NomProyecto";

            if (ddlProyectos.SelectedValue == string.Empty)
                ddlProyectos.Items.Add("No existen proyectos");

            //enlaza el origen de base de datos al control
            ddlProyectos.DataBind();

        }

        //metodo para llenar el gridview 
        public void tareas()
        {
            //obtiene el origen de la base de datos
            gdvTareas.DataSource = negocio.fnMostrarTareas();

            //enlaza el origen de base de datos al control
            gdvTareas.DataBind();
        }
        protected void btnNuevaTarea_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ViewState["nModificar"]) == 0)
            {
                if (ViewState["IdTarea"] != null)
                {
                    //muestra el control Ajax para confirmar la nueva tarea
                    mdlNuevaTarea.Show();

                    //termina la ejecucion del metodo 
                    return;
                }
                else
                {
                    //muestra el control Ajax aviso
                    mdlAvisoTarea.Show();

                    //termina la ejecucion del metodo 
                    return;
                }
            }
            else
            {
                if (ViewState["IdTarea"] != null)
                {
                    lblmdlAgregarTarea.Text = Resources.Resource_es.lblActualizar;
                    //muestra el control Ajax para confirmar la nueva tarea
                    mdlNuevaTarea.Show();

                    //termina la ejecucion del metodo 
                    return;
                }
                else
                {
                    //muestra el control Ajax aviso
                    mdlAvisoTarea.Show();

                    //termina la ejecucion del metodo 
                    return;
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //llena los campos de los controles con espacio
            txtHoras.Text = "";

        }
        protected void btnOK_Click(object sender, EventArgs e)
        {

            //variables con los valores determinados por los controles
            DateTime fecha = DateTime.Now;
            int horas = Convert.ToInt32(txtHoras.Text);
            int idpersonal = Convert.ToInt32(ddlPersonal.SelectedValue);
            int idproyecto = Convert.ToInt32(ddlProyectos.SelectedValue);
            int idTarea = Convert.ToInt32(ViewState["IdTarea"].ToString());
            int id_estatus = 1;

            if (Convert.ToInt32(ViewState["nModificar"]) == 0)//nuevo
            {
                //la variable logica checa que la tarea no exista dos veces en el mismo personal contiene el resultado de la funcion que resive parametros
                bool tarea_asignada = negocio.fnChecarActividad(idpersonal, idproyecto, idTarea, id_estatus);
                if (tarea_asignada == true)
                {
                    //muestra el control Ajax aviso la tarea ya esta asiganada al personal
                    mdlexiste.Show();
                }
                else
                {
                    //llama una funcion para asignar la tarea
                    negocio.fnAsignaTarea(idproyecto, idpersonal, idTarea, fecha, id_estatus, horas);

                    //muestra el control Ajax aviso
                    mdlAviso.Show();
                }
            }
            else
            {
                bool resp = negocio.fnActualizarTareaAsignada(idproyecto, idpersonal, idTarea, Convert.ToInt32(ViewState["id_estatus"]), horas, Convert.ToInt32(ViewState["IdAsig_Tarea"]));
                if (resp == true)
                {
                    lblmdlAvisoTareaAsig.Text = Resources.Resource_es.lblTareaAcualizada;
                    mdlAviso.Show();
                }
                else
                {
                    lblmdlAvisoTareaAsig.Text = Resources.Resource_es.lblErrorActu;
                    mdlAviso.Show();
                }
                ViewState.Clear();
            }
        }
        protected void gdvTareas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //llama el metodo para llenar el grid
            tareas();

            //la variable de tipo gridviewrow obtiene la fila seleccionada del control
            GridViewRow fila = gdvTareas.SelectedRow;

            //variables obtienen el valor determinado de la fila seleccionada
            ViewState["IdTarea"] = gdvTareas.DataKeys[fila.RowIndex].Values["IdTarea"];

            ViewState["tarea"] = gdvTareas.DataKeys[fila.RowIndex].Values["Tarea"];

            fila.BackColor = Color.Gray;
        }
        private void fnRecorrerGridView()
        {
            for (int i = 0; i < gdvTareas.Rows.Count; i++)
            {
                int idtarea = Convert.ToInt32(gdvTareas.DataKeys[i].Values["IdTarea"]);
                if (idtarea == Convert.ToInt32(ViewState["IdTarea"]))
                    gdvTareas.SelectRow(i);
            }
        }
        protected void btnexisteOk_Click(object sender, EventArgs e)
        {
            //llena los campos de los controles con espacio
            txtHoras.Text = "";

            //llama el metodo para llenar el grid
            tareas();
        }

        protected void btnTareaAsig_Click(object sender, EventArgs e)
        {
            //obtiene el origen de la base de datos
            ddlSelecPersonal.DataSource = negocio.fnMostrarPersonal();
            //establece el valor del campo del origen de la base de datos
            ddlSelecPersonal.DataValueField = "IdPersonal";
            //obtiene el texto del campo del origen de base de datos
            ddlSelecPersonal.DataTextField = "Nombre";
            //enlaza el origen de base de datos al control
            ddlSelecPersonal.DataBind();

            ViewState["IdPersonal"] = ddlSelecPersonal.SelectedValue;

            int IdPersonal = Convert.ToInt32(ViewState["IdPersonal"]);

            //obtiene el origen de la base de datos
            gdvTareasAsignadas.DataSource = negocio.fnMostrarTareasAsignadas(IdPersonal);

            //enlaza el origen de base de datos al control
            gdvTareasAsignadas.DataBind();

            mdlModificarTarea.Show();
        }

        protected void gdvTareasAsignadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //la variable de tipo gridviewrow obtiene la fila seleccionada del control
            GridViewRow fila = gdvTareasAsignadas.SelectedRow;

            ViewState["IdAsig_Tarea"] = Convert.ToInt32(gdvTareasAsignadas.DataKeys[fila.RowIndex].Values["IdAsig_Tarea"]);
            int idproyecto = Convert.ToInt32(gdvTareasAsignadas.DataKeys[fila.RowIndex].Values["IdProyecto"]);
            ddlProyectos.SelectedValue = idproyecto.ToString();
            ddlPersonal.SelectedValue = ddlSelecPersonal.SelectedValue;
            int Horas = Convert.ToInt32(gdvTareasAsignadas.DataKeys[fila.RowIndex].Values["Horas"]);
            ViewState["IdTarea"] = Convert.ToInt32(gdvTareasAsignadas.DataKeys[fila.RowIndex].Values["IdTarea"]);

            ViewState["id_estatus"] = Convert.ToInt32(gdvTareasAsignadas.DataKeys[fila.RowIndex].Values["id_estatus"]);

            txtHoras.Text = Horas.ToString();
            btnNuevaTarea.Text = Resources.Resource_es.lblActualizar; ;
            ViewState["nModificar"] = 1;
            fnRecorrerGridView();
        }

        protected void ddlSelecPersonal_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["IdPersonal"] = ddlSelecPersonal.SelectedValue;

            int IdPersonal = Convert.ToInt32(ViewState["IdPersonal"]);

            //obtiene el origen de la base de datos
            gdvTareasAsignadas.DataSource = negocio.fnMostrarTareasAsignadas(IdPersonal);

            //enlaza el origen de base de datos al control
            gdvTareasAsignadas.DataBind();
            mdlModificarTarea.Show();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //llena los campos de los controles con espacio
            txtHoras.Text = "";

            //llama el metodo para llenar el grid
            tareas();
        }
    }
}
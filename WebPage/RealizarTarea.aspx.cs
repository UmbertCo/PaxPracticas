using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using System.Data;
using System.Drawing;

namespace WebPage
{
    public partial class RealizarTarea : System.Web.UI.Page
    {
        //crea una nueva instancia de la capa logica
        Logica negocio = new Logica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //llama el metodo para llenar el combobox con los proyectos
                proyectos();

                //termina la ejecucion del metodo 
                return;
            }
        }

        //metodo para llenar combobox
        public void proyectos()
        {
            bool bReporte = false;
            int idPersonal = Convert.ToInt32(Session["idpersonal"]);

            //obtiene el origen de la base de datos
            ddlProyectos.DataSource = negocio.fnMostrarProyectosAsignados(idPersonal,bReporte);

            //establece el valor del campo del origen de la base de datos
            ddlProyectos.DataValueField = "IdProyecto";

            //obtiene el texto del campo del origen de base de datos
            ddlProyectos.DataTextField = "NomProyecto";

            //enlaza el origen de base de datos al control
            ddlProyectos.DataBind();


            //variables con los valores determinados por los controles
            ViewState["IdProyecto"] = ddlProyectos.SelectedValue;

            try
            {

                int idproyecto = 0;
                int idpersonal = Convert.ToInt32(Session["idpersonal"]);
                int idAsig_Tarea = Convert.ToInt32(ViewState["IdAsig_Tarea"]);
                int id_estatus = Convert.ToInt32(ViewState["id_estatus"]);
                int IdTarea_realizada = Convert.ToInt32(ViewState["IdTarea_realizada"]);
                if (ViewState["IdProyecto"] == string.Empty)
                    ddlProyectos.Items.Add("Sin proyectos asignados");
                else
                    idproyecto = Convert.ToInt32(ViewState["IdProyecto"]);

                //llama el metodo para llenar el grid que recive parametros
                tareas(idproyecto, idpersonal);
            }
            catch (Exception)
            {

            }

        }

        //metodo para llenar el grid y recive parametros
        private void tareas(int idproyecto, int idpersonal)
        {
            //obtiene el origen de la base de datos
            gdvTareas.DataSource = negocio.fnMostrarTareasAsignadas(idproyecto, idpersonal);

            //enlaza el origen de base de datos al control
            gdvTareas.DataBind();
        }
        protected void ddlProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {

            ViewState["IdProyecto"] = Convert.ToInt32(ddlProyectos.SelectedValue);

            //las variables obtienen el valor de otras varibales 
            int idproyecto = Convert.ToInt32(ViewState["IdProyecto"]);
            int idpersonal = Convert.ToInt32(Session["idpersonal"]);

            tareas(idproyecto, idpersonal);
        }
        public void btninicio_Click(object sender, EventArgs e)
        {
            if (ViewState["IdAsig_Tarea"] == null || ViewState["id_estatus"] == null)
            {
                //muestra el control Ajax aviso
                mdlAviso.Show();

                //termina la ejecucion del metodo 
                return;
            }
            if (ViewState["id_estatus"].ToString() == "1" || ViewState["id_estatus"].ToString() == "3")
            {
                //la variable de tipo gridviewrow obtiene la fila seleccionada del control
                GridViewRow fila = gdvTareas.SelectedRow;

                ViewState["id_estatus"] = "2";

                //las variables obtienen el valor de otras varibales 
                int idproyecto = Convert.ToInt32(ViewState["IdProyecto"]);
                int idpersonal = Convert.ToInt32(Session["idpersonal"]);
                int idtarea = Convert.ToInt32(Session["idTarea"]);
                int id_estatus = Convert.ToInt32(ViewState["id_estatus"]);
                int IdAsig_Tarea = Convert.ToInt32(ViewState["IdAsig_Tarea"]);

                //la variable contiene el resultado de la funcion que recive parametros
                ViewState["IdTarea_realizada"] = negocio.fnIniciaTarea(idproyecto, idpersonal, idtarea, DateTime.Now, id_estatus, IdAsig_Tarea);

                //llama el metodo para llenar el grid que recive parametros
                tareas(idproyecto, idpersonal);

                //pinta la fila del color verde
                fila.BackColor = Color.Gray;
            }
            else
            {
                //el control resive un texto
                lblAviso.Text = Resources.Resource_es.lblmdlNotificacionTareaIn;

                //muestra el control Ajax notifica la tarea
                mdlNotificacion.Show();
            }
        }

        public void gdvTareas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["id_estatus"] == null || ViewState["id_estatus"].ToString() == "1" || ViewState["id_estatus"].ToString() == "3")
            {
                //la variable de tipo gridviewrow obtiene la fila seleccionada del control
                GridViewRow fila = gdvTareas.SelectedRow;

                //variables obtienen el valor determinado de la fila seleccionada
                lblSelNoTarea.Text = gdvTareas.DataKeys[fila.RowIndex].Values["Tarea"].ToString();
                Session["IdAsig_Tarea"] = Convert.ToInt32(gdvTareas.DataKeys[fila.RowIndex].Values["IdAsig_Tarea"]);

                //muestra el control Ajax confirma la tarea 
                mdlConfirmarTarea.Show();

                //pinta la fila del color verde
                fila.BackColor = Color.Gray;

                //termina la ejecucion del metodo 
                return;
            }
            else
            {
                if (ViewState["id_estatus"].ToString() == "2")
                {
                    //la variable resive un texto
                    lblAviso.Text = Resources.Resource_es.lblmdlNotificacionAccionTarea;

                    //muestra el control Ajax notifica la tarea
                    mdlNotificacion.Show();

                    //termina la ejecucion del metodo 
                    return;
                }
            }
        }
        protected void btnpausar_Click(object sender, EventArgs e)
        {
            if (ViewState["IdAsig_Tarea"] == null || ViewState["id_estatus"] == null)
            {
                //muestra el control Ajax notifica la tarea
                mdlAviso.Show();

                //termina la ejecucion del metodo 
                return;
            }
            if (ViewState["id_estatus"].ToString() == "3")
            {
                //la variable resive un texto
                lblAviso.Text = Resources.Resource_es.lblmdlNotificacionTareaPau;

                //muestra el control Ajax notifica la tarea
                mdlNotificacion.Show();

                //termina la ejecucion del metodo 
                return;
            }
            if (ViewState["id_estatus"].ToString() == "1")
            {
                //la variable resive un texto
                lblAviso.Text = Resources.Resource_es.lblmdlNotificacionTareaAviso;

                //muestra el control Ajax notifica la tarea
                mdlNotificacion.Show();
            }
            else
            {
                //las variables obtienen el valor de otras varibales 
                ViewState["btnPausa"] = "btnPausa";
                lblReporte.Text = Resources.Resource_es.lblmdlTituloReporteTareaPausa;
                int id_estatus = Convert.ToInt32(ViewState["id_estatus"]);
                int idTarea = Convert.ToInt32(Session["idTarea"]);

                //la variable resive el valor de la funcion
                ViewState["IdTarea_realizada"] = negocio.fnIdTareaAsignada(id_estatus, idTarea);

                //muestra el control Ajax con caja de texto para el reporte
                mdlReporte.Show();
            }
        }
        protected void btnterminar_Click(object sender, EventArgs e)
        {
            if (ViewState["IdAsig_Tarea"] == null || ViewState["id_estatus"] == null)
            {
                //muestra el control Ajax aviso
                mdlAviso.Show();

                //termina la ejecucion del metodo 
                return;
            }
            if (ViewState["id_estatus"].ToString() == "3" || ViewState["id_estatus"].ToString() == "2")
            {
                //las variables obtienen el valor de otras varibales 
                ViewState["btnTerminar"] = "btnTerminar";
                lblReporte.Text = Resources.Resource_es.lblmdlTituloReporteTareaTer;
                int id_estatus = Convert.ToInt32(ViewState["id_estatus"]);
                int idTarea = Convert.ToInt32(Session["idTarea"]);

                //la variable resive el valor de la funcion
                //ViewState["IdTarea_realizada"] = negocio.fn_regresa_id_tarea_ralizada(id_estatus, idTarea);
                //negocio.fn_terminar_tarea(Convert.ToInt32(ViewState["IdProyecto"]), Convert.ToInt32(Session["idpersonal"]), idTarea, lblReporte.Text, DateTime.Now, id_estatus);

                //muestra el control Ajax con caja de texto para el reporte
                mdlReporte.Show();
            }
            else
            {
                //la variable resive un texto
                lblAviso.Text = Resources.Resource_es.lblmdlNotificacionTareaAviso;

                //muestra el control Ajax notifica la tarea
                mdlNotificacion.Show();
            }

        }
        protected void btnConfirmarSi_Click(object sender, EventArgs e)
        {
            //la variable de tipo gridviewrow obtiene la fila seleccionada del control
            GridViewRow fila = gdvTareas.SelectedRow;

            //variables obtienen el valor determinado de la fila seleccionada
            ViewState["IdAsig_Tarea"] = gdvTareas.DataKeys[fila.RowIndex].Values["IdAsig_Tarea"];
            Session["idTarea"] = gdvTareas.DataKeys[fila.RowIndex].Values["IdTarea"];
            ViewState["id_estatus"] = negocio.fnRecuperaEstatus(Convert.ToInt32(ViewState["IdAsig_Tarea"]));
            lblTareaSel.Text = Resources.Resource_es.lblmdlAvisoTareaSelec + gdvTareas.DataKeys[fila.RowIndex].Values["Tarea"].ToString();

        }
        protected void btnReporte_Click(object sender, EventArgs e)
        {

            if (ViewState["btnPausa"] != null)
            {
                if (ViewState["btnPausa"].ToString() == "btnPausa")
                {
                    //las variables obtienen el valor de otras varibales 
                    string Descripcion = txtReporte.Text;
                    ViewState["id_estatus"] = "3";
                    int idproyecto = Convert.ToInt32(ViewState["IdProyecto"]);
                    int idpersonal = Convert.ToInt32(Session["idpersonal"]);
                    int idTarea = Convert.ToInt32(Session["idTarea"]);
                    int id_estatus = Convert.ToInt32(ViewState["id_estatus"]);
                    int IdAsig_Tarea = Convert.ToInt32(Session["IdAsig_Tarea"]);
                    int IdTarea_realizada = Convert.ToInt32(ViewState["IdTarea_realizada"]);
                    string btnSel = "P";

                    //la funcion actualiza la tarea
                    negocio.fnPausaDeActividad(idproyecto, idpersonal, idTarea, id_estatus, DateTime.Now, Descripcion, IdAsig_Tarea, IdTarea_realizada, btnSel);

                    //llama el metodo que llena el grid
                    tareas(idproyecto, idpersonal);

                    //llena los campos de los controles con espacio
                    txtReporte.Text = "";

                    //limpia la variable especificada 
                    ViewState.Remove("btnPausa");

                    //termina la ejecucion del metodo 
                    return;
                }
            }
            if (ViewState["btnTerminar"] != null)
            {
                if (ViewState["btnTerminar"].ToString() == "btnTerminar")
                {
                    //las variables obtienen el valor de otras varibales 
                    string Descripcion = txtReporte.Text;
                    ViewState["id_estatus"] = "4";
                    int idproyecto = Convert.ToInt32(ViewState["IdProyecto"]);
                    int idpersonal = Convert.ToInt32(Session["idpersonal"]);
                    int idTarea = Convert.ToInt32(Session["idTarea"]);
                    int id_estatus = Convert.ToInt32(ViewState["id_estatus"]);
                    int IdAsig_Tarea = Convert.ToInt32(Session["IdAsig_Tarea"]);
                    int IdTarea_realizada = Convert.ToInt32(ViewState["IdTarea_realizada"]);
                    //string btnSel = "T";

                    //Funcion para terminar la tarea realizada
                    negocio.fnFinalizarActividad(idproyecto, idpersonal, idTarea, txtReporte.Text, DateTime.Now, id_estatus, IdAsig_Tarea);
                    //llama el metodo que llena el grid
                    tareas(idproyecto, idpersonal);

                    //llena los campos de los controles con espacio
                    txtReporte.Text = "";
                    lblSelNoTarea.Text = "";
                    lblTareaSel.Text = "";

                    //limpia la variable especificada 
                    ViewState.Remove("id_estatus");
                    ViewState.Remove("idTarea");
                    ViewState.Remove("btnTerminar");

                    proyectos();

                    //termina la ejecucion del metodo 
                    return;
                }
            }
        }

        protected void btnConfirmarNo_Click(object sender, EventArgs e)
        {
            //las variables obtienen el valor de otras varibales 
            int idproyecto = Convert.ToInt32(ViewState["IdProyecto"]);
            int idpersonal = Convert.ToInt32(Session["idpersonal"]);

            //llama el metodo que llena el grid con las tareas
            tareas(idproyecto, idpersonal);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using System.Text;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data;

namespace PAXActividades
{
    public partial class Reporte : System.Web.UI.Page
    {
        //crea una nueva instancia de la capa logica
        Logica negocio = new Logica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //llama el metodo para llenar un combobox con el personal
                personal();
                chkReporte.Checked = true;
                btnExportar.Visible = false;
                apagado_fechas();
            }
        }
        private void apagado_fechas()
        {
            ltlFechain.Visible = false;
            ltlFechafin.Visible = false;
            txtFechain.Visible = false;
            txtFechafin.Visible = false;
            imgCalendarioin.Visible = false;
            imgcalendariofin.Visible = false;
            lblFormFechaIni.Visible = false;
            lblFormFechaFin.Visible = false;
        }

        //metodo llena el combobox con el personal
        public void personal()
        {
            //obtiene el origen de la base de datos
            ddlpersonal.DataSource = negocio.fnMostrarPersonal();

            //establece el valor del campo del origen de la base de datos
            ddlpersonal.DataValueField = "IdPersonal";

            //obtiene el texto del campo del origen de base de datos
            ddlpersonal.DataTextField = "Nombre";

            //enlaza el origen de base de datos al control
            ddlpersonal.DataBind();

            //llama el metodo que llena el combobox con los proyectos
            proyectos();
        }
        //metodo llena el combobox con los proyectos
        private void proyectos()
        {
            int IdPersonal = 0;

            if (ddlpersonal.SelectedValue == string.Empty)
                ddlpersonal.Items.Add("No ahi personal dado de alta");
            else
                IdPersonal = Convert.ToInt32(ddlpersonal.SelectedValue);

            //obtiene el origen de la base de datos
            ddlProyectos.DataSource = negocio.fnMostrarProyectos();

            //establece el valor del campo del origen de la base de datos
            ddlProyectos.DataValueField = "IdProyecto";

            //obtiene el texto del campo del origen de base de datos
            ddlProyectos.DataTextField = "NomProyecto";

            //enlaza el origen de base de datos al control
            ddlProyectos.DataBind();
        }
        protected void mostrar_Click(object sender, EventArgs e)
        {
            mdlSeleccion.Show();
        }
        protected void ddlpersonal_SelectedIndexChanged(object sender, EventArgs e)
        {
            //llama el metodo para llenar el combobox con los proyectos
            proyectos();

            DataTable tabla = new DataTable();

            //variables reciben el valor del origen de la base de datos
            int IdPersonal = Convert.ToInt32(ddlpersonal.SelectedValue);
            int IdProyecto = Convert.ToInt32(ddlProyectos.SelectedValue);

            //obtiene el origen de la base de datos y el valor de la funcion
            tabla = negocio.fnMostrarReporte(IdPersonal, IdProyecto);

            ViewState["tabla"] = tabla;

            gdvReporte.DataSource = ViewState["tabla"];

            //enlaza el origen de base de datos al control
            gdvReporte.DataBind();
            if (tabla.Rows.Count <= 0)
            {
                lblMensaje.Text = Resources.Resource_es.lblVacio;
                mdlAviso.Show();
                btnExportar.Visible = false;
            }
            else
            {
                btnExportar.Visible = true;
            }

        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            if (ViewState["tabla"] != null)
            {
                DataTable tabla = (DataTable)ViewState["tabla"];

                if (tabla.Rows.Count > 0)
                {
                    dataTableAExcel(tabla);
                }
            }

            lblMensaje.Text = Resources.Resource_es.lblMensaje;
            mdlAviso.Show();
        }
        private void dataTableAExcel(DataTable tabla)
        {
            try
            {
                ScriptManager SM = ScriptManager.GetCurrent(this);
                SM.RegisterPostBackControl(btnExportar);
                if (tabla.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    StringWriter sw = new StringWriter(sb);
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    System.Web.UI.Page pagina = new System.Web.UI.Page();
                    HtmlForm form = new HtmlForm();
                    GridView dg = new GridView();
                    dg.EnableViewState = false;
                    dg.DataSource = tabla;
                    dg.DataBind();
                    pagina.EnableEventValidation = false;
                    pagina.DesignerInitialize();
                    pagina.Controls.Add(form);
                    form.Controls.Add(dg);
                    pagina.RenderControl(htw);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + Guid.NewGuid().ToString() + ".xls");
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = Encoding.Default;
                    Response.Write(sb.ToString());
                    Response.End();
                }
            }
            catch (Exception)
            {

            }
        }
        private void Prender_fechas()
        {
            ltlFechain.Visible = true;
            ltlFechafin.Visible = true;
            txtFechain.Visible = true;
            txtFechafin.Visible = true;
            imgCalendarioin.Visible = true;
            imgcalendariofin.Visible = true;
            lblFormFechaIni.Visible = true;
            lblFormFechaFin.Visible = true;
        }
        protected void chkReporte_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReporte.Checked == false)
            {
                Prender_fechas();
            }
            else
            {
                apagado_fechas();
                txtFechain.Text = "";
                txtFechafin.Text = "";
            }
        }

        protected void btnRerporte_Click(object sender, EventArgs e)
        {
            if (chkReporte.Checked == true)
            {
                DataTable tabla = new DataTable();

                int IdPersonal = 0;
                int IdProyecto = 0;

                if (ddlpersonal.SelectedValue == string.Empty)
                    ddlpersonal.Items.Add("No ahi personal dado de alta");
                else
                    IdPersonal = Convert.ToInt32(ddlpersonal.SelectedValue);

                if (ddlProyectos.SelectedValue != string.Empty)
                    IdProyecto = Convert.ToInt32(ddlProyectos.SelectedValue);

                DateTime fechainicio = DateTime.Now;
                DateTime fechafinal = DateTime.Now;
                string btnSelec = "C";

                //obtiene el origen de la base de datos y el valor de la funcion
                tabla = negocio.fnMostrarReportePersonal(IdPersonal, IdProyecto, fechainicio, fechafinal, btnSelec);

                ViewState["tabla"] = tabla;

                gdvReporte.DataSource = ViewState["tabla"];

                //enlaza el origen de base de datos al control
                gdvReporte.DataBind();
                if (tabla.Rows.Count <= 0)
                {
                    lblMensaje.Text = Resources.Resource_es.lblVacio;
                    mdlAviso.Show();
                    btnExportar.Visible = false;
                }
                else
                {
                    btnExportar.Visible = true;
                }
            }
            else
            {
                DataTable tabla = new DataTable();

                //variables reciben el valor del origen de la base de datos
                int IdPersonal = Convert.ToInt32(ddlpersonal.SelectedValue);
                int IdProyecto = Convert.ToInt32(ddlProyectos.SelectedValue);
                string inicio = txtFechain.Text;
                string fin = txtFechafin.Text;
                if (inicio == "" || fin == "")
                {
                    lblMensaje.Text = Resources.Resource_es.lblSelecFecha;
                    mdlAviso.Show();
                }
                else
                {
                    DateTime fechainicio = Convert.ToDateTime(txtFechain.Text);
                    DateTime fechafinal = Convert.ToDateTime(txtFechafin.Text);
                    string btnSelec = "F";

                    //obtiene el origen de la base de datos y el valor de la funcion
                    tabla = negocio.fnMostrarReportePersonal(IdPersonal, IdProyecto, fechainicio, fechafinal, btnSelec);

                    ViewState["tabla"] = tabla;

                    gdvReporte.DataSource = ViewState["tabla"];

                    //enlaza el origen de base de datos al control
                    gdvReporte.DataBind();
                    if (tabla.Rows.Count <= 0)
                    {
                        lblMensaje.Text = Resources.Resource_es.lblVacio;
                        mdlAviso.Show();
                        btnExportar.Visible = false;
                    }
                    else
                    {
                        btnExportar.Visible = true;
                    }
                }
            }
        }
        protected void ddlProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable tabla = new DataTable();

            //variables reciben el valor del origen de la base de datos
            int IdPersonal = Convert.ToInt32(ddlpersonal.SelectedValue);
            int IdProyecto = Convert.ToInt32(ddlProyectos.SelectedValue);

            //obtiene el origen de la base de datos y el valor de la funcion
            tabla = negocio.fnMostrarReporte(IdPersonal, IdProyecto);

            ViewState["tabla"] = tabla;


            gdvReporte.DataSource = ViewState["tabla"];


            //enlaza el origen de base de datos al control
            gdvReporte.DataBind();
            if (tabla.Rows.Count <= 0)
            {
                lblMensaje.Text = Resources.Resource_es.lblVacio;
                mdlAviso.Show();
                btnExportar.Visible = false;
            }
            else
            {
                btnExportar.Visible = true;
            }
        }
    }
}
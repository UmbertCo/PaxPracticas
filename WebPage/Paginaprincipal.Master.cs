using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Negocio;


namespace WebPage
{
    public partial class Paginaprincipal : System.Web.UI.MasterPage
    {
        //crea una nueva instancia de la capa logica
        Logica negocio = new Logica();

        public void Page_Load(object sender, EventArgs e)
        {

            //el control muestra la fecha del dia 
            lblFecha.Text = DateTime.Now.ToShortDateString();

            if (Session["typo"] != null)
            {
                if (!IsPostBack)
                {
                    //el control contiene el valor de la variable
                    lbltypo.Text = Session["typo"].ToString();

                    //llama el metod donde se muestra el menu
                    ver_menu();
                }

                //el control contiene el valor null
                lbltypo.Text = null;

                //llama el metod donde se muestra el menu
                ver_menu();
            }
        }

        /***Agregamos el Menu dinamico***/
        #region
        //metodo muestra el menu dependiendo del personal
        public void ver_menu()
        {
            //la variable contiene el resultado de la funcion
            DataTable dt = negocio.fnMotrarMenu(lbltypo.Text);
            //recorremos el datatable para agregar los itemn en la cabezera (itemns padres ) 
            foreach (DataRow drmenuItem in dt.Rows)
            {
                //crea una nueva instancia de tipo MenuItem
                MenuItem NuevoMenuItem = new MenuItem();
                //indicamos qué elementos son padres 
                if (drmenuItem["IdMenu"].Equals(drmenuItem["IdPadre"]))
                {

                    NuevoMenuItem.Value = drmenuItem["IdMenu"].ToString();
                    NuevoMenuItem.Text = drmenuItem["Descripcion"].ToString();

                    Menu1.Items.Add(NuevoMenuItem);
                    //llamamos al metodo recursivo encargado de general el arbol 
                    agregarmenu(ref NuevoMenuItem, dt);
                }
            }
        }

        public void agregarmenu(ref MenuItem mmenuItem, DataTable dtmenuItems)
        {
            //recorremos el datatable para definir los items hijos dado el parametro 
            //pasado por referencia
            foreach (DataRow drmenuItem in dtmenuItems.Rows)
            {
                //indicamos qué items son hijos
                if (drmenuItem["IdPadre"].ToString().Equals(mmenuItem.Value) &&
                    !(drmenuItem["IdMenu"].Equals(drmenuItem["IdPadre"])))
                {
                    MenuItem NuevoMenuItem = new MenuItem();
                    NuevoMenuItem.Value = drmenuItem["IdMenu"].ToString();
                    NuevoMenuItem.Text = drmenuItem["Descripcion"].ToString();

                    mmenuItem.ChildItems.Add(NuevoMenuItem);
                    //llamamos recursivamente el metodo para ver si aun tiene items hijos
                    agregarmenu(ref NuevoMenuItem, dtmenuItems);
                }
            }
        }
    #endregion

        protected void Menu1_MenuItemClick1(object sender, MenuEventArgs e)
        {
            switch (Menu1.SelectedItem.Text)
            {
                case "Nuevo Personal":
                    Response.Redirect("NuevoPersonal.aspx");
                    break;
                case "Actualizar Personal":
                    Response.Redirect("ActPersonal.aspx");
                    break;
                case "Nuevo Proyecto":
                    Response.Redirect("NuevoProyecto.aspx");
                    break;
                case "Actualizar Proyecto":
                    Response.Redirect("ActProyecto.aspx");
                    break;
                case "Salir":
                    Session.RemoveAll();
                    Session.Abandon();
                    Response.Redirect("Login.aspx");
                    break;
                case "Establecer Tareas":
                    Response.Redirect("AsignarTarea.aspx");
                    break;
                case "Realizar Tareas":
                    Response.Redirect("RealizarTarea.aspx");
                    break;
                case "Reporte de Tareas":
                    Response.Redirect("Reporte.aspx");
                    break;
                case "Nueva Tarea":
                    Response.Redirect("NuevaTarea.aspx");
                    break;
                case "Actualizar Tarea":
                    Response.Redirect("ActTarea.aspx");
                    break;
                case "Reporte de Actividades":
                    Response.Redirect("ReporteUsuario.aspx");
                    break;
            }
        }
    }
}
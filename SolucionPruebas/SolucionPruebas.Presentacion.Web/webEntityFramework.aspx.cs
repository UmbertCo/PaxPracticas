using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web
{
    public partial class webEntityFramework : System.Web.UI.Page
    {
        private SchoolEntities seContexto;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Initialize the ObjectContext
                seContexto = new SchoolEntities();

                // Define a query that returns all Department  
                // objects and course objects, ordered by name.
                //var departmentQuery = from d in seContexto.Departments.Include("Courses")
                //                      orderby d.Name
                //                      select d;

                var departmentQuery = from d in seContexto.Departments
                                      orderby d.Name
                                      select d;

                try
                {
                    // Bind the ComboBox control to the query, 
                    // which is executed during data binding.
                    // To prevent the query from being executed multiple times during binding, 
                    // it is recommended to bind controls to the result of the Execute method. 
                    ddlListaDepartamentos.DataMember = "Name";
                    ddlListaDepartamentos.DataTextField = "Name";
                    ddlListaDepartamentos.DataValueField = "DepartmentID";
                    ddlListaDepartamentos.DataSource = ((ObjectQuery)departmentQuery).Execute(MergeOption.AppendOnly);
                    ddlListaDepartamentos.DataBind();
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        protected void ddlListaDepartamentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nDepartamento = 0;
            try
            {
                //Get the object for the selected department.
                //EntityDataSource edsCourses = new EntityDataSource();
                //edsCourses.ContextTypeName = "SolucionPruebas.Presentacion.Web.SchoolEntities";
                //edsCourses.EntitySetName = "Courses";
                //edsCourses.AutoGenerateWhereClause = true;
                //edsCourses.WhereParameters.Add("DepartmentID", DbType.Int32, ddlListaDepartamentos.SelectedValue);
                //edsCourses.DataBind();

                seContexto = new SchoolEntities();

                nDepartamento = Convert.ToInt32(ddlListaDepartamentos.SelectedValue);

                // Define a query that returns all Department  
                // objects and course objects, ordered by name.
                var CoursesQuery = from d in seContexto.Courses
                                   where d.DepartmentID == nDepartamento
                                      orderby d.Title
                                      select d;


                //Bind the grid view to the collection of Course objects
                // that are related to the selected Department object.
                gvCursos.DataSource = ((ObjectQuery)CoursesQuery).Execute(MergeOption.AppendOnly);
                gvCursos.DataBind();

                // Hide the columns that are bound to the navigation properties on Course.
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
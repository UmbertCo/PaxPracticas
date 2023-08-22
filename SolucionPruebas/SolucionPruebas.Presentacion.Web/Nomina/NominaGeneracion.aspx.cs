using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web.Nomina
{
    public partial class NominaGeneracion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gdvPagosNomina.DataSource = fnObtenerComprobantePagoNomina();
            gdvPagosNomina.DataBind();

            Session["Id_Nomina"] = 0;
        }

        public DataTable fnObtenerComprobantePagoNomina()
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=192.168.3.106;Initial Catalog=CFDI;Persist Security Info=True;User ID=sa;Password=F4cturax10n"))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_nom_Timbrado_LastNomina_Sel";
                        cmd.Parameters.Add(new SqlParameter("IdEstructura", 824));
                        cmd.Parameters.Add(new SqlParameter("Estatus", "A"));
                        cmd.Parameters.Add(new SqlParameter("IdTipoPeriodo", 3));

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtResultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dtResultado;
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            int nId_Pago_Nomina = 0;
            foreach (GridViewRow renglon in gdvPagosNomina.Rows)
            {
                CheckBox chSeleccion = (CheckBox)renglon.FindControl("cbSeleccion");

                nId_Pago_Nomina = Convert.ToInt32(gdvPagosNomina.DataKeys[renglon.RowIndex].Values["Id_PagoNomina"].ToString());

                clsTimbradoNomina cTimbradoNomina = new clsTimbradoNomina();
                cTimbradoNomina.fnGenerarNomina(nId_Pago_Nomina, 824, DateTime.Now, DateTime.Now,
                        DateTime.Now.AddDays(15), 15);

                if(chSeleccion.Checked)
                {
                    //nId_Pago_Nomina = gdvPagosNomina.SelectedDataKey["Id_PagoNomina"]
                    
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class PruebasWCF_webConsultaProveedores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tablaConsulta = new DataTable();

            if (tablaConsulta.Rows.Count > 0)
            {

            }
        }
        catch
        {
            clsComun.fnMostrarMensaje(this, "No hay respuesta del servidor, intente de nuevo", Resources.resCorpusCFDIEs.varContribuyente);
        }
    }
}
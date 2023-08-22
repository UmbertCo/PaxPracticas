using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PruebasWCF_sebGeneradorPDF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnRecuperar_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
            clsComun.fnMostrarMensaje(this, "No hay respuesta del servidor, intente de nuevo", Resources.resCorpusCFDIEs.varContribuyente);
        }
    }
}
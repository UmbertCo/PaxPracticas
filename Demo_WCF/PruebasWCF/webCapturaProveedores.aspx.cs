using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PruebasWCF_webCapturaProveedores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAlta_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
            clsComun.fnMostrarMensaje(this, "No hay respuesta del servidor, intente de nuevo", Resources.resCorpusCFDIEs.varContribuyente);
        }
    }

    protected void btnBaja_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
            clsComun.fnMostrarMensaje(this, "No hay respuesta del servidor, intente de nuevo", Resources.resCorpusCFDIEs.varContribuyente);
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
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
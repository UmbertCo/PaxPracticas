using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class About : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sOrigen = string.Empty;

        sOrigen = this.Request.QueryString["tpResult"];
        try
        {
            if (sOrigen == "Recupera")
            {
                lblCuentaCreada.Text = Resources.resCorpusCFDIEs.lblCuentaRecuperada;
                lblCuentaDetalle.Text = Resources.resCorpusCFDIEs.lblCuentaRecDetalle;
                lnkHome.NavigateUrl = "~/Account/Login.aspx";
                clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "Recupera" + "|" + "Aviso de usuario de recuperacion de contraseña.");
            }

            if (string.IsNullOrEmpty(sOrigen))
            {
                lblCuentaCreada.Text = Resources.resCorpusCFDIEs.vrMantenimiento;// "Mantenimiento";// Resources.resCorpusCFDIEs.lblCuentaRecuperada;
                lblCuentaDetalle.Text = Resources.resCorpusCFDIEs.lblMantenimiento; //"Disculpe las molestias estamos trabajando";//lblManteniento Resources.resCorpusCFDIEs.lblCuentaRecDetalle;
                imgAviso.Visible = true;
                imgAviso.ImageUrl = "~/Imagenes/Trabajando.jpg";
                lnkHome.NavigateUrl = "~/Account/Login.aspx";

            }
        }
        catch (Exception ex)
        { clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion); }
    }
}

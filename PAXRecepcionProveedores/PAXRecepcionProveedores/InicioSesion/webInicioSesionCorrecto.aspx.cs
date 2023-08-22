using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;

namespace PAXRecepcionProveedores.InicioSesion
{
    public partial class webInicioSesionCorrecto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                string sOrigen = string.Empty;

                sOrigen = this.Request.QueryString["tpResult"];

                if (sOrigen == "Recupera")
                {
                    lblCuentaCreada.Text = Resources.resCorpusCFDIEs.lblCuentaRecuperada;
                    lblCuentaDetalle.Text = Resources.resCorpusCFDIEs.lblCuentaRecDetalle;
                    clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "Recupera" + "|" + "Aviso de usuario de recuperacion de contraseña.");

                }
                if (sOrigen == "Registro")
                {
                    lblCuentaCreada.Text = Resources.resCorpusCFDIEs.lblCuentaCreada;
                    lblCuentaDetalle.Text = Resources.resCorpusCFDIEs.lblCuentaDetalle;
                    clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "Registro" + "|" + "Aviso a usuario de registro completado.");

                }
                if (sOrigen == "Reactivar")
                {
                    lblCuentaCreada.Text = Resources.resCorpusCFDIEs.lblReactivar;
                    lblCuentaDetalle.Text = Resources.resCorpusCFDIEs.lblCuentaRacDetalle;
                    clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "Reactivar" + "|" + "Aviso de usuario de reactivacion de contraseña.");

                }

                if (sOrigen == "Soporte")
                {
                    lblCuentaCreada.Text = Resources.resCorpusCFDIEs.lblTitSoporte;
                    lblCuentaDetalle.Text = Resources.resCorpusCFDIEs.lblDescSoporte;

                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/webGlobalError.aspx");
            }
        }
        protected override void InitializeCulture()
        {
            if (Session["Culture"] != null)
            {
                string lang = Session["Culture"].ToString();
                if ((lang != null) || (lang != ""))
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                }
            }
            else
            {
                string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            }
        }
    }
}
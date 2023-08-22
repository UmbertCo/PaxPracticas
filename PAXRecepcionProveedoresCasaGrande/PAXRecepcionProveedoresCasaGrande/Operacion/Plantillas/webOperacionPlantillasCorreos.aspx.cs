using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Globalization;

public partial class Operacion_Plantillas_webOperacionPlantillasCorreos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            clsInicioSesionUsuario sesUsuario = clsComun.fnUsuarioEnSesion();
            clsOperacionUsuarios oOpUsuarios = new clsOperacionUsuarios();
            int nId_Usuario = sesUsuario.id_usuario;
            if (nId_Usuario > 0)
            {
                DataTable tblModulosPerfil = oOpUsuarios.fnSeleccionaModulosHijo(sesUsuario.Id_perfil, true);
                string[] urlActual = Request.Url.AbsolutePath.Split('/');
                int encontrado = tblModulosPerfil.AsEnumerable().Where(t => t.Field<string>("modulo").Contains(urlActual[urlActual.Length - 1])).Count();
                if (encontrado < 1)
                    Response.Redirect("~/Default.aspx", false);
            }
            if (!IsPostBack)
            {
                fnLlenarPlantillas();
                ddlPlantillas_SelectedIndexChanged(null, null);
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/webGlobalError.aspx");
        }
    }

    private void fnLlenarPlantillas()
    {
        ddlPlantillas.Items.Clear();
        ddlPlantillas.Items.Add(new ListItem("Acuse Pago", "acuse_pago"));
        ddlPlantillas.Items.Add(new ListItem("Acuse Validación", "acuse_validacion"));
        ddlPlantillas.Items.Add(new ListItem("Alta Proveedor", "alta_proveedor"));
        ddlPlantillas.DataBind();
    }
    protected void ddlPlantillas_SelectedIndexChanged(object sender, EventArgs e)
    {
        clsPlantillaCorreo plantilla = new clsPlantillaCorreo();
        string sMensaje = string.Empty;
        DataTable dtEjemplo = new DataTable();
        switch (ddlPlantillas.SelectedIndex)
        {
            case 0:
                plantilla = new clsPlantillaCorreo(clsPlantillaCorreo.Tipo.AcusePago);
                dtEjemplo = plantilla.fnObtenerTablaEjemplo(clsPlantillaCorreo.Tipo.AcusePago);
                sMensaje = plantilla.fnObtenerMensajeAcusePago(dtEjemplo,
                            (plantilla.logoImagen != null && plantilla.logoImagen.Length > 0),
                        (plantilla.firmaImagen != null && plantilla.firmaImagen.Length > 0));
                break;
            case 1:
                plantilla = new clsPlantillaCorreo(clsPlantillaCorreo.Tipo.AcuseValidacion);
                dtEjemplo = plantilla.fnObtenerTablaEjemplo(clsPlantillaCorreo.Tipo.AcuseValidacion);
                sMensaje = plantilla.fnObtenerMensajeAcuseValidacion(dtEjemplo,
                            (plantilla.logoImagen != null && plantilla.logoImagen.Length > 0),
                        (plantilla.firmaImagen != null && plantilla.firmaImagen.Length > 0));
                break;
            case 2:
                plantilla = new clsPlantillaCorreo(clsPlantillaCorreo.Tipo.AltaProveedor);
                sMensaje = plantilla.fnObtenerMensajeAltaProveedor("Nombre Ejemplo", "usuario.ejemplo", "passwordEjemplo",
                            (plantilla.logoImagen != null && plantilla.logoImagen.Length > 0),
                        (plantilla.firmaImagen != null && plantilla.firmaImagen.Length > 0));
                break;
        }
        lblAsuntoVista.Text = plantilla.asunto;
        sMensaje = sMensaje.Replace("<img src=cid:imgLogo>", "");
        sMensaje = sMensaje.Replace("<img src=cid:imgFirma>", "");
        ltPlantilla.Text = sMensaje;
        if (plantilla.logoImagen != null && plantilla.logoImagen.Length > 0)
        {
            imgLogo.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(plantilla.logoImagen);
            imgLogo.Visible = true;
        }
        else
            imgLogo.Visible = false;
        if (plantilla.firmaImagen != null && plantilla.firmaImagen.Length > 0)
        {
            imgFirma.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(plantilla.firmaImagen);
            imgFirma.Visible = true;
        }
        else
            imgFirma.Visible = false;

    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        clsPlantillaCorreo plantilla = new clsPlantillaCorreo();
        clsPlantillaCorreo.Tipo tipo = clsPlantillaCorreo.Tipo.AcusePago;
        switch (ddlPlantillas.SelectedIndex)
        {
            case 0:
                tipo = clsPlantillaCorreo.Tipo.AcusePago;
                plantilla = new clsPlantillaCorreo(clsPlantillaCorreo.Tipo.AcusePago);
                break;
            case 1:
                tipo = clsPlantillaCorreo.Tipo.AcuseValidacion;
                plantilla = new clsPlantillaCorreo(clsPlantillaCorreo.Tipo.AcuseValidacion);
                break;
            case 2:
                tipo = clsPlantillaCorreo.Tipo.AltaProveedor;
                plantilla = new clsPlantillaCorreo(clsPlantillaCorreo.Tipo.AltaProveedor);
                break;
        }
        if (btnEditar.Text == Resources.resCorpusCFDIEs.lblEditar)
        {


            txtAsunto.Text = plantilla.asunto;
            txtColorEncabezado.Text = plantilla.colorTablaEncabezado;
            if (!string.IsNullOrEmpty(plantilla.colorTablaEncabezado))
                lblColorMuestra.BackColor = Color.FromName(plantilla.colorTablaEncabezado);
            if (!string.IsNullOrEmpty(plantilla.mensaje))
                txtMensaje.Text = plantilla.mensaje.Replace("<br/>", "\n");
            if (!string.IsNullOrEmpty(plantilla.firma))
                txtFirma.Text = plantilla.firma.Replace("<br/>", "\n");
            txtAsunto.Enabled = true;
            txtColorEncabezado.Enabled = true;
            fupImagenFirma.Enabled = true;
            fupImagenLogo.Enabled = true;
            txtFirma.Enabled = true;
            txtMensaje.Enabled = true;
            cpeColorEncabezado.Enabled = true;
            btnEditar.Text = Resources.resCorpusCFDIEs.btnGuardar;
            ddlPlantillas.Enabled = false;
            cbFirmaEliminar.Checked = false;
            cbLogoEliminar.Checked = false;
            cbFirmaEliminar.Enabled = true;
            cbLogoEliminar.Enabled = true;
        }
        else
        {
            try
            {
                byte[] bLogoUrl = null;
                byte[] bFirmaUrl = null;
                if (!cbFirmaEliminar.Checked)
                {
                    if (fupImagenFirma.HasFile)
                    {
                        if (Path.GetExtension(fupImagenFirma.PostedFile.FileName).ToLower() != ".jpg"
                            && Path.GetExtension(fupImagenFirma.PostedFile.FileName).ToLower() != ".png")
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varTipoImagen);
                            return;
                        }

                        double nTamanioArchivo = fupImagenFirma.PostedFile.ContentLength / 1024;
                        int nTamanioMax = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
                        if (nTamanioArchivo > nTamanioMax)
                        {
                            clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.VarErrorLogo));
                            return;
                        }
                        bFirmaUrl = fupImagenFirma.FileBytes;
                    }
                }
                else
                {
                    bFirmaUrl = new byte[0];
                }
                if (!cbLogoEliminar.Checked)
                {
                    if (fupImagenLogo.HasFile)
                    {
                        if (Path.GetExtension(fupImagenLogo.PostedFile.FileName).ToLower() != ".jpg"
                            && Path.GetExtension(fupImagenLogo.PostedFile.FileName).ToLower() != ".png")
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varTipoImagen);
                            return;
                        }

                        double nTamanioArchivo = fupImagenLogo.PostedFile.ContentLength / 1024;
                        int nTamanioMax = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
                        if (nTamanioArchivo > nTamanioMax)
                        {
                            clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.VarErrorLogo));
                            return;
                        }
                        bLogoUrl = fupImagenLogo.FileBytes;
                    }
                }
                else
                {
                    bLogoUrl = new byte[0];
                }

                string sColor = txtColorEncabezado.Text;
                if (!sColor.StartsWith("#"))
                    sColor = "#" + sColor;
                new clsPlantillaCorreo().fnGuardarPlantilla(tipo, txtAsunto.Text,
                    txtMensaje.Text.Replace("\n", "<br/>"), txtFirma.Text.Replace("\n", "<br/>"), sColor,
                    "", "", bLogoUrl, bFirmaUrl);
                txtAsunto.Text = "";
                txtColorEncabezado.Text = "";
                lblColorEncabezado.BackColor = Color.Empty;
                txtMensaje.Text = "";
                txtFirma.Text = "";
                txtAsunto.Enabled = false;
                txtColorEncabezado.Enabled = false;
                fupImagenFirma.Enabled = false;
                fupImagenLogo.Enabled = false;
                txtFirma.Enabled = false;
                txtMensaje.Enabled = false;
                cpeColorEncabezado.Enabled = false;
                btnEditar.Text = Resources.resCorpusCFDIEs.lblEditar;
                ddlPlantillas.Enabled = true;
                cbFirmaEliminar.Checked = false;
                cbLogoEliminar.Checked = false;
                cbFirmaEliminar.Enabled = false;
                cbLogoEliminar.Enabled = false;
                ddlPlantillas_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {

            }
        }
    }

    protected void btnNCancelar_Click(object sender, EventArgs e)
    {
        txtAsunto.Text = "";
        txtColorEncabezado.Text = "";
        lblColorEncabezado.BackColor = Color.White;
        txtMensaje.Text = "";
        txtFirma.Text = "";
        txtAsunto.Enabled = false;
        txtColorEncabezado.Enabled = false;
        fupImagenFirma.Enabled = false;
        fupImagenLogo.Enabled = false;
        txtFirma.Enabled = false;
        txtMensaje.Enabled = false;
        cpeColorEncabezado.Enabled = false;
        btnEditar.Text = Resources.resCorpusCFDIEs.lblEditar;
        ddlPlantillas.Enabled = true;
        cbFirmaEliminar.Checked = false;
        cbLogoEliminar.Checked = false;
        cbFirmaEliminar.Enabled = false;
        cbLogoEliminar.Enabled = false;
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
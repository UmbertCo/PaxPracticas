using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webGlobalCreditos : System.Web.UI.Page
{
    private static DataSet creditos;
    clsInicioSesionUsuario datosUsuario = new clsInicioSesionUsuario();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            if (datosUsuario == null)
                return;

            if (!fnRevisaAutorizacion())
            {
                Exception myException = new Exception(string.Format("El usuario {0} no tiene los suficientes permisos para accesar a la página {1}", datosUsuario.userName, "webGlobalCreditos.aspx")) { Source = "webGlobalCreditos.aspx|Page_Load" };
                clsErrorLog.fnNuevaEntrada(myException, clsErrorLog.TipoErroresLog.Datos);
                modalCreditos.Show();
            }

            TabContainer1.ActiveTabIndex = 0;
            fnRecuperaServicios();
        }
    }
    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            clsErrorLog.fnNuevaEntrada(objErr, clsErrorLog.TipoErroresLog.Datos);
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx");
        }
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {

        DataTable tblUsuario = new DataTable();
        DataTable tblCreditos = new DataTable();
        DataTable tblServicios = new DataTable();

       
        try
        {
            creditos = fnRecuperaCreditosusuario(txtUsuario.Text);
            if (creditos.Tables[0].Rows.Count > 0)
            {
                txtUsuario0.Text = txtUsuario.Text;
                btnAgregar.Enabled = true;
            }

            tblUsuario = creditos.Tables[0];
            tblCreditos = creditos.Tables[1];
            tblServicios = creditos.Tables[2];

            if(tblCreditos.Rows.Count > 0)
            {
                string CreditosS = tblCreditos.Rows[0]["creditos"].ToString();          
                lblValCred.Text = CreditosS;
            }
            else
            {
                lblValCred.Text = "0";
            }
            grdServicios.DataSource = tblServicios;
            grdServicios.DataBind();


            for (int i = 0; i < cbServicios.Items.Count; i++)
            {              
                    cbServicios.Items[i].Selected = false;              
            }

            grdServiciosAsig.DataSource = tblServicios;
            grdServiciosAsig.DataBind();

            for (int i = 0; i < cbServicios.Items.Count; i++)
            {
                foreach (DataRow renglon in tblServicios.Rows)
                {
                    string Descripcion = Convert.ToString(renglon["descripcion"]);
                    string Servicio = Convert.ToString(cbServicios.Items[i].Text);
                    if (Descripcion == Servicio)
                    {
                        cbServicios.Items[i].Selected = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //lblValCred.Text = "No hay creditos para este usuario.";
            grdServicios.DataSource = null;
            grdServicios.DataBind();
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void btnAgregar_Click(object sender, EventArgs e)
    {

        DataTable tblUsuario = new DataTable();
        DataTable tblCreditos = new DataTable();
        DataTable tblServicios = new DataTable();

        creditos = fnRecuperaCreditosusuario(txtUsuario.Text);

        try
        {
            if (!fnRevisaAutorizacion())
            {
                Exception myException = new Exception(string.Format("El usuario {0} no tiene los suficientes permisos para accesar a la página {1}", datosUsuario.userName, "webGlobalCreditos.aspx")) { Source = "webGlobalCreditos.aspx|btnAgregar_Click" };
                clsErrorLog.fnNuevaEntrada(myException, clsErrorLog.TipoErroresLog.Datos);
                modalCreditos.Show();
                return;
            }

            clsConfiguracionCreditos Creditos = new clsConfiguracionCreditos();

            tblUsuario = creditos.Tables[0];
            tblCreditos = creditos.Tables[1];
            tblServicios = creditos.Tables[2];
            int idEstructura = Convert.ToInt32(tblUsuario.Rows[0]["estructura"]);


            foreach (GridViewRow renglon in grdServiciosAsig.Rows)
            {
                double sPrecio = 0;
                TextBox tbCalculo = (TextBox)renglon.Cells[3].Controls[1];
                TextBox tbPrecioUnit = (TextBox)renglon.Cells[4].Controls[1];
                int tbServicio = Convert.ToInt32(renglon.Cells[0].Text);
                if (tbCalculo.Text != "")
                {
                    string Servicios = string.Empty;

                    if (tbPrecioUnit.Text != "")
                    {
                        sPrecio = Convert.ToDouble(tbPrecioUnit.Text);
                    }

                    double Precio = Creditos.fnRecuperaPrecioServicio(tbServicio);
                    double sCreditos = Convert.ToDouble(tbCalculo.Text) * Precio;
                    if (Precio > 0 && sCreditos > 0)
                    {
                        Creditos.fnActualizaCreditos(idEstructura, sCreditos, 'A', DateTime.Now, sPrecio, Convert.ToString(tbServicio));
                    }
                    else
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblMsgCreditosNeg);
                        return;
                    }
                }

            }

            txtCreditos.Text = string.Empty;

            btnBuscar_Click(sender, e);

            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);


        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void btnActualiza_Click(object sender, EventArgs e)
    {
        try
        {

            DataSet creditos2 = new DataSet();
            DataTable tblUsuario = new DataTable();
            DataTable tblCreditos = new DataTable();
            DataTable tblServicios = new DataTable();

            creditos2 = fnRecuperaCreditosusuario(txtUsuario.Text);

            tblUsuario = creditos2.Tables[0];
            tblCreditos = creditos2.Tables[1];
            tblServicios = creditos2.Tables[2];

            clsConfiguracionCreditos Creditos = new clsConfiguracionCreditos();

            int idEstructura = Convert.ToInt32(tblUsuario.Rows[0]["estructura"]);
            string Servicios = string.Empty;


            for (int i = 0; i < cbServicios.Items.Count; i++)
            {
                if (cbServicios.Items[i].Selected == true)
                {
                    if (Servicios == string.Empty)
                        Servicios = cbServicios.Items[i].Value;
                    else
                        Servicios = Servicios + "," + cbServicios.Items[i].Value;
                }
            }
            if (Servicios == string.Empty)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varServicios);
                return;
            }

            Creditos.fnActualizaServicios(idEstructura, Servicios);

            btnBuscar_Click(sender, e);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void txtPrecio_TextChanged(object sender, EventArgs e)
    {
        fnCalcularCreditos();
    }
    protected void tbImporte_TextChanged(object sender, EventArgs e)
    {
        fnCalcularCreditos();
    }
    protected void btnBuscarUsu_Click(object sender, EventArgs e)
    {
        DataTable tblUsuario = new DataTable();
        tblUsuario = fnBuscarUsuarios(txtBuscarUsu.Text);
        if (tblUsuario.Rows.Count > 0)
        {
            grdBusqueda.DataSource = tblUsuario;
            grdBusqueda.DataBind();
        }
    }
    protected void btnAcepCreditos_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx", false);
    }   

    /// <summary>
    /// Loads the language specific resources
    /// </summary>
    protected override void InitializeCulture()
    {
        if (Session["Culture"] != null)
        {
            string lang = Session["Culture"].ToString();
            if ((lang != null) || (lang != ""))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
            }
        }
        else
        {
            string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }
    }

    public DataTable fnBuscarUsuarios(string sClaveUsuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Buscar_Usuario_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", sClaveUsuario));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;
    }

    public void fnCalcularCreditos()
    {
        try
        {
            clsConfiguracionCreditos Creditos = new clsConfiguracionCreditos();
            double Precio = 0;
            double Operacion = 0;
            string Servicios = string.Empty;
            txtCreditos.Text = "";
            foreach (GridViewRow renglon in grdServiciosAsig.Rows)
            {

                TextBox tbCalculo = (TextBox)renglon.Cells[3].Controls[1];
                //GridViewRow row = ((TextBox)sender).Parent.Parent as GridViewRow;
                Label tbImporte = (Label)renglon.Cells[5].Controls[1]; ;
                int pIdServicio = Convert.ToInt32(renglon.Cells[0].Text);
                Precio = Creditos.fnRecuperaPrecioServicio(pIdServicio);
                if (tbCalculo.Text != "")
                {
                    Operacion = Convert.ToDouble(tbCalculo.Text) * Precio;
                    tbImporte.Text = Operacion.ToString();
                    if (txtCreditos.Text != "")
                    {
                        double creditos = Convert.ToDouble(txtCreditos.Text);
                        double calculo = creditos + Operacion;
                        txtCreditos.Text = Convert.ToString(calculo);
                    }
                    else
                    {
                        txtCreditos.Text = Convert.ToString(Operacion);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    /// <summary>
    /// Función que se encarga de revisar si existe una autorización para revisar un modulo del usuario logueado
    /// </summary>
    /// <param name="pnId_Usuario">ID del Usuario</param>
    /// <param name="pnId_Modulo">ID del Modulo</param>
    /// <returns></returns>
    public bool fnExisteAutorizacion(int pnId_Usuario, int pnId_Modulo)
    {
        bool bResultado = false;
        int nId_Autorizacion_Usuario = 0;
        try
        {
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ses_Autorizacion_Usuarios_Modulo_ExisteIdUsuarioIdModulo_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", pnId_Usuario));
                    cmd.Parameters.Add(new SqlParameter("nId_Modulo", pnId_Modulo));

                    nId_Autorizacion_Usuario = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            if (nId_Autorizacion_Usuario > 0)
                bResultado = true;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return bResultado;
    }

    private DataSet fnRecuperaCreditosusuario(string clave_usuario)
    {

        DataSet creditos = new DataSet();
        clsConfiguracionCreditos Creditos = new clsConfiguracionCreditos();
        try
        {
            creditos = Creditos.fnRecuperaCreditos(clave_usuario);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

        return creditos;
    }

    public void fnRecuperaServicios()
    {
        try
        {
            DataSet creditos = new DataSet();
            clsConfiguracionCreditos Creditos = new clsConfiguracionCreditos();
            DataTable dtServicios = Creditos.fnObtieneServicios();
            cbServicios.DataSource = dtServicios;
            cbServicios.DataTextField = "descripcion";
            cbServicios.DataValueField = "id_servicios";
            cbServicios.DataBind();

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    private bool fnRevisaAutorizacion()
    {
        bool bAutorizacion = false;
        datosUsuario = clsComun.fnUsuarioEnSesion();

        bAutorizacion = fnExisteAutorizacion(datosUsuario.id_usuario, 40);

        return bAutorizacion;
    }
}
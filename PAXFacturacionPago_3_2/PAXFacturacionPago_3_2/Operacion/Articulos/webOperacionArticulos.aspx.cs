using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Globalization;

public partial class Operacion_Articulos_webOperacionArticulos : System.Web.UI.Page
{
    clsInicioSesionUsuario datosUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            clsComun.fnPonerTitulo(this);
            if (!IsPostBack)
            {
                fnCargarSucursalesEmisor();
                if (ddlSucursales.Items.Count > 0)
                {
                    fnCargaArticulos(Convert.ToInt32(ddlSucursales.SelectedValue));
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblErrorEstructura, Resources.resCorpusCFDIEs.lblContribuyente);

                   txtDescripcion.Enabled = false;
                   txtMedida.Enabled = false;
                   txtPrecio.Enabled = false;
                   txtIVACon.Enabled = false;
                   //txtIVA.Enabled = false;
                   txtIEPS.Enabled = false;
                   txtISR.Enabled = false;
                   txtIVARet.Enabled = false;
                   ddlMoneda.Enabled = false;
           
                  
                   btnNuevoCorreo.Enabled = false;
                   btnExcel.Enabled = false;
                   btnGuardar.Enabled = false;
                }
            }
            //fnCargaArticulos(Convert.ToInt32(ddlSucursales.SelectedValue));
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    /// <summary>
    /// Trae la lista filtrada de las sucursales de los emisores.
    /// </summary>
    private void fnCargarSucursalesEmisor()
    {
        try
        {

            datosUsuario = clsComun.fnUsuarioEnSesion();
            ddlSucursales.DataSource = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario);
            ddlSucursales.DataBind();
            //fnCargaArticulos(Convert.ToInt32(ddlSucursales.SelectedValue));
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlSucursales.DataSource = null;
            ddlSucursales.DataBind();
        }
        catch
        {
            //referencia nula
        }
    }
    protected void btnNuevoUsuario_Click(object sender, EventArgs e)
    {
        txtDescripcion.Text = string.Empty;
        txtIEPS.SelectedValue = "0";
        txtISR.Text = string.Empty;
        //txtIVA.Text = string.Empty;
        txtIVACon.SelectedIndex = 0; 
        txtIVARet.Text = string.Empty;
        txtMedida.Text = string.Empty;
        txtPrecio.Text = string.Empty;
        txtClave.Text = string.Empty;
        btnGuardar.Enabled = true;
        Session["psIdArticulo"] = null;
        //txtISH.Text = string.Empty;
        //txtCNG.Text = string.Empty;
        ddlMoneda.SelectedIndex = 0;
        //fnCargarSucursalesEmisor();
        
        txtDescripcion.Enabled = true;
        txtIEPS.Enabled = true;
        txtISR.Enabled = true;
        //txtIVA.Enabled = true;
        txtIVACon.Enabled = true; 
        txtClave.Enabled = true;
        txtIVARet.Enabled = true;
        txtMedida.Enabled = true;
        txtPrecio.Enabled = true;
        //txtISH.Enabled = true;
        //txtCNG.Enabled = true;
        ddlMoneda.Enabled = true;
       
        gdvArticulos.SelectedIndex = -1;
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            clsOperacionArticulos gDAL = new clsOperacionArticulos();
             if ((Session["psIdArticulo"] == null))               
             {
                
                double Precio = Convert.ToDouble(txtPrecio.Text);
                string IVA = null;
                if (txtIVACon.SelectedValue == "Exento")
                    IVA = null; //Excento
                else
                    IVA = txtIVACon.SelectedValue;

                double IEPS, ISR, IVARet, ISH, CNG;
                IEPS = ISR = IVARet = ISH = CNG = 0;

                 if(!(txtIEPS.SelectedValue == "0"))
                 {
                  IEPS = Convert.ToDouble(txtIEPS.SelectedValue);
                 }
                 if (!(txtISR.Text == string.Empty))
                 {
                     ISR = Convert.ToDouble(txtISR.Text);
                 }
                 if (!(txtIVARet.Text == string.Empty))
                 {
                     IVARet = Convert.ToDouble(txtIVARet.Text);
                 }
                  
                 //if (!(txtISH.Text == string.Empty))
                 //    ISH = Convert.ToDouble(txtISH.Text);

                 //if (!(txtCNG.Text == string.Empty))
                 //    CNG = Convert.ToDouble(txtCNG.Text);

                 gDAL.fnUpdateArticulo(0, txtDescripcion.Text, txtMedida.Text, Precio, IVA, IEPS, ISR, IVARet, ddlMoneda.SelectedValue,
                    "A", Convert.ToInt32(ddlSucursales.SelectedValue), txtClave.Text);
                 fnCargaArticulos(Convert.ToInt32(ddlSucursales.SelectedValue));

            }
            else
            {
                      int psIdArticulo = Convert.ToInt32(Session["psIdArticulo"]);
                      double Precio = Convert.ToDouble(txtPrecio.Text);
                      string IVA = null;
                      if (txtIVACon.SelectedValue == "Exento")
                          IVA = null; //Excento
                      else
                          IVA =  txtIVACon.SelectedValue;

                      double IEPS, ISR, IVARet, ISH, CNG;
                      IEPS = ISR = IVARet = ISH = CNG = 0;
                      
                      if (!(txtIEPS.SelectedValue == "0"))
                      {
                          IEPS = Convert.ToDouble(txtIEPS.SelectedValue);
                      }
                      if (!(txtISR.Text == string.Empty))
                      {
                          ISR = Convert.ToDouble(txtISR.Text);
                      }
                      if (!(txtIVARet.Text == string.Empty))
                      {
                          IVARet = Convert.ToDouble(txtIVARet.Text);
                      }

                 //if (!(txtISH.Text == string.Empty))
                 //    ISH = Convert.ToDouble(txtISH.Text);

                 //if (!(txtCNG.Text == string.Empty))
                 //    CNG = Convert.ToDouble(txtCNG.Text);

                      gDAL.fnUpdateArticulo(psIdArticulo, txtDescripcion.Text, txtMedida.Text, Precio, IVA, IEPS, ISR, IVARet, ddlMoneda.SelectedValue,
                         "A", Convert.ToInt32(ddlSucursales.SelectedValue),txtClave.Text);
                      fnCargaArticulos(Convert.ToInt32(ddlSucursales.SelectedValue));
                      
             }
             btnNuevoUsuario_Click(sender, e);
             //btnNCancelar_Click(sender, e);
             gdvArticulos.SelectedIndex = -1;

             ddlMoneda.SelectedIndex = 0;
             btnGuardar.Enabled = false;
             //fnCargarSucursalesEmisor();

             txtDescripcion.Enabled = false;
             txtIEPS.Enabled = false;
             txtISR.Enabled = false;
             txtIVACon.Enabled = false;
             //txtIVA.Enabled = false;
             txtIVARet.Enabled = false;
             txtMedida.Enabled = false;
             txtPrecio.Enabled = false;
             txtClave.Enabled = false;
             ddlMoneda.Enabled = false;
             //txtISH.Enabled = false;
             //txtCNG.Enabled = false;
             btnGuardar.Enabled = false;

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {

    }
    protected void gdvArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gdvArticulos.PageIndex = e.NewPageIndex;
            fnCargaArticulos(Convert.ToInt32(ddlSucursales.SelectedValue));
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    private void fnCargaArticulos(int pidEstructura)
    {
        try
        {
            clsOperacionArticulos gDAL = new clsOperacionArticulos();
            gdvArticulos.DataSource = gDAL.fnObtieneArticulosEstructura(pidEstructura);
            gdvArticulos.DataBind();
            gdvArticulos.Enabled = true;
            ViewState["dtTabla"] = gdvArticulos.DataSource;

        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void gdvArticulos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            //cancelamos la acción por defecto
            e.Cancel = false;
            //Obtenemos el ID de la fila seleccionada
            int psidArticulo = Convert.ToInt32(e.Keys["id_articulo"].ToString());
            clsOperacionArticulos gDAL = new clsOperacionArticulos();
            gDAL.fnBajaArticulo(psidArticulo);
            fnCargaArticulos(Convert.ToInt32(ddlSucursales.SelectedValue));
        }
        catch (Exception ex)
        {
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        
    }
    protected void gdvArticulos_SelectedIndexChanged(object sender, EventArgs e)
    {
        clsOperacionArticulos gDAL = new clsOperacionArticulos();
        GridViewRow gvrFila = (GridViewRow)gdvArticulos.SelectedRow;
        Label idartuculo = (Label)gvrFila.FindControl("lbleidArticulo");
        Session["psIdArticulo"] = Convert.ToInt32(idartuculo.Text);
        DataTable dtInfo = new DataTable();
        dtInfo = gDAL.fnObtieneArticulo(Convert.ToInt32(Convert.ToInt32(idartuculo.Text)));
        foreach (DataRow renglon in dtInfo.Rows)
        {
            try
            {
                txtDescripcion.Text = Convert.ToString(renglon["descripcion"]);
                txtMedida.Text = Convert.ToString(renglon["medida"]);
                txtPrecio.Text = Convert.ToString(renglon["precio"]);
                if (!(Convert.ToString(renglon["iva"]) == string.Empty))
                    txtIVACon.SelectedValue = Convert.ToString(renglon["iva"]);
                else
                    txtIVACon.SelectedValue = "Exento";

                //txtIVA.Text = Convert.ToString(renglon["iva"]);
                if (Convert.ToDouble(renglon["ieps"]) != 0)
                    txtIEPS.SelectedValue = string.Format("{0:n6}", Convert.ToDouble(renglon["ieps"]));
                else
                    txtIEPS.SelectedValue = "0";
                txtISR.Text = Convert.ToString(renglon["isr"]);
                txtIVARet.Text = Convert.ToString(renglon["ivaretenido"]);
                ddlMoneda.SelectedValue = Convert.ToString(renglon["moneda"]);
                txtClave.Text = Convert.ToString(renglon["clave"]);
                ddlSucursales.SelectedValue = Convert.ToString(renglon["id_estructura"]);
                //txtISH.Text = Convert.ToString(renglon["ish"]);
                //txtCNG.Text = Convert.ToString(renglon["cargosNoGravables"]);
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            }
          
            btnGuardar.Enabled = true;

            txtDescripcion.Enabled = true;
            txtIEPS.Enabled = true;
            txtISR.Enabled = true;
            txtIVACon.Enabled = true; 
            //txtIVA.Enabled = true;
            txtIVARet.Enabled = true;
            txtMedida.Enabled = true;
            txtPrecio.Enabled = true;
            txtClave.Enabled = true;
            ddlMoneda.Enabled = true;
            //txtISH.Enabled = true;
            //txtCNG.Enabled = true;
           
        }
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
    protected void ddlSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargaArticulos(Convert.ToInt32(ddlSucursales.SelectedValue));
    }
    protected void btnNCancelar_Click(object sender, EventArgs e)
    {
        gdvArticulos.SelectedIndex = -1;
        btnNuevoUsuario_Click(sender, e);
        txtDescripcion.Enabled = false;
        txtIEPS.Enabled = false;
        txtISR.Enabled = false;
        txtIVACon.Enabled = false;
        //txtIVA.Enabled = false;
        txtIVARet.Enabled = false;
        txtMedida.Enabled = false;
        txtPrecio.Enabled = false;
        ddlMoneda.Enabled = false;
        txtClave.Enabled = false;
        //txtISH.Enabled = false;
        //txtCNG.Enabled = false;
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
            {
                ViewState["sortDirection"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["sortDirection"];
        }
        set 
        {
            ViewState["sortDirection"] = value; 
        }
    }

    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = null;
        dt = (DataTable)ViewState["dtTabla"];
        System.Data.DataView dv = new System.Data.DataView(dt);
        dv.Sort = sortExpression + " " + direction;
        gdvArticulos.DataSource = dv;
        gdvArticulos.DataBind();

    }
    protected void gdvArticulos_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            gdvArticulos.HeaderStyle.CssClass = "descendingCssClass";
            SortGridView(sortExpression, "DESC");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            gdvArticulos.HeaderStyle.CssClass = "ascendingCssClass";
            SortGridView(sortExpression, "ASC");
        }
    }
}
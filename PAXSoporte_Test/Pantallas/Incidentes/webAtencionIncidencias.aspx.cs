using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Threading;
using System.Globalization;

public partial class Pantallas_Incidentes_webAtencionIncidenciast : System.Web.UI.Page
{
    private clsIncidencias gINS;
    private clsUsuarios gDAL;
    public int psIncidente = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["objUsuario"] == null)
            {
                Response.Redirect("~/Pantallas/Login/webInicioSesionLogin.aspx");
            }
            if (!IsPostBack)
            {
                tbInformacionIncidencias.ActiveTabIndex = 0;
                fnCargarIncidencia(Convert.ToInt32(Session["psIncidenteIncidencia"]));
                fnCargaTipoIncidencias();
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "Page_load", "webAtencionIncidencias");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "Page_load", "webAtencionIncidencias");
        }
    }

    private void fnCargarIncidencia(int psiIncidencia)
    {
        try
        {
            gINS = new clsIncidencias();
            DataSet dsIncidencias = null;
            dsIncidencias = gINS.fnObtenerInformacionIncidencia(psiIncidencia);
            foreach (DataRow renglon in dsIncidencias.Tables[0].Rows)
            {
                txtDescripcion.Text = Convert.ToString(renglon["descripcion"]);
                lblTicket.Text = Convert.ToString(renglon["ticket"]);

                if (!string.IsNullOrEmpty(renglon["id_notificacion_sat"].ToString()))
                {
                    tbNotificacion.Text = Convert.ToString(renglon["id_notificacion_sat"]);
                }
                else
                {
                    tbNotificacion.Text = DateTime.Now +"\n\n"
                                        + "Notificación de Incidencias de Seguridad al SAT \n\n"
                                        + "Por medio del presente correo se informa acerca del incidente de seguridad presentado en el sistema de facturación gratuita \n"
                                        + "de CORPUS Facturación S.A de C.V. para el cual se anexa el plan de "
                                        + "acción a seguir. \n\n"
                                        + "En espera de respuesta de Autorización del plan de acción para poder realizar las modificaciones \n"
                                        + "necesarias para la resolución de la incidencia. \n\n"
                                        + "Atte. \n"
                                        + Convert.ToString(renglon["nombre_sop"]) + "\n"
                                        + "PAX Facturación S.A de C.V.";
                }
                if (!string.IsNullOrEmpty(renglon["solucion_usuario"].ToString()))
                {
                    txtSolucionUsuario.Text = Convert.ToString(renglon["solucion_usuario"]);
                    //btnTerminar.Enabled = true;
                }
                Session["id_usuaer_sop"] = Convert.ToInt32(renglon["id_usuario_soporte"]);
                if (!string.IsNullOrEmpty(renglon["solucion_soporte"].ToString()))
                {
                    txtSolucionSoporte.Text = Convert.ToString(renglon["solucion_soporte"]);
                    txtSolucionSoporteP.Text = Convert.ToString(renglon["solucion_soporte"]);
                   // btnTerminar.Enabled = true;
                }
                txtFecha.Text = Convert.ToString(renglon["fecha_registro"]);
                txtUsuario.Text = Convert.ToString(renglon["nombre"]);

                txtCorreo.Text = "El ticket: " + Convert.ToString(renglon["ticket"]) + " \n" + "Está siendo atendido por "
                     + Convert.ToString(renglon["nombre_sop"]) + " \n" +
                     "Gracias por su atención. Estamos a sus servicios." + " \n" + "PAX Facturación";

                psIncidente = Convert.ToInt32(renglon["id_incidente"]);
                Session["nsidIncidencia"] = Convert.ToInt32(renglon["id_incidente"]);
                Session["id_usuario_sop_ant"] = Convert.ToInt32(renglon["id_usuario_soporte"]);
                ddlIncidencia.SelectedValue = Convert.ToString(renglon["id_tipo_incidente"]);

                int psTipoIncidente = Convert.ToInt32 (renglon["id_tipo_incidente"]);
                Session["psTipoIncidente"] = psTipoIncidente;
                if (psTipoIncidente == 3)
                {
                    tbInformacionIncidencias.Tabs[1].Enabled = true;
                }
                else
                {
                    tbInformacionIncidencias.Tabs[1].Enabled = false;
                }

                int psestatusdeprueba = 0;
                try{psestatusdeprueba = Convert.ToInt32(renglon["estatus_prueba"]);}catch{psestatusdeprueba = 0;}

                if (psestatusdeprueba == 1)
                {
                    cbAprobado.Checked = true;      

                }
               
                if (!string.IsNullOrEmpty(renglon["solucion_prueba"].ToString()))
                {
                    txtSolucionPruebas.Text = Convert.ToString(renglon["solucion_prueba"]);
                }

                if (!string.IsNullOrEmpty(renglon["ticket_problema"].ToString()))
                {
                    tbInformacionIncidencias.Enabled = false;
                    btnReasignar0.Enabled = false;
                }

                if (!string.IsNullOrEmpty(renglon["estatus_sat"].ToString()))
                {
                    int psEstatusSat = Convert.ToInt32(renglon["estatus_sat"]);
                    if (psEstatusSat == 1)
                    {
                        cbAutorizaSAT.Checked = true;
                       
                    }
                    else
                    {
                        cbAutorizaSAT.Checked = false;
                      
                    }
                }
                else
                {
                    if (psTipoIncidente == 3)
                    {
                        cbAutorizaSAT.Checked = false;
                    
                    }
                    else
                    {
                        cbAutorizaSAT.Checked = true;
                      
                    }
                    
                }

                if (!string.IsNullOrEmpty(renglon["fecha_atencion"].ToString()))
                {
                    txtfechaatn.Text = Convert.ToString(renglon["fecha_atencion"].ToString());
                    btnEnviaCorreo.Enabled = false;
                    btnReasignar0.Enabled = false;              
                    btnSAT.Enabled = true;
                    btnGuardarS0.Enabled = true;
                  
                }
     


                if (!string.IsNullOrEmpty(renglon["fecha_sat"].ToString()))
                {
                    txtfechanot.Text = Convert.ToString(renglon["fecha_sat"].ToString());
                    btnAutorizaSat.Enabled = true;
                    btnSAT.Enabled = false;
                }
              

                  if (!string.IsNullOrEmpty(renglon["fecha_prueba"].ToString()))
                  {
                      txtfechaprue.Text = Convert.ToString(renglon["fecha_prueba"].ToString());
                      btnGuardarSolUsuario0.Enabled = true;
                      
                  }

                  if (!string.IsNullOrEmpty(renglon["fecha_terminacion"].ToString()))
                  {
                      txtfechater.Text = Convert.ToString(renglon["fecha_terminacion"].ToString());
                      btnTerminar.Enabled = false;
                  }

                  if (!string.IsNullOrEmpty(renglon["fecha_solucion_usuario"].ToString()))
                  {
                      txtfechausu.Text = Convert.ToString(renglon["fecha_solucion_usuario"].ToString());
                      btnTerminar.Enabled = true;
 
                  }

                  if (!string.IsNullOrEmpty(renglon["fecha_soporte"].ToString()))
                  {
                      txtfechasop.Text = Convert.ToString(renglon["fecha_soporte"].ToString());
                      btnGuardarSolucionPrueba.Enabled = true;
                  }
                 

                  if (!string.IsNullOrEmpty(renglon["fecha_not_sat"].ToString()))
                  {
                      txtfechanoti.Text = Convert.ToString(renglon["fecha_not_sat"].ToString());
                      btnAutorizaSat.Enabled = false;
                      btnSAT.Enabled = false;
                      btnGuardarS0.Enabled = true;
                  }

                  if (!string.IsNullOrEmpty(renglon["estatus_prueba"].ToString()))
                  {
                                       
                    int psEstatusPruebas = Convert.ToInt32(renglon["estatus_prueba"]);
                
                  }
                  if (!string.IsNullOrEmpty(renglon["id_notificacion_sat"].ToString()))
                {
                    tbNotificacion.Text = Convert.ToString(renglon["id_notificacion_sat"]);
                    //tbNotificacion.Enabled = false;
                    //fuSat.Enabled = false;
                  
                }

                  if (!string.IsNullOrEmpty(renglon["notifica_sat"].ToString()))
                  {
                      tbAutorizador.Text = Convert.ToString(renglon["notifica_sat"]);
                      tbAutorizador.Enabled = false;
                      fuSat.Enabled = false;
                      cbAutorizaSAT.Enabled = false;
                  }

                  if (!string.IsNullOrEmpty(renglon["estatus"].ToString()))
                  {
                      string pestatus = renglon["estatus"].ToString();
                      if (pestatus == "C")
                      {
                          tbInformacionIncidencias.Enabled = false;
                      }
                  }

                  Session["idRelUsuSis"] = Convert.ToInt32(renglon["id_relacion"]);
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargarIncidencia", "webAtencionIncidencias");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargarIncidencia", "webAtencionIncidencias");
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtSolucionSoporte.Text != "")
            {

               
                    gINS = new clsIncidencias();
                    txtfechasop.Text = Convert.ToString(DateTime.Now);
                    gINS.fnActualizaIncidencia( Convert.ToInt32(Session["psIncidenteIncidencia"]), string.Empty, txtSolucionSoporte.Text);
                    txtSolucionSoporteP.Text = txtSolucionSoporte.Text;
                    btnGuardarSolucionPrueba.Enabled = true; 
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblGuardadoSolucionSoporte);

            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblSoporteSolucion);
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnGuardar_Click", "webAtencionIncidencias");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnGuardar_Click", "webAtencionIncidencias");
        }
    }
    protected void btnTerminar_Click(object sender, EventArgs e)
    {
        try
        {

            gINS = new clsIncidencias();
            gINS.fnTerminaIncidencia(Convert.ToInt32(Session["psIncidenteIncidencia"]));
           
               string Solucion =  "El Ticket" + lblTicket.Text + "\r\n" + " fue solucionado" + "\r\n" + txtSolucionUsuario.Text;
               txtfechater.Text = Convert.ToString(DateTime.Now);
               gINS.fnEnviarNotificacionAtencionIncidencia(lblTicket.Text, ddlIncidencia.SelectedValue, Solucion, Convert.ToInt32(Session["id_usuaer_sop"]),Convert.ToInt32(Session["idRelUsuSis"]));
               tbInformacionIncidencias.Enabled = false;
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblTerminarIncidencia);
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnTerminar_Click", "webAtencionIncidencias");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnTerminar_Click", "webAtencionIncidencias");
        }
    }

    protected void btnEnviaCorreo_Click(object sender, EventArgs e)
    {
        try
        {
            gINS = new clsIncidencias();
            bool atend1 = gINS.fnActualizaFechaRegistroIncidencia(Convert.ToInt32(Session["psIncidenteIncidencia"]));
            bool atend2 = gINS.fnEnviarNotificacionAtencionIncidencia(lblTicket.Text, ddlIncidencia.SelectedValue, txtCorreo.Text, Convert.ToInt32(Session["id_usuaer_sop"]), Convert.ToInt32(Session["idRelUsuSis"]));
            if (atend1 == false || atend2 == false)
            {
                throw new Exception("Error en la conexion");
            }
            txtfechaatn.Text = Convert.ToString(DateTime.Now);
            btnEnviaCorreo.Enabled = false;
              btnReasignar0.Enabled = false;
            btnSAT.Enabled = true;
            btnGuardarS0.Enabled = true;
            /*int psTipoIncidente = Convert.ToInt32(Session["psTipoIncidente"]);
            if (psTipoIncidente != 3)
            {
                btnGuardarS0.Enabled = true;
            }*/
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblEnvioCorreo);
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnEnviaCorreo_Click", "webAtencionIncidencias.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnEnviaCorreo_Click", "webAtencionIncidencias.aspx.cs");
        }
    }


    protected void btnGuardarSolUsuario_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtSolucionSoporte.Text != null)
            {

                gINS = new clsIncidencias();
                gINS.fnActualizaIncidencia(Convert.ToInt32(Session["psIncidenteIncidencia"]), txtSolucionUsuario.Text,string.Empty);
                txtfechausu.Text = Convert.ToString(DateTime.Now);
                btnTerminar.Enabled = true;
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblGuardadoSolucionUsuario);

            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblSoporteSolucion);
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnGuardarSolUsuario_Click", "webAtencionIncidencias.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnGuardarSolUsuario_Click", "webAtencionIncidencias.aspx.cs");
        }
    }

    private void fnCargaTipoIncidencias()
    {
        try
        {
            gDAL = new clsUsuarios();
            ddlIncidencia.DataSource = gDAL.fnCargarCatalogoTipoIncidencias();
            ddlIncidencia.DataTextField = "tipo_incidente";
            ddlIncidencia.DataValueField = "id_tipo_incidente";
            ddlIncidencia.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargaTipoIncidencias", "webAtencionIncidencias.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargaTipoIncidencias", "webAtencionIncidencias.aspx.cs");
        }
    }

    protected void btnReasignar_Click(object sender, EventArgs e)
    {
        try
        {
            gINS = new clsIncidencias();
            int psIdUsuarioSoporte = 0;
            int psincidencia = 0;
            int psIdUsuarioSoporteAnt = 0;

            DataTable Tabla = new DataTable("usuarios");
            Tabla.Columns.Add("idusuario", typeof(Int32));
            Tabla.Columns.Add("carga", typeof(Int32));

            psincidencia = Convert.ToInt32(ddlIncidencia.SelectedValue);
            DataSet dsUsuario = gINS.fnObtieneUsuarioSoporte(psincidencia);
            foreach (DataRow renglon in dsUsuario.Tables[0].Rows)
            {

                DataTable dsCarga = new DataTable();
                dsCarga = gINS.fnObtieneIncidenciasporUsuario(Convert.ToInt32(renglon["id_usuario_soporte"]));
                DataRow nuevo;
                nuevo = Tabla.NewRow();
                nuevo["idusuario"] = Convert.ToInt32(renglon["id_usuario_soporte"]);
                nuevo["carga"] = Convert.ToInt32(dsCarga.Rows.Count);
                Tabla.Rows.Add(nuevo);
            }
            DataTable dtOrdenado = FiltrarDataTable(Tabla, "", "carga ASC");

            psIdUsuarioSoporteAnt= Convert.ToInt32(Session["id_usuario_sop_ant"]);
            psIdUsuarioSoporte = Convert.ToInt32(dtOrdenado.Rows[0]["idusuario"]);

            gINS.fnReasignaIncidencia(Convert.ToInt32(Session["psIncidenteIncidencia"]), psIdUsuarioSoporteAnt, psIdUsuarioSoporte, psincidencia);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblReasignado);
            tbInformacionIncidencias.Enabled = false;

            Response.Redirect("~/Pantallas/Incidentes/webAtencionIncidencias.aspx"); 
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnReasignar_Click", "webAtencionIncidencias.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnReasignar_Click", "webAtencionIncidencias.aspx.cs");
        }
    }
    public DataTable FiltrarDataTable(DataTable dt, string filtro, string orden)
    {
        DataRow[] rows;
        DataTable dtNew;

        try
        {
            dtNew = dt.Clone();
            rows = dt.Select(filtro, orden);

            Array.ForEach(rows, dtNew.ImportRow);

            return dtNew;
        }
        catch (Exception ex)
        {
            throw new Exception(String.Format("FiltrarDataTable - {0} - {1}", ex.Source, ex.Message));
        }
    }


    protected void btnGuardarSolucionPrueba_Click(object sender, EventArgs e)
    {
         try
        {
               gINS = new clsIncidencias();
            string ruta = null;
            int archivoEnBytes = 0;
            bool psarchivo = true;
            bool Archivo = true;
            if (fuPruebas.FileName.ToString() != "")
            {
                Archivo = gINS.fnverificaarchivo(fuPruebas.FileName);
                if (Archivo == true)
                {
                    string psArchivo = fuPruebas.FileName.ToString();
                    System.Guid miGUID = System.Guid.NewGuid();
                    String sGUID = miGUID.ToString();
                    fuPruebas.SaveAs(clsComun.fnObtenerParamentro("RutaDocPruebas") + sGUID+fuPruebas.FileName);
                    ruta = clsComun.fnObtenerParamentro("RutaDocPruebas") + sGUID + fuPruebas.FileName;
                    FileInfo info = new FileInfo(ruta);
                    archivoEnBytes = (Convert.ToInt32(info.Length) / 1024);
                    psarchivo = gINS.fnVerificaTamanioMax(archivoEnBytes);
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorArchivo + " " + clsComun.fnObtenerParamentro("Extensiones"));
                }
            }
            
       
          if (fuPruebas.FileName.ToString() == "")
            {
                Archivo = true;
            }
            if (Archivo == true)
            {
                if (psarchivo == false)
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorTamanio);
                }
                else
                {
                    int psEstatusPrueba = 0;
                    if (cbAprobado.Checked)
                    {
                        psEstatusPrueba = 1;
                    }

                    gINS.fnActualizaIncidenciaPrueba(Convert.ToInt32(Session["psIncidenteIncidencia"]), txtSolucionPruebas.Text, psEstatusPrueba);
                    txtfechaprue.Text = Convert.ToString(DateTime.Now);
                    if (psEstatusPrueba == 1)
                    {

                        //btnTerminar.Enabled = true;
                       
                        string Solucion = "El Ticket" + lblTicket.Text + "\r\n" + " paso las pruebas de calidad" + "\r\n" + txtSolucionPruebas.Text;
                        //txtfechater.Text = Convert.ToString(DateTime.Now);
                        gINS.fnEnviarNotificacionAtencionIncidenciaPrueba(lblTicket.Text, ddlIncidencia.SelectedValue, Solucion, Convert.ToInt32(Session["id_usuaer_sop"]), ruta,Convert.ToInt32(Session["idRelUsuSis"]));
                        btnGuardarSolUsuario0.Enabled = true;
                    }
                    else
                    {
                        string Solucion = "El Ticket" + lblTicket.Text + "\r\n" + " no paso las pruebas de calidad" + "\r\n" + txtSolucionPruebas.Text;
                        //txtfechater.Text = Convert.ToString(DateTime.Now);
                        gINS.fnEnviarNotificacionAtencionIncidenciaPrueba(lblTicket.Text, ddlIncidencia.SelectedValue, Solucion, Convert.ToInt32(Session["id_usuaer_sop"]), ruta,Convert.ToInt32(Session["idRelUsuSis"]));
                    }
                    lblpruebaa.Text = fuPruebas.FileName.ToString();
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblPruebasGuardado);
                }
            }
        
        else
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorArchivo + " " + clsComun.fnObtenerParamentro("Extensiones"));
        }
        }
         catch (SqlException ex)
         {
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnGuardarSolucionPrueba_Click", "webAtencionIncidencias.aspx.cs");
         }
         catch (Exception ex)
         {
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnGuardarSolucionPrueba_Click", "webAtencionIncidencias.aspx.cs");
         }
    }
     
    protected void btnSAT_Click(object sender, EventArgs e)
    {
         try
        {
            if (fuSat.FileName.ToString() != "")
            {

                string psArchivo = fuSat.FileName.ToString();
                gINS = new clsIncidencias();
                Guid ng = new Guid();
                fuSat.SaveAs(clsComun.fnObtenerParamentro("RutaDocSAT") + ng + fuSat.FileName);
                string ruta = clsComun.fnObtenerParamentro("RutaDocSAT") + ng + fuSat.FileName;
                gINS.fnActualizaNotificacionSAT(Convert.ToInt32(Session["psIncidenteIncidencia"]));
                gINS.fnEnviarNotificacionSAT(lblTicket.Text, ddlIncidencia.SelectedValue, tbNotificacion.Text, ruta, Convert.ToInt32(Session["idRelUsuSis"]));
                txtfechanoti.Text = Convert.ToString(DateTime.Now);
                btnSAT.Enabled = false;
                btnAutorizaSat.Enabled = true;
                lblarchivo.Text = fuSat.FileName.ToString();
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblEnvioCorreo);

            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblNotSat);
            }
                   
        }
         catch (SqlException ex)
         {
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnSAT_Click", "webAtencionIncidencias.aspx.cs");
         }
         catch (Exception ex)
         {
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnSAT_Click", "webAtencionIncidencias.aspx.cs");
         }

    }
    protected void btnAutorizaSat_Click(object sender, EventArgs e)
    {
        try
        {
            gINS = new clsIncidencias();
            int estatusSAT = 0;
            if (cbAutorizaSAT.Checked)
            {
                estatusSAT = 1;
            }
            gINS.fnActualizaEstatusSAT(Convert.ToInt32(Session["psIncidenteIncidencia"]), tbAutorizador.Text, estatusSAT, tbNotificacion.Text);
            txtfechanot.Text = Convert.ToString(DateTime.Now);
           
            if (estatusSAT == 1)
            {
                string Solucion = "El Ticket" + lblTicket.Text + "\r\n" + " fue autorizado por el SAT" + "\r\n";
                gINS.fnEnviarNotificacionAtencionIncidencia(lblTicket.Text, ddlIncidencia.SelectedValue, Solucion, Convert.ToInt32(Session["id_usuaer_sop"]),Convert.ToInt32(Session["idRelUsuSis"]));
                btnAutorizaSat.Enabled = false;
                btnGuardarS0.Enabled = true;
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblAutorizado);
            }
          
          
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnAutorizaSat_Click", "webAtencionIncidencias.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnAutorizaSat_Click", "webAtencionIncidencias.aspx.cs");
        }
    }
    protected void cbAutorizaSAT_CheckedChanged(object sender, EventArgs e)
    {
        btnSAT.Enabled = false;
        if (cbAutorizaSAT.Checked)
        {
            tbAutorizador.Enabled = true;
        }
        else
        {
          tbAutorizador.Enabled = false;
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

    public void fnManejaTabs(int psIdTipoIncidente)
    {
        if (psIdTipoIncidente == 3)
        {
            tbInformacionIncidencias.Tabs[1].Enabled = true;
        }
        else
        {
            tbInformacionIncidencias.Tabs[1].Enabled = false;
        }

    }
}
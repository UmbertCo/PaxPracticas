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

public partial class Pantallas_Problemas_webAtencionProblemas : System.Web.UI.Page
{
    private clsUsuarios gDAL;
    private clsBusquedaIncidentes gPRO;
    private clsIncidencias gINS;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/Pantallas/Login/webInicioSesionLogin.aspx");
        }
        if (!IsPostBack)
        {
            tbInformacionIncidencias.ActiveTabIndex = 0;
            fnCargaProblema(Convert.ToInt32(Session["psIncidenteProblema"]));
            fnCargaTipoIncidencias();
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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargaTipoIncidencias", "webAtencionProblemas.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargaTipoIncidencias", "webAtencionProblemas.aspx.cs");
        }
    }
    private void fnCargaProblema(int psIdProblema)
    { 
        try
        {
            gPRO = new clsBusquedaIncidentes();
            DataSet dsIncidencias = null;
            dsIncidencias = gPRO.fnObtenerInformacionProblema(psIdProblema);
            foreach (DataRow renglon in dsIncidencias.Tables[0].Rows)
            {
                txtDescripcion.Text = Convert.ToString(renglon["descripcion"]);
                lblTicket.Text = Convert.ToString(renglon["ticket_problema"]);

                
                    tbNotificacion.Text = DateTime.Now + "\n\n"
                                        + "Notificación de Incidencias de Seguridad al SAT \n\n"
                                        + "Por medio del presente correo se informa acerca del incidente de seguridad presentado en el sistema de facturación gratuita \n"
                                        + "de CORPUS Facturación S.A de C.V. para el cual se anexa el plan de "
                                        + "acción a seguir. \n\n"
                                        + "En espera de respuesta de Autorización del plan de acción para poder realizar las modificaciones \n"
                                        + "necesarias para la resolución de la incidencia. \n\n"
                                        + "Atte. \n"
                                        + Convert.ToString(renglon["nombre_sop"]) + "\n"
                                        + "PAX Facturación S.A de C.V.";
                

                if (!string.IsNullOrEmpty(renglon["solucion_usuario"].ToString()))
                {
                    txtSolucionUsuario.Text = Convert.ToString(renglon["solucion_usuario"]);
                   // btnTerminarP.Enabled = true;
                }

                if (!string.IsNullOrEmpty(renglon["solucion_soporte"].ToString()))
                {
                    txtSolucionSoporte.Text = Convert.ToString(renglon["solucion_soporte"]);
                    txtSolucionSoporteP.Text = Convert.ToString(renglon["solucion_soporte"]);
                    //btnTerminarP.Enabled = true;
                }
                txtFecha.Text = Convert.ToString(renglon["fecha_alta"]);
                txtUsuario.Text = Convert.ToString(renglon["nombre"]);

                txtCorreo.Text = "El ticket: " + Convert.ToString(renglon["ticket_problema"]) + " \n" + "Está siendo atendido por "
                     + Convert.ToString(renglon["nombre_sop"]) + " \n" +
                     "Gracias por su atención. Estamos a sus servicios." + " \n" + "PAX Facturación";

                psIdProblema = Convert.ToInt32(renglon["id_problema"]);
                Session["nsidProblema"] = Convert.ToInt32(renglon["id_problema"]);
                Session["id_usuario_sop_ant"] = Convert.ToInt32(renglon["id_usuario_soporte"]);
                ddlIncidencia.SelectedValue = Convert.ToString(renglon["id_tipo_incidente"]);

                int psTipoIncidente = Convert.ToInt32(renglon["id_tipo_incidente"]);
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

                try { psestatusdeprueba = Convert.ToInt32(renglon["estatus_prueba"]); }catch { psestatusdeprueba = 0; }
                if (psestatusdeprueba == 1)
                {
                    cbAprobado.Checked = true;
                  
                }
               

                if (!string.IsNullOrEmpty(renglon["solucion_prueba"].ToString()))
                {
                    txtSolucionPruebas.Text = Convert.ToString(renglon["solucion_prueba"]);
                }
                Session["id_user_sop"] = Convert.ToInt32(renglon["id_usuario_soporte"]);

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

                if (!string.IsNullOrEmpty(renglon["fecha_atencion_problema"].ToString()))
                {
                    txtfechaatn.Text = Convert.ToString(renglon["fecha_atencion_problema"].ToString());
                    btnEnviaCorreo.Enabled = false;
                    btnReasignar.Enabled = false;
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
                    btnTerminarP.Enabled = false;
                }

                if (!string.IsNullOrEmpty(renglon["fecha_solucion_usuario"].ToString()))
                {
                    txtfechausu.Text = Convert.ToString(renglon["fecha_solucion_usuario"].ToString());
                    btnTerminarP.Enabled = true;
                    
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
                    if (psEstatusPruebas == 1)
                    {
                        cbAprobado.Checked = true;
                    }
                 
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
                
                }
               
                if (!string.IsNullOrEmpty(renglon["id_estatus"].ToString()))
                {
                    string pestatus = renglon["id_estatus"].ToString();
                    if (pestatus == "C")
                    {
                        tbInformacionIncidencias.Enabled = false;
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargaProblema", "webAtencionProblemas.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargaProblema", "webAtencionProblemas.aspx.cs");
        }
    }

    protected void btnEnviaCorreo_Click(object sender, EventArgs e)
    {
        try
        {
            gPRO = new clsBusquedaIncidentes();
            bool atend1 = gPRO.fnActualizaFechaRegistroProblema(Convert.ToInt32(Session["psIncidenteProblema"]));
            bool atend2 = gPRO.fnEnviarNotificacionAtencionProblema(lblTicket.Text, ddlIncidencia.SelectedValue, txtCorreo.Text,Convert.ToInt32( Session["id_user_sop"]));
            if (atend1 == false || atend2 == false)
            {
                throw new Exception("Error en la conexion");
            }
            txtfechaatn.Text = Convert.ToString(DateTime.Now);
            btnEnviaCorreo.Enabled = false;
            btnReasignar.Enabled = false;
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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnEnviaCorreo_Click", "webAtencionProblemas.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnEnviaCorreo_Click", "webAtencionProblemas.aspx.cs");
        }
    }
    protected void btnSAT_Click(object sender, EventArgs e)
    {
        try
        {
            if (fuSat.FileName.ToString() != "")
            {

                string psArchivo = fuSat.FileName.ToString();
                gPRO = new clsBusquedaIncidentes();
                Guid ng = new Guid();
                fuSat.SaveAs(clsComun.fnObtenerParamentro("RutaDocSAT") + ng + fuSat.FileName);
                string ruta = clsComun.fnObtenerParamentro("RutaDocSAT") + ng + fuSat.FileName;
                gPRO.fnActualizaNotificacionSAT(Convert.ToInt32(Session["psIncidenteProblema"]));
                gPRO.fnEnviarNotificacionSAT(lblTicket.Text, ddlIncidencia.SelectedValue, tbNotificacion.Text, ruta);
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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnSAT_Click", "webAtencionProblemas.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnSAT_Click", "webAtencionProblemas.aspx.cs");
        }
    }
    protected void btnAutorizaSat_Click(object sender, EventArgs e)
    {
        try
        {
            gPRO = new clsBusquedaIncidentes();
            int estatusSAT = 0;
            if (cbAutorizaSAT.Checked)
            {
                estatusSAT = 1;
            }
            gPRO.fnActualizaEstatusSAT(Convert.ToInt32(Session["psIncidenteProblema"]), tbAutorizador.Text, estatusSAT, tbNotificacion.Text);
            txtfechanot.Text = Convert.ToString(DateTime.Now);
            

            if (estatusSAT == 1)
            { 
                string Solucion = "El Ticket" + lblTicket.Text + "\r\n" + " fue autorizado por el SAT" + "\r\n";
                gPRO.fnEnviarNotificacionAtencionProblema(lblTicket.Text, ddlIncidencia.SelectedValue, Solucion,Convert.ToInt32( Session["id_user_sop"]));               
                btnAutorizaSat.Enabled = false;
                btnGuardarS0.Enabled = true;
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblAutorizado);
                
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnAutorizaSat_Click", "webAtencionProblemas.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnAutorizaSat_Click", "webAtencionProblemas.aspx.cs");
        }
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtSolucionSoporte.Text != "")
            {
              
                    gPRO =  new clsBusquedaIncidentes();
                    txtfechasop.Text = Convert.ToString(DateTime.Now);
                    gPRO.fnActualizaProblema(Convert.ToInt32(Session["psIncidenteProblema"]), string.Empty, txtSolucionSoporte.Text);
                    txtSolucionSoporteP.Text = txtSolucionSoporte.Text;
                    btnGuardarSolucionPrueba.Enabled = true;
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblGuardadoSolucionSoporte);
                   // btnTerminarP.Enabled = true;
                    

            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblSoporteSolucion);
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnGuardar_Click", "webAtencionProblemas.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnGuardar_Click", "webAtencionProblemas.aspx.cs");
        }
    }
    protected void  btnGuardarSolucionPrueba_Click(object sender, EventArgs e)
{
        try
        {
              gPRO = new clsBusquedaIncidentes();
            string ruta = null;
            int archivoEnBytes = 0;
            bool psarchivo = true;
            bool Archivo = false;
            if (fuPruebas.FileName.ToString() != "")
            {
                
                Archivo = gPRO.fnverificaarchivo(fuPruebas.FileName);
                if (Archivo == true)
                {
                    string psArchivo = fuPruebas.FileName.ToString();
                    System.Guid miGUID = System.Guid.NewGuid();
                    String sGUID = miGUID.ToString();
                    fuPruebas.SaveAs(clsComun.fnObtenerParamentro("RutaDocPruebas") + sGUID + fuPruebas.FileName);
                    ruta = clsComun.fnObtenerParamentro("RutaDocPruebas") + sGUID + fuPruebas.FileName;
                    FileInfo info = new FileInfo(ruta);
                    archivoEnBytes = (Convert.ToInt32(info.Length) / 1024);
                    psarchivo = gPRO.fnVerificaTamanioMax(archivoEnBytes);
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

                      txtfechaprue.Text = Convert.ToString(DateTime.Now);
                      gPRO.fnActualizaProblemaPrueba(Convert.ToInt32(Session["psIncidenteProblema"]), txtSolucionPruebas.Text, psEstatusPrueba);

                      if (psEstatusPrueba == 1)
                      {
                         // btnTerminarP.Enabled = true;
                          

                          tbInformacionIncidencias.Tabs[4].Enabled = true;
                          string Solucion = "El Ticket" + lblTicket.Text + "\r\n" + " paso las pruebas de calidad" + "\r\n" + txtSolucionPruebas.Text;
                          //txtfechater.Text = Convert.ToString(DateTime.Now);
                          gPRO.fnEnviarNotificacionPruebas(lblTicket.Text, ddlIncidencia.SelectedValue, Solucion, ruta);
                          btnGuardarSolUsuario0.Enabled = true;

                      }
                      else
                      {
                          string Solucion = "El Ticket" + lblTicket.Text + "\r\n" + " no paso las pruebas de calidad" + "\r\n" + txtSolucionPruebas.Text;
                         // txtfechater.Text = Convert.ToString(DateTime.Now);
                          gPRO.fnEnviarNotificacionPruebas(lblTicket.Text, ddlIncidencia.SelectedValue, Solucion, ruta);
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
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnGuardarSolucionPrueba_Click", "webAtencionProblemas.aspx.cs");
         }
         catch (Exception ex)
         {
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnGuardarSolucionPrueba_Click", "webAtencionProblemas.aspx.cs");
         }
}
protected void  btnGuardarSolUsuario_Click(object sender, EventArgs e)
{
    try
        {
            if (txtSolucionSoporte.Text != null)
            {

                gPRO = new clsBusquedaIncidentes();
                gPRO.fnActualizaProblema(Convert.ToInt32(Session["psIncidenteProblema"]), txtSolucionUsuario.Text, string.Empty);
                btnTerminarP.Enabled = true;
                txtfechausu.Text = Convert.ToString(DateTime.Now);
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblGuardadoSolucionUsuario);
                
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblSoporteSolucion);
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnGuardarSolUsuario_Click", "webAtencionProblemas.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnGuardarSolUsuario_Click", "webAtencionProblemas.aspx.cs");
        }
}
protected void  btnTerminar_Click(object sender, EventArgs e)
{
     try
        {

            gPRO = new clsBusquedaIncidentes();
            gINS = new clsIncidencias();
            DataSet dsAuxiliar = new DataSet();
            dsAuxiliar = gPRO.fnObtieneIncidenciasbyProblema(Convert.ToInt32(Session["psIncidenteProblema"]));
            foreach (DataRow renglon in dsAuxiliar.Tables[0].Rows)
            {
                gPRO.fnFInalizaIncidenciabyProblema(Convert.ToInt32(renglon["id_incidente"]));
                DataSet dsincidencias = new DataSet();
                dsincidencias = gINS.fnObtenerInformacionIncidencia(Convert.ToInt32(renglon["id_incidente"]));
                foreach (DataRow inc in dsincidencias.Tables[0].Rows)
                {
                    string SolucionInc = "El Ticket " + Convert.ToString(inc["ticket"]) + "\r\n" + " fue solucionado" + "\r\n" + txtSolucionUsuario.Text;
                    gINS.fnEnviarNotificacionAtencionIncidencia(Convert.ToString(inc["ticket"]), ddlIncidencia.SelectedValue, SolucionInc, Convert.ToInt32(inc["id_usuario_soporte"]), Convert.ToInt32(inc["id_relacion"]));
                }
            }
         
            gPRO.fnTerminaProblema(Convert.ToInt32(Session["psIncidenteProblema"]));
            txtfechater.Text = Convert.ToString(DateTime.Now);
               string Solucion =  "El Ticket " + lblTicket.Text + "\r\n" + " fue solucionado" + "\r\n" + txtSolucionUsuario.Text;
               gPRO.fnEnviarNotificacionAtencionProblema(lblTicket.Text, ddlIncidencia.SelectedValue, Solucion, Convert.ToInt32(Session["id_user_sop"]));
               tbInformacionIncidencias.Enabled = false;
               clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblTerminarProblema);
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnTerminar_Click", "webAtencionProblemas.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnTerminar_Click", "webAtencionProblemas.aspx.cs");
        }
}

protected void btnReasignar_Click(object sender, EventArgs e)
{
    try
    {
        gPRO = new clsBusquedaIncidentes();
        int psIdUsuarioSoporte = 0;
        int psincidencia = 0;
        int psIdUsuarioSoporteAnt = 0;
        DataTable Tabla = new DataTable("usuarios");
        Tabla.Columns.Add("idusuario", typeof(Int32));
        Tabla.Columns.Add("carga", typeof(Int32));

        psincidencia = Convert.ToInt32(ddlIncidencia.SelectedValue);
        psIdUsuarioSoporte = gPRO.fnObtieneUsuarioSoporteporTipoProblema(psincidencia);
        DataSet dsUsuario = gPRO.fnObtieneUsuarioSoporte(psincidencia);
         foreach (DataRow renglon in dsUsuario.Tables[0].Rows)
         {
             DataTable dsCarga = new DataTable();
             dsCarga = gPRO.fnObtieneProblemasporUsuario(Convert.ToInt32(renglon["id_usuario_soporte"]));
             DataRow nuevo;
             nuevo = Tabla.NewRow();
             nuevo["idusuario"] = Convert.ToInt32(renglon["id_usuario_soporte"]);
             nuevo["carga"] = Convert.ToInt32(dsCarga.Rows.Count);
             Tabla.Rows.Add(nuevo);
         }

        DataTable dtOrdenado = FiltrarDataTable(Tabla, "", "carga ASC");

        psIdUsuarioSoporte = Convert.ToInt32(dtOrdenado.Rows[0]["idusuario"]);
        psIdUsuarioSoporteAnt = Convert.ToInt32(Session["id_usuario_sop_ant"]);

        gPRO.fnReasignaProblema(Convert.ToInt32(Session["psIncidenteProblema"]), psIdUsuarioSoporteAnt, psIdUsuarioSoporte, psincidencia);
        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblReasignado);
        tbInformacionIncidencias.Enabled = false;

        Response.Redirect("~/Pantallas/Problemas/webAtencionProblemas.aspx"); 
    }
    catch (SqlException ex)
    {
        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnReasignar_Click", "webAtencionProblemas.aspx.cs");
    }
    catch (Exception ex)
    {
        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnReasignar_Click", "webAtencionProblemas.aspx.cs");
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
}
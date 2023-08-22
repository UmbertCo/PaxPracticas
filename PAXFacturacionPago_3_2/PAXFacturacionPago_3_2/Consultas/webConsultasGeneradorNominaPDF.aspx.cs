using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.IO;
public partial class Consultas_webConsultasGeneradorPDF : System.Web.UI.Page
{
    private clsOperacionConsulta gDAL;
    private clsInicioSesionUsuario DatosUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                int nIdContribuyente = clsComun.fnUsuarioEnSesion().id_contribuyente;
                string sIdCfd = HttpUtility.UrlDecode(Request.QueryString["nic"]);
                string sTipoDocumento = HttpUtility.UrlDecode(Request.QueryString["doc"]);
                string sTipoConexion = HttpUtility.UrlDecode(Request.QueryString["ver"]);

                DatosUsuario = clsComun.fnUsuarioEnSesion();
                

                if (!string.IsNullOrEmpty(sIdCfd))
                    if (sTipoConexion == null)
                    {
                        fnGenerarPDF(nIdContribuyente, sIdCfd, sTipoDocumento, DatosUsuario.id_rfc, DatosUsuario.color);
                    }
                    else
                    {
                        fnGenerarPDFProveedor(sIdCfd, sTipoDocumento);
                    }
            }
            catch (Exception)
            {
                //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
            }
        }
    }

    private void fnGenerarPDF(int pnIdContribuyente, string psIdCfd, string psTipoDocumento,int sId_rfc,string scolor)
    {
        gDAL = new clsOperacionConsulta();
        //clsOperacionConsultaPdf pdf;
        clsPlantillaLista pdf;
        clsConfiguracionPlantilla PlantillaC = new clsConfiguracionPlantilla();
        
        //string plantilla = Session["plantilla"].ToString();
        //string color = Session["color"].ToString();
        //int id_rfc = Convert.ToInt32(Session["id_rfc"].ToString());
       // string plantilla = HttpUtility.UrlDecode(Request.QueryString["plantilla"]);

        try
        {
            XmlDocument XmlDoc = new XmlDocument();
            pdf = new clsPlantillaLista();
            DatosUsuario = clsComun.fnUsuarioEnSesion();
            string plantilla = "Nomina";//PlantillaC.fnRecuperaPlantillaNombre(DatosUsuario.plantilla);
            XmlDoc = gDAL.fnObtenerComprobanteXML(pnIdContribuyente, psIdCfd);
            pdf.fnObtenerPLantilla(XmlDoc, plantilla, psIdCfd, psTipoDocumento, this, string.Empty, pnIdContribuyente, DatosUsuario.id_rfc, DatosUsuario.color);


            //XmlDoc = gDAL.fnObtenerComprobanteXML(pnIdContribuyente, psIdCfd);

            //pdf = new clsOperacionConsultaPdf(XmlDoc);
            //if (!string.IsNullOrEmpty(psTipoDocumento))
            //    pdf.TipoDocumento = psTipoDocumento.ToUpper();
            //pdf.fnGenerarPDF();

            //clsComun.fnNuevaPistaAuditoria(
            //    "webConsultasGeneradorPDF",
            //    "fnGenerarPDF",
            //    "Se generó el PDF para el comprobante con ID " + psIdCfd
            //    );

            //pdf.fnMostrarPDF(this);     
        }
        catch (System.Xml.XmlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
        }
        catch (Root.Reports.ReportException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.PDF);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
        }
        catch (Exception)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
        }
    }

    private void fnGenerarPDFProveedor(string psIdCfd, string psTipoDocumento)
    {
        gDAL = new clsOperacionConsulta();
        clsPlantillaLista pdf;

        //clsOperacionConsultaPdf pdf;
        //clsOperacionConsultaPdfV2 pdf2;

        try
        {
            XmlDocument XDoc = new XmlDocument();
            pdf = new clsPlantillaLista();
            XDoc = gDAL.fnObtenerComprobanteXMLProveedor(psIdCfd);
            string VersionX = string.Empty;
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(XDoc.NameTable);
            
            //intentamos agregar el namespace para la version 3.0
            try
            {
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                XPathNavigator navDoc = XDoc.CreateNavigator();
                VersionX = navDoc.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.PDF, "El documento no es versión 3.0", clsComun.fnUsuarioEnSesion().id_usuario);
            }

            //intentamos agregar el namespace para la version 2.0
            try
            {
                nsmComprobante.RemoveNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");
                XPathNavigator navDoc = XDoc.CreateNavigator();
                VersionX = navDoc.SelectSingleNode("/cfd:Comprobante/@version", nsmComprobante).Value;
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.PDF, "El documento no es versión 2.0", clsComun.fnUsuarioEnSesion().id_usuario);
            }
            switch (VersionX)
            {
                case "2.0":
                    pdf.fnObtenerPLantilla(XDoc, "V2", psIdCfd, psTipoDocumento, this, string.Empty,0,0,string.Empty);
                    //pdf2 = new clsOperacionConsultaPdfV2(XDoc);
                    //  if (!string.IsNullOrEmpty(psTipoDocumento))
                    //      pdf2.TipoDocumento = psTipoDocumento.ToUpper();
                    //  pdf2.fnGenerarPDF();

                    //    clsComun.fnNuevaPistaAuditoria(
                    //      "webConsultasGeneradorPDF",
                    //       "fnGenerarPDF",
                    //       "Se generó el PDF para el comprobante con ID " + psIdCfd
                    //         );

                    //    pdf2.fnMostrarPDF(this);
                break;

                case "3.0":
                pdf.fnObtenerPLantilla(XDoc, string.Empty, psIdCfd, psTipoDocumento, this, string.Empty, 0,0, string.Empty);
                      //pdf = new clsOperacionConsultaPdf(XDoc);
                      //if (!string.IsNullOrEmpty(psTipoDocumento))
                      //  pdf.TipoDocumento = psTipoDocumento.ToUpper();
                      //pdf.fnGenerarPDF();

                      // clsComun.fnNuevaPistaAuditoria(
                      //  "webConsultasGeneradorPDF",
                      //  "fnGenerarPDF",
                      //  "Se generó el PDF para el comprobante con ID " + psIdCfd
                      //  );

                      //  pdf.fnMostrarPDF(this);
                break;           
            }

            
          
        }
        catch (System.Xml.XmlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
        }
        catch (Root.Reports.ReportException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.PDF);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
        }
        catch (Exception)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
        }
    }
}
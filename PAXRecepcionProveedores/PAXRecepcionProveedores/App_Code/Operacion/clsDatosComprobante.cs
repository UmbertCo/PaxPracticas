using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Xml;
using System.Xml.XPath;


public class clsDatosComprobante
{
    /// <summary>
    /// RFC del emisor del comprobante
    /// </summary>
    string sRfcEmisor { get; set; }
    /// <summary>
    /// Razon Socual del emisor del comprobante
    /// </summary>
    string sNombreEmisor { get; set; }
    /// <summary>
    /// RFC del receptor del comprobante
    /// </summary>
    string sRfcReceptor { get; set; }
    /// <summary>
    /// Nombre del Receptor del comprobante
    /// </summary>
    string sNombreReceptor { get; set; }
    /// <summary>
    /// Serie del comprobante
    /// </summary>
    string sSerie { get; set; }
    /// <summary>
    /// folio del comprobante
    /// </summary>
    int nFolio { get; set; }
    /// <summary>
    /// UUID del Comprobante (CFDI)
    /// </summary>
    string sUuid { get; set; }
    /// <summary>
    /// Fecha del comprobante
    /// </summary>
    DateTime dFechaDocumento { get; set; }
    /// <summary>
    /// Fecha de validacion del comprobante
    /// </summary>
    DateTime dFechaValidacion { get; set; }
    /// <summary>
    /// Indica el resultado de la validacion del comprobante
    /// </summary>
    bool bValido { get; set; }
    /// <summary>
    /// Mensaje devuelto por el ws de validacion
    /// </summary>
    string sMensaje { get; set; }
    /// <summary>
    /// Version del comprobante
    /// </summary>
    string sVersion { get; set; }
    /// <summary>
    /// Ruta Fisica donde se encuentra el archivo
    /// </summary>
    string sArchivo { get; set; }
    /// <summary>
    /// id del usuario que valido el comprobante
    /// </summary>
    int nIdUsuario { get; set; }
    /// <summary>
    /// El archivo xml
    /// </summary>
    XmlDocument xComprobante { get; set; }
    /// <summary>
    /// El archivo PDF en Bytes
    /// </summary>
    byte[] bPdf { get; set; }
    /// <summary>
    /// El Id de la sucursal a la que pertenece
    /// </summary>
    int nIdSucursal { get; set; }

    public clsDatosComprobante(XmlDocument xComprobante, clsResultadoValidacion res, string ruta, int idUsuario, byte[] bPdf, int nIdSucursal)
    {
        string sVersionDoc = string.Empty;
        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xComprobante.NameTable);
        nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

        XPathNavigator navComprobante = xComprobante.CreateNavigator();

        try
        {
            this.xComprobante = xComprobante;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        try
        {
            sVersionDoc = navComprobante.SelectSingleNode("/cfd:Comprobante/@version", nsmComprobante).Value;
        }
        catch
        {
            try
            {
                sVersionDoc = navComprobante.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region CFD
        if (sVersionDoc.StartsWith("2"))
        {
            try
            {
                sRfcEmisor = navComprobante.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/@rfc", nsmComprobante).Value;
            }
            catch
            {
                sRfcEmisor = string.Empty;
            }
            try
            {
                sNombreEmisor = navComprobante.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/@nombre", nsmComprobante).Value;
            }
            catch
            {
                sNombreEmisor = string.Empty;
            }
            try
            {
                sRfcReceptor = navComprobante.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/@rfc", nsmComprobante).Value;
            }
            catch
            {
                sRfcReceptor = string.Empty;
            }
            try
            {
                sNombreReceptor = navComprobante.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/@nombre", nsmComprobante).Value;
            }
            catch
            {
                sNombreReceptor = string.Empty;
            }
            try
            {
                sSerie = navComprobante.SelectSingleNode("/cfd:Comprobante/@serie", nsmComprobante).Value;
            }
            catch
            {
                sSerie = string.Empty;
            }
            try
            {
                nFolio = Convert.ToInt32(navComprobante.SelectSingleNode("/cfd:Comprobante/@folio", nsmComprobante).Value);
            }
            catch
            {
                nFolio = 0;
            }
            sUuid = string.Empty;
            try
            {
                dFechaDocumento = Convert.ToDateTime(navComprobante.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).Value);
            }
            catch
            {
                dFechaDocumento = DateTime.MinValue;
            }
        }
        #endregion
        #region CFDI
        if (sVersionDoc.StartsWith("3"))
        {
            try
            {
                sRfcEmisor = navComprobante.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
            }
            catch
            {
                sRfcEmisor = string.Empty;
            }
            try
            {
                sNombreEmisor = navComprobante.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobante).Value;
            }
            catch
            {
                sNombreEmisor = string.Empty;
            }
            try
            {
                sRfcReceptor = navComprobante.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value;
            }
            catch
            {
                sRfcReceptor = string.Empty;
            }
            try
            {
                sNombreReceptor = navComprobante.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobante).Value;
            }
            catch
            {
                sNombreReceptor = string.Empty;
            }
            try
            {
                sSerie = navComprobante.SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobante).Value;
            }
            catch
            {
                sSerie = string.Empty;
            }
            try
            {
                nFolio = Convert.ToInt32(navComprobante.SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobante).Value);
            }
            catch
            {
                nFolio = 0;
            }
            try
            {
                sUuid = navComprobante.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
            }
            catch
            {
                sUuid = string.Empty;
            }
            try
            {
                dFechaDocumento = Convert.ToDateTime(navComprobante.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).Value);
            }
            catch
            {
                dFechaDocumento = DateTime.MinValue;
            }
        }
        #endregion
        this.bPdf = bPdf;
        dFechaValidacion = DateTime.Now;
        bValido = res.valido;
        sMensaje = res.mensaje;
        sVersion = sVersionDoc;
        sArchivo = ruta;
        nIdUsuario = idUsuario;
        this.nIdSucursal = nIdSucursal;
    }

    public void fnGuardarComprobante()
    {
        try
        {
            InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");
            iSql.AgregarParametro("sRfc_emisor", this.sRfcEmisor);
            iSql.AgregarParametro("sNombre_Emisor", this.sNombreEmisor);
            iSql.AgregarParametro("sRfc_Receptor", this.sRfcReceptor);
            iSql.AgregarParametro("sNombre_Receptor", this.sNombreReceptor);
            if (!string.IsNullOrEmpty(this.sSerie))
                iSql.AgregarParametro("sSerie", this.sSerie);
            if (this.nFolio > 0)
                iSql.AgregarParametro("nFolio", this.nFolio);
            if (!string.IsNullOrWhiteSpace(this.sUuid))
                iSql.AgregarParametro("sUuid", this.sUuid);
            iSql.AgregarParametro("dFecha_Documento", this.dFechaDocumento);
            iSql.AgregarParametro("dFecha_Validacion", this.dFechaValidacion);
            iSql.AgregarParametro("bValido", this.bValido);
            if (!this.bValido)
                iSql.AgregarParametro("sMensaje", this.sMensaje);
            iSql.AgregarParametro("sVersion", this.sVersion);
            //iSql.AgregarParametro("sArchivo", this.sArchivo);
            iSql.AgregarParametro("nId_Usuario", this.nIdUsuario);
            iSql.AgregarParametro("xXml", this.xComprobante.OuterXml.ToString());
            if(bPdf.Length>0)
                iSql.AgregarParametro("bPdf", this.bPdf);
            iSql.AgregarParametro("nIdSucursal", this.nIdSucursal);
            iSql.NoQuery("usp_rfp_GuardaComprobante_Ins", true);

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
}

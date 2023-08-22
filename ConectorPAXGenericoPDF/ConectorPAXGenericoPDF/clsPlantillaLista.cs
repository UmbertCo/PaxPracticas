using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Data;

/// <summary>
/// Clase encargada de manjera la lista de plantillas desponibles por usuario.
/// </summary>
public class clsPlantillaLista
{
    public clsPlantillaLista()
	{

	}
    /// <summary>
    /// Crea archivo pdf segun plantilla configurada para su posterior envio de correo
    /// </summary>
    /// <param name="pxComprobante"></param>
    /// <param name="sPlantilla"></param>
    /// <param name="psIdCfd"></param>
    /// <param name="psTipoDocumento"></param>
    /// <param name="pagina"></param>
    /// <param name="sRuta"></param>
    /// <param name="id_contribuyente"></param>
    /// <param name="id_rfc"></param>
    /// <param name="scolor"></param>
    public void fnCrearPLantillaEnvio(XmlDocument pxComprobante, string psTipoDocumento, string sRuta)
    {

        if (!(sRuta == string.Empty))
        {

            clsOperacionConsultaPdf pdf = new clsOperacionConsultaPdf(pxComprobante);

            if (!string.IsNullOrEmpty(psTipoDocumento))
                pdf.TipoDocumento = psTipoDocumento.ToUpper();
            pdf.fnGenerarPDFSave(sRuta, "Black");

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;


public class clsPlantillaLista
{
    public clsPlantillaLista()
    {

    }

    public void fnObtenerPlantilla(XmlDocument xComprobante, int nTipoSucursal, System.Web.UI.Page pagina, int nIdComprobante)
    {
        //Para pruebas, todas las plantillas apuntan a la genérica
        //clsOperacionConsultaPdf pdf = new clsOperacionConsultaPdf(xComprobante);
        clsPlantillaCasaGrande pdf = new clsPlantillaCasaGrande(xComprobante);
        pdf.fnGenerarPDF(nIdComprobante);
        //pdf.fnGenerarPDFSave(nIdComprobante, "black", "c:/pruebas/guardar.pdf");
        pdf.fnMostrarPDF(pagina);
        
        //Con el siguiente código, se seleccionará la plantilla correspondiente al número

        //switch (nTipoSucursal)
        //{
        //    case 2:
        //        clsOperacionConsultaPdf pdf = new clsOperacionConsultaPdf(xComprobante);
        //        pdf.fnGenerarPDF("black");
        //        pdf.fnMostrarPDF(pagina);
        //        break;
        //}
    }
}

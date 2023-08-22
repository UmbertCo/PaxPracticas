using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SolucionPruebas.Servicios.Modulos
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string fnEnviarXML(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion);

        [OperationContract]
        string fnAplicarHojaTransformacion(string psDocumento);

        [OperationContract]
        bool fnGenerarPfxPem(string psRutaPfx, string psCertificado, string psLlave, string psPassword, bool pbIncludeCertsInChain);

        [OperationContract]
        bool fnGenerarPfxBytes(string psRutaPfx, byte[] psCertificado, byte[] psLlave, string psPassword, bool pbIncludeCertsInChain);

        [OperationContract]
        bool fnGenerarPfxRuta(string psRutaPfx, string psRutaCertificado, string psRutaLlave, string psPassword, bool pbIncludeCertsInChain);

        [OperationContract]
        byte[] fnGenerarPfxRutasByte(string psRutaPfx, string psRutaCertificado, string psRutaLlave, string psPassword, bool pbIncludeCertsInChain);

        [OperationContract]
        string fnGenerarSello(string psRutaPEM, string psCadenaOriginal, byte[] psLlave, string psPassword);

        [OperationContract]
        string fnGenerarSelloRutas(string psRutaPEM, string psCadenaOriginal, string psLlave, string psPassword);

        [OperationContract]
        string fnGenerarSelloOpenSSL(string psRutaCertificado, string psRutaLlave, string psPassword, string psNombreCertificado, string psNombreLlave, string psCadenaOriginal, string psRutaPfx);

        [OperationContract]
        [WebInvoke(Method = "POST")]
        string fnServicioPrueba(string psPrueba);
    }
}

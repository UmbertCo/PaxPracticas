using SolucionPruebas.Negocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace SolucionPruebas.Servicios.Modulos
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service : IService
    {
        public string fnEnviarXML(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion)
        {
            return "Exito";
        }

        public string fnAplicarHojaTransformacion(string psDocumento)
        {
            XML BXml = new XML();
            return BXml.fnAplicarHojaTransformacion(psDocumento);
        }

        public bool fnGenerarPfxPem(string psRutaPfx, string psCertificado, string psLlave, string psPassword, bool pbIncludeCertsInChain)
        {
            Chilkat BChilkat = new Chilkat();
            return BChilkat.fnGenerarPfxPem(psRutaPfx, psCertificado, psLlave, psPassword, pbIncludeCertsInChain);
        }

        public bool fnGenerarPfxBytes(string psRutaPfx, byte[] pbCertificado, byte[] pbLlave, string psPassword, bool pbIncludeCertsInChain)
        {
            Chilkat BChilkat = new Chilkat();
            return BChilkat.fnGenerarPfxBytes(psRutaPfx, pbCertificado, pbLlave, psPassword, pbIncludeCertsInChain);
        }

        public bool fnGenerarPfxRuta(string psRutaPfx, string psRutaCertificado, string psRutaLlave, string psPassword, bool pbIncludeCertsInChain)
        {
            Chilkat BChilkat = new Chilkat();
            return BChilkat.fnGenerarPfxRuta(psRutaPfx, psRutaCertificado, psRutaLlave, psPassword, pbIncludeCertsInChain);
        }

        public byte[] fnGenerarPfxRutasByte(string psRutaPfx, string psRutaCertificado, string psRutaLlave, string psPassword, bool pbIncludeCertsInChain)
        {
            Chilkat BChilkat = new Chilkat();
            return BChilkat.fnGenerarPfxRutasByte(psRutaPfx, psRutaCertificado, psRutaLlave, psPassword, pbIncludeCertsInChain);
        }

        public string fnGenerarSello(string psRutaPEM, string psCadenaOriginal, byte[] psLlave, string psPassword)
        {
            OpenSSL BOpenSSL = new OpenSSL();
            return BOpenSSL.fnGenerarSello(psRutaPEM, psCadenaOriginal, psLlave, psPassword);
        }

        public string fnGenerarSelloRutas(string psRutaPEM, string psCadenaOriginal, string psLlave, string psPassword)
        {
            OpenSSL BOpenSSL = new OpenSSL();
            return BOpenSSL.fnGenerarSello(psRutaPEM, psCadenaOriginal, psLlave, psPassword);
        }

        public string fnGenerarSelloOpenSSL(string psRutaCertificado, string psRutaLlave, string psPassword, string psNombreCertificado, string psNombreLlave, string psCadenaOriginal, string psRutaPfx)
        {
            OpenSSL BOpenSSL = new OpenSSL();
            return BOpenSSL.fnGenerarSello(psRutaCertificado, psRutaLlave, psPassword, psNombreCertificado, psNombreLlave, psCadenaOriginal, psRutaPfx);
        }

        public string fnServicioPrueba(string psPrueba)
        {
            return psPrueba + "-" + "Regresado";
        }
    }
}

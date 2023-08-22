using SolucionPruebas.Presentacion.Servicios.wsRegistroUsuariosProduccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolucionPruebas.Presentacion.Servicios
{
    public static class ProxyLocator
    {
        #region Timbrado

        public static wsRecepcionProduccion.wcfRecepcionASMXSoapClient ObtenerServicioRecepcionProduccion()
        {
            return new wsRecepcionProduccion.wcfRecepcionASMXSoapClient();
        }

        public static wsRecepcionDesarrollo.wcfRecepcionASMXSoapClient ObtenerServicioTimbradoDesarrollo()
        {
            return new wsRecepcionDesarrollo.wcfRecepcionASMXSoapClient();
        }

        public static wsRecepcionDP.wcfRecepcionASMXSoapClient ObtenerServicioTimbradoDP()
        {
            return new wsRecepcionDP.wcfRecepcionASMXSoapClient();
        }

        public static wsRecepcionTestASMX.wcfRecepcionASMXSoapClient ObtenerServicioTimbradoTest()
        {
            return new wsRecepcionTestASMX.wcfRecepcionASMXSoapClient();
        }

        public static wsRecepcionDPSVC.IwcfRecepcionClient ObtenerServicioTimbradoDPSVC()
        {
            return new wsRecepcionDPSVC.IwcfRecepcionClient();
        }

        public static wsRecepcionDPAspel.wcfRecepcionASPELSoapClient ObtenerServicioTimbradoDPAspel()
        {
            return new wsRecepcionDPAspel.wcfRecepcionASPELSoapClient();
        }

        #endregion

        #region Servicios Locales

        public static ServicioLocal.ServiceClient ObtenerServicioLocal()
        {
            return new ServicioLocal.ServiceClient();
        }

        public static CatalogoServicioLocal.CatalogoServiceClient ObtenerCatalogoServicioLocal()
        {
            return new CatalogoServicioLocal.CatalogoServiceClient();
        }

        public static SesionServicioLocal.SesionServiceClient ObtenerSesionServicioLocal()
        {
            return new SesionServicioLocal.SesionServiceClient();
        }

        public static wsServicioAutenticacion.ServiceAuthenticationSoapClient ObtenerServicioAutenticacionLocal()
        {
            return new wsServicioAutenticacion.ServiceAuthenticationSoapClient();
        }

        #endregion        

        #region Cancelacion

        public static CancelacionService.wcfCancelaASMXSoapClient ObtenerServicioCancelacionWCF()
        {
            return new CancelacionService.wcfCancelaASMXSoapClient();
        }

        public static wsCancelacionDistribuidor.wcfCancelaASMXSoapClient ObtenerServicioCancelacionDistribuidor()
        {
            return new wsCancelacionDistribuidor.wcfCancelaASMXSoapClient();
        }

        public static wsCancelaPruebas.wcfCancelaASMXSoapClient ObtenerServicioCancelacionPruebas()
        {
            return new wsCancelaPruebas.wcfCancelaASMXSoapClient();
        }

        #endregion

        #region Validacion

        public static wsValidacionTest.wcfValidaASMXSoapClient ObtenerServicioValidacionPrueba()
        {
            return new wsValidacionTest.wcfValidaASMXSoapClient();
        }

        public static wsValidacionDesarrollo.wcfValidaASMXSoapClient ObtenerServicioValidacionDesarrollo()
        {
            return new wsValidacionDesarrollo.wcfValidaASMXSoapClient();
        }

        #endregion

        #region LCO

        public static wcfLCO.wcfRecuperaLCOSoapClient ObtenerServicioLCO()
        {
            return new wcfLCO.wcfRecuperaLCOSoapClient();
        }

        public static ntcpLCO.IwcfRecuperaClient ObtenerServicioLCO_NetTcp()
        {
            return new ntcpLCO.IwcfRecuperaClient();
        }

        #endregion

        #region Generacion y Timbrado

        public static wsGeneracionTimbrado.wcfRecepcionASMXSoapClient ObtenerServicioGeneracionTimbrado()
        {
            return new wsGeneracionTimbrado.wcfRecepcionASMXSoapClient();
        }

        #endregion

        #region Registro Usuarios

        public static wsRegistroUsuariosLocal.wcfRegistroSoapClient ObtenerServicioRegistroUsuariosLocal()
        {
            return new wsRegistroUsuariosLocal.wcfRegistroSoapClient();
        }

        public static wsRegistroUsuariosSVCLocal.IwcfRegistroSVCClient ObtenerServicioRegistroUsuariosSVCLocal()
        {
            return new wsRegistroUsuariosSVCLocal.IwcfRegistroSVCClient();
        }

        public static wsRegistroUsuariosTest.wcfRegistroSoapClient ObtenerServicioRegistroUsuariosTest()
        {
            return new wsRegistroUsuariosTest.wcfRegistroSoapClient();
        }

        public static wsRegistroUsuariosTestN.wcfRegistroSoapClient ObtenerServicioRegistroUsuariosTestNuevo()
        {
            return new wsRegistroUsuariosTestN.wcfRegistroSoapClient();
        }

        public static wsRegistroUsuariosProduccion.IwcfRegistroSVCClient ObtenerServicioRegistroUsuariosProduccionSVC()
        {
            return new wsRegistroUsuariosProduccion.IwcfRegistroSVCClient();
        }

        public static wsRegistroUsuariosProduccionasmx.wcfRegistroSoapClient ObtenerServicioRegistroUsuariosProduccionAsmx()
        {
            return new wsRegistroUsuariosProduccionasmx.wcfRegistroSoapClient();
        }

        #endregion

        #region Retencion

        public static wsValidaRetencionTest.wcfValidaASMXSoapClient ObtenerServicioValidacionRetencionTest()
        {
            return new wsValidaRetencionTest.wcfValidaASMXSoapClient();
        }

        public static wsRecepcionLocalRetencion.wcfRecepcionASMXSoapClient ObtenerServicioRecepcionRetencionLocal()
        {
            return new wsRecepcionLocalRetencion.wcfRecepcionASMXSoapClient();
        }

        public static wsRecepcionRetencionProduccion.wcfRecepcionASMXSoapClient ObtenerServicioRecepcionRetencionProduccion()
        {
            return new wsRecepcionRetencionProduccion.wcfRecepcionASMXSoapClient();
        }

        public static wsCancelacionRetencionLocal.wcfCancelaASMXSoapClient ObtenerServicioCancelacionRetencionLocal()
        {
            return new wsCancelacionRetencionLocal.wcfCancelaASMXSoapClient();
        }

        #endregion

        #region Carta Manifiesto

        public static wsRecepcionCartaManifiesto.wcfRecepcionASMXSoapClient ObtenerServicioRegistroCartaManifiestoLocal()
        {
            return new wsRecepcionCartaManifiesto.wcfRecepcionASMXSoapClient();
        }

        public static wsRecepcionCartaManifiestoTest.wcfRecepcionManifiestoSoapClient ObtenerServicioRegistroCartaManifiestoTest()
        {
            return new wsRecepcionCartaManifiestoTest.wcfRecepcionManifiestoSoapClient();
        }

        public static wsRecepcionCartaManifiestoProductivo.wcfRecepcionManifiestoSoapClient ObtenerServicioRegistroCartaManifiestoProductivo()
        {
            return new wsRecepcionCartaManifiestoProductivo.wcfRecepcionManifiestoSoapClient();
        }
        #endregion


        public static WSConsultaAlSuper.wcfConsultaTicketSoapClient ObtenerServicioConsultaAlSuperProducktivo()
        {
            return new WSConsultaAlSuper.wcfConsultaTicketSoapClient();
        }
    }
}

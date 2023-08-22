using SolucionPruebas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolucionPruebas.Negocios
{
    public static class BRError
    {
        /// <summary>
        /// Metodo que se encarga de registrar un error
        /// </summary>
        /// <param name="psTipoError">Tipo de Error</param>
        /// <param name="psModulo">Modulo</param>
        /// <param name="psMetodoOrigen">Metodo en que se presento el error</param>
        /// <param name="sMensaje">Mensaje del error</param>
        public static void fnRegistrar(string psTipoError, string psModulo, string psMetodoOrigen, Exception Excepcion)
        {
            AccesoDatos.ADError ADError;
            Entidades.Error ENError;
            try
            {
                ENError = new Error();
                ENError.sTipoError = psTipoError;
                ENError.sMensaje = (Excepcion.Message.Length > 300 ? Excepcion.Message.Substring(0, 300) : Excepcion.Message);
                ENError.sModulo = psModulo;
                ENError.sMetodoOrigen = psMetodoOrigen;
                ENError.dFecha = DateTime.Now;
                ENError.nIdUsuario = 0;
                ENError.sObservaciones = (Excepcion.InnerException != null ? (Excepcion.InnerException.Message.Length > 4000 ? Excepcion.InnerException.Message.Substring(0, 4000) : Excepcion.InnerException.Message) : (Excepcion.Message.Length > 4000 ? Excepcion.Message.Substring(0, 4000) : Excepcion.Message));

                ADError = new AccesoDatos.ADError();
                ADError.fnInsertar(ENError, false, false, false);
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible registrar el error: " + ex.Message);
            }
        }
    }
}

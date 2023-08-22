using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data.SqlClient;
using System.Data;
using System.Text;

/// <summary>
/// Clase de capa de negocios para la pantalla webGlobalCuenta
/// </summary>
public class clsOperacionCuenta
{
    private InterfazSQL giSql;
    private string conCuenta = "conConfiguracion";

    /// <summary>
    /// Retorna los datos fiscales de la sucursal matriz
    /// </summary>
    /// <returns>Retorna un SqlDataReader con los datos fiscales de la matriz</returns>
    public SqlDataReader fnObtenerDatosFiscales(int nidCFD, string sTienda)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);

        giSql.AgregarParametro("@idCFD", nidCFD);
        if(!string.IsNullOrEmpty(sTienda))
            giSql.AgregarParametro("@sTienda", sTienda);

        return giSql.Query("usp_Con_Cuenta_Sel", true);
    }


    /// <summary>
    /// Actualiza la información fiscal de la sucursal matriz
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la sucursal matriz</param>
    /// <param name="psSucursal">Nombre de la sucursal matriz</param>
    /// <param name="psCalle">Nombre de la calle</param>
    /// <param name="psNoExterior">El número exterior del lugar</param>
    /// <param name="psNoInterior">El número interior del lugar</param>
    /// <param name="psColonia">El nombre de la colonia</param>
    /// <param name="psReferencia">Descripción de referencia</param>
    /// <param name="psCodigoPostal">El numero de código postal del área</param>
    /// <param name="psLocalidad">El nombre de la localidad</param>
    /// <param name="psMunicipio">El nombre del municipio</param>
    /// <param name="psIdEstado">Identificador del estado</param>
    /// <returns>Retorna un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnGuardarDatosFiscales(string psIdEstructura, string psSucursal, string psCalle,
                                    string psNoExterior, string psNoInterior, string psColonia,
                                    string psReferencia, string psLocalidad, string psMunicipio,
                                    string psIdEstado, string psCodigoPostal, string psRegimenFiscal)
    {
         giSql = clsComun.fnCrearConexion(conCuenta);

        giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        giSql.AgregarParametro("@sSucursal", psSucursal);
        giSql.AgregarParametro("@sCalle", psCalle);
        if (!string.IsNullOrEmpty(psNoExterior))
            giSql.AgregarParametro("@sNumero_Exterior", psNoExterior);
        if (!string.IsNullOrEmpty(psNoInterior))
            giSql.AgregarParametro("@sNumero_Interior", psNoInterior);
        if (!string.IsNullOrEmpty(psColonia))
            giSql.AgregarParametro("@sColonia", psColonia);
        if (!string.IsNullOrEmpty(psReferencia))
            giSql.AgregarParametro("@sReferencia", psReferencia);
        giSql.AgregarParametro("@sCodigo_Postal", string.Format("{0:00000}", Convert.ToInt32(psCodigoPostal)));
        if (!string.IsNullOrEmpty(psLocalidad))
            giSql.AgregarParametro("@sLocalidad", psLocalidad);
        giSql.AgregarParametro("@sMunicipio", psMunicipio);
        giSql.AgregarParametro("@nId_Estado", psIdEstado);
        if (!string.IsNullOrEmpty(psRegimenFiscal))
            giSql.AgregarParametro("@sRegimen_Fiscal", psRegimenFiscal);

        return giSql.NoQuery("usp_Con_Cuenta_Upd", true);
    }

    /// <summary>
    /// Actualiza los archivos CSD en la base de datos para la sucursal matriz
    /// </summary>
    /// <param name="psIdRfc">Identificador del RFC de la matriz</param>
    /// <param name="vValidadorCertificado">Objeto de validación y manipulación del certificado</param>
    /// <param name="pbKey">Arreglo de bytes del archivo key</param>
    /// <param name="psPassword">Contraseña de cifrado del archivo key</param>
    /// <returns>Retorna un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnGuardarCertificados(string psIdRfc, clsValCertificado vValidadorCertificado)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);

        giSql.AgregarParametro("nId_Rfc", psIdRfc);
        giSql.AgregarParametro("mCertificado", vValidadorCertificado.fnEncriptarCertificado());
        giSql.AgregarParametro("mKey", vValidadorCertificado.fnEncriptarLlave());
        giSql.AgregarParametro("sPassword", vValidadorCertificado.fnEncriptarPassword());
        giSql.AgregarParametro("dFecha_Inicio", vValidadorCertificado.Certificado.NotBefore);
        giSql.AgregarParametro("dFecha_Termino", vValidadorCertificado.Certificado.NotAfter);
        giSql.AgregarParametro("nId_Usuario", "");

        return giSql.NoQuery("usp_Con_Certificado", true);
    }


    /// <summary>
    /// Recupera los datos fiscales del emisor ex
    /// </summary>
    /// <returns></returns>
    public SqlDataReader fnObtenerDatosFiscalesEx()
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("nId_Contribuyente", "");
        return giSql.Query("usp_Con_Cuenta_Ex_Sel", true);
    }


    /// <summary>
    /// Actualiza los datos fiscales ex
    /// </summary>
    /// <param name="nId_Contribuyente"></param>
    /// <param name="psCalle"></param>
    /// <param name="psMunicipio"></param>
    /// <param name="psIdEstado"></param>
    /// <param name="psIdPais"></param>
    /// <param name="psCodigoPostal"></param>
    /// <returns></returns>
    public int fnGuardarDatosFiscalesEx(int nId_Contribuyente, string psCalle,
                                string psMunicipio,
                                string psIdEstado, string psIdPais, string psCodigoPostal)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);

        giSql.AgregarParametro("nId_Contribuyente", nId_Contribuyente);
        giSql.AgregarParametro("@sCalle", psCalle);
        //giSql.AgregarParametro("@sCodigo_Postal", string.Format("{0:00000}", Convert.ToInt32(psCodigoPostal)));
        giSql.AgregarParametro("sCodigo_Postal", psCodigoPostal);
        giSql.AgregarParametro("@sMunicipio", psMunicipio);
        giSql.AgregarParametro("@nId_Estado", psIdEstado);
        giSql.AgregarParametro("@nId_Pais", psIdPais);

        return giSql.NoQuery("usp_Con_Cuenta_Ex_Upd", true);
    }




    /// <summary>
    /// Actualiza los archivos CSD en la base de datos para la sucursal matriz
    /// </summary>
    /// <param name="psIdRfc">Identificador del RFC de la matriz</param>
    /// <param name="vValidadorCertificado">Objeto de validación y manipulación del certificado</param>
    /// <param name="pbKey">Arreglo de bytes del archivo key</param>
    /// <param name="psPassword">Contraseña de cifrado del archivo key</param>
    /// <returns>Retorna un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnGuardarCertificadosCobro(string psIdRfc, clsValCertificado vValidadorCertificado)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);

        giSql.AgregarParametro("nId_Rfc", psIdRfc);
        giSql.AgregarParametro("mCertificado", vValidadorCertificado.fnEncriptarCertificado());
        giSql.AgregarParametro("mKey", vValidadorCertificado.fnEncriptarLlave());
        giSql.AgregarParametro("sPassword", vValidadorCertificado.fnEncriptarPassword());
        giSql.AgregarParametro("dFecha_Inicio", vValidadorCertificado.Certificado.NotBefore);
        giSql.AgregarParametro("dFecha_Termino", vValidadorCertificado.Certificado.NotAfter);
        giSql.AgregarParametro("nId_Usuario", "");
        return giSql.NoQuery("usp_Con_Certificado_Cobro", true);
    }

    /// <summary>
    /// Retorna los datos fiscales de la matriz pertenecientes a la sucursal
    /// </summary>
    /// <returns>Retorna un SqlDataReader con los datos fiscales de la Matriz</returns>
    public SqlDataReader fnObtenerDatosFiscalesMatriz(int pnIdsucursal, string sNoTienda)
    {
            //int idContribuyente = 0;
            giSql = clsComun.fnCrearConexion(conCuenta);

            giSql.AgregarParametro("@nIdSucursal", pnIdsucursal);
            giSql.AgregarParametro("@sNoTienda", sNoTienda);

            return giSql.Query("usp_Con_RfcSuc_Sel", true);
    }

    //public DataTable fnCargarComplementos()
    //{
    //    DataTable dtAuxiliar = new DataTable();
    //    giSql = clsComun.fnCrearConexion(conCuenta);

    //    giSql.Query("usp_ctp_Complementos_Sel", true, ref dtAuxiliar);
    //    return dtAuxiliar;
    //}


    public string fnRetornaRutaComplemento(int nIdComplemento)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            giSql.AgregarParametro("@nIdComplemento", nIdComplemento);
            string Resultado = Convert.ToString(giSql.TraerEscalar("usp_Cfd_ObtieneRutaComplemento_sel", true));
            return Resultado;
        }
        catch(Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }
}

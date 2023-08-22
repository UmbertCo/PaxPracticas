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
    public SqlDataReader fnObtenerDatosFiscales()
    {
        //int idContribuyente = 0;
        giSql = clsComun.fnCrearConexion(conCuenta);
        ////Se obtiene contribuyente padre, si es que el usuario es hijo del nodo
        //giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        //SqlDataReader sdrUsuario = giSql.Query("usp_Ctp_DatosPadre_sel", true);
        //if (sdrUsuario != null && sdrUsuario.HasRows && sdrUsuario.Read())
        //{
        //    idContribuyente = Convert.ToInt32(sdrUsuario["id_contribuyente"].ToString());
        //}
        //else
        //{
        //    idContribuyente = clsComun.fnUsuarioEnSesion().id_contribuyente;
        //}
        //sdrUsuario.Close();
        //giSql.LimpiarParametros();
        giSql.AgregarParametro("nId_Contribuyente", clsComun.fnUsuarioEnSesion().id_contribuyente);

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
        giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);

        return giSql.NoQuery("usp_Con_Certificado", true);
    }

    /// <summary>
    /// Autentica al usuario y actualiza el estatus del usuario en sesión para ponerlo en estado de baja.
    /// </summary>
    /// <param name="psPass">Password encriptado del usuario</param>
    /// <param name="psUserName">Clave del usuario</param>
    /// <param name="psEmail">Email del usuario</param>
    /// <returns>booleano indicando si la baja tuvo éxito</returns>
    public bool fnEliminarCuenta(string psPass, string psUserName, string psEmail)
    {
        clsInicioSesionSolicitudReg reg = new clsInicioSesionSolicitudReg();

        try
        {
            DataTable dtAuxiliar = reg.buscarUsuario(psUserName);
            string sRawPass = Utilerias.Encriptacion.Classica.Desencriptar(psPass);

            if (sRawPass == Utilerias.Encriptacion.Classica.Desencriptar(dtAuxiliar.Rows[0]["password"].ToString()))
                return reg.actualizaEstadoActual(psUserName, psEmail, 'B');
            else
                return false;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return false;
        }
    }

    public bool fnConstruirCuerpoCorreo(string psPass)
    {
        clsInicioSesionSolicitudReg reg = new clsInicioSesionSolicitudReg();
        clsGeneraEMAIL email = new clsGeneraEMAIL();
        DataTable dtAuxiliar = reg.buscarUsuario(clsComun.fnUsuarioEnSesion().userName);

        //Verificamos la contraseña
        if (psPass != Utilerias.Encriptacion.Classica.Desencriptar(dtAuxiliar.Rows[0]["password"].ToString()))
            throw new Exception("No se pudo dar de baja la cuenta del usuario");

        string sName = clsComun.fnUsuarioEnSesion().userName;
        string sEmail = clsComun.fnUsuarioEnSesion().email;
        string sPass = Utilerias.Encriptacion.Classica.Encriptar(psPass);
        string servidor = clsComun.ObtenerParamentro("urlHostCosto");

        string url = sPass + ":" + sName + ":" + sEmail;
        Byte[] bBytes = Utilerias.Encriptacion.DES3.Encriptar(System.Text.Encoding.UTF8.GetBytes(url));

        StringBuilder url64 = new StringBuilder();
        foreach (byte b in bBytes)
        {
            url64.Append(b);
            url64.Append(",");
        }

        //enviamos correo de autenticación
        return  email.EnviarCorreo(
            sEmail,
            Resources.resCorpusCFDIEs.varSubjectBajaCuenta,
            string.Format(Resources.resCorpusCFDIEs.varBajaMailBody, sName, sEmail, servidor, url64.ToString().Trim(',')));

        
    }

    /// <summary>
    /// Recupera los datos fiscales del emisor ex
    /// </summary>
    /// <returns></returns>
    public SqlDataReader fnObtenerDatosFiscalesEx()
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("nId_Contribuyente", clsComun.fnUsuarioEnSesion().id_contribuyente);
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
    public int fnGuardarCertificadosCobro(string psIdRfc, clsValCertificado vValidadorCertificado, int pId_Estructura)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);

        giSql.AgregarParametro("nId_Rfc", psIdRfc);
        giSql.AgregarParametro("mCertificado", vValidadorCertificado.fnEncriptarCertificado());
        giSql.AgregarParametro("mKey", vValidadorCertificado.fnEncriptarLlave());
        giSql.AgregarParametro("sPassword", vValidadorCertificado.fnEncriptarPassword());
        giSql.AgregarParametro("dFecha_Inicio", vValidadorCertificado.Certificado.NotBefore);
        giSql.AgregarParametro("dFecha_Termino", vValidadorCertificado.Certificado.NotAfter);
        giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        giSql.AgregarParametro("nId_Estructura", pId_Estructura);
        return giSql.NoQuery("usp_Con_Certificado_Cobro", true);
    }

    /// <summary>
    /// Retorna los datos fiscales de la sucursal seleccionada
    /// </summary>
    /// <returns>Retorna un SqlDataReader con los datos fiscales de la sucursal</returns>
    public SqlDataReader fnObtenerDatosFiscalesSuc(int pnIdsucursal)
    {
            //int idContribuyente = 0;
            giSql = clsComun.fnCrearConexion(conCuenta);

            giSql.AgregarParametro("@nIdSucursal", pnIdsucursal);

            return giSql.Query("usp_Con_RfcSuc_Sel", true);
    }
}

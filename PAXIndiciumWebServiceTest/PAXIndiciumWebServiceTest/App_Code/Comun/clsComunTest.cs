using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Configuration;

/// <summary>
/// Clase encargada de proporcionar funciones de ayuda a todas las demás clases.
/// </summary>
public class clsComunTest
{
    private static DataTable gdtAuxiliar;
    
    /// <summary>
    /// Inserta un nuevo registro de acuse del PAC
    /// </summary>
    /// <param name="p_idcte">identificador del cliente</param>
    /// <param name="p_UUID">UUID del comprobante</param>
    ///  <param name="sFecha">fecha de envio</param>
    ///  <param name="p_CorreoElectronico">correo electronico</param>
    ///  <param name="p_tipoAcuse">tipo de acuse</param>
    /// <returns></returns>
    public static Int32 fnInsertaAcusePAC(int p_idcte, string p_UUID, string p_Acuse, DateTime sFecha, string p_idcodigo, string p_Soap, string origen, string p_CorreoElectronico)
    {
        Int32 Resultado = 0;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conControlTest"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Acuse_Indicium_PAC_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 200;
                    cmd.Parameters.AddWithValue("@nid_Contribuyente", p_idcte);
                    cmd.Parameters.AddWithValue("@sUUID", p_UUID);
                    cmd.Parameters.AddWithValue("@sAcuse", p_Acuse);
                    cmd.Parameters.AddWithValue("@sFecha", sFecha);
                    cmd.Parameters.AddWithValue("@nid_codigo", p_idcodigo);
                    cmd.Parameters.AddWithValue("@sSoap", p_Soap);
                    cmd.Parameters.AddWithValue("@sOrigen", origen);
                    cmd.Parameters.AddWithValue("@sMail", p_CorreoElectronico);
                    con.Open();

                    Resultado = Convert.ToInt32(cmd.ExecuteNonQuery());
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }
        return Resultado;
    }

    /// <summary>
    /// Inserta un nuevo registro de acuse del emitido por el SAT
    /// </summary>
    ///  <param name="p_UUID">UUID del comprobante</param>
    ///  <param name="sFecha">fecha de envio</param>
    ///  <param name="p_Acuse">xml con el acuse que emitio el SAT</param>
    ///  <param name="p_idcodigo">tipo de codigo que se genero</param>
    /// <returns></returns>
    public static Int32 fnInsertaAcuseSAT(string sId_Contribuyente, string p_UUID, string p_Acuse, DateTime sFecha, string p_idcodigo, string p_Soap, string origen)
    {
        Int32 resultado = 0;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conControlTest"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Acuse_Indicium_SAT_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 200;
                    cmd.Parameters.AddWithValue("@nid_Contribuyente", sId_Contribuyente);
                    cmd.Parameters.AddWithValue("@sUUID", p_UUID);
                    cmd.Parameters.AddWithValue("@sAcuse", p_Acuse);
                    cmd.Parameters.AddWithValue("@sFecha", sFecha);
                    cmd.Parameters.AddWithValue("@nid_codigo", p_idcodigo);
                    cmd.Parameters.AddWithValue("@sSoap", p_Soap);
                    cmd.Parameters.AddWithValue("@sOrigen", origen);
                    con.Open();

                    resultado = Convert.ToInt32(cmd.ExecuteNonQuery());
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;
    }

    /// <summary>
    /// Obtiene el valor del parametro a buscar.
    /// </summary>
    /// <param name="sParametro">Nombre del parametros a buscar</param>
    /// <returns>Cadena con le valor del parámetro</returns>
    public static string fnObtenerParametro(string sParametro)
    {
        string nRetorno = string.Empty;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conControlTest"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_BuscarParametro_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 200;
                    cmd.Parameters.AddWithValue("sParametro", sParametro);
                    con.Open();
                    nRetorno = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                nRetorno = null;
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }
        return nRetorno;
    }

    /// <summary>
    /// Función que regresa la descripción del error
    /// </summary>
    /// <param name="p_idcodigo"></param>
    /// <param name="p_sTipo"></param>
    /// <returns></returns>
    public static string fnRecuperaErrorSAT(string p_idcodigo, string p_sTipo)
    {
        string Resultado = string.Empty;

        switch (p_idcodigo.Trim())
        {
            case "301":
                Resultado = "301 - XML mal formado.";
                break;
            case "97":
                Resultado = "97 - No hay créditos disponibles.";
                break;
            case "406":
                Resultado = "406 - El nombre de documento no corresponde a ningúno del sistema.";
                break;
            case "333":
                Resultado = "333 - El xml no cumple con el esquema de Hacienda.";
                break;
            case "570":
                Resultado = "570 - No se pudó recuperar el certificado del comprobante.";
                break;
            case "306":
                Resultado = "306 - El certificado no es de tipo CSD.";
                break;
            case "308":
                Resultado = "308 - Certificado no expedido por el SAT.";
                break;
            case "N-504":
                Resultado = "N-504 - La estructura del comprobante recibido no es válida.";
                break;
            case "307":
                Resultado = "307 - El CFDI contiene un timbre previo.";
                break;
            case "303":
                Resultado = "303 - Sello no corresponde a emisor o caduco.";
                break;
            case "304":
                Resultado = "304 - Certificado revocado o caduco.";
                break;
            case "305":
                Resultado = "305 - La fecha de emisión no está dentro de la vigencia del CSD del Emisor.";
                break;
            case "403":
                Resultado = "403 - Que la fecha de emisión sea posterior al 01 de Enero 2011.";
                break;
            case "402":
                Resultado = "402 - Que exista el RFC del emisor conforme al régimen autorizado (Lista de validación de régimen) LCO.";
                break;
            case "401":
                Resultado = "401 - Que el rango de la fecha de generación no sea mayor a 72 horas para la emisión del timbre.";
                break;
            case "302":
                Resultado = "302 - Sello mal formado o inválido.";
                break;
            case "622":
                Resultado = "622 - El servicio no esta disponible.";
                break;
            case "817":
                Resultado = "817 - No se pudo generar el sello del PAC.";
                break;
            case "98":
                Resultado = "98 - El comprobante ya se encuentra en la base de datos.";
                break;
            case "999":
                Resultado = "999 - No se pudo registrar el comprobante.";
                break;
            case "95":
                Resultado = "95 - Usuario inexistente.";
                break;
            case "90":
                Resultado = "90 - Los datos del usuario son requeridos.";
                break;
            case "96":
                Resultado = "96 - Usuario o contraseña incorrecta.";
                break;
            case "91":
                Resultado = "91 - El usuario esta en estado pendiente.";
                break;
            case "0":
                Resultado = "0 - Sin Errores.";
                break;
            case "92":
                Resultado = "92 - El usuario esta en estado bloqueado.";
                break;
            case "93":
                Resultado = "93 - El usuario esta en estado expirado.";
                break;
            case "94":
                Resultado = "94 - El usuario esta en estado por cambiar contraseña.";
                break;
            case "201":
                Resultado = "201 - UUID Cancelado.";
                break;
            case "202":
                Resultado = "202 - UUID Previamente Cancelado.";
                break;
            case "203":
                Resultado = "203 - UUID No corresponde al emisor.";
                break;
            case "204":
                Resultado = "204 - UUID No aplicable para Cancelación, intente en 10 minutos.";
                break;
            case "205":
                Resultado = "205 - UUID No existente, intente en 10 minutos.";
                break;

            default:
                Resultado = "999 - Intente de nuevo.";
                break;

        }

        return Resultado;
    }
    
    /// <summary>
    /// Función que se encarga de generar el acuse de recepción 
    /// </summary>
    /// <param name="psComprobante">Comprobante</param>
    /// <param name="psTipoDocumento">Tipo de Documento</param>
    /// <param name="pnId_Estructura">ID de la Estructura</param>
    /// <param name="sNombre">Nombre</param>
    /// <param name="sContraseña">Contraseña</param>
    /// <returns></returns>
    public static string fnRequestRecepcion(string psComprobante, string psTipoDocumento,
        string pnId_Estructura, string sNombre, string sContraseña)
    {
        //Respuesta Request *************************************************************
        string strSoapMessage =
        "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope\">" +
            "<s:Header>" +
            "<Action s:mustUnderstand=\"1\" xmlns=\"http://schemas.microsoft.com/ws/2005/05/addressing/none\">https://paxfacturacion.com.mx/IwcfRecepcion/fnEnviarXML</Action>" +
            "</s:Header>" +
            "<s:Body>" +
            "<fnEnviarXML xmlns=\"https://paxfacturacion.com.mx\">" +
                "<psComprobante>" + psComprobante + "</psComprobante>" +
                "<psTipoDocumento>" + psTipoDocumento + "</psTipoDocumento>" +
                "<pnId_Estructura>" + pnId_Estructura + "</pnId_Estructura>" +
                "<sNombre>" + sNombre + "</sNombre>" +
            //"<sContraseña>" + sContraseña + "</sContraseña>" +
            "</fnEnviarXML>" +
            "</s:Body>" +
        "</s:Envelope>";
        return strSoapMessage;
        //Respuesta Request *************************************************************
    }

    /// <summary>
    /// Regresa el response armado para la recepcion
    /// </summary>
    /// <returns></returns>
    public static string fnResponseRecepcion(string fnEnviarXMLResult)
    {
        //Respuesta Response*************************************************************
        string strSoapMessage =
        "<s:Acuse xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope\">" +
          "<s:Header />" +
          "<s:Body>" +
            "<fnEnviarXMLResponse xmlns=\"https://paxfacturacion.com.mx\">" +
              "<fnEnviarXMLFecha>" + DateTime.Now.ToString("s") + "</fnEnviarXMLFecha>" +
              "<fnEnviarXMLResult>" + fnEnviarXMLResult + "</fnEnviarXMLResult>" +
            "</fnEnviarXMLResponse>" +
          "</s:Body>" +
        "</s:Acuse>";

        return strSoapMessage;
        //Respuesta Response*************************************************************
    }

    /// <summary>
    /// Verifica la fecha de emisión no es posterior al 01 de enero 2011
    /// </summary>
    /// <returns></returns>
    public static bool fnRevisarFechaNoPosterior(DateTime pdFecha)
    {
        bool retorno = false;

        try
        {
            if (pdFecha >= Convert.ToDateTime(fnObtenerParametro("FechaPosterior_v3")))
                retorno = true;
            else
                retorno = false;
        }
        catch
        {
            if (pdFecha <= Convert.ToDateTime("2011-01-01"))
                retorno = true;
            else
                retorno = false;
        }

        return retorno;
    }

    /// <summary>
    /// Función que se encarga de evaluar un valor de una expresión dada
    /// </summary>
    /// <param name="sValor">Valor</param>
    /// <param name="expresion">Expresión REgular</param>
    /// <returns></returns>
    public static bool fnValidaExpresion(string sValor, string expresion)
    {
        bool bRetorno = false;

        if (Regex.IsMatch(sValor, expresion))
        {
            bRetorno = true;
        }

        return bRetorno;
    }
}

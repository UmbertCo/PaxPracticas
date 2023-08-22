using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using Utilerias.SQL;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Collections;


/// <summary>
/// Clase encargada de proporcionar funciones de ayuda a todas las demás clases.
/// </summary>
public class clsComun
{
    private static InterfazSQL giSql;
    private static DataTable gdtAuxiliar;
    
    #region Franquicias PAX (Marco santana )

    /// <summary>
    /// Genera un nuevo objeto de conexión a la base de datos
    /// </summary>
    /// /// <param name="psNombreConexion">Nombre de la cadena de conexión guardada en el web config</param>
    /// <returns>objeto InterfazSQL</returns>
    public static InterfazSQL fnCrearConexion(string psNombreConexion)
    {
        try
        {
            string cadena = System.Configuration.ConfigurationManager.ConnectionStrings[psNombreConexion].ConnectionString;
            return new InterfazSQL(psNombreConexion);//Utilerias.Encriptacion.Base64.DesencriptarBase64(psNombreConexion));
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    /// <summary>
    /// Valida que la estructura no esta asignada a un usuario
    /// </summary>
    /// <param name="nidEstructura"></param>
    /// <returns></returns>
    public static bool fnValidarEstructura(int nidEstructura)
    {
        giSql = clsComun.fnCrearConexion("conConfiguracion");

        giSql.AgregarParametro("@nidEstructura", nidEstructura);

        int valor = Convert.ToInt32(giSql.TraerEscalar("usp_val_Estructura_Usuario_rel", true));

        return (valor > 0);
    }

    public static int fnValidaEstructuraTicket(int nidEstructura)
    {
        int nRetorno = 0;
        try
        {
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            giSql.AgregarParametro("@nidEstructura", nidEstructura);

            nRetorno = Convert.ToInt32(giSql.TraerEscalar("usp_Valida_Estructura_Ticket_Sel", true));
        }
        catch 
        {
            
        }

        return nRetorno;
    }



    public static int fnDesbloquear(string sUsuario, string sPassword)
    {

        giSql = clsComun.fnCrearConexion("conConfiguracion");

        giSql.AgregarParametro("@Clave_Usuario", sUsuario);
        giSql.AgregarParametro("@Password", sPassword);

        int valor = Convert.ToInt32(giSql.TraerEscalar("usp_Desbloquea_Usuario_Sel", true));

        return valor;
    }

    
    /// <summary>
    /// Recupera datos del emisor para timbrar 
    /// </summary>
    /// <param name="nidEstructura"></param>
    /// <returns></returns>
    public static DataTable fnRecuperaridestructura(int nidEstructura, int nidcfd)
    {
        DataTable dtAuxiliar = new DataTable();

        giSql = clsComun.fnCrearConexion("conConfiguracion");

        if(nidEstructura > 0)
            giSql.AgregarParametro("@nidEstructura", nidEstructura);

        if(nidcfd > 0)
            giSql.AgregarParametro("@nidcfd", nidcfd);

        giSql.Query("usp_rec_EstructuraCobro_Sel",true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Valida que la Tienda Cuente con el estado del RFC activo
    /// </summary>
    /// <param name="nidEstructura"></param>
    /// <returns></returns>
    public static bool fnValidarRFC(int nidEstructura)
    {
        giSql = clsComun.fnCrearConexion("conConfiguracion");

        giSql.AgregarParametro("@nidEstructura", nidEstructura);

        int valor = Convert.ToInt32(giSql.TraerEscalar("usp_val_RFC_Sel", true));

        return (valor > 0);
    }

    /// <summary>
    /// Valida que el Emisor Cuente con Tiembres disponibles
    /// </summary>
    /// <returns></returns>
    public static int fnValidarTimbresDisponibles( int nidEstructura)
    {
        giSql = clsComun.fnCrearConexion("conConfiguracion");

        giSql.AgregarParametro("@nidEstructura", nidEstructura);

        int valor = Convert.ToInt32(giSql.TraerEscalar("usp_valida_creditos_Cobro", true));

        return  valor;
    }

    /// <summary>
    /// Devuelve el catalogo de sucursales para este usuario
    /// </summary>
    /// <param name="pbDrop">Indica si también se quiere devolver la matriz</param>
    /// <returns></returns>
    public static DataTable LlenarDropSucursales(bool pbMatriz)
    {
        giSql = clsComun.fnCrearConexion("conConfiguracion");
        DataTable gdtAuxiliar = new DataTable("Sucursales");
        int idusr = fnUsuarioEnSesion().id_usuario;
        giSql.AgregarParametro("nId_Usuario", idusr);
        giSql.AgregarParametro("bDrop", pbMatriz);
        giSql.Query("usp_Con_Sucursal_Sel", true, ref gdtAuxiliar);

        return gdtAuxiliar;
    }

    /// <summary>
    /// Retorna un objeto con la información del usuario en sesión
    /// </summary>
    /// <returns>Objeto clsInicioSesionUsuario</returns>
    public static clsInicioSesionUsuario fnUsuarioEnSesion()
    {
        try
        {
            return (clsInicioSesionUsuario)System.Web.HttpContext.Current.Session["objUsuario"];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Llema el combo con las matriz cuando el nidpadre es null
    /// de lo contrario llena el gridview con las sucursales hijas de la matriz
    /// </summary>
    /// <param name="nId_usuario"></param>
    /// <returns></returns>
    public static DataTable fnLlenarSucursales(int nId_usuario, int nidpadre)
    {
        giSql = fnCrearConexion("conConfiguracion");
        gdtAuxiliar = new DataTable();

        try
        {
            giSql.AgregarParametro("@nId_Usuario", nId_usuario);
            if(nidpadre>0)
                giSql.AgregarParametro("@idpadre", nidpadre);
            giSql.Query("usp_Obtener_Sucursales_Sel", true, ref gdtAuxiliar);
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }

        return gdtAuxiliar;
    }

    /// <summary>
    /// Devuelve el catalogo de paises
    /// </summary>
    /// <returns>DataTable con los paises disponibles</returns>
    public static DataTable fnLlenarDropPaises()
    {
        giSql = fnCrearConexion("conConfiguracion");
        gdtAuxiliar = new DataTable();

        try
        {
            giSql.Query("usp_Con_Obtener_Paises_Sel", true, ref gdtAuxiliar);
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }

        return gdtAuxiliar;
    }

    /// <summary>
    /// Devuelve las Reglas de Negocio
    /// </summary>
    /// <returns>DataTable con las Reglas disponibles</returns>
    public static DataTable llenarRegla()
    {
        giSql = fnCrearConexion("conConfiguracion");
        gdtAuxiliar = new DataTable();

        try
        {
            giSql.Query("usp_obtiene_Reglas_Sel", true, ref gdtAuxiliar);
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }

        return gdtAuxiliar;
    }

    /// <summary>
    /// Devuelve una parte del catalogo de estados, filtrando por el país indicado
    /// </summary>
    /// <param name="idPais">ID del país para el cual se quiere recuperar sus estados</param>
    /// <returns>DataTable con los estados filtrados por país</returns>
    public static DataTable fnLlenarDropEstados(string psIdPais)
    {
        giSql = fnCrearConexion("conConfiguracion");
        gdtAuxiliar = new DataTable();

        try
        {
            giSql.AgregarParametro("nId_Pais", psIdPais);
            giSql.Query("usp_Con_Obtener_Estados_Sel", true, ref gdtAuxiliar);
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }

        return gdtAuxiliar;
    }

    /// <summary>
    /// fnRecuperaTicket
    /// </summary>
    /// Recuepra el ticket solicitado
    /// <param name="noTicket"></param>
    /// <param name="sucursal"></param>
    /// <param name="total"></param>
    /// <param name="dFecha_Inicio"></param>
    /// <returns></returns>
    public static DataTable fnRecuperaTicket(int nidEstructura, int noTicket, string sucursal, string total, string dFecha_Inicio)
    {
        giSql = clsComun.fnCrearConexion("conConfiguracion");
        DataTable gdtAuxiliar = new DataTable("ticket");

        giSql.AgregarParametro("@nidEstructura", nidEstructura);
        giSql.AgregarParametro("@noTicket", noTicket);
        giSql.AgregarParametro("@sucursal", sucursal);
        giSql.AgregarParametro("@total", total);
        giSql.AgregarParametro("@dFecha_Inicio", dFecha_Inicio);//"yyyyMMdd HH:mm:ss"));

        giSql.Query("usp_con_Ticket_Sel", true, ref gdtAuxiliar);

        return gdtAuxiliar;
    }

    /// <summary>
    /// Recupera la regla de negocio según sucursal
    /// </summary>
    /// <param name="sSucursal"></param>
    /// <returns></returns>
    public static DataTable fnRecuperaReglaDeNegocio(int idTienda)
    {
        DataTable dtAuxiliar = new DataTable();
        try
        {
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            giSql.AgregarParametro("@idTienda", idTienda);

            giSql.Query("usp_con_Regla_Sucursal_Sel", true, ref dtAuxiliar);
        }
        catch 
        {
        }

        return dtAuxiliar;
    }

    /// <summary>
    /// Actualiza el estado del Ticket despues de ser timbrado
    /// </summary>
    /// <param name="id_ticket"></param>
    /// <param name="id_cfd"></param>
    /// <returns></returns>
    public static int fnActualziaTicket(int id_ticket, int id_cfd)
    {
        int retorno = 0;

        giSql = clsComun.fnCrearConexion("conConfiguracion");

        giSql.AgregarParametro("@id_Ticket", id_ticket);
        giSql.AgregarParametro("@id_cfd", id_cfd);

        retorno = Convert.ToInt32(giSql.NoQuery("usp_con_Ticket_Upd", true));

        return retorno;
    }

    /// <summary>
    /// Recupera el RFC del Receptor
    /// </summary>
    /// <param name="sRfcs"></param>
    /// <param name="nid_estructura"></param>
    /// <returns></returns>
    public static DataTable fnRecuperaReceptor(string sRfcs, int nid_estructura)
    {
        DataTable dtAuxiliar = new DataTable("DatosReceptor");

        giSql = clsComun.fnCrearConexion("conConfiguracion");

        giSql.AgregarParametro("@nId_Estructura", nid_estructura);
        giSql.AgregarParametro("@sRfc", sRfcs);

        giSql.Query("usp_Cli_ReceptoresSuc_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Agrega los datos del Receptor y se ligan a la sucursal despues de haber timbrado el comprobante
    /// </summary>
    /// <param name="nid_estructura"></param>
    /// <param name="sRFC"></param>
    /// <param name="sRazonSocial"></param>
    /// <param name="sPais"></param>
    /// <param name="sEstado"></param>
    /// <param name="sMunicipio"></param>
    /// <param name="sLocalidad"></param>
    /// <param name="sCalle"></param>
    /// <param name="sNoExterior"></param>
    /// <param name="sNoInterior"></param>
    /// <param name="sColonia"></param>
    /// <param name="nCodPostal"></param>
    /// <param name="sTelefono"></param>
    /// <param name="sEmail"></param>
    /// <param name="nModif"></param>
    public static void fnAgregarReceptor(int nid_estructura, string sRFC, string sRazonSocial, string sPais, string sEstado, string sMunicipio, string sLocalidad,
        string sCalle, string sNoExterior, string sNoInterior, string sColonia, int nCodPostal, string sTelefono, string sEmail, int nModif)
    {
        try
        {
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            giSql.AgregarParametro("@id_estructura", nid_estructura);
            giSql.AgregarParametro("@rfc_receptor", sRFC);
            giSql.AgregarParametro("@nombre_receptor", sRazonSocial);
            giSql.AgregarParametro("@pais", sPais);
            giSql.AgregarParametro("@estado", sEstado);
            giSql.AgregarParametro("@municipio", sMunicipio);
            if (!string.IsNullOrEmpty(sLocalidad))
                giSql.AgregarParametro("@localidad", sLocalidad);
            giSql.AgregarParametro("@calle", sCalle);
            if (!string.IsNullOrEmpty(sNoExterior))
                giSql.AgregarParametro("@numero_exterior", sNoExterior);
            if (!string.IsNullOrEmpty(sNoInterior))
                giSql.AgregarParametro("@numero_interior", sNoInterior);
            if (!string.IsNullOrEmpty(sColonia))
                giSql.AgregarParametro("@colonia", sColonia);
            if (!string.IsNullOrEmpty(Convert.ToString(nCodPostal)))
                giSql.AgregarParametro("@codigo_postal", nCodPostal);
            if (!string.IsNullOrEmpty(Convert.ToString(sTelefono)))
                giSql.AgregarParametro("@sTelefono", sTelefono);
            if (!string.IsNullOrEmpty(Convert.ToString(sEmail)))
                giSql.AgregarParametro("@sEmail", sEmail);
            giSql.AgregarParametro("@nMofi", nModif);


            giSql.Query("usp_con_Receptor_Ins", true);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Muestra el Menu principal
    /// </summary>
    /// <returns></returns>
    public static DataTable fnMostrarMenu()
    {
        try
        {
            DataTable dtMenu = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            giSql.Query("usp_mos_menu_sel", true, ref dtMenu);

            return dtMenu;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// Obtener Paramentro Correo Obtiene el valor del parametro para el correo.
    /// </summary>
    /// <param name="sParametro">Nombre del parametros a buscar</param>
    /// <returns>Cadena con le valor del parámetro</returns>
    public static string ObtenerParamentro(string sParametro)
    {

        string nRetorno = string.Empty;

        try
        {
            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conConfiguracion");

            conexion.AgregarParametro("sParametro", sParametro);

            nRetorno = (string)conexion.TraerEscalar("usp_Ctp_BuscarParametro_Sel", true);
            conexion.conexion.Close();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return nRetorno;
    }

    /// <summary>
    /// Se Agrega el menu lateral para la parte adimnistrativa
    /// </summary>
    /// <param name="sNombreMostrar"></param>
    /// <param name="sUrl"></param>
    /// <param name="nId_estructura"></param>
    public static void fnAgregarMenuLateral(string sNombreMostrar, byte[] byImg, int nId_estructura, byte[] BytesTicket)
    {
        try
        {
            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conConfiguracion");
            conexion.AgregarParametro("sNombre", sNombreMostrar);
            if (byImg.Length > 0)
                conexion.AgregarParametro("sUrl", byImg);
            conexion.AgregarParametro("nIdEstructura", nId_estructura);
            if (BytesTicket.Length > 0)
                conexion.AgregarParametro("BytesTicket", BytesTicket);

            conexion.Query("usp_Ctp_Menu_Agregar_Ins", true);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Obtiene el url del logo a partir del CFD
    /// </summary>
    /// <param name="nIdCfd">Id del CFD</param>
    /// <returns></returns>
    public static byte[] ObtenerUrlLogo(int nIdCfd)
    {
        byte[] nRetorno = {};

        try
        {
            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conConfiguracion");

            conexion.AgregarParametro("nIdCfd", nIdCfd);

            nRetorno = (byte[])conexion.TraerEscalar("usp_Cfd_Obtener_Url_Logo_Cfd_sel", true);
            conexion.conexion.Close();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return nRetorno;
    }

    /// <summary>
    /// Obtiene el Nodo Principal
    /// </summary>
    /// <param name="nIdEstructura"></param>
    /// <returns></returns>
    public static DataTable fnObtnerNodoRaiz(int nIdEstructura)
    {
        DataTable dtDatos = new DataTable();
        try
        {
            giSql = clsComun.fnCrearConexion("conConfiguracion");
            giSql.AgregarParametro("@nIdEstructura", nIdEstructura);

            giSql.Query("usp_mos_nombre_nodeprincipal_sel",true, ref dtDatos);

            return dtDatos;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// Obtiene el menu adminstrativo
    /// </summary>
    /// <param name="nIdEstructura"></param>
    /// <returns></returns>
    public static DataTable fnObtenerMenu(int nIdEstructura)
    {
        try
        {
            DataTable dtMenu = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");
            giSql.AgregarParametro("nIdEstructura", nIdEstructura);
            giSql.Query("usp_mos_menu_IdEstructura_sel", true, ref dtMenu);

            return dtMenu;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// Obtiene los datos de la publicidad para modificar
    /// </summary>
    /// <param name="idPublicidad"></param>
    /// <returns></returns>
    public static DataTable fnObtenerPublicidadSelec(int idPublicidad)
    {
        try
        {
            DataTable dtMenu = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            giSql.AgregarParametro("@idPublicidad", idPublicidad);

            giSql.Query("usp_Obtiene_Datos_Publicidad_Sel", true, ref dtMenu);

            return dtMenu;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            return null;
        }
    }

    /// <summary>
    /// Muestra los datos de publicidad
    /// </summary>
    /// <returns></returns>
    public static DataTable fnObtenerPublicidad()
    {
        try
        {
            DataTable dtMenu = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            giSql.Query("usp_Obtiene_Publicidad_Sel", true, ref dtMenu);

            return dtMenu;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            return null;
        }
    }

    /// <summary>
    /// Cambia el esta
    /// </summary>
    /// <param name="nEstructura"></param>
    public static void fnBajaPublicidad(int nEstructura)
    {
        try
        {
            giSql = clsComun.fnCrearConexion("conConfiguracion");
            giSql.AgregarParametro("@nEstructura", nEstructura);

            giSql.NoQuery("usp_Publicidad_Del", true);
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Recuperamos la serie y folio en caso de tener asignados
    /// </summary>
    /// <returns></returns>
    public static DataTable fnObtenerSerieFolio(int nEstructura, string sNoTienda)
    {
        try
        {
            DataTable dtSerieFolio = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            giSql.AgregarParametro("@nEstructura", nEstructura);
            if(!string.IsNullOrEmpty(sNoTienda))
                giSql.AgregarParametro("@sNoTienda", sNoTienda);

            giSql.Query("usp_Obtener_SerioFolio_Sel", true, ref dtSerieFolio);

            return dtSerieFolio;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            return null;
        }
    }

    /// <summary>
    /// Agremos los datos de la publicidad
    /// </summary>
    /// <param name="id_Publicidad"></param>
    /// <param name="sTitulo"></param>
    /// <param name="sDescripcion"></param>
    /// <param name="byimagen"></param>
    /// <returns></returns>
    public static int fnAgregarPublicidad(int id_Publicidad, string sTitulo, string sDescripcion, byte[] byimagen, bool bSeleccion)
    {
        int nAuxiliar = 0;
        try
        {
            DataTable dtMenu = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            if(id_Publicidad>0)
                giSql.AgregarParametro("@id_Publicidad", id_Publicidad);
            giSql.AgregarParametro("@Titulo", sTitulo);
            giSql.AgregarParametro("@Descripcion", sDescripcion);
            if(byimagen.Length >0)
                giSql.AgregarParametro("@imagen", byimagen);
            giSql.AgregarParametro("@bSeleccion", bSeleccion);

            nAuxiliar = Convert.ToInt32(giSql.TraerEscalar("usp_con_publicidad_Ins_Upd", true));

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return nAuxiliar;   
    }

    /// <summary>
    /// Borra la seleccion pasada y actualiza el la nueva seleccion correspondiente la estructura de la publicidad
    /// </summary>
    /// <param name="bCondicion"></param>
    public static void fnActualziaTabla()
    {
        try
        {
            giSql = clsComun.fnCrearConexion("conConfiguracion");
            giSql.NoQuery("usp_Selecc_Upd", true);
        }
        catch
        {

        }

    }

    public static void fnAgregarRelacionSerieFolio(int nidEstructura, int nidSerie)
    {
        try 
        {
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            giSql.AgregarParametro("@Estructura", nidEstructura);
            if(nidSerie>0)
                giSql.AgregarParametro("@Serie", nidSerie);

            giSql.NoQuery("usp_serie_folio_rel_Ins",true);
        }
        catch{
            
        }
        
    }

    /// <summary>
    /// Obtemos el Email del Cliente
    /// </summary>
    /// <param name="nCFD"></param>
    /// <returns></returns>
    public static string fnObtenerEmailCliente(int nCFD,string srfc)
    {
        string sAuxliliar = string.Empty;
        try 
        {
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            giSql.AgregarParametro("@IdCFD", nCFD);
            giSql.AgregarParametro("@srfc", srfc);

            sAuxliliar = Convert.ToString(giSql.TraerEscalar("usp_Obtener_Email_Sel", true));
        }
        catch{
            
        }
        return sAuxliliar;
    }

    /// <summary>
    /// Obtiene la estructura del ticket 
    /// </summary>
    /// <param name="nIdEstructura"></param>
    /// <returns></returns>
    public static int fnObteneridSucursal(string sNumeroTienda)
    {
        int nAuxiliar = 0;
        try
        {
            DataTable dtMenu = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");
            
            giSql.AgregarParametro("@sNumeroTienda", sNumeroTienda);

            nAuxiliar = Convert.ToInt32(giSql.TraerEscalar("usp_Obtener_Estructura_Sel", true));
            
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return nAuxiliar;
        
    }

    /// <summary>
    /// Obtiene el Padre de la Sucursal
    /// </summary>
    /// <param name="nIdEstructura"></param>
    /// <returns></returns>
    public static int fnObteneridPadreSucursal(string sNumeroTienda)
    {
        int nAuxiliar = 0;
        try
        {
            DataTable dtMenu = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            giSql.AgregarParametro("@sNumeroTienda", sNumeroTienda);

            nAuxiliar = Convert.ToInt32(giSql.TraerEscalar("usp_Obtener_EstructuraPadreTienda_Sel", true));

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return nAuxiliar;

    }

    /// <summary>
    /// Obtenemos los idD de comprobantes para cancelarlos
    /// </summary>
    /// <param name="sUUID"></param>
    /// <returns></returns>
    public static int fnObtenerIdCFDI(string sUUID)
    {
        int nAuxiliar = 0;
        try
        {
            DataTable dtMenu = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");
            
            giSql.AgregarParametro("@sUUID", sUUID);

            nAuxiliar = Convert.ToInt32(giSql.TraerEscalar("", true));
            
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return nAuxiliar;
        
    }

    /// <summary>
    /// Obtiene la imagen del nodo hijo
    /// </summary>
    /// <param name="nIdEstructura"></param>
    /// <returns></returns>
    public static byte[] fnObterimagenNodeHijo(int nIdEstructura,string sValor)
    {
        byte[] bytAuxiliar = null;
        try
        {
            DataTable dtMenu = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");
            giSql.AgregarParametro("@Idestructura", nIdEstructura);
            if(!string.IsNullOrEmpty(sValor))
                giSql.AgregarParametro("@sValor", sValor);
            bytAuxiliar = (byte[])giSql.TraerEscalar("usp_obtiene_Imagen_Sel", true);

            return bytAuxiliar;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// Obtenemos la imagen de publicidad
    /// </summary>
    /// <returns></returns>
    public static byte[] fnObtenerImgPublic()
    {
        byte[] bytAuxiliar = null;
        try
        {
            
            giSql = clsComun.fnCrearConexion("conConfiguracion");
            bytAuxiliar = (byte[])giSql.TraerEscalar("usp_obtiene_Imagen_Public_Sel", true);

            return bytAuxiliar;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// Obtiene las imagenes
    /// </summary>
    /// <param name="nIdEstructura"></param>
    /// <returns></returns>
    public static byte[] fnObterImagenes(int nIdEstructura)
    {
        byte[] bytAuxiliar = null;
        try
        {
            DataTable dtMenu = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            giSql.AgregarParametro("@nEstructura", nIdEstructura);

            bytAuxiliar = (byte[])giSql.TraerEscalar("usp_Obtiene_imagen_Ticket_Sel", true);

            return bytAuxiliar;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /// <summary>
    /// Vuelve el ticket al estado activo para poder volverlo a facturar
    /// </summary>
    /// <param name="nIdCfd">Id del CFD del ticket</param>
    public static void fnCancelarTicket(int nIdCfd)
    {
        try
        {
            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conConfiguracion");
            conexion.AgregarParametro("nIdCfd", nIdCfd);
            conexion.Query("usp_Cfd_Cancelar_Ticket", true);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Crea una nueva entrada para las pistas de auditoría para el usuario en sesión
    /// en el momento exacto de la llamada del método y con la descipción de acción especificada
    /// </summary>
    /// <param name="args">Argumentos para la descripción de la acción</param>
    public static void fnNuevaPistaAuditoria(params object[] args)
    {
        try
        {
            int id_usuario = fnUsuarioEnSesion().id_usuario;
            DateTime fecha = DateTime.Now;
            StringBuilder accion = new StringBuilder();

            foreach (object arg in args)
            {
                accion.Append(arg);
                accion.Append(" | ");
            }

            clsPistasAuditoria.fnGenerarPistasAuditoria(id_usuario, fecha, accion.ToString().Trim('|'));
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    /// <summary>
    /// Encargado de validar si la expreson es verdadera
    /// </summary>
    /// <param name="sValor">valor a evaluar</param>
    /// <param name="expresion">expresion regular</param>
    /// <returns>retorna si es verdadero</returns>
    public static bool fnValidaExpresion(string sValor, string expresion)
    {
        bool bRetorno = false;

        if (Regex.IsMatch(sValor, expresion))
        {
            bRetorno = true;
        }

        return bRetorno;
    }

    /// <summary>
    /// Aplicamos la Regla de negocio
    /// </summary>
    /// <param name="tablaTicket"></param>
    /// <returns></returns>
    public static bool fnRegladenegocio(DataTable tablaTicket, int idTienda)
    {
        string sScurusalXML = string.Empty;
        bool retorno  = false;

        DateTime fecha = new DateTime();
        XmlDocument xml = new XmlDocument();

        xml.LoadXml(tablaTicket.Rows[0]["xml_ticket"].ToString());
        XmlNodeList xmlList = xml.GetElementsByTagName("doc:Ticket");
        foreach (XmlElement xmlElem in xmlList)
        {
            //sScurusalXML = xmlElem.GetAttribute("sucursal");
            ///Obtengo la Fecha del XML
            fecha = Convert.ToDateTime(xmlElem.GetAttribute("fecha"));
        }
        DataTable dtRegla = fnRecuperaReglaDeNegocio(idTienda);

        if (dtRegla.Rows.Count > 0)
        {
            switch (dtRegla.Rows[0]["idRegla"].ToString())
            {
                case "0":
                    retorno = true;
                    break;
                case "1":
                    TimeSpan tsMinutos = fecha - DateTime.Now;
                    if (Math.Abs(tsMinutos.TotalMinutes) > Convert.ToInt32(dtRegla.Rows[0]["valor"]))///Evaluamos que la fecha del XML pase de 30 minutos
                        retorno = true;
                    break;
                case "2":
                    TimeSpan tsResul = fecha - DateTime.Now;
                    if (Math.Abs(tsResul.TotalHours) > Convert.ToInt32(dtRegla.Rows[0]["valor"]))//Evaluamos que la fecha del XML pase de 24 horas
                        retorno = true;
                    break;
                default:
                    retorno = false;
                    break;
            }
        }
        
        return retorno;
    }
    
    #endregion

    /// <summary>
    /// Utileria para mostrar mensaje al estilo JQuery
    /// </summary>
    /// <param name="pPagina">La página que llama al mensaje</param>
    /// <param name="psMensaje">El mensaje a mostrar</param>
    public static void fnMostrarMensaje(Page pPagina, string psMensaje)
    {

        fnMostrarMensaje(pPagina, psMensaje, Resources.resCorpusCFDIEs.varContribuyente);
    }

    /// <summary>
    /// Utileria para mostrar mensaje al estilo JQuery
    /// </summary>
    /// <param name="pPagina">La página que llama al mensaje</param>
    /// <param name="psMensaje">El mensaje a mostrar</param>
    /// <param name="psTitulo">El titulo que tendrá el modal</param>
    public static void fnMostrarMensaje(Page pPagina, string psMensaje, string psTitulo)
    {
        //ScriptManager.RegisterStartupScript(pPagina, typeof(Page), "Mensaje" + DateTime.Now.Millisecond.ToString(),
        //    "jAlert('" + psMensaje + "', '" + psTitulo + "');", true);
        ScriptManager.RegisterStartupScript(pPagina, typeof(Page), "Mensaje" + DateTime.Now.Millisecond.ToString(),
            "alert('" + psMensaje + "', '" + psTitulo + "');", true);

    }

    /// <summary>
    /// Utileria para mostrar mensaje al estilo JQuery
    /// </summary>
    /// <param name="pPagina">Control de usaurio que llama al mensaje</param>
    /// <param name="psMensaje">El mensaje a mostrar</param>
    /// <param name="psTitulo">El titulo que tendrá el modal</param>
    public static void fnMostrarMensaje(UserControl pPagina, string psMensaje, string psTitulo)
    {
        ScriptManager.RegisterStartupScript(pPagina, typeof(UserControl), "Mensaje" + DateTime.Now.Millisecond.ToString(),
            "jAlert('" + psMensaje + "', '" + psTitulo + "');", true);

    }

    /// <summary>
    /// Revisa que el valor sea entero.
    /// </summary>
    /// <param name="Expression">valor a evaluar</param>
    /// <returns>retorna si es verdadero.</returns>
    public static bool fnIsInt(object Expression)
    {
        bool isNum;
        int retNum;

        isNum = int.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        return isNum;
    }

    /// <summary>
    /// Muestra un mensaje de error en la parte superior de la página
    /// </summary>
    /// <param name="pPagina">Página que llama al método</param>
    /// <param name="psMensajeError">Mensaje de error a mostrar</param>
    public static void fnMostrarError(System.Web.UI.Page pPagina, string psMensajeError)
    {
        Label lbl = (Label)pPagina.Master.FindControl("lblErrorGenerico");
        lbl.Text = psMensajeError;
    }

    #region  Cancelacion del SAT

    /// <summary>
    /// Inserta un nuevo registro de acuse del emitido por el SAT
    /// </summary>
    /// /// <param name="p_UUID">UUID del comprobante</param>
    ///  <param name="sFecha">fecha de envio</param>
    ///  <param name="p_Acuse">xml con el acuse que emitio el SAT</param>
    ///  <param name="p_idcodigo">tipo de codigo que se genero</param>
    /// <returns></returns>
    public static Int32 fnInsertaAcuseSAT(string sId_Contribuyente, string p_UUID, string p_Acuse, DateTime sFecha, string p_idcodigo, string p_Soap, string origen)
    {
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.AgregarParametro("@nid_Contribuyente", sId_Contribuyente);
        giSql.AgregarParametro("@sUUID", p_UUID);
        giSql.AgregarParametro("@sAcuse", p_Acuse);
        giSql.AgregarParametro("@sFecha", sFecha);
        giSql.AgregarParametro("@nid_codigo", p_idcodigo);
        giSql.AgregarParametro("@sSoap", p_Soap);
        giSql.AgregarParametro("@sOrigen", origen);
        Int32 Resultado = Convert.ToInt32(giSql.NoQuery("usp_Cfd_Acuse_SAT_Ins", true));
        return Resultado;
    }

    /// <summary>
    /// Devuelve la descripcion del error del SAT
    /// </summary>
    /// /// <param name="p_idcodigo">numero de error</param>
    /// /// <param name="p_sTipo">tipo de error</param>
    /// <returns></returns>
    public static string fnRecuperaErrorSAT(string p_idcodigo, string p_sTipo)
    {
        giSql = clsComun.fnCrearConexion("conConfiguracion");
        giSql.AgregarParametro("@nId_Codigo", p_idcodigo);
        giSql.AgregarParametro("@nsTipo", p_sTipo);
        string Resultado = Convert.ToString(giSql.TraerEscalar("usp_Ctp_RecuperaCodigo_Sel", true));
        return Resultado;
    }

    #endregion

    #region Cancelacion Pruebas 

    /// <summary>
    /// Actualiza el comprobante especificado poniendo su estatus a 'Cancelado'
    /// </summary>
    /// <param name="psIdCfd">Identificador del comprobante a cancelar</param>
    /// <returns></returns>
    public static int fnCancelarComprobante(int psIdCfd, string comentario, int nidUsuario)
    {
        giSql = clsComun.fnCrearConexion("conConfiguracion");

        giSql.AgregarParametro("nId_Cfd", psIdCfd);
        giSql.AgregarParametro("nId_Usuario", nidUsuario);
        giSql.AgregarParametro("sComentarioCancelacion", comentario);
        return giSql.NoQuery("usp_Cfd_Cancela_Comprobante_Upd", true);
    }

    /// <summary>
    /// metodo para recuperar el identificador del UUID en la base de datos
    /// </summary>
    /// <param name="psUUID"></param>
    /// <returns></returns>
    public static int fnRecuperaIdCFD(string psUUID)
    {

        giSql = clsComun.fnCrearConexion("conConfiguracion");

        giSql.AgregarParametro("@nUUID", psUUID);

        return Convert.ToInt32(giSql.TraerEscalar("usp_Cfd_RecuperaComprobanteUUID_sel", true));
    }


    #endregion

}
///// <summary>
///// 
///// </summary>
///// <param name="nidEstructura"></param>
///// <returns></returns>
//public static int fnRecuperaridestructura(int nidEstructura)
//{
//    giSql = clsComun.fnCrearConexion("conConfiguracion");

//    giSql.AgregarParametro("@nidEstructura", nidEstructura); 

//    int valor = Convert.ToInt32(giSql.TraerEscalar("usp_rec_EstructuraCobro_Sel", true));

//    return valor;
//}

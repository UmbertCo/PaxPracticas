using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data;


    public class clsOperacionProveedores
    {
        private InterfazSQL giSql;
        private DataTable dtAuxiliar;
        private string conSucursales = "conRecepcionProveedores";

        /// <summary>
        /// Guarda el proveedor
        /// </summary>
        /// <param name="sNombre"></param>
        /// <param name="sContacto"></param>
        /// <param name="nIdMunicipio"></param>
        /// <param name="sLocalidad"></param>
        /// <param name="sColonia"></param>
        /// <param name="sCalle"></param>
        /// <param name="sNoExterior"></param>
        /// <param name="sNoInterior"></param>
        /// <param name="nCodigoPostal"></param>
        /// <param name="sEmail"></param>
        /// <param name="sTelefono"></param>
        /// <param name="sUsuario"></param>
        /// <param name="sPassword"></param>
        /// <returns></returns>
        public int fnGuardarProveedor(string sNombre, string sContacto,
        int nIdMunicipio, string sLocalidad, string sColonia, string sCalle, string sNoExterior,
        string sNoInterior, string nCodigoPostal, string sEmail, string sTelefono, int nIdUsuario)
        {
            int res = 0;
            try
            {
                giSql = clsComun.fnCrearConexion(conSucursales);
                
                giSql.AgregarParametro("@sNombre", sNombre);
                giSql.AgregarParametro("@sContacto", sContacto);
                giSql.AgregarParametro("@nIdMunicipio", nIdMunicipio);
                if (!string.IsNullOrEmpty(sLocalidad))
                    giSql.AgregarParametro("@sLocalidad", sLocalidad);
                if (!string.IsNullOrEmpty(sColonia))
                    giSql.AgregarParametro("@sColonia", sColonia);
                if (!string.IsNullOrEmpty(sCalle))
                    giSql.AgregarParametro("@sCalle", sCalle);
                if (!string.IsNullOrEmpty(sNoExterior))
                    giSql.AgregarParametro("@sNoExterior", sNoExterior);
                if (!string.IsNullOrEmpty(sNoInterior))
                    giSql.AgregarParametro("@sNoInterior", sNoInterior);
                if (!string.IsNullOrEmpty(nCodigoPostal))
                    giSql.AgregarParametro("@nCodigoPostal", nCodigoPostal);
                giSql.AgregarParametro("@sEmail", sEmail);
                if(!string.IsNullOrEmpty(sTelefono))
                    giSql.AgregarParametro("@sTelefono", sTelefono);
                giSql.AgregarParametro("@nIdUsuario", nIdUsuario);

                res = Convert.ToInt32(giSql.TraerEscalar("usp_rfp_Registro_Proveedor_ins", true));
            }
            catch (Exception ex)
            {
                return 0;
            }
            return res;
        }
        /// <summary>
        /// Verifica si el nombre del usuario existe
        /// </summary>
        /// <param name="sNombre"></param>
        /// <returns></returns>
        public int fnNombreExiste(string sNombre)
        {
            int res = 0;
            try
            {
                giSql = clsComun.fnCrearConexion(conSucursales);

                giSql.AgregarParametro("@sNombre", sNombre);

                res = Convert.ToInt32(giSql.TraerEscalar("usp_rfp_Obtener_Proveedor_sel", true));
            }
            catch (Exception ex)
            {
            }
            return res;
        }
        /// <summary>
        /// Obtiene todos los proveedores
        /// </summary>
        /// <returns></returns>
        public DataTable fnObtenerProveedores()
        {
            DataTable res = new DataTable();
            try
            {
                giSql = clsComun.fnCrearConexion(conSucursales);


                giSql.Query("usp_rfp_Obtener_Proveedores_sel", true, ref res);
            }
            catch (Exception ex)
            {
            }
            return res ;
        }

        /// <summary>
        /// Obtiene todos los proveedores
        /// </summary>
        /// <returns></returns>
        public DataTable fnObtenerProveedor(int nIdUsuario)
        {
            DataTable res = new DataTable();
            try
            {
                giSql = clsComun.fnCrearConexion(conSucursales);

                giSql.AgregarParametro("nIdUsuario", nIdUsuario);
                giSql.Query("usp_rfp_Obtener_Proveedor_Usuario_Datos_sel", true, ref res);
            }
            catch (Exception ex)
            {
            }
            return res;
        }
        /// <summary>
        /// Guarda la relación proveedor-sucursal
        /// </summary>
        /// <param name="nIdProveedor"></param>
        /// <param name="nIdSucursal"></param>
        public void fnProveedorSucursalRel(int nIdProveedor, int nIdSucursal)
        {
            try
            {
                giSql = clsComun.fnCrearConexion(conSucursales);

                giSql.AgregarParametro("@nIdProveedor", nIdProveedor);
                giSql.AgregarParametro("@nIdSucursal", nIdSucursal);

               giSql.TraerEscalar("usp_rfp_Proveedor_Sucursal_Relacion_ins", true);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Edita el proveedor seleccionado
        /// </summary>
        /// <param name="nIdProveedor"></param>
        /// <param name="sNombre"></param>
        /// <param name="sContacto"></param>
        /// <param name="nIdMunicipio"></param>
        /// <param name="sLocalidad"></param>
        /// <param name="sColonia"></param>
        /// <param name="sCalle"></param>
        /// <param name="sNoExterior"></param>
        /// <param name="sNoInterior"></param>
        /// <param name="nCodigoPostal"></param>
        /// <param name="sEmail"></param>
        /// <param name="sTelefono"></param>
        /// <returns></returns>
        public int fnEditarProveedor(int nIdProveedor,string sNombre, string sContacto,
            int nIdMunicipio, string sLocalidad, string sColonia, string sCalle, string sNoExterior,
            string sNoInterior, string nCodigoPostal, string sEmail, string sTelefono)
        {
            int res = 0;
            try
            {
                giSql = clsComun.fnCrearConexion(conSucursales);
                giSql.AgregarParametro("@nIdProveedor", nIdProveedor);
                giSql.AgregarParametro("@sNombre", sNombre);
                giSql.AgregarParametro("@sContacto", sContacto);
                giSql.AgregarParametro("@nIdMunicipio", nIdMunicipio);
                if (!string.IsNullOrEmpty(sLocalidad))
                    giSql.AgregarParametro("@sLocalidad", sLocalidad);
                if (!string.IsNullOrEmpty(sColonia))
                    giSql.AgregarParametro("@sColonia", sColonia);
                if (!string.IsNullOrEmpty(sCalle))
                    giSql.AgregarParametro("@sCalle", sCalle);
                if (!string.IsNullOrEmpty(sNoExterior))
                    giSql.AgregarParametro("@sNoExterior", sNoExterior);
                if (!string.IsNullOrEmpty(sNoInterior))
                    giSql.AgregarParametro("@sNoInterior", sNoInterior);
                if (!string.IsNullOrEmpty(nCodigoPostal))
                    giSql.AgregarParametro("@nCodigoPostal", nCodigoPostal);
                giSql.AgregarParametro("@sEmail", sEmail);
                if (!string.IsNullOrEmpty(sTelefono))
                    giSql.AgregarParametro("@sTelefono", sTelefono);
                res = Convert.ToInt32(giSql.TraerEscalar("usp_rfp_Editar_Proveedor_up", true));
            }
            catch (Exception ex)
            {
                return 0;
            }
            return res;
        }
        /// <summary>
        /// Elimina la relación del proveedor con las sucursales
        /// </summary>
        /// <param name="nIdProveedor"></param>
        public void fnEliminarProveedorSucursalRel(int nIdProveedor)
        {
            try
            {
                giSql = clsComun.fnCrearConexion(conSucursales);

                giSql.AgregarParametro("@nIdProveedor", nIdProveedor);

                giSql.TraerEscalar("usp_rfp_Eliminar_Proveedor_Sucursal_Rel_del", true);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Obtiene los datos de un proveedor
        /// </summary>
        /// <param name="nIdProveedor"></param>
        /// <returns></returns>
        public DataTable fnObtenerDatosProveedor(int nIdProveedor)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                giSql = clsComun.fnCrearConexion(conSucursales);

                giSql.AgregarParametro("@nIdProveedor", nIdProveedor);

                giSql.Query("usp_rfp_Obtener_Proveedor_Datos_sel", true,ref dtResultado);
            }
            catch (Exception ex)
            {
            }
            return dtResultado;
        }
        /// <summary>
        /// Elimina el proveedor seleccionado
        /// </summary>
        /// <param name="nIdProveedor"></param>
        public void fnEliminarProveedor(int nIdProveedor)
        {
            try
            {
                giSql = clsComun.fnCrearConexion(conSucursales);

                giSql.AgregarParametro("@nIdProveedor", nIdProveedor);

                giSql.Query("usp_rfp_Eliminar_Proveedor_up", true);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Obtiene las sucursales ligadas al proveedor
        /// </summary>
        /// <param name="nIdProveedor"></param>
        /// <returns></returns>
        public DataTable fnObtenerSucursalesProveedor(int nIdProveedor)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                giSql = clsComun.fnCrearConexion(conSucursales);

                giSql.AgregarParametro("@nIdProveedor", nIdProveedor);

                giSql.Query("usp_rfp_Obtener_Sucursales_Proveedor_sel", true, ref dtResultado);
            }
            catch (Exception ex)
            {
            }
            return dtResultado;
        }

        /// <summary>
        /// Obtiene las sucursales ligadas al proveedor por medio del usuario
        /// </summary>
        /// <param name="nIdProveedor"></param>
        /// <returns></returns>
        public DataTable fnObtenerSucursalesProveedorUsuario(int nIdUsuario)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                giSql = clsComun.fnCrearConexion(conSucursales);

                giSql.AgregarParametro("@nIdUsuario", nIdUsuario);

                giSql.Query("usp_rfp_Obtener_Sucursales_Proveedor_Usu_sel", true, ref dtResultado);
            }
            catch (Exception ex)
            {
            }
            return dtResultado;
        }

        public string fnObtenerProveedorUsuario(int nIdUsuario)
        {
            string res = string.Empty;
            try
            {
                giSql = clsComun.fnCrearConexion(conSucursales);

                giSql.AgregarParametro("@nIdUsuario", nIdUsuario);

                res = giSql.TraerEscalar("usp_rfp_Obtener_Proveedor_Usuario_sel", true).ToString();
            }
            catch (Exception ex)
            {
            }
            return res;
        }
    }

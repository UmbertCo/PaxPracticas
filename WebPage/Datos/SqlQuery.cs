using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Sql;

namespace Datos
{
    public class SqlQuery
    {
        /// <summary>
        /// Muestra el Personal
        /// </summary>
        /// <returns>dtPersonal</returns>
        public static DataTable fnMostrarPersonal()
        {
            //se establece la cadena coneccion con el url de la base de datos
            ConnectionString cadena = new ConnectionString();

            //sqlconnection establece la conexion con la base de datos
            SqlConnection conn = new SqlConnection(cadena.conexionString());

            //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
            SqlCommand cmd = new SqlCommand("usp_mos_personal_sel", conn);

            // se establece el tipo al sqlcommand 
            cmd.CommandType = CommandType.StoredProcedure;

            //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            //crea la intanstancia de Conjunto de Datos recuperados de un BD
            DataTable dtPersonal = new DataTable();

            //agrega las filas en el intervalo especificado de la tabla 
            adapter.Fill(dtPersonal);

            //regresa un Conjunto de datos
            return dtPersonal;
        }

        /// <summary>
        /// Muestra los Puestos
        /// </summary>
        /// <returns>dtPuestos</returns>
        public static DataTable fnMostrarPuestos()
        {
            //se establece la cadena coneccion con el url de la base de datos
            ConnectionString cadena = new ConnectionString();

            //sqlconnection establece la conexion con la base de datos
            SqlConnection conn = new SqlConnection(cadena.conexionString());

            //añade como valor hasta el final de la sentencia y se le pasa el valor
            SqlCommand cmd = new SqlCommand("usp_mos_puestos_sel", conn);

            // se establece el tipo al sqlcommand 
            cmd.CommandType = CommandType.StoredProcedure;

            //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
            SqlDataAdapter dr = new SqlDataAdapter(cmd);

            //crea la intanstancia de Conjunto de Datos recuperados de un BD
            DataTable dtPuestos = new DataTable();

            //agrega las filas en el intervalo especificado de la tabla 
            dr.Fill(dtPuestos);

            //regresa un Conjunto de datos
            return dtPuestos;
        }

        /// <summary>
        /// funcion obtiene los datos del personal
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>dtTablaPersonal</returns>
        public static DataTable fnMostraTabladelPersonal(string usuario)
        {
            //crea la intanstancia de Conjunto de Datos recuperados de un BD
            DataTable dtTablaPersonal = new DataTable();

            //instrucción para controlar errores
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                SqlCommand cmd = new SqlCommand("usp_tabla_de_Personal_Sel", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@usuario", usuario);

                //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
                SqlDataAdapter dr = new SqlDataAdapter(cmd);

                //agrega las filas en el intervalo especificado de la tabla 
                dr.Fill(dtTablaPersonal);

                //Abre la conexion 
                conn.Open();
                //Cierra la conexion
                conn.Close();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa el Conjunto de datos del resulset 
            return dtTablaPersonal;
        }

        /// <summary>
        /// Muestra las Tareas que fueron Asignadas al personal
        /// </summary>
        /// <param name="IdProyecto"></param>
        /// <param name="IdPersonal"></param>
        /// <returns>dtTareaAsignada</returns>
        public static DataTable fnMostrarTareasAsignadas(int IdProyecto, int IdPersonal)
        {
            //se crea una instancia de tipo DataTable
            DataTable dtTareaAsignada = new DataTable();

            //instrucción controla errores
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_mostrar_Tarea_asignada_sel", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@IdProyecto", IdProyecto);
                cmd.Parameters.AddWithValue("@IdPersonal", IdPersonal);

                //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
                SqlDataAdapter adr = new SqlDataAdapter(cmd);

                //agrega las filas en el intervalo especificado de la tabla 
                adr.Fill(dtTareaAsignada);

                //Abre la conexion 
                conn.Open();
                //Cierra la conexion
                conn.Close();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa una tabla
            return dtTareaAsignada;
        }

        /// <summary>
        /// Muestra las Tareas
        /// </summary>
        /// <returns>dtTareas</returns>
        public static DataTable fnMostrarTareas()
        {
            //se crea una instancia de tipo DataTable
            DataTable dtTareas = new DataTable();

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_mos_cat_tarea_sel", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
                SqlDataAdapter adr = new SqlDataAdapter(cmd);

                //agrega las filas en el intervalo especificado de la tabla 
                adr.Fill(dtTareas);

                //Abre la conexion 
                conn.Open();

                //Cierra la conexion
                conn.Close();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa una tabla
            return dtTareas;
        }

        /// <summary>
        /// Muestra las Tareas Asignadas al Personal
        /// </summary>
        /// <param name="IdPersonal"></param>
        /// <returns>dtTareasAsignadas</returns>
        public static DataTable fnMostrarTareasAsignadas(int IdPersonal)
        {
            //se crea una instancia de tipo DataTable
            DataTable dtTareasAsignadas = new DataTable();

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_mostrar_tareas_asignadas", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@IdPersonal", IdPersonal);

                //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
                SqlDataAdapter adr = new SqlDataAdapter(cmd);

                //agrega las filas en el intervalo especificado de la tabla 
                adr.Fill(dtTareasAsignadas);

                //Abre la conexion 
                conn.Open();

                //Cierra la conexion
                conn.Close();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa una tabla
            return dtTareasAsignadas;
        }

        /// <summary>
        /// Muestras las tareas de los Proyectos
        /// </summary>
        /// <param name="idproyecto"></param>
        /// <returns>dtTareasProyecto</returns>
        public static DataTable fnMostrarTareasDeProyecto(int idproyecto)
        {
            //se crea una instancia de tipo DataTable
            DataTable dtTareasProyecto = new DataTable();

            //controla los errores
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_mos_tarea_sel", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("IdProyecto", idproyecto);

                //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
                SqlDataAdapter adr = new SqlDataAdapter(cmd);

                //agrega las filas en el intervalo especificado de la tabla 
                adr.Fill(dtTareasProyecto);

                //Abre la conexion 
                conn.Open();

                //Cierra la conexion
                conn.Close();
            }
            //cacha los errores
            catch
            {

            }
            //regresa una tabla
            return dtTareasProyecto;
        }

        /// <summary>
        /// Verifica si el Usuario Existe
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>dtUsuario</returns>
        public static DataTable fnVericaUsuarioExiste(string usuario)
        {
            //se crea una instancia de tipo DataTable
            DataTable dtUsuario = new DataTable();

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_personal_login", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@usuario", usuario);

                //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
                SqlDataAdapter sqdar = new SqlDataAdapter(cmd);

                //agrega las filas en el intervalo especificado de la tabla 
                sqdar.Fill(dtUsuario);

                //abre la conexion
                conn.Open();

                //Cierra la conexion
                conn.Close();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa una tabla
            return dtUsuario;
        }

        /// <summary>
        /// Muestra los Proyectos que son asigandos al personal en el Combo
        /// </summary>
        /// <param name="idPersonal"></param>
        /// <returns>dtProyectosAsignados</returns>
        public static DataTable fnMostrarProyectosAsignados(int idPersonal,bool bReporte)
        {
            //se crea una instancia de tipo DataTable
            DataTable dtProyectosAsignados = new DataTable();

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_proyectos_asignados_personal", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@IdPersonal", idPersonal);
                cmd.Parameters.AddWithValue("@bReporte",bReporte);

                //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                //agrega las filas en el intervalo especificado de la tabla 
                adp.Fill(dtProyectosAsignados);

                //abre la conexion
                conn.Open();

                //cierra la conexion
                conn.Close();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa una tabla
            return dtProyectosAsignados;
        }

        /// <summary>
        /// Muestra los Proyectos
        /// </summary>
        /// <returns>dtProyectos</returns>
        public static DataTable fnMostrarProyectos()
        {
            //se crea una instancia de tipo DataTable
            DataTable dtProyectos = new DataTable();

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_lista_proyecto", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                //agrega las filas en el intervalo especificado de la tabla 
                adp.Fill(dtProyectos);

                //abre la conexion
                conn.Open();

                //cierra la conexion
                conn.Close();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa una tabla
            return dtProyectos;
        }

        /// <summary>
        /// Muestra el menú de acuerdo al tipo de permiso
        /// </summary>
        /// <param name="typo"></param>
        /// <returns>dtMenu</returns>
        public static DataTable fnMotrarMenu(string typo)
        {
            //se crea una instancia de tipo DataTable
            DataTable dtMenu = new DataTable();

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_mos_menu_sel", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@typo", typo);

                //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //agrega las filas en el intervalo especificado de la tabla 
                adapter.Fill(dtMenu);

                //abre la conexion
                conn.Open();

                //cierra la conexion
                conn.Close();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa una tabla
            return dtMenu;
        }

        /// <summary>
        /// Muestra el Reporte del personal
        /// </summary>
        /// <param name="IdPersonal"></param>
        /// <param name="IdProyecto"></param>
        /// <param name="fechainicio"></param>
        /// <param name="fechafinal"></param>
        /// <param name="btnSelec"></param>
        /// <returns>dtReportePersonal</returns>
        public static DataTable fnMostrarReportePersonal(int IdPersonal, int IdProyecto, DateTime fechainicio, DateTime fechafinal, string btnSelec)
        {
            //se crea una instancia de tipo DataTable
            DataTable dtReportePersonal = new DataTable();

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_mostrar_reporte_de_personal_sel", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@IdPersonal", IdPersonal);
                cmd.Parameters.AddWithValue("@IdProyecto", IdProyecto);
                cmd.Parameters.AddWithValue("@fechainicio", fechainicio);
                cmd.Parameters.AddWithValue("@fechafinal", fechafinal);
                cmd.Parameters.AddWithValue("@btnSelec", btnSelec);

                //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
                SqlDataAdapter adr = new SqlDataAdapter(cmd);

                //agrega las filas en el intervalo especificado de la tabla 
                adr.Fill(dtReportePersonal);

                //Abre la conexion 
                conn.Open();

                //Cierra la conexion
                conn.Close();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa una tabla
            return dtReportePersonal;
        }

        /// <summary>
        /// Muestra Reporte por personal de acuerdo al proyecto
        /// </summary>
        /// <param name="IdPersonal"></param>
        /// <param name="IdProyecto"></param>
        /// <returns>dtReportes</returns>
        public static DataTable fnMostrarReporte(int IdPersonal, int IdProyecto)
        {
            //se crea una instancia de tipo DataTable
            DataTable dtReportes = new DataTable();

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_mostrar_reporte_por_proyecto_sel", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@IdPersonal", IdPersonal);
                cmd.Parameters.AddWithValue("@IdProyecto", IdProyecto);

                //lee una secuencia de filas, crea una intancia con la espeficicacion del SelectCommand
                SqlDataAdapter adr = new SqlDataAdapter(cmd);

                //agrega las filas en el intervalo especificado de la tabla 
                adr.Fill(dtReportes);

                //Abre la conexion 
                conn.Open();

                //Cierra la conexion
                conn.Close();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa una tabla
            return dtReportes;
        }

        //############################ Metodo Enteros #############################################

        /// <summary>
        /// Agrega personal 
        /// </summary>
        /// <param name="Nombre"></param>
        /// <param name="ApPaterno"></param>
        /// <param name="ApMaterno"></param>
        /// <param name="Direccion"></param>
        /// <param name="Telefono"></param>
        /// <param name="Correo"></param>
        /// <param name="idPuesto"></param>
        /// <param name="estatus"></param>
        /// <param name="alta"></param>
        /// <returns>idpersonal</returns>
        public static int fnAgregarPersonal(string Nombre, string ApPaterno, string ApMaterno, string Direccion, string Telefono, string Correo, int idPuesto, bool estatus, DateTime alta)
        {
            //crea una variabla de tipo entero inicializada en cero
            int idpersonal = 0;

            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_agre_personal_ins", conn);


                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@ApPaterno", ApPaterno);
                cmd.Parameters.AddWithValue("@ApMaterno", ApMaterno);
                cmd.Parameters.AddWithValue("@Direccion", Direccion);
                cmd.Parameters.AddWithValue("@Telefono", Telefono);
                cmd.Parameters.AddWithValue("@Correo", Correo);
                cmd.Parameters.AddWithValue("@IdPuesto", idPuesto);
                cmd.Parameters.AddWithValue("@estatus", estatus);
                cmd.Parameters.AddWithValue("@FeAlta", alta);

                //abre conexion
                conn.Open();
                //la variable almacena la primera fila y columna del conjunto de resultados devuelto por la consulta
                idpersonal = Convert.ToInt32(cmd.ExecuteScalar());

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa un entero 
            return idpersonal;
        }

        /// <summary>
        /// Agrega el usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="contrasena"></param>
        /// <param name="typo"></param>
        /// <returns></returns>
        public static int fnAgregaUsuario(string usuario, string contrasena, string typo)
        {
            //crea una variabla de tipo entero inicializada en cero
            int idUsuario = 0;

            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_agre_usuario_ins", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);
                cmd.Parameters.AddWithValue("@typo", typo);

                //abre conexion
                conn.Open();

                //la variable almacena la primera fila y columna del conjunto de resultados devuelto por la consulta
                idUsuario = Convert.ToInt32(cmd.ExecuteScalar());

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa un entero 
            return idUsuario;
        }

        /// <summary>
        /// Regresa el id de la tarea Realizada
        /// </summary>
        /// <param name="id_estatus"></param>
        /// <param name="IdTarea"></param>
        /// <returns></returns>
        public static int fnIdTareaAsignada(int id_estatus, int IdTarea)
        {
            //crea una variabla de tipo entero inicializada en cero
            int IdTareaRealizada = 0;

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_regresa_id_tarea_realizada", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@id_estatus", id_estatus);
                cmd.Parameters.AddWithValue("@IdTarea", IdTarea);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                IdTareaRealizada = Convert.ToInt32(cmd.ExecuteScalar());

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
            }
            return IdTareaRealizada;
        }

        /// <summary>
        /// Recupera el estatus
        /// </summary>
        /// <param name="idAsignacion"></param>
        /// <returns></returns>
        public static int fnRecuperaEstatus(int idAsignacion)
        {
            //variable de tipo entera
            int estatus = 0;

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //se crea una instancia de tipo DataTable
                DataTable tb = new DataTable();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());
                //instrucción controla los errores posibles 

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_con_recupera_asignacion_sel", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@idAsignacion", idAsignacion);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                estatus = Convert.ToInt32(cmd.ExecuteScalar());

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
            //regresa una exprexion logica
            return estatus;
        }

        /// <summary>
        /// Inicia la Tarea
        /// </summary>
        /// <param name="IdProyecto"></param>
        /// <param name="IdPersonal"></param>
        /// <param name="IdTarea"></param>
        /// <param name="Inicio"></param>
        /// <param name="id_estatus"></param>
        /// <param name="IdAsig_Tarea"></param>
        /// <returns>nIdTareaIniciada</returns>
        public static int fnIniciaTarea(int IdProyecto, int IdPersonal, int IdTarea, DateTime Inicio, int id_estatus, int IdAsig_Tarea)
        {
            //variable de tipo entera
            int nIdTareaIniciada = 0;

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_agre_asig_tarea_ins", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@IdProyecto", IdProyecto);
                cmd.Parameters.AddWithValue("@IdPersonal", IdPersonal);
                cmd.Parameters.AddWithValue("@IdTarea", IdTarea);
                cmd.Parameters.AddWithValue("@Inicio", Inicio);
                cmd.Parameters.AddWithValue("@id_estatus", id_estatus);
                cmd.Parameters.AddWithValue("@IdAsig_Tarea", IdAsig_Tarea);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                nIdTareaIniciada = Convert.ToInt32(cmd.ExecuteScalar());

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
            }
            //Regresa el id de la tarea iniciada
            return nIdTareaIniciada;
        }

        //############################ Metodos Boleanos ###########################################

        /// <summary>
        /// Actualiza Catalogo de Tareas
        /// </summary>
        /// <param name="IdTarea"></param>
        /// <param name="Tarea"></param>
        /// <param name="DescripcionTarea"></param>
        /// <param name="btnactualizar"></param>
        /// <returns>bRealizada</returns>
        public static bool fnActualizarTarea(int IdTarea, string Tarea, string DescripcionTarea, string btnactualizar)
        {
            //Variable logica
            bool bRealizada = false;

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_actualizar_catalogo_tareas_upd", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@IdTarea", IdTarea);
                cmd.Parameters.AddWithValue("@Tarea", Tarea);
                cmd.Parameters.AddWithValue("@DescripcionTarea", DescripcionTarea);
                cmd.Parameters.AddWithValue("@btnactualizar", btnactualizar);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                bRealizada = Convert.ToBoolean(cmd.ExecuteNonQuery());

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //inicializa la variable falsa
                bRealizada = false;
            }
            //regresa una exprexion logica
            return bRealizada;
        }

        /// <summary>
        /// Agrega Proyecto
        /// </summary>
        /// <param name="NomProyecto"></param>
        /// <param name="DescProyecto"></param>
        /// <param name="FechaProyecto"></param>
        /// <param name="estatus"></param>
        /// <returns></returns>
        public static void fnAgregarProyecto(string NomProyecto, string DescProyecto, DateTime FechaProyecto, bool estatus)
        {
            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_agre_proyecto_ins", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@NomProyecto", NomProyecto);
                cmd.Parameters.AddWithValue("@DescProyecto", DescProyecto);
                cmd.Parameters.AddWithValue("@FechaProyecto", FechaProyecto);
                cmd.Parameters.AddWithValue("@Estatus", estatus);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                cmd.ExecuteNonQuery();

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
        }

        /// <summary>
        /// Actualiza Personal
        /// </summary>
        /// <param name="idpersonal"></param>
        /// <param name="nombre"></param>
        /// <param name="direccion"></param>
        /// <param name="paterno"></param>
        /// <param name="materno"></param>
        /// <param name="correo"></param>
        /// <param name="telefono"></param>
        /// <param name="estatus"></param>
        /// <param name="puesto"></param>
        public static void fnActualizarPersonal(int idpersonal, string nombre, string direccion, string paterno, string materno, string correo, string telefono, bool estatus, string puesto)
        {
            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_actu_personal_upd", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@IdPersonal ", idpersonal);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Direccion", direccion);
                cmd.Parameters.AddWithValue("@ApPaterno", paterno);
                cmd.Parameters.AddWithValue("@ApMaterno", materno);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@Telefono", telefono);
                cmd.Parameters.AddWithValue("@estatus", estatus);
                cmd.Parameters.AddWithValue("@Puesto", puesto);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                cmd.ExecuteNonQuery();

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
        }


        public static bool baja_proyecto(int idproyecto, bool estatus)
        {
            //se establece la cadena coneccion con el url de la base de datos
            ConnectionString cadena = new ConnectionString();

            //sqlconnection establece la conexion con la base de datos
            SqlConnection conn = new SqlConnection(cadena.conexionString());

            //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
            SqlCommand cmd = new SqlCommand("usp_baja_proyecto_upd", conn);

            // se establece el tipo al sqlcommand 
            cmd.CommandType = CommandType.StoredProcedure;

            //añade como valor hasta el final de la sentencia y se le pasa el valor
            cmd.Parameters.AddWithValue("@IdProyecto", idproyecto);
            cmd.Parameters.AddWithValue("@Estatus", estatus);

            //Variable logica
            bool rt;

            //instrucción controla los errores posibles 
            try
            {
                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                cmd.ExecuteNonQuery();

                //inicializa la variable verdadera
                rt = true;
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //inicializa la variable falsa
                rt = false;
            }
            finally
            {
                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }

            //regresa una exprexion logica
            return rt;
        }
        public static bool Actualizar_proyecto(int idproyecto, string NomProyecto, string Descproyecto, DateTime Fmodi)
        {
            //se establece la cadena coneccion con el url de la base de datos
            ConnectionString cadena = new ConnectionString();

            //sqlconnection establece la conexion con la base de datos
            SqlConnection conn = new SqlConnection(cadena.conexionString());

            //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
            SqlCommand cmd = new SqlCommand("usp_actu_proyectos_udp", conn);

            // se establece el tipo al sqlcommand 
            cmd.CommandType = CommandType.StoredProcedure;

            //añade como valor hasta el final de la sentencia y se le pasa el valor
            cmd.Parameters.AddWithValue("@IdProyecto", idproyecto);
            cmd.Parameters.AddWithValue("@NomProyecto", NomProyecto);
            cmd.Parameters.AddWithValue("@DescProyecto", Descproyecto);
            cmd.Parameters.AddWithValue("@FechaModif", Fmodi);

            //Variable logica
            bool rt;

            //instrucción controla los errores posibles 
            try
            {
                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                cmd.ExecuteNonQuery();

                //inicializa la variable verdadera
                rt = true;
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //inicializa la variable falsa
                rt = false;
            }
            finally
            {
                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }

            //regresa una exprexion logica
            return rt;
        }

        /// <summary>
        /// Se asigna tareas al personal
        /// </summary>
        /// <param name="idproyecto"></param>
        /// <param name="idpersonal"></param>
        /// <param name="idTarea"></param>
        /// <param name="FechaTareAsignada"></param>
        /// <param name="id_estatus"></param>
        /// <param name="Horas"></param>
        public static void fnAsignaTarea(int idproyecto, int idpersonal, int idTarea, DateTime FechaTareAsignada, int id_estatus, int Horas)
        {
            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_asig_tarea_ins", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@idproyecto", idproyecto);
                cmd.Parameters.AddWithValue("@idpersonal", idpersonal);
                cmd.Parameters.AddWithValue("@idTarea", idTarea);
                cmd.Parameters.AddWithValue("@FechaTareAsignada", FechaTareAsignada);
                cmd.Parameters.AddWithValue("@id_estatus", id_estatus);
                cmd.Parameters.AddWithValue("@Horas", Horas);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                cmd.ExecuteNonQuery();

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
        }

        /// <summary>
        /// Actualiza las tareas asignadas
        /// </summary>
        /// <param name="idproyecto"></param>
        /// <param name="idpersonal"></param>
        /// <param name="idTarea"></param>
        /// <param name="id_estatus"></param>
        /// <param name="Horas"></param>
        /// <param name="nIdAsig_Tarea"></param>
        /// <returns>bActualizacion</returns>
        public static bool fnActualizarTareaAsignada(int idproyecto, int idpersonal, int idTarea, int id_estatus, int Horas, int nIdAsig_Tarea)
        {
            //Variable logica
            bool bActualizacion = false;

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_Modificar_Tarea_Asignada_upd", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@idproyecto", idproyecto);
                cmd.Parameters.AddWithValue("@idpersonal", idpersonal);
                cmd.Parameters.AddWithValue("@idTarea", idTarea);
                cmd.Parameters.AddWithValue("@id_estatus", id_estatus);
                cmd.Parameters.AddWithValue("@Horas", Horas);
                cmd.Parameters.AddWithValue("@nIdAsig_Tarea", nIdAsig_Tarea);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                bActualizacion = Convert.ToBoolean(cmd.ExecuteScalar());

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //inicializa la variable falsa
                bActualizacion = false;
            }
            //regresa una exprexion logica
            return bActualizacion;
        }

        /// <summary>
        /// Agrega Tareas
        /// </summary>
        /// <param name="tarea"></param>
        /// <param name="descripciontarea"></param>
        /// <param name="fechatarea"></param>
        /// <param name="estatus"></param>
        /// <returns>bNueva</returns>
        public static bool fnAgregarTarea(string tarea, string descripciontarea, DateTime fechatarea, bool estatus)
        {
            //Variable logica
            bool bNueva = false;

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_agregar_tareas_ins", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@Tarea", tarea);
                cmd.Parameters.AddWithValue("@DescripcionTarea", descripciontarea);
                cmd.Parameters.AddWithValue("@FechaTarea", fechatarea);
                cmd.Parameters.AddWithValue("@EstatusTarea", estatus);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                cmd.ExecuteNonQuery();

                //inicializa la variable verdadera
                bNueva = true;

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //inicializa la variable falsa
                bNueva = false;
            }
            //regresa una exprexion logica
            return bNueva;
        }

        /// <summary>
        /// Checa que el Personal exista en la base de datos
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <returns>bexiste</returns>
        public static bool busqueda(string nombre, string apellido)
        {
            //variable logica
            bool bexiste = false;

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());
                //instrucción controla los errores posibles 

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_busqu_personal_sel", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@ApPaterno", apellido);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                bexiste = Convert.ToBoolean(cmd.ExecuteScalar());

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //inicializa la variable falsa
                bexiste = false;
            }
            //regresa una exprexion logica
            return bexiste;
        }

        /// <summary>
        /// Realiza relacion de personal y usuario
        /// </summary>
        /// <param name="idpersonal"></param>
        /// <param name="idUsuario"></param>
        public static void fnRelacionPersonalUsuario(int idpersonal, int idUsuario)
        {
            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_llena_tbl_con_personal_usuario_rel", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@idpersonal", idpersonal);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                cmd.ExecuteNonQuery();

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
        }

        /// <summary>
        /// Verifica que la actividad no se repita
        /// </summary>
        /// <param name="idpersonal"></param>
        /// <param name="idproyecto"></param>
        /// <param name="idTarea"></param>
        /// <param name="id_estatus"></param>
        /// <returns>bActividadAsignada</returns>
        public static bool fnChecarActividad(int idpersonal, int idproyecto, int idTarea, int id_estatus)
        {
            //Variable logica
            bool bActividadAsignada;

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("upd_checar_tarea_asignada_sel", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@idpersonal", idpersonal);
                cmd.Parameters.AddWithValue("@idproyecto", idproyecto);
                cmd.Parameters.AddWithValue("@idTarea", idTarea);
                cmd.Parameters.AddWithValue("@id_estatus", id_estatus);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                bActividadAsignada = Convert.ToBoolean(cmd.ExecuteScalar());

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //inicializa la variable falsa
                bActividadAsignada = false;
            }
            //regresa una exprexion logica
            return bActividadAsignada;
        }

        /// <summary>
        /// Verifica que el proyecto no se encuentre asignado para dar de baja
        /// </summary>
        /// <param name="idproyecto"></param>
        /// <returns>bChecarProyecto</returns>
        public static bool fnChecaProyectoAsignado(int idproyecto)
        {
            //Variable logica
            bool bChecarProyecto = false;

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("upd_checar_proyecto_asignado_sel", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@idproyecto", idproyecto);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                bChecarProyecto = Convert.ToBoolean(cmd.ExecuteScalar());

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //inicializa la variable falsa
                bChecarProyecto = false;
            }
            //regresa una exprexion logica
            return bChecarProyecto;
        }

        /// <summary>
        /// Verifica que la tarea no esta asignada para dar de baja
        /// </summary>
        /// <param name="idTarea"></param>
        /// <returns>bChecarTarea</returns>
        public static bool fnChecaTareaAsignada(int idTarea)
        {
            //Variable logica
            bool bChecarTarea;

            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_tarea_asignada_proyecto", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@idTarea", idTarea);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                bChecarTarea = Convert.ToBoolean(cmd.ExecuteScalar());

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                //inicializa la variable falsa
                bChecarTarea = false;
            }
            //regresa una exprexion logica
            return bChecarTarea;
        }

        /// <summary>
        /// Realiza la pausa de la actividad asignada 
        /// </summary>
        /// <param name="IdProyecto"></param>
        /// <param name="IdPersonal"></param>
        /// <param name="IdTarea"></param>
        /// <param name="id_estatus"></param>
        /// <param name="tiempo"></param>
        /// <param name="Descripcion"></param>
        /// <param name="IdAsig_Tarea"></param>
        /// <param name="IdTarea_realizada"></param>
        /// <param name="btnSel"></param>
        public static void fnPausaDeActividad(int IdProyecto, int IdPersonal, int IdTarea, int id_estatus, DateTime tiempo, string Descripcion, int IdAsig_Tarea, int IdTarea_realizada, string btnSel)
        {
            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_actualizar_tarea_realizada_upd", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@IdProyecto", IdProyecto);
                cmd.Parameters.AddWithValue("@IdPersonal", IdPersonal);
                cmd.Parameters.AddWithValue("@IdTarea", IdTarea);
                cmd.Parameters.AddWithValue("@id_estatus", id_estatus);
                cmd.Parameters.AddWithValue("@tiempo", tiempo);
                cmd.Parameters.AddWithValue("@DescripcionPausa", Descripcion);
                cmd.Parameters.AddWithValue("@IdTarea_realizada", IdTarea_realizada);
                cmd.Parameters.AddWithValue("@btnSel", btnSel);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                cmd.ExecuteNonQuery();

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {
                
            }
        }

        /// <summary>
        /// Finaliza la actividad asignada
        /// </summary>
        /// <param name="nIdProyecto"></param>
        /// <param name="nIdPersonal"></param>
        /// <param name="nIdTarea"></param>
        /// <param name="sDescripcionTerminar"></param>
        /// <param name="dTerminar"></param>
        /// <param name="nid_estatus"></param>
        /// <param name="nIdAsig_Tarea"></param>
        public static void fnFinalizarActividad(int nIdProyecto, int nIdPersonal, int nIdTarea, string sDescripcionTerminar, DateTime dTerminar, int nid_estatus, int nIdAsig_Tarea)
        {
            //instrucción controla los errores posibles 
            try
            {
                //se establece la cadena coneccion con el url de la base de datos
                ConnectionString cadena = new ConnectionString();

                //sqlconnection establece la conexion con la base de datos
                SqlConnection conn = new SqlConnection(cadena.conexionString());

                //se crea una nueva instancia de tipo sqlcommand con el Stored procedure y la conexion a la BD
                SqlCommand cmd = new SqlCommand("usp_terminar_tarea_asignada", conn);

                // se establece el tipo al sqlcommand 
                cmd.CommandType = CommandType.StoredProcedure;

                //añade como valor hasta el final de la sentencia y se le pasa el valor
                cmd.Parameters.AddWithValue("@IdProyecto", nIdProyecto);
                cmd.Parameters.AddWithValue("@IdPersonal", nIdPersonal);
                cmd.Parameters.AddWithValue("@IdTarea", nIdTarea);
                cmd.Parameters.AddWithValue("@DescripcionTerminar", sDescripcionTerminar);
                cmd.Parameters.AddWithValue("@Terminar", dTerminar);
                cmd.Parameters.AddWithValue("@id_estatus", nid_estatus);
                cmd.Parameters.AddWithValue("@IdAsig_Tarea", nIdAsig_Tarea);

                //abre conexion
                conn.Open();

                //crea el objeto de la base de datos
                cmd.ExecuteNonQuery();

                //libera recursos
                conn.Dispose();

                //libera recursos
                cmd.Dispose();
            }
            //atrapa diferentes clases de Excepciones
            catch
            {

            }
        }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using System.Data.SqlClient;
using System.Data;

namespace Negocio
{
    public class Logica
    {
        DataTable dtVariable;

        /// <summary>
        /// Muestra personal
        /// </summary>
        /// <returns>dtVariable</returns>
        public DataTable fnMostrarPersonal()
        {
            dtVariable = SqlQuery.fnMostrarPersonal();
            return dtVariable;
        }

        /// <summary>
        /// Muestra Proyectos 1
        /// </summary>
        /// <returns>dtVariable</returns>
        public DataTable fnMostrarPuestos()
        {
            dtVariable = SqlQuery.fnMostrarPuestos();
            return dtVariable;
        }

        /// <summary>
        /// Muestra los datos del personal
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>dtVariable</returns>
        public DataTable fnMostraTabladelPersonal(string usuario)
        {
            if (usuario != string.Empty)
                dtVariable = SqlQuery.fnMostraTabladelPersonal(usuario);
            return dtVariable;
        }

        /// <summary>
        /// Muestra reporte de personal
        /// </summary>
        /// <param name="IdPersonal"></param>
        /// <param name="IdProyecto"></param>
        /// <param name="fechainicio"></param>
        /// <param name="fechafinal"></param>
        /// <param name="btnSelec"></param>
        /// <returns>dtVariable</returns>
        public DataTable fnMostrarReportePersonal(int IdPersonal, int IdProyecto, DateTime fechainicio, DateTime fechafinal, string btnSelec)
        {
            dtVariable = SqlQuery.fnMostrarReportePersonal(IdPersonal, IdProyecto, fechainicio, fechafinal, btnSelec);
            return dtVariable;
        }

        /// <summary>
        /// Muestra las tareas
        /// </summary>
        /// <returns></returns>
        public DataTable fnMostrarTareas()
        {
            dtVariable = SqlQuery.fnMostrarTareas();
            return dtVariable;
        }

        /// <summary>
        /// Verifica si el usuario existe
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="pass"></param>
        /// <returns>dtVariable</returns>
        public DataTable fnVericaUsuarioExiste(string usuario, string pass)
        {
            dtVariable = SqlQuery.fnVericaUsuarioExiste(usuario/*, pass*/);
            return dtVariable;
        }

        /// <summary>
        /// Muestra menu de acuerdo al tipo de permiso
        /// </summary>
        /// <param name="typo"></param>
        /// <returns>dtVariable</returns>
        public DataTable fnMotrarMenu(string typo)
        {
            dtVariable = SqlQuery.fnMotrarMenu(typo);
            return dtVariable;
        }

        /// <summary>
        /// Muestra los proyectos 2
        /// </summary>
        /// <returns>dtVariable</returns>
        public DataTable fnMostrarProyectos()
        {
            dtVariable = SqlQuery.fnMostrarProyectos();
            return dtVariable;
        }

        /// <summary>
        /// Muestra los proyectos aque fueron asignados al personal
        /// </summary>
        /// <param name="idPersonal"></param>
        /// <returns></returns>
        public DataTable fnMostrarProyectosAsignados(int idPersonal,bool bReporte)
        {
            dtVariable = SqlQuery.fnMostrarProyectosAsignados(idPersonal,bReporte);
            return dtVariable;
        }

        /// <summary>
        /// Muestra las tareas depende el proyecto
        /// </summary>
        /// <param name="idproyecto"></param>
        /// <returns></returns>
        public DataTable fnMostrarTareasDeProyecto(int idproyecto)
        {
            dtVariable = SqlQuery.fnMostrarTareasDeProyecto(idproyecto);
            return dtVariable;
        }

        /// <summary>
        /// Muestra las tareas que fueron asignadas al personal 1
        /// </summary>
        /// <param name="IdProyecto"></param>
        /// <param name="IdPersonal"></param>
        /// <returns>dtVariable</returns>
        public DataTable fnMostrarTareasAsignadas(int IdProyecto, int IdPersonal)
        {
            dtVariable = SqlQuery.fnMostrarTareasAsignadas(IdProyecto, IdPersonal);
            return dtVariable;
        }

        /// <summary>
        /// Muestra las tareas asignadas al personal 2
        /// </summary>
        /// <param name="IdPersonal"></param>
        /// <returns></returns>
        public DataTable fnMostrarTareasAsignadas(int IdPersonal)
        {
            dtVariable = SqlQuery.fnMostrarTareasAsignadas(IdPersonal);
            return dtVariable;
        }

        /// <summary>
        /// Muestra reporte de personal de acuerdo al proyecto
        /// </summary>
        /// <param name="IdPersonal"></param>
        /// <param name="IdProyecto"></param>
        /// <returns></returns>
        public DataTable fnMostrarReporte(int IdPersonal, int IdProyecto)
        {
            dtVariable = SqlQuery.fnMostrarReporte(IdPersonal, IdProyecto);
            return dtVariable;
        }


        public bool baja_proyecto(int idproyecto, bool estatus)
        {
            bool baja = SqlQuery.baja_proyecto(idproyecto, estatus);
            return baja;
        }
        public bool Actualizar_proyecto(int idproyecto, string NomProyecto, string Descproyecto, DateTime Fmodi)
        {
            bool ActProyect = SqlQuery.Actualizar_proyecto(idproyecto, NomProyecto, Descproyecto, Fmodi);
            return ActProyect;
        }

        /// <summary>
        /// Agrega Proyecto
        /// </summary>
        /// <param name="NomProyecto"></param>
        /// <param name="DescProyecto"></param>
        /// <param name="FechaProyecto"></param>
        /// <param name="estatus"></param>
        public void fnAgregarProyecto(string NomProyecto, string DescProyecto, DateTime FechaProyecto, bool estatus)
        {
            SqlQuery.fnAgregarProyecto(NomProyecto, DescProyecto, FechaProyecto, estatus);
        }

        /// <summary>
        /// Actualiza personal
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
        public void fnActualizarPersonal(int idpersonal, string nombre, string direccion, string paterno, string materno, string correo, string telefono, bool estatus, string puesto)
        {
            SqlQuery.fnActualizarPersonal(idpersonal, nombre, direccion, paterno, materno, correo, telefono, estatus, puesto);
        }

        /// <summary>
        /// Agrega Tareas
        /// </summary>
        /// <param name="tarea"></param>
        /// <param name="descripciontarea"></param>
        /// <param name="fechatarea"></param>
        /// <param name="estatus"></param>
        /// <returns></returns>
        public bool fnAgregarTarea(string tarea, string descripciontarea, DateTime fechatarea, bool estatus)
        {
            bool tareas = SqlQuery.fnAgregarTarea(tarea, descripciontarea, fechatarea, estatus);
            return tareas;
        }

        /// <summary>
        /// Checa que el Personal exista en la base de datos
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <returns></returns>
        public bool busqueda(string nombre, string apellido)
        {
            bool dt = SqlQuery.busqueda(nombre, apellido);

            return dt;
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
        /// <returns></returns>
        public bool fnActualizarTareaAsignada(int idproyecto, int idpersonal, int idTarea, int id_estatus, int Horas, int nIdAsig_Tarea)
        {
            bool act = SqlQuery.fnActualizarTareaAsignada(idproyecto, idpersonal, idTarea, id_estatus, Horas, nIdAsig_Tarea);
            return act;
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
        public void fnAsignaTarea(int idproyecto, int idpersonal, int idTarea, DateTime FechaTareAsignada, int id_estatus, int Horas)
        {
            SqlQuery.fnAsignaTarea(idproyecto, idpersonal, idTarea, FechaTareAsignada, id_estatus, Horas);
        }

        /// <summary>
        /// Realiza relacion de personal y usuario
        /// </summary>
        /// <param name="idpersonal"></param>
        /// <param name="idUsuario"></param>
        public void fnRelacionPersonalUsuario(int idpersonal, int idUsuario)
        {
            SqlQuery.fnRelacionPersonalUsuario(idpersonal, idUsuario);
        }

        /// <summary>
        /// Verifica que la actividad no se repita
        /// </summary>
        /// <param name="idpersonal"></param>
        /// <param name="idproyecto"></param>
        /// <param name="idTarea"></param>
        /// <param name="id_estatus"></param>
        /// <returns></returns>
        public bool fnChecarActividad(int idpersonal, int idproyecto, int idTarea, int id_estatus)
        {
            bool checar = SqlQuery.fnChecarActividad(idpersonal, idproyecto, idTarea, id_estatus);
            return checar;
        }

        /// <summary>
        /// Actualiza Catalogo de Tareas
        /// </summary>
        /// <param name="IdTarea"></param>
        /// <param name="Tarea"></param>
        /// <param name="DescripcionTarea"></param>
        /// <param name="btnactualizar"></param>
        /// <returns></returns>
        public bool fnActualizarTarea(int IdTarea, string Tarea, string DescripcionTarea, string btnactualizar)
        {
            bool actualizar = SqlQuery.fnActualizarTarea(IdTarea, Tarea, DescripcionTarea, btnactualizar);
            return actualizar;
        }

        /// <summary>
        /// Verifica que el proyecto no se encuentre asignado para dar de baja
        /// </summary>
        /// <param name="idproyecto"></param>
        /// <returns></returns>
        public bool fnChecaProyectoAsignado(int idproyecto)
        {
            bool checar = SqlQuery.fnChecaProyectoAsignado(idproyecto);
            return checar;
        }

        /// <summary>
        /// Verifica que la tarea no esta asignada para dar de baja
        /// </summary>
        /// <param name="idTarea"></param>
        /// <returns></returns>
        public bool fnChecaTareaAsignada(int idTarea)
        {
            bool checar = SqlQuery.fnChecaTareaAsignada(idTarea);
            return checar;
        }

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
        /// <returns></returns>
        public int fnAgregarPersonal(string Nombre, string ApPaterno, string ApMaterno, string Direccion, string Telefono, string Correo, int idPuesto, bool estatus, DateTime alta)
        {
            int AgreParsonal = SqlQuery.fnAgregarPersonal(Nombre, ApPaterno, ApMaterno, Direccion, Telefono, Correo, idPuesto, estatus, alta);
            return AgreParsonal;
        }

        /// <summary>
        /// Agrega el usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="contrasena"></param>
        /// <param name="typo"></param>
        /// <returns></returns>
        public int fnAgregaUsuario(string usuario, string contrasena, string typo)//, int IdPersonal)
        {
            int idUsuario = SqlQuery.fnAgregaUsuario(usuario, contrasena, typo);//, IdPersonal);
            return idUsuario;
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
        /// <returns></returns>
        public int fnIniciaTarea(int IdProyecto, int IdPersonal, int IdTarea, DateTime Inicio, int id_estatus, int IdAsig_Tarea)
        {
            int IdTarea_realizada = SqlQuery.fnIniciaTarea(IdProyecto, IdPersonal, IdTarea, Inicio, id_estatus, IdAsig_Tarea);
            return IdTarea_realizada;
        }

        /// <summary>
        /// Recupera el estatus
        /// </summary>
        /// <param name="idAsignacion"></param>
        /// <returns></returns>
        public int fnRecuperaEstatus(int idAsignacion)
        {
            int estatus = SqlQuery.fnRecuperaEstatus(idAsignacion);
            return estatus;
        }

        /// <summary>
        /// Regresa el id de la tarea Realizada
        /// </summary>
        /// <param name="id_estatus"></param>
        /// <param name="IdTarea"></param>
        /// <returns></returns>
        public int fnIdTareaAsignada(int id_estatus, int IdTarea)
        {
            int IdTarea_realizada = SqlQuery.fnIdTareaAsignada(id_estatus, IdTarea);
            return IdTarea_realizada;
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
        public void fnPausaDeActividad(int IdProyecto, int IdPersonal, int IdTarea, int id_estatus, DateTime tiempo, string Descripcion, int IdAsig_Tarea, int IdTarea_realizada, string btnSel)
        {
            SqlQuery.fnPausaDeActividad(IdProyecto, IdPersonal, IdTarea, id_estatus, tiempo, Descripcion, IdAsig_Tarea, IdTarea_realizada, btnSel);
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
        public void fnFinalizarActividad(int nIdProyecto, int nIdPersonal, int nIdTarea, string sDescripcionTerminar, DateTime dTerminar, int nid_estatus, int nIdAsig_Tarea)
        {
            SqlQuery.fnFinalizarActividad(nIdProyecto, nIdPersonal, nIdTarea, sDescripcionTerminar, dTerminar, nid_estatus, nIdAsig_Tarea);
        }
    }
}
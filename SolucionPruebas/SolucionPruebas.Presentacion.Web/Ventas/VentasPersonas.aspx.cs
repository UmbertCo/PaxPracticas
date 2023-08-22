using SolucionPruebas.Entidades;
using SolucionPruebas.Presentacion.Servicios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web
{
    public partial class VentasPersonas : System.Web.UI.Page
    {
        Servicios.CatalogoServicioLocal.CatalogoServiceClient SDCatalogo;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Reasignar_Click(object sender, EventArgs e)
        {
            //List<ReasignacionesPersonas> ListaReasignacionPersonas;
            //InicioSesionUsuario EInicioSesionUsuario;
            //Mensajes EMensajes;
            try
            {
                Entidades.Personas EPersonas = new Personas();
                fnActualizar(ref EPersonas, "usp_Personas_ins");

                //SDOperacion = ProxyLocator.ObtenerServicioOperacion();

                //ListaReasignacionPersonas = new List<ReasignacionesPersonas>();
                //EInicioSesionUsuario= new InicioSesionUsuario();
                //EMensajes= new Mensajes();

                //ListaReasignacionPersonas.Add(new ReasignacionesPersonas { Id_Persona = 6 });

                //SDOperacion.fnRegistrarReasignaciones(44, 54, "se me antojo irme de vaciones", ListaReasignacionPersonas, EInicioSesionUsuario, ref EMensajes);
            }
            catch (Exception ex)
            {

            } 
        }
        protected void btnPersonas_Click(object sender, EventArgs e)
        {
            fnIngresarPersonaContacto();
        }

        public void fnIngresarPersona()
        {
            string cadena_conexion = string.Empty;
            int id_persona;
            int id_contacto;
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, System.TimeSpan.MaxValue))
            {
                cadena_conexion = string.Format("Data source={0}; Initial catalog={1};User ID={2}; Password={3};", "192.168.3.106", "PAXSistemaVentas", "Desarrollo", "F4cturax10n");
                //cadena_conexion = string.Format("Data source={0}; Initial catalog={1};Trusted_Connection=True;", "localhost", "Prueba");
                try
                {

                    Entidades.Personas EPersonas = new Personas();
                    fnActualizar(ref EPersonas, "usp_cli_Personas_ins");


                    Utilerias.SQL.InterfazSQL conexion = new Utilerias.SQL.InterfazSQL(cadena_conexion);
                    conexion.AgregarParametro("sNombre", "Ismael");
                    id_persona = Convert.ToInt32(conexion.TraerEscalar("usp_Personas_ins", true));

                    Utilerias.SQL.InterfazSQL conexion3 = new Utilerias.SQL.InterfazSQL(cadena_conexion);
                    conexion3.AgregarParametro("nId_Persona", id_persona);
                    conexion3.AgregarParametro("sNombre", "Ismael");
                    id_contacto = Convert.ToInt32(conexion3.TraerEscalar("usp_Contacto_ins", true));

                    //Servicios.Pista.InsertarPista();

                    tran.Complete();
                }
                catch (Exception ex)
                {
                    tran.Dispose();
                }
            }
        }

        public void fnIngresarPersonaContacto()
        {
            Entidades.Personas EPersona;
            Entidades.Contacto EContacto;
            try
            {
                EPersona = new Personas();
                EPersona.sEmpresa = "Empresa Patito SA _ 36";
                EPersona.sTelefono = "4110622";
                EPersona.sTelefono2 = "";
                EPersona.sTelefono3 = "";
                EPersona.sEmail = "diurno_nos@hotmail.com";
                EPersona.nIdCiudad = 1;
                EPersona.dFechaCaptura = DateTime.Now;
                EPersona.nIdEstatus = 1;
                EPersona.nIdTipoCliente = 1;
                EPersona.nIdTipoCompra = 19;
                EPersona.nIdUsuario = 2;
                EPersona.sRfc = "AAA010101AAA";
                EPersona.sRazonSocial = "Empresa Patito SA _ 36";
                EPersona.sUsuarioCobro = "";

                EContacto = new Contacto();
                EContacto.sNombre = "Ismael Hidalgo Escobedo 36";
                EContacto.sProfesion = "Ingeniero";
                EContacto.sPuestoEmpresa = "Gerente";
                EContacto.sEmail = "diurno_nos@hotmail.com";
                EContacto.sTelefono = "4110622";
                EContacto.sTelefono2 = "";
                EContacto.sTelefono3 = "";
                EContacto.sEstatus = "A";

                SDCatalogo = ProxyLocator.ObtenerCatalogoServicioLocal();
                SDCatalogo.fnRegistrarPersona(EPersona, EContacto, true, false);
            }
            catch (System.ServiceModel.FaultException ex)
            {
                //lblResultado.Text = ex.Message;
            }
            catch (Exception ex)
            {
                //lblResultado.Text = ex.Message;
            }
        }

        public void fnActualizar<T>(ref T Entidad, string psProcedimientoAlmacenado)
        {
            fnCargarParametros(Entidad, psProcedimientoAlmacenado);
        }

        public void Insertar<T>(ref T Entidad, string psProcedimientoAlmacenado)
        {
            fnCargarParametros(Entidad, psProcedimientoAlmacenado);
        }

        private SqlCommand fnCargarParametros<T>(T Entidad, string psProcedimientoAlmacenado)
        {
            string cadena_conexion = string.Empty;
            SqlCommand Comando = new SqlCommand();
            object oValor = null;
            try
            {
                cadena_conexion = string.Format("Data source={0}; Initial catalog={1};User ID={2}; Password={3};", "192.168.3.106", "PAXSistemaVentas", "sa", "F4cturax10n");

                //SqlConnection conexion = new SqlConnection(ConfigurationManager.AppSettings["conexion"]);
                SqlConnection conexion = new SqlConnection(cadena_conexion);
                Comando.CommandText = psProcedimientoAlmacenado;
                Comando.Connection = conexion;
                Comando.CommandType = CommandType.StoredProcedure;

                conexion.Open();
                SqlCommandBuilder.DeriveParameters(Comando);

                foreach (SqlParameter parametro in Comando.Parameters)
                {
                    if (!parametro.Direction.Equals(System.Data.ParameterDirection.ReturnValue))
                    {
                        string sNombreParametro = parametro.ParameterName.Split('@')[1];
                        try
                        {
                            oValor = Entidad.GetType().GetProperty(sNombreParametro).GetValue(Entidad, null);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Error al obtener el valor {0} de la Entidad {1}. " + ex.Message, sNombreParametro, Entidad.GetType()));
                        }
                        Comando.Parameters[parametro.ParameterName].Value = oValor;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se ha generado un error a la hora de cargar los parametros: " + ex.Message);
            }
            return Comando;
        }
    }
}
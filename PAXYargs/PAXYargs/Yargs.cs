using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PAXYargs
{
    public class Yargs
    {
        private Dictionary<string, cOpcion> Opciones = new Dictionary<string, cOpcion>();
        private Dictionary<string, string> ParametrosArgs = new Dictionary<string, string>();
        private Dictionary<string, object> Parametros = new Dictionary<string, object>();
        private List<string> _ = new List<string>();

        List<string> Errores = new List<string>();

        string[] args = { };

        public Yargs(string[] args)
        {
            Init(args);

            _argv = new Argv(Parametros, _);

            Opcion("version", new cOpcion { alias = "v",  descripcion = "muestra la version",  tipo = typeof(Boolean), Accion= new Action(fnInvocarVersion) });

        }


        public void fnGenerarHelp()
        {
            Opcion("help", new cOpcion { alias = "h", descripcion = "muestra el help para la aplicacion " + Assembly.GetEntryAssembly().GetName().Name, tipo = typeof(Boolean), Accion = new Action(fnInvocarHelp) });
        }

        private void fnInvocarHelp()
        {
            object objVersion = Parametros["help"];

            if (objVersion == null) return;

            int nMayor = 0;

            foreach (KeyValuePair<string, cOpcion> kvp in Opciones)
            {
                cOpcion opcion = kvp.Value;

                string sMensaje = ((opcion.demandaropcion ? "requerido" : "") +
                    " --" + kvp.Key.ToString() 
                          + (opcion.alias!=null? "(--" + opcion.alias + ")":""));

                nMayor = nMayor > sMensaje.Length ?
                         nMayor :
                         sMensaje.Length;


            }

            foreach (KeyValuePair<string, cOpcion> kvp in Opciones)
            {
                cOpcion opcion = kvp.Value;

                string sMensaje = ((opcion.demandaropcion ? "requerido" : "") +
                    " --" + kvp.Key.ToString()
                          + (opcion.alias != null ? "(--" + opcion.alias + ")" : ""));


                int nEspacios = nMayor - sMensaje.Length;


                Console.WriteLine(sMensaje.PadRight(sMensaje.Length + nEspacios) +"      " + opcion.descripcion );

                
                //" + opcion.descripcion );



            }



            Environment.Exit(0);

        }

        private void fnInvocarVersion()
        {
            object objVersion = Parametros["version"];

            if (objVersion == null) return;

            if ((bool)objVersion) { 

            Console.WriteLine( Assembly.GetEntryAssembly().GetName().Version);

            Environment.Exit(0);
            }
        }

        private void Init(string[] args)
        {
            this.args = args;


            for (int i = 0; i < args.Length;)
            {
                string[] arg = { };
                string[] arrvalor = { };
                string valor = string.Empty;
                string nombre = string.Empty;

                if (args[i].StartsWith("--"))
                {

                    if (args[i].Contains("="))
                    {
                        arg = args[i].Split('=');

                        nombre = arg[0].Replace("--", "");

                        arrvalor = new string[arg.Length - 1];

                        Array.Copy(arg, 1, arrvalor, 0, arg.Length - 1);

                        valor = string.Join("", arrvalor);




                    }
                    else if (args[i].Contains(":"))
                    {

                        arg = args[i].Split(':');

                        nombre = arg[0].Replace("--", "");

                        arrvalor = new string[arg.Length - 1];

                        Array.Copy(arg, 1, arrvalor, 0, arg.Length - 1);

                        valor = string.Join("", arrvalor);



                    }
                    else if (i != args.Length - 1)
                    {
                        if (!args[i + 1].StartsWith("--"))
                        {
                            nombre = args[i].Replace("--", "");
                            valor = args[i + 1];

                            i += 2;

                            ParametrosArgs.Add(nombre, valor);
                            continue;

                        }
                        else
                        {
                            nombre = args[i].Replace("--", "");
                            valor = "true";

                        }

                    }
                    else
                    {
                        nombre = args[i].Replace("--", "");
                        valor = "true";
                    }


                }
                else
                {
                    _.Add(args[i]);
                    i++;

                    continue;
                }

                i++;

                if (nombre != "")
                    ParametrosArgs.Add(nombre, valor);
            }

        }

        public Yargs Opcion(string sAlias, cOpcion poOpcion) {

            try
            {
                Opciones.Add(sAlias, poOpcion);
            }
            catch(ArgumentException ex)
            {

            }

            if(poOpcion.alias !=null)
            if (poOpcion.alias != sAlias)
            {
                cOpcion auxOpcion = cOpcion.Clonar(poOpcion);

                //auxOpcion.alias = sAlias;

                try
                {
                    Parametros.Add(poOpcion.alias, fnProcesarOpcion(sAlias, auxOpcion));
                }
                catch(ArgumentException ex) { }
            }

            try { 
                Parametros.Add(sAlias, fnProcesarOpcion(sAlias,poOpcion));
            }
            catch(ArgumentException ex) { }

            if (poOpcion.Accion != null)
            {
                poOpcion.Accion.Invoke();
            }

            return this;
        }

        private object fnProcesarOpcion(string psAlias, cOpcion pOpcion)
        {
            Object objSalida = null;

        


            string sValor = null;


            try
            {
               sValor =  ParametrosArgs[pOpcion.alias];
            }
            catch
            {
                try
                {
                    sValor = ParametrosArgs[psAlias];
                }
                catch
                {
                    sValor = null;
                }
                
            }


            if (pOpcion.demandaropcion)
                if (sValor == null)
                {
                    string sErr = "El parametro < " + psAlias + " > es requerido";

                    //bool bErr = false;

                    //foreach (string sErrAux in Errores)
                    //{
                    //    if (sErrAux.Equals(sErr))
                    //    {
                    //        bErr = true;
                    //    }

                    //}

                    //if (!bErr)
                    //    Errores.Add(sErr);

                    if (!Errores.Exists(xErr => xErr.Equals(sErr)))
                    {
                        Errores.Add(sErr);
                    }

                    //if ( !Errores.Any(alias => alias.Equals(sErr,StringComparison.InvariantCulture) ))
                    //    Errores.Add("El parametro <" + psAlias + "> es requerido");

                        //Environment.Exit(1);
                }

            if (pOpcion.def != null)
                if (sValor == string.Empty)
                {
                    sValor = pOpcion.def.ToString();
                }


            try
            {

                if(sValor!=null)
                objSalida = Convert.ChangeType(sValor, pOpcion.tipo);
            }
            catch
            {
                Console.WriteLine("El parametro <" + psAlias + "> no pudo ser convertido en " + pOpcion.tipo.ToString());

                Environment.Exit(1);

            }

           


            return objSalida;

        }

        private Argv _argv;

        public Argv argv{  get {

                fnGenerarHelp();

                if (Errores.Count > 0)
                {
                    foreach (string sErr in Errores)
                    {
                        Console.WriteLine(sErr);

                    }

                    Environment.Exit(1);
                }

                return _argv;
            } }
    }

    public class Argv
    {
        
        private Dictionary<string, object> Parametros = new Dictionary<string, object>();
        private List<string> _ = new List<string>();

        public Argv( Dictionary<string, object> Parametros, List<string> _)
        {
            
            this.Parametros = Parametros;
            this._ = _;

        }

        public object this[string sOpcion]
        {
            get {
                object objAux = null;

                try
                {
                   return Parametros[sOpcion];
                }
                catch { return null; }
            }
        }
    }

    public class cOpcion
    {
        public string alias = null;
        public bool demandaropcion;
        public string descripcion = null;
        public object def = null ;
        public Type tipo = typeof(string);
        public Action Accion = null;

        public static cOpcion Clonar(cOpcion poOpcion)
        {

           return  new cOpcion { alias = poOpcion.alias, def = poOpcion.def, demandaropcion = poOpcion.demandaropcion, descripcion = poOpcion.descripcion, tipo =poOpcion.tipo, Accion= poOpcion.Accion };


        }

    }

}

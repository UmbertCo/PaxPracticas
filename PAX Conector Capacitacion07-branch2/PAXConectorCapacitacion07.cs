using System.Configuration;
using System.ServiceProcess;



namespace PAX_Conector_Capacitacion06
{
    public partial class PAXConectorCapacitacion06 : ServiceBase
    {
        static clsLog sLog = new clsLog(ConfigurationManager.AppSettings["sLog"]);


        public PAXConectorCapacitacion06()
        {

           
            InitializeComponent();
            sLog.fnAgregarLog("Inicio correcto public PAXConectorCapacitacion06");


            //clsComunes.fnPrincipal();


            sLog.fnAgregarLog("Fin  correcto public PAXConectorCapacitacion06");

        }

        protected override void OnStart(string[] args)
        {
           
            sLog.fnAgregarLog("Inicio correcto OnStart ");

            //clsComunes.fnPrincipal();

            sLog.fnAgregarLog("FIN correcto OnStart ");
        }

        protected override void OnStop()
        {
            sLog.fnAgregarLog("Inicio correcto OnStop");


            sLog.fnAgregarLog("FIN correcto OnStop");
        }


        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            sLog.fnAgregarLog("Inicio timer_Elapsed");

            clsComunes.fnPrincipal();


            sLog.fnAgregarLog("FIN timer_Elapsed");
        }
    }
}

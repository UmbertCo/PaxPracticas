using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Reflection;

    public partial class frmConsulta : Form
    {
       // frmSplash fSplash = new frmSplash();
        public event Action aTerminoConsulta;

        string sRutaEXE  = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        string sEsqueletoConsulta = "<CONSULTA>"+
                                    "<RFC></RFC>"+
                                    "<UUID></UUID>"+
                                    "<PASS></PASS>"+
                                    "<FI></FI>"+
                                    "<FF></FF>"+
                                    "<E></E>"+
                                    "<T></T>"+
                                    "<RFC_R></RFC_R>"+
                                    "<MODO>E</MODO>" +
                                    "</CONSULTA>"                     ;

        DataTable dtTipos;

        XmlDocument xdDocumentoConsulta;



        public frmConsulta()
        {
            InitializeComponent();
           // fSplash.Show();
           // fSplash.Visible = false;

         //   aTerminoConsulta += new Action(frmConsulta_aTerminoConsulta);

            dtTipos = new DataTable();
            dtTipos.Columns.Add("nClave", typeof(long));
            dtTipos.Columns.Add("sLegenda", typeof(string));

            dtTipos.Rows.Add(-1, "Cualquiera");
            dtTipos.Rows.Add(8, "Estándar");
            dtTipos.Rows.Add(4294967296, "Acreditamiento de IEPS");
            dtTipos.Rows.Add(8388608, "Aerolíneas");
            dtTipos.Rows.Add(1073741824, "Certificado de Destrucción");
            dtTipos.Rows.Add(17179869184, "Comercio Exterior");
            dtTipos.Rows.Add(4, "Compra Venta de Divisas");
            dtTipos.Rows.Add(16777216, "Consumo de Combustibles");
            dtTipos.Rows.Add(64, "Donatarias");
            dtTipos.Rows.Add(256, "Estado De Cuenta Bancario");
            dtTipos.Rows.Add(8589934592, "Estado de cuenta de combustibles de monederos electrónicos.");
            dtTipos.Rows.Add(68719476736, "INE 1.1");
            dtTipos.Rows.Add(1024, "Instituciones Educativas Privadas dtTipos.Rows.add(Pago de colegiatura)");
            dtTipos.Rows.Add(4096, "Leyendas Fiscales");
            dtTipos.Rows.Add(524288, "Mis Cuentas");
            dtTipos.Rows.Add(67108864, "Notarios Públicos");
            dtTipos.Rows.Add(536870912, "Obras de artes y antigüedades");
            dtTipos.Rows.Add(2048, "Otros Derechos e Impuestos");
            dtTipos.Rows.Add(4194304, "Pago en Especie");
            dtTipos.Rows.Add(8192, "Persona Física Integrante de Coordinado");
            dtTipos.Rows.Add(128, "Recibo de donativo");
            dtTipos.Rows.Add(1048576, "Recibo de Pago de Salarios");
            dtTipos.Rows.Add(137438953472, "Recibo de Pago de Salarios 1.2");
            dtTipos.Rows.Add(32, "Sector de Ventas al Detalle dtTipos.Rows.add(Detallista)");
            dtTipos.Rows.Add(268435456, "Servicios de construcción");
            dtTipos.Rows.Add(16384, "SPEI de Tercero a Tercero");
            dtTipos.Rows.Add(2147483648, "Sustitución y renovación vehicular");
            dtTipos.Rows.Add(32768, "Terceros");
            dtTipos.Rows.Add(65536, "Terceros");
            dtTipos.Rows.Add(16, "Turista o Pasajero Extranjero");
            dtTipos.Rows.Add(33554432, "Vales de Despensa");
            dtTipos.Rows.Add(134217728, "Vehículo Usado");
            dtTipos.Rows.Add(2097152, "Venta de Vehiculos");

            cmbTipo.DataSource = dtTipos;

            cmbTipo.DisplayMember = "sLegenda";
            cmbTipo.ValueMember = "nClave";

            DataTable dtEstado = new DataTable();

            dtEstado.Columns.Add("nClave", typeof(int));
            dtEstado.Columns.Add("sLegenda", typeof(string));

            dtEstado.Rows.Add(-1, "Cualquiera");
            dtEstado.Rows.Add(1, "Vigente");
            dtEstado.Rows.Add(0, "Cancelado");

            cmbEstatus.DataSource = dtEstado;

            cmbEstatus.DisplayMember = "sLegenda";
            cmbEstatus.ValueMember = "nClave";
        }

        void frmConsulta_aTerminoConsulta()
        {
            fnMostrarSplash(false);
        }

        public void fnMostrarSplash(bool bMostrar) 
        {
          //return;
          //if(fSplash.InvokeRequired){
          //    fSplash.Invoke(new delSetVisible(fnMostrarSplash),bMostrar);
          //if (bMostrar)
          //{
          //    
          //    fSplash.SetBounds((this.Location.X + this.Width / 2)- fSplash.Width/2, (this.Location.Y + this.Height / 2)-fSplash.Height/2, fSplash.Width, fSplash.Height);
          //
          //}
          //}
          //
        
        }

        public void fnArmarConsulta() 
        {
            xdDocumentoConsulta = new XmlDocument();
            xdDocumentoConsulta.LoadXml(sEsqueletoConsulta);

           XPathNavigator xpnConsulta = xdDocumentoConsulta.CreateNavigator();

           xpnConsulta.SelectSingleNode("/CONSULTA/RFC").SetValue(txtRFC.Text);
           xpnConsulta.SelectSingleNode("/CONSULTA/UUID").SetValue(txtUUID.Text);
           xpnConsulta.SelectSingleNode("/CONSULTA/PASS").SetValue(txtPass.Text);
           xpnConsulta.SelectSingleNode("/CONSULTA/FI").SetValue(dtIni.Value.ToString("dd-MM-yyyy")+"T"+txtHI.Text);
           xpnConsulta.SelectSingleNode("/CONSULTA/FF").SetValue(dtFin.Value.ToString("dd-MM-yyyy") + "T" + txtHF.Text);
           xpnConsulta.SelectSingleNode("/CONSULTA/E").SetValue(cmbEstatus.SelectedValue.ToString());
           xpnConsulta.SelectSingleNode("/CONSULTA/T").SetValue(cmbTipo.SelectedValue.ToString());
           xpnConsulta.SelectSingleNode("/CONSULTA/RFC_R").SetValue(txtRFC_R.Text);
           xpnConsulta.SelectSingleNode("/CONSULTA/MODO").SetValue(cmbModo.Text);
        }

        public void fnEnviarConsulta()
        {
            txtEstatus.Text = "Descargando";

            Thread.Sleep(1000);

            try
            {
                P_PruebasConPaxDescargaWS.wsConsulta.wsPAXSATDescarga wsConsulta = new P_PruebasConPaxDescargaWS.wsConsulta.wsPAXSATDescarga();

                if (!String.IsNullOrEmpty(txtUrl.Text))
                    wsConsulta.Url = txtUrl.Text;

                wsConsulta.Timeout = (int) P_PruebasConPaxDescargaWS.Properties.Settings.Default.timeout.TotalMilliseconds;

               // txtEstatus.Text = "Descargando";

                string sEstatusConsulta = "";
                Byte[] bRespuesta = wsConsulta.fnConsultaDescargaSAT(txtUsuarioPAX.Text, txtPassPAX.Text, xdDocumentoConsulta.OuterXml, out sEstatusConsulta);

                String sRutaZIP = sRutaEXE + Path.DirectorySeparatorChar + "Consulta " + DateTime.Now.ToString("dd_MM_yyyy__hh_mm_ss") + ".zip";

                if (bRespuesta.Length != 0)
                {
                    File.WriteAllBytes(sRutaZIP, bRespuesta);
                    MessageBox.Show("Consulta se guardo en \""+sRutaZIP+"\"" );
                }
                txtEstatus.Text = sEstatusConsulta;

              

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {

         

            Thread tProceso = new Thread(new ThreadStart(delegate()
            {


             //  while (Login.tblResultado == null)
             //  {
             //      System.Threading.Thread.Sleep(1000);
             //
             //  }
             //
             //  gvColsulta.Invoke(new delBindGV(fnBindGV), Login.tblResultado);
             //
             //  //gvColsulta.DataSource = Login.tblResultado;
             //
             // 

              //  aTerminoConsulta();
                
                
                
              }));

           // tProceso.Start();

        }


        public delegate void delSetVisible(bool bVisible);

        public void fnSetVisible(bool bVisible)
        {
            //fSplash.Visible = bVisible;

        }

        delegate void delBindGV(DataTable ptblGV);
        public void fnBindGV(DataTable ptblGV) 
        {

            //gvColsulta.DataSource = ptblGV;

            
        }

        private void frmConsulta_Move(object sender, EventArgs e)
        {  //
           //if (fSplash.Visible)
           //{
           //    fSplash.SetBounds((this.Location.X + this.Width / 2) - fSplash.Width / 2, (this.Location.Y + this.Height / 2) - fSplash.Height / 2, fSplash.Width, fSplash.Height);
           //    fSplash.Focus();
           //}
        }
        
        private void frmConsulta_ResizeEnd(object sender, EventArgs e)
        {
           // if (fSplash.Visible)
           // {
           //     fSplash.SetBounds((this.Location.X + this.Width / 2) - fSplash.Width / 2, (this.Location.Y + this.Height / 2) - fSplash.Height / 2, fSplash.Width, fSplash.Height);
           //     fSplash.Focus();
           // }
        }

        private void btnConsultar_Click_1(object sender, EventArgs e)
        {
            fnArmarConsulta();
            fnEnviarConsulta(); 
        }
    }


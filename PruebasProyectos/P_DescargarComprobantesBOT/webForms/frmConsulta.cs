using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace P_DescargarComprobantesBOT.webForms
{
    public partial class frmConsulta : Form
    {
        frmSplash fSplash = new frmSplash();
        public event Action aTerminoConsulta;

        public frmConsulta()
        {
            InitializeComponent();
            fSplash.Show();
            fSplash.Visible = false;

            aTerminoConsulta += new Action(frmConsulta_aTerminoConsulta);
        }

        void frmConsulta_aTerminoConsulta()
        {
            fnMostrarSplash(false);
        }

        public void fnMostrarSplash(bool bMostrar) 
        {
            return;
            if(fSplash.InvokeRequired){
                fSplash.Invoke(new delSetVisible(fnMostrarSplash),bMostrar);
            if (bMostrar)
            {
                
                fSplash.SetBounds((this.Location.X + this.Width / 2)- fSplash.Width/2, (this.Location.Y + this.Height / 2)-fSplash.Height/2, fSplash.Width, fSplash.Height);

            }
            }
        
        
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {

            fnMostrarSplash(true);

            frmLogin Login = new frmLogin(new clsConsulta() { dtFechaIni = dtIni.Value, dtFechaFin = dtFin.Value, bConsulta = true });

            Login.Show();


            Thread tProceso = new Thread(new ThreadStart(delegate()
            {


                while (Login.tblResultado == null)
                {
                    System.Threading.Thread.Sleep(1000);

                }

                gvColsulta.Invoke(new delBindGV(fnBindGV), Login.tblResultado);

                //gvColsulta.DataSource = Login.tblResultado;

               

                aTerminoConsulta();
                
                
                
              }));

            tProceso.Start();

        }


        public delegate void delSetVisible(bool bVisible);

        public void fnSetVisible(bool bVisible)
        {
            fSplash.Visible = bVisible;

        }

        delegate void delBindGV(DataTable ptblGV);
        public void fnBindGV(DataTable ptblGV) 
        {

            gvColsulta.DataSource = ptblGV;

            
        }

        private void frmConsulta_Move(object sender, EventArgs e)
        {
            if (fSplash.Visible)
            {
                fSplash.SetBounds((this.Location.X + this.Width / 2) - fSplash.Width / 2, (this.Location.Y + this.Height / 2) - fSplash.Height / 2, fSplash.Width, fSplash.Height);
                fSplash.Focus();
            }
        }
        
        private void frmConsulta_ResizeEnd(object sender, EventArgs e)
        {
            if (fSplash.Visible)
            {
                fSplash.SetBounds((this.Location.X + this.Width / 2) - fSplash.Width / 2, (this.Location.Y + this.Height / 2) - fSplash.Height / 2, fSplash.Width, fSplash.Height);
                fSplash.Focus();
            }
        }
    }
}

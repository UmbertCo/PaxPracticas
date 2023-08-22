using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            string nombreServicio = "PAX Service Myers";
            ServiceController servicio = new ServiceController(nombreServicio);
            int timeoutMilliseconds = 5000;
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                servicio.Start();
                servicio.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {

            }
        }

        private void btnDetener_Click(object sender, EventArgs e)
        {
            string nombreServicio = "PAX Service Myers";
            ServiceController servicio = new ServiceController(nombreServicio);
            int timeoutMilliseconds = 5000;
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                servicio.Stop();
                servicio.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {

            }
        }
    }
}

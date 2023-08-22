using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmLCO : Form
    {
        public frmLCO()
        {
            InitializeComponent();
        }

        private void btnConsumirLCO_Click(object sender, EventArgs e)
        {
            TcpClient tcLCO = new TcpClient();
            string sEncabezado = string.Empty;
            string sSoap = string.Empty;
            string sPeticion = string.Empty;
            string sRespuesta = string.Empty;
            int nTamanioContenido = 0;
            try
            {
                tcLCO.Connect("192.168.3.106", 8080);
                sEncabezado = txtEncabezado.Text;
                sSoap = txtSoap.Text;

                nTamanioContenido = sSoap.Length;

                sEncabezado = string.Format(sEncabezado, nTamanioContenido);

                sPeticion = sEncabezado + System.Environment.NewLine + System.Environment.NewLine + sSoap;

                NetworkStream netStream = tcLCO.GetStream();

                if (netStream.CanWrite)
                {
                    Byte[] sendBytes = Encoding.UTF8.GetBytes(sPeticion);
                    netStream.Write(sendBytes, 0, sendBytes.Length);
                }
                else
                {
                    sRespuesta = "You cannot write data to this stream.";
                    tcLCO.Close();

                    // Closing the tcpClient instance does not close the network stream.
                    netStream.Close();
                    return;
                }
              
                if (netStream.CanRead)
                {
                    // Reads NetworkStream into a byte buffer. 
                    byte[] bytes = new byte[tcLCO.ReceiveBufferSize];

                    // Read can return anything from 0 to numBytesToRead.  
                    // This method blocks until at least one byte is read.
                    netStream.Read(bytes, 0, (int)tcLCO.ReceiveBufferSize);

                    // Returns the data received from the host to the console. 
                    sRespuesta = Encoding.UTF8.GetString(bytes);
                }
                else
                {
                    sRespuesta = "You cannot read data from this stream.";
                    tcLCO.Close();

                    // Closing the tcpClient instance does not close the network stream.
                    netStream.Close();
                }
                netStream.Close();

                txtRespuesta.Text = sRespuesta;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consumir la LCO por NET.TCP: " + ex.Message);
            }
        }
    }
}

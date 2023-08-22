using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms1
{
    public partial class frmPruebaCaracteres : Form
    {
        public frmPruebaCaracteres()
        {
            InitializeComponent();
        }

        private void frmPruebaCaracteres_Load(object sender, EventArgs e)
        {
            //txtCaracter.Text = FromHexString("26237831333B") + "PQ0026967";
            //txtCaracter.Text = "\u0013" + "PQ0026967";


            txtCaracter.Text = System.Text.Encoding.Unicode.GetString(System.Text.Encoding.Unicode.GetBytes("\u0013")) + "PQ0026967";
        }

        public static string FromHexString(string sHexString)
        {
            var abBytes = new byte[sHexString.Length / 2];
            for (var i = 0; i < abBytes.Length; i++)
            {
                abBytes[i] = Convert.ToByte(sHexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(abBytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
        }
    }
}

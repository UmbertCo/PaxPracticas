using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmEncriptacion : Form
    {
        static byte[] abTextoEncriptado;

        static byte[] abLlave = System.Text.Encoding.UTF8.GetBytes("F4cturax10n");
        static byte[] abVectorInicializacion = System.Text.Encoding.UTF8.GetBytes("P4ssw0rd");

        public frmEncriptacion()
        {
            InitializeComponent();
        }

        public enum TiposEncriptacion : int
        {
            Clasica = 1,
            Base64 = 2,
            DES3 = 3,
            AES_128 = 4
        }

        public enum Accion : int
        {
            Encriptar = 1,
            Desencriptar = 2
        }

        private void frmEncriptacion_Load(object sender, EventArgs e)
        {
            fnCargarTiposEncriptacion();
            fnCargarAccion();
        }
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(cbTipoEncriptacion.SelectedValue).Equals((int)TiposEncriptacion.Base64))
                {
                    if (Convert.ToInt32(cbAccion.SelectedValue).Equals((int)Accion.Encriptar))
                    {
                        txtResultado.Text = PAXCrypto.Base64.EncriptarBase64(txtFuente.Text);
                    }
                    else
                    {
                        txtResultado.Text = PAXCrypto.Base64.DesencriptarBase64(txtFuente.Text);
                    }
                }
                else if (Convert.ToInt32(cbTipoEncriptacion.SelectedValue).Equals((int)TiposEncriptacion.Clasica))
                {
                    if (Convert.ToInt32(cbAccion.SelectedValue).Equals((int)Accion.Encriptar))
                    {
                        txtResultado.Text = PAXCrypto.Classica.Encriptar(txtFuente.Text);
                    }
                    else
                    {
                        txtResultado.Text = PAXCrypto.Classica.Desencriptar(txtFuente.Text);
                    }
                }
                else if (Convert.ToInt32(cbTipoEncriptacion.SelectedValue).Equals((int)TiposEncriptacion.AES_128))
                {
                    if (Convert.ToInt32(cbAccion.SelectedValue).Equals((int)Accion.Encriptar))
                    {
                        txtResultado.Text = PAXCrypto.CryptoAES.EncriptarAES64(txtFuente.Text);
                    }
                    else
                    {
                        txtResultado.Text = PAXCrypto.CryptoAES.DesencriptaAES64(txtFuente.Text);
                    }
                }
                else 
                {
                    //if (Convert.ToInt32(cbAccion.SelectedValue).Equals((int)Accion.Encriptar))
                    //{
                    //    txtResultado.Text = System.Convert.ToBase64String(PAXCrypto.DES3.Encriptar(System.Text.Encoding.UTF8.GetBytes(txtFuente.Text)));
                    //}
                    //else
                    //{
                    //    txtResultado.Text = System.Text.Encoding.UTF8.GetString(Utilerias.Encriptacion.DES3.Desencriptar(System.Convert.FromBase64String(txtFuente.Text)));
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la aplicación. " + ex.Message);
            }
        }
        private void btnLimpiarBase64_Click(object sender, EventArgs e)
        {
            txtFuente.Text = string.Empty;
            txtResultado.Text = string.Empty;
        }

        private void fnCargarAccion()
        {
            DataTable dtTipoEncriptacion = new DataTable();
            dtTipoEncriptacion.Columns.Add("IdAccion", typeof(int));
            dtTipoEncriptacion.Columns.Add("Accion", typeof(string));

            DataRow drRenglon;
            drRenglon = dtTipoEncriptacion.NewRow();
            drRenglon["IdAccion"] = (int)Accion.Encriptar;
            drRenglon["Accion"] = Accion.Encriptar;
            dtTipoEncriptacion.Rows.Add(drRenglon);

            drRenglon = dtTipoEncriptacion.NewRow();
            drRenglon["IdAccion"] = (int)Accion.Desencriptar;
            drRenglon["Accion"] = Accion.Desencriptar;
            dtTipoEncriptacion.Rows.Add(drRenglon);

            cbAccion.DataSource = dtTipoEncriptacion;
        }

        private void fnCargarTiposEncriptacion()
        {
            DataTable dtTipoEncriptacion = new DataTable();
            dtTipoEncriptacion.Columns.Add("IdTipoEncriptacion", typeof(int));
            dtTipoEncriptacion.Columns.Add("TipoEncriptacion", typeof(string));

            DataRow drRenglon;
            drRenglon = dtTipoEncriptacion.NewRow();
            drRenglon["IdTipoEncriptacion"] = (int)TiposEncriptacion.Clasica;
            drRenglon["TipoEncriptacion"] = TiposEncriptacion.Clasica;
            dtTipoEncriptacion.Rows.Add(drRenglon);

            drRenglon = dtTipoEncriptacion.NewRow();
            drRenglon["IdTipoEncriptacion"] = (int)TiposEncriptacion.Base64;
            drRenglon["TipoEncriptacion"] = TiposEncriptacion.Base64;
            dtTipoEncriptacion.Rows.Add(drRenglon);

            drRenglon = dtTipoEncriptacion.NewRow();
            drRenglon["IdTipoEncriptacion"] = (int)TiposEncriptacion.DES3;
            drRenglon["TipoEncriptacion"] = TiposEncriptacion.DES3;
            dtTipoEncriptacion.Rows.Add(drRenglon);

            drRenglon = dtTipoEncriptacion.NewRow();
            drRenglon["IdTipoEncriptacion"] = (int)TiposEncriptacion.AES_128;
            drRenglon["TipoEncriptacion"] = TiposEncriptacion.AES_128;
            dtTipoEncriptacion.Rows.Add(drRenglon);

            cbTipoEncriptacion.DataSource = dtTipoEncriptacion;
        }

        static byte[] fnEncriptar_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.KeySize = 128;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        static string fnDesencriptar_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.KeySize = 128;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;

        }
    }
}

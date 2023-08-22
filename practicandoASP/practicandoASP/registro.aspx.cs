using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace practicandoASP
{
    public partial class registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblAlerta = new Label();
            Label lblAlerta2 = new Label();
            if (Session.Contents["usuario"] != null)
            {
                lblAlerta.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('Ya has ingresado en la cuenta: " + Session.Contents["usuario"].ToString() + ".')</script>";
                lblAlerta2.Text = "<script language='javascript'>" + Environment.NewLine + "setTimeout(function(){ window.location.replace('../index.aspx') },3000);</script>";  
                Page.Controls.Add(lblAlerta2);
                Page.Controls.Add(lblAlerta);
                txtUsuario.Enabled = false;
                txtPass.Enabled = false;
                txtNombre.Enabled = false;
                txtApellidos.Enabled = false;
                txtEdad.Enabled = false;
                rdbMasculino.Enabled = false;
                rdbFemenino.Enabled = false;
                btnRegistrarme.Enabled = false;
                btnLimpiar.Enabled = false;
            }
        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = "";
            txtPass.Text = "";
            txtNombre.Text = "";
            txtApellidos.Text = "";
            txtEdad.Text = "";
            txtTelefono.Text = "";
            rdbMasculino.Checked = true;
            rdbFemenino.Checked = false;
                
        }

        protected void btnRegistrarme_Click(object sender, EventArgs e)
        {
            Label lblAlerta = new Label();
            lblAlerta.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('Registro exitoso: Usuario " + txtNombre.Text + ".')</script>";
            Page.Controls.Add(lblAlerta);
        }
    }
}
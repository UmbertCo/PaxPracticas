using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace practicandoASP.cuentas
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblAlerta = new Label();
            Label lblAlerta2 = new Label();
            if (Session.Contents["usuario"] != null)
            {
                lblAlerta.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('Ya has ingresado en la cuenta: " + Session.Contents["usuario"].ToString() + ".')</script>";         
                lblAlerta2.Text = "<script language='javascript'>" + Environment.NewLine + "setTimeout(function(){ window.location.replace('../index.aspx') },3000);</script>";
                Page.Controls.Add(lblAlerta);
                Page.Controls.Add(lblAlerta2);
                txtUsuario.Enabled = false;
                txtPassword.Enabled = false;
                btnEntrar.Enabled = false;
            }
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            String sUsuario, sPass;
            sUsuario = txtUsuario.Text;
            sPass = txtPassword.Text;

            Label lblAlerta = new Label();
            Label lblAlerta2 = new Label();
            Label lblAlerta3 = new Label();
            if (sUsuario == "cesar" && sPass == "123")
            {
                lblAlerta.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('Bienvenido " + sUsuario + ".')</script>";
                Session.Contents["usuario"] = txtUsuario.Text;
                lblAlerta2.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('Sesión iniciada como: " + Session.Contents["usuario"].ToString() + ".')</script>";
                Page.Controls.Add(lblAlerta2);
                lblAlerta3.Text = "<script language='javascript'>" + Environment.NewLine + "setTimeout(function(){ window.location.replace('../index.aspx') },3000);</script>";
                Page.Controls.Add(lblAlerta3);
                //Response.Redirect("../index.aspx");
            }
            else {
                lblAlerta.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('Error en los datos introducidos. Por favor vuelva a intentar.')</script>";               
            }
            Page.Controls.Add(lblAlerta);
        }
    }
}